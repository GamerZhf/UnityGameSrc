namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdInstantUpgradeItem : ICommand
    {
        private ItemInstance m_itemInstance;
        private CharacterInstance m_owner;

        public CmdInstantUpgradeItem(ItemInstance itemInstance, CharacterInstance owner)
        {
            this.m_itemInstance = itemInstance;
            this.m_owner = owner;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator79 iterator = new <executeRoutine>c__Iterator79();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(ItemInstance itemInstance, CharacterInstance owner)
        {
            int num2;
            Player player = GameLogic.Binder.GameState.Player;
            double diamondCost = App.Binder.ConfigMeta.ItemInstantUpgradeCostCurve();
            int rankUpCount = owner.getItemInstantUpgradeCount(itemInstance, out num2);
            if (rankUpCount > 0)
            {
                CmdRankUpItem.ExecuteStatic(owner, itemInstance, rankUpCount, true);
                CmdGainResources.ExecuteStatic(player, ResourceType.Diamond, -diamondCost, false, string.Empty, null);
            }
            GameLogic.Binder.EventBus.ItemInstantUpgraded(itemInstance, diamondCost);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator79 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdInstantUpgradeItem <>f__this;

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
                    CmdInstantUpgradeItem.ExecuteStatic(this.<>f__this.m_itemInstance, this.<>f__this.m_owner);
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

