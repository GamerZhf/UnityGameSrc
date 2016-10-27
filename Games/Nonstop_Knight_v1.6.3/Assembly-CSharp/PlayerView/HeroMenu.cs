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

    public class HeroMenu : Menu
    {
        public MenuOverlay BackgroundOverlay;
        public Button CloseButton;
        public Text InfoBarTitle;
        private int m_activeTabIdx = -1;
        private TransformAnimation m_panelTransformAnimation;
        public const int OPTIONS_TAB_INDEX = 1;
        public CanvasGroup PanelRoot;
        public List<IconWithText> TabButtons;
        public List<GameObject> TabContentRoots;

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator11F iteratorf = new <hideRoutine>c__Iterator11F();
            iteratorf.instant = instant;
            iteratorf.<$>instant = instant;
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        protected override void onAwake()
        {
            for (int i = 0; i < this.TabContentRoots.Count; i++)
            {
                this.TabContentRoots[i].SetActive(false);
            }
            this.m_panelTransformAnimation = this.PanelRoot.gameObject.AddComponent<TransformAnimation>();
            this.BackgroundOverlay.setTransparent(true);
        }

        public void onCloseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        protected override void onRefresh()
        {
            if (this.m_activeTabIdx == 1)
            {
            }
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
            <preShowRoutine>c__Iterator11D iteratord = new <preShowRoutine>c__Iterator11D();
            iteratord.parameter = parameter;
            iteratord.<$>parameter = parameter;
            iteratord.<>f__this = this;
            return iteratord;
        }

        private void reconstructContent()
        {
            if (this.m_activeTabIdx == 1)
            {
                this.InfoBarTitle.text = "OPTIONS";
            }
            this.onRefresh();
        }

        public void setActiveTabIndex(int idx)
        {
            if (this.m_activeTabIdx != idx)
            {
                if (this.m_activeTabIdx != -1)
                {
                    this.TabContentRoots[this.m_activeTabIdx].SetActive(false);
                    this.TabButtons[this.m_activeTabIdx].Background.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "uix_tab_inactive_0");
                }
                this.m_activeTabIdx = idx;
                this.TabContentRoots[this.m_activeTabIdx].SetActive(true);
                this.TabButtons[this.m_activeTabIdx].Background.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "uix_tab_active_0");
            }
            this.reconstructContent();
        }

        public void setTabVisibility(int idx, bool visible)
        {
            this.TabButtons[idx].gameObject.SetActive(visible);
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator11E iteratore = new <showRoutine>c__Iterator11E();
            iteratore.<>f__this = this;
            return iteratore;
        }

        public int ActiveTabIndex
        {
            get
            {
                return this.m_activeTabIdx;
            }
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
                return PlayerView.MenuType.HeroMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator11F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal HeroMenu <>f__this;
            internal float <easedV>__2;
            internal int <i>__3;
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
                            this.<>f__this.BackgroundOverlay.fadeToTransparent(0f, Easing.Function.LINEAR);
                            goto Label_0167;
                        }
                        this.<tt>__0 = new TransformAnimationTask(this.<>f__this.m_panelTransformAnimation.transform, ConfigUi.POPUP_TRANSITION_DURATION_OUT, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__0.scale((Vector3) (Vector3.one * ConfigUi.POPUP_TRANSITION_SCALE), true, ConfigUi.POPUP_EASING_OUT);
                        this.<>f__this.m_panelTransformAnimation.addTask(this.<tt>__0);
                        this.<>f__this.BackgroundOverlay.fadeToTransparent(ConfigUi.POPUP_TRANSITION_DURATION_OUT, Easing.Function.LINEAR);
                        this.<timer>__1 = new ManualTimer(ConfigUi.POPUP_TRANSITION_DURATION_OUT);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_0137;

                    default:
                        goto Label_01E5;
                }
                if (!this.<timer>__1.Idle)
                {
                    this.<easedV>__2 = Easing.Apply(this.<timer>__1.normalizedProgress(), Easing.Function.LINEAR);
                    this.<>f__this.PanelRoot.alpha = 1f - this.<easedV>__2;
                    this.<timer>__1.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01E7;
                }
            Label_0137:
                while (this.<>f__this.BackgroundOverlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01E7;
                }
            Label_0167:
                this.<>f__this.PanelRoot.gameObject.SetActive(false);
                this.<>f__this.PanelRoot.alpha = 1f;
                this.<i>__3 = 0;
                while (this.<i>__3 < this.<>f__this.TabButtons.Count)
                {
                    this.<>f__this.setTabVisibility(this.<i>__3, true);
                    this.<i>__3++;
                }
                goto Label_01E5;
                this.$PC = -1;
            Label_01E5:
                return false;
            Label_01E7:
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
        private sealed class <preShowRoutine>c__Iterator11D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal HeroMenu <>f__this;
            internal int <i>__1;
            internal HeroMenu.InputParameters <parameters>__0;
            internal object parameter;

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
                    if (this.parameter != null)
                    {
                        this.<parameters>__0 = (HeroMenu.InputParameters) this.parameter;
                        this.<>f__this.setActiveTabIndex(this.<parameters>__0.ActiveTabIndex);
                        if (this.<parameters>__0.HiddenTabIndices != null)
                        {
                            this.<i>__1 = 0;
                            while (this.<i>__1 < this.<parameters>__0.HiddenTabIndices.Count)
                            {
                                this.<>f__this.setTabVisibility(this.<parameters>__0.HiddenTabIndices[this.<i>__1], false);
                                this.<i>__1++;
                            }
                        }
                    }
                    else if (this.<>f__this.m_activeTabIdx == -1)
                    {
                        this.<>f__this.setActiveTabIndex(0);
                    }
                    else
                    {
                        this.<>f__this.setActiveTabIndex(this.<>f__this.m_activeTabIdx);
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
        private sealed class <showRoutine>c__Iterator11E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal HeroMenu <>f__this;
            internal float <easedV>__2;
            internal ManualTimer <timer>__1;
            internal TransformAnimationTask <tt>__0;

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
                        this.<>f__this.m_panelTransformAnimation.transform.localScale = (Vector3) (Vector3.one * ConfigUi.POPUP_TRANSITION_SCALE);
                        this.<tt>__0 = new TransformAnimationTask(this.<>f__this.m_panelTransformAnimation.transform, ConfigUi.POPUP_TRANSITION_DURATION_IN, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__0.scale(Vector3.one, true, ConfigUi.MENU_EASING_OUT);
                        this.<>f__this.m_panelTransformAnimation.addTask(this.<tt>__0);
                        this.<>f__this.BackgroundOverlay.fadeToBlack(ConfigUi.POPUP_TRANSITION_DURATION_IN, ConfigUi.MENU_BACKGROUND_ALPHA, Easing.Function.LINEAR);
                        this.<timer>__1 = new ManualTimer(ConfigUi.POPUP_TRANSITION_DURATION_IN);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0161;
                }
                if (!this.<timer>__1.Idle)
                {
                    this.<easedV>__2 = Easing.Apply(this.<timer>__1.normalizedProgress(), Easing.Function.LINEAR);
                    this.<>f__this.PanelRoot.alpha = this.<easedV>__2;
                    this.<timer>__1.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<>f__this.PanelRoot.alpha = 1f;
                goto Label_0161;
                this.$PC = -1;
            Label_0161:
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
            public int ActiveTabIndex;
            public List<int> HiddenTabIndices;
        }
    }
}

