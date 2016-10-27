using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

[AddComponentMenu("Pathfinding/AI/AISimpleLerp (2D,3D generic)"), RequireComponent(typeof(Seeker)), HelpURL("http://arongranberg.com/astar/docs/class_a_i_lerp.php")]
public class AILerp : MonoBehaviour
{
    [CompilerGenerated]
    private bool <targetReached>k__BackingField;
    public bool canMove = true;
    public bool canSearch = true;
    protected bool canSearchAgain = true;
    protected int currentWaypointIndex;
    protected float distanceAlongSegment;
    public bool enableRotation = true;
    public bool interpolatePathSwitches = true;
    protected float lastRepath = -9999f;
    protected ABPath path;
    protected Vector3 previousMovementDirection;
    protected Vector3 previousMovementOrigin;
    protected float previousMovementStartTime = -9999f;
    public float repathRate = 0.5f;
    public bool rotationIn2D;
    public float rotationSpeed = 10f;
    protected Seeker seeker;
    public float speed = 3f;
    private bool startHasRun;
    public float switchPathInterpolationSpeed = 5f;
    public Transform target;
    protected Transform tr;

    protected virtual void Awake()
    {
        this.tr = base.transform;
        this.seeker = base.GetComponent<Seeker>();
        this.seeker.startEndModifier.adjustStartPoint = delegate {
            return this.tr.position;
        };
    }

    protected virtual Vector3 CalculateNextPosition(out Vector3 direction)
    {
        if (((this.path == null) || (this.path.vectorPath == null)) || (this.path.vectorPath.Count == 0))
        {
            direction = Vector3.zero;
            return this.tr.position;
        }
        List<Vector3> vectorPath = this.path.vectorPath;
        this.currentWaypointIndex = Mathf.Clamp(this.currentWaypointIndex, 1, vectorPath.Count - 1);
        Vector3 vector = vectorPath[this.currentWaypointIndex] - vectorPath[this.currentWaypointIndex - 1];
        float magnitude = vector.magnitude;
        this.distanceAlongSegment += Time.deltaTime * this.speed;
        if ((this.distanceAlongSegment >= magnitude) && (this.currentWaypointIndex < (vectorPath.Count - 1)))
        {
            float num2 = this.distanceAlongSegment - magnitude;
            while (true)
            {
                this.currentWaypointIndex++;
                Vector3 vector2 = vectorPath[this.currentWaypointIndex] - vectorPath[this.currentWaypointIndex - 1];
                float num3 = vector2.magnitude;
                if ((num2 <= num3) || (this.currentWaypointIndex == (vectorPath.Count - 1)))
                {
                    vector = vector2;
                    magnitude = num3;
                    this.distanceAlongSegment = num2;
                    break;
                }
                num2 -= num3;
            }
        }
        if ((this.distanceAlongSegment >= magnitude) && (this.currentWaypointIndex == (vectorPath.Count - 1)))
        {
            if (!this.targetReached)
            {
                this.OnTargetReached();
            }
            this.targetReached = true;
        }
        Vector3 to = ((Vector3) (vector * Mathf.Clamp01((magnitude <= 0f) ? 1f : (this.distanceAlongSegment / magnitude)))) + vectorPath[this.currentWaypointIndex - 1];
        direction = vector;
        if (this.interpolatePathSwitches)
        {
            Vector3 from = this.previousMovementOrigin + Vector3.ClampMagnitude(this.previousMovementDirection, this.speed * (Time.time - this.previousMovementStartTime));
            return Vector3.Lerp(from, to, this.switchPathInterpolationSpeed * (Time.time - this.previousMovementStartTime));
        }
        return to;
    }

    protected virtual void ConfigureNewPath()
    {
        List<Vector3> vectorPath = this.path.vectorPath;
        Vector3 feetPosition = this.GetFeetPosition();
        float num = 0f;
        float positiveInfinity = float.PositiveInfinity;
        Vector3 zero = Vector3.zero;
        int num3 = 1;
        for (int i = 0; i < (vectorPath.Count - 1); i++)
        {
            float t = Mathf.Clamp01(VectorMath.ClosestPointOnLineFactor(vectorPath[i], vectorPath[i + 1], feetPosition));
            Vector3 vector3 = Vector3.Lerp(vectorPath[i], vectorPath[i + 1], t);
            Vector3 vector4 = feetPosition - vector3;
            float sqrMagnitude = vector4.sqrMagnitude;
            if (sqrMagnitude < positiveInfinity)
            {
                positiveInfinity = sqrMagnitude;
                zero = vectorPath[i + 1] - vectorPath[i];
                num = t * zero.magnitude;
                num3 = i + 1;
            }
        }
        this.currentWaypointIndex = num3;
        this.distanceAlongSegment = num;
        if (this.interpolatePathSwitches && (this.switchPathInterpolationSpeed > 0.01f))
        {
            float num7 = Mathf.Max(-Vector3.Dot(this.previousMovementDirection.normalized, zero.normalized), 0f);
            this.distanceAlongSegment -= (this.speed * num7) * (1f / this.switchPathInterpolationSpeed);
        }
    }

    protected virtual void ConfigurePathSwitchInterpolation()
    {
        bool flag = ((this.path != null) && (this.path.vectorPath != null)) && (this.path.vectorPath.Count > 1);
        bool flag2 = false;
        if (flag)
        {
            flag2 = (this.currentWaypointIndex == (this.path.vectorPath.Count - 1)) && (this.distanceAlongSegment >= (this.path.vectorPath[this.path.vectorPath.Count - 1] - this.path.vectorPath[this.path.vectorPath.Count - 2]).magnitude);
        }
        if (flag && !flag2)
        {
            List<Vector3> vectorPath = this.path.vectorPath;
            this.currentWaypointIndex = Mathf.Clamp(this.currentWaypointIndex, 1, vectorPath.Count - 1);
            Vector3 vector = vectorPath[this.currentWaypointIndex] - vectorPath[this.currentWaypointIndex - 1];
            float num2 = vector.magnitude * Mathf.Clamp01(1f - this.distanceAlongSegment);
            for (int i = this.currentWaypointIndex; i < (vectorPath.Count - 1); i++)
            {
                Vector3 vector3 = vectorPath[i + 1] - vectorPath[i];
                num2 += vector3.magnitude;
            }
            this.previousMovementOrigin = this.GetFeetPosition();
            this.previousMovementDirection = (Vector3) (vector.normalized * num2);
            this.previousMovementStartTime = Time.time;
        }
        else
        {
            this.previousMovementOrigin = Vector3.zero;
            this.previousMovementDirection = Vector3.zero;
            this.previousMovementStartTime = -9999f;
        }
    }

    public virtual void ForceSearchPath()
    {
        if (this.target == null)
        {
            throw new InvalidOperationException("Target is null");
        }
        this.lastRepath = Time.time;
        Vector3 position = this.target.position;
        Vector3 feetPosition = this.GetFeetPosition();
        if ((this.path != null) && (this.path.vectorPath.Count > 1))
        {
            feetPosition = this.path.vectorPath[this.currentWaypointIndex];
        }
        this.canSearchAgain = false;
        this.seeker.StartPath(feetPosition, position);
    }

    public virtual Vector3 GetFeetPosition()
    {
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
            if (this.interpolatePathSwitches)
            {
                this.ConfigurePathSwitchInterpolation();
            }
            if (this.path != null)
            {
                this.path.Release(this, false);
            }
            this.path = path;
            if ((this.path.vectorPath != null) && (this.path.vectorPath.Count == 1))
            {
                this.path.vectorPath.Insert(0, this.GetFeetPosition());
            }
            this.targetReached = false;
            this.ConfigureNewPath();
        }
    }

    public virtual void OnTargetReached()
    {
    }

    [DebuggerHidden]
    protected IEnumerator RepeatTrySearchPath()
    {
        <RepeatTrySearchPath>c__Iterator0 iterator = new <RepeatTrySearchPath>c__Iterator0();
        iterator.<>f__this = this;
        return iterator;
    }

    public virtual void SearchPath()
    {
        this.ForceSearchPath();
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

    protected virtual void Update()
    {
        if (this.canMove)
        {
            Vector3 vector;
            Vector3 vector2 = this.CalculateNextPosition(out vector);
            if (this.enableRotation && (vector != Vector3.zero))
            {
                if (this.rotationIn2D)
                {
                    float b = (Mathf.Atan2(vector.x, -vector.y) * 57.29578f) + 180f;
                    Vector3 eulerAngles = this.tr.eulerAngles;
                    eulerAngles.z = Mathf.LerpAngle(eulerAngles.z, b, Time.deltaTime * this.rotationSpeed);
                    this.tr.eulerAngles = eulerAngles;
                }
                else
                {
                    Quaternion rotation = this.tr.rotation;
                    Quaternion to = Quaternion.LookRotation(vector);
                    this.tr.rotation = Quaternion.Slerp(rotation, to, Time.deltaTime * this.rotationSpeed);
                }
            }
            this.tr.position = vector2;
        }
    }

    public bool targetReached
    {
        [CompilerGenerated]
        get
        {
            return this.<targetReached>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<targetReached>k__BackingField = value;
        }
    }

    [CompilerGenerated]
    private sealed class <RepeatTrySearchPath>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal AILerp <>f__this;
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

