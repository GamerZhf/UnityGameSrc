namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class PoolableAudioSource : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private UnityEngine.AudioSource <AudioSource>k__BackingField;
        [CompilerGenerated]
        private PlayerView.AudioSourceType <AudioSourceType>k__BackingField;
        [CompilerGenerated]
        private float <OrigPitch>k__BackingField;
        [CompilerGenerated]
        private float <OrigVolume>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;

        protected void Awake()
        {
            this.Tm = base.transform;
            this.AudioSource = base.GetComponent<UnityEngine.AudioSource>();
            if (this.AudioSource != null)
            {
                this.OrigPitch = this.AudioSource.pitch;
                this.OrigVolume = this.AudioSource.volume;
            }
        }

        public void cleanUpForReuse()
        {
            if (this.AudioSource != null)
            {
                this.AudioSource.pitch = this.OrigPitch;
                this.AudioSource.volume = this.OrigVolume;
            }
        }

        public UnityEngine.AudioSource AudioSource
        {
            [CompilerGenerated]
            get
            {
                return this.<AudioSource>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<AudioSource>k__BackingField = value;
            }
        }

        public PlayerView.AudioSourceType AudioSourceType
        {
            [CompilerGenerated]
            get
            {
                return this.<AudioSourceType>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<AudioSourceType>k__BackingField = value;
            }
        }

        public float OrigPitch
        {
            [CompilerGenerated]
            get
            {
                return this.<OrigPitch>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<OrigPitch>k__BackingField = value;
            }
        }

        public float OrigVolume
        {
            [CompilerGenerated]
            get
            {
                return this.<OrigVolume>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<OrigVolume>k__BackingField = value;
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

