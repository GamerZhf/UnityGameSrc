namespace Service
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [Serializable]
    public class TournamentContribution
    {
        [CompilerGenerated]
        private TournamentEntry <Entry>k__BackingField;
        [CompilerGenerated]
        private List<TournamentLogEvent> <LogEvents>k__BackingField;

        public TournamentContribution()
        {
        }

        public TournamentContribution(TournamentEntry entry, [Optional, DefaultParameterValue(null)] List<TournamentLogEvent> logEvents)
        {
            this.Entry = entry;
            if (logEvents == null)
            {
            }
            this.LogEvents = new List<TournamentLogEvent>();
        }

        public TournamentEntry Entry
        {
            [CompilerGenerated]
            get
            {
                return this.<Entry>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Entry>k__BackingField = value;
            }
        }

        public List<TournamentLogEvent> LogEvents
        {
            [CompilerGenerated]
            get
            {
                return this.<LogEvents>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<LogEvents>k__BackingField = value;
            }
        }
    }
}

