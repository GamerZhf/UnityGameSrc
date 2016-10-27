namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdRollPetBundleLootTable : ICommand
    {
        private LootTable m_lootTable;
        private Player m_player;

        public CmdRollPetBundleLootTable(LootTable lootTable, Player player)
        {
            this.m_lootTable = lootTable;
            this.m_player = player;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator64 iterator = new <executeRoutine>c__Iterator64();
            iterator.<>f__this = this;
            return iterator;
        }

        public static string ExecuteStatic(LootTable lootTable, Player player)
        {
            ModifyChestTableWeights(lootTable, player);
            string str = Roll(lootTable, player);
            lootTable.resetWeights(true);
            return str;
        }

        private static void ModifyChestTableWeights(LootTable lootTable, Player player)
        {
            for (int i = 0; i < lootTable.Items.Count; i++)
            {
                LootTableItem lti = lootTable.Items[i];
                if (!App.Binder.ConfigMeta.IsActivePetId(lti.Id))
                {
                    lootTable.setWeightForItem(lti, 0);
                }
            }
        }

        private static string Roll(LootTable lootTable, Player player)
        {
            LootTableItem item = lootTable.roll();
            if (item != null)
            {
                string id = item.Id;
                if (GameLogic.Binder.CharacterResources.hasCharacter(id))
                {
                    return id;
                }
                UnityEngine.Debug.LogError("No pet found for pet bundle loot table roll: " + id);
            }
            return App.Binder.ConfigMeta.GetRandomActivePetId();
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator64 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdRollPetBundleLootTable <>f__this;

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
                    CmdRollPetBundleLootTable.ExecuteStatic(this.<>f__this.m_lootTable, this.<>f__this.m_player);
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

