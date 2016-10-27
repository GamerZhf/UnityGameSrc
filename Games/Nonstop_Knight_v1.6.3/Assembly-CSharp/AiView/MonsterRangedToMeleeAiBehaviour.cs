namespace AiView
{
    using GameLogic;
    using System;

    public class MonsterRangedToMeleeAiBehaviour : AiBehaviour
    {
        private PursuitAndRangedMeleeAttackAiBehaviour m_attackBehaviour;
        private MonsterTargetingAiBehaviour m_targetingBehaviour;

        public MonsterRangedToMeleeAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
            this.m_targetingBehaviour = new MonsterTargetingAiBehaviour(character);
            this.m_attackBehaviour = new PursuitAndRangedMeleeAttackAiBehaviour(character);
        }

        public override void update(float dt)
        {
            this.m_targetingBehaviour.update(dt);
            this.m_attackBehaviour.update(dt);
        }
    }
}

