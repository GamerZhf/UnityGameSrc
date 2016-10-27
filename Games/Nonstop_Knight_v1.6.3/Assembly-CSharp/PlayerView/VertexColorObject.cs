namespace PlayerView
{
    using System;
    using UnityEngine;

    public class VertexColorObject
    {
        private Mesh m_mesh;
        private Color[] m_vertexColors;

        public VertexColorObject(Mesh mesh)
        {
            this.m_mesh = mesh;
            this.m_vertexColors = new Color[mesh.vertexCount];
        }

        public void setColor(Color c)
        {
            for (int i = 0; i < this.m_mesh.vertexCount; i++)
            {
                this.m_vertexColors[i] = c;
            }
            this.m_mesh.colors = this.m_vertexColors;
        }
    }
}

