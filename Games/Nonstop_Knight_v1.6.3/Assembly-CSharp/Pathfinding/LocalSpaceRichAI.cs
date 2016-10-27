namespace Pathfinding
{
    using System;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_local_space_rich_a_i.php")]
    public class LocalSpaceRichAI : RichAI
    {
        public LocalSpaceGraph graph;

        public override void UpdatePath()
        {
            base.canSearchPath = true;
            base.waitingForPathCalc = false;
            Path currentPath = base.seeker.GetCurrentPath();
            if ((currentPath != null) && !base.seeker.IsDone())
            {
                currentPath.Error();
                currentPath.Claim(this);
                currentPath.Release(this, false);
            }
            base.waitingForPathCalc = true;
            base.lastRepath = Time.time;
            Matrix4x4 matrix = this.graph.GetMatrix();
            Vector3 start = matrix.MultiplyPoint3x4(base.tr.position);
            base.seeker.StartPath(start, matrix.MultiplyPoint3x4(base.target.position));
        }

        protected override Vector3 UpdateTarget(RichFunnel fn)
        {
            bool flag;
            Matrix4x4 matrix = this.graph.GetMatrix();
            Matrix4x4 inverse = matrix.inverse;
            Debug.DrawRay(matrix.MultiplyPoint3x4(base.tr.position), (Vector3) (Vector3.up * 2f), Color.red);
            Debug.DrawRay(inverse.MultiplyPoint3x4(base.tr.position), (Vector3) (Vector3.up * 2f), Color.green);
            base.buffer.Clear();
            Vector3 position = base.tr.position;
            Vector3 vector2 = matrix.MultiplyPoint3x4(position);
            vector2 = fn.Update(vector2, base.buffer, 2, out this.lastCorner, out flag);
            position = inverse.MultiplyPoint3x4(vector2);
            Debug.DrawRay(position, (Vector3) (Vector3.up * 3f), Color.black);
            for (int i = 0; i < base.buffer.Count; i++)
            {
                base.buffer[i] = inverse.MultiplyPoint3x4(base.buffer[i]);
                Debug.DrawRay(base.buffer[i], (Vector3) (Vector3.up * 3f), Color.yellow);
            }
            if (flag && !base.waitingForPathCalc)
            {
                this.UpdatePath();
            }
            return position;
        }
    }
}

