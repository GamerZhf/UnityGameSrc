namespace Service
{
    using App;
    using GameLogic;
    using PlayerView;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using UnityEngine;

    public class PromotionManager
    {
        public const string DUNGEON_RELOAD_TRIGGER_ID = "dungeon_reload";

        public void CheckForPromotionGotActiveTrackingEvent(RemotePromotion promotion)
        {
            if (promotion.State.Active && !promotion.State.ActivatedOnce)
            {
                promotion.State.ActivatedOnce = true;
                Service.Binder.EventBus.PromotionAction(promotion.promotionid, "Activate");
                CmdSavePlayerDataToPersistentStorage.ExecuteStatic(GameLogic.Binder.GameState.Player);
            }
        }

        public void ConsumePromotion(RemotePromotion promotion, bool considerConsumeFlag)
        {
            if (!considerConsumeFlag || promotion.ParsedCustomParams.ConsumeOnAction)
            {
                promotion.State.Consumed = true;
                CmdSavePlayerDataToPersistentStorage.ExecuteStatic(GameLogic.Binder.GameState.Player);
            }
        }

        public List<ProductReward> GetCosts(RemotePromotion promotion)
        {
            RewardParams parsedCustomParams = promotion.ParsedCustomParams as RewardParams;
            if (parsedCustomParams == null)
            {
                return null;
            }
            return parsedCustomParams.Costs;
        }

        public void GetEventPromotions(ref List<RemotePromotion> output)
        {
            for (int i = 0; i < Service.Binder.PromotionService.Promotions.Count; i++)
            {
                RemotePromotion promotion = Service.Binder.PromotionService.Promotions[i];
                if (((promotion.PromotionType == EPromotionType.Event) && promotion.State.Active) && this.ValidatePlayerConditions(promotion))
                {
                    output.Add(promotion);
                }
            }
        }

        public string GetFlareProductIdForPromoSlot(string promoSlot)
        {
            List<RemotePromotion> promotions = Service.Binder.PromotionService.Promotions;
            if ((promotions != null) && (promotions.Count != 0))
            {
                foreach (RemotePromotion promotion in promotions)
                {
                    if ((promotion.PromotionType == EPromotionType.IapPlacement) && (this.GetRemainingTimeSeconds(promotion) != 0))
                    {
                        foreach (KeyValuePair<string, object> pair in promotion.customParams)
                        {
                            string str = RemotePromotion.FixPromoSlot(pair.Key);
                            if (promoSlot.Equals(str))
                            {
                                return pair.Value.ToString();
                            }
                        }
                    }
                }
            }
            return null;
        }

        public RemotePromotion GetNextPromotionVisibleInTaskPanel()
        {
            for (int i = 0; i < Service.Binder.PromotionService.Promotions.Count; i++)
            {
                RemotePromotion promotion = Service.Binder.PromotionService.Promotions[i];
                if (((promotion.IsValid() && (this.GetRemainingTimeSeconds(promotion) > 0L)) && (this.ValidatePlayerConditions(promotion) && promotion.ParsedCustomParams.ShowInTaskPanel)) && !promotion.State.TaskpanelShown)
                {
                    this.CheckForPromotionGotActiveTrackingEvent(promotion);
                    return promotion;
                }
            }
            return null;
        }

        public string GetProductReplacementId(string _origProductId)
        {
            List<RemotePromotion> promotions = Service.Binder.PromotionService.Promotions;
            if ((promotions != null) && (promotions.Count != 0))
            {
                foreach (RemotePromotion promotion in promotions)
                {
                    if (((promotion.promotionActionType == ActionType.premiumproduct) && (this.GetRemainingTimeSeconds(promotion) != 0)) && this.ValidatePlayerConditions(promotion))
                    {
                        foreach (PromotionPremiumProduct product in promotion.promotedPremiumProducts)
                        {
                            if (string.Equals(product.replaces, _origProductId))
                            {
                                return product.shopitemid;
                            }
                        }
                    }
                }
            }
            return _origProductId;
        }

        public RemotePromotion GetPromotion(string id, [Optional, DefaultParameterValue(0)] EPromotionType type)
        {
            if (!string.IsNullOrEmpty(id))
            {
                for (int i = 0; i < Service.Binder.PromotionService.Promotions.Count; i++)
                {
                    RemotePromotion promotion = Service.Binder.PromotionService.Promotions[i];
                    if (((type == EPromotionType.Unknown) || (promotion.PromotionType == type)) && (promotion.promotionid == id))
                    {
                        return promotion;
                    }
                }
            }
            return null;
        }

        public RemotePromotion GetPromotionForPromoSlot(string promoSlot)
        {
            List<RemotePromotion> promotions = Service.Binder.PromotionService.Promotions;
            if ((promotions != null) && (promotions.Count != 0))
            {
                foreach (RemotePromotion promotion in promotions)
                {
                    if ((this.GetRemainingTimeSeconds(promotion) != 0) && this.ValidatePlayerConditions(promotion))
                    {
                        this.CheckForPromotionGotActiveTrackingEvent(promotion);
                        if (promotion.PromotionType != EPromotionType.IapPlacement)
                        {
                            if (promoSlot.Equals(promotion.ParsedCustomParams.Placement))
                            {
                                return promotion;
                            }
                        }
                        else
                        {
                            foreach (KeyValuePair<string, object> pair in promotion.customParams)
                            {
                                string str = RemotePromotion.FixPromoSlot(pair.Key);
                                if (promoSlot.Equals(str))
                                {
                                    return promotion;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        public ulong GetRemainingTimeSeconds(RemotePromotion promotion)
        {
            if (!promotion.State.Active)
            {
                return 0L;
            }
            long gameTime = Service.Binder.ServerTime.GameTime;
            ulong timestamp = promotion.State.Timestamp;
            if (timestamp > 0L)
            {
                long num3 = ((long) timestamp) - gameTime;
                if (num3 < 0L)
                {
                    this.ConsumePromotion(promotion, false);
                }
                return (ulong) num3;
            }
            if (promotion.timing == null)
            {
                return 0x7fffffffffffffffL;
            }
            if (gameTime < promotion.timing.start)
            {
                return 0L;
            }
            return (promotion.timing.end - ((ulong) gameTime));
        }

        public List<Reward> GetRewards(RemotePromotion promotion)
        {
            RewardParams parsedCustomParams = promotion.ParsedCustomParams as RewardParams;
            if (parsedCustomParams == null)
            {
                return null;
            }
            List<Reward> list = new List<Reward>();
            foreach (ProductReward reward in parsedCustomParams.Rewards)
            {
                Reward rewardFromProductReward = ConfigShops.GetRewardFromProductReward(reward, null);
                list.Add(rewardFromProductReward);
            }
            return list;
        }

        public Texture2D GetShopIconReplacement()
        {
            foreach (RemotePromotion promotion in Service.Binder.PromotionService.Promotions)
            {
                if ((this.GetRemainingTimeSeconds(promotion) > 0L) && this.ValidatePlayerConditions(promotion))
                {
                    return promotion.ParsedCustomParams.ShopIcon;
                }
            }
            return null;
        }

        public string GetSlotForPromotion(RemotePromotion promotion)
        {
            if (promotion.ParsedCustomParams.Placement != null)
            {
                return promotion.ParsedCustomParams.Placement;
            }
            foreach (string str in ShopManager.ValidPromoSlots)
            {
                if (Service.Binder.PromotionManager.GetPromotionForPromoSlot(str) == promotion)
                {
                    return str;
                }
            }
            foreach (string str2 in ShopManager.ValidSecondaryPromoSlots)
            {
                if (Service.Binder.PromotionManager.GetPromotionForPromoSlot(str2) == promotion)
                {
                    return str2;
                }
            }
            return null;
        }

        [DebuggerHidden]
        public IEnumerable<KeyValuePair<string, string>> GetSlotReplacements(HashSet<string> slotSet)
        {
            <GetSlotReplacements>c__Iterator22C iteratorc = new <GetSlotReplacements>c__Iterator22C();
            iteratorc.slotSet = slotSet;
            iteratorc.<$>slotSet = slotSet;
            iteratorc.<>f__this = this;
            iteratorc.$PC = -2;
            return iteratorc;
        }

        public string GetTimeLeftFormatted(RemotePromotion promotion)
        {
            ulong remainingTimeSeconds = this.GetRemainingTimeSeconds(promotion);
            double hour = Math.Floor((((double) remainingTimeSeconds) / 60.0) / 60.0);
            double days = Math.Floor(hour / 24.0);
            if (hour > 72.0)
            {
                return _.T(ConfigLoca.PROMOTION_TIME_DAYS, new <>__AnonType25<double>(days));
            }
            double minute = (remainingTimeSeconds / ((ulong) 60L)) - (hour * 60.0);
            return _.T(ConfigLoca.PROMOTION_TIME_HOURS_MINUTES, new <>__AnonType26<double, double>(hour, minute));
        }

        public void Initialize()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.OnGameStateInitialized);
            GameLogic.Binder.EventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.OnGameplayStarted);
            GameLogic.Binder.EventBus.OnPromotionEventInspected += new GameLogic.Events.PromotionEventInspected(this.onPromotionEventInspected);
            Service.Binder.EventBus.OnTrackingEvent += new Service.Events.TrackingSendEvent(this.OnTrackingEvent);
        }

        public void InspectPromotion(RemotePromotion promotion)
        {
            if (!promotion.State.TaskpanelShown)
            {
                promotion.State.TaskpanelShown = true;
                CmdSavePlayerDataToPersistentStorage.ExecuteStatic(GameLogic.Binder.GameState.Player);
            }
        }

        private void OnGameplayStarted(ActiveDungeon activeDungeon)
        {
            if (!activeDungeon.SeamlessTransition)
            {
                this.ProcessTrigger("dungeon_reload");
            }
        }

        private void OnGameStateInitialized()
        {
            Dictionary<string, ulong> promotionStates = GameLogic.Binder.GameState.Player.PromotionStates;
            List<string> list = new List<string>(promotionStates.Keys);
            for (int i = 0; i < list.Count; i++)
            {
                PromotionState state = new PromotionState(promotionStates[list[i]]);
                state.DungeonReloaded = true;
                promotionStates[list[i]] = state.GetAsLong();
            }
        }

        private void onPromotionEventInspected(Player player, string promotionId)
        {
            RemotePromotion promotion = this.GetPromotion(promotionId, EPromotionType.Unknown);
            if (promotion != null)
            {
                this.InspectPromotion(promotion);
                this.CheckForPromotionGotActiveTrackingEvent(promotion);
            }
        }

        private void OnTrackingEvent(TrackingEvent tEvent)
        {
            this.ProcessTrigger(tEvent.EventName);
        }

        public void PostInitializePromotion(RemotePromotion promotion)
        {
            if (promotion.State.DungeonReloaded)
            {
                this.ProcessTrigger(promotion, "dungeon_reload");
            }
        }

        private void ProcessTrigger(string id)
        {
            List<RemotePromotion> promotions = Service.Binder.PromotionService.Promotions;
            if ((promotions != null) && (promotions.Count != 0))
            {
                foreach (RemotePromotion promotion in promotions)
                {
                    this.ProcessTrigger(promotion, id);
                }
            }
        }

        public void ProcessTrigger(RemotePromotion promotion, string id)
        {
            if (promotion.State.WaitingForTrigger && !promotion.State.TriggerSolved)
            {
                promotion.ProcessTrigger(id);
                if (!promotion.HasTriggers())
                {
                    this.StartTriggeredPromotion(promotion);
                }
            }
        }

        public void SetPromotionTimestampByActiveHours(RemotePromotion promotion)
        {
            if ((promotion.ParsedCustomParams.ActiveHours > 0) && (promotion.State.Timestamp == 0))
            {
                long num = (promotion.ParsedCustomParams.ActiveHours * 60L) * 60L;
                long gameTime = Service.Binder.ServerTime.GameTime;
                promotion.State.Timestamp = (ulong) (gameTime + num);
            }
        }

        public bool ShouldShowTimer(RemotePromotion promotion)
        {
            if (promotion.ParsedCustomParams.HideTimer)
            {
                return false;
            }
            return ((((this.GetRemainingTimeSeconds(promotion) / ((ulong) 60L)) / ((ulong) 60L)) / ((ulong) 0x18L)) < 0x1fL);
        }

        private void ShowDeeplinkPromotionPopup(RemotePromotion promotion)
        {
            PromoLoca parsedLoca = promotion.ParsedLoca;
            DeeplinkParams parsedCustomParams = promotion.ParsedCustomParams as DeeplinkParams;
            PromotionPopupContent.InputParameters parameters2 = new PromotionPopupContent.InputParameters();
            parameters2.Promotion = promotion;
            parameters2.DisposableBackgroundTexture = promotion.ParsedCustomParams.PopupImage;
            parameters2.Headline = parsedLoca.PopupHeadline;
            parameters2.Title = parsedLoca.PopupTitle;
            parameters2.Message = parsedLoca.PopupBody;
            parameters2.ButtonText = parsedLoca.PopupButton;
            parameters2.ButtonIcon = promotion.ParsedCustomParams.ButtonIcon;
            parameters2.DeeplinkUrl = parsedCustomParams.OpenURL;
            PromotionPopupContent.InputParameters parameter = parameters2;
            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.PromotionPopupContent, parameter, 0f, false, true);
        }

        private void ShowEventPromotionPopup(RemotePromotion promotion)
        {
            PlayerView.Binder.DungeonHud.onPromotionEventsButtonClicked();
        }

        private void ShowIAPPromotionPopup(RemotePromotion promotion, string slot)
        {
            if (!string.IsNullOrEmpty(Service.Binder.PromotionManager.GetFlareProductIdForPromoSlot(slot)))
            {
                PromoLoca parsedLoca = promotion.ParsedLoca;
                PromotionIAPPopupContent.InputParameters parameters2 = new PromotionIAPPopupContent.InputParameters();
                parameters2.Slot = slot;
                parameters2.Promotion = promotion;
                parameters2.DisposableBackgroundTexture = promotion.ParsedCustomParams.PopupImage;
                parameters2.Headline = parsedLoca.PopupHeadline;
                parameters2.Title = parsedLoca.PopupTitle;
                parameters2.Message = parsedLoca.PopupBody;
                PromotionIAPPopupContent.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.PromotionIAPPopupContent, parameter, 0f, false, true);
            }
        }

        private void ShowInfoPromotionPopup(RemotePromotion promotion)
        {
            PromoLoca parsedLoca = promotion.ParsedLoca;
            PromotionPopupContent.InputParameters parameters2 = new PromotionPopupContent.InputParameters();
            parameters2.Promotion = promotion;
            parameters2.DisposableBackgroundTexture = promotion.ParsedCustomParams.PopupImage;
            parameters2.Headline = parsedLoca.PopupHeadline;
            parameters2.Title = parsedLoca.PopupTitle;
            parameters2.Message = parsedLoca.PopupBody;
            PromotionPopupContent.InputParameters parameter = parameters2;
            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.PromotionPopupContent, parameter, 0f, false, true);
        }

        public void ShowPromotionPopup(RemotePromotion promotion)
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                switch (promotion.PromotionType)
                {
                    case EPromotionType.IapPlacement:
                        this.ShowIAPPromotionPopup(promotion, this.GetSlotForPromotion(promotion));
                        break;

                    case EPromotionType.Reward:
                        this.ShowRewardPromotionPopup(promotion);
                        break;

                    case EPromotionType.Deeplink:
                        this.ShowDeeplinkPromotionPopup(promotion);
                        break;

                    case EPromotionType.Info:
                        this.ShowInfoPromotionPopup(promotion);
                        break;

                    case EPromotionType.Event:
                        this.ShowEventPromotionPopup(promotion);
                        break;
                }
                this.InspectPromotion(promotion);
            }
        }

        private void ShowRewardPromotionPopup(RemotePromotion promotion)
        {
            List<ProductReward> costs = Service.Binder.PromotionManager.GetCosts(promotion);
            PromoLoca parsedLoca = promotion.ParsedLoca;
            PromotionPopupContent.InputParameters parameters2 = new PromotionPopupContent.InputParameters();
            parameters2.Promotion = promotion;
            parameters2.DisposableBackgroundTexture = promotion.ParsedCustomParams.PopupImage;
            parameters2.Headline = parsedLoca.PopupHeadline;
            parameters2.Title = parsedLoca.PopupTitle;
            parameters2.Message = parsedLoca.PopupBody;
            PromotionPopupContent.InputParameters parameter = parameters2;
            if ((costs != null) && (costs.Count > 0))
            {
                parameter.CostAmount = costs[0].amount;
                parameter.CostResourceType = costs[0].key;
            }
            else
            {
                parameter.ButtonText = parsedLoca.PopupButton;
                parameter.ButtonIcon = promotion.ParsedCustomParams.ButtonIcon;
            }
            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.PromotionPopupContent, parameter, 0f, false, true);
        }

        private void StartTriggeredPromotion(RemotePromotion promotion)
        {
            promotion.State.TriggerSolved = true;
            this.SetPromotionTimestampByActiveHours(promotion);
            CmdSavePlayerDataToPersistentStorage.ExecuteStatic(GameLogic.Binder.GameState.Player);
            Service.Binder.EventBus.PromotionsAvailable();
        }

        public void UnInitialize()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.OnGameStateInitialized);
            GameLogic.Binder.EventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.OnGameplayStarted);
            GameLogic.Binder.EventBus.OnPromotionEventInspected -= new GameLogic.Events.PromotionEventInspected(this.onPromotionEventInspected);
            Service.Binder.EventBus.OnTrackingEvent -= new Service.Events.TrackingSendEvent(this.OnTrackingEvent);
        }

        private bool ValidatePlayerConditions(RemotePromotion promotion)
        {
            foreach (KeyValuePair<string, object> pair in promotion.ParsedCustomParams.PlayerConditions)
            {
                FieldInfo field = typeof(Player).GetField(pair.Key);
                if ((field != null) && !field.GetValue(GameLogic.Binder.GameState.Player).Equals(pair.Value))
                {
                    return false;
                }
            }
            return true;
        }

        [CompilerGenerated]
        private sealed class <GetSlotReplacements>c__Iterator22C : IEnumerator, IDisposable, IEnumerable, IEnumerable<KeyValuePair<string, string>>, IEnumerator<KeyValuePair<string, string>>
        {
            internal KeyValuePair<string, string> $current;
            internal int $PC;
            internal HashSet<string> <$>slotSet;
            internal List<RemotePromotion>.Enumerator <$s_508>__1;
            internal Dictionary<string, object>.Enumerator <$s_509>__3;
            internal PromotionManager <>f__this;
            internal KeyValuePair<string, object> <kv>__4;
            internal List<RemotePromotion> <promotions>__0;
            internal RemotePromotion <remotePromotion>__2;
            internal string <slot>__5;
            internal HashSet<string> slotSet;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                            try
                            {
                            }
                            finally
                            {
                                this.<$s_509>__3.Dispose();
                            }
                        }
                        finally
                        {
                            this.<$s_508>__1.Dispose();
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
                        this.<promotions>__0 = Service.Binder.PromotionService.Promotions;
                        if ((this.<promotions>__0 != null) && (this.<promotions>__0.Count != 0))
                        {
                            this.<$s_508>__1 = this.<promotions>__0.GetEnumerator();
                            num = 0xfffffffd;
                            break;
                        }
                        goto Label_01CA;

                    case 1:
                        break;

                    default:
                        goto Label_01CA;
                }
                try
                {
                    switch (num)
                    {
                        case 1:
                            goto Label_00EE;
                    }
                    while (this.<$s_508>__1.MoveNext())
                    {
                        this.<remotePromotion>__2 = this.<$s_508>__1.Current;
                        if (((this.<remotePromotion>__2.PromotionType != EPromotionType.IapPlacement) || (this.<>f__this.GetRemainingTimeSeconds(this.<remotePromotion>__2) == 0)) || !this.<>f__this.ValidatePlayerConditions(this.<remotePromotion>__2))
                        {
                            continue;
                        }
                        this.<$s_509>__3 = this.<remotePromotion>__2.customParams.GetEnumerator();
                        num = 0xfffffffd;
                    Label_00EE:
                        try
                        {
                            while (this.<$s_509>__3.MoveNext())
                            {
                                this.<kv>__4 = this.<$s_509>__3.Current;
                                this.<slot>__5 = this.<kv>__4.Key.Replace('_', '.');
                                if (this.slotSet.Contains(this.<slot>__5))
                                {
                                    this.$current = new KeyValuePair<string, string>(this.<slot>__5, this.<kv>__4.Value.ToString());
                                    this.$PC = 1;
                                    flag = true;
                                    return true;
                                }
                            }
                            continue;
                        }
                        finally
                        {
                            if (!flag)
                            {
                            }
                            this.<$s_509>__3.Dispose();
                        }
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.<$s_508>__1.Dispose();
                }
                this.$PC = -1;
            Label_01CA:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                PromotionManager.<GetSlotReplacements>c__Iterator22C iteratorc = new PromotionManager.<GetSlotReplacements>c__Iterator22C();
                iteratorc.<>f__this = this.<>f__this;
                iteratorc.slotSet = this.<$>slotSet;
                return iteratorc;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string,string>>.GetEnumerator();
            }

            KeyValuePair<string, string> IEnumerator<KeyValuePair<string, string>>.Current
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

