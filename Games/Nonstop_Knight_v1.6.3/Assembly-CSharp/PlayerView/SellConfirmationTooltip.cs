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

    public class SellConfirmationTooltip : MenuContent
    {
        [CompilerGenerated]
        private GameLogic.ItemInstance <ItemInstance>k__BackingField;
        public RectTransform ArrowRectTm;
        public RectTransform ButtonTm;
        private Vector3 m_arrowDefaultLocalPos;
        private Coroutine m_arrowRefreshRoutine;

        [DebuggerHidden]
        private IEnumerator arrowRefreshRoutine()
        {
            <arrowRefreshRoutine>c__Iterator17A iteratora = new <arrowRefreshRoutine>c__Iterator17A();
            iteratora.<>f__this = this;
            return iteratora;
        }

        protected override void onAwake()
        {
            this.m_arrowDefaultLocalPos = this.ArrowRectTm.localPosition;
        }

        protected override void onCleanup()
        {
            UnityUtils.StopCoroutine(this, ref this.m_arrowRefreshRoutine);
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.ItemInstance = (GameLogic.ItemInstance) param;
            this.m_arrowRefreshRoutine = UnityUtils.StartCoroutine(this, this.arrowRefreshRoutine());
        }

        protected override void onRefresh()
        {
        }

        public void onSellButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                CmdSellItem.ExecuteStatic(GameLogic.Binder.GameState.Player.ActiveCharacter, this.ItemInstance, this.ButtonTm);
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.SellConfirmationTooltip;
            }
        }

        public GameLogic.ItemInstance ItemInstance
        {
            [CompilerGenerated]
            get
            {
                return this.<ItemInstance>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ItemInstance>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <arrowRefreshRoutine>c__Iterator17A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal SellConfirmationTooltip <>f__this;
            internal TooltipMenu <tooltipMenu>__0;

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
                        this.<tooltipMenu>__0 = (TooltipMenu) this.<>f__this.m_contentMenu;
                        break;

                    case 1:
                        break;
                        this.$PC = -1;
                        goto Label_0077;

                    default:
                        goto Label_0077;
                }
                this.<tooltipMenu>__0.refreshArrowPosition(this.<>f__this.ArrowRectTm, this.<>f__this.m_arrowDefaultLocalPos);
                this.$current = null;
                this.$PC = 1;
                return true;
            Label_0077:
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

