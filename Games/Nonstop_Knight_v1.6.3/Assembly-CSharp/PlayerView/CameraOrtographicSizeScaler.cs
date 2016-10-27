namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [ExecuteInEditMode]
    public class CameraOrtographicSizeScaler : MonoBehaviour
    {
        [CompilerGenerated]
        private UnityEngine.Camera <Camera>k__BackingField;
        private int m_activeCamPixelHeight;
        private int m_activeCamPixelWidth;
        public float RefenceAspectRatio = 1.777778f;
        public float ReferenceOrtographicSize = 7.68f;

        protected void Awake()
        {
            this.Camera = base.GetComponent<UnityEngine.Camera>();
            this.refresh();
        }

        public void refresh()
        {
            this.Camera.orthographicSize = (this.RefenceAspectRatio * this.ReferenceOrtographicSize) / (((float) this.Camera.pixelWidth) / ((float) this.Camera.pixelHeight));
            this.m_activeCamPixelHeight = this.Camera.pixelHeight;
            this.m_activeCamPixelWidth = this.Camera.pixelWidth;
        }

        protected void Update()
        {
            if ((this.Camera.pixelWidth != this.m_activeCamPixelWidth) || (this.Camera.pixelHeight != this.m_activeCamPixelHeight))
            {
                this.refresh();
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
    }
}

