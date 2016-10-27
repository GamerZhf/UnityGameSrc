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

    public class GachaPopupContent : MenuContent
    {
        [CompilerGenerated]
        private GameLogic.ShopEntry <ShopEntry>k__BackingField;
        public GameObject ChestEffect;
        public Text CostText;
        public PlayerView.RewardCarousel RewardCarousel;

        protected override void onAwake()
        {
        }

        public void onBuyButtonClicked()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (!PlayerView.Binder.MenuSystem.InTransition && (activeDungeon.ActiveRoom != null))
            {
            }
        }

        protected override void onCleanup()
        {
            this.ChestEffect.SetActive(false);
        }

        public void onCustomCloseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        protected void OnDisable()
        {
        }

        protected void OnEnable()
        {
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.CostText.text = this.ShopEntry.CostAmount.ToString("0");
            this.ChestEffect.SetActive(true);
            this.onRefresh();
        }

        protected override void onPreWarm([Optional, DefaultParameterValue(null)] object param)
        {
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle("GOLDEN CHEST", string.Empty, string.Empty);
        }

        private void onShopClosed()
        {
            PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.ThinPopupMenu, MenuContentType.GachaPopupContent, null);
        }

        private void onShopPurchaseCompleted(GameLogic.ShopEntry shopEntry, PurchaseResult purchaseResult)
        {
            PlayerView.Binder.MenuSystem.waitAndTransitionToNewMenu(MenuType.ThinPopupMenu, MenuContentType.GachaPopupContent, null);
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator11C iteratorc = new <onShow>c__Iterator11C();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.GachaPopupContent;
            }
        }

        public GameLogic.ShopEntry ShopEntry
        {
            [CompilerGenerated]
            get
            {
                return this.<ShopEntry>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ShopEntry>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator11C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal GachaPopupContent <>f__this;

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
                    this.<>f__this.RewardCarousel.animate();
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

