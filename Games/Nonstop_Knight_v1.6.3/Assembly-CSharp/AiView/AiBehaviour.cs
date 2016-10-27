namespace AiView
{
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;

    public abstract class AiBehaviour
    {
        [CompilerGenerated]
        private CharacterInstance <Character>k__BackingField;
        public ManualTimer UpdateTimer = new ManualTimer(ConfigGameplay.AI_UPDATE_INTERVAL);

        protected AiBehaviour()
        {
        }

        public virtual void cleanup()
        {
        }

        public virtual void preUpdate(float dt)
        {
        }

        public abstract void update(float dt);

        public CharacterInstance Character
        {
            [CompilerGenerated]
            get
            {
                return this.<Character>k__BackingField;
            }
            [CompilerGenerated]
            protected set
            {
                this.<Character>k__BackingField = value;
            }
        }
    }
}

