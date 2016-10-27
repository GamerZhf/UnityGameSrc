namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class HoverTransformAnimation : MonoBehaviour
    {
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        private bool m_enabled;
        private Vector3 m_origLocalPos;
        private Quaternion m_origLocalRotation;
        private Vector3 m_origLocalScale;
        private Vector2 m_randomTranslatePhase;
        public bool Rotate;
        public bool Scale;
        public bool Translate = true;
        public float WaveAmplitudeX = 4f;
        public float WaveAmplitudeY = 8f;
        public float WaveFrequencyX = 1f;
        public float WaveFrequencyY = 1.5f;
        public float WaveLengthX = 1f;
        public float WaveLengthY = 2f;

        protected void Awake()
        {
            this.Tm = base.transform;
            if (this.Tm != null)
            {
                this.cacheStartingOrientation();
            }
            this.m_randomTranslatePhase = new Vector2(UnityEngine.Random.Range((float) 0f, (float) 3.141593f), UnityEngine.Random.Range((float) 0f, (float) 3.141593f));
        }

        public void cacheStartingOrientation()
        {
            this.m_origLocalPos = base.transform.localPosition;
            this.m_origLocalScale = base.transform.localScale;
            this.m_origLocalRotation = base.transform.localRotation;
        }

        public ParameterSet getParameterSet()
        {
            ParameterSet set = new ParameterSet();
            set.WaveFrequencyX = this.WaveFrequencyX;
            set.WaveLengthX = this.WaveLengthX;
            set.WaveAmplitudeX = this.WaveAmplitudeX;
            set.WaveFrequencyY = this.WaveFrequencyY;
            set.WaveLengthY = this.WaveLengthY;
            set.WaveAmplitudeY = this.WaveAmplitudeY;
            set.Scale = this.Scale;
            set.Translate = this.Translate;
            set.Rotate = this.Rotate;
            return set;
        }

        public void loadParameterSet(ParameterSet parameters)
        {
            this.WaveFrequencyX = parameters.WaveFrequencyX;
            this.WaveLengthX = parameters.WaveLengthX;
            this.WaveAmplitudeX = parameters.WaveAmplitudeX;
            this.WaveFrequencyY = parameters.WaveFrequencyY;
            this.WaveLengthY = parameters.WaveLengthY;
            this.WaveAmplitudeY = parameters.WaveAmplitudeY;
            this.Scale = parameters.Scale;
            this.Translate = parameters.Translate;
            this.Rotate = parameters.Rotate;
        }

        protected void Update()
        {
            if (this.m_enabled)
            {
                if (this.Translate)
                {
                    float x = this.m_origLocalPos.x + (Mathf.Sin(((Time.unscaledTime * this.WaveFrequencyX) + this.m_randomTranslatePhase.x) + this.WaveLengthX) * this.WaveAmplitudeX);
                    float y = this.m_origLocalPos.y + (Mathf.Sin(((Time.unscaledTime * this.WaveFrequencyY) + this.m_randomTranslatePhase.y) + this.WaveLengthY) * this.WaveAmplitudeY);
                    this.Tm.localPosition = new Vector3(x, y, this.m_origLocalPos.z);
                }
                if (this.Scale)
                {
                    float num3 = this.m_origLocalScale.x + (Mathf.Sin((Time.unscaledTime * this.WaveFrequencyX) + this.WaveLengthX) * this.WaveAmplitudeX);
                    float num4 = this.m_origLocalScale.y + (Mathf.Sin((Time.unscaledTime * this.WaveFrequencyY) + this.WaveLengthY) * this.WaveAmplitudeY);
                    this.Tm.localScale = new Vector3(num3, num4, 0f);
                }
                if (this.Rotate)
                {
                    Vector3 eulerAngles = this.m_origLocalRotation.eulerAngles;
                    float z = eulerAngles.z + (Mathf.Sin((Time.unscaledTime * this.WaveFrequencyX) + this.WaveLengthX) * this.WaveAmplitudeX);
                    this.Tm.localRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, z);
                }
            }
        }

        public bool Enabled
        {
            get
            {
                return this.m_enabled;
            }
            set
            {
                this.m_enabled = value;
                if (!this.m_enabled && (this.Tm != null))
                {
                    this.Tm.localPosition = this.m_origLocalPos;
                    this.Tm.localScale = this.m_origLocalScale;
                }
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

        [StructLayout(LayoutKind.Sequential)]
        public struct ParameterSet
        {
            public float WaveFrequencyX;
            public float WaveLengthX;
            public float WaveAmplitudeX;
            public float WaveFrequencyY;
            public float WaveLengthY;
            public float WaveAmplitudeY;
            public bool Scale;
            public bool Translate;
            public bool Rotate;
        }
    }
}

