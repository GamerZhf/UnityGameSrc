namespace GameLogic
{
    using App;
    using Pathfinding;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class EventBus : MonoBehaviour, GameLogic.IEventBus
    {
        [CompilerGenerated]
        private bool <ProcessingQueuedEvents>k__BackingField;
        private Stopwatch m_frameTimer = new Stopwatch();
        private QueuedEventHandlers m_onCharacterKilledHandlers = new QueuedEventHandlers();
        private Coroutine m_queuedEventProcessingRoutine;
        private Queue<QueuedEventData> m_queuedEvents = new Queue<QueuedEventData>(0x80);

        public event GameLogic.Events.AchievementClaimed OnAchievementClaimed;

        public event GameLogic.Events.ActiveLeagueChestTypeChanged OnActiveLeagueChestTypeChanged;

        public event GameLogic.Events.AreaEffectPreDestroy OnAreaEffectPreDestroy;

        public event GameLogic.Events.AreaEffectSpawned OnAreaEffectSpawned;

        public event GameLogic.Events.BoostActivated OnBoostActivated;

        public event GameLogic.Events.BoostStopped OnBoostStopped;

        public event GameLogic.Events.BossTrainEnded OnBossTrainEnded;

        public event GameLogic.Events.BossTrainStarted OnBossTrainStarted;

        public event GameLogic.Events.BuffEnded OnBuffEnded;

        public event GameLogic.Events.BuffPreEnd OnBuffPreEnd;

        public event GameLogic.Events.BuffRefreshed OnBuffRefreshed;

        public event GameLogic.Events.BuffStarted OnBuffStarted;

        public event GameLogic.Events.CharacterAttackStopped OnCharacterAttackStopped;

        public event GameLogic.Events.CharacterBlinked OnCharacterBlinked;

        public event GameLogic.Events.CharacterCharmConditionChanged OnCharacterCharmConditionChanged;

        public event GameLogic.Events.CharacterConfusedConditionChanged OnCharacterConfusedConditionChanged;

        public event GameLogic.Events.CharacterDealtDamage OnCharacterDealtDamage;

        public event GameLogic.Events.CharacterHordeSpawned OnCharacterHordeSpawned;

        public event GameLogic.Events.CharacterHpGained OnCharacterHpGained;

        public event GameLogic.Events.CharacterInterrupted OnCharacterInterrupted;

        public event GameLogic.Events.CharacterKilled OnCharacterKilled
        {
            add
            {
                this.m_onCharacterKilledHandlers.register(value);
            }
            remove
            {
                this.m_onCharacterKilledHandlers.unregister(value);
            }
        }

        public event GameLogic.Events.CharacterManualTargetUpdated OnCharacterManualTargetUpdated;

        public event GameLogic.Events.CharacterMeleeAttackContact OnCharacterMeleeAttackContact;

        public event GameLogic.Events.CharacterMeleeAttackEnded OnCharacterMeleeAttackEnded;

        public event GameLogic.Events.CharacterMeleeAttackStarted OnCharacterMeleeAttackStarted;

        public event GameLogic.Events.CharacterPathPlanningUpdated OnCharacterPathPlanningUpdated;

        public event GameLogic.Events.CharacterPreBlink OnCharacterPreBlink;

        public event GameLogic.Events.CharacterPreDestroyed OnCharacterPreDestroyed;

        public event GameLogic.Events.CharacterRangedAttackEnded OnCharacterRangedAttackEnded;

        public event GameLogic.Events.CharacterRangedAttackStarted OnCharacterRangedAttackStarted;

        public event GameLogic.Events.CharacterRankUpped OnCharacterRankUpped;

        public event GameLogic.Events.CharacterRevived OnCharacterRevived;

        public event GameLogic.Events.CharacterSkillActivated OnCharacterSkillActivated;

        public event GameLogic.Events.CharacterSkillBuildupCompleted OnCharacterSkillBuildupCompleted;

        public event GameLogic.Events.CharacterSkillCooldownEnded OnCharacterSkillCooldownEnded;

        public event GameLogic.Events.CharacterSkillExecuted OnCharacterSkillExecuted;

        public event GameLogic.Events.CharacterSkillExecutionMidpoint OnCharacterSkillExecutionMidpoint;

        public event GameLogic.Events.CharacterSkillRankUpped OnCharacterSkillRankUpped;

        public event GameLogic.Events.CharacterSkillsChanged OnCharacterSkillsChanged;

        public event GameLogic.Events.CharacterSkillStopped OnCharacterSkillStopped;

        public event GameLogic.Events.CharacterSpawned OnCharacterSpawned;

        public event GameLogic.Events.CharacterSpawnStarted OnCharacterSpawnStarted;

        public event GameLogic.Events.CharacterStunConditionChanged OnCharacterStunConditionChanged;

        public event GameLogic.Events.CharacterTargetUpdated OnCharacterTargetUpdated;

        public event GameLogic.Events.CharacterUnlocked OnCharacterUnlocked;

        public event GameLogic.Events.CharacterUpgraded OnCharacterUpgraded;

        public event GameLogic.Events.CharacterVelocityUpdated OnCharacterVelocityUpdated;

        public event GameLogic.Events.DropLootTableRolled OnDropLootTableRolled;

        public event GameLogic.Events.DungeonBoostActivated OnDungeonBoostActivated;

        public event GameLogic.Events.DungeonBoostPreDestroy OnDungeonBoostPreDestroy;

        public event GameLogic.Events.DungeonBoostSpawned OnDungeonBoostSpawned;

        public event GameLogic.Events.DungeonDecosRefreshed OnDungeonDecosRefreshed;

        public event GameLogic.Events.DungeonExplored OnDungeonExplored;

        public event GameLogic.Events.FrenzyActivated OnFrenzyActivated;

        public event GameLogic.Events.FrenzyDeactivated OnFrenzyDeactivated;

        public event GameLogic.Events.GameplayEnded OnGameplayEnded;

        public event GameLogic.Events.GameplayEndingStarted OnGameplayEndingStarted;

        public event GameLogic.Events.GameplayLoadingStarted OnGameplayLoadingStarted;

        public event GameLogic.Events.GameplayStarted OnGameplayStarted;

        public event GameLogic.Events.GameplayStateChanged OnGameplayStateChanged;

        public event GameLogic.Events.GameplayStateChangeStarted OnGameplayStateChangeStarted;

        public event GameLogic.Events.GameplayTimeSlowdownToggled OnGameplayTimeSlowdownToggled;

        public event GameLogic.Events.GameStateInitialized OnGameStateInitialized;

        public event GameLogic.Events.ItemEquipped OnItemEquipped;

        public event GameLogic.Events.ItemEvolved OnItemEvolved;

        public event GameLogic.Events.ItemGained OnItemGained;

        public event GameLogic.Events.ItemInspected OnItemInspected;

        public event GameLogic.Events.ItemInstantUpgraded OnItemInstantUpgraded;

        public event GameLogic.Events.ItemPerksRerolled OnItemPerksRerolled;

        public event GameLogic.Events.ItemRankUpped OnItemRankUpped;

        public event GameLogic.Events.ItemSold OnItemSold;

        public event GameLogic.Events.ItemUnlocked OnItemUnlocked;

        public event GameLogic.Events.LeaderboardOutperformed OnLeaderboardOutperformed;

        public event GameLogic.Events.MissionEnded OnMissionEnded;

        public event GameLogic.Events.MissionInspected OnMissionInspected;

        public event GameLogic.Events.MissionStarted OnMissionStarted;

        public event GameLogic.Events.MultikillBonusGranted OnMultikillBonusGranted;

        public event GameLogic.Events.PassiveProgress OnPassiveProgress;

        public event GameLogic.Events.PauseToggled OnPauseToggled;

        public event GameLogic.Events.PetGained OnPetGained;

        public event GameLogic.Events.PetInspected OnPetInspected;

        public event GameLogic.Events.PetLevelUpped OnPetLevelUpped;

        public event GameLogic.Events.PetSelected OnPetSelected;

        public event GameLogic.Events.PlayerActiveCharacterChanged OnPlayerActiveCharacterChanged;

        public event GameLogic.Events.PlayerActiveCharacterSwitched OnPlayerActiveCharacterSwitched;

        public event GameLogic.Events.PlayerAugmentationGained OnPlayerAugmentationGained;

        public event GameLogic.Events.PlayerAugmentationPurchased OnPlayerAugmentationPurchased;

        public event GameLogic.Events.PlayerRankUpped OnPlayerRankUpped;

        public event GameLogic.Events.PlayerRenamed OnPlayerRenamed;

        public event GameLogic.Events.PlayerRetired OnPlayerRetired;

        public event GameLogic.Events.PlayerSkillUpgradeGained OnPlayerSkillUpgradeGained;

        public event GameLogic.Events.PlayerSkillUpgradePurchased OnPlayerSkillUpgradePurchased;

        public event GameLogic.Events.PotionsGained OnPotionsGained;

        public event GameLogic.Events.ProjectileCollided OnProjectileCollided;

        public event GameLogic.Events.ProjectilePreDestroy OnProjectilePreDestroy;

        public event GameLogic.Events.ProjectileSpawned OnProjectileSpawned;

        public event GameLogic.Events.PromotionEventEnded OnPromotionEventEnded;

        public event GameLogic.Events.PromotionEventInspected OnPromotionEventInspected;

        public event GameLogic.Events.PromotionEventRefreshed OnPromotionEventRefreshed;

        public event GameLogic.Events.PromotionEventStarted OnPromotionEventStarted;

        public event GameLogic.Events.ResourcesGained OnResourcesGained;

        public event GameLogic.Events.RewardConsumed OnRewardConsumed;

        public event GameLogic.Events.RoomCompleted OnRoomCompleted;

        public event GameLogic.Events.RoomLoaded OnRoomLoaded;

        public event GameLogic.Events.RunestoneGained OnRunestoneGained;

        public event GameLogic.Events.RunestoneInspected OnRunestoneInspected;

        public event GameLogic.Events.RunestoneLevelUpped OnRunestoneLevelUpped;

        public event GameLogic.Events.RunestoneRankUpped OnRunestoneRankUpped;

        public event GameLogic.Events.RunestoneSelected OnRunestoneSelected;

        public event GameLogic.Events.RunestoneUnlocked OnRunestoneUnlocked;

        public event GameLogic.Events.SkillInspected OnSkillInspected;

        public event GameLogic.Events.SuspectedSystemClockCheat OnSuspectedSystemClockCheat;

        public event GameLogic.Events.TimescaleChanged OnTimescaleChanged;

        public event GameLogic.Events.TimescaleChangeStarted OnTimescaleChangeStarted;

        public event GameLogic.Events.TournamentDonationMade OnTournamentDonationMade;

        public event GameLogic.Events.TournamentSelected OnTournamentSelected;

        public event GameLogic.Events.TournamentUpgradeGained OnTournamentUpgradeGained;

        public event GameLogic.Events.TutorialCompleted OnTutorialCompleted;

        public event GameLogic.Events.TutorialStarted OnTutorialStarted;

        public event GameLogic.Events.VendorInventoryInspected OnVendorInventoryInspected;

        public event GameLogic.Events.VendorInventoryRefreshed OnVendorInventoryRefreshed;

        public void AchievementClaimed(Player player, string achievementId, int tier)
        {
            if (this.OnAchievementClaimed != null)
            {
                this.OnAchievementClaimed(player, achievementId, tier);
            }
        }

        public void ActiveLeagueChestTypeChanged(Player player)
        {
            if (this.OnActiveLeagueChestTypeChanged != null)
            {
                this.OnActiveLeagueChestTypeChanged(player);
            }
        }

        public void AreaEffectPreDestroy(AreaEffect areaEffect)
        {
            if (this.OnAreaEffectPreDestroy != null)
            {
                this.OnAreaEffectPreDestroy(areaEffect);
            }
        }

        public void AreaEffectSpawned(AreaEffect areaEffect)
        {
            if (this.OnAreaEffectSpawned != null)
            {
                this.OnAreaEffectSpawned(areaEffect);
            }
        }

        public void BoostActivated(Player player, BoostType boost, string analyticsSourceId)
        {
            if (this.OnBoostActivated != null)
            {
                this.OnBoostActivated(player, boost, analyticsSourceId);
            }
        }

        public void BoostStopped(Player player, BoostType boost)
        {
            if (this.OnBoostStopped != null)
            {
                this.OnBoostStopped(player, boost);
            }
        }

        public void BossTrainEnded(Player player, int numCharges, int numBossesKilled)
        {
            if (this.OnBossTrainEnded != null)
            {
                this.OnBossTrainEnded(player, numCharges, numBossesKilled);
            }
        }

        public void BossTrainStarted(Player player, int numCharges)
        {
            if (this.OnBossTrainStarted != null)
            {
                this.OnBossTrainStarted(player, numCharges);
            }
        }

        public void BuffEnded(CharacterInstance character, Buff buff)
        {
            if (this.OnBuffEnded != null)
            {
                this.OnBuffEnded(character, buff);
            }
        }

        public void BuffPreEnd(CharacterInstance character, Buff buff)
        {
            if (this.OnBuffPreEnd != null)
            {
                this.OnBuffPreEnd(character, buff);
            }
        }

        public void BuffRefreshed(CharacterInstance character, Buff buff)
        {
            if (this.OnBuffRefreshed != null)
            {
                this.OnBuffRefreshed(character, buff);
            }
        }

        public void BuffStarted(CharacterInstance character, Buff buff)
        {
            if (this.OnBuffStarted != null)
            {
                this.OnBuffStarted(character, buff);
            }
        }

        public void CharacterAttackStopped(CharacterInstance character)
        {
            if (this.OnCharacterAttackStopped != null)
            {
                this.OnCharacterAttackStopped(character);
            }
        }

        public void CharacterBlinked(CharacterInstance character)
        {
            if (this.OnCharacterBlinked != null)
            {
                this.OnCharacterBlinked(character);
            }
        }

        public void CharacterCharmConditionChanged(CharacterInstance character)
        {
            if (this.OnCharacterCharmConditionChanged != null)
            {
                this.OnCharacterCharmConditionChanged(character);
            }
        }

        public void CharacterConfusedConditionChanged(CharacterInstance character)
        {
            if (this.OnCharacterConfusedConditionChanged != null)
            {
                this.OnCharacterConfusedConditionChanged(character);
            }
        }

        public void CharacterDealtDamage(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, Vector3 worldPos, double amount, bool critted, bool damageReduced, DamageType damageType, SkillType fromSkill)
        {
            if (this.OnCharacterDealtDamage != null)
            {
                this.OnCharacterDealtDamage(sourceCharacter, targetCharacter, worldPos, amount, critted, damageReduced, damageType, fromSkill);
            }
        }

        public void CharacterHordeSpawned(Room.Spawnpoint spawnpoint, bool isBoss)
        {
            if (this.OnCharacterHordeSpawned != null)
            {
                this.OnCharacterHordeSpawned(spawnpoint, isBoss);
            }
        }

        public void CharacterHpGained(CharacterInstance character, double amount, bool silent)
        {
            if (this.OnCharacterHpGained != null)
            {
                this.OnCharacterHpGained(character, amount, silent);
            }
        }

        public void CharacterInterrupted(CharacterInstance character, bool stopSkills)
        {
            if (this.OnCharacterInterrupted != null)
            {
                this.OnCharacterInterrupted(character, stopSkills);
            }
        }

        public void CharacterKilled_Direct(CharacterInstance target, CharacterInstance killer, bool critted, SkillType fromSkill)
        {
            for (int i = 0; i < this.m_onCharacterKilledHandlers.Delegates.Count; i++)
            {
                ((GameLogic.Events.CharacterKilled) this.m_onCharacterKilledHandlers.Delegates[i])(target, killer, critted, fromSkill);
            }
        }

        public void CharacterKilled_Queued(CharacterInstance target, CharacterInstance killer, bool critted, SkillType fromSkill)
        {
            QueuedEvent_CharacterKilled item = new QueuedEvent_CharacterKilled();
            item.Handlers = this.m_onCharacterKilledHandlers.Delegates;
            item.Param1 = target;
            item.Param2 = killer;
            item.Param3 = critted;
            item.Param4 = fromSkill;
            this.m_queuedEvents.Enqueue(item);
        }

        public void CharacterManualTargetUpdated(CharacterInstance character, Vector3 targetWorldPt)
        {
            if (this.OnCharacterManualTargetUpdated != null)
            {
                this.OnCharacterManualTargetUpdated(character, targetWorldPt);
            }
        }

        public void CharacterMeleeAttackContact(CharacterInstance character, Vector3 contactWorldPt, bool critted)
        {
            if (this.OnCharacterMeleeAttackContact != null)
            {
                this.OnCharacterMeleeAttackContact(character, contactWorldPt, critted);
            }
        }

        public void CharacterMeleeAttackEnded(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, Vector3 contactWorldPt, int killCount)
        {
            if (this.OnCharacterMeleeAttackEnded != null)
            {
                this.OnCharacterMeleeAttackEnded(sourceCharacter, targetCharacter, contactWorldPt, killCount);
            }
        }

        public void CharacterMeleeAttackStarted(CharacterInstance sourceCharacter, CharacterInstance targetCharacter)
        {
            if (this.OnCharacterMeleeAttackStarted != null)
            {
                this.OnCharacterMeleeAttackStarted(sourceCharacter, targetCharacter);
            }
        }

        public void CharacterPathPlanningUpdated(CharacterInstance character, Path path)
        {
            if (this.OnCharacterPathPlanningUpdated != null)
            {
                this.OnCharacterPathPlanningUpdated(character, path);
            }
        }

        public void CharacterPreBlink(CharacterInstance character)
        {
            if (this.OnCharacterPreBlink != null)
            {
                this.OnCharacterPreBlink(character);
            }
        }

        public void CharacterPreDestroyed(CharacterInstance character)
        {
            if (this.OnCharacterPreDestroyed != null)
            {
                this.OnCharacterPreDestroyed(character);
            }
        }

        public void CharacterRangedAttackEnded(CharacterInstance sourceCharacter, Vector3 targetWorldPt, Projectile projectile)
        {
            if (this.OnCharacterRangedAttackEnded != null)
            {
                this.OnCharacterRangedAttackEnded(sourceCharacter, targetWorldPt, projectile);
            }
        }

        public void CharacterRangedAttackStarted(CharacterInstance sourceCharacter, Vector3 targetWorldPt)
        {
            if (this.OnCharacterRangedAttackStarted != null)
            {
                this.OnCharacterRangedAttackStarted(sourceCharacter, targetWorldPt);
            }
        }

        public void CharacterRankUpped(CharacterInstance character)
        {
            if (this.OnCharacterRankUpped != null)
            {
                this.OnCharacterRankUpped(character);
            }
        }

        public void CharacterRevived(CharacterInstance character)
        {
            if (this.OnCharacterRevived != null)
            {
                this.OnCharacterRevived(character);
            }
        }

        public void CharacterSkillActivated(CharacterInstance character, SkillType skillType, float buildupTime, SkillExecutionStats executionStats)
        {
            if (this.OnCharacterSkillActivated != null)
            {
                this.OnCharacterSkillActivated(character, skillType, buildupTime, executionStats);
            }
        }

        public void CharacterSkillBuildupCompleted(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats)
        {
            if (this.OnCharacterSkillBuildupCompleted != null)
            {
                this.OnCharacterSkillBuildupCompleted(character, skillType, executionStats);
            }
        }

        public void CharacterSkillCooldownEnded(CharacterInstance character, SkillType skillType)
        {
            if (this.OnCharacterSkillCooldownEnded != null)
            {
                this.OnCharacterSkillCooldownEnded(character, skillType);
            }
        }

        public void CharacterSkillExecuted(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats)
        {
            if (this.OnCharacterSkillExecuted != null)
            {
                this.OnCharacterSkillExecuted(character, skillType, executionStats);
            }
        }

        public void CharacterSkillExecutionMidpoint(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats)
        {
            if (this.OnCharacterSkillExecutionMidpoint != null)
            {
                this.OnCharacterSkillExecutionMidpoint(character, skillType, executionStats);
            }
        }

        public void CharacterSkillRankUpped(CharacterInstance character, SkillInstance skillInstance)
        {
            if (this.OnCharacterSkillRankUpped != null)
            {
                this.OnCharacterSkillRankUpped(character, skillInstance);
            }
        }

        public void CharacterSkillsChanged(CharacterInstance character)
        {
            if (this.OnCharacterSkillsChanged != null)
            {
                this.OnCharacterSkillsChanged(character);
            }
        }

        public void CharacterSkillStopped(CharacterInstance character, SkillType skillType)
        {
            if (this.OnCharacterSkillStopped != null)
            {
                this.OnCharacterSkillStopped(character, skillType);
            }
        }

        public void CharacterSpawned(CharacterInstance character)
        {
            if (this.OnCharacterSpawned != null)
            {
                this.OnCharacterSpawned(character);
            }
        }

        public void CharacterSpawnStarted(CharacterInstance character)
        {
            if (this.OnCharacterSpawnStarted != null)
            {
                this.OnCharacterSpawnStarted(character);
            }
        }

        public void CharacterStunConditionChanged(CharacterInstance character)
        {
            if (this.OnCharacterStunConditionChanged != null)
            {
                this.OnCharacterStunConditionChanged(character);
            }
        }

        public void CharacterTargetUpdated(CharacterInstance character, CharacterInstance oldTarget)
        {
            if (this.OnCharacterTargetUpdated != null)
            {
                this.OnCharacterTargetUpdated(character, oldTarget);
            }
        }

        public void CharacterUnlocked(CharacterInstance character)
        {
            if (this.OnCharacterUnlocked != null)
            {
                this.OnCharacterUnlocked(character);
            }
        }

        public void CharacterUpgraded(CharacterInstance character)
        {
            if (this.OnCharacterUpgraded != null)
            {
                this.OnCharacterUpgraded(character);
            }
        }

        public void CharacterVelocityUpdated(CharacterInstance character)
        {
            if (this.OnCharacterVelocityUpdated != null)
            {
                this.OnCharacterVelocityUpdated(character);
            }
        }

        public void DropLootTableRolled(LootTable lootTable, Vector3 worldPos, Reward drop)
        {
            if (this.OnDropLootTableRolled != null)
            {
                this.OnDropLootTableRolled(lootTable, worldPos, drop);
            }
        }

        public void DungeonBoostActivated(DungeonBoost dungeonBoost, SkillType fromSkill)
        {
            if (this.OnDungeonBoostActivated != null)
            {
                this.OnDungeonBoostActivated(dungeonBoost, fromSkill);
            }
        }

        public void DungeonBoostPreDestroy(DungeonBoost dungeonBoost)
        {
            if (this.OnDungeonBoostPreDestroy != null)
            {
                this.OnDungeonBoostPreDestroy(dungeonBoost);
            }
        }

        public void DungeonBoostSpawned(DungeonBoost dungeonBoost)
        {
            if (this.OnDungeonBoostSpawned != null)
            {
                this.OnDungeonBoostSpawned(dungeonBoost);
            }
        }

        public void DungeonDecosRefreshed()
        {
            if (this.OnDungeonDecosRefreshed != null)
            {
                this.OnDungeonDecosRefreshed();
            }
        }

        public void DungeonExplored(CharacterInstance character, string dungeonId)
        {
            if (this.OnDungeonExplored != null)
            {
                this.OnDungeonExplored(character, dungeonId);
            }
        }

        public void FrenzyActivated()
        {
            if (this.OnFrenzyActivated != null)
            {
                this.OnFrenzyActivated();
            }
        }

        public void FrenzyDeactivated()
        {
            if (this.OnFrenzyDeactivated != null)
            {
                this.OnFrenzyDeactivated();
            }
        }

        public void GameplayEnded(ActiveDungeon activeDungeon)
        {
            if (this.OnGameplayEnded != null)
            {
                this.OnGameplayEnded(activeDungeon);
            }
        }

        public void GameplayEndingStarted(ActiveDungeon activeDungeon)
        {
            if (this.OnGameplayEndingStarted != null)
            {
                this.OnGameplayEndingStarted(activeDungeon);
            }
        }

        public void GameplayLoadingStarted(ActiveDungeon activeDungeon)
        {
            if (this.OnGameplayLoadingStarted != null)
            {
                this.OnGameplayLoadingStarted(activeDungeon);
            }
        }

        public void GameplayStarted(ActiveDungeon activeDungeon)
        {
            if (this.OnGameplayStarted != null)
            {
                this.OnGameplayStarted(activeDungeon);
            }
        }

        public void GameplayStateChanged(GameplayState previousState, GameplayState currentState)
        {
            if (this.OnGameplayStateChanged != null)
            {
                this.OnGameplayStateChanged(previousState, currentState);
            }
        }

        public void GameplayStateChangeStarted(GameplayState fromState, GameplayState targetState, float transitionDelay)
        {
            if (this.OnGameplayStateChangeStarted != null)
            {
                this.OnGameplayStateChangeStarted(fromState, targetState, transitionDelay);
            }
        }

        public void GameplayTimeSlowdownToggled(bool enabled)
        {
            if (this.OnGameplayTimeSlowdownToggled != null)
            {
                this.OnGameplayTimeSlowdownToggled(enabled);
            }
        }

        public void GameStateInitialized()
        {
            if (this.OnGameStateInitialized != null)
            {
                this.OnGameStateInitialized();
            }
        }

        public void ItemEquipped(CharacterInstance character, ItemInstance itemInstance, ItemInstance replacedItemInstance)
        {
            if (this.OnItemEquipped != null)
            {
                this.OnItemEquipped(character, itemInstance, replacedItemInstance);
            }
        }

        public void ItemEvolved(CharacterInstance character, ItemInstance itemInstance)
        {
            if (this.OnItemEvolved != null)
            {
                this.OnItemEvolved(character, itemInstance);
            }
        }

        public void ItemGained(CharacterInstance character, ItemInstance itemInstance, string trackingId)
        {
            if (this.OnItemGained != null)
            {
                this.OnItemGained(character, itemInstance, trackingId);
            }
        }

        public void ItemInspected(ItemInstance itemInstance)
        {
            if (this.OnItemInspected != null)
            {
                this.OnItemInspected(itemInstance);
            }
        }

        public void ItemInstantUpgraded(ItemInstance itemInstance, double diamondCost)
        {
            if (this.OnItemInstantUpgraded != null)
            {
                this.OnItemInstantUpgraded(itemInstance, diamondCost);
            }
        }

        public void ItemPerksRerolled(Player player, ItemInstance itemInstance, double diamondCost)
        {
            if (this.OnItemPerksRerolled != null)
            {
                this.OnItemPerksRerolled(player, itemInstance, diamondCost);
            }
        }

        public void ItemRankUpped(CharacterInstance character, ItemInstance itemInstance, int rankUpCount, bool free)
        {
            if (this.OnItemRankUpped != null)
            {
                this.OnItemRankUpped(character, itemInstance, rankUpCount, free);
            }
        }

        public void ItemSold(CharacterInstance character, ItemInstance itemInstance, double value, RectTransform flyToHudOrigin)
        {
            if (this.OnItemSold != null)
            {
                this.OnItemSold(character, itemInstance, value, flyToHudOrigin);
            }
        }

        public void ItemUnlocked(CharacterInstance character, ItemInstance itemInstance)
        {
            if (this.OnItemUnlocked != null)
            {
                this.OnItemUnlocked(character, itemInstance);
            }
        }

        public void LeaderboardOutperformed(LeaderboardType leaderboardType)
        {
            if (this.OnLeaderboardOutperformed != null)
            {
                this.OnLeaderboardOutperformed(leaderboardType);
            }
        }

        public void MissionEnded(Player player, MissionInstance mission, bool success)
        {
            if (this.OnMissionEnded != null)
            {
                this.OnMissionEnded(player, mission, success);
            }
        }

        public void MissionInspected(Player player, MissionInstance mission)
        {
            if (this.OnMissionInspected != null)
            {
                this.OnMissionInspected(player, mission);
            }
        }

        public void MissionStarted(Player player, MissionInstance mission)
        {
            if (this.OnMissionStarted != null)
            {
                this.OnMissionStarted(player, mission);
            }
        }

        public void MultikillBonusGranted(Player player, int killCount, double coinAmount)
        {
            if (this.OnMultikillBonusGranted != null)
            {
                this.OnMultikillBonusGranted(player, killCount, coinAmount);
            }
        }

        protected void OnDisable()
        {
            UnityUtils.StopCoroutine(this, ref this.m_queuedEventProcessingRoutine);
        }

        protected void OnEnable()
        {
            this.m_queuedEventProcessingRoutine = UnityUtils.StartCoroutine(this, this.queuedEventProcessingRoutine());
        }

        public void PassiveProgress(Player player, int numPassiveMinionKills, int numPassiveFloorCompletions, int numPassiveBossKills)
        {
            if (this.OnPassiveProgress != null)
            {
                this.OnPassiveProgress(player, numPassiveMinionKills, numPassiveFloorCompletions, numPassiveBossKills);
            }
        }

        public void PauseToggled(bool enabled)
        {
            if (this.OnPauseToggled != null)
            {
                this.OnPauseToggled(enabled);
            }
        }

        public void PetGained(Player player, string petId, bool cheated)
        {
            if (this.OnPetGained != null)
            {
                this.OnPetGained(player, petId, cheated);
            }
        }

        public void PetInspected(Player player, string petId, bool cheated)
        {
            if (this.OnPetInspected != null)
            {
                this.OnPetInspected(player, petId, cheated);
            }
        }

        public void PetLevelUpped(Player player, string petId, bool cheated)
        {
            if (this.OnPetLevelUpped != null)
            {
                this.OnPetLevelUpped(player, petId, cheated);
            }
        }

        public void PetSelected(Player player, PetInstance pet)
        {
            if (this.OnPetSelected != null)
            {
                this.OnPetSelected(player, pet);
            }
        }

        public void PlayerActiveCharacterChanged(CharacterInstance activeCharacter)
        {
            if (this.OnPlayerActiveCharacterChanged != null)
            {
                this.OnPlayerActiveCharacterChanged(activeCharacter);
            }
        }

        public void PlayerActiveCharacterSwitched(CharacterInstance activeCharacter)
        {
            if (this.OnPlayerActiveCharacterSwitched != null)
            {
                this.OnPlayerActiveCharacterSwitched(activeCharacter);
            }
        }

        public void PlayerAugmentationGained(Player player, string id)
        {
            if (this.OnPlayerAugmentationGained != null)
            {
                this.OnPlayerAugmentationGained(player, id);
            }
        }

        public void PlayerAugmentationPurchased(Player player, string id, ResourceType spentResource, double price)
        {
            if (this.OnPlayerAugmentationPurchased != null)
            {
                this.OnPlayerAugmentationPurchased(player, id, spentResource, price);
            }
        }

        public void PlayerRankUpped(Player player, bool cheated)
        {
            if (this.OnPlayerRankUpped != null)
            {
                this.OnPlayerRankUpped(player, cheated);
            }
        }

        public void PlayerRenamed(Player player)
        {
            if (this.OnPlayerRenamed != null)
            {
                this.OnPlayerRenamed(player);
            }
        }

        public void PlayerRetired(Player player, int retirementFloor)
        {
            if (this.OnPlayerRetired != null)
            {
                this.OnPlayerRetired(player, retirementFloor);
            }
        }

        public void PlayerSkillUpgradeGained(Player player, string id)
        {
            if (this.OnPlayerSkillUpgradeGained != null)
            {
                this.OnPlayerSkillUpgradeGained(player, id);
            }
        }

        public void PlayerSkillUpgradePurchased(Player player, string id, ResourceType spentResource, double price)
        {
            if (this.OnPlayerSkillUpgradePurchased != null)
            {
                this.OnPlayerSkillUpgradePurchased(player, id, spentResource, price);
            }
        }

        public void PotionsGained(CharacterInstance character, PotionType potionType, int amount)
        {
            if (this.OnPotionsGained != null)
            {
                this.OnPotionsGained(character, potionType, amount);
            }
        }

        public void ProjectileCollided(Projectile projectile, Collider collider)
        {
            if (this.OnProjectileCollided != null)
            {
                this.OnProjectileCollided(projectile, collider);
            }
        }

        public void ProjectilePreDestroy(Projectile projectile)
        {
            if (this.OnProjectilePreDestroy != null)
            {
                this.OnProjectilePreDestroy(projectile);
            }
        }

        public void ProjectileSpawned(Projectile projectile)
        {
            if (this.OnProjectileSpawned != null)
            {
                this.OnProjectileSpawned(projectile);
            }
        }

        public void PromotionEventEnded(Player player, string promotionId)
        {
            if (this.OnPromotionEventEnded != null)
            {
                this.OnPromotionEventEnded(player, promotionId);
            }
        }

        public void PromotionEventInspected(Player player, string promotionId)
        {
            if (this.OnPromotionEventInspected != null)
            {
                this.OnPromotionEventInspected(player, promotionId);
            }
        }

        public void PromotionEventRefreshed(Player player, string promotionId)
        {
            if (this.OnPromotionEventRefreshed != null)
            {
                this.OnPromotionEventRefreshed(player, promotionId);
            }
        }

        public void PromotionEventStarted(Player player, string promotionId)
        {
            if (this.OnPromotionEventStarted != null)
            {
                this.OnPromotionEventStarted(player, promotionId);
            }
        }

        [DebuggerHidden]
        private IEnumerator queuedEventProcessingRoutine()
        {
            <queuedEventProcessingRoutine>c__Iterator45 iterator = new <queuedEventProcessingRoutine>c__Iterator45();
            iterator.<>f__this = this;
            return iterator;
        }

        public void ResourcesGained(Player player, ResourceType resourceType, double amount, bool instant, string trackingId, Vector3? worldPt)
        {
            if (this.OnResourcesGained != null)
            {
                this.OnResourcesGained(player, resourceType, amount, instant, trackingId, worldPt);
            }
        }

        public void RewardConsumed(Player player, Reward reward)
        {
            if (this.OnRewardConsumed != null)
            {
                this.OnRewardConsumed(player, reward);
            }
        }

        public void RoomCompleted(Room room)
        {
            if (this.OnRoomCompleted != null)
            {
                this.OnRoomCompleted(room);
            }
        }

        public void RoomLoaded(Room room)
        {
            if (this.OnRoomLoaded != null)
            {
                this.OnRoomLoaded(room);
            }
        }

        public void RunestoneGained(Player player, RunestoneInstance runestone, bool cheated)
        {
            if (this.OnRunestoneGained != null)
            {
                this.OnRunestoneGained(player, runestone, cheated);
            }
        }

        public void RunestoneInspected(Player player, RunestoneInstance runestone)
        {
            if (this.OnRunestoneInspected != null)
            {
                this.OnRunestoneInspected(player, runestone);
            }
        }

        public void RunestoneLevelUpped(Player player, RunestoneInstance runestone)
        {
            if (this.OnRunestoneLevelUpped != null)
            {
                this.OnRunestoneLevelUpped(player, runestone);
            }
        }

        public void RunestoneRankUpped(Player player, RunestoneInstance runestone)
        {
            if (this.OnRunestoneRankUpped != null)
            {
                this.OnRunestoneRankUpped(player, runestone);
            }
        }

        public void RunestoneSelected(Player player, RunestoneInstance runestone)
        {
            if (this.OnRunestoneSelected != null)
            {
                this.OnRunestoneSelected(player, runestone);
            }
        }

        public void RunestoneUnlocked(Player player, RunestoneInstance runestone)
        {
            if (this.OnRunestoneUnlocked != null)
            {
                this.OnRunestoneUnlocked(player, runestone);
            }
        }

        public void SkillInspected(SkillInstance skillInstance)
        {
            if (this.OnSkillInspected != null)
            {
                this.OnSkillInspected(skillInstance);
            }
        }

        public void SuspectedSystemClockCheat(long timeOffsetSeconds)
        {
            if (this.OnSuspectedSystemClockCheat != null)
            {
                this.OnSuspectedSystemClockCheat(timeOffsetSeconds);
            }
        }

        public void TimescaleChanged(float timescale)
        {
            if (this.OnTimescaleChanged != null)
            {
                this.OnTimescaleChanged(timescale);
            }
        }

        public void TimescaleChangeStarted(float targetTimescale)
        {
            if (this.OnTimescaleChangeStarted != null)
            {
                this.OnTimescaleChangeStarted(targetTimescale);
            }
        }

        public void TournamentDonationMade(Player player, TournamentInstance tournament, int count, double totalPrice)
        {
            if (this.OnTournamentDonationMade != null)
            {
                this.OnTournamentDonationMade(player, tournament, count, totalPrice);
            }
        }

        public void TournamentSelected(Player player, TournamentInstance selectedTournament, TournamentInstance unselectedTournament)
        {
            if (this.OnTournamentSelected != null)
            {
                this.OnTournamentSelected(player, selectedTournament, unselectedTournament);
            }
        }

        public void TournamentUpgradeGained(Player player, string id, bool epicVersion, int numMilestonesCompleted)
        {
            if (this.OnTournamentUpgradeGained != null)
            {
                this.OnTournamentUpgradeGained(player, id, epicVersion, numMilestonesCompleted);
            }
        }

        public void TutorialCompleted(Player player, string tutorialId)
        {
            if (this.OnTutorialCompleted != null)
            {
                this.OnTutorialCompleted(player, tutorialId);
            }
        }

        public void TutorialStarted(Player player, string tutorialId)
        {
            if (this.OnTutorialStarted != null)
            {
                this.OnTutorialStarted(player, tutorialId);
            }
        }

        public void VendorInventoryInspected(Player player)
        {
            if (this.OnVendorInventoryInspected != null)
            {
                this.OnVendorInventoryInspected(player);
            }
        }

        public void VendorInventoryRefreshed(Player player)
        {
            if (this.OnVendorInventoryRefreshed != null)
            {
                this.OnVendorInventoryRefreshed(player);
            }
        }

        public bool ProcessingQueuedEvents
        {
            [CompilerGenerated]
            get
            {
                return this.<ProcessingQueuedEvents>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ProcessingQueuedEvents>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <queuedEventProcessingRoutine>c__Iterator45 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal GameLogic.EventBus <>f__this;
            internal QueuedEventData <e>__1;
            internal int <i>__2;
            internal long <MS_PER_FRAME_LIMIT>__0;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (ConfigDevice.DeviceQuality() != DeviceQualityType.High)
                        {
                            if (ConfigDevice.DeviceQuality() == DeviceQualityType.Med)
                            {
                                this.<MS_PER_FRAME_LIMIT>__0 = 3L;
                            }
                            else
                            {
                                this.<MS_PER_FRAME_LIMIT>__0 = 4L;
                            }
                            break;
                        }
                        this.<MS_PER_FRAME_LIMIT>__0 = 2L;
                        break;

                    case 1:
                        goto Label_00FF;

                    case 2:
                        break;
                        this.$PC = -1;
                        goto Label_0189;

                    default:
                        goto Label_0189;
                }
                this.<>f__this.m_frameTimer.Reset();
                this.<>f__this.m_frameTimer.Start();
                while (this.<>f__this.m_queuedEvents.Count > 0)
                {
                    this.<>f__this.ProcessingQueuedEvents = true;
                    this.<e>__1 = this.<>f__this.m_queuedEvents.Dequeue();
                    this.<i>__2 = 0;
                    while (this.<i>__2 < this.<e>__1.Handlers.Count)
                    {
                        this.<e>__1.dispatch(this.<e>__1.Handlers[this.<i>__2]);
                        if (this.<>f__this.m_frameTimer.ElapsedMilliseconds <= this.<MS_PER_FRAME_LIMIT>__0)
                        {
                            goto Label_011F;
                        }
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_018B;
                    Label_00FF:
                        this.<>f__this.m_frameTimer.Reset();
                        this.<>f__this.m_frameTimer.Start();
                    Label_011F:
                        this.<i>__2++;
                    }
                }
                this.<>f__this.ProcessingQueuedEvents = false;
                this.$current = null;
                this.$PC = 2;
                goto Label_018B;
            Label_0189:
                return false;
            Label_018B:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        public class QueuedEvent_CharacterKilled : QueuedEventData
        {
            public override void dispatch(object target)
            {
                GameLogic.Events.CharacterKilled killed = (GameLogic.Events.CharacterKilled) target;
                killed((CharacterInstance) base.Param1, (CharacterInstance) base.Param2, (bool) base.Param3, (SkillType) ((int) base.Param4));
            }
        }
    }
}

