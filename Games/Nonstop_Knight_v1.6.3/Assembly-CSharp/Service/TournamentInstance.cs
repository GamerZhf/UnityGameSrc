namespace Service
{
    using App;
    using GameLogic;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TournamentInstance : IJsonData
    {
        [CompilerGenerated]
        private GameLogic.Player <Player>k__BackingField;
        public State CurrentState;
        public int DailyAdCountCards;
        public int DailyAdCountMystery;
        public bool EntryAnnounced;
        public GameLogic.HeroStats HeroStats;
        public int HighestClaimedMilestoneThreshold;
        public GameLogic.Inventory Inventory;
        public List<ItemSlot> ItemSlots;
        public int LastAdventurePanelSubTabIdx;
        public bool LastBossEncounterFailed;
        public long LastCardPackAdViewTimestamp;
        public int LastCompletedFloor;
        private Service.TournamentView m_tournamentView;
        public int MinionsKilledSinceLastRoomCompletion;
        public int NumDonationsMade;
        public int NumMilestonesClaimed;
        public List<TournamentLogEvent> OutgoingLogQueue;
        public Dictionary<string, double> Resources;
        public string TournamentId;
        public TournamentUpgrades Upgrades;
        public int WildBossesKilledSinceLastRoomCompletion;
        public int WildBossesKilledTotal;
        public float WildBossSpawnChance;

        public TournamentInstance()
        {
            this.OutgoingLogQueue = new List<TournamentLogEvent>();
            this.HeroStats = new GameLogic.HeroStats();
            this.Resources = new Dictionary<string, double>();
            this.Upgrades = new TournamentUpgrades();
            this.LastAdventurePanelSubTabIdx = -1;
        }

        public TournamentInstance(TournamentInfo tournamentInfo, GameLogic.Player player)
        {
            this.OutgoingLogQueue = new List<TournamentLogEvent>();
            this.HeroStats = new GameLogic.HeroStats();
            this.Resources = new Dictionary<string, double>();
            this.Upgrades = new TournamentUpgrades();
            this.LastAdventurePanelSubTabIdx = -1;
            this.TournamentId = tournamentInfo.Id;
            this.Player = player;
            this.CurrentState = State.PENDING_JOIN_CONFIRMATION;
        }

        public bool cardPackGetAllAdViewAllowed()
        {
            if (!ConfigTournaments.TOURNAMENT_GET_ALL_AD_VIEWING_ENABLED)
            {
                return false;
            }
            if (this.DailyAdCountCards >= App.Binder.ConfigMeta.DAILY_ADS_LIMIT_TOURNAMENT_CARDS)
            {
                return false;
            }
            long num = Service.Binder.ServerTime.GameTime - this.LastCardPackAdViewTimestamp;
            return (num > (ConfigTournaments.TOURNAMENT_CARD_PACK_AD_VIEW_COOLDOWN_MINUTES * 60L));
        }

        public void ClearLogEvents(List<TournamentLogEvent> events)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < events.Count; i++)
            {
                list.Add(events[i].Id);
            }
            List<TournamentLogEvent> list2 = new List<TournamentLogEvent>();
            for (int j = 0; j < this.OutgoingLogQueue.Count; j++)
            {
                if (!list.Contains(this.OutgoingLogQueue[j].Id))
                {
                    list2.Add(this.OutgoingLogQueue[j]);
                }
            }
            this.OutgoingLogQueue = list2;
        }

        public int GetContribution()
        {
            return this.HeroStats.BossesBeat;
        }

        public double getDonationPrice()
        {
            return App.Binder.ConfigMeta.TOURNAMENT_DONATION_PRICE;
        }

        public int getDonationsRemaining()
        {
            if (ConfigApp.CHEAT_UNLIMITED_TOURNAMENT_DONATES)
            {
                return 0x10;
            }
            return Mathf.Max(App.Binder.ConfigMeta.TOURNAMENT_MAX_DONATIONS_PER_PLAYER_PER_TOUNAMENT - this.NumDonationsMade, 0);
        }

        public int getWildBossSummonsRemaining()
        {
            int num = ConfigTournaments.TOURNAMENT_WILD_BOSS_CAP_BASE + (this.NumMilestonesClaimed * ConfigTournaments.TOURNAMENT_WILD_BOSS_CAP_INCREASE_PER_COMPLETED_MILESTONE);
            return Mathf.Max(num - this.WildBossesKilledTotal, 0);
        }

        public void postDeserializeInitialization()
        {
            this.Upgrades.Player = this.Player;
            this.Upgrades.postDeserializeInitialization();
            IEnumerator enumerator = Enum.GetValues(typeof(ResourceType)).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ResourceType current = (ResourceType) ((int) enumerator.Current);
                    if (!this.Resources.ContainsKey(current.ToString()))
                    {
                        this.Resources.Add(current.ToString(), 0.0);
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            if ((this.Inventory == null) || (this.ItemSlots == null))
            {
                GameLogic.Player player = JsonUtils.Deserialize<GameLogic.Player>(ResourceUtil.LoadSafe<TextAsset>("Players/humanPlayer1", false).text, true);
                if (this.ItemSlots == null)
                {
                    this.ItemSlots = player.ActiveCharacter.ItemSlots;
                }
                if (this.Inventory == null)
                {
                    this.Inventory = player.ActiveCharacter.Inventory;
                }
            }
            IEnumerator enumerator2 = Enum.GetValues(typeof(ItemType)).GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    ItemType type2 = (ItemType) ((int) enumerator2.Current);
                    if (type2 != ItemType.UNSPECIFIED)
                    {
                        bool flag = false;
                        for (int n = 0; n < this.ItemSlots.Count; n++)
                        {
                            if (this.ItemSlots[n].CompatibleItemType == type2)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            ItemSlot item = new ItemSlot();
                            item.CompatibleItemType = type2;
                            this.ItemSlots.Add(item);
                        }
                    }
                }
            }
            finally
            {
                IDisposable disposable2 = enumerator2 as IDisposable;
                if (disposable2 == null)
                {
                }
                disposable2.Dispose();
            }
            this.Inventory.postDeserializeInitialization();
            for (int i = 0; i < this.ItemSlots.Count; i++)
            {
                this.ItemSlots[i].postDeserializeInitialization();
            }
            for (int j = 0; j < this.ItemSlots.Count; j++)
            {
                if (this.ItemSlots[j].ItemInstance != null)
                {
                    this.ItemSlots[j].ItemInstance.enforcePerkLegality(this.Player);
                }
            }
            for (int k = 0; k < this.Inventory.ItemInstances.Count; k++)
            {
                this.Inventory.ItemInstances[k].enforcePerkLegality(this.Player);
            }
            for (int m = 0; m < this.ItemSlots.Count; m++)
            {
                ItemInstance itemInstance = this.ItemSlots[m].ItemInstance;
                if (itemInstance != null)
                {
                    itemInstance.InspectedByPlayer = true;
                }
            }
            this.WildBossSpawnChance = ConfigTournaments.TOURNAMENT_WILD_BOSS_SPAWN_CHANCE_BASE;
        }

        [JsonIgnore]
        public double DifficultyModifier
        {
            get
            {
                if (((this.TournamentView != null) && (this.TournamentView.TournamentInfo != null)) && (this.TournamentView.TournamentInfo.DifficultyModifier > 0.0))
                {
                    return this.TournamentView.TournamentInfo.DifficultyModifier;
                }
                return 1.0;
            }
        }

        [JsonIgnore]
        public GameLogic.Player Player
        {
            [CompilerGenerated]
            get
            {
                return this.<Player>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Player>k__BackingField = value;
            }
        }

        [JsonIgnore]
        public Service.TournamentView TournamentView
        {
            get
            {
                if (this.m_tournamentView == null)
                {
                    this.m_tournamentView = Service.Binder.TournamentSystem.GetTournamentView(this.TournamentId);
                }
                return this.m_tournamentView;
            }
        }

        public enum State
        {
            PENDING_JOIN_CONFIRMATION,
            ERROR_JOIN_TOO_EARLY,
            ERROR_JOIN_TOO_LATE,
            ERROR_EXPIRED,
            ACTIVE,
            PENDING_END_ANNOUNCEMENT,
            CLEARED_FOR_REMOVAL
        }
    }
}

