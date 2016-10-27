namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class MilestoneRewardMiniPopupContent : MenuContent
    {
        private Coroutine m_claimRewardRoutine;
        private string m_rewardId;

        [DebuggerHidden]
        private IEnumerator claimRewardRoutine()
        {
            <claimRewardRoutine>c__Iterator14F iteratorf = new <claimRewardRoutine>c__Iterator14F();
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        protected override void onAwake()
        {
        }

        public void onClaimButtonClicked()
        {
            if (!UnityUtils.CoroutineRunning(ref this.m_claimRewardRoutine))
            {
                this.m_claimRewardRoutine = UnityUtils.StartCoroutine(this, this.claimRewardRoutine());
            }
        }

        protected override void onCleanup()
        {
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.m_rewardId = (string) param;
            base.m_contentMenu.setCloseButtonVisibility(false);
            this.onRefresh();
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle("REWARD", string.Empty, string.Empty);
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.MilestoneRewardMiniPopup;
            }
        }

        [CompilerGenerated]
        private sealed class <claimRewardRoutine>c__Iterator14F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MilestoneRewardMiniPopupContent <>f__this;
            internal Player <player>__0;

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
                    this.<player>__0 = GameLogic.Binder.GameState.Player;
                    this.<player>__0.ClaimedRewardIds.Add(this.<>f__this.m_rewardId);
                    CmdGainResources.ExecuteStatic(this.<player>__0, ResourceType.Diamond, 5.0, false, string.Empty, null);
                    PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                    this.<>f__this.m_claimRewardRoutine = null;
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

