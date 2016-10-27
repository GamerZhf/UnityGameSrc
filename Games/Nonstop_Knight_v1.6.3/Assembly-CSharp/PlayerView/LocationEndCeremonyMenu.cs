namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class LocationEndCeremonyMenu : Menu
    {
        public const float ANIM_DURATION_ENTRY = 0.5f;
        public const float ANIM_DURATION_EXIT = 0.5f;
        public GameObject BackgroundRoot;
        public Text DidYouKnowText;
        public Text DidYouKnowTitle;
        public Text HeaderText;
        public GameObject HighlightsRoot;
        public Text LoadingText;
        public GameObject LoadingTextRoot;
        private InputParams m_params;
        public CanvasGroupAlphaFading PanelCanvasGroup;
        public AnimatedProgressBar ProgressBar;
        public Text RetirementLoadingText;
        public GameObject RetirementLoadingTextRoot;

        [ContextMenu("editorTest")]
        private void editorTest()
        {
            base.StartCoroutine(this.editorTestRoutine());
        }

        [DebuggerHidden]
        private IEnumerator editorTestRoutine()
        {
            <editorTestRoutine>c__Iterator130 iterator = new <editorTestRoutine>c__Iterator130();
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator12F iteratorf = new <hideRoutine>c__Iterator12F();
            iteratorf.instant = instant;
            iteratorf.<$>instant = instant;
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        protected override void onAwake()
        {
            this.LoadingText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.LOCATION_END_LOADING_TEXT, null, false));
            this.RetirementLoadingText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ASCENSION_SCREEN_LOADING_TEXT, null, false));
            this.DidYouKnowTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.LOCATION_END_DID_YOU_KNOW, null, false));
        }

        public void onLoadProgress(float v)
        {
            if (this.ProgressBar != null)
            {
                this.ProgressBar.animateToNormalizedValue(v, 0.4f, null, null, 0f);
            }
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator12D iteratord = new <preShowRoutine>c__Iterator12D();
            iteratord.parameter = parameter;
            iteratord.<$>parameter = parameter;
            iteratord.<>f__this = this;
            return iteratord;
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator12E iteratore = new <showRoutine>c__Iterator12E();
            iteratore.<>f__this = this;
            return iteratore;
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
                return PlayerView.MenuType.LocationEndCeremonyMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <editorTestRoutine>c__Iterator130 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal LocationEndCeremonyMenu <>f__this;
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
                        this.<ie>__0 = this.<>f__this.preShowRoutine(MenuContentType.NONE, null);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_009F;

                    default:
                        goto Label_00BB;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    goto Label_00BD;
                }
                this.<ie>__0 = this.<>f__this.showRoutine(MenuContentType.NONE, null);
            Label_009F:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 2;
                    goto Label_00BD;
                }
                goto Label_00BB;
                this.$PC = -1;
            Label_00BB:
                return false;
            Label_00BD:
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
        private sealed class <hideRoutine>c__Iterator12F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal LocationEndCeremonyMenu <>f__this;
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
                            break;
                        }
                        this.$current = this.<>f__this.PanelCanvasGroup.animateToTransparent(0.5f, 0f);
                        this.$PC = 1;
                        return true;

                    case 1:
                        break;
                        this.$PC = -1;
                        break;
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
        private sealed class <preShowRoutine>c__Iterator12D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal LocationEndCeremonyMenu <>f__this;
            internal HeroStats <heroStats>__1;
            internal Player <player>__0;
            internal int <tipIdx>__2;
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
                    this.<>f__this.m_params = (LocationEndCeremonyMenu.InputParams) this.parameter;
                    this.<>f__this.BackgroundRoot.SetActive(!this.<>f__this.m_params.IsRetirement && string.IsNullOrEmpty(this.<>f__this.m_params.CustomText));
                    this.<>f__this.HighlightsRoot.SetActive(!this.<>f__this.m_params.IsRetirement && string.IsNullOrEmpty(this.<>f__this.m_params.CustomText));
                    this.<>f__this.LoadingTextRoot.SetActive(!this.<>f__this.m_params.IsRetirement && string.IsNullOrEmpty(this.<>f__this.m_params.CustomText));
                    if (this.<>f__this.m_params.IsRetirement)
                    {
                        this.<>f__this.RetirementLoadingText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ASCENSION_SCREEN_LOADING_TEXT, null, false));
                        this.<>f__this.RetirementLoadingTextRoot.SetActive(true);
                    }
                    else if (!string.IsNullOrEmpty(this.<>f__this.m_params.CustomText))
                    {
                        this.<>f__this.RetirementLoadingText.text = this.<>f__this.m_params.CustomText;
                        this.<>f__this.RetirementLoadingTextRoot.SetActive(true);
                    }
                    else
                    {
                        this.<>f__this.RetirementLoadingTextRoot.SetActive(false);
                    }
                    this.<player>__0 = GameLogic.Binder.GameState.Player;
                    this.<>f__this.PanelCanvasGroup.setTransparent(true);
                    this.<heroStats>__1 = new HeroStats(this.<player>__0.CumulativeRetiredHeroStats);
                    this.<heroStats>__1.add(this.<player>__0.ActiveCharacter.HeroStats);
                    this.<tipIdx>__2 = Mathf.Clamp(this.<player>__0.TipIndex, 0, ConfigUi.TIPS.Count - 1);
                    try
                    {
                        this.<>f__this.DidYouKnowText.text = MenuHelpers.GetFormattedTip(ConfigUi.TIPS[this.<tipIdx>__2], this.<heroStats>__1);
                    }
                    catch (Exception)
                    {
                        this.<>f__this.DidYouKnowText.text = string.Empty;
                        UnityEngine.Debug.LogError("Error in formatting location end tip at index: " + this.<tipIdx>__2);
                    }
                    this.<player>__0.TipIndex = (this.<tipIdx>__2 + 1) % ConfigUi.TIPS.Count;
                    this.<>f__this.ProgressBar.setNormalizedValue(0f);
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
        private sealed class <showRoutine>c__Iterator12E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal LocationEndCeremonyMenu <>f__this;

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
                        this.$current = this.<>f__this.PanelCanvasGroup.animateToBlack(0.5f, 0f);
                        this.$PC = 1;
                        return true;

                    case 1:
                        break;
                        this.$PC = -1;
                        break;
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

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParams
        {
            public bool IsRetirement;
            public string CustomText;
        }
    }
}

