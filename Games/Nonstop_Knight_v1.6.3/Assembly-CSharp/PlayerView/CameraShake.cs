namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CameraShake : MonoBehaviour
    {
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        private float m_decay;
        private float m_intensity;
        private Vector3 m_startPos;
        private Quaternion m_startRot;

        protected void Awake()
        {
            this.Tm = base.transform;
        }

        public void endShake()
        {
            this.m_intensity = 0f;
        }

        public void shake(float intensity, float decay)
        {
            if (!this.Shaking)
            {
                this.m_intensity = intensity;
                this.m_decay = decay * 60f;
                this.m_startPos = this.Tm.position;
                this.m_startRot = this.Tm.rotation;
            }
        }

        [ContextMenu("testShake")]
        private void testShake()
        {
            this.shake(0.25f, 0.015f);
        }

        protected void Update()
        {
            if (this.m_intensity > 0f)
            {
                this.Tm.position = this.m_startPos + ((Vector3) (UnityEngine.Random.insideUnitSphere * this.m_intensity));
                this.Tm.rotation = new Quaternion(this.m_startRot.x + (UnityEngine.Random.Range(-this.m_intensity, this.m_intensity) * 0.2f), this.m_startRot.y + (UnityEngine.Random.Range(-this.m_intensity, this.m_intensity) * 0.2f), this.m_startRot.z + (UnityEngine.Random.Range(-this.m_intensity, this.m_intensity) * 0.2f), this.m_startRot.w + (UnityEngine.Random.Range(-this.m_intensity, this.m_intensity) * 0.2f));
                this.m_intensity -= this.m_decay * Time.deltaTime;
                if (this.m_intensity <= 0f)
                {
                    this.Tm.rotation = this.m_startRot;
                }
            }
        }

        public bool Shaking
        {
            get
            {
                return (this.m_intensity > 0f);
            }
        }

        public Transform Tm
        {
            [CompilerGenerated]
            get
            {
                return this.<Tm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Tm>k__BackingField = value;
            }
        }
    }
}

