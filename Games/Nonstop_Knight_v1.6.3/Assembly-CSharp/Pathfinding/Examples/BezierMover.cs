namespace Pathfinding.Examples
{
    using Pathfinding;
    using System;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_bezier_mover.php")]
    public class BezierMover : MonoBehaviour
    {
        public Transform[] points;
        public float speed = 1f;
        public float tangentLengths = 5f;
        private float time;

        private void Move(bool progress)
        {
            float time = this.time;
            float num2 = this.time + 1f;
            while ((num2 - time) > 0.0001f)
            {
                float t = (time + num2) / 2f;
                Vector3 vector4 = this.Plot(t) - base.transform.position;
                if (vector4.sqrMagnitude > ((this.speed * Time.deltaTime) * (this.speed * Time.deltaTime)))
                {
                    num2 = t;
                }
                else
                {
                    time = t;
                }
            }
            this.time = (time + num2) / 2f;
            Vector3 vector2 = this.Plot(this.time);
            Vector3 vector3 = this.Plot(this.time + 0.001f);
            base.transform.position = vector2;
            base.transform.rotation = Quaternion.LookRotation(vector3 - vector2);
        }

        public void OnDrawGizmos()
        {
            if (this.points.Length >= 3)
            {
                for (int i = 0; i < this.points.Length; i++)
                {
                    if (this.points[i] == null)
                    {
                        return;
                    }
                }
                for (int j = 0; j < this.points.Length; j++)
                {
                    int length = this.points.Length;
                    Vector3 vector5 = this.points[(j + 1) % length].position - this.points[j].position;
                    Vector3 vector6 = this.points[((j - 1) + length) % length].position - this.points[j].position;
                    Vector3 vector7 = vector5.normalized - vector6.normalized;
                    Vector3 normalized = vector7.normalized;
                    Vector3 vector8 = this.points[(j + 2) % length].position - this.points[(j + 1) % length].position;
                    Vector3 vector9 = this.points[(j + length) % length].position - this.points[(j + 1) % length].position;
                    Vector3 vector10 = vector8.normalized - vector9.normalized;
                    Vector3 vector2 = vector10.normalized;
                    Vector3 position = this.points[j].position;
                    for (int k = 1; k <= 100; k++)
                    {
                        Vector3 to = AstarSplines.CubicBezier(this.points[j].position, this.points[j].position + ((Vector3) (normalized * this.tangentLengths)), this.points[(j + 1) % length].position - ((Vector3) (vector2 * this.tangentLengths)), this.points[(j + 1) % length].position, ((float) k) / 100f);
                        Gizmos.DrawLine(position, to);
                        position = to;
                    }
                }
            }
        }

        private Vector3 Plot(float t)
        {
            int length = this.points.Length;
            int num2 = Mathf.FloorToInt(t);
            Vector3 vector3 = this.points[(num2 + 1) % length].position - this.points[num2 % length].position;
            Vector3 vector4 = this.points[((num2 - 1) + length) % length].position - this.points[num2 % length].position;
            Vector3 vector5 = vector3.normalized - vector4.normalized;
            Vector3 normalized = vector5.normalized;
            Vector3 vector6 = this.points[(num2 + 2) % length].position - this.points[(num2 + 1) % length].position;
            Vector3 vector7 = this.points[(num2 + length) % length].position - this.points[(num2 + 1) % length].position;
            Vector3 vector8 = vector6.normalized - vector7.normalized;
            Vector3 vector2 = vector8.normalized;
            Debug.DrawLine(this.points[num2 % length].position, this.points[num2 % length].position + ((Vector3) (normalized * this.tangentLengths)), Color.red);
            Debug.DrawLine(this.points[(num2 + 1) % length].position - ((Vector3) (vector2 * this.tangentLengths)), this.points[(num2 + 1) % length].position, Color.green);
            return AstarSplines.CubicBezier(this.points[num2 % length].position, this.points[num2 % length].position + ((Vector3) (normalized * this.tangentLengths)), this.points[(num2 + 1) % length].position - ((Vector3) (vector2 * this.tangentLengths)), this.points[(num2 + 1) % length].position, t - num2);
        }

        private void Update()
        {
            this.Move(true);
        }
    }
}

