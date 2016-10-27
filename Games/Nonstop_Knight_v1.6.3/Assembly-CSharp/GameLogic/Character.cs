namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Character : IJsonData
    {
        [CompilerGenerated]
        private SpriteAtlasEntry <AvatarSprite>k__BackingField;
        public float AttackContactTimeNormalized;
        public string AvatarSpriteId = string.Empty;
        public Dictionary<string, double> BaseStats = new Dictionary<string, double>();
        [JsonIgnore]
        public Dictionary<BaseStatProperty, double> BaseStatsDouble = new Dictionary<BaseStatProperty, double>(new BaseStatPropertyBoxAvoidanceComparer());
        [JsonIgnore]
        public Dictionary<BaseStatProperty, float> BaseStatsFloat = new Dictionary<BaseStatProperty, float>(new BaseStatPropertyBoxAvoidanceComparer());
        public AiBehaviourType BossAiBehaviour;
        public string[] BossAiParameters;
        public PerkType BossPerk;
        public AiBehaviourType CoreAiBehaviour;
        public CurveType DamageCurve;
        public GatedPerkContainer FixedPerks;
        public string FlavorText = string.Empty;
        public string Id = string.Empty;
        public CurveType LifeCurve;
        public float LimitedLifetimeSeconds;
        public float MainHeroDamagePerHitPct;
        public string Name = string.Empty;
        public CharacterPrefab Prefab;
        public float Radius;
        public ProjectileType RangedProjectileType;
        public int Rarity;
        public float RotationSpeed = 10f;
        public CharacterType Type;

        public double getBaseStatDouble(BaseStatProperty prop)
        {
            return this.BaseStatsDouble[prop];
        }

        public float getBaseStatFloat(BaseStatProperty prop)
        {
            return this.BaseStatsFloat[prop];
        }

        public void postDeserializeInitialization()
        {
            this.BaseStatsDouble.Clear();
            foreach (KeyValuePair<string, double> pair in this.BaseStats)
            {
                BaseStatProperty key = (BaseStatProperty) ((int) Enum.Parse(typeof(BaseStatProperty), pair.Key));
                switch (key)
                {
                    case BaseStatProperty.UNSPECIFIED:
                        break;

                    case BaseStatProperty.DamagePerHit:
                    case BaseStatProperty.Life:
                    case BaseStatProperty.SkillDamage:
                        this.BaseStatsDouble.Add(key, pair.Value);
                        break;

                    default:
                        this.BaseStatsFloat.Add(key, (float) pair.Value);
                        break;
                }
            }
            if (!string.IsNullOrEmpty(this.AvatarSpriteId))
            {
                this.AvatarSprite = new SpriteAtlasEntry("Menu", this.AvatarSpriteId);
            }
        }

        [JsonIgnore]
        public SpriteAtlasEntry AvatarSprite
        {
            [CompilerGenerated]
            get
            {
                return this.<AvatarSprite>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<AvatarSprite>k__BackingField = value;
            }
        }
    }
}

