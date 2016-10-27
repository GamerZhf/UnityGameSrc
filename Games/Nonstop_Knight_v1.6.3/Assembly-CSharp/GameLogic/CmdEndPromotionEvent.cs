namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdEndPromotionEvent : ICommand
    {
        private Player m_player;
        private string m_promotionId;

        public CmdEndPromotionEvent(Player player, string promotionId)
        {
            this.m_player = player;
            this.m_promotionId = promotionId;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorBB rbb = new <executeRoutine>c__IteratorBB();
            rbb.<>f__this = this;
            return rbb;
        }

        public static void ExecuteStatic(Player player, string promotionId)
        {
            if (!player.PromotionEvents.Instances.ContainsKey(promotionId))
            {
                UnityEngine.Debug.Log("Trying to end a promotion event that doesn't exist (id: " + promotionId + ")");
            }
            else
            {
                player.PromotionEvents.Instances.Remove(promotionId);
                Binder.EventBus.PromotionEventEnded(player, promotionId);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorBB : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdEndPromotionEvent <>f__this;

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
                    CmdEndPromotionEvent.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_promotionId);
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

