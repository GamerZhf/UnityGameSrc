namespace Pathfinding.Examples
{
    using Pathfinding;
    using Pathfinding.RVO;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_r_v_o_example_agent.php")]
    public class RVOExampleAgent : MonoBehaviour
    {
        private bool canSearchAgain = true;
        private RVOController controller;
        public float moveNextDist = 1f;
        private float nextRepath;
        private Path path;
        private MeshRenderer[] rends;
        public float repathRate = 1f;
        private Seeker seeker;
        private Vector3 target;
        private List<Vector3> vectorPath;
        private int wp;

        public void Awake()
        {
            this.seeker = base.GetComponent<Seeker>();
        }

        public void OnPathComplete(Path _p)
        {
            ABPath path = _p as ABPath;
            this.canSearchAgain = true;
            if (this.path != null)
            {
                this.path.Release(this, false);
            }
            this.path = path;
            path.Claim(this);
            if (path.error)
            {
                this.wp = 0;
                this.vectorPath = null;
            }
            else
            {
                Vector3 originalStartPoint = path.originalStartPoint;
                Vector3 position = base.transform.position;
                originalStartPoint.y = position.y;
                Vector3 vector5 = position - originalStartPoint;
                float magnitude = vector5.magnitude;
                this.wp = 0;
                this.vectorPath = path.vectorPath;
                for (float i = 0f; i <= magnitude; i += this.moveNextDist * 0.6f)
                {
                    Vector3 vector6;
                    this.wp--;
                    Vector3 vector4 = originalStartPoint + ((Vector3) ((position - originalStartPoint) * i));
                    do
                    {
                        this.wp++;
                        Vector3 vector3 = this.vectorPath[this.wp];
                        vector3.y = vector4.y;
                        vector6 = vector4 - vector3;
                    }
                    while ((vector6.sqrMagnitude < (this.moveNextDist * this.moveNextDist)) && (this.wp != (this.vectorPath.Count - 1)));
                }
            }
        }

        public void RecalculatePath()
        {
            this.canSearchAgain = false;
            this.nextRepath = Time.time + (this.repathRate * (UnityEngine.Random.value + 0.5f));
            this.seeker.StartPath(base.transform.position, this.target, new OnPathDelegate(this.OnPathComplete));
        }

        public void SetColor(Color col)
        {
            if (this.rends == null)
            {
                this.rends = base.GetComponentsInChildren<MeshRenderer>();
            }
            foreach (MeshRenderer renderer in this.rends)
            {
                Color color = renderer.material.GetColor("_TintColor");
                AnimationCurve curve = AnimationCurve.Linear(0f, color.r, 1f, col.r);
                AnimationCurve curve2 = AnimationCurve.Linear(0f, color.g, 1f, col.g);
                AnimationCurve curve3 = AnimationCurve.Linear(0f, color.b, 1f, col.b);
                AnimationClip clip = new AnimationClip();
                clip.legacy = true;
                clip.SetCurve(string.Empty, typeof(Material), "_TintColor.r", curve);
                clip.SetCurve(string.Empty, typeof(Material), "_TintColor.g", curve2);
                clip.SetCurve(string.Empty, typeof(Material), "_TintColor.b", curve3);
                Animation component = renderer.gameObject.GetComponent<Animation>();
                if (component == null)
                {
                    component = renderer.gameObject.AddComponent<Animation>();
                }
                clip.wrapMode = WrapMode.Once;
                component.AddClip(clip, "ColorAnim");
                component.Play("ColorAnim");
            }
        }

        public void SetTarget(Vector3 target)
        {
            this.target = target;
            this.RecalculatePath();
        }

        public void Start()
        {
            this.SetTarget(-base.transform.position);
            this.controller = base.GetComponent<RVOController>();
        }

        public void Update()
        {
            Vector3 vector4;
            if ((Time.time >= this.nextRepath) && this.canSearchAgain)
            {
                this.RecalculatePath();
            }
            Vector3 zero = Vector3.zero;
            Vector3 position = base.transform.position;
            if ((this.vectorPath == null) || (this.vectorPath.Count == 0))
            {
                goto Label_0114;
            }
            Vector3 vector3 = this.vectorPath[this.wp];
            vector3.y = position.y;
        Label_00A1:
            vector4 = position - vector3;
            if ((vector4.sqrMagnitude < (this.moveNextDist * this.moveNextDist)) && (this.wp != (this.vectorPath.Count - 1)))
            {
                this.wp++;
                vector3 = this.vectorPath[this.wp];
                vector3.y = position.y;
                goto Label_00A1;
            }
            zero = vector3 - position;
            float magnitude = zero.magnitude;
            if (magnitude > 0f)
            {
                float num2 = Mathf.Min(magnitude, this.controller.maxSpeed);
                zero = (Vector3) (zero * (num2 / magnitude));
            }
        Label_0114:
            this.controller.Move(zero);
        }
    }
}

