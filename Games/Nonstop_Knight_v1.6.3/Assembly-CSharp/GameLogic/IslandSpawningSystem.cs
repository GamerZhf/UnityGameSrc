namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class IslandSpawningSystem : MonoBehaviour, IIslandSpawningSystem
    {
        private string m_activeIslandId;
        private Room.Spawnpoint m_activeIslandSpawnpoint;
        private List<string> m_islandIds = new List<string>(100);
        private Dictionary<string, ObjectLayout> m_islandPool = new Dictionary<string, ObjectLayout>(100);
        private string m_prevIslandId;
        private Room.Spawnpoint m_prevIslandSpawnpoint;

        private void assignActiveIslandAsPrevious()
        {
            this.m_prevIslandId = this.m_activeIslandId;
            this.m_prevIslandSpawnpoint = this.m_activeIslandSpawnpoint;
        }

        protected void Awake()
        {
            foreach (UnityEngine.Object obj2 in ResourceUtil.LoadResourcesAtPath(ConfigDungeons.DUNGEON_ISLAND_RESOURCE_PATH))
            {
                if (obj2 is TextAsset)
                {
                    this.m_islandIds.Add(obj2.name);
                }
            }
        }

        private void clearActiveIsland()
        {
            if (this.m_activeIslandSpawnpoint != null)
            {
                this.m_islandPool[this.m_activeIslandId].gameObject.SetActive(false);
                this.m_activeIslandId = string.Empty;
                this.m_activeIslandSpawnpoint = null;
            }
        }

        private void clearPreviousIsland()
        {
            if (this.m_prevIslandSpawnpoint != null)
            {
                this.m_islandPool[this.m_prevIslandId].gameObject.SetActive(false);
                this.m_prevIslandId = string.Empty;
                this.m_prevIslandSpawnpoint = null;
            }
        }

        private string getRandomFreeIslandId()
        {
            string randomValueFromList;
            do
            {
                randomValueFromList = LangUtil.GetRandomValueFromList<string>(this.m_islandIds);
            }
            while (randomValueFromList == this.m_prevIslandId);
            return randomValueFromList;
        }

        private void onCharacterSpawned(CharacterInstance character)
        {
            if (!character.IsPlayerCharacter)
            {
                Room.Spawnpoint spawnpoint = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.getClosestIslandSpawnpoint(character.PhysicsBody.Transform.position);
                if (spawnpoint != this.m_activeIslandSpawnpoint)
                {
                    this.clearPreviousIsland();
                    this.assignActiveIslandAsPrevious();
                    this.spawnActiveIsland(spawnpoint);
                }
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            GameLogic.Binder.EventBus.OnRoomLoaded -= new GameLogic.Events.RoomLoaded(this.onRoomLoaded);
            GameLogic.Binder.EventBus.OnGameplayEnded -= new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            GameLogic.Binder.EventBus.OnCharacterSpawned -= new GameLogic.Events.CharacterSpawned(this.onCharacterSpawned);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            GameLogic.Binder.EventBus.OnRoomLoaded += new GameLogic.Events.RoomLoaded(this.onRoomLoaded);
            GameLogic.Binder.EventBus.OnGameplayEnded += new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            GameLogic.Binder.EventBus.OnCharacterSpawned += new GameLogic.Events.CharacterSpawned(this.onCharacterSpawned);
        }

        private void onGameplayEnded(ActiveDungeon activeDungeon)
        {
            this.clearPreviousIsland();
            this.clearActiveIsland();
        }

        private void onGameStateInitialized()
        {
            for (int i = 0; i < this.m_islandIds.Count; i++)
            {
                string key = this.m_islandIds[i];
                GameObject obj2 = new GameObject("DungeonIsland-" + key);
                obj2.transform.SetParent(base.transform, false);
                ObjectLayout layout = obj2.AddComponent<ObjectLayout>();
                layout.ResourcePath = ConfigDungeons.DUNGEON_ISLAND_RESOURCE_PATH;
                layout.Id = key;
                layout.load(null);
                this.m_islandPool.Add(key, layout);
                obj2.SetActive(false);
            }
        }

        private void onRoomLoaded(Room room)
        {
            Room.Spawnpoint spawnpoint = room.getClosestIslandSpawnpoint(room.CharacterSpawnpoints[room.PlayerStartingSpawnpointIndex].WorldPt);
            if (spawnpoint != null)
            {
                this.spawnActiveIsland(spawnpoint);
            }
        }

        private void spawnActiveIsland(Room.Spawnpoint spawnpoint)
        {
            string islandId = this.getRandomFreeIslandId();
            this.spawnIslandAt(islandId, spawnpoint);
            this.m_activeIslandId = islandId;
            this.m_activeIslandSpawnpoint = spawnpoint;
        }

        private void spawnIslandAt(string islandId, Room.Spawnpoint spawnpoint)
        {
            ObjectLayout layout = this.m_islandPool[islandId];
            layout.Tm.position = spawnpoint.WorldPt;
            layout.gameObject.SetActive(true);
        }
    }
}

