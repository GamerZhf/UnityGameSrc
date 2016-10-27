namespace Pathfinding
{
    using System;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_target_mover.php")]
    public class TargetMover : MonoBehaviour
    {
        private RichAI[] ais;
        private AIPath[] ais2;
        private AILerp[] ais3;
        private Camera cam;
        public LayerMask mask;
        public bool onlyOnDoubleClick;
        public Transform target;
        public bool use2D;

        public void OnGUI()
        {
            if ((this.onlyOnDoubleClick && (this.cam != null)) && ((Event.current.type == UnityEngine.EventType.MouseDown) && (Event.current.clickCount == 2)))
            {
                this.UpdateTargetPosition();
            }
        }

        public void Start()
        {
            this.cam = Camera.main;
            this.ais = UnityEngine.Object.FindObjectsOfType<RichAI>();
            this.ais2 = UnityEngine.Object.FindObjectsOfType<AIPath>();
            this.ais3 = UnityEngine.Object.FindObjectsOfType<AILerp>();
            base.useGUILayout = false;
        }

        private void Update()
        {
            if (!this.onlyOnDoubleClick && (this.cam != null))
            {
                this.UpdateTargetPosition();
            }
        }

        public void UpdateTargetPosition()
        {
            Vector3 zero = Vector3.zero;
            bool flag = false;
            if (this.use2D)
            {
                zero = this.cam.ScreenToWorldPoint(Input.mousePosition);
                zero.z = 0f;
                flag = true;
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(this.cam.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity, (int) this.mask))
                {
                    zero = hit.point;
                    flag = true;
                }
            }
            if (flag && (zero != this.target.position))
            {
                this.target.position = zero;
                if (this.onlyOnDoubleClick)
                {
                    if (this.ais != null)
                    {
                        for (int i = 0; i < this.ais.Length; i++)
                        {
                            if (this.ais[i] != null)
                            {
                                this.ais[i].UpdatePath();
                            }
                        }
                    }
                    if (this.ais2 != null)
                    {
                        for (int j = 0; j < this.ais2.Length; j++)
                        {
                            if (this.ais2[j] != null)
                            {
                                this.ais2[j].SearchPath();
                            }
                        }
                    }
                    if (this.ais3 != null)
                    {
                        for (int k = 0; k < this.ais3.Length; k++)
                        {
                            if (this.ais3[k] != null)
                            {
                                this.ais3[k].SearchPath();
                            }
                        }
                    }
                }
            }
        }
    }
}

