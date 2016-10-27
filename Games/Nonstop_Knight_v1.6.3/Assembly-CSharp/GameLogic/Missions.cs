namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Missions : IJsonData
    {
        [CompilerGenerated]
        private GameLogic.Player <Player>k__BackingField;
        public List<MissionInstance> Instances;
        public GameLogic.MissionType MissionType;
        public int NumClaimedBigPrizes;
        public int NumUnclaimedMissionCompletions;

        public Missions()
        {
            this.Instances = new List<MissionInstance>();
        }

        public Missions(GameLogic.MissionType MissionType)
        {
            this.Instances = new List<MissionInstance>();
            this.MissionType = MissionType;
        }

        public void enforceMissionLegality()
        {
            if (this.MissionType == GameLogic.MissionType.Adventure)
            {
                for (int i = 0; i < this.Instances.Count; i++)
                {
                    MissionInstance mission = this.Instances[i];
                    if (!string.IsNullOrEmpty(mission.MissionId) && !App.Binder.ConfigMeta.IsMissionIdActive(mission.MissionId))
                    {
                        CmdStartMission.ExecuteStatic(this.Player, mission);
                    }
                }
            }
        }

        public MissionInstance getFirstRewardableMissionInstance()
        {
            for (int i = 0; i < this.Instances.Count; i++)
            {
                double num2;
                MissionInstance instance = this.Instances[i];
                if (instance.isActive() && (instance.getMissionProgress(this.Player, out num2) >= 1f))
                {
                    return instance;
                }
            }
            return null;
        }

        public long getMaxRemainingCooldownSeconds()
        {
            long num = 0L;
            for (int i = 0; i < this.Instances.Count; i++)
            {
                if (this.Instances[i].OnCooldown)
                {
                    long num3 = this.Instances[i].getRemainingCooldownSeconds();
                    if (num3 > num)
                    {
                        num = num3;
                    }
                }
            }
            return num;
        }

        public long getMinRemainingCooldownSeconds()
        {
            long num = 0x7fffffffffffffffL;
            for (int i = 0; i < this.Instances.Count; i++)
            {
                if (this.Instances[i].OnCooldown)
                {
                    long num3 = this.Instances[i].getRemainingCooldownSeconds();
                    if (num3 < num)
                    {
                        num = num3;
                    }
                }
            }
            return ((num == 0x7fffffffffffffffL) ? 0L : num);
        }

        public int getNumActiveMissions()
        {
            int num = 0;
            for (int i = 0; i < this.Instances.Count; i++)
            {
                if (this.Instances[i].isActive())
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumCompletedMissionsRequiredForBigPrize()
        {
            GameLogic.MissionType missionType = this.MissionType;
            if (missionType != GameLogic.MissionType.Adventure)
            {
                if (missionType == GameLogic.MissionType.PromotionEvent)
                {
                    return ((this.Instances.Count == 0) ? 0x7fffffff : this.Instances.Count);
                }
                return 0x7fffffff;
            }
            return App.Binder.ConfigMeta.GetNumCompletedMissionsRequiredForBigPrize(this.Player);
        }

        public int getNumRewardableMissions()
        {
            int num = 0;
            for (int i = 0; i < this.Instances.Count; i++)
            {
                double num3;
                MissionInstance instance = this.Instances[i];
                if (instance.isActive() && (instance.getMissionProgress(this.Player, out num3) >= 1f))
                {
                    num++;
                }
            }
            return num;
        }

        public int getNumUninspectedMissions()
        {
            int num = 0;
            for (int i = 0; i < this.Instances.Count; i++)
            {
                if (!this.Instances[i].Inspected)
                {
                    num++;
                }
            }
            return num;
        }

        public bool hasAllMissionsOnCooldown()
        {
            for (int i = 0; i < this.Instances.Count; i++)
            {
                if (!this.Instances[i].OnCooldown)
                {
                    return false;
                }
            }
            return true;
        }

        public bool hasEnoughCompletedMissionsForBigPrize()
        {
            return (this.NumUnclaimedMissionCompletions >= this.getNumCompletedMissionsRequiredForBigPrize());
        }

        public bool hasMissionOnCooldown()
        {
            for (int i = 0; i < this.Instances.Count; i++)
            {
                if (this.Instances[i].OnCooldown)
                {
                    return true;
                }
            }
            return false;
        }

        public bool hasUninspectedMissions()
        {
            return (this.getNumUninspectedMissions() > 0);
        }

        public void postDeserializeInitialization()
        {
            if (this.MissionType == GameLogic.MissionType.Adventure)
            {
                while (this.Instances.Count > ConfigMissions.NUM_ACTIVE_MISSIONS)
                {
                    this.Instances.RemoveAt(this.Instances.Count - 1);
                }
                while (this.Instances.Count < ConfigMissions.NUM_ACTIVE_MISSIONS)
                {
                    this.Instances.Add(new MissionInstance());
                }
            }
            for (int i = 0; i < this.Instances.Count; i++)
            {
                this.Instances[i].Missions = this;
            }
        }

        [JsonIgnore]
        public GameLogic.Player Player
        {
            [CompilerGenerated]
            get
            {
                return this.<Player>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Player>k__BackingField = value;
            }
        }
    }
}

