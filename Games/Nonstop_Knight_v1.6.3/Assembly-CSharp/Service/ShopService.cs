namespace Service
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ShopService
    {
        private Dictionary<string, string> m_placement = new Dictionary<string, string>();
        private Dictionary<string, PremiumProduct> m_products = new Dictionary<string, PremiumProduct>();
        private Dictionary<string, PremiumProduct> m_productsByStore = new Dictionary<string, PremiumProduct>();

        [DebuggerHidden]
        public virtual IEnumerator Acc(Purchase purchase)
        {
            <Acc>c__Iterator212 iterator = new <Acc>c__Iterator212();
            iterator.purchase = purchase;
            iterator.<$>purchase = purchase;
            return iterator;
        }

        private void AddTrackingPayload(Purchase purchase, string pathToShop)
        {
            Player player = GameLogic.Binder.GameState.Player;
            purchase.TrackingPayload.Add("source", !string.IsNullOrEmpty(pathToShop) ? pathToShop : PathToShopType.Vendor.ToString());
            purchase.TrackingPayload.Add("ascension_count", player.CumulativeRetiredHeroStats.HeroesRetired.ToString());
            purchase.TrackingPayload.Add("player_level", player.Rank.ToString());
            purchase.TrackingPayload.Add("floor", (player.getLastCompletedFloor(false) + 1).ToString());
            purchase.TrackingPayload.Add("gender", player.ActiveCharacter.isFemale().ToString());
        }

        [DebuggerHidden]
        private IEnumerator ExecutePurchase(Purchase purchase, PathToShopType pathToShop, PurchaseResultCallback cb)
        {
            <ExecutePurchase>c__Iterator214 iterator = new <ExecutePurchase>c__Iterator214();
            iterator.purchase = purchase;
            iterator.pathToShop = pathToShop;
            iterator.cb = cb;
            iterator.<$>purchase = purchase;
            iterator.<$>pathToShop = pathToShop;
            iterator.<$>cb = cb;
            iterator.<>f__this = this;
            return iterator;
        }

        public Dictionary<string, string> GetDefaultPlacement()
        {
            return this.m_placement;
        }

        public PremiumProduct GetProduct(string flaregamesProductId)
        {
            PremiumProduct product;
            UnityEngine.Debug.Log("ShopService::GetProduct " + this.m_products.Count);
            this.m_products.TryGetValue(flaregamesProductId, out product);
            return product;
        }

        public PremiumProduct GetProductByStoreProductId(string _storeProductId)
        {
            PremiumProduct product = null;
            this.m_productsByStore.TryGetValue(_storeProductId, out product);
            return product;
        }

        public virtual IEnumerable<PremiumProduct> GetProducts()
        {
            return this.m_products.Values;
        }

        [DebuggerHidden]
        public IEnumerator LoadProducts(string currency)
        {
            <LoadProducts>c__Iterator211 iterator = new <LoadProducts>c__Iterator211();
            iterator.currency = currency;
            iterator.<$>currency = currency;
            iterator.<>f__this = this;
            return iterator;
        }

        private static void Log(string str)
        {
            Service.Binder.Logger.Log(str);
        }

        private bool OnPurchaseException(Exception ex, PurchaseResultCallback cb)
        {
            UnityEngine.Debug.LogError(ex);
            cb(PurchaseResult.Fail);
            return false;
        }

        [DebuggerHidden]
        private IEnumerator ResumePending(PurchaseResultCallback cb)
        {
            <ResumePending>c__Iterator215 iterator = new <ResumePending>c__Iterator215();
            iterator.cb = cb;
            iterator.<$>cb = cb;
            return iterator;
        }

        public void ResumePendingPurchases(PurchaseResultCallback cb)
        {
            <ResumePendingPurchases>c__AnonStorey2F0 storeyf = new <ResumePendingPurchases>c__AnonStorey2F0();
            storeyf.cb = cb;
            storeyf.<>f__this = this;
            Service.Binder.TaskManager.StartTask(this.ResumePending(storeyf.cb), new TaskManager.ExceptionActionDelegate(storeyf.<>m__19E));
        }

        public void StartPurchase(Purchase purchase, PathToShopType pathToShop, PurchaseResultCallback cb)
        {
            <StartPurchase>c__AnonStorey2EF storeyef = new <StartPurchase>c__AnonStorey2EF();
            storeyef.cb = cb;
            storeyef.<>f__this = this;
            Service.Binder.TaskManager.StartTask(this.ExecutePurchase(purchase, pathToShop, storeyef.cb), new TaskManager.ExceptionActionDelegate(storeyef.<>m__19D));
        }

        [DebuggerHidden]
        public virtual IEnumerator Validate(Purchase purchase)
        {
            <Validate>c__Iterator213 iterator = new <Validate>c__Iterator213();
            iterator.purchase = purchase;
            iterator.<$>purchase = purchase;
            return iterator;
        }

        public bool ProductsAvailable
        {
            get
            {
                return (this.m_products.Count > 0);
            }
        }

        public string[] StoreProductIds
        {
            get
            {
                string[] strArray = new string[this.m_products.Count];
                int num = 0;
                foreach (PremiumProduct product in this.m_products.Values)
                {
                    strArray[num++] = product.storeProductId;
                }
                return strArray;
            }
        }

        [CompilerGenerated]
        private sealed class <Acc>c__Iterator212 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Purchase <$>purchase;
            internal ValidationAccRequest <accRequest>__1;
            internal ValidationRequest <receipt>__0;
            internal Request<UpdateResponse> <req>__2;
            internal Purchase purchase;

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
                    {
                        this.<receipt>__0 = this.purchase.CreateValidationRequest();
                        ValidationAccRequest request = new ValidationAccRequest();
                        request.receipt = this.<receipt>__0.receipt;
                        this.<accRequest>__1 = request;
                        this.<req>__2 = Request<UpdateResponse>.Post("/player/{sessionId}/accencash", this.<accRequest>__1);
                        this.$current = this.<req>__2.Task;
                        this.$PC = 1;
                        return true;
                    }
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
        private sealed class <ExecutePurchase>c__Iterator214 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ShopService.PurchaseResultCallback <$>cb;
            internal PathToShopType <$>pathToShop;
            internal Purchase <$>purchase;
            internal ShopService <>f__this;
            internal List<Reward> <rewards>__0;
            internal bool <showCeremony>__1;
            internal ShopService.PurchaseResultCallback cb;
            internal PathToShopType pathToShop;
            internal Purchase purchase;

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
                        this.$current = this.purchase.DoPurchase();
                        this.$PC = 1;
                        goto Label_0185;

                    case 1:
                        if (this.purchase.State != EPurchaseState.Success)
                        {
                            break;
                        }
                        this.<>f__this.AddTrackingPayload(this.purchase, this.pathToShop.ToString());
                        this.$current = this.purchase.DoValidate();
                        this.$PC = 2;
                        goto Label_0185;

                    case 2:
                        break;

                    case 3:
                        goto Label_00D3;

                    case 4:
                        goto Label_017C;

                    default:
                        goto Label_0183;
                }
                if ((this.purchase.State != EPurchaseState.Validated) && (this.purchase.State != EPurchaseState.Commited))
                {
                    if (this.purchase.State == EPurchaseState.Cancelled)
                    {
                        this.cb(PurchaseResult.Cancel);
                    }
                    else
                    {
                        this.cb(PurchaseResult.Fail);
                    }
                    goto Label_017C;
                }
            Label_00D3:
                while (!Service.Binder.ShopService.ProductsAvailable)
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0185;
                }
                this.<rewards>__0 = ConfigShops.InstantiateRewardsFromServerCommands(this.purchase.Update.InboxCommands);
                this.<showCeremony>__1 = this.pathToShop != PathToShopType.CardPack;
                RewardHelper.ClaimReward(this.<rewards>__0, this.<showCeremony>__1);
                this.cb(PurchaseResult.Success);
                this.$current = this.purchase.DoAcc();
                this.$PC = 4;
                goto Label_0185;
            Label_017C:
                this.$PC = -1;
            Label_0183:
                return false;
            Label_0185:
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

        [CompilerGenerated]
        private sealed class <LoadProducts>c__Iterator211 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal string <$>currency;
            internal List<PremiumProduct>.Enumerator <$s_482>__2;
            internal ShopService <>f__this;
            internal PremiumProduct <prod>__3;
            internal Request<PremiumShopContainer> <req>__1;
            internal string <storeId>__0;
            internal string currency;

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
                        this.<storeId>__0 = StoreId.gplay.ToString();
                        this.<req>__1 = Request<PremiumShopContainer>.Get("/player/{sessionId}/shop/products/" + this.currency + "/" + this.<storeId>__0);
                        this.$current = this.<req>__1.Task;
                        this.$PC = 1;
                        return true;

                    case 1:
                        if (this.<req>__1.Success)
                        {
                            this.<>f__this.m_products.Clear();
                            this.<$s_482>__2 = this.<req>__1.Result.products.GetEnumerator();
                            try
                            {
                                while (this.<$s_482>__2.MoveNext())
                                {
                                    this.<prod>__3 = this.<$s_482>__2.Current;
                                    this.<>f__this.m_products[this.<prod>__3.flareProductId] = this.<prod>__3;
                                    this.<>f__this.m_productsByStore[this.<prod>__3.storeProductId] = this.<prod>__3;
                                }
                            }
                            finally
                            {
                                this.<$s_482>__2.Dispose();
                            }
                            this.<>f__this.m_placement = this.<req>__1.Result.placement;
                            this.$PC = -1;
                            break;
                        }
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
        private sealed class <ResumePending>c__Iterator215 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ShopService.PurchaseResultCallback <$>cb;
            internal List<Purchase>.Enumerator <$s_483>__3;
            internal List<Reward>.Enumerator <$s_484>__6;
            internal Exception <e>__2;
            internal List<Purchase> <pending>__1;
            internal Purchase <purch>__4;
            internal List<Reward> <purchaseRewards>__5;
            internal Reward <reward>__7;
            internal List<Reward> <totalRewards>__0;
            internal ShopService.PurchaseResultCallback cb;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 2:
                    case 3:
                        try
                        {
                        }
                        finally
                        {
                            this.<$s_483>__3.Dispose();
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.<totalRewards>__0 = new List<Reward>();
                        break;

                    case 1:
                        break;

                    case 2:
                    case 3:
                        goto Label_00D0;

                    default:
                        goto Label_023E;
                }
                if (!Service.Binder.ShopService.ProductsAvailable)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_0240;
                }
                this.<pending>__1 = null;
                try
                {
                    this.<pending>__1 = Service.Binder.ShopManager.GetPending();
                }
                catch (Exception exception)
                {
                    this.<e>__2 = exception;
                    UnityEngine.Debug.Log("What? :" + this.<e>__2.ToString());
                }
                if (this.<pending>__1 == null)
                {
                    this.cb(PurchaseResult.Fail);
                    goto Label_023E;
                }
                this.<$s_483>__3 = this.<pending>__1.GetEnumerator();
                num = 0xfffffffd;
            Label_00D0:
                try
                {
                    switch (num)
                    {
                        case 2:
                            goto Label_011A;

                        case 3:
                            goto Label_015B;
                    }
                    while (this.<$s_483>__3.MoveNext())
                    {
                        this.<purch>__4 = this.<$s_483>__3.Current;
                        this.$current = Service.Binder.ShopManager.ResumePending(this.<purch>__4);
                        this.$PC = 2;
                        flag = true;
                        goto Label_0240;
                    Label_011A:
                        if ((this.<purch>__4.State != EPurchaseState.Validated) && (this.<purch>__4.State != EPurchaseState.Commited))
                        {
                            continue;
                        }
                        this.$current = this.<purch>__4.DoAcc();
                        this.$PC = 3;
                        flag = true;
                        goto Label_0240;
                    Label_015B:
                        this.<purchaseRewards>__5 = ConfigShops.InstantiateRewardsFromServerCommands(this.<purch>__4.Update.InboxCommands);
                        this.<$s_484>__6 = this.<purchaseRewards>__5.GetEnumerator();
                        try
                        {
                            while (this.<$s_484>__6.MoveNext())
                            {
                                this.<reward>__7 = this.<$s_484>__6.Current;
                                this.<totalRewards>__0.Add(this.<reward>__7);
                            }
                            continue;
                        }
                        finally
                        {
                            this.<$s_484>__6.Dispose();
                        }
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.<$s_483>__3.Dispose();
                }
                if (this.<totalRewards>__0.Count == 0)
                {
                    this.cb(PurchaseResult.Fail);
                }
                else
                {
                    RewardHelper.ClaimReward(this.<totalRewards>__0, true);
                    this.cb(PurchaseResult.Success);
                }
                this.$PC = -1;
            Label_023E:
                return false;
            Label_0240:
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

        [CompilerGenerated]
        private sealed class <ResumePendingPurchases>c__AnonStorey2F0
        {
            internal ShopService <>f__this;
            internal ShopService.PurchaseResultCallback cb;

            internal bool <>m__19E(Exception _ex)
            {
                return this.<>f__this.OnPurchaseException(_ex, this.cb);
            }
        }

        [CompilerGenerated]
        private sealed class <StartPurchase>c__AnonStorey2EF
        {
            internal ShopService <>f__this;
            internal ShopService.PurchaseResultCallback cb;

            internal bool <>m__19D(Exception _ex)
            {
                return this.<>f__this.OnPurchaseException(_ex, this.cb);
            }
        }

        [CompilerGenerated]
        private sealed class <Validate>c__Iterator213 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Purchase <$>purchase;
            internal ValidationRequest <receipt>__0;
            internal Request<UpdateResponse> <req>__1;
            internal Purchase purchase;

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
                        this.<receipt>__0 = this.purchase.CreateValidationRequest();
                        this.<req>__1 = Request<UpdateResponse>.Post("/player/{sessionId}/encash2", this.<receipt>__0);
                        this.$current = this.<req>__1.Task;
                        this.$PC = 1;
                        return true;

                    case 1:
                        if (!this.<req>__1.Success)
                        {
                            ShopService.Log("validation fail:" + this.<req>__1.ErrorMsg);
                            this.purchase.State = EPurchaseState.Pending;
                            break;
                        }
                        ShopService.Log("validation ok:" + this.<req>__1.ErrorMsg);
                        this.purchase.Update = this.<req>__1.Result;
                        this.purchase.State = EPurchaseState.Validated;
                        break;

                    default:
                        goto Label_00E3;
                }
                this.$PC = -1;
            Label_00E3:
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

        public delegate void PurchaseResultCallback(PurchaseResult result);
    }
}

