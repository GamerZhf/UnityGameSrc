namespace PlayerView
{
    using App;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class PendingPurchasePopupContent : MenuContent
    {
        public GameObject Button;
        public UnityEngine.UI.Text ButtonText;
        public GameObject LoadingIndicator;
        public UnityEngine.UI.Text Text;

        protected override void onAwake()
        {
        }

        public void onButtonClicked()
        {
            this.LoadingIndicator.SetActive(true);
            this.Button.gameObject.SetActive(false);
            base.m_contentMenu.setCloseButtonVisibility(false);
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.IAP_VALIDATING, null, false)), string.Empty, string.Empty);
            this.Text.text = null;
            Service.Binder.ShopService.ResumePendingPurchases(new ShopService.PurchaseResultCallback(this.OnResumePurchaseResult));
        }

        protected override void onCleanup()
        {
            Service.Binder.ShopManager.PendingPopupShowed = true;
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.LoadingIndicator.SetActive(false);
            this.Button.gameObject.SetActive(true);
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.IAP_PURCHASE, null, false)), string.Empty, string.Empty);
            base.m_contentMenu.setCloseButtonVisibility(true);
            this.onRefresh();
        }

        protected override void onRefresh()
        {
            this.ButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.IAP_VALIDATE, null, false));
        }

        private void OnResumePurchaseResult(PurchaseResult result)
        {
            base.m_contentMenu.setCloseButtonVisibility(true);
            if (result == PurchaseResult.Fail)
            {
                this.LoadingIndicator.SetActive(false);
                this.Text.text = _.L(ConfigLoca.IAP_SOMETHING_WENT_WRONG, null, false);
            }
            else
            {
                (base.m_contentMenu as TechPopupMenu).onCloseButtonClicked();
            }
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator15B iteratorb = new <onShow>c__Iterator15B();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.PendingPurchasesPopupContent;
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator15B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal PendingPurchasePopupContent <>f__this;

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
                    this.<>f__this.Text.text = _.L(ConfigLoca.IAP_HAS_PENDING, new <>__AnonType4<int>(Service.Binder.ShopManager.PendingCount), false);
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

