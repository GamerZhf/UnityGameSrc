namespace AiView
{
    using GameLogic;
    using System;

    public class SupportMeleeAttackAiBehaviour : SupportAiBehaviour
    {
        public SupportMeleeAttackAiBehaviour(CharacterInstance character) : base(character, null)
        {
        }

        public override void update(float dt)
        {
            base.update(dt);
            if (((base.Character.TargetCharacter != null) && base.m_movementBehaviour.WithinAttackDistance) && base.Character.PhysicsBody.rotatedEnoughForMovement())
            {
                base.Character.AttackRoutine = GameLogic.Binder.CommandProcessor.executeCharacterSpecific(base.Character, new CmdAttackMelee(base.Character, base.Character.TargetCharacter), 0f);
                base.seekTacticalPosition();
            }
        }
    }
}

