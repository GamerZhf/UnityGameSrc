namespace OrbCreationExtensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using UnityEngine;

    [Extension]
    public static class MeshExtensions
    {
        [Extension]
        public static bool CheckUvsWithin01Range(Mesh mesh)
        {
            foreach (Vector2 vector in mesh.uv)
            {
                if (((vector.x < 0f) || (vector.x > 1f)) || ((vector.y < 0f) || (vector.y > 1f)))
                {
                    return false;
                }
            }
            return true;
        }

        [Extension]
        public static void ClampUvs(Mesh mesh)
        {
            Vector2[] uv = mesh.uv;
            for (int i = 0; i < uv.Length; i++)
            {
                Vector2 vector = uv[i];
                vector.x = Mathf.Clamp01(vector.x);
                vector.y = Mathf.Clamp01(vector.y);
                uv[i] = vector;
            }
            mesh.uv = uv;
        }

        [Extension]
        public static Mesh CopyAndRemoveHiddenTriangles(Mesh orig, int subMeshIdx, Matrix4x4 localToWorldMatrix, Mesh[] hidingMeshes, int[] hidingSubMeshes, Matrix4x4[] hidingLocalToWorldMatrices, [Optional, DefaultParameterValue(0.01f)] float maxRemoveDistance)
        {
            if (subMeshIdx >= orig.subMeshCount)
            {
                return null;
            }
            if (hidingMeshes.Length <= 0)
            {
                return null;
            }
            if ((hidingMeshes.Length != hidingSubMeshes.Length) || (hidingMeshes.Length != hidingLocalToWorldMatrices.Length))
            {
                return null;
            }
            Mesh mesh = UnityEngine.Object.Instantiate<Mesh>(orig);
            List<List<int>> subMeshes = new List<List<int>>(mesh.subMeshCount);
            List<Vector3> vs = new List<Vector3>(orig.vertices);
            List<Vector3> ns = new List<Vector3>(orig.vertexCount);
            List<Vector2> list4 = new List<Vector2>(orig.vertexCount);
            List<Vector2> list5 = new List<Vector2>();
            List<Vector2> list6 = new List<Vector2>();
            List<Vector2> list7 = new List<Vector2>();
            List<Color32> list8 = new List<Color32>();
            List<BoneWeight> bws = new List<BoneWeight>();
            ns.AddRange(orig.normals);
            if ((ns == null) || (ns.Count <= 0))
            {
                orig.RecalculateNormals();
                ns.AddRange(orig.normals);
            }
            list4.AddRange(orig.uv);
            list5.AddRange(orig.uv2);
            list6.AddRange(orig.uv3);
            list7.AddRange(orig.uv4);
            list8.AddRange(orig.colors32);
            bws.AddRange(orig.boneWeights);
            for (int i = 0; i < orig.subMeshCount; i++)
            {
                List<int> item = new List<int>();
                item.AddRange(orig.GetTriangles(i));
                subMeshes.Add(item);
            }
            List<int> list11 = subMeshes[subMeshIdx];
            List<Vector3> hidingVs = new List<Vector3>();
            List<int> hidingTs = new List<int>();
            Mesh mesh2 = null;
            int num2 = 0;
            int length = 0;
            for (int j = 0; j < hidingMeshes.Length; j++)
            {
                Mesh mesh3 = hidingMeshes[j];
                int[] triangles = mesh3.GetTriangles(hidingSubMeshes[j]);
                if (mesh2 != mesh3)
                {
                    num2 += length;
                }
                for (int num5 = 0; num5 < triangles.Length; num5++)
                {
                    hidingTs.Add(triangles[num5] + num2);
                }
                if (mesh2 != mesh3)
                {
                    Matrix4x4 matrixx = hidingLocalToWorldMatrices[j];
                    Vector3[] vertices = mesh3.vertices;
                    for (int num6 = 0; num6 < vertices.Length; num6++)
                    {
                        hidingVs.Add(matrixx.MultiplyPoint3x4(vertices[num6]));
                    }
                    length = vertices.Length;
                    mesh2 = mesh3;
                }
            }
            List<Vector3> triMinCorners = new List<Vector3>();
            List<Vector3> triMaxCorners = new List<Vector3>();
            for (int k = 0; k < hidingTs.Count; k += 3)
            {
                Vector3 vector = hidingVs[hidingTs[k]];
                Vector3 vector2 = hidingVs[hidingTs[k + 1]];
                Vector3 vector3 = hidingVs[hidingTs[k + 2]];
                float x = Mathf.Min(Mathf.Min(vector.x, vector2.x), vector3.x);
                float y = Mathf.Min(Mathf.Min(vector.y, vector2.y), vector3.y);
                triMinCorners.Add(new Vector3(x, y, Mathf.Min(Mathf.Min(vector.z, vector2.z), vector3.z)));
                float introduced39 = Mathf.Max(Mathf.Max(vector.x, vector2.x), vector3.x);
                float introduced40 = Mathf.Max(Mathf.Max(vector.y, vector2.y), vector3.y);
                triMaxCorners.Add(new Vector3(introduced39, introduced40, Mathf.Max(Mathf.Max(vector.z, vector2.z), vector3.z)));
            }
            List<int> list16 = new List<int>();
            for (int m = 0; m < list11.Count; m += 3)
            {
                Vector3 vector4 = localToWorldMatrix.MultiplyPoint3x4(vs[list11[m]]);
                Vector3 vector5 = localToWorldMatrix.MultiplyPoint3x4(vs[list11[m + 1]]);
                Vector3 vector6 = localToWorldMatrix.MultiplyPoint3x4(vs[list11[m + 2]]);
                if (!IsTriangleHidden(vector4, vector5, vector6, maxRemoveDistance, triMinCorners, triMaxCorners, hidingVs, hidingTs))
                {
                    list16.Add(list11[m]);
                    list16.Add(list11[m + 1]);
                    list16.Add(list11[m + 2]);
                }
            }
            subMeshes[subMeshIdx] = list16;
            LODMaker.RemoveUnusedVertices(vs, ns, list4, list5, list6, list7, list8, bws, subMeshes);
            mesh.uv4 = null;
            mesh.uv3 = null;
            mesh.uv2 = null;
            mesh.uv2 = null;
            mesh.boneWeights = null;
            mesh.colors32 = null;
            mesh.normals = null;
            mesh.tangents = null;
            mesh.triangles = null;
            mesh.vertices = vs.ToArray();
            if (ns.Count > 0)
            {
                mesh.normals = ns.ToArray();
            }
            if (list4.Count > 0)
            {
                mesh.uv = list4.ToArray();
            }
            if (list5.Count > 0)
            {
                mesh.uv2 = list5.ToArray();
            }
            if (list6.Count > 0)
            {
                mesh.uv3 = list6.ToArray();
            }
            if (list7.Count > 0)
            {
                mesh.uv4 = list7.ToArray();
            }
            if (list8.Count > 0)
            {
                mesh.colors32 = list8.ToArray();
            }
            if (bws.Count > 0)
            {
                mesh.boneWeights = bws.ToArray();
            }
            mesh.subMeshCount = subMeshes.Count;
            for (int n = 0; n < subMeshes.Count; n++)
            {
                if (n == subMeshIdx)
                {
                    mesh.SetTriangles(list16.ToArray(), n);
                }
                else
                {
                    mesh.SetTriangles(subMeshes[n].ToArray(), n);
                }
            }
            if ((ns == null) || (ns.Count <= 0))
            {
                mesh.RecalculateNormals();
            }
            RecalculateTangents(mesh);
            mesh.RecalculateBounds();
            return mesh;
        }

        [Extension]
        public static Mesh CopyAndRemoveSubmeshes(Mesh orig, int[] submeshesToRemove)
        {
            Mesh mesh = UnityEngine.Object.Instantiate<Mesh>(orig);
            List<List<int>> subMeshes = new List<List<int>>(mesh.subMeshCount);
            List<Vector3> vs = new List<Vector3>(orig.vertices);
            List<Vector3> ns = new List<Vector3>(orig.vertexCount);
            List<Vector2> list4 = new List<Vector2>(orig.vertexCount);
            List<Vector2> list5 = new List<Vector2>();
            List<Vector2> list6 = new List<Vector2>();
            List<Vector2> list7 = new List<Vector2>();
            List<Color32> list8 = new List<Color32>();
            List<BoneWeight> bws = new List<BoneWeight>();
            ns.AddRange(orig.normals);
            list4.AddRange(orig.uv);
            list5.AddRange(orig.uv2);
            list6.AddRange(orig.uv3);
            list7.AddRange(orig.uv4);
            list8.AddRange(orig.colors32);
            bws.AddRange(orig.boneWeights);
            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                bool flag = false;
                for (int k = 0; k < submeshesToRemove.Length; k++)
                {
                    if (submeshesToRemove[k] == i)
                    {
                        flag = true;
                    }
                }
                if (!flag)
                {
                    List<int> item = new List<int>();
                    item.AddRange(mesh.GetTriangles(i));
                    subMeshes.Add(item);
                }
            }
            LODMaker.RemoveUnusedVertices(vs, ns, list4, list5, list6, list7, list8, bws, subMeshes);
            mesh.uv4 = null;
            mesh.uv3 = null;
            mesh.uv2 = null;
            mesh.uv2 = null;
            mesh.boneWeights = null;
            mesh.colors32 = null;
            mesh.normals = null;
            mesh.tangents = null;
            mesh.triangles = null;
            mesh.vertices = vs.ToArray();
            if (ns.Count > 0)
            {
                mesh.normals = ns.ToArray();
            }
            if (list4.Count > 0)
            {
                mesh.uv = list4.ToArray();
            }
            if (list5.Count > 0)
            {
                mesh.uv2 = list5.ToArray();
            }
            if (list6.Count > 0)
            {
                mesh.uv3 = list6.ToArray();
            }
            if (list7.Count > 0)
            {
                mesh.uv4 = list7.ToArray();
            }
            if (list8.Count > 0)
            {
                mesh.colors32 = list8.ToArray();
            }
            if (bws.Count > 0)
            {
                mesh.boneWeights = bws.ToArray();
            }
            mesh.subMeshCount = subMeshes.Count;
            for (int j = 0; j < subMeshes.Count; j++)
            {
                mesh.SetTriangles(subMeshes[j].ToArray(), j);
            }
            if ((ns == null) || (ns.Count <= 0))
            {
                mesh.RecalculateNormals();
            }
            RecalculateTangents(mesh);
            mesh.RecalculateBounds();
            return mesh;
        }

        public static float DistanceToPlane(Vector3 fromPos, Vector3 direction, Vector3 pointOnPlane, Vector3 normalPlane)
        {
            float positiveInfinity = float.PositiveInfinity;
            if (OrbCreationExtensions.Vector3Extensions.InProduct(direction, normalPlane) != 0f)
            {
                positiveInfinity = OrbCreationExtensions.Vector3Extensions.InProduct(pointOnPlane - fromPos, normalPlane) / OrbCreationExtensions.Vector3Extensions.InProduct(direction, normalPlane);
            }
            return positiveInfinity;
        }

        public static Vector3 GetNormal(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            return Vector3.Cross(v1 - v0, v2 - v0).normalized;
        }

        [Extension]
        public static int GetTriangleCount(Mesh orig)
        {
            return (orig.triangles.Length / 3);
        }

        private static List<int> GetTrianglesWithinRange(Vector3 v0, Vector3 v1, Vector3 v2, float maxDistance, List<Vector3> triMinCorners, List<Vector3> triMaxCorners)
        {
            List<int> list = new List<int>();
            Vector3 vector = new Vector3(Mathf.Min(Mathf.Min(v0.x, v1.x), v2.x) - maxDistance, Mathf.Min(Mathf.Min(v0.y, v1.y), v2.y) - maxDistance, Mathf.Min(Mathf.Min(v0.z, v1.z), v2.z) - maxDistance);
            Vector3 vector2 = new Vector3(Mathf.Max(Mathf.Max(v0.x, v1.x), v2.x) + maxDistance, Mathf.Max(Mathf.Max(v0.y, v1.y), v2.y) + maxDistance, Mathf.Max(Mathf.Max(v0.z, v1.z), v2.z) + maxDistance);
            for (int i = 0; i < triMaxCorners.Count; i++)
            {
                Vector3 vector3 = triMaxCorners[i];
                if (((vector3.x > vector.x) && (vector3.y > vector.y)) && (vector3.z > vector.z))
                {
                    Vector3 vector4 = triMinCorners[i];
                    if (((vector4.x < vector2.x) && (vector4.y < vector2.y)) && (vector4.z < vector2.z))
                    {
                        list.Add(i);
                    }
                }
            }
            return list;
        }

        [Extension]
        public static Vector4 GetUvRange(Mesh mesh)
        {
            Vector4 vector = new Vector4(0f, 0f, 1f, 1f);
            foreach (Vector2 vector2 in mesh.uv)
            {
                if (vector2.x < vector.x)
                {
                    vector.x = vector2.x;
                }
                if (vector2.y < vector.y)
                {
                    vector.y = vector2.y;
                }
                if (vector2.x > vector.z)
                {
                    vector.z = vector2.x;
                }
                if (vector2.y > vector.w)
                {
                    vector.w = vector2.y;
                }
            }
            return vector;
        }

        private static bool IsHidden(Vector3 v, Vector3 n, float maxDistance, List<Vector3> hidingVs, List<int> hidingTs, List<int> trianglesToCheck)
        {
            for (int i = 0; i < trianglesToCheck.Count; i++)
            {
                int num2 = trianglesToCheck[i] * 3;
                Vector3 vector = hidingVs[hidingTs[num2]];
                Vector3 vector2 = hidingVs[hidingTs[num2 + 1]];
                Vector3 vector3 = hidingVs[hidingTs[num2 + 2]];
                Vector3 to = GetNormal(vector, vector2, vector3);
                if (Vector3.Angle(n, to) < 60f)
                {
                    float num4 = DistanceToPlane(v, n, vector, to);
                    if ((num4 > 0f) && (num4 <= maxDistance))
                    {
                        Vector3 p = v + ((Vector3) (n * num4));
                        if (OrbCreationExtensions.Vector2Extensions.IsBarycentricInTriangle(OrbCreationExtensions.Vector3Extensions.Barycentric(p, vector, vector2, vector3)))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        [Extension]
        public static bool IsSkinnedMesh(Mesh mesh)
        {
            return ((mesh.blendShapeCount > 0) || ((mesh.bindposes != null) && (mesh.bindposes.Length > 0)));
        }

        private static bool IsTriangleHidden(Vector3 v0, Vector3 v1, Vector3 v2, float maxDistance, List<Vector3> triMinCorners, List<Vector3> triMaxCorners, List<Vector3> hidingVs, List<int> hidingTs)
        {
            Vector3 n = GetNormal(v0, v1, v2);
            List<int> trianglesToCheck = GetTrianglesWithinRange(v0, v1, v2, maxDistance, triMinCorners, triMaxCorners);
            if (!IsHidden((Vector3) (((v0 + v1) + v2) / 3f), n, maxDistance, hidingVs, hidingTs, trianglesToCheck))
            {
                return false;
            }
            if (!IsHidden(v0, n, maxDistance, hidingVs, hidingTs, trianglesToCheck))
            {
                return false;
            }
            if (!IsHidden(v1, n, maxDistance, hidingVs, hidingTs, trianglesToCheck))
            {
                return false;
            }
            if (!IsHidden(v2, n, maxDistance, hidingVs, hidingTs, trianglesToCheck))
            {
                return false;
            }
            return true;
        }

        [Extension]
        public static Mesh MakeLODMesh(Mesh orig, float aMaxWeight, bool recalcNormals, [Optional, DefaultParameterValue(1f)] float removeSmallParts, [Optional, DefaultParameterValue(1f)] float protectNormals, [Optional, DefaultParameterValue(1f)] float protectUvs, [Optional, DefaultParameterValue(1f)] float protectSubMeshesAndSharpEdges, [Optional, DefaultParameterValue(1f)] float smallTrianglesFirst)
        {
            return LODMaker.MakeLODMesh(orig, aMaxWeight, removeSmallParts, protectNormals, protectUvs, protectSubMeshesAndSharpEdges, smallTrianglesFirst, recalcNormals, false);
        }

        [Extension]
        public static Mesh[] MakeLODMeshes(Mesh mesh, float[] maxWeights, bool recalcNormals, [Optional, DefaultParameterValue(1f)] float removeSmallParts, [Optional, DefaultParameterValue(1f)] float protectNormals, [Optional, DefaultParameterValue(1f)] float protectUvs, [Optional, DefaultParameterValue(1f)] float protectSubMeshesAndSharpEdges, [Optional, DefaultParameterValue(1f)] float smallTrianglesFirst, [Optional, DefaultParameterValue(1)] int nrOfSteps)
        {
            if (maxWeights.Length < 1)
            {
                UnityEngine.Debug.LogError("Mesh.GetLODLevelMeshes: maxWeights arrays is empty");
                return null;
            }
            Mesh[] meshArray = new Mesh[maxWeights.Length];
            float num = 0f;
            for (int i = 0; i < maxWeights.Length; i++)
            {
                if (nrOfSteps < 1)
                {
                    nrOfSteps = 1;
                }
                for (int j = 0; j < nrOfSteps; j++)
                {
                    float num4 = maxWeights[i] - num;
                    mesh = MakeLODMesh(mesh, ((j + 1) * (num4 / ((float) nrOfSteps))) + num, recalcNormals, removeSmallParts, protectNormals, protectUvs, protectSubMeshesAndSharpEdges, smallTrianglesFirst);
                }
                num = maxWeights[i];
                meshArray[i] = mesh;
            }
            return meshArray;
        }

        [Extension, DebuggerHidden]
        public static IEnumerator MakeLODMeshInBackground(Mesh mesh, float maxWeight, bool recalcNormals, float removeSmallParts, Action<Mesh> result)
        {
            <MakeLODMeshInBackground>c__Iterator1E iteratore = new <MakeLODMeshInBackground>c__Iterator1E();
            iteratore.maxWeight = maxWeight;
            iteratore.removeSmallParts = removeSmallParts;
            iteratore.mesh = mesh;
            iteratore.result = result;
            iteratore.recalcNormals = recalcNormals;
            iteratore.<$>maxWeight = maxWeight;
            iteratore.<$>removeSmallParts = removeSmallParts;
            iteratore.<$>mesh = mesh;
            iteratore.<$>result = result;
            iteratore.<$>recalcNormals = recalcNormals;
            return iteratore;
        }

        [Extension]
        public static void MergeSubmeshInto(Mesh mesh, int from, int to)
        {
            int[] triangles = mesh.GetTriangles(from);
            int[] numArray2 = mesh.GetTriangles(to);
            List<int> list = new List<int>();
            for (int i = 0; i < numArray2.Length; i++)
            {
                list.Add(numArray2[i]);
            }
            for (int j = 0; j < triangles.Length; j++)
            {
                list.Add(triangles[j]);
            }
            mesh.SetTriangles(list.ToArray(), to);
            for (int k = from + 1; k < mesh.subMeshCount; k++)
            {
                mesh.SetTriangles(mesh.GetTriangles(k), k - 1);
            }
            mesh.SetTriangles(null, mesh.subMeshCount - 1);
            mesh.subMeshCount--;
        }

        [Extension]
        public static void RecalculateTangents(Mesh mesh)
        {
            int vertexCount = mesh.vertexCount;
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;
            Vector2[] uv = mesh.uv;
            int[] triangles = mesh.triangles;
            int num2 = triangles.Length / 3;
            Vector4[] vectorArray4 = new Vector4[vertexCount];
            Vector3[] vectorArray5 = new Vector3[vertexCount];
            Vector3[] vectorArray6 = new Vector3[vertexCount];
            int index = 0;
            if (uv.Length > 0)
            {
                for (int i = 0; i < num2; i++)
                {
                    int num5 = triangles[index];
                    int num6 = triangles[index + 1];
                    int num7 = triangles[index + 2];
                    Vector3 vector = vertices[num5];
                    Vector3 vector2 = vertices[num6];
                    Vector3 vector3 = vertices[num7];
                    Vector2 vector4 = uv[num5];
                    Vector2 vector5 = uv[num6];
                    Vector2 vector6 = uv[num7];
                    float num8 = vector2.x - vector.x;
                    float num9 = vector3.x - vector.x;
                    float num10 = vector2.y - vector.y;
                    float num11 = vector3.y - vector.y;
                    float num12 = vector2.z - vector.z;
                    float num13 = vector3.z - vector.z;
                    float num14 = vector5.x - vector4.x;
                    float num15 = vector6.x - vector4.x;
                    float num16 = vector5.y - vector4.y;
                    float num17 = vector6.y - vector4.y;
                    float num18 = (num14 * num17) - (num15 * num16);
                    float num19 = (num18 != 0f) ? (1f / num18) : 0f;
                    Vector3 vector7 = new Vector3(((num17 * num8) - (num16 * num9)) * num19, ((num17 * num10) - (num16 * num11)) * num19, ((num17 * num12) - (num16 * num13)) * num19);
                    Vector3 vector8 = new Vector3(((num14 * num9) - (num15 * num8)) * num19, ((num14 * num11) - (num15 * num10)) * num19, ((num14 * num13) - (num15 * num12)) * num19);
                    vectorArray5[num5] += vector7;
                    vectorArray5[num6] += vector7;
                    vectorArray5[num7] += vector7;
                    vectorArray6[num5] += vector8;
                    vectorArray6[num6] += vector8;
                    vectorArray6[num7] += vector8;
                    index += 3;
                }
                for (int j = 0; j < vertexCount; j++)
                {
                    Vector3 normal = normals[j];
                    Vector3 tangent = vectorArray5[j];
                    Vector3.OrthoNormalize(ref normal, ref tangent);
                    vectorArray4[j].x = tangent.x;
                    vectorArray4[j].y = tangent.y;
                    vectorArray4[j].z = tangent.z;
                    vectorArray4[j].w = (Vector3.Dot(Vector3.Cross(normal, tangent), vectorArray6[j]) >= 0f) ? 1f : -1f;
                }
                mesh.tangents = vectorArray4;
            }
        }

        [Extension]
        public static Mesh ScaledRotatedTranslatedMesh(Mesh mesh, Vector3 scale, Quaternion rotate, Vector3 translate)
        {
            Mesh mesh2 = UnityEngine.Object.Instantiate<Mesh>(mesh);
            Vector3[] vertices = mesh2.vertices;
            Vector3[] normals = mesh2.normals;
            bool flag = true;
            if (((normals == null) || (normals.Length < vertices.Length)) || (rotate == Quaternion.identity))
            {
                flag = false;
            }
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].x *= scale.x;
                vertices[i].y *= scale.y;
                vertices[i].z *= scale.z;
                vertices[i] = (Vector3) (rotate * vertices[i]);
                if (flag)
                {
                    normals[i] = (Vector3) (rotate * normals[i]);
                }
                vertices[i] += translate;
            }
            mesh2.vertices = vertices;
            if (flag)
            {
                mesh2.normals = normals;
            }
            mesh2.RecalculateBounds();
            return mesh2;
        }

        [Extension]
        public static void SetAtlasRectForSubmesh(Mesh mesh, Rect atlasRect, int submeshIndex)
        {
            if (submeshIndex < mesh.subMeshCount)
            {
                int[] triangles = mesh.GetTriangles(submeshIndex);
                List<int> list = new List<int>();
                for (int i = 0; i < triangles.Length; i++)
                {
                    int num2 = 0;
                    while (num2 < list.Count)
                    {
                        if (list[num2] == triangles[i])
                        {
                            break;
                        }
                        num2++;
                    }
                    if (num2 >= list.Count)
                    {
                        list.Add(triangles[i]);
                    }
                }
                Vector2[] uv = mesh.uv;
                for (int j = 0; j < list.Count; j++)
                {
                    Vector2 vector = uv[list[j]];
                    vector.x = (Mathf.Clamp01(vector.x) * atlasRect.width) + atlasRect.x;
                    vector.y = (Mathf.Clamp01(vector.y) * atlasRect.height) + atlasRect.y;
                    uv[list[j]] = vector;
                }
                mesh.uv = uv;
            }
        }

        [Extension]
        public static void WrapUvs(Mesh mesh)
        {
            Vector2[] uv = mesh.uv;
            for (int i = 0; i < uv.Length; i++)
            {
                Vector2 vector = uv[i];
                while (vector.x > 1f)
                {
                    vector.x--;
                }
                while (vector.x < 0f)
                {
                    vector.x++;
                }
                while (vector.y > 1f)
                {
                    vector.y--;
                }
                while (vector.y < 0f)
                {
                    vector.y++;
                }
                uv[i] = vector;
            }
            mesh.uv = uv;
        }

        [CompilerGenerated]
        private sealed class <MakeLODMeshInBackground>c__Iterator1E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal float <$>maxWeight;
            internal Mesh <$>mesh;
            internal bool <$>recalcNormals;
            internal float <$>removeSmallParts;
            internal Action<Mesh> <$>result;
            internal Matrix4x4[] <bindposes>__9;
            internal BoneWeight[] <bws>__10;
            internal Color32[] <colors32>__7;
            internal Hashtable <lodInfo>__0;
            internal Bounds <meshBounds>__15;
            internal Vector3[] <ns>__2;
            internal int <s>__12;
            internal int[] <subMeshOffsets>__11;
            internal int[] <subTs>__13;
            internal int <t>__14;
            internal Thread <thread>__16;
            internal int[] <ts>__8;
            internal Vector2[] <uv1s>__3;
            internal Vector2[] <uv2s>__4;
            internal Vector2[] <uv3s>__5;
            internal Vector2[] <uv4s>__6;
            internal Vector3[] <vs>__1;
            internal float maxWeight;
            internal Mesh mesh;
            internal bool recalcNormals;
            internal float removeSmallParts;
            internal Action<Mesh> result;

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
                        this.<lodInfo>__0 = new Hashtable();
                        this.<lodInfo>__0["maxWeight"] = this.maxWeight;
                        this.<lodInfo>__0["removeSmallParts"] = this.removeSmallParts;
                        this.<vs>__1 = this.mesh.vertices;
                        if (this.<vs>__1.Length <= 0)
                        {
                            throw new ApplicationException("Mesh was empty");
                        }
                        this.<ns>__2 = this.mesh.normals;
                        if (this.<ns>__2.Length == 0)
                        {
                            this.mesh.RecalculateNormals();
                            this.<ns>__2 = this.mesh.normals;
                        }
                        this.<uv1s>__3 = this.mesh.uv;
                        this.<uv2s>__4 = this.mesh.uv2;
                        this.<uv3s>__5 = this.mesh.uv3;
                        this.<uv4s>__6 = this.mesh.uv4;
                        this.<colors32>__7 = this.mesh.colors32;
                        this.<ts>__8 = this.mesh.triangles;
                        this.<bindposes>__9 = this.mesh.bindposes;
                        this.<bws>__10 = this.mesh.boneWeights;
                        this.<subMeshOffsets>__11 = new int[this.mesh.subMeshCount];
                        if (this.mesh.subMeshCount > 1)
                        {
                            this.<s>__12 = 0;
                            while (this.<s>__12 < this.mesh.subMeshCount)
                            {
                                this.<subTs>__13 = this.mesh.GetTriangles(this.<s>__12);
                                this.<t>__14 = 0;
                                while (this.<t>__14 < this.<subTs>__13.Length)
                                {
                                    this.<ts>__8[this.<subMeshOffsets>__11[this.<s>__12] + this.<t>__14] = this.<subTs>__13[this.<t>__14];
                                    this.<t>__14++;
                                }
                                if ((this.<s>__12 + 1) < this.mesh.subMeshCount)
                                {
                                    this.<subMeshOffsets>__11[this.<s>__12 + 1] = this.<subMeshOffsets>__11[this.<s>__12] + this.<t>__14;
                                }
                                this.<s>__12++;
                            }
                        }
                        this.<meshBounds>__15 = this.mesh.bounds;
                        this.<lodInfo>__0["vertices"] = this.<vs>__1;
                        this.<lodInfo>__0["normals"] = this.<ns>__2;
                        this.<lodInfo>__0["uv1s"] = this.<uv1s>__3;
                        this.<lodInfo>__0["uv2s"] = this.<uv2s>__4;
                        this.<lodInfo>__0["uv3s"] = this.<uv3s>__5;
                        this.<lodInfo>__0["uv4s"] = this.<uv4s>__6;
                        this.<lodInfo>__0["colors32"] = this.<colors32>__7;
                        this.<lodInfo>__0["triangles"] = this.<ts>__8;
                        this.<lodInfo>__0["bindposes"] = this.<bindposes>__9;
                        this.<lodInfo>__0["boneWeights"] = this.<bws>__10;
                        this.<lodInfo>__0["subMeshOffsets"] = this.<subMeshOffsets>__11;
                        this.<lodInfo>__0["meshBounds"] = this.<meshBounds>__15;
                        this.<thread>__16 = new Thread(new ParameterizedThreadStart(LODMaker.MakeLODMeshInBackground));
                        this.<thread>__16.Start(this.<lodInfo>__0);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_04CC;
                }
                while (!this.<lodInfo>__0.ContainsKey("ready"))
                {
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 1;
                    return true;
                }
                this.result(LODMaker.CreateNewMesh((Vector3[]) this.<lodInfo>__0["vertices"], (Vector3[]) this.<lodInfo>__0["normals"], (Vector2[]) this.<lodInfo>__0["uv1s"], (Vector2[]) this.<lodInfo>__0["uv2s"], (Vector2[]) this.<lodInfo>__0["uv3s"], (Vector2[]) this.<lodInfo>__0["uv4s"], (Color32[]) this.<lodInfo>__0["colors32"], (int[]) this.<lodInfo>__0["triangles"], (BoneWeight[]) this.<lodInfo>__0["boneWeights"], (Matrix4x4[]) this.<lodInfo>__0["bindposes"], (int[]) this.<lodInfo>__0["subMeshOffsets"], this.recalcNormals));
                this.$PC = -1;
            Label_04CC:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
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
    }
}

