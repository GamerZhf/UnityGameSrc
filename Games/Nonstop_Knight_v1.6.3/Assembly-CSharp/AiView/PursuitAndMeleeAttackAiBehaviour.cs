namespace AiView
{
    using App;
    using GameLogic;
    using System;
    using UnityEngine;

    public class PursuitAndMeleeAttackAiBehaviour : AiBehaviour
    {
        private PursuitAiBehaviour m_pursuitBehaviour;

        public PursuitAndMeleeAttackAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
            this.m_pursuitBehaviour = new PursuitAiBehaviour(character);
        }

        public override void update(float dt)
        {
            if ((base.Character.TargetCharacter == null) && (base.Character.ManualTargetPos == Vector3.zero))
            {
                CmdSetCharacterVelocityAndFacing.ExecuteStatic(base.Character, Vector3.zero, base.Character.Facing);
            }
            else if (base.Character.RemainInPlaceOnGuard && ((base.Character.TargetCharacter == null) || (PhysicsUtil.DistBetween(base.Character, base.Character.TargetCharacter) > ConfigGameplay.AGGRO_RANGE_IDLE)))
            {
                CmdSetCharacterVelocityAndFacing.ExecuteStatic(base.Character, Vector3.zero, base.Character.Facing);
            }
            else
            {
                this.m_pursuitBehaviour.update(dt);
                if (this.m_pursuitBehaviour.WithinAttackDistance && base.Character.PhysicsBody.rotatedEnoughForMovement())
                {
                    base.Character.AttackRoutine = GameLogic.Binder.CommandProcessor.executeCharacterSpecific(base.Character, new CmdAttackMelee(base.Character, base.Character.TargetCharacter), 0f);
                }
            }
        }
    }
}

