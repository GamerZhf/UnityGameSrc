namespace GameLogic
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public abstract class AreaEffect : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private CharacterInstance <OwningCharacter>k__BackingField;
        [CompilerGenerated]
        private AreaEffectProperties <Properties>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.Transform <Transform>k__BackingField;
        private float m_lifetimeTimer;

        protected AreaEffect()
        {
        }

        protected void Awake()
        {
            this.Transform = base.transform;
        }

        public void cleanUpForReuse()
        {
            this.OwningCharacter = null;
        }

        public void destroy()
        {
            this.onPreDestroy();
            Binder.EventBus.AreaEffectPreDestroy(this);
            Binder.GameState.ActiveDungeon.ActiveRoom.ActiveAreaEffects.Remove(this);
            Binder.AreaEffectPool.returnObject(this, this.Type);
        }

        protected void FixedUpdate()
        {
            this.onFixedUpdate(Time.deltaTime * Time.timeScale);
            this.m_lifetimeTimer -= Time.deltaTime * Time.timeScale;
            if (this.m_lifetimeTimer <= 0f)
            {
                this.destroy();
            }
        }

        protected virtual void onFixedUpdate(float dt)
        {
        }

        protected virtual void onPreDestroy()
        {
        }

        protected virtual void onSpawn()
        {
        }

        public void spawn(CharacterInstance owner, Vector3 spawnWorldPt, AreaEffectProperties props)
        {
            this.OwningCharacter = owner;
            this.Transform.position = spawnWorldPt;
            this.Properties = props;
            this.m_lifetimeTimer = props.MaxLifetime;
            Binder.GameState.ActiveDungeon.ActiveRoom.ActiveAreaEffects.Add(this);
            this.onSpawn();
            Binder.EventBus.AreaEffectSpawned(this);
        }

        public CharacterInstance OwningCharacter
        {
            [CompilerGenerated]
            get
            {
                return this.<OwningCharacter>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<OwningCharacter>k__BackingField = value;
            }
        }

        public AreaEffectProperties Properties
        {
            [CompilerGenerated]
            get
            {
                return this.<Properties>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Properties>k__BackingField = value;
            }
        }

        public UnityEngine.Transform Transform
        {
            [CompilerGenerated]
            get
            {
                return this.<Transform>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Transform>k__BackingField = value;
            }
        }

        public abstract AreaEffectType Type { get; }

        [StructLayout(LayoutKind.Sequential)]
        public struct AreaEffectProperties
        {
            public float MaxLifetime;
            public object CustomProperties;
            public AreaEffectProperties(AreaEffect.AreaEffectProperties another)
            {
                this.MaxLifetime = another.MaxLifetime;
                this.CustomProperties = another.CustomProperties;
            }
        }
    }
}

