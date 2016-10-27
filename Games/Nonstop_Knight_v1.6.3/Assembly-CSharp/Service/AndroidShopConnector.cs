namespace Service
{
    using App;
    using Prime31;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AndroidShopConnector : IPlatformShopConnector
    {
        private bool billingSupported = true;
        private string currentCurrency;
        private AndroidPurchase currentPurchase;
        private GameObject nativeMessageListener;
        private readonly Dictionary<string, AndroidPurchase> pending = new Dictionary<string, AndroidPurchase>();
        private readonly Dictionary<string, PremiumProduct> platformProdMap = new Dictionary<string, PremiumProduct>();
        private Dictionary<string, GoogleSkuInfo> products = new Dictionary<string, GoogleSkuInfo>();
        private string refProductId;
        private bool unreachable;
        private bool waitingForProducts;

        public Purchase CreatePurchase(PremiumProduct _product)
        {
            AndroidPurchase purchase = new AndroidPurchase(this);
            purchase.product = _product;
            this.currentPurchase = purchase;
            return this.currentPurchase;
        }

        private void EndPurchase(AndroidPurchase _purchase)
        {
            if ((_purchase.State == EPurchaseState.Commited) && (_purchase.GooglePurchase != null))
            {
                GoogleIAB.consumeProduct(_purchase.GooglePurchase.productId);
            }
            else if (this.currentPurchase == _purchase)
            {
                this.currentPurchase = null;
            }
        }

        public List<Purchase> GetPending()
        {
            List<Purchase> list = new List<Purchase>();
            if (this.pending != null)
            {
                foreach (AndroidPurchase purchase in this.pending.Values)
                {
                    list.Add(purchase);
                }
            }
            return list;
        }

        [DebuggerHidden]
        public IEnumerator Initialize(string _refProduct)
        {
            <Initialize>c__Iterator204 iterator = new <Initialize>c__Iterator204();
            iterator._refProduct = _refProduct;
            iterator.<$>_refProduct = _refProduct;
            iterator.<>f__this = this;
            return iterator;
        }

        private static void Log(string _str)
        {
            Service.Binder.Logger.Log(_str);
        }

        public bool MergeProductData(PremiumProduct _product)
        {
            GoogleSkuInfo info;
            if (this.products.TryGetValue(_product.storeProductId, out info))
            {
                _product.price = Mathf.Floor(((float) info.priceAmountMicros) / 10000f) / 100f;
                _product.currencyCode = info.priceCurrencyCode;
                _product.productDescription = info.description;
                _product.productName = info.title;
                _product.formattedPrice = info.price;
                this.platformProdMap[info.productId] = _product;
                return true;
            }
            return false;
        }

        private void OnBillingNotSupported(string _error)
        {
            Log("Billing not supported: " + _error);
            this.billingSupported = false;
            this.unreachable = true;
        }

        private void OnBillingSupported()
        {
            Log("Billing supported");
            this.billingSupported = true;
            string[] skus = new string[] { this.refProductId };
            GoogleIAB.queryInventory(skus);
        }

        private void OnConsumePurchaseFailed(string _error)
        {
            Log("Consume failed:" + _error);
            this.currentPurchase = null;
        }

        private void OnConsumePurchaseSucceeded(GooglePurchase _purchase)
        {
            Log("Consume succeeded:" + _purchase.purchaseToken);
            if (this.pending.ContainsKey(_purchase.purchaseToken))
            {
                this.pending.Remove(_purchase.purchaseToken);
            }
            this.currentPurchase = null;
        }

        private void OnPendingPurchase(GooglePurchase _purchase)
        {
            Log(string.Concat(new object[] { "OnPendingPurchase: ", _purchase.purchaseToken, " ", _purchase.purchaseState }));
            if (!this.pending.ContainsKey(_purchase.purchaseToken) && (((this.currentPurchase == null) || (this.currentPurchase.GooglePurchase == null)) || (this.currentPurchase.GooglePurchase.purchaseToken != _purchase.purchaseToken)))
            {
                AndroidPurchase purchase = new AndroidPurchase(this);
                purchase.GooglePurchase = _purchase;
                purchase.State = EPurchaseState.Pending;
                this.platformProdMap.TryGetValue(_purchase.productId, out purchase.product);
                this.pending[_purchase.purchaseToken] = purchase;
            }
        }

        private void OnPurchaseFailed(string _error, int _errorCode)
        {
            Log(string.Concat(new object[] { "Purchase failed:", _error, " [", _errorCode, "]" }));
            if (this.currentPurchase != null)
            {
                this.currentPurchase.State = ((_errorCode != -1005) && !(_error == "User cancelled")) ? EPurchaseState.Failed : EPurchaseState.Cancelled;
                this.EndPurchase(this.currentPurchase);
            }
        }

        private void OnPurchaseSucceeded(GooglePurchase _purchase)
        {
            Log(string.Concat(new object[] { "OnPurchaseSucceeded: ", _purchase.purchaseToken, " ", _purchase.purchaseState }));
            if (this.currentPurchase != null)
            {
                this.currentPurchase.GooglePurchase = _purchase;
                this.currentPurchase.State = EPurchaseState.Success;
                this.pending[_purchase.purchaseToken] = this.currentPurchase;
            }
            else
            {
                this.OnPendingPurchase(_purchase);
            }
        }

        private void OnQueryInventoryFailed(string _error)
        {
            Log("Query inventory failed: " + _error);
            this.unreachable = true;
            this.waitingForProducts = false;
        }

        private void OnQueryInventorySucceeded(List<GooglePurchase> _purchases, List<GoogleSkuInfo> _products)
        {
            Log("purchases recieved #" + _purchases.Count);
            foreach (GooglePurchase purchase in _purchases)
            {
                if (purchase.purchaseState == GooglePurchase.GooglePurchaseState.Purchased)
                {
                    this.OnPendingPurchase(purchase);
                }
            }
            Log("products recieved #" + _products.Count);
            if (_products.Count > 0)
            {
                this.products = new Dictionary<string, GoogleSkuInfo>();
                foreach (GoogleSkuInfo info in _products)
                {
                    this.products[info.productId] = info;
                }
            }
            this.waitingForProducts = false;
        }

        [DebuggerHidden]
        public IEnumerator RequestPlatformProducts(string[] _prodIds)
        {
            <RequestPlatformProducts>c__Iterator205 iterator = new <RequestPlatformProducts>c__Iterator205();
            iterator._prodIds = _prodIds;
            iterator.<$>_prodIds = _prodIds;
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        public IEnumerator ResumePending(Purchase _purchase)
        {
            <ResumePending>c__Iterator206 iterator = new <ResumePending>c__Iterator206();
            iterator._purchase = _purchase;
            iterator.<$>_purchase = _purchase;
            iterator.<>f__this = this;
            return iterator;
        }

        public void UnInitialize()
        {
            GoogleIABManager.billingSupportedEvent -= new Action(this.OnBillingSupported);
            GoogleIABManager.billingNotSupportedEvent -= new Action<string>(this.OnBillingNotSupported);
            GoogleIABManager.queryInventorySucceededEvent -= new Action<List<GooglePurchase>, List<GoogleSkuInfo>>(this.OnQueryInventorySucceeded);
            GoogleIABManager.queryInventoryFailedEvent -= new Action<string>(this.OnQueryInventoryFailed);
            GoogleIABManager.purchaseSucceededEvent -= new Action<GooglePurchase>(this.OnPurchaseSucceeded);
            GoogleIABManager.purchaseFailedEvent -= new Action<string, int>(this.OnPurchaseFailed);
            GoogleIABManager.consumePurchaseSucceededEvent -= new Action<GooglePurchase>(this.OnConsumePurchaseSucceeded);
            GoogleIABManager.consumePurchaseFailedEvent -= new Action<string>(this.OnConsumePurchaseFailed);
            GoogleIAB.unbindService();
            UnityEngine.Object.Destroy(this.nativeMessageListener);
            this.nativeMessageListener = null;
        }

        public bool Available
        {
            get
            {
                return (this.billingSupported && !this.unreachable);
            }
        }

        public string CurrencyCode
        {
            get
            {
                return this.currentCurrency;
            }
        }

        public int PendingCount
        {
            get
            {
                return this.pending.Count;
            }
        }

        public ShopManagerState State
        {
            get
            {
                if (this.currentPurchase == null)
                {
                    return ShopManagerState.Available;
                }
                if (this.currentPurchase.State != EPurchaseState.Success)
                {
                    return ShopManagerState.Purchasing;
                }
                return ShopManagerState.Validating;
            }
        }

        [CompilerGenerated]
        private sealed class <Initialize>c__Iterator204 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal string _refProduct;
            internal string <$>_refProduct;
            internal AndroidShopConnector <>f__this;
            internal Dictionary<string, GoogleSkuInfo>.Enumerator <e>__0;

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
                        AndroidShopConnector.Log("Setup GoogleIAB");
                        this.<>f__this.nativeMessageListener = new GameObject("AndroidShopConnector.NativeMessageListener");
                        this.<>f__this.nativeMessageListener.AddComponent<AndroidShopConnector.NativeMessageListener>().Connector = this.<>f__this;
                        GoogleIABManager.billingSupportedEvent += new Action(this.<>f__this.OnBillingSupported);
                        GoogleIABManager.billingNotSupportedEvent += new Action<string>(this.<>f__this.OnBillingNotSupported);
                        GoogleIABManager.queryInventorySucceededEvent += new Action<List<GooglePurchase>, List<GoogleSkuInfo>>(this.<>f__this.OnQueryInventorySucceeded);
                        GoogleIABManager.queryInventoryFailedEvent += new Action<string>(this.<>f__this.OnQueryInventoryFailed);
                        GoogleIABManager.purchaseSucceededEvent += new Action<GooglePurchase>(this.<>f__this.OnPurchaseSucceeded);
                        GoogleIABManager.purchaseFailedEvent += new Action<string, int>(this.<>f__this.OnPurchaseFailed);
                        GoogleIABManager.consumePurchaseSucceededEvent += new Action<GooglePurchase>(this.<>f__this.OnConsumePurchaseSucceeded);
                        GoogleIABManager.consumePurchaseFailedEvent += new Action<string>(this.<>f__this.OnConsumePurchaseFailed);
                        this.<>f__this.billingSupported = true;
                        this.<>f__this.unreachable = false;
                        this.<>f__this.waitingForProducts = true;
                        this.<>f__this.refProductId = this._refProduct;
                        AndroidShopConnector.Log("Request reference products:" + this._refProduct);
                        GoogleIAB.setAutoVerifySignatures(ConfigApp.GoogleStoreAutoVerifySignatures);
                        GoogleIAB.init(ConfigApp.GoogleStorePublicKey);
                        AndroidShopConnector.Log("Requested");
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0234;
                }
                if (this.<>f__this.waitingForProducts && !this.<>f__this.unreachable)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                if (!this.<>f__this.unreachable)
                {
                    AndroidShopConnector.Log("product there");
                    this.<e>__0 = this.<>f__this.products.GetEnumerator();
                    if (this.<e>__0.MoveNext())
                    {
                        this.<>f__this.currentCurrency = this.<e>__0.Current.Value.priceCurrencyCode;
                        AndroidShopConnector.Log("extract currency:" + this.<>f__this.currentCurrency);
                    }
                    this.$PC = -1;
                }
            Label_0234:
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
        private sealed class <RequestPlatformProducts>c__Iterator205 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal string[] _prodIds;
            internal string[] <$>_prodIds;
            internal AndroidShopConnector <>f__this;

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
                        this.<>f__this.waitingForProducts = true;
                        GoogleIAB.queryInventory(this._prodIds);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0077;
                }
                if (this.<>f__this.waitingForProducts && !this.<>f__this.unreachable)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.$PC = -1;
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

        [CompilerGenerated]
        private sealed class <ResumePending>c__Iterator206 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Purchase _purchase;
            internal Purchase <$>_purchase;
            internal AndroidShopConnector <>f__this;
            internal AndroidShopConnector.AndroidPurchase <androidPurchase>__0;

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
                        this.<androidPurchase>__0 = (AndroidShopConnector.AndroidPurchase) this._purchase;
                        this.<>f__this.currentPurchase = this.<androidPurchase>__0;
                        AndroidShopConnector.Log(string.Concat(new object[] { "Resume pending #", this.<>f__this.PendingCount, " ", this.<androidPurchase>__0.GooglePurchase.purchaseToken }));
                        this.$current = this.<androidPurchase>__0.DoValidate();
                        this.$PC = 1;
                        return true;

                    case 1:
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

        private class AndroidPurchase : Purchase
        {
            public Prime31.GooglePurchase GooglePurchase;
            private readonly AndroidShopConnector wrapper;

            public AndroidPurchase(AndroidShopConnector _wrapper)
            {
                this.wrapper = _wrapper;
            }

            public override void AbortPurchase()
            {
                if (this.GooglePurchase != null)
                {
                    this.wrapper.pending[this.GooglePurchase.purchaseToken] = this;
                }
                this.wrapper.EndPurchase(this);
            }

            public override ValidationRequest CreateValidationRequest()
            {
                ValidationRequest request = new ValidationRequest();
                request.gameUserId = Service.Binder.SessionData.UserId;
                request.isoCurrencyCode = this.wrapper.CurrencyCode;
                request.storeId = StoreId.gplay.ToString();
                request.trackingUserId = Service.Binder.SessionData.FgUserHandle;
                request.signature = this.GooglePurchase.signature;
                request.receipt = this.GooglePurchase.originalJson;
                request.trackingPayload = base.TrackingPayload;
                return request;
            }

            [DebuggerHidden]
            public override IEnumerator DoCommit()
            {
                <DoCommit>c__Iterator20A iteratora = new <DoCommit>c__Iterator20A();
                iteratora.<>f__this = this;
                return iteratora;
            }

            [DebuggerHidden]
            public override IEnumerator DoPurchase()
            {
                <DoPurchase>c__Iterator209 iterator = new <DoPurchase>c__Iterator209();
                iterator.<>f__this = this;
                return iterator;
            }

            [CompilerGenerated]
            private sealed class <DoCommit>c__Iterator20A : IEnumerator, IDisposable, IEnumerator<object>
            {
                internal object $current;
                internal int $PC;
                internal AndroidShopConnector.AndroidPurchase <>f__this;

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
                            this.<>f__this.State = EPurchaseState.Commited;
                            this.<>f__this.wrapper.EndPurchase(this.<>f__this);
                            this.$current = null;
                            this.$PC = 1;
                            return true;

                        case 1:
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
            private sealed class <DoPurchase>c__Iterator209 : IEnumerator, IDisposable, IEnumerator<object>
            {
                internal object $current;
                internal int $PC;
                internal AndroidShopConnector.AndroidPurchase <>f__this;

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
                            this.<>f__this.State = EPurchaseState.Initiated;
                            GoogleIAB.purchaseProduct(this.<>f__this.product.storeProductId);
                            break;

                        case 1:
                            break;

                        default:
                            goto Label_0071;
                    }
                    if (this.<>f__this.State == EPurchaseState.Initiated)
                    {
                        this.$current = null;
                        this.$PC = 1;
                        return true;
                    }
                    this.$PC = -1;
                Label_0071:
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

        public class NativeMessageListener : MonoBehaviour
        {
            public AndroidShopConnector Connector;

            public void OnMessage(string _message)
            {
                if ((_message == "com.android.vending.billing.PURCHASES_UPDATED") && ((this.Connector.currentPurchase == null) && (Service.Binder.ShopManager != null)))
                {
                    Service.Binder.ShopManager.PendingPopupShowed = false;
                    Service.Binder.ShopManager.RefreshProducts();
                }
            }
        }
    }
}

