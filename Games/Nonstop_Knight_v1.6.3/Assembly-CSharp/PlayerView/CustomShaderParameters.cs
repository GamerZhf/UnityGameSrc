namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [ExecuteInEditMode]
    public class CustomShaderParameters : MonoBehaviour
    {
        [CompilerGenerated]
        private Color <BackgroundColor>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.Camera <Camera>k__BackingField;
        [CompilerGenerated]
        private float <FloorAndDecoMoodContribution>k__BackingField;
        [CompilerGenerated]
        private bool <FogEnabled>k__BackingField;
        [CompilerGenerated]
        private Color <HeroLightColor>k__BackingField;
        [CompilerGenerated]
        private float <HeroLightIntensity>k__BackingField;
        [CompilerGenerated]
        private float <HeroLightRange>k__BackingField;
        [CompilerGenerated]
        private Vector3 <HeroLightWorldPos>k__BackingField;
        [CompilerGenerated]
        private float <HorizontalFogEndTerm>k__BackingField;
        [CompilerGenerated]
        private float <HorizontalFogStartTerm>k__BackingField;
        [CompilerGenerated]
        private Color <PropColor>k__BackingField;

        protected void Awake()
        {
            this.Camera = base.gameObject.GetComponent<UnityEngine.Camera>();
            this.FogEnabled = true;
        }

        protected void OnPreCull()
        {
            if (this.FogEnabled)
            {
                float num = -1f / (this.HorizontalFogEndTerm - this.HorizontalFogStartTerm);
                float num2 = this.HorizontalFogEndTerm / (this.HorizontalFogEndTerm - this.HorizontalFogStartTerm);
                Shader.SetGlobalFloat("_CustomFogTerm1", num);
                Shader.SetGlobalFloat("_CustomFogTerm2", num2);
            }
            else
            {
                Shader.SetGlobalFloat("_CustomFogTerm1", float.MaxValue);
                Shader.SetGlobalFloat("_CustomFogTerm2", float.MaxValue);
            }
            Shader.SetGlobalColor("_CameraBackgroundColor", this.BackgroundColor);
            Shader.SetGlobalVector("_HeroLightWorldPos", this.HeroLightWorldPos);
            Shader.SetGlobalVector("_HeroLightColor", (Vector4) this.HeroLightColor);
            Shader.SetGlobalFloat("_HeroLightIntensity", this.HeroLightIntensity);
            Shader.SetGlobalFloat("_HeroLightRange", this.HeroLightRange);
            Shader.SetGlobalColor("_PropColor", this.PropColor);
            Shader.SetGlobalFloat("_FloorAndDecoMoodContribution", this.FloorAndDecoMoodContribution);
        }

        public Color BackgroundColor
        {
            [CompilerGenerated]
            get
            {
                return this.<BackgroundColor>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<BackgroundColor>k__BackingField = value;
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

        public float FloorAndDecoMoodContribution
        {
            [CompilerGenerated]
            get
            {
                return this.<FloorAndDecoMoodContribution>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<FloorAndDecoMoodContribution>k__BackingField = value;
            }
        }

        public bool FogEnabled
        {
            [CompilerGenerated]
            get
            {
                return this.<FogEnabled>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<FogEnabled>k__BackingField = value;
            }
        }

        public Color HeroLightColor
        {
            [CompilerGenerated]
            get
            {
                return this.<HeroLightColor>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<HeroLightColor>k__BackingField = value;
            }
        }

        public float HeroLightIntensity
        {
            [CompilerGenerated]
            get
            {
                return this.<HeroLightIntensity>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<HeroLightIntensity>k__BackingField = value;
            }
        }

        public float HeroLightRange
        {
            [CompilerGenerated]
            get
            {
                return this.<HeroLightRange>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<HeroLightRange>k__BackingField = value;
            }
        }

        public Vector3 HeroLightWorldPos
        {
            [CompilerGenerated]
            get
            {
                return this.<HeroLightWorldPos>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<HeroLightWorldPos>k__BackingField = value;
            }
        }

        public float HorizontalFogEndTerm
        {
            [CompilerGenerated]
            get
            {
                return this.<HorizontalFogEndTerm>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<HorizontalFogEndTerm>k__BackingField = value;
            }
        }

        public float HorizontalFogStartTerm
        {
            [CompilerGenerated]
            get
            {
                return this.<HorizontalFogStartTerm>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<HorizontalFogStartTerm>k__BackingField = value;
            }
        }

        public Color PropColor
        {
            [CompilerGenerated]
            get
            {
                return this.<PropColor>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<PropColor>k__BackingField = value;
            }
        }
    }
}

