namespace Pathfinding
{
    using System;

    [Serializable]
    public abstract class PathModifier : IPathModifier
    {
        [NonSerialized]
        public Seeker seeker;

        protected PathModifier()
        {
        }

        public abstract void Apply(Path p);
        public void Awake(Seeker s)
        {
            this.seeker = s;
            if (s != null)
            {
                s.RegisterModifier(this);
            }
        }

        public void OnDestroy(Seeker s)
        {
            if (s != null)
            {
                s.DeregisterModifier(this);
            }
        }

        public virtual void PreProcess(Path p)
        {
        }

        public abstract int Order { get; }
    }
}

