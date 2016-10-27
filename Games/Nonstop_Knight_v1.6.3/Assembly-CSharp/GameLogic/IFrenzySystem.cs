namespace GameLogic
{
    using System;

    public interface IFrenzySystem
    {
        void activateFrenzy();
        void deactivateFrenzy();
        float getDuration(ICharacterStatModifier target);
        float getDurationBonusPerKill();
        float getNormalizedFrenzyGauge();
        bool isFrenzyActive();
    }
}

