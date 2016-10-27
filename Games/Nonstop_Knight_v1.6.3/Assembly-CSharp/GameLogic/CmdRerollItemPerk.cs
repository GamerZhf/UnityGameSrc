namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdRerollItemPerk : ICommand
    {
        private ItemInstance m_itemInstance;
        private int m_perkIndex;
        private Player m_player;

        public CmdRerollItemPerk(ItemInstance itemInstance, int perkIndex, Player player)
        {
            this.m_itemInstance = itemInstance;
            this.m_perkIndex = perkIndex;
            this.m_player = player;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator7B iteratorb = new <executeRoutine>c__Iterator7B();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        public static void ExecuteStatic(ItemInstance itemInstance, int perkIndex, Player player)
        {
            if (perkIndex >= itemInstance.Perks.PerkInstances.Count)
            {
                UnityEngine.Debug.LogError("Invalid perk index for item instance: " + itemInstance.Item.Name);
            }
            else
            {
                itemInstance.Perks.PerkInstances[perkIndex] = ConfigPerks.RollNewRandomItemPerkInstance(itemInstance.Item.Type, itemInstance.Perks, player);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator7B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdRerollItemPerk <>f__this;

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
                    CmdRerollItemPerk.ExecuteStatic(this.<>f__this.m_itemInstance, this.<>f__this.m_perkIndex, this.<>f__this.m_player);
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

