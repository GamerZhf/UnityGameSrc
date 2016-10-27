namespace PlayerView
{
    using Android;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class QuitConfirmationPopup
    {
        private MonoBehaviour owner;

        public QuitConfirmationPopup(MonoBehaviour owner)
        {
            this.owner = owner;
        }

        private void onQuitConfirmationYes()
        {
            UnityUtils.StartCoroutine(this.owner, this.performQuit());
        }

        [DebuggerHidden]
        private IEnumerator performQuit()
        {
            return new <performQuit>c__Iterator166();
        }

        public void Show()
        {
            ConfirmationPopupContent.InputParameters parameters2 = new ConfirmationPopupContent.InputParameters();
            parameters2.TitleText = _.L(ConfigLoca.UI_PROMPT_QUIT, null, false);
            parameters2.DescriptionText = _.L(ConfigLoca.UI_PROMPT_QUIT_DESCRIPTION, null, false);
            parameters2.LeftButtonText = _.L(ConfigLoca.UI_BUTTON_NO, null, false);
            parameters2.RightButtonText = _.L(ConfigLoca.UI_BUTTON_YES, null, false);
            parameters2.RightButtonCallback = new System.Action(this.onQuitConfirmationYes);
            parameters2.DisableCloseButton = true;
            parameters2.SkipPauseGame = !GameLogic.Binder.TimeSystem.paused();
            ConfirmationPopupContent.InputParameters parameter = parameters2;
            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TechPopupMenu, MenuContentType.ConfirmationPopupContent, parameter, 0f, false, true);
        }

        [CompilerGenerated]
        private sealed class <performQuit>c__Iterator166 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
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
                        this.$current = PlayerView.Binder.MenuSystem.waitForMenuToBeClosed(MenuType.TechPopupMenu);
                        this.$PC = 1;
                        goto Label_0093;

                    case 1:
                        this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(0.1f);
                        break;

                    case 2:
                        break;

                    default:
                        goto Label_0091;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 2;
                    goto Label_0093;
                }
                AndroidApplication.Suspend();
                this.$PC = -1;
            Label_0091:
                return false;
            Label_0093:
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

