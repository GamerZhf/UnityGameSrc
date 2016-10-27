namespace Pathfinding
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable, AddComponentMenu("Pathfinding/Modifiers/Raycast Simplifier"), RequireComponent(typeof(Seeker)), HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_raycast_modifier.php")]
    public class RaycastModifier : MonoModifier
    {
        [Tooltip("How many iterations to try to simplify the path. If the path is changed in one iteration, the next iteration may find more simplification oppourtunities")]
        public int iterations = 2;
        public LayerMask mask = -1;
        [Tooltip("Offset from the original positions to perform the raycast.\nCan be useful to avoid the raycast intersecting the ground or similar things you do not want to it intersect")]
        public Vector3 raycastOffset = Vector3.zero;
        [Tooltip("Subdivides the path every iteration to be able to find shorter paths")]
        public bool subdivideEveryIter;
        [Tooltip("Checks around the line between two points, not just the exact line.\nMake sure the ground is either too far below or is not inside the mask since otherwise the raycast might always hit the ground.")]
        public bool thickRaycast;
        [Tooltip("Distance from the ray which will be checked for colliders")]
        public float thickRaycastRadius;
        [Tooltip("Use raycasting on the graphs. Only currently works with GridGraph and NavmeshGraph and RecastGraph. This is a pro version feature.")]
        public bool useGraphRaycasting;
        public bool useRaycasting = true;

        public override void Apply(Path p)
        {
            if (this.iterations > 0)
            {
                if (!this.useRaycasting && !this.useGraphRaycasting)
                {
                    Debug.LogWarning("RaycastModifier is configured to not use either raycasting or graph raycasting. This would simplify the path to a straight line. The modifier will not be applied.");
                }
                else
                {
                    List<Vector3> vectorPath = p.vectorPath;
                    for (int i = 0; i < this.iterations; i++)
                    {
                        if (this.subdivideEveryIter && (i != 0))
                        {
                            Subdivide(vectorPath);
                        }
                        int num2 = 0;
                        while (num2 < (vectorPath.Count - 2))
                        {
                            Vector3 vector = vectorPath[num2];
                            Vector3 vector2 = vectorPath[num2 + 2];
                            if (this.ValidateLine(null, null, vector, vector2))
                            {
                                vectorPath.RemoveAt(num2 + 1);
                            }
                            else
                            {
                                num2++;
                            }
                        }
                    }
                }
            }
        }

        private static void Subdivide(List<Vector3> points)
        {
            if (points.Capacity < (points.Count * 3))
            {
                points.Capacity = points.Count * 3;
            }
            int count = points.Count;
            for (int i = 0; i < (count - 1); i++)
            {
                points.Add(Vector3.zero);
                points.Add(Vector3.zero);
            }
            for (int j = count - 1; j > 0; j--)
            {
                Vector3 from = points[j];
                Vector3 to = points[j + 1];
                points[j * 3] = points[j];
                if (j != (count - 1))
                {
                    points[(j * 3) + 1] = Vector3.Lerp(from, to, 0.33f);
                    points[(j * 3) + 2] = Vector3.Lerp(from, to, 0.66f);
                }
            }
        }

        public bool ValidateLine(GraphNode n1, GraphNode n2, Vector3 v1, Vector3 v2)
        {
            if (this.useRaycasting)
            {
                if (this.thickRaycast && (this.thickRaycastRadius > 0f))
                {
                    Vector3 vector = v2 - v1;
                    if (Physics.SphereCast(new Ray(v1 + this.raycastOffset, v2 - v1), this.thickRaycastRadius, vector.magnitude, (int) this.mask))
                    {
                        return false;
                    }
                }
                else if (Physics.Linecast(v1 + this.raycastOffset, v2 + this.raycastOffset, (int) this.mask))
                {
                    return false;
                }
            }
            if (this.useGraphRaycasting && (n1 == null))
            {
                n1 = AstarPath.active.GetNearest(v1).node;
                n2 = AstarPath.active.GetNearest(v2).node;
            }
            if ((this.useGraphRaycasting && (n1 != null)) && (n2 != null))
            {
                NavGraph graph = AstarData.GetGraph(n1);
                NavGraph graph2 = AstarData.GetGraph(n2);
                if (graph != graph2)
                {
                    return false;
                }
                if (graph != null)
                {
                    IRaycastableGraph graph3 = graph as IRaycastableGraph;
                    if (graph3 != null)
                    {
                        return !graph3.Linecast(v1, v2, n1);
                    }
                }
            }
            return true;
        }

        public override int Order
        {
            get
            {
                return 40;
            }
        }
    }
}

