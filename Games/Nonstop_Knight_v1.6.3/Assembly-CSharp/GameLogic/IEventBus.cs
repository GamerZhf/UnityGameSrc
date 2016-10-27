namespace GameLogic
{
    using Pathfinding;
    using Service;
    using System;
    using UnityEngine;

    public interface IEventBus
    {
        event GameLogic.Events.AchievementClaimed OnAchievementClaimed;

        event GameLogic.Events.ActiveLeagueChestTypeChanged OnActiveLeagueChestTypeChanged;

        event GameLogic.Events.AreaEffectPreDestroy OnAreaEffectPreDestroy;

        event GameLogic.Events.AreaEffectSpawned OnAreaEffectSpawned;

        event GameLogic.Events.BoostActivated OnBoostActivated;

        event GameLogic.Events.BoostStopped OnBoostStopped;

        event GameLogic.Events.BossTrainEnded OnBossTrainEnded;

        event GameLogic.Events.BossTrainStarted OnBossTrainStarted;

        event GameLogic.Events.BuffEnded OnBuffEnded;

        event GameLogic.Events.BuffPreEnd OnBuffPreEnd;

        event GameLogic.Events.BuffRefreshed OnBuffRefreshed;

        event GameLogic.Events.BuffStarted OnBuffStarted;

        event GameLogic.Events.CharacterAttackStopped OnCharacterAttackStopped;

        event GameLogic.Events.CharacterBlinked OnCharacterBlinked;

        event GameLogic.Events.CharacterCharmConditionChanged OnCharacterCharmConditionChanged;

        event GameLogic.Events.CharacterConfusedConditionChanged OnCharacterConfusedConditionChanged;

        event GameLogic.Events.CharacterDealtDamage OnCharacterDealtDamage;

        event GameLogic.Events.CharacterHordeSpawned OnCharacterHordeSpawned;

        event GameLogic.Events.CharacterHpGained OnCharacterHpGained;

        event GameLogic.Events.CharacterInterrupted OnCharacterInterrupted;

        event GameLogic.Events.CharacterKilled OnCharacterKilled;

        event GameLogic.Events.CharacterMeleeAttackContact OnCharacterMeleeAttackContact;

        event GameLogic.Events.CharacterMeleeAttackEnded OnCharacterMeleeAttackEnded;

        event GameLogic.Events.CharacterMeleeAttackStarted OnCharacterMeleeAttackStarted;

        event GameLogic.Events.CharacterPathPlanningUpdated OnCharacterPathPlanningUpdated;

        event GameLogic.Events.CharacterPreBlink OnCharacterPreBlink;

        event GameLogic.Events.CharacterPreDestroyed OnCharacterPreDestroyed;

        event GameLogic.Events.CharacterRangedAttackEnded OnCharacterRangedAttackEnded;

        event GameLogic.Events.CharacterRangedAttackStarted OnCharacterRangedAttackStarted;

        event GameLogic.Events.CharacterRankUpped OnCharacterRankUpped;

        event GameLogic.Events.CharacterRevived OnCharacterRevived;

        event GameLogic.Events.CharacterSkillActivated OnCharacterSkillActivated;

        event GameLogic.Events.CharacterSkillBuildupCompleted OnCharacterSkillBuildupCompleted;

        event GameLogic.Events.CharacterSkillCooldownEnded OnCharacterSkillCooldownEnded;

        event GameLogic.Events.CharacterSkillExecuted OnCharacterSkillExecuted;

        event GameLogic.Events.CharacterSkillExecutionMidpoint OnCharacterSkillExecutionMidpoint;

        event GameLogic.Events.CharacterSkillRankUpped OnCharacterSkillRankUpped;

        event GameLogic.Events.CharacterSkillsChanged OnCharacterSkillsChanged;

        event GameLogic.Events.CharacterSkillStopped OnCharacterSkillStopped;

        event GameLogic.Events.CharacterSpawned OnCharacterSpawned;

        event GameLogic.Events.CharacterSpawnStarted OnCharacterSpawnStarted;

        event GameLogic.Events.CharacterStunConditionChanged OnCharacterStunConditionChanged;

        event GameLogic.Events.CharacterTargetUpdated OnCharacterTargetUpdated;

        event GameLogic.Events.CharacterUnlocked OnCharacterUnlocked;

        event GameLogic.Events.CharacterUpgraded OnCharacterUpgraded;

        event GameLogic.Events.CharacterVelocityUpdated OnCharacterVelocityUpdated;

        event GameLogic.Events.DropLootTableRolled OnDropLootTableRolled;

        event GameLogic.Events.DungeonBoostActivated OnDungeonBoostActivated;

        event GameLogic.Events.DungeonBoostPreDestroy OnDungeonBoostPreDestroy;

        event GameLogic.Events.DungeonBoostSpawned OnDungeonBoostSpawned;

        event GameLogic.Events.DungeonDecosRefreshed OnDungeonDecosRefreshed;

        event GameLogic.Events.DungeonExplored OnDungeonExplored;

        event GameLogic.Events.FrenzyActivated OnFrenzyActivated;

        event GameLogic.Events.FrenzyDeactivated OnFrenzyDeactivated;

        event GameLogic.Events.GameplayEnded OnGameplayEnded;

        event GameLogic.Events.GameplayEndingStarted OnGameplayEndingStarted;

        event GameLogic.Events.GameplayLoadingStarted OnGameplayLoadingStarted;

        event GameLogic.Events.GameplayStarted OnGameplayStarted;

        event GameLogic.Events.GameplayStateChanged OnGameplayStateChanged;

        event GameLogic.Events.GameplayStateChangeStarted OnGameplayStateChangeStarted;

        event GameLogic.Events.GameplayTimeSlowdownToggled OnGameplayTimeSlowdownToggled;

        event GameLogic.Events.GameStateInitialized OnGameStateInitialized;

        event GameLogic.Events.ItemEquipped OnItemEquipped;

        event GameLogic.Events.ItemEvolved OnItemEvolved;

        event GameLogic.Events.ItemGained OnItemGained;

        event GameLogic.Events.ItemInspected OnItemInspected;

        event GameLogic.Events.ItemInstantUpgraded OnItemInstantUpgraded;

        event GameLogic.Events.ItemPerksRerolled OnItemPerksRerolled;

        event GameLogic.Events.ItemRankUpped OnItemRankUpped;

        event GameLogic.Events.ItemSold OnItemSold;

        event GameLogic.Events.ItemUnlocked OnItemUnlocked;

        event GameLogic.Events.LeaderboardOutperformed OnLeaderboardOutperformed;

        event GameLogic.Events.MissionEnded OnMissionEnded;

        event GameLogic.Events.MissionInspected OnMissionInspected;

        event GameLogic.Events.MissionStarted OnMissionStarted;

        event GameLogic.Events.MultikillBonusGranted OnMultikillBonusGranted;

        event GameLogic.Events.PassiveProgress OnPassiveProgress;

        event GameLogic.Events.PauseToggled OnPauseToggled;

        event GameLogic.Events.PetGained OnPetGained;

        event GameLogic.Events.PetInspected OnPetInspected;

        event GameLogic.Events.PetLevelUpped OnPetLevelUpped;

        event GameLogic.Events.PetSelected OnPetSelected;

        event GameLogic.Events.PlayerActiveCharacterChanged OnPlayerActiveCharacterChanged;

        event GameLogic.Events.PlayerActiveCharacterSwitched OnPlayerActiveCharacterSwitched;

        event GameLogic.Events.PlayerAugmentationGained OnPlayerAugmentationGained;

        event GameLogic.Events.PlayerAugmentationPurchased OnPlayerAugmentationPurchased;

        event GameLogic.Events.PlayerRankUpped OnPlayerRankUpped;

        event GameLogic.Events.PlayerRenamed OnPlayerRenamed;

        event GameLogic.Events.PlayerRetired OnPlayerRetired;

        event GameLogic.Events.PlayerSkillUpgradeGained OnPlayerSkillUpgradeGained;

        event GameLogic.Events.PlayerSkillUpgradePurchased OnPlayerSkillUpgradePurchased;

        event GameLogic.Events.PotionsGained OnPotionsGained;

        event GameLogic.Events.ProjectileCollided OnProjectileCollided;

        event GameLogic.Events.ProjectilePreDestroy OnProjectilePreDestroy;

        event GameLogic.Events.ProjectileSpawned OnProjectileSpawned;

        event GameLogic.Events.PromotionEventEnded OnPromotionEventEnded;

        event GameLogic.Events.PromotionEventInspected OnPromotionEventInspected;

        event GameLogic.Events.PromotionEventRefreshed OnPromotionEventRefreshed;

        event GameLogic.Events.PromotionEventStarted OnPromotionEventStarted;

        event GameLogic.Events.ResourcesGained OnResourcesGained;

        event GameLogic.Events.RewardConsumed OnRewardConsumed;

        event GameLogic.Events.RoomCompleted OnRoomCompleted;

        event GameLogic.Events.RoomLoaded OnRoomLoaded;

        event GameLogic.Events.RunestoneGained OnRunestoneGained;

        event GameLogic.Events.RunestoneInspected OnRunestoneInspected;

        event GameLogic.Events.RunestoneLevelUpped OnRunestoneLevelUpped;

        event GameLogic.Events.RunestoneRankUpped OnRunestoneRankUpped;

        event GameLogic.Events.RunestoneSelected OnRunestoneSelected;

        event GameLogic.Events.RunestoneUnlocked OnRunestoneUnlocked;

        event GameLogic.Events.SkillInspected OnSkillInspected;

        event GameLogic.Events.SuspectedSystemClockCheat OnSuspectedSystemClockCheat;

        event GameLogic.Events.TimescaleChanged OnTimescaleChanged;

        event GameLogic.Events.TimescaleChangeStarted OnTimescaleChangeStarted;

        event GameLogic.Events.TournamentDonationMade OnTournamentDonationMade;

        event GameLogic.Events.TournamentSelected OnTournamentSelected;

        event GameLogic.Events.TournamentUpgradeGained OnTournamentUpgradeGained;

        event GameLogic.Events.TutorialCompleted OnTutorialCompleted;

        event GameLogic.Events.TutorialStarted OnTutorialStarted;

        event GameLogic.Events.VendorInventoryInspected OnVendorInventoryInspected;

        event GameLogic.Events.VendorInventoryRefreshed OnVendorInventoryRefreshed;

        void AchievementClaimed(Player player, string achievementId, int tier);
        void ActiveLeagueChestTypeChanged(Player player);
        void AreaEffectPreDestroy(AreaEffect areaEffect);
        void AreaEffectSpawned(AreaEffect areaEffect);
        void BoostActivated(Player player, BoostType boost, string resourceEventItemId);
        void BoostStopped(Player player, BoostType boost);
        void BossTrainEnded(Player player, int numCharges, int numBossesKilled);
        void BossTrainStarted(Player player, int numCharges);
        void BuffEnded(CharacterInstance character, Buff buff);
        void BuffPreEnd(CharacterInstance character, Buff buff);
        void BuffRefreshed(CharacterInstance character, Buff buff);
        void BuffStarted(CharacterInstance character, Buff buff);
        void CharacterAttackStopped(CharacterInstance character);
        void CharacterBlinked(CharacterInstance character);
        void CharacterCharmConditionChanged(CharacterInstance character);
        void CharacterConfusedConditionChanged(CharacterInstance character);
        void CharacterDealtDamage(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, Vector3 worldPos, double amount, bool critted, bool damageReduced, DamageType damageType, SkillType fromSkill);
        void CharacterHordeSpawned(Room.Spawnpoint spawnpoint, bool isBoss);
        void CharacterHpGained(CharacterInstance character, double amount, bool silent);
        void CharacterInterrupted(CharacterInstance character, bool stopSkills);
        void CharacterKilled_Direct(CharacterInstance target, CharacterInstance killer, bool critted, SkillType fromSkill);
        void CharacterKilled_Queued(CharacterInstance target, CharacterInstance killer, bool critted, SkillType fromSkill);
        void CharacterMeleeAttackContact(CharacterInstance character, Vector3 contactWorldPt, bool critted);
        void CharacterMeleeAttackEnded(CharacterInstance character, CharacterInstance targetCharacter, Vector3 contactWorldPt, int killCount);
        void CharacterMeleeAttackStarted(CharacterInstance sourceCharacter, CharacterInstance targetCharacter);
        void CharacterPathPlanningUpdated(CharacterInstance character, Path path);
        void CharacterPreBlink(CharacterInstance character);
        void CharacterPreDestroyed(CharacterInstance character);
        void CharacterRangedAttackEnded(CharacterInstance sourceCharacter, Vector3 targetWorldPt, Projectile projectile);
        void CharacterRangedAttackStarted(CharacterInstance sourceCharacter, Vector3 targetWorldPt);
        void CharacterRankUpped(CharacterInstance character);
        void CharacterRevived(CharacterInstance character);
        void CharacterSkillActivated(CharacterInstance character, SkillType skillType, float buildupTime, SkillExecutionStats executionStats);
        void CharacterSkillBuildupCompleted(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats);
        void CharacterSkillCooldownEnded(CharacterInstance character, SkillType skillType);
        void CharacterSkillExecuted(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats);
        void CharacterSkillExecutionMidpoint(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats);
        void CharacterSkillRankUpped(CharacterInstance character, SkillInstance skillInstance);
        void CharacterSkillsChanged(CharacterInstance character);
        void CharacterSkillStopped(CharacterInstance character, SkillType skillType);
        void CharacterSpawned(CharacterInstance character);
        void CharacterSpawnStarted(CharacterInstance character);
        void CharacterStunConditionChanged(CharacterInstance character);
        void CharacterTargetUpdated(CharacterInstance character, CharacterInstance oldTarget);
        void CharacterUnlocked(CharacterInstance character);
        void CharacterUpgraded(CharacterInstance character);
        void CharacterVelocityUpdated(CharacterInstance character);
        void DropLootTableRolled(LootTable lootTable, Vector3 worldPos, Reward drop);
        void DungeonBoostActivated(DungeonBoost dungeonBoost, SkillType fromSkill);
        void DungeonBoostPreDestroy(DungeonBoost dungeonBoost);
        void DungeonBoostSpawned(DungeonBoost dungeonBoost);
        void DungeonDecosRefreshed();
        void DungeonExplored(CharacterInstance character, string dungeonId);
        void FrenzyActivated();
        void FrenzyDeactivated();
        void GameplayEnded(ActiveDungeon activeDungeon);
        void GameplayEndingStarted(ActiveDungeon activeDungeon);
        void GameplayLoadingStarted(ActiveDungeon activeDungeon);
        void GameplayStarted(ActiveDungeon activeDungeon);
        void GameplayStateChanged(GameplayState previousState, GameplayState currentState);
        void GameplayStateChangeStarted(GameplayState fromState, GameplayState targetState, float transitionDelay);
        void GameplayTimeSlowdownToggled(bool enabled);
        void GameStateInitialized();
        void ItemEquipped(CharacterInstance character, ItemInstance itemInstance, ItemInstance replacedItemInstance);
        void ItemEvolved(CharacterInstance character, ItemInstance itemInstance);
        void ItemGained(CharacterInstance character, ItemInstance itemInstance, string trackingId);
        void ItemInspected(ItemInstance itemInstance);
        void ItemInstantUpgraded(ItemInstance itemInstance, double diamondCost);
        void ItemPerksRerolled(Player player, ItemInstance itemInstance, double diamondCost);
        void ItemRankUpped(CharacterInstance character, ItemInstance itemInstance, int rankUpCount, bool free);
        void ItemSold(CharacterInstance character, ItemInstance itemInstance, double value, RectTransform flyToHudOrigin);
        void ItemUnlocked(CharacterInstance character, ItemInstance itemInstance);
        void LeaderboardOutperformed(LeaderboardType leaderboardType);
        void MissionEnded(Player player, MissionInstance mission, bool success);
        void MissionInspected(Player player, MissionInstance mission);
        void MissionStarted(Player player, MissionInstance mission);
        void MultikillBonusGranted(Player player, int killCount, double coinAmount);
        void PassiveProgress(Player player, int numPassiveMinionKills, int numPassiveFloorCompletions, int numPassiveBossKills);
        void PauseToggled(bool enabled);
        void PetGained(Player player, string petId, bool cheated);
        void PetInspected(Player player, string petId, bool cheated);
        void PetLevelUpped(Player player, string petId, bool cheated);
        void PetSelected(Player player, PetInstance pet);
        void PlayerActiveCharacterChanged(CharacterInstance activeCharacter);
        void PlayerActiveCharacterSwitched(CharacterInstance activeCharacter);
        void PlayerAugmentationGained(Player player, string id);
        void PlayerAugmentationPurchased(Player player, string id, ResourceType spentResource, double price);
        void PlayerRankUpped(Player player, bool cheated);
        void PlayerRenamed(Player player);
        void PlayerRetired(Player player, int retirementFloor);
        void PlayerSkillUpgradeGained(Player player, string id);
        void PlayerSkillUpgradePurchased(Player player, string id, ResourceType spentResource, double price);
        void PotionsGained(CharacterInstance character, PotionType potionType, int amount);
        void ProjectileCollided(Projectile projectile, Collider collider);
        void ProjectilePreDestroy(Projectile projectile);
        void ProjectileSpawned(Projectile projectile);
        void PromotionEventEnded(Player player, string promotionId);
        void PromotionEventInspected(Player player, string promotionId);
        void PromotionEventRefreshed(Player player, string promotionId);
        void PromotionEventStarted(Player player, string promotionId);
        void ResourcesGained(Player player, ResourceType resourceType, double amount, bool instant, string trackingId, Vector3? worldPt);
        void RewardConsumed(Player player, Reward reward);
        void RoomCompleted(Room room);
        void RoomLoaded(Room room);
        void RunestoneGained(Player player, RunestoneInstance runestone, bool cheated);
        void RunestoneInspected(Player player, RunestoneInstance runestone);
        void RunestoneLevelUpped(Player player, RunestoneInstance runestone);
        void RunestoneRankUpped(Player player, RunestoneInstance runestone);
        void RunestoneSelected(Player player, RunestoneInstance runestone);
        void RunestoneUnlocked(Player player, RunestoneInstance runestone);
        void SkillInspected(SkillInstance skillInstance);
        void SuspectedSystemClockCheat(long timeOffsetSeconds);
        void TimescaleChanged(float timescale);
        void TimescaleChangeStarted(float targetTimescale);
        void TournamentDonationMade(Player player, TournamentInstance tournament, int count, double totalPrice);
        void TournamentSelected(Player player, TournamentInstance selectedTournament, TournamentInstance unselectedTournament);
        void TournamentUpgradeGained(Player player, string id, bool epicVersion, int numMilestonesCompleted);
        void TutorialCompleted(Player player, string tutorialId);
        void TutorialStarted(Player player, string tutorialId);
        void VendorInventoryInspected(Player player);
        void VendorInventoryRefreshed(Player player);

        bool ProcessingQueuedEvents { get; }
    }
}

