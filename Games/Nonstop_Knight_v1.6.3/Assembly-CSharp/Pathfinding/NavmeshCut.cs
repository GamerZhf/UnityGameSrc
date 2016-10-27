namespace Pathfinding
{
    using Pathfinding.ClipperLib;
    using Pathfinding.Util;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [AddComponentMenu("Pathfinding/Navmesh/Navmesh Cut"), HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_navmesh_cut.php")]
    public class NavmeshCut : MonoBehaviour
    {
        private static List<NavmeshCut> allCuts = new List<NavmeshCut>();
        public Vector3 center;
        public float circleRadius = 1f;
        public int circleResolution = 6;
        private Vector3[][] contours;
        public bool cutsAddedGeom = true;
        private static readonly Dictionary<Int2, int> edges = new Dictionary<Int2, int>();
        public static readonly Color GizmoColor = new Color(0.145098f, 0.7215686f, 0.9372549f);
        public float height = 1f;
        [Tooltip("Only makes a split in the navmesh, but does not remove the geometry to make a hole")]
        public bool isDual;
        private Bounds lastBounds;
        private Mesh lastMesh;
        private Vector3 lastPosition;
        private Quaternion lastRotation;
        [Tooltip("The contour(s) of the mesh will be extracted. This mesh should only be a 2D surface, not a volume (see documentation).")]
        public Mesh mesh;
        [Tooltip("Scale of the custom mesh")]
        public float meshScale = 1f;
        private static readonly Dictionary<int, int> pointers = new Dictionary<int, int>();
        public Vector2 rectangleSize = new Vector2(1f, 1f);
        protected Transform tr;
        [Tooltip("Shape of the cut")]
        public MeshType type;
        [Tooltip("Distance between positions to require an update of the navmesh\nA smaller distance gives better accuracy, but requires more updates when moving the object over time, so it is often slower.")]
        public float updateDistance = 0.4f;
        [Tooltip("How many degrees rotation that is required for an update to the navmesh. Should be between 0 and 180.")]
        public float updateRotationDistance = 10f;
        [Tooltip("Includes rotation in calculations. This is slower since a lot more matrix multiplications are needed but gives more flexibility.")]
        public bool useRotation;
        private bool wasEnabled;

        public static  event Action<NavmeshCut> OnDestroyCallback;

        private static void AddCut(NavmeshCut obj)
        {
            allCuts.Add(obj);
        }

        public void Awake()
        {
            this.tr = base.transform;
            AddCut(this);
        }

        private void CalculateMeshContour()
        {
            if (this.mesh != null)
            {
                edges.Clear();
                pointers.Clear();
                Vector3[] vertices = this.mesh.vertices;
                int[] triangles = this.mesh.triangles;
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    if (VectorMath.IsClockwiseXZ(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]]))
                    {
                        int num2 = triangles[i];
                        triangles[i] = triangles[i + 2];
                        triangles[i + 2] = num2;
                    }
                    edges[new Int2(triangles[i], triangles[i + 1])] = i;
                    edges[new Int2(triangles[i + 1], triangles[i + 2])] = i;
                    edges[new Int2(triangles[i + 2], triangles[i])] = i;
                }
                for (int j = 0; j < triangles.Length; j += 3)
                {
                    for (int m = 0; m < 3; m++)
                    {
                        if (!edges.ContainsKey(new Int2(triangles[j + ((m + 1) % 3)], triangles[j + (m % 3)])))
                        {
                            pointers[triangles[j + (m % 3)]] = triangles[j + ((m + 1) % 3)];
                        }
                    }
                }
                List<Vector3[]> list = new List<Vector3[]>();
                List<Vector3> list2 = ListPool<Vector3>.Claim();
                for (int k = 0; k < vertices.Length; k++)
                {
                    if (!pointers.ContainsKey(k))
                    {
                        continue;
                    }
                    list2.Clear();
                    int index = k;
                    do
                    {
                        int num7 = pointers[index];
                        if (num7 == -1)
                        {
                            break;
                        }
                        pointers[index] = -1;
                        list2.Add(vertices[index]);
                        switch (num7)
                        {
                            case -1:
                                Debug.LogError("Invalid Mesh '" + this.mesh.name + " in " + base.gameObject.name);
                                break;
                        }
                    }
                    while (index != k);
                    if (list2.Count > 0)
                    {
                        list.Add(list2.ToArray());
                    }
                }
                ListPool<Vector3>.Release(list2);
                this.contours = list.ToArray();
            }
        }

        public void ForceUpdate()
        {
            this.lastPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
        }

        public static List<NavmeshCut> GetAll()
        {
            return allCuts;
        }

        public static List<NavmeshCut> GetAllInRange(Bounds b)
        {
            List<NavmeshCut> list = ListPool<NavmeshCut>.Claim();
            for (int i = 0; i < allCuts.Count; i++)
            {
                if (allCuts[i].enabled && Intersects(b, allCuts[i].GetBounds()))
                {
                    list.Add(allCuts[i]);
                }
            }
            return list;
        }

        public Bounds GetBounds()
        {
            Bounds bounds;
            switch (this.type)
            {
                case MeshType.Rectangle:
                {
                    if (!this.useRotation)
                    {
                        return new Bounds(this.tr.position + this.center, new Vector3(this.rectangleSize.x, this.height, this.rectangleSize.y));
                    }
                    Matrix4x4 localToWorldMatrix = this.tr.localToWorldMatrix;
                    bounds = new Bounds(localToWorldMatrix.MultiplyPoint3x4(this.center + ((Vector3) (new Vector3(-this.rectangleSize.x, -this.height, -this.rectangleSize.y) * 0.5f))), Vector3.zero);
                    bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + ((Vector3) (new Vector3(this.rectangleSize.x, -this.height, -this.rectangleSize.y) * 0.5f))));
                    bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + ((Vector3) (new Vector3(this.rectangleSize.x, -this.height, this.rectangleSize.y) * 0.5f))));
                    bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + ((Vector3) (new Vector3(-this.rectangleSize.x, -this.height, this.rectangleSize.y) * 0.5f))));
                    bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + ((Vector3) (new Vector3(-this.rectangleSize.x, this.height, -this.rectangleSize.y) * 0.5f))));
                    bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + ((Vector3) (new Vector3(this.rectangleSize.x, this.height, -this.rectangleSize.y) * 0.5f))));
                    bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + ((Vector3) (new Vector3(this.rectangleSize.x, this.height, this.rectangleSize.y) * 0.5f))));
                    bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + ((Vector3) (new Vector3(-this.rectangleSize.x, this.height, this.rectangleSize.y) * 0.5f))));
                    return bounds;
                }
                case MeshType.Circle:
                {
                    if (!this.useRotation)
                    {
                        return new Bounds(base.transform.position + this.center, new Vector3(this.circleRadius * 2f, this.height, this.circleRadius * 2f));
                    }
                    Matrix4x4 matrixx2 = this.tr.localToWorldMatrix;
                    return new Bounds(matrixx2.MultiplyPoint3x4(this.center), new Vector3(this.circleRadius * 2f, this.height, this.circleRadius * 2f));
                }
                case MeshType.CustomMesh:
                    if (this.mesh != null)
                    {
                        Bounds bounds2 = this.mesh.bounds;
                        if (this.useRotation)
                        {
                            Matrix4x4 matrixx3 = this.tr.localToWorldMatrix;
                            bounds2.center = (Vector3) (bounds2.center * this.meshScale);
                            bounds2.size = (Vector3) (bounds2.size * this.meshScale);
                            bounds = new Bounds(matrixx3.MultiplyPoint3x4(this.center + bounds2.center), Vector3.zero);
                            Vector3 max = bounds2.max;
                            Vector3 min = bounds2.min;
                            bounds.Encapsulate(matrixx3.MultiplyPoint3x4(this.center + new Vector3(max.x, max.y, max.z)));
                            bounds.Encapsulate(matrixx3.MultiplyPoint3x4(this.center + new Vector3(min.x, max.y, max.z)));
                            bounds.Encapsulate(matrixx3.MultiplyPoint3x4(this.center + new Vector3(min.x, max.y, min.z)));
                            bounds.Encapsulate(matrixx3.MultiplyPoint3x4(this.center + new Vector3(max.x, max.y, min.z)));
                            bounds.Encapsulate(matrixx3.MultiplyPoint3x4(this.center + new Vector3(max.x, min.y, max.z)));
                            bounds.Encapsulate(matrixx3.MultiplyPoint3x4(this.center + new Vector3(min.x, min.y, max.z)));
                            bounds.Encapsulate(matrixx3.MultiplyPoint3x4(this.center + new Vector3(min.x, min.y, min.z)));
                            bounds.Encapsulate(matrixx3.MultiplyPoint3x4(this.center + new Vector3(max.x, min.y, min.z)));
                            Vector3 vector3 = bounds.size;
                            vector3.y = Mathf.Max(vector3.y, this.height * this.tr.lossyScale.y);
                            bounds.size = vector3;
                            return bounds;
                        }
                        Vector3 size = (Vector3) (bounds2.size * this.meshScale);
                        size.y = Mathf.Max(size.y, this.height);
                        return new Bounds((base.transform.position + this.center) + ((Vector3) (bounds2.center * this.meshScale)), size);
                    }
                    return new Bounds();
            }
            throw new Exception("Invalid mesh type");
        }

        public void GetContour(List<List<IntPoint>> buffer)
        {
            List<IntPoint> list;
            if (this.circleResolution < 3)
            {
                this.circleResolution = 3;
            }
            Vector3 position = this.tr.position;
            Matrix4x4 identity = Matrix4x4.identity;
            bool flag = false;
            if (this.useRotation)
            {
                identity = this.tr.localToWorldMatrix;
                flag = VectorMath.ReversesFaceOrientationsXZ(identity);
            }
            switch (this.type)
            {
                case MeshType.Rectangle:
                    list = ListPool<IntPoint>.Claim();
                    flag ^= (this.rectangleSize.x < 0f) ^ (this.rectangleSize.y < 0f);
                    if (!this.useRotation)
                    {
                        position += this.center;
                        list.Add(V3ToIntPoint(position + ((Vector3) (new Vector3(-this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f))));
                        list.Add(V3ToIntPoint(position + ((Vector3) (new Vector3(this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f))));
                        list.Add(V3ToIntPoint(position + ((Vector3) (new Vector3(this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f))));
                        list.Add(V3ToIntPoint(position + ((Vector3) (new Vector3(-this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f))));
                        break;
                    }
                    list.Add(V3ToIntPoint(identity.MultiplyPoint3x4(this.center + ((Vector3) (new Vector3(-this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f)))));
                    list.Add(V3ToIntPoint(identity.MultiplyPoint3x4(this.center + ((Vector3) (new Vector3(this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f)))));
                    list.Add(V3ToIntPoint(identity.MultiplyPoint3x4(this.center + ((Vector3) (new Vector3(this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f)))));
                    list.Add(V3ToIntPoint(identity.MultiplyPoint3x4(this.center + ((Vector3) (new Vector3(-this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f)))));
                    break;

                case MeshType.Circle:
                    list = ListPool<IntPoint>.Claim(this.circleResolution);
                    flag ^= this.circleRadius < 0f;
                    if (!this.useRotation)
                    {
                        position += this.center;
                        for (int i = 0; i < this.circleResolution; i++)
                        {
                            list.Add(V3ToIntPoint(position + ((Vector3) (new Vector3(Mathf.Cos(((i * 2) * 3.141593f) / ((float) this.circleResolution)), 0f, Mathf.Sin(((i * 2) * 3.141593f) / ((float) this.circleResolution))) * this.circleRadius))));
                        }
                    }
                    else
                    {
                        for (int j = 0; j < this.circleResolution; j++)
                        {
                            list.Add(V3ToIntPoint(identity.MultiplyPoint3x4(this.center + ((Vector3) (new Vector3(Mathf.Cos(((j * 2) * 3.141593f) / ((float) this.circleResolution)), 0f, Mathf.Sin(((j * 2) * 3.141593f) / ((float) this.circleResolution))) * this.circleRadius)))));
                        }
                    }
                    if (flag)
                    {
                        list.Reverse();
                    }
                    buffer.Add(list);
                    return;

                case MeshType.CustomMesh:
                    if ((this.mesh != this.lastMesh) || (this.contours == null))
                    {
                        this.CalculateMeshContour();
                        this.lastMesh = this.mesh;
                    }
                    if (this.contours != null)
                    {
                        position += this.center;
                        flag ^= this.meshScale < 0f;
                        for (int k = 0; k < this.contours.Length; k++)
                        {
                            Vector3[] vectorArray = this.contours[k];
                            list = ListPool<IntPoint>.Claim(vectorArray.Length);
                            if (this.useRotation)
                            {
                                for (int m = 0; m < vectorArray.Length; m++)
                                {
                                    list.Add(V3ToIntPoint(identity.MultiplyPoint3x4(this.center + ((Vector3) (vectorArray[m] * this.meshScale)))));
                                }
                            }
                            else
                            {
                                for (int n = 0; n < vectorArray.Length; n++)
                                {
                                    list.Add(V3ToIntPoint(position + ((Vector3) (vectorArray[n] * this.meshScale))));
                                }
                            }
                            if (flag)
                            {
                                list.Reverse();
                            }
                            buffer.Add(list);
                        }
                    }
                    return;

                default:
                    return;
            }
            if (flag)
            {
                list.Reverse();
            }
            buffer.Add(list);
        }

        private static bool Intersects(Bounds b1, Bounds b2)
        {
            Vector3 min = b1.min;
            Vector3 max = b1.max;
            Vector3 vector3 = b2.min;
            Vector3 vector4 = b2.max;
            return (((((min.x <= vector4.x) && (max.x >= vector3.x)) && ((min.y <= vector4.y) && (max.y >= vector3.y))) && (min.z <= vector4.z)) && (max.z >= vector3.z));
        }

        public static Vector3 IntPointToV3(IntPoint p)
        {
            Int3 num = new Int3((int) p.X, 0, (int) p.Y);
            return (Vector3) num;
        }

        public void NotifyUpdated()
        {
            this.wasEnabled = base.enabled;
            if (this.wasEnabled)
            {
                this.lastPosition = this.tr.position;
                this.lastBounds = this.GetBounds();
                if (this.useRotation)
                {
                    this.lastRotation = this.tr.rotation;
                }
            }
        }

        public void OnDestroy()
        {
            if (OnDestroyCallback != null)
            {
                OnDestroyCallback(this);
            }
            RemoveCut(this);
        }

        public void OnDrawGizmos()
        {
            if (this.tr == null)
            {
                this.tr = base.transform;
            }
            List<List<IntPoint>> buffer = ListPool<List<IntPoint>>.Claim();
            this.GetContour(buffer);
            Gizmos.color = GizmoColor;
            Bounds bounds = this.GetBounds();
            float y = bounds.min.y;
            Vector3 vector = (Vector3) (Vector3.up * (bounds.max.y - y));
            for (int i = 0; i < buffer.Count; i++)
            {
                List<IntPoint> list2 = buffer[i];
                for (int j = 0; j < list2.Count; j++)
                {
                    Vector3 from = IntPointToV3(list2[j]);
                    from.y = y;
                    Vector3 to = IntPointToV3(list2[(j + 1) % list2.Count]);
                    to.y = y;
                    Gizmos.DrawLine(from, to);
                    Gizmos.DrawLine(from + vector, to + vector);
                    Gizmos.DrawLine(from, from + vector);
                    Gizmos.DrawLine(to, to + vector);
                }
            }
            ListPool<List<IntPoint>>.Release(buffer);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.Lerp(GizmoColor, new Color(1f, 1f, 1f, 0.2f), 0.9f);
            Bounds bounds = this.GetBounds();
            Gizmos.DrawCube(bounds.center, bounds.size);
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }

        public void OnEnable()
        {
            this.lastPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            this.lastRotation = this.tr.rotation;
        }

        private static void RemoveCut(NavmeshCut obj)
        {
            allCuts.Remove(obj);
        }

        public bool RequiresUpdate()
        {
            return ((this.wasEnabled != base.enabled) || (this.wasEnabled && (((this.tr.position - this.lastPosition).sqrMagnitude > (this.updateDistance * this.updateDistance)) || (this.useRotation && (Quaternion.Angle(this.lastRotation, this.tr.rotation) > this.updateRotationDistance)))));
        }

        public virtual void UsedForCut()
        {
        }

        public static IntPoint V3ToIntPoint(Vector3 p)
        {
            Int3 num = (Int3) p;
            return new IntPoint((long) num.x, (long) num.z);
        }

        public Bounds LastBounds
        {
            get
            {
                return this.lastBounds;
            }
        }

        public enum MeshType
        {
            Rectangle,
            Circle,
            CustomMesh
        }
    }
}

