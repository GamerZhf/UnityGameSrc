namespace GameLogic
{
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class DecoSpawningSystem : MonoBehaviour, IDecoSpawningSystem
    {
        [CompilerGenerated]
        private float <VisibleDistanceThreshold>k__BackingField;
        private Dictionary<Room.Spawnpoint, bool> m_islandVisibility = new Dictionary<Room.Spawnpoint, bool>();
        private Dictionary<DungeonDecoLayerType, string> m_layers = new Dictionary<DungeonDecoLayerType, string>(new DungeonDecoLayerTypeBoxAvoidanceComparer());

        protected void Awake()
        {
            this.VisibleDistanceThreshold = 45f;
        }

        public void loadAllDecos(Room room)
        {
            for (int i = 0; i < room.IslandSpawnpoints.Count; i++)
            {
                this.loadDecosForIsland(room, room.IslandSpawnpoints[i], room.ActiveDungeon.Dungeon.Theme, room.ActiveDungeon.Mood);
            }
        }

        [DebuggerHidden]
        public IEnumerator loadAllDecosAsync(Room room)
        {
            <loadAllDecosAsync>c__Iterator44 iterator = new <loadAllDecosAsync>c__Iterator44();
            iterator.room = room;
            iterator.<$>room = room;
            iterator.<>f__this = this;
            return iterator;
        }

        private int loadDecosForIsland(Room room, Room.Spawnpoint islandSpawnpoint, DungeonThemeType theme, DungeonMood mood)
        {
            int num = 0;
            List<MarkerSpawnPointDeco> list = room.getDecoSpawnpointMarkersForIsland(islandSpawnpoint);
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].Loaded)
                {
                    list[i].load(theme, mood, this.m_layers);
                    num++;
                }
            }
            return num;
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameplayLoadingStarted -= new GameLogic.Events.GameplayLoadingStarted(this.onGameplayLoadingStarted);
            GameLogic.Binder.EventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnGameplayEndingStarted -= new GameLogic.Events.GameplayEndingStarted(this.onGameplayEndingStarted);
            GameLogic.Binder.EventBus.OnTournamentSelected -= new GameLogic.Events.TournamentSelected(this.onTournamentSelected);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameplayLoadingStarted += new GameLogic.Events.GameplayLoadingStarted(this.onGameplayLoadingStarted);
            GameLogic.Binder.EventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnGameplayEndingStarted += new GameLogic.Events.GameplayEndingStarted(this.onGameplayEndingStarted);
            GameLogic.Binder.EventBus.OnTournamentSelected += new GameLogic.Events.TournamentSelected(this.onTournamentSelected);
        }

        private void onGameplayEndingStarted(ActiveDungeon activeDungeon)
        {
            if (!activeDungeon.SeamlessTransition)
            {
                this.unloadAllDecos(activeDungeon.ActiveRoom);
            }
        }

        private void onGameplayLoadingStarted(ActiveDungeon activeDungeon)
        {
            this.refreshTournamentLayer(activeDungeon.PrimaryPlayerCharacter.OwningPlayer);
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            Room activeRoom = activeDungeon.ActiveRoom;
            this.m_islandVisibility.Clear();
            for (int i = 0; i < activeDungeon.ActiveRoom.IslandSpawnpoints.Count; i++)
            {
                this.m_islandVisibility.Add(activeRoom.IslandSpawnpoints[i], false);
            }
            this.refreshDecosInProximity(activeRoom, activeDungeon.PrimaryPlayerCharacter.PhysicsBody.Transform.position, true);
        }

        private void onTournamentSelected(Player player, TournamentInstance selectedTournament, TournamentInstance unselectedTournament)
        {
            this.refreshTournamentLayer(player);
        }

        private void refreshDecosInProximity(Room room, Vector3 worldPt, bool initialRefresh)
        {
            for (int i = 0; i < room.IslandSpawnpoints.Count; i++)
            {
                Room.Spawnpoint islandSpawnpoint = room.IslandSpawnpoints[i];
                if (Vector3.Distance(worldPt, islandSpawnpoint.WorldPt) < this.VisibleDistanceThreshold)
                {
                    if (initialRefresh || !this.m_islandVisibility[islandSpawnpoint])
                    {
                        this.refreshIslandVisibility(room, islandSpawnpoint, true);
                    }
                }
                else if (initialRefresh || this.m_islandVisibility[islandSpawnpoint])
                {
                    this.refreshIslandVisibility(room, islandSpawnpoint, false);
                }
            }
        }

        private void refreshIslandVisibility(Room room, Room.Spawnpoint islandSpawnpoint, bool visible)
        {
            List<MarkerSpawnPointDeco> list = room.getDecoSpawnpointMarkersForIsland(islandSpawnpoint);
            for (int i = 0; i < list.Count; i++)
            {
                MarkerSpawnPointDeco deco = list[i];
                if (deco.ActiveDeco != null)
                {
                    deco.ActiveDeco.gameObject.SetActive(visible);
                }
            }
            this.m_islandVisibility[islandSpawnpoint] = visible;
        }

        private void refreshTournamentLayer(Player player)
        {
            this.m_layers.Clear();
            if (player.Tournaments.hasTournamentSelected())
            {
                this.m_layers.Add(DungeonDecoLayerType.Tournament, "BossHunt");
                this.m_layers.Add(DungeonDecoLayerType.TournamentCloth, "BossHunt");
            }
        }

        public void unloadAllDecos(Room room)
        {
            for (int i = 0; i < room.IslandSpawnpoints.Count; i++)
            {
                this.unloadDecosForIsland(room, room.IslandSpawnpoints[i]);
            }
        }

        private int unloadDecosForIsland(Room room, Room.Spawnpoint islandSpawnpoint)
        {
            int num = 0;
            List<MarkerSpawnPointDeco> list = room.getDecoSpawnpointMarkersForIsland(islandSpawnpoint);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Loaded)
                {
                    list[i].unload();
                    num++;
                }
            }
            return num;
        }

        protected void Update()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (activeDungeon != null)
            {
                Room activeRoom = activeDungeon.ActiveRoom;
                if (activeRoom != null)
                {
                    CharacterInstance primaryPlayerCharacter = activeDungeon.PrimaryPlayerCharacter;
                    if (!primaryPlayerCharacter.IsDead && ((activeDungeon.CurrentGameplayState == GameplayState.ACTION) || (activeDungeon.CurrentGameplayState == GameplayState.END_CEREMONY)))
                    {
                        this.refreshDecosInProximity(activeRoom, primaryPlayerCharacter.PhysicsBody.Transform.position, false);
                    }
                }
            }
        }

        public float VisibleDistanceThreshold
        {
            [CompilerGenerated]
            get
            {
                return this.<VisibleDistanceThreshold>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<VisibleDistanceThreshold>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <loadAllDecosAsync>c__Iterator44 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Room <$>room;
            internal DecoSpawningSystem <>f__this;
            internal int <i>__0;
            internal Room room;

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
                        this.<i>__0 = 0;
                        break;

                    case 1:
                        this.<i>__0++;
                        break;

                    default:
                        goto Label_00C2;
                }
                if (this.<i>__0 < this.room.IslandSpawnpoints.Count)
                {
                    this.<>f__this.loadDecosForIsland(this.room, this.room.IslandSpawnpoints[this.<i>__0], this.room.ActiveDungeon.Dungeon.Theme, this.room.ActiveDungeon.Mood);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                goto Label_00C2;
                this.$PC = -1;
            Label_00C2:
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

