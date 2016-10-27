namespace AiView
{
    using GameLogic;
    using System;
    using UnityEngine;

    public class PursuitAndRangedAttackAiBehaviour : AiBehaviour
    {
        private PursuitAiBehaviour m_pursuitBehaviour;

        public PursuitAndRangedAttackAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
            this.m_pursuitBehaviour = new PursuitAiBehaviour(character);
        }

        public override void update(float dt)
        {
            if (base.Character.TargetCharacter == null)
            {
                CmdSetCharacterVelocityAndFacing.ExecuteStatic(base.Character, Vector3.zero, base.Character.Facing);
            }
            else
            {
                this.m_pursuitBehaviour.update(dt);
                if ((this.m_pursuitBehaviour.WithinAttackDistance && this.m_pursuitBehaviour.LineOfSightToTargetCharacter) && base.Character.PhysicsBody.rotatedEnoughForMovement())
                {
                    base.Character.AttackRoutine = GameLogic.Binder.CommandProcessor.executeCharacterSpecific(base.Character, new CmdAttackRanged(base.Character, base.Character.TargetCharacter.PhysicsBody.Transform.position), 0f);
                }
            }
        }
    }
}

