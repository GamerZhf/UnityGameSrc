namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdExploreDungeon : ICommand
    {
        private CharacterInstance m_character;
        private string m_dungeonId;

        public CmdExploreDungeon(CharacterInstance character, string dungeonId)
        {
            this.m_character = character;
            this.m_dungeonId = dungeonId;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator89 iterator = new <executeRoutine>c__Iterator89();
            iterator.<>f__this = this;
            return iterator;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator89 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdExploreDungeon <>f__this;
            internal Dungeon <dungeon>__1;
            internal Player <player>__0;

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
                    this.<player>__0 = this.<>f__this.m_character.OwningPlayer;
                    if (this.<player>__0.DungeonExplored.ContainsKey(this.<>f__this.m_dungeonId))
                    {
                        this.<player>__0.DungeonExplored[this.<>f__this.m_dungeonId] = true;
                    }
                    else
                    {
                        this.<player>__0.DungeonExplored.Add(this.<>f__this.m_dungeonId, true);
                    }
                    this.<dungeon>__1 = Binder.DungeonResources.getResource(this.<>f__this.m_dungeonId);
                    CmdGainResources.ExecuteStatic(this.<>f__this.m_character.OwningPlayer, ResourceType.Coin, (double) -this.<dungeon>__1.ExploreCost, false, string.Empty, null);
                    Binder.EventBus.DungeonExplored(this.<>f__this.m_character, this.<>f__this.m_dungeonId);
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

