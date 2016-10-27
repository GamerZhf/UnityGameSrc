namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdUnlockCharacter : ICommand
    {
        private Character m_character;
        private Player m_player;

        public CmdUnlockCharacter(Player player, Character character)
        {
            this.m_player = player;
            this.m_character = character;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorB9 rb = new <executeRoutine>c__IteratorB9();
            rb.<>f__this = this;
            return rb;
        }

        public static CharacterInstance ExecuteStatic(Player player, Character character)
        {
            if (player.hasUnlockedCharacter(character.Id))
            {
                UnityEngine.Debug.LogWarning("Player has already unlocked character: " + character.Id);
                return null;
            }
            CmdSpawnCharacter.SpawningData data2 = new CmdSpawnCharacter.SpawningData();
            data2.CharacterPrototype = character;
            data2.Rank = 1;
            data2.SpawnWorldPos = player.ActiveCharacter.PhysicsBody.Transform.position + new Vector3(UnityEngine.Random.insideUnitCircle.x, 0f, UnityEngine.Random.insideUnitCircle.y);
            data2.IsPlayerCharacter = true;
            data2.IsPersistent = true;
            CmdSpawnCharacter.SpawningData data = data2;
            CharacterInstance item = CmdSpawnCharacter.ExecuteStatic(data);
            player.CharacterInstances.Add(item);
            ActiveDungeon activeDungeon = Binder.GameState.ActiveDungeon;
            if (activeDungeon.ActiveRoom != null)
            {
                activeDungeon.ActiveRoom.ActiveCharacters.Add(item);
                item.PhysicsBody.gameObject.SetActive(true);
            }
            Binder.EventBus.CharacterUnlocked(item);
            return item;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorB9 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdUnlockCharacter <>f__this;

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
                    CmdUnlockCharacter.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_character);
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

