namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdKillCharacter : ICommand
    {
        private CharacterInstance m_character;
        private bool m_critted;
        private SkillType m_fromSkill;
        private bool m_instantDestruction;
        private CharacterInstance m_killer;

        public CmdKillCharacter(CharacterInstance character, CharacterInstance killer, bool critted, bool instantDestruction, SkillType fromSkill)
        {
            this.m_character = character;
            this.m_killer = killer;
            this.m_critted = critted;
            this.m_instantDestruction = instantDestruction;
            this.m_fromSkill = fromSkill;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator5B iteratorb = new <executeRoutine>c__Iterator5B();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        public static void ExecuteStatic(CharacterInstance target, CharacterInstance killer, bool critted, bool instantDestruction, SkillType fromSkill)
        {
            ActiveDungeon activeDungeon = Binder.GameState.ActiveDungeon;
            if (target.OwningPlayer != null)
            {
                target.OwningPlayer.onCharacterOwnedPreKill(target);
            }
            target.IsDead = true;
            target.PhysicsBody.CharacterController.enabled = false;
            CmdInterruptCharacter.ExecuteStatic(target, true);
            if (target.IsBoss)
            {
                if (target.IsWildBoss)
                {
                    if (activeDungeon.ActiveTournament != null)
                    {
                        activeDungeon.ActiveTournament.WildBossesKilledSinceLastRoomCompletion++;
                        activeDungeon.ActiveTournament.WildBossesKilledTotal++;
                    }
                }
                else
                {
                    activeDungeon.BossesKilled++;
                }
            }
            else if ((!target.IsPlayerCharacter && (killer != null)) && killer.IsPlayerCharacter)
            {
                killer.OwningPlayer.setMinionsKilledSinceLastRoomCompletion(killer.OwningPlayer.getMinionsKilledSinceLastRoomCompletion(false) + 1, false);
            }
            target.PositionAtTimeOfDeath = target.PhysicsBody.Transform.position;
            Room activeRoom = Binder.GameState.ActiveDungeon.ActiveRoom;
            List<CharacterInstance> activeCharacters = activeRoom.ActiveCharacters;
            for (int i = 0; i < activeCharacters.Count; i++)
            {
                CharacterInstance character = activeCharacters[i];
                if (target == character.TargetCharacter)
                {
                    CmdSetCharacterTarget.ExecuteStatic(character, null, Vector3.zero);
                }
            }
            for (int j = 0; j < activeRoom.ActiveProjectiles.Count; j++)
            {
                if (activeRoom.ActiveProjectiles[j].OwningCharacter == target)
                {
                    activeRoom.ActiveProjectiles[j].OwningCharacter = null;
                }
            }
            if (target.OwningPlayer != null)
            {
                for (int k = 0; k < target.OwningPlayer.Pets.Instances.Count; k++)
                {
                    if (target.OwningPlayer.Pets.Instances[k].SpawnedCharacterInstance == target)
                    {
                        target.OwningPlayer.Pets.Instances[k].SpawnedCharacterInstance = null;
                    }
                }
            }
            if (instantDestruction)
            {
                Binder.EventBus.CharacterKilled_Direct(target, killer, critted, fromSkill);
            }
            else
            {
                Binder.EventBus.CharacterKilled_Queued(target, killer, critted, fromSkill);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator5B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdKillCharacter <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    CmdKillCharacter.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_killer, this.<>f__this.m_critted, this.<>f__this.m_instantDestruction, this.<>f__this.m_fromSkill);
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

