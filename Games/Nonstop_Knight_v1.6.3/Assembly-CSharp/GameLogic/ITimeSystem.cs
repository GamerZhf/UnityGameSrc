namespace GameLogic
{
    using System;

    public interface ITimeSystem
    {
        void gameplaySlowdown(bool enabled);
        void pause(bool paused);
        bool paused();
        void speedCheat(float targetTimescale);
        void tutorialSlowdown(bool enabled);
        bool tutorialSlowdownEnabled();
    }
}

