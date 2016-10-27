namespace Service
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class EventParams : CustomParams
    {
        [CompilerGenerated]
        private List<SpriteAtlasEntry> <DescriptionIcons>k__BackingField;
        [CompilerGenerated]
        private ConfigPromotionEvents.EventHeader <Header>k__BackingField;
        [CompilerGenerated]
        private string <IapPlacementPromotionId>k__BackingField;
        [CompilerGenerated]
        private ConfigPromotionEvents.EventMissions <Missions>k__BackingField;
        private const string PARAM_DESCRIPTION_ICONS = "event-description-icons";
        private const string PARAM_HEADER = "event-header";
        private const string PARAM_IAP_PLACEMENT_PROMOTION_ID = "event-iap-placement-promotion-id";
        private const string PARAM_MISSION1 = "event-mission1";
        private const string PARAM_MISSION2 = "event-mission2";
        private const string PARAM_MISSION3 = "event-mission3";
        private const string PARAM_MISSIONS_BIG_PRIZE = "event-missions-bigprize";

        public EventParams(Dictionary<string, object> promoParams) : base(promoParams)
        {
            this.DescriptionIcons = new List<SpriteAtlasEntry>();
            this.Header = new ConfigPromotionEvents.EventHeader();
            this.Missions = new ConfigPromotionEvents.EventMissions();
            if (promoParams != null)
            {
                this.ParseDescriptionIcons(promoParams);
                this.ParseHeader(promoParams);
                this.ParseMissionsBigPrize(promoParams, "event-missions-bigprize");
                this.ParseMission(promoParams, "event-mission1");
                this.ParseMission(promoParams, "event-mission2");
                this.ParseMission(promoParams, "event-mission3");
                this.ParseIapPlacementPromotionId(promoParams);
            }
        }

        private void ParseDescriptionIcons(Dictionary<string, object> promoParams)
        {
            object obj2;
            promoParams.TryGetValue("event-description-icons", out obj2);
            if (obj2 != null)
            {
                char[] separator = new char[] { ';' };
                string[] strArray = obj2.ToString().Split(separator);
                for (int i = 0; i < strArray.Length; i++)
                {
                    SpriteAtlasEntry item = this.TryParseSpriteAtlasEntry(strArray[i]);
                    if (item != null)
                    {
                        this.DescriptionIcons.Add(item);
                    }
                }
            }
        }

        private void ParseHeader(Dictionary<string, object> promoParams)
        {
            object obj2;
            promoParams.TryGetValue("event-header", out obj2);
            if (obj2 != null)
            {
                char[] separator = new char[] { ';' };
                string[] strArray = obj2.ToString().Split(separator);
                this.Header.Type = LangUtil.TryParseEnum<PromotionEventHeaderType>(strArray[0]);
                switch (this.Header.Type)
                {
                    case PromotionEventHeaderType.HeroAvatar:
                        if (strArray.Length >= 2)
                        {
                            this.Header.Items.Add(ItemType.Weapon, strArray[1]);
                        }
                        if (strArray.Length >= 3)
                        {
                            this.Header.Items.Add(ItemType.Armor, strArray[2]);
                        }
                        if (strArray.Length >= 4)
                        {
                            this.Header.Items.Add(ItemType.Cloak, strArray[3]);
                        }
                        break;

                    case PromotionEventHeaderType.PetAvatar:
                        if (strArray.Length >= 2)
                        {
                            this.Header.CharacterPrefab = LangUtil.TryParseEnum<CharacterPrefab>(strArray[1]);
                        }
                        break;
                }
            }
        }

        private void ParseIapPlacementPromotionId(Dictionary<string, object> promoParams)
        {
            object obj2;
            promoParams.TryGetValue("event-iap-placement-promotion-id", out obj2);
            if (obj2 != null)
            {
                this.IapPlacementPromotionId = obj2 as string;
            }
        }

        private void ParseMission(Dictionary<string, object> promoParams, string paramId)
        {
            object obj2;
            promoParams.TryGetValue(paramId, out obj2);
            if (obj2 != null)
            {
                char[] separator = new char[] { ';' };
                string[] strArray = obj2.ToString().Split(separator);
                if (strArray.Length == 4)
                {
                    ConfigPromotionEvents.EventMissionInstance item = new ConfigPromotionEvents.EventMissionInstance();
                    item.MissionId = strArray[0];
                    double.TryParse(strArray[1], out item.Requirement);
                    long.TryParse(strArray[2], out item.StartTimestampOffset);
                    double.TryParse(strArray[3], out item.RewardDiamonds);
                    this.Missions.Instances.Add(item);
                }
            }
        }

        private void ParseMissionsBigPrize(Dictionary<string, object> promoParams, string paramId)
        {
            object obj2;
            promoParams.TryGetValue(paramId, out obj2);
            if (obj2 != null)
            {
                char[] separator = new char[] { ';' };
                string[] strArray = obj2.ToString().Split(separator);
                if (strArray.Length == 2)
                {
                    this.Missions.BigPrizeRewardChestType = LangUtil.TryParseEnum<ChestType>(strArray[0]);
                    this.Missions.BigPrizeSprite = this.TryParseSpriteAtlasEntry(strArray[1]);
                }
            }
        }

        private SpriteAtlasEntry TryParseSpriteAtlasEntry(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                return null;
            }
            char[] separator = new char[] { '.' };
            string[] strArray = param.Split(separator);
            return ((strArray.Length != 2) ? null : new SpriteAtlasEntry(strArray[0], strArray[1]));
        }

        public override bool Validate()
        {
            return base.Validate();
        }

        public List<SpriteAtlasEntry> DescriptionIcons
        {
            [CompilerGenerated]
            get
            {
                return this.<DescriptionIcons>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<DescriptionIcons>k__BackingField = value;
            }
        }

        public ConfigPromotionEvents.EventHeader Header
        {
            [CompilerGenerated]
            get
            {
                return this.<Header>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Header>k__BackingField = value;
            }
        }

        public string IapPlacementPromotionId
        {
            [CompilerGenerated]
            get
            {
                return this.<IapPlacementPromotionId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<IapPlacementPromotionId>k__BackingField = value;
            }
        }

        public ConfigPromotionEvents.EventMissions Missions
        {
            [CompilerGenerated]
            get
            {
                return this.<Missions>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Missions>k__BackingField = value;
            }
        }
    }
}

