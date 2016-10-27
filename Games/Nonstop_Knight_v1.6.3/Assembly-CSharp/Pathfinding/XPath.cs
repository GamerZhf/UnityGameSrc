namespace Pathfinding
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class XPath : ABPath
    {
        public PathEndingCondition endingCondition;

        public override void CalculateStep(long targetTick)
        {
            for (int i = 0; base.CompleteState == PathCompleteState.NotCalculated; i++)
            {
                base.searchedNodes++;
                if (this.endingCondition.TargetFound(base.currentR))
                {
                    base.CompleteState = PathCompleteState.Complete;
                    break;
                }
                base.currentR.node.Open(this, base.currentR, base.pathHandler);
                if (base.pathHandler.heap.isEmpty)
                {
                    base.Error();
                    return;
                }
                base.currentR = base.pathHandler.heap.Remove();
                if (i > 500)
                {
                    if (DateTime.UtcNow.Ticks >= targetTick)
                    {
                        return;
                    }
                    i = 0;
                    if (base.searchedNodes > 0xf4240)
                    {
                        throw new Exception("Probable infinite loop. Over 1,000,000 nodes searched");
                    }
                }
            }
            if (base.CompleteState == PathCompleteState.Complete)
            {
                this.ChangeEndNode(base.currentR.node);
                this.Trace(base.currentR);
            }
        }

        private void ChangeEndNode(GraphNode target)
        {
            if ((base.endNode != null) && (base.endNode != base.startNode))
            {
                PathNode pathNode = base.pathHandler.GetPathNode(base.endNode);
                bool flag = false;
                pathNode.flag2 = flag;
                pathNode.flag1 = flag;
            }
            base.endNode = target;
            base.endPoint = (Vector3) target.position;
        }

        protected override void CompletePathIfStartIsValidTarget()
        {
            PathNode pathNode = base.pathHandler.GetPathNode(base.startNode);
            if (this.endingCondition.TargetFound(pathNode))
            {
                this.ChangeEndNode(base.startNode);
                this.Trace(pathNode);
                base.CompleteState = PathCompleteState.Complete;
            }
        }

        public static XPath Construct(Vector3 start, Vector3 end, [Optional, DefaultParameterValue(null)] OnPathDelegate callback)
        {
            XPath p = PathPool.GetPath<XPath>();
            p.Setup(start, end, callback);
            p.endingCondition = new ABPathEndingCondition(p);
            return p;
        }

        protected override bool EndPointGridGraphSpecialCase(GraphNode endNode)
        {
            return false;
        }

        public override void Reset()
        {
            base.Reset();
            this.endingCondition = null;
        }
    }
}

