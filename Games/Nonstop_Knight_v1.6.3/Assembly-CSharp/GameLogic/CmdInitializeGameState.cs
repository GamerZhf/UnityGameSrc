namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdInitializeGameState : ICommand
    {
        private Player m_player;

        public CmdInitializeGameState(Player player)
        {
            this.m_player = player;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator41 iterator = new <executeRoutine>c__Iterator41();
            iterator.<>f__this = this;
            return iterator;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator41 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdInitializeGameState <>f__this;
            internal CharacterInstance <c>__1;
            internal int <i>__0;
            internal CmdSpawnCharacter.SpawningData <sd>__2;
            internal CharacterInstance <spawnedCharacter>__3;

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
                    Binder.GameState.Player = this.<>f__this.m_player;
                    this.<>f__this.m_player.postDeserializeInitialization();
                    this.<i>__0 = 0;
                    while (this.<i>__0 < this.<>f__this.m_player.CharacterInstances.Count)
                    {
                        this.<c>__1 = this.<>f__this.m_player.CharacterInstances[this.<i>__0];
                        CmdSpawnCharacter.SpawningData data = new CmdSpawnCharacter.SpawningData();
                        data.CharacterInstancePrototype = this.<c>__1;
                        data.Rank = this.<c>__1.Rank;
                        data.SpawnWorldPos = Vector3.zero;
                        data.IsPlayerCharacter = true;
                        data.IsPersistent = true;
                        this.<sd>__2 = data;
                        this.<spawnedCharacter>__3 = CmdSpawnCharacter.ExecuteStatic(this.<sd>__2);
                        this.<>f__this.m_player.CharacterInstances[this.<i>__0] = this.<spawnedCharacter>__3;
                        this.<i>__0++;
                    }
                    Binder.EventBus.GameStateInitialized();
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

