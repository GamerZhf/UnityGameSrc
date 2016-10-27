namespace Pathfinding
{
    using Pathfinding.Recast;
    using Pathfinding.Serialization;
    using Pathfinding.Serialization.JsonFx;
    using Pathfinding.Util;
    using Pathfinding.Voxels;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using UnityEngine;

    [Serializable, JsonOptIn]
    public class RecastGraph : NavGraph, IUpdatableGraph, IRaycastableGraph, INavmesh, INavmeshHolder
    {
        private bool batchTileUpdate;
        private List<int> batchUpdatedTiles = new List<int>();
        public const int BorderVertexMask = 1;
        public const int BorderVertexOffset = 0x1f;
        [JsonMember]
        public float cellSize = 0.5f;
        [JsonMember]
        public float characterRadius = 1.5f;
        [JsonMember]
        public float colliderRasterizeDetail = 10f;
        [JsonMember]
        public float contourMaxError = 2f;
        public bool dynamic = true;
        [JsonMember]
        public int editorTileSize = 0x80;
        [JsonMember]
        public Vector3 forcedBoundsCenter;
        [JsonMember]
        public Vector3 forcedBoundsSize = new Vector3(100f, 40f, 100f);
        private Voxelize globalVox;
        [JsonMember]
        public LayerMask mask = -1;
        [JsonMember]
        public float maxEdgeLength = 20f;
        [JsonMember]
        public float maxSlope = 30f;
        [JsonMember]
        public float minRegionSize = 3f;
        [JsonMember]
        public bool nearestSearchOnlyXZ;
        [JsonMember]
        public bool rasterizeColliders;
        [JsonMember]
        public bool rasterizeMeshes = true;
        [JsonMember]
        public bool rasterizeTerrain = true;
        [JsonMember]
        public bool rasterizeTrees = true;
        [JsonMember]
        public RelevantGraphSurfaceMode relevantGraphSurfaceMode;
        public bool scanEmptyGraph;
        [JsonMember]
        public bool showMeshOutline = true;
        [JsonMember]
        public bool showMeshSurface;
        [JsonMember]
        public bool showNodeConnections;
        [JsonMember]
        public List<string> tagMask = new List<string>();
        [JsonMember]
        public int terrainSampleSize = 3;
        public const int TileIndexMask = 0x7ffff;
        public const int TileIndexOffset = 12;
        private NavmeshTile[] tiles;
        [JsonMember]
        public int tileSizeX = 0x80;
        [JsonMember]
        public int tileSizeZ = 0x80;
        public int tileXCount;
        public int tileZCount;
        [JsonMember]
        public bool useTiles;
        public const int VertexIndexMask = 0xfff;
        [JsonMember]
        public float walkableClimb = 0.5f;
        [JsonMember]
        public float walkableHeight = 2f;

        protected void BuildTileMesh(Voxelize vox, int x, int z)
        {
            VoxelMesh mesh;
            vox.borderSize = this.TileBorderSizeInVoxels;
            vox.forcedBounds = this.CalculateTileBoundsWithBorder(x, z);
            vox.width = this.tileSizeX + (vox.borderSize * 2);
            vox.depth = this.tileSizeZ + (vox.borderSize * 2);
            if (!this.useTiles && (this.relevantGraphSurfaceMode == RelevantGraphSurfaceMode.OnlyForCompletelyInsideTile))
            {
                vox.relevantGraphSurfaceMode = RelevantGraphSurfaceMode.RequireForAll;
            }
            else
            {
                vox.relevantGraphSurfaceMode = this.relevantGraphSurfaceMode;
            }
            vox.minRegionSize = Mathf.RoundToInt(this.minRegionSize / (this.cellSize * this.cellSize));
            vox.Init();
            vox.VoxelizeInput();
            vox.FilterLedges(vox.voxelWalkableHeight, vox.voxelWalkableClimb, vox.cellSize, vox.cellHeight, vox.forcedBounds.min);
            vox.FilterLowHeightSpans(vox.voxelWalkableHeight, vox.cellSize, vox.cellHeight, vox.forcedBounds.min);
            vox.BuildCompactField();
            vox.BuildVoxelConnections();
            vox.ErodeWalkableArea(this.CharacterRadiusInVoxels);
            vox.BuildDistanceField();
            vox.BuildRegions();
            VoxelContourSet cset = new VoxelContourSet();
            vox.BuildContours(this.contourMaxError, 1, cset, 1);
            vox.BuildPolyMesh(cset, 3, out mesh);
            for (int i = 0; i < mesh.verts.Length; i++)
            {
                mesh.verts[i] = vox.VoxelToWorldInt3(mesh.verts[i]);
            }
            NavmeshTile tile = this.CreateTile(vox, mesh, x, z);
            this.tiles[tile.x + (tile.z * this.tileXCount)] = tile;
        }

        private void BuildTiles(Queue<Int2> tileQueue, List<RasterizationMesh>[] meshBuckets, ManualResetEvent doneEvent)
        {
            try
            {
                Voxelize vox = new Voxelize(this.CellHeight, this.cellSize, this.walkableClimb, this.walkableHeight, this.maxSlope);
                vox.maxEdgeLength = this.maxEdgeLength;
                while (true)
                {
                    Int2 num;
                    Queue<Int2> queue = tileQueue;
                    lock (queue)
                    {
                        if (tileQueue.Count == 0)
                        {
                            return;
                        }
                        num = tileQueue.Dequeue();
                    }
                    vox.inputMeshes = meshBuckets[num.x + (num.y * this.tileXCount)];
                    this.BuildTileMesh(vox, num.x, num.y);
                }
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogException(exception);
            }
            finally
            {
                if (doneEvent != null)
                {
                    doneEvent.Set();
                }
            }
        }

        private Bounds CalculateTileBoundsWithBorder(int x, int z)
        {
            float num = this.tileSizeX * this.cellSize;
            float num2 = this.tileSizeZ * this.cellSize;
            Vector3 min = this.forcedBounds.min;
            Vector3 max = this.forcedBounds.max;
            Bounds bounds = new Bounds();
            bounds.SetMinMax(new Vector3(x * num, 0f, z * num2) + min, new Vector3(((x + 1) * num) + min.x, max.y, ((z + 1) * num2) + min.z));
            bounds.Expand((Vector3) ((new Vector3(1f, 0f, 1f) * this.TileBorderSizeInWorldUnits) * 2f));
            return bounds;
        }

        public GraphUpdateThreading CanUpdateAsync(GraphUpdateObject o)
        {
            return (!o.updatePhysics ? GraphUpdateThreading.SeparateThread : GraphUpdateThreading.SeparateAndUnityInit);
        }

        public Vector3 ClosestPointOnNode(TriangleMeshNode node, Vector3 pos)
        {
            return Polygon.ClosestPointOnTriangle((Vector3) this.GetVertex(node.v0), (Vector3) this.GetVertex(node.v1), (Vector3) this.GetVertex(node.v2), pos);
        }

        private List<RasterizationMesh> CollectMeshes(Bounds bounds)
        {
            List<RasterizationMesh> meshes = new List<RasterizationMesh>();
            RecastMeshGatherer gatherer = new RecastMeshGatherer(bounds, this.terrainSampleSize, this.mask, this.tagMask, this.colliderRasterizeDetail);
            if (this.rasterizeMeshes)
            {
                gatherer.CollectSceneMeshes(meshes);
            }
            gatherer.CollectRecastMeshObjs(meshes);
            if (this.rasterizeTerrain)
            {
                float desiredChunkSize = this.cellSize * Math.Max(this.tileSizeX, this.tileSizeZ);
                gatherer.CollectTerrainMeshes(this.rasterizeTrees, desiredChunkSize, meshes);
            }
            if (this.rasterizeColliders)
            {
                gatherer.CollectColliderMeshes(meshes);
            }
            if (meshes.Count == 0)
            {
                UnityEngine.Debug.LogWarning("No MeshFilters were found contained in the layers specified by the 'mask' variables");
            }
            return meshes;
        }

        private void ConnectTiles(NavmeshTile tile1, NavmeshTile tile2)
        {
            if ((tile1 != null) && (tile2 != null))
            {
                int num5;
                int num6;
                int num7;
                int num8;
                float num9;
                if (tile1.nodes == null)
                {
                    throw new ArgumentException("tile1 does not contain any nodes");
                }
                if (tile2.nodes == null)
                {
                    throw new ArgumentException("tile2 does not contain any nodes");
                }
                int num = Mathf.Clamp(tile2.x, tile1.x, (tile1.x + tile1.w) - 1);
                int num2 = Mathf.Clamp(tile1.x, tile2.x, (tile2.x + tile2.w) - 1);
                int num3 = Mathf.Clamp(tile2.z, tile1.z, (tile1.z + tile1.d) - 1);
                int num4 = Mathf.Clamp(tile1.z, tile2.z, (tile2.z + tile2.d) - 1);
                if (num == num2)
                {
                    num5 = 2;
                    num6 = 0;
                    num7 = num3;
                    num8 = num4;
                    num9 = this.tileSizeZ * this.cellSize;
                }
                else
                {
                    if (num3 != num4)
                    {
                        throw new ArgumentException("Tiles are not adjacent (neither x or z coordinates match)");
                    }
                    num5 = 0;
                    num6 = 2;
                    num7 = num;
                    num8 = num2;
                    num9 = this.tileSizeX * this.cellSize;
                }
                if (Math.Abs((int) (num7 - num8)) != 1)
                {
                    UnityEngine.Debug.Log(string.Concat(new object[] { 
                        tile1.x, " ", tile1.z, " ", tile1.w, " ", tile1.d, "\n", tile2.x, " ", tile2.z, " ", tile2.w, " ", tile2.d, "\n", 
                        num, " ", num3, " ", num2, " ", num4
                     }));
                    object[] objArray2 = new object[] { "Tiles are not adjacent (tile coordinates must differ by exactly 1. Got '", num7, "' and '", num8, "')" };
                    throw new ArgumentException(string.Concat(objArray2));
                }
                int num10 = (int) Math.Round((double) (((Math.Max(num7, num8) * num9) + this.forcedBounds.min[num5]) * 1000f));
                TriangleMeshNode[] nodes = tile1.nodes;
                TriangleMeshNode[] nodeArray2 = tile2.nodes;
                for (int i = 0; i < nodes.Length; i++)
                {
                    TriangleMeshNode node = nodes[i];
                    int vertexCount = node.GetVertexCount();
                    for (int j = 0; j < vertexCount; j++)
                    {
                        Int3 vertex = node.GetVertex(j);
                        Int3 num15 = node.GetVertex((j + 1) % vertexCount);
                        if ((Math.Abs((int) (vertex[num5] - num10)) < 2) && (Math.Abs((int) (num15[num5] - num10)) < 2))
                        {
                            int num16 = Math.Min(vertex[num6], num15[num6]);
                            int num17 = Math.Max(vertex[num6], num15[num6]);
                            if (num16 != num17)
                            {
                                for (int k = 0; k < nodeArray2.Length; k++)
                                {
                                    TriangleMeshNode node2 = nodeArray2[k];
                                    int num19 = node2.GetVertexCount();
                                    for (int m = 0; m < num19; m++)
                                    {
                                        Int3 num21 = node2.GetVertex(m);
                                        Int3 num22 = node2.GetVertex((m + 1) % vertexCount);
                                        if ((Math.Abs((int) (num21[num5] - num10)) < 2) && (Math.Abs((int) (num22[num5] - num10)) < 2))
                                        {
                                            int num23 = Math.Min(num21[num6], num22[num6]);
                                            int num24 = Math.Max(num21[num6], num22[num6]);
                                            if (((num23 != num24) && ((num17 > num23) && (num16 < num24))) && ((((vertex == num21) && (num15 == num22)) || ((vertex == num22) && (num15 == num21))) || (VectorMath.SqrDistanceSegmentSegment((Vector3) vertex, (Vector3) num15, (Vector3) num21, (Vector3) num22) < (this.walkableClimb * this.walkableClimb))))
                                            {
                                                Int3 num26 = node.position - node2.position;
                                                uint costMagnitude = (uint) num26.costMagnitude;
                                                node.AddConnection(node2, costMagnitude);
                                                node2.AddConnection(node, costMagnitude);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ConnectTiles(Queue<Int2> tileQueue, ManualResetEvent doneEvent)
        {
            try
            {
                while (true)
                {
                    Int2 num;
                    Queue<Int2> queue = tileQueue;
                    lock (queue)
                    {
                        if (tileQueue.Count == 0)
                        {
                            return;
                        }
                        num = tileQueue.Dequeue();
                    }
                    if (num.x < (this.tileXCount - 1))
                    {
                        this.ConnectTiles(this.tiles[num.x + (num.y * this.tileXCount)], this.tiles[(num.x + 1) + (num.y * this.tileXCount)]);
                    }
                    if (num.y < (this.tileZCount - 1))
                    {
                        this.ConnectTiles(this.tiles[num.x + (num.y * this.tileXCount)], this.tiles[num.x + ((num.y + 1) * this.tileXCount)]);
                    }
                }
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogException(exception);
            }
            finally
            {
                if (doneEvent != null)
                {
                    doneEvent.Set();
                }
            }
        }

        private void ConnectTileWithNeighbours(NavmeshTile tile)
        {
            if (tile.x > 0)
            {
                int num = tile.x - 1;
                for (int i = tile.z; i < (tile.z + tile.d); i++)
                {
                    this.ConnectTiles(this.tiles[num + (i * this.tileXCount)], tile);
                }
            }
            if ((tile.x + tile.w) < this.tileXCount)
            {
                int num3 = tile.x + tile.w;
                for (int j = tile.z; j < (tile.z + tile.d); j++)
                {
                    this.ConnectTiles(this.tiles[num3 + (j * this.tileXCount)], tile);
                }
            }
            if (tile.z > 0)
            {
                int num5 = tile.z - 1;
                for (int k = tile.x; k < (tile.x + tile.w); k++)
                {
                    this.ConnectTiles(this.tiles[k + (num5 * this.tileXCount)], tile);
                }
            }
            if ((tile.z + tile.d) < this.tileZCount)
            {
                int num7 = tile.z + tile.d;
                for (int m = tile.x; m < (tile.x + tile.w); m++)
                {
                    this.ConnectTiles(this.tiles[m + (num7 * this.tileXCount)], tile);
                }
            }
        }

        public bool ContainsPoint(TriangleMeshNode node, Vector3 pos)
        {
            return ((VectorMath.IsClockwiseXZ((Vector3) this.GetVertex(node.v0), (Vector3) this.GetVertex(node.v1), pos) && VectorMath.IsClockwiseXZ((Vector3) this.GetVertex(node.v1), (Vector3) this.GetVertex(node.v2), pos)) && VectorMath.IsClockwiseXZ((Vector3) this.GetVertex(node.v2), (Vector3) this.GetVertex(node.v0), pos));
        }

        private void CreateNodeConnections(TriangleMeshNode[] nodes)
        {
            List<MeshNode> list = ListPool<MeshNode>.Claim();
            List<uint> list2 = ListPool<uint>.Claim();
            Dictionary<Int2, int> dictionary = ObjectPoolSimple<Dictionary<Int2, int>>.Claim();
            dictionary.Clear();
            for (int i = 0; i < nodes.Length; i++)
            {
                TriangleMeshNode node = nodes[i];
                int vertexCount = node.GetVertexCount();
                for (int k = 0; k < vertexCount; k++)
                {
                    Int2 key = new Int2(node.GetVertexIndex(k), node.GetVertexIndex((k + 1) % vertexCount));
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary.Add(key, i);
                    }
                }
            }
            for (int j = 0; j < nodes.Length; j++)
            {
                TriangleMeshNode node2 = nodes[j];
                list.Clear();
                list2.Clear();
                int num6 = node2.GetVertexCount();
                for (int m = 0; m < num6; m++)
                {
                    int num10;
                    int vertexIndex = node2.GetVertexIndex(m);
                    int x = node2.GetVertexIndex((m + 1) % num6);
                    if (dictionary.TryGetValue(new Int2(x, vertexIndex), out num10))
                    {
                        TriangleMeshNode item = nodes[num10];
                        int num11 = item.GetVertexCount();
                        for (int n = 0; n < num11; n++)
                        {
                            if ((item.GetVertexIndex(n) == x) && (item.GetVertexIndex((n + 1) % num11) == vertexIndex))
                            {
                                Int3 num14 = node2.position - item.position;
                                uint costMagnitude = (uint) num14.costMagnitude;
                                list.Add(item);
                                list2.Add(costMagnitude);
                                break;
                            }
                        }
                    }
                }
                node2.connections = list.ToArray();
                node2.connectionCosts = list2.ToArray();
            }
            dictionary.Clear();
            ObjectPoolSimple<Dictionary<Int2, int>>.Release(ref dictionary);
            ListPool<MeshNode>.Release(list);
            ListPool<uint>.Release(list2);
        }

        private NavmeshTile CreateTile(Voxelize vox, VoxelMesh mesh, int x, int z)
        {
            if (mesh.tris == null)
            {
                throw new ArgumentNullException("mesh.tris");
            }
            if (mesh.verts == null)
            {
                throw new ArgumentNullException("mesh.verts");
            }
            NavmeshTile tile2 = new NavmeshTile();
            tile2.x = x;
            tile2.z = z;
            tile2.w = 1;
            tile2.d = 1;
            tile2.tris = mesh.tris;
            tile2.verts = mesh.verts;
            tile2.bbTree = new BBTree();
            NavmeshTile graph = tile2;
            if ((graph.tris.Length % 3) != 0)
            {
                throw new ArgumentException("Indices array's length must be a multiple of 3 (mesh.tris)");
            }
            if (graph.verts.Length >= 0xfff)
            {
                if ((this.tileXCount * this.tileZCount) == 1)
                {
                    throw new ArgumentException("Too many vertices per tile (more than " + 0xfff + ").\n<b>Try enabling tiling in the recast graph settings.</b>\n");
                }
                throw new ArgumentException("Too many vertices per tile (more than " + 0xfff + ").\n<b>Try reducing tile size or enabling ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* Inspector</b>");
            }
            graph.verts = Utility.RemoveDuplicateVertices(graph.verts, graph.tris);
            TriangleMeshNode[] nodes = new TriangleMeshNode[graph.tris.Length / 3];
            graph.nodes = nodes;
            int graphIndex = AstarPath.active.astarData.graphs.Length + Thread.CurrentThread.ManagedThreadId;
            int num2 = x + (z * this.tileXCount);
            num2 = num2 << 12;
            TriangleMeshNode.SetNavmeshHolder(graphIndex, graph);
            AstarPath active = AstarPath.active;
            lock (active)
            {
                for (int i = 0; i < nodes.Length; i++)
                {
                    TriangleMeshNode node = new TriangleMeshNode(base.active);
                    nodes[i] = node;
                    node.GraphIndex = (uint) graphIndex;
                    node.v0 = graph.tris[i * 3] | num2;
                    node.v1 = graph.tris[(i * 3) + 1] | num2;
                    node.v2 = graph.tris[(i * 3) + 2] | num2;
                    if (!VectorMath.IsClockwiseXZ(node.GetVertex(0), node.GetVertex(1), node.GetVertex(2)))
                    {
                        int num4 = node.v0;
                        node.v0 = node.v2;
                        node.v2 = num4;
                    }
                    node.Walkable = true;
                    node.Penalty = base.initialPenalty;
                    node.UpdatePositionFromVertices();
                }
            }
            graph.bbTree.RebuildFrom(nodes);
            this.CreateNodeConnections(graph.nodes);
            TriangleMeshNode.SetNavmeshHolder(graphIndex, null);
            return graph;
        }

        public override void DeserializeExtraInfo(GraphSerializationContext ctx)
        {
            BinaryReader reader = ctx.reader;
            this.tileXCount = reader.ReadInt32();
            if (this.tileXCount >= 0)
            {
                this.tileZCount = reader.ReadInt32();
                this.tiles = new NavmeshTile[this.tileXCount * this.tileZCount];
                TriangleMeshNode.SetNavmeshHolder((int) ctx.graphIndex, this);
                for (int i = 0; i < this.tileZCount; i++)
                {
                    for (int j = 0; j < this.tileXCount; j++)
                    {
                        int index = j + (i * this.tileXCount);
                        int num4 = reader.ReadInt32();
                        if (num4 < 0)
                        {
                            throw new Exception("Invalid tile coordinates (x < 0)");
                        }
                        int num5 = reader.ReadInt32();
                        if (num5 < 0)
                        {
                            throw new Exception("Invalid tile coordinates (z < 0)");
                        }
                        if ((num4 != j) || (num5 != i))
                        {
                            this.tiles[index] = this.tiles[(num5 * this.tileXCount) + num4];
                        }
                        else
                        {
                            NavmeshTile tile = new NavmeshTile();
                            tile.x = num4;
                            tile.z = num5;
                            tile.w = reader.ReadInt32();
                            tile.d = reader.ReadInt32();
                            tile.bbTree = new BBTree();
                            this.tiles[index] = tile;
                            int num6 = reader.ReadInt32();
                            if ((num6 % 3) != 0)
                            {
                                throw new Exception("Corrupt data. Triangle indices count must be divisable by 3. Got " + num6);
                            }
                            tile.tris = new int[num6];
                            for (int k = 0; k < tile.tris.Length; k++)
                            {
                                tile.tris[k] = reader.ReadInt32();
                            }
                            tile.verts = new Int3[reader.ReadInt32()];
                            for (int m = 0; m < tile.verts.Length; m++)
                            {
                                tile.verts[m] = ctx.DeserializeInt3();
                            }
                            int num9 = reader.ReadInt32();
                            tile.nodes = new TriangleMeshNode[num9];
                            index = index << 12;
                            for (int n = 0; n < tile.nodes.Length; n++)
                            {
                                TriangleMeshNode node = new TriangleMeshNode(base.active);
                                tile.nodes[n] = node;
                                node.DeserializeNode(ctx);
                                node.v0 = tile.tris[n * 3] | index;
                                node.v1 = tile.tris[(n * 3) + 1] | index;
                                node.v2 = tile.tris[(n * 3) + 2] | index;
                                node.UpdatePositionFromVertices();
                            }
                            tile.bbTree.RebuildFrom(tile.nodes);
                        }
                    }
                }
            }
        }

        public void EndBatchTileUpdate()
        {
            if (!this.batchTileUpdate)
            {
                throw new InvalidOperationException("Calling EndBatchLoad when batching not enabled");
            }
            this.batchTileUpdate = false;
            int tileXCount = this.tileXCount;
            int tileZCount = this.tileZCount;
            for (int i = 0; i < tileZCount; i++)
            {
                for (int m = 0; m < tileXCount; m++)
                {
                    this.tiles[m + (i * this.tileXCount)].flag = false;
                }
            }
            for (int j = 0; j < this.batchUpdatedTiles.Count; j++)
            {
                this.tiles[this.batchUpdatedTiles[j]].flag = true;
            }
            for (int k = 0; k < tileZCount; k++)
            {
                for (int n = 0; n < tileXCount; n++)
                {
                    if (((n < (tileXCount - 1)) && (this.tiles[n + (k * this.tileXCount)].flag || this.tiles[(n + 1) + (k * this.tileXCount)].flag)) && (this.tiles[n + (k * this.tileXCount)] != this.tiles[(n + 1) + (k * this.tileXCount)]))
                    {
                        this.ConnectTiles(this.tiles[n + (k * this.tileXCount)], this.tiles[(n + 1) + (k * this.tileXCount)]);
                    }
                    if (((k < (tileZCount - 1)) && (this.tiles[n + (k * this.tileXCount)].flag || this.tiles[n + ((k + 1) * this.tileXCount)].flag)) && (this.tiles[n + (k * this.tileXCount)] != this.tiles[n + ((k + 1) * this.tileXCount)]))
                    {
                        this.ConnectTiles(this.tiles[n + (k * this.tileXCount)], this.tiles[n + ((k + 1) * this.tileXCount)]);
                    }
                }
            }
            this.batchUpdatedTiles.Clear();
        }

        private void FillWithEmptyTiles()
        {
            for (int i = 0; i < this.tileZCount; i++)
            {
                for (int j = 0; j < this.tileXCount; j++)
                {
                    this.tiles[(i * this.tileXCount) + j] = NewEmptyTile(j, i);
                }
            }
        }

        public override NNInfoInternal GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
        {
            return this.GetNearestForce(position, null);
        }

        public override NNInfoInternal GetNearestForce(Vector3 position, NNConstraint constraint)
        {
            if (this.tiles == null)
            {
                return new NNInfoInternal();
            }
            Vector3 vector = position - this.forcedBounds.min;
            int num = Mathf.FloorToInt(vector.x / (this.cellSize * this.tileSizeX));
            int num2 = Mathf.FloorToInt(vector.z / (this.cellSize * this.tileSizeZ));
            num = Mathf.Clamp(num, 0, this.tileXCount - 1);
            num2 = Mathf.Clamp(num2, 0, this.tileZCount - 1);
            int num3 = Math.Max(this.tileXCount, this.tileZCount);
            NNInfoInternal previous = new NNInfoInternal();
            float positiveInfinity = float.PositiveInfinity;
            bool flag = this.nearestSearchOnlyXZ || ((constraint != null) && constraint.distanceXZ);
            for (int i = 0; i < num3; i++)
            {
                if (!flag && (positiveInfinity < (((i - 1) * this.cellSize) * Math.Max(this.tileSizeX, this.tileSizeZ))))
                {
                    break;
                }
                int num6 = Math.Min((i + num2) + 1, this.tileZCount);
                for (int j = Math.Max(-i + num2, 0); j < num6; j++)
                {
                    int num8 = Math.Abs((int) (i - Math.Abs((int) (j - num2))));
                    if ((-num8 + num) >= 0)
                    {
                        int num9 = -num8 + num;
                        NavmeshTile tile = this.tiles[num9 + (j * this.tileXCount)];
                        if (tile != null)
                        {
                            if (flag)
                            {
                                previous = tile.bbTree.QueryClosestXZ(position, constraint, ref positiveInfinity, previous);
                                if (positiveInfinity < float.PositiveInfinity)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                previous = tile.bbTree.QueryClosest(position, constraint, ref positiveInfinity, previous);
                            }
                        }
                    }
                    if ((num8 != 0) && ((num8 + num) < this.tileXCount))
                    {
                        int num10 = num8 + num;
                        NavmeshTile tile2 = this.tiles[num10 + (j * this.tileXCount)];
                        if (tile2 != null)
                        {
                            if (flag)
                            {
                                previous = tile2.bbTree.QueryClosestXZ(position, constraint, ref positiveInfinity, previous);
                                if (positiveInfinity >= float.PositiveInfinity)
                                {
                                    continue;
                                }
                                break;
                            }
                            previous = tile2.bbTree.QueryClosest(position, constraint, ref positiveInfinity, previous);
                        }
                    }
                }
            }
            previous.node = previous.constrainedNode;
            previous.constrainedNode = null;
            previous.clampedPosition = previous.constClampedPosition;
            return previous;
        }

        public override void GetNodes(GraphNodeDelegateCancelable del)
        {
            if (this.tiles != null)
            {
                for (int i = 0; i < this.tiles.Length; i++)
                {
                    if ((this.tiles[i] != null) && ((this.tiles[i].x + (this.tiles[i].z * this.tileXCount)) == i))
                    {
                        TriangleMeshNode[] nodes = this.tiles[i].nodes;
                        if (nodes != null)
                        {
                            for (int j = 0; (j < nodes.Length) && del(nodes[j]); j++)
                            {
                            }
                        }
                    }
                }
            }
        }

        public Bounds GetTileBounds(IntRect rect)
        {
            return this.GetTileBounds(rect.xmin, rect.ymin, rect.Width, rect.Height);
        }

        public Bounds GetTileBounds(int x, int z, [Optional, DefaultParameterValue(1)] int width, [Optional, DefaultParameterValue(1)] int depth)
        {
            Bounds bounds = new Bounds();
            bounds.SetMinMax(new Vector3((x * this.tileSizeX) * this.cellSize, 0f, (z * this.tileSizeZ) * this.cellSize) + this.forcedBounds.min, new Vector3(((x + width) * this.tileSizeX) * this.cellSize, this.forcedBounds.size.y, ((z + depth) * this.tileSizeZ) * this.cellSize) + this.forcedBounds.min);
            return bounds;
        }

        public Int2 GetTileCoordinates(Vector3 p)
        {
            p -= this.forcedBounds.min;
            p.x /= this.cellSize * this.tileSizeX;
            p.z /= this.cellSize * this.tileSizeZ;
            return new Int2((int) p.x, (int) p.z);
        }

        public void GetTileCoordinates(int tileIndex, out int x, out int z)
        {
            z = tileIndex / this.tileXCount;
            x = tileIndex - (z * this.tileXCount);
        }

        public int GetTileIndex(int index)
        {
            return ((index >> 12) & 0x7ffff);
        }

        public NavmeshTile[] GetTiles()
        {
            return this.tiles;
        }

        public IntRect GetTouchingTiles(Bounds b)
        {
            b.center -= this.forcedBounds.min;
            IntRect a = new IntRect(Mathf.FloorToInt(b.min.x / (this.tileSizeX * this.cellSize)), Mathf.FloorToInt(b.min.z / (this.tileSizeZ * this.cellSize)), Mathf.FloorToInt(b.max.x / (this.tileSizeX * this.cellSize)), Mathf.FloorToInt(b.max.z / (this.tileSizeZ * this.cellSize)));
            return IntRect.Intersection(a, new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1));
        }

        public IntRect GetTouchingTilesRound(Bounds b)
        {
            b.center -= this.forcedBounds.min;
            IntRect a = new IntRect(Mathf.RoundToInt(b.min.x / (this.tileSizeX * this.cellSize)), Mathf.RoundToInt(b.min.z / (this.tileSizeZ * this.cellSize)), Mathf.RoundToInt(b.max.x / (this.tileSizeX * this.cellSize)) - 1, Mathf.RoundToInt(b.max.z / (this.tileSizeZ * this.cellSize)) - 1);
            return IntRect.Intersection(a, new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1));
        }

        public Int3 GetVertex(int index)
        {
            int num = (index >> 12) & 0x7ffff;
            return this.tiles[num].GetVertex(index);
        }

        public int GetVertexArrayIndex(int index)
        {
            return (index & 0xfff);
        }

        private void InitializeTileInfo()
        {
            int num = (int) ((this.forcedBounds.size.x / this.cellSize) + 0.5f);
            int num2 = (int) ((this.forcedBounds.size.z / this.cellSize) + 0.5f);
            if (!this.useTiles)
            {
                this.tileSizeX = num;
                this.tileSizeZ = num2;
            }
            else
            {
                this.tileSizeX = this.editorTileSize;
                this.tileSizeZ = this.editorTileSize;
            }
            int num3 = ((num + this.tileSizeX) - 1) / this.tileSizeX;
            int num4 = ((num2 + this.tileSizeZ) - 1) / this.tileSizeZ;
            this.tileXCount = num3;
            this.tileZCount = num4;
            if ((this.tileXCount * this.tileZCount) > 0x80000)
            {
                object[] objArray1 = new object[] { "Too many tiles (", this.tileXCount * this.tileZCount, ") maximum is ", 0x80000, "\nTry disabling ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* inspector." };
                throw new Exception(string.Concat(objArray1));
            }
            this.tiles = new NavmeshTile[this.tileXCount * this.tileZCount];
        }

        public bool Linecast(Vector3 origin, Vector3 end)
        {
            return this.Linecast(origin, end, base.GetNearest(origin, NNConstraint.None).node);
        }

        public bool Linecast(Vector3 origin, Vector3 end, GraphNode hint)
        {
            GraphHitInfo info;
            return NavMeshGraph.Linecast(this, origin, end, hint, out info, null);
        }

        public bool Linecast(Vector3 origin, Vector3 end, GraphNode hint, out GraphHitInfo hit)
        {
            return NavMeshGraph.Linecast(this, origin, end, hint, out hit, null);
        }

        public bool Linecast(Vector3 tmp_origin, Vector3 tmp_end, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace)
        {
            return NavMeshGraph.Linecast(this, tmp_origin, tmp_end, hint, out hit, trace);
        }

        private static NavmeshTile NewEmptyTile(int x, int z)
        {
            NavmeshTile tile = new NavmeshTile();
            tile.x = x;
            tile.z = z;
            tile.w = 1;
            tile.d = 1;
            tile.verts = new Int3[0];
            tile.tris = new int[0];
            tile.nodes = new TriangleMeshNode[0];
            tile.bbTree = new BBTree();
            return tile;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            TriangleMeshNode.SetNavmeshHolder(base.active.astarData.GetGraphIndex(this), null);
        }

        public override void OnDrawGizmos(bool drawNodes)
        {
            <OnDrawGizmos>c__AnonStorey255 storey = new <OnDrawGizmos>c__AnonStorey255();
            storey.<>f__this = this;
            if (drawNodes)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(this.forcedBounds.center, this.forcedBounds.size);
                storey.debugData = AstarPath.active.debugPathData;
                GraphNodeDelegateCancelable del = new GraphNodeDelegateCancelable(storey.<>m__28);
                this.GetNodes(del);
            }
        }

        public GraphNode PointOnNavmesh(Vector3 position, NNConstraint constraint)
        {
            if (this.tiles != null)
            {
                Vector3 vector = position - this.forcedBounds.min;
                int num = Mathf.FloorToInt(vector.x / (this.cellSize * this.tileSizeX));
                int num2 = Mathf.FloorToInt(vector.z / (this.cellSize * this.tileSizeZ));
                if (((num < 0) || (num2 < 0)) || ((num >= this.tileXCount) || (num2 >= this.tileZCount)))
                {
                    return null;
                }
                NavmeshTile tile = this.tiles[num + (num2 * this.tileXCount)];
                if (tile != null)
                {
                    return tile.bbTree.QueryInside(position, constraint);
                }
            }
            return null;
        }

        private List<RasterizationMesh>[] PutMeshesIntoTileBuckets(List<RasterizationMesh> meshes)
        {
            List<RasterizationMesh>[] listArray = new List<RasterizationMesh>[this.tiles.Length];
            Vector3 amount = (Vector3) ((new Vector3(1f, 0f, 1f) * this.TileBorderSizeInWorldUnits) * 2f);
            for (int i = 0; i < listArray.Length; i++)
            {
                listArray[i] = new List<RasterizationMesh>();
            }
            for (int j = 0; j < meshes.Count; j++)
            {
                RasterizationMesh item = meshes[j];
                Bounds b = item.bounds;
                b.Expand(amount);
                IntRect touchingTiles = this.GetTouchingTiles(b);
                for (int k = touchingTiles.ymin; k <= touchingTiles.ymax; k++)
                {
                    for (int m = touchingTiles.xmin; m <= touchingTiles.xmax; m++)
                    {
                        listArray[m + (k * this.tileXCount)].Add(item);
                    }
                }
            }
            return listArray;
        }

        public override void RelocateNodes(Matrix4x4 oldMatrix, Matrix4x4 newMatrix)
        {
            if (this.tiles != null)
            {
                Matrix4x4 inverse = oldMatrix.inverse;
                Matrix4x4 matrixx2 = newMatrix * inverse;
                if (this.tiles.Length > 1)
                {
                    throw new Exception("RelocateNodes cannot be used on tiled recast graphs");
                }
                for (int i = 0; i < this.tiles.Length; i++)
                {
                    NavmeshTile tile = this.tiles[i];
                    if (tile != null)
                    {
                        Int3[] verts = tile.verts;
                        for (int j = 0; j < verts.Length; j++)
                        {
                            verts[j] = (Int3) matrixx2.MultiplyPoint((Vector3) verts[j]);
                        }
                        for (int k = 0; k < tile.nodes.Length; k++)
                        {
                            tile.nodes[k].UpdatePositionFromVertices();
                        }
                        tile.bbTree.RebuildFrom(tile.nodes);
                    }
                }
            }
            base.SetMatrix(newMatrix);
        }

        private void RemoveConnectionsFromTile(NavmeshTile tile)
        {
            if (tile.x > 0)
            {
                int num = tile.x - 1;
                for (int i = tile.z; i < (tile.z + tile.d); i++)
                {
                    this.RemoveConnectionsFromTo(this.tiles[num + (i * this.tileXCount)], tile);
                }
            }
            if ((tile.x + tile.w) < this.tileXCount)
            {
                int num3 = tile.x + tile.w;
                for (int j = tile.z; j < (tile.z + tile.d); j++)
                {
                    this.RemoveConnectionsFromTo(this.tiles[num3 + (j * this.tileXCount)], tile);
                }
            }
            if (tile.z > 0)
            {
                int num5 = tile.z - 1;
                for (int k = tile.x; k < (tile.x + tile.w); k++)
                {
                    this.RemoveConnectionsFromTo(this.tiles[k + (num5 * this.tileXCount)], tile);
                }
            }
            if ((tile.z + tile.d) < this.tileZCount)
            {
                int num7 = tile.z + tile.d;
                for (int m = tile.x; m < (tile.x + tile.w); m++)
                {
                    this.RemoveConnectionsFromTo(this.tiles[m + (num7 * this.tileXCount)], tile);
                }
            }
        }

        private void RemoveConnectionsFromTo(NavmeshTile a, NavmeshTile b)
        {
            if (((a != null) && (b != null)) && (a != b))
            {
                int num = b.x + (b.z * this.tileXCount);
                for (int i = 0; i < a.nodes.Length; i++)
                {
                    TriangleMeshNode node = a.nodes[i];
                    if (node.connections != null)
                    {
                        for (int j = 0; j < node.connections.Length; j++)
                        {
                            TriangleMeshNode node2 = node.connections[j] as TriangleMeshNode;
                            if (node2 != null)
                            {
                                int num4 = (node2.GetVertexIndex(0) >> 12) & 0x7ffff;
                                if (num4 == num)
                                {
                                    node.RemoveConnection(node.connections[j]);
                                    j--;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ReplaceTile(int x, int z, Int3[] verts, int[] tris, bool worldSpace)
        {
            this.ReplaceTile(x, z, 1, 1, verts, tris, worldSpace);
        }

        public void ReplaceTile(int x, int z, int w, int d, Int3[] verts, int[] tris, bool worldSpace)
        {
            if ((((x + w) > this.tileXCount) || ((z + d) > this.tileZCount)) || ((x < 0) || (z < 0)))
            {
                object[] objArray1 = new object[] { "Tile is placed at an out of bounds position or extends out of the graph bounds (", x, ", ", z, " [", w, ", ", d, "] ", this.tileXCount, " ", this.tileZCount, ")" };
                throw new ArgumentException(string.Concat(objArray1));
            }
            if ((w < 1) || (d < 1))
            {
                object[] objArray2 = new object[] { "width and depth must be greater or equal to 1. Was ", w, ", ", d };
                throw new ArgumentException(string.Concat(objArray2));
            }
            for (int i = z; i < (z + d); i++)
            {
                for (int n = x; n < (x + w); n++)
                {
                    NavmeshTile tile = this.tiles[n + (i * this.tileXCount)];
                    if (tile != null)
                    {
                        this.RemoveConnectionsFromTile(tile);
                        for (int num3 = 0; num3 < tile.nodes.Length; num3++)
                        {
                            tile.nodes[num3].Destroy();
                        }
                        for (int num4 = tile.z; num4 < (tile.z + tile.d); num4++)
                        {
                            for (int num5 = tile.x; num5 < (tile.x + tile.w); num5++)
                            {
                                NavmeshTile tile2 = this.tiles[num5 + (num4 * this.tileXCount)];
                                if ((tile2 == null) || (tile2 != tile))
                                {
                                    throw new Exception("This should not happen");
                                }
                                if (((num4 < z) || (num4 >= (z + d))) || ((num5 < x) || (num5 >= (x + w))))
                                {
                                    this.tiles[num5 + (num4 * this.tileXCount)] = NewEmptyTile(num5, num4);
                                    if (this.batchTileUpdate)
                                    {
                                        this.batchUpdatedTiles.Add(num5 + (num4 * this.tileXCount));
                                    }
                                }
                                else
                                {
                                    this.tiles[num5 + (num4 * this.tileXCount)] = null;
                                }
                            }
                        }
                    }
                }
            }
            NavmeshTile graph = new NavmeshTile();
            graph.x = x;
            graph.z = z;
            graph.w = w;
            graph.d = d;
            graph.tris = tris;
            graph.verts = verts;
            graph.bbTree = new BBTree();
            if ((graph.tris.Length % 3) != 0)
            {
                throw new ArgumentException("Triangle array's length must be a multiple of 3 (tris)");
            }
            if (graph.verts.Length > 0xffff)
            {
                throw new ArgumentException("Too many vertices per tile (more than 65535)");
            }
            if (!worldSpace)
            {
                if (!Mathf.Approximately(((x * this.tileSizeX) * this.cellSize) * 1000f, (float) Math.Round((double) (((x * this.tileSizeX) * this.cellSize) * 1000f))))
                {
                    UnityEngine.Debug.LogWarning("Possible numerical imprecision. Consider adjusting tileSize and/or cellSize");
                }
                if (!Mathf.Approximately(((z * this.tileSizeZ) * this.cellSize) * 1000f, (float) Math.Round((double) (((z * this.tileSizeZ) * this.cellSize) * 1000f))))
                {
                    UnityEngine.Debug.LogWarning("Possible numerical imprecision. Consider adjusting tileSize and/or cellSize");
                }
                Int3 num6 = (Int3) (new Vector3((x * this.tileSizeX) * this.cellSize, 0f, (z * this.tileSizeZ) * this.cellSize) + this.forcedBounds.min);
                for (int num7 = 0; num7 < verts.Length; num7++)
                {
                    verts[num7] += num6;
                }
            }
            TriangleMeshNode[] nodes = new TriangleMeshNode[graph.tris.Length / 3];
            graph.nodes = nodes;
            int length = AstarPath.active.astarData.graphs.Length;
            TriangleMeshNode.SetNavmeshHolder(length, graph);
            int num9 = x + (z * this.tileXCount);
            num9 = num9 << 12;
            for (int j = 0; j < nodes.Length; j++)
            {
                TriangleMeshNode node = new TriangleMeshNode(base.active);
                nodes[j] = node;
                node.GraphIndex = (uint) length;
                node.v0 = graph.tris[j * 3] | num9;
                node.v1 = graph.tris[(j * 3) + 1] | num9;
                node.v2 = graph.tris[(j * 3) + 2] | num9;
                if (!VectorMath.IsClockwiseXZ(node.GetVertex(0), node.GetVertex(1), node.GetVertex(2)))
                {
                    int num11 = node.v0;
                    node.v0 = node.v2;
                    node.v2 = num11;
                }
                node.Walkable = true;
                node.Penalty = base.initialPenalty;
                node.UpdatePositionFromVertices();
            }
            graph.bbTree.RebuildFrom(nodes);
            this.CreateNodeConnections(graph.nodes);
            for (int k = z; k < (z + d); k++)
            {
                for (int num13 = x; num13 < (x + w); num13++)
                {
                    this.tiles[num13 + (k * this.tileXCount)] = graph;
                }
            }
            if (this.batchTileUpdate)
            {
                this.batchUpdatedTiles.Add(x + (z * this.tileXCount));
            }
            else
            {
                this.ConnectTileWithNeighbours(graph);
            }
            TriangleMeshNode.SetNavmeshHolder(length, null);
            length = AstarPath.active.astarData.GetGraphIndex(this);
            for (int m = 0; m < nodes.Length; m++)
            {
                nodes[m].GraphIndex = (uint) length;
            }
        }

        [DebuggerHidden]
        protected IEnumerable<Progress> ScanAllTiles()
        {
            <ScanAllTiles>c__Iterator19 iterator = new <ScanAllTiles>c__Iterator19();
            iterator.<>f__this = this;
            iterator.$PC = -2;
            return iterator;
        }

        [DebuggerHidden]
        public override IEnumerable<Progress> ScanInternal()
        {
            <ScanInternal>c__Iterator18 iterator = new <ScanInternal>c__Iterator18();
            iterator.<>f__this = this;
            iterator.$PC = -2;
            return iterator;
        }

        public override void SerializeExtraInfo(GraphSerializationContext ctx)
        {
            BinaryWriter writer = ctx.writer;
            if (this.tiles == null)
            {
                writer.Write(-1);
            }
            else
            {
                writer.Write(this.tileXCount);
                writer.Write(this.tileZCount);
                for (int i = 0; i < this.tileZCount; i++)
                {
                    for (int j = 0; j < this.tileXCount; j++)
                    {
                        NavmeshTile tile = this.tiles[j + (i * this.tileXCount)];
                        if (tile == null)
                        {
                            throw new Exception("NULL Tile");
                        }
                        writer.Write(tile.x);
                        writer.Write(tile.z);
                        if ((tile.x == j) && (tile.z == i))
                        {
                            writer.Write(tile.w);
                            writer.Write(tile.d);
                            writer.Write(tile.tris.Length);
                            for (int k = 0; k < tile.tris.Length; k++)
                            {
                                writer.Write(tile.tris[k]);
                            }
                            writer.Write(tile.verts.Length);
                            for (int m = 0; m < tile.verts.Length; m++)
                            {
                                ctx.SerializeInt3(tile.verts[m]);
                            }
                            writer.Write(tile.nodes.Length);
                            for (int n = 0; n < tile.nodes.Length; n++)
                            {
                                tile.nodes[n].SerializeNode(ctx);
                            }
                        }
                    }
                }
            }
        }

        public void SnapForceBoundsToScene()
        {
            List<RasterizationMesh> list = this.CollectMeshes(new Bounds(Vector3.zero, new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity)));
            if (list.Count != 0)
            {
                Bounds bounds = list[0].bounds;
                for (int i = 1; i < list.Count; i++)
                {
                    bounds.Encapsulate(list[i].bounds);
                }
                this.forcedBoundsCenter = bounds.center;
                this.forcedBoundsSize = bounds.size;
            }
        }

        public void StartBatchTileUpdate()
        {
            if (this.batchTileUpdate)
            {
                throw new InvalidOperationException("Calling StartBatchLoad when batching is already enabled");
            }
            this.batchTileUpdate = true;
        }

        public void UpdateArea(GraphUpdateObject guo)
        {
            IntRect touchingTiles = this.GetTouchingTiles(guo.bounds);
            if (!guo.updatePhysics)
            {
                for (int i = touchingTiles.ymin; i <= touchingTiles.ymax; i++)
                {
                    for (int j = touchingTiles.xmin; j <= touchingTiles.xmax; j++)
                    {
                        NavmeshTile graph = this.tiles[(i * this.tileXCount) + j];
                        NavMeshGraph.UpdateArea(guo, graph);
                    }
                }
            }
            else
            {
                if (!this.dynamic)
                {
                    throw new Exception("Recast graph must be marked as dynamic to enable graph updates with updatePhysics = true");
                }
                Voxelize globalVox = this.globalVox;
                if (globalVox == null)
                {
                    throw new InvalidOperationException("No Voxelizer object. UpdateAreaInit should have been called before this function.");
                }
                for (int k = touchingTiles.xmin; k <= touchingTiles.xmax; k++)
                {
                    for (int num4 = touchingTiles.ymin; num4 <= touchingTiles.ymax; num4++)
                    {
                        this.RemoveConnectionsFromTile(this.tiles[k + (num4 * this.tileXCount)]);
                    }
                }
                for (int m = touchingTiles.xmin; m <= touchingTiles.xmax; m++)
                {
                    for (int num6 = touchingTiles.ymin; num6 <= touchingTiles.ymax; num6++)
                    {
                        this.BuildTileMesh(globalVox, m, num6);
                    }
                }
                uint graphIndex = (uint) AstarPath.active.astarData.GetGraphIndex(this);
                for (int n = touchingTiles.xmin; n <= touchingTiles.xmax; n++)
                {
                    for (int num9 = touchingTiles.ymin; num9 <= touchingTiles.ymax; num9++)
                    {
                        NavmeshTile tile2 = this.tiles[n + (num9 * this.tileXCount)];
                        GraphNode[] nodes = tile2.nodes;
                        for (int num10 = 0; num10 < nodes.Length; num10++)
                        {
                            nodes[num10].GraphIndex = graphIndex;
                        }
                    }
                }
                touchingTiles = IntRect.Intersection(touchingTiles.Expand(1), new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1));
                for (int num11 = touchingTiles.xmin; num11 <= touchingTiles.xmax; num11++)
                {
                    for (int num12 = touchingTiles.ymin; num12 <= touchingTiles.ymax; num12++)
                    {
                        if ((num11 < (this.tileXCount - 1)) && touchingTiles.Contains(num11 + 1, num12))
                        {
                            this.ConnectTiles(this.tiles[num11 + (num12 * this.tileXCount)], this.tiles[(num11 + 1) + (num12 * this.tileXCount)]);
                        }
                        if ((num12 < (this.tileZCount - 1)) && touchingTiles.Contains(num11, num12 + 1))
                        {
                            this.ConnectTiles(this.tiles[num11 + (num12 * this.tileXCount)], this.tiles[num11 + ((num12 + 1) * this.tileXCount)]);
                        }
                    }
                }
            }
        }

        public void UpdateAreaInit(GraphUpdateObject o)
        {
            if (o.updatePhysics)
            {
                if (!this.dynamic)
                {
                    throw new Exception("Recast graph must be marked as dynamic to enable graph updates");
                }
                RelevantGraphSurface.UpdateAllPositions();
                IntRect touchingTiles = this.GetTouchingTiles(o.bounds);
                Bounds tileBounds = this.GetTileBounds(touchingTiles);
                tileBounds.Expand((Vector3) ((new Vector3(1f, 0f, 1f) * this.TileBorderSizeInWorldUnits) * 2f));
                List<RasterizationMesh> list = this.CollectMeshes(tileBounds);
                Voxelize globalVox = this.globalVox;
                if (globalVox == null)
                {
                    globalVox = new Voxelize(this.CellHeight, this.cellSize, this.walkableClimb, this.walkableHeight, this.maxSlope);
                    globalVox.maxEdgeLength = this.maxEdgeLength;
                    if (this.dynamic)
                    {
                        this.globalVox = globalVox;
                    }
                }
                globalVox.inputMeshes = list;
            }
        }

        private float CellHeight
        {
            get
            {
                return Mathf.Max((float) (this.forcedBounds.size.y / 64000f), (float) 0.001f);
            }
        }

        private int CharacterRadiusInVoxels
        {
            get
            {
                return Mathf.CeilToInt((this.characterRadius / this.cellSize) - 0.1f);
            }
        }

        public Bounds forcedBounds
        {
            get
            {
                return new Bounds(this.forcedBoundsCenter, this.forcedBoundsSize);
            }
        }

        private int TileBorderSizeInVoxels
        {
            get
            {
                return (this.CharacterRadiusInVoxels + 3);
            }
        }

        private float TileBorderSizeInWorldUnits
        {
            get
            {
                return (this.TileBorderSizeInVoxels * this.cellSize);
            }
        }

        [CompilerGenerated]
        private sealed class <OnDrawGizmos>c__AnonStorey255
        {
            internal RecastGraph <>f__this;
            internal PathHandler debugData;

            internal bool <>m__28(GraphNode _node)
            {
                TriangleMeshNode node = _node as TriangleMeshNode;
                if (AstarPath.active.showSearchTree && (this.debugData != null))
                {
                    bool flag = NavGraph.InSearchTree(node, AstarPath.active.debugPath);
                    if ((flag && this.<>f__this.showNodeConnections) && (this.debugData.GetPathNode(node).parent != null))
                    {
                        Gizmos.color = this.<>f__this.NodeColor(node, this.debugData);
                        Gizmos.DrawLine((Vector3) node.position, (Vector3) this.debugData.GetPathNode(node).parent.node.position);
                    }
                    if (this.<>f__this.showMeshOutline)
                    {
                        Gizmos.color = !node.Walkable ? AstarColor.UnwalkableNode : this.<>f__this.NodeColor(node, this.debugData);
                        if (!flag)
                        {
                            Gizmos.color *= new Color(1f, 1f, 1f, 0.1f);
                        }
                        Gizmos.DrawLine((Vector3) node.GetVertex(0), (Vector3) node.GetVertex(1));
                        Gizmos.DrawLine((Vector3) node.GetVertex(1), (Vector3) node.GetVertex(2));
                        Gizmos.DrawLine((Vector3) node.GetVertex(2), (Vector3) node.GetVertex(0));
                    }
                }
                else
                {
                    if (this.<>f__this.showNodeConnections)
                    {
                        Gizmos.color = this.<>f__this.NodeColor(node, null);
                        for (int i = 0; i < node.connections.Length; i++)
                        {
                            Gizmos.DrawLine((Vector3) node.position, Vector3.Lerp((Vector3) node.connections[i].position, (Vector3) node.position, 0.4f));
                        }
                    }
                    if (this.<>f__this.showMeshOutline)
                    {
                        Gizmos.color = !node.Walkable ? AstarColor.UnwalkableNode : this.<>f__this.NodeColor(node, this.debugData);
                        Gizmos.DrawLine((Vector3) node.GetVertex(0), (Vector3) node.GetVertex(1));
                        Gizmos.DrawLine((Vector3) node.GetVertex(1), (Vector3) node.GetVertex(2));
                        Gizmos.DrawLine((Vector3) node.GetVertex(2), (Vector3) node.GetVertex(0));
                    }
                }
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <ScanAllTiles>c__Iterator19 : IEnumerator, IEnumerable<Progress>, IEnumerator<Progress>, IDisposable, IEnumerable
        {
            internal Progress $current;
            internal int $PC;
            internal Queue<Int2> <$s_38>__10;
            internal Queue<Int2> <$s_39>__16;
            internal RecastGraph <>f__this;
            internal List<RasterizationMesh>[] <buckets>__1;
            internal int <coordinateSum>__12;
            internal int <count>__15;
            internal int <count>__9;
            internal uint <graphIndex>__11;
            internal int <i>__13;
            internal int <i>__14;
            internal int <i>__7;
            internal List<RasterizationMesh> <meshes>__0;
            internal int <threadCount>__5;
            internal Queue<Int2> <tileQueue>__2;
            internal int <timeoutMillis>__8;
            internal ManualResetEvent[] <waitEvents>__6;
            internal int <x>__4;
            internal int <z>__3;

            internal void <>m__29(object state)
            {
                this.<>f__this.BuildTiles(this.<tileQueue>__2, this.<buckets>__1, state as ManualResetEvent);
            }

            internal bool <>m__2A(GraphNode node)
            {
                node.GraphIndex = this.<graphIndex>__11;
                return true;
            }

            internal void <>m__2B(object state)
            {
                this.<>f__this.ConnectTiles(this.<tileQueue>__2, state as ManualResetEvent);
            }

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<>f__this.InitializeTileInfo();
                        if (!this.<>f__this.scanEmptyGraph)
                        {
                            this.$current = new Progress(0f, "Finding Meshes");
                            this.$PC = 1;
                            goto Label_057A;
                        }
                        this.<>f__this.FillWithEmptyTiles();
                        goto Label_0578;

                    case 1:
                        this.<meshes>__0 = this.<>f__this.CollectMeshes(this.<>f__this.forcedBounds);
                        this.<>f__this.walkableClimb = Mathf.Min(this.<>f__this.walkableClimb, this.<>f__this.walkableHeight);
                        this.<buckets>__1 = this.<>f__this.PutMeshesIntoTileBuckets(this.<meshes>__0);
                        this.<tileQueue>__2 = new Queue<Int2>();
                        this.<z>__3 = 0;
                        while (this.<z>__3 < this.<>f__this.tileZCount)
                        {
                            this.<x>__4 = 0;
                            while (this.<x>__4 < this.<>f__this.tileXCount)
                            {
                                this.<tileQueue>__2.Enqueue(new Int2(this.<x>__4, this.<z>__3));
                                this.<x>__4++;
                            }
                            this.<z>__3++;
                        }
                        this.<threadCount>__5 = AstarPath.CalculateThreadCount(ThreadCount.AutomaticHighLoad);
                        this.<waitEvents>__6 = new ManualResetEvent[this.<threadCount>__5];
                        this.<i>__7 = 0;
                        while (this.<i>__7 < this.<waitEvents>__6.Length)
                        {
                            this.<waitEvents>__6[this.<i>__7] = new ManualResetEvent(false);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(this.<>m__29), this.<waitEvents>__6[this.<i>__7]);
                            this.<i>__7++;
                        }
                        this.<timeoutMillis>__8 = !Application.isPlaying ? 200 : 1;
                        break;

                    case 2:
                        break;

                    case 3:
                        this.<graphIndex>__11 = (uint) AstarPath.active.astarData.GetGraphIndex(this.<>f__this);
                        this.<>f__this.GetNodes(new GraphNodeDelegateCancelable(this.<>m__2A));
                        this.<coordinateSum>__12 = 0;
                        while (this.<coordinateSum>__12 <= 1)
                        {
                            this.<i>__13 = 0;
                            while (this.<i>__13 < this.<>f__this.tiles.Length)
                            {
                                if (((this.<>f__this.tiles[this.<i>__13].x + this.<>f__this.tiles[this.<i>__13].z) % 2) == this.<coordinateSum>__12)
                                {
                                    this.<tileQueue>__2.Enqueue(new Int2(this.<>f__this.tiles[this.<i>__13].x, this.<>f__this.tiles[this.<i>__13].z));
                                }
                                this.<i>__13++;
                            }
                            this.<i>__14 = 0;
                            while (this.<i>__14 < this.<waitEvents>__6.Length)
                            {
                                this.<waitEvents>__6[this.<i>__14].Reset();
                                ThreadPool.QueueUserWorkItem(new WaitCallback(this.<>m__2B), this.<waitEvents>__6[this.<i>__14]);
                                this.<i>__14++;
                            }
                        Label_0541:
                            while (!WaitHandle.WaitAll(this.<waitEvents>__6, this.<timeoutMillis>__8))
                            {
                                this.<$s_39>__16 = this.<tileQueue>__2;
                                lock (this.<$s_39>__16)
                                {
                                    this.<count>__15 = this.<tileQueue>__2.Count;
                                }
                                object[] objArray2 = new object[] { "Connecting Tile ", (this.<>f__this.tiles.Length - this.<count>__15) + 1, "/", this.<>f__this.tiles.Length, " (Phase ", this.<coordinateSum>__12 + 1, ")" };
                                this.$current = new Progress(Mathf.Lerp(0.9f, 1f, ((float) ((this.<>f__this.tiles.Length - this.<count>__15) + 1)) / ((float) this.<>f__this.tiles.Length)), string.Concat(objArray2));
                                this.$PC = 4;
                                goto Label_057A;
                            }
                            this.<coordinateSum>__12++;
                        }
                        this.$PC = -1;
                        goto Label_0578;

                    case 4:
                        goto Label_0541;

                    default:
                        goto Label_0578;
                }
                while (!WaitHandle.WaitAll(this.<waitEvents>__6, this.<timeoutMillis>__8))
                {
                    this.<$s_38>__10 = this.<tileQueue>__2;
                    lock (this.<$s_38>__10)
                    {
                        this.<count>__9 = this.<tileQueue>__2.Count;
                    }
                    object[] objArray1 = new object[] { "Generating Tile ", (this.<>f__this.tiles.Length - this.<count>__9) + 1, "/", this.<>f__this.tiles.Length };
                    this.$current = new Progress(Mathf.Lerp(0.1f, 0.9f, ((float) ((this.<>f__this.tiles.Length - this.<count>__9) + 1)) / ((float) this.<>f__this.tiles.Length)), string.Concat(objArray1));
                    this.$PC = 2;
                    goto Label_057A;
                }
                this.$current = new Progress(0.9f, "Assigning Graph Indices");
                this.$PC = 3;
                goto Label_057A;
            Label_0578:
                return false;
            Label_057A:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<Progress> IEnumerable<Progress>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                RecastGraph.<ScanAllTiles>c__Iterator19 iterator = new RecastGraph.<ScanAllTiles>c__Iterator19();
                iterator.<>f__this = this.<>f__this;
                return iterator;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Pathfinding.Progress>.GetEnumerator();
            }

            Progress IEnumerator<Progress>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ScanInternal>c__Iterator18 : IEnumerator, IEnumerable<Progress>, IEnumerator<Progress>, IDisposable, IEnumerable
        {
            internal Progress $current;
            internal int $PC;
            internal IEnumerator<Progress> <$s_35>__0;
            internal RecastGraph <>f__this;
            internal Progress <p>__1;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            if (this.<$s_35>__0 == null)
                            {
                            }
                            this.<$s_35>__0.Dispose();
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        TriangleMeshNode.SetNavmeshHolder(AstarPath.active.astarData.GetGraphIndex(this.<>f__this), this.<>f__this);
                        this.<$s_35>__0 = this.<>f__this.ScanAllTiles().GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00CD;
                }
                try
                {
                    while (this.<$s_35>__0.MoveNext())
                    {
                        this.<p>__1 = this.<$s_35>__0.Current;
                        this.$current = this.<p>__1;
                        this.$PC = 1;
                        flag = true;
                        return true;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    if (this.<$s_35>__0 == null)
                    {
                    }
                    this.<$s_35>__0.Dispose();
                }
                this.$PC = -1;
            Label_00CD:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<Progress> IEnumerable<Progress>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                RecastGraph.<ScanInternal>c__Iterator18 iterator = new RecastGraph.<ScanInternal>c__Iterator18();
                iterator.<>f__this = this.<>f__this;
                return iterator;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Pathfinding.Progress>.GetEnumerator();
            }

            Progress IEnumerator<Progress>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        public class NavmeshTile : INavmesh, INavmeshHolder
        {
            public BBTree bbTree;
            public int d;
            public bool flag;
            public TriangleMeshNode[] nodes;
            public int[] tris;
            public Int3[] verts;
            public int w;
            public int x;
            public int z;

            public void GetNodes(GraphNodeDelegateCancelable del)
            {
                if (this.nodes != null)
                {
                    for (int i = 0; (i < this.nodes.Length) && del(this.nodes[i]); i++)
                    {
                    }
                }
            }

            public void GetTileCoordinates(int tileIndex, out int x, out int z)
            {
                x = this.x;
                z = this.z;
            }

            public Int3 GetVertex(int index)
            {
                int num = index & 0xfff;
                return this.verts[num];
            }

            public int GetVertexArrayIndex(int index)
            {
                return (index & 0xfff);
            }
        }

        public enum RelevantGraphSurfaceMode
        {
            DoNotRequire,
            OnlyForCompletelyInsideTile,
            RequireForAll
        }
    }
}

