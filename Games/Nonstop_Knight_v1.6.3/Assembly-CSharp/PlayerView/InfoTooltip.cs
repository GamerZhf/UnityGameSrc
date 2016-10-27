namespace PlayerView
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class InfoTooltip : MenuContent
    {
        public RectTransform ArrowRectTm;
        private Vector3 m_arrowDefaultLocalPos;
        private Coroutine m_arrowRefreshRoutine;
        public UnityEngine.UI.Text Text;

        [DebuggerHidden]
        private IEnumerator arrowRefreshRoutine()
        {
            <arrowRefreshRoutine>c__Iterator125 iterator = new <arrowRefreshRoutine>c__Iterator125();
            iterator.<>f__this = this;
            return iterator;
        }

        protected override void onAwake()
        {
            this.m_arrowDefaultLocalPos = this.ArrowRectTm.localPosition;
        }

        protected override void onCleanup()
        {
            UnityUtils.StopCoroutine(this, ref this.m_arrowRefreshRoutine);
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.Text.text = (string) param;
            this.m_arrowRefreshRoutine = UnityUtils.StartCoroutine(this, this.arrowRefreshRoutine());
        }

        protected override void onRefresh()
        {
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator124 iterator = new <onShow>c__Iterator124();
            iterator.<>f__this = this;
            return iterator;
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.InfoTooltip;
            }
        }

        [CompilerGenerated]
        private sealed class <arrowRefreshRoutine>c__Iterator125 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal InfoTooltip <>f__this;
            internal TooltipMenu <tooltipMenu>__0;

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
                        this.<tooltipMenu>__0 = (TooltipMenu) this.<>f__this.m_contentMenu;
                        break;

                    case 1:
                        break;
                        this.$PC = -1;
                        goto Label_0077;

                    default:
                        goto Label_0077;
                }
                this.<tooltipMenu>__0.refreshArrowPosition(this.<>f__this.ArrowRectTm, this.<>f__this.m_arrowDefaultLocalPos);
                this.$current = null;
                this.$PC = 1;
                return true;
            Label_0077:
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
        private sealed class <onShow>c__Iterator124 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal InfoTooltip <>f__this;

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
                    UnityUtils.StopCoroutine(this.<>f__this, ref this.<>f__this.m_arrowRefreshRoutine);
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

