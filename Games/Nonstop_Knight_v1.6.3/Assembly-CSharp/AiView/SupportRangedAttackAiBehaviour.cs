namespace AiView
{
    using GameLogic;
    using System;

    public class SupportRangedAttackAiBehaviour : SupportAiBehaviour
    {
        public SupportRangedAttackAiBehaviour(CharacterInstance character) : base(character, null)
        {
        }

        public override void update(float dt)
        {
            base.update(dt);
            if (((base.Character.TargetCharacter != null) && base.m_movementBehaviour.WithinAttackDistance) && (base.m_movementBehaviour.LineOfSightToTargetCharacter && base.Character.PhysicsBody.rotatedEnoughForMovement()))
            {
                base.Character.AttackRoutine = GameLogic.Binder.CommandProcessor.executeCharacterSpecific(base.Character, new CmdAttackRanged(base.Character, base.Character.TargetCharacter.PhysicsBody.Transform.position), 0f);
            }
        }
    }
}

