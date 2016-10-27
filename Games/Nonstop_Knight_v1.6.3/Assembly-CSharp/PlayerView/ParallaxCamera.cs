namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [ExecuteInEditMode]
    public class ParallaxCamera : MonoBehaviour
    {
        [CompilerGenerated]
        private UnityEngine.Camera <Camera>k__BackingField;
        private Vector3 m_prevPos;
        private Transform m_tm;
        public ParallaxCameraTranslateCallback OnCameraTranslated;

        protected void Awake()
        {
            this.initialize();
        }

        public void initialize()
        {
            this.m_tm = base.transform;
            this.Camera = base.GetComponent<UnityEngine.Camera>();
            this.m_prevPos = this.m_tm.position;
        }

        protected void Update()
        {
            if (this.m_tm.position != this.m_prevPos)
            {
                if (this.OnCameraTranslated != null)
                {
                    Vector3 deltaMovement = this.m_tm.position - this.m_prevPos;
                    this.OnCameraTranslated(deltaMovement);
                }
                this.m_prevPos = this.m_tm.position;
            }
        }

        public UnityEngine.Camera Camera
        {
            [CompilerGenerated]
            get
            {
                return this.<Camera>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Camera>k__BackingField = value;
            }
        }

        public delegate void ParallaxCameraTranslateCallback(Vector3 deltaMovement);
    }
}

