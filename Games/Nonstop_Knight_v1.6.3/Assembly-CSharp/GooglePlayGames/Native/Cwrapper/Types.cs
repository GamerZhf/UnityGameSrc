namespace GooglePlayGames.Native.Cwrapper
{
    using System;

    internal static class Types
    {
        internal enum AchievementState
        {
            HIDDEN = 1,
            REVEALED = 2,
            UNLOCKED = 3
        }

        internal enum AchievementType
        {
            INCREMENTAL = 2,
            STANDARD = 1
        }

        internal enum AuthOperation
        {
            SIGN_IN = 1,
            SIGN_OUT = 2
        }

        internal enum DataSource
        {
            CACHE_OR_NETWORK = 1,
            NETWORK_ONLY = 2
        }

        internal enum EventVisibility
        {
            HIDDEN = 1,
            REVEALED = 2
        }

        internal enum ImageResolution
        {
            HI_RES = 2,
            ICON = 1
        }

        internal enum LeaderboardCollection
        {
            PUBLIC = 1,
            SOCIAL = 2
        }

        internal enum LeaderboardOrder
        {
            LARGER_IS_BETTER = 1,
            SMALLER_IS_BETTER = 2
        }

        internal enum LeaderboardStart
        {
            PLAYER_CENTERED = 2,
            TOP_SCORES = 1
        }

        internal enum LeaderboardTimeSpan
        {
            ALL_TIME = 3,
            DAILY = 1,
            WEEKLY = 2
        }

        internal enum LogLevel
        {
            ERROR = 4,
            INFO = 2,
            VERBOSE = 1,
            WARNING = 3
        }

        internal enum MatchResult
        {
            DISAGREED = 1,
            DISCONNECTED = 2,
            LOSS = 3,
            NONE = 4,
            TIE = 5,
            WIN = 6
        }

        internal enum MatchStatus
        {
            CANCELED = 6,
            COMPLETED = 5,
            EXPIRED = 7,
            INVITED = 1,
            MY_TURN = 3,
            PENDING_COMPLETION = 4,
            THEIR_TURN = 2
        }

        internal enum MultiplayerEvent
        {
            REMOVED = 3,
            UPDATED = 1,
            UPDATED_FROM_APP_LAUNCH = 2
        }

        internal enum MultiplayerInvitationType
        {
            REAL_TIME = 2,
            TURN_BASED = 1
        }

        internal enum ParticipantStatus
        {
            DECLINED = 3,
            FINISHED = 6,
            INVITED = 1,
            JOINED = 2,
            LEFT = 4,
            NOT_INVITED_YET = 5,
            UNRESPONSIVE = 7
        }

        internal enum QuestMilestoneState
        {
            CLAIMED = 4,
            COMPLETED_NOT_CLAIMED = 3,
            NOT_COMPLETED = 2,
            NOT_STARTED = 1
        }

        internal enum QuestState
        {
            ACCEPTED = 3,
            COMPLETED = 4,
            EXPIRED = 5,
            FAILED = 6,
            OPEN = 2,
            UPCOMING = 1
        }

        internal enum RealTimeRoomStatus
        {
            ACTIVE = 4,
            AUTO_MATCHING = 3,
            CONNECTING = 2,
            DELETED = 5,
            INVITING = 1
        }

        internal enum SnapshotConflictPolicy
        {
            LAST_KNOWN_GOOD = 3,
            LONGEST_PLAYTIME = 2,
            MANUAL = 1,
            MOST_RECENTLY_MODIFIED = 4
        }
    }
}

