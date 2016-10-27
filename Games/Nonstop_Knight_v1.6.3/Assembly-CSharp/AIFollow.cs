using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[HelpURL("http://arongranberg.com/astar/docs/class_a_i_follow.php"), RequireComponent(typeof(Seeker)), RequireComponent(typeof(CharacterController)), AddComponentMenu("Pathfinding/AI/AIFollow (deprecated)")]
public class AIFollow : MonoBehaviour
{
    public bool canMove = true;
    public bool canSearch = true;
    protected CharacterController controller;
    public bool drawGizmos;
    protected float lastPathSearch = -9999f;
    protected Vector3[] path;
    protected int pathIndex;
    public float pickNextWaypointDistance = 1f;
    public float repathRate = 0.1f;
    public float rotationSpeed = 1f;
    protected Seeker seeker;
    public float speed = 5f;
    public Transform target;
    public float targetReached = 0.2f;
    protected Transform tr;

    public void OnDrawGizmos()
    {
        if ((this.drawGizmos && (this.path != null)) && ((this.pathIndex < this.path.Length) && (this.pathIndex >= 0)))
        {
            Vector3 end = this.path[this.pathIndex];
            end.y = this.tr.position.y;
            UnityEngine.Debug.DrawLine(base.transform.position, end, Color.blue);
            float pickNextWaypointDistance = this.pickNextWaypointDistance;
            if (this.pathIndex == (this.path.Length - 1))
            {
                pickNextWaypointDistance *= this.targetReached;
            }
            Vector3 start = end + ((Vector3) (pickNextWaypointDistance * new Vector3(1f, 0f, 0f)));
            for (float i = 0f; i < 6.2831853071795862; i += 0.1f)
            {
                Vector3 vector3 = end + new Vector3(((float) Math.Cos((double) i)) * pickNextWaypointDistance, 0f, ((float) Math.Sin((double) i)) * pickNextWaypointDistance);
                UnityEngine.Debug.DrawLine(start, vector3, Color.yellow);
                start = vector3;
            }
            UnityEngine.Debug.DrawLine(start, end + ((Vector3) (pickNextWaypointDistance * new Vector3(1f, 0f, 0f))), Color.yellow);
        }
    }

    public void OnPathComplete(Path p)
    {
        base.StartCoroutine(this.WaitToRepath());
        if (!p.error)
        {
            this.path = p.vectorPath.ToArray();
            float positiveInfinity = float.PositiveInfinity;
            int num2 = 0;
            for (int i = 0; i < (this.path.Length - 1); i++)
            {
                float num4 = VectorMath.SqrDistancePointSegment(this.path[i], this.path[i + 1], this.tr.position);
                if (num4 < positiveInfinity)
                {
                    num2 = 0;
                    positiveInfinity = num4;
                    this.pathIndex = i + 1;
                }
                else if (num2 > 6)
                {
                    break;
                }
            }
        }
    }

    public void PathToTarget(Vector3 targetPoint)
    {
        this.lastPathSearch = Time.time;
        if (this.seeker != null)
        {
            this.seeker.StartPath(base.transform.position, targetPoint, new OnPathDelegate(this.OnPathComplete));
        }
    }

    public virtual void ReachedEndOfPath()
    {
    }

    public virtual void Repath()
    {
        this.lastPathSearch = Time.time;
        if (((this.seeker == null) || (this.target == null)) || (!this.canSearch || !this.seeker.IsDone()))
        {
            base.StartCoroutine(this.WaitToRepath());
        }
        else
        {
            Path p = ABPath.Construct(base.transform.position, this.target.position, null);
            this.seeker.StartPath(p, new OnPathDelegate(this.OnPathComplete), -1);
        }
    }

    public void Reset()
    {
        this.path = null;
    }

    public void Resume()
    {
        this.canMove = true;
        this.canSearch = true;
    }

    public void Start()
    {
        this.seeker = base.GetComponent<Seeker>();
        this.controller = base.GetComponent<CharacterController>();
        this.tr = base.transform;
        this.Repath();
    }

    public void Stop()
    {
        this.canMove = false;
        this.canSearch = false;
    }

    public void Update()
    {
        Vector3 vector7;
        if (((this.path == null) || (this.pathIndex >= this.path.Length)) || ((this.pathIndex < 0) || !this.canMove))
        {
            return;
        }
        Vector3 vector = this.path[this.pathIndex];
        vector.y = this.tr.position.y;
    Label_0113:
        vector7 = vector - this.tr.position;
        if (vector7.sqrMagnitude < (this.pickNextWaypointDistance * this.pickNextWaypointDistance))
        {
            this.pathIndex++;
            if (this.pathIndex >= this.path.Length)
            {
                Vector3 vector5 = vector - this.tr.position;
                if (vector5.sqrMagnitude < ((this.pickNextWaypointDistance * this.targetReached) * (this.pickNextWaypointDistance * this.targetReached)))
                {
                    this.ReachedEndOfPath();
                    return;
                }
                this.pathIndex--;
            }
            else
            {
                vector = this.path[this.pathIndex];
                vector.y = this.tr.position.y;
                goto Label_0113;
            }
        }
        Vector3 forward = vector - this.tr.position;
        this.tr.rotation = Quaternion.Slerp(this.tr.rotation, Quaternion.LookRotation(forward), this.rotationSpeed * Time.deltaTime);
        this.tr.eulerAngles = new Vector3(0f, this.tr.eulerAngles.y, 0f);
        Vector3 speed = (Vector3) (base.transform.forward * this.speed);
        speed = (Vector3) (speed * Mathf.Clamp01(Vector3.Dot(forward.normalized, this.tr.forward)));
        if (this.controller != null)
        {
            this.controller.SimpleMove(speed);
        }
        else
        {
            base.transform.Translate((Vector3) (speed * Time.deltaTime), Space.World);
        }
    }

    [DebuggerHidden]
    public IEnumerator WaitToRepath()
    {
        <WaitToRepath>c__Iterator13 iterator = new <WaitToRepath>c__Iterator13();
        iterator.<>f__this = this;
        return iterator;
    }

    [CompilerGenerated]
    private sealed class <WaitToRepath>c__Iterator13 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal AIFollow <>f__this;
        internal float <timeLeft>__0;

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
                    this.<timeLeft>__0 = this.<>f__this.repathRate - (Time.time - this.<>f__this.lastPathSearch);
                    this.$current = new WaitForSeconds(this.<timeLeft>__0);
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.Repath();
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

