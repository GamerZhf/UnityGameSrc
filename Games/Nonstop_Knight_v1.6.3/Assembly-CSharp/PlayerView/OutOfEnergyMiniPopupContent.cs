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
    using UnityEngine.UI;

    public class OutOfEnergyMiniPopupContent : MenuContent
    {
        public UnityEngine.UI.Text Cost;
        private InputParams m_params;
        private Coroutine m_purchaseRoutine;
        public UnityEngine.UI.Text Text;

        protected override void onAwake()
        {
        }

        protected override void onCleanup()
        {
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.m_params = (InputParams) param;
            this.onRefresh();
        }

        public void onPurchaseButtonClicked()
        {
            if (UnityUtils.CoroutineRunning(ref this.m_purchaseRoutine))
            {
            }
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle("OUT OF ENERGY!", string.Empty, string.Empty);
        }

        [DebuggerHidden]
        private IEnumerator purchaseRoutine()
        {
            <purchaseRoutine>c__Iterator159 iterator = new <purchaseRoutine>c__Iterator159();
            iterator.<>f__this = this;
            return iterator;
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.OutOfEnergyMiniPopup;
            }
        }

        [CompilerGenerated]
        private sealed class <purchaseRoutine>c__Iterator159 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal OutOfEnergyMiniPopupContent <>f__this;
            internal int <maxEnergy>__1;
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
                    this.<maxEnergy>__1 = GameLogic.Binder.LevelUpRules.getMaxEnergyForLevel(this.<player>__0.Rank);
                    CmdGainResources.ExecuteStatic(this.<player>__0, ResourceType.Energy, (double) this.<maxEnergy>__1, false, string.Empty, null);
                    this.<>f__this.m_purchaseRoutine = null;
                    if (this.<>f__this.m_params.SuccessCallback != null)
                    {
                        this.<>f__this.m_params.SuccessCallback();
                    }
                    else
                    {
                        PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                    }
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
            public System.Action SuccessCallback;
        }
    }
}

