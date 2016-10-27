namespace Pathfinding.Examples
{
    using Pathfinding;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_r_v_o_agent_placer.php")]
    public class RVOAgentPlacer : MonoBehaviour
    {
        public int agents = 100;
        public Vector3 goalOffset;
        public LayerMask mask;
        public GameObject prefab;
        private const float rad2Deg = 57.29578f;
        public float repathRate = 1f;
        public float ringSize = 100f;

        public Color GetColor(float angle)
        {
            return AstarMath.HSVToRGB(angle * 57.29578f, 0.8f, 0.6f);
        }

        [DebuggerHidden]
        private IEnumerator Start()
        {
            <Start>c__IteratorD rd = new <Start>c__IteratorD();
            rd.<>f__this = this;
            return rd;
        }

        [CompilerGenerated]
        private sealed class <Start>c__IteratorD : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal RVOAgentPlacer <>f__this;
            internal RVOExampleAgent <ag>__5;
            internal float <angle>__1;
            internal Vector3 <antipodal>__3;
            internal GameObject <go>__4;
            internal int <i>__0;
            internal Vector3 <pos>__2;

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
                        return true;

                    case 1:
                        this.<i>__0 = 0;
                        while (this.<i>__0 < this.<>f__this.agents)
                        {
                            this.<angle>__1 = ((((float) this.<i>__0) / ((float) this.<>f__this.agents)) * 3.141593f) * 2f;
                            this.<pos>__2 = (Vector3) (new Vector3((float) Math.Cos((double) this.<angle>__1), 0f, (float) Math.Sin((double) this.<angle>__1)) * this.<>f__this.ringSize);
                            this.<antipodal>__3 = -this.<pos>__2 + this.<>f__this.goalOffset;
                            this.<go>__4 = UnityEngine.Object.Instantiate(this.<>f__this.prefab, Vector3.zero, Quaternion.Euler(0f, this.<angle>__1 + 180f, 0f)) as GameObject;
                            this.<ag>__5 = this.<go>__4.GetComponent<RVOExampleAgent>();
                            if (this.<ag>__5 == null)
                            {
                                UnityEngine.Debug.LogError("Prefab does not have an RVOExampleAgent component attached");
                                break;
                            }
                            this.<go>__4.transform.parent = this.<>f__this.transform;
                            this.<go>__4.transform.position = this.<pos>__2;
                            this.<ag>__5.repathRate = this.<>f__this.repathRate;
                            this.<ag>__5.SetTarget(this.<antipodal>__3);
                            this.<ag>__5.SetColor(this.<>f__this.GetColor(this.<angle>__1));
                            this.<i>__0++;
                        }
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
    }
}

