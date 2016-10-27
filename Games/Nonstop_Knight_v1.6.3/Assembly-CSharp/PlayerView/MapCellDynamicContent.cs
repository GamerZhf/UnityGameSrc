namespace PlayerView
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class MapCellDynamicContent : MonoBehaviour
    {
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public Canvas BackgroundCanvas;
        public GameObject LockedContentRoot;
        private Coroutine m_colorTintRoutine;
        private Dictionary<Image, Color> m_dimmableImageOriginalColors = new Dictionary<Image, Color>();
        private List<Image> m_dimmableImages = new List<Image>();
        private RectTransform m_dummyRectTmObj;
        private Dictionary<TransformAnimation, MenuOverlay> m_lockedContentImages = new Dictionary<TransformAnimation, MenuOverlay>();
        private Dictionary<TransformAnimation, Vector2> m_lockedContentOriginalAnchoredPositions = new Dictionary<TransformAnimation, Vector2>();
        private Dictionary<TransformAnimation, RectTransform> m_lockedContentRectTransforms = new Dictionary<TransformAnimation, RectTransform>();
        private List<TransformAnimation> m_lockedContentTransformAnimations = new List<TransformAnimation>();
        public GameObject OpenContentRoot;
        public RectTransform UiAnchor;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            Transform[] children = TransformExtensions.GetChildren(this.LockedContentRoot.transform, false);
            for (int i = 0; i < children.Length; i++)
            {
                TransformAnimation item = GameObjectExtensions.AddOrGetComponent<TransformAnimation>(children[i].gameObject);
                this.m_lockedContentTransformAnimations.Add(item);
                RectTransform transform = children[i].GetComponent<RectTransform>();
                this.m_lockedContentRectTransforms.Add(item, transform);
                this.m_lockedContentOriginalAnchoredPositions.Add(item, transform.anchoredPosition);
                Image image = children[i].GetComponent<Image>();
                MenuOverlay overlay = GameObjectExtensions.AddOrGetComponent<MenuOverlay>(children[i].gameObject);
                overlay.Image = image;
                this.m_lockedContentImages.Add(item, overlay);
            }
            this.m_dummyRectTmObj = new GameObject("DummyRectTm").AddComponent<RectTransform>();
            this.m_dummyRectTmObj.sizeDelta = Vector2.zero;
            this.m_dummyRectTmObj.SetParent(this.LockedContentRoot.transform, false);
            this.m_dimmableImages.Clear();
            Image component = this.BackgroundCanvas.GetComponent<Image>();
            this.m_dimmableImages.Add(component);
            this.m_dimmableImageOriginalColors.Add(component, component.color);
            Image[] componentsInChildren = this.OpenContentRoot.GetComponentsInChildren<Image>();
            for (int j = 0; j < componentsInChildren.Length; j++)
            {
                this.m_dimmableImages.Add(componentsInChildren[j]);
                this.m_dimmableImageOriginalColors.Add(componentsInChildren[j], componentsInChildren[j].color);
            }
        }

        [DebuggerHidden]
        private IEnumerator colorTintRoutine(bool enabled, float duration)
        {
            <colorTintRoutine>c__Iterator131 iterator = new <colorTintRoutine>c__Iterator131();
            iterator.duration = duration;
            iterator.enabled = enabled;
            iterator.<$>duration = duration;
            iterator.<$>enabled = enabled;
            iterator.<>f__this = this;
            return iterator;
        }

        public void openLockedContent(float animationDuration)
        {
            for (int i = 0; i < this.m_lockedContentTransformAnimations.Count; i++)
            {
                TransformAnimation animation = this.m_lockedContentTransformAnimations[i];
                RectTransform tm = this.m_lockedContentRectTransforms[animation];
                Vector2 anchoredPosition = tm.anchoredPosition;
                if (tm.anchoredPosition.x <= 0f)
                {
                    anchoredPosition.x = (-this.RectTm.rect.width * 0.5f) - (tm.rect.width * 0.5f);
                }
                else
                {
                    anchoredPosition.x = (this.RectTm.rect.width * 0.5f) + (tm.rect.width * 0.5f);
                }
                this.m_dummyRectTmObj.sizeDelta = tm.sizeDelta;
                this.m_dummyRectTmObj.anchoredPosition = anchoredPosition;
                Vector3 localPosition = this.m_dummyRectTmObj.localPosition;
                float num2 = UnityEngine.Random.Range((float) 0f, (float) 0.3f);
                TransformAnimationTask animationTask = new TransformAnimationTask(tm, animationDuration + num2, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                animationTask.translate(localPosition, true, ConfigUi.MAP_EXPLORE_CLOUD_EASING);
                animation.addTask(animationTask);
                this.m_lockedContentImages[animation].fadeToTransparent((animationDuration * 0.8f) + num2, ConfigUi.MAP_EXPLORE_CLOUD_EASING);
            }
        }

        public void reset()
        {
            for (int i = 0; i < this.m_lockedContentTransformAnimations.Count; i++)
            {
                TransformAnimation animation = this.m_lockedContentTransformAnimations[i];
                this.m_lockedContentRectTransforms[animation].anchoredPosition = this.m_lockedContentOriginalAnchoredPositions[animation];
                this.m_lockedContentImages[animation].setTransparent(false);
            }
        }

        public void setColorTint(bool enabled, float duration)
        {
            if (duration > 0f)
            {
                UnityUtils.StopCoroutine(this, ref this.m_colorTintRoutine);
                this.m_colorTintRoutine = UnityUtils.StartCoroutine(this, this.colorTintRoutine(enabled, duration));
            }
            else
            {
                for (int i = 0; i < this.m_dimmableImages.Count; i++)
                {
                    Color color;
                    Image image = this.m_dimmableImages[i];
                    if (enabled)
                    {
                        color = ConfigUi.MAP_DIM_COLOR;
                    }
                    else
                    {
                        color = this.m_dimmableImageOriginalColors[image];
                    }
                    image.color = color;
                }
            }
        }

        public bool IsAnimating
        {
            get
            {
                for (int i = 0; i < this.m_lockedContentTransformAnimations.Count; i++)
                {
                    TransformAnimation animation = this.m_lockedContentTransformAnimations[i];
                    if ((animation.HasTasks || this.m_lockedContentImages[animation].IsAnimating) || UnityUtils.CoroutineRunning(ref this.m_colorTintRoutine))
                    {
                        return true;
                    }
                }
                return false;
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
        private sealed class <colorTintRoutine>c__Iterator131 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal float <$>duration;
            internal bool <$>enabled;
            internal MapCellDynamicContent <>f__this;
            internal int <i>__1;
            internal int <i>__4;
            internal Image <img>__2;
            internal Image <img>__5;
            internal Color <targetColor>__3;
            internal Color <targetColor>__6;
            internal ManualTimer <timer>__0;
            internal float duration;
            internal bool enabled;

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
                        this.<timer>__0 = new ManualTimer(this.duration);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_01D0;
                }
                if (!this.<timer>__0.Idle)
                {
                    this.<i>__1 = 0;
                    while (this.<i>__1 < this.<>f__this.m_dimmableImages.Count)
                    {
                        this.<img>__2 = this.<>f__this.m_dimmableImages[this.<i>__1];
                        if (this.enabled)
                        {
                            this.<targetColor>__3 = ConfigUi.MAP_DIM_COLOR;
                        }
                        else
                        {
                            this.<targetColor>__3 = this.<>f__this.m_dimmableImageOriginalColors[this.<img>__2];
                        }
                        this.<img>__2.color = Color.Lerp(this.<img>__2.color, this.<targetColor>__3, this.<timer>__0.normalizedProgress());
                        this.<i>__1++;
                    }
                    this.<timer>__0.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<i>__4 = 0;
                while (this.<i>__4 < this.<>f__this.m_dimmableImages.Count)
                {
                    this.<img>__5 = this.<>f__this.m_dimmableImages[this.<i>__4];
                    if (this.enabled)
                    {
                        this.<targetColor>__6 = ConfigUi.MAP_DIM_COLOR;
                    }
                    else
                    {
                        this.<targetColor>__6 = this.<>f__this.m_dimmableImageOriginalColors[this.<img>__5];
                    }
                    this.<img>__5.color = this.<targetColor>__6;
                    this.<i>__4++;
                }
                this.<>f__this.m_colorTintRoutine = null;
                goto Label_01D0;
                this.$PC = -1;
            Label_01D0:
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

