namespace AiView
{
    using GameLogic;
    using System;

    public class PursuitAndRangedMeleeAttackAiBehaviour : AiBehaviour
    {
        private PursuitAndMeleeAttackAiBehaviour m_meleeAttackBehaviour;
        private PursuitAndRangedAttackAiBehaviour m_rangedAttackBehaviour;

        public PursuitAndRangedMeleeAttackAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
            this.m_meleeAttackBehaviour = new PursuitAndMeleeAttackAiBehaviour(character);
            this.m_rangedAttackBehaviour = new PursuitAndRangedAttackAiBehaviour(character);
        }

        public override void update(float dt)
        {
            if ((base.Character.TargetCharacter == null) || (PhysicsUtil.DistBetween(base.Character, base.Character.TargetCharacter) < 3f))
            {
                this.m_meleeAttackBehaviour.update(dt);
            }
            else
            {
                this.m_rangedAttackBehaviour.update(dt);
            }
        }
    }
}

