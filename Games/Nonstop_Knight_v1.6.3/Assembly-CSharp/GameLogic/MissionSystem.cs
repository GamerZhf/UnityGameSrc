namespace GameLogic
{
    using Service;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class MissionSystem : MonoBehaviour, IMissionSystem
    {
        protected void Awake()
        {
        }

        protected void FixedUpdate()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (((player != null) && player.HasUnlockedMissions) && (Service.Binder.ContentService.MasterRemoteContent != null))
            {
                for (int i = 0; i < player.Missions.Instances.Count; i++)
                {
                    MissionInstance mission = player.Missions.Instances[i];
                    if (string.IsNullOrEmpty(mission.MissionId))
                    {
                        CmdStartMission.ExecuteStatic(player, mission);
                    }
                }
                for (int j = 0; j < player.Missions.Instances.Count; j++)
                {
                    MissionInstance instance2 = player.Missions.Instances[j];
                    if (instance2.OnCooldown && (instance2.getRemainingCooldownSeconds() <= 0L))
                    {
                        CmdStartMission.ExecuteStatic(player, instance2);
                    }
                }
                foreach (KeyValuePair<string, PromotionEventInstance> pair in player.PromotionEvents.Instances)
                {
                    PromotionEventInstance instance3 = pair.Value;
                    for (int k = 0; k < instance3.Missions.Instances.Count; k++)
                    {
                        MissionInstance instance4 = instance3.Missions.Instances[k];
                        if (instance4.OnCooldown && (instance4.getRemainingCooldownSeconds() <= 0L))
                        {
                            CmdStartMission.ExecuteStatic(player, instance4);
                        }
                    }
                }
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            GameLogic.Binder.EventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnMissionEnded -= new GameLogic.Events.MissionEnded(this.onMissionEnded);
            GameLogic.Binder.EventBus.OnSuspectedSystemClockCheat -= new GameLogic.Events.SuspectedSystemClockCheat(this.onSuspectedSystemClockCheat);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            GameLogic.Binder.EventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnMissionEnded += new GameLogic.Events.MissionEnded(this.onMissionEnded);
            GameLogic.Binder.EventBus.OnSuspectedSystemClockCheat += new GameLogic.Events.SuspectedSystemClockCheat(this.onSuspectedSystemClockCheat);
        }

        private void onGameplayStarted(ActiveDungeon ad)
        {
            Player owningPlayer = ad.PrimaryPlayerCharacter.OwningPlayer;
            if (owningPlayer.doUnlockMissions())
            {
                owningPlayer.HasUnlockedMissions = true;
            }
        }

        private void onGameStateInitialized()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (player.HasUnlockedMissions)
            {
                player.Missions.enforceMissionLegality();
            }
        }

        private void onMissionEnded(Player player, MissionInstance mission, bool success)
        {
            if (success)
            {
                Missions missions = mission.Missions;
                missions.NumUnclaimedMissionCompletions++;
            }
        }

        private void onSuspectedSystemClockCheat(long timeOffsetSeconds)
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (player.HasUnlockedMissions)
            {
                for (int i = 0; i < player.Missions.Instances.Count; i++)
                {
                    CmdEndMission.ExecuteStatic(player, player.Missions.Instances[i], false);
                }
            }
        }
    }
}

