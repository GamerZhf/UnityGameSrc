namespace Service
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class PromotionEventSystem : MonoBehaviour, IPromotionEventSystem
    {
        private const string DEBUG_EVENT_PROMOTION_ID = "debug_event";
        private List<string> m_promotionsToEnd = new List<string>(1);
        private Dictionary<string, ConfigPromotionEvents.Event> m_remotePromotionEvents = new Dictionary<string, ConfigPromotionEvents.Event>();
        private List<RemotePromotion> m_remotePromotions = new List<RemotePromotion>(2);
        private const string REMOTE_CACHE_PATH = "cached_remote_promotion_events.json";

        private void addDebugEvent()
        {
            if (ConfigPromotionEvents.DEBUG_EVENT_ENABLED)
            {
                this.m_remotePromotionEvents.Add("debug_event", ConfigPromotionEvents.DEBUG_EVENT);
            }
        }

        private void clearDebugEvent()
        {
            GameLogic.Binder.GameState.Player.PromotionEvents.Instances.Remove("debug_event");
        }

        public ConfigPromotionEvents.Event getPromotionEventData(string promotionId)
        {
            return (!this.m_remotePromotionEvents.ContainsKey(promotionId) ? null : this.m_remotePromotionEvents[promotionId]);
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            Service.Binder.EventBus.OnPromotionsAvailable -= new Service.Events.PromotionsAvailable(this.onPromotionsAvailable);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            Service.Binder.EventBus.OnPromotionsAvailable += new Service.Events.PromotionsAvailable(this.onPromotionsAvailable);
        }

        private void onGameStateInitialized()
        {
            string json = IOUtil.LoadFromPersistentStorage("cached_remote_promotion_events.json");
            if (json != null)
            {
                this.m_remotePromotionEvents = JsonUtils.Deserialize<Dictionary<string, ConfigPromotionEvents.Event>>(json, true);
            }
            this.clearDebugEvent();
            this.addDebugEvent();
            this.refresh();
        }

        private void onPromotionsAvailable()
        {
            this.m_remotePromotionEvents.Clear();
            this.m_remotePromotions.Clear();
            Service.Binder.PromotionManager.GetEventPromotions(ref this.m_remotePromotions);
            for (int i = 0; i < this.m_remotePromotions.Count; i++)
            {
                RemotePromotion remotePromotion = this.m_remotePromotions[i];
                this.m_remotePromotionEvents.Add(remotePromotion.promotionid, new ConfigPromotionEvents.Event(remotePromotion));
            }
            IOUtil.SaveToPersistentStorage(JsonUtils.Serialize(this.m_remotePromotionEvents), "cached_remote_promotion_events.json", ConfigApp.PersistentStorageEncryptionEnabled, true);
            this.addDebugEvent();
            this.refresh();
        }

        private void refresh()
        {
            Player player = GameLogic.Binder.GameState.Player;
            foreach (KeyValuePair<string, ConfigPromotionEvents.Event> pair in this.m_remotePromotionEvents)
            {
                string key = pair.Key;
                ConfigPromotionEvents.Event data = pair.Value;
                if (!player.PromotionEvents.Instances.ContainsKey(key))
                {
                    CmdStartPromotionEvent.ExecuteStatic(player, key, data);
                }
                else
                {
                    CmdRefreshPromotionEvent.ExecuteStatic(player, key);
                }
            }
            this.m_promotionsToEnd.Clear();
            foreach (KeyValuePair<string, PromotionEventInstance> pair2 in player.PromotionEvents.Instances)
            {
                string str2 = pair2.Key;
                if (!this.m_remotePromotionEvents.ContainsKey(str2))
                {
                    this.m_promotionsToEnd.Add(str2);
                }
            }
            for (int i = 0; i < this.m_promotionsToEnd.Count; i++)
            {
                CmdEndPromotionEvent.ExecuteStatic(player, this.m_promotionsToEnd[i]);
            }
        }
    }
}

