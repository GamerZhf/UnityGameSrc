namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class DungeonBoostSpawningSystem : MonoBehaviour, IDungeonBoostSpawningSystem
    {
        private Room.Spawnpoint m_lastIslandSpawnpointForBoxes;
        private List<MarkerSpawnPointDeco> m_validDecoSpawnpoints = new List<MarkerSpawnPointDeco>(0x10);

        private void onCharacterHordeSpawned(Room.Spawnpoint spawnpoint, bool isBoss)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            Player player = GameLogic.Binder.GameState.Player;
            activeDungeon.ActiveRoom.flagAllDungeonBoostsForOffscreenDestroy();
            Room.Spawnpoint islandSpawnpoint = activeDungeon.ActiveRoom.getClosestIslandSpawnpoint(spawnpoint.WorldPt);
            List<MarkerSpawnPointDeco> list = activeDungeon.ActiveRoom.getDecoSpawnpointMarkersForIsland(islandSpawnpoint);
            if ((App.Binder.ConfigMeta.DUNGEON_BOOST_BOX_SPAWN_ENABLED_DURING_FRENZY || !GameLogic.Binder.FrenzySystem.isFrenzyActive()) && (islandSpawnpoint != this.m_lastIslandSpawnpointForBoxes))
            {
                this.m_validDecoSpawnpoints.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    MarkerSpawnPointDeco item = list[i];
                    if (ConfigDungeons.DUNGEON_DECO_CATEGORIES_AS_DUNGEON_BOOST_SPAWNPOINTS.Contains(item.DecoCategoryType) && (item.ActiveDeco != null))
                    {
                        this.m_validDecoSpawnpoints.Add(item);
                    }
                }
                int num2 = ConfigDungeonBoosts.GetBoxSpawnCountForFloor(player, activeDungeon.Floor, this.m_validDecoSpawnpoints.Count);
                for (int j = 0; (j < this.m_validDecoSpawnpoints.Count) && (j < num2); j++)
                {
                    MarkerSpawnPointDeco deco2 = this.m_validDecoSpawnpoints[j];
                    Vector3 position = deco2.transform.position;
                    Vector3 vector4 = islandSpawnpoint.WorldPt - deco2.transform.position;
                    Vector3 normalized = vector4.normalized;
                    Vector3 worldPt = position + ((Vector3) (normalized * 1f));
                    float fallbackStep = ConfigDungeonBoosts.BOX_RADIUS;
                    worldPt = activeDungeon.ActiveRoom.calculateNearestEmptySpot(worldPt, normalized, 1f, fallbackStep, 6f, new int?(Layers.DungeonBoostEmptySpotLayerMask));
                    worldPt.y = activeDungeon.ActiveRoom.WorldGroundPosY;
                    SpawnBox(CmdRollDungeonBoostBoxTable.ExecuteStatic(App.Binder.ConfigLootTables.DungeonBoostBoxTable, player), player, worldPt);
                }
            }
            this.m_lastIslandSpawnpointForBoxes = islandSpawnpoint;
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnCharacterHordeSpawned -= new GameLogic.Events.CharacterHordeSpawned(this.onCharacterHordeSpawned);
            GameLogic.Binder.EventBus.OnGameplayLoadingStarted -= new GameLogic.Events.GameplayLoadingStarted(this.onGameplayEnded);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnCharacterHordeSpawned += new GameLogic.Events.CharacterHordeSpawned(this.onCharacterHordeSpawned);
            GameLogic.Binder.EventBus.OnGameplayEnded += new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
        }

        private void onGameplayEnded(ActiveDungeon ad)
        {
            this.m_lastIslandSpawnpointForBoxes = null;
        }

        private static void SpawnBox(DungeonBoostType type, Player player, Vector3 position)
        {
            if (type != DungeonBoostType.None)
            {
                DungeonBoost.DungeonBoostProperties properties2 = new DungeonBoost.DungeonBoostProperties();
                properties2.Type = type;
                properties2.ActivationType = DungeonBoostActivationType.DestructibleHit;
                properties2.DoDestroyOnActivation = true;
                properties2.Radius = ConfigDungeonBoosts.BOX_RADIUS;
                DungeonBoost.DungeonBoostProperties props = properties2;
                switch (type)
                {
                    case DungeonBoostType.BuffBox:
                    case DungeonBoostType.ExplosiveBox:
                        props.BuffPerkType = LangUtil.GetRandomValueFromList<PerkType>(ConfigPerks.DUNGEON_BOOST_PERK_POOLS[type]);
                        break;

                    case DungeonBoostType.ResourceBox:
                        props.ShopEntryId = CmdRollDungeonBoostResourceBoxLootTable.ExecuteStatic(App.Binder.ConfigLootTables.DungeonBoostResourceBoxLootTable, player);
                        break;
                }
                DungeonBoost dungeonBoost = GameLogic.Binder.DungeonBoostPool.getObject();
                dungeonBoost.gameObject.SetActive(true);
                dungeonBoost.initialize(position, props);
                GameLogic.Binder.EventBus.DungeonBoostSpawned(dungeonBoost);
            }
        }
    }
}

