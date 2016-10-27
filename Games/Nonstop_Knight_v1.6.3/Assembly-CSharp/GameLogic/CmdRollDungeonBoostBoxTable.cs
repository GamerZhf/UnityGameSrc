namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdRollDungeonBoostBoxTable : ICommand
    {
        private LootTable m_lootTable;
        private Player m_player;

        public CmdRollDungeonBoostBoxTable(LootTable lootTable, Player player)
        {
            this.m_lootTable = lootTable;
            this.m_player = player;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator62 iterator = new <executeRoutine>c__Iterator62();
            iterator.<>f__this = this;
            return iterator;
        }

        public static DungeonBoostType ExecuteStatic(LootTable lootTable, Player player)
        {
            ModifyChestTableWeights(lootTable, player);
            DungeonBoostType type = Roll(lootTable, player);
            lootTable.resetWeights(true);
            return type;
        }

        private static void ModifyChestTableWeights(LootTable lootTable, Player player)
        {
        }

        private static DungeonBoostType Roll(LootTable lootTable, Player player)
        {
            LootTableItem item = lootTable.roll();
            DungeonBoostType none = DungeonBoostType.None;
            if (item != null)
            {
                try
                {
                    none = (DungeonBoostType) ((int) Enum.Parse(typeof(DungeonBoostType), item.Id));
                }
                catch (Exception)
                {
                    UnityEngine.Debug.LogError("Cannot parse DungeonBoostType enum from dungeon boost box table roll: " + item.Id);
                }
            }
            return none;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator62 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdRollDungeonBoostBoxTable <>f__this;

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
                    CmdRollDungeonBoostBoxTable.ExecuteStatic(this.<>f__this.m_lootTable, this.<>f__this.m_player);
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

