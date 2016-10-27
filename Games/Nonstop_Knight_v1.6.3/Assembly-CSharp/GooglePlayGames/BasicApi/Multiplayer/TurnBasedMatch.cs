namespace GooglePlayGames.BasicApi.Multiplayer
{
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class TurnBasedMatch
    {
        [CompilerGenerated]
        private static Func<Participant, string> <>f__am$cacheB;
        private uint mAvailableAutomatchSlots;
        private bool mCanRematch;
        private byte[] mData;
        private string mMatchId;
        private MatchStatus mMatchStatus;
        private List<Participant> mParticipants;
        private string mPendingParticipantId;
        private string mSelfParticipantId;
        private MatchTurnStatus mTurnStatus;
        private uint mVariant;
        private uint mVersion;

        internal TurnBasedMatch(string matchId, byte[] data, bool canRematch, string selfParticipantId, List<Participant> participants, uint availableAutomatchSlots, string pendingParticipantId, MatchTurnStatus turnStatus, MatchStatus matchStatus, uint variant, uint version)
        {
            this.mMatchId = matchId;
            this.mData = data;
            this.mCanRematch = canRematch;
            this.mSelfParticipantId = selfParticipantId;
            this.mParticipants = participants;
            this.mParticipants.Sort();
            this.mAvailableAutomatchSlots = availableAutomatchSlots;
            this.mPendingParticipantId = pendingParticipantId;
            this.mTurnStatus = turnStatus;
            this.mMatchStatus = matchStatus;
            this.mVariant = variant;
            this.mVersion = version;
        }

        public Participant GetParticipant(string participantId)
        {
            foreach (Participant participant in this.mParticipants)
            {
                if (participant.ParticipantId.Equals(participantId))
                {
                    return participant;
                }
            }
            Logger.w("Participant not found in turn-based match: " + participantId);
            return null;
        }

        public override string ToString()
        {
            object[] args = new object[10];
            args[0] = this.mMatchId;
            args[1] = this.mData;
            args[2] = this.mCanRematch;
            args[3] = this.mSelfParticipantId;
            if (<>f__am$cacheB == null)
            {
                <>f__am$cacheB = delegate (Participant p) {
                    return p.ToString();
                };
            }
            args[4] = string.Join(",", Enumerable.ToArray<string>(Enumerable.Select<Participant, string>(this.mParticipants, <>f__am$cacheB)));
            args[5] = this.mPendingParticipantId;
            args[6] = this.mTurnStatus;
            args[7] = this.mMatchStatus;
            args[8] = this.mVariant;
            args[9] = this.mVersion;
            return string.Format("[TurnBasedMatch: mMatchId={0}, mData={1}, mCanRematch={2}, mSelfParticipantId={3}, mParticipants={4}, mPendingParticipantId={5}, mTurnStatus={6}, mMatchStatus={7}, mVariant={8}, mVersion={9}]", args);
        }

        public uint AvailableAutomatchSlots
        {
            get
            {
                return this.mAvailableAutomatchSlots;
            }
        }

        public bool CanRematch
        {
            get
            {
                return this.mCanRematch;
            }
        }

        public byte[] Data
        {
            get
            {
                return this.mData;
            }
        }

        public string MatchId
        {
            get
            {
                return this.mMatchId;
            }
        }

        public List<Participant> Participants
        {
            get
            {
                return this.mParticipants;
            }
        }

        public Participant PendingParticipant
        {
            get
            {
                return ((this.mPendingParticipantId != null) ? this.GetParticipant(this.mPendingParticipantId) : null);
            }
        }

        public string PendingParticipantId
        {
            get
            {
                return this.mPendingParticipantId;
            }
        }

        public Participant Self
        {
            get
            {
                return this.GetParticipant(this.mSelfParticipantId);
            }
        }

        public string SelfParticipantId
        {
            get
            {
                return this.mSelfParticipantId;
            }
        }

        public MatchStatus Status
        {
            get
            {
                return this.mMatchStatus;
            }
        }

        public MatchTurnStatus TurnStatus
        {
            get
            {
                return this.mTurnStatus;
            }
        }

        public uint Variant
        {
            get
            {
                return this.mVariant;
            }
        }

        public uint Version
        {
            get
            {
                return this.mVersion;
            }
        }

        public enum MatchStatus
        {
            Active,
            AutoMatching,
            Cancelled,
            Complete,
            Expired,
            Unknown,
            Deleted
        }

        public enum MatchTurnStatus
        {
            Complete,
            Invited,
            MyTurn,
            TheirTurn,
            Unknown
        }
    }
}

