namespace GameLogic
{
    using System;

    public enum GameplayState
    {
        UNDETERMINED,
        START_CEREMONY_STEP1,
        START_CEREMONY_STEP2,
        WAITING,
        ACTION,
        BOSS_START,
        BOSS_FIGHT,
        ROOM_COMPLETION,
        END_CEREMONY,
        REVIVAL,
        RETIREMENT,
        ENDED
    }
}

