namespace Pathfinding.RVO
{
    using System;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_r_v_o_1_1_r_v_o_square_obstacle.php"), AddComponentMenu("Pathfinding/Local Avoidance/Square Obstacle")]
    public class RVOSquareObstacle : RVOObstacle
    {
        public Vector2 center = Vector3.one;
        public float height = 1f;
        public Vector2 size = Vector3.one;

        protected override bool AreGizmosDirty()
        {
            return false;
        }

        protected override void CreateObstacles()
        {
            this.size.x = Mathf.Abs(this.size.x);
            this.size.y = Mathf.Abs(this.size.y);
            this.height = Mathf.Abs(this.height);
            Vector3[] vertices = new Vector3[] { new Vector3(1f, 0f, -1f), new Vector3(1f, 0f, 1f), new Vector3(-1f, 0f, 1f), new Vector3(-1f, 0f, -1f) };
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Scale(new Vector3(this.size.x * 0.5f, 0f, this.size.y * 0.5f));
                vertices[i] += new Vector3(this.center.x, 0f, this.center.y);
            }
            base.AddObstacle(vertices, this.height);
        }

        protected override bool ExecuteInEditor
        {
            get
            {
                return true;
            }
        }

        protected override float Height
        {
            get
            {
                return this.height;
            }
        }

        protected override bool LocalCoordinates
        {
            get
            {
                return true;
            }
        }

        protected override bool StaticObstacle
        {
            get
            {
                return false;
            }
        }
    }
}

