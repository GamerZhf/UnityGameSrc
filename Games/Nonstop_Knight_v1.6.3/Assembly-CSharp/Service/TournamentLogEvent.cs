namespace Service
{
    using App;
    using GameLogic;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TournamentLogEvent
    {
        [CompilerGenerated]
        private string <ContributorIconIdentifier>k__BackingField;
        [CompilerGenerated]
        private string <IconIdentifier>k__BackingField;
        [CompilerGenerated]
        private string <LogDisplayText>k__BackingField;
        [CompilerGenerated]
        private string <TickerLeftText>k__BackingField;
        [CompilerGenerated]
        private string <TickerRightText>k__BackingField;
        public string AdditionalData;
        public string Id;
        public string PlayerId;
        public LogEventType Type;

        public void PrepareForDisplay(TournamentView view)
        {
            TournamentEntry tournamentEntry = view.TournamentEntries.Find(delegate (TournamentEntry e) {
                return e.PlayerId == this.PlayerId;
            });
            string playerName = (tournamentEntry == null) ? "A party member" : tournamentEntry.PlayerDisplayName;
            if (tournamentEntry != null)
            {
                switch (view.getLeaderboardRanking(tournamentEntry))
                {
                    case 0:
                        this.ContributorIconIdentifier = "icon_mini_contributor_gold";
                        break;

                    case 1:
                        this.ContributorIconIdentifier = "icon_mini_contributor_silver";
                        break;

                    case 2:
                        this.ContributorIconIdentifier = "icon_mini_contributor_bronze";
                        break;
                }
            }
            else
            {
                this.ContributorIconIdentifier = null;
            }
            switch (this.Type)
            {
                case LogEventType.BossHuntStarted:
                    this.LogDisplayText = _.L(ConfigLoca.BH_LOG_BOSS_HUNT_STARTED_LOG, null, false);
                    this.TickerLeftText = null;
                    this.TickerRightText = _.L(ConfigLoca.BH_LOG_BOSS_HUNT_STARTED_TICKER_RIGHT, null, false);
                    this.IconIdentifier = "icon_mini_star";
                    return;

                case LogEventType.CardPackDonated:
                    this.LogDisplayText = _.L(ConfigLoca.BH_LOG_BOSS_HUNT_CARD_PACK_DONATED_LOG, new <>__AnonType5<string>(playerName), false);
                    this.TickerLeftText = playerName;
                    this.TickerRightText = _.L(ConfigLoca.BH_LOG_BOSS_HUNT_CARD_PACK_DONATED_TICKER_RIGHT, null, false);
                    this.IconIdentifier = "icon_mini_plus_gold";
                    return;

                case LogEventType.MultiCardPackDonated:
                    int num2;
                    if (!int.TryParse(this.AdditionalData, out num2))
                    {
                        num2 = 1;
                    }
                    num2 = (num2 <= 0) ? 1 : num2;
                    this.LogDisplayText = _.L(ConfigLoca.BH_LOG_BOSS_HUNT_MULTI_CARD_PACK_DONATED_LOG, new <>__AnonType3<string, int>(playerName, num2), false);
                    this.TickerLeftText = playerName;
                    this.TickerRightText = _.L(ConfigLoca.BH_LOG_BOSS_HUNT_MULTI_CARD_PACK_DONATED_TICKER_RIGHT, new <>__AnonType4<int>(num2), false);
                    this.IconIdentifier = "icon_mini_plus_gold";
                    return;

                case LogEventType.BossKilled:
                case LogEventType.EliteBossKilled:
                {
                    string name = GameLogic.Binder.CharacterResources.getResource(this.AdditionalData).Name;
                    this.LogDisplayText = _.L(ConfigLoca.BH_LOG_BOSS_KILLED_LOG, new <>__AnonType2<string, string>(playerName, name), false);
                    this.TickerLeftText = playerName;
                    this.TickerRightText = name;
                    this.IconIdentifier = "icon_mini_logo_bosshunt";
                    return;
                }
                case LogEventType.MileStoneReached:
                    this.LogDisplayText = _.L(ConfigLoca.BH_LOG_BOSS_HUNT_MILESTONE_LOG, null, false);
                    this.TickerLeftText = null;
                    this.TickerRightText = _.L(ConfigLoca.BH_LOG_BOSS_HUNT_MILESTONE_TICKER_RIGHT, null, false);
                    this.IconIdentifier = "icon_mini_star";
                    return;

                case LogEventType.CardSelected:
                {
                    TournamentUpgrade tournamentUpgrade = App.Binder.ConfigMeta.GetTournamentUpgrade(this.AdditionalData);
                    ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[tournamentUpgrade.PerkType];
                    string upgrade = _.L(data.ShortDescription, null, false);
                    this.LogDisplayText = _.L(ConfigLoca.BH_LOG_CARD_SELECTED_LOG, new <>__AnonType6<string, string>(playerName, upgrade), false);
                    this.TickerLeftText = playerName;
                    this.TickerRightText = upgrade;
                    this.IconIdentifier = "icon_mini_cardstack";
                    return;
                }
                case LogEventType.EpicCardSelected:
                {
                    TournamentUpgrade upgrade2 = App.Binder.ConfigMeta.GetTournamentUpgrade(this.AdditionalData);
                    ConfigPerks.SharedData data2 = ConfigPerks.SHARED_DATA[upgrade2.PerkType];
                    string str4 = _.L(data2.ShortDescription, null, false);
                    this.LogDisplayText = _.L(ConfigLoca.BH_LOG_CARD_SELECTED_LOG, new <>__AnonType6<string, string>(playerName, str4), false);
                    this.TickerLeftText = playerName;
                    this.TickerRightText = str4;
                    this.IconIdentifier = "icon_mini_cardstack_gold";
                    return;
                }
                case LogEventType.PlayerJoined:
                    this.LogDisplayText = _.L(ConfigLoca.BH_LOG_PLAYER_JOINED_LOG, new <>__AnonType5<string>(playerName), false);
                    this.TickerLeftText = playerName;
                    this.TickerRightText = _.L(ConfigLoca.BH_LOG_PLAYER_JOINED_TICKER_RIGHT, null, false);
                    this.IconIdentifier = "icon_mini_contributor";
                    return;

                case LogEventType.PlayerGainedMaxRarityItem:
                {
                    string itemName = GameLogic.Binder.ItemResources.getResource(this.AdditionalData).Name;
                    ItemType type = GameLogic.Binder.ItemResources.getResource(this.AdditionalData).Type;
                    this.LogDisplayText = _.L(ConfigLoca.BH_LOG_PLAYER_GAINED_MAX_RARITY_ITEM, new <>__AnonType7<string, string>(playerName, itemName), false);
                    this.TickerLeftText = playerName;
                    this.TickerRightText = itemName;
                    switch (type)
                    {
                        case ItemType.Armor:
                            this.IconIdentifier = "icon_mini_armor";
                            return;

                        case ItemType.Cloak:
                            this.IconIdentifier = "icon_mini_cloak";
                            return;
                    }
                    break;
                }
                default:
                    Debug.LogError("Non-existing logevent type");
                    return;
            }
            this.IconIdentifier = "icon_mini_weapon";
        }

        [JsonIgnore]
        public string ContributorIconIdentifier
        {
            [CompilerGenerated]
            get
            {
                return this.<ContributorIconIdentifier>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ContributorIconIdentifier>k__BackingField = value;
            }
        }

        [JsonIgnore]
        public string IconIdentifier
        {
            [CompilerGenerated]
            get
            {
                return this.<IconIdentifier>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<IconIdentifier>k__BackingField = value;
            }
        }

        [JsonIgnore]
        public string LogDisplayText
        {
            [CompilerGenerated]
            get
            {
                return this.<LogDisplayText>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<LogDisplayText>k__BackingField = value;
            }
        }

        [JsonIgnore]
        public string TickerLeftText
        {
            [CompilerGenerated]
            get
            {
                return this.<TickerLeftText>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<TickerLeftText>k__BackingField = value;
            }
        }

        [JsonIgnore]
        public string TickerRightText
        {
            [CompilerGenerated]
            get
            {
                return this.<TickerRightText>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<TickerRightText>k__BackingField = value;
            }
        }

        public enum LogEventType
        {
            BossHuntStarted,
            CardPackDonated,
            MultiCardPackDonated,
            BossKilled,
            EliteBossKilled,
            MileStoneReached,
            CardSelected,
            EpicCardSelected,
            PlayerJoined,
            PlayerGainedMaxRarityItem
        }
    }
}

