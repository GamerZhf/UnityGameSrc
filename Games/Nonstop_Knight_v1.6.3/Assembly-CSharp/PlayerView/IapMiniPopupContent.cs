namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class IapMiniPopupContent : MenuContent
    {
        public GameObject Button;
        public UnityEngine.UI.Text ButtonText;
        public GameObject LoadingIndicator;
        private InputParameters m_inputParams;
        public UnityEngine.UI.Text Text;

        protected override void onAwake()
        {
        }

        public void onButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                this.waitAndClose();
            }
        }

        protected override void onCleanup()
        {
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.m_inputParams = (InputParameters) param;
            base.m_contentMenu.setCloseButtonVisibility(false);
            this.Button.gameObject.SetActive(false);
            this.LoadingIndicator.SetActive(true);
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.IAP_PURCHASING, null, false)), string.Empty, string.Empty);
            this.Text.text = null;
            this.onRefresh();
        }

        private void onPurchaseResult(PurchaseResult result)
        {
            if (result == PurchaseResult.Success)
            {
                this.waitAndClose();
            }
            else if (result == PurchaseResult.Cancel)
            {
                this.waitAndClose();
            }
            else
            {
                this.showFailedMessage();
            }
            if (this.m_inputParams.PurchaseCallback != null)
            {
                this.m_inputParams.PurchaseCallback(this.m_inputParams.ShopEntry, result);
            }
        }

        protected override void onRefresh()
        {
        }

        private void onResumePurchaseResult(PurchaseResult result)
        {
            if (this.m_inputParams.PurchaseCallback != null)
            {
                this.m_inputParams.PurchaseCallback(this.m_inputParams.ShopEntry, result);
            }
            if (result == PurchaseResult.Fail)
            {
                this.showFailedMessage();
            }
            else
            {
                (base.m_contentMenu as TechPopupMenu).onCloseButtonClicked();
            }
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator121 iterator = new <onShow>c__Iterator121();
            iterator.<>f__this = this;
            return iterator;
        }

        private void purchase()
        {
            Purchase purchase = Service.Binder.ShopManager.CreatePurchase(this.m_inputParams.ShopEntry.Id);
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.IAP_PURCHASING, null, false)), string.Empty, string.Empty);
            Service.Binder.ShopService.StartPurchase(purchase, this.m_inputParams.PathToShop, new ShopService.PurchaseResultCallback(this.onPurchaseResult));
        }

        private void showAskToBuyMessage()
        {
            this.LoadingIndicator.SetActive(false);
            this.Text.text = _.L(ConfigLoca.IAP_LATER_DELIVERY, null, false);
            this.Button.gameObject.SetActive(true);
        }

        private void showFailedMessage()
        {
            this.LoadingIndicator.SetActive(false);
            this.Text.text = _.L(ConfigLoca.IAP_SOMETHING_WENT_WRONG, null, false);
            this.ButtonText.text = _.L(ConfigLoca.UI_PROMPT_CONTINUE, null, false);
            this.Button.gameObject.SetActive(true);
        }

        [DebuggerHidden]
        private IEnumerator testPurchaseRoutine()
        {
            <testPurchaseRoutine>c__Iterator122 iterator = new <testPurchaseRoutine>c__Iterator122();
            iterator.<>f__this = this;
            return iterator;
        }

        private void waitAndClose()
        {
            base.StartCoroutine(this.waitForMenuTransitionRoutine());
        }

        [DebuggerHidden]
        private IEnumerator waitForMenuTransitionRoutine()
        {
            return new <waitForMenuTransitionRoutine>c__Iterator123();
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.IapMiniPopupContent;
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator121 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal IapMiniPopupContent <>f__this;

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
                    if (!ConfigApp.ProductionBuild && ConfigApp.CHEAT_IAP_TEST_MODE)
                    {
                        this.<>f__this.StartCoroutine(this.<>f__this.testPurchaseRoutine());
                    }
                    else if (Service.Binder.ShopManager.PendingCount > 0)
                    {
                        this.<>f__this.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.IAP_RESTORING, new <>__AnonType4<int>(Service.Binder.ShopManager.PendingCount), false)), string.Empty, string.Empty);
                        Service.Binder.ShopService.ResumePendingPurchases(new ShopService.PurchaseResultCallback(this.<>f__this.onResumePurchaseResult));
                    }
                    else
                    {
                        this.<>f__this.purchase();
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

        [CompilerGenerated]
        private sealed class <testPurchaseRoutine>c__Iterator122 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal IapMiniPopupContent <>f__this;
            internal IEnumerator <ie>__0;
            internal Reward <r>__2;
            internal bool <showCeremony>__1;

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
                        this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(2f);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00EC;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    return true;
                }
                this.<showCeremony>__1 = this.<>f__this.m_inputParams.PathToShop != PathToShopType.CardPack;
                this.<r>__2 = new Reward();
                MathUtil.DistributeValuesIntoChunksDouble(this.<>f__this.m_inputParams.ShopEntry.BuyResourceAmounts[ResourceType.Diamond], 1, ref this.<r>__2.DiamondDrops);
                List<Reward> rewards = new List<Reward>();
                rewards.Add(this.<r>__2);
                RewardHelper.ClaimReward(rewards, this.<showCeremony>__1);
                this.<>f__this.onPurchaseResult(PurchaseResult.Success);
                goto Label_00EC;
                this.$PC = -1;
            Label_00EC:
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
        private sealed class <waitForMenuTransitionRoutine>c__Iterator123 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;

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
                    case 1:
                        if (PlayerView.Binder.MenuSystem.InTransition)
                        {
                            this.$current = null;
                            this.$PC = 1;
                            return true;
                        }
                        PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
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
        public struct InputParameters
        {
            public GameLogic.ShopEntry ShopEntry;
            public PathToShopType PathToShop;
            public Action<GameLogic.ShopEntry, PurchaseResult> PurchaseCallback;
        }
    }
}

