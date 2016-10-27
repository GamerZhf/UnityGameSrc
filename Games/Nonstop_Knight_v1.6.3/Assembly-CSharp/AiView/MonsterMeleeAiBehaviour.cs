namespace AiView
{
    using GameLogic;
    using System;

    public class MonsterMeleeAiBehaviour : AiBehaviour
    {
        private PursuitAndMeleeAttackAiBehaviour m_attackBehaviour;
        private MonsterTargetingAiBehaviour m_targetingBehaviour;

        public MonsterMeleeAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
            this.m_targetingBehaviour = new MonsterTargetingAiBehaviour(character);
            this.m_attackBehaviour = new PursuitAndMeleeAttackAiBehaviour(character);
        }

        public override void update(float dt)
        {
            this.m_targetingBehaviour.update(dt);
            this.m_attackBehaviour.update(dt);
        }
    }
}

