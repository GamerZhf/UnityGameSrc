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

    public class PulsatingGraphic : MonoBehaviour
    {
        public float ANIMATION_DURATION = 1.5f;
        public Easing.Function EASING = Easing.Function.OUT_CUBIC;
        public bool LOOPING = true;
        private List<Duplicate> m_duplicates = new List<Duplicate>(3);
        private RectTransform m_targetRectTm;
        public const int MAX_NUM_SIMULTANEOUS_PULSES = 3;
        public float MAX_SCALE = 1.3f;
        public bool PLAY_ON_AWAKE = true;
        public Image TargetImage;
        public Text TargetText;
        public float WAIT_BETWEEN_CYCLES = 0.2f;

        [DebuggerHidden]
        private IEnumerator animationRoutine(Duplicate d)
        {
            <animationRoutine>c__Iterator1FB iteratorfb = new <animationRoutine>c__Iterator1FB();
            iteratorfb.d = d;
            iteratorfb.<$>d = d;
            iteratorfb.<>f__this = this;
            return iteratorfb;
        }

        protected void Awake()
        {
            if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Low)
            {
                base.enabled = false;
            }
            else
            {
                if (this.TargetImage != null)
                {
                    this.m_targetRectTm = this.TargetImage.rectTransform;
                    for (int i = 0; i < 3; i++)
                    {
                        Duplicate d = new Duplicate();
                        d.Go = new GameObject(this.TargetImage.name + "-duplicate");
                        d.Go.layer = Layers.UI;
                        d.Image = d.Go.AddComponent<Image>();
                        d.Image.enabled = false;
                        d.Tm = GameObjectExtensions.AddOrGetComponent<RectTransform>(d.Go);
                        d.Ta = d.Go.AddComponent<TransformAnimation>();
                        d.TaTask = new TransformAnimationTask(d.Tm, this.ANIMATION_DURATION, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        d.TaTask.scale((Vector3) (Vector3.one * this.MAX_SCALE), true, this.EASING);
                        d.Cg = d.Go.AddComponent<CanvasGroup>();
                        d.Cg.interactable = false;
                        d.Cg.blocksRaycasts = false;
                        d.Tm.SetParent(this.m_targetRectTm, false);
                        RectTransformExtensions.SetSize(d.Tm, this.m_targetRectTm.rect.size);
                        d.Tm.anchoredPosition = Vector2.zero;
                        d.Tm.SetParent(this.m_targetRectTm.parent, true);
                        d.Tm.SetSiblingIndex(this.m_targetRectTm.GetSiblingIndex());
                        d.Go.SetActive(false);
                        d.Enabled = false;
                        this.refreshContent(d);
                        this.m_duplicates.Add(d);
                    }
                }
                if (this.TargetText != null)
                {
                    this.m_targetRectTm = this.TargetText.rectTransform;
                    for (int j = 0; j < 3; j++)
                    {
                        Duplicate duplicate2 = new Duplicate();
                        duplicate2.Go = new GameObject(this.TargetText.name + "-duplicate");
                        duplicate2.Go.layer = Layers.UI;
                        duplicate2.Text = duplicate2.Go.AddComponent<Text>();
                        Shadow component = this.TargetText.GetComponent<Shadow>();
                        if (component != null)
                        {
                            Shadow shadow2 = duplicate2.Go.AddComponent<Shadow>();
                            shadow2.effectColor = component.effectColor;
                            shadow2.effectDistance = component.effectDistance;
                            shadow2.useGraphicAlpha = component.useGraphicAlpha;
                        }
                        duplicate2.Text.enabled = false;
                        duplicate2.Tm = GameObjectExtensions.AddOrGetComponent<RectTransform>(duplicate2.Go);
                        duplicate2.Ta = duplicate2.Go.AddComponent<TransformAnimation>();
                        duplicate2.TaTask = new TransformAnimationTask(duplicate2.Tm, this.ANIMATION_DURATION, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        duplicate2.TaTask.scale((Vector3) (Vector3.one * this.MAX_SCALE), true, this.EASING);
                        duplicate2.Cg = duplicate2.Go.AddComponent<CanvasGroup>();
                        duplicate2.Cg.interactable = false;
                        duplicate2.Cg.blocksRaycasts = false;
                        duplicate2.Tm.SetParent(this.m_targetRectTm, false);
                        RectTransformExtensions.SetSize(duplicate2.Tm, this.m_targetRectTm.rect.size);
                        duplicate2.Tm.anchoredPosition = Vector2.zero;
                        duplicate2.Tm.SetParent(this.m_targetRectTm.parent, true);
                        duplicate2.Tm.SetSiblingIndex(this.m_targetRectTm.GetSiblingIndex());
                        duplicate2.Go.SetActive(false);
                        duplicate2.Enabled = false;
                        this.refreshContent(duplicate2);
                        this.m_duplicates.Add(duplicate2);
                    }
                }
            }
        }

        private Duplicate getFirstFreeDuplicate()
        {
            for (int i = 0; i < this.m_duplicates.Count; i++)
            {
                Duplicate duplicate = this.m_duplicates[i];
                if (!duplicate.Enabled)
                {
                    return duplicate;
                }
            }
            return null;
        }

        protected void OnDisable()
        {
            if (base.gameObject != null)
            {
                for (int i = 0; i < this.m_duplicates.Count; i++)
                {
                    Duplicate duplicate = this.m_duplicates[i];
                    UnityUtils.StopCoroutine(this, ref duplicate.AnimationRoutine);
                    duplicate.Enabled = false;
                    if (duplicate.Go != null)
                    {
                        duplicate.Go.SetActive(false);
                    }
                }
            }
        }

        protected void OnEnable()
        {
            if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Low)
            {
                base.enabled = false;
            }
            else
            {
                for (int i = 0; i < this.m_duplicates.Count; i++)
                {
                    Duplicate d = this.m_duplicates[i];
                    d.Enabled = false;
                    d.AnimationRoutine = UnityUtils.StartCoroutine(this, this.animationRoutine(d));
                }
                if (this.PLAY_ON_AWAKE && (this.m_duplicates.Count > 0))
                {
                    this.m_duplicates[0].Go.SetActive(true);
                    this.m_duplicates[0].Enabled = true;
                }
            }
        }

        public void play()
        {
            if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Low)
            {
                base.enabled = false;
            }
            else
            {
                Duplicate duplicate = this.getFirstFreeDuplicate();
                if (duplicate != null)
                {
                    duplicate.Enabled = true;
                    duplicate.Go.SetActive(true);
                }
            }
        }

        private void refreshContent(Duplicate d)
        {
            if (this.TargetImage != null)
            {
                d.Image.sprite = this.TargetImage.sprite;
                d.Image.color = this.TargetImage.color;
                d.Image.material = this.TargetImage.material;
                d.Image.preserveAspect = this.TargetImage.preserveAspect;
            }
            if (this.TargetText != null)
            {
                d.Text.text = this.TargetText.text;
                d.Text.color = this.TargetText.color;
                d.Text.font = this.TargetText.font;
                d.Text.fontStyle = this.TargetText.fontStyle;
                d.Text.fontSize = this.TargetText.fontSize;
                d.Text.lineSpacing = this.TargetText.lineSpacing;
                d.Text.supportRichText = this.TargetText.supportRichText;
                d.Text.alignment = this.TargetText.alignment;
                d.Text.horizontalOverflow = this.TargetText.horizontalOverflow;
                d.Text.verticalOverflow = this.TargetText.verticalOverflow;
                d.Text.resizeTextForBestFit = this.TargetText.resizeTextForBestFit;
                d.Text.material = this.TargetText.material;
            }
            d.Tm.position = this.m_targetRectTm.position;
        }

        [CompilerGenerated]
        private sealed class <animationRoutine>c__Iterator1FB : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal PulsatingGraphic.Duplicate <$>d;
            internal PulsatingGraphic <>f__this;
            internal float <easedV>__1;
            internal IEnumerator <ie>__2;
            internal ManualTimer <timer>__0;
            internal PulsatingGraphic.Duplicate d;

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
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_0313;

                    case 1:
                        this.<timer>__0 = new ManualTimer(this.<>f__this.ANIMATION_DURATION);
                        break;

                    case 2:
                        break;

                    case 3:
                        goto Label_01F5;

                    case 4:
                        goto Label_0278;

                    case 5:
                        break;
                        this.$PC = -1;
                        goto Label_0311;

                    default:
                        goto Label_0311;
                }
                if (!this.d.Enabled)
                {
                    this.$current = new WaitForSeconds(UnityEngine.Random.Range((float) 0.05f, (float) 0.1f));
                    this.$PC = 2;
                    goto Label_0313;
                }
                if ((this.d.Image != null) && !this.d.Image.enabled)
                {
                    this.d.Image.enabled = true;
                }
                if ((this.d.Text != null) && !this.d.Text.enabled)
                {
                    this.d.Text.enabled = true;
                }
                this.<>f__this.refreshContent(this.d);
                this.d.Cg.alpha = 1f;
                this.d.Tm.localScale = Vector3.one;
                this.d.TaTask.reset();
                this.d.Ta.addTask(this.d.TaTask);
                this.<timer>__0.set(this.<>f__this.ANIMATION_DURATION);
            Label_01F5:
                while (this.d.Ta.HasTasks || !this.<timer>__0.Idle)
                {
                    this.<easedV>__1 = Easing.Apply(this.<timer>__0.normalizedProgress(), this.<>f__this.EASING);
                    this.d.Cg.alpha = Mathf.Clamp01(1f - this.<easedV>__1);
                    this.<timer>__0.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0313;
                }
                this.d.Cg.alpha = 0f;
                this.<timer>__0.set(this.<>f__this.WAIT_BETWEEN_CYCLES);
                this.<ie>__2 = this.<timer>__0.tickUntilEndUnscaled();
            Label_0278:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 4;
                    goto Label_0313;
                }
                if (!this.<>f__this.LOOPING)
                {
                    this.d.Enabled = false;
                    if (this.d.Image != null)
                    {
                        this.d.Image.enabled = false;
                    }
                    if (this.d.Text != null)
                    {
                        this.d.Text.enabled = false;
                    }
                }
                this.$current = null;
                this.$PC = 5;
                goto Label_0313;
            Label_0311:
                return false;
            Label_0313:
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

        private class Duplicate
        {
            public Coroutine AnimationRoutine;
            public CanvasGroup Cg;
            public bool Enabled;
            public GameObject Go;
            public UnityEngine.UI.Image Image;
            public TransformAnimation Ta;
            public TransformAnimationTask TaTask;
            public UnityEngine.UI.Text Text;
            public RectTransform Tm;
        }
    }
}

