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
    using UnityEngine.EventSystems;

    public class TooltipMenu : Menu
    {
        public MenuOverlay BackgroundOverlay;
        public RectTransform ContentAreaTm;
        private Vector3 m_centerOnTargetLastPosition;
        private MenuContent m_content;
        private InputParameters m_inputParams;
        private Transform m_panelRootTm;
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
            <hideRoutine>c__Iterator19F iteratorf = new <hideRoutine>c__Iterator19F();
            iteratorf.instant = instant;
            iteratorf.<$>instant = instant;
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        protected override void onAwake()
        {
            this.m_panelRootTm = this.PanelRoot.transform;
            this.m_panelTransformAnimation = this.PanelRoot.gameObject.AddComponent<TransformAnimation>();
            this.BackgroundOverlay.setTransparent(true);
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
            if (((this.m_panelRootTm.localScale == Vector3.one) && !PlayerView.Binder.MenuSystem.InTransition) && PlayerView.Binder.InputSystem.InputEnabled)
            {
                if (this.m_inputParams.CenterOnTm != null)
                {
                    bool flag = Input.GetMouseButton(0) || (Input.touchCount > 0);
                    bool flag2 = this.m_centerOnTargetLastPosition != this.m_inputParams.CenterOnTm.position;
                    if (flag && flag2)
                    {
                        this.BackgroundOverlay.setTransparent(true);
                        this.m_content.gameObject.SetActive(false);
                    }
                    this.m_centerOnTargetLastPosition = this.m_inputParams.CenterOnTm.position;
                }
                if (Input.GetMouseButtonUp(0) || ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended)))
                {
                    PointerEventData eventData = new PointerEventData(EventSystem.current);
                    if (PlayerView.Binder.InputSystem.UsesTouch)
                    {
                        eventData.position = Input.GetTouch(0).position;
                    }
                    else
                    {
                        eventData.position = Input.mousePosition;
                    }
                    List<RaycastResult> raycastResults = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(eventData, raycastResults);
                    bool flag3 = false;
                    for (int i = 0; i < raycastResults.Count; i++)
                    {
                        RaycastResult result = raycastResults[i];
                        if (result.gameObject == this.m_content.gameObject)
                        {
                            flag3 = true;
                            break;
                        }
                    }
                    if (!flag3)
                    {
                        this.onCloseButtonClicked();
                    }
                }
            }
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator19D iteratord = new <preShowRoutine>c__Iterator19D();
            iteratord.parameter = parameter;
            iteratord.targetMenuContentType = targetMenuContentType;
            iteratord.<$>parameter = parameter;
            iteratord.<$>targetMenuContentType = targetMenuContentType;
            iteratord.<>f__this = this;
            return iteratord;
        }

        public void pushContent(MenuContentType menuContentType, [Optional, DefaultParameterValue(null)] object param, [Optional, DefaultParameterValue(true)] bool instant)
        {
        }

        public void refreshArrowPosition(RectTransform arrowRectTm, Vector3 arrowDefaultLocalPosition)
        {
            if (this.m_inputParams.CenterOnTm != null)
            {
                Vector3 vector2;
                Camera menuCamera = PlayerView.Binder.MenuSystem.MenuCamera;
                Vector3 screenPoint = menuCamera.WorldToScreenPoint(this.m_inputParams.CenterOnTm.position);
                RectTransformUtility.ScreenPointToWorldPointInRectangle(base.RectTm, screenPoint, menuCamera, out vector2);
                arrowRectTm.position = vector2;
                arrowRectTm.localPosition = new Vector3(arrowRectTm.localPosition.x, arrowDefaultLocalPosition.y, arrowDefaultLocalPosition.z);
            }
        }

        public override void setCloseButtonVisibility(bool visible)
        {
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator19E iteratore = new <showRoutine>c__Iterator19E();
            iteratore.<>f__this = this;
            return iteratore;
        }

        [ContextMenu("test()")]
        private void test()
        {
            base.StopAllCoroutines();
            base.Awake();
            base.StartCoroutine(this.showRoutine(MenuContentType.NONE, SkillType.Whirlwind));
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
                return PlayerView.MenuType.TooltipMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator19F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal TooltipMenu <>f__this;
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
                            goto Label_0147;
                        }
                        this.<tt>__0 = new TransformAnimationTask(this.<>f__this.m_panelRootTm, ConfigUi.TOOLTIP_TRANSITION_DURATION_OUT, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__0.scale((Vector3) (Vector3.one * ConfigUi.TOOLTIP_TRANSITION_SCALE), true, ConfigUi.TOOLTIP_EASING_OUT);
                        this.<>f__this.m_panelTransformAnimation.addTask(this.<tt>__0);
                        this.<>f__this.BackgroundOverlay.fadeToTransparent(ConfigUi.TOOLTIP_TRANSITION_DURATION_OUT, Easing.Function.LINEAR);
                        this.<timer>__1 = new ManualTimer(ConfigUi.TOOLTIP_TRANSITION_DURATION_OUT);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_0132;

                    default:
                        goto Label_01AF;
                }
                if (!this.<timer>__1.Idle)
                {
                    this.<easedV>__2 = Easing.Apply(this.<timer>__1.normalizedProgress(), Easing.Function.LINEAR);
                    this.<>f__this.PanelRoot.alpha = 1f - this.<easedV>__2;
                    this.<timer>__1.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01B1;
                }
            Label_0132:
                while (this.<>f__this.BackgroundOverlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01B1;
                }
            Label_0147:
                this.<>f__this.m_panelRootTm.localScale = Vector3.zero;
                this.<>f__this.m_content.cleanup();
                this.<>f__this.m_content.transform.SetParent(this.<>f__this.transform, false);
                this.<>f__this.m_content.gameObject.SetActive(false);
                goto Label_01AF;
                this.$PC = -1;
            Label_01AF:
                return false;
            Label_01B1:
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
        private sealed class <preShowRoutine>c__Iterator19D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal MenuContentType <$>targetMenuContentType;
            internal TooltipMenu <>f__this;
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
                    this.<>f__this.m_inputParams = (TooltipMenu.InputParameters) this.parameter;
                    if (this.<>f__this.m_inputParams.CenterOnTm != null)
                    {
                        this.<>f__this.m_centerOnTargetLastPosition = this.<>f__this.m_inputParams.CenterOnTm.position;
                    }
                    this.<>f__this.setCloseButtonVisibility(true);
                    this.<contentObj>__0 = PlayerView.Binder.MenuContentResources.getSharedResource(this.targetMenuContentType);
                    this.<contentObj>__0.transform.SetParent(this.<>f__this.ContentAreaTm, false);
                    this.<>f__this.m_content = this.<contentObj>__0.GetComponent<MenuContent>();
                    this.<contentObj>__0.SetActive(true);
                    this.<>f__this.m_content.preShow(this.<>f__this, this.<>f__this.m_inputParams.MenuContentParams);
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
        private sealed class <showRoutine>c__Iterator19E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TooltipMenu <>f__this;
            internal RectTransform <centerOnTm>__3;
            internal RectTransform <contentRectTm>__1;
            internal float <easedV>__8;
            internal IEnumerator <ie>__9;
            internal Camera <menuCam>__2;
            internal Vector3 <screenPt>__4;
            internal ManualTimer <timer>__7;
            internal TransformAnimationTask <tt>__0;
            internal Vector2 <verticalOffset>__6;
            internal Vector3 <worldPt>__5;

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
                        this.<>f__this.m_panelRootTm.localScale = (Vector3) (Vector3.one * ConfigUi.TOOLTIP_TRANSITION_SCALE);
                        this.<>f__this.PanelRoot.alpha = 0f;
                        this.<>f__this.PanelRoot.interactable = this.<>f__this.m_content.CapturesInput;
                        this.<>f__this.PanelRoot.blocksRaycasts = this.<>f__this.m_content.CapturesInput;
                        this.<tt>__0 = new TransformAnimationTask(this.<>f__this.m_panelRootTm, ConfigUi.TOOLTIP_TRANSITION_DURATION_IN, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__0.scale(Vector3.one, true, ConfigUi.MENU_EASING_OUT);
                        this.<>f__this.m_panelTransformAnimation.addTask(this.<tt>__0);
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_03DD;

                    case 1:
                        this.<contentRectTm>__1 = this.<>f__this.m_content.GetComponent<RectTransform>();
                        this.<menuCam>__2 = PlayerView.Binder.MenuSystem.MenuCamera;
                        this.<centerOnTm>__3 = null;
                        if (this.<>f__this.m_inputParams.CenterOnTm != null)
                        {
                            this.<centerOnTm>__3 = this.<>f__this.m_inputParams.CenterOnTm;
                        }
                        if (this.<centerOnTm>__3 == null)
                        {
                            this.<contentRectTm>__1.anchoredPosition3D = Vector3.zero;
                            break;
                        }
                        this.<screenPt>__4 = this.<menuCam>__2.WorldToScreenPoint(this.<centerOnTm>__3.position);
                        RectTransformUtility.ScreenPointToWorldPointInRectangle(this.<>f__this.ContentAreaTm, this.<screenPt>__4, this.<menuCam>__2, out this.<worldPt>__5);
                        this.<contentRectTm>__1.position = this.<worldPt>__5;
                        this.<verticalOffset>__6 = new Vector2(0f, (this.<centerOnTm>__3.rect.height * 0.5f) + 30f);
                        if (this.<contentRectTm>__1.anchoredPosition.y > (this.<>f__this.ContentAreaTm.rect.height * 0.25f))
                        {
                            this.<contentRectTm>__1.anchoredPosition -= this.<verticalOffset>__6;
                        }
                        else
                        {
                            this.<contentRectTm>__1.anchoredPosition += this.<verticalOffset>__6;
                        }
                        UiUtil.LimitToParentBounds(this.<contentRectTm>__1, this.<>f__this.ContentAreaTm, 50f, 160f);
                        break;

                    case 2:
                        goto Label_0325;

                    case 3:
                        goto Label_0362;

                    case 4:
                        goto Label_03BF;

                    default:
                        goto Label_03DB;
                }
                this.<>f__this.BackgroundOverlay.fadeToBlack(ConfigUi.TOOLTIP_TRANSITION_DURATION_IN, ConfigUi.TOOLTIP_MENU_BACKGROUND_ALPHA, Easing.Function.LINEAR);
                this.<timer>__7 = new ManualTimer(ConfigUi.TOOLTIP_TRANSITION_DURATION_IN);
            Label_0325:
                while (!this.<timer>__7.Idle)
                {
                    this.<easedV>__8 = Easing.Apply(this.<timer>__7.normalizedProgress(), Easing.Function.LINEAR);
                    this.<>f__this.PanelRoot.alpha = this.<easedV>__8;
                    this.<timer>__7.tick(Time.deltaTime / Time.timeScale);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_03DD;
                }
                this.<>f__this.PanelRoot.alpha = 1f;
            Label_0362:
                while (this.<>f__this.BackgroundOverlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_03DD;
                }
                this.<ie>__9 = this.<>f__this.m_content.show(this.<>f__this.m_inputParams.MenuContentParams);
            Label_03BF:
                while (this.<ie>__9.MoveNext())
                {
                    this.$current = this.<ie>__9.Current;
                    this.$PC = 4;
                    goto Label_03DD;
                }
                goto Label_03DB;
                this.$PC = -1;
            Label_03DB:
                return false;
            Label_03DD:
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

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public RectTransform CenterOnTm;
            public object MenuContentParams;
        }
    }
}

