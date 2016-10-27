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

    public abstract class Menu : MonoBehaviour
    {
        [CompilerGenerated]
        private UnityEngine.Canvas <Canvas>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.UI.GraphicRaycaster <GraphicRaycaster>k__BackingField;
        [CompilerGenerated]
        private bool <PreWarmed>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;

        protected Menu()
        {
        }

        public virtual MenuContent activeContentObject()
        {
            return null;
        }

        public virtual MenuContentType activeContentType()
        {
            return MenuContentType.NONE;
        }

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            this.Canvas = base.GetComponent<UnityEngine.Canvas>();
            this.GraphicRaycaster = base.GetComponent<UnityEngine.UI.GraphicRaycaster>();
            this.GraphicRaycaster.enabled = false;
            this.onAwake();
        }

        [DebuggerHidden]
        public virtual IEnumerator hideRoutine(bool instant)
        {
            return new <hideRoutine>c__Iterator10B();
        }

        public void initialize(Camera canvasCamera)
        {
            this.Canvas.worldCamera = canvasCamera;
            this.onInitialize();
        }

        protected virtual void onAwake()
        {
        }

        protected virtual void onInitialize()
        {
        }

        protected virtual void onPreWarm(List<MenuContentType> menuContentTypes, List<object> contentParams)
        {
        }

        protected virtual void onRefresh()
        {
        }

        protected virtual void onUpdate(float dt)
        {
        }

        [DebuggerHidden]
        public virtual IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            return new <preShowRoutine>c__Iterator109();
        }

        public virtual void preWarm(List<MenuContentType> menuContentTypes, List<object> contentParams)
        {
            this.PreWarmed = true;
            this.onPreWarm(menuContentTypes, contentParams);
        }

        public void refresh()
        {
            this.onRefresh();
        }

        public virtual void refreshTabs()
        {
        }

        public virtual void refreshTitle(string title, [Optional, DefaultParameterValue("")] string additionalText1, [Optional, DefaultParameterValue("")] string additionalText2)
        {
        }

        public virtual void setCloseButtonVisibility(bool visible)
        {
        }

        [DebuggerHidden]
        public virtual IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            return new <showRoutine>c__Iterator10A();
        }

        protected void Update()
        {
            if (this.Canvas.enabled)
            {
                this.onUpdate(Time.deltaTime);
            }
        }

        public UnityEngine.Canvas Canvas
        {
            [CompilerGenerated]
            get
            {
                return this.<Canvas>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Canvas>k__BackingField = value;
            }
        }

        public UnityEngine.UI.GraphicRaycaster GraphicRaycaster
        {
            [CompilerGenerated]
            get
            {
                return this.<GraphicRaycaster>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<GraphicRaycaster>k__BackingField = value;
            }
        }

        public virtual bool IsOverlayMenu
        {
            get
            {
                return false;
            }
        }

        public abstract PlayerView.MenuType MenuType { get; }

        public bool PreWarmed
        {
            [CompilerGenerated]
            get
            {
                return this.<PreWarmed>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PreWarmed>k__BackingField = value;
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

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator10B : IEnumerator, IDisposable, IEnumerator<object>
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
        private sealed class <preShowRoutine>c__Iterator109 : IEnumerator, IDisposable, IEnumerator<object>
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
        private sealed class <showRoutine>c__Iterator10A : IEnumerator, IDisposable, IEnumerator<object>
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
    }
}

