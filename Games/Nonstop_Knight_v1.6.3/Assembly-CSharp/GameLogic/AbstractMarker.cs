namespace GameLogic
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode]
    public abstract class AbstractMarker : MonoBehaviour
    {
        public Color color = new Color(1f, 1f, 1f, 0.7f);
        public bool customSize;
        public float customSphereRadius;
        public Vector3 m_center = Vector3.zero;
        public Shape shape = Shape.CUBE;
        public bool wire;

        protected AbstractMarker()
        {
        }

        protected virtual void onDrawGizmos()
        {
        }

        protected virtual void onRenderObject()
        {
        }

        public enum Shape
        {
            NONE,
            CUBE,
            SPHERE
        }
    }
}

