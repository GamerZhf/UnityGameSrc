namespace Pathfinding.Voxels
{
    using System;
    using UnityEngine;

    public class RasterizationMesh
    {
        public int area;
        public Bounds bounds;
        public Matrix4x4 matrix;
        public MeshFilter original;
        public int[] triangles;
        public Vector3[] vertices;

        public RasterizationMesh()
        {
        }

        public RasterizationMesh(Vector3[] vertices, int[] triangles, Bounds bounds)
        {
            this.matrix = Matrix4x4.identity;
            this.vertices = vertices;
            this.triangles = triangles;
            this.bounds = bounds;
            this.original = null;
            this.area = 0;
        }

        public RasterizationMesh(Vector3[] vertices, int[] triangles, Bounds bounds, Matrix4x4 matrix)
        {
            this.matrix = matrix;
            this.vertices = vertices;
            this.triangles = triangles;
            this.bounds = bounds;
            this.original = null;
            this.area = 0;
        }

        public void RecalculateBounds()
        {
            Bounds bounds = new Bounds(this.matrix.MultiplyPoint3x4(this.vertices[0]), Vector3.zero);
            for (int i = 1; i < this.vertices.Length; i++)
            {
                bounds.Encapsulate(this.matrix.MultiplyPoint3x4(this.vertices[i]));
            }
            this.bounds = bounds;
        }
    }
}

