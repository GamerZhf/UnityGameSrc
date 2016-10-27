namespace Pathfinding.Util
{
    using Pathfinding;
    using Pathfinding.ClipperLib;
    using Pathfinding.Poly2Tri;
    using Pathfinding.Voxels;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class TileHandler
    {
        private readonly RecastGraph _graph;
        private readonly int[] activeTileOffsets;
        private readonly int[] activeTileRotations;
        private readonly TileType[] activeTileTypes;
        private int[] cached_int_array = new int[0x20];
        private readonly Dictionary<Int2, int> cached_Int2_int_dict = new Dictionary<Int2, int>();
        private readonly Dictionary<Int3, int> cached_Int3_int_dict = new Dictionary<Int3, int>();
        private readonly Clipper clipper = new Clipper(0);
        private bool isBatching;
        private readonly bool[] reloadedInBatch;
        private readonly VoxelPolygonClipper simpleClipper;
        private readonly List<TileType> tileTypes = new List<TileType>();
        private readonly int tileXCount;
        private readonly int tileZCount;

        public TileHandler(RecastGraph graph)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }
            if (graph.GetTiles() == null)
            {
                Debug.LogWarning("Creating a TileHandler for a graph with no tiles. Please scan the graph before creating a TileHandler");
            }
            this.tileXCount = graph.tileXCount;
            this.tileZCount = graph.tileZCount;
            this.activeTileTypes = new TileType[this.tileXCount * this.tileZCount];
            this.activeTileRotations = new int[this.activeTileTypes.Length];
            this.activeTileOffsets = new int[this.activeTileTypes.Length];
            this.reloadedInBatch = new bool[this.activeTileTypes.Length];
            this._graph = graph;
        }

        public void ClearTile(int x, int z)
        {
            <ClearTile>c__AnonStorey25B storeyb = new <ClearTile>c__AnonStorey25B();
            storeyb.x = x;
            storeyb.z = z;
            storeyb.<>f__this = this;
            if ((AstarPath.active != null) && (((storeyb.x >= 0) && (storeyb.z >= 0)) && ((storeyb.x < this.graph.tileXCount) && (storeyb.z < this.graph.tileZCount))))
            {
                AstarPath.active.AddWorkItem(new AstarWorkItem(new Func<IWorkItemContext, bool, bool>(storeyb.<>m__34)));
            }
        }

        public void CreateTileTypesFromGraph()
        {
            RecastGraph.NavmeshTile[] tiles = this.graph.GetTiles();
            if (tiles != null)
            {
                if (!this.isValid)
                {
                    throw new InvalidOperationException("Graph tiles are invalid (number of tiles is not equal to width*depth of the graph). You need to create a new tile handler if you have changed the graph.");
                }
                for (int i = 0; i < this.graph.tileZCount; i++)
                {
                    for (int j = 0; j < this.graph.tileXCount; j++)
                    {
                        RecastGraph.NavmeshTile tile = tiles[j + (i * this.graph.tileXCount)];
                        Int3 min = (Int3) this.graph.GetTileBounds(j, i, 1, 1).min;
                        Int3 tileSize = (Int3) (new Int3(this.graph.tileSizeX, 1, this.graph.tileSizeZ) * (1000f * this.graph.cellSize));
                        min += new Int3((tileSize.x * tile.w) / 2, 0, (tileSize.z * tile.d) / 2);
                        min = -min;
                        TileType item = new TileType(tile.verts, tile.tris, tileSize, min, tile.w, tile.d);
                        this.tileTypes.Add(item);
                        int index = j + (i * this.graph.tileXCount);
                        this.activeTileTypes[index] = item;
                        this.activeTileRotations[index] = 0;
                        this.activeTileOffsets[index] = 0;
                    }
                }
            }
        }

        private void CutPoly(Int3[] verts, int[] tris, ref Int3[] outVertsArr, ref int[] outTrisArr, out int outVCount, out int outTCount, Int3[] extraShape, Int3 cuttingOffset, Bounds realBounds, [Optional, DefaultParameterValue(3)] CutMode mode, [Optional, DefaultParameterValue(0)] int perturbate)
        {
            List<NavmeshCut> list7;
            if ((verts.Length == 0) || (tris.Length == 0))
            {
                outVCount = 0;
                outTCount = 0;
                outTrisArr = new int[0];
                outVertsArr = new Int3[0];
                return;
            }
            List<IntPoint> pg = null;
            if ((extraShape == null) && ((mode & CutMode.CutExtra) != 0))
            {
                throw new Exception("extraShape is null and the CutMode specifies that it should be used. Cannot use null shape.");
            }
            if ((mode & CutMode.CutExtra) != 0)
            {
                pg = new List<IntPoint>(extraShape.Length);
                for (int num = 0; num < extraShape.Length; num++)
                {
                    pg.Add(new IntPoint((long) (extraShape[num].x + cuttingOffset.x), (long) (extraShape[num].z + cuttingOffset.z)));
                }
            }
            List<IntPoint> list2 = new List<IntPoint>(5);
            Dictionary<TriangulationPoint, int> dictionary = new Dictionary<TriangulationPoint, int>();
            List<PolygonPoint> list = new List<PolygonPoint>();
            Pathfinding.IntRect b = new Pathfinding.IntRect(verts[0].x, verts[0].z, verts[0].x, verts[0].z);
            for (int i = 0; i < verts.Length; i++)
            {
                b = b.ExpandToContain(verts[i].x, verts[i].z);
            }
            List<Int3> list4 = ListPool<Int3>.Claim(verts.Length * 2);
            List<int> list5 = ListPool<int>.Claim(tris.Length);
            PolyTree polytree = new PolyTree();
            List<List<IntPoint>> solution = new List<List<IntPoint>>();
            Stack<Pathfinding.Poly2Tri.Polygon> stack = new Stack<Pathfinding.Poly2Tri.Polygon>();
            this.clipper.ReverseSolution = true;
            this.clipper.StrictlySimple = true;
            if (mode == CutMode.CutExtra)
            {
                list7 = ListPool<NavmeshCut>.Claim();
            }
            else
            {
                list7 = NavmeshCut.GetAllInRange(realBounds);
            }
            List<int> list8 = ListPool<int>.Claim();
            List<Pathfinding.IntRect> list9 = ListPool<Pathfinding.IntRect>.Claim();
            List<Int2> list10 = ListPool<Int2>.Claim();
            List<List<IntPoint>> buffer = new List<List<IntPoint>>();
            List<bool> list12 = ListPool<bool>.Claim();
            List<bool> list13 = ListPool<bool>.Claim();
            if (perturbate > 10)
            {
                Debug.LogError("Too many perturbations aborting : " + mode);
                Debug.Break();
                outVCount = verts.Length;
                outTCount = tris.Length;
                outTrisArr = tris;
                outVertsArr = verts;
                return;
            }
            System.Random random = null;
            if (perturbate > 0)
            {
                random = new System.Random();
            }
            for (int j = 0; j < list7.Count; j++)
            {
                Bounds bounds = list7[j].GetBounds();
                Int3 num4 = ((Int3) bounds.min) + cuttingOffset;
                Int3 num5 = ((Int3) bounds.max) + cuttingOffset;
                Pathfinding.IntRect a = new Pathfinding.IntRect(num4.x, num4.z, num5.x, num5.z);
                if (Pathfinding.IntRect.Intersects(a, b))
                {
                    Int2 num6 = new Int2(0, 0);
                    if (perturbate > 0)
                    {
                        num6.x = ((random.Next() % 6) * perturbate) - (3 * perturbate);
                        if (num6.x >= 0)
                        {
                            num6.x++;
                        }
                        num6.y = ((random.Next() % 6) * perturbate) - (3 * perturbate);
                        if (num6.y >= 0)
                        {
                            num6.y++;
                        }
                    }
                    int count = buffer.Count;
                    list7[j].GetContour(buffer);
                    for (int num8 = count; num8 < buffer.Count; num8++)
                    {
                        List<IntPoint> list14 = buffer[num8];
                        if (list14.Count == 0)
                        {
                            Debug.LogError("Zero Length Contour");
                            Pathfinding.IntRect item = new Pathfinding.IntRect();
                            list9.Add(item);
                            list10.Add(new Int2(0, 0));
                        }
                        else
                        {
                            IntPoint point3 = list14[0];
                            IntPoint point4 = list14[0];
                            IntPoint point5 = list14[0];
                            IntPoint point6 = list14[0];
                            Pathfinding.IntRect rect3 = new Pathfinding.IntRect(((int) point3.X) + cuttingOffset.x, ((int) point4.Y) + cuttingOffset.y, ((int) point5.X) + cuttingOffset.x, ((int) point6.Y) + cuttingOffset.y);
                            for (int num9 = 0; num9 < list14.Count; num9++)
                            {
                                IntPoint point = list14[num9];
                                point.X += cuttingOffset.x;
                                point.Y += cuttingOffset.z;
                                if (perturbate > 0)
                                {
                                    point.X += num6.x;
                                    point.Y += num6.y;
                                }
                                list14[num9] = point;
                                rect3 = rect3.ExpandToContain((int) point.X, (int) point.Y);
                            }
                            list10.Add(new Int2(num4.y, num5.y));
                            list9.Add(rect3);
                            list12.Add(list7[j].isDual);
                            list13.Add(list7[j].cutsAddedGeom);
                        }
                    }
                }
            }
            List<NavmeshAdd> allInRange = NavmeshAdd.GetAllInRange(realBounds);
            Int3[] vbuffer = verts;
            int[] tbuffer = tris;
            int num10 = -1;
            int index = -3;
            Int3[] vIn = null;
            Int3[] vOut = null;
            Int3 zero = Int3.zero;
            if (allInRange.Count > 0)
            {
                vIn = new Int3[7];
                vOut = new Int3[7];
                zero = (Int3) realBounds.extents;
            }
        Label_0530:
            index += 3;
            while (index >= tbuffer.Length)
            {
                num10++;
                index = 0;
                if (num10 >= allInRange.Count)
                {
                    vbuffer = null;
                    break;
                }
                if (vbuffer == verts)
                {
                    vbuffer = null;
                }
                allInRange[num10].GetMesh(cuttingOffset, ref vbuffer, out tbuffer);
            }
            if (vbuffer != null)
            {
                Int3 num13 = vbuffer[tbuffer[index]];
                Int3 num14 = vbuffer[tbuffer[index + 1]];
                Int3 num15 = vbuffer[tbuffer[index + 2]];
                Pathfinding.IntRect rect4 = new Pathfinding.IntRect(num13.x, num13.z, num13.x, num13.z);
                rect4 = rect4.ExpandToContain(num14.x, num14.z).ExpandToContain(num15.x, num15.z);
                int num16 = Math.Min(num13.y, Math.Min(num14.y, num15.y));
                int num17 = Math.Max(num13.y, Math.Max(num14.y, num15.y));
                list8.Clear();
                bool flag = false;
                for (int num18 = 0; num18 < buffer.Count; num18++)
                {
                    Int2 num54 = list10[num18];
                    int x = num54.x;
                    Int2 num55 = list10[num18];
                    int y = num55.y;
                    if (((Pathfinding.IntRect.Intersects(rect4, list9[num18]) && (y >= num16)) && (x <= num17)) && (list13[num18] || (num10 == -1)))
                    {
                        Int3 num21 = num13;
                        num21.y = x;
                        Int3 num22 = num13;
                        num22.y = y;
                        list8.Add(num18);
                        flag |= list12[num18];
                    }
                }
                if (((list8.Count == 0) && ((mode & CutMode.CutExtra) == 0)) && (((mode & CutMode.CutAll) != 0) && (num10 == -1)))
                {
                    list5.Add(list4.Count);
                    list5.Add(list4.Count + 1);
                    list5.Add(list4.Count + 2);
                    list4.Add(num13);
                    list4.Add(num14);
                    list4.Add(num15);
                }
                else
                {
                    list2.Clear();
                    if (num10 == -1)
                    {
                        list2.Add(new IntPoint((long) num13.x, (long) num13.z));
                        list2.Add(new IntPoint((long) num14.x, (long) num14.z));
                        list2.Add(new IntPoint((long) num15.x, (long) num15.z));
                    }
                    else
                    {
                        vIn[0] = num13;
                        vIn[1] = num14;
                        vIn[2] = num15;
                        int num23 = this.simpleClipper.ClipPolygon(vIn, 3, vOut, 1, 0, 0);
                        if (num23 == 0)
                        {
                            goto Label_0530;
                        }
                        num23 = this.simpleClipper.ClipPolygon(vOut, num23, vIn, -1, 2 * zero.x, 0);
                        if (num23 == 0)
                        {
                            goto Label_0530;
                        }
                        num23 = this.simpleClipper.ClipPolygon(vIn, num23, vOut, 1, 0, 2);
                        if (num23 == 0)
                        {
                            goto Label_0530;
                        }
                        num23 = this.simpleClipper.ClipPolygon(vOut, num23, vIn, -1, 2 * zero.z, 2);
                        if (num23 == 0)
                        {
                            goto Label_0530;
                        }
                        for (int num24 = 0; num24 < num23; num24++)
                        {
                            list2.Add(new IntPoint((long) vIn[num24].x, (long) vIn[num24].z));
                        }
                    }
                    dictionary.Clear();
                    Int3 num25 = num14 - num13;
                    Int3 num26 = num15 - num13;
                    Int3 num27 = num25;
                    Int3 num28 = num26;
                    num27.y = 0;
                    num28.y = 0;
                    for (int num29 = 0; num29 < 0x10; num29++)
                    {
                        if (((((int) mode) >> num29) & 1) != 0)
                        {
                            if ((((int) 1) << num29) == 1)
                            {
                                this.clipper.Clear();
                                this.clipper.AddPolygon(list2, PolyType.ptSubject);
                                for (int num30 = 0; num30 < list8.Count; num30++)
                                {
                                    this.clipper.AddPolygon(buffer[list8[num30]], PolyType.ptClip);
                                }
                                polytree.Clear();
                                this.clipper.Execute(ClipType.ctDifference, polytree, PolyFillType.pftEvenOdd, PolyFillType.pftNonZero);
                            }
                            else if ((((int) 1) << num29) == 2)
                            {
                                if (!flag)
                                {
                                    continue;
                                }
                                this.clipper.Clear();
                                this.clipper.AddPolygon(list2, PolyType.ptSubject);
                                for (int num31 = 0; num31 < list8.Count; num31++)
                                {
                                    if (list12[list8[num31]])
                                    {
                                        this.clipper.AddPolygon(buffer[list8[num31]], PolyType.ptClip);
                                    }
                                }
                                solution.Clear();
                                this.clipper.Execute(ClipType.ctIntersection, solution, PolyFillType.pftEvenOdd, PolyFillType.pftNonZero);
                                this.clipper.Clear();
                                for (int num32 = 0; num32 < solution.Count; num32++)
                                {
                                    this.clipper.AddPolygon(solution[num32], !Clipper.Orientation(solution[num32]) ? PolyType.ptSubject : PolyType.ptClip);
                                }
                                for (int num33 = 0; num33 < list8.Count; num33++)
                                {
                                    if (!list12[list8[num33]])
                                    {
                                        this.clipper.AddPolygon(buffer[list8[num33]], PolyType.ptClip);
                                    }
                                }
                                polytree.Clear();
                                this.clipper.Execute(ClipType.ctDifference, polytree, PolyFillType.pftEvenOdd, PolyFillType.pftNonZero);
                            }
                            else if ((((int) 1) << num29) == 4)
                            {
                                this.clipper.Clear();
                                this.clipper.AddPolygon(list2, PolyType.ptSubject);
                                this.clipper.AddPolygon(pg, PolyType.ptClip);
                                polytree.Clear();
                                this.clipper.Execute(ClipType.ctIntersection, polytree, PolyFillType.pftEvenOdd, PolyFillType.pftNonZero);
                            }
                            for (int num34 = 0; num34 < polytree.ChildCount; num34++)
                            {
                                PolyNode node = polytree.Childs[num34];
                                List<IntPoint> contour = node.Contour;
                                List<PolyNode> childs = node.Childs;
                                if (((childs.Count == 0) && (contour.Count == 3)) && (num10 == -1))
                                {
                                    for (int num35 = 0; num35 < contour.Count; num35++)
                                    {
                                        IntPoint point7 = contour[num35];
                                        IntPoint point8 = contour[num35];
                                        Int3 num36 = new Int3((int) point7.X, 0, (int) point8.Y);
                                        double num37 = ((num14.z - num15.z) * (num13.x - num15.x)) + ((num15.x - num14.x) * (num13.z - num15.z));
                                        if (num37 == 0.0)
                                        {
                                            Debug.LogWarning("Degenerate triangle");
                                        }
                                        else
                                        {
                                            double num38 = (((num14.z - num15.z) * (num36.x - num15.x)) + ((num15.x - num14.x) * (num36.z - num15.z))) / num37;
                                            double num39 = (((num15.z - num13.z) * (num36.x - num15.x)) + ((num13.x - num15.x) * (num36.z - num15.z))) / num37;
                                            num36.y = (int) Math.Round((double) (((num38 * num13.y) + (num39 * num14.y)) + (((1.0 - num38) - num39) * num15.y)));
                                            list5.Add(list4.Count);
                                            list4.Add(num36);
                                        }
                                    }
                                }
                                else
                                {
                                    Pathfinding.Poly2Tri.Polygon p = null;
                                    int num40 = -1;
                                    for (List<IntPoint> list18 = contour; list18 != null; list18 = (num40 >= childs.Count) ? null : childs[num40].Contour)
                                    {
                                        list.Clear();
                                        for (int num41 = 0; num41 < list18.Count; num41++)
                                        {
                                            IntPoint point9 = list18[num41];
                                            IntPoint point10 = list18[num41];
                                            PolygonPoint point2 = new PolygonPoint((double) point9.X, (double) point10.Y);
                                            list.Add(point2);
                                            IntPoint point11 = list18[num41];
                                            IntPoint point12 = list18[num41];
                                            Int3 num42 = new Int3((int) point11.X, 0, (int) point12.Y);
                                            double num43 = ((num14.z - num15.z) * (num13.x - num15.x)) + ((num15.x - num14.x) * (num13.z - num15.z));
                                            if (num43 == 0.0)
                                            {
                                                Debug.LogWarning("Degenerate triangle");
                                            }
                                            else
                                            {
                                                double num44 = (((num14.z - num15.z) * (num42.x - num15.x)) + ((num15.x - num14.x) * (num42.z - num15.z))) / num43;
                                                double num45 = (((num15.z - num13.z) * (num42.x - num15.x)) + ((num13.x - num15.x) * (num42.z - num15.z))) / num43;
                                                num42.y = (int) Math.Round((double) (((num44 * num13.y) + (num45 * num14.y)) + (((1.0 - num44) - num45) * num15.y)));
                                                dictionary[point2] = list4.Count;
                                                list4.Add(num42);
                                            }
                                        }
                                        Pathfinding.Poly2Tri.Polygon poly = null;
                                        if (stack.Count > 0)
                                        {
                                            poly = stack.Pop();
                                            poly.AddPoints(list);
                                        }
                                        else
                                        {
                                            poly = new Pathfinding.Poly2Tri.Polygon(list);
                                        }
                                        if (p == null)
                                        {
                                            p = poly;
                                        }
                                        else
                                        {
                                            p.AddHole(poly);
                                        }
                                        num40++;
                                    }
                                    try
                                    {
                                        P2T.Triangulate(p);
                                    }
                                    catch (PointOnEdgeException)
                                    {
                                        Debug.LogWarning(string.Concat(new object[] { "PointOnEdgeException, perturbating vertices slightly ( at ", num29, " in ", mode, ")" }));
                                        this.CutPoly(verts, tris, ref outVertsArr, ref outTrisArr, out outVCount, out outTCount, extraShape, cuttingOffset, realBounds, mode, perturbate + 1);
                                        return;
                                    }
                                    for (int num46 = 0; num46 < p.Triangles.Count; num46++)
                                    {
                                        DelaunayTriangle triangle = p.Triangles[num46];
                                        list5.Add(dictionary[triangle.Points._0]);
                                        list5.Add(dictionary[triangle.Points._1]);
                                        list5.Add(dictionary[triangle.Points._2]);
                                    }
                                    if (p.Holes != null)
                                    {
                                        for (int num47 = 0; num47 < p.Holes.Count; num47++)
                                        {
                                            p.Holes[num47].Points.Clear();
                                            p.Holes[num47].ClearTriangles();
                                            if (p.Holes[num47].Holes != null)
                                            {
                                                p.Holes[num47].Holes.Clear();
                                            }
                                            stack.Push(p.Holes[num47]);
                                        }
                                    }
                                    p.ClearTriangles();
                                    if (p.Holes != null)
                                    {
                                        p.Holes.Clear();
                                    }
                                    p.Points.Clear();
                                    stack.Push(p);
                                }
                            }
                        }
                    }
                }
                goto Label_0530;
            }
            Dictionary<Int3, int> dictionary2 = this.cached_Int3_int_dict;
            dictionary2.Clear();
            if (this.cached_int_array.Length < list4.Count)
            {
                this.cached_int_array = new int[Math.Max(this.cached_int_array.Length * 2, list4.Count)];
            }
            int[] numArray5 = this.cached_int_array;
            int num48 = 0;
            for (int k = 0; k < list4.Count; k++)
            {
                int num50;
                if (!dictionary2.TryGetValue(list4[k], out num50))
                {
                    dictionary2.Add(list4[k], num48);
                    numArray5[k] = num48;
                    list4[num48] = list4[k];
                    num48++;
                }
                else
                {
                    numArray5[k] = num50;
                }
            }
            outTCount = list5.Count;
            if ((outTrisArr == null) || (outTrisArr.Length < outTCount))
            {
                outTrisArr = new int[outTCount];
            }
            for (int m = 0; m < outTCount; m++)
            {
                outTrisArr[m] = numArray5[list5[m]];
            }
            outVCount = num48;
            if ((outVertsArr == null) || (outVertsArr.Length < outVCount))
            {
                outVertsArr = new Int3[outVCount];
            }
            for (int n = 0; n < outVCount; n++)
            {
                outVertsArr[n] = list4[n];
            }
            for (int num53 = 0; num53 < list7.Count; num53++)
            {
                list7[num53].UsedForCut();
            }
            ListPool<Int3>.Release(list4);
            ListPool<int>.Release(list5);
            ListPool<int>.Release(list8);
            ListPool<Int2>.Release(list10);
            ListPool<bool>.Release(list12);
            ListPool<bool>.Release(list13);
            ListPool<Pathfinding.IntRect>.Release(list9);
            ListPool<NavmeshCut>.Release(list7);
        }

        public void CutShapeWithTile(int x, int z, Int3[] shape, ref Int3[] verts, ref int[] tris, out int vCount, out int tCount)
        {
            if (this.isBatching)
            {
                throw new Exception("Cannot cut with shape when batching. Please stop batching first.");
            }
            int index = x + (z * this.graph.tileXCount);
            if (((x < 0) || (z < 0)) || (((x >= this.graph.tileXCount) || (z >= this.graph.tileZCount)) || (this.activeTileTypes[index] == null)))
            {
                verts = new Int3[0];
                tris = new int[0];
                vCount = 0;
                tCount = 0;
            }
            else
            {
                Int3[] numArray;
                int[] numArray2;
                this.activeTileTypes[index].Load(out numArray, out numArray2, this.activeTileRotations[index], this.activeTileOffsets[index]);
                Bounds realBounds = this.graph.GetTileBounds(x, z, 1, 1);
                Int3 min = (Int3) realBounds.min;
                min = -min;
                this.CutPoly(numArray, numArray2, ref verts, ref tris, out vCount, out tCount, shape, min, realBounds, CutMode.CutExtra, 0);
                for (int i = 0; i < verts.Length; i++)
                {
                    verts[i] -= min;
                }
            }
        }

        private void DelaunayRefinement(Int3[] verts, int[] tris, ref int vCount, ref int tCount, bool delaunay, bool colinear, Int3 worldOffset)
        {
            if ((tCount % 3) != 0)
            {
                throw new Exception("Triangle array length must be a multiple of 3");
            }
            Dictionary<Int2, int> dictionary = this.cached_Int2_int_dict;
            dictionary.Clear();
            for (int i = 0; i < tCount; i += 3)
            {
                if (!VectorMath.IsClockwiseXZ(verts[tris[i]], verts[tris[i + 1]], verts[tris[i + 2]]))
                {
                    int num2 = tris[i];
                    tris[i] = tris[i + 2];
                    tris[i + 2] = num2;
                }
                dictionary[new Int2(tris[i], tris[i + 1])] = i + 2;
                dictionary[new Int2(tris[i + 1], tris[i + 2])] = i;
                dictionary[new Int2(tris[i + 2], tris[i])] = i + 1;
            }
            for (int j = 0; j < tCount; j += 3)
            {
                for (int k = 0; k < 3; k++)
                {
                    int num6;
                    if (dictionary.TryGetValue(new Int2(tris[j + ((k + 1) % 3)], tris[j + (k % 3)]), out num6))
                    {
                        Int3 a = verts[tris[j + ((k + 2) % 3)]];
                        Int3 b = verts[tris[j + ((k + 1) % 3)]];
                        Int3 num9 = verts[tris[j + ((k + 3) % 3)]];
                        Int3 p = verts[tris[num6]];
                        a.y = 0;
                        b.y = 0;
                        num9.y = 0;
                        p.y = 0;
                        bool flag = false;
                        if (!VectorMath.RightOrColinearXZ(a, num9, p) || VectorMath.RightXZ(a, b, p))
                        {
                            if (!colinear)
                            {
                                continue;
                            }
                            flag = true;
                        }
                        if ((colinear && (VectorMath.SqrDistancePointSegmentApproximate(a, p, b) < 9f)) && (!dictionary.ContainsKey(new Int2(tris[j + ((k + 2) % 3)], tris[j + ((k + 1) % 3)])) && !dictionary.ContainsKey(new Int2(tris[j + ((k + 1) % 3)], tris[num6]))))
                        {
                            tCount -= 3;
                            int index = (num6 / 3) * 3;
                            tris[j + ((k + 1) % 3)] = tris[num6];
                            if (index != tCount)
                            {
                                tris[index] = tris[tCount];
                                tris[index + 1] = tris[tCount + 1];
                                tris[index + 2] = tris[tCount + 2];
                                dictionary[new Int2(tris[index], tris[index + 1])] = index + 2;
                                dictionary[new Int2(tris[index + 1], tris[index + 2])] = index;
                                dictionary[new Int2(tris[index + 2], tris[index])] = index + 1;
                                tris[tCount] = 0;
                                tris[tCount + 1] = 0;
                                tris[tCount + 2] = 0;
                            }
                            else
                            {
                                tCount += 3;
                            }
                            dictionary[new Int2(tris[j], tris[j + 1])] = j + 2;
                            dictionary[new Int2(tris[j + 1], tris[j + 2])] = j;
                            dictionary[new Int2(tris[j + 2], tris[j])] = j + 1;
                        }
                        else if (delaunay && !flag)
                        {
                            float num12 = Int3.Angle(b - a, num9 - a);
                            if (Int3.Angle(b - p, num9 - p) > (6.283185f - (2f * num12)))
                            {
                                tris[j + ((k + 1) % 3)] = tris[num6];
                                int num14 = (num6 / 3) * 3;
                                int num15 = num6 - num14;
                                tris[num14 + (((num15 - 1) + 3) % 3)] = tris[j + ((k + 2) % 3)];
                                dictionary[new Int2(tris[j], tris[j + 1])] = j + 2;
                                dictionary[new Int2(tris[j + 1], tris[j + 2])] = j;
                                dictionary[new Int2(tris[j + 2], tris[j])] = j + 1;
                                dictionary[new Int2(tris[num14], tris[num14 + 1])] = num14 + 2;
                                dictionary[new Int2(tris[num14 + 1], tris[num14 + 2])] = num14;
                                dictionary[new Int2(tris[num14 + 2], tris[num14])] = num14 + 1;
                            }
                        }
                    }
                }
            }
        }

        public void EndBatchLoad()
        {
            if (!this.isBatching)
            {
                throw new Exception("Ending batching when batching has not been started");
            }
            for (int i = 0; i < this.reloadedInBatch.Length; i++)
            {
                this.reloadedInBatch[i] = false;
            }
            this.isBatching = false;
            AstarPath.active.AddWorkItem(new AstarWorkItem(delegate (bool force) {
                this.graph.EndBatchTileUpdate();
                return true;
            }));
        }

        public int GetActiveRotation(Int2 p)
        {
            return this.activeTileRotations[p.x + (p.y * this._graph.tileXCount)];
        }

        public TileType GetTileType(int index)
        {
            return this.tileTypes[index];
        }

        public int GetTileTypeCount()
        {
            return this.tileTypes.Count;
        }

        private Int3 IntPoint2Int3(IntPoint p)
        {
            return new Int3((int) p.X, 0, (int) p.Y);
        }

        public unsafe void LoadTile(TileType tile, int x, int z, int rotation, int yoffset)
        {
            <LoadTile>c__AnonStorey25C storeyc = new <LoadTile>c__AnonStorey25C();
            storeyc.yoffset = yoffset;
            storeyc.rotation = rotation;
            storeyc.tile = tile;
            storeyc.x = x;
            storeyc.z = z;
            storeyc.<>f__this = this;
            if (storeyc.tile == null)
            {
                throw new ArgumentNullException("tile");
            }
            if (AstarPath.active != null)
            {
                storeyc.index = storeyc.x + (storeyc.z * this.graph.tileXCount);
                storeyc.rotation = storeyc.rotation % 4;
                if (((!this.isBatching || !this.reloadedInBatch[storeyc.index]) || ((this.activeTileOffsets[storeyc.index] != storeyc.yoffset) || (this.activeTileRotations[storeyc.index] != storeyc.rotation))) || (this.activeTileTypes[storeyc.index] != storeyc.tile))
                {
                    *((sbyte*) &(this.reloadedInBatch[storeyc.index])) |= this.isBatching;
                    this.activeTileOffsets[storeyc.index] = storeyc.yoffset;
                    this.activeTileRotations[storeyc.index] = storeyc.rotation;
                    this.activeTileTypes[storeyc.index] = storeyc.tile;
                    AstarPath.active.AddWorkItem(new AstarWorkItem(new Func<IWorkItemContext, bool, bool>(storeyc.<>m__35)));
                }
            }
        }

        private Vector3 Point2D2V3(TriangulationPoint p)
        {
            return (Vector3) (new Vector3((float) p.X, 0f, (float) p.Y) * 0.001f);
        }

        public TileType RegisterTileType(Mesh source, Int3 centerOffset, [Optional, DefaultParameterValue(1)] int width, [Optional, DefaultParameterValue(1)] int depth)
        {
            TileType item = new TileType(source, (Int3) (new Int3(this.graph.tileSizeX, 1, this.graph.tileSizeZ) * (1000f * this.graph.cellSize)), centerOffset, width, depth);
            this.tileTypes.Add(item);
            return item;
        }

        public void ReloadInBounds(Bounds b)
        {
            Int2 tileCoordinates = this.graph.GetTileCoordinates(b.min);
            Int2 num2 = this.graph.GetTileCoordinates(b.max);
            Pathfinding.IntRect a = new Pathfinding.IntRect(tileCoordinates.x, tileCoordinates.y, num2.x, num2.y);
            a = Pathfinding.IntRect.Intersection(a, new Pathfinding.IntRect(0, 0, this.graph.tileXCount - 1, this.graph.tileZCount - 1));
            if (a.IsValid())
            {
                for (int i = a.ymin; i <= a.ymax; i++)
                {
                    for (int j = a.xmin; j <= a.xmax; j++)
                    {
                        this.ReloadTile(j, i);
                    }
                }
            }
        }

        public void ReloadTile(int x, int z)
        {
            if (((x >= 0) && (z >= 0)) && ((x < this.graph.tileXCount) && (z < this.graph.tileZCount)))
            {
                int index = x + (z * this.graph.tileXCount);
                if (this.activeTileTypes[index] != null)
                {
                    this.LoadTile(this.activeTileTypes[index], x, z, this.activeTileRotations[index], this.activeTileOffsets[index]);
                }
            }
        }

        protected static T[] ShrinkArray<T>(T[] arr, int newLength)
        {
            newLength = Math.Min(newLength, arr.Length);
            T[] localArray = new T[newLength];
            if ((newLength % 4) == 0)
            {
                for (int j = 0; j < newLength; j += 4)
                {
                    localArray[j] = arr[j];
                    localArray[j + 1] = arr[j + 1];
                    localArray[j + 2] = arr[j + 2];
                    localArray[j + 3] = arr[j + 3];
                }
                return localArray;
            }
            if ((newLength % 3) == 0)
            {
                for (int k = 0; k < newLength; k += 3)
                {
                    localArray[k] = arr[k];
                    localArray[k + 1] = arr[k + 1];
                    localArray[k + 2] = arr[k + 2];
                }
                return localArray;
            }
            if ((newLength % 2) == 0)
            {
                for (int m = 0; m < newLength; m += 2)
                {
                    localArray[m] = arr[m];
                    localArray[m + 1] = arr[m + 1];
                }
                return localArray;
            }
            for (int i = 0; i < newLength; i++)
            {
                localArray[i] = arr[i];
            }
            return localArray;
        }

        public bool StartBatchLoad()
        {
            if (this.isBatching)
            {
                return false;
            }
            this.isBatching = true;
            AstarPath.active.AddWorkItem(new AstarWorkItem(delegate (bool force) {
                this.graph.StartBatchTileUpdate();
                return true;
            }));
            return true;
        }

        public RecastGraph graph
        {
            get
            {
                return this._graph;
            }
        }

        public bool isValid
        {
            get
            {
                return (((this._graph != null) && (this.tileXCount == this._graph.tileXCount)) && (this.tileZCount == this._graph.tileZCount));
            }
        }

        [CompilerGenerated]
        private sealed class <ClearTile>c__AnonStorey25B
        {
            internal TileHandler <>f__this;
            internal int x;
            internal int z;

            internal bool <>m__34(IWorkItemContext context, bool force)
            {
                this.<>f__this.graph.ReplaceTile(this.x, this.z, new Int3[0], new int[0], false);
                this.<>f__this.activeTileTypes[this.x + (this.z * this.<>f__this.graph.tileXCount)] = null;
                GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdate);
                context.QueueFloodFill();
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <LoadTile>c__AnonStorey25C
        {
            internal TileHandler <>f__this;
            internal int index;
            internal int rotation;
            internal TileHandler.TileType tile;
            internal int x;
            internal int yoffset;
            internal int z;

            internal bool <>m__35(IWorkItemContext context, bool force)
            {
                if (((this.<>f__this.activeTileOffsets[this.index] == this.yoffset) && (this.<>f__this.activeTileRotations[this.index] == this.rotation)) && (this.<>f__this.activeTileTypes[this.index] == this.tile))
                {
                    Int3[] numArray;
                    int[] numArray2;
                    int num2;
                    int num3;
                    GraphModifier.TriggerEvent(GraphModifier.EventType.PreUpdate);
                    this.tile.Load(out numArray, out numArray2, this.rotation, this.yoffset);
                    Bounds realBounds = this.<>f__this.graph.GetTileBounds(this.x, this.z, this.tile.Width, this.tile.Depth);
                    Int3 min = (Int3) realBounds.min;
                    min = -min;
                    Int3[] outVertsArr = null;
                    int[] outTrisArr = null;
                    this.<>f__this.CutPoly(numArray, numArray2, ref outVertsArr, ref outTrisArr, out num2, out num3, null, min, realBounds, TileHandler.CutMode.CutDual | TileHandler.CutMode.CutAll, 0);
                    this.<>f__this.DelaunayRefinement(outVertsArr, outTrisArr, ref num2, ref num3, true, false, -min);
                    if (num3 != outTrisArr.Length)
                    {
                        outTrisArr = TileHandler.ShrinkArray<int>(outTrisArr, num3);
                    }
                    if (num2 != outVertsArr.Length)
                    {
                        outVertsArr = TileHandler.ShrinkArray<Int3>(outVertsArr, num2);
                    }
                    int w = ((this.rotation % 2) != 0) ? this.tile.Depth : this.tile.Width;
                    int d = ((this.rotation % 2) != 0) ? this.tile.Width : this.tile.Depth;
                    this.<>f__this.graph.ReplaceTile(this.x, this.z, w, d, outVertsArr, outTrisArr, false);
                    GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdate);
                    context.QueueFloodFill();
                }
                return true;
            }
        }

        [Flags]
        public enum CutMode
        {
            CutAll = 1,
            CutDual = 2,
            CutExtra = 4
        }

        public class TileType
        {
            private int depth;
            private int lastRotation;
            private int lastYOffset;
            private Int3 offset;
            private static readonly int[] Rotations = new int[] { 1, 0, 0, 1, 0, 1, -1, 0, -1, 0, 0, -1, 0, -1, 1, 0 };
            private int[] tris;
            private Int3[] verts;
            private int width;

            public TileType(Mesh source, Int3 tileSize, Int3 centerOffset, [Optional, DefaultParameterValue(1)] int width, [Optional, DefaultParameterValue(1)] int depth)
            {
                if (source == null)
                {
                    throw new ArgumentNullException("source");
                }
                Vector3[] vertices = source.vertices;
                this.tris = source.triangles;
                this.verts = new Int3[vertices.Length];
                for (int i = 0; i < vertices.Length; i++)
                {
                    this.verts[i] = ((Int3) vertices[i]) + centerOffset;
                }
                this.offset = (Int3) (tileSize / 2f);
                this.offset.x *= width;
                this.offset.z *= depth;
                this.offset.y = 0;
                for (int j = 0; j < vertices.Length; j++)
                {
                    this.verts[j] += this.offset;
                }
                this.lastRotation = 0;
                this.lastYOffset = 0;
                this.width = width;
                this.depth = depth;
            }

            public TileType(Int3[] sourceVerts, int[] sourceTris, Int3 tileSize, Int3 centerOffset, [Optional, DefaultParameterValue(1)] int width, [Optional, DefaultParameterValue(1)] int depth)
            {
                if (sourceVerts == null)
                {
                    throw new ArgumentNullException("sourceVerts");
                }
                if (sourceTris == null)
                {
                    throw new ArgumentNullException("sourceTris");
                }
                this.tris = new int[sourceTris.Length];
                for (int i = 0; i < this.tris.Length; i++)
                {
                    this.tris[i] = sourceTris[i];
                }
                this.verts = new Int3[sourceVerts.Length];
                for (int j = 0; j < sourceVerts.Length; j++)
                {
                    this.verts[j] = sourceVerts[j] + centerOffset;
                }
                this.offset = (Int3) (tileSize / 2f);
                this.offset.x *= width;
                this.offset.z *= depth;
                this.offset.y = 0;
                for (int k = 0; k < sourceVerts.Length; k++)
                {
                    this.verts[k] += this.offset;
                }
                this.lastRotation = 0;
                this.lastYOffset = 0;
                this.width = width;
                this.depth = depth;
            }

            public void Load(out Int3[] verts, out int[] tris, int rotation, int yoffset)
            {
                rotation = ((rotation % 4) + 4) % 4;
                int num = rotation;
                rotation = ((rotation - (this.lastRotation % 4)) + 4) % 4;
                this.lastRotation = num;
                verts = this.verts;
                int num2 = yoffset - this.lastYOffset;
                this.lastYOffset = yoffset;
                if ((rotation != 0) || (num2 != 0))
                {
                    for (int i = 0; i < verts.Length; i++)
                    {
                        Int3 num4 = verts[i] - this.offset;
                        Int3 num5 = num4;
                        num5.y += num2;
                        num5.x = (num4.x * Rotations[rotation * 4]) + (num4.z * Rotations[(rotation * 4) + 1]);
                        num5.z = (num4.x * Rotations[(rotation * 4) + 2]) + (num4.z * Rotations[(rotation * 4) + 3]);
                        verts[i] = num5 + this.offset;
                    }
                }
                tris = this.tris;
            }

            public int Depth
            {
                get
                {
                    return this.depth;
                }
            }

            public int Width
            {
                get
                {
                    return this.width;
                }
            }
        }
    }
}

