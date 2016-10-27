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

    public class ThinPopupMenu : Menu
    {
        public MenuOverlay BackgroundOverlay;
        public RectTransform ContentAreaTm;
        private MenuContent m_content;
        private TransformAnimation m_panelTransformAnimation;
        public CanvasGroupAlphaFading PanelRoot;

        public override MenuContent activeContentObject()
        {
            return this.m_content;
        }

        public override MenuContentType activeContentType()
        {
            if (this.m_content != null)
            {
                return this.m_content.ContentType;
            }
            return MenuContentType.NONE;
        }

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator19C iteratorc = new <hideRoutine>c__Iterator19C();
            iteratorc.instant = instant;
            iteratorc.<$>instant = instant;
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        protected override void onAwake()
        {
            this.m_panelTransformAnimation = this.PanelRoot.gameObject.AddComponent<TransformAnimation>();
            this.BackgroundOverlay.fadeToTransparent(0f, Easing.Function.LINEAR);
        }

        public void onBackButtonClicked()
        {
        }

        public void onBackgroundOverlayClicked()
        {
            if (!this.m_content.onBackgroundOverlayClicked() && !PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        public void onCloseButtonClicked()
        {
            if (!this.m_content.onCloseButtonClicked() && !PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator19A iteratora = new <preShowRoutine>c__Iterator19A();
            iteratora.targetMenuContentType = targetMenuContentType;
            iteratora.parameter = parameter;
            iteratora.<$>targetMenuContentType = targetMenuContentType;
            iteratora.<$>parameter = parameter;
            iteratora.<>f__this = this;
            return iteratora;
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator19B iteratorb = new <showRoutine>c__Iterator19B();
            iteratorb.parameter = parameter;
            iteratorb.<$>parameter = parameter;
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        public override bool IsOverlayMenu
        {
            get
            {
                return true;
            }
        }

        public override PlayerView.MenuType MenuType
        {
            get
            {
                return PlayerView.MenuType.ThinPopupMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator19C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal ThinPopupMenu <>f__this;
            internal IEnumerator <ie>__0;
            internal IEnumerator <ie>__1;
            internal TransformAnimationTask <tt>__2;
            internal bool instant;

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
                        if (!this.instant)
                        {
                            this.<>f__this.BackgroundOverlay.fadeToTransparent(ConfigUi.POPUP_TRANSITION_DURATION_OUT, Easing.Function.LINEAR);
                            this.<ie>__1 = this.<>f__this.m_content.hide();
                            break;
                        }
                        this.<>f__this.BackgroundOverlay.setTransparent(true);
                        this.<ie>__0 = this.<>f__this.m_content.hide();
                        while (this.<ie>__0.MoveNext())
                        {
                        }
                        goto Label_019E;

                    case 1:
                        break;

                    case 2:
                        goto Label_015F;

                    default:
                        goto Label_01EA;
                }
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    goto Label_01EC;
                }
                this.<>f__this.PanelRoot.animateToTransparent(ConfigUi.POPUP_TRANSITION_DURATION_OUT, 0f);
                this.<tt>__2 = new TransformAnimationTask(this.<>f__this.m_panelTransformAnimation.transform, ConfigUi.POPUP_TRANSITION_DURATION_OUT, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__2.scale((Vector3) (Vector3.one * ConfigUi.POPUP_TRANSITION_SCALE), true, ConfigUi.POPUP_EASING_OUT);
                this.<>f__this.m_panelTransformAnimation.addTask(this.<tt>__2);
            Label_015F:
                while ((this.<>f__this.m_panelTransformAnimation.HasTasks || this.<>f__this.BackgroundOverlay.IsAnimating) || this.<>f__this.PanelRoot.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01EC;
                }
            Label_019E:
                this.<>f__this.PanelRoot.transform.localScale = Vector3.zero;
                this.<>f__this.m_content.cleanup();
                this.<>f__this.m_content.gameObject.SetActive(false);
                goto Label_01EA;
                this.$PC = -1;
            Label_01EA:
                return false;
            Label_01EC:
                return true;
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
        private sealed class <preShowRoutine>c__Iterator19A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal MenuContentType <$>targetMenuContentType;
            internal ThinPopupMenu <>f__this;
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
        private sealed class <showRoutine>c__Iterator19B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal ThinPopupMenu <>f__this;
            internal IEnumerator <ie>__2;
            internal bool <instant>__0;
            internal TransformAnimationTask <tt>__1;
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
                        this.<instant>__0 = false;
                        if (!this.<instant>__0)
                        {
                            this.<>f__this.m_panelTransformAnimation.transform.localScale = (Vector3) (Vector3.one * ConfigUi.POPUP_TRANSITION_SCALE);
                            this.<tt>__1 = new TransformAnimationTask(this.<>f__this.m_panelTransformAnimation.transform, ConfigUi.POPUP_TRANSITION_DURATION_IN, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                            this.<tt>__1.scale(Vector3.one, true, ConfigUi.MENU_EASING_OUT);
                            this.<>f__this.m_panelTransformAnimation.addTask(this.<tt>__1);
                            this.<>f__this.BackgroundOverlay.fadeToBlack(ConfigUi.POPUP_TRANSITION_DURATION_IN, ConfigUi.MENU_BACKGROUND_ALPHA, Easing.Function.LINEAR);
                            this.<>f__this.PanelRoot.animateToBlack(ConfigUi.POPUP_TRANSITION_DURATION_IN, 0f);
                            break;
                        }
                        this.<>f__this.PanelRoot.transform.localScale = Vector3.one;
                        this.<>f__this.BackgroundOverlay.fadeToBlack(0f, ConfigUi.MENU_BACKGROUND_ALPHA, Easing.Function.LINEAR);
                        this.<>f__this.PanelRoot.setTransparent(false);
                        break;

                    case 1:
                        goto Label_0189;

                    case 2:
                        goto Label_01B1;

                    default:
                        goto Label_01E7;
                }
                this.<ie>__2 = this.<>f__this.m_content.show(this.parameter);
            Label_0189:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 1;
                    goto Label_01E9;
                }
            Label_01B1:
                while (this.<>f__this.BackgroundOverlay.IsAnimating || this.<>f__this.PanelRoot.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01E9;
                }
                goto Label_01E7;
                this.$PC = -1;
            Label_01E7:
                return false;
            Label_01E9:
                return true;
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

