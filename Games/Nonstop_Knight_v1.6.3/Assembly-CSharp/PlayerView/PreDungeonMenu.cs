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

    public class PreDungeonMenu : Menu
    {
        public RectTransform ContentAreaTm;
        private MenuContent m_content;
        private TransformAnimation m_panelTransformAnimation;
        public CanvasGroup PanelRoot;

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
            <hideRoutine>c__Iterator164 iterator = new <hideRoutine>c__Iterator164();
            iterator.instant = instant;
            iterator.<$>instant = instant;
            iterator.<>f__this = this;
            return iterator;
        }

        protected override void onAwake()
        {
            this.m_panelTransformAnimation = this.PanelRoot.gameObject.AddComponent<TransformAnimation>();
        }

        public void onBackButtonClicked()
        {
        }

        public void onCloseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(false);
            }
        }

        protected override void onUpdate(float dt)
        {
            if (PlayerView.Binder.InputSystem.InputEnabled && (Input.GetMouseButtonUp(0) || ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended))))
            {
                this.onCloseButtonClicked();
            }
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator162 iterator = new <preShowRoutine>c__Iterator162();
            iterator.targetMenuContentType = targetMenuContentType;
            iterator.parameter = parameter;
            iterator.<$>targetMenuContentType = targetMenuContentType;
            iterator.<$>parameter = parameter;
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator163 iterator = new <showRoutine>c__Iterator163();
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
                return PlayerView.MenuType.PreDungeonMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator164 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal PreDungeonMenu <>f__this;
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
                        if (this.instant)
                        {
                            goto Label_0105;
                        }
                        this.<tt>__0 = new TransformAnimationTask(this.<>f__this.m_panelTransformAnimation.transform, ConfigUi.POPUP_TRANSITION_DURATION_OUT, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__0.scale((Vector3) (Vector3.one * ConfigUi.POPUP_TRANSITION_SCALE), true, ConfigUi.POPUP_EASING_OUT);
                        this.<>f__this.m_panelTransformAnimation.addTask(this.<tt>__0);
                        this.<timer>__1 = new ManualTimer(ConfigUi.POPUP_TRANSITION_DURATION_OUT);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0183;
                }
                if (!this.<timer>__1.Idle)
                {
                    this.<easedV>__2 = Easing.Apply(this.<timer>__1.normalizedProgress(), Easing.Function.LINEAR);
                    this.<>f__this.PanelRoot.alpha = 1f - this.<easedV>__2;
                    this.<timer>__1.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
            Label_0105:
                this.<>f__this.PanelRoot.gameObject.SetActive(false);
                this.<>f__this.PanelRoot.alpha = 1f;
                this.<>f__this.m_content.cleanup();
                this.<>f__this.m_content.transform.SetParent(this.<>f__this.transform, false);
                this.<>f__this.m_content.gameObject.SetActive(false);
                goto Label_0183;
                this.$PC = -1;
            Label_0183:
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
        private sealed class <preShowRoutine>c__Iterator162 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal MenuContentType <$>targetMenuContentType;
            internal PreDungeonMenu <>f__this;
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
        private sealed class <showRoutine>c__Iterator163 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal PreDungeonMenu <>f__this;
            internal RectTransform <contentRectTm>__0;
            internal float <easedV>__8;
            internal Camera <menuCam>__1;
            internal Vector3 <screenPt>__3;
            internal RectTransform <selectedRectTm>__2;
            internal ManualTimer <timer>__7;
            internal TransformAnimationTask <tt>__5;
            internal Vector2 <verticalOffset>__6;
            internal Vector3 <worldPt>__4;

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
                        this.<>f__this.PanelRoot.gameObject.SetActive(true);
                        this.<>f__this.m_panelTransformAnimation.transform.localScale = Vector3.one;
                        this.<>f__this.PanelRoot.interactable = this.<>f__this.m_content.CapturesInput;
                        this.<>f__this.PanelRoot.blocksRaycasts = this.<>f__this.m_content.CapturesInput;
                        this.<contentRectTm>__0 = this.<>f__this.m_content.GetComponent<RectTransform>();
                        this.<menuCam>__1 = PlayerView.Binder.MenuSystem.MenuCamera;
                        this.<selectedRectTm>__2 = PlayerView.Binder.InputSystem.getRectTransformUnderMouse();
                        if (this.<selectedRectTm>__2 == null)
                        {
                            UnityEngine.Debug.LogWarning("Valid RectTransform not found for click handling.");
                            break;
                        }
                        this.<screenPt>__3 = this.<menuCam>__1.WorldToScreenPoint(this.<selectedRectTm>__2.position);
                        RectTransformUtility.ScreenPointToWorldPointInRectangle(this.<>f__this.ContentAreaTm, this.<screenPt>__3, this.<menuCam>__1, out this.<worldPt>__4);
                        this.<contentRectTm>__0.position = this.<worldPt>__4;
                        break;

                    case 1:
                        goto Label_02DB;

                    default:
                        goto Label_030C;
                }
                this.<>f__this.m_panelTransformAnimation.transform.localScale = (Vector3) (Vector3.one * ConfigUi.POPUP_TRANSITION_SCALE);
                this.<tt>__5 = new TransformAnimationTask(this.<>f__this.m_panelTransformAnimation.transform, ConfigUi.POPUP_TRANSITION_DURATION_IN, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__5.scale(Vector3.one, true, ConfigUi.MENU_EASING_OUT);
                this.<>f__this.m_panelTransformAnimation.addTask(this.<tt>__5);
                this.<verticalOffset>__6 = new Vector2(0f, (this.<contentRectTm>__0.rect.height * 0.5f) + 100f);
                if (this.<contentRectTm>__0.anchoredPosition.y > (this.<>f__this.ContentAreaTm.rect.height * 0.25f))
                {
                    this.<contentRectTm>__0.anchoredPosition -= this.<verticalOffset>__6;
                }
                else
                {
                    this.<contentRectTm>__0.anchoredPosition += this.<verticalOffset>__6;
                }
                UiUtil.LimitToParentBounds(this.<contentRectTm>__0, this.<>f__this.ContentAreaTm, 50f, 160f);
                this.<timer>__7 = new ManualTimer(ConfigUi.POPUP_TRANSITION_DURATION_IN);
            Label_02DB:
                while (!this.<timer>__7.Idle)
                {
                    this.<easedV>__8 = Easing.Apply(this.<timer>__7.normalizedProgress(), Easing.Function.LINEAR);
                    this.<>f__this.PanelRoot.alpha = this.<easedV>__8;
                    this.<timer>__7.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<>f__this.PanelRoot.alpha = 1f;
                goto Label_030C;
                this.$PC = -1;
            Label_030C:
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

