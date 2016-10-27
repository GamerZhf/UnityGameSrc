namespace Pathfinding
{
    using System;
    using UnityEngine;

    public class RecastBBTreeBox
    {
        public RecastBBTreeBox c1;
        public RecastBBTreeBox c2;
        public RecastMeshObj mesh;
        public Rect rect;

        public RecastBBTreeBox(RecastMeshObj mesh)
        {
            this.mesh = mesh;
            Vector3 min = mesh.bounds.min;
            Vector3 max = mesh.bounds.max;
            this.rect = Rect.MinMaxRect(min.x, min.z, max.x, max.z);
        }

        public bool Contains(Vector3 p)
        {
            return this.rect.Contains(p);
        }
    }
}

