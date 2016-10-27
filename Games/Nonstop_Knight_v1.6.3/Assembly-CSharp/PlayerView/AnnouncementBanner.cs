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

    public class AnnouncementBanner : MonoBehaviour
    {
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public Image Bg;
        public CanvasGroupAlphaFading BgGroup;
        public TransformAnimation BgTa;
        private Coroutine m_activeRoutine;
        public CanvasGroupAlphaFading RootGroup;
        public TransformAnimation RootTa;
        public Text Text1;
        public Text Text2;
        public CanvasGroupAlphaFading TextGroup1;
        public CanvasGroupAlphaFading TextGroup2;
        public TransformAnimation TextTa1;
        public TransformAnimation TextTa2;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
        }

        protected void OnEnable()
        {
            this.RectTm.localScale = Vector3.zero;
        }

        [DebuggerHidden]
        private IEnumerator openRoutine(string text1, string text2, bool slam, float delay, float durationMultiplier, Color bgColor)
        {
            <openRoutine>c__IteratorF6 rf = new <openRoutine>c__IteratorF6();
            rf.text1 = text1;
            rf.text2 = text2;
            rf.bgColor = bgColor;
            rf.delay = delay;
            rf.durationMultiplier = durationMultiplier;
            rf.slam = slam;
            rf.<$>text1 = text1;
            rf.<$>text2 = text2;
            rf.<$>bgColor = bgColor;
            rf.<$>delay = delay;
            rf.<$>durationMultiplier = durationMultiplier;
            rf.<$>slam = slam;
            rf.<>f__this = this;
            return rf;
        }

        public void show(string text1, string text2, bool slam, [Optional, DefaultParameterValue(0f)] float delay, [Optional, DefaultParameterValue(1f)] float durationMultiplier, [Optional, DefaultParameterValue(null)] Color? bgColor)
        {
            UnityUtils.StopCoroutine(this, ref this.m_activeRoutine);
            Color color = bgColor.HasValue ? bgColor.Value : Color.white;
            this.m_activeRoutine = UnityUtils.StartCoroutine(this, this.openRoutine(text1, text2, slam, delay, durationMultiplier, color));
        }

        [ContextMenu("openDefault()")]
        private void testOpenDefault()
        {
            this.show("FLOOR 128", "MAGICAL MEADOWS", false, 0f, 1f, null);
        }

        [ContextMenu("openSlam()")]
        private void testOpenSlam()
        {
            this.show("BOSS", "GORAN THE BRAVE", true, 0f, 1f, null);
        }

        public bool IsVisible
        {
            get
            {
                return UnityUtils.CoroutineRunning(ref this.m_activeRoutine);
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
        private sealed class <openRoutine>c__IteratorF6 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Color <$>bgColor;
            internal float <$>delay;
            internal float <$>durationMultiplier;
            internal bool <$>slam;
            internal string <$>text1;
            internal string <$>text2;
            internal AnnouncementBanner <>f__this;
            internal float <DURATION>__1;
            internal float <DURATION>__10;
            internal float <DURATION>__13;
            internal float <DURATION>__15;
            internal float <DURATION>__19;
            internal float <DURATION>__20;
            internal float <DURATION>__21;
            internal float <DURATION>__22;
            internal float <DURATION>__23;
            internal float <DURATION>__4;
            internal float <DURATION>__8;
            internal IEnumerator <ie>__0;
            internal IEnumerator <ie>__14;
            internal IEnumerator <ie>__18;
            internal IEnumerator <ie>__5;
            internal IEnumerator <ie>__9;
            internal float <outroTime>__7;
            internal float <STAY_DURATION>__6;
            internal TransformAnimation <ta>__11;
            internal TransformAnimation <ta>__16;
            internal TransformAnimation <ta>__2;
            internal TransformAnimation <ta>__24;
            internal TransformAnimationTask <tt>__12;
            internal TransformAnimationTask <tt>__17;
            internal TransformAnimationTask <tt>__25;
            internal TransformAnimationTask <tt>__3;
            internal Color bgColor;
            internal float delay;
            internal float durationMultiplier;
            internal bool slam;
            internal string text1;
            internal string text2;

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
                        this.<>f__this.RectTm.localScale = Vector3.one;
                        this.<>f__this.BgTa.Tm.localScale = new Vector3(1f, 0f, 1f);
                        this.<>f__this.TextGroup1.setTransparent(true);
                        this.<>f__this.TextGroup2.setTransparent(true);
                        this.<>f__this.Text1.text = !string.IsNullOrEmpty(this.text1) ? StringExtensions.ToUpperLoca(this.text1) : null;
                        this.<>f__this.Text2.text = !string.IsNullOrEmpty(this.text2) ? StringExtensions.ToUpperLoca(this.text2) : null;
                        this.<>f__this.Text2.rectTransform.anchoredPosition = !string.IsNullOrEmpty(this.text1) ? new Vector2(0f, -35f) : Vector2.zero;
                        this.<>f__this.Bg.color = this.bgColor;
                        this.<>f__this.Bg.material = !(this.bgColor == Color.white) ? Binder.MonochromeUiMaterial : null;
                        this.<>f__this.Bg.enabled = true;
                        this.<>f__this.Text1.enabled = true;
                        this.<>f__this.Text2.enabled = true;
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_0978;

                    case 1:
                        if (this.delay <= 0f)
                        {
                            goto Label_022C;
                        }
                        this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(this.delay);
                        break;

                    case 2:
                        break;

                    case 3:
                        goto Label_02DB;

                    case 4:
                        goto Label_035F;

                    case 5:
                        this.<tt>__12 = new TransformAnimationTask(this.<ta>__11.transform, this.<DURATION>__10, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__12.scale(Vector3.one, true, Easing.Function.IN_QUART);
                        this.<ta>__11.addTask(this.<tt>__12);
                        if ((Binder.RoomView != null) && (Binder.RoomView.RoomCamera != null))
                        {
                            Binder.RoomView.RoomCamera.shakeCamera(0.1f, 0.01f);
                            Binder.RoomView.RoomCamera.shakeCamera(0.05f, 0.001f);
                        }
                        this.<DURATION>__13 = 0.3f * this.durationMultiplier;
                        this.<ie>__14 = TimeUtil.WaitForUnscaledSeconds(this.<DURATION>__13);
                        goto Label_04FE;

                    case 6:
                        goto Label_04FE;

                    case 7:
                        this.<tt>__17 = new TransformAnimationTask(this.<ta>__16.transform, this.<DURATION>__15, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__17.scale(Vector3.one, true, Easing.Function.IN_QUART);
                        this.<ta>__16.addTask(this.<tt>__17);
                        goto Label_0670;

                    case 8:
                        goto Label_0670;

                    case 9:
                        goto Label_0712;

                    case 10:
                        goto Label_0775;

                    case 11:
                        goto Label_0818;

                    case 12:
                        goto Label_0916;

                    default:
                        goto Label_0976;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 2;
                    goto Label_0978;
                }
            Label_022C:
                this.<DURATION>__1 = 0.35f * this.durationMultiplier;
                this.<ta>__2 = this.<>f__this.BgTa;
                this.<tt>__3 = new TransformAnimationTask(this.<ta>__2.Tm, this.<DURATION>__1, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__3.scale(Vector3.one, true, Easing.Function.OUT_BOUNCE);
                this.<ta>__2.addTask(this.<tt>__3);
                this.<DURATION>__4 = 0.5f * this.durationMultiplier;
                this.<ie>__5 = TimeUtil.WaitForUnscaledSeconds(this.<DURATION>__4);
            Label_02DB:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 3;
                    goto Label_0978;
                }
                this.<STAY_DURATION>__6 = 2f * this.durationMultiplier;
                this.<outroTime>__7 = Time.time + this.<STAY_DURATION>__6;
                if (!this.slam)
                {
                    this.<DURATION>__19 = 0.3f * this.durationMultiplier;
                    this.<>f__this.TextGroup1.animateToBlack(this.<DURATION>__19, 0f);
                    this.<DURATION>__20 = 0.3f * this.durationMultiplier;
                    this.<>f__this.TextGroup2.animateToBlack(this.<DURATION>__20, 0f);
                    goto Label_0818;
                }
                this.<DURATION>__8 = 0.25f * this.durationMultiplier;
                this.<ie>__9 = TimeUtil.WaitForUnscaledSeconds(this.<DURATION>__8);
            Label_035F:
                while (this.<ie>__9.MoveNext())
                {
                    this.$current = this.<ie>__9.Current;
                    this.$PC = 4;
                    goto Label_0978;
                }
                this.<DURATION>__10 = 0.1f * this.durationMultiplier;
                AudioSystem.PlaybackParameters pp = new AudioSystem.PlaybackParameters();
                pp.DelayMin = this.<DURATION>__10;
                pp.DelayMax = this.<DURATION>__10;
                pp.PitchMin = 0.9f;
                pp.PitchMax = 0.9f;
                Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_UISlam, pp);
                this.<ta>__11 = this.<>f__this.TextTa1;
                this.<ta>__11.Tm.localScale = (Vector3) (Vector3.one * 5f);
                this.<>f__this.TextGroup1.setTransparent(false);
                this.$current = null;
                this.$PC = 5;
                goto Label_0978;
            Label_04FE:
                while (this.<ie>__14.MoveNext())
                {
                    this.$current = this.<ie>__14.Current;
                    this.$PC = 6;
                    goto Label_0978;
                }
                if ((Binder.RoomView != null) && (Binder.RoomView.RoomCamera != null))
                {
                    Binder.RoomView.RoomCamera.shakeCamera(0.2f, 0.01f);
                    Binder.RoomView.RoomCamera.shakeCamera(0.05f, 0.001f);
                }
                this.<DURATION>__15 = 0.1f * this.durationMultiplier;
                AudioSystem.PlaybackParameters parameters2 = new AudioSystem.PlaybackParameters();
                parameters2.DelayMin = this.<DURATION>__15;
                parameters2.DelayMax = this.<DURATION>__15;
                parameters2.PitchMin = 0.8f;
                parameters2.PitchMax = 0.8f;
                Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_UISlam, parameters2);
                this.<ta>__16 = this.<>f__this.TextTa2;
                this.<ta>__16.Tm.localScale = (Vector3) (Vector3.one * 5f);
                this.<>f__this.TextGroup2.setTransparent(false);
                this.$current = null;
                this.$PC = 7;
                goto Label_0978;
            Label_0670:
                if (this.<>f__this.TextTa1.HasTasks || this.<>f__this.TextTa2.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 8;
                    goto Label_0978;
                }
                if ((Binder.RoomView == null) || (Binder.RoomView.RoomCamera == null))
                {
                    goto Label_0818;
                }
                Binder.RoomView.RoomCamera.shakeCamera(0.03f, 0.001f);
                this.<ie>__18 = TimeUtil.WaitForUnscaledSeconds(0.1f * this.durationMultiplier);
            Label_0712:
                while (this.<ie>__18.MoveNext())
                {
                    this.$current = this.<ie>__18.Current;
                    this.$PC = 9;
                    goto Label_0978;
                }
                Binder.RoomView.RoomCamera.shakeCamera(0.02f, 0.001f);
                this.<ie>__18 = TimeUtil.WaitForUnscaledSeconds(0.1f * this.durationMultiplier);
            Label_0775:
                while (this.<ie>__18.MoveNext())
                {
                    this.$current = this.<ie>__18.Current;
                    this.$PC = 10;
                    goto Label_0978;
                }
                Binder.RoomView.RoomCamera.shakeCamera(0.01f, 0.001f);
            Label_0818:
                while (Time.time < this.<outroTime>__7)
                {
                    this.$current = null;
                    this.$PC = 11;
                    goto Label_0978;
                }
                this.<DURATION>__21 = 0.15f * this.durationMultiplier;
                this.<>f__this.TextGroup1.animateToTransparent(this.<DURATION>__21, 0f);
                this.<DURATION>__22 = 0.15f * this.durationMultiplier;
                this.<>f__this.TextGroup2.animateToTransparent(this.<DURATION>__22, 0f);
                this.<DURATION>__23 = 0.2f * this.durationMultiplier;
                this.<ta>__24 = this.<>f__this.BgTa;
                this.<tt>__25 = new TransformAnimationTask(this.<ta>__24.Tm, this.<DURATION>__23, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__25.scale(new Vector3(1f, 0f, 1f), true, Easing.Function.IN_BACK);
                this.<ta>__24.addTask(this.<tt>__25);
            Label_0916:
                while (this.<>f__this.BgTa.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 12;
                    goto Label_0978;
                }
                this.<>f__this.Bg.enabled = false;
                this.<>f__this.Text1.enabled = false;
                this.<>f__this.Text2.enabled = false;
                this.<>f__this.m_activeRoutine = null;
                goto Label_0976;
                this.$PC = -1;
            Label_0976:
                return false;
            Label_0978:
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

