namespace Pathfinding
{
    using System;
    using UnityEngine;

    [Serializable]
    public abstract class MonoModifier : MonoBehaviour, IPathModifier
    {
        [NonSerialized]
        public Seeker seeker;

        protected MonoModifier()
        {
        }

        public abstract void Apply(Path p);
        public void Awake()
        {
            this.seeker = base.GetComponent<Seeker>();
            if (this.seeker != null)
            {
                this.seeker.RegisterModifier(this);
            }
        }

        public void OnDestroy()
        {
            if (this.seeker != null)
            {
                this.seeker.DeregisterModifier(this);
            }
        }

        public void OnDisable()
        {
        }

        public void OnEnable()
        {
        }

        public virtual void PreProcess(Path p)
        {
        }

        public abstract int Order { get; }
    }
}

