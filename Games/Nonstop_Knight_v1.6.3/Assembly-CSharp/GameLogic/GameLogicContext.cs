namespace GameLogic
{
    using App;
    using PlayerView;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GameLogicContext : Context, ICoroutineExecutor
    {
        public DungeonResources m_dungeonResourcesBeforeWinterTheme;
        public DungeonResources m_dungeonResourcesDefault;
        public const string ROOM_ROOT_TM = "Room";

        Coroutine ICoroutineExecutor.StartCoroutine(IEnumerator routine)
        {
            return base.StartCoroutine(routine);
        }

        void ICoroutineExecutor.StopAllCoroutines()
        {
            base.StopAllCoroutines();
        }

        [DebuggerHidden]
        protected override IEnumerator mapBindings(bool allocatePersistentObjectPools)
        {
            <mapBindings>c__Iterator46 iterator = new <mapBindings>c__Iterator46();
            iterator.allocatePersistentObjectPools = allocatePersistentObjectPools;
            iterator.<$>allocatePersistentObjectPools = allocatePersistentObjectPools;
            iterator.<>f__this = this;
            return iterator;
        }

        protected void OnApplicationFocus(bool focused)
        {
            if ((Application.isEditor && ConfigApp.CHEAT_EDITOR_APP_FOCUS_SIMULATES_PAUSING) && !focused)
            {
                this.OnApplicationQuit();
            }
        }

        protected void OnApplicationPause(bool paused)
        {
            if (App.Binder.AppContext.systemsShouldReactToApplicationPause() && paused)
            {
                this.OnApplicationQuit();
            }
        }

        protected void OnApplicationQuit()
        {
            if ((GameLogic.Binder.GameState != null) && (GameLogic.Binder.GameState.Player != null))
            {
                CmdSavePlayerDataToPersistentStorage.ExecuteStatic(GameLogic.Binder.GameState.Player);
            }
        }

        protected override void onCleanup()
        {
            GameLogic.Binder.GameLogicContext = null;
            GameLogic.Binder.CoroutineExecutor = null;
            GameLogic.Binder.CommandProcessor = null;
            GameLogic.Binder.GameState = null;
            GameLogic.Binder.LevelUpRules = null;
            GameLogic.Binder.ItemResources = null;
            GameLogic.Binder.EpisodeResources = null;
            GameLogic.Binder.CharacterResources = null;
            GameLogic.Binder.DungeonMoodResources = null;
            GameLogic.Binder.DungeonBlockResources = null;
            GameLogic.Binder.DungeonResources = null;
            GameLogic.Binder.UpgradeRules = null;
            GameLogic.Binder.DungeonEventRules = null;
            base.destroyPersistentGameObject<GameplayStateMachine>((GameplayStateMachine) GameLogic.Binder.GameplayStateMachine);
            GameLogic.Binder.GameplayStateMachine = null;
            base.destroyPersistentGameObject<TimeSystem>((TimeSystem) GameLogic.Binder.TimeSystem);
            GameLogic.Binder.TimeSystem = null;
            base.destroyPersistentGameObject<DeathSystem>((DeathSystem) GameLogic.Binder.DeathSystem);
            GameLogic.Binder.DeathSystem = null;
            base.destroyPersistentGameObject<SkillSystem>((SkillSystem) GameLogic.Binder.SkillSystem);
            GameLogic.Binder.SkillSystem = null;
            base.destroyPersistentGameObject<BoostSystem>((BoostSystem) GameLogic.Binder.BoostSystem);
            GameLogic.Binder.BoostSystem = null;
            base.destroyPersistentGameObject<LootSystem>((LootSystem) GameLogic.Binder.LootSystem);
            GameLogic.Binder.LootSystem = null;
            base.destroyPersistentGameObject<MissionSystem>((MissionSystem) GameLogic.Binder.MissionSystem);
            GameLogic.Binder.MissionSystem = null;
            base.destroyPersistentGameObject<CharacterSpawningSystem>((CharacterSpawningSystem) GameLogic.Binder.CharacterSpawningSystem);
            GameLogic.Binder.CharacterSpawningSystem = null;
            base.destroyPersistentGameObject<HealthSystem>((HealthSystem) GameLogic.Binder.HealthSystem);
            GameLogic.Binder.HealthSystem = null;
            base.destroyPersistentGameObject<PassiveTreasureGainSystem>((PassiveTreasureGainSystem) GameLogic.Binder.PassiveTreasureGainSystem);
            GameLogic.Binder.PassiveTreasureGainSystem = null;
            base.destroyPersistentGameObject<HeroStatRecordingSystem>((HeroStatRecordingSystem) GameLogic.Binder.HeroStatRecordingSystem);
            GameLogic.Binder.HeroStatRecordingSystem = null;
            base.destroyPersistentGameObject<DecoSpawningSystem>((DecoSpawningSystem) GameLogic.Binder.DecoSpawningSystem);
            GameLogic.Binder.DecoSpawningSystem = null;
            base.destroyPersistentGameObject<VendorSystem>((VendorSystem) GameLogic.Binder.VendorSystem);
            GameLogic.Binder.VendorSystem = null;
            base.destroyPersistentGameObject<BuffSystem>((BuffSystem) GameLogic.Binder.BuffSystem);
            GameLogic.Binder.BuffSystem = null;
            base.destroyPersistentGameObject<LeaderboardSystem>((LeaderboardSystem) GameLogic.Binder.LeaderboardSystem);
            GameLogic.Binder.LeaderboardSystem = null;
            base.destroyPersistentGameObject<FrenzySystem>((FrenzySystem) GameLogic.Binder.FrenzySystem);
            GameLogic.Binder.FrenzySystem = null;
            base.destroyPersistentGameObject<BlinkSystem>((BlinkSystem) GameLogic.Binder.BlinkSystem);
            GameLogic.Binder.BlinkSystem = null;
            base.destroyPersistentGameObject<PerkSystem>((PerkSystem) GameLogic.Binder.PerkSystem);
            GameLogic.Binder.PerkSystem = null;
            base.destroyPersistentGameObject<PathfindingSystem>((PathfindingSystem) GameLogic.Binder.PathfindingSystem);
            GameLogic.Binder.PathfindingSystem = null;
            base.destroyPersistentGameObject<DungeonBoostSpawningSystem>((DungeonBoostSpawningSystem) GameLogic.Binder.DungeonBoostSpawningSystem);
            GameLogic.Binder.DungeonBoostSpawningSystem = null;
        }

        private void onContentReady()
        {
            this.refreshDungeonResources();
        }

        protected override void onPostInitialize()
        {
            Service.Binder.EventBus.OnContentReady -= new Service.Events.ContentReady(this.onContentReady);
            Service.Binder.EventBus.OnContentReady += new Service.Events.ContentReady(this.onContentReady);
        }

        private void refreshDungeonResources()
        {
            GameLogic.Binder.DungeonResources = App.Binder.ConfigMeta.WINTER_THEME_ENABLED ? this.m_dungeonResourcesDefault : this.m_dungeonResourcesBeforeWinterTheme;
        }

        [CompilerGenerated]
        private sealed class <mapBindings>c__Iterator46 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>allocatePersistentObjectPools;
            internal GameLogicContext <>f__this;
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
                        GameLogic.Binder.GameLogicContext = this.<>f__this;
                        GameLogic.Binder.CoroutineExecutor = this.<>f__this;
                        GameLogic.Binder.CommandProcessor = new CommandProcessor(this.<>f__this);
                        GameLogic.Binder.EventBus = this.<>f__this.createPersistentGameObject<GameLogic.EventBus>(this.<>f__this.Tm);
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_04BC;

                    case 1:
                        if (!this.allocatePersistentObjectPools)
                        {
                            break;
                        }
                        GameLogic.Binder.CharacterPool = new ObjectPool<CharacterInstance>(new CharacterProvider(Layers.CHARACTERS), 0x20, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) GameLogic.Binder.CharacterPool);
                        this.$current = null;
                        this.$PC = 2;
                        goto Label_04BC;

                    case 2:
                        GameLogic.Binder.ProjectilePool = new ObjectPool<Projectile>(new ProjectileProvider(Layers.PROJECTILES), 0x40, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) GameLogic.Binder.ProjectilePool);
                        this.$current = null;
                        this.$PC = 3;
                        goto Label_04BC;

                    case 3:
                        GameLogic.Binder.AreaEffectPool = new TypedObjectPool<AreaEffect, AreaEffectType>(new AreaEffectProvider(Layers.AREA_EFFECTS), 1, ConfigObjectPools.PERSISTENT_AREA_EFFECTS, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) GameLogic.Binder.AreaEffectPool);
                        this.$current = null;
                        this.$PC = 4;
                        goto Label_04BC;

                    case 4:
                        GameLogic.Binder.DungeonBoostPool = new ObjectPool<DungeonBoost>(new DungeonBoostProvider(Layers.DUNGEON_BOOSTS), 0x20, ObjectPoolExpansionMethod.DOUBLE, true);
                        this.<>f__this.m_persistentObjectPools.Add((IGenericObjectPool) GameLogic.Binder.DungeonBoostPool);
                        this.$current = null;
                        this.$PC = 5;
                        goto Label_04BC;

                    case 5:
                        break;

                    case 6:
                        GameLogic.Binder.CharacterResources = new CharacterResources();
                        this.$current = null;
                        this.$PC = 7;
                        goto Label_04BC;

                    case 7:
                        GameLogic.Binder.DungeonMoodResources = new DungeonMoodResources();
                        GameLogic.Binder.DungeonBlockResources = new DungeonBlockResources();
                        this.<>f__this.m_dungeonResourcesDefault = new DungeonResources("Rules/Dungeon-Instances");
                        this.<>f__this.m_dungeonResourcesBeforeWinterTheme = new DungeonResources("Rules/Dungeon-Instances-v6");
                        this.$current = null;
                        this.$PC = 8;
                        goto Label_04BC;

                    case 8:
                        GameLogic.Binder.GameplayStateMachine = this.<>f__this.createPersistentGameObject<GameplayStateMachine>(this.<>f__this.Tm);
                        GameLogic.Binder.TimeSystem = this.<>f__this.createPersistentGameObject<TimeSystem>(this.<>f__this.Tm);
                        GameLogic.Binder.DeathSystem = this.<>f__this.createPersistentGameObject<DeathSystem>(this.<>f__this.Tm);
                        GameLogic.Binder.SkillSystem = this.<>f__this.createPersistentGameObject<SkillSystem>(this.<>f__this.Tm);
                        GameLogic.Binder.BoostSystem = this.<>f__this.createPersistentGameObject<BoostSystem>(this.<>f__this.Tm);
                        GameLogic.Binder.LootSystem = this.<>f__this.createPersistentGameObject<LootSystem>(this.<>f__this.Tm);
                        GameLogic.Binder.MissionSystem = this.<>f__this.createPersistentGameObject<MissionSystem>(this.<>f__this.Tm);
                        this.$current = null;
                        this.$PC = 9;
                        goto Label_04BC;

                    case 9:
                        GameLogic.Binder.CharacterSpawningSystem = this.<>f__this.createPersistentGameObject<CharacterSpawningSystem>(this.<>f__this.Tm);
                        GameLogic.Binder.HealthSystem = this.<>f__this.createPersistentGameObject<HealthSystem>(this.<>f__this.Tm);
                        GameLogic.Binder.PassiveTreasureGainSystem = this.<>f__this.createPersistentGameObject<PassiveTreasureGainSystem>(this.<>f__this.Tm);
                        GameLogic.Binder.HeroStatRecordingSystem = this.<>f__this.createPersistentGameObject<HeroStatRecordingSystem>(this.<>f__this.Tm);
                        GameLogic.Binder.DecoSpawningSystem = this.<>f__this.createPersistentGameObject<DecoSpawningSystem>(this.<>f__this.Tm);
                        this.$current = null;
                        this.$PC = 10;
                        goto Label_04BC;

                    case 10:
                        GameLogic.Binder.VendorSystem = this.<>f__this.createPersistentGameObject<VendorSystem>(this.<>f__this.Tm);
                        GameLogic.Binder.BuffSystem = this.<>f__this.createPersistentGameObject<BuffSystem>(this.<>f__this.Tm);
                        GameLogic.Binder.LeaderboardSystem = this.<>f__this.createPersistentGameObject<LeaderboardSystem>(this.<>f__this.Tm);
                        GameLogic.Binder.FrenzySystem = this.<>f__this.createPersistentGameObject<FrenzySystem>(this.<>f__this.Tm);
                        GameLogic.Binder.BlinkSystem = this.<>f__this.createPersistentGameObject<BlinkSystem>(this.<>f__this.Tm);
                        GameLogic.Binder.PerkSystem = this.<>f__this.createPersistentGameObject<PerkSystem>(this.<>f__this.Tm);
                        GameLogic.Binder.PlayerAugmentationResources = new PlayerAugmentationResources();
                        GameLogic.Binder.PathfindingSystem = this.<>f__this.createPersistentGameObject<PathfindingSystem>(this.<>f__this.Tm);
                        GameLogic.Binder.DungeonBoostSpawningSystem = this.<>f__this.createPersistentGameObject<DungeonBoostSpawningSystem>(this.<>f__this.Tm);
                        this.$PC = -1;
                        goto Label_04BA;

                    default:
                        goto Label_04BA;
                }
                GameLogic.Binder.DungeonDecoResources = new DungeonDecoResources();
                GameLogic.Binder.GameState = new GameState();
                GameLogic.Binder.ItemResources = new ItemResources();
                GameLogic.Binder.ItemResources.initialize();
                this.$current = null;
                this.$PC = 6;
                goto Label_04BC;
            Label_04BA:
                return false;
            Label_04BC:
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

