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

    public class TabbedPopupMenu : Menu
    {
        public Text AdditionalText1;
        public Text AdditionalText2;
        public MenuOverlay BackgroundOverlay;
        public RectTransform ContentAreaTm;
        private int m_activeTabIdx = -1;
        private InputParameters m_inputParams;
        private TransformAnimation m_panelTransformAnimation;
        private List<MenuContent> m_tabContentObjects = new List<MenuContent>(4);
        public CanvasGroup PanelRoot;
        public List<IconWithText> TabButtons;
        public List<GameObject> TabUnlockNotifiers = new List<GameObject>();
        public List<GameObject> TabUpgrageNotifiers = new List<GameObject>();
        public Image TitleBackground;
        public Button TitleCloseButton;
        public Text TitleText;

        public override MenuContent activeContentObject()
        {
            return this.m_tabContentObjects[this.m_activeTabIdx];
        }

        public override MenuContentType activeContentType()
        {
            return this.activeContentObject().ContentType;
        }

        private object getInputParameterForActiveContentObject()
        {
            if (this.m_inputParams.ContentParams == null)
            {
                return null;
            }
            if (this.m_activeTabIdx >= this.m_inputParams.ContentParams.Count)
            {
                return null;
            }
            return this.m_inputParams.ContentParams[this.m_activeTabIdx];
        }

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator199 iterator = new <hideRoutine>c__Iterator199();
            iterator.instant = instant;
            iterator.<$>instant = instant;
            iterator.<>f__this = this;
            return iterator;
        }

        protected override void onAwake()
        {
            this.m_panelTransformAnimation = this.PanelRoot.gameObject.AddComponent<TransformAnimation>();
            this.BackgroundOverlay.fadeToTransparent(0f, Easing.Function.LINEAR);
            for (int i = 0; i < this.TabUnlockNotifiers.Count; i++)
            {
                this.TabUnlockNotifiers[i].SetActive(false);
            }
            for (int j = 0; j < this.TabUpgrageNotifiers.Count; j++)
            {
                this.TabUpgrageNotifiers[j].SetActive(false);
            }
            for (int k = 0; k < this.TabButtons.Count; k++)
            {
                this.TabButtons[k].gameObject.SetActive(false);
            }
        }

        public void onBackButtonClicked()
        {
        }

        public void onCloseButtonClicked()
        {
            if (!this.activeContentObject().onCloseButtonClicked() && !PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        protected override void onRefresh()
        {
            this.activeContentObject().refresh();
        }

        public void onTabButtonClicked(int idx)
        {
            if (this.m_activeTabIdx != idx)
            {
                this.setActiveTabIndex(idx);
            }
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator197 iterator = new <preShowRoutine>c__Iterator197();
            iterator.targetMenuContentType = targetMenuContentType;
            iterator.parameter = parameter;
            iterator.<$>targetMenuContentType = targetMenuContentType;
            iterator.<$>parameter = parameter;
            iterator.<>f__this = this;
            return iterator;
        }

        public override void refreshTabs()
        {
            bool flag = this.m_inputParams.TabbedMenuContentTypes.Count > 1;
            for (int i = 0; i < this.TabButtons.Count; i++)
            {
                IconWithText text = this.TabButtons[i];
                if (flag && (i < this.m_inputParams.TabbedMenuContentTypes.Count))
                {
                    text.gameObject.SetActive(true);
                    MenuContent content = this.m_tabContentObjects[i];
                    MenuContent.TabSpriteParameters tabSprite = content.TabSprite;
                    if (string.IsNullOrEmpty(tabSprite.SpriteId))
                    {
                        text.Text.gameObject.SetActive(true);
                        text.Icon.gameObject.SetActive(false);
                        text.Text.text = content.TabTitle;
                    }
                    else
                    {
                        text.Text.gameObject.SetActive(false);
                        text.Icon.gameObject.SetActive(true);
                        text.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(tabSprite.SpriteAtlasId, tabSprite.SpriteId);
                        if (tabSprite.SpriteSize.HasValue)
                        {
                            text.Icon.rectTransform.sizeDelta = tabSprite.SpriteSize.Value;
                        }
                        else
                        {
                            text.Icon.rectTransform.sizeDelta = new Vector2(108f, 108f);
                        }
                    }
                    if (i < this.TabUnlockNotifiers.Count)
                    {
                        this.TabUnlockNotifiers[i].SetActive(content.UnlockNotificationActive);
                    }
                    if (i < this.TabUpgrageNotifiers.Count)
                    {
                        this.TabUpgrageNotifiers[i].SetActive(content.UpgradeNotificationActive);
                    }
                }
                else
                {
                    text.gameObject.SetActive(false);
                }
                if (i == this.m_activeTabIdx)
                {
                    text.Background.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "uix_tab_active_0");
                }
                else
                {
                    text.Background.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "uix_tab_inactive_0");
                }
            }
        }

        public override void refreshTitle(string title, string additionalText1, string additionalText2)
        {
            this.TitleText.text = title;
            this.AdditionalText1.text = additionalText1;
            this.AdditionalText2.text = additionalText2;
        }

        public void setActiveTabIndex(int idx)
        {
            if (this.m_activeTabIdx != idx)
            {
                if ((this.m_activeTabIdx != -1) && this.activeContentObject().gameObject.activeSelf)
                {
                    this.activeContentObject().cleanup();
                    this.activeContentObject().gameObject.SetActive(false);
                }
                this.m_activeTabIdx = idx;
                if (this.m_activeTabIdx != -1)
                {
                    this.activeContentObject().gameObject.SetActive(true);
                    this.activeContentObject().preShow(this, this.getInputParameterForActiveContentObject());
                }
            }
            if (this.m_activeTabIdx != -1)
            {
                this.refreshTabs();
            }
        }

        public override void setCloseButtonVisibility(bool visible)
        {
            this.TitleCloseButton.enabled = visible;
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator198 iterator = new <showRoutine>c__Iterator198();
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
                return PlayerView.MenuType.TabbedPopupMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator199 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal TabbedPopupMenu <>f__this;
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
                        goto Label_01B4;
                }
                while (!this.<timer>__1.Idle)
                {
                    this.<easedV>__2 = Easing.Apply(this.<timer>__1.normalizedProgress(), Easing.Function.LINEAR);
                    this.<>f__this.PanelRoot.alpha = 1f - this.<easedV>__2;
                    this.<timer>__1.tick(Time.deltaTime / Time.timeScale);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01B6;
                }
            Label_0153:
                while (this.<>f__this.BackgroundOverlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01B6;
                }
            Label_0168:
                this.<>f__this.PanelRoot.transform.localScale = Vector3.zero;
                this.<>f__this.activeContentObject().cleanup();
                this.<>f__this.activeContentObject().gameObject.SetActive(false);
                goto Label_01B4;
                this.$PC = -1;
            Label_01B4:
                return false;
            Label_01B6:
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
        private sealed class <preShowRoutine>c__Iterator197 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal MenuContentType <$>targetMenuContentType;
            internal TabbedPopupMenu <>f__this;
            internal MenuContentType <contentType>__1;
            internal int <i>__0;
            internal MenuContent <mc>__2;
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
                    if (this.targetMenuContentType != MenuContentType.NONE)
                    {
                        TabbedPopupMenu.InputParameters parameters = new TabbedPopupMenu.InputParameters();
                        List<MenuContentType> list = new List<MenuContentType>();
                        list.Add(this.targetMenuContentType);
                        parameters.TabbedMenuContentTypes = list;
                        List<object> list2 = new List<object>();
                        list2.Add(this.parameter);
                        parameters.ContentParams = list2;
                        this.<>f__this.m_inputParams = parameters;
                    }
                    else
                    {
                        this.<>f__this.m_inputParams = (TabbedPopupMenu.InputParameters) this.parameter;
                    }
                    this.<>f__this.m_tabContentObjects.Clear();
                    this.<i>__0 = 0;
                    while (this.<i>__0 < this.<>f__this.m_inputParams.TabbedMenuContentTypes.Count)
                    {
                        this.<contentType>__1 = this.<>f__this.m_inputParams.TabbedMenuContentTypes[this.<i>__0];
                        this.<mc>__2 = PlayerView.Binder.MenuContentResources.getSharedResource(this.<contentType>__1).GetComponent<MenuContent>();
                        this.<mc>__2.RectTm.SetParent(this.<>f__this.ContentAreaTm, false);
                        this.<mc>__2.gameObject.SetActive(false);
                        this.<>f__this.m_tabContentObjects.Add(this.<mc>__2);
                        this.<i>__0++;
                    }
                    if (this.<>f__this.m_inputParams.OverrideOpenTabIndex.HasValue && (this.<>f__this.m_inputParams.OverrideOpenTabIndex.Value != this.<>f__this.m_activeTabIdx))
                    {
                        this.<>f__this.setActiveTabIndex(this.<>f__this.m_inputParams.OverrideOpenTabIndex.Value);
                    }
                    else if (this.<>f__this.m_activeTabIdx == -1)
                    {
                        this.<>f__this.setActiveTabIndex(0);
                    }
                    else
                    {
                        this.<>f__this.activeContentObject().gameObject.SetActive(true);
                        this.<>f__this.activeContentObject().preShow(this.<>f__this, this.<>f__this.getInputParameterForActiveContentObject());
                        this.<>f__this.refreshTabs();
                    }
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
        private sealed class <showRoutine>c__Iterator198 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TabbedPopupMenu <>f__this;
            internal float <easedV>__3;
            internal bool <instant>__0;
            internal ManualTimer <timer>__2;
            internal TransformAnimationTask <tt>__1;

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
                            this.<timer>__2 = new ManualTimer(ConfigUi.POPUP_TRANSITION_DURATION_IN);
                            break;
                        }
                        this.<>f__this.BackgroundOverlay.fadeToBlack(0f, ConfigUi.MENU_BACKGROUND_ALPHA, Easing.Function.LINEAR);
                        this.<>f__this.PanelRoot.alpha = 1f;
                        goto Label_01B3;

                    case 1:
                        break;

                    default:
                        goto Label_01B3;
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
                goto Label_01B3;
                this.$PC = -1;
            Label_01B3:
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

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public List<MenuContentType> TabbedMenuContentTypes;
            public List<object> ContentParams;
            public int? OverrideOpenTabIndex;
        }
    }
}

