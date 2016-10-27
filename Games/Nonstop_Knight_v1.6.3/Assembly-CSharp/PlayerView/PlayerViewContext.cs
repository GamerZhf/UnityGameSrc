namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class PlayerViewContext : Context
    {
        [DebuggerHidden]
        protected override IEnumerator mapBindings(bool allocatePersistentObjectPools)
        {
            <mapBindings>c__Iterator1A7 iteratora = new <mapBindings>c__Iterator1A7();
            iteratora.allocatePersistentObjectPools = allocatePersistentObjectPools;
            iteratora.<$>allocatePersistentObjectPools = allocatePersistentObjectPools;
            iteratora.<>f__this = this;
            return iteratora;
        }

        protected override void onCleanup()
        {
            PlayerView.Binder.PlayerViewContext = null;
            PlayerView.Binder.MarketingBuildController = null;
            PlayerView.Binder.SpriteResources = null;
            PlayerView.Binder.MenuContentResources = null;
            PlayerView.Binder.RoomView = null;
            PlayerView.Binder.InputSystem = null;
            PlayerView.Binder.AudioSystem = null;
            PlayerView.Binder.EffectSystem = null;
            PlayerView.Binder.MenuSystem = null;
            PlayerView.Binder.DungeonHud = null;
            PlayerView.Binder.ScreenTransitionEffect = null;
            PlayerView.Binder.TutorialCanvas = null;
            PlayerView.Binder.TutorialSystem = null;
            PlayerView.Binder.DisabledUiMaterial = null;
            PlayerView.Binder.TransitionSystem = null;
            PlayerView.Binder.AnnouncementSystem = null;
            PlayerView.Binder.MaterialStorage = null;
            PlayerView.Binder.AccessoryStorage = null;
            PlayerView.Binder.NotificationSystem = null;
            PlayerView.Binder.SlidingTaskPanelController = null;
            PlayerView.Binder.SlidingAdventurePanelController = null;
            PlayerView.Binder.AdventureMilestones = null;
        }

        protected void OnDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            eventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded -= new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
        }

        protected void OnEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            eventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded += new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
        }

        private void onGameplayEnded(ActiveDungeon activeDungeon)
        {
            if (PlayerView.Binder.MenuHudTop != null)
            {
                PlayerView.Binder.MenuHudTop.gameObject.SetActive(true);
            }
            if (PlayerView.Binder.MenuHudBottom != null)
            {
                PlayerView.Binder.MenuHudBottom.gameObject.SetActive(true);
            }
            PlayerView.Binder.DungeonHud.onGameplayEnded(activeDungeon);
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            if (PlayerView.Binder.MenuHudTop != null)
            {
                PlayerView.Binder.MenuHudTop.gameObject.SetActive(false);
            }
            if (PlayerView.Binder.MenuHudBottom != null)
            {
                PlayerView.Binder.MenuHudBottom.gameObject.SetActive(false);
            }
            PlayerView.Binder.DungeonHud.gameObject.SetActive(true);
            PlayerView.Binder.DungeonHud.onGameplayStarted(activeDungeon);
        }

        private void onGameStateInitialized()
        {
            RenderSettings.fog = false;
            RenderSettings.ambientLight = ConfigUi.MENU_AMBIENT_COLOR;
            RenderSettings.ambientIntensity = 1f;
            LocaSystem locaSystem = App.Binder.LocaSystem;
            if (!locaSystem.Initialized)
            {
                locaSystem.Initialize(SystemLanguage.Unknown);
            }
            if (locaSystem.IsRightToLeft(locaSystem.selectedLanguage))
            {
                Service.Binder.NSKRTLManager = new NSKRTLManager();
                Service.Binder.NSKRTLManager.Initialize("PlayerView.PlayerViewContext");
            }
        }

        protected override void onInitialize()
        {
        }

        [CompilerGenerated]
        private sealed class <mapBindings>c__Iterator1A7 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>allocatePersistentObjectPools;
            internal PlayerViewContext <>f__this;
            internal GameObject <go>__0;
            internal GameObject <transObj>__1;
            internal GameObject <tutorialCanvasObj>__2;
            internal bool allocatePersistentObjectPools;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        PlayerView.Binder.PlayerViewContext = this.<>f__this;
                        PlayerView.Binder.EventBus = new PlayerView.EventBus();
                        if (ConfigApp.CHEAT_MARKETING_MODE_ENABLED)
                        {
                            PlayerView.Binder.MarketingBuildController = this.<>f__this.createPersistentGameObject<MarketingBuildController>(this.<>f__this.Tm);
                        }
                        PlayerView.Binder.SpriteResources = new SpriteResources();
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_0BF5;

                    case 1:
                        PlayerView.Binder.RoomView = RoomView.Instantiate(this.<>f__this.Tm);
                        this.$current = null;
                        this.$PC = 2;
                        goto Label_0BF5;

                    case 2:
                        PlayerView.Binder.InputSystem = this.<>f__this.createPersistentGameObject<InputSystem>(ResourceUtil.Instantiate<GameObject>("Prefabs/InputSystem"), this.<>f__this.Tm);
                        this.$current = null;
                        this.$PC = 3;
                        goto Label_0BF5;

                    case 3:
                        PlayerView.Binder.MaterialStorage = new MaterialStorage();
                        this.$current = null;
                        this.$PC = 4;
                        goto Label_0BF5;

                    case 4:
                        if (!this.allocatePersistentObjectPools)
                        {
                            break;
                        }
                        PlayerView.Binder.PersistentCharacterViewPool = new TypedObjectPool<CharacterView, CharacterPrefab>(new CharacterViewProvider(Layers.CHARACTER_VIEWS, App.Binder.PersistentObjectRootTm), 1, ConfigObjectPools.PERSISTENT_CHARACTER_PREFABS, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.PersistentCharacterViewPool);
                        this.$current = null;
                        this.$PC = 5;
                        goto Label_0BF5;

                    case 5:
                        PlayerView.Binder.CombatTextPoolIngame = new ObjectPool<CombatTextIngame>(new CombatTextProviderIngame("Prefabs/Dungeon/CombatTextIngame", Layers.DEFAULT), 0x18, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.CombatTextPoolIngame);
                        this.$current = null;
                        this.$PC = 6;
                        goto Label_0BF5;

                    case 6:
                        PlayerView.Binder.CombatTextPoolMenu = new ObjectPool<CombatTextMenu>(new CombatTextProviderMenu("Prefabs/Hud/CombatTextMenu", Layers.DEFAULT), 0x18, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.CombatTextPoolMenu);
                        this.$current = null;
                        this.$PC = 7;
                        goto Label_0BF5;

                    case 7:
                        PlayerView.Binder.HpIndicatorPool = new ObjectPool<HpIndicator>(new HpIndicatorProvider("Prefabs/Hud/HpIndicator", Layers.DEFAULT), 0x19, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.HpIndicatorPool);
                        this.$current = null;
                        this.$PC = 8;
                        goto Label_0BF5;

                    case 8:
                        PlayerView.Binder.SkillHudButtonPool = new ObjectPool<SkillHudButton>(new SkillHudButtonProvider("Prefabs/Hud/SkillHudButton", Layers.DEFAULT), 4, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.SkillHudButtonPool);
                        PlayerView.Binder.ItemCellPool = new ObjectPool<ItemCell>(new ItemCellProvider("Prefabs/Menu/ItemCell", Layers.DEFAULT), 0x20, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.ItemCellPool);
                        this.$current = null;
                        this.$PC = 9;
                        goto Label_0BF5;

                    case 9:
                        PlayerView.Binder.PersistentParticleEffectPool = new TypedObjectPool<PoolableParticleSystem, EffectType>(new PoolableParticleSystemProvider(App.Binder.PersistentObjectRootTm), 1, ConfigObjectPools.PERSISTENT_PARTICLE_EFFECTS, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.PersistentParticleEffectPool);
                        this.$current = null;
                        this.$PC = 10;
                        goto Label_0BF5;

                    case 10:
                        PlayerView.Binder.ResourceGainImagePool = new ObjectPool<ResourceGainImage>(new ResourceGainImageProvider("Prefabs/Menu/ResourceGainImage", Layers.DEFAULT), 0x80, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.ResourceGainImagePool);
                        this.$current = null;
                        this.$PC = 11;
                        goto Label_0BF5;

                    case 11:
                        PlayerView.Binder.DungeonDropViewPool = new ObjectPool<DungeonDropView>(new DungeonDropViewProvider("Prefabs/Dungeon/DungeonDropView", Layers.CLICK_COLLIDERS), 0x10, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.DungeonDropViewPool);
                        this.$current = null;
                        this.$PC = 12;
                        goto Label_0BF5;

                    case 12:
                        PlayerView.Binder.SkillDualCellPool = new ObjectPool<SkillDualCell>(new SkillDualCellProvider("Prefabs/Menu/SkillDualCell", Layers.UI), ConfigSkills.SkillGroupCount, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.SkillDualCellPool);
                        this.$current = null;
                        this.$PC = 13;
                        goto Label_0BF5;

                    case 13:
                        PlayerView.Binder.AchievementCellPool = new ObjectPool<AchievementCell>(new AchievementCellProvider("Prefabs/Menu/AchievementCell", Layers.UI), ConfigAchievements.SHARED_DATA.Count, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.AchievementCellPool);
                        this.$current = null;
                        this.$PC = 14;
                        goto Label_0BF5;

                    case 14:
                        PlayerView.Binder.RunestoneCellPool = new ObjectPool<RunestoneCell>(new RunestoneCellProvider("Prefabs/Menu/RunestoneCell", Layers.UI), ConfigRunestones.RUNESTONES.Length, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.RunestoneCellPool);
                        this.$current = null;
                        this.$PC = 15;
                        goto Label_0BF5;

                    case 15:
                        PlayerView.Binder.PersistentAudioSourcePool = new TypedObjectPool<PoolableAudioSource, AudioSourceType>(new PoolableAudioSourceProvider(Layers.DEFAULT, App.Binder.PersistentObjectRootTm), 1, ConfigObjectPools.PERSISTENT_AUDIO_SOURCES, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.PersistentAudioSourcePool);
                        this.$current = null;
                        this.$PC = 0x10;
                        goto Label_0BF5;

                    case 0x10:
                        PlayerView.Binder.BuffHudTimerPool = new ObjectPool<BuffHudTimer>(new BuffHudTimerProvider("Prefabs/Menu/BuffHudTimer", Layers.UI), 8, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.BuffHudTimerPool);
                        PlayerView.Binder.LeaderboardCellPool = new ObjectPool<LeaderboardCell>(new LeaderboardCellProvider("Prefabs/Menu/LeaderboardCell", Layers.UI), ConfigLeaderboard.MAX_NUM_VISIBLE_LEADERBOARD_CELLS, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.LeaderboardCellPool);
                        this.$current = null;
                        this.$PC = 0x11;
                        goto Label_0BF5;

                    case 0x11:
                        PlayerView.Binder.RewardGalleryCellPool = new TypedObjectPool<RewardGalleryCell, RewardGalleryCellType>(new RewardGalleryCellProvider(App.Binder.PersistentObjectRootTm), 8, null, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.RewardGalleryCellPool);
                        PlayerView.Binder.MenuTreasureChestPool = new ObjectPool<MenuTreasureChest>(new MenuTreasureChestProvider("Prefabs/Menu/MenuTreasureChest", Layers.UI), 1, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.MenuTreasureChestPool);
                        PlayerView.Binder.CardRewardPool = new ObjectPool<Card>(new CardProvider("Prefabs/Menu/CardReward", Layers.UI), 4, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.CardRewardPool);
                        PlayerView.Binder.CardButtonPool = new ObjectPool<Card>(new CardProvider("Prefabs/Menu/CardButton", Layers.UI), 0x18, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.CardButtonPool);
                        PlayerView.Binder.PromotionCardAugmentationPool = new ObjectPool<PromotionCard>(new PromotionCardProvider("Prefabs/Menu/PromoCell", Layers.UI), 0x10, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.PromotionCardAugmentationPool);
                        PlayerView.Binder.ChestGalleryRowPool = new ObjectPool<ChestGalleryRow>(new ChestGalleryRowProvider("Prefabs/Menu/ChestGalleryRow", Layers.UI), ConfigMeta.BOSS_CHESTS.Count, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.ChestGalleryRowPool);
                        PlayerView.Binder.TaskPanelItemPool = new ObjectPool<TaskPanelItem>(new TaskPanelItemProvider("Prefabs/Hud/TaskPanelItem", Layers.UI), ConfigUi.TASK_PANEL_ITEM_BLUEPRINTS.Count, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.TaskPanelItemPool);
                        PlayerView.Binder.PetCellPool = new ObjectPool<PetCell>(new PetCellProvider("Prefabs/Menu/PetCell", Layers.UI), 0x10, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.PetCellPool);
                        PlayerView.Binder.BossHuntTickerCellPool = new ObjectPool<BossHuntTickerCell>(new BossHuntTickerCellProvider("Prefabs/Menu/BossHuntTickerCell", Layers.UI), 8, ObjectPoolExpansionMethod.SINGLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.BossHuntTickerCellPool);
                        PlayerView.Binder.MissionCellPool = new ObjectPool<MissionCell>(new MissionCellProvider("Prefabs/Menu/MissionCell", Layers.UI), 4, ObjectPoolExpansionMethod.SINGLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) PlayerView.Binder.MissionCellPool);
                        this.$current = null;
                        this.$PC = 0x12;
                        goto Label_0BF5;

                    case 0x12:
                        break;

                    case 0x13:
                        PlayerView.Binder.AccessoryStorage = this.<>f__this.createPersistentGameObject<AccessoryStorage>(this.<>f__this.Tm);
                        this.$current = null;
                        this.$PC = 20;
                        goto Label_0BF5;

                    case 20:
                        PlayerView.Binder.AudioSystem = this.<>f__this.createPersistentGameObject<AudioSystem>(this.<>f__this.Tm);
                        this.$current = null;
                        this.$PC = 0x15;
                        goto Label_0BF5;

                    case 0x15:
                        PlayerView.Binder.EffectSystem = this.<>f__this.createPersistentGameObject<EffectSystem>(this.<>f__this.Tm);
                        this.$current = null;
                        this.$PC = 0x16;
                        goto Label_0BF5;

                    case 0x16:
                        PlayerView.Binder.MenuSystem = this.<>f__this.createPersistentGameObject<MenuSystem>(this.<>f__this.Tm);
                        PlayerView.Binder.MenuSystem.transform.localPosition = new Vector3(0f, 200f, 0f);
                        this.$current = null;
                        this.$PC = 0x17;
                        goto Label_0BF5;

                    case 0x17:
                        this.<go>__0 = App.Binder.AssetBundleLoader.instantiatePrefab("Prefabs/Hud/DungeonHud");
                        this.<go>__0.transform.SetParent(this.<>f__this.Tm, false);
                        this.<go>__0.transform.position = new Vector3(1000f, 1000f, 0f);
                        PlayerView.Binder.DungeonHud = this.<go>__0.GetComponent<DungeonHud>();
                        PlayerView.Binder.DungeonHud.initialize(PlayerView.Binder.MenuSystem.MenuCamera);
                        PlayerView.Binder.DungeonHud.gameObject.SetActive(true);
                        this.$current = null;
                        this.$PC = 0x18;
                        goto Label_0BF5;

                    case 0x18:
                        this.<transObj>__1 = App.Binder.AssetBundleLoader.instantiatePrefab("Prefabs/UI/ScreenTransitionEffect");
                        this.<transObj>__1.transform.SetParent(this.<>f__this.Tm, false);
                        PlayerView.Binder.ScreenTransitionEffect = this.<transObj>__1.GetComponent<ScreenTransitionEffect>();
                        this.$current = null;
                        this.$PC = 0x19;
                        goto Label_0BF5;

                    case 0x19:
                        this.<tutorialCanvasObj>__2 = App.Binder.AssetBundleLoader.instantiatePrefab("Prefabs/UI/TutorialCanvas");
                        this.<tutorialCanvasObj>__2.transform.SetParent(this.<>f__this.Tm, false);
                        PlayerView.Binder.TutorialCanvas = this.<tutorialCanvasObj>__2.GetComponent<RectTransform>();
                        this.<tutorialCanvasObj>__2.SetActive(false);
                        this.$current = null;
                        this.$PC = 0x1a;
                        goto Label_0BF5;

                    case 0x1a:
                        PlayerView.Binder.TutorialSystem = this.<>f__this.createPersistentGameObject<TutorialSystem>(this.<>f__this.Tm);
                        this.$current = null;
                        this.$PC = 0x1b;
                        goto Label_0BF5;

                    case 0x1b:
                        PlayerView.Binder.DisabledUiMaterial = Resources.Load<Material>("Materials/ui_disabled");
                        PlayerView.Binder.MonochromeUiMaterial = Resources.Load<Material>("Materials/ui_monochrome_boosted");
                        PlayerView.Binder.TransitionSystem = this.<>f__this.createPersistentGameObject<TransitionSystem>(this.<>f__this.Tm);
                        this.$current = null;
                        this.$PC = 0x1c;
                        goto Label_0BF5;

                    case 0x1c:
                        PlayerView.Binder.AnnouncementSystem = this.<>f__this.createPersistentGameObject<AnnouncementSystem>(this.<>f__this.Tm);
                        this.$current = null;
                        this.$PC = 0x1d;
                        goto Label_0BF5;

                    case 0x1d:
                        PlayerView.Binder.NotificationSystem = this.<>f__this.createPersistentGameObject<NotificationSystem>(this.<>f__this.Tm);
                        PlayerView.Binder.SlidingTaskPanelController = this.<>f__this.createPersistentGameObject<SlidingPanelController>(this.<>f__this.Tm);
                        PlayerView.Binder.SlidingTaskPanelController.name = "SlidingTaskPanelController";
                        PlayerView.Binder.SlidingTaskPanelController.initialize((ISlidingPanel) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.SlidingTaskPanel), 2f, SlidingPanelController.PanDirection.Right);
                        PlayerView.Binder.SlidingAdventurePanelController = this.<>f__this.createPersistentGameObject<SlidingPanelController>(this.<>f__this.Tm);
                        PlayerView.Binder.SlidingAdventurePanelController.name = "SlidingAdventurePanelController";
                        PlayerView.Binder.SlidingAdventurePanelController.initialize((ISlidingPanel) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.SlidingAdventurePanel), -4f, SlidingPanelController.PanDirection.Left);
                        PlayerView.Binder.AdventureMilestones = new AdventureMilestones();
                        this.$PC = -1;
                        goto Label_0BF3;

                    default:
                        goto Label_0BF3;
                }
                this.<go>__0 = new GameObject("MenuContentResources");
                TransformExtensions.SetParentAndMaintainLocalTm(this.<go>__0.transform, this.<>f__this.Tm);
                PlayerView.Binder.MenuContentResources = this.<go>__0.AddComponent<MenuContentResources>();
                this.$current = null;
                this.$PC = 0x13;
                goto Label_0BF5;
            Label_0BF3:
                return false;
            Label_0BF5:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

