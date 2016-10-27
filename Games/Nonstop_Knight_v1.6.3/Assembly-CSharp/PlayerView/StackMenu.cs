namespace PlayerView
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class StackMenu : Menu
    {
        public const float ANIMATION_SCALE = 0.8f;
        public RectTransform ContentAreaTm;
        private List<TransformAnimation> m_childTransformAnimations = new List<TransformAnimation>();
        private Stack<MenuContent> m_contentStack = new Stack<MenuContent>();
        public MenuOverlay Overlay;
        public CanvasGroup PanelRoot;
        public Button TitleBackButton;
        public Text TitleText;

        public override MenuContent activeContentObject()
        {
            if (this.m_contentStack.Count > 0)
            {
                return this.m_contentStack.Peek();
            }
            return null;
        }

        public override MenuContentType activeContentType()
        {
            if (this.m_contentStack.Count > 0)
            {
                return this.m_contentStack.Peek().ContentType;
            }
            return MenuContentType.NONE;
        }

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator193 iterator = new <hideRoutine>c__Iterator193();
            iterator.<>f__this = this;
            return iterator;
        }

        protected override void onAwake()
        {
            if ((Binder.MenuHudTop != null) && (Binder.MenuHudBottom != null))
            {
                RectTransform component = this.PanelRoot.GetComponent<RectTransform>();
                float num = ((float) Screen.width) / ((float) Screen.height);
                float num2 = 0.5097f + (0.8716f * num);
                component.offsetMin = new Vector2(0f, Binder.MenuHudBottom.BottomPanelTm.sizeDelta.y * num2);
                component.offsetMax = new Vector2(0f, -Binder.MenuHudTop.TopPanelTm.sizeDelta.y * num2);
            }
            for (int i = 0; i < base.RectTm.childCount; i++)
            {
                Transform child = base.RectTm.GetChild(i);
                TransformAnimation item = child.GetComponent<TransformAnimation>();
                if (item == null)
                {
                    item = child.gameObject.AddComponent<TransformAnimation>();
                }
                this.m_childTransformAnimations.Add(item);
            }
        }

        public void onBackButtonClicked()
        {
            this.popContent();
            if (this.m_contentStack.Count > 0)
            {
                MenuContent content = this.m_contentStack.Peek();
                content.gameObject.SetActive(true);
                content.refresh();
            }
        }

        protected override void onRefresh()
        {
            if (this.m_contentStack.Count > 0)
            {
                this.m_contentStack.Peek().refresh();
            }
        }

        public void popAllContent()
        {
            for (int i = this.m_contentStack.Count - 1; i >= 0; i--)
            {
                this.popContent();
            }
        }

        public void popContent()
        {
            if (this.m_contentStack.Count > 0)
            {
                MenuContent content = this.m_contentStack.Pop();
                content.cleanup();
                content.transform.SetParent(base.transform, false);
                content.gameObject.SetActive(false);
            }
            this.refreshButtons();
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            return new <preShowRoutine>c__Iterator191();
        }

        public void pushContent(MenuContentType menuContentType, [Optional, DefaultParameterValue(null)] object param, [Optional, DefaultParameterValue(true)] bool instant)
        {
            if (this.m_contentStack.Count > 0)
            {
                this.m_contentStack.Peek().gameObject.SetActive(false);
            }
            GameObject obj2 = Binder.MenuContentResources.getSharedResource(menuContentType);
            obj2.transform.SetParent(this.ContentAreaTm, false);
            obj2.SetActive(true);
            MenuContent component = obj2.GetComponent<MenuContent>();
            component.preShow(this, param);
            this.m_contentStack.Push(component);
            this.refreshButtons();
        }

        private void refreshButtons()
        {
            this.TitleBackButton.gameObject.SetActive(this.m_contentStack.Count > 1);
        }

        public override void refreshTitle(string title, string additionalText1, string additionalText2)
        {
            this.TitleText.text = title;
        }

        public override void setCloseButtonVisibility(bool visible)
        {
            this.TitleBackButton.enabled = visible;
        }

        public void setContentType(MenuContentType contentType, [Optional, DefaultParameterValue(null)] object param)
        {
            this.popAllContent();
            this.pushContent(contentType, param, true);
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator192 iterator = new <showRoutine>c__Iterator192();
            iterator.targetMenuContentType = targetMenuContentType;
            iterator.parameter = parameter;
            iterator.<$>targetMenuContentType = targetMenuContentType;
            iterator.<$>parameter = parameter;
            iterator.<>f__this = this;
            return iterator;
        }

        public override PlayerView.MenuType MenuType
        {
            get
            {
                return PlayerView.MenuType.StackMenu;
            }
        }

        public MenuContentType VisibleContentType
        {
            get
            {
                if (this.m_contentStack.Count > 0)
                {
                    return this.m_contentStack.Peek().ContentType;
                }
                return MenuContentType.NONE;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator193 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal StackMenu <>f__this;

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
                    this.<>f__this.Overlay.setTransparent(false);
                    this.<>f__this.PanelRoot.gameObject.SetActive(false);
                    this.<>f__this.popAllContent();
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
        private sealed class <preShowRoutine>c__Iterator191 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;

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
        private sealed class <showRoutine>c__Iterator192 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal MenuContentType <$>targetMenuContentType;
            internal StackMenu <>f__this;
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
                    this.<>f__this.PanelRoot.gameObject.SetActive(true);
                    this.<>f__this.pushContent(this.targetMenuContentType, this.parameter, true);
                    this.<>f__this.Overlay.setTransparent(true);
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

