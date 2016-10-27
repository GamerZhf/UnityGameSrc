namespace Pathfinding
{
    using Pathfinding.Util;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class RichPath
    {
        private int currentPart;
        private readonly List<RichPathPart> parts = new List<RichPathPart>();
        public Seeker seeker;

        public RichPathPart GetCurrentPart()
        {
            return ((this.currentPart >= this.parts.Count) ? null : this.parts[this.currentPart]);
        }

        public void Initialize(Seeker s, Path p, bool mergePartEndpoints, RichFunnel.FunnelSimplification simplificationMode)
        {
            if (p.error)
            {
                throw new ArgumentException("Path has an error");
            }
            List<GraphNode> path = p.path;
            if (path.Count == 0)
            {
                throw new ArgumentException("Path traverses no nodes");
            }
            this.seeker = s;
            for (int i = 0; i < this.parts.Count; i++)
            {
                RichFunnel funnel = this.parts[i] as RichFunnel;
                RichSpecial special = this.parts[i] as RichSpecial;
                if (funnel != null)
                {
                    Pathfinding.Util.ObjectPool<RichFunnel>.Release(ref funnel);
                }
                else if (special != null)
                {
                    Pathfinding.Util.ObjectPool<RichSpecial>.Release(ref special);
                }
            }
            this.parts.Clear();
            this.currentPart = 0;
            for (int j = 0; j < path.Count; j++)
            {
                if (path[j] is TriangleMeshNode)
                {
                    NavGraph graph = AstarData.GetGraph(path[j]);
                    RichFunnel item = Pathfinding.Util.ObjectPool<RichFunnel>.Claim().Initialize(this, graph);
                    item.funnelSimplificationMode = simplificationMode;
                    int start = j;
                    uint graphIndex = path[start].GraphIndex;
                    while (j < path.Count)
                    {
                        if ((path[j].GraphIndex != graphIndex) && !(path[j] is NodeLink3Node))
                        {
                            break;
                        }
                        j++;
                    }
                    j--;
                    if (start == 0)
                    {
                        item.exactStart = p.vectorPath[0];
                    }
                    else
                    {
                        item.exactStart = (Vector3) path[!mergePartEndpoints ? start : (start - 1)].position;
                    }
                    if (j == (path.Count - 1))
                    {
                        item.exactEnd = p.vectorPath[p.vectorPath.Count - 1];
                    }
                    else
                    {
                        item.exactEnd = (Vector3) path[!mergePartEndpoints ? j : (j + 1)].position;
                    }
                    item.BuildFunnelCorridor(path, start, j);
                    this.parts.Add(item);
                    continue;
                }
                if (NodeLink2.GetNodeLink(path[j]) != null)
                {
                    NodeLink2 nodeLink = NodeLink2.GetNodeLink(path[j]);
                    int num5 = j;
                    uint num6 = path[num5].GraphIndex;
                    j++;
                    while (j < path.Count)
                    {
                        if (path[j].GraphIndex != num6)
                        {
                            break;
                        }
                        j++;
                    }
                    j--;
                    if ((j - num5) > 1)
                    {
                        throw new Exception("NodeLink2 path length greater than two (2) nodes. " + (j - num5));
                    }
                    if ((j - num5) != 0)
                    {
                        RichSpecial special2 = Pathfinding.Util.ObjectPool<RichSpecial>.Claim().Initialize(nodeLink, path[num5]);
                        this.parts.Add(special2);
                    }
                }
            }
        }

        public void NextPart()
        {
            this.currentPart++;
            if (this.currentPart >= this.parts.Count)
            {
                this.currentPart = this.parts.Count;
            }
        }

        public bool PartsLeft()
        {
            return (this.currentPart < this.parts.Count);
        }
    }
}

