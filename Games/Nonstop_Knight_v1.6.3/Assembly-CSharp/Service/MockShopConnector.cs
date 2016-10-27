namespace Service
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class MockShopConnector : IPlatformShopConnector
    {
        private static string[] currencies = new string[] { "EUR", "USD", "RUB" };
        public static int currencyIndex = 0;
        public string currentCurrency;
        private MockPurchase currentPurchase;
        private IDictionary<string, MockPurchase> pending = new Dictionary<string, MockPurchase>();
        private static EPurchaseState resultState = EPurchaseState.Success;

        public static void ChangeCurrency()
        {
            currencyIndex = (currencyIndex + 1) % currencies.Length;
        }

        public Purchase CreatePurchase(PremiumProduct product)
        {
            MockPurchase purchase = new MockPurchase();
            purchase.product = product;
            purchase.State = EPurchaseState.Initiated;
            purchase.connector = this;
            return (this.currentPurchase = purchase);
        }

        public void EndPurchase(Purchase purch)
        {
            if (this.currentPurchase == purch)
            {
                this.currentPurchase = null;
            }
        }

        public List<Purchase> GetPending()
        {
            UnityEngine.Debug.Log("Resume pending");
            List<Purchase> list = new List<Purchase>();
            if (this.pending != null)
            {
                IEnumerator<MockPurchase> enumerator = this.pending.get_Values().GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        MockPurchase current = enumerator.Current;
                        list.Add(current);
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
            }
            return list;
        }

        [DebuggerHidden]
        public IEnumerator Initialize(string _refProduct)
        {
            <Initialize>c__Iterator20B iteratorb = new <Initialize>c__Iterator20B();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        public bool MergeProductData(PremiumProduct product)
        {
            char[] separator = new char[] { '.' };
            string[] strArray = product.flareProductId.Split(separator);
            string str = strArray[strArray.Length - 1];
            product.price = 0.99f;
            product.currencyCode = this.CurrencyCode;
            product.productName = str;
            product.productDescription = str;
            product.formattedPrice = product.price + " " + this.CurrencyCode;
            return true;
        }

        [DebuggerHidden]
        public IEnumerator RequestPlatformProducts(string[] prodIds)
        {
            <RequestPlatformProducts>c__Iterator20C iteratorc = new <RequestPlatformProducts>c__Iterator20C();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        [DebuggerHidden]
        public IEnumerator ResumePending(Purchase purch)
        {
            <ResumePending>c__Iterator20D iteratord = new <ResumePending>c__Iterator20D();
            iteratord.purch = purch;
            iteratord.<$>purch = purch;
            iteratord.<>f__this = this;
            return iteratord;
        }

        public static void SetFailed()
        {
            resultState = EPurchaseState.Failed;
        }

        public static void SetSuccess()
        {
            resultState = EPurchaseState.Success;
        }

        public void UnInitialize()
        {
        }

        public bool Available
        {
            get
            {
                return true;
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
        private sealed class <Initialize>c__Iterator20B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MockShopConnector <>f__this;

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
                        this.$current = new WaitForSeconds(1f);
                        this.$PC = 1;
                        return true;

                    case 1:
                        this.<>f__this.currentCurrency = MockShopConnector.currencies[MockShopConnector.currencyIndex];
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
        private sealed class <RequestPlatformProducts>c__Iterator20C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MockShopConnector <>f__this;

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
                        this.$current = new WaitForSeconds(1f);
                        this.$PC = 1;
                        return true;

                    case 1:
                        this.<>f__this.currentCurrency = MockShopConnector.currencies[MockShopConnector.currencyIndex];
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
        private sealed class <ResumePending>c__Iterator20D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Purchase <$>purch;
            internal MockShopConnector <>f__this;
            internal MockShopConnector.MockPurchase <mockPurch>__0;
            internal Purchase purch;

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
                        this.<mockPurch>__0 = (MockShopConnector.MockPurchase) this.purch;
                        this.<>f__this.currentPurchase = this.<mockPurch>__0;
                        this.$current = this.<>f__this.currentPurchase.DoValidate();
                        this.$PC = 1;
                        return true;

                    case 1:
                        if ((this.<mockPurch>__0.State == EPurchaseState.Validated) || (this.<mockPurch>__0.State == EPurchaseState.Commited))
                        {
                            this.<>f__this.pending.Remove(this.<mockPurch>__0.id);
                        }
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

        private class MockPurchase : Purchase
        {
            public MockShopConnector connector;
            public string id = Guid.NewGuid().ToString();

            public override void AbortPurchase()
            {
                this.connector.pending[this.id] = this;
                this.connector.currentPurchase = null;
            }

            public override ValidationRequest CreateValidationRequest()
            {
                string str = Guid.NewGuid().ToString();
                ValidationRequest request = new ValidationRequest();
                request.gameUserId = Binder.SessionData.UserId;
                request.isoCurrencyCode = this.connector.CurrencyCode;
                request.storeId = StoreId.shoptest.ToString();
                request.trackingUserId = Binder.SessionData.FgUserHandle;
                request.signature = "test-signature";
                MockShopConnector.MockReceipt dataObject = new MockShopConnector.MockReceipt();
                dataObject.storeProductId = base.product.storeProductId;
                dataObject.storeReceiptId = str;
                request.receipt = JsonUtils.Serialize(dataObject);
                return request;
            }

            public override IEnumerator DoCommit()
            {
                base.State = EPurchaseState.Commited;
                this.connector.EndPurchase(this);
                return null;
            }

            [DebuggerHidden]
            public override IEnumerator DoPurchase()
            {
                <DoPurchase>c__Iterator20E iteratore = new <DoPurchase>c__Iterator20E();
                iteratore.<>f__this = this;
                return iteratore;
            }

            [CompilerGenerated]
            private sealed class <DoPurchase>c__Iterator20E : IEnumerator, IDisposable, IEnumerator<object>
            {
                internal object $current;
                internal int $PC;
                internal MockShopConnector.MockPurchase <>f__this;
                internal int <k>__0;

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
                            this.<k>__0 = 0;
                            break;

                        case 1:
                            this.<k>__0++;
                            break;

                        default:
                            goto Label_0081;
                    }
                    if (this.<k>__0 < 60)
                    {
                        this.$current = null;
                        this.$PC = 1;
                        return true;
                    }
                    this.<>f__this.State = MockShopConnector.resultState;
                    UnityEngine.Debug.Log("success..");
                    goto Label_0081;
                    this.$PC = -1;
                Label_0081:
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

        public class MockReceipt
        {
            public string storeProductId;
            public string storeReceiptId;
        }
    }
}

