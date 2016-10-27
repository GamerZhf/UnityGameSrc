namespace Service
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class TournamentView
    {
        [CompilerGenerated]
        private static Predicate<TournamentEntry> <>f__am$cache9;
        [CompilerGenerated]
        private string <BucketId>k__BackingField;
        [CompilerGenerated]
        private long <BucketStartTime>k__BackingField;
        [CompilerGenerated]
        private TournamentInstance <Instance>k__BackingField;
        [CompilerGenerated]
        private TournamentEntry <PlayerEntry>k__BackingField;
        [CompilerGenerated]
        private TournamentViewRemote.Status <ServerJoinStatus>k__BackingField;
        [CompilerGenerated]
        private List<TournamentEntry> <TournamentEntries>k__BackingField;
        [CompilerGenerated]
        private Service.TournamentInfo <TournamentInfo>k__BackingField;
        private TournamentLog m_log;
        public const int RANKING_BRONZE = 2;
        public const int RANKING_GOLD = 0;
        public const int RANKING_NONE = -1;
        public const int RANKING_SILVER = 1;
        private static readonly TournamentViewStableSortComparer s_stableViewComparer = new TournamentViewStableSortComparer();

        public TournamentView(Service.TournamentInfo tournamentInfo, TournamentLog log, TournamentViewRemote.Status status, [Optional, DefaultParameterValue(null)] TournamentInstance tournamentInstance, [Optional, DefaultParameterValue(null)] List<TournamentEntry> tournamentEntries, [Optional, DefaultParameterValue(-1)] long bucketStartTime)
        {
            this.TournamentInfo = tournamentInfo;
            this.Instance = tournamentInstance;
            this.BucketStartTime = bucketStartTime;
            this.ServerJoinStatus = status;
            this.TournamentEntries = new List<TournamentEntry>();
            this.UpdateEntries(tournamentEntries);
            this.UpdateLog(log);
        }

        public int CountClaimedMilestones()
        {
            int num = 0;
            if (this.FullyComposed)
            {
                for (int i = 0; i < this.TournamentInfo.RewardMilestones.Count; i++)
                {
                    RewardMilestone milestone = this.TournamentInfo.RewardMilestones[i];
                    if (milestone.Threshold <= this.Instance.HighestClaimedMilestoneThreshold)
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        public int CountNumCompletedMilestones()
        {
            return (this.CountUnclaimedMilestones() + this.CountClaimedMilestones());
        }

        public int CountUnclaimedMilestones()
        {
            int num = 0;
            if (this.FullyComposed)
            {
                int totalContribution = this.GetTotalContribution();
                for (int i = 0; i < this.TournamentInfo.RewardMilestones.Count; i++)
                {
                    RewardMilestone milestone = this.TournamentInfo.RewardMilestones[i];
                    if ((milestone.Threshold > this.Instance.HighestClaimedMilestoneThreshold) && (milestone.Threshold <= totalContribution))
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        public int getLeaderboardRanking(TournamentEntry tournamentEntry)
        {
            this.RefreshPlayerEntry();
            if ((this.TournamentEntries != null) && (this.TournamentEntries.Count != 0))
            {
                int num = 0;
                int contribution = this.TournamentEntries[0].Contribution;
                for (int i = 0; i < this.TournamentEntries.Count; i++)
                {
                    if (this.TournamentEntries[i].Contribution == contribution)
                    {
                        num++;
                        if (tournamentEntry.Contribution == contribution)
                        {
                            return 0;
                        }
                    }
                }
                int num4 = 0;
                if ((num == 1) && (num < this.TournamentEntries.Count))
                {
                    int num5 = this.TournamentEntries[num].Contribution;
                    for (int j = 0; j < this.TournamentEntries.Count; j++)
                    {
                        if (this.TournamentEntries[j].Contribution == num5)
                        {
                            num4++;
                            if (tournamentEntry.Contribution == num5)
                            {
                                return 1;
                            }
                        }
                    }
                }
                int num7 = 0;
                if (((num == 2) || ((num == 1) && (num4 == 1))) && ((num + num4) < this.TournamentEntries.Count))
                {
                    int num8 = this.TournamentEntries[num + num4].Contribution;
                    for (int k = 0; k < this.TournamentEntries.Count; k++)
                    {
                        if (this.TournamentEntries[k].Contribution == num8)
                        {
                            num7++;
                            if (tournamentEntry.Contribution == num8)
                            {
                                return 2;
                            }
                        }
                    }
                }
            }
            return -1;
        }

        public int getMilestoneCount()
        {
            return this.TournamentInfo.RewardMilestones.Count;
        }

        public int getMilestoneNumber(RewardMilestone milestone)
        {
            return (this.TournamentInfo.RewardMilestones.IndexOf(milestone) + 1);
        }

        public RewardMilestone GetMilestoneWithLowestCompletion(ref float lowestCompletion, ref string tournamentId)
        {
            if (!this.FullyComposed)
            {
                return null;
            }
            RewardMilestone milestone = null;
            int totalContribution = this.GetTotalContribution();
            for (int i = 0; i < this.TournamentInfo.RewardMilestones.Count; i++)
            {
                RewardMilestone rewardMilestone = this.TournamentInfo.RewardMilestones[i];
                if ((rewardMilestone.Threshold > this.Instance.HighestClaimedMilestoneThreshold) && (rewardMilestone.Threshold <= totalContribution))
                {
                    float completionAtMilestone = this.TournamentInfo.GetCompletionAtMilestone(rewardMilestone);
                    if (completionAtMilestone < lowestCompletion)
                    {
                        lowestCompletion = completionAtMilestone;
                        milestone = rewardMilestone;
                        tournamentId = this.TournamentInfo.Id;
                    }
                }
            }
            return milestone;
        }

        public RewardMilestone getNextMilestone()
        {
            int totalContribution = this.GetTotalContribution();
            for (int i = 0; i < this.TournamentInfo.RewardMilestones.Count; i++)
            {
                RewardMilestone milestone = this.TournamentInfo.RewardMilestones[i];
                if (milestone.Threshold > totalContribution)
                {
                    return milestone;
                }
            }
            return null;
        }

        public static TournamentView getNextTournamentView(List<TournamentView> views, Player player)
        {
            if (views.Count == 0)
            {
                return null;
            }
            views.Sort(s_stableViewComparer);
            TournamentView view = null;
            for (int i = 0; i < views.Count; i++)
            {
                if (((player.Tournaments.LatestCompletedTournamentStartTime < views[i].TournamentInfo.StartTimeEpoch) && !player.Tournaments.playerHasCompletedTournament(views[i].TournamentInfo.Id)) && ((views[i].ServerJoinStatus == TournamentViewRemote.Status.OK) || (views[i].ServerJoinStatus == TournamentViewRemote.Status.TooEarlyForJoin)))
                {
                    if (view == null)
                    {
                        view = views[i];
                        if (view.TournamentInfo.SegmentedTournament)
                        {
                            return view;
                        }
                    }
                    else if (view.TournamentInfo.OverlapsWith(views[i].TournamentInfo) && views[i].TournamentInfo.SegmentedTournament)
                    {
                        return views[i];
                    }
                }
            }
            return view;
        }

        public long GetSecondsUntilEnd()
        {
            if (this.BucketStartTime == -1L)
            {
                return -1L;
            }
            return MathUtil.Clamp((long) ((this.BucketStartTime + this.TournamentInfo.DurationSeconds) - Service.Binder.ServerTime.GameTime), (long) 0L, (long) 0x7fffffffffffffffL);
        }

        public int GetTotalContribution()
        {
            this.RefreshPlayerEntry();
            int num = 0;
            for (int i = 0; i < this.TournamentEntries.Count; i++)
            {
                TournamentEntry entry = this.TournamentEntries[i];
                num += entry.Contribution;
            }
            return num;
        }

        public void JoinTournament(TournamentInstance instance)
        {
            this.Instance = instance;
            this.PlayerEntry = new TournamentEntry();
        }

        public void RefreshPlayerEntry()
        {
            if (this.PlayerHasJoined)
            {
                if (this.PlayerEntry == null)
                {
                    Debug.LogWarning("Refreshing a null player entry, this should not happen in normal circumstances.");
                    this.PlayerEntry = new TournamentEntry();
                }
                Player player = GameLogic.Binder.GameState.Player;
                this.PlayerEntry.PlayerId = player._id;
                this.PlayerEntry.Contribution = this.Instance.GetContribution();
                this.PlayerEntry.PlayerDisplayName = player.SocialData.Name;
                this.PlayerEntry.NormalUpgrades = this.Instance.Upgrades.NormalUpgrades;
                this.PlayerEntry.EpicUpgrades = this.Instance.Upgrades.EpicUpgrades;
                this.TournamentEntries.Sort(TournamentEntry.StableSortByContribution);
            }
        }

        public void UpdateEntries(List<TournamentEntry> entries)
        {
            if ((entries != null) && (entries.Count != 0))
            {
                this.TournamentEntries = entries;
                if (<>f__am$cache9 == null)
                {
                    <>f__am$cache9 = delegate (TournamentEntry te) {
                        return te.PlayerId == GameLogic.Binder.GameState.Player._id;
                    };
                }
                this.PlayerEntry = this.TournamentEntries.Find(<>f__am$cache9);
                if (this.PlayerEntry == null)
                {
                    Debug.LogError(string.Format("Could not find the player's own entry from TournamentEntries. TournamentId: {0}", this.TournamentInfo.Id));
                }
                this.TournamentEntries.Sort(TournamentEntry.StableSortByContribution);
                if (this.Instance != null)
                {
                    this.Instance.Upgrades.initializeWithExternalUpgrades(entries);
                }
            }
        }

        public void updateHighestClaimedMilestoneThreshold(int threshold)
        {
            if (this.FullyComposed)
            {
                this.Instance.HighestClaimedMilestoneThreshold = threshold;
                if ((this.Instance.CurrentState == TournamentInstance.State.ACTIVE) && (this.CountClaimedMilestones() >= this.getMilestoneCount()))
                {
                    this.Instance.CurrentState = TournamentInstance.State.PENDING_END_ANNOUNCEMENT;
                }
            }
        }

        public void UpdateLog(TournamentLog newLog)
        {
            if (this.FullyComposed)
            {
                this.Log.UpdateLog(newLog.LogEvents);
            }
        }

        public string BucketId
        {
            [CompilerGenerated]
            get
            {
                return this.<BucketId>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<BucketId>k__BackingField = value;
            }
        }

        public long BucketStartTime
        {
            [CompilerGenerated]
            get
            {
                return this.<BucketStartTime>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<BucketStartTime>k__BackingField = value;
            }
        }

        public bool FullyComposed
        {
            get
            {
                return (((this.TournamentInfo != null) && (this.Instance != null)) && (this.TournamentEntries.Count > 0));
            }
        }

        public TournamentInstance Instance
        {
            [CompilerGenerated]
            get
            {
                return this.<Instance>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Instance>k__BackingField = value;
            }
        }

        public TournamentLog Log
        {
            get
            {
                if (this.m_log == null)
                {
                    this.m_log = new TournamentLog();
                    this.m_log.Initialize(this);
                }
                return this.m_log;
            }
        }

        public TournamentEntry PlayerEntry
        {
            [CompilerGenerated]
            get
            {
                return this.<PlayerEntry>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PlayerEntry>k__BackingField = value;
            }
        }

        public bool PlayerHasJoined
        {
            get
            {
                return (this.Instance != null);
            }
        }

        public TournamentViewRemote.Status ServerJoinStatus
        {
            [CompilerGenerated]
            get
            {
                return this.<ServerJoinStatus>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ServerJoinStatus>k__BackingField = value;
            }
        }

        public List<TournamentEntry> TournamentEntries
        {
            [CompilerGenerated]
            get
            {
                return this.<TournamentEntries>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<TournamentEntries>k__BackingField = value;
            }
        }

        public Service.TournamentInfo TournamentInfo
        {
            [CompilerGenerated]
            get
            {
                return this.<TournamentInfo>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<TournamentInfo>k__BackingField = value;
            }
        }

        public class TournamentViewStableSortComparer : IComparer<TournamentView>
        {
            public int Compare(TournamentView x, TournamentView y)
            {
                if ((x == null) && (y != null))
                {
                    return 1;
                }
                if ((y == null) && (x == null))
                {
                    return 0;
                }
                if (y == null)
                {
                    return -1;
                }
                return x.TournamentInfo.CompareStartTimes(y.TournamentInfo);
            }
        }
    }
}

