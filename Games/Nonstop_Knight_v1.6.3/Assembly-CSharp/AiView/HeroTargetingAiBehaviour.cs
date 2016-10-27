namespace AiView
{
    using GameLogic;
    using System;
    using UnityEngine;

    public class HeroTargetingAiBehaviour : AiBehaviour
    {
        public HeroTargetingAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
        }

        public override void update(float dt)
        {
            Room activeRoom = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom;
            CharacterInstance targetCharacter = activeRoom.getClosestEnemyCharacter(base.Character, true);
            if (targetCharacter == null)
            {
                targetCharacter = activeRoom.getClosestEnemyCharacter(base.Character, false);
            }
            if (targetCharacter == null)
            {
                CmdSetCharacterTarget.ExecuteStatic(base.Character, null, Vector3.zero);
            }
            else if ((base.Character.TargetCharacter != null) && base.Character.TargetCharacter.IsPlayerCharacter)
            {
                CmdSetCharacterTarget.ExecuteStatic(base.Character, targetCharacter, Vector3.zero);
            }
            else if ((targetCharacter != base.Character.TargetCharacter) && (((base.Character.TargetCharacter == null) || base.Character.TargetCharacter.IsDead) || (Mathf.Abs((float) (PhysicsUtil.DistBetween(base.Character, base.Character.TargetCharacter) - PhysicsUtil.DistBetween(base.Character, targetCharacter))) > 2f)))
            {
                CmdSetCharacterTarget.ExecuteStatic(base.Character, targetCharacter, Vector3.zero);
            }
        }
    }
}

