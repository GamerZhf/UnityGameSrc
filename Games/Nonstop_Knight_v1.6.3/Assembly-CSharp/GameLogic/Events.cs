namespace GameLogic
{
    using Pathfinding;
    using Service;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Events
    {
        public delegate void AchievementClaimed(Player player, string achievementId, int tier);

        public delegate void ActiveLeagueChestTypeChanged(Player player);

        public delegate void AreaEffectPreDestroy(AreaEffect areaEffect);

        public delegate void AreaEffectSpawned(AreaEffect areaEffect);

        public delegate void BoostActivated(Player player, BoostType boost, string analyticsSourceId);

        public delegate void BoostStopped(Player player, BoostType boost);

        public delegate void BossTrainEnded(Player player, int numCharges, int numBossesKilled);

        public delegate void BossTrainStarted(Player player, int numCharges);

        public delegate void BuffEnded(CharacterInstance character, Buff buff);

        public delegate void BuffPreEnd(CharacterInstance character, Buff buff);

        public delegate void BuffRefreshed(CharacterInstance character, Buff buff);

        public delegate void BuffStarted(CharacterInstance character, Buff buff);

        public delegate void CharacterAttackStopped(CharacterInstance character);

        public delegate void CharacterBlinked(CharacterInstance character);

        public delegate void CharacterCharmConditionChanged(CharacterInstance character);

        public delegate void CharacterConfusedConditionChanged(CharacterInstance character);

        public delegate void CharacterDealtDamage(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, Vector3 worldPos, double amount, bool critted, bool damageReduced, DamageType damageType, SkillType fromSkill);

        public delegate void CharacterGainedXp(CharacterInstance character, int amount);

        public delegate void CharacterHordeSpawned(Room.Spawnpoint spawnpoint, bool isBoss);

        public delegate void CharacterHpGained(CharacterInstance character, double amount, bool silent);

        public delegate void CharacterInterrupted(CharacterInstance character, bool stopSkills);

        public delegate void CharacterKilled(CharacterInstance target, CharacterInstance killer, bool critted, SkillType fromSkill);

        public delegate void CharacterManualTargetUpdated(CharacterInstance character, Vector3 targetWorldPt);

        public delegate void CharacterMeleeAttackContact(CharacterInstance sourceCharacter, Vector3 contactWorldPt, bool critted);

        public delegate void CharacterMeleeAttackEnded(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, Vector3 contactWorldPt, int killCount);

        public delegate void CharacterMeleeAttackStarted(CharacterInstance sourceCharacter, CharacterInstance targetCharacter);

        public delegate void CharacterPathPlanningUpdated(CharacterInstance character, Path path);

        public delegate void CharacterPreBlink(CharacterInstance character);

        public delegate void CharacterPreDestroyed(CharacterInstance character);

        public delegate void CharacterRangedAttackEnded(CharacterInstance sourceCharacter, Vector3 targetWorldPt, Projectile projectile);

        public delegate void CharacterRangedAttackStarted(CharacterInstance sourceCharacter, Vector3 targetWorldPt);

        public delegate void CharacterRankUpped(CharacterInstance character);

        public delegate void CharacterRevived(CharacterInstance character);

        public delegate void CharacterSkillActivated(CharacterInstance character, SkillType skillType, float buildupTime, SkillExecutionStats executionStats);

        public delegate void CharacterSkillBuildupCompleted(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats);

        public delegate void CharacterSkillCooldownEnded(CharacterInstance character, SkillType skillType);

        public delegate void CharacterSkillExecuted(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats);

        public delegate void CharacterSkillExecutionMidpoint(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats);

        public delegate void CharacterSkillRankUpped(CharacterInstance character, SkillInstance skillInstance);

        public delegate void CharacterSkillsChanged(CharacterInstance character);

        public delegate void CharacterSkillStopped(CharacterInstance character, SkillType skillType);

        public delegate void CharacterSpawned(CharacterInstance character);

        public delegate void CharacterSpawnStarted(CharacterInstance character);

        public delegate void CharacterStunConditionChanged(CharacterInstance character);

        public delegate void CharacterTargetUpdated(CharacterInstance character, CharacterInstance oldTarget);

        public delegate void CharacterUnlocked(CharacterInstance character);

        public delegate void CharacterUpgraded(CharacterInstance character);

        public delegate void CharacterVelocityUpdated(CharacterInstance character);

        public delegate void DropLootTableRolled(LootTable lootTable, Vector3 worldPos, Reward drop);

        public delegate void DungeonBoostActivated(DungeonBoost dungeonBoost, SkillType fromSkill);

        public delegate void DungeonBoostPreDestroy(DungeonBoost dungeonBoost);

        public delegate void DungeonBoostSpawned(DungeonBoost dungeonBoost);

        public delegate void DungeonDecosRefreshed();

        public delegate void DungeonExplored(CharacterInstance character, string dungeonId);

        public delegate void FrenzyActivated();

        public delegate void FrenzyDeactivated();

        public delegate void GameplayEnded(ActiveDungeon activeDungeon);

        public delegate void GameplayEndingStarted(ActiveDungeon activeDungeon);

        public delegate void GameplayLoadingStarted(ActiveDungeon activeDungeon);

        public delegate void GameplayStarted(ActiveDungeon activeDungeon);

        public delegate void GameplayStateChanged(GameplayState previousState, GameplayState currentState);

        public delegate void GameplayStateChangeStarted(GameplayState fromState, GameplayState targetState, float transitionDelay);

        public delegate void GameplayTimeSlowdownToggled(bool enabled);

        public delegate void GameStateInitialized();

        public delegate void ItemEquipped(CharacterInstance character, ItemInstance itemInstance, ItemInstance replacedItemInstance);

        public delegate void ItemEvolved(CharacterInstance character, ItemInstance itemInstance);

        public delegate void ItemGained(CharacterInstance character, ItemInstance itemInstance, string analyticsSourceId);

        public delegate void ItemInspected(ItemInstance itemInstance);

        public delegate void ItemInstantUpgraded(ItemInstance itemInstance, double diamondCost);

        public delegate void ItemPerksRerolled(Player player, ItemInstance itemInstance, double diamondCost);

        public delegate void ItemRankUpped(CharacterInstance character, ItemInstance itemInstance, int rankUpCount, bool free);

        public delegate void ItemSold(CharacterInstance character, ItemInstance itemInstance, double value, RectTransform flyToHudOrigin);

        public delegate void ItemUnlocked(CharacterInstance character, ItemInstance itemInstance);

        public delegate void LeaderboardOutperformed(LeaderboardType leaderboardType);

        public delegate void MissionEnded(Player player, MissionInstance mission, bool success);

        public delegate void MissionInspected(Player player, MissionInstance mission);

        public delegate void MissionStarted(Player player, MissionInstance mission);

        public delegate void MultikillBonusGranted(Player player, int killCount, double coinAmount);

        public delegate void PassiveProgress(Player player, int numPassiveMinionKills, int numPassiveFloorProgressions, int numPassiveBossKills);

        public delegate void PauseToggled(bool enabled);

        public delegate void PetGained(Player player, string petId, bool cheated);

        public delegate void PetInspected(Player player, string petId, bool cheated);

        public delegate void PetLevelUpped(Player player, string petId, bool cheated);

        public delegate void PetSelected(Player player, PetInstance pet);

        public delegate void PlayerActiveCharacterChanged(CharacterInstance activeCharacter);

        public delegate void PlayerActiveCharacterSwitched(CharacterInstance activeCharacter);

        public delegate void PlayerAugmentationGained(Player player, string id);

        public delegate void PlayerAugmentationPurchased(Player player, string id, ResourceType spentResource, double price);

        public delegate void PlayerRankUpped(Player player, bool cheated);

        public delegate void PlayerRenamed(Player player);

        public delegate void PlayerRetired(Player player, int retirementFloor);

        public delegate void PlayerSkillUpgradeGained(Player player, string id);

        public delegate void PlayerSkillUpgradePurchased(Player player, string id, ResourceType spentResource, double price);

        public delegate void PlayerXpGained(Player player, double amount, Vector3 worldPt);

        public delegate void PotionsGained(CharacterInstance character, PotionType potionType, int amount);

        public delegate void ProjectileCollided(Projectile projectile, Collider collider);

        public delegate void ProjectilePreDestroy(Projectile projectile);

        public delegate void ProjectileSpawned(Projectile projectile);

        public delegate void PromotionEventEnded(Player player, string promotionId);

        public delegate void PromotionEventInspected(Player player, string promotionId);

        public delegate void PromotionEventRefreshed(Player player, string promotionId);

        public delegate void PromotionEventStarted(Player player, string promotionId);

        public delegate void ResourcesGained(Player player, ResourceType type, double amount, bool instant, string analyticsSourceOrSinkId, Vector3? worldPt);

        public delegate void RewardConsumed(Player player, Reward reward);

        public delegate void RoomCleanedUp(Room room);

        public delegate void RoomCompleted(Room room);

        public delegate void RoomLoaded(Room room);

        public delegate void RunestoneGained(Player player, RunestoneInstance runestone, bool cheated);

        public delegate void RunestoneInspected(Player player, RunestoneInstance runestone);

        public delegate void RunestoneLevelUpped(Player player, RunestoneInstance runestone);

        public delegate void RunestoneRankUpped(Player player, RunestoneInstance runestone);

        public delegate void RunestoneSelected(Player player, RunestoneInstance runestone);

        public delegate void RunestoneUnlocked(Player player, RunestoneInstance runestone);

        public delegate void SkillInspected(SkillInstance skillInstance);

        public delegate void SuspectedSystemClockCheat(long timeOffsetSeconds);

        public delegate void TimescaleChanged(float timescale);

        public delegate void TimescaleChangeStarted(float targetTimescale);

        public delegate void TournamentDonationMade(Player player, TournamentInstance tournament, int count, double totalPrice);

        public delegate void TournamentSelected(Player player, TournamentInstance selectedTournament, TournamentInstance unselectedTournament);

        public delegate void TournamentUpgradeGained(Player player, string id, bool epicVersion, int numMilestonesCompleted);

        public delegate void TutorialCompleted(Player player, string tutorialId);

        public delegate void TutorialStarted(Player player, string tutorialId);

        public delegate void VendorInventoryInspected(Player player);

        public delegate void VendorInventoryRefreshed(Player player);
    }
}

