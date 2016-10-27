namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [RequireComponent(typeof(RectTransform))]
    public class OffscreenOpenClose : MonoBehaviour
    {
        [CompilerGenerated]
        private bool <IsOpen>k__BackingField;
        [CompilerGenerated]
        private Vector2 <OriginalAnchoredPos>k__BackingField;
        [CompilerGenerated]
        private float <OriginalPixelWidth>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public UnityEngine.RectTransform.Edge Edge;
        private TransformAnimationTask m_closeAnimationTask;
        private Vector3 m_extremePos;
        private TransformAnimationTask m_openAnimationTask;
        private Vector3 m_originalPos;
        private TransformAnimation m_tmAnimation;

        private void animate(bool open, float duration, Easing.Function easingFunction, float delay)
        {
            bool flag = duration <= 0f;
            if (open)
            {
                if (flag)
                {
                    this.RectTm.localPosition = this.m_originalPos;
                    this.IsOpen = true;
                }
                else
                {
                    this.m_openAnimationTask.reset(duration, delay, easingFunction);
                    this.m_tmAnimation.stopAll();
                    this.m_tmAnimation.addTask(this.m_openAnimationTask);
                }
            }
            else if (flag)
            {
                this.RectTm.localPosition = this.m_extremePos;
                this.IsOpen = false;
            }
            else
            {
                this.m_closeAnimationTask.reset(duration, delay, easingFunction);
                this.m_tmAnimation.stopAll();
                this.m_tmAnimation.addTask(this.m_closeAnimationTask);
            }
        }

        protected void Awake()
        {
            this.RectTm = base.gameObject.GetComponent<RectTransform>();
            this.m_tmAnimation = base.gameObject.GetComponent<TransformAnimation>();
            if (this.m_tmAnimation == null)
            {
                this.m_tmAnimation = base.gameObject.AddComponent<TransformAnimation>();
            }
        }

        public void close(float duration, [Optional, DefaultParameterValue(0)] Easing.Function easingFunction, [Optional, DefaultParameterValue(0f)] float delay)
        {
            if (this.m_closeAnimationTask == null)
            {
                this.refreshInitialPosition();
            }
            this.animate(false, duration, easingFunction, delay);
        }

        [ContextMenu("closeTest")]
        private void closeTest()
        {
        }

        private void onCloseAnimationEnded(TransformAnimation ta)
        {
            this.IsOpen = false;
        }

        private void onOpenAnimationEnded(TransformAnimation ta)
        {
            this.IsOpen = true;
        }

        public void open(float duration, [Optional, DefaultParameterValue(0)] Easing.Function easingFunction, [Optional, DefaultParameterValue(0f)] float delay)
        {
            if (this.m_openAnimationTask == null)
            {
                this.refreshInitialPosition();
            }
            this.animate(true, duration, easingFunction, delay);
        }

        [ContextMenu("openTest")]
        private void openTest()
        {
        }

        public void refreshInitialPosition()
        {
            if (this.RectTm == null)
            {
                this.RectTm = base.gameObject.GetComponent<RectTransform>();
            }
            this.m_originalPos = this.RectTm.localPosition;
            Vector2 zero = Vector2.zero;
            switch (this.Edge)
            {
                case UnityEngine.RectTransform.Edge.Left:
                    zero = new Vector2(-this.RectTm.sizeDelta.x * 0.5f, this.RectTm.anchoredPosition.y);
                    break;

                case UnityEngine.RectTransform.Edge.Right:
                    zero = new Vector2(this.RectTm.sizeDelta.x * 0.5f, this.RectTm.anchoredPosition.y);
                    break;

                case UnityEngine.RectTransform.Edge.Top:
                    zero = new Vector2(this.RectTm.anchoredPosition.x, this.RectTm.sizeDelta.y * 0.5f);
                    break;

                case UnityEngine.RectTransform.Edge.Bottom:
                    zero = new Vector2(this.RectTm.anchoredPosition.x, -this.RectTm.sizeDelta.y * 0.5f);
                    break;
            }
            this.RectTm.anchoredPosition = zero;
            this.m_extremePos = this.RectTm.localPosition;
            this.m_openAnimationTask = new TransformAnimationTask(this.RectTm, 0f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
            this.m_openAnimationTask.EndCallback = new Action<TransformAnimation>(this.onOpenAnimationEnded);
            this.m_openAnimationTask.translate(this.m_originalPos, true, Easing.Function.LINEAR);
            this.m_closeAnimationTask = new TransformAnimationTask(this.RectTm, 0f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
            this.m_closeAnimationTask.EndCallback = new Action<TransformAnimation>(this.onCloseAnimationEnded);
            this.m_closeAnimationTask.translate(this.m_extremePos, true, Easing.Function.LINEAR);
            this.OriginalPixelWidth = RectTransformExtensions.GetWidth(this.RectTm);
            this.OriginalAnchoredPos = this.RectTm.anchoredPosition;
            this.IsOpen = true;
        }

        public bool Animating
        {
            get
            {
                return this.m_tmAnimation.HasTasks;
            }
        }

        public bool IsOpen
        {
            [CompilerGenerated]
            get
            {
                return this.<IsOpen>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<IsOpen>k__BackingField = value;
            }
        }

        public Vector2 OriginalAnchoredPos
        {
            [CompilerGenerated]
            get
            {
                return this.<OriginalAnchoredPos>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<OriginalAnchoredPos>k__BackingField = value;
            }
        }

        public float OriginalPixelWidth
        {
            [CompilerGenerated]
            get
            {
                return this.<OriginalPixelWidth>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<OriginalPixelWidth>k__BackingField = value;
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
    }
}

