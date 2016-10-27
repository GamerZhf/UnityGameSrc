namespace Service
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class TournamentInfo
    {
        [CompilerGenerated]
        private double <DifficultyModifier>k__BackingField;
        [CompilerGenerated]
        private int <DurationHours>k__BackingField;
        [CompilerGenerated]
        private int <ExpirationDaysFromStart>k__BackingField;
        [CompilerGenerated]
        private string <Id>k__BackingField;
        [CompilerGenerated]
        private int <JoinTimeHoursFromStart>k__BackingField;
        [CompilerGenerated]
        private int <MilestoneThresholdInterval>k__BackingField;
        [CompilerGenerated]
        private int <PlayersPerBucket>k__BackingField;
        [CompilerGenerated]
        private List<RewardMilestone> <RewardMilestones>k__BackingField;
        [CompilerGenerated]
        private bool <SegmentedTournament>k__BackingField;
        [CompilerGenerated]
        private long <StartTimeEpoch>k__BackingField;
        [CompilerGenerated]
        private string <TournamentRulesetId>k__BackingField;
        [CompilerGenerated]
        private long <VisibleHoursBeforeStart>k__BackingField;

        public TournamentInfo()
        {
            this.RewardMilestones = new List<RewardMilestone>();
        }

        public TournamentInfo(string id, long visibleHoursBeforeStart, long startTimeEpoch, int joinTimeHoursFromStart, int durationHours, int expirationDaysFromStart, string tournamentRulesetId, int playersPerBucket, List<RewardMilestone> rewardMilestones, int milestoneThresholdInterval, [Optional, DefaultParameterValue(1.0)] double difficultyModifier, [Optional, DefaultParameterValue(false)] bool segmentedTournament)
        {
            this.Id = id;
            this.VisibleHoursBeforeStart = visibleHoursBeforeStart;
            this.StartTimeEpoch = startTimeEpoch;
            this.JoinTimeHoursFromStart = joinTimeHoursFromStart;
            this.DurationHours = durationHours;
            this.ExpirationDaysFromStart = expirationDaysFromStart;
            this.TournamentRulesetId = tournamentRulesetId;
            this.PlayersPerBucket = playersPerBucket;
            this.RewardMilestones = rewardMilestones;
            this.MilestoneThresholdInterval = milestoneThresholdInterval;
            this.DifficultyModifier = difficultyModifier;
            this.SegmentedTournament = segmentedTournament;
            for (int i = 0; i < this.RewardMilestones.Count; i++)
            {
                if (this.RewardMilestones[i].Threshold == 0)
                {
                    this.RewardMilestones[i].Threshold = (i + 1) * this.MilestoneThresholdInterval;
                }
            }
        }

        public int CompareStartTimes(TournamentInfo other)
        {
            if (other == null)
            {
                return 1;
            }
            if (this.StartTimeEpoch != other.StartTimeEpoch)
            {
                return this.StartTimeEpoch.CompareTo(other.StartTimeEpoch);
            }
            return string.CompareOrdinal(this.Id, other.Id);
        }

        public float GetCompletionAtMilestone(RewardMilestone rewardMilestone)
        {
            return MathUtil.Clamp01(((double) this.GetMileStoneRank(rewardMilestone)) / ((double) this.RewardMilestones.Count));
        }

        public int GetMileStoneRank(RewardMilestone rewardMilestone)
        {
            int num = 1;
            foreach (RewardMilestone milestone in this.RewardMilestones)
            {
                if (milestone.Threshold < rewardMilestone.Threshold)
                {
                    num++;
                }
            }
            return num;
        }

        public long getSecondsUntilAvailable()
        {
            return MathUtil.Clamp((long) (this.StartTimeEpoch - Binder.ServerTime.GameTime), (long) 0L, (long) 0x7fffffffffffffffL);
        }

        public long getSecondsUntilJoinsEnd()
        {
            return MathUtil.Clamp((long) (this.LastJoinTimeEpoch - Binder.ServerTime.GameTime), (long) 0L, (long) 0x7fffffffffffffffL);
        }

        public bool OverlapsWith(TournamentInfo other)
        {
            long num = this.LastJoinTimeEpoch + this.DurationSeconds;
            long num2 = other.LastJoinTimeEpoch + other.DurationSeconds;
            return ((num <= num2) ? (other.StartTimeEpoch < num) : (this.StartTimeEpoch < num2));
        }

        public double DifficultyModifier
        {
            [CompilerGenerated]
            get
            {
                return this.<DifficultyModifier>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<DifficultyModifier>k__BackingField = value;
            }
        }

        public int DurationHours
        {
            [CompilerGenerated]
            get
            {
                return this.<DurationHours>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<DurationHours>k__BackingField = value;
            }
        }

        public int DurationSeconds
        {
            get
            {
                return ((this.DurationHours * 60) * 60);
            }
        }

        public int ExpirationDaysFromStart
        {
            [CompilerGenerated]
            get
            {
                return this.<ExpirationDaysFromStart>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ExpirationDaysFromStart>k__BackingField = value;
            }
        }

        public long ExpirationTimeEpoch
        {
            get
            {
                return (this.StartTimeEpoch + (((this.ExpirationDaysFromStart * 0x18) * 60) * 60));
            }
        }

        public string Id
        {
            [CompilerGenerated]
            get
            {
                return this.<Id>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Id>k__BackingField = value;
            }
        }

        public int JoinTimeHoursFromStart
        {
            [CompilerGenerated]
            get
            {
                return this.<JoinTimeHoursFromStart>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<JoinTimeHoursFromStart>k__BackingField = value;
            }
        }

        public long LastJoinTimeEpoch
        {
            get
            {
                return (this.StartTimeEpoch + ((this.JoinTimeHoursFromStart * 60) * 60));
            }
        }

        public long MaxContribution
        {
            get
            {
                long threshold = 0L;
                foreach (RewardMilestone milestone in this.RewardMilestones)
                {
                    if (milestone.Threshold > threshold)
                    {
                        threshold = milestone.Threshold;
                    }
                }
                return threshold;
            }
        }

        public int MilestoneThresholdInterval
        {
            [CompilerGenerated]
            get
            {
                return this.<MilestoneThresholdInterval>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<MilestoneThresholdInterval>k__BackingField = value;
            }
        }

        public int PlayersPerBucket
        {
            [CompilerGenerated]
            get
            {
                return this.<PlayersPerBucket>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PlayersPerBucket>k__BackingField = value;
            }
        }

        public List<RewardMilestone> RewardMilestones
        {
            [CompilerGenerated]
            get
            {
                return this.<RewardMilestones>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RewardMilestones>k__BackingField = value;
            }
        }

        public bool SegmentedTournament
        {
            [CompilerGenerated]
            get
            {
                return this.<SegmentedTournament>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<SegmentedTournament>k__BackingField = value;
            }
        }

        public long StartTimeEpoch
        {
            [CompilerGenerated]
            get
            {
                return this.<StartTimeEpoch>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<StartTimeEpoch>k__BackingField = value;
            }
        }

        public string TournamentRulesetId
        {
            [CompilerGenerated]
            get
            {
                return this.<TournamentRulesetId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<TournamentRulesetId>k__BackingField = value;
            }
        }

        public long VisibleHoursBeforeStart
        {
            [CompilerGenerated]
            get
            {
                return this.<VisibleHoursBeforeStart>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<VisibleHoursBeforeStart>k__BackingField = value;
            }
        }

        public long VisibleTimeEpoch
        {
            get
            {
                return (this.StartTimeEpoch - ((this.VisibleHoursBeforeStart * 60L) * 60L));
            }
        }
    }
}

