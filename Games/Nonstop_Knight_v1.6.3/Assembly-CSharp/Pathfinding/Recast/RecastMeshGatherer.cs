namespace Pathfinding.Recast
{
    using Pathfinding;
    using Pathfinding.Util;
    using Pathfinding.Voxels;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal class RecastMeshGatherer
    {
        private readonly Bounds bounds;
        private static readonly int[] BoxColliderTris = new int[] { 
            0, 1, 2, 0, 2, 3, 6, 5, 4, 7, 6, 4, 0, 5, 1, 0, 
            4, 5, 1, 6, 2, 1, 5, 6, 2, 7, 3, 2, 6, 7, 3, 4, 
            0, 3, 7, 4
         };
        private static readonly Vector3[] BoxColliderVerts = new Vector3[] { new Vector3(-1f, -1f, -1f), new Vector3(1f, -1f, -1f), new Vector3(1f, -1f, 1f), new Vector3(-1f, -1f, 1f), new Vector3(-1f, 1f, -1f), new Vector3(1f, 1f, -1f), new Vector3(1f, 1f, 1f), new Vector3(-1f, 1f, 1f) };
        private List<CapsuleCache> capsuleCache = new List<CapsuleCache>();
        private readonly float colliderRasterizeDetail;
        private readonly LayerMask mask;
        private readonly List<string> tagMask;
        private readonly int terrainSampleSize;

        public RecastMeshGatherer(Bounds bounds, int terrainSampleSize, LayerMask mask, List<string> tagMask, float colliderRasterizeDetail)
        {
            terrainSampleSize = Math.Max(terrainSampleSize, 1);
            this.bounds = bounds;
            this.terrainSampleSize = terrainSampleSize;
            this.mask = mask;
            if (tagMask == null)
            {
            }
            this.tagMask = new List<string>();
            this.colliderRasterizeDetail = colliderRasterizeDetail;
        }

        private static int CeilDivision(int lhs, int rhs)
        {
            return (((lhs + rhs) - 1) / rhs);
        }

        public void CollectColliderMeshes(List<RasterizationMesh> result)
        {
            Collider[] colliderArray = UnityEngine.Object.FindObjectsOfType<Collider>();
            if ((this.tagMask.Count > 0) || (this.mask != 0))
            {
                for (int i = 0; i < colliderArray.Length; i++)
                {
                    Collider col = colliderArray[i];
                    if (((((this.mask >> (col.gameObject.layer & 0x1f)) & 1) != 0) || this.tagMask.Contains(col.tag)) && ((col.enabled && !col.isTrigger) && (col.bounds.Intersects(this.bounds) && (col.GetComponent<RecastMeshObj>() == null))))
                    {
                        RasterizationMesh item = this.RasterizeCollider(col);
                        if (item != null)
                        {
                            result.Add(item);
                        }
                    }
                }
            }
            this.capsuleCache.Clear();
        }

        public void CollectRecastMeshObjs(List<RasterizationMesh> buffer)
        {
            List<RecastMeshObj> list = ListPool<RecastMeshObj>.Claim();
            RecastMeshObj.GetAllInBounds(list, this.bounds);
            Dictionary<Mesh, Vector3[]> dictionary = new Dictionary<Mesh, Vector3[]>();
            Dictionary<Mesh, int[]> dictionary2 = new Dictionary<Mesh, int[]>();
            for (int i = 0; i < list.Count; i++)
            {
                MeshFilter meshFilter = list[i].GetMeshFilter();
                Renderer renderer = (meshFilter == null) ? null : meshFilter.GetComponent<Renderer>();
                if ((meshFilter != null) && (renderer != null))
                {
                    Mesh sharedMesh = meshFilter.sharedMesh;
                    RasterizationMesh item = new RasterizationMesh();
                    item.matrix = renderer.localToWorldMatrix;
                    item.original = meshFilter;
                    item.area = list[i].area;
                    if (dictionary.ContainsKey(sharedMesh))
                    {
                        item.vertices = dictionary[sharedMesh];
                        item.triangles = dictionary2[sharedMesh];
                    }
                    else
                    {
                        item.vertices = sharedMesh.vertices;
                        item.triangles = sharedMesh.triangles;
                        dictionary[sharedMesh] = item.vertices;
                        dictionary2[sharedMesh] = item.triangles;
                    }
                    item.bounds = renderer.bounds;
                    buffer.Add(item);
                }
                else
                {
                    Collider col = list[i].GetCollider();
                    if (col == null)
                    {
                        Debug.LogError("RecastMeshObject (" + list[i].gameObject.name + ") didn't have a collider or MeshFilter+Renderer attached", list[i].gameObject);
                    }
                    else
                    {
                        RasterizationMesh mesh3 = this.RasterizeCollider(col);
                        if (mesh3 != null)
                        {
                            mesh3.area = list[i].area;
                            buffer.Add(mesh3);
                        }
                    }
                }
            }
            this.capsuleCache.Clear();
            ListPool<RecastMeshObj>.Release(list);
        }

        public void CollectSceneMeshes(List<RasterizationMesh> meshes)
        {
            if ((this.tagMask.Count > 0) || (this.mask != 0))
            {
                List<MeshFilter> list = FilterMeshes(UnityEngine.Object.FindObjectsOfType<MeshFilter>(), this.tagMask, this.mask);
                Dictionary<Mesh, Vector3[]> dictionary = new Dictionary<Mesh, Vector3[]>();
                Dictionary<Mesh, int[]> dictionary2 = new Dictionary<Mesh, int[]>();
                bool flag = false;
                for (int i = 0; i < list.Count; i++)
                {
                    MeshFilter filter = list[i];
                    Renderer component = filter.GetComponent<Renderer>();
                    if (component.isPartOfStaticBatch)
                    {
                        flag = true;
                    }
                    else if (component.bounds.Intersects(this.bounds))
                    {
                        Mesh sharedMesh = filter.sharedMesh;
                        RasterizationMesh item = new RasterizationMesh();
                        item.matrix = component.localToWorldMatrix;
                        item.original = filter;
                        if (dictionary.ContainsKey(sharedMesh))
                        {
                            item.vertices = dictionary[sharedMesh];
                            item.triangles = dictionary2[sharedMesh];
                        }
                        else
                        {
                            item.vertices = sharedMesh.vertices;
                            item.triangles = sharedMesh.triangles;
                            dictionary[sharedMesh] = item.vertices;
                            dictionary2[sharedMesh] = item.triangles;
                        }
                        item.bounds = component.bounds;
                        meshes.Add(item);
                    }
                    if (flag)
                    {
                        Debug.LogWarning("Some meshes were statically batched. These meshes can not be used for navmesh calculation due to technical constraints.\nDuring runtime scripts cannot access the data of meshes which have been statically batched.\nOne way to solve this problem is to use cached startup (Save & Load tab in the inspector) to only calculate the graph when the game is not playing.");
                    }
                }
            }
        }

        public void CollectTerrainMeshes(bool rasterizeTrees, float desiredChunkSize, List<RasterizationMesh> result)
        {
            Terrain[] terrainArray = UnityEngine.Object.FindObjectsOfType(typeof(Terrain)) as Terrain[];
            if (terrainArray.Length > 0)
            {
                for (int i = 0; i < terrainArray.Length; i++)
                {
                    if (terrainArray[i].terrainData != null)
                    {
                        this.GenerateTerrainChunks(terrainArray[i], this.bounds, desiredChunkSize, result);
                        if (rasterizeTrees)
                        {
                            this.CollectTreeMeshes(terrainArray[i], result);
                        }
                    }
                }
            }
        }

        private void CollectTreeMeshes(Terrain terrain, List<RasterizationMesh> result)
        {
            TerrainData terrainData = terrain.terrainData;
            for (int i = 0; i < terrainData.treeInstances.Length; i++)
            {
                TreeInstance instance = terrainData.treeInstances[i];
                TreePrototype prototype = terrainData.treePrototypes[instance.prototypeIndex];
                if (prototype.prefab != null)
                {
                    Collider component = prototype.prefab.GetComponent<Collider>();
                    Vector3 pos = terrain.transform.position + Vector3.Scale(instance.position, terrainData.size);
                    if (component == null)
                    {
                        Bounds bounds = new Bounds(terrain.transform.position + Vector3.Scale(instance.position, terrainData.size), new Vector3(instance.widthScale, instance.heightScale, instance.widthScale));
                        Matrix4x4 matrix = Matrix4x4.TRS(pos, Quaternion.identity, (Vector3) (new Vector3(instance.widthScale, instance.heightScale, instance.widthScale) * 0.5f));
                        RasterizationMesh item = new RasterizationMesh(BoxColliderVerts, BoxColliderTris, bounds, matrix);
                        result.Add(item);
                    }
                    else
                    {
                        Vector3 s = new Vector3(instance.widthScale, instance.heightScale, instance.widthScale);
                        RasterizationMesh mesh2 = this.RasterizeCollider(component, Matrix4x4.TRS(pos, Quaternion.identity, s));
                        if (mesh2 != null)
                        {
                            mesh2.RecalculateBounds();
                            result.Add(mesh2);
                        }
                    }
                }
            }
        }

        private static List<MeshFilter> FilterMeshes(MeshFilter[] meshFilters, List<string> tagMask, LayerMask layerMask)
        {
            List<MeshFilter> list = new List<MeshFilter>(meshFilters.Length / 3);
            for (int i = 0; i < meshFilters.Length; i++)
            {
                MeshFilter item = meshFilters[i];
                Renderer component = item.GetComponent<Renderer>();
                if (((((component != null) && (item.sharedMesh != null)) && component.enabled) && ((((((int) 1) << item.gameObject.layer) & layerMask) != 0) || tagMask.Contains(item.tag))) && (item.GetComponent<RecastMeshObj>() == null))
                {
                    list.Add(item);
                }
            }
            return list;
        }

        private RasterizationMesh GenerateHeightmapChunk(float[,] heights, Vector3 sampleSize, Vector3 offset, int x0, int z0, int width, int depth, int stride)
        {
            int num = CeilDivision(width, this.terrainSampleSize) + 1;
            int num2 = CeilDivision(depth, this.terrainSampleSize) + 1;
            int length = heights.GetLength(0);
            int num4 = heights.GetLength(1);
            Vector3[] vertices = new Vector3[num * num2];
            for (int i = 0; i < num2; i++)
            {
                for (int k = 0; k < num; k++)
                {
                    int num7 = Math.Min((int) (x0 + (k * stride)), (int) (length - 1));
                    int num8 = Math.Min((int) (z0 + (i * stride)), (int) (num4 - 1));
                    vertices[(i * num) + k] = new Vector3(num8 * sampleSize.x, heights[num7, num8] * sampleSize.y, num7 * sampleSize.z) + offset;
                }
            }
            int[] triangles = new int[(((num - 1) * (num2 - 1)) * 2) * 3];
            int index = 0;
            for (int j = 0; j < (num2 - 1); j++)
            {
                for (int m = 0; m < (num - 1); m++)
                {
                    triangles[index] = (j * num) + m;
                    triangles[index + 1] = ((j * num) + m) + 1;
                    triangles[index + 2] = (((j + 1) * num) + m) + 1;
                    index += 3;
                    triangles[index] = (j * num) + m;
                    triangles[index + 1] = (((j + 1) * num) + m) + 1;
                    triangles[index + 2] = ((j + 1) * num) + m;
                    index += 3;
                }
            }
            RasterizationMesh mesh = new RasterizationMesh(vertices, triangles, new Bounds());
            mesh.RecalculateBounds();
            return mesh;
        }

        private void GenerateTerrainChunks(Terrain terrain, Bounds bounds, float desiredChunkSize, List<RasterizationMesh> result)
        {
            TerrainData terrainData = terrain.terrainData;
            if (terrainData == null)
            {
                throw new ArgumentException("Terrain contains no terrain data");
            }
            Vector3 position = terrain.GetPosition();
            Vector3 center = position + ((Vector3) (terrainData.size * 0.5f));
            Bounds bounds2 = new Bounds(center, terrainData.size);
            if (bounds2.Intersects(bounds))
            {
                int heightmapWidth = terrainData.heightmapWidth;
                int heightmapHeight = terrainData.heightmapHeight;
                float[,] heights = terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);
                Vector3 heightmapScale = terrainData.heightmapScale;
                heightmapScale.y = terrainData.size.y;
                int a = Mathf.CeilToInt(Mathf.Max((float) (desiredChunkSize / (heightmapScale.x * this.terrainSampleSize)), (float) 12f)) * this.terrainSampleSize;
                int num5 = Mathf.CeilToInt(Mathf.Max((float) (desiredChunkSize / (heightmapScale.z * this.terrainSampleSize)), (float) 12f)) * this.terrainSampleSize;
                for (int i = 0; i < heightmapHeight; i += num5)
                {
                    for (int j = 0; j < heightmapWidth; j += a)
                    {
                        int width = Mathf.Min(a, heightmapWidth - j);
                        int depth = Mathf.Min(num5, heightmapHeight - i);
                        RasterizationMesh item = this.GenerateHeightmapChunk(heights, heightmapScale, position, j, i, width, depth, this.terrainSampleSize);
                        result.Add(item);
                    }
                }
            }
        }

        private RasterizationMesh RasterizeBoxCollider(BoxCollider collider, Matrix4x4 localToWorldMatrix)
        {
            Matrix4x4 matrixx = Matrix4x4.TRS(collider.center, Quaternion.identity, (Vector3) (collider.size * 0.5f));
            return new RasterizationMesh(BoxColliderVerts, BoxColliderTris, collider.bounds, localToWorldMatrix * matrixx);
        }

        private RasterizationMesh RasterizeCapsuleCollider(float radius, float height, Bounds bounds, Matrix4x4 localToWorldMatrix)
        {
            Vector3[] verts;
            int num = Mathf.Max(4, Mathf.RoundToInt(this.colliderRasterizeDetail * Mathf.Sqrt(localToWorldMatrix.MultiplyVector(Vector3.one).magnitude)));
            if (num > 100)
            {
                Debug.LogWarning("Very large detail for some collider meshes. Consider decreasing Collider Rasterize Detail (RecastGraph)");
            }
            int num2 = num;
            CapsuleCache item = null;
            for (int i = 0; i < this.capsuleCache.Count; i++)
            {
                CapsuleCache cache2 = this.capsuleCache[i];
                if ((cache2.rows == num) && Mathf.Approximately(cache2.height, height))
                {
                    item = cache2;
                }
            }
            if (item == null)
            {
                verts = new Vector3[(num * num2) + 2];
                List<int> list = new List<int>();
                verts[verts.Length - 1] = Vector3.up;
                for (int j = 0; j < num; j++)
                {
                    for (int num5 = 0; num5 < num2; num5++)
                    {
                        verts[num5 + (j * num2)] = new Vector3(Mathf.Cos(((num5 * 3.141593f) * 2f) / ((float) num2)) * Mathf.Sin((j * 3.141593f) / ((float) (num - 1))), Mathf.Cos((j * 3.141593f) / ((float) (num - 1))) + ((j >= (num / 2)) ? -height : height), Mathf.Sin(((num5 * 3.141593f) * 2f) / ((float) num2)) * Mathf.Sin((j * 3.141593f) / ((float) (num - 1))));
                    }
                }
                verts[verts.Length - 2] = Vector3.down;
                int num6 = 0;
                for (int k = num2 - 1; num6 < num2; k = num6++)
                {
                    list.Add(verts.Length - 1);
                    list.Add((0 * num2) + k);
                    list.Add((0 * num2) + num6);
                }
                for (int m = 1; m < num; m++)
                {
                    int num9 = 0;
                    for (int num10 = num2 - 1; num9 < num2; num10 = num9++)
                    {
                        list.Add((m * num2) + num9);
                        list.Add((m * num2) + num10);
                        list.Add(((m - 1) * num2) + num9);
                        list.Add(((m - 1) * num2) + num10);
                        list.Add(((m - 1) * num2) + num9);
                        list.Add((m * num2) + num10);
                    }
                }
                int num11 = 0;
                for (int n = num2 - 1; num11 < num2; n = num11++)
                {
                    list.Add(verts.Length - 2);
                    list.Add(((num - 1) * num2) + n);
                    list.Add(((num - 1) * num2) + num11);
                }
                item = new CapsuleCache();
                item.rows = num;
                item.height = height;
                item.verts = verts;
                item.tris = list.ToArray();
                this.capsuleCache.Add(item);
            }
            verts = item.verts;
            return new RasterizationMesh(verts, item.tris, bounds, localToWorldMatrix);
        }

        private RasterizationMesh RasterizeCollider(Collider col)
        {
            return this.RasterizeCollider(col, col.transform.localToWorldMatrix);
        }

        private RasterizationMesh RasterizeCollider(Collider col, Matrix4x4 localToWorldMatrix)
        {
            RasterizationMesh mesh = null;
            if (col is BoxCollider)
            {
                return this.RasterizeBoxCollider(col as BoxCollider, localToWorldMatrix);
            }
            if ((col is SphereCollider) || (col is CapsuleCollider))
            {
                SphereCollider collider = col as SphereCollider;
                CapsuleCollider collider2 = col as CapsuleCollider;
                float radius = (collider == null) ? collider2.radius : collider.radius;
                float height = (collider == null) ? (((collider2.height * 0.5f) / radius) - 1f) : 0f;
                Matrix4x4 matrixx = Matrix4x4.TRS((collider == null) ? collider2.center : collider.center, Quaternion.identity, (Vector3) (Vector3.one * radius));
                matrixx = localToWorldMatrix * matrixx;
                return this.RasterizeCapsuleCollider(radius, height, col.bounds, matrixx);
            }
            if (col is MeshCollider)
            {
                MeshCollider collider3 = col as MeshCollider;
                if (collider3.sharedMesh != null)
                {
                    mesh = new RasterizationMesh(collider3.sharedMesh.vertices, collider3.sharedMesh.triangles, collider3.bounds, localToWorldMatrix);
                }
            }
            return mesh;
        }

        private class CapsuleCache
        {
            public float height;
            public int rows;
            public int[] tris;
            public Vector3[] verts;
        }
    }
}

