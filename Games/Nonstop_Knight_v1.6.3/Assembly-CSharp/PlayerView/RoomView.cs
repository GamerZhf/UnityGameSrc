namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class RoomView : MonoBehaviour
    {
        [CompilerGenerated]
        private bool <GameplayPrewarmingCompleted>k__BackingField;
        [CompilerGenerated]
        private GameLogic.Room <Room>k__BackingField;
        [CompilerGenerated]
        private PlayerView.RoomCamera <RoomCamera>k__BackingField;
        private Dictionary<CharacterInstance, CharacterView> m_characterToCharacterViewMapping = new Dictionary<CharacterInstance, CharacterView>();
        private List<CharacterView> m_characterViews = new List<CharacterView>();
        private Dictionary<DungeonBoost, DungeonBoostView> m_dungeonBoostToDungeonBoostViewMapping = new Dictionary<DungeonBoost, DungeonBoostView>();
        private List<DungeonDropView> m_dungeonItemDropViews = new List<DungeonDropView>();
        private ITypedObjectPool<CharacterView, CharacterPrefab> m_dynamicCharacterViewPool;
        private ITypedObjectPool<DungeonBoostView, DungeonBoostType> m_dynamicDungeonBoostViewPool;
        private Dictionary<GameObject, CharacterView> m_gameObjectToCharacterViewMapping = new Dictionary<GameObject, CharacterView>();
        private Dictionary<CharacterPrefab, bool> m_persistentCharacterPrefabQuickLookup = new Dictionary<CharacterPrefab, bool>(new CharacterPrefabBoxAvoidanceComparer());

        private void addCharacterViewToContainers(CharacterView characterView)
        {
            this.m_characterViews.Add(characterView);
            this.m_characterToCharacterViewMapping.Add(characterView.Character, characterView);
            this.m_gameObjectToCharacterViewMapping.Add(characterView.gameObject, characterView);
        }

        protected void Awake()
        {
            IEnumerator enumerator = Enum.GetValues(typeof(CharacterPrefab)).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    CharacterPrefab current = (CharacterPrefab) ((int) enumerator.Current);
                    if (ConfigObjectPools.PERSISTENT_CHARACTER_PREFABS.ContainsKey(current))
                    {
                        this.m_persistentCharacterPrefabQuickLookup.Add(current, true);
                    }
                    else
                    {
                        this.m_persistentCharacterPrefabQuickLookup.Add(current, false);
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            GameObject obj2 = ResourceUtil.Instantiate<GameObject>("Prefabs/RoomCamera");
            obj2.transform.SetParent(base.transform, false);
            obj2.transform.position = new Vector3(0f, 0f, 100f);
            this.RoomCamera = obj2.GetComponent<PlayerView.RoomCamera>();
        }

        private void cleanupDungeonItemDropView(int idx)
        {
            DungeonDropView view = this.m_dungeonItemDropViews[idx];
            PlayerView.Binder.DungeonDropViewPool.returnObject(view);
            this.m_dungeonItemDropViews.Remove(view);
        }

        private void cleanupDungeonItemDropViews()
        {
            for (int i = this.m_dungeonItemDropViews.Count - 1; i >= 0; i--)
            {
                this.cleanupDungeonItemDropView(i);
            }
        }

        private bool doSkipObjectPooling(CharacterInstance character)
        {
            return character.IsPet;
        }

        [DebuggerHidden]
        private IEnumerator gameplayStartRoutine(ActiveDungeon activeDungeon)
        {
            <gameplayStartRoutine>c__Iterator1A8 iteratora = new <gameplayStartRoutine>c__Iterator1A8();
            iteratora.activeDungeon = activeDungeon;
            iteratora.<$>activeDungeon = activeDungeon;
            iteratora.<>f__this = this;
            return iteratora;
        }

        public CharacterView getCharacterViewForCharacter(CharacterInstance character)
        {
            if ((character != null) && this.m_characterToCharacterViewMapping.ContainsKey(character))
            {
                return this.m_characterToCharacterViewMapping[character];
            }
            return null;
        }

        public CharacterView getCharacterViewForGameObject(GameObject go)
        {
            if ((go != null) && this.m_gameObjectToCharacterViewMapping.ContainsKey(go))
            {
                return this.m_gameObjectToCharacterViewMapping[go];
            }
            return null;
        }

        public static RoomView Instantiate(Transform parentTm)
        {
            GameObject obj2 = new GameObject("RoomView");
            TransformExtensions.SetParentAndMaintainLocalTm(obj2.transform, parentTm);
            obj2.transform.localPosition = Vector3.zero;
            return obj2.AddComponent<RoomView>();
        }

        public bool isDungeonBoostOnscreen(DungeonBoost dungeonBoost)
        {
            return this.m_dungeonBoostToDungeonBoostViewMapping[dungeonBoost].isOnscreen();
        }

        private bool isPersistentCharacterPrefab(CharacterPrefab prefab)
        {
            return this.m_persistentCharacterPrefabQuickLookup[prefab];
        }

        private void onCharacterKilled(CharacterInstance character, CharacterInstance killer, bool critted, SkillType fromSkill)
        {
            CharacterView view = this.m_characterToCharacterViewMapping[character];
            if (!ConfigGameplay.RAGDOLL_DEATHS_ENABLED)
            {
                if (!character.IsPersistent)
                {
                    view.setVisibility(false);
                }
            }
            else
            {
                Vector3 vector3 = character.PhysicsBody.Transform.position - killer.PhysicsBody.Transform.position;
                Vector3 normalized = vector3.normalized;
                float num = UnityEngine.Random.Range((float) 15f, (float) 25f);
                if (critted)
                {
                    num *= 2f;
                }
                Vector3 force = (Vector3) (normalized * num);
                for (int i = 0; i < view.RagdollRigidbodies.Count; i++)
                {
                    Rigidbody rigidbody = view.RagdollRigidbodies[i];
                    rigidbody.detectCollisions = true;
                    rigidbody.isKinematic = false;
                    rigidbody.useGravity = true;
                    rigidbody.AddForce(force, ForceMode.Impulse);
                }
                view.Animator.enabled = false;
                if (view.ShadowRenderer != null)
                {
                    view.ShadowRenderer.enabled = false;
                }
            }
        }

        private void onCharacterPreDestroyed(CharacterInstance character)
        {
            CharacterView characterView = this.m_characterToCharacterViewMapping[character];
            this.removeCharacterViewFromContainers(characterView);
            if (this.doSkipObjectPooling(character))
            {
                UnityEngine.Object.Destroy(characterView.gameObject);
            }
            else if (this.isPersistentCharacterPrefab(characterView.CharacterPrefab))
            {
                PlayerView.Binder.PersistentCharacterViewPool.returnObject(characterView, characterView.CharacterPrefab);
            }
            else
            {
                this.m_dynamicCharacterViewPool.returnObject(characterView, characterView.CharacterPrefab);
            }
        }

        private void onCharacterRevived(CharacterInstance character)
        {
            this.m_characterToCharacterViewMapping[character].setVisibility(true);
        }

        private void onCharacterSpawned(CharacterInstance character)
        {
            CharacterView view = this.spawnCharacterView(character);
            view.gameObject.SetActive(true);
            view.transform.position = view.Character.PhysicsBody.Transform.position;
            view.transform.localScale = (Vector3) (Vector3.one * ConfigGameplay.SPAWN_ENTRY_START_SCALE);
            view.setVisibility(true);
            view.scale(ConfigGameplay.GetCharacterVisualScale(character), !view.Character.IsPersistent ? ConfigGameplay.SPAWN_ENTRY_DURATION : 0f);
        }

        protected void OnDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayLoadingStarted -= new GameLogic.Events.GameplayLoadingStarted(this.onGameplayLoadingStarted);
            eventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded -= new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            eventBus.OnCharacterSpawned -= new GameLogic.Events.CharacterSpawned(this.onCharacterSpawned);
            eventBus.OnCharacterKilled -= new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            eventBus.OnCharacterRevived -= new GameLogic.Events.CharacterRevived(this.onCharacterRevived);
            eventBus.OnPlayerActiveCharacterSwitched -= new GameLogic.Events.PlayerActiveCharacterSwitched(this.onPlayerActiveCharacterSwitched);
            eventBus.OnCharacterPreDestroyed -= new GameLogic.Events.CharacterPreDestroyed(this.onCharacterPreDestroyed);
            eventBus.OnDropLootTableRolled -= new GameLogic.Events.DropLootTableRolled(this.onDropLootTableRolled);
            eventBus.OnDungeonBoostSpawned -= new GameLogic.Events.DungeonBoostSpawned(this.onDungeonBoostSpawned);
            eventBus.OnDungeonBoostPreDestroy -= new GameLogic.Events.DungeonBoostPreDestroy(this.onDungeonBoostPreDestroy);
        }

        private void onDropLootTableRolled(LootTable lootTable, Vector3 worldPos, Reward reward)
        {
            if (worldPos != Vector3.zero)
            {
                this.visualizeChestDrop(reward, worldPos, null);
            }
        }

        private void onDungeonBoostPreDestroy(DungeonBoost dungeonBoost)
        {
            DungeonBoostView view = this.m_dungeonBoostToDungeonBoostViewMapping[dungeonBoost];
            this.m_dungeonBoostToDungeonBoostViewMapping.Remove(dungeonBoost);
            this.m_dynamicDungeonBoostViewPool.returnObject(view, view.Type);
        }

        private void onDungeonBoostSpawned(DungeonBoost dungeonBoost)
        {
            DungeonBoostType type = (dungeonBoost.Properties.Type != DungeonBoostType.EmptyBox) ? dungeonBoost.Properties.Type : LangUtil.GetRandomValueFromList<DungeonBoostType>(ConfigDungeonBoosts.EMPTY_BOX_VIEW_POOL);
            DungeonBoostView view = this.m_dynamicDungeonBoostViewPool.getObject(type);
            dungeonBoost.PrefabType = view.PrefabType;
            view.gameObject.SetActive(true);
            view.initialize(dungeonBoost);
            this.m_dungeonBoostToDungeonBoostViewMapping.Add(dungeonBoost, view);
            view.Transform.localScale = (Vector3) (Vector3.one * ConfigGameplay.SPAWN_ENTRY_START_SCALE);
            view.scale(1f, ConfigGameplay.SPAWN_ENTRY_DURATION);
        }

        protected void OnEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayLoadingStarted += new GameLogic.Events.GameplayLoadingStarted(this.onGameplayLoadingStarted);
            eventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded += new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            eventBus.OnCharacterSpawned += new GameLogic.Events.CharacterSpawned(this.onCharacterSpawned);
            eventBus.OnCharacterKilled += new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            eventBus.OnCharacterRevived += new GameLogic.Events.CharacterRevived(this.onCharacterRevived);
            eventBus.OnPlayerActiveCharacterSwitched += new GameLogic.Events.PlayerActiveCharacterSwitched(this.onPlayerActiveCharacterSwitched);
            eventBus.OnCharacterPreDestroyed += new GameLogic.Events.CharacterPreDestroyed(this.onCharacterPreDestroyed);
            eventBus.OnDropLootTableRolled += new GameLogic.Events.DropLootTableRolled(this.onDropLootTableRolled);
            eventBus.OnDungeonBoostSpawned += new GameLogic.Events.DungeonBoostSpawned(this.onDungeonBoostSpawned);
            eventBus.OnDungeonBoostPreDestroy += new GameLogic.Events.DungeonBoostPreDestroy(this.onDungeonBoostPreDestroy);
        }

        private void onGameplayEnded(ActiveDungeon activeDungeon)
        {
            if (!activeDungeon.SeamlessTransition)
            {
                this.m_dynamicCharacterViewPool.destroy();
                this.m_dynamicCharacterViewPool = null;
                this.m_dynamicDungeonBoostViewPool.destroy();
                this.m_dynamicDungeonBoostViewPool = null;
            }
        }

        private void onGameplayLoadingStarted(ActiveDungeon activeDungeon)
        {
            if (!activeDungeon.SeamlessTransition)
            {
                for (int i = 0; i < this.m_characterViews.Count; i++)
                {
                    CharacterView view = this.m_characterViews[i];
                    view.gameObject.SetActive(true);
                    view.setVisibility(true);
                }
            }
            this.GameplayPrewarmingCompleted = false;
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            if (!activeDungeon.SeamlessTransition)
            {
                Dictionary<CharacterPrefab, int> initialCapacityPerType = ConfigObjectPools.PER_THEME_CHARACTER_PREFABS[activeDungeon.Dungeon.Theme];
                this.m_dynamicCharacterViewPool = new TypedObjectPool<CharacterView, CharacterPrefab>(new CharacterViewProvider(Layers.CHARACTER_VIEWS, App.Binder.DynamicObjectRootTm), 8, initialCapacityPerType, ObjectPoolExpansionMethod.DOUBLE, true);
                string categoryId = (activeDungeon.DungeonEventType == DungeonEventType.None) ? activeDungeon.Dungeon.Theme.ToString() : activeDungeon.DungeonEventType.ToString();
                this.m_dynamicDungeonBoostViewPool = new TypedObjectPool<DungeonBoostView, DungeonBoostType>(new DungeonBoostViewProvider(Layers.DUNGEON_BOOST_VIEWS, App.Binder.DynamicObjectRootTm, categoryId), 5, ConfigObjectPools.DUNGEON_BOOSTS, ObjectPoolExpansionMethod.DOUBLE, true);
            }
            base.StartCoroutine(this.gameplayStartRoutine(activeDungeon));
        }

        public void onGroundClick(Vector3 contactPos)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (activeDungeon.CurrentGameplayState == GameplayState.ACTION)
            {
                CmdSetCharacterTarget.ExecuteStatic(activeDungeon.PrimaryPlayerCharacter, null, contactPos);
            }
        }

        private void onPlayerActiveCharacterSwitched(CharacterInstance activeCharacter)
        {
            CharacterView characterView = this.getCharacterViewForCharacter(activeCharacter);
            this.removeCharacterViewFromContainers(characterView);
            if (this.doSkipObjectPooling(activeCharacter))
            {
                UnityEngine.Object.Destroy(characterView.gameObject);
            }
            else if (this.isPersistentCharacterPrefab(characterView.CharacterPrefab))
            {
                PlayerView.Binder.PersistentCharacterViewPool.returnObject(characterView, characterView.CharacterPrefab);
            }
            else
            {
                this.m_dynamicCharacterViewPool.returnObject(characterView, characterView.CharacterPrefab);
            }
            this.onCharacterSpawned(activeCharacter);
            this.getCharacterViewForCharacter(activeCharacter).setVisibility(true);
            if ((this.RoomCamera.getActiveCameraMode() != null) && (this.RoomCamera.getActiveCameraMode().getTarget() == activeCharacter))
            {
                this.RoomCamera.setActiveCameraModeTarget(activeCharacter);
            }
        }

        private void removeCharacterViewFromContainers(CharacterView characterView)
        {
            this.m_characterViews.Remove(characterView);
            this.m_characterToCharacterViewMapping.Remove(characterView.Character);
            this.m_gameObjectToCharacterViewMapping.Remove(characterView.gameObject);
        }

        private CharacterView spawnCharacterView(CharacterInstance character)
        {
            CharacterView view;
            if (this.doSkipObjectPooling(character))
            {
                view = PlayerView.Binder.PersistentCharacterViewPool.InstanceProvider.instantiate(character.Prefab);
                view.Transform.SetParent(base.transform, false);
            }
            else if (this.isPersistentCharacterPrefab(character.Prefab))
            {
                view = PlayerView.Binder.PersistentCharacterViewPool.getObject(character.Prefab);
            }
            else
            {
                view = this.m_dynamicCharacterViewPool.getObject(character.Prefab);
            }
            view.initialize(character, false);
            object[] objArray1 = new object[] { "Character_", character.Prefab, "_", character.Id };
            view.name = string.Concat(objArray1);
            this.addCharacterViewToContainers(view);
            return view;
        }

        protected void Update()
        {
            for (int i = this.m_dungeonItemDropViews.Count - 1; i >= 0; i--)
            {
                DungeonDropView view = this.m_dungeonItemDropViews[i];
                if (view.DropSequenceComplete)
                {
                    this.cleanupDungeonItemDropView(i);
                }
            }
        }

        public void visualizeChestDrop(Reward reward, Vector3 worldPos, [Optional, DefaultParameterValue(null)] Sprite overrideSprite)
        {
            DungeonDropView item = PlayerView.Binder.DungeonDropViewPool.getObject();
            item.gameObject.SetActive(true);
            item.initialize(reward);
            item.Tm.SetParent(base.transform);
            item.Tm.position = worldPos + ((Vector3) (Vector3.up * 0.3f));
            if (reward.ChestType != ChestType.NONE)
            {
                item.startChestDropSequence(overrideSprite);
            }
            else if (reward.CoinDrops.Count > 0)
            {
                item.startCoinDropSequence();
            }
            else if (reward.DiamondDrops.Count > 0)
            {
                item.startDiamondDropSequence();
            }
            this.m_dungeonItemDropViews.Add(item);
        }

        private void warmUpCharacterView(string characterId)
        {
            Character character = GameLogic.Binder.CharacterResources.getResource(characterId);
            CharacterView view = this.m_dynamicCharacterViewPool.getObject(character.Prefab);
            foreach (KeyValuePair<Renderer, Mesh> pair in view.Meshes)
            {
                Material sharedMaterial = pair.Key.sharedMaterial;
                Mesh mesh = pair.Value;
                sharedMaterial.SetPass(0);
                Graphics.DrawMeshNow(mesh, Vector3.zero, Quaternion.identity);
            }
            this.m_dynamicCharacterViewPool.returnObject(view, character.Prefab);
        }

        public bool AllDungeonDropsGathered
        {
            get
            {
                return (this.m_dungeonItemDropViews.Count == 0);
            }
        }

        public bool GameplayPrewarmingCompleted
        {
            [CompilerGenerated]
            get
            {
                return this.<GameplayPrewarmingCompleted>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<GameplayPrewarmingCompleted>k__BackingField = value;
            }
        }

        public GameLogic.Room Room
        {
            [CompilerGenerated]
            get
            {
                return this.<Room>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Room>k__BackingField = value;
            }
        }

        public PlayerView.RoomCamera RoomCamera
        {
            [CompilerGenerated]
            get
            {
                return this.<RoomCamera>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RoomCamera>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <gameplayStartRoutine>c__Iterator1A8 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ActiveDungeon <$>activeDungeon;
            internal Dictionary<string, int>.Enumerator <$s_453>__3;
            internal Dictionary<string, int>.Enumerator <$s_454>__5;
            internal RoomView <>f__this;
            internal KeyValuePair<string, int> <kv>__4;
            internal KeyValuePair<string, int> <kv>__6;
            internal CharacterInstance <pc>__0;
            internal Vector3 <toCamDirXz>__1;
            internal Stopwatch <watch>__2;
            internal ActiveDungeon activeDungeon;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<$s_453>__3.Dispose();
                        }
                        break;

                    case 3:
                        try
                        {
                        }
                        finally
                        {
                            this.<$s_454>__5.Dispose();
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.<pc>__0 = GameLogic.Binder.GameState.Player.ActiveCharacter;
                        this.<pc>__0.ExternallyControlled = true;
                        this.<toCamDirXz>__1 = Vector3Extensions.ToXzVector3(PlayerView.Binder.RoomView.RoomCamera.Transform.position - this.<pc>__0.PhysicsBody.Transform.position).normalized;
                        this.<toCamDirXz>__1 = (Vector3) (Quaternion.Euler(0f, UnityEngine.Random.Range((float) -30f, (float) 30f), 0f) * this.<toCamDirXz>__1);
                        CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.<pc>__0, Vector3.zero, this.<toCamDirXz>__1);
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_02AF;

                    case 1:
                        this.<pc>__0.ExternallyControlled = false;
                        if (this.activeDungeon.SeamlessTransition)
                        {
                            goto Label_0295;
                        }
                        this.<watch>__2 = DebugUtil.StartStopwatch();
                        this.<$s_453>__3 = ConfigGameplay.CHARACTER_SPAWN_WEIGHTS[this.activeDungeon.Dungeon.PrimaryMinionType].GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 2:
                        break;

                    case 3:
                        goto Label_01F3;

                    default:
                        goto Label_02AD;
                }
                try
                {
                    while (this.<$s_453>__3.MoveNext())
                    {
                        this.<kv>__4 = this.<$s_453>__3.Current;
                        this.<>f__this.warmUpCharacterView(this.<kv>__4.Key);
                        if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Low)
                        {
                            this.$current = null;
                            this.$PC = 2;
                            flag = true;
                            goto Label_02AF;
                        }
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.<$s_453>__3.Dispose();
                }
                if (this.activeDungeon.Dungeon.SecondaryMinionType == GameLogic.CharacterType.UNSPECIFIED)
                {
                    goto Label_0275;
                }
                this.<$s_454>__5 = ConfigGameplay.CHARACTER_SPAWN_WEIGHTS[this.activeDungeon.Dungeon.SecondaryMinionType].GetEnumerator();
                num = 0xfffffffd;
            Label_01F3:
                try
                {
                    while (this.<$s_454>__5.MoveNext())
                    {
                        this.<kv>__6 = this.<$s_454>__5.Current;
                        this.<>f__this.warmUpCharacterView(this.<kv>__6.Key);
                        if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Low)
                        {
                            this.$current = null;
                            this.$PC = 3;
                            flag = true;
                            goto Label_02AF;
                        }
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.<$s_454>__5.Dispose();
                }
            Label_0275:
                this.<>f__this.warmUpCharacterView(CloneSkill.CLONE_CHARACTER_ID);
                this.<>f__this.warmUpCharacterView("Critter001");
            Label_0295:
                this.<>f__this.GameplayPrewarmingCompleted = true;
                goto Label_02AD;
                this.$PC = -1;
            Label_02AD:
                return false;
            Label_02AF:
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

