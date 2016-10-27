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

    public class SpeechBubbleMenu : Menu
    {
        public const float ANIMATION_DURATION = 0.25f;
        public OffscreenOpenClose AvatarAndBubblePanel;
        public Text AvatarAndBubbleSpeakerDialogue;
        public MenuOverlay BackgroundOverlay;
        public RawImage HeroAvatarImage;
        private InputParams m_inputParams;
        private Coroutine m_tapToContinueTimerRoutine;
        public OffscreenOpenClose TapToContinue;
        public const float TAPTOCONTINUE_DELAY = 2f;
        public Text TapToContinueText;

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator18F iteratorf = new <hideRoutine>c__Iterator18F();
            iteratorf.instant = instant;
            iteratorf.<$>instant = instant;
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        protected override void onAwake()
        {
            this.TapToContinueText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_TAP_TO_CONTINUE, null, false));
        }

        public void onButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator18D iteratord = new <preShowRoutine>c__Iterator18D();
            iteratord.parameter = parameter;
            iteratord.<$>parameter = parameter;
            iteratord.<>f__this = this;
            return iteratord;
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator18E iteratore = new <showRoutine>c__Iterator18E();
            iteratore.<>f__this = this;
            return iteratore;
        }

        [DebuggerHidden]
        private IEnumerator tapToContinueTimerRoutine()
        {
            <tapToContinueTimerRoutine>c__Iterator190 iterator = new <tapToContinueTimerRoutine>c__Iterator190();
            iterator.<>f__this = this;
            return iterator;
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
                return PlayerView.MenuType.SpeechBubble;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator18F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal SpeechBubbleMenu <>f__this;
            internal ManualTimer <timer>__0;
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
                        UnityUtils.StopCoroutine(this.<>f__this, ref this.<>f__this.m_tapToContinueTimerRoutine);
                        if (this.instant)
                        {
                            this.<>f__this.BackgroundOverlay.fadeToTransparent(0f, Easing.Function.LINEAR);
                            goto Label_012A;
                        }
                        this.<>f__this.BackgroundOverlay.fadeToTransparent(ConfigUi.POPUP_TRANSITION_DURATION_OUT, Easing.Function.LINEAR);
                        this.<timer>__0 = new ManualTimer();
                        this.<>f__this.AvatarAndBubblePanel.close(0.25f, ConfigUi.MENU_EASING_IN, 0f);
                        this.<>f__this.TapToContinue.close(0.125f, ConfigUi.MENU_EASING_IN, 0f);
                        this.<timer>__0.set(0.25f);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00FA;

                    default:
                        goto Label_01A9;
                }
                if (!this.<timer>__0.tick(Time.unscaledDeltaTime))
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01AB;
                }
            Label_00FA:
                while (this.<>f__this.BackgroundOverlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01AB;
                }
            Label_012A:
                PlayerView.Binder.MenuSystem.MenuHero.CharacterView.Animator.UseUnscaledTime = false;
                PlayerView.Binder.MenuSystem.CharacterMenuCamera.Camera.enabled = false;
                PlayerView.Binder.MenuSystem.MenuHero.CharacterView.gameObject.SetActive(false);
                if (this.<>f__this.m_inputParams.CloseCallback != null)
                {
                    this.<>f__this.m_inputParams.CloseCallback();
                    goto Label_01A9;
                    this.$PC = -1;
                }
            Label_01A9:
                return false;
            Label_01AB:
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
        private sealed class <preShowRoutine>c__Iterator18D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal SpeechBubbleMenu <>f__this;
            internal Player <player>__0;
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
                    this.<>f__this.m_inputParams = (SpeechBubbleMenu.InputParams) this.parameter;
                    this.<player>__0 = GameLogic.Binder.GameState.Player;
                    this.<>f__this.AvatarAndBubbleSpeakerDialogue.text = this.<>f__this.m_inputParams.Message;
                    PlayerView.Binder.MenuSystem.initializeMenuHero(this.<player>__0.ActiveCharacter);
                    PlayerView.Binder.MenuSystem.CharacterMenuCamera.Target = PlayerView.Binder.MenuSystem.MenuHero;
                    PlayerView.Binder.MenuSystem.MenuHero.CharacterView.Animator.UseUnscaledTime = true;
                    PlayerView.Binder.MenuSystem.MenuHero.CharacterView.gameObject.SetActive(true);
                    PlayerView.Binder.MenuSystem.MenuHero.CharacterView.setVisibility(true);
                    PlayerView.Binder.MenuSystem.MenuHero.CharacterView.Transform.rotation = Quaternion.Euler(0f, -20f, 0f);
                    PlayerView.Binder.MenuSystem.CharacterMenuCamera.Camera.enabled = true;
                    this.<>f__this.HeroAvatarImage.texture = PlayerView.Binder.MenuSystem.CharacterMenuCamera.RenderTexture;
                    this.<>f__this.HeroAvatarImage.enabled = true;
                    this.<>f__this.AvatarAndBubblePanel.close(0f, Easing.Function.LINEAR, 0f);
                    this.<>f__this.TapToContinue.close(0f, Easing.Function.LINEAR, 0f);
                    PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_PopupTutorial, (float) 0.2f);
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
        private sealed class <showRoutine>c__Iterator18E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal SpeechBubbleMenu <>f__this;
            internal ManualTimer <timer>__0;

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
                        this.<timer>__0 = new ManualTimer();
                        this.<>f__this.BackgroundOverlay.fadeToBlack(ConfigUi.POPUP_TRANSITION_DURATION_IN, ConfigUi.MENU_BACKGROUND_ALPHA, Easing.Function.LINEAR);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00C0;

                    default:
                        goto Label_0102;
                }
                if (this.<>f__this.BackgroundOverlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_0104;
                }
                this.<>f__this.AvatarAndBubblePanel.open(0.25f, ConfigUi.MENU_EASING_OUT, 0f);
                this.<timer>__0.set(0.25f);
            Label_00C0:
                while (!this.<timer>__0.tick(Time.unscaledDeltaTime))
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0104;
                }
                this.<>f__this.m_tapToContinueTimerRoutine = UnityUtils.StartCoroutine(this.<>f__this, this.<>f__this.tapToContinueTimerRoutine());
                goto Label_0102;
                this.$PC = -1;
            Label_0102:
                return false;
            Label_0104:
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
        private sealed class <tapToContinueTimerRoutine>c__Iterator190 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal SpeechBubbleMenu <>f__this;
            internal ManualTimer <timer>__0;

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
                        this.<timer>__0 = new ManualTimer(2f);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00A5;

                    default:
                        goto Label_00D2;
                }
                if (!this.<timer>__0.Idle)
                {
                    this.<timer>__0.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_00D4;
                }
                this.<>f__this.TapToContinue.open(0.25f, ConfigUi.MENU_EASING_OUT, 0f);
            Label_00A5:
                while (this.<>f__this.TapToContinue.Animating)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_00D4;
                }
                this.<>f__this.m_tapToContinueTimerRoutine = null;
                goto Label_00D2;
                this.$PC = -1;
            Label_00D2:
                return false;
            Label_00D4:
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

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParams
        {
            public string Message;
            public System.Action CloseCallback;
        }
    }
}

