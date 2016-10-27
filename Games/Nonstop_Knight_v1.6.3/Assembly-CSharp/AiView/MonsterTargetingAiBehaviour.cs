namespace AiView
{
    using GameLogic;
    using System;
    using UnityEngine;

    public class MonsterTargetingAiBehaviour : AiBehaviour
    {
        public MonsterTargetingAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
        }

        public override void update(float dt)
        {
            CharacterInstance instance;
            if (base.Character.Confused)
            {
                instance = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.getClosestEnemyCharacter(base.Character, false);
            }
            else
            {
                instance = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.getEnemyCharacterWithHighestThreat(base.Character, false);
            }
            if ((instance != null) && instance.IsDead)
            {
                CmdSetCharacterTarget.ExecuteStatic(base.Character, null, Vector3.zero);
            }
            else
            {
                CmdSetCharacterTarget.ExecuteStatic(base.Character, instance, Vector3.zero);
            }
        }
    }
}

