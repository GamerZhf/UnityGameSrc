namespace App
{
    using AiView;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class ConfigSkills
    {
        public static List<SkillType> ACTIVE_HERO_SKILLS;
        public static List<SkillType> ACTIVE_HERO_SKILLS_SORTED_BY_GROUP;
        public static List<SkillType> ALL_HERO_SKILLS;
        public static Dictionary<SkillType, BossSummonerSkillSharedData> BOSS_SUMMONER_SKILLS;
        public static float CooldownHigh;
        public static float CooldownLow;
        public static float CooldownMed;
        public static List<SkillType> PASSIVE_HERO_SKILLS_SORTED_BY_GROUP;
        public static Dictionary<SkillType, SharedData> SHARED_DATA;
        public static int SkillGroupCount;

        static ConfigSkills()
        {
            List<SkillType> list = new List<SkillType>();
            list.Add(SkillType.Whirlwind);
            list.Add(SkillType.Leap);
            list.Add(SkillType.Clone);
            list.Add(SkillType.Slam);
            list.Add(SkillType.Omnislash);
            list.Add(SkillType.Implosion);
            ACTIVE_HERO_SKILLS = list;
            ACTIVE_HERO_SKILLS_SORTED_BY_GROUP = null;
            PASSIVE_HERO_SKILLS_SORTED_BY_GROUP = null;
            ALL_HERO_SKILLS = new List<SkillType>(ACTIVE_HERO_SKILLS);
            CooldownLow = 6f;
            CooldownMed = 10f;
            CooldownHigh = 15f;
            SkillGroupCount = 3;
            Dictionary<SkillType, SharedData> dictionary = new Dictionary<SkillType, SharedData>(new SkillTypeBoxAvoidanceComparer());
            SharedData data = new SharedData();
            data.UnlockRank = 1;
            data.Cooldown = CooldownLow;
            data.BossCooldown = 2.5f;
            data.BossBuildupTime = 0.75f;
            data.BossFirstCastDelay = 1.5f;
            data.Name = ConfigLoca.SKILL_WHIRLWIND_NAME;
            data.Sprite = "sprite_skill001";
            data.Spritesheet = "DungeonHud";
            data.ExternalControl = true;
            data.Description = ConfigLoca.SKILL_WHIRLWIND_DESCRIPTION;
            dictionary.Add(SkillType.Whirlwind, data);
            data = new SharedData();
            data.UnlockRank = 1;
            data.Group = 1;
            data.Cooldown = CooldownMed;
            data.BossCooldown = 2.5f;
            data.BossBuildupTime = 0.75f;
            data.BossFirstCastDelay = 2f;
            data.Invulnerability = true;
            data.Name = ConfigLoca.SKILL_LEAP_NAME;
            data.Sprite = "sprite_skill002";
            data.Spritesheet = "DungeonHud";
            data.ExternalControl = true;
            data.Description = ConfigLoca.SKILL_LEAP_DESCRIPTION;
            dictionary.Add(SkillType.Leap, data);
            data = new SharedData();
            data.UnlockRank = 2;
            data.Group = 2;
            data.Cooldown = CooldownHigh;
            data.BossCooldown = 2.5f;
            data.BossBuildupTime = 0.75f;
            data.BossFirstCastDelay = 1.5f;
            data.Name = ConfigLoca.SKILL_CLONE_NAME;
            data.Sprite = "sprite_skill003";
            data.Spritesheet = "DungeonHud";
            data.ExternalControl = true;
            data.Description = ConfigLoca.SKILL_CLONE_DESCRIPTION;
            dictionary.Add(SkillType.Clone, data);
            data = new SharedData();
            data.UnlockRank = 10;
            data.Cooldown = 8f;
            data.BossCooldown = 2.5f;
            data.BuildupTime = 0.3f;
            data.BossBuildupTime = 0.75f;
            data.BossFirstCastDelay = 1.5f;
            data.Name = ConfigLoca.SKILL_SLAM_NAME;
            data.Sprite = "sprite_skill015";
            data.Spritesheet = "DungeonHud";
            data.ExternalControl = true;
            data.Description = ConfigLoca.SKILL_SLAM_DESCRIPTION;
            dictionary.Add(SkillType.Slam, data);
            data = new SharedData();
            data.UnlockRank = 0x12;
            data.Group = 1;
            data.Cooldown = CooldownMed;
            data.BossCooldown = 6f;
            data.BossBuildupTime = 0.75f;
            data.BossFirstCastDelay = 1.5f;
            data.Invulnerability = true;
            data.Name = ConfigLoca.SKILL_OMNISLASH_NAME;
            data.Sprite = "sprite_skill014";
            data.Spritesheet = "DungeonHud";
            data.ExternalControl = true;
            data.Description = ConfigLoca.SKILL_OMNISLASH_DESCRIPTION;
            dictionary.Add(SkillType.Omnislash, data);
            data = new SharedData();
            data.UnlockRank = 0x1b;
            data.Group = 2;
            data.Cooldown = CooldownHigh;
            data.BossCooldown = 2.5f;
            data.BossBuildupTime = 0.75f;
            data.BossFirstCastDelay = 1.5f;
            data.Name = ConfigLoca.SKILL_IMPLOSION_NAME;
            data.Sprite = "sprite_skill016";
            data.Spritesheet = "DungeonHud";
            data.ExternalControl = true;
            data.Description = ConfigLoca.SKILL_IMPLOSION_DESCRIPTION;
            dictionary.Add(SkillType.Implosion, data);
            data = new SharedData();
            data.Invulnerability = true;
            data.ExternalControl = true;
            data.Name = "Charge";
            dictionary.Add(SkillType.Charge, data);
            data = new SharedData();
            data.BossBuildupTime = 0.5f;
            data.ExternalControl = true;
            dictionary.Add(SkillType.BossSummoner, data);
            data = new SharedData();
            data.BossBuildupTime = 0.5f;
            data.ExternalControl = true;
            dictionary.Add(SkillType.BossLeader, data);
            data = new SharedData();
            data.BossBuildupTime = 0.5f;
            dictionary.Add(SkillType.BossBreeder, data);
            data = new SharedData();
            data.BossBuildupTime = 0.5f;
            data.ExternalControl = true;
            dictionary.Add(SkillType.BossEscaper, data);
            data = new SharedData();
            data.BossBuildupTime = 0.5f;
            dictionary.Add(SkillType.BossBreederEscaper, data);
            dictionary.Add(SkillType.PoisonPuff, new SharedData());
            data = new SharedData();
            data.BossBuildupTime = 1f;
            data.ExternalControl = true;
            dictionary.Add(SkillType.Escape, data);
            data = new SharedData();
            data.Cooldown = 2.5f;
            data.BossCooldown = 2.5f;
            data.BossFirstCastDelay = 0.5f;
            dictionary.Add(SkillType.BossDefender, data);
            data = new SharedData();
            data.Cooldown = 2.5f;
            data.BossCooldown = 2.5f;
            data.BossFirstCastDelay = 2f;
            dictionary.Add(SkillType.BossSplitter, data);
            data = new SharedData();
            data.Cooldown = 2.5f;
            data.BossCooldown = 2.5f;
            data.BuildupTime = 0.5f;
            data.BossBuildupTime = 0.75f;
            data.BossFirstCastDelay = 1.5f;
            data.Name = ConfigLoca.SKILL_HEAL_NAME;
            dictionary.Add(SkillType.Heal, data);
            data = new SharedData();
            data.BossBuildupTime = 0.75f;
            data.ExternalControl = true;
            dictionary.Add(SkillType.BossSlam, data);
            data = new SharedData();
            data.Name = ConfigLoca.SKILL_EXPLOSION_NAME;
            dictionary.Add(SkillType.Explosion, data);
            data = new SharedData();
            data.Cooldown = 10f;
            data.BossCooldown = 2.5f;
            data.BuildupTime = 0.5f;
            data.BossBuildupTime = 0.75f;
            data.BossFirstCastDelay = 1.5f;
            data.Invulnerability = true;
            data.ExternalControl = true;
            dictionary.Add(SkillType.Cluster, data);
            data = new SharedData();
            data.Cooldown = 20f;
            data.BossCooldown = 2.5f;
            data.BossBuildupTime = 0.75f;
            data.BossFirstCastDelay = 1.5f;
            dictionary.Add(SkillType.Shield, data);
            data = new SharedData();
            data.Cooldown = 20f;
            data.BossCooldown = 2.5f;
            data.BossBuildupTime = 0.75f;
            data.BossFirstCastDelay = 1.5f;
            dictionary.Add(SkillType.Midas, data);
            data = new SharedData();
            data.Cooldown = 20f;
            data.BossCooldown = 2.5f;
            data.BossBuildupTime = 0.75f;
            data.BossFirstCastDelay = 1.5f;
            dictionary.Add(SkillType.Frenzy, data);
            data = new SharedData();
            data.Cooldown = 2f;
            data.BossCooldown = 2.5f;
            data.BossBuildupTime = 0.75f;
            data.BossFirstCastDelay = 1.5f;
            dictionary.Add(SkillType.Fireball, data);
            data = new SharedData();
            data.Cooldown = 7f;
            data.BossCooldown = 2.5f;
            data.BossBuildupTime = 0.75f;
            data.BossFirstCastDelay = 1.5f;
            dictionary.Add(SkillType.Blast, data);
            data = new SharedData();
            data.Cooldown = 3f;
            data.BossCooldown = 2.5f;
            data.BuildupTime = 0.5f;
            data.BossBuildupTime = 0.75f;
            data.BossFirstCastDelay = 1.5f;
            data.Invulnerability = true;
            data.ExternalControl = true;
            dictionary.Add(SkillType.Dash, data);
            data = new SharedData();
            data.Cooldown = 8f;
            data.BossCooldown = 2.5f;
            data.BossBuildupTime = 0.75f;
            data.BossFirstCastDelay = 1.5f;
            data.Invulnerability = true;
            data.ExternalControl = true;
            dictionary.Add(SkillType.Decoy, data);
            data = new SharedData();
            data.Cooldown = 10f;
            data.BossCooldown = 2.5f;
            data.BossBuildupTime = 0.75f;
            data.BossFirstCastDelay = 1.5f;
            dictionary.Add(SkillType.Vanish, data);
            SHARED_DATA = dictionary;
            Dictionary<SkillType, BossSummonerSkillSharedData> dictionary2 = new Dictionary<SkillType, BossSummonerSkillSharedData>(new SkillTypeBoxAvoidanceComparer());
            BossSummonerSkillSharedData data2 = new BossSummonerSkillSharedData();
            data2.AiTriggerType = BossSummonerAiBehaviour.TriggerType.Health;
            data2.AiTriggerModifier = 0.6f;
            data2.MinionType = BossSummonerMinionType.Ranged;
            data2.SummonCount = 4;
            dictionary2.Add(SkillType.BossLeader, data2);
            BossSummonerSkillSharedData data3 = new BossSummonerSkillSharedData();
            data3.AiTriggerType = BossSummonerAiBehaviour.TriggerType.TimeInterval;
            data3.AiTriggerModifier = 0.5f;
            data3.MinionType = BossSummonerMinionType.Mixed;
            data3.SummonCount = 1;
            dictionary2.Add(SkillType.BossBreeder, data3);
            BossSummonerSkillSharedData data4 = new BossSummonerSkillSharedData();
            data4.AiTriggerType = BossSummonerAiBehaviour.TriggerType.Health;
            data4.AiTriggerModifier = 0.6f;
            data4.EscapeAfterCasting = true;
            data4.MinionType = BossSummonerMinionType.Melee;
            data4.SummonCount = 4;
            dictionary2.Add(SkillType.BossEscaper, data4);
            BossSummonerSkillSharedData data5 = new BossSummonerSkillSharedData();
            data5.AiTriggerType = BossSummonerAiBehaviour.TriggerType.TimeInterval;
            data5.AiTriggerModifier = 1f;
            data5.EscapeAfterCasting = true;
            data5.MinionType = BossSummonerMinionType.Melee;
            data5.SummonCount = 1;
            dictionary2.Add(SkillType.BossBreederEscaper, data5);
            BOSS_SUMMONER_SKILLS = dictionary2;
        }

        public static SharedData GetDataForInstance(SkillInstance si)
        {
            return SHARED_DATA[si.SkillType];
        }

        public static class Blast
        {
            public static float DamagePct = 1.5f;
            public static float PushForce = 30f;
            public static float Radius = 4f;
        }

        public static class BossDefender
        {
            public static float ArmorBonus = 0.65f;
            public static float Duration = 2f;
        }

        public static class BossLeap
        {
            public static float DamagePct = 2.1f;
        }

        public static class BossOmnislash
        {
            public static float UpgradedDashDamagePct = 1f;
        }

        public static class BossSlam
        {
            public static float DamagePct = 2f;
            public static float DragPerSecond = 5f;
            public static float LeapDuration = 0.25f;
            public static float LeapTargetHeight = 1.4f;
            public static float MovementForce = 35f;
            public static float PostLeapWaitTime;
        }

        public static class BossSplitter
        {
            public static float DamageMultiplierPerAliveClone = 0.75f;
            public static float HpMultiplierPerSplit = 1f;
            public static int MaxNumAliveClones = 3;
        }

        public static class BossSummoner
        {
            public static int SummonCountLimit = 6;
            public static float SummonRadius = 3.5f;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BossSummonerSkillSharedData
        {
            public BossSummonerAiBehaviour.TriggerType AiTriggerType;
            public float AiTriggerModifier;
            public bool EscapeAfterCasting;
            public BossSummonerMinionType MinionType;
            public int SummonCount;
        }

        public static class BossWhirlwind
        {
            public static float HugeRuneTotalDamagePct = 5.6f;
            public static float TotalDamagePct = 2f;
        }

        public static class Charge
        {
            public static float DamagePct = 4f;
            public static float MovementForce = 25f;
            public static float PushForce = 15f;
        }

        public static class Clone
        {
            public static float AttackSpeedMultiplier = 1.15f;
            public static float BaseHPMultiplier = 1f;
            public static float BehindSummonDistance = 6f;
            public static float DecoyHPMultiplier = 0.33f;
            public static float DefaultSummonDistance = 4f;
            public static float DphMultiplier = 0.35f;
            public static float MovementSpeedMultiplier = 1.1f;
            public static int NumberOfClones = 1;
            public static float ThreatMultiplier = 1f;
            public static float WaitAfterSummon = 0.3f;
            public static float WaitBetweenSummons = 0.15f;
        }

        public static class Cluster
        {
            public static float CameraShakeDecay = 0.01f;
            public static float CameraShakeIntensity = 0.08f;
            public static int NumProjectiles = 8;
            public static float ProjectileDamagePct = 2.5f;
            public static float ProjectileDamageRadius = 1.5f;
            public static float ProjectileGravityFactor = 18f;
            public static float ProjectileRadius = 1f;
            public static float ProjectileSpeedMax = 20f;
            public static float ProjectileSpeedMin = 10f;
            public static float SpinDuration = 0.35f;
            public static float ThrowArc = 45f;
            public static float VerticalLift = 1.4f;
            public static float WaitAfterSpin;
        }

        public static class Dash
        {
            public static float DamagePct = 4f;
            public static float MaxDistanceTraveled = 8f;
            public static float MovementForce = 45f;
            public static float NormalizedExecutionMidpoint = 0.5f;
            public static float PushForce = 30f;
        }

        public static class Decoy
        {
            public static float DragPerSecond = 5f;
            public static float LeapDuration = 0.25f;
            public static float LeapTargetHeight = 1.4f;
            public static float MovementForce = 20f;
            public static float PostLeapWaitTime;
        }

        public static class Explosion
        {
            public const float DamagePct = 0.25f;
            public static float PushForce = 30f;
            public static float Radius = 4f;
        }

        public static class Fireball
        {
            public static float DamagePct = 1.5f;
            public static float PushForce = 30f;
            public static float Radius = 4f;
        }

        public static class Frenzy
        {
            public static float AttackSpeedMultiplier = 2f;
            public static float Duration = 3f;
        }

        public static class Heal
        {
            public static float LifeGainPct = 0.1f;
        }

        public static class Implosion
        {
            public static float CameraShakeDecay = 0.007f;
            public static float CameraShakeIntensity = 0.12f;
            public static float DamagePct = 2.6f;
            public static float DestructibleBlastRadius = 3.25f;
            public static float DrawDuration = 0.4f;
            public static float DrawForceMax = 80f;
            public static float LeapDuration = 0.5f;
            public static float LeapTargetHeight = 1f;
            public static float Radius = 12f;
        }

        public static class Leap
        {
            public static float ClosestEnemyDistanceThreshold = 6f;
            public static float DamagePct = 2.9f;
            public static float DragPerSecond = 3f;
            public static float LeapDuration = 0.5f;
            public static float LeapTargetHeight = 2.5f;
            public static float MovementForce = 23f;
            public static float PostBlastPushForce = 25f;
            public static float PostBlastRadius = 5f;
            public static float PushAndBlastRadius = 1f;
            public static float PushForcePerVelocity = 0.5f;
            public static int UpgradedBonusCoinCountPerUsage = 3;
            public static float UpgradedClosestEnemyDistanceThreshold = 7f;
            public static int UpgradedNumCloneInstancesAllowed = 1;
        }

        public static class Midas
        {
            public static float Duration = ConfigBoosts.SHARED_DATA[BoostType.Midas].DurationSeconds;
        }

        public static class Omnislash
        {
            public static float DelayBetweenSlashes = 0.15f;
            public static float EffectiveHitRadius = 1.5f;
            public static float Radius = 10f;
            public static double SameTargetDiminishingDamageMultiplier = 0.5;
            public static int SlashCount = 5;
            public static float TotalDamageMultiplier = 11.25f;
            public static float TotalDamageMultiplierIncreasePerExtraTarget = 2.25f;
            public static int UpgradedBonusCoinCountPerKill = 1;
            public static float UpgradedDashClosestEnemyDistanceThreshold = 6f;
            public static float UpgradedDashDamagePct = 1.5f;
            public static float UpgradedDashDragPerSecond = 10f;
            public static float UpgradedDashMovementForce = 80f;
            public static float UpgradedDashPushForcePerVelocity = 0.5f;
        }

        public class SharedData
        {
            public float BossBuildupTime;
            public float BossCooldown;
            public float BossFirstCastDelay;
            public float BuildupTime;
            public float Cooldown;
            public string Description;
            public bool ExternalControl;
            public int Group;
            public bool Invulnerability;
            public bool ManualTargeting;
            public string Name;
            public bool Passive;
            public string Sprite;
            public string Spritesheet;
            public int UnlockRank;
        }

        public static class Shield
        {
            public static float ArmorBonus = 1f;
            public static float Duration = 8f;
        }

        public static class Slam
        {
            public static float CameraShakeDecay = 0.007f;
            public static float CameraShakeIntensity = 0.13f;
            public static float DamagePct = 3f;
            public static float Radius = 2.15f;
            public static float RockConcentrationCenterDistance = 1.5f;
            public static float RockConcentrationDamagePct = 0.15f;
            public static float RockConcentrationProjectileLifetime = 0.3f;
            public static float RockConcentrationProjectileRadius = 1.5f;
            public static float RockConcentrationProjectileSpawnRate = 0.15f;
            public static float RockConcentrationProjectileVisualScale = 0.8f;
            public static float RockConcentrationRadius = 2.5f;
            public static float RockRingDamagePct = 0.3f;
            public static float RockRingEndRadius = 3.5f;
            public static int RockRingProjectileCount = 8;
            public static float RockRingProjectileLifetime = 0.25f;
            public static float RockRingProjectileRadius = 1f;
            public static float RockRingProjectileVisualScale = 0.65f;
            public static float RockRingScalePet = 0.5f;
            public static float RockRingSectorEndRadius = 5.6f;
            public static int RockRingSectorProjectileCount = 3;
            public static float RockRingSectorSize = 90f;
            public static int RockRingSectorWaveCount = 5;
            public static float RockRingStartRadius = 1f;
            public static int RockRingWaveCount = 3;
            public static float RockRingWaveInterval = 0.1f;
            public static float Speed = 20f;
            public static float WaitBeforeMoving = 0.15f;
        }

        public static class Vanish
        {
            public static float BlastDamagePct = 0.75f;
            public static float BlastPushForce = 25f;
            public static float BlastRadius = 3.5f;
            public static float Duration = 5f;
        }

        public static class Whirlwind
        {
            public static float Duration = 0.6f;
            public static float HugeRuneDuration = 1.2f;
            public static float HugeRuneRadius = 5f;
            public static float HugeRuneTotalDamagePct = 5.6f;
            public static int HugeRuneTotalSpinCount = 6;
            public static float Radius = 3.25f;
            public static float ShieldRuneDuration = 0.6f;
            public static int ShieldRuneTotalSpinCount = 3;
            public static float TotalDamagePct = 2.8f;
            public static int TotalSpinCount = 3;
        }
    }
}

