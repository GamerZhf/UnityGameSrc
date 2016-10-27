namespace AiView
{
    using GameLogic;
    using System;

    public class HeroMeleeAiBehaviour : AiBehaviour
    {
        private PursuitAndMeleeAttackAiBehaviour m_attackBehaviour;
        private HeroTargetingAiBehaviour m_targetingBehaviour;

        public HeroMeleeAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
            this.m_targetingBehaviour = new HeroTargetingAiBehaviour(character);
            this.m_attackBehaviour = new PursuitAndMeleeAttackAiBehaviour(character);
        }

        public override void update(float dt)
        {
            this.m_targetingBehaviour.update(dt);
            this.m_attackBehaviour.update(dt);
        }
    }
}

