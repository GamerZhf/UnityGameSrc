namespace App
{
    using GameLogic;
    using Service;
    using System;
    using System.Collections.Generic;

    public static class ConfigPromotionEvents
    {
        public static Event DEBUG_EVENT;
        public static bool DEBUG_EVENT_ENABLED = false;

        static ConfigPromotionEvents()
        {
            Event event2 = new Event();
            EventHeader header = new EventHeader();
            header.Type = PromotionEventHeaderType.HeroAvatar;
            Dictionary<ItemType, string> dictionary = new Dictionary<ItemType, string>();
            dictionary.Add(ItemType.Weapon, "Weapon034");
            dictionary.Add(ItemType.Armor, "Armor031");
            dictionary.Add(ItemType.Cloak, "Cloak032");
            header.Items = dictionary;
            event2.Header = header;
            EventInfo info = new EventInfo();
            info.Title = "Halloween";
            info.Subtitle = "Trick or treating in the dungeon";
            info.Flavor = "This is Halloween! Time to embark on an exclusive chest quest and smash through a pumpkin ridden Dungeon!";
            List<EventInfo.DescriptionRow> list = new List<EventInfo.DescriptionRow>();
            EventInfo.DescriptionRow item = new EventInfo.DescriptionRow();
            item.Icon = new SpriteAtlasEntry("Menu", "icon_bounty004_floater");
            item.Text = "1st description row";
            list.Add(item);
            item = new EventInfo.DescriptionRow();
            item.Icon = new SpriteAtlasEntry("DungeonHud", "icon_coin_floater");
            item.Text = "2nd description row";
            list.Add(item);
            item = new EventInfo.DescriptionRow();
            item.Icon = new SpriteAtlasEntry("Menu", "icon_logo_bosshunt_floater");
            item.Text = "3rd description row";
            list.Add(item);
            info.DescriptionRows = list;
            event2.Info = info;
            EventMissions missions = new EventMissions();
            missions.BigPrizeDescription = "Complete all the Event bounties to unlock the horrors inside the Halloween chest!";
            missions.BigPrizeRewardChestType = ChestType.EventHalloween;
            missions.BigPrizeSprite = new SpriteAtlasEntry("Menu", "floater_chest_halloween");
            List<EventMissionInstance> list2 = new List<EventMissionInstance>();
            EventMissionInstance instance = new EventMissionInstance();
            instance.MissionId = "DestroyDungeonBoxesUsingWhirlwind";
            instance.Title = "Pumpkin Whirl!";
            instance.Description = "Destroy $Amount$ pumpkins using Whirl";
            instance.Requirement = 3.0;
            instance.RewardDiamonds = 5.0;
            list2.Add(instance);
            instance = new EventMissionInstance();
            instance.MissionId = "DestroyDungeonBoxesUsingLeap";
            instance.Title = "Pumpkin Leap!";
            instance.Description = "Destroy $Amount$ pumpkins using Leap";
            instance.Requirement = 3.0;
            instance.StartTimestampOffset = 60L;
            instance.RewardDiamonds = 10.0;
            list2.Add(instance);
            instance = new EventMissionInstance();
            instance.MissionId = "DestroyDungeonBoxesUsingSlam";
            instance.Title = "Pumpkin Slam!";
            instance.Description = "Destroy $Amount$ pumpkins using Slam";
            instance.Requirement = 3.0;
            instance.StartTimestampOffset = 120L;
            instance.RewardDiamonds = 15.0;
            list2.Add(instance);
            missions.Instances = list2;
            event2.Missions = missions;
            event2.StartTimestamp = TimeUtil.DateTimeToUnixTimestamp(DateTime.UtcNow);
            event2.EndTimestamp = TimeUtil.DateTimeToUnixTimestamp(DateTime.UtcNow) + 0x127500L;
            DEBUG_EVENT = event2;
        }

        public class Event
        {
            public long EndTimestamp;
            public ConfigPromotionEvents.EventHeader Header;
            public bool HideTimer;
            public string IapPlacementPromotionId;
            public ConfigPromotionEvents.EventInfo Info;
            public ConfigPromotionEvents.EventMissions Missions;
            public long StartTimestamp;

            public Event()
            {
            }

            public Event(RemotePromotion remotePromotion)
            {
                EventParams parsedCustomParams = remotePromotion.ParsedCustomParams as EventParams;
                this.Header = parsedCustomParams.Header;
                this.Info = new ConfigPromotionEvents.EventInfo(remotePromotion);
                this.Missions = new ConfigPromotionEvents.EventMissions(remotePromotion);
                this.IapPlacementPromotionId = parsedCustomParams.IapPlacementPromotionId;
                this.StartTimestamp = (long) remotePromotion.timing.start;
                this.EndTimestamp = (long) remotePromotion.timing.end;
                this.HideTimer = parsedCustomParams.HideTimer;
            }
        }

        public class EventHeader : IEquatable<ConfigPromotionEvents.EventHeader>
        {
            public GameLogic.CharacterPrefab CharacterPrefab;
            public Dictionary<ItemType, string> Items = new Dictionary<ItemType, string>();
            public PromotionEventHeaderType Type;

            public bool Equals(ConfigPromotionEvents.EventHeader other)
            {
                if (((this.Type != other.Type) || (this.CharacterPrefab != other.CharacterPrefab)) || (this.Items.Count != other.Items.Count))
                {
                    return false;
                }
                foreach (KeyValuePair<ItemType, string> pair in this.Items)
                {
                    if (!other.Items.ContainsKey(pair.Key))
                    {
                        return false;
                    }
                    if (pair.Value != other.Items[pair.Key])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public class EventInfo
        {
            public List<DescriptionRow> DescriptionRows;
            public string Flavor;
            public string Subtitle;
            public string Title;

            public EventInfo()
            {
                this.DescriptionRows = new List<DescriptionRow>();
            }

            public EventInfo(RemotePromotion remotePromotion)
            {
                this.DescriptionRows = new List<DescriptionRow>();
                EventParams parsedCustomParams = remotePromotion.ParsedCustomParams as EventParams;
                this.Title = remotePromotion.ParsedLoca.PopupHeadline;
                this.Subtitle = remotePromotion.ParsedLoca.PopupTitle;
                this.Flavor = remotePromotion.ParsedLoca.PopupDescription;
                this.DescriptionRows.Clear();
                if (!string.IsNullOrEmpty(remotePromotion.ParsedLoca.PopupBody))
                {
                    char[] separator = new char[] { ';' };
                    string[] strArray = remotePromotion.ParsedLoca.PopupBody.Split(separator);
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        string str = strArray[i];
                        if (str != null)
                        {
                            str = str.Trim();
                            if (!string.IsNullOrEmpty(str))
                            {
                                SpriteAtlasEntry entry = (parsedCustomParams.DescriptionIcons.Count <= i) ? null : parsedCustomParams.DescriptionIcons[i];
                                DescriptionRow item = new DescriptionRow();
                                item.Icon = entry;
                                item.Text = str;
                                this.DescriptionRows.Add(item);
                            }
                        }
                    }
                }
            }

            public class DescriptionRow
            {
                public SpriteAtlasEntry Icon = new SpriteAtlasEntry("Menu", "icon_mini_perk");
                public string Text;
            }
        }

        public class EventMissionInstance
        {
            public string Description;
            public string MissionId;
            public double Requirement;
            public double RewardDiamonds;
            public long StartTimestampOffset;
            public string Title;

            public long getStartTimestampOffset(ConfigPromotionEvents.Event data)
            {
                return (((data.StartTimestamp + this.StartTimestampOffset) >= data.EndTimestamp) ? 0L : MathUtil.Clamp(this.StartTimestampOffset, 0L, 0x7fffffffffffffffL));
            }

            public void setInfo(PromoLoca.Mission remotePromotionLoca)
            {
                this.Title = remotePromotionLoca.Title;
                this.Description = remotePromotionLoca.Description;
            }
        }

        public class EventMissions
        {
            public string BigPrizeDescription;
            public ChestType BigPrizeRewardChestType;
            public SpriteAtlasEntry BigPrizeSprite;
            public List<ConfigPromotionEvents.EventMissionInstance> Instances;

            public EventMissions()
            {
                this.Instances = new List<ConfigPromotionEvents.EventMissionInstance>();
            }

            public EventMissions(RemotePromotion remotePromotion)
            {
                this.Instances = new List<ConfigPromotionEvents.EventMissionInstance>();
                EventParams parsedCustomParams = remotePromotion.ParsedCustomParams as EventParams;
                this.BigPrizeDescription = remotePromotion.ParsedLoca.PopupMissionsBigPrize;
                this.BigPrizeRewardChestType = parsedCustomParams.Missions.BigPrizeRewardChestType;
                this.BigPrizeSprite = parsedCustomParams.Missions.BigPrizeSprite;
                this.Instances = parsedCustomParams.Missions.Instances;
                for (int i = 0; (i < this.Instances.Count) && (i < remotePromotion.ParsedLoca.PopupMissions.Count); i++)
                {
                    this.Instances[i].setInfo(remotePromotion.ParsedLoca.PopupMissions[i]);
                }
            }

            public string getDescriptionOverride(string missionId)
            {
                ConfigPromotionEvents.EventMissionInstance instance = this.getMatch(missionId);
                return ((instance == null) ? null : instance.Description);
            }

            private ConfigPromotionEvents.EventMissionInstance getMatch(string missionId)
            {
                for (int i = 0; i < this.Instances.Count; i++)
                {
                    if (this.Instances[i].MissionId == missionId)
                    {
                        return this.Instances[i];
                    }
                }
                return null;
            }

            public string getTitleOverride(string missionId)
            {
                ConfigPromotionEvents.EventMissionInstance instance = this.getMatch(missionId);
                return ((instance == null) ? null : instance.Title);
            }
        }
    }
}

