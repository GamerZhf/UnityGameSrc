namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ConsoleCommand("startmission")]
    public class CmdStartMission : ICommand
    {
        private MissionInstance m_mission;
        private Player m_player;
        private static List<string> sm_activeMissionIdPool = new List<string>();
        private static List<string> sm_nextMissionPool = new List<string>();
        private static List<string> sm_nextMissionPoolWithoutGroupRestriction = new List<string>();

        public CmdStartMission(string[] serialized)
        {
            this.m_player = GameLogic.Binder.GameState.Player;
            if (serialized[0] == "event")
            {
                this.m_mission = this.m_player.PromotionEvents.getNewestEventInstance().Missions.Instances[int.Parse(serialized[1])];
            }
            else
            {
                this.m_mission = this.m_player.Missions.Instances[int.Parse(serialized[0])];
            }
        }

        public CmdStartMission(Player player, MissionInstance mission)
        {
            this.m_player = player;
            this.m_mission = mission;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorB7 rb = new <executeRoutine>c__IteratorB7();
            rb.<>f__this = this;
            return rb;
        }

        public static void ExecuteStatic(Player player, MissionInstance mission)
        {
            if (mission.MissionType == MissionType.Adventure)
            {
                string str;
                double num;
                player.HasUnlockedMissions = true;
                App.Binder.ConfigMeta.GetActiveMissionsIds(ref sm_activeMissionIdPool);
                if (sm_activeMissionIdPool.Count == 0)
                {
                    return;
                }
                mission.MissionId = null;
                mission.StartValue = 0.0;
                mission.Requirement = 0.0;
                mission.ForceCompleted = false;
                mission.RewardShopEntryId = null;
                mission.RewardClaimed = false;
                mission.OnCooldown = false;
                mission.Inspected = true;
                RollAdventureMission(player, sm_activeMissionIdPool, out str, out num);
                mission.MissionId = str;
                mission.Requirement = num;
                LootTable missionRewardLootTable = App.Binder.ConfigLootTables.MissionRewardLootTable;
                string id = missionRewardLootTable.roll().Id;
                missionRewardLootTable.resetWeights(true);
                mission.RewardShopEntryId = id;
            }
            mission.StartValue = ConfigMissions.MISSIONS[mission.MissionId].getValue(player, null);
            mission.OnCooldown = false;
            mission.Inspected = false;
            GameLogic.Binder.EventBus.MissionStarted(player, mission);
        }

        private static void RollAdventureMission(Player player, List<string> missionIdPool, out string missionId, out double missionRequirement)
        {
            int difficultyIndex = player.CumulativeRetiredHeroStats.MissionsStarted + player.ActiveCharacter.HeroStats.MissionsStarted;
            if (difficultyIndex < App.Binder.ConfigMeta.MISSIONS_FTUE.Count)
            {
                MissionConfig config = App.Binder.ConfigMeta.MISSIONS_FTUE[difficultyIndex];
                missionId = config.Id;
                missionRequirement = config.Difficulty[0];
            }
            else
            {
                sm_nextMissionPool.Clear();
                sm_nextMissionPool.AddRange(missionIdPool);
                for (int i = sm_nextMissionPool.Count - 1; i >= 0; i--)
                {
                    ConfigMissions.Mission mission = ConfigMissions.MISSIONS[sm_nextMissionPool[i]];
                    if (!mission.canComplete(player))
                    {
                        sm_nextMissionPool.RemoveAt(i);
                    }
                }
                sm_nextMissionPoolWithoutGroupRestriction.Clear();
                sm_nextMissionPoolWithoutGroupRestriction.AddRange(sm_nextMissionPool);
                for (int j = 0; j < player.Missions.Instances.Count; j++)
                {
                    MissionInstance instance = player.Missions.Instances[j];
                    if (instance.isActive())
                    {
                        sm_nextMissionPool.Remove(instance.MissionId);
                        sm_nextMissionPoolWithoutGroupRestriction.Remove(instance.MissionId);
                        MissionGroup group = ConfigMissions.GetMissionData(instance.MissionId).Group;
                        for (int k = sm_nextMissionPool.Count - 1; k >= 0; k--)
                        {
                            ConfigMissions.Mission mission2 = ConfigMissions.MISSIONS[sm_nextMissionPool[k]];
                            if (mission2.Group == group)
                            {
                                sm_nextMissionPool.RemoveAt(k);
                            }
                        }
                    }
                }
                if (sm_nextMissionPool.Count == 0)
                {
                    if (sm_nextMissionPoolWithoutGroupRestriction.Count > 0)
                    {
                        sm_nextMissionPool.AddRange(sm_nextMissionPoolWithoutGroupRestriction);
                    }
                    else
                    {
                        sm_nextMissionPool.AddRange(missionIdPool);
                    }
                }
                missionId = LangUtil.GetRandomValueFromList<string>(sm_nextMissionPool);
                missionRequirement = App.Binder.ConfigMeta.GetMissionRequirement(missionId, difficultyIndex);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorB7 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdStartMission <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    CmdStartMission.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_mission);
                }
                return false;
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
    }
}

