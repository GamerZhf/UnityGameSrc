namespace GameLogic
{
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdCollectRenewableResources : ICommand
    {
        private Player m_player;
        private ResourceType m_resourceType;

        public CmdCollectRenewableResources(Player player, ResourceType resourceType)
        {
            this.m_player = player;
            this.m_resourceType = resourceType;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator50 iterator = new <executeRoutine>c__Iterator50();
            iterator.<>f__this = this;
            return iterator;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator50 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdCollectRenewableResources <>f__this;
            internal int <amountCollected>__3;
            internal long <lastCollectionSeconds>__1;
            internal long <secondsElapsed>__2;
            internal long <secondsNow>__0;
            internal long <secondsPerUnitGained>__4;

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
                    this.<secondsNow>__0 = Service.Binder.ServerTime.GameTime;
                    this.<lastCollectionSeconds>__1 = 0L;
                    if (!this.<>f__this.m_player.RenewableResourceTimestamps.ContainsKey(this.<>f__this.m_resourceType.ToString()))
                    {
                        this.<>f__this.m_player.RenewableResourceTimestamps.Add(this.<>f__this.m_resourceType.ToString(), 0L);
                    }
                    if (this.<>f__this.m_player.RenewableResourceTimestamps[this.<>f__this.m_resourceType.ToString()] > 0L)
                    {
                        this.<lastCollectionSeconds>__1 = this.<>f__this.m_player.RenewableResourceTimestamps[this.<>f__this.m_resourceType.ToString()];
                    }
                    else
                    {
                        this.<lastCollectionSeconds>__1 = this.<secondsNow>__0;
                    }
                    this.<secondsElapsed>__2 = MathUtil.Clamp((long) (this.<secondsNow>__0 - this.<lastCollectionSeconds>__1), (long) 0L, (long) 0x7fffffffffffffffL);
                    this.<amountCollected>__3 = (int) (this.<secondsElapsed>__2 / ((long) RenewableResourceSystem.RENEWABLE_RESOURCE_TIMERS[this.<>f__this.m_resourceType]));
                    if (this.<amountCollected>__3 > 0)
                    {
                        CmdGainResources.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_resourceType, (double) this.<amountCollected>__3, false, string.Empty, null);
                    }
                    this.<secondsPerUnitGained>__4 = (long) RenewableResourceSystem.RENEWABLE_RESOURCE_TIMERS[this.<>f__this.m_resourceType];
                    this.<>f__this.m_player.RenewableResourceTimestamps[this.<>f__this.m_resourceType.ToString()] = this.<lastCollectionSeconds>__1 + (this.<amountCollected>__3 * this.<secondsPerUnitGained>__4);
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

