namespace Pathfinding.Examples
{
    using Pathfinding;
    using Pathfinding.Util;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_path_types_demo.php")]
    public class PathTypesDemo : MonoBehaviour
    {
        public DemoMode activeDemo;
        public RichAI[] agents;
        public float aimStrength;
        public Transform end;
        private FloodPath lastFlood;
        private Path lastPath;
        private List<GameObject> lastRender = new List<GameObject>();
        public Material lineMat;
        public float lineWidth;
        private Vector2 mouseDragStart;
        private float mouseDragStartTime;
        private List<Vector3> multipoints = new List<Vector3>();
        public Vector3 pathOffset;
        public int searchLength = 0x3e8;
        public int spread = 100;
        public Material squareMat;
        public Transform start;

        [DebuggerHidden]
        public IEnumerator CalculateConstantPath()
        {
            <CalculateConstantPath>c__Iterator12 iterator = new <CalculateConstantPath>c__Iterator12();
            iterator.<>f__this = this;
            return iterator;
        }

        private void ClearPrevious()
        {
            for (int i = 0; i < this.lastRender.Count; i++)
            {
                UnityEngine.Object.Destroy(this.lastRender[i]);
            }
            this.lastRender.Clear();
        }

        private void DemoPath()
        {
            Path p = null;
            if (this.activeDemo == DemoMode.ABPath)
            {
                p = ABPath.Construct(this.start.position, this.end.position, new OnPathDelegate(this.OnPathComplete));
                if ((this.agents != null) && (this.agents.Length > 0))
                {
                    List<Vector3> previousPoints = ListPool<Vector3>.Claim(this.agents.Length);
                    Vector3 zero = Vector3.zero;
                    for (int i = 0; i < this.agents.Length; i++)
                    {
                        previousPoints.Add(this.agents[i].transform.position);
                        zero += previousPoints[i];
                    }
                    zero = (Vector3) (zero / ((float) previousPoints.Count));
                    for (int j = 0; j < this.agents.Length; j++)
                    {
                        List<Vector3> list2;
                        int num4;
                        Vector3 vector2 = list2[num4];
                        (list2 = previousPoints)[num4 = j] = vector2 - zero;
                    }
                    PathUtilities.GetPointsAroundPoint(this.end.position, AstarPath.active.graphs[0] as IRaycastableGraph, previousPoints, 0f, 0.2f);
                    for (int k = 0; k < this.agents.Length; k++)
                    {
                        if (this.agents[k] != null)
                        {
                            this.agents[k].target.position = previousPoints[k];
                            this.agents[k].UpdatePath();
                        }
                    }
                }
            }
            else if (this.activeDemo == DemoMode.MultiTargetPath)
            {
                p = MultiTargetPath.Construct(this.multipoints.ToArray(), this.end.position, null, new OnPathDelegate(this.OnPathComplete));
            }
            else if (this.activeDemo == DemoMode.RandomPath)
            {
                RandomPath path3 = RandomPath.Construct(this.start.position, this.searchLength, new OnPathDelegate(this.OnPathComplete));
                path3.spread = this.spread;
                path3.aimStrength = this.aimStrength;
                path3.aim = this.end.position;
                p = path3;
            }
            else if (this.activeDemo == DemoMode.FleePath)
            {
                FleePath path4 = FleePath.Construct(this.start.position, this.end.position, this.searchLength, new OnPathDelegate(this.OnPathComplete));
                path4.aimStrength = this.aimStrength;
                path4.spread = this.spread;
                p = path4;
            }
            else if (this.activeDemo == DemoMode.ConstantPath)
            {
                base.StartCoroutine(this.CalculateConstantPath());
                p = null;
            }
            else if (this.activeDemo == DemoMode.FloodPath)
            {
                FloodPath path5 = FloodPath.Construct(this.end.position, null);
                this.lastFlood = path5;
                p = path5;
            }
            else if ((this.activeDemo == DemoMode.FloodPathTracer) && (this.lastFlood != null))
            {
                p = FloodPathTracer.Construct(this.end.position, this.lastFlood, new OnPathDelegate(this.OnPathComplete));
            }
            if (p != null)
            {
                AstarPath.StartPath(p, false);
                this.lastPath = p;
            }
        }

        private void OnApplicationQuit()
        {
            this.ClearPrevious();
            this.lastRender = null;
        }

        public void OnGUI()
        {
            GUILayout.BeginArea(new Rect(5f, 5f, 220f, (float) (Screen.height - 10)), string.Empty, "Box");
            switch (this.activeDemo)
            {
                case DemoMode.ABPath:
                    GUILayout.Label("Basic path. Finds a path from point A to point B.", new GUILayoutOption[0]);
                    break;

                case DemoMode.MultiTargetPath:
                    GUILayout.Label("Multi Target Path. Finds a path quickly from one point to many others in a single search.", new GUILayoutOption[0]);
                    break;

                case DemoMode.RandomPath:
                    GUILayout.Label("Randomized Path. Finds a path with a specified length in a random direction or biased towards some point when using a larger aim strenggth.", new GUILayoutOption[0]);
                    break;

                case DemoMode.FleePath:
                    GUILayout.Label("Flee Path. Tries to flee from a specified point. Remember to set Flee Strength!", new GUILayoutOption[0]);
                    break;

                case DemoMode.ConstantPath:
                    GUILayout.Label("Finds all nodes which it costs less than some value to reach.", new GUILayoutOption[0]);
                    break;

                case DemoMode.FloodPath:
                    GUILayout.Label("Searches the whole graph from a specific point. FloodPathTracer can then be used to quickly find a path to that point", new GUILayoutOption[0]);
                    break;

                case DemoMode.FloodPathTracer:
                    GUILayout.Label("Traces a path to where the FloodPath started. Compare the claculation times for this path with ABPath!\nGreat for TD games", new GUILayoutOption[0]);
                    break;
            }
            GUILayout.Space(5f);
            GUILayout.Label("Note that the paths are rendered without ANY post-processing applied, so they might look a bit edgy", new GUILayoutOption[0]);
            GUILayout.Space(5f);
            GUILayout.Label("Click anywhere to recalculate the path. Hold Alt to continuously recalculate the path while the mouse is pressed.", new GUILayoutOption[0]);
            if (((this.activeDemo == DemoMode.ConstantPath) || (this.activeDemo == DemoMode.RandomPath)) || (this.activeDemo == DemoMode.FleePath))
            {
                GUILayout.Label("Search Distance (" + this.searchLength + ")", new GUILayoutOption[0]);
                this.searchLength = Mathf.RoundToInt(GUILayout.HorizontalSlider((float) this.searchLength, 0f, 100000f, new GUILayoutOption[0]));
            }
            if ((this.activeDemo == DemoMode.RandomPath) || (this.activeDemo == DemoMode.FleePath))
            {
                GUILayout.Label("Spread (" + this.spread + ")", new GUILayoutOption[0]);
                this.spread = Mathf.RoundToInt(GUILayout.HorizontalSlider((float) this.spread, 0f, 40000f, new GUILayoutOption[0]));
                GUILayout.Label(string.Concat(new object[] { (this.activeDemo != DemoMode.RandomPath) ? "Flee strength" : "Aim strength", " (", this.aimStrength, ")" }), new GUILayoutOption[0]);
                this.aimStrength = GUILayout.HorizontalSlider(this.aimStrength, 0f, 1f, new GUILayoutOption[0]);
            }
            if (this.activeDemo == DemoMode.MultiTargetPath)
            {
                GUILayout.Label("Hold shift and click to add new target points. Hold ctr and click to remove all target points", new GUILayoutOption[0]);
            }
            if (GUILayout.Button("A to B path", new GUILayoutOption[0]))
            {
                this.activeDemo = DemoMode.ABPath;
            }
            if (GUILayout.Button("Multi Target Path", new GUILayoutOption[0]))
            {
                this.activeDemo = DemoMode.MultiTargetPath;
            }
            if (GUILayout.Button("Random Path", new GUILayoutOption[0]))
            {
                this.activeDemo = DemoMode.RandomPath;
            }
            if (GUILayout.Button("Flee path", new GUILayoutOption[0]))
            {
                this.activeDemo = DemoMode.FleePath;
            }
            if (GUILayout.Button("Constant Path", new GUILayoutOption[0]))
            {
                this.activeDemo = DemoMode.ConstantPath;
            }
            if (GUILayout.Button("Flood Path", new GUILayoutOption[0]))
            {
                this.activeDemo = DemoMode.FloodPath;
            }
            if (GUILayout.Button("Flood Path Tracer", new GUILayoutOption[0]))
            {
                this.activeDemo = DemoMode.FloodPathTracer;
            }
            GUILayout.EndArea();
        }

        public void OnPathComplete(Path p)
        {
            if (this.lastRender != null)
            {
                if (p.error)
                {
                    this.ClearPrevious();
                }
                else if (p.GetType() == typeof(MultiTargetPath))
                {
                    List<GameObject> list = new List<GameObject>(this.lastRender);
                    this.lastRender.Clear();
                    MultiTargetPath path = p as MultiTargetPath;
                    for (int i = 0; i < path.vectorPaths.Length; i++)
                    {
                        if (path.vectorPaths[i] != null)
                        {
                            List<Vector3> list2 = path.vectorPaths[i];
                            GameObject item = null;
                            if ((list.Count > i) && (list[i].GetComponent<LineRenderer>() != null))
                            {
                                item = list[i];
                                list.RemoveAt(i);
                            }
                            else
                            {
                                System.Type[] components = new System.Type[] { typeof(LineRenderer) };
                                item = new GameObject("LineRenderer_" + i, components);
                            }
                            LineRenderer component = item.GetComponent<LineRenderer>();
                            component.sharedMaterial = this.lineMat;
                            component.SetWidth(this.lineWidth, this.lineWidth);
                            component.SetVertexCount(list2.Count);
                            for (int k = 0; k < list2.Count; k++)
                            {
                                component.SetPosition(k, list2[k] + this.pathOffset);
                            }
                            this.lastRender.Add(item);
                        }
                    }
                    for (int j = 0; j < list.Count; j++)
                    {
                        UnityEngine.Object.Destroy(list[j]);
                    }
                }
                else if (p.GetType() == typeof(ConstantPath))
                {
                    this.ClearPrevious();
                    ConstantPath path2 = p as ConstantPath;
                    List<GraphNode> allNodes = path2.allNodes;
                    Mesh mesh = new Mesh();
                    List<Vector3> list4 = new List<Vector3>();
                    bool flag = false;
                    for (int m = allNodes.Count - 1; m >= 0; m--)
                    {
                        Vector3 start = ((Vector3) allNodes[m].position) + this.pathOffset;
                        if ((list4.Count == 0xfde8) && !flag)
                        {
                            UnityEngine.Debug.LogError("Too many nodes, rendering a mesh would throw 65K vertex error. Using Debug.DrawRay instead for the rest of the nodes");
                            flag = true;
                        }
                        if (flag)
                        {
                            UnityEngine.Debug.DrawRay(start, Vector3.up, Color.blue);
                        }
                        else
                        {
                            GridGraph graph = AstarData.GetGraph(allNodes[m]) as GridGraph;
                            float nodeSize = 1f;
                            if (graph != null)
                            {
                                nodeSize = graph.nodeSize;
                            }
                            list4.Add(start + ((Vector3) (new Vector3(-0.5f, 0f, -0.5f) * nodeSize)));
                            list4.Add(start + ((Vector3) (new Vector3(0.5f, 0f, -0.5f) * nodeSize)));
                            list4.Add(start + ((Vector3) (new Vector3(-0.5f, 0f, 0.5f) * nodeSize)));
                            list4.Add(start + ((Vector3) (new Vector3(0.5f, 0f, 0.5f) * nodeSize)));
                        }
                    }
                    Vector3[] vectorArray = list4.ToArray();
                    int[] numArray = new int[(3 * vectorArray.Length) / 2];
                    int num6 = 0;
                    int index = 0;
                    while (num6 < vectorArray.Length)
                    {
                        numArray[index] = num6;
                        numArray[index + 1] = num6 + 1;
                        numArray[index + 2] = num6 + 2;
                        numArray[index + 3] = num6 + 1;
                        numArray[index + 4] = num6 + 3;
                        numArray[index + 5] = num6 + 2;
                        index += 6;
                        num6 += 4;
                    }
                    Vector2[] vectorArray2 = new Vector2[vectorArray.Length];
                    for (int n = 0; n < vectorArray2.Length; n += 4)
                    {
                        vectorArray2[n] = new Vector2(0f, 0f);
                        vectorArray2[n + 1] = new Vector2(1f, 0f);
                        vectorArray2[n + 2] = new Vector2(0f, 1f);
                        vectorArray2[n + 3] = new Vector2(1f, 1f);
                    }
                    mesh.vertices = vectorArray;
                    mesh.triangles = numArray;
                    mesh.uv = vectorArray2;
                    mesh.RecalculateNormals();
                    System.Type[] typeArray2 = new System.Type[] { typeof(MeshRenderer), typeof(MeshFilter) };
                    GameObject obj3 = new GameObject("Mesh", typeArray2);
                    obj3.GetComponent<MeshFilter>().mesh = mesh;
                    obj3.GetComponent<MeshRenderer>().material = this.squareMat;
                    this.lastRender.Add(obj3);
                }
                else
                {
                    this.ClearPrevious();
                    System.Type[] typeArray3 = new System.Type[] { typeof(LineRenderer) };
                    GameObject obj4 = new GameObject("LineRenderer", typeArray3);
                    LineRenderer renderer3 = obj4.GetComponent<LineRenderer>();
                    renderer3.sharedMaterial = this.lineMat;
                    renderer3.SetWidth(this.lineWidth, this.lineWidth);
                    renderer3.SetVertexCount(p.vectorPath.Count);
                    for (int num9 = 0; num9 < p.vectorPath.Count; num9++)
                    {
                        renderer3.SetPosition(num9, p.vectorPath[num9] + this.pathOffset);
                    }
                    this.lastRender.Add(obj4);
                }
            }
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 item = ray.origin + ((Vector3) (ray.direction * (ray.origin.y / -ray.direction.y)));
            this.end.position = item;
            if (Input.GetMouseButtonDown(0))
            {
                this.mouseDragStart = Input.mousePosition;
                this.mouseDragStartTime = Time.realtimeSinceStartup;
            }
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 mousePosition = Input.mousePosition;
                Vector2 vector6 = mousePosition - this.mouseDragStart;
                if ((vector6.sqrMagnitude > 25f) && ((Time.realtimeSinceStartup - this.mouseDragStartTime) > 0.3f))
                {
                    float left = Mathf.Min(this.mouseDragStart.x, mousePosition.x);
                    float top = Mathf.Min(this.mouseDragStart.y, mousePosition.y);
                    float right = Mathf.Max(this.mouseDragStart.x, mousePosition.x);
                    Rect rect = Rect.MinMaxRect(left, top, right, Mathf.Max(this.mouseDragStart.y, mousePosition.y));
                    RichAI[] haiArray = UnityEngine.Object.FindObjectsOfType(typeof(RichAI)) as RichAI[];
                    List<RichAI> list = new List<RichAI>();
                    for (int i = 0; i < haiArray.Length; i++)
                    {
                        Vector2 point = Camera.main.WorldToScreenPoint(haiArray[i].transform.position);
                        if (rect.Contains(point))
                        {
                            list.Add(haiArray[i]);
                        }
                    }
                    this.agents = list.ToArray();
                }
                else
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        this.multipoints.Add(item);
                    }
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        this.multipoints.Clear();
                    }
                    if (Input.mousePosition.x > 225f)
                    {
                        this.DemoPath();
                    }
                }
            }
            if ((Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftAlt)) && ((this.lastPath == null) || this.lastPath.IsDone()))
            {
                this.DemoPath();
            }
        }

        [CompilerGenerated]
        private sealed class <CalculateConstantPath>c__Iterator12 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal PathTypesDemo <>f__this;
            internal ConstantPath <constPath>__0;

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
                        this.<constPath>__0 = ConstantPath.Construct(this.<>f__this.end.position, this.<>f__this.searchLength, new OnPathDelegate(this.<>f__this.OnPathComplete));
                        AstarPath.StartPath(this.<constPath>__0, false);
                        this.<>f__this.lastPath = this.<constPath>__0;
                        this.$current = this.<constPath>__0.WaitForPath();
                        this.$PC = 1;
                        return true;

                    case 1:
                        this.$PC = -1;
                        break;
                }
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

        public enum DemoMode
        {
            ABPath,
            MultiTargetPath,
            RandomPath,
            FleePath,
            ConstantPath,
            FloodPath,
            FloodPathTracer
        }
    }
}

