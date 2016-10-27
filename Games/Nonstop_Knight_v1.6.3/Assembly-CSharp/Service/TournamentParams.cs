namespace Service
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class TournamentParams : CustomParams
    {
        [CompilerGenerated]
        private string <TournamentId>k__BackingField;
        private const string PARAM_TOURNAMENT_ID = "tournament-id";

        public TournamentParams(Dictionary<string, object> promoParams) : base(promoParams)
        {
            if (promoParams != null)
            {
                object obj2;
                promoParams.TryGetValue("tournament-id", out obj2);
                if (obj2 != null)
                {
                    this.TournamentId = obj2.ToString();
                }
            }
        }

        public string TournamentId
        {
            [CompilerGenerated]
            get
            {
                return this.<TournamentId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<TournamentId>k__BackingField = value;
            }
        }
    }
}

