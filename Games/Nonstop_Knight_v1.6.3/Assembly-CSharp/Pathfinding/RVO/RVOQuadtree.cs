namespace Pathfinding.RVO
{
    using Pathfinding.RVO.Sampled;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class RVOQuadtree
    {
        private Rect bounds;
        private int filledNodes = 1;
        private const int LeafSize = 15;
        private float maxRadius;
        private Node[] nodes = new Node[0x2a];

        public void Clear()
        {
            this.nodes[0] = new Node();
            this.filledNodes = 1;
            this.maxRadius = 0f;
        }

        public void DebugDraw()
        {
            this.DebugDrawRec(0, this.bounds);
        }

        private void DebugDrawRec(int i, Rect r)
        {
            Debug.DrawLine(new Vector3(r.xMin, 0f, r.yMin), new Vector3(r.xMax, 0f, r.yMin), Color.white);
            Debug.DrawLine(new Vector3(r.xMax, 0f, r.yMin), new Vector3(r.xMax, 0f, r.yMax), Color.white);
            Debug.DrawLine(new Vector3(r.xMax, 0f, r.yMax), new Vector3(r.xMin, 0f, r.yMax), Color.white);
            Debug.DrawLine(new Vector3(r.xMin, 0f, r.yMax), new Vector3(r.xMin, 0f, r.yMin), Color.white);
            if (this.nodes[i].child00 != i)
            {
                Vector2 center = r.center;
                this.DebugDrawRec(this.nodes[i].child11, Rect.MinMaxRect(center.x, center.y, r.xMax, r.yMax));
                this.DebugDrawRec(this.nodes[i].child10, Rect.MinMaxRect(center.x, r.yMin, r.xMax, center.y));
                this.DebugDrawRec(this.nodes[i].child01, Rect.MinMaxRect(r.xMin, center.y, center.x, r.yMax));
                this.DebugDrawRec(this.nodes[i].child00, Rect.MinMaxRect(r.xMin, r.yMin, center.x, center.y));
            }
            for (Agent agent = this.nodes[i].linkedList; agent != null; agent = agent.next)
            {
                Debug.DrawLine(this.nodes[i].linkedList.position + Vector3.up, agent.position + Vector3.up, new Color(1f, 1f, 0f, 0.5f));
            }
        }

        public int GetNodeIndex()
        {
            if (this.filledNodes == this.nodes.Length)
            {
                Node[] nodeArray = new Node[this.nodes.Length * 2];
                for (int i = 0; i < this.nodes.Length; i++)
                {
                    nodeArray[i] = this.nodes[i];
                }
                this.nodes = nodeArray;
            }
            this.nodes[this.filledNodes] = new Node();
            this.nodes[this.filledNodes].child00 = this.filledNodes;
            this.filledNodes++;
            return (this.filledNodes - 1);
        }

        public void Insert(Agent agent)
        {
            int index = 0;
            Rect bounds = this.bounds;
            Vector2 vector = new Vector2(agent.position.x, agent.position.z);
            agent.next = null;
            this.maxRadius = Math.Max(agent.radius, this.maxRadius);
            int num2 = 0;
            while (true)
            {
                num2++;
                if (this.nodes[index].child00 == index)
                {
                    if ((this.nodes[index].count < 15) || (num2 > 10))
                    {
                        this.nodes[index].Add(agent);
                        this.nodes[index].count = (byte) (this.nodes[index].count + 1);
                        return;
                    }
                    Node node = this.nodes[index];
                    node.child00 = this.GetNodeIndex();
                    node.child01 = this.GetNodeIndex();
                    node.child10 = this.GetNodeIndex();
                    node.child11 = this.GetNodeIndex();
                    this.nodes[index] = node;
                    this.nodes[index].Distribute(this.nodes, bounds);
                }
                if (this.nodes[index].child00 != index)
                {
                    Vector2 center = bounds.center;
                    if (vector.x > center.x)
                    {
                        if (vector.y > center.y)
                        {
                            index = this.nodes[index].child11;
                            bounds = Rect.MinMaxRect(center.x, center.y, bounds.xMax, bounds.yMax);
                        }
                        else
                        {
                            index = this.nodes[index].child10;
                            bounds = Rect.MinMaxRect(center.x, bounds.yMin, bounds.xMax, center.y);
                        }
                    }
                    else if (vector.y > center.y)
                    {
                        index = this.nodes[index].child01;
                        bounds = Rect.MinMaxRect(bounds.xMin, center.y, center.x, bounds.yMax);
                    }
                    else
                    {
                        index = this.nodes[index].child00;
                        bounds = Rect.MinMaxRect(bounds.xMin, bounds.yMin, center.x, center.y);
                    }
                }
            }
        }

        public void Query(Vector2 p, float radius, Agent agent)
        {
            this.QueryRec(0, p, radius, agent, this.bounds);
        }

        private float QueryRec(int i, Vector2 p, float radius, Agent agent, Rect r)
        {
            if (this.nodes[i].child00 == i)
            {
                for (Agent agent2 = this.nodes[i].linkedList; agent2 != null; agent2 = agent2.next)
                {
                    float f = agent.InsertAgentNeighbour(agent2, radius * radius);
                    if (f < (radius * radius))
                    {
                        radius = Mathf.Sqrt(f);
                    }
                }
                return radius;
            }
            Vector2 center = r.center;
            if ((p.x - radius) < center.x)
            {
                if ((p.y - radius) < center.y)
                {
                    radius = this.QueryRec(this.nodes[i].child00, p, radius, agent, Rect.MinMaxRect(r.xMin, r.yMin, center.x, center.y));
                }
                if ((p.y + radius) > center.y)
                {
                    radius = this.QueryRec(this.nodes[i].child01, p, radius, agent, Rect.MinMaxRect(r.xMin, center.y, center.x, r.yMax));
                }
            }
            if ((p.x + radius) > center.x)
            {
                if ((p.y - radius) < center.y)
                {
                    radius = this.QueryRec(this.nodes[i].child10, p, radius, agent, Rect.MinMaxRect(center.x, r.yMin, r.xMax, center.y));
                }
                if ((p.y + radius) > center.y)
                {
                    radius = this.QueryRec(this.nodes[i].child11, p, radius, agent, Rect.MinMaxRect(center.x, center.y, r.xMax, r.yMax));
                }
            }
            return radius;
        }

        public void SetBounds(Rect r)
        {
            this.bounds = r;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Node
        {
            public int child00;
            public int child01;
            public int child10;
            public int child11;
            public byte count;
            public Agent linkedList;
            public void Add(Agent agent)
            {
                agent.next = this.linkedList;
                this.linkedList = agent;
            }

            public void Distribute(RVOQuadtree.Node[] nodes, Rect r)
            {
                Vector2 center = r.center;
                while (this.linkedList != null)
                {
                    Agent next = this.linkedList.next;
                    if (this.linkedList.position.x > center.x)
                    {
                        if (this.linkedList.position.z > center.y)
                        {
                            nodes[this.child11].Add(this.linkedList);
                        }
                        else
                        {
                            nodes[this.child10].Add(this.linkedList);
                        }
                    }
                    else if (this.linkedList.position.z > center.y)
                    {
                        nodes[this.child01].Add(this.linkedList);
                    }
                    else
                    {
                        nodes[this.child00].Add(this.linkedList);
                    }
                    this.linkedList = next;
                }
                this.count = 0;
            }
        }
    }
}

