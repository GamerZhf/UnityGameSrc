namespace GooglePlayGames.Native
{
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;

    internal static class CallbackUtils
    {
        [CompilerGenerated]
        private static void <ToOnGameThread`1>m__98<T>(T)
        {
        }

        [CompilerGenerated]
        private static void <ToOnGameThread`2>m__9A<T1, T2>(T1, T2)
        {
        }

        [CompilerGenerated]
        private static void <ToOnGameThread`3>m__9C<T1, T2, T3>(T1, T2, T3)
        {
        }

        internal static Action<T> ToOnGameThread<T>(Action<T> toConvert)
        {
            <ToOnGameThread>c__AnonStorey274<T> storey = new <ToOnGameThread>c__AnonStorey274<T>();
            storey.toConvert = toConvert;
            if (storey.toConvert == null)
            {
                return new Action<T>(CallbackUtils.<ToOnGameThread`1>m__98<T>);
            }
            return new Action<T>(storey.<>m__99);
        }

        internal static Action<T1, T2> ToOnGameThread<T1, T2>(Action<T1, T2> toConvert)
        {
            <ToOnGameThread>c__AnonStorey276<T1, T2> storey = new <ToOnGameThread>c__AnonStorey276<T1, T2>();
            storey.toConvert = toConvert;
            if (storey.toConvert == null)
            {
                return new Action<T1, T2>(CallbackUtils.<ToOnGameThread`2>m__9A<T1, T2>);
            }
            return new Action<T1, T2>(storey.<>m__9B);
        }

        internal static Action<T1, T2, T3> ToOnGameThread<T1, T2, T3>(Action<T1, T2, T3> toConvert)
        {
            <ToOnGameThread>c__AnonStorey278<T1, T2, T3> storey = new <ToOnGameThread>c__AnonStorey278<T1, T2, T3>();
            storey.toConvert = toConvert;
            if (storey.toConvert == null)
            {
                return new Action<T1, T2, T3>(CallbackUtils.<ToOnGameThread`3>m__9C<T1, T2, T3>);
            }
            return new Action<T1, T2, T3>(storey.<>m__9D);
        }

        [CompilerGenerated]
        private sealed class <ToOnGameThread>c__AnonStorey274<T>
        {
            internal Action<T> toConvert;

            internal void <>m__99(T val)
            {
                <ToOnGameThread>c__AnonStorey275<T> storey = new <ToOnGameThread>c__AnonStorey275<T>();
                storey.<>f__ref$628 = (CallbackUtils.<ToOnGameThread>c__AnonStorey274<T>) this;
                storey.val = val;
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__9E));
            }

            private sealed class <ToOnGameThread>c__AnonStorey275
            {
                internal CallbackUtils.<ToOnGameThread>c__AnonStorey274<T> <>f__ref$628;
                internal T val;

                internal void <>m__9E()
                {
                    this.<>f__ref$628.toConvert(this.val);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ToOnGameThread>c__AnonStorey276<T1, T2>
        {
            internal Action<T1, T2> toConvert;

            internal void <>m__9B(T1 val1, T2 val2)
            {
                <ToOnGameThread>c__AnonStorey277<T1, T2> storey = new <ToOnGameThread>c__AnonStorey277<T1, T2>();
                storey.<>f__ref$630 = (CallbackUtils.<ToOnGameThread>c__AnonStorey276<T1, T2>) this;
                storey.val1 = val1;
                storey.val2 = val2;
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__9F));
            }

            private sealed class <ToOnGameThread>c__AnonStorey277
            {
                internal CallbackUtils.<ToOnGameThread>c__AnonStorey276<T1, T2> <>f__ref$630;
                internal T1 val1;
                internal T2 val2;

                internal void <>m__9F()
                {
                    this.<>f__ref$630.toConvert(this.val1, this.val2);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ToOnGameThread>c__AnonStorey278<T1, T2, T3>
        {
            internal Action<T1, T2, T3> toConvert;

            internal void <>m__9D(T1 val1, T2 val2, T3 val3)
            {
                <ToOnGameThread>c__AnonStorey279<T1, T2, T3> storey = new <ToOnGameThread>c__AnonStorey279<T1, T2, T3>();
                storey.<>f__ref$632 = (CallbackUtils.<ToOnGameThread>c__AnonStorey278<T1, T2, T3>) this;
                storey.val1 = val1;
                storey.val2 = val2;
                storey.val3 = val3;
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__A0));
            }

            private sealed class <ToOnGameThread>c__AnonStorey279
            {
                internal CallbackUtils.<ToOnGameThread>c__AnonStorey278<T1, T2, T3> <>f__ref$632;
                internal T1 val1;
                internal T2 val2;
                internal T3 val3;

                internal void <>m__A0()
                {
                    this.<>f__ref$632.toConvert(this.val1, this.val2, this.val3);
                }
            }
        }
    }
}

