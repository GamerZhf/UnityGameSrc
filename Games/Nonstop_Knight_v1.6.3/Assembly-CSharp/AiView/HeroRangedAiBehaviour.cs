namespace AiView
{
    using GameLogic;
    using System;

    public class HeroRangedAiBehaviour : AiBehaviour
    {
        private PursuitAndRangedAttackAiBehaviour m_attackBehaviour;
        private HeroTargetingAiBehaviour m_targetingBehaviour;

        public HeroRangedAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
            this.m_targetingBehaviour = new HeroTargetingAiBehaviour(character);
            this.m_attackBehaviour = new PursuitAndRangedAttackAiBehaviour(character);
        }

        public override void update(float dt)
        {
            this.m_targetingBehaviour.update(dt);
            this.m_attackBehaviour.update(dt);
        }
    }
}

