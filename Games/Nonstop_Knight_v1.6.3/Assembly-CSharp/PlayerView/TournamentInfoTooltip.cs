namespace PlayerView
{
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class TournamentInfoTooltip : MenuContent
    {
        [CompilerGenerated]
        private Service.TournamentView <TournamentView>k__BackingField;
        public RectTransform ArrowRectTm;
        private Vector3 m_arrowDefaultLocalPos;
        private Coroutine m_arrowRefreshRoutine;

        [DebuggerHidden]
        private IEnumerator arrowRefreshRoutine()
        {
            <arrowRefreshRoutine>c__Iterator1A1 iteratora = new <arrowRefreshRoutine>c__Iterator1A1();
            iteratora.<>f__this = this;
            return iteratora;
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
            this.TournamentView = (Service.TournamentView) param;
            this.m_arrowRefreshRoutine = UnityUtils.StartCoroutine(this, this.arrowRefreshRoutine());
        }

        protected override void onRefresh()
        {
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator1A0 iteratora = new <onShow>c__Iterator1A0();
            iteratora.<>f__this = this;
            return iteratora;
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.TournamentInfoTooltip;
            }
        }

        public Service.TournamentView TournamentView
        {
            [CompilerGenerated]
            get
            {
                return this.<TournamentView>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<TournamentView>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <arrowRefreshRoutine>c__Iterator1A1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TournamentInfoTooltip <>f__this;
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
        private sealed class <onShow>c__Iterator1A0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TournamentInfoTooltip <>f__this;

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

