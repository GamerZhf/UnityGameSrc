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
    public static class GameObjectExtensions
    {
        private static unsafe Vector3 ApplyBindPose(Vector3 vertex, Transform bone, Matrix4x4 bindpose, float boneWeight)
        {
            Matrix4x4 matrixx = bone.localToWorldMatrix * bindpose;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    ref Matrix4x4 matrixxRef;
                    int num3;
                    int num4;
                    float num5 = matrixxRef[num3, num4];
                    (matrixxRef = (Matrix4x4) &matrixx)[num3 = i, num4 = j] = num5 * boneWeight;
                }
            }
            return matrixx.MultiplyPoint3x4(vertex);
        }

        [Extension]
        public static Mesh[] CombineMeshes(GameObject aGO)
        {
            return CombineMeshes(aGO, new string[0], true);
        }

        [Extension]
        public static Mesh[] CombineMeshes(GameObject aGO, string[] skipSubmeshNames, [Optional, DefaultParameterValue(true)] bool makeNewGameObjectWhenRendererPresent)
        {
            List<Mesh> list = new List<Mesh>();
            MeshRenderer[] componentsInChildren = aGO.GetComponentsInChildren<MeshRenderer>(false);
            SkinnedMeshRenderer[] rendererArray2 = aGO.GetComponentsInChildren<SkinnedMeshRenderer>(false);
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int lightmapIndex = -999;
            bool flag = false;
            if ((aGO.GetComponent<SkinnedMeshRenderer>() != null) || (aGO.GetComponent<MeshRenderer>() != null))
            {
                flag = makeNewGameObjectWhenRendererPresent;
            }
            if ((rendererArray2 != null) && (rendererArray2.Length > 0))
            {
                foreach (SkinnedMeshRenderer renderer in rendererArray2)
                {
                    if (renderer.sharedMesh != null)
                    {
                        num += renderer.sharedMesh.vertexCount;
                        num2++;
                        num3++;
                    }
                }
            }
            if ((componentsInChildren != null) && (componentsInChildren.Length > 0))
            {
                foreach (MeshRenderer renderer2 in componentsInChildren)
                {
                    MeshFilter component = renderer2.gameObject.GetComponent<MeshFilter>();
                    if ((component != null) && (component.sharedMesh != null))
                    {
                        if (((lightmapIndex == -999) && (renderer2.lightmapIndex >= 0)) && (renderer2.lightmapIndex <= 0xfd))
                        {
                            lightmapIndex = renderer2.lightmapIndex;
                        }
                        if (((lightmapIndex < 0) || (renderer2.lightmapIndex < 0)) || ((renderer2.lightmapIndex > 0xfd) || (lightmapIndex == renderer2.lightmapIndex)))
                        {
                            num += component.sharedMesh.vertexCount;
                            num2++;
                        }
                    }
                    num3++;
                }
            }
            if (num2 == 0)
            {
                throw new ApplicationException("No meshes found in children. There's nothing to combine.");
            }
            if (flag)
            {
                GameObject obj2 = new GameObject();
                string str = aGO.name + "_Merged";
                string name = str;
                for (int i = 0; GameObject.Find(name) != null; i++)
                {
                    name = str + "_" + i;
                }
                obj2.name = name;
                obj2.transform.SetParent(aGO.transform.parent);
                obj2.transform.localPosition = aGO.transform.localPosition;
                obj2.transform.localRotation = aGO.transform.localRotation;
                obj2.transform.localScale = aGO.transform.localScale;
                aGO = obj2;
            }
            int num8 = 1;
            int num9 = -1;
            int num10 = 0;
            while (num10 < num2)
            {
                if (num9 == num10)
                {
                    break;
                }
                num9 = num10;
                GameObject obj3 = aGO;
                if (num > 0xfffe)
                {
                    obj3 = new GameObject();
                    obj3.name = "Merged part " + num8++;
                    obj3.transform.SetParent(aGO.transform);
                    obj3.transform.localPosition = Vector3.zero;
                    obj3.transform.localRotation = Quaternion.identity;
                    obj3.transform.localScale = Vector3.one;
                }
                Mesh mesh = null;
                List<Vector3> vertices = new List<Vector3>();
                List<Vector3> normals = new List<Vector3>();
                List<Vector2> list4 = new List<Vector2>();
                List<Vector2> list5 = new List<Vector2>();
                List<Vector2> list6 = new List<Vector2>();
                List<Vector2> list7 = new List<Vector2>();
                List<Color32> list8 = new List<Color32>();
                List<Transform> bones = new List<Transform>();
                List<Matrix4x4> bindposes = new List<Matrix4x4>();
                List<BoneWeight> boneWeights = new List<BoneWeight>();
                Dictionary<Material, List<int>> subMeshes = new Dictionary<Material, List<int>>();
                if ((rendererArray2 != null) && (rendererArray2.Length > 0))
                {
                    bool flag2 = false;
                    bool flag3 = false;
                    int num11 = 0;
                    int num12 = -1;
                    foreach (SkinnedMeshRenderer renderer3 in rendererArray2)
                    {
                        if (renderer3.sharedMesh != null)
                        {
                            if ((vertices.Count + renderer3.sharedMesh.vertexCount) > 0xfffe)
                            {
                                flag2 = true;
                            }
                            if ((num10 <= num11) && !flag2)
                            {
                                if (((num11 != num12) && MergeMeshInto(renderer3.sharedMesh, renderer3.bones, renderer3.sharedMaterials, vertices, normals, list4, list5, list6, list7, list8, boneWeights, bones, bindposes, subMeshes, ((renderer3.transform.localScale.x * renderer3.transform.localScale.y) * renderer3.transform.localScale.z) < 0f, new Vector4(1f, 1f, 0f, 0f), renderer3.transform, obj3.transform, renderer3.gameObject.name + "_" + renderer3.sharedMesh.name, skipSubmeshNames)) && (renderer3.gameObject != obj3))
                                {
                                    renderer3.gameObject.SetActive(false);
                                }
                                flag3 = true;
                                num10++;
                            }
                            num11++;
                        }
                    }
                    if (((componentsInChildren != null) && (componentsInChildren.Length > 0)) && flag3)
                    {
                        foreach (MeshRenderer renderer4 in componentsInChildren)
                        {
                            MeshFilter filter2 = renderer4.gameObject.GetComponent<MeshFilter>();
                            if (((filter2 != null) && (filter2.sharedMesh != null)) && (filter2.gameObject != obj3))
                            {
                                if ((vertices.Count + filter2.sharedMesh.vertexCount) > 0xfffe)
                                {
                                    flag2 = true;
                                }
                                if ((num10 <= num11) && !flag2)
                                {
                                    if (MergeMeshInto(filter2.sharedMesh, null, renderer4.sharedMaterials, vertices, normals, list4, list5, list6, list7, list8, boneWeights, bones, bindposes, subMeshes, ((filter2.transform.localScale.x * filter2.transform.localScale.y) * filter2.transform.localScale.z) < 0f, renderer4.lightmapScaleOffset, filter2.transform, obj3.transform, filter2.gameObject.name + "_" + filter2.sharedMesh.name, skipSubmeshNames))
                                    {
                                        renderer4.enabled = false;
                                    }
                                    num10++;
                                }
                                num11++;
                            }
                        }
                    }
                }
                else if ((componentsInChildren != null) && (componentsInChildren.Length > 0))
                {
                    int num15 = 0;
                    foreach (MeshRenderer renderer5 in componentsInChildren)
                    {
                        if (((lightmapIndex < 0) || (renderer5.lightmapIndex < 0)) || ((renderer5.lightmapIndex > 0xfd) || (lightmapIndex == renderer5.lightmapIndex)))
                        {
                            MeshFilter filter3 = renderer5.gameObject.GetComponent<MeshFilter>();
                            if ((filter3 != null) && (filter3.sharedMesh != null))
                            {
                                if ((num10 <= num15) && ((vertices.Count + filter3.sharedMesh.vertexCount) <= 0xfffe))
                                {
                                    if (MergeMeshInto(filter3.sharedMesh, null, renderer5.sharedMaterials, vertices, normals, list4, list5, list6, list7, list8, boneWeights, bones, bindposes, subMeshes, ((filter3.transform.localScale.x * filter3.transform.localScale.y) * filter3.transform.localScale.z) < 0f, renderer5.lightmapScaleOffset, filter3.transform, obj3.transform, filter3.gameObject.name + "_" + filter3.sharedMesh.name, skipSubmeshNames) && (filter3.gameObject != obj3))
                                    {
                                        filter3.gameObject.SetActive(false);
                                        Transform parent = filter3.gameObject.transform.parent;
                                        if ((parent != null) && (parent.gameObject != obj3))
                                        {
                                            parent.gameObject.SetActive(false);
                                        }
                                    }
                                    num10++;
                                }
                                num15++;
                            }
                        }
                    }
                }
                LODMaker.RemoveUnusedVertices(vertices, normals, list4, list5, list6, list7, list8, boneWeights, subMeshes);
                if (mesh == null)
                {
                    mesh = new Mesh();
                }
                mesh.vertices = vertices.ToArray();
                bool flag7 = false;
                if (normals.Count > 0)
                {
                    mesh.normals = normals.ToArray();
                }
                flag7 = false;
                for (int j = 0; j < list4.Count; j++)
                {
                    Vector2 vector10 = list4[j];
                    if (vector10.x == 0f)
                    {
                        Vector2 vector11 = list4[j];
                        if (vector11.y == 0f)
                        {
                            continue;
                        }
                    }
                    flag7 = true;
                    break;
                }
                if (flag7)
                {
                    mesh.uv = list4.ToArray();
                }
                flag7 = false;
                for (int k = 0; k < list5.Count; k++)
                {
                    Vector2 vector12 = list5[k];
                    if (vector12.x == 0f)
                    {
                        Vector2 vector13 = list5[k];
                        if (vector13.y == 0f)
                        {
                            continue;
                        }
                    }
                    flag7 = true;
                    break;
                }
                if (flag7)
                {
                    mesh.uv2 = list5.ToArray();
                }
                flag7 = false;
                for (int m = 0; m < list6.Count; m++)
                {
                    Vector2 vector14 = list6[m];
                    if (vector14.x == 0f)
                    {
                        Vector2 vector15 = list6[m];
                        if (vector15.y == 0f)
                        {
                            continue;
                        }
                    }
                    flag7 = true;
                    break;
                }
                if (flag7)
                {
                    mesh.uv3 = list6.ToArray();
                }
                flag7 = false;
                for (int n = 0; n < list7.Count; n++)
                {
                    Vector2 vector16 = list7[n];
                    if (vector16.x == 0f)
                    {
                        Vector2 vector17 = list7[n];
                        if (vector17.y == 0f)
                        {
                            continue;
                        }
                    }
                    flag7 = true;
                    break;
                }
                if (flag7)
                {
                    mesh.uv4 = list7.ToArray();
                }
                flag7 = false;
                for (int num21 = 0; num21 < list8.Count; num21++)
                {
                    Color32 color = list8[num21];
                    if (color.r <= 0)
                    {
                        Color32 color2 = list8[num21];
                        if (color2.g <= 0)
                        {
                            Color32 color3 = list8[num21];
                            if (color3.b <= 0)
                            {
                                continue;
                            }
                        }
                    }
                    flag7 = true;
                    break;
                }
                if (flag7)
                {
                    mesh.colors32 = list8.ToArray();
                }
                if (bindposes.Count > 0)
                {
                    mesh.bindposes = bindposes.ToArray();
                }
                if (boneWeights.Count > 0)
                {
                    if (boneWeights.Count == vertices.Count)
                    {
                        mesh.boneWeights = boneWeights.ToArray();
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarning("Nr of bone weights not equal to nr of vertices.");
                    }
                }
                mesh.subMeshCount = subMeshes.Keys.Count;
                Material[] materialArray = new Material[subMeshes.Keys.Count];
                int index = 0;
                foreach (Material material in subMeshes.Keys)
                {
                    materialArray[index] = material;
                    mesh.SetTriangles(subMeshes[material].ToArray(), index++);
                }
                if ((normals == null) || (normals.Count <= 0))
                {
                    mesh.RecalculateNormals();
                }
                MeshExtensions.RecalculateTangents(mesh);
                mesh.RecalculateBounds();
                if ((rendererArray2 != null) && (rendererArray2.Length > 0))
                {
                    SkinnedMeshRenderer renderer6 = obj3.GetComponent<SkinnedMeshRenderer>();
                    if (renderer6 == null)
                    {
                        renderer6 = obj3.AddComponent<SkinnedMeshRenderer>();
                    }
                    renderer6.quality = rendererArray2[0].quality;
                    renderer6.sharedMesh = mesh;
                    renderer6.sharedMaterials = materialArray;
                    renderer6.bones = bones.ToArray();
                }
                else if ((componentsInChildren != null) && (componentsInChildren.Length > 0))
                {
                    MeshRenderer renderer7 = obj3.GetComponent<MeshRenderer>();
                    if (renderer7 == null)
                    {
                        renderer7 = obj3.AddComponent<MeshRenderer>();
                    }
                    if ((lightmapIndex >= 0) && (lightmapIndex <= 0xfd))
                    {
                        renderer7.lightmapIndex = lightmapIndex;
                    }
                    renderer7.sharedMaterials = materialArray;
                    MeshFilter filter4 = obj3.GetComponent<MeshFilter>();
                    if (filter4 == null)
                    {
                        filter4 = obj3.AddComponent<MeshFilter>();
                    }
                    filter4.sharedMesh = mesh;
                }
                list.Add(mesh);
            }
            return list.ToArray();
        }

        [Extension]
        public static void CountChildrenWithName(GameObject go, string childName, ref int total)
        {
            if (go.name == childName)
            {
                total++;
            }
            IEnumerator enumerator = go.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    CountChildrenWithName(current.gameObject, childName, ref total);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
        }

        [Extension]
        public static void DestroyChildren(GameObject go, bool disabledOnly)
        {
            List<Transform> list = new List<Transform>();
            IEnumerator enumerator = go.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    if (!current.gameObject.activeSelf || !disabledOnly)
                    {
                        list.Add(current);
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            for (int i = list.Count - 1; i >= 0; i--)
            {
                list[i].SetParent(null);
                UnityEngine.Object.Destroy(list[i].gameObject);
            }
        }

        [Extension]
        public static GameObject FindFirstChildWithName(GameObject go, string childName)
        {
            if (go.name == childName)
            {
                return go;
            }
            IEnumerator enumerator = go.transform.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    GameObject obj2 = FindFirstChildWithName(current.gameObject, childName);
                    if (obj2 != null)
                    {
                        return obj2;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            return null;
        }

        [Extension]
        public static GameObject FindMutualParent(GameObject go1, GameObject go2)
        {
            if ((go2 == null) || (go1 == go2))
            {
                return null;
            }
            for (Transform transform = go2.transform; transform != null; transform = transform.parent)
            {
                if (go1 == transform.gameObject)
                {
                    return go1;
                }
            }
            Transform parent = go1.transform.parent;
            if (parent == null)
            {
                return null;
            }
            return FindMutualParent(parent.gameObject, go2);
        }

        [Extension]
        public static GameObject FindParentWithName(GameObject go, string parentName)
        {
            if (go.name == parentName)
            {
                return go;
            }
            Transform parent = go.transform.parent;
            if (parent == null)
            {
                return null;
            }
            return FindParentWithName(parent.gameObject, parentName);
        }

        [Extension]
        public static Mesh Get1stSharedMesh(GameObject aGo)
        {
            MeshFilter[] componentsInChildren = aGo.GetComponentsInChildren<MeshFilter>(false);
            for (int i = 0; (componentsInChildren != null) && (i < componentsInChildren.Length); i++)
            {
                if (componentsInChildren[i].sharedMesh != null)
                {
                    return componentsInChildren[i].sharedMesh;
                }
            }
            SkinnedMeshRenderer[] rendererArray = aGo.GetComponentsInChildren<SkinnedMeshRenderer>(false);
            for (int j = 0; (rendererArray != null) && (j < rendererArray.Length); j++)
            {
                if (rendererArray[j].sharedMesh != null)
                {
                    return rendererArray[j].sharedMesh;
                }
            }
            return null;
        }

        [Extension]
        public static Vector3[] GetBoundsCenterAndCorners(Bounds bounds)
        {
            Vector3[] vectorArray = new Vector3[9];
            vectorArray[0] = bounds.center;
            for (int i = 1; i < 9; i++)
            {
                vectorArray[i] = bounds.min;
                if ((i & 1) > 0)
                {
                    vectorArray[i].x += bounds.size.x;
                }
                if ((i & 2) > 0)
                {
                    vectorArray[i].y += bounds.size.y;
                }
                if ((i & 4) > 0)
                {
                    vectorArray[i].z += bounds.size.z;
                }
            }
            return vectorArray;
        }

        [Extension]
        public static Vector3[] GetBoundsCorners(Bounds bounds)
        {
            Vector3[] vectorArray = new Vector3[8];
            for (int i = 0; i < 8; i++)
            {
                vectorArray[i] = bounds.min;
                if ((i & 1) > 0)
                {
                    vectorArray[i].x += bounds.size.x;
                }
                if ((i & 2) > 0)
                {
                    vectorArray[i].y += bounds.size.y;
                }
                if ((i & 4) > 0)
                {
                    vectorArray[i].z += bounds.size.z;
                }
            }
            return vectorArray;
        }

        [Extension]
        public static T GetFirstComponentInChildren<T>(GameObject go) where T: Component
        {
            T[] componentsInChildren = go.GetComponentsInChildren<T>();
            if ((componentsInChildren != null) && (componentsInChildren.Length > 0))
            {
                return componentsInChildren[0];
            }
            return null;
        }

        [Extension]
        public static T GetFirstComponentInParents<T>(GameObject go) where T: Component
        {
            T component = go.GetComponent<T>();
            if (component != null)
            {
                return component;
            }
            if ((go.transform.parent != null) && (go.transform.parent.gameObject != go))
            {
                return GetFirstComponentInParents<T>(go.transform.parent.gameObject);
            }
            return null;
        }

        [Extension]
        public static Material[] GetMaterials(GameObject aGo, bool includeDisabled)
        {
            List<Material> list = new List<Material>();
            MeshRenderer[] componentsInChildren = aGo.GetComponentsInChildren<MeshRenderer>(includeDisabled);
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                list.AddRange(componentsInChildren[i].sharedMaterials);
            }
            SkinnedMeshRenderer[] rendererArray2 = aGo.GetComponentsInChildren<SkinnedMeshRenderer>(includeDisabled);
            for (int j = 0; j < rendererArray2.Length; j++)
            {
                list.AddRange(rendererArray2[j].sharedMaterials);
            }
            return list.ToArray();
        }

        [Extension]
        public static Mesh[] GetMeshes(GameObject aGo)
        {
            return GetMeshes(aGo, true);
        }

        [Extension]
        public static Mesh[] GetMeshes(GameObject aGo, bool includeDisabled)
        {
            MeshFilter[] componentsInChildren = aGo.GetComponentsInChildren<MeshFilter>(includeDisabled);
            SkinnedMeshRenderer[] rendererArray = aGo.GetComponentsInChildren<SkinnedMeshRenderer>(includeDisabled);
            int num = 0;
            if (componentsInChildren != null)
            {
                num += componentsInChildren.Length;
            }
            if (rendererArray != null)
            {
                num += rendererArray.Length;
            }
            if (num == 0)
            {
                return null;
            }
            Mesh[] meshArray = new Mesh[num];
            int index = 0;
            while ((componentsInChildren != null) && (index < componentsInChildren.Length))
            {
                meshArray[index] = componentsInChildren[index].sharedMesh;
                index++;
            }
            int num3 = index;
            for (index = 0; (rendererArray != null) && (index < rendererArray.Length); index++)
            {
                meshArray[index + num3] = rendererArray[index].sharedMesh;
            }
            return meshArray;
        }

        [Extension]
        public static float GetModelComplexity(GameObject go)
        {
            float num = 0f;
            foreach (MeshFilter filter in go.GetComponentsInChildren<MeshFilter>(true))
            {
                Mesh sharedMesh = filter.sharedMesh;
                float num3 = 1f;
                for (int i = 0; i < sharedMesh.subMeshCount; i++)
                {
                    num += ((num3 * sharedMesh.GetTriangles(i).Length) / 3f) / 65536f;
                    num3 *= 1.1f;
                }
            }
            return num;
        }

        [Extension]
        public static string GetModelInfoString(GameObject go)
        {
            string str = string.Empty;
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            Bounds worldBounds = GetWorldBounds(go);
            foreach (MeshFilter filter in go.GetComponentsInChildren<MeshFilter>(true))
            {
                Mesh sharedMesh = filter.sharedMesh;
                num++;
                num2 += sharedMesh.subMeshCount;
                num3 += sharedMesh.vertices.Length;
                num4 += sharedMesh.triangles.Length / 3;
            }
            return (((((str + num + " meshes\n") + num2 + " submeshes\n") + num3 + " vertices\n") + num4 + " triangles\n") + worldBounds.size + " meters");
        }

        [Extension]
        public static Mesh GetSimplifiedMesh(GameObject go, float maxWeight, bool recalcNormals, float removeSmallParts, [Optional, DefaultParameterValue(1f)] float protectNormals, [Optional, DefaultParameterValue(1f)] float protectUvs, [Optional, DefaultParameterValue(1f)] float protectSubMeshesAndSharpEdges, [Optional, DefaultParameterValue(1f)] float smallTrianglesFirst, [Optional, DefaultParameterValue(1)] int nrOfSteps)
        {
            Mesh sharedMesh = null;
            LODSwitcher component = go.GetComponent<LODSwitcher>();
            if (component != null)
            {
                component.ReleaseFixedLODLevel();
                component.SetLODLevel(0);
            }
            MeshFilter filter = null;
            SkinnedMeshRenderer renderer = go.GetComponent<SkinnedMeshRenderer>();
            if (maxWeight <= 0f)
            {
                throw new ApplicationException("MaxWeight should be more that 0 or else this operation will have no effect");
            }
            if (renderer != null)
            {
                sharedMesh = renderer.sharedMesh;
            }
            else
            {
                filter = go.GetComponent<MeshFilter>();
                if (filter != null)
                {
                    sharedMesh = filter.sharedMesh;
                }
            }
            if (sharedMesh == null)
            {
                throw new ApplicationException("No mesh found. Maybe you need to select a child object?");
            }
            Mesh orig = sharedMesh;
            if (nrOfSteps < 1)
            {
                nrOfSteps = 1;
            }
            for (int i = 0; i < nrOfSteps; i++)
            {
                orig = MeshExtensions.MakeLODMesh(orig, (i + 1) * (maxWeight / ((float) nrOfSteps)), recalcNormals, removeSmallParts, protectNormals, protectUvs, protectSubMeshesAndSharpEdges, smallTrianglesFirst);
            }
            if (renderer != null)
            {
                renderer.sharedMesh = orig;
                return orig;
            }
            if (filter != null)
            {
                filter.sharedMesh = orig;
            }
            return orig;
        }

        [Extension, DebuggerHidden]
        public static IEnumerator GetSimplifiedMeshInBackground(GameObject go, float maxWeight, bool recalcNormals, float removeSmallParts, Action<Mesh> result)
        {
            <GetSimplifiedMeshInBackground>c__Iterator1D iteratord = new <GetSimplifiedMeshInBackground>c__Iterator1D();
            iteratord.go = go;
            iteratord.maxWeight = maxWeight;
            iteratord.removeSmallParts = removeSmallParts;
            iteratord.result = result;
            iteratord.recalcNormals = recalcNormals;
            iteratord.<$>go = go;
            iteratord.<$>maxWeight = maxWeight;
            iteratord.<$>removeSmallParts = removeSmallParts;
            iteratord.<$>result = result;
            iteratord.<$>recalcNormals = recalcNormals;
            return iteratord;
        }

        [Extension]
        public static int GetTotalVertexCount(GameObject aGo)
        {
            MeshFilter[] componentsInChildren = aGo.GetComponentsInChildren<MeshFilter>(false);
            SkinnedMeshRenderer[] rendererArray = aGo.GetComponentsInChildren<SkinnedMeshRenderer>(false);
            int num = 0;
            for (int i = 0; (componentsInChildren != null) && (i < componentsInChildren.Length); i++)
            {
                Mesh sharedMesh = componentsInChildren[i].sharedMesh;
                if (sharedMesh != null)
                {
                    num += sharedMesh.vertexCount;
                }
            }
            for (int j = 0; (rendererArray != null) && (j < rendererArray.Length); j++)
            {
                Mesh mesh2 = rendererArray[j].sharedMesh;
                if (mesh2 != null)
                {
                    num += mesh2.vertexCount;
                }
            }
            return num;
        }

        [Extension]
        public static Bounds GetWorldBounds(GameObject go)
        {
            if (go.transform == null)
            {
                return new Bounds();
            }
            Bounds bounds = new Bounds(go.transform.position, Vector3.zero);
            Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>(true);
            if ((componentsInChildren != null) && (componentsInChildren.Length > 0))
            {
                bounds = componentsInChildren[0].bounds;
            }
            foreach (Renderer renderer in componentsInChildren)
            {
                Bounds bounds2 = renderer.bounds;
                bounds.Encapsulate(bounds2);
            }
            return bounds;
        }

        [Extension]
        public static Vector3[] GetWorldBoundsCenterAndCorners(GameObject go)
        {
            return GetBoundsCenterAndCorners(GetWorldBounds(go));
        }

        [Extension]
        public static Vector3[] GetWorldBoundsCorners(GameObject go)
        {
            return GetBoundsCorners(GetWorldBounds(go));
        }

        private static int GiveUniqueNameIfNeeded(GameObject aGo, GameObject topGO, int uniqueId)
        {
            if (!IsChildWithNameUnique(topGO, aGo.name))
            {
                aGo.name = aGo.name + "_simpleLod" + ++uniqueId;
            }
            return uniqueId;
        }

        [Extension]
        public static bool IsChildWithNameUnique(GameObject go, string childName)
        {
            int total = 0;
            CountChildrenWithName(go, childName, ref total);
            return (total <= 1);
        }

        private static bool MergeMeshInto(Mesh fromMesh, Transform[] fromBones, Material[] fromMaterials, List<Vector3> vertices, List<Vector3> normals, List<Vector2> uv1s, List<Vector2> uv2s, List<Vector2> uv3s, List<Vector2> uv4s, List<Color32> colors32, List<BoneWeight> boneWeights, List<Transform> bones, List<Matrix4x4> bindposes, Dictionary<Material, List<int>> subMeshes, bool usesNegativeScale, Vector4 lightmapScaleOffset, Transform fromTransform, Transform topTransform, string submeshName, string[] skipSubmeshNames)
        {
            if (fromMesh == null)
            {
                return false;
            }
            bool flag = true;
            int count = vertices.Count;
            Vector3[] collection = fromMesh.vertices;
            Vector3[] vectorArray2 = fromMesh.normals;
            Vector2[] uv = fromMesh.uv;
            Vector2[] vectorArray4 = fromMesh.uv2;
            Vector2[] vectorArray5 = fromMesh.uv3;
            Vector2[] vectorArray6 = fromMesh.uv4;
            Color32[] colorArray = fromMesh.colors32;
            BoneWeight[] weightArray = fromMesh.boneWeights;
            Matrix4x4[] matrixxArray = fromMesh.bindposes;
            List<int> list = new List<int>();
            Vector3 localPosition = fromTransform.localPosition;
            Quaternion localRotation = fromTransform.localRotation;
            Vector3 localScale = fromTransform.localScale;
            bool flag2 = false;
            if (fromBones != null)
            {
                fromTransform.localPosition = Vector3.zero;
                fromTransform.localRotation = Quaternion.identity;
                fromTransform.localScale = Vector3.one;
            }
            if ((fromBones == null) || (fromBones.Length == 0))
            {
                flag2 = true;
            }
            if (((fromBones == null) || (fromBones.Length == 0)) && ((bones != null) && (bones.Count > 0)))
            {
                fromBones = new Transform[] { fromTransform };
                Matrix4x4 matrixx = fromTransform.worldToLocalMatrix * topTransform.localToWorldMatrix;
                matrixxArray = new Matrix4x4[] { matrixx };
                weightArray = new BoneWeight[collection.Length];
                for (int k = 0; k < collection.Length; k++)
                {
                    weightArray[k].boneIndex0 = 0;
                    weightArray[k].weight0 = 1f;
                }
            }
            if (fromBones != null)
            {
                bool flag3 = false;
                for (int m = 0; m < fromBones.Length; m++)
                {
                    int num4 = 0;
                    list.Add(m);
                    while (num4 < bones.Count)
                    {
                        if (fromBones[m] == bones[num4])
                        {
                            list[m] = num4;
                            if (matrixxArray[m] != bindposes[num4])
                            {
                                flag3 = true;
                                if (fromBones[m] != null)
                                {
                                    UnityEngine.Debug.Log(fromTransform.gameObject.name + ": The bindpose of " + fromBones[m].gameObject.name + " is different, vertices will be moved to match the bindpose of the merged mesh");
                                }
                                else
                                {
                                    UnityEngine.Debug.LogError(fromTransform.gameObject.name + ": There is an error in the bonestructure. A bone could not be found.");
                                }
                            }
                        }
                        num4++;
                    }
                    if (num4 >= bones.Count)
                    {
                        list[m] = bones.Count;
                        bones.Add(fromBones[m]);
                        bindposes.Add(matrixxArray[m]);
                    }
                }
                if (flag3)
                {
                    for (int n = 0; n < collection.Length; n++)
                    {
                        Vector3 vector3 = collection[n];
                        BoneWeight weight = weightArray[n];
                        if (fromBones[weight.boneIndex0] != null)
                        {
                            vector3 = ApplyBindPose(collection[n], fromBones[weight.boneIndex0], matrixxArray[weight.boneIndex0], weight.weight0);
                            if (weight.weight1 > 0f)
                            {
                                vector3 += ApplyBindPose(collection[n], fromBones[weight.boneIndex1], matrixxArray[weight.boneIndex1], weight.weight1);
                            }
                            if (weight.weight2 > 0f)
                            {
                                vector3 += ApplyBindPose(collection[n], fromBones[weight.boneIndex2], matrixxArray[weight.boneIndex2], weight.weight2);
                            }
                            if (weight.weight3 > 0f)
                            {
                                vector3 += ApplyBindPose(collection[n], fromBones[weight.boneIndex3], matrixxArray[weight.boneIndex3], weight.weight3);
                            }
                            Vector3 vertex = vector3;
                            vector3 = UnApplyBindPose(vertex, bones[list[weight.boneIndex0]], bindposes[list[weight.boneIndex0]], weight.weight0);
                            if (weight.weight1 > 0f)
                            {
                                vector3 += UnApplyBindPose(vertex, bones[list[weight.boneIndex1]], bindposes[list[weight.boneIndex1]], weight.weight1);
                            }
                            if (weight.weight2 > 0f)
                            {
                                vector3 += UnApplyBindPose(vertex, bones[list[weight.boneIndex2]], bindposes[list[weight.boneIndex2]], weight.weight2);
                            }
                            if (weight.weight3 > 0f)
                            {
                                vector3 += UnApplyBindPose(vertex, bones[list[weight.boneIndex3]], bindposes[list[weight.boneIndex3]], weight.weight3);
                            }
                            collection[n] = vector3;
                        }
                    }
                }
            }
            if (((boneWeights != null) && (weightArray != null)) && (weightArray.Length > 0))
            {
                for (int num6 = 0; num6 < weightArray.Length; num6++)
                {
                    BoneWeight item = new BoneWeight();
                    item.boneIndex0 = list[weightArray[num6].boneIndex0];
                    item.boneIndex1 = list[weightArray[num6].boneIndex1];
                    item.boneIndex2 = list[weightArray[num6].boneIndex2];
                    item.boneIndex3 = list[weightArray[num6].boneIndex3];
                    item.weight0 = weightArray[num6].weight0;
                    item.weight1 = weightArray[num6].weight1;
                    item.weight2 = weightArray[num6].weight2;
                    item.weight3 = weightArray[num6].weight3;
                    boneWeights.Add(item);
                }
            }
            Matrix4x4 matrixx2 = topTransform.worldToLocalMatrix * fromTransform.localToWorldMatrix;
            if (flag2)
            {
                for (int num7 = 0; num7 < collection.Length; num7++)
                {
                    Vector3 v = collection[num7];
                    collection[num7] = matrixx2.MultiplyPoint3x4(v);
                }
            }
            vertices.AddRange(collection);
            Vector4 column = matrixx2.GetColumn(2);
            Quaternion quaternion2 = Quaternion.LookRotation((Vector3) column, (Vector3) matrixx2.GetColumn(1));
            if ((vectorArray2 != null) && (vectorArray2.Length > 0))
            {
                for (int num8 = 0; num8 < vectorArray2.Length; num8++)
                {
                    vectorArray2[num8] = (Vector3) (quaternion2 * vectorArray2[num8]);
                }
                normals.AddRange(vectorArray2);
            }
            if ((uv == null) || (uv.Length != collection.Length))
            {
                uv = new Vector2[collection.Length];
            }
            if ((uv != null) && (uv.Length > 0))
            {
                uv1s.AddRange(uv);
            }
            if ((vectorArray4 == null) || (vectorArray4.Length != collection.Length))
            {
                vectorArray4 = new Vector2[collection.Length];
            }
            for (int i = 0; (vectorArray4 != null) && (i < vectorArray4.Length); i++)
            {
                uv2s.Add(new Vector2(lightmapScaleOffset.z + (vectorArray4[i].x * lightmapScaleOffset.x), lightmapScaleOffset.w + (vectorArray4[i].y * lightmapScaleOffset.y)));
            }
            if ((vectorArray5 == null) || (vectorArray5.Length != collection.Length))
            {
                vectorArray5 = new Vector2[collection.Length];
            }
            if ((vectorArray5 != null) && (vectorArray5.Length > 0))
            {
                uv3s.AddRange(vectorArray5);
            }
            if ((vectorArray6 == null) || (vectorArray6.Length != collection.Length))
            {
                vectorArray6 = new Vector2[collection.Length];
            }
            if ((vectorArray6 != null) && (vectorArray6.Length > 0))
            {
                uv4s.AddRange(vectorArray5);
            }
            if ((colorArray == null) || (colorArray.Length != collection.Length))
            {
                colorArray = new Color32[collection.Length];
            }
            if ((colorArray != null) && (colorArray.Length > 0))
            {
                colors32.AddRange(colorArray);
            }
            int num10 = 0;
            for (int j = 0; j < fromMaterials.Length; j++)
            {
                if (j >= fromMesh.subMeshCount)
                {
                    continue;
                }
                string str = submeshName + "_" + j;
                int index = 0;
                while (index < skipSubmeshNames.Length)
                {
                    if (str == skipSubmeshNames[index])
                    {
                        break;
                    }
                    index++;
                }
                if (index >= skipSubmeshNames.Length)
                {
                    int[] triangles = fromMesh.GetTriangles(j);
                    if (triangles.Length > 0)
                    {
                        if ((fromMaterials[j] != null) && !subMeshes.ContainsKey(fromMaterials[j]))
                        {
                            subMeshes.Add(fromMaterials[j], new List<int>());
                        }
                        List<int> list2 = subMeshes[fromMaterials[j]];
                        for (int num13 = 0; num13 < triangles.Length; num13 += 3)
                        {
                            if (usesNegativeScale)
                            {
                                int num14 = triangles[num13 + 1];
                                int num15 = triangles[num13 + 2];
                                triangles[num13 + 1] = num15;
                                triangles[num13 + 2] = num14;
                                num10++;
                            }
                            triangles[num13] += count;
                            triangles[num13 + 1] += count;
                            triangles[num13 + 2] += count;
                        }
                        list2.AddRange(triangles);
                    }
                }
                else
                {
                    flag = false;
                }
            }
            if (fromBones != null)
            {
                fromTransform.localPosition = localPosition;
                fromTransform.localRotation = localRotation;
                fromTransform.localScale = localScale;
            }
            return flag;
        }

        [Extension]
        public static void SetMeshes(GameObject aGo, Mesh[] meshes)
        {
            SetMeshes(aGo, meshes, true, 0);
        }

        [Extension]
        public static void SetMeshes(GameObject aGo, Mesh[] meshes, int lodLevel)
        {
            SetMeshes(aGo, meshes, true, lodLevel);
        }

        [Extension]
        public static void SetMeshes(GameObject aGo, Mesh[] meshes, bool includeDisabled, int lodLevel)
        {
            MeshFilter[] componentsInChildren = aGo.GetComponentsInChildren<MeshFilter>(includeDisabled);
            SkinnedMeshRenderer[] rendererArray = aGo.GetComponentsInChildren<SkinnedMeshRenderer>(includeDisabled);
            int num = 0;
            if (componentsInChildren != null)
            {
                num += componentsInChildren.Length;
            }
            if (rendererArray != null)
            {
                num += rendererArray.Length;
            }
            if (num != 0)
            {
                int index = 0;
                while ((componentsInChildren != null) && (index < componentsInChildren.Length))
                {
                    LODSwitcher component = componentsInChildren[index].gameObject.GetComponent<LODSwitcher>();
                    if ((meshes != null) && (meshes.Length > index))
                    {
                        if (lodLevel == 0)
                        {
                            componentsInChildren[index].sharedMesh = meshes[index];
                        }
                        if ((component == null) && (lodLevel > 0))
                        {
                            component = componentsInChildren[index].gameObject.AddComponent<LODSwitcher>();
                            component.SetMesh(componentsInChildren[index].sharedMesh, 0);
                        }
                        if (component != null)
                        {
                            component.SetMesh(meshes[index], lodLevel);
                        }
                    }
                    else
                    {
                        if (component != null)
                        {
                            component.SetMesh(null, lodLevel);
                        }
                        if (lodLevel == 0)
                        {
                            componentsInChildren[index].sharedMesh = null;
                        }
                    }
                    index++;
                }
                int num3 = index;
                for (index = 0; (rendererArray != null) && (index < rendererArray.Length); index++)
                {
                    LODSwitcher switcher2 = rendererArray[index].gameObject.GetComponent<LODSwitcher>();
                    if ((meshes != null) && (meshes.Length > (index + num3)))
                    {
                        if (lodLevel == 0)
                        {
                            rendererArray[index].sharedMesh = meshes[index + num3];
                        }
                        if ((switcher2 == null) && (lodLevel > 0))
                        {
                            switcher2 = rendererArray[index].gameObject.AddComponent<LODSwitcher>();
                            switcher2.SetMesh(rendererArray[index].sharedMesh, 0);
                        }
                        if (switcher2 != null)
                        {
                            switcher2.SetMesh(meshes[index + num3], lodLevel);
                        }
                    }
                    else
                    {
                        if (switcher2 != null)
                        {
                            switcher2.SetMesh(null, lodLevel);
                        }
                        if (lodLevel == 0)
                        {
                            rendererArray[index].sharedMesh = null;
                        }
                    }
                }
            }
        }

        [Extension]
        public static void SetUpLODLevels(GameObject go)
        {
            SetUpLODLevels(go, 1f);
        }

        [Extension]
        public static void SetUpLODLevels(GameObject go, float maxWeight)
        {
            float[] lodScreenSizes = new float[] { 0.6f, 0.3f, 0.15f };
            float[] maxWeights = new float[] { maxWeight * 0.65f, maxWeight, maxWeight * 1.5f };
            SetUpLODLevels(go, lodScreenSizes, maxWeights);
        }

        [Extension]
        public static void SetUpLODLevels(GameObject go, float[] lodScreenSizes, float[] maxWeights)
        {
            MeshFilter[] componentsInChildren = go.GetComponentsInChildren<MeshFilter>(false);
            if ((componentsInChildren != null) && (componentsInChildren.Length != 0))
            {
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    SetUpLODLevelsWithLODSwitcher(componentsInChildren[i].gameObject, lodScreenSizes, maxWeights, true, 1f, 1f, 1f, 1f, 1f, 1);
                }
            }
        }

        [Extension]
        public static Mesh[] SetUpLODLevelsAndChildrenWithLODGroup(GameObject go, float[] relativeTransitionHeights, float[] maxWeights, bool recalcNormals, float removeSmallParts, [Optional, DefaultParameterValue(1f)] float protectNormals, [Optional, DefaultParameterValue(1f)] float protectUvs, [Optional, DefaultParameterValue(1f)] float protectSubMeshesAndSharpEdges, [Optional, DefaultParameterValue(1f)] float smallTrianglesFirst, [Optional, DefaultParameterValue(1)] int nrOfSteps)
        {
            GameObject obj2 = null;
            LODGroup group = null;
            if ((relativeTransitionHeights.Length < 0) || (relativeTransitionHeights.Length != maxWeights.Length))
            {
                throw new ApplicationException("relativeTransitionHeights and maxWeights arrays need to have equal length and be longer than 0. Example: SetUpLODLevelsWithLODGroup(go, new float[2] {0.6f, 0.4f}, new float[2] {1f, 1.75f})");
            }
            for (int i = 0; i < maxWeights.Length; i++)
            {
                if (maxWeights[i] <= 0f)
                {
                    throw new ApplicationException("MaxWeight should be more that 0 or else this operation will have no effect");
                }
            }
            obj2 = new GameObject(go.name + "_$LodGrp");
            obj2.transform.position = go.transform.position;
            obj2.transform.rotation = go.transform.rotation;
            obj2.transform.localScale = go.transform.localScale;
            obj2.transform.SetParent(go.transform.parent);
            if (group == null)
            {
                group = obj2.AddComponent<LODGroup>();
            }
            else
            {
                Transform transform = OrbCreationExtensions.TransformExtensions.FindFirstChildWhereNameContains(obj2.transform, go.name + "_$Lod:");
                int num2 = 0;
                while ((transform != null) && (num2++ < 10))
                {
                    transform.SetParent(null);
                    UnityEngine.Object.Destroy(transform.gameObject);
                    transform = OrbCreationExtensions.TransformExtensions.FindFirstChildWhereNameContains(obj2.transform, go.name + "_$Lod:");
                }
            }
            LOD[] lods = new LOD[maxWeights.Length + 1];
            lods[0] = new LOD(relativeTransitionHeights[0], go.GetComponentsInChildren<MeshRenderer>(false));
            List<Mesh> list = new List<Mesh>();
            Mesh[] meshes = GetMeshes(go, false);
            float num3 = 0f;
            for (int j = 1; j < lods.Length; j++)
            {
                Mesh[] meshArray2 = new Mesh[meshes.Length];
                for (int k = 0; k < meshes.Length; k++)
                {
                    Mesh orig = meshes[k];
                    if (nrOfSteps < 1)
                    {
                        nrOfSteps = 1;
                    }
                    for (int m = 0; m < nrOfSteps; m++)
                    {
                        float num7 = maxWeights[j - 1] - num3;
                        orig = MeshExtensions.MakeLODMesh(orig, ((m + 1) * (num7 / ((float) nrOfSteps))) + num3, recalcNormals, removeSmallParts, protectNormals, protectUvs, protectSubMeshesAndSharpEdges, smallTrianglesFirst);
                    }
                    num3 = maxWeights[j - 1];
                    meshArray2[k] = orig;
                    object[] objArray1 = new object[] { go.name, "_", k, "_LOD", j };
                    orig.name = string.Concat(objArray1);
                    list.Add(orig);
                }
                GameObject aGo = UnityEngine.Object.Instantiate<GameObject>(go);
                aGo.name = go.name + "_$Lod:" + j;
                aGo.transform.SetParent(obj2.transform);
                aGo.transform.localPosition = go.transform.localPosition;
                aGo.transform.localRotation = go.transform.localRotation;
                aGo.transform.localScale = go.transform.localScale;
                SetMeshes(aGo, meshArray2);
                float screenRelativeTransitionHeight = (j >= relativeTransitionHeights.Length) ? 0f : relativeTransitionHeights[j];
                lods[j] = new LOD(screenRelativeTransitionHeight, aGo.GetComponentsInChildren<MeshRenderer>(false));
                meshes = meshArray2;
            }
            go.SetActive(false);
            group.SetLODs(lods);
            group.RecalculateBounds();
            group.ForceLOD(-1);
            return list.ToArray();
        }

        [Extension]
        public static Mesh[] SetUpLODLevelsAndChildrenWithLODSwitcher(GameObject go, float[] lodScreenSizes, float[] maxWeights, bool recalcNormals, float removeSmallParts, [Optional, DefaultParameterValue(1f)] float protectNormals, [Optional, DefaultParameterValue(1f)] float protectUvs, [Optional, DefaultParameterValue(1f)] float protectSubMeshesAndSharpEdges, [Optional, DefaultParameterValue(1f)] float smallTrianglesFirst, [Optional, DefaultParameterValue(1)] int nrOfSteps)
        {
            Mesh sharedMesh = null;
            Material[] sharedMaterials = null;
            LODSwitcher component = go.GetComponent<LODSwitcher>();
            if (component != null)
            {
                component.ReleaseFixedLODLevel();
                component.SetLODLevel(0);
            }
            SkinnedMeshRenderer renderer = go.GetComponent<SkinnedMeshRenderer>();
            if (renderer != null)
            {
                sharedMesh = renderer.sharedMesh;
                sharedMaterials = renderer.sharedMaterials;
                renderer.enabled = false;
            }
            else
            {
                MeshFilter filter = go.GetComponent<MeshFilter>();
                if (filter != null)
                {
                    sharedMesh = filter.sharedMesh;
                }
                MeshRenderer renderer2 = go.GetComponent<MeshRenderer>();
                if (renderer2 == null)
                {
                    throw new ApplicationException("No MeshRenderer found");
                }
                sharedMaterials = renderer2.sharedMaterials;
                renderer2.enabled = false;
            }
            if (sharedMesh == null)
            {
                throw new ApplicationException("No mesh found in " + go.name + ". Maybe you need to select a child object?");
            }
            for (int i = 0; i < maxWeights.Length; i++)
            {
                if (maxWeights[i] <= 0f)
                {
                    throw new ApplicationException("MaxWeight should be more that 0 or else this operation will have no effect");
                }
            }
            Mesh[] meshArray = MeshExtensions.MakeLODMeshes(sharedMesh, maxWeights, recalcNormals, removeSmallParts, protectNormals, protectUvs, protectSubMeshesAndSharpEdges, smallTrianglesFirst, nrOfSteps);
            if (meshArray == null)
            {
                return null;
            }
            Mesh[] meshArray2 = new Mesh[meshArray.Length + 1];
            meshArray2[0] = sharedMesh;
            for (int j = 0; j < meshArray.Length; j++)
            {
                meshArray2[j + 1] = meshArray[j];
            }
            if (component == null)
            {
                component = go.AddComponent<LODSwitcher>();
            }
            component.lodScreenSizes = lodScreenSizes;
            GameObject[] objArray = new GameObject[meshArray2.Length];
            for (int k = 0; k < meshArray2.Length; k++)
            {
                Transform transform = OrbCreationExtensions.TransformExtensions.FindFirstChildWithName(go.transform, go.name + "_LOD" + k);
                if (transform != null)
                {
                    objArray[k] = transform.gameObject;
                    objArray[k].SetActive(true);
                }
                if (objArray[k] == null)
                {
                    objArray[k] = new GameObject(go.name + "_LOD" + k);
                    objArray[k].transform.SetParent(go.transform);
                    objArray[k].transform.localPosition = Vector3.zero;
                    objArray[k].transform.localRotation = Quaternion.identity;
                    objArray[k].transform.localScale = Vector3.one;
                }
                if (renderer != null)
                {
                    SkinnedMeshRenderer renderer3 = objArray[k].GetComponent<SkinnedMeshRenderer>();
                    if (renderer3 == null)
                    {
                        renderer3 = objArray[k].AddComponent<SkinnedMeshRenderer>();
                    }
                    renderer3.sharedMesh = meshArray2[k];
                    renderer3.sharedMaterials = sharedMaterials;
                }
                else
                {
                    MeshFilter filter2 = objArray[k].GetComponent<MeshFilter>();
                    if (filter2 == null)
                    {
                        filter2 = objArray[k].AddComponent<MeshFilter>();
                    }
                    filter2.sharedMesh = meshArray2[k];
                    MeshRenderer renderer4 = objArray[k].GetComponent<MeshRenderer>();
                    if (renderer4 == null)
                    {
                        renderer4 = objArray[k].AddComponent<MeshRenderer>();
                    }
                    renderer4.sharedMaterials = sharedMaterials;
                }
                objArray[k].SetActive(k == 0);
            }
            component.lodGameObjects = objArray;
            component.ComputeDimensions();
            component.enabled = true;
            return meshArray2;
        }

        [Extension]
        public static Mesh[] SetUpLODLevelsWithLODSwitcher(GameObject go, float[] lodScreenSizes, float[] maxWeights, bool recalcNormals, [Optional, DefaultParameterValue(1f)] float removeSmallParts, [Optional, DefaultParameterValue(1f)] float protectNormals, [Optional, DefaultParameterValue(1f)] float protectUvs, [Optional, DefaultParameterValue(1f)] float protectSubMeshesAndSharpEdges, [Optional, DefaultParameterValue(1f)] float smallTrianglesFirst, [Optional, DefaultParameterValue(1)] int nrOfSteps)
        {
            Mesh sharedMesh = null;
            LODSwitcher component = go.GetComponent<LODSwitcher>();
            if (component != null)
            {
                component.ReleaseFixedLODLevel();
                component.SetLODLevel(0);
            }
            SkinnedMeshRenderer renderer = go.GetComponent<SkinnedMeshRenderer>();
            if (renderer != null)
            {
                sharedMesh = renderer.sharedMesh;
            }
            else
            {
                MeshFilter filter = go.GetComponent<MeshFilter>();
                if (filter != null)
                {
                    sharedMesh = filter.sharedMesh;
                }
            }
            if (sharedMesh == null)
            {
                throw new ApplicationException("No mesh found in " + go.name + ". Maybe you need to select a child object?");
            }
            for (int i = 0; i < maxWeights.Length; i++)
            {
                if (maxWeights[i] <= 0f)
                {
                    throw new ApplicationException("MaxWeight should be more that 0 or else this operation will have no effect");
                }
            }
            Mesh[] array = MeshExtensions.MakeLODMeshes(sharedMesh, maxWeights, recalcNormals, removeSmallParts, protectNormals, protectUvs, protectSubMeshesAndSharpEdges, smallTrianglesFirst, nrOfSteps);
            if (array == null)
            {
                return null;
            }
            if (component == null)
            {
                component = go.AddComponent<LODSwitcher>();
            }
            Array.Resize<Mesh>(ref array, maxWeights.Length + 1);
            for (int j = maxWeights.Length; j > 0; j--)
            {
                array[j] = array[j - 1];
            }
            array[0] = sharedMesh;
            component.lodMeshes = array;
            component.lodScreenSizes = lodScreenSizes;
            component.ComputeDimensions();
            component.enabled = true;
            return array;
        }

        [Extension, DebuggerHidden]
        public static IEnumerator SetUpLODLevelsWithLODSwitcherInBackground(GameObject go, float[] lodScreenSizes, float[] maxWeights, bool recalcNormals, [Optional, DefaultParameterValue(1f)] float removeSmallParts, [Optional, DefaultParameterValue(1f)] float protectNormals, [Optional, DefaultParameterValue(1f)] float protectUvs, [Optional, DefaultParameterValue(1f)] float protectSubMeshesAndSharpEdges, [Optional, DefaultParameterValue(1f)] float smallTrianglesFirst)
        {
            <SetUpLODLevelsWithLODSwitcherInBackground>c__Iterator1C iteratorc = new <SetUpLODLevelsWithLODSwitcherInBackground>c__Iterator1C();
            iteratorc.go = go;
            iteratorc.maxWeights = maxWeights;
            iteratorc.removeSmallParts = removeSmallParts;
            iteratorc.recalcNormals = recalcNormals;
            iteratorc.lodScreenSizes = lodScreenSizes;
            iteratorc.<$>go = go;
            iteratorc.<$>maxWeights = maxWeights;
            iteratorc.<$>removeSmallParts = removeSmallParts;
            iteratorc.<$>recalcNormals = recalcNormals;
            iteratorc.<$>lodScreenSizes = lodScreenSizes;
            return iteratorc;
        }

        [Extension]
        public static GameObject TopParent(GameObject go)
        {
            Transform parent = go.transform.parent;
            if (parent == null)
            {
                return go;
            }
            return TopParent(parent.gameObject);
        }

        private static unsafe Vector3 UnApplyBindPose(Vector3 vertex, Transform bone, Matrix4x4 bindpose, float boneWeight)
        {
            Matrix4x4 matrixx2 = bone.localToWorldMatrix * bindpose;
            Matrix4x4 inverse = matrixx2.inverse;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    ref Matrix4x4 matrixxRef;
                    int num3;
                    int num4;
                    float num5 = matrixxRef[num3, num4];
                    (matrixxRef = (Matrix4x4) &inverse)[num3 = i, num4 = j] = num5 * boneWeight;
                }
            }
            return inverse.MultiplyPoint3x4(vertex);
        }

        [CompilerGenerated]
        private sealed class <GetSimplifiedMeshInBackground>c__Iterator1D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal GameObject <$>go;
            internal float <$>maxWeight;
            internal bool <$>recalcNormals;
            internal float <$>removeSmallParts;
            internal Action<Mesh> <$>result;
            internal Matrix4x4[] <bindposes>__13;
            internal BoneWeight[] <bws>__14;
            internal Color32[] <colors32>__11;
            internal Hashtable <lodInfo>__4;
            internal LODSwitcher <lodSwitcher>__1;
            internal Mesh <mesh>__0;
            internal Bounds <meshBounds>__19;
            internal MeshFilter <meshFilter>__3;
            internal Vector3[] <ns>__6;
            internal int <s>__16;
            internal SkinnedMeshRenderer <smr>__2;
            internal int[] <subMeshOffsets>__15;
            internal int[] <subTs>__17;
            internal int <t>__18;
            internal Thread <thread>__20;
            internal int[] <ts>__12;
            internal Vector2[] <uv1s>__7;
            internal Vector2[] <uv2s>__8;
            internal Vector2[] <uv3s>__9;
            internal Vector2[] <uv4s>__10;
            internal Vector3[] <vs>__5;
            internal GameObject go;
            internal float maxWeight;
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
                        this.<mesh>__0 = null;
                        this.<lodSwitcher>__1 = this.go.GetComponent<LODSwitcher>();
                        if (this.<lodSwitcher>__1 != null)
                        {
                            this.<lodSwitcher>__1.ReleaseFixedLODLevel();
                            this.<lodSwitcher>__1.SetLODLevel(0);
                        }
                        this.<smr>__2 = this.go.GetComponent<SkinnedMeshRenderer>();
                        if (this.maxWeight <= 0f)
                        {
                            throw new ApplicationException("MaxWeight should be more that 0 or else this operation will have no effect");
                        }
                        if (this.<smr>__2 != null)
                        {
                            this.<mesh>__0 = this.<smr>__2.sharedMesh;
                        }
                        else
                        {
                            this.<meshFilter>__3 = this.go.GetComponent<MeshFilter>();
                            if (this.<meshFilter>__3 != null)
                            {
                                this.<mesh>__0 = this.<meshFilter>__3.sharedMesh;
                            }
                        }
                        if (this.<mesh>__0 == null)
                        {
                            throw new ApplicationException("No mesh found. Maybe you need to select a child object?");
                        }
                        this.<lodInfo>__4 = new Hashtable();
                        this.<lodInfo>__4["maxWeight"] = this.maxWeight;
                        this.<lodInfo>__4["removeSmallParts"] = this.removeSmallParts;
                        this.<vs>__5 = this.<mesh>__0.vertices;
                        if (this.<vs>__5.Length <= 0)
                        {
                            throw new ApplicationException("Mesh was empty");
                        }
                        this.<ns>__6 = this.<mesh>__0.normals;
                        if (this.<ns>__6.Length == 0)
                        {
                            this.<mesh>__0.RecalculateNormals();
                            this.<ns>__6 = this.<mesh>__0.normals;
                        }
                        this.<uv1s>__7 = this.<mesh>__0.uv;
                        this.<uv2s>__8 = this.<mesh>__0.uv2;
                        this.<uv3s>__9 = this.<mesh>__0.uv3;
                        this.<uv4s>__10 = this.<mesh>__0.uv4;
                        this.<colors32>__11 = this.<mesh>__0.colors32;
                        this.<ts>__12 = this.<mesh>__0.triangles;
                        this.<bindposes>__13 = this.<mesh>__0.bindposes;
                        this.<bws>__14 = this.<mesh>__0.boneWeights;
                        this.<subMeshOffsets>__15 = new int[this.<mesh>__0.subMeshCount];
                        if (this.<mesh>__0.subMeshCount > 1)
                        {
                            this.<s>__16 = 0;
                            while (this.<s>__16 < this.<mesh>__0.subMeshCount)
                            {
                                this.<subTs>__17 = this.<mesh>__0.GetTriangles(this.<s>__16);
                                this.<t>__18 = 0;
                                while (this.<t>__18 < this.<subTs>__17.Length)
                                {
                                    this.<ts>__12[this.<subMeshOffsets>__15[this.<s>__16] + this.<t>__18] = this.<subTs>__17[this.<t>__18];
                                    this.<t>__18++;
                                }
                                if ((this.<s>__16 + 1) < this.<mesh>__0.subMeshCount)
                                {
                                    this.<subMeshOffsets>__15[this.<s>__16 + 1] = this.<subMeshOffsets>__15[this.<s>__16] + this.<t>__18;
                                }
                                this.<s>__16++;
                            }
                        }
                        this.<meshBounds>__19 = this.<mesh>__0.bounds;
                        this.<lodInfo>__4["vertices"] = this.<vs>__5;
                        this.<lodInfo>__4["normals"] = this.<ns>__6;
                        this.<lodInfo>__4["uv1s"] = this.<uv1s>__7;
                        this.<lodInfo>__4["uv2s"] = this.<uv2s>__8;
                        this.<lodInfo>__4["uv3s"] = this.<uv3s>__9;
                        this.<lodInfo>__4["uv4s"] = this.<uv4s>__10;
                        this.<lodInfo>__4["colors32"] = this.<colors32>__11;
                        this.<lodInfo>__4["triangles"] = this.<ts>__12;
                        this.<lodInfo>__4["bindposes"] = this.<bindposes>__13;
                        this.<lodInfo>__4["boneWeights"] = this.<bws>__14;
                        this.<lodInfo>__4["subMeshOffsets"] = this.<subMeshOffsets>__15;
                        this.<lodInfo>__4["meshBounds"] = this.<meshBounds>__19;
                        this.<thread>__20 = new Thread(new ParameterizedThreadStart(LODMaker.MakeLODMeshInBackground));
                        this.<thread>__20.Start(this.<lodInfo>__4);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_05AE;
                }
                while (!this.<lodInfo>__4.ContainsKey("ready"))
                {
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 1;
                    return true;
                }
                this.result(LODMaker.CreateNewMesh((Vector3[]) this.<lodInfo>__4["vertices"], (Vector3[]) this.<lodInfo>__4["normals"], (Vector2[]) this.<lodInfo>__4["uv1s"], (Vector2[]) this.<lodInfo>__4["uv2s"], (Vector2[]) this.<lodInfo>__4["uv3s"], (Vector2[]) this.<lodInfo>__4["uv4s"], (Color32[]) this.<lodInfo>__4["colors32"], (int[]) this.<lodInfo>__4["triangles"], (BoneWeight[]) this.<lodInfo>__4["boneWeights"], (Matrix4x4[]) this.<lodInfo>__4["bindposes"], (int[]) this.<lodInfo>__4["subMeshOffsets"], this.recalcNormals));
                this.$PC = -1;
            Label_05AE:
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

        [CompilerGenerated]
        private sealed class <SetUpLODLevelsWithLODSwitcherInBackground>c__Iterator1C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal GameObject <$>go;
            internal float[] <$>lodScreenSizes;
            internal float[] <$>maxWeights;
            internal bool <$>recalcNormals;
            internal float <$>removeSmallParts;
            internal Matrix4x4[] <bindposes>__17;
            internal BoneWeight[] <bws>__18;
            internal Color32[] <colors32>__15;
            internal int <i>__25;
            internal int <i>__4;
            internal int <i>__7;
            internal Hashtable <lodInfo>__8;
            internal Mesh[] <lodMeshes>__6;
            internal LODSwitcher <lodSwitcher>__1;
            internal Mesh <mesh>__0;
            internal Mesh <mesh0>__5;
            internal Bounds <meshBounds>__23;
            internal MeshFilter <meshFilter>__3;
            internal Vector3[] <ns>__10;
            internal int <s>__20;
            internal SkinnedMeshRenderer <smr>__2;
            internal int[] <subMeshOffsets>__19;
            internal int[] <subTs>__21;
            internal int <t>__22;
            internal Thread <thread>__24;
            internal int[] <ts>__16;
            internal Vector2[] <uv1s>__11;
            internal Vector2[] <uv2s>__12;
            internal Vector2[] <uv3s>__13;
            internal Vector2[] <uv4s>__14;
            internal Vector3[] <vs>__9;
            internal GameObject go;
            internal float[] lodScreenSizes;
            internal float[] maxWeights;
            internal bool recalcNormals;
            internal float removeSmallParts;

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
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_07DB;

                    case 1:
                        this.<mesh>__0 = null;
                        this.<lodSwitcher>__1 = this.go.GetComponent<LODSwitcher>();
                        if (this.<lodSwitcher>__1 != null)
                        {
                            this.<lodSwitcher>__1.ReleaseFixedLODLevel();
                            this.<lodSwitcher>__1.SetLODLevel(0);
                        }
                        this.<smr>__2 = this.go.GetComponent<SkinnedMeshRenderer>();
                        if (this.<smr>__2 != null)
                        {
                            this.<mesh>__0 = this.<smr>__2.sharedMesh;
                        }
                        else
                        {
                            this.<meshFilter>__3 = this.go.GetComponent<MeshFilter>();
                            if (this.<meshFilter>__3 != null)
                            {
                                this.<mesh>__0 = this.<meshFilter>__3.sharedMesh;
                            }
                        }
                        if (this.<mesh>__0 == null)
                        {
                            throw new ApplicationException("No mesh found in " + this.go.name + ". Maybe you need to select a child object?");
                        }
                        this.<i>__4 = 0;
                        while (this.<i>__4 < this.maxWeights.Length)
                        {
                            if (this.maxWeights[this.<i>__4] <= 0f)
                            {
                                throw new ApplicationException("MaxWeight should be more that 0 or else this operation will have no effect");
                            }
                            this.<i>__4++;
                        }
                        this.<mesh0>__5 = this.<mesh>__0;
                        this.<lodMeshes>__6 = new Mesh[this.maxWeights.Length];
                        this.<i>__7 = 0;
                        while (this.<i>__7 < this.maxWeights.Length)
                        {
                            this.$current = null;
                            this.$PC = 2;
                            goto Label_07DB;
                        Label_021B:
                            this.<ns>__10 = this.<mesh>__0.normals;
                            if (this.<ns>__10.Length == 0)
                            {
                                this.<mesh>__0.RecalculateNormals();
                                this.<ns>__10 = this.<mesh>__0.normals;
                            }
                            this.<uv1s>__11 = this.<mesh>__0.uv;
                            this.<uv2s>__12 = this.<mesh>__0.uv2;
                            this.<uv3s>__13 = this.<mesh>__0.uv3;
                            this.<uv4s>__14 = this.<mesh>__0.uv4;
                            this.<colors32>__15 = this.<mesh>__0.colors32;
                            this.<ts>__16 = this.<mesh>__0.triangles;
                            this.<bindposes>__17 = this.<mesh>__0.bindposes;
                            this.<bws>__18 = this.<mesh>__0.boneWeights;
                            this.<subMeshOffsets>__19 = new int[this.<mesh>__0.subMeshCount];
                            if (this.<mesh>__0.subMeshCount > 1)
                            {
                                this.<s>__20 = 0;
                                while (this.<s>__20 < this.<mesh>__0.subMeshCount)
                                {
                                    this.<subTs>__21 = this.<mesh>__0.GetTriangles(this.<s>__20);
                                    this.<t>__22 = 0;
                                    while (this.<t>__22 < this.<subTs>__21.Length)
                                    {
                                        this.<ts>__16[this.<subMeshOffsets>__19[this.<s>__20] + this.<t>__22] = this.<subTs>__21[this.<t>__22];
                                        this.<t>__22++;
                                    }
                                    if ((this.<s>__20 + 1) < this.<mesh>__0.subMeshCount)
                                    {
                                        this.<subMeshOffsets>__19[this.<s>__20 + 1] = this.<subMeshOffsets>__19[this.<s>__20] + this.<t>__22;
                                    }
                                    this.<s>__20++;
                                }
                            }
                            this.<meshBounds>__23 = this.<mesh>__0.bounds;
                            this.<lodInfo>__8["vertices"] = this.<vs>__9;
                            this.<lodInfo>__8["normals"] = this.<ns>__10;
                            this.<lodInfo>__8["uv1s"] = this.<uv1s>__11;
                            this.<lodInfo>__8["uv2s"] = this.<uv2s>__12;
                            this.<lodInfo>__8["uv3s"] = this.<uv3s>__13;
                            this.<lodInfo>__8["uv4s"] = this.<uv4s>__14;
                            this.<lodInfo>__8["colors32"] = this.<colors32>__15;
                            this.<lodInfo>__8["triangles"] = this.<ts>__16;
                            this.<lodInfo>__8["bindposes"] = this.<bindposes>__17;
                            this.<lodInfo>__8["boneWeights"] = this.<bws>__18;
                            this.<lodInfo>__8["subMeshOffsets"] = this.<subMeshOffsets>__19;
                            this.<lodInfo>__8["meshBounds"] = this.<meshBounds>__23;
                            this.<thread>__24 = new Thread(new ParameterizedThreadStart(LODMaker.MakeLODMeshInBackground));
                            this.<thread>__24.Start(this.<lodInfo>__8);
                        Label_0542:
                            while (!this.<lodInfo>__8.ContainsKey("ready"))
                            {
                                this.$current = new WaitForSeconds(0.2f);
                                this.$PC = 3;
                                goto Label_07DB;
                            }
                            this.<lodMeshes>__6[this.<i>__7] = LODMaker.CreateNewMesh((Vector3[]) this.<lodInfo>__8["vertices"], (Vector3[]) this.<lodInfo>__8["normals"], (Vector2[]) this.<lodInfo>__8["uv1s"], (Vector2[]) this.<lodInfo>__8["uv2s"], (Vector2[]) this.<lodInfo>__8["uv3s"], (Vector2[]) this.<lodInfo>__8["uv4s"], (Color32[]) this.<lodInfo>__8["colors32"], (int[]) this.<lodInfo>__8["triangles"], (BoneWeight[]) this.<lodInfo>__8["boneWeights"], (Matrix4x4[]) this.<lodInfo>__8["bindposes"], (int[]) this.<lodInfo>__8["subMeshOffsets"], this.recalcNormals);
                            this.<mesh>__0 = this.<lodMeshes>__6[this.<i>__7];
                            this.go.transform.parent.gameObject.BroadcastMessage("LOD" + (this.<i>__7 + 1) + "IsReady", this.go, SendMessageOptions.DontRequireReceiver);
                            this.<i>__7++;
                        }
                        this.$current = null;
                        this.$PC = 4;
                        goto Label_07DB;

                    case 2:
                        this.<lodInfo>__8 = new Hashtable();
                        this.<lodInfo>__8["maxWeight"] = this.maxWeights[this.<i>__7];
                        this.<lodInfo>__8["removeSmallParts"] = this.removeSmallParts;
                        this.<vs>__9 = this.<mesh>__0.vertices;
                        if (this.<vs>__9.Length <= 0)
                        {
                            throw new ApplicationException("Mesh was empty");
                        }
                        goto Label_021B;

                    case 3:
                        goto Label_0542;

                    case 4:
                        if (this.<lodMeshes>__6 != null)
                        {
                            if (this.<lodSwitcher>__1 == null)
                            {
                                this.<lodSwitcher>__1 = this.go.AddComponent<LODSwitcher>();
                            }
                            Array.Resize<Mesh>(ref this.<lodMeshes>__6, this.maxWeights.Length + 1);
                            this.<i>__25 = this.maxWeights.Length;
                            while (this.<i>__25 > 0)
                            {
                                this.<lodMeshes>__6[this.<i>__25] = this.<lodMeshes>__6[this.<i>__25 - 1];
                                this.<i>__25--;
                            }
                            this.<lodMeshes>__6[0] = this.<mesh0>__5;
                            this.<lodSwitcher>__1.lodMeshes = this.<lodMeshes>__6;
                            this.<lodSwitcher>__1.lodScreenSizes = this.lodScreenSizes;
                            this.<lodSwitcher>__1.ComputeDimensions();
                            this.<lodSwitcher>__1.enabled = true;
                        }
                        this.go.transform.parent.gameObject.BroadcastMessage("LODsAreReady", this.go, SendMessageOptions.DontRequireReceiver);
                        this.$PC = -1;
                        break;
                }
                return false;
            Label_07DB:
                return true;
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

