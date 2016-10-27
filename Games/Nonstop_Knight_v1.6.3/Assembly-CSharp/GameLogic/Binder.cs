namespace GameLogic
{
    using App;
    using System;
    using System.Runtime.CompilerServices;

    public static class Binder
    {
        [CompilerGenerated]
        private static ITypedObjectPool<AreaEffect, AreaEffectType> <AreaEffectPool>k__BackingField;
        [CompilerGenerated]
        private static IBlinkSystem <BlinkSystem>k__BackingField;
        [CompilerGenerated]
        private static IBoostSystem <BoostSystem>k__BackingField;
        [CompilerGenerated]
        private static IBuffSystem <BuffSystem>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<CharacterInstance> <CharacterPool>k__BackingField;
        [CompilerGenerated]
        private static GameLogic.CharacterResources <CharacterResources>k__BackingField;
        [CompilerGenerated]
        private static ICharacterSpawningSystem <CharacterSpawningSystem>k__BackingField;
        [CompilerGenerated]
        private static ICommandProcessor <CommandProcessor>k__BackingField;
        [CompilerGenerated]
        private static ICoroutineExecutor <CoroutineExecutor>k__BackingField;
        [CompilerGenerated]
        private static IDeathSystem <DeathSystem>k__BackingField;
        [CompilerGenerated]
        private static IDecoSpawningSystem <DecoSpawningSystem>k__BackingField;
        [CompilerGenerated]
        private static GameLogic.DungeonBlockResources <DungeonBlockResources>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<DungeonBoost> <DungeonBoostPool>k__BackingField;
        [CompilerGenerated]
        private static IDungeonBoostSpawningSystem <DungeonBoostSpawningSystem>k__BackingField;
        [CompilerGenerated]
        private static GameLogic.DungeonDecoResources <DungeonDecoResources>k__BackingField;
        [CompilerGenerated]
        private static GameLogic.DungeonEventRules <DungeonEventRules>k__BackingField;
        [CompilerGenerated]
        private static GameLogic.DungeonMoodResources <DungeonMoodResources>k__BackingField;
        [CompilerGenerated]
        private static GameLogic.DungeonResources <DungeonResources>k__BackingField;
        [CompilerGenerated]
        private static GameLogic.EpisodeResources <EpisodeResources>k__BackingField;
        [CompilerGenerated]
        private static GameLogic.IEventBus <EventBus>k__BackingField;
        [CompilerGenerated]
        private static IFrenzySystem <FrenzySystem>k__BackingField;
        [CompilerGenerated]
        private static GameLogic.GameLogicContext <GameLogicContext>k__BackingField;
        [CompilerGenerated]
        private static IGameplayStateMachine <GameplayStateMachine>k__BackingField;
        [CompilerGenerated]
        private static GameLogic.GameState <GameState>k__BackingField;
        [CompilerGenerated]
        private static IHealthSystem <HealthSystem>k__BackingField;
        [CompilerGenerated]
        private static IHeroStatRecordingSystem <HeroStatRecordingSystem>k__BackingField;
        [CompilerGenerated]
        private static IIslandSpawningSystem <IslandSpawningSystem>k__BackingField;
        [CompilerGenerated]
        private static GameLogic.ItemResources <ItemResources>k__BackingField;
        [CompilerGenerated]
        private static ILeaderboardSystem <LeaderboardSystem>k__BackingField;
        [CompilerGenerated]
        private static GameLogic.LevelUpRules <LevelUpRules>k__BackingField;
        [CompilerGenerated]
        private static ILootSystem <LootSystem>k__BackingField;
        [CompilerGenerated]
        private static IMissionSystem <MissionSystem>k__BackingField;
        [CompilerGenerated]
        private static IPassiveTreasureGainSystem <PassiveTreasureGainSystem>k__BackingField;
        [CompilerGenerated]
        private static IPathfindingSystem <PathfindingSystem>k__BackingField;
        [CompilerGenerated]
        private static IPerkSystem <PerkSystem>k__BackingField;
        [CompilerGenerated]
        private static GameLogic.PlayerAugmentationResources <PlayerAugmentationResources>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<Projectile> <ProjectilePool>k__BackingField;
        [CompilerGenerated]
        private static IRenewableResourceSystem <RenewableResourceSystem>k__BackingField;
        [CompilerGenerated]
        private static ISkillSystem <SkillSystem>k__BackingField;
        [CompilerGenerated]
        private static ITimeSystem <TimeSystem>k__BackingField;
        [CompilerGenerated]
        private static GameLogic.UpgradeRules <UpgradeRules>k__BackingField;
        [CompilerGenerated]
        private static IVendorSystem <VendorSystem>k__BackingField;

        public static ITypedObjectPool<AreaEffect, AreaEffectType> AreaEffectPool
        {
            [CompilerGenerated]
            get
            {
                return <AreaEffectPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <AreaEffectPool>k__BackingField = value;
            }
        }

        public static IBlinkSystem BlinkSystem
        {
            [CompilerGenerated]
            get
            {
                return <BlinkSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <BlinkSystem>k__BackingField = value;
            }
        }

        public static IBoostSystem BoostSystem
        {
            [CompilerGenerated]
            get
            {
                return <BoostSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <BoostSystem>k__BackingField = value;
            }
        }

        public static IBuffSystem BuffSystem
        {
            [CompilerGenerated]
            get
            {
                return <BuffSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <BuffSystem>k__BackingField = value;
            }
        }

        public static IObjectPool<CharacterInstance> CharacterPool
        {
            [CompilerGenerated]
            get
            {
                return <CharacterPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <CharacterPool>k__BackingField = value;
            }
        }

        public static GameLogic.CharacterResources CharacterResources
        {
            [CompilerGenerated]
            get
            {
                return <CharacterResources>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <CharacterResources>k__BackingField = value;
            }
        }

        public static ICharacterSpawningSystem CharacterSpawningSystem
        {
            [CompilerGenerated]
            get
            {
                return <CharacterSpawningSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <CharacterSpawningSystem>k__BackingField = value;
            }
        }

        public static ICommandProcessor CommandProcessor
        {
            [CompilerGenerated]
            get
            {
                return <CommandProcessor>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <CommandProcessor>k__BackingField = value;
            }
        }

        public static ICoroutineExecutor CoroutineExecutor
        {
            [CompilerGenerated]
            get
            {
                return <CoroutineExecutor>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <CoroutineExecutor>k__BackingField = value;
            }
        }

        public static IDeathSystem DeathSystem
        {
            [CompilerGenerated]
            get
            {
                return <DeathSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <DeathSystem>k__BackingField = value;
            }
        }

        public static IDecoSpawningSystem DecoSpawningSystem
        {
            [CompilerGenerated]
            get
            {
                return <DecoSpawningSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <DecoSpawningSystem>k__BackingField = value;
            }
        }

        public static GameLogic.DungeonBlockResources DungeonBlockResources
        {
            [CompilerGenerated]
            get
            {
                return <DungeonBlockResources>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <DungeonBlockResources>k__BackingField = value;
            }
        }

        public static IObjectPool<DungeonBoost> DungeonBoostPool
        {
            [CompilerGenerated]
            get
            {
                return <DungeonBoostPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <DungeonBoostPool>k__BackingField = value;
            }
        }

        public static IDungeonBoostSpawningSystem DungeonBoostSpawningSystem
        {
            [CompilerGenerated]
            get
            {
                return <DungeonBoostSpawningSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <DungeonBoostSpawningSystem>k__BackingField = value;
            }
        }

        public static GameLogic.DungeonDecoResources DungeonDecoResources
        {
            [CompilerGenerated]
            get
            {
                return <DungeonDecoResources>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <DungeonDecoResources>k__BackingField = value;
            }
        }

        public static GameLogic.DungeonEventRules DungeonEventRules
        {
            [CompilerGenerated]
            get
            {
                return <DungeonEventRules>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <DungeonEventRules>k__BackingField = value;
            }
        }

        public static GameLogic.DungeonMoodResources DungeonMoodResources
        {
            [CompilerGenerated]
            get
            {
                return <DungeonMoodResources>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <DungeonMoodResources>k__BackingField = value;
            }
        }

        public static GameLogic.DungeonResources DungeonResources
        {
            [CompilerGenerated]
            get
            {
                return <DungeonResources>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <DungeonResources>k__BackingField = value;
            }
        }

        public static GameLogic.EpisodeResources EpisodeResources
        {
            [CompilerGenerated]
            get
            {
                return <EpisodeResources>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <EpisodeResources>k__BackingField = value;
            }
        }

        public static GameLogic.IEventBus EventBus
        {
            [CompilerGenerated]
            get
            {
                return <EventBus>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <EventBus>k__BackingField = value;
            }
        }

        public static IFrenzySystem FrenzySystem
        {
            [CompilerGenerated]
            get
            {
                return <FrenzySystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <FrenzySystem>k__BackingField = value;
            }
        }

        public static GameLogic.GameLogicContext GameLogicContext
        {
            [CompilerGenerated]
            get
            {
                return <GameLogicContext>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <GameLogicContext>k__BackingField = value;
            }
        }

        public static IGameplayStateMachine GameplayStateMachine
        {
            [CompilerGenerated]
            get
            {
                return <GameplayStateMachine>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <GameplayStateMachine>k__BackingField = value;
            }
        }

        public static GameLogic.GameState GameState
        {
            [CompilerGenerated]
            get
            {
                return <GameState>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <GameState>k__BackingField = value;
            }
        }

        public static IHealthSystem HealthSystem
        {
            [CompilerGenerated]
            get
            {
                return <HealthSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <HealthSystem>k__BackingField = value;
            }
        }

        public static IHeroStatRecordingSystem HeroStatRecordingSystem
        {
            [CompilerGenerated]
            get
            {
                return <HeroStatRecordingSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <HeroStatRecordingSystem>k__BackingField = value;
            }
        }

        public static IIslandSpawningSystem IslandSpawningSystem
        {
            [CompilerGenerated]
            get
            {
                return <IslandSpawningSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <IslandSpawningSystem>k__BackingField = value;
            }
        }

        public static GameLogic.ItemResources ItemResources
        {
            [CompilerGenerated]
            get
            {
                return <ItemResources>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <ItemResources>k__BackingField = value;
            }
        }

        public static ILeaderboardSystem LeaderboardSystem
        {
            [CompilerGenerated]
            get
            {
                return <LeaderboardSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <LeaderboardSystem>k__BackingField = value;
            }
        }

        public static GameLogic.LevelUpRules LevelUpRules
        {
            [CompilerGenerated]
            get
            {
                return <LevelUpRules>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <LevelUpRules>k__BackingField = value;
            }
        }

        public static ILootSystem LootSystem
        {
            [CompilerGenerated]
            get
            {
                return <LootSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <LootSystem>k__BackingField = value;
            }
        }

        public static IMissionSystem MissionSystem
        {
            [CompilerGenerated]
            get
            {
                return <MissionSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <MissionSystem>k__BackingField = value;
            }
        }

        public static IPassiveTreasureGainSystem PassiveTreasureGainSystem
        {
            [CompilerGenerated]
            get
            {
                return <PassiveTreasureGainSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <PassiveTreasureGainSystem>k__BackingField = value;
            }
        }

        public static IPathfindingSystem PathfindingSystem
        {
            [CompilerGenerated]
            get
            {
                return <PathfindingSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <PathfindingSystem>k__BackingField = value;
            }
        }

        public static IPerkSystem PerkSystem
        {
            [CompilerGenerated]
            get
            {
                return <PerkSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <PerkSystem>k__BackingField = value;
            }
        }

        public static GameLogic.PlayerAugmentationResources PlayerAugmentationResources
        {
            [CompilerGenerated]
            get
            {
                return <PlayerAugmentationResources>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <PlayerAugmentationResources>k__BackingField = value;
            }
        }

        public static IObjectPool<Projectile> ProjectilePool
        {
            [CompilerGenerated]
            get
            {
                return <ProjectilePool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <ProjectilePool>k__BackingField = value;
            }
        }

        public static IRenewableResourceSystem RenewableResourceSystem
        {
            [CompilerGenerated]
            get
            {
                return <RenewableResourceSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <RenewableResourceSystem>k__BackingField = value;
            }
        }

        public static ISkillSystem SkillSystem
        {
            [CompilerGenerated]
            get
            {
                return <SkillSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <SkillSystem>k__BackingField = value;
            }
        }

        public static ITimeSystem TimeSystem
        {
            [CompilerGenerated]
            get
            {
                return <TimeSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <TimeSystem>k__BackingField = value;
            }
        }

        public static GameLogic.UpgradeRules UpgradeRules
        {
            [CompilerGenerated]
            get
            {
                return <UpgradeRules>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <UpgradeRules>k__BackingField = value;
            }
        }

        public static IVendorSystem VendorSystem
        {
            [CompilerGenerated]
            get
            {
                return <VendorSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <VendorSystem>k__BackingField = value;
            }
        }
    }
}

