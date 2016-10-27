using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[HelpURL("http://arongranberg.com/astar/docs/class_procedural_grid_mover.php")]
public class ProceduralGridMover : MonoBehaviour
{
    [CompilerGenerated]
    private bool <updatingGraph>k__BackingField;
    public bool floodFill;
    private GridGraph graph;
    public Transform target;
    private GridNode[] tmp;
    public float updateDistance = 10f;

    private Vector3 PointToGraphSpace(Vector3 p)
    {
        return this.graph.inverseMatrix.MultiplyPoint(p);
    }

    public void Start()
    {
        if (AstarPath.active == null)
        {
            throw new Exception("There is no AstarPath object in the scene");
        }
        this.graph = AstarPath.active.astarData.gridGraph;
        if (this.graph == null)
        {
            throw new Exception("The AstarPath object has no GridGraph");
        }
        this.UpdateGraph();
    }

    private void Update()
    {
        Vector3 a = this.PointToGraphSpace(this.graph.center);
        Vector3 b = this.PointToGraphSpace(this.target.position);
        if (VectorMath.SqrDistanceXZ(a, b) > (this.updateDistance * this.updateDistance))
        {
            this.UpdateGraph();
        }
    }

    public void UpdateGraph()
    {
        <UpdateGraph>c__AnonStorey24E storeye = new <UpdateGraph>c__AnonStorey24E();
        storeye.<>f__this = this;
        if (!this.updatingGraph)
        {
            this.updatingGraph = true;
            storeye.ie = this.UpdateGraphCoroutine();
            AstarPath.active.AddWorkItem(new AstarWorkItem(new Func<IWorkItemContext, bool, bool>(storeye.<>m__1D)));
        }
    }

    [DebuggerHidden]
    private IEnumerator UpdateGraphCoroutine()
    {
        <UpdateGraphCoroutine>c__IteratorE re = new <UpdateGraphCoroutine>c__IteratorE();
        re.<>f__this = this;
        return re;
    }

    public bool updatingGraph
    {
        [CompilerGenerated]
        get
        {
            return this.<updatingGraph>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<updatingGraph>k__BackingField = value;
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateGraph>c__AnonStorey24E
    {
        internal ProceduralGridMover <>f__this;
        internal IEnumerator ie;

        internal bool <>m__1D(IWorkItemContext context, bool force)
        {
            if (this.<>f__this.floodFill)
            {
                context.QueueFloodFill();
            }
            if (force)
            {
                while (this.ie.MoveNext())
                {
                }
            }
            bool flag = !this.ie.MoveNext();
            if (flag)
            {
                this.<>f__this.updatingGraph = false;
            }
            return flag;
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateGraphCoroutine>c__IteratorE : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal ProceduralGridMover <>f__this;
        internal int <depth>__3;
        internal Vector3 <dir>__0;
        internal int <maxz>__15;
        internal int <minz>__14;
        internal GridNode <node>__12;
        internal GridNode[] <nodes>__4;
        internal Int2 <offset>__1;
        internal int <pz>__10;
        internal int <pz>__6;
        internal IntRect <r>__13;
        internal int <tmp2>__16;
        internal int <tmp2>__17;
        internal int <tz>__7;
        internal int <width>__2;
        internal int <x>__11;
        internal int <x>__19;
        internal int <x>__21;
        internal int <x>__23;
        internal int <x>__25;
        internal int <x>__27;
        internal int <x>__29;
        internal int <x>__31;
        internal int <x>__8;
        internal int <z>__18;
        internal int <z>__20;
        internal int <z>__22;
        internal int <z>__24;
        internal int <z>__26;
        internal int <z>__28;
        internal int <z>__30;
        internal int <z>__5;
        internal int <z>__9;

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
                    this.<dir>__0 = this.<>f__this.PointToGraphSpace(this.<>f__this.target.position) - this.<>f__this.PointToGraphSpace(this.<>f__this.graph.center);
                    this.<dir>__0.x = Mathf.Round(this.<dir>__0.x);
                    this.<dir>__0.z = Mathf.Round(this.<dir>__0.z);
                    this.<dir>__0.y = 0f;
                    if (!(this.<dir>__0 == Vector3.zero))
                    {
                        this.<offset>__1 = new Int2(-Mathf.RoundToInt(this.<dir>__0.x), -Mathf.RoundToInt(this.<dir>__0.z));
                        this.<>f__this.graph.center += this.<>f__this.graph.matrix.MultiplyVector(this.<dir>__0);
                        this.<>f__this.graph.GenerateMatrix();
                        if ((this.<>f__this.tmp == null) || (this.<>f__this.tmp.Length != this.<>f__this.graph.nodes.Length))
                        {
                            this.<>f__this.tmp = new GridNode[this.<>f__this.graph.nodes.Length];
                        }
                        this.<width>__2 = this.<>f__this.graph.width;
                        this.<depth>__3 = this.<>f__this.graph.depth;
                        this.<nodes>__4 = this.<>f__this.graph.nodes;
                        if ((Mathf.Abs(this.<offset>__1.x) > this.<width>__2) || (Mathf.Abs(this.<offset>__1.y) > this.<depth>__3))
                        {
                            this.<z>__28 = 0;
                            while (this.<z>__28 < this.<depth>__3)
                            {
                                this.<x>__29 = 0;
                                while (this.<x>__29 < this.<width>__2)
                                {
                                    this.<>f__this.graph.UpdateNodePositionCollision(this.<nodes>__4[(this.<z>__28 * this.<width>__2) + this.<x>__29], this.<x>__29, this.<z>__28, false);
                                    this.<x>__29++;
                                }
                                this.<z>__28++;
                            }
                            this.<z>__30 = 0;
                            while (this.<z>__30 < this.<depth>__3)
                            {
                                this.<x>__31 = 0;
                                while (this.<x>__31 < this.<width>__2)
                                {
                                    this.<>f__this.graph.CalculateConnections(this.<x>__31, this.<z>__30, this.<nodes>__4[(this.<z>__30 * this.<width>__2) + this.<x>__31]);
                                    this.<x>__31++;
                                }
                                this.<z>__30++;
                            }
                            break;
                        }
                        this.<z>__5 = 0;
                        while (this.<z>__5 < this.<depth>__3)
                        {
                            this.<pz>__6 = this.<z>__5 * this.<width>__2;
                            this.<tz>__7 = (((this.<z>__5 + this.<offset>__1.y) + this.<depth>__3) % this.<depth>__3) * this.<width>__2;
                            this.<x>__8 = 0;
                            while (this.<x>__8 < this.<width>__2)
                            {
                                this.<>f__this.tmp[this.<tz>__7 + (((this.<x>__8 + this.<offset>__1.x) + this.<width>__2) % this.<width>__2)] = this.<nodes>__4[this.<pz>__6 + this.<x>__8];
                                this.<x>__8++;
                            }
                            this.<z>__5++;
                        }
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_0A06;
                    }
                    goto Label_0A04;

                case 1:
                    this.<z>__9 = 0;
                    while (this.<z>__9 < this.<depth>__3)
                    {
                        this.<pz>__10 = this.<z>__9 * this.<width>__2;
                        this.<x>__11 = 0;
                        while (this.<x>__11 < this.<width>__2)
                        {
                            this.<node>__12 = this.<>f__this.tmp[this.<pz>__10 + this.<x>__11];
                            this.<node>__12.NodeInGridIndex = this.<pz>__10 + this.<x>__11;
                            this.<nodes>__4[this.<pz>__10 + this.<x>__11] = this.<node>__12;
                            this.<x>__11++;
                        }
                        this.<z>__9++;
                    }
                    this.<r>__13 = new IntRect(0, 0, this.<offset>__1.x, this.<offset>__1.y);
                    this.<minz>__14 = this.<r>__13.ymax;
                    this.<maxz>__15 = this.<depth>__3;
                    if (this.<r>__13.xmin > this.<r>__13.xmax)
                    {
                        this.<tmp2>__16 = this.<r>__13.xmax;
                        this.<r>__13.xmax = this.<width>__2 + this.<r>__13.xmin;
                        this.<r>__13.xmin = this.<width>__2 + this.<tmp2>__16;
                    }
                    if (this.<r>__13.ymin > this.<r>__13.ymax)
                    {
                        this.<tmp2>__17 = this.<r>__13.ymax;
                        this.<r>__13.ymax = this.<depth>__3 + this.<r>__13.ymin;
                        this.<r>__13.ymin = this.<depth>__3 + this.<tmp2>__17;
                        this.<minz>__14 = 0;
                        this.<maxz>__15 = this.<r>__13.ymin;
                    }
                    this.<r>__13 = this.<r>__13.Expand(this.<>f__this.graph.erodeIterations + 1);
                    this.<r>__13 = IntRect.Intersection(this.<r>__13, new IntRect(0, 0, this.<width>__2, this.<depth>__3));
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0A06;

                case 2:
                    this.<z>__18 = this.<r>__13.ymin;
                    while (this.<z>__18 < this.<r>__13.ymax)
                    {
                        this.<x>__19 = 0;
                        while (this.<x>__19 < this.<width>__2)
                        {
                            this.<>f__this.graph.UpdateNodePositionCollision(this.<nodes>__4[(this.<z>__18 * this.<width>__2) + this.<x>__19], this.<x>__19, this.<z>__18, false);
                            this.<x>__19++;
                        }
                        this.<z>__18++;
                    }
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0A06;

                case 3:
                    this.<z>__20 = this.<minz>__14;
                    while (this.<z>__20 < this.<maxz>__15)
                    {
                        this.<x>__21 = this.<r>__13.xmin;
                        while (this.<x>__21 < this.<r>__13.xmax)
                        {
                            this.<>f__this.graph.UpdateNodePositionCollision(this.<nodes>__4[(this.<z>__20 * this.<width>__2) + this.<x>__21], this.<x>__21, this.<z>__20, false);
                            this.<x>__21++;
                        }
                        this.<z>__20++;
                    }
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_0A06;

                case 4:
                    this.<z>__22 = this.<r>__13.ymin;
                    while (this.<z>__22 < this.<r>__13.ymax)
                    {
                        this.<x>__23 = 0;
                        while (this.<x>__23 < this.<width>__2)
                        {
                            this.<>f__this.graph.CalculateConnections(this.<x>__23, this.<z>__22, this.<nodes>__4[(this.<z>__22 * this.<width>__2) + this.<x>__23]);
                            this.<x>__23++;
                        }
                        this.<z>__22++;
                    }
                    this.$current = null;
                    this.$PC = 5;
                    goto Label_0A06;

                case 5:
                    this.<z>__24 = this.<minz>__14;
                    while (this.<z>__24 < this.<maxz>__15)
                    {
                        this.<x>__25 = this.<r>__13.xmin;
                        while (this.<x>__25 < this.<r>__13.xmax)
                        {
                            this.<>f__this.graph.CalculateConnections(this.<x>__25, this.<z>__24, this.<nodes>__4[(this.<z>__24 * this.<width>__2) + this.<x>__25]);
                            this.<x>__25++;
                        }
                        this.<z>__24++;
                    }
                    this.$current = null;
                    this.$PC = 6;
                    goto Label_0A06;

                case 6:
                    this.<z>__26 = 0;
                    while (this.<z>__26 < this.<depth>__3)
                    {
                        this.<x>__27 = 0;
                        while (this.<x>__27 < this.<width>__2)
                        {
                            if (((this.<x>__27 == 0) || (this.<z>__26 == 0)) || ((this.<x>__27 >= (this.<width>__2 - 1)) || (this.<z>__26 >= (this.<depth>__3 - 1))))
                            {
                                this.<>f__this.graph.CalculateConnections(this.<x>__27, this.<z>__26, this.<nodes>__4[(this.<z>__26 * this.<width>__2) + this.<x>__27]);
                            }
                            this.<x>__27++;
                        }
                        this.<z>__26++;
                    }
                    break;

                case 7:
                    this.$PC = -1;
                    goto Label_0A04;

                default:
                    goto Label_0A04;
            }
            this.$current = null;
            this.$PC = 7;
            goto Label_0A06;
        Label_0A04:
            return false;
        Label_0A06:
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

