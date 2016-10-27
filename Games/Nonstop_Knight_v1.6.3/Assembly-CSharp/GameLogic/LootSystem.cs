namespace GameLogic
{
    using App;
    using Service;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class LootSystem : MonoBehaviour, ILootSystem
    {
        protected void Awake()
        {
        }

        public void awardBossRewards(ActiveDungeon ad, GameLogic.CharacterType killedBossType, bool wildBoss)
        {
            ad.VisualizableBossRewards = new ActiveDungeon.BossRewards();
            CharacterInstance primaryPlayerCharacter = ad.PrimaryPlayerCharacter;
            Player owningPlayer = primaryPlayerCharacter.OwningPlayer;
            int floor = ad.Floor;
            double baseCoinReward = App.Binder.ConfigMeta.BossCoinDropCurve(floor, owningPlayer.BossTrain.Active);
            double num3 = owningPlayer.calculateStandardCoinRoll(baseCoinReward, killedBossType, 1);
            ad.VisualizableBossRewards.CoinDropCount = UnityEngine.Random.Range(3, 7);
            ad.VisualizableBossRewards.CoinsPerDrop = num3 / ((double) ad.VisualizableBossRewards.CoinDropCount);
            bool isHighestFloor = ad.Floor == owningPlayer.getHighestFloorReached();
            double num4 = App.Binder.ConfigMeta.XpFromBossKill(ad.Floor, isHighestFloor) / ((double) ad.VisualizableBossRewards.CoinDropCount);
            float num5 = primaryPlayerCharacter.getCharacterTypeXpModifier(killedBossType) + primaryPlayerCharacter.UniversalXpBonus(true);
            num4 += num4 * num5;
            ad.VisualizableBossRewards.XpPerDrop = num4;
            bool flag2 = !ad.hasDungeonModifier(DungeonModifierType.MonsterNoCoins);
            for (int i = 0; i < ad.VisualizableBossRewards.CoinDropCount; i++)
            {
                if (flag2)
                {
                    Vector3? nullable = null;
                    CmdGainResources.ExecuteStatic(owningPlayer, ResourceType.Coin, ad.VisualizableBossRewards.CoinsPerDrop, true, "TRACKING_ID_GAMEPLAY_LOOT_GAIN", nullable);
                }
                Vector3? worldPt = null;
                CmdGainResources.ExecuteStatic(owningPlayer, ResourceType.Xp, ad.VisualizableBossRewards.XpPerDrop, true, string.Empty, worldPt);
            }
            bool flag3 = false;
            if (!wildBoss)
            {
                flag3 = this.grantRetirementTriggerChestIfAllowed();
            }
            if (!wildBoss && owningPlayer.canRetire())
            {
                Reward reward = owningPlayer.getFirstUnclaimedRetirementTriggerChest();
                ad.VisualizableBossRewards.Tokens = App.Binder.ConfigMeta.RetirementTokenReward(primaryPlayerCharacter, ad.Floor);
                reward.TokenDrops.Add(ad.VisualizableBossRewards.Tokens);
                if (App.Binder.ConfigMeta.BossShouldDropFrenzyPotionAtFloor(ad.Floor))
                {
                    ad.VisualizableBossRewards.FrenzyPotions = 1;
                    reward.FrenzyPotions += ad.VisualizableBossRewards.FrenzyPotions;
                }
            }
            if ((wildBoss || owningPlayer.hasRetired()) || !flag3)
            {
                ad.VisualizableBossRewards.MainDrops.Add(CmdRollDropLootTable.ExecuteStatic(App.Binder.ConfigLootTables.BossDropLootTable, owningPlayer, Vector3.zero, killedBossType, null, ChestType.NONE));
                int num7 = owningPlayer.CumulativeRetiredHeroStats.BossesBeat + owningPlayer.ActiveCharacter.HeroStats.BossesBeat;
                if ((owningPlayer.getPerkInstanceCount(PerkType.ChesterChestDrop) > 0) && ((((float) num7) % Mathf.Floor(ConfigPerks.GetBestModifier(PerkType.ChesterChestDrop))) == 0f))
                {
                    ad.VisualizableBossRewards.MainDrops.Add(CmdRollDropLootTable.ExecuteStatic(App.Binder.ConfigLootTables.BossDropLootTable, owningPlayer, Vector3.zero, killedBossType, null, ChestType.ChesterChest));
                }
            }
            if ((!wildBoss && owningPlayer.canRetire()) && ConfigMeta.BOSS_ADDITIONAL_DROPS_ENABLED)
            {
                if (flag3 && !owningPlayer.hasRetired())
                {
                    Reward item = CmdRollDropLootTable.ExecuteStatic(App.Binder.ConfigLootTables.BossAdditionalDropLootTable, owningPlayer, Vector3.zero, killedBossType, ChestType.RewardBoxCommon.ToString(), ChestType.NONE);
                    item.clearContent();
                    item.ShopEntryId = "CoinBundleSmall";
                    double num8 = App.Binder.ConfigMeta.CoinBundleSize(owningPlayer, item.ShopEntryId, 0.0);
                    item.CoinDrops.Add(num8);
                    ad.VisualizableBossRewards.RiggedRewards.Add(item);
                }
                else if (App.Binder.ConfigMeta.BossShouldDropRewardBoxAtFloor(ad.Floor))
                {
                    ad.VisualizableBossRewards.AdditionalDrop = CmdRollDropLootTable.ExecuteStatic(App.Binder.ConfigLootTables.BossAdditionalDropLootTable, owningPlayer, Vector3.zero, killedBossType, null, ChestType.NONE);
                }
            }
        }

        protected void FixedUpdate()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (player != null)
            {
                long num = Service.Binder.ServerTime.GameTime - player.LastDailyDiamondDropCountResetTimestamp;
                if (num > 0x15180L)
                {
                    CmdResetDailyDiamondDrops.ExecuteStatic(player);
                }
            }
        }

        private void grantMultikillReward(int killCount)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (!activeDungeon.isTutorialDungeon())
            {
                Player player = GameLogic.Binder.GameState.Player;
                double baseAmount = App.Binder.ConfigMeta.MultikillCoinGainCurve(killCount, activeDungeon.Floor);
                baseAmount = CharacterStatModifierUtil.ApplyCoinBonuses(player.ActiveCharacter, GameLogic.CharacterType.UNSPECIFIED, baseAmount, false);
                CmdGainResources.ExecuteStatic(player, ResourceType.Coin, baseAmount, true, "TRACKING_ID_GAMEPLAY_LOOT_GAIN", null);
                GameLogic.Binder.EventBus.MultikillBonusGranted(player, killCount, baseAmount);
            }
        }

        public bool grantRetirementTriggerChestIfAllowed()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if ((((player.getLastCompletedFloor(false) + 1) >= App.Binder.ConfigMeta.RETIREMENT_MIN_FLOOR) && (player.getFirstUnclaimedRetirementTriggerChest() == null)) && !player.Tournaments.hasTournamentSelected())
            {
                Reward reward2 = new Reward();
                reward2.ChestType = ChestType.RetirementTrigger;
                Reward item = reward2;
                player.UnclaimedRewards.Add(item);
                return true;
            }
            return false;
        }

        private void onCharacterKilled(CharacterInstance target, CharacterInstance killer, bool critted, SkillType fromSkill)
        {
            Player player = GameLogic.Binder.GameState.Player;
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (((killer != null) && !target.IsPlayerCharacter) && (killer.IsPlayerCharacter && !activeDungeon.isTutorialDungeon()))
            {
                Vector3 positionAtTimeOfDeath = target.PositionAtTimeOfDeath;
                if (target.IsBoss && (activeDungeon.ActiveRoom.numberOfBossesAlive() == 0))
                {
                    activeDungeon.LastBossKillWorldPt = positionAtTimeOfDeath;
                }
                else if (!activeDungeon.ActiveRoom.MainBossSummoned)
                {
                    if (!activeDungeon.hasDungeonModifier(DungeonModifierType.MonsterNoCoins))
                    {
                        double baseCoinReward = App.Binder.ConfigMeta.MinionCoinDropCurve(activeDungeon.Floor);
                        double num2 = player.calculateStandardCoinRoll(baseCoinReward, target.Type, 1);
                        CmdGainResources.ExecuteStatic(player, ResourceType.Coin, num2, true, "TRACKING_ID_GAMEPLAY_LOOT_GAIN", new Vector3?(positionAtTimeOfDeath));
                    }
                    CharacterInstance primaryPlayerCharacter = activeDungeon.PrimaryPlayerCharacter;
                    double amount = App.Binder.ConfigMeta.XpFromMinionKill(activeDungeon.Floor);
                    float num4 = primaryPlayerCharacter.getCharacterTypeXpModifier(target.Type) + primaryPlayerCharacter.UniversalXpBonus(true);
                    amount += amount * num4;
                    CmdGainResources.ExecuteStatic(player, ResourceType.Xp, amount, true, string.Empty, new Vector3?(positionAtTimeOfDeath));
                    CmdRollDropLootTable.ExecuteStatic(App.Binder.ConfigLootTables.MinionDropLootTable, player, positionAtTimeOfDeath, target.Type, null, ChestType.NONE);
                }
                if (target.IsWildBoss && !target.IsBossClone)
                {
                    GameLogic.Binder.LootSystem.awardBossRewards(activeDungeon, target.Type, true);
                }
            }
        }

        private void onCharacterMeleeAttackContact(CharacterInstance sourceCharacter, Vector3 contactWorldPt, bool critted)
        {
            if (sourceCharacter.IsPlayerCharacter)
            {
                Buff buff = GameLogic.Binder.BuffSystem.getBuffFromBoost(sourceCharacter, BoostType.Midas);
                if (buff != null)
                {
                    ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                    double amountPerDrop = Math.Max((double) (App.Binder.ConfigMeta.MinionCoinDropCurve(activeDungeon.Floor) * App.Binder.ConfigMeta.COIN_GAIN_CONTROLLER), (double) 1.0);
                    int dropCount = UnityEngine.Random.Range(1, ((int) buff.TotalModifier) + 1);
                    this.triggerResourceExplosion(ResourceType.Coin, contactWorldPt, amountPerDrop, dropCount, "TRACKING_ID_GAMEPLAY_LOOT_GAIN");
                }
            }
        }

        private void onCharacterMeleeAttackEnded(CharacterInstance character, CharacterInstance targetCharacter, Vector3 contactWorldPt, int killCount)
        {
            if (character.IsPlayerCharacter && (killCount >= ConfigMeta.MULTIKILL_REWARD_MIN_KILL_COUNT))
            {
                this.grantMultikillReward(killCount);
            }
        }

        private void onCharacterSkillExecuted(CharacterInstance c, SkillType skillType, SkillExecutionStats executionStats)
        {
            if (c.IsPrimaryPlayerCharacter && (executionStats.KillCount >= ConfigMeta.MULTIKILL_REWARD_MIN_KILL_COUNT))
            {
                this.grantMultikillReward(executionStats.KillCount);
            }
        }

        protected void OnDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            eventBus.OnGameplayLoadingStarted -= new GameLogic.Events.GameplayLoadingStarted(this.onGameplayLoadingStarted);
            eventBus.OnRoomCompleted -= new GameLogic.Events.RoomCompleted(this.onRoomCompleted);
            eventBus.OnCharacterKilled -= new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            eventBus.OnCharacterMeleeAttackContact -= new GameLogic.Events.CharacterMeleeAttackContact(this.onCharacterMeleeAttackContact);
            eventBus.OnCharacterMeleeAttackEnded -= new GameLogic.Events.CharacterMeleeAttackEnded(this.onCharacterMeleeAttackEnded);
            eventBus.OnCharacterSkillExecuted -= new GameLogic.Events.CharacterSkillExecuted(this.onCharacterSkillExecuted);
            eventBus.OnProjectilePreDestroy -= new GameLogic.Events.ProjectilePreDestroy(this.onProjectilePreDestroy);
            eventBus.OnDungeonBoostActivated -= new GameLogic.Events.DungeonBoostActivated(this.onDungeonBoostActivated);
        }

        private void onDungeonBoostActivated(DungeonBoost dungeonBoost, SkillType fromSkill)
        {
            if (dungeonBoost.Properties.Type == DungeonBoostType.EmptyBox)
            {
                ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                Player player = GameLogic.Binder.GameState.Player;
                double baseCoinReward = App.Binder.ConfigMeta.MinionCoinDropCurve(activeDungeon.Floor);
                double v = player.calculateStandardCoinRoll(baseCoinReward, GameLogic.CharacterType.UNSPECIFIED, 1) * App.Binder.ConfigMeta.DUNGEON_BOOST_EMPTY_BOX_COIN_GAIN_CONTROLLER;
                if (activeDungeon.hasDungeonModifier(DungeonModifierType.DungeonBoostBoxBonusCoins))
                {
                    v *= ConfigDungeonModifiers.DungeonBoostBoxBonusCoins.CoinMultiplier;
                }
                v = MathUtil.Clamp(v, 1.0, double.MaxValue);
                CmdGainResources.ExecuteStatic(player, ResourceType.Coin, v, true, "TRACKING_ID_GAMEPLAY_LOOT_GAIN", new Vector3?(dungeonBoost.Transform.position));
            }
        }

        protected void OnEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            eventBus.OnGameplayLoadingStarted += new GameLogic.Events.GameplayLoadingStarted(this.onGameplayLoadingStarted);
            eventBus.OnRoomCompleted += new GameLogic.Events.RoomCompleted(this.onRoomCompleted);
            eventBus.OnCharacterKilled += new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            eventBus.OnCharacterMeleeAttackContact += new GameLogic.Events.CharacterMeleeAttackContact(this.onCharacterMeleeAttackContact);
            eventBus.OnCharacterMeleeAttackEnded += new GameLogic.Events.CharacterMeleeAttackEnded(this.onCharacterMeleeAttackEnded);
            eventBus.OnCharacterSkillExecuted += new GameLogic.Events.CharacterSkillExecuted(this.onCharacterSkillExecuted);
            eventBus.OnProjectilePreDestroy += new GameLogic.Events.ProjectilePreDestroy(this.onProjectilePreDestroy);
            eventBus.OnDungeonBoostActivated += new GameLogic.Events.DungeonBoostActivated(this.onDungeonBoostActivated);
        }

        private void onGameplayLoadingStarted(ActiveDungeon ad)
        {
        }

        private void onGameStateInitialized()
        {
            GameLogic.Binder.GameState.Player.LastMysteryChestDropTimestamp = Service.Binder.ServerTime.GameTime;
        }

        private void onProjectilePreDestroy(Projectile projectile)
        {
            if (((projectile.OwningCharacter != null) && projectile.OwningCharacter.IsPlayerCharacter) && ((projectile.Properties.Type == ProjectileType.Slam) && (projectile.KillCounter >= ConfigMeta.MULTIKILL_REWARD_MIN_KILL_COUNT)))
            {
                this.grantMultikillReward(projectile.KillCounter);
            }
        }

        private void onRoomCompleted(Room room)
        {
        }

        public void triggerResourceExplosion(ResourceType resourceType, Vector3 centerWorldPos, double amountPerDrop, int dropCount, [Optional, DefaultParameterValue("")] string trackingId)
        {
            Player player = GameLogic.Binder.GameState.Player;
            for (int i = 0; i < dropCount; i++)
            {
                Vector2 normalized = UnityEngine.Random.insideUnitCircle.normalized;
                Vector3 vector2 = centerWorldPos + new Vector3(normalized.x, 0f, normalized.y);
                CmdGainResources.ExecuteStatic(player, resourceType, amountPerDrop, true, trackingId, new Vector3?(vector2));
            }
        }
    }
}

