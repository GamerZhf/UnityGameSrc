namespace AiView
{
    using GameLogic;
    using System;

    public class MonsterPufferAiBehaviour : AiBehaviour
    {
        private AiBehaviour m_attackBehaviour;
        private AiBehaviour m_targetingBehaviour;

        public MonsterPufferAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
            this.m_targetingBehaviour = new MonsterTargetingAiBehaviour(character);
            this.m_attackBehaviour = new StationarySkillUseAiBehaviour(character, SkillType.PoisonPuff);
        }

        public override void preUpdate(float dt)
        {
            this.m_targetingBehaviour.preUpdate(dt);
            this.m_attackBehaviour.preUpdate(dt);
        }

        public override void update(float dt)
        {
            this.m_targetingBehaviour.update(dt);
            this.m_attackBehaviour.update(dt);
        }
    }
}

