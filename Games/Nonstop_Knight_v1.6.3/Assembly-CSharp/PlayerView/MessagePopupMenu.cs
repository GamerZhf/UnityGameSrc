namespace PlayerView
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class MessagePopupMenu : TechPopupMenu
    {
        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator14E iteratore = new <hideRoutine>c__Iterator14E();
            iteratore.<>f__this = this;
            return iteratore;
        }

        protected override void onAwake()
        {
            base.TitleText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_BUTTON_INFO, null, false));
            base.BackgroundOverlay.fadeToTransparent(0f, Easing.Function.LINEAR);
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator14C iteratorc = new <preShowRoutine>c__Iterator14C();
            iteratorc.targetMenuContentType = targetMenuContentType;
            iteratorc.parameter = parameter;
            iteratorc.<$>targetMenuContentType = targetMenuContentType;
            iteratorc.<$>parameter = parameter;
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator14D iteratord = new <showRoutine>c__Iterator14D();
            iteratord.parameter = parameter;
            iteratord.<$>parameter = parameter;
            iteratord.<>f__this = this;
            return iteratord;
        }

        public override PlayerView.MenuType MenuType
        {
            get
            {
                return PlayerView.MenuType.MessagePopupMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator14E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MessagePopupMenu <>f__this;

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
                    this.<>f__this.BackgroundOverlay.setTransparent(true);
                    this.<>f__this.PanelRoot.transform.localScale = Vector3.zero;
                    this.<>f__this.m_content.cleanup();
                    this.<>f__this.m_content.transform.SetParent(this.<>f__this.DisabledContentTm, false);
                    this.<>f__this.m_content.gameObject.SetActive(false);
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

        [CompilerGenerated]
        private sealed class <preShowRoutine>c__Iterator14C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal MenuContentType <$>targetMenuContentType;
            internal MessagePopupMenu <>f__this;
            internal GameObject <contentObj>__0;
            internal object parameter;
            internal MenuContentType targetMenuContentType;

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
                    this.<>f__this.setCloseButtonVisibility(true);
                    this.<contentObj>__0 = PlayerView.Binder.MenuContentResources.getSharedResource(this.targetMenuContentType);
                    this.<contentObj>__0.transform.SetParent(this.<>f__this.ContentAreaTm, false);
                    this.<>f__this.m_content = this.<contentObj>__0.GetComponent<MenuContent>();
                    this.<>f__this.m_content.preShow(this.<>f__this, this.parameter);
                    this.<contentObj>__0.SetActive(true);
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

        [CompilerGenerated]
        private sealed class <showRoutine>c__Iterator14D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal MessagePopupMenu <>f__this;
            internal IEnumerator <ie>__0;
            internal object parameter;

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
                        this.<>f__this.PanelRoot.transform.localScale = Vector3.one;
                        this.<ie>__0 = this.<>f__this.m_content.show(this.parameter);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00E0;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    return true;
                }
                this.<>f__this.PanelRoot.transform.localScale = Vector3.one;
                this.<>f__this.BackgroundOverlay.fadeToBlack(0f, ConfigUi.MENU_BACKGROUND_ALPHA, Easing.Function.LINEAR);
                this.<>f__this.PanelRoot.alpha = 1f;
                goto Label_00E0;
                this.$PC = -1;
            Label_00E0:
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

