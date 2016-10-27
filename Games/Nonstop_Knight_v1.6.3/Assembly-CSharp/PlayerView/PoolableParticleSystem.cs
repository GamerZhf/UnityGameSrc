namespace PlayerView
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class PoolableParticleSystem : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private PlayerView.EffectType <EffectType>k__BackingField;
        [CompilerGenerated]
        private Transform <FollowTm>k__BackingField;
        [CompilerGenerated]
        private float <ManualDurationTimer>k__BackingField;
        [CompilerGenerated]
        private bool <ManualSimulationWithUnscaledTime>k__BackingField;
        [CompilerGenerated]
        private int <OriginalLayer>k__BackingField;
        [CompilerGenerated]
        private bool <OriginalLoopable>k__BackingField;
        [CompilerGenerated]
        private float <OriginalStartRotation>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.ParticleSystem <ParticleSystem>k__BackingField;
        [CompilerGenerated]
        private ParticleSystemColor <ParticleSystemColor>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        private List<UnityEngine.ParticleSystem> m_allParticleSystemsIncludingChildren;
        private Dictionary<UnityEngine.ParticleSystem, UnityEngine.ParticleSystem.Particle[]> m_fadeOutParticles;
        private ManualTimer m_fadeOutTimer = new ManualTimer();
        private Dictionary<UnityEngine.ParticleSystem, float> m_originalStartSizes;
        public const float PARTICLE_FADE_OUT_DURATION = 0.3f;

        protected void Awake()
        {
            this.Tm = base.transform;
            this.ParticleSystem = base.GetComponent<UnityEngine.ParticleSystem>();
            if (this.ParticleSystem == null)
            {
                this.ParticleSystem = base.GetComponentInChildren<UnityEngine.ParticleSystem>();
            }
            this.OriginalLayer = this.ParticleSystem.gameObject.layer;
            UnityEngine.ParticleSystem[] componentsInChildren = base.GetComponentsInChildren<UnityEngine.ParticleSystem>();
            this.m_allParticleSystemsIncludingChildren = new List<UnityEngine.ParticleSystem>(1 + componentsInChildren.Length);
            this.m_allParticleSystemsIncludingChildren.Add(this.ParticleSystem);
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                UnityEngine.ParticleSystem item = componentsInChildren[i];
                item.playOnAwake = false;
                if (!this.m_allParticleSystemsIncludingChildren.Contains(item))
                {
                    this.m_allParticleSystemsIncludingChildren.Add(item);
                }
            }
            this.m_originalStartSizes = new Dictionary<UnityEngine.ParticleSystem, float>(this.m_allParticleSystemsIncludingChildren.Count);
            for (int j = 0; j < this.m_allParticleSystemsIncludingChildren.Count; j++)
            {
                this.m_originalStartSizes.Add(this.m_allParticleSystemsIncludingChildren[j], this.m_allParticleSystemsIncludingChildren[j].startSize);
            }
            this.OriginalStartRotation = this.ParticleSystem.startRotation;
            this.OriginalLoopable = this.ParticleSystem.loop;
            this.ParticleSystem.playOnAwake = false;
            this.ManualDurationTimer = -1f;
            this.ManualSimulationWithUnscaledTime = false;
            this.ParticleSystemColor = base.GetComponent<ParticleSystemColor>();
            if (this.ParticleSystemColor != null)
            {
                this.ParticleSystemColor.instantiateMaterials();
            }
        }

        public void cleanUpForReuse()
        {
            this.ParticleSystem.Simulate(0f, true, true);
            this.ParticleSystem.Stop();
            this.ParticleSystem.Clear();
            this.ParticleSystem.startRotation = this.OriginalStartRotation;
            this.ParticleSystem.loop = this.OriginalLoopable;
            this.ParticleSystem.enableEmission = true;
            this.FollowTm = null;
            this.ManualDurationTimer = -1f;
            this.ManualSimulationWithUnscaledTime = false;
            GameObjectExtensions.SetLayerRecursively(base.gameObject, this.OriginalLayer);
            this.m_fadeOutParticles = null;
            this.m_fadeOutTimer.end();
            for (int i = 0; i < this.m_allParticleSystemsIncludingChildren.Count; i++)
            {
                this.m_allParticleSystemsIncludingChildren[i].startSize = this.m_originalStartSizes[this.m_allParticleSystemsIncludingChildren[i]];
            }
            if (this.ParticleSystemColor != null)
            {
                this.ParticleSystemColor.resetColor();
            }
        }

        public bool isAlive()
        {
            for (int i = 0; i < this.m_allParticleSystemsIncludingChildren.Count; i++)
            {
                if (this.m_allParticleSystemsIncludingChildren[i].IsAlive(false))
                {
                    return true;
                }
            }
            return false;
        }

        protected void LateUpdate()
        {
            if (!this.m_fadeOutTimer.Idle)
            {
                this.m_fadeOutTimer.tick(Time.deltaTime);
                for (int i = 0; i < this.m_allParticleSystemsIncludingChildren.Count; i++)
                {
                    UnityEngine.ParticleSystem system = this.m_allParticleSystemsIncludingChildren[i];
                    UnityEngine.ParticleSystem.Particle[] particles = this.m_fadeOutParticles[system];
                    int size = system.GetParticles(particles);
                    for (int j = 0; j < size; j++)
                    {
                        float num4 = 1f - this.m_fadeOutTimer.normalizedProgress();
                        Color32 color = particles[j].color;
                        color.a = (byte) (color.a * num4);
                        particles[j].color = color;
                        particles[j].size *= num4;
                    }
                    system.SetParticles(particles, size);
                }
            }
        }

        public void setColorParameters(object param)
        {
            if (this.ParticleSystemColor != null)
            {
                this.ParticleSystemColor.setColor(param);
            }
        }

        public void setMultipliedStartSize(float multiplier, bool includeChildParticleSystems)
        {
            if (includeChildParticleSystems)
            {
                for (int i = 0; i < this.m_allParticleSystemsIncludingChildren.Count; i++)
                {
                    UnityEngine.ParticleSystem system = this.m_allParticleSystemsIncludingChildren[i];
                    system.startSize = this.m_originalStartSizes[system] * multiplier;
                }
            }
            else
            {
                this.ParticleSystem.startSize = this.m_originalStartSizes[this.ParticleSystem] * multiplier;
            }
        }

        public void stopAndFadeOutParticles()
        {
            bool flag = false;
            if (this.m_fadeOutParticles == null)
            {
                this.m_fadeOutParticles = new Dictionary<UnityEngine.ParticleSystem, UnityEngine.ParticleSystem.Particle[]>();
                flag = true;
            }
            for (int i = 0; i < this.m_allParticleSystemsIncludingChildren.Count; i++)
            {
                UnityEngine.ParticleSystem key = this.m_allParticleSystemsIncludingChildren[i];
                key.Stop(true);
                if (flag)
                {
                    this.m_fadeOutParticles.Add(key, new UnityEngine.ParticleSystem.Particle[key.maxParticles]);
                }
            }
            this.m_fadeOutTimer.set(0.3f);
        }

        public PlayerView.EffectType EffectType
        {
            [CompilerGenerated]
            get
            {
                return this.<EffectType>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<EffectType>k__BackingField = value;
            }
        }

        public Transform FollowTm
        {
            [CompilerGenerated]
            get
            {
                return this.<FollowTm>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<FollowTm>k__BackingField = value;
            }
        }

        public float ManualDurationTimer
        {
            [CompilerGenerated]
            get
            {
                return this.<ManualDurationTimer>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ManualDurationTimer>k__BackingField = value;
            }
        }

        public bool ManualSimulationWithUnscaledTime
        {
            [CompilerGenerated]
            get
            {
                return this.<ManualSimulationWithUnscaledTime>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ManualSimulationWithUnscaledTime>k__BackingField = value;
            }
        }

        public int OriginalLayer
        {
            [CompilerGenerated]
            get
            {
                return this.<OriginalLayer>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<OriginalLayer>k__BackingField = value;
            }
        }

        public bool OriginalLoopable
        {
            [CompilerGenerated]
            get
            {
                return this.<OriginalLoopable>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<OriginalLoopable>k__BackingField = value;
            }
        }

        public float OriginalStartRotation
        {
            [CompilerGenerated]
            get
            {
                return this.<OriginalStartRotation>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<OriginalStartRotation>k__BackingField = value;
            }
        }

        public UnityEngine.ParticleSystem ParticleSystem
        {
            [CompilerGenerated]
            get
            {
                return this.<ParticleSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ParticleSystem>k__BackingField = value;
            }
        }

        public ParticleSystemColor ParticleSystemColor
        {
            [CompilerGenerated]
            get
            {
                return this.<ParticleSystemColor>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ParticleSystemColor>k__BackingField = value;
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

