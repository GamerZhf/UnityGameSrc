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

    public class PromotionService : MonoBehaviour
    {
        [CompilerGenerated]
        private List<RemotePromotion> <Promotions>k__BackingField;
        private bool dontLoadPromotions;

        public PromotionService()
        {
            this.Promotions = new List<RemotePromotion>();
        }

        public void Initialize()
        {
            if (App.Binder.ConfigMeta.HIGH_FREQUENT_PROMOTION_UPDATES)
            {
                GameLogic.Binder.EventBus.OnFrenzyActivated += new GameLogic.Events.FrenzyActivated(this.onFrenzyActivated);
                GameLogic.Binder.EventBus.OnFrenzyDeactivated += new GameLogic.Events.FrenzyDeactivated(this.onFrenzyDeactivated);
                GameLogic.Binder.EventBus.OnBossTrainStarted += new GameLogic.Events.BossTrainStarted(this.onBossTrainStarted);
                GameLogic.Binder.EventBus.OnBossTrainEnded += new GameLogic.Events.BossTrainEnded(this.onBossTrainEnded);
                GameLogic.Binder.EventBus.OnPlayerRetired += new GameLogic.Events.PlayerRetired(this.onPlayerRetired);
                GameLogic.Binder.EventBus.OnRoomCompleted += new GameLogic.Events.RoomCompleted(this.onRoomCompleted);
            }
        }

        [DebuggerHidden]
        public IEnumerator LoadPromotions()
        {
            <LoadPromotions>c__Iterator22D iteratord = new <LoadPromotions>c__Iterator22D();
            iteratord.<>f__this = this;
            return iteratord;
        }

        private void onBossTrainEnded(Player player, int numcharges, int numbosseskilled)
        {
            this.dontLoadPromotions = false;
            this.ReloadPromotions();
        }

        private void onBossTrainStarted(Player player, int numcharges)
        {
            this.dontLoadPromotions = true;
        }

        private void onFrenzyActivated()
        {
            this.dontLoadPromotions = true;
        }

        private void onFrenzyDeactivated()
        {
            this.dontLoadPromotions = false;
            this.ReloadPromotions();
        }

        private void onPlayerRetired(Player player, int retirementfloor)
        {
            this.ReloadPromotions();
        }

        private void onRoomCompleted(Room room)
        {
            if ((GameLogic.Binder.GameState.Player.getLastCompletedFloor(false) % 15) == 0)
            {
                this.ReloadPromotions();
            }
        }

        private void ReloadPromotions()
        {
            if (!this.dontLoadPromotions)
            {
                Service.Binder.TaskManager.StartTask(Service.Binder.ContentService.LoadRemoteContent(), null);
                Service.Binder.TaskManager.StartTask(this.LoadPromotions(), null);
            }
        }

        private int SortByPriority(RemotePromotion a, RemotePromotion b)
        {
            return (b.ParsedCustomParams.Priority - a.ParsedCustomParams.Priority);
        }

        public void UnInitialize()
        {
            GameLogic.Binder.EventBus.OnFrenzyActivated -= new GameLogic.Events.FrenzyActivated(this.onFrenzyActivated);
            GameLogic.Binder.EventBus.OnFrenzyDeactivated -= new GameLogic.Events.FrenzyDeactivated(this.onFrenzyDeactivated);
            GameLogic.Binder.EventBus.OnBossTrainStarted -= new GameLogic.Events.BossTrainStarted(this.onBossTrainStarted);
            GameLogic.Binder.EventBus.OnBossTrainEnded -= new GameLogic.Events.BossTrainEnded(this.onBossTrainEnded);
            GameLogic.Binder.EventBus.OnPlayerRetired -= new GameLogic.Events.PlayerRetired(this.onPlayerRetired);
            GameLogic.Binder.EventBus.OnRoomCompleted -= new GameLogic.Events.RoomCompleted(this.onRoomCompleted);
            this.dontLoadPromotions = false;
        }

        public List<RemotePromotion> Promotions
        {
            [CompilerGenerated]
            get
            {
                return this.<Promotions>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Promotions>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <LoadPromotions>c__Iterator22D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal List<RemotePromotion>.Enumerator <$s_515>__4;
            internal PromotionService <>f__this;
            internal Dictionary<string, object> <overrideAttributes>__1;
            internal Player <player>__0;
            internal Dictionary<string, ulong> <promoStates>__3;
            internal RemotePromotion <promotion>__5;
            internal Request<PromotionResponse> <resp>__2;
            internal ulong <stateCode>__6;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<$s_515>__4.Dispose();
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
                    {
                        Service.Binder.Logger.Log("PromotionService::LoadPromotions()");
                        this.<player>__0 = GameLogic.Binder.GameState.Player;
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        dictionary.Add("player_level", this.<player>__0.Rank);
                        dictionary.Add("fb_connected", this.<player>__0.SocialData.FacebookId != null);
                        dictionary.Add("floor", this.<player>__0.getLastCompletedFloor(false));
                        dictionary.Add("ascension_count", this.<player>__0.CumulativeRetiredHeroStats.HeroesRetired);
                        this.<overrideAttributes>__1 = dictionary;
                        this.<resp>__2 = Request<PromotionResponse>.Post("/player/{sessionId}/promotions/", this.<overrideAttributes>__1);
                        this.$current = this.<resp>__2.Task;
                        this.$PC = 1;
                        goto Label_033D;
                    }
                    case 1:
                        if (!this.<resp>__2.Success)
                        {
                            Service.Binder.Logger.Log("failed " + this.<resp>__2.ErrorMsg);
                            goto Label_0334;
                        }
                        Service.Binder.Logger.Log("PromotionService::LoadPromotions() SUCCESS");
                        this.<>f__this.Promotions.Clear();
                        this.<promoStates>__3 = this.<player>__0.PromotionStates;
                        this.<$s_515>__4 = this.<resp>__2.Result.promotions.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 2:
                        break;

                    default:
                        goto Label_033B;
                }
                try
                {
                    switch (num)
                    {
                        case 2:
                            Service.Binder.PromotionManager.PostInitializePromotion(this.<promotion>__5);
                            this.<>f__this.Promotions.Add(this.<promotion>__5);
                            break;
                    }
                    while (this.<$s_515>__4.MoveNext())
                    {
                        this.<promotion>__5 = this.<$s_515>__4.Current;
                        this.<promotion>__5.InitAfterDataAvailable();
                        if (this.<promotion>__5.IsValid())
                        {
                            if (!this.<promoStates>__3.ContainsKey(this.<promotion>__5.promotionid))
                            {
                                Service.Binder.EventBus.PromotionAction(this.<promotion>__5.promotionid, "Delivered");
                            }
                            this.<stateCode>__6 = !this.<promoStates>__3.ContainsKey(this.<promotion>__5.promotionid) ? ((ulong) 0L) : this.<promoStates>__3[this.<promotion>__5.promotionid];
                            this.<promotion>__5.State = new PromotionState(this.<stateCode>__6);
                            if (this.<promotion>__5.HasTriggers())
                            {
                                this.<promotion>__5.State.WaitingForTrigger = true;
                            }
                            else
                            {
                                Service.Binder.PromotionManager.SetPromotionTimestampByActiveHours(this.<promotion>__5);
                            }
                            this.$current = this.<promotion>__5.ParsedCustomParams.LoadResources();
                            this.$PC = 2;
                            flag = true;
                            goto Label_033D;
                        }
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.<$s_515>__4.Dispose();
                }
                this.<>f__this.Promotions.Sort(new Comparison<RemotePromotion>(this.<>f__this.SortByPriority));
                CmdSavePlayerDataToPersistentStorage.ExecuteStatic(GameLogic.Binder.GameState.Player);
                Service.Binder.EventBus.PromotionsAvailable();
            Label_0334:
                this.$PC = -1;
            Label_033B:
                return false;
            Label_033D:
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
    }
}

