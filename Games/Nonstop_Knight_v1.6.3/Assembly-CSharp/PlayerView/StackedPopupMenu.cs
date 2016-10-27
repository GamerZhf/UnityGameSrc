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
    using UnityEngine.UI;

    public class StackedPopupMenu : Menu
    {
        public Text AdditionalText1;
        public Text AdditionalText2;
        public MenuOverlay BackgroundOverlay;
        public RectTransform ContentAreaTm;
        private TransformAnimation m_panelTransformAnimation;
        public CanvasGroup PanelRoot;
        public StackedMenuContentController Smcc;
        public Image TitleBackground;
        public Button TitleCloseButton;
        public Text TitleText;

        public override MenuContent activeContentObject()
        {
            return this.Smcc.getTopmostContent();
        }

        public override MenuContentType activeContentType()
        {
            return this.Smcc.getTopmostContentType();
        }

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator196 iterator = new <hideRoutine>c__Iterator196();
            iterator.instant = instant;
            iterator.<$>instant = instant;
            iterator.<>f__this = this;
            return iterator;
        }

        protected override void onAwake()
        {
            this.m_panelTransformAnimation = this.PanelRoot.gameObject.AddComponent<TransformAnimation>();
            this.BackgroundOverlay.fadeToTransparent(0f, Easing.Function.LINEAR);
        }

        public void onBackButtonClicked()
        {
            this.Smcc.onBackButtonClicked();
        }

        public void onCloseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                MenuContent content = this.activeContentObject();
                if (((content != null) && !content.onCloseButtonClicked()) && !PlayerView.Binder.MenuSystem.InTransition)
                {
                    PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                }
            }
        }

        protected override void onRefresh()
        {
            MenuContent content = this.activeContentObject();
            if (content != null)
            {
                content.refresh();
            }
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator194 iterator = new <preShowRoutine>c__Iterator194();
            iterator.<>f__this = this;
            return iterator;
        }

        public override void refreshTitle(string title, string additionalText1, string additionalText2)
        {
            this.TitleText.text = title;
            this.AdditionalText1.text = additionalText1;
            this.AdditionalText2.text = additionalText2;
        }

        public override void setCloseButtonVisibility(bool visible)
        {
            this.TitleCloseButton.enabled = visible;
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator195 iterator = new <showRoutine>c__Iterator195();
            iterator.targetMenuContentType = targetMenuContentType;
            iterator.parameter = parameter;
            iterator.<$>targetMenuContentType = targetMenuContentType;
            iterator.<$>parameter = parameter;
            iterator.<>f__this = this;
            return iterator;
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
                return PlayerView.MenuType.StackedPopupMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator196 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal StackedPopupMenu <>f__this;
            internal float <easedV>__2;
            internal ManualTimer <timer>__1;
            internal TransformAnimationTask <tt>__0;
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
                            this.<tt>__0 = new TransformAnimationTask(this.<>f__this.m_panelTransformAnimation.transform, ConfigUi.POPUP_TRANSITION_DURATION_OUT, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                            this.<tt>__0.scale((Vector3) (Vector3.one * ConfigUi.POPUP_TRANSITION_SCALE), true, ConfigUi.POPUP_EASING_OUT);
                            this.<>f__this.m_panelTransformAnimation.addTask(this.<tt>__0);
                            this.<>f__this.BackgroundOverlay.fadeToTransparent(ConfigUi.POPUP_TRANSITION_DURATION_OUT, Easing.Function.LINEAR);
                            this.<timer>__1 = new ManualTimer(ConfigUi.POPUP_TRANSITION_DURATION_OUT);
                            break;
                        }
                        this.<>f__this.BackgroundOverlay.setTransparent(true);
                        goto Label_0168;

                    case 1:
                        break;

                    case 2:
                        goto Label_0153;

                    default:
                        goto Label_019E;
                }
                while (!this.<timer>__1.Idle)
                {
                    this.<easedV>__2 = Easing.Apply(this.<timer>__1.normalizedProgress(), Easing.Function.LINEAR);
                    this.<>f__this.PanelRoot.alpha = 1f - this.<easedV>__2;
                    this.<timer>__1.tick(Time.deltaTime / Time.timeScale);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01A0;
                }
            Label_0153:
                while (this.<>f__this.BackgroundOverlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01A0;
                }
            Label_0168:
                this.<>f__this.PanelRoot.transform.localScale = Vector3.zero;
                this.<>f__this.Smcc.popAllContent();
                goto Label_019E;
                this.$PC = -1;
            Label_019E:
                return false;
            Label_01A0:
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
        private sealed class <preShowRoutine>c__Iterator194 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal StackedPopupMenu <>f__this;

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
                    this.<>f__this.Smcc.initialize(this.<>f__this);
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
        private sealed class <showRoutine>c__Iterator195 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal MenuContentType <$>targetMenuContentType;
            internal StackedPopupMenu <>f__this;
            internal float <easedV>__3;
            internal bool <instant>__0;
            internal ManualTimer <timer>__2;
            internal TransformAnimationTask <tt>__1;
            internal object parameter;
            internal MenuContentType targetMenuContentType;

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
                        this.<>f__this.Smcc.pushContent(this.targetMenuContentType, this.parameter, false);
                        this.<instant>__0 = false;
                        if (!this.<instant>__0)
                        {
                            this.<>f__this.m_panelTransformAnimation.transform.localScale = (Vector3) (Vector3.one * ConfigUi.POPUP_TRANSITION_SCALE);
                            this.<tt>__1 = new TransformAnimationTask(this.<>f__this.m_panelTransformAnimation.transform, ConfigUi.POPUP_TRANSITION_DURATION_IN, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                            this.<tt>__1.scale(Vector3.one, true, ConfigUi.MENU_EASING_OUT);
                            this.<>f__this.m_panelTransformAnimation.addTask(this.<tt>__1);
                            this.<>f__this.BackgroundOverlay.fadeToBlack(ConfigUi.POPUP_TRANSITION_DURATION_IN, ConfigUi.MENU_BACKGROUND_ALPHA, Easing.Function.LINEAR);
                            this.<timer>__2 = new ManualTimer(ConfigUi.POPUP_TRANSITION_DURATION_IN);
                            break;
                        }
                        this.<>f__this.m_panelTransformAnimation.transform.localScale = Vector3.one;
                        this.<>f__this.BackgroundOverlay.fadeToBlack(0f, ConfigUi.MENU_BACKGROUND_ALPHA, Easing.Function.LINEAR);
                        this.<>f__this.PanelRoot.alpha = 1f;
                        goto Label_01D0;

                    case 1:
                        break;

                    default:
                        goto Label_01D0;
                }
                while (!this.<timer>__2.Idle)
                {
                    this.<easedV>__3 = Easing.Apply(this.<timer>__2.normalizedProgress(), Easing.Function.LINEAR);
                    this.<>f__this.PanelRoot.alpha = this.<easedV>__3;
                    this.<timer>__2.tick(Time.deltaTime / Time.timeScale);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<>f__this.PanelRoot.alpha = 1f;
                goto Label_01D0;
                this.$PC = -1;
            Label_01D0:
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

