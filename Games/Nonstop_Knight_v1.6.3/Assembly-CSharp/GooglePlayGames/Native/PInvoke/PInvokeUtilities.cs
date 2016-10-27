namespace GooglePlayGames.Native.PInvoke
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    internal static class PInvokeUtilities
    {
        private static readonly DateTime UnixEpoch = DateTime.SpecifyKind(new DateTime(0x7b2, 1, 1), DateTimeKind.Utc);

        internal static UIntPtr ArrayToSizeT<T>(T[] array)
        {
            if (array == null)
            {
                return UIntPtr.Zero;
            }
            return new UIntPtr((ulong) array.Length);
        }

        internal static HandleRef CheckNonNull(HandleRef reference)
        {
            if (IsNull(reference))
            {
                throw new InvalidOperationException();
            }
            return reference;
        }

        internal static DateTime FromMillisSinceUnixEpoch(long millisSinceEpoch)
        {
            return UnixEpoch.Add(TimeSpan.FromMilliseconds((double) millisSinceEpoch));
        }

        internal static bool IsNull(IntPtr pointer)
        {
            return pointer.Equals(IntPtr.Zero);
        }

        internal static bool IsNull(HandleRef reference)
        {
            return IsNull(HandleRef.ToIntPtr(reference));
        }

        internal static T[] OutParamsToArray<T>(OutMethod<T> outMethod)
        {
            UIntPtr ptr = outMethod(null, UIntPtr.Zero);
            if (ptr.Equals(UIntPtr.Zero))
            {
                return new T[0];
            }
            T[] localArray = new T[ptr.ToUInt64()];
            outMethod(localArray, ptr);
            return localArray;
        }

        internal static string OutParamsToString(OutStringMethod outStringMethod)
        {
            UIntPtr ptr = outStringMethod(null, UIntPtr.Zero);
            if (ptr.Equals(UIntPtr.Zero))
            {
                return null;
            }
            StringBuilder builder = new StringBuilder((int) ptr.ToUInt32());
            outStringMethod(builder, ptr);
            return builder.ToString();
        }

        [DebuggerHidden]
        internal static IEnumerable<T> ToEnumerable<T>(UIntPtr size, Func<UIntPtr, T> getElement)
        {
            <ToEnumerable>c__Iterator27<T> iterator = new <ToEnumerable>c__Iterator27<T>();
            iterator.size = size;
            iterator.getElement = getElement;
            iterator.<$>size = size;
            iterator.<$>getElement = getElement;
            iterator.$PC = -2;
            return iterator;
        }

        internal static IEnumerator<T> ToEnumerator<T>(UIntPtr size, Func<UIntPtr, T> getElement)
        {
            return ToEnumerable<T>(size, getElement).GetEnumerator();
        }

        internal static long ToMilliseconds(TimeSpan span)
        {
            double totalMilliseconds = span.TotalMilliseconds;
            if (totalMilliseconds > 9.2233720368547758E+18)
            {
                return 0x7fffffffffffffffL;
            }
            if (totalMilliseconds < -9.2233720368547758E+18)
            {
                return -9223372036854775808L;
            }
            return Convert.ToInt64(totalMilliseconds);
        }

        [CompilerGenerated]
        private sealed class <ToEnumerable>c__Iterator27<T> : IEnumerator, IDisposable, IEnumerable, IEnumerable<T>, IEnumerator<T>
        {
            internal T $current;
            internal int $PC;
            internal Func<UIntPtr, T> <$>getElement;
            internal UIntPtr <$>size;
            internal ulong <i>__0;
            internal Func<UIntPtr, T> getElement;
            internal UIntPtr size;

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
                        this.<i>__0 = 0L;
                        break;

                    case 1:
                        this.<i>__0 += (ulong) 1L;
                        break;

                    default:
                        goto Label_0082;
                }
                if (this.<i>__0 < this.size.ToUInt64())
                {
                    this.$current = this.getElement(new UIntPtr(this.<i>__0));
                    this.$PC = 1;
                    return true;
                }
                this.$PC = -1;
            Label_0082:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                PInvokeUtilities.<ToEnumerable>c__Iterator27<T> iterator = new PInvokeUtilities.<ToEnumerable>c__Iterator27<T>();
                iterator.size = this.<$>size;
                iterator.getElement = this.<$>getElement;
                return iterator;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
            }

            T IEnumerator<T>.Current
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

        internal delegate UIntPtr OutMethod<T>([In, Out] T[] out_bytes, UIntPtr out_size);

        internal delegate UIntPtr OutStringMethod(StringBuilder out_string, UIntPtr out_size);
    }
}

