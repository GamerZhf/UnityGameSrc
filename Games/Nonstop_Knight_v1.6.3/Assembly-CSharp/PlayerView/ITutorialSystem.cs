namespace PlayerView
{
    using System;

    public interface ITutorialSystem
    {
        bool isContextTutorialActive();
        void startContextTutorial(string tutorialId);

        string ActiveContextTutorialId { get; }

        string ActiveFtueTutorialId { get; }

        bool StartCeremonyBlocking { get; }
    }
}

