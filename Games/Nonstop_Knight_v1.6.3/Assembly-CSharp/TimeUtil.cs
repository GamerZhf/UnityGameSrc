using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class TimeUtil
{
    public static long DateTimeToUnixTimestamp(DateTime dateTime)
    {
        return (long) dateTime.Subtract(new DateTime(0x7b2, 1, 1)).TotalSeconds;
    }

    public static long DaysToTicks(int days)
    {
        return (days * 0xc92a69c000L);
    }

    public static float GetDeltaTime(DeltaTimeType dtType)
    {
        switch (dtType)
        {
            case DeltaTimeType.DELTA_TIME:
                return Time.deltaTime;

            case DeltaTimeType.FIXED_DELTA_TIME:
                return Time.fixedDeltaTime;

            case DeltaTimeType.UNSCALED_DELTA_TIME:
                return Time.unscaledDeltaTime;
        }
        return Time.maximumDeltaTime;
    }

    public static long HoursToTicks(int hours)
    {
        return (hours * 0x861c46800L);
    }

    public static long JavaSystemTime(DateTime date)
    {
        DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan span = (TimeSpan) (date - time);
        return Convert.ToInt64(span.TotalSeconds);
    }

    public static DateTime JavaSystemTime(long time)
    {
        DateTime time2 = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return time2.Add(new TimeSpan(time * 0x2710L));
    }

    public static long MillisToTicks(long millis)
    {
        return (millis * 0x2710L);
    }

    public static long MinutesToTicks(int minutes)
    {
        return (minutes * 0x23c34600L);
    }

    public static float SecondsElapsed(float tFrom, float tTo)
    {
        return Mathf.Clamp(tTo - tFrom, 0f, float.MaxValue);
    }

    public static long SecondsToTicks(int seconds)
    {
        return (seconds * 0x989680L);
    }

    public static long TicksToMillis(long ticks)
    {
        return (ticks / 0x2710L);
    }

    public static long TicksToSeconds(long ticks)
    {
        return (ticks / 0x989680L);
    }

    public static DateTime UnixTimestampToDateTime(long unixTimestamp)
    {
        DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, 0);
        return time.AddSeconds((double) unixTimestamp);
    }

    [DebuggerHidden]
    public static IEnumerator WaitForFixedSeconds(float seconds)
    {
        <WaitForFixedSeconds>c__Iterator39 iterator = new <WaitForFixedSeconds>c__Iterator39();
        iterator.seconds = seconds;
        iterator.<$>seconds = seconds;
        return iterator;
    }

    [DebuggerHidden]
    public static IEnumerator WaitForUnscaledSeconds(float seconds)
    {
        <WaitForUnscaledSeconds>c__Iterator38 iterator = new <WaitForUnscaledSeconds>c__Iterator38();
        iterator.seconds = seconds;
        iterator.<$>seconds = seconds;
        return iterator;
    }

    public static long CurrentTimeInMilliseconds
    {
        get
        {
            return (DateTime.Now.Ticks / 0x2710L);
        }
    }

    public static long CurrentTimeInTicks
    {
        get
        {
            return DateTime.Now.Ticks;
        }
    }

    [CompilerGenerated]
    private sealed class <WaitForFixedSeconds>c__Iterator39 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal float <$>seconds;
        internal float <timer>__0;
        internal float seconds;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<timer>__0 = this.seconds;
                    break;

                case 1:
                    break;

                default:
                    goto Label_007D;
            }
            if (this.<timer>__0 > 0f)
            {
                this.<timer>__0 -= Time.fixedDeltaTime * Time.timeScale;
                this.$current = new WaitForFixedUpdate();
                this.$PC = 1;
                return true;
            }
            goto Label_007D;
            this.$PC = -1;
        Label_007D:
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

    [CompilerGenerated]
    private sealed class <WaitForUnscaledSeconds>c__Iterator38 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal float <$>seconds;
        internal float <timer>__0;
        internal float seconds;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<timer>__0 = this.seconds;
                    break;

                case 1:
                    break;

                default:
                    goto Label_0079;
            }
            if (this.<timer>__0 > 0f)
            {
                this.<timer>__0 -= Time.deltaTime / Time.timeScale;
                this.$current = null;
                this.$PC = 1;
                return true;
            }
            goto Label_0079;
            this.$PC = -1;
        Label_0079:
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

    public enum DeltaTimeType
    {
        DELTA_TIME,
        FIXED_DELTA_TIME,
        UNSCALED_DELTA_TIME
    }
}

