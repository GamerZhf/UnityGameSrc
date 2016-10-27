namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdConvertReward : ICommand
    {
        private Reward m_drop;
        private Player m_player;
        private ChestType m_targetChestType;

        public CmdConvertReward(Player player, Reward drop, ChestType targetChestType)
        {
            this.m_player = player;
            this.m_drop = drop;
            this.m_targetChestType = targetChestType;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator98 iterator = new <executeRoutine>c__Iterator98();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(Player player, Reward drop, ChestType targetChestType)
        {
            if ((drop.ChestType == ChestType.MysterySpecialOffer) && (targetChestType == ChestType.MysteryStandard))
            {
                drop.clearContent();
                drop.ChestType = targetChestType;
                double item = App.Binder.ConfigMeta.CoinBundleSize(player, "CoinBundleXSmall", 0.0);
                drop.CoinDrops.Add(item);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator98 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdConvertReward <>f__this;

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
                    CmdConvertReward.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_drop, this.<>f__this.m_targetChestType);
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

