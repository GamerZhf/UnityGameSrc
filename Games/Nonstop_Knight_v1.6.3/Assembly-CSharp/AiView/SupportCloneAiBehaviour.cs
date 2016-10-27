namespace AiView
{
    using App;
    using GameLogic;
    using System;
    using UnityEngine;

    public class SupportCloneAiBehaviour : AiBehaviour
    {
        private PursuitAiBehaviour m_movementBehaviour;
        private bool m_seekTacticalPosition;
        private HeroTargetingAiBehaviour m_targetingAiBehaviour;

        public SupportCloneAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
            this.m_targetingAiBehaviour = new HeroTargetingAiBehaviour(character);
            this.m_movementBehaviour = new PursuitAiBehaviour(character);
            this.m_seekTacticalPosition = true;
        }

        public override void cleanup()
        {
        }

        public override void update(float dt)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            CharacterInstance primaryPlayerCharacter = activeDungeon.PrimaryPlayerCharacter;
            if ((base.Character.ManualTargetPos != Vector3.zero) && (PhysicsUtil.DistBetween(base.Character, base.Character.ManualTargetPos) > 1f))
            {
                this.m_movementBehaviour.update(dt);
            }
            else if (((primaryPlayerCharacter.TargetCharacter != null) && (PhysicsUtil.DistBetween(primaryPlayerCharacter, primaryPlayerCharacter.TargetCharacter) > ConfigGameplay.PASSIVE_HP_REGEN_PROXIMITY_THRESHOLD)) || (primaryPlayerCharacter.ManualTargetPos != Vector3.zero))
            {
                if ((base.Character.TargetCharacter != primaryPlayerCharacter.TargetCharacter) || (base.Character.ManualTargetPos != primaryPlayerCharacter.ManualTargetPos))
                {
                    CmdSetCharacterTarget.ExecuteStatic(base.Character, primaryPlayerCharacter.TargetCharacter, primaryPlayerCharacter.ManualTargetPos);
                }
                this.m_movementBehaviour.update(dt);
                this.m_seekTacticalPosition = true;
            }
            else if (this.m_seekTacticalPosition)
            {
                this.m_targetingAiBehaviour.update(dt);
                if (base.Character.TargetCharacter != null)
                {
                    Vector3 worldPt = base.Character.TargetCharacter.PhysicsBody.Transform.position - ((Vector3) (base.Character.TargetCharacter.PhysicsBody.Transform.forward * 1.5f));
                    worldPt = activeDungeon.ActiveRoom.calculateNearestEmptySpot(worldPt, worldPt - base.Character.TargetCharacter.PhysicsBody.Transform.position, 1f, 1f, 6f, null);
                    CmdSetCharacterTarget.ExecuteStatic(base.Character, null, worldPt);
                    this.m_seekTacticalPosition = false;
                }
            }
            else
            {
                this.m_targetingAiBehaviour.update(dt);
                this.m_movementBehaviour.update(dt);
                if (((base.Character.TargetCharacter != null) && this.m_movementBehaviour.WithinAttackDistance) && base.Character.PhysicsBody.rotatedEnoughForMovement())
                {
                    base.Character.AttackRoutine = GameLogic.Binder.CommandProcessor.executeCharacterSpecific(base.Character, new CmdAttackMelee(base.Character, base.Character.TargetCharacter), 0f);
                }
            }
        }
    }
}

