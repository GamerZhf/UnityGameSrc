namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdInspectPromotionEvent : ICommand
    {
        private Player m_player;
        private string m_promotionId;

        public CmdInspectPromotionEvent(Player player, string promotionId)
        {
            this.m_player = player;
            this.m_promotionId = promotionId;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorBC rbc = new <executeRoutine>c__IteratorBC();
            rbc.<>f__this = this;
            return rbc;
        }

        public static void ExecuteStatic(Player player, string promotionId)
        {
            PromotionEventInstance instance = player.PromotionEvents.getEventInstance(promotionId);
            if (instance == null)
            {
                UnityEngine.Debug.LogWarning("Trying to inspect a promotion event that doesn't exist (id: " + promotionId + ")");
            }
            else
            {
                instance.Inspected = true;
                Binder.EventBus.PromotionEventInspected(player, promotionId);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorBC : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdInspectPromotionEvent <>f__this;

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
                    CmdInspectPromotionEvent.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_promotionId);
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

