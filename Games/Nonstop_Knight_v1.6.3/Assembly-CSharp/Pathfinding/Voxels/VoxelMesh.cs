namespace Pathfinding.Voxels
{
    using Pathfinding;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct VoxelMesh
    {
        public Int3[] verts;
        public int[] tris;
        public int[] areas;
    }
}

