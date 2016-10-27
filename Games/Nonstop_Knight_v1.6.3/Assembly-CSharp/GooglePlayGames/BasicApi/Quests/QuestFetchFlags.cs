namespace GooglePlayGames.BasicApi.Quests
{
    using System;

    [Flags]
    public enum QuestFetchFlags
    {
        Accepted = 4,
        All = -1,
        Completed = 8,
        CompletedNotClaimed = 0x10,
        EndingSoon = 0x40,
        Expired = 0x20,
        Failed = 0x80,
        Open = 2,
        Upcoming = 1
    }
}

