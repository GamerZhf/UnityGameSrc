namespace App
{
    using GameLogic;
    using PlayerView;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class ConfigMissions
    {
        public static Dictionary<string, Mission> MISSIONS;
        public static int NUM_ACTIVE_MISSIONS = 3;
        public static SpriteAtlasEntry PROMOTION_EVENT_MISSION_ICON = new SpriteAtlasEntry("Menu", "icon_bounty004");
        public static SpriteAtlasEntry PROMOTION_EVENT_MISSION_ICON_FLOATER = new SpriteAtlasEntry("Menu", "icon_bounty004_floater");

        static ConfigMissions()
        {
            Dictionary<string, Mission> dictionary = new Dictionary<string, Mission>();
            CompleteFloors floors = new CompleteFloors();
            floors.Group = MissionGroup.Grind;
            floors.Icon = new SpriteAtlasEntry("Menu", "icon_bounty001");
            floors.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty001_floater");
            floors.Description = ConfigLoca.MISSION_COMPLETE_FLOORS_DESCRIPTION;
            floors.Title = ConfigLoca.MISSION_COMPLETE_FLOORS_TITLE;
            dictionary.Add("CompleteFloors", floors);
            UpgradeItems items = new UpgradeItems();
            items.Group = MissionGroup.Grind;
            items.Icon = new SpriteAtlasEntry("Menu", "icon_bounty001");
            items.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty001_floater");
            items.Description = ConfigLoca.MISSION_UPGRADE_ITEMS_DESCRIPTION;
            items.Title = ConfigLoca.MISSION_UPGRADE_ITEMS_TITLE;
            dictionary.Add("UpgradeItems", items);
            OpenMysteryChests chests = new OpenMysteryChests();
            chests.Group = MissionGroup.Grind;
            chests.Icon = new SpriteAtlasEntry("Menu", "icon_bounty001");
            chests.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty001_floater");
            chests.Description = ConfigLoca.MISSION_OPEN_MYSTERY_CHESTS_DESCRIPTION;
            chests.Title = ConfigLoca.MISSION_OPEN_MYSTERY_CHESTS_TITLE;
            dictionary.Add("OpenMysteryChests", chests);
            DestroyDungeonBoostBoxes boxes = new DestroyDungeonBoostBoxes();
            boxes.Group = MissionGroup.Grind;
            boxes.Icon = new SpriteAtlasEntry("Menu", "icon_bounty001");
            boxes.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty001_floater");
            boxes.Description = ConfigLoca.MISSION_DESTROY_DUNGEON_BOOST_BOXES_DESCRIPTION;
            boxes.Title = ConfigLoca.MISSION_DESTROY_DUNGEON_BOOST_BOXES_TITLE;
            dictionary.Add("DestroyDungeonBoxes", boxes);
            DestroyDungeonBoostBoxesUsingSkill skill = new DestroyDungeonBoostBoxesUsingSkill(SkillType.Leap);
            skill.Group = MissionGroup.Skill;
            skill.Icon = new SpriteAtlasEntry("Menu", "icon_bounty002");
            skill.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty002_floater");
            skill.Description = ConfigLoca.MISSION_DESTROY_DUNGEON_BOOST_BOXES_USING_LEAP_DESCRIPTION;
            skill.Title = ConfigLoca.MISSION_DESTROY_DUNGEON_BOOST_BOXES_USING_LEAP_TITLE;
            dictionary.Add("DestroyDungeonBoxesUsingLeap", skill);
            skill = new DestroyDungeonBoostBoxesUsingSkill(SkillType.Slam);
            skill.Group = MissionGroup.Skill;
            skill.Icon = new SpriteAtlasEntry("Menu", "icon_bounty002");
            skill.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty002_floater");
            skill.Description = ConfigLoca.MISSION_DESTROY_DUNGEON_BOOST_BOXES_USING_SLAM_DESCRIPTION;
            skill.Title = ConfigLoca.MISSION_DESTROY_DUNGEON_BOOST_BOXES_USING_SLAM_TITLE;
            dictionary.Add("DestroyDungeonBoxesUsingSlam", skill);
            skill = new DestroyDungeonBoostBoxesUsingSkill(SkillType.Whirlwind);
            skill.Group = MissionGroup.Skill;
            skill.Icon = new SpriteAtlasEntry("Menu", "icon_bounty002");
            skill.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty002_floater");
            skill.Description = ConfigLoca.MISSION_DESTROY_DUNGEON_BOOST_BOXES_USING_WHIRLWIND_DESCRIPTION;
            skill.Title = ConfigLoca.MISSION_DESTROY_DUNGEON_BOOST_BOXES_USING_WHIRLWIND_TITLE;
            dictionary.Add("DestroyDungeonBoxesUsingWhirlwind", skill);
            BeatBosses bosses = new BeatBosses();
            bosses.Group = MissionGroup.Kill;
            bosses.Icon = new SpriteAtlasEntry("Menu", "icon_bounty003");
            bosses.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty003_floater");
            bosses.Description = ConfigLoca.MISSION_BEAT_BOSSES_DESCRIPTION;
            bosses.Title = ConfigLoca.MISSION_BEAT_BOSSES_TITLE;
            dictionary.Add("BeatBosses", bosses);
            BeatBossesDuringFrenzy frenzy = new BeatBossesDuringFrenzy();
            frenzy.Group = MissionGroup.Kill;
            frenzy.Icon = new SpriteAtlasEntry("Menu", "icon_bounty003");
            frenzy.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty003_floater");
            frenzy.Description = ConfigLoca.MISSION_BEAT_BOSSES_DURING_FRENZY_DESCRIPTION;
            frenzy.Title = ConfigLoca.MISSION_BEAT_BOSSES_DURING_FRENZY_TITLE;
            dictionary.Add("BeatBossesDuringFrenzy", frenzy);
            KillMinionsDuringFrenzy frenzy2 = new KillMinionsDuringFrenzy();
            frenzy2.Group = MissionGroup.Kill;
            frenzy2.Icon = new SpriteAtlasEntry("Menu", "icon_bounty003");
            frenzy2.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty003_floater");
            frenzy2.Description = ConfigLoca.MISSION_KILL_MINIONS_DURING_FRENZY_DESCRIPTION;
            frenzy2.Title = ConfigLoca.MISSION_KILL_MINIONS_DURING_FRENZY_TITLE;
            dictionary.Add("KillMinionsDuringFrenzy", frenzy2);
            KillMinionsUsingSkill skill2 = new KillMinionsUsingSkill(SkillType.Leap);
            skill2.Group = MissionGroup.Skill;
            skill2.Icon = new SpriteAtlasEntry("Menu", "icon_bounty002");
            skill2.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty002_floater");
            skill2.Description = ConfigLoca.MISSION_KILL_MINIONS_USING_LEAP_DESCRIPTION;
            skill2.Title = ConfigLoca.MISSION_KILL_MINIONS_USING_LEAP_TITLE;
            dictionary.Add("KillMinionsUsingLeap", skill2);
            skill2 = new KillMinionsUsingSkill(SkillType.Omnislash);
            skill2.Group = MissionGroup.Skill;
            skill2.Icon = new SpriteAtlasEntry("Menu", "icon_bounty002");
            skill2.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty002_floater");
            skill2.Description = ConfigLoca.MISSION_KILL_MINIONS_USING_OMNISLASH_DESCRIPTION;
            skill2.Title = ConfigLoca.MISSION_KILL_MINIONS_USING_OMNISLASH_TITLE;
            dictionary.Add("KillMinionsUsingOmnislash", skill2);
            skill2 = new KillMinionsUsingSkill(SkillType.Slam);
            skill2.Group = MissionGroup.Skill;
            skill2.Icon = new SpriteAtlasEntry("Menu", "icon_bounty002");
            skill2.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty002_floater");
            skill2.Description = ConfigLoca.MISSION_KILL_MINIONS_USING_SLAM_DESCRIPTION;
            skill2.Title = ConfigLoca.MISSION_KILL_MINIONS_USING_SLAM_TITLE;
            dictionary.Add("KillMinionsUsingSlam", skill2);
            skill2 = new KillMinionsUsingSkill(SkillType.Whirlwind);
            skill2.Group = MissionGroup.Skill;
            skill2.Icon = new SpriteAtlasEntry("Menu", "icon_bounty002");
            skill2.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty002_floater");
            skill2.Description = ConfigLoca.MISSION_KILL_MINIONS_USING_WHIRLWIND_DESCRIPTION;
            skill2.Title = ConfigLoca.MISSION_KILL_MINIONS_USING_WHIRLWIND_TITLE;
            dictionary.Add("KillMinionsUsingWhirlwind", skill2);
            DebuffEnemies enemies = new DebuffEnemies(DebuffEnemies.Type.Freeze);
            enemies.Group = MissionGroup.Skill;
            enemies.Icon = new SpriteAtlasEntry("Menu", "icon_bounty002");
            enemies.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty002_floater");
            enemies.Description = ConfigLoca.MISSION_FREEZE_ENEMIES_DESCRIPTION;
            enemies.Title = ConfigLoca.MISSION_FREEZE_ENEMIES_TITLE;
            dictionary.Add("FreezeEnemies", enemies);
            enemies = new DebuffEnemies(DebuffEnemies.Type.Poison);
            enemies.Group = MissionGroup.Skill;
            enemies.Icon = new SpriteAtlasEntry("Menu", "icon_bounty002");
            enemies.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty002_floater");
            enemies.Description = ConfigLoca.MISSION_POISON_ENEMIES_DESCRIPTION;
            enemies.Title = ConfigLoca.MISSION_POISON_ENEMIES_TITLE;
            dictionary.Add("PoisonEnemies", enemies);
            enemies = new DebuffEnemies(DebuffEnemies.Type.Stun);
            enemies.Group = MissionGroup.Skill;
            enemies.Icon = new SpriteAtlasEntry("Menu", "icon_bounty002");
            enemies.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty002_floater");
            enemies.Description = ConfigLoca.MISSION_STUN_ENEMIES_DESCRIPTION;
            enemies.Title = ConfigLoca.MISSION_STUN_ENEMIES_TITLE;
            dictionary.Add("StunEnemies", enemies);
            Multikill multikill = new Multikill(5);
            multikill.Group = MissionGroup.Kill;
            multikill.Icon = new SpriteAtlasEntry("Menu", "icon_bounty003");
            multikill.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty003_floater");
            multikill.Description = ConfigLoca.MISSION_MULTIKILL_DESCRIPTION;
            multikill.Title = ConfigLoca.MISSION_MULTIKILL_TITLE;
            dictionary.Add("Multikill", multikill);
            CompleteBossTicket ticket = new CompleteBossTicket();
            ticket.Group = MissionGroup.Kill;
            ticket.Icon = new SpriteAtlasEntry("Menu", "icon_bounty003");
            ticket.IconFloater = new SpriteAtlasEntry("Menu", "icon_bounty003_floater");
            ticket.Description = ConfigLoca.MISSION_COMPLETE_BOSS_TICKET_DESCRIPTION;
            ticket.Title = ConfigLoca.MISSION_COMPLETE_BOSS_TICKET_TITLE;
            dictionary.Add("CompleteBossTicket", ticket);
            MISSIONS = dictionary;
        }

        public static Mission GetMissionData(string id)
        {
            return ((string.IsNullOrEmpty(id) || !MISSIONS.ContainsKey(id)) ? null : MISSIONS[id]);
        }

        public static Sprite GetMissionRewardIcon(MissionInstance mission)
        {
            if (!string.IsNullOrEmpty(mission.RewardShopEntryId))
            {
                return PlayerView.Binder.SpriteResources.getSprite(ConfigUi.GetFloaterSpriteForShopEntry(mission.RewardShopEntryId));
            }
            if (mission.RewardDiamonds > 0.0)
            {
                return PlayerView.Binder.SpriteResources.getSprite(ConfigUi.RESOURCE_TYPE_SPRITES[ResourceType.Diamond]);
            }
            return null;
        }

        public static string GetMissionRewardTitle(MissionInstance mission)
        {
            if (!string.IsNullOrEmpty(mission.RewardShopEntryId))
            {
                string shopEntryDropTitle = MenuTreasureChest.GetShopEntryDropTitle(ConfigShops.GetShopEntry(mission.RewardShopEntryId), null);
                return (string.IsNullOrEmpty(shopEntryDropTitle) ? "1" : shopEntryDropTitle.Replace("+", string.Empty));
            }
            if (mission.RewardDiamonds > 0.0)
            {
                return mission.RewardDiamonds.ToString();
            }
            return null;
        }

        public class BeatBosses : ConfigMissions.Mission
        {
            public override bool canComplete(Player player)
            {
                return true;
            }

            public override double getValue(Player player, [Optional, DefaultParameterValue(null)] HeroStats fromStats)
            {
                if (fromStats != null)
                {
                    return (double) fromStats.BossesBeat;
                }
                return (double) (player.CumulativeRetiredHeroStats.BossesBeat + player.ActiveCharacter.HeroStats.BossesBeat);
            }
        }

        public class BeatBossesDuringFrenzy : ConfigMissions.Mission
        {
            public override bool canComplete(Player player)
            {
                return (player.ActiveCharacter.Inventory.FrenzyPotions > 0);
            }

            public override double getValue(Player player, [Optional, DefaultParameterValue(null)] HeroStats fromStats)
            {
                if (fromStats != null)
                {
                    return (double) fromStats.BossesBeatDuringFrenzy;
                }
                return (double) (player.CumulativeRetiredHeroStats.BossesBeatDuringFrenzy + player.ActiveCharacter.HeroStats.BossesBeatDuringFrenzy);
            }
        }

        public class CompleteBossTicket : ConfigMissions.Mission
        {
            public override bool canComplete(Player player)
            {
                return (player.ActiveCharacter.Inventory.BossPotions > 0);
            }

            public override double getValue(Player player, [Optional, DefaultParameterValue(null)] HeroStats fromStats)
            {
                if (fromStats != null)
                {
                    return (double) fromStats.CompletedBossTickets;
                }
                return (double) (player.CumulativeRetiredHeroStats.CompletedBossTickets + player.ActiveCharacter.HeroStats.CompletedBossTickets);
            }
        }

        public class CompleteFloors : ConfigMissions.Mission
        {
            public override bool canComplete(Player player)
            {
                return true;
            }

            public override double getValue(Player player, [Optional, DefaultParameterValue(null)] HeroStats fromStats)
            {
                if (fromStats != null)
                {
                    return (double) fromStats.FloorsCompleted;
                }
                return (double) (player.CumulativeRetiredHeroStats.FloorsCompleted + player.ActiveCharacter.HeroStats.FloorsCompleted);
            }
        }

        public class DebuffEnemies : ConfigMissions.Mission
        {
            private Type m_type;

            public DebuffEnemies(Type type)
            {
                this.m_type = type;
            }

            public override bool canComplete(Player player)
            {
                switch (this.m_type)
                {
                    case Type.Freeze:
                        for (int i = 0; i < player.Runestones.RunestoneInstances.Count; i++)
                        {
                            PerkType type = player.Runestones.RunestoneInstances[i].getPerkType();
                            ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[type];
                            if (data.EffectType == ConfigPerks.GlobalFrostEffect.EffectType)
                            {
                                return true;
                            }
                        }
                        break;

                    case Type.Poison:
                        for (int j = 0; j < player.Runestones.RunestoneInstances.Count; j++)
                        {
                            PerkType type2 = player.Runestones.RunestoneInstances[j].getPerkType();
                            ConfigPerks.SharedData data2 = ConfigPerks.SHARED_DATA[type2];
                            if (data2.EffectType == ConfigPerks.GlobalPoisonEffect.EffectType)
                            {
                                return true;
                            }
                        }
                        break;

                    case Type.Stun:
                        for (int k = 0; k < player.Runestones.RunestoneInstances.Count; k++)
                        {
                            PerkType type3 = player.Runestones.RunestoneInstances[k].getPerkType();
                            ConfigPerks.SharedData data3 = ConfigPerks.SHARED_DATA[type3];
                            if (data3.Stuns)
                            {
                                return true;
                            }
                        }
                        break;
                }
                return false;
            }

            public override double getValue(Player player, [Optional, DefaultParameterValue(null)] HeroStats fromStats)
            {
                if (fromStats != null)
                {
                    switch (this.m_type)
                    {
                        case Type.Freeze:
                            return (double) fromStats.EnemiesFrozen;

                        case Type.Poison:
                            return (double) fromStats.EnemiesPoisoned;

                        case Type.Stun:
                            return (double) fromStats.EnemiesStunned;
                    }
                    return 0.0;
                }
                switch (this.m_type)
                {
                    case Type.Freeze:
                        return (double) (player.CumulativeRetiredHeroStats.EnemiesFrozen + player.ActiveCharacter.HeroStats.EnemiesFrozen);

                    case Type.Poison:
                        return (double) (player.CumulativeRetiredHeroStats.EnemiesPoisoned + player.ActiveCharacter.HeroStats.EnemiesPoisoned);

                    case Type.Stun:
                        return (double) (player.CumulativeRetiredHeroStats.EnemiesStunned + player.ActiveCharacter.HeroStats.EnemiesStunned);
                }
                return 0.0;
            }

            public enum Type
            {
                None,
                Freeze,
                Poison,
                Stun
            }
        }

        public class DestroyDungeonBoostBoxes : ConfigMissions.Mission
        {
            public override bool canComplete(Player player)
            {
                return true;
            }

            public override double getValue(Player player, [Optional, DefaultParameterValue(null)] HeroStats fromStats)
            {
                if (fromStats != null)
                {
                    return fromStats.DungeonBoostBoxesDestroyed;
                }
                return (player.CumulativeRetiredHeroStats.DungeonBoostBoxesDestroyed + player.ActiveCharacter.HeroStats.DungeonBoostBoxesDestroyed);
            }
        }

        public class DestroyDungeonBoostBoxesUsingSkill : ConfigMissions.Mission
        {
            private SkillType m_skillType;

            public DestroyDungeonBoostBoxesUsingSkill(SkillType skillType)
            {
                this.m_skillType = skillType;
            }

            public override bool canComplete(Player player)
            {
                return player.hasUnlockedSkill(this.m_skillType);
            }

            public override double getValue(Player player, [Optional, DefaultParameterValue(null)] HeroStats fromStats)
            {
                string key = this.m_skillType.ToString();
                if (fromStats != null)
                {
                    Dictionary<string, double> dictionary = player.Tournaments.SelectedTournament.HeroStats.SkillDungeonBoostBoxDestructionCounts;
                    return (!dictionary.ContainsKey(key) ? 0.0 : dictionary[key]);
                }
                Dictionary<string, double> skillDungeonBoostBoxDestructionCounts = player.CumulativeRetiredHeroStats.SkillDungeonBoostBoxDestructionCounts;
                double num2 = !skillDungeonBoostBoxDestructionCounts.ContainsKey(key) ? 0.0 : skillDungeonBoostBoxDestructionCounts[key];
                Dictionary<string, double> dictionary3 = player.ActiveCharacter.HeroStats.SkillDungeonBoostBoxDestructionCounts;
                double num3 = !dictionary3.ContainsKey(key) ? 0.0 : dictionary3[key];
                return (num2 + num3);
            }
        }

        public class KillMinionsDuringFrenzy : ConfigMissions.Mission
        {
            public override bool canComplete(Player player)
            {
                return (player.ActiveCharacter.Inventory.FrenzyPotions > 0);
            }

            public override double getValue(Player player, [Optional, DefaultParameterValue(null)] HeroStats fromStats)
            {
                if (fromStats != null)
                {
                    return (double) fromStats.MinionsKilledDuringFrenzy;
                }
                return (double) (player.CumulativeRetiredHeroStats.MinionsKilledDuringFrenzy + player.ActiveCharacter.HeroStats.MinionsKilledDuringFrenzy);
            }
        }

        public class KillMinionsUsingSkill : ConfigMissions.Mission
        {
            private SkillType m_skillType;

            public KillMinionsUsingSkill(SkillType skillType)
            {
                this.m_skillType = skillType;
            }

            public override bool canComplete(Player player)
            {
                return player.hasUnlockedSkill(this.m_skillType);
            }

            public override double getValue(Player player, [Optional, DefaultParameterValue(null)] HeroStats fromStats)
            {
                string key = this.m_skillType.ToString();
                if (fromStats != null)
                {
                    Dictionary<string, int> dictionary = fromStats.SkillMinionKills;
                    return (!dictionary.ContainsKey(key) ? 0.0 : ((double) dictionary[key]));
                }
                Dictionary<string, int> skillMinionKills = player.CumulativeRetiredHeroStats.SkillMinionKills;
                double num2 = !skillMinionKills.ContainsKey(key) ? 0.0 : ((double) skillMinionKills[key]);
                Dictionary<string, int> dictionary3 = player.ActiveCharacter.HeroStats.SkillMinionKills;
                double num3 = !dictionary3.ContainsKey(key) ? 0.0 : ((double) dictionary3[key]);
                return (num2 + num3);
            }
        }

        public abstract class Mission
        {
            public string Description;
            public MissionGroup Group;
            public SpriteAtlasEntry Icon;
            public SpriteAtlasEntry IconFloater;
            public string Title;

            protected Mission()
            {
            }

            public abstract bool canComplete(Player player);
            public virtual string getFormattedMissionDescription(double requirement, bool colors, [Optional, DefaultParameterValue(null)] string descriptionOverride)
            {
                string description = (descriptionOverride == null) ? _.L(this.Description, null, false) : descriptionOverride;
                return (!colors ? MenuHelpers.GetFormattedDescription(description, "$Amount$", requirement.ToString("0")) : MenuHelpers.GetFormattedDescriptionColored(description, "$Amount$", requirement.ToString("0")));
            }

            public virtual float getMissionProgress(Player player, double startValue, double requirement, out double result)
            {
                result = this.getValue(player, null) - startValue;
                return ((requirement <= 0.0) ? 0f : Mathf.Clamp01((float) (result / requirement)));
            }

            public abstract double getValue(Player player, [Optional, DefaultParameterValue(null)] HeroStats fromStats);
        }

        public class Multikill : ConfigMissions.Mission
        {
            private int m_size;

            public Multikill(int size)
            {
                this.m_size = size;
            }

            public override bool canComplete(Player player)
            {
                return true;
            }

            public override string getFormattedMissionDescription(double requirement, bool colors, [Optional, DefaultParameterValue(null)] string descriptionOverride)
            {
                string description = (descriptionOverride == null) ? _.L(base.Description, null, false) : descriptionOverride;
                description = description.Replace("$Size$", this.m_size.ToString());
                return (!colors ? MenuHelpers.GetFormattedDescription(description, "$Amount$", requirement.ToString("0")) : MenuHelpers.GetFormattedDescriptionColored(description, "$Amount$", requirement.ToString("0")));
            }

            public override double getValue(Player player, [Optional, DefaultParameterValue(null)] HeroStats fromStats)
            {
                double num = 0.0;
                int num2 = Mathf.Max(App.Binder.ConfigMeta.FRENZY_MOB_SPAWNCOUNT_PER_SPAWNPOINT_MAX, ConfigGameplay.MOB_SPAWNCOUNT_PER_SPAWNPOINT_MAX_RANGE.Max);
                if (fromStats != null)
                {
                    Dictionary<int, int> dictionary = fromStats.Multikills;
                    for (int j = this.m_size; j <= num2; j++)
                    {
                        num += !dictionary.ContainsKey(j) ? ((double) 0) : ((double) dictionary[j]);
                    }
                    return num;
                }
                Dictionary<int, int> multikills = player.CumulativeRetiredHeroStats.Multikills;
                Dictionary<int, int> dictionary3 = player.ActiveCharacter.HeroStats.Multikills;
                for (int i = this.m_size; i <= num2; i++)
                {
                    int num5 = !multikills.ContainsKey(i) ? 0 : multikills[i];
                    int num6 = !dictionary3.ContainsKey(i) ? 0 : dictionary3[i];
                    num += num5 + num6;
                }
                return num;
            }
        }

        public class OpenMysteryChests : ConfigMissions.Mission
        {
            public override bool canComplete(Player player)
            {
                return true;
            }

            public override double getValue(Player player, [Optional, DefaultParameterValue(null)] HeroStats fromStats)
            {
                if (fromStats != null)
                {
                    return (double) fromStats.SilverChestsOpened;
                }
                return (double) (player.CumulativeRetiredHeroStats.SilverChestsOpened + player.ActiveCharacter.HeroStats.SilverChestsOpened);
            }
        }

        public class UpgradeItems : ConfigMissions.Mission
        {
            public override bool canComplete(Player player)
            {
                return true;
            }

            public override double getValue(Player player, [Optional, DefaultParameterValue(null)] HeroStats fromStats)
            {
                if (fromStats != null)
                {
                    return fromStats.ItemUpgrades;
                }
                return (player.CumulativeRetiredHeroStats.ItemUpgrades + player.ActiveCharacter.HeroStats.ItemUpgrades);
            }
        }
    }
}

