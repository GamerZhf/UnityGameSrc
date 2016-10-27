namespace PlayerView
{
    using System;
    using UnityEngine;

    public class RotateMenuTransformAnimation : MonoBehaviour
    {
        public float AngularVelocity;
        private RectTransform m_rectTm;

        protected void Awake()
        {
            this.m_rectTm = base.GetComponent<RectTransform>();
        }

        public void initialize()
        {
        }

        protected void LateUpdate()
        {
            this.m_rectTm.Rotate(Vector3.forward, (float) (this.AngularVelocity * Time.unscaledDeltaTime));
        }
    }
}

