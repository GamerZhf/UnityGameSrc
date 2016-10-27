namespace GameLogic
{
    using App;
    using Pathfinding;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class Room
    {
        public List<AreaEffect> ActiveAreaEffects = new List<AreaEffect>(8);
        public List<CharacterInstance> ActiveCharacters = new List<CharacterInstance>(100);
        public GameLogic.ActiveDungeon ActiveDungeon;
        public List<DungeonBoost> ActiveDungeonBoosts = new List<DungeonBoost>(0x20);
        public string ActiveLayoutId = string.Empty;
        public List<Projectile> ActiveProjectiles = new List<Projectile>(0x40);
        public AstarPath AstarPath;
        public string BossDifficultyDuringSummon;
        public BossSummonMethod BossSummonedWith;
        public List<Spawnpoint> CharacterSpawnpoints = new List<Spawnpoint>(100);
        public bool CompletionTriggered;
        public List<MarkerSpawnPointDeco> DecoSpawnpoints = new List<MarkerSpawnPointDeco>(500);
        public List<DungeonBlock> DungeonBlocks = new List<DungeonBlock>(0x3e8);
        public GameObject DungeonFog;
        public RoomEndCondition EndCondition = RoomEndCondition.FAIL;
        public List<Spawnpoint> IslandSpawnpoints = new List<Spawnpoint>(50);
        public GameObject LayoutRoot;
        private NNConstraint m_defaultAstarConstraint = new NNConstraint();
        private Dictionary<Spawnpoint, List<MarkerSpawnPointDeco>> m_islandToDecoMap = new Dictionary<Spawnpoint, List<MarkerSpawnPointDeco>>();
        public bool MainBossSummoned;
        public int NumMobsSpawned;
        public int PlayerStartingSpawnpointIndex;
        public GameLogic.RoomLayout RoomLayout;
        private static List<CharacterInstance> sm_tempCandidateList = new List<CharacterInstance>(0x40);
        public GameObject SpawnpointRoot;
        public string WildBossSummoned;
        public float WorldGroundPosY;

        public Vector3 adjustToNearestGridPoint(Vector3 worldPt)
        {
            return this.AstarPath.GetNearest(worldPt, this.m_defaultAstarConstraint).position;
        }

        public void calculateIslandToDecoMapping()
        {
            this.m_islandToDecoMap.Clear();
            if (this.DecoSpawnpoints.Count != 0)
            {
                if ((this.DecoSpawnpoints.Count > 0) && (this.IslandSpawnpoints.Count == 0))
                {
                    Debug.LogError("Decos require at least one island spawnpoint.");
                }
                else
                {
                    for (int i = 0; i < this.IslandSpawnpoints.Count; i++)
                    {
                        this.m_islandToDecoMap.Add(this.IslandSpawnpoints[i], new List<MarkerSpawnPointDeco>());
                    }
                    for (int j = 0; j < this.DecoSpawnpoints.Count; j++)
                    {
                        MarkerSpawnPointDeco item = this.DecoSpawnpoints[j];
                        float maxValue = float.MaxValue;
                        Spawnpoint spawnpoint = null;
                        for (int k = 0; k < this.IslandSpawnpoints.Count; k++)
                        {
                            Spawnpoint spawnpoint2 = this.IslandSpawnpoints[k];
                            float num5 = Vector3.Distance(item.Tm.position, spawnpoint2.WorldPt);
                            if (num5 < maxValue)
                            {
                                spawnpoint = spawnpoint2;
                                maxValue = num5;
                            }
                        }
                        this.m_islandToDecoMap[spawnpoint].Add(item);
                    }
                }
            }
        }

        public Vector3 calculateNearestEmptySpot(Vector3 worldPt, Vector3 preferredFallbackDirection, [Optional, DefaultParameterValue(1f)] float emptySpotRadius, [Optional, DefaultParameterValue(1f)] float fallbackStep, [Optional, DefaultParameterValue(6f)] float maxFallbackDistance, [Optional, DefaultParameterValue(null)] int? mask)
        {
            Vector3 normalized;
            mask = mask.HasValue ? mask : new int?(Layers.EmptySpotLayerMask);
            if (preferredFallbackDirection != Vector3.zero)
            {
                normalized = Vector3Extensions.ToXzVector3(preferredFallbackDirection).normalized;
            }
            else
            {
                Vector3 vector5 = new Vector3(UnityEngine.Random.insideUnitCircle.x, 0f, UnityEngine.Random.insideUnitCircle.y);
                normalized = vector5.normalized;
            }
            Vector3 vector2 = worldPt;
            for (float i = 0f; i < maxFallbackDistance; i += fallbackStep)
            {
                for (float j = 0f; j <= 315f; j += 45f)
                {
                    Vector3 vector3 = (Vector3) (Quaternion.AngleAxis(j, Vector3.up) * normalized);
                    vector2 += (Vector3) (vector3 * i);
                    vector2 = this.adjustToNearestGridPoint(vector2);
                    if (!Physics.CheckSphere(vector2, emptySpotRadius, mask.Value))
                    {
                        return vector2;
                    }
                }
            }
            return worldPt;
        }

        public bool characterWithinAttackDistance(CharacterInstance attacker, CharacterInstance target)
        {
            if ((attacker == null) || (target == null))
            {
                return false;
            }
            Vector3 position = attacker.PhysicsBody.Transform.position;
            return (Vector3.Distance(Vector3Extensions.ToXzVector3(target.PhysicsBody.Transform.position), position) <= (attacker.AttackRange(true) + target.Radius));
        }

        public void destroyAllDungeonBoosts()
        {
            for (int i = this.ActiveDungeonBoosts.Count - 1; i >= 0; i--)
            {
                this.ActiveDungeonBoosts[i].destroy();
            }
        }

        public void destroyAllProjectiles()
        {
            for (int i = this.ActiveProjectiles.Count - 1; i >= 0; i--)
            {
                this.ActiveProjectiles[i].destroy();
            }
        }

        public void destroyAllProjectilesFromCharacter(CharacterInstance character)
        {
            for (int i = this.ActiveProjectiles.Count - 1; i >= 0; i--)
            {
                if (this.ActiveProjectiles[i].OwningCharacter == character)
                {
                    this.ActiveProjectiles[i].destroy();
                }
            }
        }

        public void flagAllDungeonBoostsForOffscreenDestroy()
        {
            for (int i = this.ActiveDungeonBoosts.Count - 1; i >= 0; i--)
            {
                this.ActiveDungeonBoosts[i].flagForOffscreenDestroy();
            }
        }

        public List<CharacterInstance> getCharactersWithinRadius(Vector3 worldPt, float radius)
        {
            List<CharacterInstance> list = new List<CharacterInstance>();
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance item = this.ActiveCharacters[i];
                if ((!item.IsDead && !item.isInvisible()) && (Vector3.Distance(Vector3Extensions.ToXzVector3(worldPt), Vector3Extensions.ToXzVector3(item.PhysicsBody.Transform.position)) <= (radius + item.Radius)))
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public Spawnpoint getClosestCharacterSpawnpoint(CharacterInstance refCharacter)
        {
            Spawnpoint spawnpoint = null;
            float maxValue = float.MaxValue;
            for (int i = 0; i < this.CharacterSpawnpoints.Count; i++)
            {
                Spawnpoint spawnpoint2 = this.CharacterSpawnpoints[i];
                float num3 = Vector3.Distance(spawnpoint2.WorldPt, refCharacter.PhysicsBody.Transform.position);
                if (num3 < maxValue)
                {
                    spawnpoint = spawnpoint2;
                    maxValue = num3;
                }
            }
            return spawnpoint;
        }

        public CharacterInstance getClosestEnemyBossCharacter(CharacterInstance referenceCharacter, bool lineOfSight)
        {
            Vector3 position = referenceCharacter.PhysicsBody.Transform.position;
            CharacterInstance instance = null;
            float maxValue = float.MaxValue;
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance another = this.ActiveCharacters[i];
                if (((((another != referenceCharacter) && another.IsBoss) && (!another.IsDead && !referenceCharacter.isFriendlyTowards(another))) && !another.isInvisible()) && (!lineOfSight || this.lineOfSightCharacterToCharacter(referenceCharacter, another)))
                {
                    float num3 = Vector3.Distance(position, another.PhysicsBody.Transform.position) - another.Radius;
                    if (num3 < maxValue)
                    {
                        instance = another;
                        maxValue = num3;
                    }
                }
            }
            return instance;
        }

        public CharacterInstance getClosestEnemyCharacter(CharacterInstance referenceCharacter, bool lineOfSight)
        {
            Vector3 position = referenceCharacter.PhysicsBody.Transform.position;
            CharacterInstance instance = null;
            float maxValue = float.MaxValue;
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance another = this.ActiveCharacters[i];
                if (((((another != referenceCharacter) && !another.IsDead) && !referenceCharacter.isFriendlyTowards(another)) && !another.isInvisible()) && (!lineOfSight || this.lineOfSightCharacterToCharacter(referenceCharacter, another)))
                {
                    float num3 = Vector3.Distance(position, another.PhysicsBody.Transform.position) - another.Radius;
                    if (num3 < maxValue)
                    {
                        instance = another;
                        maxValue = num3;
                    }
                }
            }
            return instance;
        }

        public CharacterInstance getClosestEnemyCharacterWithHighestLife(Vector3 worldPt, CharacterInstance referenceCharacter, bool lineOfSight, float withinRadius)
        {
            CharacterInstance instance = null;
            double minValue = double.MinValue;
            float maxValue = float.MaxValue;
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance another = this.ActiveCharacters[i];
                if (((((another != referenceCharacter) && !another.IsDead) && !referenceCharacter.isFriendlyTowards(another)) && !another.isInvisible()) && (!lineOfSight || this.lineOfSightCharacterToCharacter(referenceCharacter, another)))
                {
                    float num4 = PhysicsUtil.DistBetween(referenceCharacter, another);
                    if ((num4 <= withinRadius) && ((another.MaxLife(true) > minValue) || ((another.MaxLife(true) == minValue) && (num4 < maxValue))))
                    {
                        instance = another;
                        minValue = another.MaxLife(true);
                        maxValue = num4;
                    }
                }
            }
            return instance;
        }

        public CharacterInstance getClosestEnemyCharacterWithinRadius(Vector3 worldPt, float radius, CharacterInstance referenceCharacter)
        {
            CharacterInstance instance = null;
            float maxValue = float.MaxValue;
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance another = this.ActiveCharacters[i];
                if ((((another != referenceCharacter) && !another.IsDead) && !referenceCharacter.isFriendlyTowards(another)) && !another.isInvisible())
                {
                    float num3 = Vector3.Distance(worldPt, another.PhysicsBody.Transform.position) - another.Radius;
                    if ((num3 <= radius) && (num3 < maxValue))
                    {
                        instance = another;
                        maxValue = num3;
                    }
                }
            }
            return instance;
        }

        public Spawnpoint getClosestIslandSpawnpoint(Vector3 worldPt)
        {
            Spawnpoint spawnpoint = null;
            float maxValue = float.MaxValue;
            for (int i = 0; i < this.IslandSpawnpoints.Count; i++)
            {
                Spawnpoint spawnpoint2 = this.IslandSpawnpoints[i];
                float num3 = Vector3.Distance(spawnpoint2.WorldPt, worldPt);
                if (num3 < maxValue)
                {
                    spawnpoint = spawnpoint2;
                    maxValue = num3;
                }
            }
            return spawnpoint;
        }

        public double getCumulativeCurrentBossHp()
        {
            double num = 0.0;
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance instance = this.ActiveCharacters[i];
                if (instance.IsBoss && !instance.IsDead)
                {
                    num += instance.CurrentHp;
                }
            }
            return num;
        }

        public double getCumulativeMaxBossHp()
        {
            double num = 0.0;
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance instance = this.ActiveCharacters[i];
                if (instance.IsBoss && !instance.IsDead)
                {
                    num += instance.MaxLife(true);
                }
            }
            return num;
        }

        public float getCumulativeNormalizedBossHp()
        {
            double num = this.getCumulativeMaxBossHp();
            if (num <= 0.0)
            {
                return 0f;
            }
            return Mathf.Clamp01((float) (this.getCumulativeCurrentBossHp() / num));
        }

        public List<MarkerSpawnPointDeco> getDecoSpawnpointMarkersForIsland(Spawnpoint islandSpawnpoint)
        {
            return this.m_islandToDecoMap[islandSpawnpoint];
        }

        public List<CharacterInstance> getEnemyCharactersAlongLineSegment(Ray ray, float distance, CharacterInstance referenceCharacter)
        {
            List<CharacterInstance> list = new List<CharacterInstance>();
            RaycastHit[] hitArray = Physics.SphereCastAll(ray, 1f, distance, Layers.AllCharactersLayerMask);
            if (hitArray.Length <= 0)
            {
                return null;
            }
            for (int i = 0; i < hitArray.Length; i++)
            {
                RaycastHit hit = hitArray[i];
                PhysicsBody component = hit.transform.GetComponent<PhysicsBody>();
                if (component != null)
                {
                    CharacterInstance attachedCharacter = component.AttachedCharacter;
                    if ((((attachedCharacter != referenceCharacter) && !attachedCharacter.IsDead) && !referenceCharacter.isFriendlyTowards(attachedCharacter)) && !attachedCharacter.isInvisible())
                    {
                        list.Add(component.AttachedCharacter);
                    }
                }
            }
            return list;
        }

        public List<CharacterInstance> getEnemyCharactersWithinRadius(Vector3 worldPt, float radius, CharacterInstance referenceCharacter)
        {
            List<CharacterInstance> list = new List<CharacterInstance>();
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance another = this.ActiveCharacters[i];
                if (((((another != referenceCharacter) && !another.IsDead) && !referenceCharacter.isFriendlyTowards(another)) && !another.isInvisible()) && (Vector3.Distance(Vector3Extensions.ToXzVector3(worldPt), Vector3Extensions.ToXzVector3(another.PhysicsBody.Transform.position)) <= (radius + another.Radius)))
                {
                    list.Add(another);
                }
            }
            return list;
        }

        public List<CharacterInstance> getEnemyCharactersWithinRadiusSortedByDistance(Vector3 worldPt, float radius, CharacterInstance referenceCharacter)
        {
            <getEnemyCharactersWithinRadiusSortedByDistance>c__AnonStorey2E7 storeye = new <getEnemyCharactersWithinRadiusSortedByDistance>c__AnonStorey2E7();
            storeye.referenceCharacter = referenceCharacter;
            List<CharacterInstance> list = this.getEnemyCharactersWithinRadius(worldPt, radius, storeye.referenceCharacter);
            list.Sort(new Comparison<CharacterInstance>(storeye.<>m__182));
            return list;
        }

        public CharacterInstance getEnemyCharacterWithHighestThreat(CharacterInstance referenceCharacter, bool lineOfSight)
        {
            CharacterInstance instance = null;
            float minValue = float.MinValue;
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance another = this.ActiveCharacters[i];
                if (((((another != referenceCharacter) && !another.IsDead) && !referenceCharacter.isFriendlyTowards(another)) && !another.isInvisible()) && ((!lineOfSight || this.lineOfSightCharacterToCharacter(referenceCharacter, another)) && (another.Threat(true) > minValue)))
                {
                    instance = another;
                    minValue = another.Threat(true);
                }
            }
            return instance;
        }

        public Spawnpoint getFarthestCharacterSpawnpoint(CharacterInstance refCharacter)
        {
            Spawnpoint spawnpoint = null;
            float minValue = float.MinValue;
            for (int i = 0; i < this.CharacterSpawnpoints.Count; i++)
            {
                Spawnpoint spawnpoint2 = this.CharacterSpawnpoints[i];
                float num3 = Vector3.Distance(spawnpoint2.WorldPt, refCharacter.PhysicsBody.Transform.position);
                if (num3 > minValue)
                {
                    spawnpoint = spawnpoint2;
                    minValue = num3;
                }
            }
            return spawnpoint;
        }

        public CharacterInstance getFarthestEnemyCharacter(CharacterInstance referenceCharacter, bool lineOfSight)
        {
            Vector3 position = referenceCharacter.PhysicsBody.Transform.position;
            CharacterInstance instance = null;
            float minValue = float.MinValue;
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance another = this.ActiveCharacters[i];
                if (((((another != referenceCharacter) && !another.IsDead) && !referenceCharacter.isFriendlyTowards(another)) && !another.isInvisible()) && (!lineOfSight || this.lineOfSightCharacterToCharacter(referenceCharacter, another)))
                {
                    float num3 = Vector3.Distance(position, another.PhysicsBody.Transform.position) - another.Radius;
                    if (num3 > minValue)
                    {
                        instance = another;
                        minValue = num3;
                    }
                }
            }
            return instance;
        }

        public CharacterInstance getFirstAliveBoss()
        {
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance instance = this.ActiveCharacters[i];
                if (instance.IsBoss && !instance.IsDead)
                {
                    return instance;
                }
            }
            return null;
        }

        public CharacterInstance getFriendlyCharacterOfType(CharacterInstance referenceCharacter, CharacterPrefab prefabType)
        {
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance another = this.ActiveCharacters[i];
                if ((((another != referenceCharacter) && !another.IsDead) && (another.Prefab == prefabType)) && referenceCharacter.isFriendlyTowards(another))
                {
                    return another;
                }
            }
            return null;
        }

        public Spawnpoint getNextCharacterSpawnpoint(int currentIdx)
        {
            return this.CharacterSpawnpoints[this.getNextCharacterSpawnpointIndex(currentIdx)];
        }

        public int getNextCharacterSpawnpointIndex(int currentIdx)
        {
            return ((currentIdx + 1) % this.CharacterSpawnpoints.Count);
        }

        public int getNumSupportCharacters()
        {
            int num = 0;
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance instance = this.ActiveCharacters[i];
                if (!instance.IsDead && instance.IsSupport)
                {
                    num++;
                }
            }
            return num;
        }

        public CharacterInstance getRandomEnemyCharacterWithinRadius(Vector3 worldPt, float radius, CharacterInstance referenceCharacter, bool includeBosses)
        {
            sm_tempCandidateList.Clear();
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance another = this.ActiveCharacters[i];
                if (((((another != referenceCharacter) && !another.IsDead) && !referenceCharacter.isFriendlyTowards(another)) && !another.isInvisible()) && ((includeBosses || !another.IsBoss) && (Vector3.Distance(Vector3Extensions.ToXzVector3(worldPt), Vector3Extensions.ToXzVector3(another.PhysicsBody.Transform.position)) <= (radius + another.Radius))))
                {
                    sm_tempCandidateList.Add(another);
                }
            }
            if (sm_tempCandidateList.Count > 0)
            {
                return LangUtil.GetRandomValueFromList<CharacterInstance>(sm_tempCandidateList);
            }
            return null;
        }

        public bool lineOfSightCharacterToCharacter(CharacterInstance fromCharacter, CharacterInstance toCharacter)
        {
            RaycastHit hit;
            Vector3 direction = toCharacter.PhysicsBody.Transform.position - fromCharacter.PhysicsBody.Transform.position;
            Ray ray = new Ray(fromCharacter.PhysicsBody.Transform.position, direction);
            return (!Physics.Raycast(ray, out hit, direction.magnitude, Layers.LineOfSightLayerMask) || (hit.collider == toCharacter.PhysicsBody.CharacterController));
        }

        public int numberOfBossesAlive()
        {
            int num = 0;
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance instance = this.ActiveCharacters[i];
                if (instance.IsBoss && !instance.IsDead)
                {
                    num++;
                }
            }
            return num;
        }

        public int numberOfCharactersAlive()
        {
            int num = 0;
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance instance = this.ActiveCharacters[i];
                if (!instance.IsDead)
                {
                    num++;
                }
            }
            return num;
        }

        public int numberOfDecoyClonesAlive()
        {
            int num = 0;
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance instance = this.ActiveCharacters[i];
                if (!instance.IsDead && instance.isDecoyClone())
                {
                    num++;
                }
            }
            return num;
        }

        public int numberOfFriendlyCharactersAlive(CharacterInstance referenceCharacter, bool includeBosses)
        {
            int num = 0;
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance another = this.ActiveCharacters[i];
                if ((((another != referenceCharacter) && !another.IsDead) && referenceCharacter.isFriendlyTowards(another)) && (includeBosses || !another.IsBoss))
                {
                    num++;
                }
            }
            return num;
        }

        public int numberOfWildBossesAlive()
        {
            int num = 0;
            for (int i = 0; i < this.ActiveCharacters.Count; i++)
            {
                CharacterInstance instance = this.ActiveCharacters[i];
                if (instance.IsWildBoss && !instance.IsDead)
                {
                    num++;
                }
            }
            return num;
        }

        [CompilerGenerated]
        private sealed class <getEnemyCharactersWithinRadiusSortedByDistance>c__AnonStorey2E7
        {
            internal CharacterInstance referenceCharacter;

            internal int <>m__182(CharacterInstance c1, CharacterInstance c2)
            {
                float num = PhysicsUtil.DistBetween(this.referenceCharacter, c1);
                float num2 = PhysicsUtil.DistBetween(this.referenceCharacter, c2);
                if (num < num2)
                {
                    return -1;
                }
                if (num > num2)
                {
                    return 1;
                }
                return 0;
            }
        }

        public enum BossSummonMethod
        {
            UNSPECIFIED,
            Player,
            Frenzy,
            BossTrain,
            AutoSummonCompletedBossesToggle,
            WildBoss
        }

        public class Spawnpoint
        {
            public Vector3 WorldPt;
            public Quaternion WorldRot;
        }
    }
}

