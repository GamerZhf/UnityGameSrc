using OrbCreationExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class LODMaker
{
    private static float AngleCornerDiff(float angle)
    {
        angle = Mathf.Abs(FloatExtensions.To180Angle(angle));
        float num = Mathf.Min(angle, Mathf.Min(Mathf.Abs(FloatExtensions.To180Angle(180f - angle)), Mathf.Abs(FloatExtensions.To180Angle(90f - angle))));
        return ((num * num) * 10f);
    }

    private static float AngleDiff(float angle)
    {
        angle = Mathf.Abs(FloatExtensions.To180Angle(angle));
        float num = Mathf.Min(angle, Mathf.Abs(FloatExtensions.To180Angle(180f - angle)));
        return (num * num);
    }

    private static bool AnyWeightOK(float[] weights, float aMaxWeight)
    {
        for (int i = 0; i < 6; i++)
        {
            if (weights[i] < aMaxWeight)
            {
                return true;
            }
        }
        return false;
    }

    private static float Area(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        Vector3 vector = p1 - p0;
        Vector3 vector2 = p2 - p0;
        return ((vector.magnitude * (Mathf.Sin(Vector3.Angle(p1 - p0, p2 - p0) * 0.01745329f) * vector2.magnitude)) * 0.5f);
    }

    public static Mesh CreateNewMesh(Vector3[] vs, Vector3[] ns, Vector2[] uv1s, Vector2[] uv2s, Vector2[] uv3s, Vector2[] uv4s, Color32[] colors32, int[] ts, BoneWeight[] bws, Matrix4x4[] bindposes, int[] subMeshOffsets, bool recalcNormals)
    {
        Mesh mesh = new Mesh();
        FillMesh(mesh, vs, ns, uv1s, uv2s, uv3s, uv4s, colors32, ts, bws, bindposes, subMeshOffsets, recalcNormals);
        return mesh;
    }

    public static void FillMesh(Mesh mesh, Vector3[] vs, Vector3[] ns, Vector2[] uv1s, Vector2[] uv2s, Vector2[] uv3s, Vector2[] uv4s, Color32[] colors32, int[] ts, BoneWeight[] bws, Matrix4x4[] bindposes, int[] subMeshOffsets, bool recalcNormals)
    {
        mesh.vertices = vs;
        if ((ns != null) && (ns.Length > 0))
        {
            mesh.normals = ns;
        }
        if ((uv1s != null) && (uv1s.Length > 0))
        {
            mesh.uv = uv1s;
        }
        if ((uv2s != null) && (uv2s.Length > 0))
        {
            mesh.uv2 = uv2s;
        }
        if ((uv3s != null) && (uv2s.Length > 0))
        {
            mesh.uv3 = uv3s;
        }
        if ((uv4s != null) && (uv2s.Length > 0))
        {
            mesh.uv4 = uv4s;
        }
        if ((colors32 != null) && (colors32.Length > 0))
        {
            mesh.colors32 = colors32;
        }
        if ((bws != null) && (bws.Length > 0))
        {
            mesh.boneWeights = bws;
        }
        if ((bindposes != null) && (bindposes.Length > 0))
        {
            mesh.bindposes = bindposes;
        }
        if (subMeshOffsets.Length == 1)
        {
            mesh.triangles = ts;
        }
        else
        {
            mesh.subMeshCount = subMeshOffsets.Length;
            for (int i = 0; i < subMeshOffsets.Length; i++)
            {
                subMeshOffsets[i] = Mathf.Max(0, subMeshOffsets[i]);
                int num2 = ((i + 1) >= subMeshOffsets.Length) ? ts.Length : subMeshOffsets[i + 1];
                if ((num2 - subMeshOffsets[i]) > 0)
                {
                    int[] destinationArray = new int[num2 - subMeshOffsets[i]];
                    Array.Copy(ts, subMeshOffsets[i], destinationArray, 0, num2 - subMeshOffsets[i]);
                    mesh.SetTriangles(destinationArray, i);
                }
                else
                {
                    mesh.SetTriangles(null, i);
                }
            }
        }
        if ((recalcNormals || (mesh.normals == null)) || (mesh.normals.Length <= 0))
        {
            mesh.RecalculateNormals();
        }
        MeshExtensions.RecalculateTangents(mesh);
    }

    private static void FillNewMeshArray(Vector3[] vs, bool[] vdel, int[] movedVs, Vector3[] ns, Vector2[] uv1s, int[] movedUv1s, Vector2[] uv2s, int[] movedUv2s, Vector2[] uv3s, int[] movedUv3s, Vector2[] uv4s, int[] movedUv4s, Color32[] colors32, int[] movedColors, BoneWeight[] bws, List<Vector3> newVs, List<Vector3> newNs, List<Vector2> newUv1s, List<Vector2> newUv2s, List<Vector2> newUv3s, List<Vector2> newUv4s, List<Color32> newColors32, List<BoneWeight> newBws, int[] o2n)
    {
        bool flag = (ns != null) && (ns.Length > 0);
        bool flag2 = false;
        for (int i = 0; (uv1s != null) && (i < uv1s.Length); i++)
        {
            if ((uv1s[i].x != 0f) || (uv1s[i].y != 0f))
            {
                flag2 = true;
                break;
            }
        }
        bool flag3 = false;
        for (int j = 0; (uv2s != null) && (j < uv2s.Length); j++)
        {
            if ((uv2s[j].x != 0f) || (uv2s[j].y != 0f))
            {
                flag3 = true;
                break;
            }
        }
        bool flag4 = false;
        for (int k = 0; (uv3s != null) && (k < uv3s.Length); k++)
        {
            if ((uv3s[k].x != 0f) || (uv3s[k].y != 0f))
            {
                flag4 = true;
                break;
            }
        }
        bool flag5 = false;
        for (int m = 0; (uv4s != null) && (m < uv4s.Length); m++)
        {
            if ((uv4s[m].x != 0f) || (uv4s[m].y != 0f))
            {
                flag5 = true;
                break;
            }
        }
        bool flag6 = false;
        for (int n = 0; (colors32 != null) && (n < colors32.Length); n++)
        {
            if (((colors32[n].r > 0) || (colors32[n].g > 0)) || (colors32[n].b > 0))
            {
                flag6 = true;
                break;
            }
        }
        bool flag7 = (bws != null) && (bws.Length > 0);
        int num6 = 0;
        for (int num7 = 0; num7 < vs.Length; num7++)
        {
            if (!vdel[num7])
            {
                newVs.Add(vs[movedVs[num7]]);
                if (flag)
                {
                    newNs.Add(ns[num7]);
                }
                if (flag2)
                {
                    newUv1s.Add(uv1s[movedUv1s[num7]]);
                }
                if (flag3)
                {
                    newUv2s.Add(uv2s[movedUv2s[num7]]);
                }
                if (flag4)
                {
                    newUv3s.Add(uv3s[movedUv3s[num7]]);
                }
                if (flag5)
                {
                    newUv4s.Add(uv4s[movedUv4s[num7]]);
                }
                if (flag6)
                {
                    newColors32.Add(colors32[movedColors[num7]]);
                }
                if (flag7)
                {
                    newBws.Add(bws[num7]);
                }
                o2n[num7] = num6;
                num6++;
            }
            else
            {
                o2n[num7] = -1;
            }
        }
    }

    private static void FillNewMeshTriangles(int[] oldTriangles, int[] o2n, List<int> newTriangles, int[] subMeshOffsets, int[] triangleGroups, List<int> newTGrps)
    {
        int index = -1;
        for (int i = 0; i < oldTriangles.Length; i += 3)
        {
            int num3 = oldTriangles[i];
            int num4 = oldTriangles[i + 1];
            int num5 = oldTriangles[i + 2];
            while (((index + 1) < subMeshOffsets.Length) && (i == subMeshOffsets[index + 1]))
            {
                index++;
                subMeshOffsets[index] = newTriangles.Count;
            }
            if ((((o2n[num3] >= 0) && (o2n[num4] >= 0)) && ((o2n[num5] >= 0) && (o2n[num3] != o2n[num4]))) && ((o2n[num4] != o2n[num5]) && (o2n[num5] != o2n[num3])))
            {
                newTriangles.Add(o2n[num3]);
                newTriangles.Add(o2n[num4]);
                newTriangles.Add(o2n[num5]);
                newTGrps.Add(triangleGroups[i / 3]);
            }
        }
        while ((index + 1) < subMeshOffsets.Length)
        {
            index++;
            subMeshOffsets[index] = newTriangles.Count;
        }
    }

    public static float FindCollision(Vector3 fromPos, Vector3 direction, Vector3 pointOnPlane, Vector3 normalPlane)
    {
        float positiveInfinity = float.PositiveInfinity;
        if (OrbCreationExtensions.Vector3Extensions.InProduct(direction, normalPlane) != 0f)
        {
            positiveInfinity = OrbCreationExtensions.Vector3Extensions.InProduct(pointOnPlane - fromPos, normalPlane) / OrbCreationExtensions.Vector3Extensions.InProduct(direction, normalPlane);
        }
        return positiveInfinity;
    }

    private static int[] GetAdjacentTriangles(int[] ts, int tIdx, List<List<int>> trianglesPerVertex, int[] uniqueVs, int[] triangleGroups, List<List<int>> trianglesPerGroup)
    {
        int index = ts[tIdx];
        int num2 = ts[tIdx + 1];
        int num3 = ts[tIdx + 2];
        List<int> list = new List<int>();
        List<int> list2 = new List<int>();
        List<int> list3 = trianglesPerVertex[uniqueVs[index]];
        for (int i = 0; i < list3.Count; i++)
        {
            list2.Add(ts[list3[i] * 3]);
            list2.Add(ts[(list3[i] * 3) + 1]);
            list2.Add(ts[(list3[i] * 3) + 2]);
            list.Add(list3[i]);
            SetTriangleGroup(tIdx / 3, list3[i], triangleGroups, trianglesPerGroup);
        }
        list3 = trianglesPerVertex[uniqueVs[num2]];
        for (int j = 0; j < list3.Count; j++)
        {
            int num6 = 0;
            while (num6 < list.Count)
            {
                if (list[num6] == list3[j])
                {
                    break;
                }
                num6++;
            }
            if (num6 >= list.Count)
            {
                list2.Add(ts[list3[j] * 3]);
                list2.Add(ts[(list3[j] * 3) + 1]);
                list2.Add(ts[(list3[j] * 3) + 2]);
                list.Add(list3[j]);
                SetTriangleGroup(tIdx / 3, list3[j], triangleGroups, trianglesPerGroup);
            }
        }
        list3 = trianglesPerVertex[uniqueVs[num3]];
        for (int k = 0; k < list3.Count; k++)
        {
            int num8 = 0;
            while (num8 < list.Count)
            {
                if (list[num8] == list3[k])
                {
                    break;
                }
                num8++;
            }
            if (num8 >= list.Count)
            {
                list2.Add(ts[list3[k] * 3]);
                list2.Add(ts[(list3[k] * 3) + 1]);
                list2.Add(ts[(list3[k] * 3) + 2]);
                list.Add(list3[k]);
                SetTriangleGroup(tIdx / 3, list3[k], triangleGroups, trianglesPerGroup);
            }
        }
        return list2.ToArray();
    }

    private static int GetLastVertexWithYSmaller(float y, List<int> orderedVertices, Vector3[] vs, int limitSearchRange)
    {
        int num = Mathf.Min(orderedVertices.Count, limitSearchRange) / 2;
        int num2 = num;
        int num3 = 1;
        while (((num >= 0) && (num < limitSearchRange)) && (num < orderedVertices.Count))
        {
            int index = orderedVertices[num];
            Vector3 vector = vs[index];
            int num5 = num2;
            int num6 = num3;
            num2 = Mathf.Max(num5 / 2, 1);
            if (vector.y < y)
            {
                if (((num6 == -1) && (num5 == 1)) || (num5 == 0))
                {
                    num++;
                    while ((num < limitSearchRange) && (num < orderedVertices.Count))
                    {
                        index = orderedVertices[num];
                        if (vs[index].y >= y)
                        {
                            break;
                        }
                        num++;
                    }
                    return --num;
                }
                num3 = 1;
            }
            else if (vector.y > y)
            {
                num3 = -1;
            }
            else
            {
                num--;
                while (num >= 0)
                {
                    index = orderedVertices[num];
                    if (vs[index].y < y)
                    {
                        return num;
                    }
                    num--;
                }
                return num;
            }
            num += num3 * num2;
        }
        return -1;
    }

    private static float GetNormalDiffForCorners(Vector3[] ns, int corner1, int corner2)
    {
        if ((ns != null) && (ns.Length > 0))
        {
            return (Vector3.Angle(ns[corner1], ns[corner2]) / 180f);
        }
        return 0f;
    }

    private static void GetTotalAngleAndCenterDistanceForCorner(int[] ts, Vector3[] vs, int[] movedVs, int vertexIdx, Vector3[] centerDistances, ref float totalAngle, ref Vector3 totalCenterDist)
    {
        totalAngle = 0f;
        totalCenterDist = Vector3.zero;
        int num = 0;
        for (int i = 0; i < ts.Length; i++)
        {
            if (ts[i] == vertexIdx)
            {
                int index = (i / 3) * 3;
                int num4 = ts[index];
                int num5 = ts[index + 1];
                int num6 = ts[index + 2];
                i = index + 2;
                if (((num4 != num5) && (num5 != num6)) && (num6 != num4))
                {
                    int num7 = vertexIdx;
                    int num8 = vertexIdx;
                    if (num4 != vertexIdx)
                    {
                        num7 = num4;
                    }
                    if (num5 != vertexIdx)
                    {
                        if (num7 == vertexIdx)
                        {
                            num7 = num5;
                        }
                        else
                        {
                            num8 = ts[index + 1];
                        }
                    }
                    if (num6 != vertexIdx)
                    {
                        num8 = num6;
                    }
                    totalAngle += Vector3.Angle(vs[movedVs[num7]] - vs[movedVs[vertexIdx]], vs[movedVs[num8]] - vs[movedVs[vertexIdx]]);
                }
                totalCenterDist += (centerDistances[num4] + centerDistances[num5]) + centerDistances[num6];
                num += 3;
            }
        }
        totalCenterDist = (Vector3) (totalCenterDist / ((float) num));
    }

    private static void GetTotalAngleAndCenterDistanceForNewCorner(int[] ts, Vector3[] vs, int[] movedVs, int[] uniqueVs, int vertexIdx, int newIdx, Vector3[] centerDistances, float maxWeight, ref float totalAngle, ref Vector3 totalCenterDist, ref bool flipsTriangles)
    {
        float num = 0.5f + Mathf.Clamp01(maxWeight * 0.75f);
        totalAngle = 0f;
        totalCenterDist = Vector3.zero;
        int num2 = 0;
        flipsTriangles = false;
        for (int i = 0; i < ts.Length; i++)
        {
            if ((ts[i] == vertexIdx) || (ts[i] == newIdx))
            {
                int index = (i / 3) * 3;
                int num5 = ts[index];
                int num6 = ts[index + 1];
                int num7 = ts[index + 2];
                i = index + 2;
                if (((num5 != num6) && (num6 != num7)) && (num7 != num5))
                {
                    int num8 = vertexIdx;
                    int num9 = vertexIdx;
                    if (num5 != vertexIdx)
                    {
                        num8 = num5;
                    }
                    if (num6 != vertexIdx)
                    {
                        if (num8 == vertexIdx)
                        {
                            num8 = num6;
                        }
                        else
                        {
                            num9 = ts[index + 1];
                        }
                    }
                    if (num7 != vertexIdx)
                    {
                        num9 = num7;
                    }
                    Vector3 normalized = Vector3.Cross(vs[movedVs[uniqueVs[num8]]] - vs[movedVs[uniqueVs[vertexIdx]]], vs[movedVs[uniqueVs[num9]]] - vs[movedVs[uniqueVs[vertexIdx]]]).normalized;
                    if (num5 == vertexIdx)
                    {
                        num5 = newIdx;
                    }
                    if (num6 == vertexIdx)
                    {
                        num6 = newIdx;
                    }
                    if (num7 == vertexIdx)
                    {
                        num7 = newIdx;
                    }
                    if (((num5 != num6) && (num6 != num7)) && (num7 != num5))
                    {
                        num8 = newIdx;
                        num9 = newIdx;
                        if (num5 != newIdx)
                        {
                            num8 = num5;
                        }
                        if (num6 != newIdx)
                        {
                            if (num8 == newIdx)
                            {
                                num8 = num6;
                            }
                            else
                            {
                                num9 = ts[index + 1];
                            }
                        }
                        if (num7 != newIdx)
                        {
                            num9 = num7;
                        }
                        Vector3 vector5 = Vector3.Cross(vs[movedVs[uniqueVs[num8]]] - vs[movedVs[uniqueVs[newIdx]]], vs[movedVs[uniqueVs[num9]]] - vs[movedVs[uniqueVs[newIdx]]]).normalized + normalized;
                        if (vector5.magnitude < num)
                        {
                            flipsTriangles = true;
                        }
                        totalAngle += Vector3.Angle(vs[movedVs[uniqueVs[num8]]] - vs[movedVs[uniqueVs[newIdx]]], vs[movedVs[uniqueVs[num9]]] - vs[movedVs[uniqueVs[newIdx]]]);
                    }
                }
                totalCenterDist = ((totalCenterDist + centerDistances[num5]) + centerDistances[num6]) + centerDistances[num7];
                num2 += 3;
            }
        }
        totalCenterDist = (Vector3) (totalCenterDist / ((float) num2));
    }

    private static unsafe void GetUVStretchAndAreaForCorner(int[] ts, Vector3[] vs, int[] movedVs, int[] uniqueVs, Vector2[] uvs, int cFrom, int cTo, ref float affectedUvAreaDiff, ref float affectedAreaDiff, ref float totalUvAreaDiff, ref float totalAreaDiff)
    {
        float num = 0f;
        int num2 = 0;
        float num3 = 0f;
        float num4 = 0f;
        float num5 = 0f;
        float num6 = 0f;
        float num7 = 0f;
        float num8 = 0f;
        float num9 = 0f;
        float num10 = 0f;
        int num11 = uniqueVs[cFrom];
        int num12 = uniqueVs[cTo];
        affectedUvAreaDiff = 0f;
        affectedAreaDiff = 0f;
        int index = 0;
        int num14 = 0;
        int num15 = 0;
        float num16 = 0f;
        float num17 = 0f;
        for (int i = 0; i < ts.Length; i++)
        {
            if ((i % 3) == 0)
            {
                index = ts[i];
                num14 = ts[i + 1];
                num15 = ts[i + 2];
                if (((index == num14) || (num14 == num15)) || (num15 == index))
                {
                    i += 2;
                    continue;
                }
                num16 = Area(vs[movedVs[index]], vs[movedVs[num14]], vs[movedVs[num15]]);
                num7 += num16;
                if ((uvs != null) && (uvs.Length > 0))
                {
                    num17 = Area(*((Vector3*) &(uvs[index])), *((Vector3*) &(uvs[num14])), *((Vector3*) &(uvs[num15])));
                }
                num5 += num17;
            }
            if (uniqueVs[ts[i]] == num11)
            {
                int num19 = 1;
                if (((uniqueVs[index] == num12) || (uniqueVs[num14] == num12)) || (uniqueVs[num15] == num12))
                {
                    num19++;
                }
                if (((uvs != null) && (uvs.Length > 0)) && ((num16 > 0f) && (num19 < 2)))
                {
                    num += num17 / num16;
                    num2++;
                }
                num9 += num16;
            }
        }
        if ((num2 > 0) && (num > 0f))
        {
            num3 = num / ((float) num2);
        }
        num = 0f;
        num2 = 0;
        for (int j = 0; j < ts.Length; j++)
        {
            if ((j % 3) == 0)
            {
                index = ts[j];
                num14 = ts[j + 1];
                num15 = ts[j + 2];
                if (uniqueVs[index] == num11)
                {
                    index = cTo;
                }
                if (uniqueVs[num14] == num11)
                {
                    num14 = cTo;
                }
                if (uniqueVs[num15] == num11)
                {
                    num15 = cTo;
                }
                if (((index == num14) || (num14 == num15)) || (num15 == index))
                {
                    j += 2;
                    continue;
                }
                num16 = Area(vs[movedVs[index]], vs[movedVs[num14]], vs[movedVs[num15]]);
                num8 += num16;
                if ((uvs != null) && (uvs.Length > 0))
                {
                    num17 = Area(*((Vector3*) &(uvs[index])), *((Vector3*) &(uvs[num14])), *((Vector3*) &(uvs[num15])));
                }
                num6 += num17;
            }
            if (uniqueVs[ts[j]] == num11)
            {
                int num21 = 1;
                if (((uniqueVs[index] == num12) || (uniqueVs[num14] == num12)) || (uniqueVs[num15] == num12))
                {
                    num21++;
                }
                if (((uvs != null) && (uvs.Length > 0)) && (num16 > 0f))
                {
                    num += num17 / num16;
                    num2++;
                }
                num10 += num16;
            }
        }
        affectedAreaDiff = Mathf.Abs((float) (num10 - num9));
        totalAreaDiff = num8 - num7;
        totalUvAreaDiff = num6 - num5;
        if ((num2 > 0) && (num > 0f))
        {
            num4 = num / ((float) num2);
        }
        float f = Mathf.Abs((float) (num4 - num3));
        if (f > 0f)
        {
            affectedUvAreaDiff = Mathf.Sqrt(f) * num9;
        }
    }

    private static int GetVertexEqualTo(Vector3 v, List<int> orderedVertices, Vector3[] vs)
    {
        int num = orderedVertices.Count / 2;
        int num2 = num;
        int num3 = 1;
        int num4 = 0;
        while ((num >= 0) && (num < orderedVertices.Count))
        {
            Vector3 vector = vs[orderedVertices[num]];
            int num5 = num2;
            int num6 = num3;
            num2 = Mathf.Max(num5 / 2, 1);
            if (vector.y < v.y)
            {
                num3 = 1;
            }
            else if (vector.y > v.y)
            {
                num3 = -1;
            }
            else if (vector.z < v.z)
            {
                num3 = 1;
            }
            else if (vector.z > v.z)
            {
                num3 = -1;
            }
            else if (vector.x < v.x)
            {
                num3 = 1;
            }
            else if (vector.x > v.x)
            {
                num3 = -1;
            }
            else
            {
                while ((num >= 0) && OrbCreationExtensions.Vector3Extensions.IsEqual(vs[orderedVertices[num]], v))
                {
                    num--;
                }
                num++;
                return num;
            }
            num += num3 * num2;
            if ((((num3 != num6) && (num2 == 1)) && ((num5 == 1) && !OrbCreationExtensions.Vector3Extensions.IsEqual(vs[orderedVertices[num]], v))) || (++num4 > orderedVertices.Count))
            {
                break;
            }
        }
        return -1;
    }

    private static List<int> GetVerticesEqualTo(Vector3 v, List<int> orderedVertices, Vector3[] vs)
    {
        List<int> list = new List<int>();
        for (int i = GetVertexEqualTo(v, orderedVertices, vs); ((i >= 0) && (i < orderedVertices.Count)) && OrbCreationExtensions.Vector3Extensions.IsEqual(vs[orderedVertices[i]], v); i++)
        {
            list.Add(orderedVertices[i]);
        }
        return list;
    }

    private static List<int> GetVerticesWithinBox(Vector3 from, Vector3 to, List<int> orderedVertices, Vector3[] vs)
    {
        List<int> list = new List<int>();
        int num = GetLastVertexWithYSmaller(from.y, orderedVertices, vs, orderedVertices.Count);
        if (num < 0)
        {
            num = 0;
        }
        while ((num < orderedVertices.Count) && (vs[orderedVertices[num]].y <= to.y))
        {
            if ((((vs[orderedVertices[num]].y >= from.y) && (vs[orderedVertices[num]].x >= from.x)) && ((vs[orderedVertices[num]].x <= to.x) && (vs[orderedVertices[num]].z >= from.z))) && (vs[orderedVertices[num]].z <= to.z))
            {
                list.Add(orderedVertices[num]);
            }
            num++;
        }
        return list;
    }

    private static void GetWeights(float aMaxWeight, float removeSmallParts, float protectNormals, float protectUvs, float smallTrianglesFirst, float protectSubMeshesAndSharpEdges, out float sideLengthWeight, out float oldAngleWeight, out float newAngleWeight, out float uvWeight, out float areaDiffWeight, out float normalWeight, out float vertexWeight, out float centerDistanceWeight)
    {
        float num = 0.12f / (0.5f + aMaxWeight);
        sideLengthWeight = (3f * num) * smallTrianglesFirst;
        oldAngleWeight = (0.1f * num) * smallTrianglesFirst;
        newAngleWeight = 0.4f * num;
        uvWeight = (800f * num) * protectUvs;
        areaDiffWeight = 10f * num;
        normalWeight = (50f * num) * protectNormals;
        vertexWeight = (200f * num) * protectSubMeshesAndSharpEdges;
        centerDistanceWeight = 5000f * num;
    }

    private static bool IsVertexObscured(Vector3[] vs, Vector3[] ns, int[] ts, bool[] vObscured, int[] uniqueVs, Vector3 vertexBoxSize, List<int> orderedVertices, List<List<int>> trianglesPerVertex, int[] subMeshIdxPerVertex, float maxObscureDist, bool hiddenByOtherSubmesh, Vector3 vertex, Vector3 normal, int i)
    {
        List<int> list = GetVerticesWithinBox(vertex - vertexBoxSize, vertex + vertexBoxSize, orderedVertices, vs);
        for (int j = 0; j < list.Count; j++)
        {
            if (list[j] != i)
            {
                List<int> list2 = trianglesPerVertex[list[j]];
                for (int k = 0; k < list2.Count; k++)
                {
                    int index = list2[k] * 3;
                    if (((((vObscured == null) || !vObscured[uniqueVs[ts[index]]]) || (!vObscured[uniqueVs[ts[index + 1]]] || !vObscured[uniqueVs[ts[index + 2]]])) && ((ts[index] != i) && (ts[index + 1] != i))) && ((ts[index + 2] != i) && (!hiddenByOtherSubmesh || (subMeshIdxPerVertex[ts[index]] != subMeshIdxPerVertex[i]))))
                    {
                        Vector3 vector4 = (ns[ts[index]] + ns[ts[index + 1]]) + ns[ts[index + 2]];
                        Vector3 normalized = vector4.normalized;
                        if (Vector3.Angle(normal, normalized) < 60f)
                        {
                            float num5 = FindCollision(vertex, normal, vs[ts[index]], normalized);
                            if ((num5 > 0f) && (num5 < maxObscureDist))
                            {
                                Vector3 p = vertex + ((Vector3) (normal * num5));
                                if (OrbCreationExtensions.Vector2Extensions.IsBarycentricInTriangle(OrbCreationExtensions.Vector3Extensions.Barycentric(p, vs[ts[index]], vs[ts[index + 1]], vs[ts[index + 2]])))
                                {
                                    if (vObscured != null)
                                    {
                                        vObscured[uniqueVs[i]] = true;
                                    }
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    private static void Log(string msg)
    {
        Debug.Log(msg + "\n" + DateTime.Now.ToString("yyy/MM/dd hh:mm:ss.fff"));
    }

    private static void LogArray(string msg, List<List<int>> ts)
    {
        object[] objArray1 = new object[] { msg, " (", ts.Count, "):\n" };
        string str = string.Concat(objArray1);
        for (int i = 0; i < ts.Count; i++)
        {
            List<int> list = ts[i];
            for (int j = 0; j < list.Count; j++)
            {
                str = str + list[j] + ",";
            }
            str = str + "\n";
        }
        Log(str);
    }

    private static void LogArray(string msg, List<int> ts)
    {
        object[] objArray1 = new object[] { msg, " (", ts.Count, "):\n" };
        string str = string.Concat(objArray1);
        for (int i = 0; i < ts.Count; i++)
        {
            str = str + ts[i] + "\n";
        }
        Log(str);
    }

    private static void LogArray(string msg, List<Color32> ts)
    {
        object[] objArray1 = new object[] { msg, " (", ts.Count, "):\n" };
        string str = string.Concat(objArray1);
        for (int i = 0; i < ts.Count; i++)
        {
            str = str + ts[i] + "\n";
        }
        Log(str);
    }

    private static void LogArray(string msg, bool[] ts)
    {
        object[] objArray1 = new object[] { msg, " (", ts.Length, "):\n" };
        string str = string.Concat(objArray1);
        for (int i = 0; i < ts.Length; i += 3)
        {
            str = str + ts[i] + "\n";
        }
        Log(str);
    }

    private static void LogArray(string msg, int[] ts)
    {
        object[] objArray1 = new object[] { msg, " (", ts.Length, "):\n" };
        string str = string.Concat(objArray1);
        for (int i = 0; i < ts.Length; i++)
        {
            str = str + ts[i] + "\n";
        }
        Log(str);
    }

    private static void LogArray(string msg, float[] fs)
    {
        object[] objArray1 = new object[] { msg, " (", fs.Length, "):\n" };
        string str = string.Concat(objArray1);
        for (int i = 0; i < fs.Length; i++)
        {
            str = str + fs[i] + "\n";
        }
        Log(str);
    }

    private static void LogArray(string msg, Color32[] ts)
    {
        object[] objArray1 = new object[] { msg, " (", ts.Length, "):\n" };
        string str = string.Concat(objArray1);
        for (int i = 0; i < ts.Length; i++)
        {
            str = str + ts[i] + "\n";
        }
        Log(str);
    }

    private static void LogArray(string msg, List<Vector3> vs, int decimals)
    {
        object[] objArray1 = new object[] { msg, " (", vs.Count, "):\n" };
        string str = string.Concat(objArray1);
        for (int i = 0; i < vs.Count; i++)
        {
            object[] objArray2 = new object[] { str, string.Empty, i, ": ", OrbCreationExtensions.Vector3Extensions.MakeString(vs[i], decimals), "\n" };
            str = string.Concat(objArray2);
        }
        Log(str);
    }

    private static void LogArray(string msg, Vector2[] vs, int decimals)
    {
        object[] objArray1 = new object[] { msg, " (", vs.Length, "):\n" };
        string str = string.Concat(objArray1);
        for (int i = 0; i < vs.Length; i++)
        {
            object[] objArray2 = new object[] { str, string.Empty, i, ": ", OrbCreationExtensions.Vector2Extensions.MakeString(vs[i], decimals), "\n" };
            str = string.Concat(objArray2);
        }
        Log(str);
    }

    private static void LogArray(string msg, Vector3[] vs, int decimals)
    {
        object[] objArray1 = new object[] { msg, " (", vs.Length, "):\n" };
        string str = string.Concat(objArray1);
        for (int i = 0; i < vs.Length; i++)
        {
            object[] objArray2 = new object[] { str, string.Empty, i, ": ", OrbCreationExtensions.Vector3Extensions.MakeString(vs[i], decimals), "\n" };
            str = string.Concat(objArray2);
        }
        Log(str);
    }

    private static void LogTriArray(string msg, List<int> ts)
    {
        object[] objArray1 = new object[] { msg, " (", ts.Count, "):\n" };
        string str = string.Concat(objArray1);
        for (int i = 0; i < ts.Count; i += 3)
        {
            object[] objArray2 = new object[] { str, ts[i], ", ", ts[i + 1], ", ", ts[i + 2], "\n" };
            str = string.Concat(objArray2);
        }
        Log(str);
    }

    private static void LogTriArray(string msg, int[] ts)
    {
        object[] objArray1 = new object[] { msg, " (", ts.Length, "):\n" };
        string str = string.Concat(objArray1);
        for (int i = 0; i < ts.Length; i += 3)
        {
            object[] objArray2 = new object[] { str, ts[i], ", ", ts[i + 1], ", ", ts[i + 2], "\n" };
            str = string.Concat(objArray2);
        }
        Log(str);
    }

    private static void LogVectors(string msg, int[] idxs, Vector3[] vs, int decimals)
    {
        object[] objArray1 = new object[] { msg, " (", idxs.Length, "):\n" };
        string str = string.Concat(objArray1);
        for (int i = 0; i < idxs.Length; i++)
        {
            object[] objArray2 = new object[] { str, string.Empty, i, ": ", OrbCreationExtensions.Vector3Extensions.MakeString(vs[idxs[i]], decimals), "\n" };
            str = string.Concat(objArray2);
        }
        Log(str);
    }

    private static string LogWeights(string msg, float[] w)
    {
        string str = msg + ": ";
        string[] textArray1 = new string[] { str, " weight 0-1: ", FloatExtensions.MakeString(w[0], 1), " / ", FloatExtensions.MakeString(w[1], 1) };
        str = string.Concat(textArray1);
        string[] textArray2 = new string[] { str, " | weight 1>2: ", FloatExtensions.MakeString(w[2], 1), " / ", FloatExtensions.MakeString(w[3], 1) };
        str = string.Concat(textArray2);
        string[] textArray3 = new string[] { str, " | weight 2>0: ", FloatExtensions.MakeString(w[4], 1), " / ", FloatExtensions.MakeString(w[5], 1) };
        return string.Concat(textArray3);
    }

    public static Mesh MakeLODMesh(Mesh orig, float aMaxWeight, [Optional, DefaultParameterValue(true)] bool recalcNormals, [Optional, DefaultParameterValue(1f)] float removeSmallParts, [Optional, DefaultParameterValue(false)] bool reuseOldMesh)
    {
        return MakeLODMesh(orig, aMaxWeight, removeSmallParts, 1f, 1f, 1f, 1f, recalcNormals, reuseOldMesh);
    }

    public static Mesh MakeLODMesh(Mesh orig, float aMaxWeight, float removeSmallParts, float protectNormals, float protectUvs, float smallTrianglesFirst, float protectSubMeshesAndSharpEdges, bool recalcNormals, [Optional, DefaultParameterValue(false)] bool reuseOldMesh)
    {
        float num;
        float num2;
        float num3;
        float num4;
        float num5;
        float num6;
        float num7;
        float num8;
        if (!orig.isReadable)
        {
            Debug.LogError("Sorry, mesh was not marked for read/write upon import");
            return orig;
        }
        GetWeights(aMaxWeight, removeSmallParts, protectNormals, protectUvs, smallTrianglesFirst, protectSubMeshesAndSharpEdges, out num, out num2, out num3, out num4, out num5, out num6, out num7, out num8);
        return MakeLODMesh(orig, aMaxWeight, removeSmallParts, num, num2, num3, num4, num5, num6, num7, num8, recalcNormals, reuseOldMesh);
    }

    private static Mesh MakeLODMesh(Mesh orig, float maxWeight, float removeSmallParts, float sideLengthWeight, float oldAngleWeight, float newAngleWeight, float uvWeight, float areaDiffWeight, float normalWeight, float vertexWeight, float centerDistanceWeight, bool recalcNormals, bool reuseOldMesh)
    {
        List<Vector3> list;
        List<Vector3> list2;
        List<Vector2> list3;
        List<Vector2> list4;
        List<Vector2> list5;
        List<Vector2> list6;
        List<Color32> list7;
        List<int> list8;
        List<BoneWeight> list9;
        string str = "started " + DateTime.Now.ToString("yyy/MM/dd hh:mm:ss.fff");
        Vector3[] vertices = orig.vertices;
        if (vertices.Length <= 0)
        {
            Log("Mesh was empty");
            return orig;
        }
        Vector3[] normals = orig.normals;
        if (normals.Length == 0)
        {
            orig.RecalculateNormals();
            normals = orig.normals;
        }
        Vector2[] uv = orig.uv;
        Vector2[] vectorArray4 = orig.uv2;
        Vector2[] vectorArray5 = orig.uv3;
        Vector2[] vectorArray6 = orig.uv4;
        Color32[] colorArray = orig.colors32;
        int[] triangles = orig.triangles;
        Matrix4x4[] bindposes = orig.bindposes;
        BoneWeight[] boneWeights = orig.boneWeights;
        int[] subMeshOffsets = new int[orig.subMeshCount];
        if (orig.subMeshCount > 1)
        {
            for (int i = 0; i < orig.subMeshCount; i++)
            {
                int[] numArray3 = orig.GetTriangles(i);
                int index = 0;
                while (index < numArray3.Length)
                {
                    triangles[subMeshOffsets[i] + index] = numArray3[index];
                    index++;
                }
                if ((i + 1) < orig.subMeshCount)
                {
                    subMeshOffsets[i + 1] = subMeshOffsets[i] + index;
                }
            }
        }
        Bounds meshBounds = orig.bounds;
        MakeLODMesh(vertices, normals, uv, vectorArray4, vectorArray5, vectorArray6, colorArray, triangles, ref bindposes, boneWeights, ref subMeshOffsets, meshBounds, maxWeight, removeSmallParts, sideLengthWeight, oldAngleWeight, newAngleWeight, uvWeight, areaDiffWeight, normalWeight, vertexWeight, centerDistanceWeight, out list, out list2, out list3, out list4, out list5, out list6, out list7, out list8, out list9);
        Debug.Log(string.Concat(new object[] { "compression:", maxWeight, ", vertices:", vertices.Length, " -> ", list.Count, ", triangles:", triangles.Length / 3, " -> ", list8.Count / 3, "\n", str, "\nended ", DateTime.Now.ToString("yyy/MM/dd hh:mm:ss.fff") }));
        if (reuseOldMesh)
        {
            orig.uv4 = null;
            orig.uv3 = null;
            orig.uv2 = null;
            orig.uv = null;
            orig.colors = null;
            orig.tangents = null;
            orig.boneWeights = null;
            orig.bindposes = null;
            orig.triangles = null;
            orig.subMeshCount = 0;
            orig.normals = null;
            FillMesh(orig, list.ToArray(), list2.ToArray(), list3.ToArray(), list4.ToArray(), list5.ToArray(), list6.ToArray(), list7.ToArray(), list8.ToArray(), list9.ToArray(), bindposes, subMeshOffsets, recalcNormals);
            return orig;
        }
        return CreateNewMesh(list.ToArray(), list2.ToArray(), list3.ToArray(), list4.ToArray(), list5.ToArray(), list6.ToArray(), list7.ToArray(), list8.ToArray(), list9.ToArray(), bindposes, subMeshOffsets, recalcNormals);
    }

    private static void MakeLODMesh(Vector3[] vs, Vector3[] ns, Vector2[] uv1s, Vector2[] uv2s, Vector2[] uv3s, Vector2[] uv4s, Color32[] colors32, int[] ts, ref Matrix4x4[] bindposes, BoneWeight[] bws, ref int[] subMeshOffsets, Bounds meshBounds, float maxWeight, float removeSmallParts, float sideLengthWeight, float oldAngleWeight, float newAngleWeight, float uvWeight, float areaDiffWeight, float normalWeight, float vertexWeight, float centerDistanceWeight, out List<Vector3> newVs, out List<Vector3> newNs, out List<Vector2> newUv1s, out List<Vector2> newUv2s, out List<Vector2> newUv3s, out List<Vector2> newUv4s, out List<Color32> newColors32, out List<int> newTs, out List<BoneWeight> newBws)
    {
        int num = 1;
        Vector3 size = meshBounds.size;
        Vector3 center = meshBounds.center;
        Vector3 zero = Vector3.zero;
        if (size.x > 0f)
        {
            zero.x = 1f / size.x;
        }
        if (size.y > 0f)
        {
            zero.y = 1f / size.y;
        }
        if (size.z > 0f)
        {
            zero.z = 1f / size.z;
        }
        List<List<int>> trianglesPerGroup = new List<List<int>>();
        int[] triangleGroups = new int[ts.Length / 3];
        List<List<int>> trianglesPerVertex = new List<List<int>>();
        int[] numArray2 = new int[vs.Length];
        List<int> orderedVertices = new List<int>();
        Vector3[] centerDistances = new Vector3[vs.Length];
        bool[] deletedVertices = new bool[vs.Length];
        bool[] hasTwinVS = new bool[vs.Length];
        int[] movedVs = new int[vs.Length];
        int[] uniqueVs = new int[vs.Length];
        int[] numArray5 = new int[uv1s.Length];
        int[] numArray6 = new int[uv2s.Length];
        int[] numArray7 = new int[uv3s.Length];
        int[] numArray8 = new int[uv4s.Length];
        int[] movedColors = new int[colors32.Length];
        float[] numArray10 = new float[vs.Length];
        for (int i = 0; i < vs.Length; i++)
        {
            deletedVertices[i] = false;
            movedVs[i] = i;
            uniqueVs[i] = i;
            trianglesPerVertex.Add(new List<int>());
            numArray2[i] = -1;
        }
        for (int j = 0; j < uv1s.Length; j++)
        {
            numArray5[j] = j;
        }
        for (int k = 0; k < uv2s.Length; k++)
        {
            numArray6[k] = k;
        }
        for (int m = 0; m < uv3s.Length; m++)
        {
            numArray7[m] = m;
        }
        for (int n = 0; n < uv4s.Length; n++)
        {
            numArray8[n] = n;
        }
        for (int num7 = 0; num7 < colors32.Length; num7++)
        {
            movedColors[num7] = num7;
        }
        for (int num8 = 0; num8 < triangleGroups.Length; num8++)
        {
            triangleGroups[num8] = -1;
        }
        float num9 = Mathf.Round(meshBounds.size.x * 10000f);
        float num10 = Mathf.Round(meshBounds.size.y * 10000f);
        float num11 = Mathf.Round(meshBounds.size.z * 10000f);
        if (num9 <= 0f)
        {
            num9 = 1f;
        }
        if (num10 <= 0f)
        {
            num10 = 1f;
        }
        if (num11 <= 0f)
        {
            num11 = 1f;
        }
        for (int num12 = 0; num12 < vs.Length; num12++)
        {
            vs[num12].x = Mathf.Round(vs[num12].x * num9) / num9;
            vs[num12].y = Mathf.Round(vs[num12].y * num10) / num10;
            vs[num12].z = Mathf.Round(vs[num12].z * num11) / num11;
        }
        for (int num13 = 0; num13 < vs.Length; num13++)
        {
            int index = GetLastVertexWithYSmaller(vs[num13].y, orderedVertices, vs, num13) + 1;
            while (index < orderedVertices.Count)
            {
                if (((orderedVertices[index] < 0) || (vs[orderedVertices[index]].y > vs[num13].y)) || ((vs[orderedVertices[index]].y == vs[num13].y) && ((vs[orderedVertices[index]].z > vs[num13].z) || ((vs[num13].z == vs[orderedVertices[index]].z) && (vs[orderedVertices[index]].x > vs[num13].x)))))
                {
                    break;
                }
                index++;
            }
            orderedVertices.Insert(index, num13);
        }
        int num15 = -1;
        for (int num16 = 0; num16 < ts.Length; num16++)
        {
            if (((num16 + 1) < subMeshOffsets.Length) && (num16 >= subMeshOffsets[num15 + 1]))
            {
                num15++;
            }
            int num17 = numArray2[ts[num16]];
            if (num17 < 0)
            {
                numArray2[ts[num16]] = num15;
            }
            else if (num17 != num15)
            {
                numArray10[ts[num16]] += vs.Length;
            }
        }
        for (int num18 = 0; num18 < vs.Length; num18++)
        {
            centerDistances[num18] = OrbCreationExtensions.Vector3Extensions.Product(vs[num18] - center, zero);
            List<int> list4 = GetVerticesEqualTo(vs[num18], orderedVertices, vs);
            if (list4.Count > 1)
            {
                hasTwinVS[num18] = true;
                for (int num19 = 0; num19 < list4.Count; num19++)
                {
                    if (num19 != num18)
                    {
                        if (uniqueVs[list4[num19]] == list4[num19])
                        {
                            uniqueVs[list4[num19]] = uniqueVs[num18];
                        }
                        if (((ns != null) && (ns.Length != 0)) && (((ns[list4[0]].x != ns[list4[num19]].x) || (ns[list4[0]].y != ns[list4[num19]].y)) || (ns[list4[0]].z != ns[list4[num19]].z)))
                        {
                            numArray10[num18] += normalWeight * 0.05f;
                            numArray10[list4[num19]] += normalWeight * 0.05f;
                        }
                        if (((uv1s != null) && (uv1s.Length != 0)) && ((uv1s[list4[0]].x != uv1s[list4[num19]].x) || (uv1s[list4[0]].y != uv1s[list4[num19]].y)))
                        {
                            numArray10[num18] += uvWeight * 0.02f;
                            numArray10[list4[num19]] += uvWeight * 0.02f;
                        }
                        if (((uv2s != null) && (uv2s.Length != 0)) && ((uv2s[list4[0]].x != uv2s[list4[num19]].x) || (uv2s[list4[0]].y != uv2s[list4[num19]].y)))
                        {
                            numArray10[num18] += uvWeight * 0.013f;
                            numArray10[list4[num19]] += uvWeight * 0.013f;
                        }
                    }
                }
            }
        }
        for (int num20 = 0; num20 < ts.Length; num20++)
        {
            trianglesPerVertex[uniqueVs[ts[num20]]].Add(num20 / 3);
        }
        float aMaxWeight = maxWeight * (0.8f + ((((float) vs.Length) / 65536f) * 0.2f));
        float num22 = 0f;
        int num23 = 0;
        for (int num24 = 0; num24 < ts.Length; num24 += 3)
        {
            int num25 = ts[num24];
            int num26 = ts[num24 + 1];
            int num27 = ts[num24 + 2];
            if (((num25 != num26) && (num26 != num27)) && (num27 != num25))
            {
                num22 += Area(OrbCreationExtensions.Vector3Extensions.Product(vs[movedVs[num25]], zero), OrbCreationExtensions.Vector3Extensions.Product(vs[movedVs[num26]], zero), OrbCreationExtensions.Vector3Extensions.Product(vs[movedVs[num27]], zero));
                num23++;
            }
        }
        if (num23 > 0)
        {
            num22 /= (float) num23;
        }
        int[] numArray11 = new int[] { 1, 0, 2, 1, 0, 2 };
        int[] numArray12 = new int[] { 0, 1, 1, 2, 2, 0 };
        for (int num28 = 0; num28 < 3; num28++)
        {
            float num29 = (num22 * num28) * 0.5f;
            float positiveInfinity = (num22 * (num28 + 1)) * 0.5f;
            if (num28 >= 2)
            {
                positiveInfinity = float.PositiveInfinity;
            }
            for (int num31 = 0; num31 < ts.Length; num31 += 3)
            {
                int num32 = ts[num31];
                int num33 = ts[num31 + 1];
                int num34 = ts[num31 + 2];
                if (((num32 != num33) && (num33 != num34)) && (num34 != num32))
                {
                    int[] numArray13 = new int[] { num32, num33, num33, num34, num34, num32 };
                    int[] numArray14 = new int[] { num33, num32, num34, num33, num32, num34 };
                    float[] weights = new float[6];
                    float[] numArray16 = new float[] { OrbCreationExtensions.Vector3Extensions.Product(vs[movedVs[num32]] - vs[movedVs[num33]], zero).magnitude, OrbCreationExtensions.Vector3Extensions.Product(vs[movedVs[num33]] - vs[movedVs[num34]], zero).magnitude, OrbCreationExtensions.Vector3Extensions.Product(vs[movedVs[num34]] - vs[movedVs[num32]], zero).magnitude };
                    float f = Area(OrbCreationExtensions.Vector3Extensions.Product(vs[movedVs[num32]], zero), OrbCreationExtensions.Vector3Extensions.Product(vs[movedVs[num33]], zero), OrbCreationExtensions.Vector3Extensions.Product(vs[movedVs[num34]], zero));
                    if ((f >= num29) && (f < positiveInfinity))
                    {
                        f = Mathf.Sqrt(f);
                        for (int num36 = 0; num36 < 6; num36++)
                        {
                            weights[num36] += f * sideLengthWeight;
                        }
                        for (int num37 = 0; num37 < 6; num37++)
                        {
                            weights[num37] += numArray16[num37 / 2] * sideLengthWeight;
                        }
                        for (int num38 = 0; num38 < 6; num38++)
                        {
                            weights[num38] += (numArray10[numArray13[num38]] * f) * vertexWeight;
                        }
                        for (int num39 = 0; num39 < 6; num39++)
                        {
                            if (((num39 / 2) * 2) == num39)
                            {
                                float num40 = (GetNormalDiffForCorners(ns, numArray13[(num39 / 2) * 2], numArray14[(num39 / 2) * 2]) * numArray16[num39 / 2]) * normalWeight;
                                weights[num39] += num40;
                                weights[num39 + 1] += num40;
                            }
                        }
                        int[] numArray17 = GetAdjacentTriangles(ts, num31, trianglesPerVertex, uniqueVs, triangleGroups, trianglesPerGroup);
                        if (AnyWeightOK(weights, aMaxWeight))
                        {
                            float[] numArray18 = new float[3];
                            Vector3[] vectorArray2 = new Vector3[3];
                            for (int num41 = 0; num41 < 3; num41++)
                            {
                                GetTotalAngleAndCenterDistanceForCorner(numArray17, vs, movedVs, numArray13[num41 * 2], centerDistances, ref numArray18[num41], ref vectorArray2[num41]);
                            }
                            for (int num42 = 0; num42 < 6; num42++)
                            {
                                if (weights[num42] < aMaxWeight)
                                {
                                    weights[num42] += ((AngleDiff(numArray18[numArray12[num42]]) * numArray16[num42 / 2]) * f) * oldAngleWeight;
                                }
                            }
                            if (AnyWeightOK(weights, aMaxWeight))
                            {
                                for (int num43 = 0; num43 < 6; num43++)
                                {
                                    if (weights[num43] < aMaxWeight)
                                    {
                                        float totalAngle = 0f;
                                        Vector3 totalCenterDist = Vector3.zero;
                                        bool flipsTriangles = false;
                                        GetTotalAngleAndCenterDistanceForNewCorner(numArray17, vs, movedVs, uniqueVs, numArray13[num43], numArray14[num43], centerDistances, maxWeight, ref totalAngle, ref totalCenterDist, ref flipsTriangles);
                                        if (flipsTriangles)
                                        {
                                            weights[num43] += 100f * f;
                                        }
                                        if (Mathf.Abs(totalAngle) < 10f)
                                        {
                                            weights[num43] += (AngleCornerDiff(totalAngle - numArray18[numArray11[num43]]) * Mathf.Sqrt(numArray16[num43 / 2])) * newAngleWeight;
                                        }
                                        else
                                        {
                                            weights[num43] += (AngleDiff(totalAngle - numArray18[numArray11[num43]]) * Mathf.Sqrt(numArray16[num43 / 2])) * newAngleWeight;
                                        }
                                        if ((ns != null) && (ns.Length > 0))
                                        {
                                            Vector3 vector12 = totalCenterDist - vectorArray2[numArray11[num43]];
                                            weights[num43] += (Vector3.Project(vs[movedVs[numArray13[num43]]] - vs[movedVs[numArray14[num43]]], ns[numArray13[num43]]).magnitude * vector12.magnitude) * centerDistanceWeight;
                                        }
                                    }
                                }
                                if (AnyWeightOK(weights, aMaxWeight))
                                {
                                    float num45 = Area(vs[movedVs[num32]], vs[movedVs[num33]], vs[movedVs[num34]]);
                                    for (int num46 = 0; num46 < 6; num46++)
                                    {
                                        if (weights[num46] < aMaxWeight)
                                        {
                                            float affectedAreaDiff = 0f;
                                            float affectedUvAreaDiff = 0f;
                                            float totalAreaDiff = 0f;
                                            float totalUvAreaDiff = 0f;
                                            GetUVStretchAndAreaForCorner(numArray17, vs, movedVs, uniqueVs, uv1s, numArray13[num46], numArray14[num46], ref affectedUvAreaDiff, ref affectedAreaDiff, ref totalUvAreaDiff, ref totalAreaDiff);
                                            weights[num46] += (affectedUvAreaDiff * 10f) * uvWeight;
                                            weights[num46] += ((Mathf.Pow(Mathf.Abs(totalUvAreaDiff) + 1f, 2f) - 1f) * 30f) * uvWeight;
                                            if (num45 <= 0f)
                                            {
                                                num45 = Mathf.Max(affectedAreaDiff, totalAreaDiff);
                                            }
                                            if (num45 > 0f)
                                            {
                                                if ((affectedAreaDiff / num45) > 1f)
                                                {
                                                    weights[num46] += ((affectedAreaDiff / num45) * 0.5f) * areaDiffWeight;
                                                }
                                                weights[num46] += ((Mathf.Pow(Mathf.Abs((float) (totalAreaDiff / num45)) + 1f, 2f) - 1f) * 0.5f) * areaDiffWeight;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (AnyWeightOK(weights, aMaxWeight))
                        {
                            for (int num51 = 0; num51 < 6; num51++)
                            {
                                weights[num51] *= 0.05f + (0.95f * numArray16[num51 / 2]);
                            }
                            int num52 = -1;
                            float num53 = float.PositiveInfinity;
                            for (int num54 = 0; num54 < 6; num54++)
                            {
                                if (weights[num54] < num53)
                                {
                                    num53 = weights[num54];
                                    num52 = num54;
                                }
                            }
                            switch (num52)
                            {
                                case 0:
                                    MergeVertices(ref num32, num33, hasTwinVS, vs, ts, uv1s, uv2s, uv3s, uv4s, colors32, deletedVertices, movedVs, uniqueVs, numArray5, numArray6, numArray7, numArray8, movedColors, trianglesPerVertex, num > 1);
                                    break;

                                case 1:
                                    MergeVertices(ref num33, num32, hasTwinVS, vs, ts, uv1s, uv2s, uv3s, uv4s, colors32, deletedVertices, movedVs, uniqueVs, numArray5, numArray6, numArray7, numArray8, movedColors, trianglesPerVertex, num > 1);
                                    break;

                                case 2:
                                    MergeVertices(ref num33, num34, hasTwinVS, vs, ts, uv1s, uv2s, uv3s, uv4s, colors32, deletedVertices, movedVs, uniqueVs, numArray5, numArray6, numArray7, numArray8, movedColors, trianglesPerVertex, num > 1);
                                    break;

                                case 3:
                                    MergeVertices(ref num34, num33, hasTwinVS, vs, ts, uv1s, uv2s, uv3s, uv4s, colors32, deletedVertices, movedVs, uniqueVs, numArray5, numArray6, numArray7, numArray8, movedColors, trianglesPerVertex, num > 1);
                                    break;

                                case 4:
                                    MergeVertices(ref num34, num32, hasTwinVS, vs, ts, uv1s, uv2s, uv3s, uv4s, colors32, deletedVertices, movedVs, uniqueVs, numArray5, numArray6, numArray7, numArray8, movedColors, trianglesPerVertex, num > 1);
                                    break;

                                case 5:
                                    MergeVertices(ref num32, num34, hasTwinVS, vs, ts, uv1s, uv2s, uv3s, uv4s, colors32, deletedVertices, movedVs, uniqueVs, numArray5, numArray6, numArray7, numArray8, movedColors, trianglesPerVertex, num > 1);
                                    break;
                            }
                        }
                    }
                }
            }
        }
        newVs = new List<Vector3>();
        newNs = new List<Vector3>();
        newUv1s = new List<Vector2>();
        newUv2s = new List<Vector2>();
        newUv3s = new List<Vector2>();
        newUv4s = new List<Vector2>();
        newColors32 = new List<Color32>();
        newTs = new List<int>();
        List<int> newTGrps = new List<int>();
        newBws = new List<BoneWeight>();
        int[] numArray19 = new int[vs.Length];
        FillNewMeshArray(vs, deletedVertices, movedVs, ns, uv1s, numArray5, uv2s, numArray6, uv3s, numArray7, uv4s, numArray8, colors32, movedColors, bws, newVs, newNs, newUv1s, newUv2s, newUv3s, newUv4s, newColors32, newBws, numArray19);
        FillNewMeshTriangles(ts, numArray19, newTs, subMeshOffsets, triangleGroups, newTGrps);
        RemoveEmptyTriangles(newVs, newNs, newUv1s, newUv2s, newUv3s, newUv4s, newColors32, newTs, newBws, subMeshOffsets, newTGrps);
        if (removeSmallParts > 0f)
        {
            RemoveMiniTriangleGroups(removeSmallParts, zero, maxWeight, newVs, newTs, subMeshOffsets, newTGrps);
        }
        RemoveUnusedVertices(newVs, newNs, newUv1s, newUv2s, newUv3s, newUv4s, newColors32, newBws, newTs);
    }

    public static void MakeLODMeshInBackground(object data)
    {
        float num7;
        float num8;
        float num9;
        float num10;
        float num11;
        float num12;
        float num13;
        float num14;
        List<Vector3> list;
        List<Vector3> list2;
        List<Vector2> list3;
        List<Vector2> list4;
        List<Vector2> list5;
        List<Vector2> list6;
        List<Color32> list7;
        List<int> list8;
        List<BoneWeight> list9;
        string str = "start " + DateTime.Now.ToString("yyy/MM/dd hh:mm:ss.fff");
        Hashtable hashtable = (Hashtable) data;
        float aMaxWeight = (float) hashtable["maxWeight"];
        Vector3[] vs = (Vector3[]) hashtable["vertices"];
        int[] ts = (int[]) hashtable["triangles"];
        int[] subMeshOffsets = (int[]) hashtable["subMeshOffsets"];
        Bounds meshBounds = (Bounds) hashtable["meshBounds"];
        float removeSmallParts = 1f;
        float protectNormals = 1f;
        float protectUvs = 1f;
        float protectSubMeshesAndSharpEdges = 1f;
        float smallTrianglesFirst = 1f;
        Vector3[] ns = null;
        Vector2[] vectorArray3 = null;
        Vector2[] vectorArray4 = null;
        Vector2[] vectorArray5 = null;
        Vector2[] vectorArray6 = null;
        Color32[] colorArray = null;
        Matrix4x4[] bindposes = null;
        BoneWeight[] bws = null;
        if (hashtable.ContainsKey("removeSmallParts"))
        {
            removeSmallParts = (float) hashtable["removeSmallParts"];
        }
        if (hashtable.ContainsKey("protectNormals"))
        {
            protectNormals = (float) hashtable["protectNormals"];
        }
        if (hashtable.ContainsKey("protectUvs"))
        {
            protectUvs = (float) hashtable["protectUvs"];
        }
        if (hashtable.ContainsKey("protectSubMeshesAndSharpEdges"))
        {
            protectSubMeshesAndSharpEdges = (float) hashtable["protectSubMeshesAndSharpEdges"];
        }
        if (hashtable.ContainsKey("smallTrianglesFirst"))
        {
            smallTrianglesFirst = (float) hashtable["smallTrianglesFirst"];
        }
        if (hashtable.ContainsKey("normals"))
        {
            ns = (Vector3[]) hashtable["normals"];
        }
        if (hashtable.ContainsKey("uv1s"))
        {
            vectorArray3 = (Vector2[]) hashtable["uv1s"];
        }
        if (hashtable.ContainsKey("uv2s"))
        {
            vectorArray4 = (Vector2[]) hashtable["uv2s"];
        }
        if (hashtable.ContainsKey("uv3s"))
        {
            vectorArray5 = (Vector2[]) hashtable["uv3s"];
        }
        if (hashtable.ContainsKey("uv4s"))
        {
            vectorArray6 = (Vector2[]) hashtable["uv4s"];
        }
        if (hashtable.ContainsKey("colors32"))
        {
            colorArray = (Color32[]) hashtable["colors32"];
        }
        if (hashtable.ContainsKey("bindposes"))
        {
            bindposes = (Matrix4x4[]) hashtable["bindposes"];
        }
        if (hashtable.ContainsKey("boneWeights"))
        {
            bws = (BoneWeight[]) hashtable["boneWeights"];
        }
        GetWeights(aMaxWeight, removeSmallParts, protectNormals, protectUvs, smallTrianglesFirst, protectSubMeshesAndSharpEdges, out num7, out num8, out num9, out num10, out num11, out num12, out num13, out num14);
        MakeLODMesh(vs, ns, vectorArray3, vectorArray4, vectorArray5, vectorArray6, colorArray, ts, ref bindposes, bws, ref subMeshOffsets, meshBounds, aMaxWeight, removeSmallParts, num7, num8, num9, num10, num11, num12, num13, num14, out list, out list2, out list3, out list4, out list5, out list6, out list7, out list8, out list9);
        ((Hashtable) data)["vertices"] = list.ToArray();
        ((Hashtable) data)["normals"] = list2.ToArray();
        ((Hashtable) data)["uv1s"] = list3.ToArray();
        ((Hashtable) data)["uv2s"] = list4.ToArray();
        ((Hashtable) data)["uv3s"] = list5.ToArray();
        ((Hashtable) data)["uv4s"] = list6.ToArray();
        ((Hashtable) data)["colors32"] = list7.ToArray();
        ((Hashtable) data)["triangles"] = list8.ToArray();
        ((Hashtable) data)["bindposes"] = bindposes;
        ((Hashtable) data)["boneWeights"] = list9.ToArray();
        ((Hashtable) data)["subMeshOffsets"] = subMeshOffsets;
        ((Hashtable) data)["ready"] = true;
        Debug.Log(string.Concat(new object[] { "compression:", aMaxWeight, ", vertices:", vs.Length, " -> ", list.Count, ", triangles:", ts.Length / 3, " -> ", list8.Count / 3, "\n", str, "\nended ", DateTime.Now.ToString("yyy/MM/dd hh:mm:ss.fff") }));
    }

    private static void MergeVertices(ref int oldV, int newV, bool[] hasTwinVS, Vector3[] vs, int[] triangles, Vector2[] uv1s, Vector2[] uv2s, Vector2[] uv3s, Vector2[] uv4s, Color32[] colors32, bool[] deletedVertices, int[] movedVs, int[] uniqueVs, int[] movedUv1s, int[] movedUv2s, int[] movedUv3s, int[] movedUv4s, int[] movedColors, List<List<int>> trianglesPerVertex, bool logYN)
    {
        if (oldV != newV)
        {
            deletedVertices[oldV] = true;
            int num = uniqueVs[oldV];
            int num2 = uniqueVs[newV];
            List<int> list = trianglesPerVertex[num];
            for (int i = 0; i < list.Count; i++)
            {
                int num4 = list[i] * 3;
                for (int j = 0; j < 3; j++)
                {
                    if (triangles[num4 + j] == oldV)
                    {
                        triangles[num4 + j] = newV;
                    }
                }
            }
            if (num != num2)
            {
                trianglesPerVertex[num2].AddRange(trianglesPerVertex[num]);
                trianglesPerVertex[num].Clear();
            }
            if (hasTwinVS[oldV] || hasTwinVS[movedVs[oldV]])
            {
                MoveVertex(oldV, newV, movedVs, uniqueVs, movedUv1s, movedUv2s, movedUv3s, movedUv4s, movedColors);
                for (int k = 0; k < vs.Length; k++)
                {
                    if (((k != oldV) && (vs[oldV].x == vs[movedVs[k]].x)) && ((vs[oldV].y == vs[movedVs[k]].y) && (vs[oldV].z == vs[movedVs[k]].z)))
                    {
                        MoveVertex(k, newV, movedVs, uniqueVs, movedUv1s, movedUv2s, movedUv3s, movedUv4s, movedColors);
                    }
                }
            }
            oldV = newV;
        }
    }

    private static void MoveVertex(int oldV, int newV, int[] movedVs, int[] uniqueVs, int[] movedUv1s, int[] movedUv2s, int[] movedUv3s, int[] movedUv4s, int[] movedColors)
    {
        movedVs[oldV] = movedVs[newV];
        movedVs[movedVs[oldV]] = movedVs[newV];
        movedVs[uniqueVs[oldV]] = movedVs[newV];
        if (movedUv1s.Length > 0)
        {
            movedUv1s[oldV] = movedUv1s[newV];
            movedUv1s[movedVs[oldV]] = movedUv1s[newV];
            movedUv1s[uniqueVs[oldV]] = movedUv1s[newV];
        }
        if (movedUv2s.Length > 0)
        {
            movedUv2s[oldV] = movedUv2s[newV];
            movedUv2s[movedVs[oldV]] = movedUv2s[newV];
            movedUv2s[uniqueVs[oldV]] = movedUv2s[newV];
        }
        if (movedUv3s.Length > 0)
        {
            movedUv3s[oldV] = movedUv3s[newV];
            movedUv3s[movedVs[oldV]] = movedUv3s[newV];
            movedUv3s[uniqueVs[oldV]] = movedUv3s[newV];
        }
        if (movedUv4s.Length > 0)
        {
            movedUv4s[oldV] = movedUv4s[newV];
            movedUv4s[movedVs[oldV]] = movedUv4s[newV];
            movedUv4s[uniqueVs[oldV]] = movedUv4s[newV];
        }
        if (movedColors.Length > 0)
        {
            movedColors[oldV] = movedColors[newV];
            movedColors[movedVs[oldV]] = movedColors[newV];
            movedColors[uniqueVs[oldV]] = movedColors[newV];
        }
    }

    private static void RemoveEmptyTriangles(List<Vector3> newVs, List<Vector3> newNs, List<Vector2> newUv1s, List<Vector2> newUv2s, List<Vector2> newUv3s, List<Vector2> newUv4s, List<Color32> newColors32, List<int> newTs, List<BoneWeight> newBws, int[] subMeshOffsets, List<int> newTGrps)
    {
        int index = subMeshOffsets.Length - 1;
        bool[] flagArray = new bool[newVs.Count];
        for (int i = newTs.Count - 3; i >= 0; i -= 3)
        {
            while ((index > 0) && ((i + 3) == subMeshOffsets[index]))
            {
                index--;
            }
            if (Area(newVs[newTs[i]], newVs[newTs[i + 1]], newVs[newTs[i + 2]]) <= 0f)
            {
                newTs.RemoveAt(i + 2);
                newTs.RemoveAt(i + 1);
                newTs.RemoveAt(i);
                newTGrps.RemoveAt(i / 3);
                for (int m = index + 1; m < subMeshOffsets.Length; m++)
                {
                    subMeshOffsets[m] -= 3;
                }
            }
            else
            {
                flagArray[newTs[i]] = true;
                flagArray[newTs[i + 1]] = true;
                flagArray[newTs[i + 2]] = true;
            }
        }
        bool flag = (newNs != null) && (newNs.Count > 0);
        bool flag2 = (newUv1s != null) && (newUv1s.Count > 0);
        bool flag3 = (newUv2s != null) && (newUv2s.Count > 0);
        bool flag4 = (newUv3s != null) && (newUv3s.Count > 0);
        bool flag5 = (newUv4s != null) && (newUv4s.Count > 0);
        bool flag6 = (newColors32 != null) && (newColors32.Count > 0);
        bool flag7 = (newBws != null) && (newBws.Count > 0);
        List<int> list = new List<int>();
        for (int j = flagArray.Length - 1; j >= 0; j--)
        {
            if (!flagArray[j])
            {
                newVs.RemoveAt(j);
                if (flag)
                {
                    newNs.RemoveAt(j);
                }
                if (flag2)
                {
                    newUv1s.RemoveAt(j);
                }
                if (flag3)
                {
                    newUv2s.RemoveAt(j);
                }
                if (flag4)
                {
                    newUv3s.RemoveAt(j);
                }
                if (flag5)
                {
                    newUv4s.RemoveAt(j);
                }
                if (flag6)
                {
                    newColors32.RemoveAt(j);
                }
                if (flag7)
                {
                    newBws.RemoveAt(j);
                }
                list.Add(j);
            }
        }
        for (int k = 0; k < newTs.Count; k++)
        {
            int num6 = newTs[k];
            int num7 = 0;
            for (int n = list.Count - 1; n >= 0; n--)
            {
                if (num6 < list[n])
                {
                    break;
                }
                num7++;
            }
            if (num7 > 0)
            {
                newTs[k] = num6 - num7;
            }
        }
    }

    private static void RemoveMiniTriangleGroups(float removeSmallParts, Vector3 sizeMultiplier, float aMaxWeight, List<Vector3> newVs, List<int> newTs, int[] subMeshOffsets, List<int> newTGrps)
    {
        float num = ((aMaxWeight * 0.5f) < 1f) ? Mathf.Pow(aMaxWeight * 0.5f, 3f) : (aMaxWeight * 0.5f);
        float num2 = 0f;
        List<int> list = new List<int>();
        List<int> list2 = new List<int>();
        List<float> list3 = new List<float>();
        for (int i = 0; i < newTGrps.Count; i++)
        {
            int item = newTGrps[i];
            float num5 = Area(OrbCreationExtensions.Vector3Extensions.Product(newVs[newTs[i * 3]], sizeMultiplier), OrbCreationExtensions.Vector3Extensions.Product(newVs[newTs[(i * 3) + 1]], sizeMultiplier), OrbCreationExtensions.Vector3Extensions.Product(newVs[newTs[(i * 3) + 2]], sizeMultiplier));
            int num6 = 0;
            while (num6 < list.Count)
            {
                if ((list[num6] == item) && (item >= 0))
                {
                    break;
                }
                num6++;
            }
            if (num6 >= list.Count)
            {
                list.Add(item);
                list3.Add(0f);
                list2.Add(0);
            }
            list3[num6] += num5;
            list2[num6] += 1;
            num2 += num5;
        }
        removeSmallParts = (Mathf.Clamp(removeSmallParts, 0f, 5f) * 0.0028f) * num;
        for (int j = 0; j < list.Count; j++)
        {
            if (((list3[j] / Mathf.Pow((float) list2[j], 0.33f)) / num2) < removeSmallParts)
            {
                int num8 = list[j];
                for (int k = newTGrps.Count - 1; k >= 0; k--)
                {
                    if (newTGrps[k] == num8)
                    {
                        newTs.RemoveAt(k * 3);
                        newTs.RemoveAt(k * 3);
                        newTs.RemoveAt(k * 3);
                        newTGrps.RemoveAt(k);
                        for (int m = 0; m < subMeshOffsets.Length; m++)
                        {
                            if (subMeshOffsets[m] > (k * 3))
                            {
                                subMeshOffsets[m] -= 3;
                            }
                        }
                    }
                }
            }
        }
    }

    public static void RemoveUnusedVertices(List<Vector3> vs, List<Vector3> ns, List<Vector2> uv1s, List<Vector2> uv2s, List<Vector2> uv3s, List<Vector2> uv4s, List<Color32> colors32, List<BoneWeight> bws, Dictionary<Material, List<int>> subMeshes)
    {
        List<List<Material>> list = new List<List<Material>>();
        List<List<int>> list2 = new List<List<int>>();
        for (int i = 0; i < vs.Count; i++)
        {
            list.Add(new List<Material>());
            list2.Add(new List<int>());
        }
        foreach (Material material in subMeshes.Keys)
        {
            List<int> list3 = subMeshes[material];
            for (int k = 0; k < list3.Count; k++)
            {
                list[list3[k]].Add(material);
                list2[list3[k]].Add(k);
            }
        }
        bool flag = (ns != null) && (ns.Count > 0);
        bool flag2 = (uv1s != null) && (uv1s.Count > 0);
        bool flag3 = (uv2s != null) && (uv2s.Count > 0);
        bool flag4 = (uv3s != null) && (uv3s.Count > 0);
        bool flag5 = (uv4s != null) && (uv4s.Count > 0);
        bool flag6 = (colors32 != null) && (colors32.Count > 0);
        bool flag7 = (bws != null) && (bws.Count > 0);
        int num3 = 0;
        for (int j = 0; j < vs.Count; j++)
        {
            List<Material> list4 = list[j];
            List<int> list5 = list2[j];
            if (list5.Count > 0)
            {
                if (num3 > 0)
                {
                    for (int m = 0; m < list4.Count; m++)
                    {
                        List<int> list6;
                        int num6;
                        num6 = list6[num6];
                        (list6 = subMeshes[list4[m]])[num6 = list5[m]] = num6 - num3;
                    }
                }
            }
            else
            {
                vs.RemoveAt(j);
                list.RemoveAt(j);
                list2.RemoveAt(j);
                if (flag)
                {
                    ns.RemoveAt(j);
                }
                if (flag2)
                {
                    uv1s.RemoveAt(j);
                }
                if (flag3)
                {
                    uv2s.RemoveAt(j);
                }
                if (flag4)
                {
                    uv3s.RemoveAt(j);
                }
                if (flag5)
                {
                    uv4s.RemoveAt(j);
                }
                if (flag6)
                {
                    colors32.RemoveAt(j);
                }
                if (flag7)
                {
                    bws.RemoveAt(j);
                }
                num3++;
                j--;
            }
        }
    }

    public static void RemoveUnusedVertices(List<Vector3> vs, List<Vector3> ns, List<Vector2> uv1s, List<Vector2> uv2s, List<Vector2> uv3s, List<Vector2> uv4s, List<Color32> colors32, List<BoneWeight> bws, List<List<int>> subMeshes)
    {
        List<List<int>> list = new List<List<int>>();
        List<List<int>> list2 = new List<List<int>>();
        for (int i = 0; i < vs.Count; i++)
        {
            list.Add(new List<int>());
            list2.Add(new List<int>());
        }
        for (int j = 0; j < subMeshes.Count; j++)
        {
            List<int> list3 = subMeshes[j];
            for (int m = 0; m < list3.Count; m++)
            {
                list[list3[m]].Add(j);
                list2[list3[m]].Add(m);
            }
        }
        bool flag = (ns != null) && (ns.Count > 0);
        bool flag2 = (uv1s != null) && (uv1s.Count > 0);
        bool flag3 = (uv2s != null) && (uv2s.Count > 0);
        bool flag4 = (uv3s != null) && (uv3s.Count > 0);
        bool flag5 = (uv4s != null) && (uv4s.Count > 0);
        bool flag6 = (colors32 != null) && (colors32.Count > 0);
        bool flag7 = (bws != null) && (bws.Count > 0);
        int num4 = 0;
        for (int k = 0; k < vs.Count; k++)
        {
            List<int> list4 = list[k];
            List<int> list5 = list2[k];
            if (list5.Count > 0)
            {
                if (num4 > 0)
                {
                    for (int n = 0; n < list4.Count; n++)
                    {
                        List<int> list6;
                        int num7;
                        num7 = list6[num7];
                        (list6 = subMeshes[list4[n]])[num7 = list5[n]] = num7 - num4;
                    }
                }
            }
            else
            {
                vs.RemoveAt(k);
                list.RemoveAt(k);
                list2.RemoveAt(k);
                if (flag)
                {
                    ns.RemoveAt(k);
                }
                if (flag2)
                {
                    uv1s.RemoveAt(k);
                }
                if (flag3)
                {
                    uv2s.RemoveAt(k);
                }
                if (flag4)
                {
                    uv3s.RemoveAt(k);
                }
                if (flag5)
                {
                    uv4s.RemoveAt(k);
                }
                if (flag6)
                {
                    colors32.RemoveAt(k);
                }
                if (flag7)
                {
                    bws.RemoveAt(k);
                }
                num4++;
                k--;
            }
        }
    }

    public static void RemoveUnusedVertices(List<Vector3> vs, List<Vector3> ns, List<Vector2> uv1s, List<Vector2> uv2s, List<Vector2> uv3s, List<Vector2> uv4s, List<Color32> colors32, List<BoneWeight> bws, List<int> ts)
    {
        List<List<int>> list = new List<List<int>>();
        for (int i = 0; i < vs.Count; i++)
        {
            list.Add(new List<int>());
        }
        for (int j = 0; j < ts.Count; j++)
        {
            list[ts[j]].Add(j);
        }
        bool flag = (ns != null) && (ns.Count > 0);
        bool flag2 = (uv1s != null) && (uv1s.Count > 0);
        bool flag3 = (uv2s != null) && (uv2s.Count > 0);
        bool flag4 = (uv3s != null) && (uv3s.Count > 0);
        bool flag5 = (uv4s != null) && (uv4s.Count > 0);
        bool flag6 = (colors32 != null) && (colors32.Count > 0);
        bool flag7 = (bws != null) && (bws.Count > 0);
        int num3 = 0;
        for (int k = 0; k < vs.Count; k++)
        {
            List<int> list2 = list[k];
            if (list2.Count > 0)
            {
                if (num3 > 0)
                {
                    for (int m = 0; m < list2.Count; m++)
                    {
                        List<int> list3;
                        int num6;
                        num6 = list3[num6];
                        (list3 = ts)[num6 = list2[m]] = num6 - num3;
                    }
                }
            }
            else
            {
                vs.RemoveAt(k);
                list.RemoveAt(k);
                if (flag)
                {
                    ns.RemoveAt(k);
                }
                if (flag2)
                {
                    uv1s.RemoveAt(k);
                }
                if (flag3)
                {
                    uv2s.RemoveAt(k);
                }
                if (flag4)
                {
                    uv3s.RemoveAt(k);
                }
                if (flag5)
                {
                    uv4s.RemoveAt(k);
                }
                if (flag6)
                {
                    colors32.RemoveAt(k);
                }
                if (flag7)
                {
                    bws.RemoveAt(k);
                }
                num3++;
                k--;
            }
        }
    }

    private static void SetTriangleGroup(int tIdx0, int tIdx1, int[] triangleGroups, List<List<int>> trianglesPerGroup)
    {
        if ((triangleGroups[tIdx0] < 0) && (triangleGroups[tIdx1] < 0))
        {
            triangleGroups[tIdx0] = trianglesPerGroup.Count;
            triangleGroups[tIdx1] = trianglesPerGroup.Count;
            trianglesPerGroup.Add(new List<int>());
            trianglesPerGroup[triangleGroups[tIdx0]].Add(tIdx0);
            trianglesPerGroup[triangleGroups[tIdx0]].Add(tIdx1);
        }
        else if ((triangleGroups[tIdx0] < 0) && (triangleGroups[tIdx1] >= 0))
        {
            triangleGroups[tIdx0] = triangleGroups[tIdx1];
            trianglesPerGroup[triangleGroups[tIdx1]].Add(tIdx0);
        }
        else if ((triangleGroups[tIdx0] >= 0) && (triangleGroups[tIdx1] < 0))
        {
            triangleGroups[tIdx1] = triangleGroups[tIdx0];
            trianglesPerGroup[triangleGroups[tIdx0]].Add(tIdx1);
        }
        else if (triangleGroups[tIdx0] != triangleGroups[tIdx1])
        {
            List<int> collection = trianglesPerGroup[triangleGroups[tIdx1]];
            trianglesPerGroup[triangleGroups[tIdx0]].AddRange(collection);
            trianglesPerGroup[triangleGroups[tIdx1]] = new List<int>();
            for (int i = 0; i < collection.Count; i++)
            {
                triangleGroups[collection[i]] = triangleGroups[tIdx0];
            }
        }
    }
}

