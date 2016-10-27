namespace Pathfinding
{
    using System;
    using UnityEngine;

    public abstract class GridNodeBase : GraphNode
    {
        protected ushort gridFlags;
        private const int GridFlagsWalkableErosionMask = 0x100;
        private const int GridFlagsWalkableErosionOffset = 8;
        private const int GridFlagsWalkableTmpMask = 0x200;
        private const int GridFlagsWalkableTmpOffset = 9;
        protected int nodeInGridIndex;

        protected GridNodeBase(AstarPath astar) : base(astar)
        {
        }

        public override void AddConnection(GraphNode node, uint cost)
        {
            throw new NotImplementedException("GridNodes do not have support for adding manual connections with your current settings.\nPlease disable ASTAR_GRID_NO_CUSTOM_CONNECTIONS in the Optimizations tab in the A* Inspector");
        }

        public override Vector3 RandomPointOnSurface()
        {
            GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
            return (((Vector3) base.position) + ((Vector3) (new Vector3(UnityEngine.Random.value - 0.5f, 0f, UnityEngine.Random.value - 0.5f) * gridGraph.nodeSize)));
        }

        public override void RemoveConnection(GraphNode node)
        {
            throw new NotImplementedException("GridNodes do not have support for adding manual connections with your current settings.\nPlease disable ASTAR_GRID_NO_CUSTOM_CONNECTIONS in the Optimizations tab in the A* Inspector");
        }

        public override float SurfaceArea()
        {
            GridGraph gridGraph = GridNode.GetGridGraph(base.GraphIndex);
            return (gridGraph.nodeSize * gridGraph.nodeSize);
        }

        public int NodeInGridIndex
        {
            get
            {
                return this.nodeInGridIndex;
            }
            set
            {
                this.nodeInGridIndex = value;
            }
        }

        public bool TmpWalkable
        {
            get
            {
                return ((this.gridFlags & 0x200) != 0);
            }
            set
            {
                this.gridFlags = (ushort) ((this.gridFlags & -513) | (!value ? 0 : 0x200));
            }
        }

        public bool WalkableErosion
        {
            get
            {
                return ((this.gridFlags & 0x100) != 0);
            }
            set
            {
                this.gridFlags = (ushort) ((this.gridFlags & -257) | (!value ? 0 : 0x100));
            }
        }
    }
}

