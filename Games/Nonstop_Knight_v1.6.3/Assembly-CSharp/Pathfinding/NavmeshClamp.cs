namespace Pathfinding
{
    using System;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_navmesh_clamp.php")]
    public class NavmeshClamp : MonoBehaviour
    {
        private GraphNode prevNode;
        private Vector3 prevPos;

        private void LateUpdate()
        {
            if (this.prevNode == null)
            {
                NNInfo nearest = AstarPath.active.GetNearest(base.transform.position);
                this.prevNode = nearest.node;
                this.prevPos = base.transform.position;
            }
            if (this.prevNode != null)
            {
                if (this.prevNode != null)
                {
                    IRaycastableGraph graph = AstarData.GetGraph(this.prevNode) as IRaycastableGraph;
                    if (graph != null)
                    {
                        GraphHitInfo info2;
                        if (graph.Linecast(this.prevPos, base.transform.position, this.prevNode, out info2))
                        {
                            info2.point.y = base.transform.position.y;
                            Vector3 end = VectorMath.ClosestPointOnLine(info2.tangentOrigin, info2.tangentOrigin + info2.tangent, base.transform.position);
                            Vector3 point = info2.point;
                            point += Vector3.ClampMagnitude(((Vector3) info2.node.position) - point, 0.008f);
                            if (graph.Linecast(point, end, info2.node, out info2))
                            {
                                info2.point.y = base.transform.position.y;
                                base.transform.position = info2.point;
                            }
                            else
                            {
                                end.y = base.transform.position.y;
                                base.transform.position = end;
                            }
                        }
                        this.prevNode = info2.node;
                    }
                }
                this.prevPos = base.transform.position;
            }
        }
    }
}

