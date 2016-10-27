namespace Pathfinding
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class FloodPathTracer : ABPath
    {
        protected FloodPath flood;

        public override void CalculateStep(long targetTick)
        {
            if (!base.IsDone())
            {
                base.Error();
            }
        }

        public static FloodPathTracer Construct(Vector3 start, FloodPath flood, [Optional, DefaultParameterValue(null)] OnPathDelegate callback)
        {
            FloodPathTracer path = PathPool.GetPath<FloodPathTracer>();
            path.Setup(start, flood, callback);
            return path;
        }

        public override void Initialize()
        {
            if ((base.startNode != null) && this.flood.HasPathTo(base.startNode))
            {
                this.Trace(base.startNode);
                base.CompleteState = PathCompleteState.Complete;
            }
            else
            {
                base.Error();
            }
        }

        public override void Reset()
        {
            base.Reset();
            this.flood = null;
        }

        protected void Setup(Vector3 start, FloodPath flood, OnPathDelegate callback)
        {
            this.flood = flood;
            if ((flood == null) || (flood.GetState() < PathState.Returned))
            {
                throw new ArgumentException("You must supply a calculated FloodPath to the 'flood' argument");
            }
            base.Setup(start, flood.originalStartPoint, callback);
            base.nnConstraint = new FloodPathConstraint(flood);
        }

        public void Trace(GraphNode from)
        {
            GraphNode item = from;
            int num = 0;
            while (item != null)
            {
                base.path.Add(item);
                base.vectorPath.Add((Vector3) item.position);
                item = this.flood.GetParent(item);
                num++;
                if (num > 0x400)
                {
                    Debug.LogWarning("Inifinity loop? >1024 node path. Remove this message if you really have that long paths (FloodPathTracer.cs, Trace function)");
                    break;
                }
            }
        }

        protected override bool hasEndPoint
        {
            get
            {
                return false;
            }
        }
    }
}

