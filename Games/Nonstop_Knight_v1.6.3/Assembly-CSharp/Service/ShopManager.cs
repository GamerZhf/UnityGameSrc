namespace Service
{
    using App;
    using GameLogic;
    using PlayerView;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class ShopManager
    {
        [CompilerGenerated]
        private bool <PendingPopupShowed>k__BackingField;
        private IPlatformShopConnector connector = new AndroidShopConnector();
        private const string REF_PRODUCT_ID = "com.koplagames.kopla01.diamondssmall";
        private Dictionary<string, ShopEntry> ShopEntriesByProduct = new Dictionary<string, ShopEntry>();
        private Dictionary<string, ShopEntry> ShopEntriesBySlot = new Dictionary<string, ShopEntry>();
        private static HashSet<string> slotSet;
        private ShopManagerState state = ShopManagerState.Initializing;
        public static string[] ValidBundleSlots = new string[] { "bundle.01" };
        public static string[] ValidPlacementSlots = new string[] { "gemshop.01", "gemshop.02", "gemshop.03", "gemshop.04", "gemshop.05", "gemshop.06" };
        public static string[] ValidPromoSlots = new string[] { "promo.01", "promo.02", "promo.03" };
        public static string[] ValidSecondaryPromoSlots = new string[] { "promo.04", "promo.05", "promo.06" };
        public static string[] ValidShopOfferSlots = new string[] { "shopoffer.01" };

        private void ChangeState(ShopManagerState state)
        {
            this.state = state;
            Service.Binder.EventBus.IapShopStateChanged();
        }

        public Purchase CreatePurchase(string flaregamesProductsId)
        {
            PremiumProduct product = Service.Binder.ShopService.GetProduct(flaregamesProductsId);
            if (product == null)
            {
                throw new InvalidOperationException("no such product:" + flaregamesProductsId);
            }
            return this.connector.CreatePurchase(product);
        }

        private void CreateShopEntry(PremiumProduct prod)
        {
            if (this.connector.MergeProductData(prod))
            {
                ShopEntry entry3;
                if (prod.rewards.Count == 1)
                {
                    entry3 = new ShopEntry();
                    entry3.Id = prod.flareProductId;
                    entry3.Type = ShopEntryType.IapDiamonds;
                    entry3.CostAmount = prod.price;
                    entry3.Title = prod.productName;
                    entry3.Sprite = new SpriteAtlasEntry("Menu", prod.GetIcon());
                    entry3.FormattedPrice = prod.formattedPrice;
                    ShopEntry entry = entry3;
                    entry.initBuyResourceAmounts(prod);
                    if (entry.BuyResourceAmounts.ContainsKey(ResourceType.Diamond))
                    {
                        entry.NumBursts = MenuHelpers.CalculateNumBurstsForDiamondPurchase(entry.BuyResourceAmounts[ResourceType.Diamond]);
                    }
                    this.ShopEntriesByProduct.Add(prod.flareProductId, entry);
                }
                else
                {
                    string icon = prod.GetIcon();
                    entry3 = new ShopEntry();
                    entry3.Id = prod.flareProductId;
                    entry3.Type = ShopEntryType.IapStarterBundle;
                    entry3.CostAmount = prod.price;
                    entry3.Title = prod.productName;
                    entry3.FormattedPrice = prod.formattedPrice;
                    ShopEntry entry2 = entry3;
                    if (!string.IsNullOrEmpty(icon))
                    {
                        entry2.Sprite = new SpriteAtlasEntry("Menu", icon);
                    }
                    entry2.initBuyResourceAmounts(prod);
                    this.ShopEntriesByProduct.Add(prod.flareProductId, entry2);
                }
            }
        }

        public List<Purchase> GetPending()
        {
            return this.connector.GetPending();
        }

        public ShopEntry GetShopEntryByFlareProductId(string flareProdId)
        {
            ShopEntry entry = null;
            this.ShopEntriesByProduct.TryGetValue(flareProdId, out entry);
            return entry;
        }

        public ShopEntry GetShopEntryById(string shopEntryId)
        {
            foreach (ShopEntry entry in this.ShopEntriesByProduct.Values)
            {
                if (entry.Id == shopEntryId)
                {
                    return entry;
                }
            }
            return null;
        }

        public ShopEntry GetShopEntryByPlatformId(string platformProdId)
        {
            PremiumProduct productByStoreProductId = Service.Binder.ShopService.GetProductByStoreProductId(platformProdId);
            ShopEntry entry = null;
            if (productByStoreProductId != null)
            {
                this.ShopEntriesByProduct.TryGetValue(productByStoreProductId.flareProductId, out entry);
            }
            return entry;
        }

        public ShopEntry GetShopEntryBySlot(string slot)
        {
            ShopEntry entry = null;
            this.ShopEntriesBySlot.TryGetValue(slot, out entry);
            return entry;
        }

        [DebuggerHidden]
        public IEnumerator Initialize()
        {
            <Initialize>c__Iterator20F iteratorf = new <Initialize>c__Iterator20F();
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        private void InitializeProducts()
        {
            ShopService shopService = Service.Binder.ShopService;
            PromotionManager promotionManager = Service.Binder.PromotionManager;
            this.ShopEntriesByProduct.Clear();
            this.ShopEntriesBySlot.Clear();
            IEnumerator<PremiumProduct> enumerator = shopService.GetProducts().GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    PremiumProduct current = enumerator.Current;
                    this.CreateShopEntry(current);
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            foreach (KeyValuePair<string, string> pair in shopService.GetDefaultPlacement())
            {
                ShopEntry entry = null;
                string productReplacementId = promotionManager.GetProductReplacementId(pair.Value);
                if (this.ShopEntriesByProduct.TryGetValue(productReplacementId, out entry))
                {
                    this.ShopEntriesBySlot[pair.Key] = entry;
                }
            }
            IEnumerator<KeyValuePair<string, string>> enumerator3 = promotionManager.GetSlotReplacements(SlotSet).GetEnumerator();
            try
            {
                while (enumerator3.MoveNext())
                {
                    KeyValuePair<string, string> pair2 = enumerator3.Current;
                    ShopEntry entry2 = null;
                    string key = promotionManager.GetProductReplacementId(pair2.Value);
                    if (this.ShopEntriesByProduct.TryGetValue(key, out entry2))
                    {
                        this.ShopEntriesBySlot[pair2.Key] = entry2;
                    }
                    else
                    {
                        this.ShopEntriesBySlot.Remove(pair2.Key);
                    }
                }
            }
            finally
            {
                if (enumerator3 == null)
                {
                }
                enumerator3.Dispose();
            }
            foreach (string str3 in ValidShopOfferSlots)
            {
                if (!this.ShopEntriesBySlot.ContainsKey(str3))
                {
                    foreach (string str4 in ValidPlacementSlots)
                    {
                        if (this.ShopEntriesBySlot.ContainsKey(str4))
                        {
                            this.ShopEntriesBySlot[str3] = this.ShopEntriesBySlot[str4];
                            break;
                        }
                    }
                }
            }
        }

        public bool IsPurchasing()
        {
            return ((this.State == ShopManagerState.Purchasing) || (this.State == ShopManagerState.Validating));
        }

        private static void Log(string str)
        {
            Service.Binder.Logger.Log(str);
        }

        public bool OnInitializeError(Exception ex)
        {
            this.ChangeState(ShopManagerState.Unavailable);
            return false;
        }

        private void OnPromotionsAvailable()
        {
            this.InitializeProducts();
        }

        public void RefreshProducts()
        {
            if ((this.state == ShopManagerState.Available) || (this.state == ShopManagerState.Initializing))
            {
                Service.Binder.TaskManager.StartTask(this.RunRefreshProducts(false), null);
            }
        }

        public IEnumerator ResumePending(Purchase purchase)
        {
            return this.connector.ResumePending(purchase);
        }

        [DebuggerHidden]
        private IEnumerator RunRefreshProducts(bool forceNotify)
        {
            <RunRefreshProducts>c__Iterator210 iterator = new <RunRefreshProducts>c__Iterator210();
            iterator.forceNotify = forceNotify;
            iterator.<$>forceNotify = forceNotify;
            iterator.<>f__this = this;
            return iterator;
        }

        public bool StarterBundleAvailable()
        {
            return (ValidBundleSlots.Length > 0);
        }

        public bool StarterBundleVisible()
        {
            if (!this.StarterBundleAvailable())
            {
                return false;
            }
            Player player = GameLogic.Binder.GameState.Player;
            string slot = ValidBundleSlots[0];
            return (((App.Binder.ConfigMeta.STARTER_BUNDLE_SELLING_ENABLED && !player.HasPurchasedStarterBundle) && player.hasRetired()) && (this.GetShopEntryBySlot(slot) != null));
        }

        public void UnInitialize()
        {
            this.connector.UnInitialize();
            Service.Binder.EventBus.OnPromotionsAvailable -= new Service.Events.PromotionsAvailable(this.OnPromotionsAvailable);
        }

        public int PendingCount
        {
            get
            {
                return this.connector.PendingCount;
            }
        }

        public bool PendingPopupShowed
        {
            [CompilerGenerated]
            get
            {
                return this.<PendingPopupShowed>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<PendingPopupShowed>k__BackingField = value;
            }
        }

        public static HashSet<string> SlotSet
        {
            get
            {
                if (slotSet == null)
                {
                    slotSet = new HashSet<string>();
                    foreach (string str in ValidBundleSlots)
                    {
                        slotSet.Add(str);
                    }
                    foreach (string str2 in ValidPlacementSlots)
                    {
                        slotSet.Add(str2);
                    }
                    foreach (string str3 in ValidShopOfferSlots)
                    {
                        slotSet.Add(str3);
                    }
                    foreach (string str4 in ValidPromoSlots)
                    {
                        slotSet.Add(str4);
                    }
                    foreach (string str5 in ValidSecondaryPromoSlots)
                    {
                        slotSet.Add(str5);
                    }
                }
                return slotSet;
            }
        }

        public ShopManagerState State
        {
            get
            {
                if ((this.state != ShopManagerState.Unavailable) && (this.state != ShopManagerState.Initializing))
                {
                    return this.connector.State;
                }
                return this.state;
            }
        }

        [CompilerGenerated]
        private sealed class <Initialize>c__Iterator20F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ShopManager <>f__this;
            internal ShopService <shop>__0;

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
                        ShopManager.Log("Initialize shop");
                        this.<>f__this.ChangeState(ShopManagerState.Initializing);
                        this.<>f__this.ShopEntriesByProduct.Clear();
                        this.<>f__this.ShopEntriesBySlot.Clear();
                        this.<shop>__0 = Service.Binder.ShopService;
                        this.$current = this.<>f__this.connector.Initialize("com.koplagames.kopla01.diamondssmall");
                        this.$PC = 1;
                        goto Label_0187;

                    case 1:
                        if (this.<>f__this.connector.Available)
                        {
                            this.$current = this.<shop>__0.LoadProducts(this.<>f__this.connector.CurrencyCode);
                            this.$PC = 2;
                            goto Label_0187;
                        }
                        this.<>f__this.ChangeState(ShopManagerState.Unavailable);
                        ShopManager.Log("platform shop not available");
                        break;

                    case 2:
                        if (this.<shop>__0.ProductsAvailable)
                        {
                            this.$current = this.<>f__this.RunRefreshProducts(true);
                            this.$PC = 3;
                            goto Label_0187;
                        }
                        this.<>f__this.ChangeState(ShopManagerState.Unavailable);
                        ShopManager.Log("shop service not available");
                        break;

                    case 3:
                        this.<>f__this.ChangeState(ShopManagerState.Available);
                        Service.Binder.EventBus.IapShopInitialized();
                        Service.Binder.EventBus.OnPromotionsAvailable += new Service.Events.PromotionsAvailable(this.<>f__this.OnPromotionsAvailable);
                        this.<>f__this.PendingPopupShowed = false;
                        ShopManager.Log("Shop is available now");
                        this.$PC = -1;
                        break;
                }
                return false;
            Label_0187:
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
        private sealed class <RunRefreshProducts>c__Iterator210 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>forceNotify;
            internal ShopManager <>f__this;
            internal string <lastCurrency>__0;
            internal ShopService <shop>__1;
            internal bool forceNotify;

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
                        this.<lastCurrency>__0 = this.<>f__this.connector.CurrencyCode;
                        this.<shop>__1 = Service.Binder.ShopService;
                        this.$current = this.<>f__this.connector.RequestPlatformProducts(this.<shop>__1.StoreProductIds);
                        this.$PC = 1;
                        return true;

                    case 1:
                        this.<>f__this.InitializeProducts();
                        if ((this.<lastCurrency>__0 != this.<>f__this.connector.CurrencyCode) || this.forceNotify)
                        {
                            Service.Binder.EventBus.IapShopProductsUpdated();
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
    }
}

