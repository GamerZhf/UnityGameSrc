namespace GooglePlayGames.BasicApi.Multiplayer
{
    using System;

    public class Participant : IComparable<Participant>
    {
        private string mDisplayName = string.Empty;
        private bool mIsConnectedToRoom;
        private string mParticipantId = string.Empty;
        private GooglePlayGames.BasicApi.Multiplayer.Player mPlayer;
        private ParticipantStatus mStatus = ParticipantStatus.Unknown;

        internal Participant(string displayName, string participantId, ParticipantStatus status, GooglePlayGames.BasicApi.Multiplayer.Player player, bool connectedToRoom)
        {
            this.mDisplayName = displayName;
            this.mParticipantId = participantId;
            this.mStatus = status;
            this.mPlayer = player;
            this.mIsConnectedToRoom = connectedToRoom;
        }

        public int CompareTo(Participant other)
        {
            return this.mParticipantId.CompareTo(other.mParticipantId);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof(Participant))
            {
                return false;
            }
            Participant participant = (Participant) obj;
            return this.mParticipantId.Equals(participant.mParticipantId);
        }

        public override int GetHashCode()
        {
            return ((this.mParticipantId == null) ? 0 : this.mParticipantId.GetHashCode());
        }

        public override string ToString()
        {
            object[] args = new object[] { this.mDisplayName, this.mParticipantId, this.mStatus.ToString(), (this.mPlayer != null) ? this.mPlayer.ToString() : "NULL", this.mIsConnectedToRoom };
            return string.Format("[Participant: '{0}' (id {1}), status={2}, player={3}, connected={4}]", args);
        }

        public string DisplayName
        {
            get
            {
                return this.mDisplayName;
            }
        }

        public bool IsAutomatch
        {
            get
            {
                return (this.mPlayer == null);
            }
        }

        public bool IsConnectedToRoom
        {
            get
            {
                return this.mIsConnectedToRoom;
            }
        }

        public string ParticipantId
        {
            get
            {
                return this.mParticipantId;
            }
        }

        public GooglePlayGames.BasicApi.Multiplayer.Player Player
        {
            get
            {
                return this.mPlayer;
            }
        }

        public ParticipantStatus Status
        {
            get
            {
                return this.mStatus;
            }
        }

        public enum ParticipantStatus
        {
            NotInvitedYet,
            Invited,
            Joined,
            Declined,
            Left,
            Finished,
            Unresponsive,
            Unknown
        }
    }
}

