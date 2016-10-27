namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class TechPopupMenu : Menu
    {
        public MenuOverlay BackgroundOverlay;
        public GameObject CloseButton;
        public RectTransform ContentAreaTm;
        public RectTransform DisabledContentTm;
        protected MenuContent m_content;
        private TransformAnimation m_panelTransformAnimation;
        public CanvasGroup PanelRoot;
        public Text TitleText;

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
            <hideRoutine>c__Iterator14B iteratorb = new <hideRoutine>c__Iterator14B();
            iteratorb.instant = instant;
            iteratorb.<$>instant = instant;
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        protected override void onAwake()
        {
            this.TitleText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_BUTTON_INFO, null, false));
            this.m_panelTransformAnimation = this.PanelRoot.gameObject.AddComponent<TransformAnimation>();
            this.BackgroundOverlay.fadeToTransparent(0f, Easing.Function.LINEAR);
        }

        public void onBackButtonClicked()
        {
        }

        public void onBackgroundOverlayClicked()
        {
            if ((this.CloseButton.activeSelf && !this.m_content.onBackgroundOverlayClicked()) && !PlayerView.Binder.MenuSystem.InTransition)
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

        protected override void onUpdate(float dt)
        {
            PlayerView.Binder.InputSystem.setInputRequirement(InputSystem.Layer.TechPopup, InputSystem.Requirement.MustBeEnabled);
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator149 iterator = new <preShowRoutine>c__Iterator149();
            iterator.targetMenuContentType = targetMenuContentType;
            iterator.parameter = parameter;
            iterator.<$>targetMenuContentType = targetMenuContentType;
            iterator.<$>parameter = parameter;
            iterator.<>f__this = this;
            return iterator;
        }

        public override void refreshTitle(string title, string additionalText1, string additionalText2)
        {
            this.TitleText.text = title;
        }

        public override void setCloseButtonVisibility(bool visible)
        {
            this.CloseButton.SetActive(visible);
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator14A iteratora = new <showRoutine>c__Iterator14A();
            iteratora.parameter = parameter;
            iteratora.<$>parameter = parameter;
            iteratora.<>f__this = this;
            return iteratora;
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
                return PlayerView.MenuType.TechPopupMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator14B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal TechPopupMenu <>f__this;
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
                        goto Label_0201;
                }
                while (!this.<timer>__1.Idle)
                {
                    this.<easedV>__2 = Easing.Apply(this.<timer>__1.normalizedProgress(), Easing.Function.LINEAR);
                    this.<>f__this.PanelRoot.alpha = 1f - this.<easedV>__2;
                    this.<timer>__1.tick(Time.deltaTime / Time.timeScale);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_0203;
                }
            Label_0153:
                while (this.<>f__this.BackgroundOverlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0203;
                }
            Label_0168:
                this.<>f__this.PanelRoot.transform.localScale = Vector3.zero;
                this.<>f__this.m_content.cleanup();
                this.<>f__this.m_content.transform.SetParent(this.<>f__this.DisabledContentTm, false);
                this.<>f__this.m_content.gameObject.SetActive(false);
                PlayerView.Binder.InputSystem.setInputRequirement(InputSystem.Layer.TechPopup, InputSystem.Requirement.Neutral);
                if (this.<>f__this.m_content.PauseGame)
                {
                    GameLogic.Binder.TimeSystem.pause(false);
                    goto Label_0201;
                    this.$PC = -1;
                }
            Label_0201:
                return false;
            Label_0203:
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
        private sealed class <preShowRoutine>c__Iterator149 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal MenuContentType <$>targetMenuContentType;
            internal TechPopupMenu <>f__this;
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
                    if (this.<>f__this.m_content.PauseGame)
                    {
                        GameLogic.Binder.TimeSystem.pause(true);
                    }
                    PlayerView.Binder.InputSystem.setInputRequirement(InputSystem.Layer.TechPopup, InputSystem.Requirement.MustBeEnabled);
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
        private sealed class <showRoutine>c__Iterator14A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal TechPopupMenu <>f__this;
            internal float <easedV>__4;
            internal IEnumerator <ie>__0;
            internal bool <instant>__1;
            internal ManualTimer <timer>__3;
            internal TransformAnimationTask <tt>__2;
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

                    case 2:
                        goto Label_01EE;

                    default:
                        goto Label_021F;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    goto Label_0221;
                }
                this.<instant>__1 = true;
                if (this.<instant>__1)
                {
                    this.<>f__this.PanelRoot.transform.localScale = Vector3.one;
                    this.<>f__this.BackgroundOverlay.fadeToBlack(0f, ConfigUi.MENU_BACKGROUND_ALPHA, Easing.Function.LINEAR);
                    this.<>f__this.PanelRoot.alpha = 1f;
                    goto Label_021F;
                }
                this.<>f__this.m_panelTransformAnimation.transform.localScale = (Vector3) (Vector3.one * ConfigUi.POPUP_TRANSITION_SCALE);
                this.<tt>__2 = new TransformAnimationTask(this.<>f__this.m_panelTransformAnimation.transform, ConfigUi.POPUP_TRANSITION_DURATION_IN, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__2.scale(Vector3.one, true, ConfigUi.MENU_EASING_OUT);
                this.<>f__this.m_panelTransformAnimation.addTask(this.<tt>__2);
                this.<>f__this.BackgroundOverlay.fadeToBlack(ConfigUi.POPUP_TRANSITION_DURATION_IN, ConfigUi.MENU_BACKGROUND_ALPHA, Easing.Function.LINEAR);
                this.<timer>__3 = new ManualTimer(ConfigUi.POPUP_TRANSITION_DURATION_IN);
            Label_01EE:
                while (!this.<timer>__3.Idle)
                {
                    this.<easedV>__4 = Easing.Apply(this.<timer>__3.normalizedProgress(), Easing.Function.LINEAR);
                    this.<>f__this.PanelRoot.alpha = this.<easedV>__4;
                    this.<timer>__3.tick(Time.deltaTime / Time.timeScale);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0221;
                }
                this.<>f__this.PanelRoot.alpha = 1f;
                goto Label_021F;
                this.$PC = -1;
            Label_021F:
                return false;
            Label_0221:
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

