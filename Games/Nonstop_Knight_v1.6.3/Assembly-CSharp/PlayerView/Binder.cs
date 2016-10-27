namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class Binder
    {
        [CompilerGenerated]
        private static PlayerView.AccessoryStorage <AccessoryStorage>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<AchievementCell> <AchievementCellPool>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.AdventureMilestones <AdventureMilestones>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.AnnouncementSystem <AnnouncementSystem>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.AudioSystem <AudioSystem>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<BossHuntTickerCell> <BossHuntTickerCellPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<BuffHudTimer> <BuffHudTimerPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<Card> <CardButtonPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<Card> <CardRewardPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<ChestGalleryRow> <ChestGalleryRowPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<CombatTextIngame> <CombatTextPoolIngame>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<CombatTextMenu> <CombatTextPoolMenu>k__BackingField;
        [CompilerGenerated]
        private static Material <DisabledUiMaterial>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<DungeonCell> <DungeonCellPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<DungeonDropView> <DungeonDropViewPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<DungeonEncounterCell> <DungeonEncounterCellPool>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.DungeonHud <DungeonHud>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.EffectSystem <EffectSystem>k__BackingField;
        [CompilerGenerated]
        private static IEventBus <EventBus>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<HeroCell> <HeroCellPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<HpIndicator> <HpIndicatorPool>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.InputSystem <InputSystem>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<ItemCard> <ItemCardPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<ItemCell> <ItemCellPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<ItemListCell> <ItemListCellPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<LeaderboardCell> <LeaderboardCellPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<LeaguePlayerCell> <LeaguePlayerCellPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<LeagueTitleCell> <LeagueTitleCellPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<MapCell> <MapCellPool>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.MarketingBuildController <MarketingBuildController>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.MaterialStorage <MaterialStorage>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.MenuContentResources <MenuContentResources>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.MenuHudBottom <MenuHudBottom>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.MenuHudTop <MenuHudTop>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.MenuSystem <MenuSystem>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<MenuTreasureChest> <MenuTreasureChestPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<MilestoneCell> <MilestoneCellPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<MissionCell> <MissionCellPool>k__BackingField;
        [CompilerGenerated]
        private static Material <MonochromeUiMaterial>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.NotificationSystem <NotificationSystem>k__BackingField;
        [CompilerGenerated]
        private static ITypedObjectPool<PoolableAudioSource, AudioSourceType> <PersistentAudioSourcePool>k__BackingField;
        [CompilerGenerated]
        private static ITypedObjectPool<CharacterView, CharacterPrefab> <PersistentCharacterViewPool>k__BackingField;
        [CompilerGenerated]
        private static ITypedObjectPool<PoolableParticleSystem, EffectType> <PersistentParticleEffectPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<PetCell> <PetCellPool>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.PlayerViewContext <PlayerViewContext>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<PromotionCard> <PromotionCardAugmentationPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<ResourceGainImage> <ResourceGainImagePool>k__BackingField;
        [CompilerGenerated]
        private static ITypedObjectPool<RewardGalleryCell, RewardGalleryCellType> <RewardGalleryCellPool>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.RoomView <RoomView>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<RunestoneCell> <RunestoneCellPool>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.ScreenTransitionEffect <ScreenTransitionEffect>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<ShopCell> <ShopCellPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<SkillCell> <SkillCellPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<SkillDualCell> <SkillDualCellPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<SkillHudButton> <SkillHudButtonPool>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<SkillToggle> <SkillTogglePool>k__BackingField;
        [CompilerGenerated]
        private static SlidingPanelController <SlidingAdventurePanelController>k__BackingField;
        [CompilerGenerated]
        private static SlidingPanelController <SlidingTaskPanelController>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.SpriteResources <SpriteResources>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<StatusIndicator> <StatusIndicatorPool>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.StatusIndicatorSystem <StatusIndicatorSystem>k__BackingField;
        [CompilerGenerated]
        private static IObjectPool<TaskPanelItem> <TaskPanelItemPool>k__BackingField;
        [CompilerGenerated]
        private static PlayerView.TransitionSystem <TransitionSystem>k__BackingField;
        [CompilerGenerated]
        private static RectTransform <TutorialCanvas>k__BackingField;
        [CompilerGenerated]
        private static ITutorialSystem <TutorialSystem>k__BackingField;

        public static PlayerView.AccessoryStorage AccessoryStorage
        {
            [CompilerGenerated]
            get
            {
                return <AccessoryStorage>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <AccessoryStorage>k__BackingField = value;
            }
        }

        public static IObjectPool<AchievementCell> AchievementCellPool
        {
            [CompilerGenerated]
            get
            {
                return <AchievementCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <AchievementCellPool>k__BackingField = value;
            }
        }

        public static PlayerView.AdventureMilestones AdventureMilestones
        {
            [CompilerGenerated]
            get
            {
                return <AdventureMilestones>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <AdventureMilestones>k__BackingField = value;
            }
        }

        public static PlayerView.AnnouncementSystem AnnouncementSystem
        {
            [CompilerGenerated]
            get
            {
                return <AnnouncementSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <AnnouncementSystem>k__BackingField = value;
            }
        }

        public static PlayerView.AudioSystem AudioSystem
        {
            [CompilerGenerated]
            get
            {
                return <AudioSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <AudioSystem>k__BackingField = value;
            }
        }

        public static IObjectPool<BossHuntTickerCell> BossHuntTickerCellPool
        {
            [CompilerGenerated]
            get
            {
                return <BossHuntTickerCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <BossHuntTickerCellPool>k__BackingField = value;
            }
        }

        public static IObjectPool<BuffHudTimer> BuffHudTimerPool
        {
            [CompilerGenerated]
            get
            {
                return <BuffHudTimerPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <BuffHudTimerPool>k__BackingField = value;
            }
        }

        public static IObjectPool<Card> CardButtonPool
        {
            [CompilerGenerated]
            get
            {
                return <CardButtonPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <CardButtonPool>k__BackingField = value;
            }
        }

        public static IObjectPool<Card> CardRewardPool
        {
            [CompilerGenerated]
            get
            {
                return <CardRewardPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <CardRewardPool>k__BackingField = value;
            }
        }

        public static IObjectPool<ChestGalleryRow> ChestGalleryRowPool
        {
            [CompilerGenerated]
            get
            {
                return <ChestGalleryRowPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <ChestGalleryRowPool>k__BackingField = value;
            }
        }

        public static IObjectPool<CombatTextIngame> CombatTextPoolIngame
        {
            [CompilerGenerated]
            get
            {
                return <CombatTextPoolIngame>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <CombatTextPoolIngame>k__BackingField = value;
            }
        }

        public static IObjectPool<CombatTextMenu> CombatTextPoolMenu
        {
            [CompilerGenerated]
            get
            {
                return <CombatTextPoolMenu>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <CombatTextPoolMenu>k__BackingField = value;
            }
        }

        public static Material DisabledUiMaterial
        {
            [CompilerGenerated]
            get
            {
                return <DisabledUiMaterial>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <DisabledUiMaterial>k__BackingField = value;
            }
        }

        public static IObjectPool<DungeonCell> DungeonCellPool
        {
            [CompilerGenerated]
            get
            {
                return <DungeonCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <DungeonCellPool>k__BackingField = value;
            }
        }

        public static IObjectPool<DungeonDropView> DungeonDropViewPool
        {
            [CompilerGenerated]
            get
            {
                return <DungeonDropViewPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <DungeonDropViewPool>k__BackingField = value;
            }
        }

        public static IObjectPool<DungeonEncounterCell> DungeonEncounterCellPool
        {
            [CompilerGenerated]
            get
            {
                return <DungeonEncounterCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <DungeonEncounterCellPool>k__BackingField = value;
            }
        }

        public static PlayerView.DungeonHud DungeonHud
        {
            [CompilerGenerated]
            get
            {
                return <DungeonHud>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <DungeonHud>k__BackingField = value;
            }
        }

        public static PlayerView.EffectSystem EffectSystem
        {
            [CompilerGenerated]
            get
            {
                return <EffectSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <EffectSystem>k__BackingField = value;
            }
        }

        public static IEventBus EventBus
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

        public static IObjectPool<HeroCell> HeroCellPool
        {
            [CompilerGenerated]
            get
            {
                return <HeroCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <HeroCellPool>k__BackingField = value;
            }
        }

        public static IObjectPool<HpIndicator> HpIndicatorPool
        {
            [CompilerGenerated]
            get
            {
                return <HpIndicatorPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <HpIndicatorPool>k__BackingField = value;
            }
        }

        public static PlayerView.InputSystem InputSystem
        {
            [CompilerGenerated]
            get
            {
                return <InputSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <InputSystem>k__BackingField = value;
            }
        }

        public static IObjectPool<ItemCard> ItemCardPool
        {
            [CompilerGenerated]
            get
            {
                return <ItemCardPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <ItemCardPool>k__BackingField = value;
            }
        }

        public static IObjectPool<ItemCell> ItemCellPool
        {
            [CompilerGenerated]
            get
            {
                return <ItemCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <ItemCellPool>k__BackingField = value;
            }
        }

        public static IObjectPool<ItemListCell> ItemListCellPool
        {
            [CompilerGenerated]
            get
            {
                return <ItemListCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <ItemListCellPool>k__BackingField = value;
            }
        }

        public static IObjectPool<LeaderboardCell> LeaderboardCellPool
        {
            [CompilerGenerated]
            get
            {
                return <LeaderboardCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <LeaderboardCellPool>k__BackingField = value;
            }
        }

        public static IObjectPool<LeaguePlayerCell> LeaguePlayerCellPool
        {
            [CompilerGenerated]
            get
            {
                return <LeaguePlayerCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <LeaguePlayerCellPool>k__BackingField = value;
            }
        }

        public static IObjectPool<LeagueTitleCell> LeagueTitleCellPool
        {
            [CompilerGenerated]
            get
            {
                return <LeagueTitleCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <LeagueTitleCellPool>k__BackingField = value;
            }
        }

        public static IObjectPool<MapCell> MapCellPool
        {
            [CompilerGenerated]
            get
            {
                return <MapCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <MapCellPool>k__BackingField = value;
            }
        }

        public static PlayerView.MarketingBuildController MarketingBuildController
        {
            [CompilerGenerated]
            get
            {
                return <MarketingBuildController>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <MarketingBuildController>k__BackingField = value;
            }
        }

        public static PlayerView.MaterialStorage MaterialStorage
        {
            [CompilerGenerated]
            get
            {
                return <MaterialStorage>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <MaterialStorage>k__BackingField = value;
            }
        }

        public static PlayerView.MenuContentResources MenuContentResources
        {
            [CompilerGenerated]
            get
            {
                return <MenuContentResources>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <MenuContentResources>k__BackingField = value;
            }
        }

        public static PlayerView.MenuHudBottom MenuHudBottom
        {
            [CompilerGenerated]
            get
            {
                return <MenuHudBottom>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <MenuHudBottom>k__BackingField = value;
            }
        }

        public static PlayerView.MenuHudTop MenuHudTop
        {
            [CompilerGenerated]
            get
            {
                return <MenuHudTop>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <MenuHudTop>k__BackingField = value;
            }
        }

        public static PlayerView.MenuSystem MenuSystem
        {
            [CompilerGenerated]
            get
            {
                return <MenuSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <MenuSystem>k__BackingField = value;
            }
        }

        public static IObjectPool<MenuTreasureChest> MenuTreasureChestPool
        {
            [CompilerGenerated]
            get
            {
                return <MenuTreasureChestPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <MenuTreasureChestPool>k__BackingField = value;
            }
        }

        public static IObjectPool<MilestoneCell> MilestoneCellPool
        {
            [CompilerGenerated]
            get
            {
                return <MilestoneCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <MilestoneCellPool>k__BackingField = value;
            }
        }

        public static IObjectPool<MissionCell> MissionCellPool
        {
            [CompilerGenerated]
            get
            {
                return <MissionCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <MissionCellPool>k__BackingField = value;
            }
        }

        public static Material MonochromeUiMaterial
        {
            [CompilerGenerated]
            get
            {
                return <MonochromeUiMaterial>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <MonochromeUiMaterial>k__BackingField = value;
            }
        }

        public static PlayerView.NotificationSystem NotificationSystem
        {
            [CompilerGenerated]
            get
            {
                return <NotificationSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <NotificationSystem>k__BackingField = value;
            }
        }

        public static ITypedObjectPool<PoolableAudioSource, AudioSourceType> PersistentAudioSourcePool
        {
            [CompilerGenerated]
            get
            {
                return <PersistentAudioSourcePool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <PersistentAudioSourcePool>k__BackingField = value;
            }
        }

        public static ITypedObjectPool<CharacterView, CharacterPrefab> PersistentCharacterViewPool
        {
            [CompilerGenerated]
            get
            {
                return <PersistentCharacterViewPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <PersistentCharacterViewPool>k__BackingField = value;
            }
        }

        public static ITypedObjectPool<PoolableParticleSystem, EffectType> PersistentParticleEffectPool
        {
            [CompilerGenerated]
            get
            {
                return <PersistentParticleEffectPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <PersistentParticleEffectPool>k__BackingField = value;
            }
        }

        public static IObjectPool<PetCell> PetCellPool
        {
            [CompilerGenerated]
            get
            {
                return <PetCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <PetCellPool>k__BackingField = value;
            }
        }

        public static PlayerView.PlayerViewContext PlayerViewContext
        {
            [CompilerGenerated]
            get
            {
                return <PlayerViewContext>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <PlayerViewContext>k__BackingField = value;
            }
        }

        public static IObjectPool<PromotionCard> PromotionCardAugmentationPool
        {
            [CompilerGenerated]
            get
            {
                return <PromotionCardAugmentationPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <PromotionCardAugmentationPool>k__BackingField = value;
            }
        }

        public static IObjectPool<ResourceGainImage> ResourceGainImagePool
        {
            [CompilerGenerated]
            get
            {
                return <ResourceGainImagePool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <ResourceGainImagePool>k__BackingField = value;
            }
        }

        public static ITypedObjectPool<RewardGalleryCell, RewardGalleryCellType> RewardGalleryCellPool
        {
            [CompilerGenerated]
            get
            {
                return <RewardGalleryCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <RewardGalleryCellPool>k__BackingField = value;
            }
        }

        public static PlayerView.RoomView RoomView
        {
            [CompilerGenerated]
            get
            {
                return <RoomView>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <RoomView>k__BackingField = value;
            }
        }

        public static IObjectPool<RunestoneCell> RunestoneCellPool
        {
            [CompilerGenerated]
            get
            {
                return <RunestoneCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <RunestoneCellPool>k__BackingField = value;
            }
        }

        public static PlayerView.ScreenTransitionEffect ScreenTransitionEffect
        {
            [CompilerGenerated]
            get
            {
                return <ScreenTransitionEffect>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <ScreenTransitionEffect>k__BackingField = value;
            }
        }

        public static IObjectPool<ShopCell> ShopCellPool
        {
            [CompilerGenerated]
            get
            {
                return <ShopCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <ShopCellPool>k__BackingField = value;
            }
        }

        public static IObjectPool<SkillCell> SkillCellPool
        {
            [CompilerGenerated]
            get
            {
                return <SkillCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <SkillCellPool>k__BackingField = value;
            }
        }

        public static IObjectPool<SkillDualCell> SkillDualCellPool
        {
            [CompilerGenerated]
            get
            {
                return <SkillDualCellPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <SkillDualCellPool>k__BackingField = value;
            }
        }

        public static IObjectPool<SkillHudButton> SkillHudButtonPool
        {
            [CompilerGenerated]
            get
            {
                return <SkillHudButtonPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <SkillHudButtonPool>k__BackingField = value;
            }
        }

        public static IObjectPool<SkillToggle> SkillTogglePool
        {
            [CompilerGenerated]
            get
            {
                return <SkillTogglePool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <SkillTogglePool>k__BackingField = value;
            }
        }

        public static SlidingPanelController SlidingAdventurePanelController
        {
            [CompilerGenerated]
            get
            {
                return <SlidingAdventurePanelController>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <SlidingAdventurePanelController>k__BackingField = value;
            }
        }

        public static SlidingPanelController SlidingTaskPanelController
        {
            [CompilerGenerated]
            get
            {
                return <SlidingTaskPanelController>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <SlidingTaskPanelController>k__BackingField = value;
            }
        }

        public static PlayerView.SpriteResources SpriteResources
        {
            [CompilerGenerated]
            get
            {
                return <SpriteResources>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <SpriteResources>k__BackingField = value;
            }
        }

        public static IObjectPool<StatusIndicator> StatusIndicatorPool
        {
            [CompilerGenerated]
            get
            {
                return <StatusIndicatorPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <StatusIndicatorPool>k__BackingField = value;
            }
        }

        public static PlayerView.StatusIndicatorSystem StatusIndicatorSystem
        {
            [CompilerGenerated]
            get
            {
                return <StatusIndicatorSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <StatusIndicatorSystem>k__BackingField = value;
            }
        }

        public static IObjectPool<TaskPanelItem> TaskPanelItemPool
        {
            [CompilerGenerated]
            get
            {
                return <TaskPanelItemPool>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <TaskPanelItemPool>k__BackingField = value;
            }
        }

        public static PlayerView.TransitionSystem TransitionSystem
        {
            [CompilerGenerated]
            get
            {
                return <TransitionSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <TransitionSystem>k__BackingField = value;
            }
        }

        public static RectTransform TutorialCanvas
        {
            [CompilerGenerated]
            get
            {
                return <TutorialCanvas>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <TutorialCanvas>k__BackingField = value;
            }
        }

        public static ITutorialSystem TutorialSystem
        {
            [CompilerGenerated]
            get
            {
                return <TutorialSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <TutorialSystem>k__BackingField = value;
            }
        }
    }
}

