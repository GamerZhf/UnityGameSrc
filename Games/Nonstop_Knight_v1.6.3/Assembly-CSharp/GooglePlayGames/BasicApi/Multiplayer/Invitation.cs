namespace GooglePlayGames.BasicApi.Multiplayer
{
    using System;

    public class Invitation
    {
        private string mInvitationId;
        private InvType mInvitationType;
        private Participant mInviter;
        private int mVariant;

        internal Invitation(InvType invType, string invId, Participant inviter, int variant)
        {
            this.mInvitationType = invType;
            this.mInvitationId = invId;
            this.mInviter = inviter;
            this.mVariant = variant;
        }

        public override string ToString()
        {
            object[] args = new object[] { this.InvitationType, this.InvitationId, this.Inviter, this.Variant };
            return string.Format("[Invitation: InvitationType={0}, InvitationId={1}, Inviter={2}, Variant={3}]", args);
        }

        public string InvitationId
        {
            get
            {
                return this.mInvitationId;
            }
        }

        public InvType InvitationType
        {
            get
            {
                return this.mInvitationType;
            }
        }

        public Participant Inviter
        {
            get
            {
                return this.mInviter;
            }
        }

        public int Variant
        {
            get
            {
                return this.mVariant;
            }
        }

        public enum InvType
        {
            RealTime,
            TurnBased,
            Unknown
        }
    }
}

