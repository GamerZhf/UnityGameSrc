namespace AiView
{
    using GameLogic;
    using System;

    public class MonsterRangedAiBehaviour : AiBehaviour
    {
        private PursuitAndRangedAttackAiBehaviour m_attackBehaviour;
        private MonsterTargetingAiBehaviour m_targetingBehaviour;

        public MonsterRangedAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
            this.m_targetingBehaviour = new MonsterTargetingAiBehaviour(character);
            this.m_attackBehaviour = new PursuitAndRangedAttackAiBehaviour(character);
        }

        public override void update(float dt)
        {
            this.m_targetingBehaviour.update(dt);
            this.m_attackBehaviour.update(dt);
        }
    }
}

