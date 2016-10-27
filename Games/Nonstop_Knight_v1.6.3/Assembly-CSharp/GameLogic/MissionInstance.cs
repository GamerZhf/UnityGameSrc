namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using Service;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class MissionInstance
    {
        [CompilerGenerated]
        private GameLogic.Missions <Missions>k__BackingField;
        public long CooldownDurationSeconds;
        public long CooldownStartTimestamp;
        public bool ForceCompleted;
        public bool Inspected;
        public string MissionId;
        public bool OnCooldown;
        public double Requirement;
        public bool RewardClaimed;
        public double RewardDiamonds;
        public string RewardShopEntryId;
        public double StartValue;

        public static int CompareByCooldown(MissionInstance a, MissionInstance b)
        {
            if (a == b)
            {
                return 0;
            }
            int num = a.CooldownDurationSeconds.CompareTo(b.CooldownDurationSeconds);
            if (num != 0)
            {
                return num;
            }
            bool flag = string.IsNullOrEmpty(a.MissionId);
            bool flag2 = string.IsNullOrEmpty(b.MissionId);
            if (!flag && !flag2)
            {
                return a.MissionId.CompareTo(b.MissionId);
            }
            return flag.CompareTo(flag2);
        }

        public void fillReward(Player player, ref Reward reward)
        {
            if (!string.IsNullOrEmpty(this.RewardShopEntryId))
            {
                reward.addShopEntryDrop(player, this.RewardShopEntryId, false);
            }
            if (this.RewardDiamonds > 0.0)
            {
                reward.addResourceDrop(ResourceType.Diamond, this.RewardDiamonds);
            }
        }

        public float getMissionProgress(Player player, out double result)
        {
            ConfigMissions.Mission missionData = ConfigMissions.GetMissionData(this.MissionId);
            if (missionData == null)
            {
                result = this.Requirement;
                return 0f;
            }
            float num = missionData.getMissionProgress(player, this.StartValue, this.Requirement, out result);
            return (!this.ForceCompleted ? num : 1f);
        }

        public long getRemainingCooldownSeconds()
        {
            if (!this.OnCooldown || (this.CooldownDurationSeconds <= 0L))
            {
                return 0L;
            }
            long num = MathUtil.Clamp((long) (Service.Binder.ServerTime.GameTime - this.CooldownStartTimestamp), (long) 0L, (long) 0x7fffffffffffffffL);
            return MathUtil.Clamp((long) (this.CooldownDurationSeconds - num), (long) 0L, (long) 0x7fffffffffffffffL);
        }

        public bool isActive()
        {
            return ((!string.IsNullOrEmpty(this.MissionId) && !this.OnCooldown) && !this.RewardClaimed);
        }

        [JsonIgnore]
        public GameLogic.Missions Missions
        {
            [CompilerGenerated]
            get
            {
                return this.<Missions>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Missions>k__BackingField = value;
            }
        }

        [JsonIgnore]
        public GameLogic.MissionType MissionType
        {
            get
            {
                return this.Missions.MissionType;
            }
        }
    }
}

