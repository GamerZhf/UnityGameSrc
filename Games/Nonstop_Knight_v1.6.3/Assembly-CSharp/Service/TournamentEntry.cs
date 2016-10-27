namespace Service
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class TournamentEntry
    {
        [CompilerGenerated]
        private int <Contribution>k__BackingField;
        [CompilerGenerated]
        private string <PlayerDisplayName>k__BackingField;
        [CompilerGenerated]
        private string <PlayerId>k__BackingField;
        public Dictionary<string, TournamentUpgradeInstance> EpicUpgrades;
        public Dictionary<string, TournamentUpgradeInstance> NormalUpgrades;
        public static TournamentEntryStableSortComparer StableSortByContribution = new TournamentEntryStableSortComparer();

        public TournamentEntry()
        {
            this.NormalUpgrades = new Dictionary<string, TournamentUpgradeInstance>();
            this.EpicUpgrades = new Dictionary<string, TournamentUpgradeInstance>();
        }

        public TournamentEntry(string playerId, string playerDisplayName, int entry)
        {
            this.NormalUpgrades = new Dictionary<string, TournamentUpgradeInstance>();
            this.EpicUpgrades = new Dictionary<string, TournamentUpgradeInstance>();
            this.PlayerId = playerId;
            this.PlayerDisplayName = playerDisplayName;
            this.Contribution = entry;
        }

        public int Contribution
        {
            [CompilerGenerated]
            get
            {
                return this.<Contribution>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Contribution>k__BackingField = value;
            }
        }

        public string PlayerDisplayName
        {
            [CompilerGenerated]
            get
            {
                return this.<PlayerDisplayName>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<PlayerDisplayName>k__BackingField = value;
            }
        }

        public string PlayerId
        {
            [CompilerGenerated]
            get
            {
                return this.<PlayerId>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<PlayerId>k__BackingField = value;
            }
        }

        public class TournamentEntryStableSortComparer : IComparer<TournamentEntry>
        {
            public int Compare(TournamentEntry x, TournamentEntry y)
            {
                if (x.Contribution != y.Contribution)
                {
                    return y.Contribution.CompareTo(x.Contribution);
                }
                return string.Compare(y.PlayerId, x.PlayerId, StringComparison.Ordinal);
            }
        }
    }
}

