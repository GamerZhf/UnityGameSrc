namespace Pathfinding.Examples
{
    using System;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_astar_smooth_follow2.php")]
    public class AstarSmoothFollow2 : MonoBehaviour
    {
        public float damping = 5f;
        public float distance = 3f;
        public bool followBehind = true;
        public float height = 3f;
        public float rotationDamping = 10f;
        public bool smoothRotation = true;
        public bool staticOffset;
        public Transform target;

        private void LateUpdate()
        {
            Vector3 vector;
            if (this.staticOffset)
            {
                vector = this.target.position + new Vector3(0f, this.height, this.distance);
            }
            else if (this.followBehind)
            {
                vector = this.target.TransformPoint(0f, this.height, -this.distance);
            }
            else
            {
                vector = this.target.TransformPoint(0f, this.height, this.distance);
            }
            base.transform.position = Vector3.Lerp(base.transform.position, vector, Time.deltaTime * this.damping);
            if (this.smoothRotation)
            {
                Quaternion to = Quaternion.LookRotation(this.target.position - base.transform.position, this.target.up);
                base.transform.rotation = Quaternion.Slerp(base.transform.rotation, to, Time.deltaTime * this.rotationDamping);
            }
            else
            {
                base.transform.LookAt(this.target, this.target.up);
            }
        }
    }
}

