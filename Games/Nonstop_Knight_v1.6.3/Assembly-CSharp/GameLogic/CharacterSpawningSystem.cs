namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class CharacterSpawningSystem : MonoBehaviour, ICharacterSpawningSystem
    {
        public const float INITIAL_INTERVAL_BETWEEN_WAVES = 2f;
        private Dictionary<string, Horde> m_hordeData = new Dictionary<string, Horde>();
        private Coroutine m_hordeSpawnRoutine;
        private bool m_pendingWildBossSpawn;
        private int m_prevMobSpawnpointIndex;
        private ManualTimer m_spawnWaveTimer = new ManualTimer();
        public const int MAX_NUM_ENEMIES_ALIVE = 0x4b;
        public const string SUPPORT_CRITTER_CHARACTER_ID = "Critter001";

        protected void Awake()
        {
            this.m_hordeData.Clear();
            foreach (UnityEngine.Object obj2 in ResourceUtil.LoadResourcesAtPath("Hordes"))
            {
                if (obj2 is TextAsset)
                {
                    Horde horde = JsonUtils.Deserialize<Horde>(((TextAsset) obj2).text, true);
                    this.m_hordeData.Add(obj2.name, horde);
                }
            }
        }

        protected void FixedUpdate()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (((activeDungeon != null) && (activeDungeon.ActiveRoom != null)) && ((!activeDungeon.ActiveRoom.MainBossSummoned && !activeDungeon.ActiveRoom.CompletionTriggered) && (!activeDungeon.isTutorialDungeon() && (activeDungeon.CurrentGameplayState == GameplayState.ACTION))))
            {
                Player player = GameLogic.Binder.GameState.Player;
                bool flag = true;
                for (int i = 0; i < activeDungeon.ActiveRoom.ActiveCharacters.Count; i++)
                {
                    CharacterInstance instance = activeDungeon.ActiveRoom.ActiveCharacters[i];
                    if (!instance.IsPlayerCharacter && !instance.IsDead)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    int num3;
                    int num2 = activeDungeon.ActiveRoom.getNextCharacterSpawnpointIndex(this.m_prevMobSpawnpointIndex);
                    if (activeDungeon.hasDungeonModifier(DungeonModifierType.HordeMaxSize))
                    {
                        num3 = App.Binder.ConfigMeta.FRENZY_MOB_SPAWNCOUNT_PER_SPAWNPOINT_MAX;
                    }
                    else if (GameLogic.Binder.FrenzySystem.isFrenzyActive())
                    {
                        num3 = Mathf.FloorToInt(Mathf.Lerp((float) App.Binder.ConfigMeta.FRENZY_MOB_SPAWNCOUNT_PER_SPAWNPOINT_MIN, (float) App.Binder.ConfigMeta.FRENZY_MOB_SPAWNCOUNT_PER_SPAWNPOINT_MAX, player.ActiveCharacter.getSpurtBuffStrength()));
                    }
                    else
                    {
                        num3 = Mathf.FloorToInt(Mathf.Lerp((float) ConfigGameplay.MOB_SPAWNCOUNT_PER_SPAWNPOINT_MIN_RANGE.getRandom(), (float) ConfigGameplay.MOB_SPAWNCOUNT_PER_SPAWNPOINT_MAX_RANGE.getRandom(), player.ActiveCharacter.getSpurtBuffStrength()));
                    }
                    int num4 = player.getRemainingMinionKillsUntilFloorCompletion(activeDungeon.Floor, activeDungeon.isTutorialDungeon(), player.getLastBossEncounterFailed(false));
                    if (!activeDungeon.isBossFloor() && (num4 < num3))
                    {
                        num3 = num4;
                    }
                    this.spawnRoomMinionHordeAtSpawnpoint(activeDungeon.ActiveRoom, activeDungeon.ActiveRoom.CharacterSpawnpoints[num2], num3, null);
                    this.m_prevMobSpawnpointIndex = num2;
                }
                if (this.m_pendingWildBossSpawn)
                {
                    this.trySummonWildBoss();
                }
            }
        }

        public int getPreviousMobSpawnpointIndex()
        {
            return this.m_prevMobSpawnpointIndex;
        }

        private void onCharacterRevived(CharacterInstance character)
        {
            if (character.IsPrimaryPlayerCharacter)
            {
                this.refreshPetSummons(character.OwningPlayer);
            }
        }

        protected void OnDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayLoadingStarted -= new GameLogic.Events.GameplayLoadingStarted(this.onGameplayLoadingStarted);
            eventBus.OnGameplayStateChanged -= new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            eventBus.OnRewardConsumed -= new GameLogic.Events.RewardConsumed(this.onRewardConsumed);
            eventBus.OnCharacterRevived -= new GameLogic.Events.CharacterRevived(this.onCharacterRevived);
            eventBus.OnPetSelected -= new GameLogic.Events.PetSelected(this.onPetSelected);
        }

        protected void OnEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayLoadingStarted += new GameLogic.Events.GameplayLoadingStarted(this.onGameplayLoadingStarted);
            eventBus.OnGameplayStateChanged += new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            eventBus.OnRewardConsumed += new GameLogic.Events.RewardConsumed(this.onRewardConsumed);
            eventBus.OnCharacterRevived += new GameLogic.Events.CharacterRevived(this.onCharacterRevived);
            eventBus.OnPetSelected += new GameLogic.Events.PetSelected(this.onPetSelected);
        }

        private void onGameplayLoadingStarted(ActiveDungeon ad)
        {
            if (GameLogic.Binder.FrenzySystem.isFrenzyActive() && ad.SeamlessTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                int spawnCount = Mathf.FloorToInt(Mathf.Lerp((float) App.Binder.ConfigMeta.FRENZY_MOB_SPAWNCOUNT_PER_SPAWNPOINT_MIN, (float) App.Binder.ConfigMeta.FRENZY_MOB_SPAWNCOUNT_PER_SPAWNPOINT_MAX, player.ActiveCharacter.getSpurtBuffStrength()));
                Room.Spawnpoint randomValueFromList = LangUtil.GetRandomValueFromList<Room.Spawnpoint>(ad.ActiveRoom.CharacterSpawnpoints);
                this.spawnRoomMinionHordeAtSpawnpoint(ad.ActiveRoom, randomValueFromList, spawnCount, null);
                this.m_prevMobSpawnpointIndex = ad.ActiveRoom.CharacterSpawnpoints.IndexOf(randomValueFromList);
            }
        }

        private void onGameplayStateChanged(GameplayState previousState, GameplayState currentState)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (currentState == GameplayState.START_CEREMONY_STEP1)
            {
                int num = activeDungeon.ActiveRoom.getNextCharacterSpawnpointIndex(activeDungeon.ActiveRoom.PlayerStartingSpawnpointIndex);
                if (!activeDungeon.isTutorialDungeon())
                {
                    Player player = GameLogic.Binder.GameState.Player;
                    this.refreshPetSummons(player);
                    int spawnCount = Mathf.FloorToInt(Mathf.Lerp((float) ConfigGameplay.MOB_SPAWNCOUNT_PER_SPAWNPOINT_MIN_RANGE.getRandom(), (float) ConfigGameplay.MOB_SPAWNCOUNT_PER_SPAWNPOINT_MAX_RANGE.getRandom(), player.ActiveCharacter.getSpurtBuffStrength()));
                    this.spawnRoomMinionHordeAtSpawnpoint(activeDungeon.ActiveRoom, activeDungeon.ActiveRoom.CharacterSpawnpoints[num], spawnCount, null);
                }
                this.m_prevMobSpawnpointIndex = num;
            }
            else if (currentState == GameplayState.ACTION)
            {
                this.m_spawnWaveTimer.set(2f);
                this.m_spawnWaveTimer.end();
            }
            else if (currentState == GameplayState.BOSS_START)
            {
                UnityUtils.StopCoroutine(this, ref this.m_hordeSpawnRoutine);
            }
            else if (currentState == GameplayState.BOSS_FIGHT)
            {
                this.summonActiveRoomBoss();
            }
        }

        private void onPetSelected(Player player, PetInstance pet)
        {
            this.refreshPetSummons(player);
        }

        private void onRewardConsumed(Player player, Reward reward)
        {
            if (reward.ShopEntryId == ConfigShops.IAP_STARTER_BUNDLE_ID)
            {
                ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                if (((activeDungeon.CurrentGameplayState != GameplayState.END_CEREMONY) && (activeDungeon.CurrentGameplayState != GameplayState.ENDED)) && (activeDungeon.CurrentGameplayState != GameplayState.RETIREMENT))
                {
                    this.refreshPetSummons(player);
                }
            }
        }

        public void refreshPetSummons(Player player)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (((activeDungeon != null) && (activeDungeon.ActiveRoom != null)) && (((activeDungeon.CurrentGameplayState != GameplayState.ROOM_COMPLETION) && (activeDungeon.CurrentGameplayState != GameplayState.ENDED)) && (activeDungeon.CurrentGameplayState != GameplayState.END_CEREMONY)))
            {
                PetInstance instance = player.Pets.getSelectedPetInstance();
                if ((instance != null) && !App.Binder.ConfigMeta.GetPetConfig(instance.CharacterId).Enabled)
                {
                    CmdSelectPet.ExecuteStatic(player, null);
                }
                else if ((instance != null) && (instance.SpawnedCharacterInstance == null))
                {
                    CharacterInstance activeCharacter = player.ActiveCharacter;
                    Vector3 worldPt = Vector3Extensions.ToXzVector3(activeCharacter.PhysicsBody.Transform.position) + ((Vector3) (activeCharacter.PhysicsBody.Transform.forward * 3f));
                    worldPt.x += UnityEngine.Random.Range((float) -1.5f, (float) 1.5f);
                    worldPt.z += UnityEngine.Random.Range((float) -1.5f, (float) 1.5f);
                    worldPt = activeDungeon.ActiveRoom.calculateNearestEmptySpot(worldPt, activeCharacter.PhysicsBody.Transform.position - worldPt, 1f, 1f, 6f, null);
                    Character character = instance.Character;
                    CmdSpawnCharacter.SpawningData data2 = new CmdSpawnCharacter.SpawningData();
                    data2.CharacterPrototype = character;
                    data2.Rank = instance.Level;
                    data2.SpawnWorldPos = worldPt;
                    data2.SpawnWorlRot = activeCharacter.PhysicsBody.Transform.rotation;
                    data2.IsPlayerCharacter = true;
                    data2.IsPlayerSupportCharacter = true;
                    data2.IsPet = true;
                    CmdSpawnCharacter.SpawningData data = data2;
                    instance.SpawnedCharacterInstance = CmdSpawnCharacter.ExecuteStatic(data);
                }
            }
        }

        [DebuggerHidden]
        private IEnumerator spawnHordeAt(Horde horde, Room.Spawnpoint spawnpoint, bool isBoss, bool isEliteBoss, bool isWildBoss)
        {
            <spawnHordeAt>c__Iterator47 iterator = new <spawnHordeAt>c__Iterator47();
            iterator.horde = horde;
            iterator.spawnpoint = spawnpoint;
            iterator.isEliteBoss = isEliteBoss;
            iterator.isBoss = isBoss;
            iterator.isWildBoss = isWildBoss;
            iterator.<$>horde = horde;
            iterator.<$>spawnpoint = spawnpoint;
            iterator.<$>isEliteBoss = isEliteBoss;
            iterator.<$>isBoss = isBoss;
            iterator.<$>isWildBoss = isWildBoss;
            iterator.<>f__this = this;
            return iterator;
        }

        public void spawnRoomMinionHordeAtSpawnpoint(Room room, Room.Spawnpoint spawnPt, int spawnCount, [Optional, DefaultParameterValue(null)] string fixedCharacterId)
        {
            ActiveDungeon activeDungeon = room.ActiveDungeon;
            Player player = GameLogic.Binder.GameState.Player;
            Horde horde = new Horde();
            horde.CharacterIds = new List<string>();
            for (int i = 0; i < spawnCount; i++)
            {
                string str;
                if (!string.IsNullOrEmpty(fixedCharacterId))
                {
                    str = fixedCharacterId;
                }
                else
                {
                    str = room.ActiveDungeon.Dungeon.getRandomMinionId();
                }
                horde.CharacterIds.Add(str);
            }
            this.m_hordeSpawnRoutine = UnityUtils.StartCoroutine(this, this.spawnHordeAt(horde, spawnPt, false, false, false));
            room.NumMobsSpawned++;
            if ((((activeDungeon.ActiveTournament != null) && (room.NumMobsSpawned > 1)) && ((activeDungeon.Floor >= App.Binder.ConfigMeta.TOURNAMENT_WILD_BOSS_MIN_SUMMON_FLOOR) && (activeDungeon.ActiveTournament.WildBossesKilledSinceLastRoomCompletion < ConfigTournaments.TOURNAMENT_WILD_BOSS_MAX_KILLS_PER_FLOOR))) && (activeDungeon.ActiveTournament.getWildBossSummonsRemaining() > 0))
            {
                UnityEngine.Debug.Log(string.Concat(new object[] { "Wild boss spawn chance: ", activeDungeon.ActiveTournament.WildBossSpawnChance, " -- summons remaining: ", activeDungeon.ActiveTournament.getWildBossSummonsRemaining() }));
                if (UnityEngine.Random.Range((float) 0f, (float) 1f) <= activeDungeon.ActiveTournament.WildBossSpawnChance)
                {
                    this.m_pendingWildBossSpawn = true;
                    activeDungeon.ActiveTournament.WildBossSpawnChance = ConfigTournaments.TOURNAMENT_WILD_BOSS_SPAWN_CHANCE_BASE;
                }
                else
                {
                    Character referenceBoss = GameLogic.Binder.CharacterResources.getResource(activeDungeon.Dungeon.getRandomMinionId());
                    string key = ConfigGameplay.CalculateBossDifficulty(player, activeDungeon.Floor, activeDungeon.isEliteBossFloor(), referenceBoss, activeDungeon.getProgressDifficultyExponent());
                    float num2 = LangUtil.TryGetFloatValueFromDictionary<string>(ConfigTournaments.TOURNAMENT_WILD_BOSS_SPAWN_CHANCE_INCREASE_PER_MOB, key);
                    activeDungeon.ActiveTournament.WildBossSpawnChance += num2;
                }
            }
        }

        public void summonActiveRoomBoss()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (!string.IsNullOrEmpty(activeDungeon.BossId))
            {
                Room activeRoom = activeDungeon.ActiveRoom;
                Vector3 position = GameLogic.Binder.GameState.ActiveDungeon.PrimaryPlayerCharacter.PhysicsBody.Transform.position;
                UnityUtils.StopCoroutine(this, ref this.m_hordeSpawnRoutine);
                bool isEliteBoss = activeDungeon.isEliteBossFloor();
                float maxValue = float.MaxValue;
                Room.Spawnpoint spawnpoint3 = new Room.Spawnpoint();
                spawnpoint3.WorldPt = Vector3.zero;
                Room.Spawnpoint spawnpoint = spawnpoint3;
                for (int i = 0; i < activeRoom.CharacterSpawnpoints.Count; i++)
                {
                    Room.Spawnpoint spawnpoint2 = activeRoom.CharacterSpawnpoints[i];
                    float num3 = Vector3.Distance(position, spawnpoint2.WorldPt);
                    if ((num3 > ConfigGameplay.BOSS_SPAWN_POINT_MIN_DISTANCE) && (num3 < maxValue))
                    {
                        spawnpoint = spawnpoint2;
                        maxValue = num3;
                    }
                }
                Horde horde = new Horde();
                horde.CharacterIds.Add(activeDungeon.BossId);
                this.m_hordeSpawnRoutine = UnityUtils.StartCoroutine(this, this.spawnHordeAt(horde, spawnpoint, true, isEliteBoss, false));
            }
        }

        public void summonSupportCritters(CharacterInstance owningCharacter, int critterCount, [Optional, DefaultParameterValue(null)] Vector3? fixedSpawnPos)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (((activeDungeon != null) && (activeDungeon.ActiveRoom != null)) && (((activeDungeon.CurrentGameplayState != GameplayState.ROOM_COMPLETION) && (activeDungeon.CurrentGameplayState != GameplayState.ENDED)) && (activeDungeon.CurrentGameplayState != GameplayState.END_CEREMONY)))
            {
                int num = 0;
                for (int i = 0; i < activeDungeon.ActiveRoom.ActiveCharacters.Count; i++)
                {
                    CharacterInstance instance = activeDungeon.ActiveRoom.ActiveCharacters[i];
                    if ((instance.IsSupport && !instance.IsDead) && (instance.Prefab == CharacterPrefab.Critter))
                    {
                        num++;
                    }
                }
                if (num < ConfigGameplay.CRITTER_MAX_COUNT)
                {
                    Character summonedCharacterPrototype = GameLogic.Binder.CharacterResources.getResource("Critter001");
                    float num3 = owningCharacter.getLimitedLifetimeForSummon(summonedCharacterPrototype);
                    Vector3 vector = Vector3Extensions.ToXzVector3(owningCharacter.PhysicsBody.Transform.position);
                    for (int j = 0; j < critterCount; j++)
                    {
                        Vector3 vector2;
                        if (fixedSpawnPos.HasValue)
                        {
                            vector2 = fixedSpawnPos.Value;
                        }
                        else
                        {
                            vector2 = vector + ((Vector3) (owningCharacter.PhysicsBody.Transform.forward * 4f));
                            vector2.x += UnityEngine.Random.Range((float) -2f, (float) 2f);
                            vector2.z += UnityEngine.Random.Range((float) -2f, (float) 2f);
                        }
                        int? mask = null;
                        vector2 = activeDungeon.ActiveRoom.calculateNearestEmptySpot(vector2, owningCharacter.PhysicsBody.Transform.position - vector2, 1f, 1f, 6f, mask);
                        CmdSpawnCharacter.SpawningData data2 = new CmdSpawnCharacter.SpawningData();
                        data2.CharacterPrototype = summonedCharacterPrototype;
                        data2.SpawnWorldPos = vector2;
                        data2.SpawnWorlRot = owningCharacter.PhysicsBody.Transform.rotation;
                        data2.IsPlayerCharacter = true;
                        data2.IsPlayerSupportCharacter = true;
                        data2.LimitedLifetimeSeconds = num3;
                        CmdSpawnCharacter.SpawningData data = data2;
                        CmdSpawnCharacter.ExecuteStatic(data);
                    }
                }
            }
        }

        public void trySummonWildBoss()
        {
            if (!UnityUtils.CoroutineRunning(ref this.m_hordeSpawnRoutine))
            {
                ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                if (!GameLogic.Binder.GameState.Player.BossTrain.Active)
                {
                    string item = activeDungeon.Dungeon.getRandomMinionId();
                    Room.Spawnpoint spawnpoint = activeDungeon.ActiveRoom.CharacterSpawnpoints[this.m_prevMobSpawnpointIndex];
                    Horde horde = new Horde();
                    horde.CharacterIds.Add(item);
                    UnityUtils.StopCoroutine(this, ref this.m_hordeSpawnRoutine);
                    this.m_hordeSpawnRoutine = UnityUtils.StartCoroutine(this, this.spawnHordeAt(horde, spawnpoint, true, false, true));
                    activeDungeon.ActiveRoom.WildBossSummoned = item;
                    this.m_pendingWildBossSpawn = false;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <spawnHordeAt>c__Iterator47 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Horde <$>horde;
            internal bool <$>isBoss;
            internal bool <$>isEliteBoss;
            internal bool <$>isWildBoss;
            internal Room.Spawnpoint <$>spawnpoint;
            internal CharacterSpawningSystem <>f__this;
            internal ActiveDungeon <ad>__0;
            internal List<PerkInstance> <bossPerks>__8;
            internal string <characterId>__5;
            internal int <i>__4;
            internal float <minDistFromCenter>__3;
            internal PerkType <perkType>__9;
            internal Player <player>__2;
            internal Vector3 <pos>__6;
            internal int <rank>__7;
            internal Room <room>__1;
            internal CmdSpawnCharacter.SpawningData <sd>__10;
            internal Horde horde;
            internal bool isBoss;
            internal bool isEliteBoss;
            internal bool isWildBoss;
            internal Room.Spawnpoint spawnpoint;

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
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<room>__1 = this.<ad>__0.ActiveRoom;
                        this.<player>__2 = GameLogic.Binder.GameState.Player;
                        this.<minDistFromCenter>__3 = 1f;
                        this.<i>__4 = 0;
                        break;

                    case 1:
                        this.<i>__4++;
                        break;

                    default:
                        goto Label_031F;
                }
                if (this.<i>__4 < this.horde.CharacterIds.Count)
                {
                    this.<characterId>__5 = this.horde.CharacterIds[this.<i>__4];
                    this.<pos>__6 = this.spawnpoint.WorldPt;
                    if (this.<i>__4 > 0)
                    {
                        this.<pos>__6.x += UnityEngine.Random.Range(this.<minDistFromCenter>__3, this.<minDistFromCenter>__3 + 1f) * ((UnityEngine.Random.Range(0, 2) != 0) ? 1f : -1f);
                        this.<pos>__6.z += UnityEngine.Random.Range(this.<minDistFromCenter>__3, this.<minDistFromCenter>__3 + 1f) * ((UnityEngine.Random.Range(0, 2) != 0) ? 1f : -1f);
                        this.<minDistFromCenter>__3++;
                        if (this.<minDistFromCenter>__3 >= 4f)
                        {
                            this.<minDistFromCenter>__3 = 1f;
                        }
                        this.<pos>__6 = this.<room>__1.calculateNearestEmptySpot(this.<pos>__6, this.spawnpoint.WorldPt - this.<pos>__6, 1f, 1f, 6f, null);
                    }
                    this.<rank>__7 = this.<ad>__0.Floor;
                    this.<bossPerks>__8 = null;
                    if (this.isEliteBoss)
                    {
                        this.<bossPerks>__8 = new List<PerkInstance>();
                        this.<perkType>__9 = ConfigPerks.GetBossPerkType(this.<player>__2, this.<characterId>__5);
                        if (this.<perkType>__9 != PerkType.NONE)
                        {
                            PerkInstance item = new PerkInstance();
                            item.Type = this.<perkType>__9;
                            item.Modifier = ConfigPerks.GetBestModifier(this.<perkType>__9);
                            this.<bossPerks>__8.Add(item);
                        }
                    }
                    CmdSpawnCharacter.SpawningData data = new CmdSpawnCharacter.SpawningData();
                    data.CharacterPrototype = GameLogic.Binder.CharacterResources.getResource(this.<characterId>__5);
                    data.Rank = Mathf.Clamp(this.<rank>__7, 1, 0x7fffffff);
                    data.SpawnWorldPos = this.<pos>__6;
                    data.SpawnWorlRot = this.spawnpoint.WorldRot;
                    data.IsBoss = this.isBoss;
                    data.IsEliteBoss = this.isEliteBoss;
                    data.IsWildBoss = this.isWildBoss;
                    data.BossPerks = this.<bossPerks>__8;
                    this.<sd>__10 = data;
                    CmdSpawnCharacter.ExecuteStatic(this.<sd>__10);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                GameLogic.Binder.EventBus.CharacterHordeSpawned(this.spawnpoint, this.isBoss);
                this.<>f__this.m_hordeSpawnRoutine = null;
                this.$PC = -1;
            Label_031F:
                return false;
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

