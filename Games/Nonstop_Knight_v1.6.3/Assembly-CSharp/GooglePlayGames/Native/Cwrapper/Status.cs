namespace GooglePlayGames.Native.Cwrapper
{
    using System;

    internal static class Status
    {
        internal enum AuthStatus
        {
            ERROR_INTERNAL = -2,
            ERROR_NOT_AUTHORIZED = -3,
            ERROR_TIMEOUT = -5,
            ERROR_VERSION_UPDATE_REQUIRED = -4,
            VALID = 1
        }

        internal enum CommonErrorStatus
        {
            ERROR_INTERNAL = -2,
            ERROR_NOT_AUTHORIZED = -3,
            ERROR_TIMEOUT = -5
        }

        internal enum FlushStatus
        {
            ERROR_INTERNAL = -2,
            ERROR_NOT_AUTHORIZED = -3,
            ERROR_TIMEOUT = -5,
            ERROR_VERSION_UPDATE_REQUIRED = -4,
            FLUSHED = 4
        }

        internal enum MultiplayerStatus
        {
            ERROR_INACTIVE_MATCH = -8,
            ERROR_INTERNAL = -2,
            ERROR_INVALID_MATCH = -10,
            ERROR_INVALID_RESULTS = -9,
            ERROR_MATCH_ALREADY_REMATCHED = -7,
            ERROR_MATCH_OUT_OF_DATE = -11,
            ERROR_NOT_AUTHORIZED = -3,
            ERROR_REAL_TIME_ROOM_NOT_JOINED = -17,
            ERROR_TIMEOUT = -5,
            ERROR_VERSION_UPDATE_REQUIRED = -4,
            VALID = 1,
            VALID_BUT_STALE = 2
        }

        internal enum QuestAcceptStatus
        {
            ERROR_INTERNAL = -2,
            ERROR_NOT_AUTHORIZED = -3,
            ERROR_QUEST_NO_LONGER_AVAILABLE = -13,
            ERROR_QUEST_NOT_STARTED = -14,
            ERROR_TIMEOUT = -5,
            VALID = 1
        }

        internal enum QuestClaimMilestoneStatus
        {
            ERROR_INTERNAL = -2,
            ERROR_MILESTONE_ALREADY_CLAIMED = -15,
            ERROR_MILESTONE_CLAIM_FAILED = -16,
            ERROR_NOT_AUTHORIZED = -3,
            ERROR_TIMEOUT = -5,
            VALID = 1
        }

        internal enum ResponseStatus
        {
            ERROR_INTERNAL = -2,
            ERROR_LICENSE_CHECK_FAILED = -1,
            ERROR_NOT_AUTHORIZED = -3,
            ERROR_TIMEOUT = -5,
            ERROR_VERSION_UPDATE_REQUIRED = -4,
            VALID = 1,
            VALID_BUT_STALE = 2
        }

        internal enum SnapshotOpenStatus
        {
            ERROR_INTERNAL = -2,
            ERROR_NOT_AUTHORIZED = -3,
            ERROR_TIMEOUT = -5,
            VALID = 1,
            VALID_WITH_CONFLICT = 3
        }

        internal enum UIStatus
        {
            ERROR_CANCELED = -6,
            ERROR_INTERNAL = -2,
            ERROR_LEFT_ROOM = -18,
            ERROR_NOT_AUTHORIZED = -3,
            ERROR_TIMEOUT = -5,
            ERROR_UI_BUSY = -12,
            ERROR_VERSION_UPDATE_REQUIRED = -4,
            VALID = 1
        }
    }
}

