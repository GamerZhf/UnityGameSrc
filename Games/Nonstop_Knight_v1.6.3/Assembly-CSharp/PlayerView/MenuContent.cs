namespace PlayerView
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public abstract class MenuContent : MonoBehaviour
    {
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        protected Menu m_contentMenu;
        private int m_debugPreShowCleanupCounter;

        protected MenuContent()
        {
        }

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            this.onAwake();
        }

        public void cleanup()
        {
            this.m_debugPreShowCleanupCounter--;
            this.onCleanup();
        }

        public virtual string getTitleForTab(int idx)
        {
            return "CHANGE ME";
        }

        [DebuggerHidden]
        public IEnumerator hide()
        {
            <hide>c__Iterator103 iterator = new <hide>c__Iterator103();
            iterator.<>f__this = this;
            return iterator;
        }

        protected abstract void onAwake();
        public virtual bool onBackgroundOverlayClicked()
        {
            return false;
        }

        protected abstract void onCleanup();
        public virtual bool onCloseButtonClicked()
        {
            return false;
        }

        [DebuggerHidden]
        protected virtual IEnumerator onHide()
        {
            return new <onHide>c__Iterator105();
        }

        public virtual bool onMainButtonClicked()
        {
            return false;
        }

        protected abstract void onPreShow([Optional, DefaultParameterValue(null)] object param);
        protected virtual void onPreWarm([Optional, DefaultParameterValue(null)] object param)
        {
        }

        protected abstract void onRefresh();
        [DebuggerHidden]
        protected virtual IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            return new <onShow>c__Iterator104();
        }

        public virtual void onTabButtonClicked(int idx)
        {
        }

        public void preShow(Menu contentMenu, [Optional, DefaultParameterValue(null)] object param)
        {
            this.m_debugPreShowCleanupCounter++;
            this.m_contentMenu = contentMenu;
            this.onPreShow(param);
            Binder.EventBus.MenuContentChanged(this.m_contentMenu.gameObject);
        }

        public void preWarm(Menu contentMenu, [Optional, DefaultParameterValue(null)] object contentParam)
        {
            this.m_contentMenu = contentMenu;
            this.onPreWarm(contentParam);
        }

        public void refresh()
        {
            this.onRefresh();
        }

        [DebuggerHidden]
        public IEnumerator show([Optional, DefaultParameterValue(null)] object param)
        {
            <show>c__Iterator102 iterator = new <show>c__Iterator102();
            iterator.param = param;
            iterator.<$>param = param;
            iterator.<>f__this = this;
            return iterator;
        }

        public virtual bool CapturesInput
        {
            get
            {
                return true;
            }
        }

        public abstract MenuContentType ContentType { get; }

        public virtual bool ForceShowGameplayLayerBehind
        {
            get
            {
                return false;
            }
        }

        public virtual bool PauseGame
        {
            get
            {
                return true;
            }
        }

        public RectTransform RectTm
        {
            [CompilerGenerated]
            get
            {
                return this.<RectTm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RectTm>k__BackingField = value;
            }
        }

        public virtual TabSpriteParameters TabSprite
        {
            get
            {
                return new TabSpriteParameters();
            }
        }

        public virtual string TabTitle
        {
            get
            {
                return "CHANGE ME";
            }
        }

        public virtual bool UnlockNotificationActive
        {
            get
            {
                return false;
            }
        }

        public virtual bool UpgradeNotificationActive
        {
            get
            {
                return false;
            }
        }

        public virtual bool UsesTabs
        {
            get
            {
                return false;
            }
        }

        [CompilerGenerated]
        private sealed class <hide>c__Iterator103 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuContent <>f__this;
            internal IEnumerator <ie>__0;

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
                        this.<ie>__0 = this.<>f__this.onHide();
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0070;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    return true;
                }
                goto Label_0070;
                this.$PC = -1;
            Label_0070:
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
        private sealed class <onHide>c__Iterator105 : IEnumerator, IDisposable, IEnumerator<object>
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
        private sealed class <onShow>c__Iterator104 : IEnumerator, IDisposable, IEnumerator<object>
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
        private sealed class <show>c__Iterator102 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>param;
            internal MenuContent <>f__this;
            internal IEnumerator <ie>__0;
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
                        this.<ie>__0 = this.<>f__this.onShow(this.param);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0076;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    return true;
                }
                goto Label_0076;
                this.$PC = -1;
            Label_0076:
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
        public struct TabSpriteParameters
        {
            public string SpriteAtlasId;
            public string SpriteId;
            public Vector2? SpriteSize;
        }
    }
}

