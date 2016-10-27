namespace Service
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class TournamentContributionRequest
    {
        [CompilerGenerated]
        private Dictionary<string, TournamentContribution> <ContributionsPerTournament>k__BackingField;

        public Dictionary<string, TournamentContribution> ContributionsPerTournament
        {
            [CompilerGenerated]
            get
            {
                return this.<ContributionsPerTournament>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ContributionsPerTournament>k__BackingField = value;
            }
        }
    }
}

