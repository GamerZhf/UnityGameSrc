namespace App
{
    using AiView;
    using GameLogic;
    using PlayerView;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class ConfigGameplay
    {
        public static float AGGRO_RANGE_ALARMED;
        public static float AGGRO_RANGE_IDLE;
        public static float AI_UPDATE_INTERVAL;
        public static List<BaseStatProperty> ALL_BASE_STAT_PROPERTY_TYPES = LangUtil.GetEnumValuesWithException<BaseStatProperty>(BaseStatProperty.UNSPECIFIED);
        public static List<GameLogic.CharacterType> ALL_CHARACTER_TYPES = LangUtil.GetEnumValuesWithException<GameLogic.CharacterType>(GameLogic.CharacterType.UNSPECIFIED);
        public static List<GameLogic.CharacterType> ALL_CHARACTER_TYPES_INCLUDING_UNSPECIFIED = LangUtil.GetEnumValues<GameLogic.CharacterType>();
        public static float ATTACKS_PER_SECOND_CAP = 6f;
        public static float BLINK_DISTANCE_THRESHOLD = 12f;
        public static float BOSS_DELAY_BETWEEN_SHOWERS = 0.3f;
        public const string BOSS_DIFFICULTY_ID_HARD = "hard";
        public const string BOSS_DIFFICULTY_ID_IMPOSSIBLE = "impossible";
        public const string BOSS_DIFFICULTY_ID_READY = "ready";
        public const string BOSS_DIFFICULTY_ID_VERY_HARD = "very_hard";
        public static float BOSS_SPAWN_POINT_MIN_DISTANCE = 10f;
        public static float BUFF_VIEW_SCALE_MODIFIER_ENTRY_DURATION = 0.5f;
        public static float BUFFS_ATTACKS_PER_SECOND_DEBUFF_TOTAL_MODIFIER_CAP = -0.9f;
        public static float BUFFS_MOVEMENT_DEBUFF_TOTAL_MODIFIER_CAP = -0.9f;
        public static float BUFFS_TICK_INTERVAL = 0.5f;
        public static Easing.Function CHARACTER_FULLSPEED_ACCELERATION_EASING;
        public static float CHARACTER_FULLSPEED_ACCELERATION_TIMER;
        public static float CHARACTER_MELEE_ATTACK_DESTRUCTIBLE_HIT_RADIUS;
        public static Dictionary<GameLogic.CharacterType, Dictionary<string, int>> CHARACTER_SPAWN_WEIGHTS;
        public static Color CLONE_COOLDOWN_MATERIAL_COLOR;
        public static Color CLONE_DECOY_MATERIAL_COLOR;
        public static Color CLONE_HEAL_MATERIAL_COLOR;
        public static int CRITTER_MAX_COUNT;
        public static float DEATH_ENTRY_DURATION = 0.5f;
        public static float DEATH_REMAIN_DURATION = 5f;
        public static float DEFAULT_NORMALIZED_HP_AFTER_REVIVE;
        public static float GLOBAL_CAP_ARMOR_BLOCK_MODIFIER = 0.33f;
        public static float GLOBAL_CAP_CRITICAL_HIT_CHANCE_PCT = 0.5f;
        public static float GLOBAL_MELEE_DAMAGE_VARIATION_PCT;
        public static float HERO_HP_SAFEGUARD_MAX_HP_LOSS_PER_MINION_HIT_NORMALIZED;
        public static float HERO_SKILL_COOLDOWN_MIN = 2f;
        public static string LAYOUT_ROOT = "LayoutRoot";
        public static float MAX_MONSER_PURSUIT_PATH_LENGTH;
        public static MinMaxInt MOB_SPAWNCOUNT_PER_SPAWNPOINT_MAX_RANGE = new MinMaxInt(6, 8);
        public static MinMaxInt MOB_SPAWNCOUNT_PER_SPAWNPOINT_MIN_RANGE = new MinMaxInt(3, 5);
        public static bool OUT_OF_BOUNDS_CHECKING_ENABLED;
        public static float PASSIVE_HP_REGEN_PCT_PER_TICK;
        public static float PASSIVE_HP_REGEN_PROXIMITY_THRESHOLD;
        public static float PASSIVE_HP_REGEN_TICK_INTERVAL;
        public static int PET_MAX_LEVEL;
        public static List<int> PET_PERK_MILESTONE_LEVELS;
        public static Color POISON_MATERIAL_COLOR;
        public static Dictionary<ProjectileType, Projectile.ProjectileProperties> PROJECTILE_PROTOTYPE_PROPERTIES;
        public static bool RAGDOLL_DEATHS_ENABLED = false;
        public static float ROTATION_STEERING_THRESHOLD;
        public static Color SLOW_MATERIAL_COLOR;
        public static float SPAWN_CHANCE_FROM_PRIMARY_MINION_POOL = 0.625f;
        public static float SPAWN_ENTRY_DURATION = 0.5f;
        public static float SPAWN_ENTRY_START_SCALE = 0.5f;
        public static string SPAWNPOINT_ROOT = "SpawnpointRoot";
        public static string SPURT_BUFF_ID = "HERO_SPURT_BUFF";
        public static float SPURTING_BUFF_DURATION_SECONDS = 3.5f;
        public static float SPURTING_BUFF_MOVEMENT_BONUS = 0.13f;
        public static bool SPURTING_ENABLED = true;
        public static int SPURTING_MAX_NUM_BUFFS = 7;
        public static Color STUN_MATERIAL_COLOR;
        public static float SUPPORT_CHARACTER_AUTO_KILL_RADIUS;
        public static bool USE_RANDOMIZED_BOSS_PERKS;

        static ConfigGameplay()
        {
            Dictionary<GameLogic.CharacterType, Dictionary<string, int>> dictionary = new Dictionary<GameLogic.CharacterType, Dictionary<string, int>>(new CharacterTypeBoxAvoidanceComparer());
            Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
            dictionary2.Add("Skeleton001", 10);
            dictionary2.Add("Skeleton002", 10);
            dictionary2.Add("Skeleton003", 10);
            dictionary2.Add("Skeleton004", 10);
            dictionary2.Add("Skeleton005", 10);
            dictionary2.Add("Skeleton006", 10);
            dictionary2.Add("Skeleton007", 8);
            dictionary2.Add("Skeleton008", 6);
            dictionary.Add(GameLogic.CharacterType.Skeleton, dictionary2);
            dictionary2 = new Dictionary<string, int>();
            dictionary2.Add("Jelly001", 10);
            dictionary2.Add("Jelly002", 10);
            dictionary2.Add("Jelly003", 10);
            dictionary2.Add("Jelly004", 6);
            dictionary.Add(GameLogic.CharacterType.Jelly, dictionary2);
            dictionary2 = new Dictionary<string, int>();
            dictionary2.Add("Pygmy001", 10);
            dictionary2.Add("Pygmy002", 10);
            dictionary2.Add("Pygmy003", 10);
            dictionary2.Add("Pygmy004", 10);
            dictionary.Add(GameLogic.CharacterType.Pygmy, dictionary2);
            dictionary2 = new Dictionary<string, int>();
            dictionary2.Add("Worg001", 10);
            dictionary2.Add("Worg002", 10);
            dictionary2.Add("Worg003", 0);
            dictionary2.Add("Worg004", 0);
            dictionary.Add(GameLogic.CharacterType.Worg, dictionary2);
            dictionary2 = new Dictionary<string, int>();
            dictionary2.Add("Crocodile001", 10);
            dictionary2.Add("Crocodile002", 10);
            dictionary2.Add("Crocodile003", 10);
            dictionary.Add(GameLogic.CharacterType.Crocodile, dictionary2);
            dictionary2 = new Dictionary<string, int>();
            dictionary2.Add("Rat001", 10);
            dictionary2.Add("Rat002", 10);
            dictionary2.Add("Rat003", 10);
            dictionary.Add(GameLogic.CharacterType.Rat, dictionary2);
            dictionary2 = new Dictionary<string, int>();
            dictionary2.Add("Anubis001", 10);
            dictionary2.Add("Anubis002", 10);
            dictionary2.Add("Anubis003", 10);
            dictionary.Add(GameLogic.CharacterType.Anubis, dictionary2);
            dictionary2 = new Dictionary<string, int>();
            dictionary2.Add("Mummy001", 10);
            dictionary2.Add("Mummy002", 10);
            dictionary.Add(GameLogic.CharacterType.Mummy, dictionary2);
            dictionary2 = new Dictionary<string, int>();
            dictionary2.Add("Goblin001", 10);
            dictionary2.Add("Goblin002", 10);
            dictionary2.Add("Goblin003", 10);
            dictionary2.Add("Goblin004", 10);
            dictionary.Add(GameLogic.CharacterType.Goblin, dictionary2);
            dictionary2 = new Dictionary<string, int>();
            dictionary2.Add("Yeti001", 10);
            dictionary2.Add("Yeti002", 10);
            dictionary2.Add("Yeti003", 10);
            dictionary2.Add("Yeti004", 10);
            dictionary.Add(GameLogic.CharacterType.Yeti, dictionary2);
            dictionary2 = new Dictionary<string, int>();
            dictionary2.Add("Shroom001", 10);
            dictionary2.Add("Shroom002", 10);
            dictionary2.Add("Shroom003", 10);
            dictionary2.Add("Shroom004", 10);
            dictionary.Add(GameLogic.CharacterType.Shroom, dictionary2);
            CHARACTER_SPAWN_WEIGHTS = dictionary;
            STUN_MATERIAL_COLOR = new Color(0.08235294f, 0.8470588f, 1f, 1f);
            POISON_MATERIAL_COLOR = new Color(0.6196079f, 0.972549f, 0f, 1f);
            SLOW_MATERIAL_COLOR = new Color(0f, 1f, 1f, 1f);
            CLONE_COOLDOWN_MATERIAL_COLOR = new Color(0.3882353f, 0.8980392f, 1f, 1f);
            CLONE_DECOY_MATERIAL_COLOR = new Color(0.6156863f, 0f, 1f, 1f);
            CLONE_HEAL_MATERIAL_COLOR = new Color(0.4431373f, 1f, 0.2666667f, 1f);
            DEFAULT_NORMALIZED_HP_AFTER_REVIVE = 0.5f;
            PASSIVE_HP_REGEN_PROXIMITY_THRESHOLD = 9f;
            PASSIVE_HP_REGEN_TICK_INTERVAL = 0.2f;
            PASSIVE_HP_REGEN_PCT_PER_TICK = 0.2f;
            HERO_HP_SAFEGUARD_MAX_HP_LOSS_PER_MINION_HIT_NORMALIZED = 0.1f;
            GLOBAL_MELEE_DAMAGE_VARIATION_PCT = 0.03f;
            OUT_OF_BOUNDS_CHECKING_ENABLED = true;
            CHARACTER_FULLSPEED_ACCELERATION_TIMER = 0.18f;
            CHARACTER_FULLSPEED_ACCELERATION_EASING = Easing.Function.IN_CUBIC;
            ROTATION_STEERING_THRESHOLD = 0.9f;
            SUPPORT_CHARACTER_AUTO_KILL_RADIUS = float.MaxValue;
            CRITTER_MAX_COUNT = 8;
            AGGRO_RANGE_IDLE = 10f;
            AGGRO_RANGE_ALARMED = 5f;
            AI_UPDATE_INTERVAL = 0.0666666f;
            MAX_MONSER_PURSUIT_PATH_LENGTH = 20f;
            Dictionary<ProjectileType, Projectile.ProjectileProperties> dictionary3 = new Dictionary<ProjectileType, Projectile.ProjectileProperties>(new ProjectileTypeBoxAvoidanceComparer());
            Projectile.ProjectileProperties properties = new Projectile.ProjectileProperties();
            properties.Type = ProjectileType.Fireball;
            properties.Radius = 0.6f;
            properties.DestroyAfterContact = true;
            properties.MaxLifetime = 3f;
            properties.Speed = 8f;
            properties.DamageType = DamageType.Ranged;
            dictionary3.Add(ProjectileType.Fireball, properties);
            Projectile.ProjectileProperties properties2 = new Projectile.ProjectileProperties();
            properties2.Type = ProjectileType.Orb;
            properties2.Radius = 0.6f;
            properties2.DestroyAfterContact = true;
            properties2.MaxLifetime = 2f;
            properties2.Speed = 14f;
            properties2.DamageType = DamageType.Magic;
            dictionary3.Add(ProjectileType.Orb, properties2);
            Projectile.ProjectileProperties properties3 = new Projectile.ProjectileProperties();
            properties3.Type = ProjectileType.Dragonball;
            properties3.Radius = 0.6f;
            properties3.DestroyAfterContact = true;
            properties3.MaxLifetime = 3f;
            properties3.Speed = 10f;
            properties3.DamageType = DamageType.Ranged;
            dictionary3.Add(ProjectileType.Dragonball, properties3);
            Projectile.ProjectileProperties properties4 = new Projectile.ProjectileProperties();
            properties4.Type = ProjectileType.Rock;
            properties4.Radius = 0.6f;
            properties4.DestroyAfterContact = true;
            properties4.MaxLifetime = 3f;
            properties4.CollideWithGround = true;
            properties4.Speed = 8f;
            properties4.DamageType = DamageType.Ranged;
            dictionary3.Add(ProjectileType.Rock, properties4);
            Projectile.ProjectileProperties properties5 = new Projectile.ProjectileProperties();
            properties5.Type = ProjectileType.Frostball;
            properties5.Radius = 0.6f;
            properties5.DestroyAfterContact = true;
            properties5.MaxLifetime = 3f;
            properties5.Speed = 10f;
            properties5.DamageType = DamageType.Ranged;
            dictionary3.Add(ProjectileType.Frostball, properties5);
            Projectile.ProjectileProperties properties6 = new Projectile.ProjectileProperties();
            properties6.Type = ProjectileType.Bubbles;
            properties6.Radius = 0.6f;
            properties6.DestroyAfterContact = true;
            properties6.MaxLifetime = 3f;
            properties6.Speed = 10f;
            properties6.DamageType = DamageType.Ranged;
            dictionary3.Add(ProjectileType.Bubbles, properties6);
            Projectile.ProjectileProperties properties7 = new Projectile.ProjectileProperties();
            properties7.Type = ProjectileType.Splinters1;
            properties7.Radius = 0.6f;
            properties7.DestroyAfterContact = true;
            properties7.MaxLifetime = 3f;
            properties7.Speed = 10f;
            properties7.DamageType = DamageType.Ranged;
            dictionary3.Add(ProjectileType.Splinters1, properties7);
            Projectile.ProjectileProperties properties8 = new Projectile.ProjectileProperties();
            properties8.Type = ProjectileType.Splinters2;
            properties8.Radius = 0.6f;
            properties8.DestroyAfterContact = true;
            properties8.MaxLifetime = 3f;
            properties8.Speed = 10f;
            properties8.DamageType = DamageType.Ranged;
            dictionary3.Add(ProjectileType.Splinters2, properties8);
            Projectile.ProjectileProperties properties9 = new Projectile.ProjectileProperties();
            properties9.Type = ProjectileType.Ink;
            properties9.Radius = 0.6f;
            properties9.DestroyAfterContact = true;
            properties9.MaxLifetime = 3f;
            properties9.Speed = 10f;
            properties9.DamageType = DamageType.Ranged;
            dictionary3.Add(ProjectileType.Ink, properties9);
            Projectile.ProjectileProperties properties10 = new Projectile.ProjectileProperties();
            properties10.Type = ProjectileType.Coins;
            properties10.Radius = 0.6f;
            properties10.DestroyAfterContact = true;
            properties10.MaxLifetime = 3f;
            properties10.Speed = 10f;
            properties10.DamageType = DamageType.Ranged;
            dictionary3.Add(ProjectileType.Coins, properties10);
            PROJECTILE_PROTOTYPE_PROPERTIES = dictionary3;
            USE_RANDOMIZED_BOSS_PERKS = false;
            PET_MAX_LEVEL = 5;
            List<int> list = new List<int>();
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            PET_PERK_MILESTONE_LEVELS = list;
            CHARACTER_MELEE_ATTACK_DESTRUCTIBLE_HIT_RADIUS = 2.25f;
        }

        public static string CalculateBossDifficulty(Player player, int floor, bool isEliteBoss, Character referenceBoss, double progressDifficultyExponent)
        {
            CharacterInstance activeCharacter = player.ActiveCharacter;
            double num = activeCharacter.DamagePerHit(false) * activeCharacter.AttacksPerSecond(false);
            double num2 = activeCharacter.MaxLife(false);
            double num3 = activeCharacter.SkillDamage(false);
            double num4 = App.Binder.ConfigMeta.DIFFICULTY_AVERAGE_SKILL_DAMAGE_MULTIPLIER;
            double num5 = (num3 * activeCharacter.ActiveSkillTypes.Count) * num4;
            float num6 = activeCharacter.getTotalNumberOfRandomPerksInEquippedItems() * 0.02f;
            float num7 = activeCharacter.getTotalNumberOfFixedPerksInEquippedItems() * 0.15f;
            num *= 1.0 + ((num6 + num7) * 0.5);
            num2 *= 1.0 + ((num6 + num7) * 0.5);
            double num8 = App.Binder.ConfigMeta.BossDamagePerHitCurve(floor, progressDifficultyExponent, false) * referenceBoss.getBaseStatFloat(BaseStatProperty.AttacksPerSecond);
            double num9 = App.Binder.ConfigMeta.BossLifeCurve(floor, progressDifficultyExponent, false);
            double num10 = !isEliteBoss ? 1.0 : 1.4;
            num8 *= num10;
            num9 *= num10;
            num9 -= num5;
            double num11 = (num9 / num) / (num2 / num8);
            double num12 = App.Binder.ConfigMeta.ITEM_DAMAGE_MULTIPLIER * 3.0;
            double num13 = App.Binder.ConfigMeta.ITEM_HEALTH_MULTIPLIER;
            double num14 = App.Binder.ConfigMeta.BOSS_DAMAGE_MULTIPLIER * 1.0;
            double num16 = (App.Binder.ConfigMeta.BOSS_HEALTH_MULTIPLIER / num12) / (num13 / num14);
            double num17 = num11 / num16;
            if (num17 >= 4.5)
            {
                return "impossible";
            }
            if (num17 >= 2.0)
            {
                return "very_hard";
            }
            if (num17 >= 1.33)
            {
                return "hard";
            }
            return "ready";
        }

        public static string GetBossAiDescription(AiBehaviourType behaviour, [Optional, DefaultParameterValue(null)] string[] parameters)
        {
            AiBehaviourType type = behaviour;
            if (type == AiBehaviourType.BossSummoner)
            {
                return BossSummonerAiBehaviour.GetDescription(parameters);
            }
            if (type != AiBehaviourType.BossCaster)
            {
                return null;
            }
            return BossCasterAiBehaviour.GetDescription(parameters);
        }

        public static float GetCharacterVisualScale(CharacterInstance character)
        {
            if (!character.IsBoss)
            {
                return 1f;
            }
            float num = 1f;
            if (character.Type == GameLogic.CharacterType.Worg)
            {
                num = 1.5f;
            }
            else
            {
                num = 1.75f;
            }
            if (character.IsEliteBoss)
            {
                num *= 1.05f;
            }
            return num;
        }

        public static void GetRangedCharacterProjectileSetup(CharacterInstance sourceCharacter, Vector3 targetWorldPt, out Vector3 projectileSpawnPt, out Vector3 projectileTargetPt, out float projectileCurveHeight)
        {
            Vector3 localPosition;
            CharacterView view = PlayerView.Binder.RoomView.getCharacterViewForCharacter(sourceCharacter);
            Vector3 normalized = Vector3Extensions.ToXzVector3(targetWorldPt - sourceCharacter.PhysicsBody.Transform.position).normalized;
            if (view.ProjectileSpawnpoint != null)
            {
                localPosition = view.ProjectileSpawnpoint.localPosition;
            }
            else
            {
                localPosition = new Vector3(0f, 1.5f, 1.5f);
            }
            localPosition = (Vector3) (localPosition * GetCharacterVisualScale(sourceCharacter));
            projectileSpawnPt = ((sourceCharacter.PhysicsBody.Transform.position + normalized) + sourceCharacter.PhysicsBody.Transform.TransformVector(new Vector3(0f, 0f, -1f))) + sourceCharacter.PhysicsBody.Transform.TransformVector(localPosition);
            projectileTargetPt = new Vector3(targetWorldPt.x, 1.5f, targetWorldPt.z);
            if (sourceCharacter.Type == GameLogic.CharacterType.Yeti)
            {
                projectileCurveHeight = 3.5f;
            }
            else
            {
                projectileCurveHeight = 0f;
            }
        }
    }
}

