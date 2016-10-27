namespace Pathfinding.RVO
{
    using Pathfinding;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_r_v_o_1_1_r_v_o_navmesh.php"), AddComponentMenu("Pathfinding/Local Avoidance/RVO Navmesh")]
    public class RVONavmesh : GraphModifier
    {
        private Simulator lastSim;
        private readonly List<ObstacleVertex> obstacles = new List<ObstacleVertex>();
        public float wallHeight = 5f;

        public void AddGraphObstacles(Simulator sim, NavGraph graph)
        {
            <AddGraphObstacles>c__AnonStorey25D storeyd = new <AddGraphObstacles>c__AnonStorey25D();
            storeyd.sim = sim;
            storeyd.<>f__this = this;
            if (((this.obstacles.Count > 0) && (this.lastSim != null)) && (this.lastSim != storeyd.sim))
            {
                Debug.LogError("Simulator has changed but some old obstacles are still added for the previous simulator. Deleting previous obstacles.");
                this.RemoveObstacles();
            }
            this.lastSim = storeyd.sim;
            INavmesh navmesh = graph as INavmesh;
            if (navmesh != null)
            {
                storeyd.uses = new int[20];
                navmesh.GetNodes(new GraphNodeDelegateCancelable(storeyd.<>m__36));
            }
        }

        public override void OnLatePostScan()
        {
            if (Application.isPlaying)
            {
                this.RemoveObstacles();
                NavGraph[] graphs = AstarPath.active.graphs;
                RVOSimulator simulator = UnityEngine.Object.FindObjectOfType(typeof(RVOSimulator)) as RVOSimulator;
                if (simulator == null)
                {
                    throw new NullReferenceException("No RVOSimulator could be found in the scene. Please add one to any GameObject");
                }
                Simulator sim = simulator.GetSimulator();
                for (int i = 0; i < graphs.Length; i++)
                {
                    this.AddGraphObstacles(sim, graphs[i]);
                }
                sim.UpdateObstacles();
            }
        }

        public override void OnPostCacheLoad()
        {
            this.OnLatePostScan();
        }

        public void RemoveObstacles()
        {
            if (this.lastSim != null)
            {
                Simulator lastSim = this.lastSim;
                this.lastSim = null;
                for (int i = 0; i < this.obstacles.Count; i++)
                {
                    lastSim.RemoveObstacle(this.obstacles[i]);
                }
                this.obstacles.Clear();
            }
        }

        [CompilerGenerated]
        private sealed class <AddGraphObstacles>c__AnonStorey25D
        {
            internal RVONavmesh <>f__this;
            internal Simulator sim;
            internal int[] uses;

            internal bool <>m__36(GraphNode _node)
            {
                int num5;
                TriangleMeshNode node = _node as TriangleMeshNode;
                this.uses[2] = num5 = 0;
                this.uses[0] = this.uses[1] = num5;
                if (node != null)
                {
                    for (int i = 0; i < node.connections.Length; i++)
                    {
                        TriangleMeshNode other = node.connections[i] as TriangleMeshNode;
                        if (other != null)
                        {
                            int index = node.SharedEdge(other);
                            if (index != -1)
                            {
                                this.uses[index] = 1;
                            }
                        }
                    }
                    for (int j = 0; j < 3; j++)
                    {
                        if (this.uses[j] == 0)
                        {
                            Vector3 vertex = (Vector3) node.GetVertex(j);
                            Vector3 b = (Vector3) node.GetVertex((j + 1) % node.GetVertexCount());
                            float num4 = Math.Max(Math.Abs((float) (vertex.y - b.y)), 5f);
                            this.<>f__this.obstacles.Add(this.sim.AddObstacle(vertex, b, this.<>f__this.wallHeight));
                        }
                    }
                }
                return true;
            }
        }
    }
}

