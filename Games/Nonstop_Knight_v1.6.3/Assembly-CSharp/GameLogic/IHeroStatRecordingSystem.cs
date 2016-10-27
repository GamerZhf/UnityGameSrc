namespace GameLogic
{
    using System;

    public interface IHeroStatRecordingSystem
    {
        void registerHeroStatsTarget(HeroStats heroStats);
        void unregisterAllHeroStatsTargets();
        void unregisterHeroStatsTarget(HeroStats heroStats);

        GameLogic.RealtimeCombatStats RealtimeCombatStats { get; }
    }
}

