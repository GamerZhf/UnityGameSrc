using Pathfinding;
using Pathfinding.RVO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Pathfinding/AI/AIPath (3D)"), HelpURL("http://arongranberg.com/astar/docs/class_a_i_path.php"), RequireComponent(typeof(Seeker))]
public class AIPath : MonoBehaviour
{
    public bool canMove = true;
    public bool canSearch = true;
    protected bool canSearchAgain = true;
    public bool closestOnPathCheck = true;
    protected CharacterController controller;
    protected int currentWaypointIndex;
    public float endReachedDistance = 0.2f;
    public float forwardLook = 1f;
    protected Vector3 lastFoundWaypointPosition;
    protected float lastFoundWaypointTime = -9999f;
    protected float lastRepath = -9999f;
    protected float minMoveScale = 0.05f;
    protected Path path;
    public float pickNextWaypointDist = 2f;
    public float repathRate = 0.5f;
    protected Rigidbody rigid;
    protected RVOController rvoController;
    protected Seeker seeker;
    public float slowdownDistance = 0.6f;
    public float speed = 3f;
    private bool startHasRun;
    public Transform target;
    protected Vector3 targetDirection;
    protected Vector3 targetPoint;
    protected bool targetReached;
    protected Transform tr;
    public float turningSpeed = 5f;

    protected virtual void Awake()
    {
        this.seeker = base.GetComponent<Seeker>();
        this.tr = base.transform;
        this.controller = base.GetComponent<CharacterController>();
        this.rvoController = base.GetComponent<RVOController>();
        if (this.rvoController != null)
        {
            this.rvoController.enableRotation = false;
        }
        this.rigid = base.GetComponent<Rigidbody>();
    }

    protected Vector3 CalculateTargetPoint(Vector3 p, Vector3 a, Vector3 b)
    {
        a.y = p.y;
        b.y = p.y;
        Vector3 vector2 = a - b;
        float magnitude = vector2.magnitude;
        if (magnitude == 0f)
        {
            return a;
        }
        float num2 = Mathf.Clamp01(VectorMath.ClosestPointOnLineFactor(a, b, p));
        Vector3 vector = ((Vector3) ((b - a) * num2)) + a;
        Vector3 vector3 = vector - p;
        float num3 = vector3.magnitude;
        float num5 = Mathf.Clamp(this.forwardLook - num3, 0f, this.forwardLook) / magnitude;
        num5 = Mathf.Clamp((float) (num5 + num2), (float) 0f, (float) 1f);
        return (((Vector3) ((b - a) * num5)) + a);
    }

    protected Vector3 CalculateVelocity(Vector3 currentPosition)
    {
        if (((this.path == null) || (this.path.vectorPath == null)) || (this.path.vectorPath.Count == 0))
        {
            return Vector3.zero;
        }
        List<Vector3> vectorPath = this.path.vectorPath;
        if (vectorPath.Count == 1)
        {
            vectorPath.Insert(0, currentPosition);
        }
        if (this.currentWaypointIndex >= vectorPath.Count)
        {
            this.currentWaypointIndex = vectorPath.Count - 1;
        }
        if (this.currentWaypointIndex <= 1)
        {
            this.currentWaypointIndex = 1;
        }
        while (true)
        {
            if ((this.currentWaypointIndex >= (vectorPath.Count - 1)) || (this.XZSqrMagnitude(vectorPath[this.currentWaypointIndex], currentPosition) >= (this.pickNextWaypointDist * this.pickNextWaypointDist)))
            {
                break;
            }
            this.lastFoundWaypointPosition = currentPosition;
            this.lastFoundWaypointTime = Time.time;
            this.currentWaypointIndex++;
        }
        Vector3 vector = vectorPath[this.currentWaypointIndex] - vectorPath[this.currentWaypointIndex - 1];
        Vector3 vector2 = this.CalculateTargetPoint(currentPosition, vectorPath[this.currentWaypointIndex - 1], vectorPath[this.currentWaypointIndex]);
        vector = vector2 - currentPosition;
        vector.y = 0f;
        float magnitude = vector.magnitude;
        float num3 = Mathf.Clamp01(magnitude / this.slowdownDistance);
        this.targetDirection = vector;
        this.targetPoint = vector2;
        if ((this.currentWaypointIndex == (vectorPath.Count - 1)) && (magnitude <= this.endReachedDistance))
        {
            if (!this.targetReached)
            {
                this.targetReached = true;
                this.OnTargetReached();
            }
            return Vector3.zero;
        }
        Vector3 forward = this.tr.forward;
        float a = Vector3.Dot(vector.normalized, forward);
        float num5 = (this.speed * Mathf.Max(a, this.minMoveScale)) * num3;
        if (Time.deltaTime > 0f)
        {
            num5 = Mathf.Clamp(num5, 0f, magnitude / (Time.deltaTime * 2f));
        }
        return (Vector3) (forward * num5);
    }

    public virtual Vector3 GetFeetPosition()
    {
        if (this.rvoController != null)
        {
            return (this.tr.position - ((Vector3) ((Vector3.up * this.rvoController.height) * 0.5f)));
        }
        if (this.controller != null)
        {
            return (this.tr.position - ((Vector3) ((Vector3.up * this.controller.height) * 0.5f)));
        }
        return this.tr.position;
    }

    public void OnDisable()
    {
        if ((this.seeker != null) && !this.seeker.IsDone())
        {
            this.seeker.GetCurrentPath().Error();
        }
        if (this.path != null)
        {
            this.path.Release(this, false);
        }
        this.path = null;
        this.seeker.pathCallback = (OnPathDelegate) Delegate.Remove(this.seeker.pathCallback, new OnPathDelegate(this.OnPathComplete));
    }

    protected virtual void OnEnable()
    {
        this.lastRepath = -9999f;
        this.canSearchAgain = true;
        this.lastFoundWaypointPosition = this.GetFeetPosition();
        if (this.startHasRun)
        {
            this.seeker.pathCallback = (OnPathDelegate) Delegate.Combine(this.seeker.pathCallback, new OnPathDelegate(this.OnPathComplete));
            base.StartCoroutine(this.RepeatTrySearchPath());
        }
    }

    public virtual void OnPathComplete(Path _p)
    {
        ABPath path = _p as ABPath;
        if (path == null)
        {
            throw new Exception("This function only handles ABPaths, do not use special path types");
        }
        this.canSearchAgain = true;
        path.Claim(this);
        if (path.error)
        {
            path.Release(this, false);
        }
        else
        {
            if (this.path != null)
            {
                this.path.Release(this, false);
            }
            this.path = path;
            this.currentWaypointIndex = 0;
            this.targetReached = false;
            if (this.closestOnPathCheck)
            {
                Vector3 currentPosition = ((Time.time - this.lastFoundWaypointTime) >= 0.3f) ? path.originalStartPoint : this.lastFoundWaypointPosition;
                Vector3 vector3 = this.GetFeetPosition() - currentPosition;
                float magnitude = vector3.magnitude;
                vector3 = (Vector3) (vector3 / magnitude);
                int num2 = (int) (magnitude / this.pickNextWaypointDist);
                for (int i = 0; i <= num2; i++)
                {
                    this.CalculateVelocity(currentPosition);
                    currentPosition += vector3;
                }
            }
        }
    }

    public virtual void OnTargetReached()
    {
    }

    [DebuggerHidden]
    protected IEnumerator RepeatTrySearchPath()
    {
        <RepeatTrySearchPath>c__Iterator1 iterator = new <RepeatTrySearchPath>c__Iterator1();
        iterator.<>f__this = this;
        return iterator;
    }

    protected virtual void RotateTowards(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            Quaternion rotation = this.tr.rotation;
            Quaternion to = Quaternion.LookRotation(dir);
            Vector3 eulerAngles = Quaternion.Slerp(rotation, to, this.turningSpeed * Time.deltaTime).eulerAngles;
            eulerAngles.z = 0f;
            eulerAngles.x = 0f;
            rotation = Quaternion.Euler(eulerAngles);
            this.tr.rotation = rotation;
        }
    }

    public virtual void SearchPath()
    {
        if (this.target == null)
        {
            throw new InvalidOperationException("Target is null");
        }
        this.lastRepath = Time.time;
        Vector3 position = this.target.position;
        this.canSearchAgain = false;
        this.seeker.StartPath(this.GetFeetPosition(), position);
    }

    protected virtual void Start()
    {
        this.startHasRun = true;
        this.OnEnable();
    }

    public float TrySearchPath()
    {
        if ((((Time.time - this.lastRepath) >= this.repathRate) && this.canSearchAgain) && (this.canSearch && (this.target != null)))
        {
            this.SearchPath();
            return this.repathRate;
        }
        float num = this.repathRate - (Time.time - this.lastRepath);
        return ((num >= 0f) ? num : 0f);
    }

    public virtual void Update()
    {
        if (this.canMove)
        {
            Vector3 vel = this.CalculateVelocity(this.GetFeetPosition());
            this.RotateTowards(this.targetDirection);
            if (this.rvoController != null)
            {
                this.rvoController.Move(vel);
            }
            else if (this.controller != null)
            {
                this.controller.SimpleMove(vel);
            }
            else if (this.rigid != null)
            {
                this.rigid.AddForce(vel);
            }
            else
            {
                this.tr.Translate((Vector3) (vel * Time.deltaTime), Space.World);
            }
        }
    }

    protected float XZSqrMagnitude(Vector3 a, Vector3 b)
    {
        float num = b.x - a.x;
        float num2 = b.z - a.z;
        return ((num * num) + (num2 * num2));
    }

    public bool TargetReached
    {
        get
        {
            return this.targetReached;
        }
    }

    [CompilerGenerated]
    private sealed class <RepeatTrySearchPath>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal AIPath <>f__this;
        internal float <v>__0;

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
                    this.<v>__0 = this.<>f__this.TrySearchPath();
                    this.$current = new WaitForSeconds(this.<v>__0);
                    this.$PC = 1;
                    return true;

                default:
                    break;
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

