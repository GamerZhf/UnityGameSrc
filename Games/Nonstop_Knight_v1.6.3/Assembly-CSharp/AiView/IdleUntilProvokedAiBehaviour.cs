namespace AiView
{
    using GameLogic;
    using System;

    public class IdleUntilProvokedAiBehaviour : AiBehaviour
    {
        private AiBehaviour m_attackBehaviour;

        public IdleUntilProvokedAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
            if (base.Character.UsesRangedAttack)
            {
                this.m_attackBehaviour = new PursuitAndRangedAttackAiBehaviour(character);
            }
            else
            {
                this.m_attackBehaviour = new PursuitAndMeleeAttackAiBehaviour(character);
            }
        }

        public override void update(float dt)
        {
            if (base.Character.CurrentHp < base.Character.MaxLife(true))
            {
                this.m_attackBehaviour.update(dt);
            }
        }
    }
}

