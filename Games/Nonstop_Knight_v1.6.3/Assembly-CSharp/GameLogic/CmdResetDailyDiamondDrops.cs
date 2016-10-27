namespace GameLogic
{
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdResetDailyDiamondDrops : ICommand
    {
        private Player m_player;

        public CmdResetDailyDiamondDrops(Player player)
        {
            this.m_player = player;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorB0 rb = new <executeRoutine>c__IteratorB0();
            rb.<>f__this = this;
            return rb;
        }

        public static void ExecuteStatic(Player player)
        {
            UnityEngine.Debug.Log("Resetting daily player diamond drop count..");
            player.DailyDiamondDropCount = 0.0;
            DateTime time = TimeUtil.UnixTimestampToDateTime(Service.Binder.ServerTime.GameTime);
            DateTime dateTime = new DateTime(time.Year, time.Month, time.Day);
            player.LastDailyDiamondDropCountResetTimestamp = TimeUtil.DateTimeToUnixTimestamp(dateTime);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorB0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdResetDailyDiamondDrops <>f__this;

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
                    CmdResetDailyDiamondDrops.ExecuteStatic(this.<>f__this.m_player);
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

