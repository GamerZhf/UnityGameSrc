namespace GooglePlayGames.BasicApi
{
    using GooglePlayGames;
    using GooglePlayGames.BasicApi.Multiplayer;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PlayGamesClientConfiguration
    {
        public static readonly PlayGamesClientConfiguration DefaultConfiguration;
        private readonly bool mEnableSavedGames;
        private readonly bool mRequireGooglePlus;
        private readonly InvitationReceivedDelegate mInvitationDelegate;
        private readonly GooglePlayGames.BasicApi.Multiplayer.MatchDelegate mMatchDelegate;
        private readonly string mPermissionRationale;
        private PlayGamesClientConfiguration(Builder builder)
        {
            this.mEnableSavedGames = builder.HasEnableSaveGames();
            this.mInvitationDelegate = builder.GetInvitationDelegate();
            this.mMatchDelegate = builder.GetMatchDelegate();
            this.mPermissionRationale = builder.GetPermissionRationale();
            this.mRequireGooglePlus = builder.HasRequireGooglePlus();
        }

        static PlayGamesClientConfiguration()
        {
            DefaultConfiguration = new Builder().WithPermissionRationale("Select email address to send to this game or hit cancel to not share.").Build();
        }

        public bool EnableSavedGames
        {
            get
            {
                return this.mEnableSavedGames;
            }
        }
        public bool RequireGooglePlus
        {
            get
            {
                return this.mRequireGooglePlus;
            }
        }
        public InvitationReceivedDelegate InvitationDelegate
        {
            get
            {
                return this.mInvitationDelegate;
            }
        }
        public GooglePlayGames.BasicApi.Multiplayer.MatchDelegate MatchDelegate
        {
            get
            {
                return this.mMatchDelegate;
            }
        }
        public string PermissionRationale
        {
            get
            {
                return this.mPermissionRationale;
            }
        }
        public class Builder
        {
            [CompilerGenerated]
            private static InvitationReceivedDelegate <>f__am$cache5;
            [CompilerGenerated]
            private static MatchDelegate <>f__am$cache6;
            private bool mEnableSaveGames;
            private InvitationReceivedDelegate mInvitationDelegate;
            private MatchDelegate mMatchDelegate;
            private string mRationale;
            private bool mRequireGooglePlus;

            public Builder()
            {
                if (<>f__am$cache5 == null)
                {
                    <>f__am$cache5 = new InvitationReceivedDelegate(PlayGamesClientConfiguration.Builder.<mInvitationDelegate>m__86);
                }
                this.mInvitationDelegate = <>f__am$cache5;
                if (<>f__am$cache6 == null)
                {
                    <>f__am$cache6 = new MatchDelegate(PlayGamesClientConfiguration.Builder.<mMatchDelegate>m__87);
                }
                this.mMatchDelegate = <>f__am$cache6;
            }

            [CompilerGenerated]
            private static void <mInvitationDelegate>m__86(Invitation, bool)
            {
            }

            [CompilerGenerated]
            private static void <mMatchDelegate>m__87(TurnBasedMatch, bool)
            {
            }

            public PlayGamesClientConfiguration Build()
            {
                this.mRequireGooglePlus = GameInfo.RequireGooglePlus();
                return new PlayGamesClientConfiguration(this);
            }

            public PlayGamesClientConfiguration.Builder EnableSavedGames()
            {
                this.mEnableSaveGames = true;
                return this;
            }

            internal InvitationReceivedDelegate GetInvitationDelegate()
            {
                return this.mInvitationDelegate;
            }

            internal MatchDelegate GetMatchDelegate()
            {
                return this.mMatchDelegate;
            }

            internal string GetPermissionRationale()
            {
                return this.mRationale;
            }

            internal bool HasEnableSaveGames()
            {
                return this.mEnableSaveGames;
            }

            internal bool HasRequireGooglePlus()
            {
                return this.mRequireGooglePlus;
            }

            public PlayGamesClientConfiguration.Builder RequireGooglePlus()
            {
                this.mRequireGooglePlus = true;
                return this;
            }

            public PlayGamesClientConfiguration.Builder WithInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
            {
                this.mInvitationDelegate = Misc.CheckNotNull<InvitationReceivedDelegate>(invitationDelegate);
                return this;
            }

            public PlayGamesClientConfiguration.Builder WithMatchDelegate(MatchDelegate matchDelegate)
            {
                this.mMatchDelegate = Misc.CheckNotNull<MatchDelegate>(matchDelegate);
                return this;
            }

            public PlayGamesClientConfiguration.Builder WithPermissionRationale(string rationale)
            {
                this.mRationale = rationale;
                return this;
            }
        }
    }
}

