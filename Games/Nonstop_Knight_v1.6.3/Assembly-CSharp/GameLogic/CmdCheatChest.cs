namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ConsoleCommand("chest")]
    public class CmdCheatChest : ICommand
    {
        private string m_predefinedDropRollId;

        public CmdCheatChest([Optional, DefaultParameterValue(null)] string predefinedDropRollId)
        {
            this.m_predefinedDropRollId = predefinedDropRollId;
        }

        public CmdCheatChest(string[] serialized)
        {
            if (serialized.Length > 0)
            {
                this.m_predefinedDropRollId = LangUtil.FirstLetterToUpper(serialized[0]);
            }
            else
            {
                this.m_predefinedDropRollId = null;
            }
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator81 iterator = new <executeRoutine>c__Iterator81();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(string predefinedDropRollId)
        {
            Player player = GameLogic.Binder.GameState.Player;
            CmdRollDropLootTable.ExecuteStatic(App.Binder.ConfigLootTables.BossDropLootTable, player, player.ActiveCharacter.PhysicsBody.Transform.position, CharacterType.Skeleton, predefinedDropRollId, ChestType.NONE);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator81 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdCheatChest <>f__this;

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
                    CmdCheatChest.ExecuteStatic(this.<>f__this.m_predefinedDropRollId);
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

