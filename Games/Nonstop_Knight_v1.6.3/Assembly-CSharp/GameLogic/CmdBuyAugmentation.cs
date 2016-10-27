namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdBuyAugmentation : ICommand
    {
        private string m_id;
        private Player m_player;

        public CmdBuyAugmentation(Player player, string id)
        {
            this.m_player = player;
            this.m_id = id;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator92 iterator = new <executeRoutine>c__Iterator92();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(Player player, string id)
        {
            if (player.Augmentations.canBuy(id))
            {
                double augmentationPrice = App.Binder.ConfigMeta.GetAugmentationPrice(id);
                CmdGainResources.ExecuteStatic(player, ResourceType.Token, -augmentationPrice, false, string.Empty, null);
                CmdGainAugmentation.ExecuteStatic(player, id);
                GameLogic.Binder.EventBus.PlayerAugmentationPurchased(player, id, ResourceType.Token, augmentationPrice);
            }
            else
            {
                UnityEngine.Debug.LogError("Cannot buy augmentation: " + id);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator92 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdBuyAugmentation <>f__this;

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
                    CmdBuyAugmentation.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_id);
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

