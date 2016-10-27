namespace PlayerView
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class StackedMenuContentController : MonoBehaviour
    {
        [CompilerGenerated]
        private Menu <ContentMenu>k__BackingField;
        [CompilerGenerated]
        private RectTransform <Tm>k__BackingField;
        public GameObject BackButtonRoot;
        public RectTransform ContentAreaTm;
        private Transform m_activeContentAreaTm;
        private int m_activeTabIdx = -1;
        private Coroutine m_animatedContentPopRoutine;
        private Coroutine m_animatedContentPushRoutine;
        private Vector3 m_contentAreaDefaultLocalPos;
        private Transform m_contentAreaDuplicateTm;
        private Stack<MenuContent> m_contentStack = new Stack<MenuContent>();
        public const Easing.Function PANEL_SLIDE_ALPHA_EASING_ENTRY = Easing.Function.SMOOTHSTEP;
        public const Easing.Function PANEL_SLIDE_ALPHA_EASING_EXIT = Easing.Function.SMOOTHSTEP;
        public const float PANEL_SLIDE_DURATION_BOTTOM_ENTRY = 0.15f;
        public const float PANEL_SLIDE_DURATION_BOTTOM_EXIT = 0.15f;
        public const float PANEL_SLIDE_DURATION_TOP_ENTRY = 0.15f;
        public const float PANEL_SLIDE_DURATION_TOP_EXIT = 0.15f;
        public List<IconWithText> TabButtons;
        public GameObject TabsRoot;

        [DebuggerHidden]
        private IEnumerator animatedContentPopRoutine()
        {
            <animatedContentPopRoutine>c__Iterator1FE iteratorfe = new <animatedContentPopRoutine>c__Iterator1FE();
            iteratorfe.<>f__this = this;
            return iteratorfe;
        }

        [DebuggerHidden]
        private IEnumerator animatedContentPushRoutine(MenuContentType menuContentType, object param)
        {
            <animatedContentPushRoutine>c__Iterator1FD iteratorfd = new <animatedContentPushRoutine>c__Iterator1FD();
            iteratorfd.menuContentType = menuContentType;
            iteratorfd.param = param;
            iteratorfd.<$>menuContentType = menuContentType;
            iteratorfd.<$>param = param;
            iteratorfd.<>f__this = this;
            return iteratorfd;
        }

        protected void Awake()
        {
            this.Tm = base.GetComponent<RectTransform>();
            this.m_contentAreaDefaultLocalPos = this.ContentAreaTm.localPosition;
            CanvasGroup group = GameObjectExtensions.AddOrGetComponent<CanvasGroup>(this.ContentAreaTm.gameObject);
            group.interactable = true;
            group.blocksRaycasts = true;
            this.m_contentAreaDuplicateTm = UnityEngine.Object.Instantiate<GameObject>(this.ContentAreaTm.gameObject).transform;
            this.m_contentAreaDuplicateTm.name = this.ContentAreaTm.name + "-duplicate";
            this.m_contentAreaDuplicateTm.SetParent(this.ContentAreaTm.parent, false);
            this.m_contentAreaDuplicateTm.SetSiblingIndex(this.ContentAreaTm.transform.GetSiblingIndex() + 1);
            this.m_contentAreaDuplicateTm.gameObject.SetActive(false);
            this.m_activeContentAreaTm = this.ContentAreaTm;
        }

        public bool canPopContent()
        {
            return (this.m_contentStack.Count > 0);
        }

        public int getActiveTabIndex()
        {
            return this.m_activeTabIdx;
        }

        public MenuContent getTopmostContent()
        {
            if (this.m_contentStack.Count > 0)
            {
                return this.m_contentStack.Peek();
            }
            return null;
        }

        public MenuContentType getTopmostContentType()
        {
            if (this.m_contentStack.Count > 0)
            {
                return this.m_contentStack.Peek().ContentType;
            }
            return MenuContentType.NONE;
        }

        public void initialize(Menu contentMenu)
        {
            this.popAllContent();
            this.ContentMenu = contentMenu;
            this.setActiveTabIndex(0);
            this.BackButtonRoot.SetActive(false);
        }

        public void onBackButtonClicked()
        {
            if (!this.Animating)
            {
                this.popContent(false);
            }
        }

        public void onTabButtonClicked(int idx)
        {
            if (this.m_activeTabIdx != idx)
            {
                Binder.AudioSystem.playSfxGrp(AudioGroupType.SfxGrpUi_BookTurn, (float) 0f);
                this.setActiveTabIndex(idx);
            }
        }

        public void popAllContent()
        {
            for (int i = this.m_contentStack.Count - 1; i >= 0; i--)
            {
                this.popContent(true);
            }
            UnityUtils.StopCoroutine(this, ref this.m_animatedContentPushRoutine);
            UnityUtils.StopCoroutine(this, ref this.m_animatedContentPopRoutine);
        }

        public void popContent([Optional, DefaultParameterValue(false)] bool instant)
        {
            if (this.m_contentStack.Count != 0)
            {
                if (this.m_contentStack.Count > 1)
                {
                    Binder.InputSystem.PopBackNavigationListener();
                }
                if (instant)
                {
                    UnityUtils.StopCoroutine(this, ref this.m_animatedContentPopRoutine);
                    this.popContentInstantly();
                }
                else if (!this.Animating)
                {
                    this.m_animatedContentPopRoutine = UnityUtils.StartCoroutine(this, this.animatedContentPopRoutine());
                }
            }
        }

        private void popContentInstantly()
        {
            if (this.m_contentStack.Count > 0)
            {
                MenuContent content = this.m_contentStack.Pop();
                content.cleanup();
                content.gameObject.SetActive(false);
            }
            if (this.m_contentStack.Count > 0)
            {
                MenuContent content2 = this.m_contentStack.Peek();
                content2.gameObject.SetActive(true);
                content2.refresh();
            }
            this.refreshButtons();
        }

        public void pushContent(MenuContentType menuContentType, [Optional, DefaultParameterValue(null)] object param, [Optional, DefaultParameterValue(false)] bool instant)
        {
            if (instant)
            {
                UnityUtils.StopCoroutine(this, ref this.m_animatedContentPushRoutine);
                this.pushContentInstantly(menuContentType, param);
            }
            else
            {
                if (this.Animating)
                {
                    return;
                }
                this.m_animatedContentPushRoutine = UnityUtils.StartCoroutine(this, this.animatedContentPushRoutine(menuContentType, param));
            }
            if (this.m_contentStack.Count > 1)
            {
                Binder.InputSystem.PushBackNavigationListener(new System.Action(this.onBackButtonClicked));
            }
        }

        private void pushContentInstantly(MenuContentType menuContentType, object param)
        {
            if (this.m_contentStack.Count > 0)
            {
                this.m_contentStack.Peek().gameObject.SetActive(false);
            }
            this.m_activeContentAreaTm.gameObject.SetActive(true);
            this.m_activeContentAreaTm.GetComponent<CanvasGroup>().alpha = 1f;
            this.m_activeContentAreaTm.localPosition = this.m_contentAreaDefaultLocalPos;
            GameObject obj2 = Binder.MenuContentResources.getSharedResource(menuContentType);
            obj2.transform.SetParent(this.m_activeContentAreaTm, false);
            MenuContent component = obj2.GetComponent<MenuContent>();
            component.preShow(this.ContentMenu, param);
            this.m_contentStack.Push(component);
            obj2.SetActive(true);
            IEnumerator enumerator = component.show(param);
            while (enumerator.MoveNext())
            {
            }
            this.refreshButtons();
        }

        private void refreshButtons()
        {
            MenuContent content = this.getTopmostContent();
            bool flag = (content != null) && content.UsesTabs;
            this.TabsRoot.SetActive(flag);
            if (flag)
            {
                for (int i = 0; i < this.TabButtons.Count; i++)
                {
                    string str = content.getTitleForTab(i);
                    this.TabButtons[i].Text.text = str;
                    this.TabButtons[i].gameObject.SetActive(!string.IsNullOrEmpty(str));
                }
            }
        }

        public void setActiveTabIndex(int idx)
        {
            if (this.m_activeTabIdx != idx)
            {
                if (this.m_activeTabIdx != -1)
                {
                    this.TabButtons[this.m_activeTabIdx].Background.sprite = Binder.SpriteResources.getSprite("Menu", "uix_tab_inactive_0");
                }
                this.m_activeTabIdx = idx;
                this.TabButtons[this.m_activeTabIdx].Background.sprite = Binder.SpriteResources.getSprite("Menu", "uix_tab_active_0");
            }
            MenuContent content = this.getTopmostContent();
            if ((content != null) && content.UsesTabs)
            {
                content.onTabButtonClicked(idx);
            }
        }

        public bool Animating
        {
            get
            {
                return (UnityUtils.CoroutineRunning(ref this.m_animatedContentPopRoutine) || UnityUtils.CoroutineRunning(ref this.m_animatedContentPushRoutine));
            }
        }

        public Menu ContentMenu
        {
            [CompilerGenerated]
            get
            {
                return this.<ContentMenu>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ContentMenu>k__BackingField = value;
            }
        }

        public RectTransform Tm
        {
            [CompilerGenerated]
            get
            {
                return this.<Tm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Tm>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <animatedContentPopRoutine>c__Iterator1FE : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal StackedMenuContentController <>f__this;
            internal ManualTimer <alphaTimer1>__6;
            internal ManualTimer <alphaTimer2>__7;
            internal CanvasGroup <cg1>__1;
            internal CanvasGroup <cg2>__2;
            internal Transform <contentAreaBackTm>__0;
            internal GameObject <contentObj>__5;
            internal MenuContent <oldContent>__3;
            internal MenuContent <targetContent>__4;

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
                        this.<contentAreaBackTm>__0 = (this.<>f__this.m_activeContentAreaTm != this.<>f__this.ContentAreaTm) ? this.<>f__this.ContentAreaTm : this.<>f__this.m_contentAreaDuplicateTm;
                        this.<cg1>__1 = this.<>f__this.m_activeContentAreaTm.GetComponent<CanvasGroup>();
                        this.<cg2>__2 = this.<contentAreaBackTm>__0.GetComponent<CanvasGroup>();
                        this.<oldContent>__3 = this.<>f__this.m_contentStack.Pop();
                        this.<targetContent>__4 = this.<>f__this.m_contentStack.Peek();
                        this.<>f__this.refreshButtons();
                        this.<contentAreaBackTm>__0.gameObject.SetActive(true);
                        this.<contentObj>__5 = Binder.MenuContentResources.getSharedResource(this.<targetContent>__4.ContentType);
                        this.<contentObj>__5.transform.SetParent(this.<contentAreaBackTm>__0, false);
                        this.<contentObj>__5.SetActive(true);
                        this.<targetContent>__4.refresh();
                        this.<alphaTimer1>__6 = new ManualTimer(0.1125f);
                        this.<alphaTimer2>__7 = new ManualTimer(0.15f);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0259;
                }
                if (!this.<alphaTimer1>__6.Idle || !this.<alphaTimer2>__7.Idle)
                {
                    this.<cg1>__1.alpha = 1f - Easing.Apply(this.<alphaTimer1>__6.normalizedProgress(), Easing.Function.SMOOTHSTEP);
                    this.<cg2>__2.alpha = Easing.Apply(this.<alphaTimer2>__7.normalizedProgress(), Easing.Function.SMOOTHSTEP);
                    this.<alphaTimer1>__6.tick(Time.deltaTime / Time.timeScale);
                    this.<alphaTimer2>__7.tick(Time.deltaTime / Time.timeScale);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<cg1>__1.alpha = 0f;
                this.<cg2>__2.alpha = 1f;
                this.<>f__this.m_activeContentAreaTm.gameObject.SetActive(false);
                this.<>f__this.m_activeContentAreaTm = this.<contentAreaBackTm>__0;
                this.<oldContent>__3.cleanup();
                this.<oldContent>__3.gameObject.SetActive(false);
                this.<>f__this.m_animatedContentPopRoutine = null;
                goto Label_0259;
                this.$PC = -1;
            Label_0259:
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
        private sealed class <animatedContentPushRoutine>c__Iterator1FD : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuContentType <$>menuContentType;
            internal object <$>param;
            internal StackedMenuContentController <>f__this;
            internal ManualTimer <alphaTimer1>__7;
            internal ManualTimer <alphaTimer2>__8;
            internal CanvasGroup <cg1>__2;
            internal CanvasGroup <cg2>__3;
            internal MenuContent <content>__5;
            internal Transform <contentAreaBackTm>__1;
            internal GameObject <contentObj>__4;
            internal IEnumerator <ie>__6;
            internal MenuContent <topmostContentBeforePush>__0;
            internal MenuContentType menuContentType;
            internal object param;

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
                        this.<topmostContentBeforePush>__0 = this.<>f__this.getTopmostContent();
                        this.<contentAreaBackTm>__1 = (this.<>f__this.m_activeContentAreaTm != this.<>f__this.ContentAreaTm) ? this.<>f__this.ContentAreaTm : this.<>f__this.m_contentAreaDuplicateTm;
                        this.<cg1>__2 = this.<>f__this.m_activeContentAreaTm.GetComponent<CanvasGroup>();
                        this.<cg2>__3 = this.<contentAreaBackTm>__1.GetComponent<CanvasGroup>();
                        this.<contentAreaBackTm>__1.gameObject.SetActive(true);
                        this.<contentObj>__4 = Binder.MenuContentResources.getSharedResource(this.menuContentType);
                        this.<contentObj>__4.transform.SetParent(this.<contentAreaBackTm>__1, false);
                        this.<content>__5 = this.<contentObj>__4.GetComponent<MenuContent>();
                        this.<content>__5.preShow(this.<>f__this.ContentMenu, this.param);
                        this.<>f__this.m_contentStack.Push(this.<content>__5);
                        this.<contentObj>__4.SetActive(true);
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_02DD;

                    case 1:
                        this.<ie>__6 = this.<content>__5.show(this.param);
                        break;

                    case 2:
                        break;

                    case 3:
                        goto Label_023A;

                    default:
                        goto Label_02DB;
                }
                if (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 2;
                    goto Label_02DD;
                }
                this.<>f__this.refreshButtons();
                this.<alphaTimer1>__7 = new ManualTimer(0.1125f);
                this.<alphaTimer2>__8 = new ManualTimer(0.15f);
            Label_023A:
                while (!this.<alphaTimer1>__7.Idle || !this.<alphaTimer2>__8.Idle)
                {
                    this.<cg1>__2.alpha = 1f - Easing.Apply(this.<alphaTimer1>__7.normalizedProgress(), Easing.Function.SMOOTHSTEP);
                    this.<cg2>__3.alpha = Easing.Apply(this.<alphaTimer2>__8.normalizedProgress(), Easing.Function.SMOOTHSTEP);
                    this.<alphaTimer1>__7.tick(Time.deltaTime / Time.timeScale);
                    this.<alphaTimer2>__8.tick(Time.deltaTime / Time.timeScale);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_02DD;
                }
                this.<cg1>__2.alpha = 0f;
                this.<cg2>__3.alpha = 1f;
                if (this.<topmostContentBeforePush>__0 != null)
                {
                    this.<topmostContentBeforePush>__0.gameObject.SetActive(false);
                }
                this.<>f__this.m_activeContentAreaTm.gameObject.SetActive(false);
                this.<>f__this.m_activeContentAreaTm = this.<contentAreaBackTm>__1;
                this.<>f__this.m_animatedContentPushRoutine = null;
                goto Label_02DB;
                this.$PC = -1;
            Label_02DB:
                return false;
            Label_02DD:
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

