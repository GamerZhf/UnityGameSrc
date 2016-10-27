namespace Pathfinding
{
    using Pathfinding.RVO;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [AddComponentMenu("Pathfinding/AI/RichAI (3D, for navmesh)"), HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_rich_a_i.php"), RequireComponent(typeof(Seeker))]
    public class RichAI : MonoBehaviour
    {
        public float acceleration = 5f;
        public Animation anim;
        protected List<Vector3> buffer = new List<Vector3>();
        protected bool canSearchPath;
        public float centerOffset = 1f;
        private CharacterController controller;
        private Vector3 currentTargetDirection;
        protected bool delayUpdatePath;
        private static float deltaTime;
        private float distanceToWaypoint = 999f;
        public bool drawGizmos = true;
        public float endReachedDistance = 0.01f;
        public RichFunnel.FunnelSimplification funnelSimplification;
        public static readonly Color GizmoColorPath = new Color(0.03137255f, 0.3058824f, 0.7607843f);
        public static readonly Color GizmoColorRaycast = new Color(0.4627451f, 0.8078431f, 0.4392157f);
        public Vector3 gravity = new Vector3(0f, -9.82f, 0f);
        public LayerMask groundMask = -1;
        protected bool lastCorner;
        protected float lastRepath = -9999f;
        private Vector3 lastTargetPoint;
        public float maxSpeed = 1f;
        public bool preciseSlowdown = true;
        public bool raycastingForGroundPlacement = true;
        public float repathRate = 0.5f;
        public bool repeatedlySearchPaths;
        public float rotationSpeed = 360f;
        protected RichPath rp;
        private RVOController rvoController;
        protected Seeker seeker;
        public float slowdownTime = 0.5f;
        public bool slowWhenNotFacingTarget = true;
        private bool startHasRun;
        public Transform target;
        protected Transform tr;
        protected bool traversingSpecialPath;
        private Vector3 velocity;
        protected bool waitingForPathCalc;
        protected List<Vector3> wallBuffer = new List<Vector3>();
        public float wallDist = 1f;
        public float wallForce = 3f;

        private void Awake()
        {
            this.seeker = base.GetComponent<Seeker>();
            this.controller = base.GetComponent<CharacterController>();
            this.rvoController = base.GetComponent<RVOController>();
            if (this.rvoController != null)
            {
                this.rvoController.enableRotation = false;
            }
            this.tr = base.transform;
        }

        private void NextPart()
        {
            this.rp.NextPart();
            this.lastCorner = false;
            if (!this.rp.PartsLeft())
            {
                this.OnTargetReached();
            }
        }

        public void OnDisable()
        {
            if ((this.seeker != null) && !this.seeker.IsDone())
            {
                this.seeker.GetCurrentPath().Error();
            }
            this.seeker.pathCallback = (OnPathDelegate) Delegate.Remove(this.seeker.pathCallback, new OnPathDelegate(this.OnPathComplete));
        }

        public void OnDrawGizmos()
        {
            if (this.drawGizmos)
            {
                if (this.raycastingForGroundPlacement)
                {
                    Gizmos.color = GizmoColorRaycast;
                    Gizmos.DrawLine(base.transform.position, base.transform.position + ((Vector3) (Vector3.up * this.centerOffset)));
                    Gizmos.DrawLine(base.transform.position + ((Vector3) (Vector3.left * 0.1f)), base.transform.position + ((Vector3) (Vector3.right * 0.1f)));
                    Gizmos.DrawLine(base.transform.position + ((Vector3) (Vector3.back * 0.1f)), base.transform.position + ((Vector3) (Vector3.forward * 0.1f)));
                }
                if ((this.tr != null) && (this.buffer != null))
                {
                    Gizmos.color = GizmoColorPath;
                    Vector3 position = this.tr.position;
                    for (int i = 0; i < this.buffer.Count; i++)
                    {
                        Gizmos.DrawLine(position, this.buffer[i]);
                        position = this.buffer[i];
                    }
                }
            }
        }

        protected virtual void OnEnable()
        {
            this.lastRepath = -9999f;
            this.waitingForPathCalc = false;
            this.canSearchPath = true;
            if (this.startHasRun)
            {
                this.seeker.pathCallback = (OnPathDelegate) Delegate.Combine(this.seeker.pathCallback, new OnPathDelegate(this.OnPathComplete));
                base.StartCoroutine(this.SearchPaths());
            }
        }

        private void OnPathComplete(Path p)
        {
            this.waitingForPathCalc = false;
            p.Claim(this);
            if (p.error)
            {
                p.Release(this, false);
            }
            else
            {
                if (this.traversingSpecialPath)
                {
                    this.delayUpdatePath = true;
                }
                else
                {
                    if (this.rp == null)
                    {
                        this.rp = new RichPath();
                    }
                    this.rp.Initialize(this.seeker, p, true, this.funnelSimplification);
                }
                p.Release(this, false);
            }
        }

        protected virtual void OnTargetReached()
        {
        }

        private Vector3 RaycastPosition(Vector3 position, float lasty)
        {
            if (this.raycastingForGroundPlacement)
            {
                RaycastHit hit;
                float maxDistance = Mathf.Max(this.centerOffset, (lasty - position.y) + this.centerOffset);
                if (Physics.Raycast(position + ((Vector3) (Vector3.up * maxDistance)), Vector3.down, out hit, maxDistance, (int) this.groundMask) && (hit.distance < maxDistance))
                {
                    position = hit.point;
                    this.velocity.y = 0f;
                }
            }
            return position;
        }

        private bool RotateTowards(Vector3 trotdir)
        {
            trotdir.y = 0f;
            if (trotdir != Vector3.zero)
            {
                Quaternion rotation = this.tr.rotation;
                Vector3 eulerAngles = Quaternion.LookRotation(trotdir).eulerAngles;
                Vector3 euler = rotation.eulerAngles;
                euler.y = Mathf.MoveTowardsAngle(euler.y, eulerAngles.y, this.rotationSpeed * deltaTime);
                this.tr.rotation = Quaternion.Euler(euler);
                return (Mathf.Abs((float) (euler.y - eulerAngles.y)) < 5f);
            }
            return false;
        }

        [DebuggerHidden]
        private IEnumerator SearchPaths()
        {
            <SearchPaths>c__Iterator2 iterator = new <SearchPaths>c__Iterator2();
            iterator.<>f__this = this;
            return iterator;
        }

        protected virtual void Start()
        {
            this.startHasRun = true;
            this.OnEnable();
        }

        [DebuggerHidden]
        private IEnumerator TraverseSpecial(RichSpecial rs)
        {
            <TraverseSpecial>c__Iterator3 iterator = new <TraverseSpecial>c__Iterator3();
            iterator.rs = rs;
            iterator.<$>rs = rs;
            iterator.<>f__this = this;
            return iterator;
        }

        protected virtual void Update()
        {
            deltaTime = Mathf.Min(Time.smoothDeltaTime * 2f, Time.deltaTime);
            if (this.rp != null)
            {
                RichPathPart currentPart = this.rp.GetCurrentPart();
                RichFunnel fn = currentPart as RichFunnel;
                if (fn != null)
                {
                    Vector3 position = this.UpdateTarget(fn);
                    if ((((Time.frameCount % 5) == 0) && (this.wallForce > 0f)) && (this.wallDist > 0f))
                    {
                        this.wallBuffer.Clear();
                        fn.FindWalls(this.wallBuffer, this.wallDist);
                    }
                    int num = 0;
                    Vector3 vector2 = this.buffer[num];
                    Vector3 lhs = vector2 - position;
                    lhs.y = 0f;
                    if ((Vector3.Dot(lhs, this.currentTargetDirection) < 0f) && ((this.buffer.Count - num) > 1))
                    {
                        num++;
                        vector2 = this.buffer[num];
                    }
                    if (vector2 != this.lastTargetPoint)
                    {
                        this.currentTargetDirection = vector2 - position;
                        this.currentTargetDirection.y = 0f;
                        this.currentTargetDirection.Normalize();
                        this.lastTargetPoint = vector2;
                    }
                    lhs = vector2 - position;
                    lhs.y = 0f;
                    float magnitude = lhs.magnitude;
                    this.distanceToWaypoint = magnitude;
                    lhs = (magnitude != 0f) ? ((Vector3) (lhs / magnitude)) : Vector3.zero;
                    Vector3 vector4 = lhs;
                    Vector3 zero = Vector3.zero;
                    if ((this.wallForce > 0f) && (this.wallDist > 0f))
                    {
                        float num3 = 0f;
                        float num4 = 0f;
                        for (int i = 0; i < this.wallBuffer.Count; i += 2)
                        {
                            Vector3 vector11 = VectorMath.ClosestPointOnSegment(this.wallBuffer[i], this.wallBuffer[i + 1], this.tr.position) - position;
                            float sqrMagnitude = vector11.sqrMagnitude;
                            if (sqrMagnitude <= (this.wallDist * this.wallDist))
                            {
                                Vector3 vector12 = this.wallBuffer[i + 1] - this.wallBuffer[i];
                                Vector3 normalized = vector12.normalized;
                                float num7 = Vector3.Dot(lhs, normalized) * (1f - Math.Max((float) 0f, (float) ((2f * (sqrMagnitude / (this.wallDist * this.wallDist))) - 1f)));
                                if (num7 > 0f)
                                {
                                    num4 = Math.Max(num4, num7);
                                }
                                else
                                {
                                    num3 = Math.Max(num3, -num7);
                                }
                            }
                        }
                        zero = (Vector3) (Vector3.Cross(Vector3.up, lhs) * (num4 - num3));
                    }
                    bool flag2 = this.lastCorner && ((this.buffer.Count - num) == 1);
                    if (flag2)
                    {
                        if (this.slowdownTime < 0.001f)
                        {
                            this.slowdownTime = 0.001f;
                        }
                        Vector3 vector9 = vector2 - position;
                        vector9.y = 0f;
                        if (this.preciseSlowdown)
                        {
                            lhs = (Vector3) (((6f * vector9) - ((4f * this.slowdownTime) * this.velocity)) / (this.slowdownTime * this.slowdownTime));
                        }
                        else
                        {
                            lhs = (Vector3) ((2f * (vector9 - (this.slowdownTime * this.velocity))) / (this.slowdownTime * this.slowdownTime));
                        }
                        lhs = Vector3.ClampMagnitude(lhs, this.acceleration);
                        zero = (Vector3) (zero * Math.Min((float) (magnitude / 0.5f), (float) 1f));
                        if (magnitude < this.endReachedDistance)
                        {
                            this.NextPart();
                        }
                    }
                    else
                    {
                        lhs = (Vector3) (lhs * this.acceleration);
                    }
                    this.velocity += (Vector3) ((lhs + (zero * this.wallForce)) * deltaTime);
                    if (this.slowWhenNotFacingTarget)
                    {
                        float a = (Vector3.Dot(vector4, this.tr.forward) + 0.5f) * 0.6666667f;
                        float num9 = Mathf.Sqrt((this.velocity.x * this.velocity.x) + (this.velocity.z * this.velocity.z));
                        float y = this.velocity.y;
                        this.velocity.y = 0f;
                        float num11 = Mathf.Min(num9, this.maxSpeed * Mathf.Max(a, 0.2f));
                        this.velocity = Vector3.Lerp((Vector3) (this.tr.forward * num11), (Vector3) (this.velocity.normalized * num11), Mathf.Clamp(!flag2 ? 0f : (magnitude * 2f), 0.5f, 1f));
                        this.velocity.y = y;
                    }
                    else
                    {
                        float num12 = Mathf.Sqrt((this.velocity.x * this.velocity.x) + (this.velocity.z * this.velocity.z));
                        num12 = this.maxSpeed / num12;
                        if (num12 < 1f)
                        {
                            this.velocity.x *= num12;
                            this.velocity.z *= num12;
                        }
                    }
                    if (flag2)
                    {
                        Vector3 trotdir = Vector3.Lerp(this.velocity, this.currentTargetDirection, Math.Max((float) (1f - (magnitude * 2f)), (float) 0f));
                        this.RotateTowards(trotdir);
                    }
                    else
                    {
                        this.RotateTowards(this.velocity);
                    }
                    this.velocity += (Vector3) (deltaTime * this.gravity);
                    if ((this.rvoController != null) && this.rvoController.enabled)
                    {
                        this.tr.position = position;
                        this.rvoController.Move(this.velocity);
                    }
                    else if ((this.controller != null) && this.controller.enabled)
                    {
                        this.tr.position = position;
                        this.controller.Move((Vector3) (this.velocity * deltaTime));
                    }
                    else
                    {
                        float lasty = position.y;
                        position += (Vector3) (this.velocity * deltaTime);
                        position = this.RaycastPosition(position, lasty);
                        this.tr.position = position;
                    }
                }
                else if ((this.rvoController != null) && this.rvoController.enabled)
                {
                    this.rvoController.Move(Vector3.zero);
                }
                if ((currentPart is RichSpecial) && !this.traversingSpecialPath)
                {
                    base.StartCoroutine(this.TraverseSpecial(currentPart as RichSpecial));
                }
            }
            else if ((this.rvoController != null) && this.rvoController.enabled)
            {
                this.rvoController.Move(Vector3.zero);
            }
            else if ((this.controller == null) || !this.controller.enabled)
            {
                this.tr.position = this.RaycastPosition(this.tr.position, this.tr.position.y);
            }
        }

        public virtual void UpdatePath()
        {
            this.canSearchPath = true;
            this.waitingForPathCalc = false;
            Path currentPath = this.seeker.GetCurrentPath();
            if ((currentPath != null) && !this.seeker.IsDone())
            {
                currentPath.Error();
                currentPath.Claim(this);
                currentPath.Release(this, false);
            }
            this.waitingForPathCalc = true;
            this.lastRepath = Time.time;
            this.seeker.StartPath(this.tr.position, this.target.position);
        }

        protected virtual Vector3 UpdateTarget(RichFunnel fn)
        {
            bool flag;
            this.buffer.Clear();
            Vector3 position = this.tr.position;
            position = fn.Update(position, this.buffer, 2, out this.lastCorner, out flag);
            if (flag && !this.waitingForPathCalc)
            {
                this.UpdatePath();
            }
            return position;
        }

        public bool ApproachingPartEndpoint
        {
            get
            {
                return this.lastCorner;
            }
        }

        public bool ApproachingPathEndpoint
        {
            get
            {
                return (((this.rp != null) && this.ApproachingPartEndpoint) && !this.rp.PartsLeft());
            }
        }

        public float DistanceToNextWaypoint
        {
            get
            {
                return this.distanceToWaypoint;
            }
        }

        public Vector3 TargetPoint
        {
            get
            {
                return this.lastTargetPoint;
            }
        }

        public bool TraversingSpecial
        {
            get
            {
                return this.traversingSpecialPath;
            }
        }

        public Vector3 Velocity
        {
            get
            {
                return this.velocity;
            }
        }

        [CompilerGenerated]
        private sealed class <SearchPaths>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal RichAI <>f__this;

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
                    case 1:
                    case 2:
                        while ((!this.<>f__this.repeatedlySearchPaths || this.<>f__this.waitingForPathCalc) || (!this.<>f__this.canSearchPath || ((Time.time - this.<>f__this.lastRepath) < this.<>f__this.repathRate)))
                        {
                            this.$current = null;
                            this.$PC = 1;
                            goto Label_00BA;
                        }
                        this.<>f__this.UpdatePath();
                        this.$current = null;
                        this.$PC = 2;
                        goto Label_00BA;

                    default:
                        break;
                        this.$PC = -1;
                        break;
                }
                return false;
            Label_00BA:
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

        [CompilerGenerated]
        private sealed class <TraverseSpecial>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal RichSpecial <$>rs;
            internal RichAI <>f__this;
            internal AnimationLink <al>__0;
            internal RichSpecial rs;

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
                        this.<>f__this.traversingSpecialPath = true;
                        this.<>f__this.velocity = Vector3.zero;
                        this.<al>__0 = this.rs.nodeLink as AnimationLink;
                        if (this.<al>__0 != null)
                        {
                            break;
                        }
                        UnityEngine.Debug.LogError("Unhandled RichSpecial");
                        goto Label_02FE;

                    case 1:
                        break;

                    case 2:
                        this.<>f__this.traversingSpecialPath = false;
                        this.<>f__this.NextPart();
                        if (this.<>f__this.delayUpdatePath)
                        {
                            this.<>f__this.delayUpdatePath = false;
                            this.<>f__this.UpdatePath();
                        }
                        this.$PC = -1;
                        goto Label_02FE;

                    default:
                        goto Label_02FE;
                }
                while (!this.<>f__this.RotateTowards(this.rs.first.forward))
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_0300;
                }
                this.<>f__this.tr.parent.position = this.<>f__this.tr.position;
                this.<>f__this.tr.parent.rotation = this.<>f__this.tr.rotation;
                this.<>f__this.tr.localPosition = Vector3.zero;
                this.<>f__this.tr.localRotation = Quaternion.identity;
                if (this.rs.reverse && this.<al>__0.reverseAnim)
                {
                    this.<>f__this.anim[this.<al>__0.clip].speed = -this.<al>__0.animSpeed;
                    this.<>f__this.anim[this.<al>__0.clip].normalizedTime = 1f;
                    this.<>f__this.anim.Play(this.<al>__0.clip);
                    this.<>f__this.anim.Sample();
                }
                else
                {
                    this.<>f__this.anim[this.<al>__0.clip].speed = this.<al>__0.animSpeed;
                    this.<>f__this.anim.Rewind(this.<al>__0.clip);
                    this.<>f__this.anim.Play(this.<al>__0.clip);
                }
                Transform parent = this.<>f__this.tr.parent;
                parent.position -= this.<>f__this.tr.position - this.<>f__this.tr.parent.position;
                this.$current = new WaitForSeconds(Mathf.Abs((float) (this.<>f__this.anim[this.<al>__0.clip].length / this.<al>__0.animSpeed)));
                this.$PC = 2;
                goto Label_0300;
            Label_02FE:
                return false;
            Label_0300:
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

