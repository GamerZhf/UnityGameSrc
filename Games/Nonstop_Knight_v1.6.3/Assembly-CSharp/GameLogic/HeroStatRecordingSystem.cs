namespace GameLogic
{
    using App;
    using PlayerView;
    using Service;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class HeroStatRecordingSystem : MonoBehaviour, IHeroStatRecordingSystem
    {
        private ManualTimer m_activePlaytimeTimer = new ManualTimer(1f);
        private List<HeroStats> m_heroStats = new List<HeroStats>();
        private GameLogic.RealtimeCombatStats m_realtimeCombatStats = new GameLogic.RealtimeCombatStats(0x180, 30f);

        protected void Awake()
        {
        }

        protected void FixedUpdate()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if ((activeDungeon != null) && (activeDungeon.CurrentGameplayState == GameplayState.ACTION))
            {
                this.RealtimeCombatStats.tick(Time.deltaTime);
            }
        }

        private void onBossTrainEnded(Player player, int numCharges, int numBossesKilled)
        {
            if (numCharges == numBossesKilled)
            {
                for (int i = 0; i < this.m_heroStats.Count; i++)
                {
                    HeroStats local1 = this.m_heroStats[i];
                    local1.CompletedBossTickets++;
                }
            }
        }

        private void onBossTrainStarted(Player player, int numCharges)
        {
            for (int i = 0; i < this.m_heroStats.Count; i++)
            {
                HeroStats local1 = this.m_heroStats[i];
                local1.UsedBossTickets++;
            }
        }

        private void onBuffStarted(CharacterInstance character, Buff buff)
        {
            if (!character.IsPlayerCharacter)
            {
                EffectType effectTypeForBuff = ConfigUi.GetEffectTypeForBuff(buff);
                for (int i = 0; i < this.m_heroStats.Count; i++)
                {
                    HeroStats stats = this.m_heroStats[i];
                    if (effectTypeForBuff == ConfigPerks.GlobalFrostEffect.EffectType)
                    {
                        stats.EnemiesFrozen++;
                    }
                    else if (effectTypeForBuff == ConfigPerks.GlobalPoisonEffect.EffectType)
                    {
                        stats.EnemiesPoisoned++;
                    }
                    if (buff.Stuns)
                    {
                        stats.EnemiesStunned++;
                    }
                }
            }
        }

        private void onCharacterDealtDamage(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, Vector3 worldPos, double amount, bool critted, bool damageReduced, DamageType damageType, SkillType fromSkill)
        {
            if ((sourceCharacter != null) && sourceCharacter.IsPlayerCharacter)
            {
                for (int i = 0; i < this.m_heroStats.Count; i++)
                {
                    HeroStats stats = this.m_heroStats[i];
                    stats.DamageDealt += amount;
                    if (fromSkill != SkillType.NONE)
                    {
                        string key = fromSkill.ToString();
                        if (!stats.SkillDamageCounts.ContainsKey(key))
                        {
                            stats.SkillDamageCounts.Add(key, amount);
                        }
                        else
                        {
                            Dictionary<string, double> dictionary;
                            string str2;
                            double num2 = dictionary[str2];
                            (dictionary = stats.SkillDamageCounts)[str2 = key] = num2 + amount;
                        }
                    }
                    if (critted)
                    {
                        stats.HighestCriticalHit = Math.Max(stats.HighestCriticalHit, amount);
                    }
                }
                if (App.Binder.ConfigMeta.COMBAT_STATS_ENABLED)
                {
                    GameLogic.RealtimeCombatStats.DatapointType damageSupport;
                    if (sourceCharacter.IsSupport)
                    {
                        damageSupport = GameLogic.RealtimeCombatStats.DatapointType.DamageSupport;
                    }
                    else if (fromSkill != SkillType.NONE)
                    {
                        damageSupport = GameLogic.RealtimeCombatStats.DatapointType.DamageSkill;
                    }
                    else
                    {
                        damageSupport = GameLogic.RealtimeCombatStats.DatapointType.DamageWeapon;
                    }
                    this.m_realtimeCombatStats.addDatapoint(amount, damageSupport);
                }
            }
        }

        private void onCharacterKilled(CharacterInstance character, CharacterInstance killer, bool critted, SkillType fromSkill)
        {
            if ((killer != null) && killer.IsPlayerCharacter)
            {
                Room activeRoom = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom;
                bool flag = GameLogic.Binder.FrenzySystem.isFrenzyActive();
                int num = activeRoom.numberOfBossesAlive();
                for (int i = 0; i < this.m_heroStats.Count; i++)
                {
                    string str3;
                    int num3;
                    HeroStats stats = this.m_heroStats[i];
                    stats.MonstersKilled++;
                    bool flag2 = character.IsBoss && (num == 0);
                    if (flag2)
                    {
                        stats.BossesBeat++;
                        if (character.IsEliteBoss)
                        {
                            stats.EliteBossesBeat++;
                        }
                    }
                    if (flag)
                    {
                        if (flag2)
                        {
                            stats.BossesBeatDuringFrenzy++;
                        }
                        else
                        {
                            stats.MinionsKilledDuringFrenzy++;
                        }
                    }
                    if (!stats.EncounteredCharacterTypes.Contains(character.Type))
                    {
                        stats.EncounteredCharacterTypes.Add(character.Type);
                    }
                    string key = character.Type.ToString();
                    if (!stats.CharacterTypeKillCounts.ContainsKey(key))
                    {
                        stats.CharacterTypeKillCounts.Add(key, 1);
                    }
                    else
                    {
                        Dictionary<string, int> dictionary;
                        num3 = dictionary[str3];
                        (dictionary = stats.CharacterTypeKillCounts)[str3 = key] = num3 + 1;
                    }
                    if (fromSkill != SkillType.NONE)
                    {
                        string str2 = fromSkill.ToString();
                        if (!stats.SkillMinionKills.ContainsKey(str2))
                        {
                            stats.SkillMinionKills.Add(str2, 1);
                        }
                        else
                        {
                            Dictionary<string, int> dictionary2;
                            num3 = dictionary2[str3];
                            (dictionary2 = stats.SkillMinionKills)[str3 = str2] = num3 + 1;
                        }
                    }
                }
            }
        }

        private void onCharacterSkillActivated(CharacterInstance character, SkillType skillType, float buildupTime, SkillExecutionStats executionStats)
        {
            if (character.IsPrimaryPlayerCharacter)
            {
                string key = skillType.ToString();
                for (int i = 0; i < this.m_heroStats.Count; i++)
                {
                    if (!this.m_heroStats[i].SkillActivationCounts.ContainsKey(key))
                    {
                        this.m_heroStats[i].SkillActivationCounts.Add(key, 1);
                    }
                    else
                    {
                        Dictionary<string, int> dictionary;
                        string str2;
                        int num2 = dictionary[str2];
                        (dictionary = this.m_heroStats[i].SkillActivationCounts)[str2 = key] = num2 + 1;
                    }
                }
            }
        }

        private void onCharacterSpawned(CharacterInstance c)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (c.IsBoss && (activeDungeon.Floor == 2))
            {
                for (int i = 0; i < this.m_heroStats.Count; i++)
                {
                    HeroStats local1 = this.m_heroStats[i];
                    local1.FirstBossSummonCount++;
                }
            }
        }

        protected void OnDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            eventBus.OnCharacterSpawned -= new GameLogic.Events.CharacterSpawned(this.onCharacterSpawned);
            eventBus.OnCharacterKilled -= new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            eventBus.OnCharacterSkillActivated -= new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            eventBus.OnDropLootTableRolled -= new GameLogic.Events.DropLootTableRolled(this.onDropLootTableRolled);
            eventBus.OnMultikillBonusGranted -= new GameLogic.Events.MultikillBonusGranted(this.onMultikillBonusGranted);
            eventBus.OnRoomCompleted -= new GameLogic.Events.RoomCompleted(this.onRoomCompleted);
            eventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnResourcesGained -= new GameLogic.Events.ResourcesGained(this.onResourcesGained);
            eventBus.OnCharacterDealtDamage -= new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            eventBus.OnItemGained -= new GameLogic.Events.ItemGained(this.onItemGained);
            eventBus.OnItemInspected -= new GameLogic.Events.ItemInspected(this.onItemInspected);
            eventBus.OnItemUnlocked -= new GameLogic.Events.ItemUnlocked(this.onItemUnlocked);
            eventBus.OnItemRankUpped -= new GameLogic.Events.ItemRankUpped(this.onItemRankUpped);
            eventBus.OnPlayerRankUpped -= new GameLogic.Events.PlayerRankUpped(this.onPlayerRankUpped);
            eventBus.OnRewardConsumed -= new GameLogic.Events.RewardConsumed(this.onRewardConsumed);
            eventBus.OnPassiveProgress -= new GameLogic.Events.PassiveProgress(this.onPassiveProgress);
            eventBus.OnPlayerAugmentationGained -= new GameLogic.Events.PlayerAugmentationGained(this.onPlayerAugmentationGained);
            eventBus.OnBuffStarted -= new GameLogic.Events.BuffStarted(this.onBuffStarted);
            eventBus.OnBossTrainStarted -= new GameLogic.Events.BossTrainStarted(this.onBossTrainStarted);
            eventBus.OnBossTrainEnded -= new GameLogic.Events.BossTrainEnded(this.onBossTrainEnded);
            eventBus.OnMissionStarted -= new GameLogic.Events.MissionStarted(this.onMissionStarted);
            eventBus.OnDungeonBoostActivated -= new GameLogic.Events.DungeonBoostActivated(this.onDungeonBoostActivated);
            eventBus.OnPlayerRetired -= new GameLogic.Events.PlayerRetired(this.onPlayerRetired);
            eventBus.OnTournamentSelected -= new GameLogic.Events.TournamentSelected(this.onTournamentSelected);
        }

        public void onDropLootTableRolled(LootTable lootTable, Vector3 worldPos, Reward drop)
        {
            if (drop.ChestType != ChestType.NONE)
            {
                string item = drop.ChestType.ToString();
                for (int i = 0; i < this.m_heroStats.Count; i++)
                {
                    if (!this.m_heroStats[i].EncounteredChestTypes.Contains(item))
                    {
                        this.m_heroStats[i].EncounteredChestTypes.Add(item);
                    }
                }
            }
        }

        private void onDungeonBoostActivated(DungeonBoost dungeonBoost, SkillType fromSkill)
        {
            if (ConfigDungeonBoosts.IsBox(dungeonBoost.Properties.Type))
            {
                for (int i = 0; i < this.m_heroStats.Count; i++)
                {
                    HeroStats stats = this.m_heroStats[i];
                    if (fromSkill != SkillType.NONE)
                    {
                        string key = fromSkill.ToString();
                        if (!stats.SkillDungeonBoostBoxDestructionCounts.ContainsKey(key))
                        {
                            stats.SkillDungeonBoostBoxDestructionCounts.Add(key, 1.0);
                        }
                        else
                        {
                            Dictionary<string, double> dictionary;
                            string str2;
                            double num2 = dictionary[str2];
                            (dictionary = stats.SkillDungeonBoostBoxDestructionCounts)[str2 = key] = num2 + 1.0;
                        }
                    }
                    stats.DungeonBoostBoxesDestroyed++;
                }
            }
        }

        protected void OnEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            eventBus.OnCharacterSpawned += new GameLogic.Events.CharacterSpawned(this.onCharacterSpawned);
            eventBus.OnCharacterKilled += new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            eventBus.OnCharacterSkillActivated += new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            eventBus.OnDropLootTableRolled += new GameLogic.Events.DropLootTableRolled(this.onDropLootTableRolled);
            eventBus.OnMultikillBonusGranted += new GameLogic.Events.MultikillBonusGranted(this.onMultikillBonusGranted);
            eventBus.OnRoomCompleted += new GameLogic.Events.RoomCompleted(this.onRoomCompleted);
            eventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnResourcesGained += new GameLogic.Events.ResourcesGained(this.onResourcesGained);
            eventBus.OnCharacterDealtDamage += new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            eventBus.OnItemGained += new GameLogic.Events.ItemGained(this.onItemGained);
            eventBus.OnItemInspected += new GameLogic.Events.ItemInspected(this.onItemInspected);
            eventBus.OnItemUnlocked += new GameLogic.Events.ItemUnlocked(this.onItemUnlocked);
            eventBus.OnItemRankUpped += new GameLogic.Events.ItemRankUpped(this.onItemRankUpped);
            eventBus.OnPlayerRankUpped += new GameLogic.Events.PlayerRankUpped(this.onPlayerRankUpped);
            eventBus.OnRewardConsumed += new GameLogic.Events.RewardConsumed(this.onRewardConsumed);
            eventBus.OnPassiveProgress += new GameLogic.Events.PassiveProgress(this.onPassiveProgress);
            eventBus.OnPlayerAugmentationGained += new GameLogic.Events.PlayerAugmentationGained(this.onPlayerAugmentationGained);
            eventBus.OnBuffStarted += new GameLogic.Events.BuffStarted(this.onBuffStarted);
            eventBus.OnBossTrainStarted += new GameLogic.Events.BossTrainStarted(this.onBossTrainStarted);
            eventBus.OnBossTrainEnded += new GameLogic.Events.BossTrainEnded(this.onBossTrainEnded);
            eventBus.OnMissionStarted += new GameLogic.Events.MissionStarted(this.onMissionStarted);
            eventBus.OnDungeonBoostActivated += new GameLogic.Events.DungeonBoostActivated(this.onDungeonBoostActivated);
            eventBus.OnPlayerRetired += new GameLogic.Events.PlayerRetired(this.onPlayerRetired);
            eventBus.OnTournamentSelected += new GameLogic.Events.TournamentSelected(this.onTournamentSelected);
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.unregisterAllHeroStatsTargets();
            this.registerHeroStatsTarget(player.ActiveCharacter.HeroStats);
            if (activeDungeon.ActiveTournament != null)
            {
                this.registerHeroStatsTarget(activeDungeon.ActiveTournament.HeroStats);
            }
            for (int i = 0; i < this.m_heroStats.Count; i++)
            {
                HeroStats stats = this.m_heroStats[i];
                stats.HighestFloor = Mathf.Max(stats.HighestFloor, activeDungeon.Floor);
                stats.HighestTokenMultiplier = Math.Max(stats.HighestTokenMultiplier, player.getActiveTokenRewardFloorMultiplier());
            }
        }

        private void onGameStateInitialized()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if ((player.CumulativeRetiredHeroStats.HighestTokenGainWithRetirement <= 0.0) && player.hasRetired())
            {
                double num = Math.Floor(App.Binder.ConfigMeta.RetirementTokenReward(player.ActiveCharacter, player.CumulativeRetiredHeroStats.HighestFloor) * player.CumulativeRetiredHeroStats.HighestTokenMultiplier);
                player.CumulativeRetiredHeroStats.HighestTokenGainWithRetirement = num;
            }
        }

        private void onItemGained(CharacterInstance character, ItemInstance itemInstance, string trackingId)
        {
            for (int i = 0; i < this.m_heroStats.Count; i++)
            {
                Dictionary<int, int> dictionary;
                int num2;
                HeroStats stats = this.m_heroStats[i];
                if (!stats.ItemsGainedByRarity.ContainsKey(itemInstance.Rarity))
                {
                    stats.ItemsGainedByRarity.Add(itemInstance.Rarity, 0);
                }
                num2 = dictionary[num2];
                (dictionary = stats.ItemsGainedByRarity)[num2 = itemInstance.Rarity] = num2 + 1;
            }
        }

        private void onItemInspected(ItemInstance itemInstance)
        {
            for (int i = 0; i < this.m_heroStats.Count; i++)
            {
                if (!this.m_heroStats[i].EncounteredItemsIds.Contains(itemInstance.ItemId))
                {
                    this.m_heroStats[i].EncounteredItemsIds.Add(itemInstance.ItemId);
                }
            }
        }

        private void onItemRankUpped(CharacterInstance character, ItemInstance itemInstance, int rankUpCount, bool free)
        {
            for (int i = 0; i < this.m_heroStats.Count; i++)
            {
                HeroStats local1 = this.m_heroStats[i];
                local1.ItemUpgrades++;
            }
        }

        private void onItemUnlocked(CharacterInstance character, ItemInstance itemInstance)
        {
            for (int i = 0; i < this.m_heroStats.Count; i++)
            {
                HeroStats local1 = this.m_heroStats[i];
                local1.ItemsUnlocked++;
            }
        }

        private void onMissionStarted(Player player, MissionInstance mission)
        {
            for (int i = 0; i < this.m_heroStats.Count; i++)
            {
                HeroStats local1 = this.m_heroStats[i];
                local1.MissionsStarted++;
            }
        }

        public void onMultikillBonusGranted(Player player, int killCount, double coinAmount)
        {
            for (int i = 0; i < this.m_heroStats.Count; i++)
            {
                HeroStats stats = this.m_heroStats[i];
                stats.MultikilledMonsters += killCount;
                stats.HighestMultikill = Mathf.Max(stats.HighestMultikill, killCount);
                if (!stats.Multikills.ContainsKey(killCount))
                {
                    stats.Multikills.Add(killCount, 1);
                }
                else
                {
                    Dictionary<int, int> dictionary;
                    int num2;
                    num2 = dictionary[num2];
                    (dictionary = stats.Multikills)[num2 = killCount] = num2 + 1;
                }
            }
        }

        private void onPassiveProgress(Player player, int numPassiveMinionKills, int numPassiveFloorCompletions, int numPassiveBossKills)
        {
            for (int i = 0; i < this.m_heroStats.Count; i++)
            {
                HeroStats stats = this.m_heroStats[i];
                stats.MonstersKilled += numPassiveMinionKills;
                stats.FloorsCompleted += numPassiveFloorCompletions;
                stats.BossesBeat += numPassiveBossKills;
            }
        }

        private void onPlayerAugmentationGained(Player player, string id)
        {
            for (int i = 0; i < this.m_heroStats.Count; i++)
            {
                HeroStats local1 = this.m_heroStats[i];
                local1.KnightUpgrades++;
            }
        }

        private void onPlayerRankUpped(Player player, bool cheated)
        {
            player.CumulativeRetiredHeroStats.RankUps++;
        }

        private void onPlayerRetired(Player player, int retirementfloor)
        {
            this.m_realtimeCombatStats.reset();
        }

        private void onResourcesGained(Player player, ResourceType resourceType, double amount, bool visualizationManuallyControlled, string analyticsSourceId, Vector3? worldPt)
        {
            if (resourceType == ResourceType.Coin)
            {
                if (amount > 0.0)
                {
                    for (int i = 0; i < this.m_heroStats.Count; i++)
                    {
                        HeroStats local1 = this.m_heroStats[i];
                        local1.CoinsEarned += amount;
                    }
                    if (App.Binder.ConfigMeta.COMBAT_STATS_ENABLED && worldPt.HasValue)
                    {
                        GameLogic.Binder.HeroStatRecordingSystem.RealtimeCombatStats.addDatapoint(amount, GameLogic.RealtimeCombatStats.DatapointType.Coins);
                    }
                }
            }
            else if ((resourceType == ResourceType.Token) && (amount > 0.0))
            {
                for (int j = 0; j < this.m_heroStats.Count; j++)
                {
                    HeroStats local2 = this.m_heroStats[j];
                    local2.TokensEarned += amount;
                }
            }
        }

        private void onRewardConsumed(Player player, Reward drop)
        {
            if (drop.isWrappedInsideChest())
            {
                for (int i = 0; i < this.m_heroStats.Count; i++)
                {
                    HeroStats stats = this.m_heroStats[i];
                    if (ConfigMeta.IsMysteryChest(drop.ChestType))
                    {
                        stats.SilverChestsOpened++;
                    }
                    else if (drop.ChestType == App.Binder.ConfigMeta.MISSION_BIG_PRIZE_CHEST_TYPE)
                    {
                        stats.MissionBigPrizesOpened++;
                    }
                    else
                    {
                        stats.GoldChestsOpened++;
                    }
                    string item = drop.ChestType.ToString();
                    if (!stats.EncounteredChestTypes.Contains(item))
                    {
                        stats.EncounteredChestTypes.Add(item);
                    }
                }
            }
        }

        private void onRoomCompleted(Room activeRoom)
        {
            if ((activeRoom.EndCondition == RoomEndCondition.NORMAL_COMPLETION) || (activeRoom.EndCondition == RoomEndCondition.FRENZY_COMPLETION))
            {
                for (int i = 0; i < this.m_heroStats.Count; i++)
                {
                    HeroStats local1 = this.m_heroStats[i];
                    local1.FloorsCompleted++;
                }
            }
        }

        private void onTournamentSelected(Player player, TournamentInstance selectedTournament, TournamentInstance unselectedTournament)
        {
            if (unselectedTournament != null)
            {
                this.unregisterHeroStatsTarget(unselectedTournament.HeroStats);
            }
            if (selectedTournament != null)
            {
                this.registerHeroStatsTarget(selectedTournament.HeroStats);
            }
        }

        public void registerHeroStatsTarget(HeroStats heroStats)
        {
            if (this.m_heroStats.Contains(heroStats))
            {
                Debug.LogWarning("Trying to re-register HeroStats: " + heroStats);
            }
            else
            {
                this.m_heroStats.Add(heroStats);
            }
        }

        public void unregisterAllHeroStatsTargets()
        {
            this.m_heroStats.Clear();
        }

        public void unregisterHeroStatsTarget(HeroStats heroStats)
        {
            if (this.m_heroStats.Contains(heroStats))
            {
                this.m_heroStats.Remove(heroStats);
            }
        }

        protected void Update()
        {
            this.m_activePlaytimeTimer.tick(Time.unscaledDeltaTime);
            if (this.m_activePlaytimeTimer.Idle)
            {
                for (int i = 0; i < this.m_heroStats.Count; i++)
                {
                    HeroStats local1 = this.m_heroStats[i];
                    local1.SecondsPlayedActive += 1L;
                }
                this.m_activePlaytimeTimer.reset();
            }
        }

        public GameLogic.RealtimeCombatStats RealtimeCombatStats
        {
            get
            {
                return this.m_realtimeCombatStats;
            }
        }
    }
}

