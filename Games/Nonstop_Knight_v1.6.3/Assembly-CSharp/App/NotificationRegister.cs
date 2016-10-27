namespace App
{
    using Android;
    using GameLogic;
    using PlayerView;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UTNotifications;

    public class NotificationRegister : ILocalNotificationSystem
    {
        [CompilerGenerated]
        private bool <AskingInputFromPlayer>k__BackingField;
        [CompilerGenerated]
        private bool <Registered>k__BackingField;

        public NotificationRegister()
        {
            GameLogic.Binder.EventBus.OnTutorialCompleted += new GameLogic.Events.TutorialCompleted(this.onTutorialCompleted);
            this.init();
        }

        private void init()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (!this.Registered && player.hasCompletedTutorial("TUT200A"))
            {
                this.register(true);
            }
        }

        private void onTutorialCompleted(Player player, string tutorialId)
        {
            if (!this.Registered && (tutorialId == "TUT200A"))
            {
                this.register(true);
            }
        }

        private void register(bool duringGameplay)
        {
            if (!this.Registered)
            {
                if (duringGameplay)
                {
                    this.AskingInputFromPlayer = false;
                    PlayerView.Binder.InputSystem.setInputRequirement(InputSystem.Layer.LocalNotificationDialog, InputSystem.Requirement.MustBeDisabled);
                    Service.Binder.TaskManager.StartTask(this.ResetInputRequirementDelayed(), null);
                }
                AndroidNotificationServices.RegisterForNotifications(AndroidNotificationType.Alert, true);
                Manager.Instance.Initialize(true, 0, false);
                this.Registered = true;
            }
        }

        [DebuggerHidden]
        private IEnumerator ResetInputRequirementDelayed()
        {
            <ResetInputRequirementDelayed>c__Iterator31 iterator = new <ResetInputRequirementDelayed>c__Iterator31();
            iterator.<>f__this = this;
            return iterator;
        }

        public bool AskingInputFromPlayer
        {
            [CompilerGenerated]
            get
            {
                return this.<AskingInputFromPlayer>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<AskingInputFromPlayer>k__BackingField = value;
            }
        }

        public bool Registered
        {
            [CompilerGenerated]
            get
            {
                return this.<Registered>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Registered>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <ResetInputRequirementDelayed>c__Iterator31 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal NotificationRegister <>f__this;
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
                        this.$current = 0;
                        this.$PC = 1;
                        goto Label_0096;

                    case 1:
                        this.<timer>__0 = new ManualTimer(2f);
                        break;

                    case 2:
                        break;

                    default:
                        goto Label_0094;
                }
                if (!this.<timer>__0.Idle)
                {
                    this.<>f__this.AskingInputFromPlayer = false;
                    PlayerView.Binder.InputSystem.setInputRequirement(InputSystem.Layer.LocalNotificationDialog, InputSystem.Requirement.Neutral);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0096;
                }
                this.$PC = -1;
            Label_0094:
                return false;
            Label_0096:
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

