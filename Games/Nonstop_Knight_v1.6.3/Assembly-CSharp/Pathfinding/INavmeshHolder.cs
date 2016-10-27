namespace Pathfinding
{
    using System;
    using System.Runtime.InteropServices;

    public interface INavmeshHolder
    {
        void GetTileCoordinates(int tileIndex, out int x, out int z);
        Int3 GetVertex(int i);
        int GetVertexArrayIndex(int index);
    }
}

