namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class EffectSystem : MonoBehaviour
    {
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        public static Dictionary<EffectType, EffectType> LINKED_DESTRUCTION_EFFECTS = new Dictionary<EffectType, EffectType>();
        private List<PoolableParticleSystem> m_allParticleSystems = new List<PoolableParticleSystem>();
        private PoolableParticleSystem m_dungeonWeatherEffect;
        private ITypedObjectPool<PoolableParticleSystem, EffectType> m_dynamicParticleEffectPool;
        private List<PoolableParticleSystem> m_followParticleSystems = new List<PoolableParticleSystem>();
        private Dictionary<EffectType, bool> m_persistentEffecTypeQuickLookup = new Dictionary<EffectType, bool>(new EffectTypeBoxAvoidanceComparer());

        protected void Awake()
        {
            this.Tm = base.transform;
            IEnumerator enumerator = Enum.GetValues(typeof(EffectType)).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    EffectType current = (EffectType) ((int) enumerator.Current);
                    if (ConfigObjectPools.PERSISTENT_PARTICLE_EFFECTS.ContainsKey(current))
                    {
                        this.m_persistentEffecTypeQuickLookup.Add(current, true);
                    }
                    else
                    {
                        this.m_persistentEffecTypeQuickLookup.Add(current, false);
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
        }

        private PoolableParticleSystem getFollowParticleSystem(CharacterInstance c, EffectType effectType)
        {
            if (effectType != EffectType.NONE)
            {
                for (int i = 0; i < this.m_followParticleSystems.Count; i++)
                {
                    PoolableParticleSystem system = this.m_followParticleSystems[i];
                    if ((system.FollowTm == c.PhysicsBody.Transform) && (system.EffectType == effectType))
                    {
                        return system;
                    }
                    CharacterView view = PlayerView.Binder.RoomView.getCharacterViewForCharacter(c);
                    if (((view != null) && (system.FollowTm == view.Transform)) && (system.EffectType == effectType))
                    {
                        return system;
                    }
                }
            }
            return null;
        }

        private float getParticleSystemStartSizeMultiplier(EffectType effectType, CharacterInstance character)
        {
            if (character.IsBoss)
            {
                switch (effectType)
                {
                    case EffectType.Aura_Ice:
                    case EffectType.Aura_DamageBonus:
                        return 1.75f;

                    case EffectType.Skill_Shield:
                    case EffectType.Skill_ShieldMagic:
                        return 1.333f;
                }
            }
            return 1f;
        }

        public bool hasFollowParticleSystem(CharacterInstance c, EffectType effectType)
        {
            return (this.getFollowParticleSystem(c, effectType) != null);
        }

        private bool isPersistentEffect(EffectType effectType)
        {
            return this.m_persistentEffecTypeQuickLookup[effectType];
        }

        protected void LateUpdate()
        {
            for (int i = this.m_followParticleSystems.Count - 1; i >= 0; i--)
            {
                PoolableParticleSystem pps = this.m_followParticleSystems[i];
                if ((pps.FollowTm == null) || !pps.FollowTm.gameObject.activeSelf)
                {
                    this.stopFollowEffect(pps);
                }
                else
                {
                    pps.Tm.position = pps.FollowTm.position;
                }
            }
            for (int j = this.m_allParticleSystems.Count - 1; j >= 0; j--)
            {
                PoolableParticleSystem system2 = this.m_allParticleSystems[j];
                if (system2.ManualSimulationWithUnscaledTime)
                {
                    system2.ParticleSystem.Simulate(Time.deltaTime / Time.timeScale, true, false);
                }
            }
            for (int k = this.m_allParticleSystems.Count - 1; k >= 0; k--)
            {
                PoolableParticleSystem system3 = this.m_allParticleSystems[k];
                if (system3.ManualDurationTimer > 0f)
                {
                    system3.ManualDurationTimer = Mathf.Clamp(system3.ManualDurationTimer - Time.deltaTime, 0f, float.MaxValue);
                    if (system3.ManualDurationTimer == 0f)
                    {
                        system3.stopAndFadeOutParticles();
                    }
                }
            }
            for (int m = this.m_allParticleSystems.Count - 1; m >= 0; m--)
            {
                PoolableParticleSystem system4 = this.m_allParticleSystems[m];
                if (!system4.isAlive())
                {
                    if (this.isPersistentEffect(system4.EffectType))
                    {
                        PlayerView.Binder.PersistentParticleEffectPool.returnObject(system4, system4.EffectType);
                    }
                    else
                    {
                        this.m_dynamicParticleEffectPool.returnObject(system4, system4.EffectType);
                    }
                    this.m_allParticleSystems.Remove(system4);
                    if (this.m_followParticleSystems.Contains(system4))
                    {
                        this.m_followParticleSystems.Remove(system4);
                    }
                }
            }
        }

        private void onBuffEnded(CharacterInstance c, Buff buff)
        {
            this.refreshCharacterAuraEffects(c);
            if (buff.SourceCharacter != null)
            {
                this.refreshCharacterAuraEffects(buff.SourceCharacter);
            }
        }

        private void onBuffPreEnd(CharacterInstance c, Buff buff)
        {
            EffectType effectTypeForBuff = ConfigUi.GetEffectTypeForBuff(buff);
            if (effectTypeForBuff != EffectType.NONE)
            {
                PoolableParticleSystem pps = this.getFollowParticleSystem(c, effectTypeForBuff);
                if (pps != null)
                {
                    this.stopFollowEffect(pps);
                }
            }
        }

        private void onBuffRefreshed(CharacterInstance c, Buff buff)
        {
            EffectType effectTypeForBuff = ConfigUi.GetEffectTypeForBuff(buff);
            if (effectTypeForBuff != EffectType.NONE)
            {
                PoolableParticleSystem system = this.getFollowParticleSystem(c, effectTypeForBuff);
                if (system != null)
                {
                    system.ManualDurationTimer = buff.getSecondsRemaining();
                }
                else
                {
                    CharacterView view = PlayerView.Binder.RoomView.getCharacterViewForCharacter(c);
                    if (view != null)
                    {
                        if (c.IsBoss)
                        {
                            this.playEffectFollow(view.Transform, effectTypeForBuff, buff.getSecondsRemaining(), 2.25f, null);
                        }
                        else
                        {
                            this.playEffectFollow(view.Transform, effectTypeForBuff, buff.getSecondsRemaining(), 1f, null);
                        }
                    }
                }
            }
        }

        private void onBuffStarted(CharacterInstance c, Buff buff)
        {
            EffectType effectTypeForBuff = ConfigUi.GetEffectTypeForBuff(buff);
            if (effectTypeForBuff != EffectType.NONE)
            {
                PoolableParticleSystem pps = this.getFollowParticleSystem(c, effectTypeForBuff);
                if (pps != null)
                {
                    this.stopFollowEffect(pps);
                }
                CharacterView view = PlayerView.Binder.RoomView.getCharacterViewForCharacter(c);
                if (view != null)
                {
                    if (c.IsBoss)
                    {
                        this.playEffectFollow(view.Transform, effectTypeForBuff, buff.getSecondsRemaining(), 2.25f, null);
                    }
                    else
                    {
                        this.playEffectFollow(view.Transform, effectTypeForBuff, buff.getSecondsRemaining(), 1f, null);
                    }
                }
            }
            this.refreshCharacterAuraEffects(c);
            if (buff.SourceCharacter != null)
            {
                this.refreshCharacterAuraEffects(buff.SourceCharacter);
            }
        }

        private void onCharacterBlinked(CharacterInstance c)
        {
            if (c.Type == GameLogic.CharacterType.Dragon)
            {
                this.playEffectStatic(c.getHeadLevelWorldPos(), EffectType.Projectile_Fireball_Hit, -1f, false, 1f, null);
            }
            else
            {
                this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Blink, -1f, false, 1f, null);
            }
        }

        private void onCharacterCharmConditionChanged(CharacterInstance c)
        {
            if (c.Charmed)
            {
                this.playEffectFollow(c.PhysicsBody.Transform, EffectType.Charmed, -1f, 1f, null);
            }
            else
            {
                this.stopFollowEffect(this.getFollowParticleSystem(c, EffectType.Charmed));
            }
        }

        private void onCharacterConfusedConditionChanged(CharacterInstance c)
        {
            if (c.Confused)
            {
                this.playEffectFollow(c.PhysicsBody.Transform, EffectType.Confused, -1f, 1f, null);
            }
            else
            {
                this.stopFollowEffect(this.getFollowParticleSystem(c, EffectType.Confused));
            }
        }

        private void onCharacterKilled(CharacterInstance character, CharacterInstance killer, bool critted, SkillType fromSkill)
        {
            Color white;
            int num;
            if (character.IsPlayerCharacter)
            {
                if (character.Prefab == CharacterPrefab.Critter)
                {
                    this.playEffectStatic(character.getHeadLevelWorldPos(), EffectType.CritterSummon, -1f, false, 1f, null);
                }
                else if (character.Type == GameLogic.CharacterType.Dragon)
                {
                    this.playEffectStatic(character.getHeadLevelWorldPos(), EffectType.Projectile_Fireball_Hit, -1f, false, 1f, null);
                }
                else if (character.IsSupport)
                {
                    this.playEffectStatic(character.getHeadLevelWorldPos(), EffectType.SupportCharacterDeath, -1f, false, 1f, null);
                }
                goto Label_02D8;
            }
            if (character.IsWildBoss)
            {
                if (fromSkill == SkillType.Escape)
                {
                    return;
                }
                PlayerView.Binder.EffectSystem.playEffectStatic(character.getGroundLevelWorldPos(), EffectType.ConfettiExplosion, -1f, false, 1f, null);
            }
            switch (character.Type)
            {
                case GameLogic.CharacterType.Jelly:
                    white = Color.white;
                    switch (character.Prefab)
                    {
                        case CharacterPrefab.JellyGreen:
                            white = new Color(0.2352941f, 0.972549f, 0f, 0.9607843f);
                            goto Label_01B6;

                        case CharacterPrefab.JellyOrange:
                            white = new Color(0.9921569f, 0.6352941f, 0f, 0.9607843f);
                            goto Label_01B6;

                        case CharacterPrefab.JellyPurple:
                            white = new Color(0.8705882f, 0.1294118f, 251f, 0.9607843f);
                            goto Label_01B6;

                        case CharacterPrefab.JellySpiked:
                            white = new Color(0.2392157f, 0.3921569f, 0.5450981f, 0.9607843f);
                            goto Label_01B6;
                    }
                    break;

                case GameLogic.CharacterType.Pygmy:
                    this.playEffectStatic(character.getGroundLevelWorldPos(), EffectType.PygmyDeath, -1f, false, 1f, null);
                    goto Label_02D8;

                case GameLogic.CharacterType.Worg:
                    this.playEffectStatic(character.getGroundLevelWorldPos(), EffectType.WorgDeath, -1f, false, 1f, null);
                    goto Label_02D8;

                case GameLogic.CharacterType.Goblin:
                    this.playEffectStatic(character.getGroundLevelWorldPos(), EffectType.GoblinDeath, -1f, false, 1f, null);
                    goto Label_02D8;

                case GameLogic.CharacterType.Yeti:
                    this.playEffectStatic(character.getGroundLevelWorldPos(), EffectType.YetiDeath, -1f, false, 1f, null);
                    goto Label_02D8;

                case GameLogic.CharacterType.Crocodile:
                    this.playEffectStatic(character.getGroundLevelWorldPos(), EffectType.CrocodileDeath, -1f, false, 1f, null);
                    goto Label_02D8;

                case GameLogic.CharacterType.Rat:
                    this.playEffectStatic(character.getGroundLevelWorldPos(), EffectType.RatDeath, -1f, false, 1f, null);
                    goto Label_02D8;

                case GameLogic.CharacterType.Shroom:
                    this.playEffectStatic(character.getGroundLevelWorldPos(), EffectType.ShroomDeath, -1f, false, 1f, null);
                    goto Label_02D8;

                default:
                    this.playEffectStatic(character.getGroundLevelWorldPos(), EffectType.SkeletonDeath, -1f, false, 1f, null);
                    goto Label_02D8;
            }
        Label_01B6:
            this.playEffectStatic(character.getGroundLevelWorldPos(), EffectType.JellyDeath, -1f, false, 1f, white);
        Label_02D8:
            num = this.m_followParticleSystems.Count - 1;
            while (num >= 0)
            {
                if (this.m_followParticleSystems[num].FollowTm == character.PhysicsBody.Transform)
                {
                    this.stopFollowEffect(this.m_followParticleSystems[num]);
                }
                else
                {
                    CharacterView view = PlayerView.Binder.RoomView.getCharacterViewForCharacter(character);
                    if ((view != null) && (this.m_followParticleSystems[num].FollowTm == view.Transform))
                    {
                        this.stopFollowEffect(this.m_followParticleSystems[num]);
                    }
                }
                num--;
            }
        }

        private void onCharacterMeleeAttackContact(CharacterInstance sourceCharacter, Vector3 contactWorldPos, bool importantContact)
        {
            if (sourceCharacter.IsSupport)
            {
                if ((sourceCharacter.Prefab == CharacterPrefab.PetMoose1) || (sourceCharacter.Prefab == CharacterPrefab.PetRam1))
                {
                    CharacterView view = PlayerView.Binder.RoomView.getCharacterViewForCharacter(sourceCharacter);
                    this.playEffectStatic((view.Transform.position + ((Vector3) (view.Transform.forward * 1f))) + new Vector3(0f, 1.75f, 0f), EffectType.Attack_Moose, -1f, false, 0.75f, null).Tm.rotation = view.Transform.rotation;
                }
                else
                {
                    float num2 = Vector3Extensions.SignedAngle(Vector3Extensions.ToXzVector3(PlayerView.Binder.RoomView.getCharacterViewForCharacter(sourceCharacter).Transform.forward), Vector3Extensions.ToXzVector3(PlayerView.Binder.RoomView.RoomCamera.Transform.forward));
                    this.playEffectStatic(contactWorldPos, EffectType.AttackSwing, -1f, false, 1f, null).ParticleSystem.startRotation = (-num2 - 90f) * 0.01745329f;
                }
            }
            else
            {
                this.playEffectStatic(contactWorldPos, EffectType.MonsterAttackHit, -1f, false, 1f, null);
            }
        }

        private void onCharacterPreBlink(CharacterInstance c)
        {
            if (c.Type == GameLogic.CharacterType.Dragon)
            {
                this.playEffectStatic(c.getHeadLevelWorldPos(), EffectType.Projectile_Fireball_Hit, -1f, false, 1f, null);
            }
            else
            {
                this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Blink, -1f, false, 1f, null);
            }
        }

        private void onCharacterRevived(CharacterInstance character)
        {
            this.playEffectStatic(character.getGroundLevelWorldPos(), EffectType.CharacterRevive, -1f, false, 1f, null);
            this.refreshCharacterAuraEffects(character);
        }

        private void onCharacterSkillActivated(CharacterInstance c, SkillType skillType, float buildupTime, SkillExecutionStats executionStats)
        {
            if (buildupTime > 0f)
            {
                if (skillType == SkillType.Leap)
                {
                    this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Skill_Leap_Buildup, buildupTime, false, 1f, null);
                }
                else if (skillType == SkillType.Omnislash)
                {
                    this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Skill_Dash_Buildup, buildupTime, false, 1f, null);
                }
                else if (skillType == SkillType.Whirlwind)
                {
                    this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Skill_Whirlwind_Buildup, buildupTime, false, 1f, null);
                }
                else if (skillType == SkillType.BossSummoner)
                {
                    this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Skill_BossSummoner_Buildup, buildupTime, false, 1f, null);
                }
                else if (skillType == SkillType.BossLeader)
                {
                    this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Skill_BossLeader_Buildup, buildupTime, false, 1f, null);
                }
                else if ((skillType == SkillType.Slam) || (skillType == SkillType.BossSlam))
                {
                    if (!c.IsPet)
                    {
                        this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Skill_Slam_Buildup, buildupTime, false, 1f, null);
                    }
                }
                else if (skillType == SkillType.Heal)
                {
                    this.playEffectFollow(c.PhysicsBody.Transform, EffectType.Skill_Leap_Buildup, buildupTime, 1f, null);
                }
                else if (skillType == SkillType.PoisonPuff)
                {
                    this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Skill_PoisonPuff, -1f, false, 1f, null);
                }
                else if (skillType == SkillType.BossBreeder)
                {
                    this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Skill_BossSummoner_Buildup, buildupTime, false, 1f, null);
                }
                else if (skillType == SkillType.BossEscaper)
                {
                    this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Skill_BossSummoner_Buildup, buildupTime, false, 1f, null);
                }
                else if (skillType == SkillType.BossBreederEscaper)
                {
                    this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Skill_BossSummoner_Buildup, buildupTime, false, 1f, null);
                }
                else if (skillType == SkillType.Escape)
                {
                    this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Skill_BossSummoner_Buildup, buildupTime, false, 1f, null);
                }
            }
        }

        private void onCharacterSkillBuildupCompleted(CharacterInstance c, SkillType skillType, SkillExecutionStats executionStats)
        {
            if (skillType == SkillType.Leap)
            {
                if (!c.IsPet)
                {
                    this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Skill_Leap_PostBuildup, -1f, false, 1f, null);
                }
            }
            else if (skillType == SkillType.Heal)
            {
                this.playEffectFollow(executionStats.TargetCharacter.PhysicsBody.Transform, EffectType.Skill_Heal, -1f, 1f, null);
            }
            else if (skillType == SkillType.Shield)
            {
                float duration = ConfigSkills.Shield.Duration;
                this.playEffectFollow(c.PhysicsBody.Transform, EffectType.Skill_Shield, duration, 1f, null);
            }
            else if (skillType == SkillType.Frenzy)
            {
                float manualDuration = ConfigSkills.Frenzy.Duration;
                this.playEffectFollow(c.PhysicsBody.Transform, EffectType.GenericGlow, manualDuration, 1f, null);
            }
            else if (skillType != SkillType.Whirlwind)
            {
                if (skillType == SkillType.Fireball)
                {
                    this.playEffectStatic(c.ManualTargetPos, EffectType.Skill_Blast, -1f, false, 1f, null);
                }
                else if (skillType == SkillType.Vanish)
                {
                    float num5 = ConfigSkills.Vanish.Duration;
                    this.playEffectFollow(c.PhysicsBody.Transform, EffectType.Skill_Vanish, num5 - 3f, 1f, null);
                }
                else if (skillType == SkillType.Implosion)
                {
                    this.playEffectStatic(c.getGroundLevelWorldPos() + ((Vector3) (c.PhysicsBody.Transform.forward * 1.75f)), EffectType.Skill_Implosion, -1f, false, 1f, null);
                }
                else if (skillType == SkillType.BossDefender)
                {
                    float num6 = ConfigSkills.BossDefender.Duration;
                    this.playEffectFollow(c.PhysicsBody.Transform, EffectType.Skill_BossDefender, num6, 1f, null);
                }
                else if (skillType == SkillType.BossSplitter)
                {
                    this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Skill_BossSplitter, -1f, false, 1f, null);
                }
                else if ((skillType == SkillType.Slam) && c.IsPet)
                {
                    this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Skill_Slam_Pet, -1f, false, 1f, null);
                }
            }
            else
            {
                EffectType type2;
                PerkType type = c.getVisualRunestonePerkType(skillType);
                PerkType type3 = type;
                switch (type3)
                {
                    case PerkType.SkillUpgradeWhirlwind3:
                        type2 = EffectType.Skill_Whirlwind_Upgrade3;
                        break;

                    case PerkType.SkillUpgradeWhirlwind4:
                        type2 = EffectType.Skill_Whirlwind_Upgrade4;
                        break;

                    default:
                        if (type3 == PerkType.SkillUpgradeWhirlwind1)
                        {
                            type2 = EffectType.Skill_Whirlwind_Upgrade1;
                        }
                        else if (type3 == PerkType.SkillUpgradeWhirlwind2)
                        {
                            type2 = EffectType.Skill_Whirlwind_Upgrade2;
                        }
                        else
                        {
                            type2 = EffectType.Skill_Whirlwind;
                        }
                        break;
                }
                float startSizeMultiplier = 1f;
                if (c.IsPet)
                {
                    startSizeMultiplier = 0.75f;
                }
                this.playEffectFollow(c.PhysicsBody.Transform, type2, -1f, startSizeMultiplier, null);
                if (type == PerkType.SkillUpgradeWhirlwind1)
                {
                    this.playEffectFollow(c.PhysicsBody.Transform, EffectType.SupportCharacterDeath, -1f, 1.5f, null);
                }
            }
        }

        private void onCharacterSkillExecuted(CharacterInstance c, SkillType skillType, SkillExecutionStats executionStats)
        {
            if (skillType == SkillType.Blast)
            {
                this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Skill_Blast, -1f, false, 1f, null);
            }
            else if (skillType == SkillType.Implosion)
            {
                this.playEffectStatic(c.getGroundLevelWorldPos() + ((Vector3) (c.PhysicsBody.Transform.forward * 1.75f)), EffectType.Skill_Implosion_Blast, -1f, false, 1f, null);
            }
            else if ((skillType == SkillType.Dash) && (executionStats.EnemiesAround > 0))
            {
                this.playEffectStatic(c.getGroundLevelWorldPos() + ((Vector3) (c.PhysicsBody.Transform.forward * 1.75f)), EffectType.Skill_Dash_Hit, -1f, false, 1f, null);
            }
        }

        private void onCharacterSkillExecutionMidpoint(CharacterInstance c, SkillType skillType, SkillExecutionStats executionStats)
        {
            if (skillType == SkillType.Leap)
            {
                if (c.IsPet)
                {
                    this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Pet_Leap, -1f, false, 1f, null);
                }
                else
                {
                    this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Skill_Leap, -1f, false, 1f, null);
                }
            }
            else if (skillType != SkillType.Omnislash)
            {
                if (skillType == SkillType.Charge)
                {
                    this.playEffectStatic(c.getHeadLevelWorldPos() + ((Vector3) (c.PhysicsBody.Transform.forward * 2f)), EffectType.Attack_Ram, -1f, false, 1f, null);
                }
                else if (skillType == SkillType.Escape)
                {
                    this.playEffectStatic(c.getGroundLevelWorldPos(), EffectType.Pet_Leap, -1f, false, 4f, null);
                }
            }
            else if (executionStats.EnemiesAround > 0)
            {
                EffectType type2;
                PerkType type3 = c.getVisualRunestonePerkType(skillType);
                if (type3 == PerkType.SkillUpgradeOmnislash1)
                {
                    type2 = EffectType.Skill_Omnislash_Hit_Upgrade1;
                }
                else if (type3 == PerkType.SkillUpgradeOmnislash3)
                {
                    type2 = !executionStats.SpecialCaseThisFrame ? EffectType.Skill_Omnislash_Hit : EffectType.Skill_Omnislash_Hit_Upgrade3;
                }
                else if (type3 == PerkType.SkillUpgradeOmnislash4)
                {
                    type2 = EffectType.Skill_Omnislash_Hit_Upgrade4;
                }
                else
                {
                    type2 = EffectType.Skill_Omnislash_Hit;
                }
                object colorParameters = null;
                if ((c.Prefab == CharacterPrefab.PetShark1) || (c.Prefab == CharacterPrefab.PetShark2))
                {
                    colorParameters = new Color(0.3490196f, 0.03529412f, 1f, 1f);
                }
                this.playEffectStatic(c.getGroundLevelWorldPos(), type2, -1f, false, 1f, colorParameters);
            }
        }

        private void onCharacterSpawned(CharacterInstance c)
        {
            if (c.Prefab == CharacterPrefab.Critter)
            {
                this.playEffectStatic(c.getHeadLevelWorldPos(), EffectType.CritterSummon, -1f, false, 1f, null);
            }
            else if (c.Prefab == CharacterPrefab.KnightClone)
            {
                float manualDuration = -1f;
                if (c.FutureTimeOfDeath > 0f)
                {
                    manualDuration = (c.FutureTimeOfDeath - Time.fixedTime) - 1f;
                }
                PerkType type = c.getVisualRunestonePerkType(SkillType.Clone);
                object obj2 = null;
                switch (type)
                {
                    case PerkType.SkillUpgradeClone1:
                    {
                        CloneSmokeParticleSystem.ColorParameters parameters = new CloneSmokeParticleSystem.ColorParameters();
                        parameters.Fog = ConfigGameplay.CLONE_HEAL_MATERIAL_COLOR;
                        obj2 = parameters;
                        break;
                    }
                    case PerkType.SkillUpgradeClone3:
                    {
                        CloneSmokeParticleSystem.ColorParameters parameters2 = new CloneSmokeParticleSystem.ColorParameters();
                        parameters2.Fog = ConfigGameplay.CLONE_DECOY_MATERIAL_COLOR;
                        obj2 = parameters2;
                        break;
                    }
                    case PerkType.SkillUpgradeClone4:
                    {
                        CloneSmokeParticleSystem.ColorParameters parameters3 = new CloneSmokeParticleSystem.ColorParameters();
                        parameters3.Fog = ConfigGameplay.CLONE_COOLDOWN_MATERIAL_COLOR;
                        obj2 = parameters3;
                        break;
                    }
                }
                object colorParameters = obj2;
                this.playEffectFollow(c.PhysicsBody.Transform, EffectType.Skill_CloneSmoke, manualDuration, 1f, colorParameters);
            }
            if (c.Type == GameLogic.CharacterType.Dragon)
            {
                this.playEffectStatic(c.getHeadLevelWorldPos(), EffectType.Projectile_Fireball_Hit, -1f, false, 1f, null);
            }
            if (c.IsBoss)
            {
                float num2;
                if (c.getPerkInstanceCount(PerkType.BossResistWeaponDamage) > 0)
                {
                    num2 = this.getParticleSystemStartSizeMultiplier(EffectType.Skill_Shield, c);
                    this.playEffectFollow(c.PhysicsBody.Transform, EffectType.Skill_Shield, -1f, num2, null);
                }
                if (c.getPerkInstanceCount(PerkType.BossResistSkillDamage) > 0)
                {
                    num2 = this.getParticleSystemStartSizeMultiplier(EffectType.Skill_ShieldMagic, c);
                    this.playEffectFollow(c.PhysicsBody.Transform, EffectType.Skill_ShieldMagic, -1f, num2, null);
                }
                if (c.IsWildBoss || (GameLogic.Binder.GameState.ActiveDungeon.ActiveTournament != null))
                {
                    this.playEffectFollow(c.PhysicsBody.Transform, EffectType.WildBossTargetingReticule, -1f, 1f, null);
                }
            }
            this.refreshCharacterAuraEffects(c);
        }

        protected void OnDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded -= new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            eventBus.OnCharacterSkillActivated -= new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            eventBus.OnCharacterSkillBuildupCompleted -= new GameLogic.Events.CharacterSkillBuildupCompleted(this.onCharacterSkillBuildupCompleted);
            eventBus.OnCharacterSkillExecutionMidpoint -= new GameLogic.Events.CharacterSkillExecutionMidpoint(this.onCharacterSkillExecutionMidpoint);
            eventBus.OnCharacterSkillExecuted -= new GameLogic.Events.CharacterSkillExecuted(this.onCharacterSkillExecuted);
            eventBus.OnCharacterMeleeAttackContact -= new GameLogic.Events.CharacterMeleeAttackContact(this.onCharacterMeleeAttackContact);
            eventBus.OnCharacterCharmConditionChanged -= new GameLogic.Events.CharacterCharmConditionChanged(this.onCharacterCharmConditionChanged);
            eventBus.OnCharacterConfusedConditionChanged -= new GameLogic.Events.CharacterConfusedConditionChanged(this.onCharacterConfusedConditionChanged);
            eventBus.OnMultikillBonusGranted -= new GameLogic.Events.MultikillBonusGranted(this.onMultikillBonusGranted);
            eventBus.OnProjectileSpawned -= new GameLogic.Events.ProjectileSpawned(this.onProjectileSpawned);
            eventBus.OnProjectileCollided -= new GameLogic.Events.ProjectileCollided(this.onProjectileCollided);
            eventBus.OnCharacterSpawned -= new GameLogic.Events.CharacterSpawned(this.onCharacterSpawned);
            eventBus.OnCharacterKilled -= new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            eventBus.OnCharacterRevived -= new GameLogic.Events.CharacterRevived(this.onCharacterRevived);
            eventBus.OnItemEquipped -= new GameLogic.Events.ItemEquipped(this.onItemEquipped);
            eventBus.OnBuffStarted -= new GameLogic.Events.BuffStarted(this.onBuffStarted);
            eventBus.OnBuffPreEnd -= new GameLogic.Events.BuffPreEnd(this.onBuffPreEnd);
            eventBus.OnBuffEnded -= new GameLogic.Events.BuffEnded(this.onBuffEnded);
            eventBus.OnBuffRefreshed -= new GameLogic.Events.BuffRefreshed(this.onBuffRefreshed);
            eventBus.OnCharacterPreBlink -= new GameLogic.Events.CharacterPreBlink(this.onCharacterPreBlink);
            eventBus.OnCharacterBlinked -= new GameLogic.Events.CharacterBlinked(this.onCharacterBlinked);
            eventBus.OnDungeonBoostActivated -= new GameLogic.Events.DungeonBoostActivated(this.onDungeonBoostActivated);
            PlayerView.Binder.EventBus.OnFlyToHudStarted -= new PlayerView.Events.FlyToHudStarted(this.onFlyToHudStarted);
        }

        private void onDungeonBoostActivated(DungeonBoost dungeonBoost, SkillType fromSkill)
        {
            EffectType nONE = EffectType.NONE;
            switch (dungeonBoost.PrefabType)
            {
                case DungeonBoostPrefabType.Barrel:
                    nONE = EffectType.BarrelBreak;
                    break;

                case DungeonBoostPrefabType.Cache:
                    nONE = EffectType.CacheBreak;
                    break;

                case DungeonBoostPrefabType.Urn:
                    nONE = EffectType.UrnBreak;
                    break;

                case DungeonBoostPrefabType.Pumpkin:
                    nONE = EffectType.PumpkinBreak;
                    break;
            }
            EffectType effectType = EffectType.NONE;
            switch (dungeonBoost.Properties.BuffPerkType)
            {
                case PerkType.DungeonBoostExplosion:
                    effectType = EffectType.BlastExplosion;
                    break;

                case PerkType.DungeonBoostFrost:
                    effectType = EffectType.FrostExplosion;
                    break;

                case PerkType.DungeonBoostPoison:
                    effectType = EffectType.PoisonExplosion;
                    break;

                case PerkType.DungeonBoostStun:
                    effectType = EffectType.StunExplosion;
                    break;
            }
            if (nONE != EffectType.NONE)
            {
                this.playEffectStatic(dungeonBoost.Transform.position, nONE, -1f, false, 1f, null);
            }
            if (effectType != EffectType.NONE)
            {
                this.playEffectStatic(dungeonBoost.Transform.position, effectType, -1f, false, 1f, null);
            }
            if ((dungeonBoost.Properties.Type == DungeonBoostType.EmptyBox) || !string.IsNullOrEmpty(dungeonBoost.Properties.ShopEntryId))
            {
                this.playEffectStatic(dungeonBoost.Transform.position, EffectType.GoldExplosion, -1f, false, 1f, null);
            }
        }

        protected void OnEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded += new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            eventBus.OnCharacterSkillActivated += new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            eventBus.OnCharacterSkillBuildupCompleted += new GameLogic.Events.CharacterSkillBuildupCompleted(this.onCharacterSkillBuildupCompleted);
            eventBus.OnCharacterSkillExecutionMidpoint += new GameLogic.Events.CharacterSkillExecutionMidpoint(this.onCharacterSkillExecutionMidpoint);
            eventBus.OnCharacterSkillExecuted += new GameLogic.Events.CharacterSkillExecuted(this.onCharacterSkillExecuted);
            eventBus.OnCharacterMeleeAttackContact += new GameLogic.Events.CharacterMeleeAttackContact(this.onCharacterMeleeAttackContact);
            eventBus.OnCharacterCharmConditionChanged += new GameLogic.Events.CharacterCharmConditionChanged(this.onCharacterCharmConditionChanged);
            eventBus.OnCharacterConfusedConditionChanged += new GameLogic.Events.CharacterConfusedConditionChanged(this.onCharacterConfusedConditionChanged);
            eventBus.OnMultikillBonusGranted += new GameLogic.Events.MultikillBonusGranted(this.onMultikillBonusGranted);
            eventBus.OnProjectileSpawned += new GameLogic.Events.ProjectileSpawned(this.onProjectileSpawned);
            eventBus.OnProjectileCollided += new GameLogic.Events.ProjectileCollided(this.onProjectileCollided);
            eventBus.OnCharacterSpawned += new GameLogic.Events.CharacterSpawned(this.onCharacterSpawned);
            eventBus.OnCharacterKilled += new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            eventBus.OnCharacterRevived += new GameLogic.Events.CharacterRevived(this.onCharacterRevived);
            eventBus.OnItemEquipped += new GameLogic.Events.ItemEquipped(this.onItemEquipped);
            eventBus.OnBuffStarted += new GameLogic.Events.BuffStarted(this.onBuffStarted);
            eventBus.OnBuffPreEnd += new GameLogic.Events.BuffPreEnd(this.onBuffPreEnd);
            eventBus.OnBuffEnded += new GameLogic.Events.BuffEnded(this.onBuffEnded);
            eventBus.OnBuffRefreshed += new GameLogic.Events.BuffRefreshed(this.onBuffRefreshed);
            eventBus.OnCharacterPreBlink += new GameLogic.Events.CharacterPreBlink(this.onCharacterPreBlink);
            eventBus.OnCharacterBlinked += new GameLogic.Events.CharacterBlinked(this.onCharacterBlinked);
            eventBus.OnDungeonBoostActivated += new GameLogic.Events.DungeonBoostActivated(this.onDungeonBoostActivated);
            PlayerView.Binder.EventBus.OnFlyToHudStarted += new PlayerView.Events.FlyToHudStarted(this.onFlyToHudStarted);
        }

        private void onFlyToHudStarted(Vector2 sourceScreenPos)
        {
            Vector3 worldPos = PlayerView.Binder.MenuSystem.MenuCamera.ScreenToWorldPoint((Vector3) sourceScreenPos);
            worldPos.z = 0f;
            this.playEffectStatic(worldPos, EffectType.FlyToHudStart, -1f, false, 1f, null);
        }

        private void onGameplayEnded(ActiveDungeon activeDungeon)
        {
            if (this.m_dungeonWeatherEffect != null)
            {
                this.m_dungeonWeatherEffect.ParticleSystem.Stop(true);
                this.m_dungeonWeatherEffect.ParticleSystem.Clear(true);
                this.m_dungeonWeatherEffect = null;
            }
            if (!activeDungeon.SeamlessTransition)
            {
                for (int i = this.m_allParticleSystems.Count - 1; i >= 0; i--)
                {
                    PoolableParticleSystem system = this.m_allParticleSystems[i];
                    if (!this.isPersistentEffect(system.EffectType))
                    {
                        this.m_dynamicParticleEffectPool.returnObject(system, system.EffectType);
                        this.m_allParticleSystems.Remove(system);
                        if (this.m_followParticleSystems.Contains(system))
                        {
                            this.m_followParticleSystems.Remove(system);
                        }
                    }
                }
                this.m_dynamicParticleEffectPool.destroy();
                this.m_dynamicParticleEffectPool = null;
            }
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            if (!activeDungeon.SeamlessTransition)
            {
                Dictionary<EffectType, int> initialCapacityPerType = ConfigObjectPools.PER_THEME_CHARACTER_EFFECTS[activeDungeon.Dungeon.Theme];
                this.m_dynamicParticleEffectPool = new TypedObjectPool<PoolableParticleSystem, EffectType>(new PoolableParticleSystemProvider(App.Binder.DynamicObjectRootTm), 8, initialCapacityPerType, ObjectPoolExpansionMethod.DOUBLE, true);
            }
            if (activeDungeon.Mood.Weather != EffectType.NONE)
            {
                this.m_dungeonWeatherEffect = this.playEffectFollow(PlayerView.Binder.RoomView.RoomCamera.WeatherTransform, activeDungeon.Mood.Weather, -1f, 1f, null);
            }
            this.refreshCharacterAuraEffects(activeDungeon.PrimaryPlayerCharacter);
        }

        private void onItemEquipped(CharacterInstance character, ItemInstance itemInstance, ItemInstance replacedItemInstance)
        {
            this.refreshCharacterAuraEffects(character);
        }

        private void onMultikillBonusGranted(Player player, int killCount, double coinAmount)
        {
            if ((player.ActiveCharacter != null) && player.Tournaments.hasTournamentSelected())
            {
                PlayerView.Binder.EffectSystem.playEffectStatic(player.ActiveCharacter.getGroundLevelWorldPos(), EffectType.ConfettiExplosion, -1f, false, 1f, null);
            }
        }

        private void onProjectileCollided(Projectile projectile, Collider collider)
        {
            float startSizeMultiplier = (projectile.Properties.StartSizeMultiplier > 0f) ? projectile.Properties.StartSizeMultiplier : 1f;
            if (projectile.Properties.Type == ProjectileType.Slam)
            {
                this.playEffectStatic(projectile.Transform.position, EffectType.Projectile_Slam_Hit, -1f, false, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Cluster)
            {
                this.playEffectStatic(projectile.Transform.position, EffectType.Projectile_Cluster_Hit, -1f, false, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Orb)
            {
                this.playEffectStatic(projectile.Transform.position, EffectType.Projectile_Orb_Hit, -1f, false, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Fireball)
            {
                this.playEffectStatic(projectile.Transform.position, EffectType.Projectile_Fireball_Hit, -1f, false, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Dragonball)
            {
                this.playEffectStatic(projectile.Transform.position, EffectType.Projectile_Dragonball_Hit, -1f, false, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Frostball)
            {
                this.playEffectStatic(projectile.Transform.position, EffectType.Projectile_Frostball_Hit, -1f, false, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Bubbles)
            {
                this.playEffectStatic(projectile.Transform.position, EffectType.Projectile_Bubbles_Hit, -1f, false, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Splinters1)
            {
                this.playEffectStatic(projectile.Transform.position, EffectType.Projectile_Splinters1_Hit, -1f, false, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Splinters2)
            {
                this.playEffectStatic(projectile.Transform.position, EffectType.Projectile_Splinters2_Hit, -1f, false, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Ink)
            {
                this.playEffectStatic(projectile.Transform.position, EffectType.Projectile_Ink_Hit, -1f, false, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Coins)
            {
                this.playEffectStatic(projectile.Transform.position, EffectType.Projectile_Coins_Hit, -1f, false, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Rock)
            {
                if (projectile.OwningCharacter != null)
                {
                    startSizeMultiplier = ConfigGameplay.GetCharacterVisualScale(projectile.OwningCharacter);
                }
                this.playEffectStatic(projectile.Transform.position, EffectType.Projectile_Rock_Hit, -1f, false, startSizeMultiplier, null);
            }
        }

        private void onProjectileSpawned(Projectile projectile)
        {
            float startSizeMultiplier = (projectile.Properties.StartSizeMultiplier > 0f) ? projectile.Properties.StartSizeMultiplier : 1f;
            if (projectile.Properties.Type == ProjectileType.Slam)
            {
                if (!projectile.OwningCharacter.IsPet)
                {
                    this.playEffectFollow(projectile.Transform, EffectType.Projectile_Slam, -1f, startSizeMultiplier, null);
                }
            }
            else if (projectile.Properties.Type == ProjectileType.Cluster)
            {
                this.playEffectFollow(projectile.Transform, EffectType.Projectile_Cluster, -1f, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Orb)
            {
                this.playEffectFollow(projectile.Transform, EffectType.Projectile_Orb, -1f, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Fireball)
            {
                this.playEffectFollow(projectile.Transform, EffectType.Projectile_Fireball, -1f, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Dragonball)
            {
                this.playEffectFollow(projectile.Transform, EffectType.Projectile_Dragonball, -1f, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Frostball)
            {
                this.playEffectFollow(projectile.Transform, EffectType.Projectile_Frostball, -1f, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Bubbles)
            {
                this.playEffectFollow(projectile.Transform, EffectType.Projectile_Bubbles, -1f, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Splinters1)
            {
                this.playEffectFollow(projectile.Transform, EffectType.Projectile_Splinters1, -1f, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Splinters2)
            {
                this.playEffectFollow(projectile.Transform, EffectType.Projectile_Splinters2, -1f, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Ink)
            {
                this.playEffectFollow(projectile.Transform, EffectType.Projectile_Ink, -1f, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Coins)
            {
                this.playEffectFollow(projectile.Transform, EffectType.Projectile_Coins, -1f, startSizeMultiplier, null);
            }
            else if (projectile.Properties.Type == ProjectileType.Rock)
            {
                if (projectile.OwningCharacter != null)
                {
                    startSizeMultiplier = ConfigGameplay.GetCharacterVisualScale(projectile.OwningCharacter);
                }
                this.playEffectFollow(projectile.Transform, EffectType.Projectile_Rock, -1f, startSizeMultiplier, null);
            }
        }

        public PoolableParticleSystem playEffectFollow(Transform followTm, EffectType effectType, [Optional, DefaultParameterValue(-1f)] float manualDuration, [Optional, DefaultParameterValue(1f)] float startSizeMultiplier, [Optional, DefaultParameterValue(null)] object colorParameters)
        {
            PoolableParticleSystem system;
            if (this.isPersistentEffect(effectType))
            {
                system = PlayerView.Binder.PersistentParticleEffectPool.getObject(effectType);
            }
            else
            {
                system = this.m_dynamicParticleEffectPool.getObject(effectType);
            }
            system.Tm.position = followTm.position;
            system.gameObject.SetActive(true);
            system.ParticleSystem.Play();
            system.FollowTm = followTm;
            if (startSizeMultiplier != 1f)
            {
                system.setMultipliedStartSize(startSizeMultiplier, true);
            }
            if (colorParameters != null)
            {
                system.setColorParameters(colorParameters);
            }
            system.ManualDurationTimer = manualDuration;
            this.m_followParticleSystems.Add(system);
            this.m_allParticleSystems.Add(system);
            return system;
        }

        public PoolableParticleSystem playEffectStatic(Vector3 worldPos, EffectType effectType, [Optional, DefaultParameterValue(-1f)] float manualDuration, [Optional, DefaultParameterValue(false)] bool manualSimulationWithUnscaledTime, [Optional, DefaultParameterValue(1f)] float startSizeMultiplier, [Optional, DefaultParameterValue(null)] object colorParameters)
        {
            PoolableParticleSystem system;
            if (this.isPersistentEffect(effectType))
            {
                system = PlayerView.Binder.PersistentParticleEffectPool.getObject(effectType);
            }
            else
            {
                system = this.m_dynamicParticleEffectPool.getObject(effectType);
            }
            system.Tm.position = worldPos;
            system.gameObject.SetActive(true);
            if (startSizeMultiplier != 1f)
            {
                system.setMultipliedStartSize(startSizeMultiplier, true);
            }
            if (colorParameters != null)
            {
                system.setColorParameters(colorParameters);
            }
            system.ManualSimulationWithUnscaledTime = manualSimulationWithUnscaledTime;
            system.ParticleSystem.Play();
            system.ManualDurationTimer = manualDuration;
            this.m_allParticleSystems.Add(system);
            return system;
        }

        private void refreshAuraEffect(CharacterInstance c, EffectType effectType, bool auraActive)
        {
            PoolableParticleSystem pps = this.getFollowParticleSystem(c, effectType);
            if (auraActive)
            {
                if (pps == null)
                {
                    this.playEffectFollow(c.PhysicsBody.Transform, effectType, -1f, this.getParticleSystemStartSizeMultiplier(effectType, c), null);
                }
            }
            else if (pps != null)
            {
                this.stopFollowEffect(pps);
            }
        }

        private void refreshCharacterAuraEffects(CharacterInstance c)
        {
            if (c.IsPlayerCharacter || c.IsBoss)
            {
                IBuffSystem buffSystem = GameLogic.Binder.BuffSystem;
                this.refreshAuraEffect(c, EffectType.Aura_Ice, (c.getPerkInstanceCount(PerkType.AuraIce) > 0) || (c.getPerkInstanceCount(PerkType.BossAuraIce) > 0));
                this.refreshAuraEffect(c, EffectType.Aura_DamageBonus, (((buffSystem.hasBuffFromPerk(c, PerkType.AuraDamageBonus) || buffSystem.hasBuffFromPerk(c, PerkType.AuraLowHpArmor)) || (buffSystem.hasBuffFromPerk(c, PerkType.AuraLowHpAttackSpeed) || buffSystem.hasBuffFromPerk(c, PerkType.AuraLowHpDamage))) || (buffSystem.hasBuffFromPerk(c, PerkType.AuraLowHpDodge) || buffSystem.hasBuffFromPerk(c, PerkType.AuraFullHpDamage))) || buffSystem.hasBuffFromPerk(c, PerkType.BossAuraDamageOverTime));
                if (c.IsSupport && (c.Prefab == CharacterPrefab.KnightClone))
                {
                    PerkType type = c.getVisualRunestonePerkType(SkillType.Clone);
                    this.refreshAuraEffect(c, EffectType.Aura_Healing, type == PerkType.SkillUpgradeClone1);
                    this.refreshAuraEffect(c, EffectType.Aura_Decoy, type == PerkType.SkillUpgradeClone3);
                    this.refreshAuraEffect(c, EffectType.Aura_Mana, type == PerkType.SkillUpgradeClone4);
                }
            }
        }

        public void setFollowParticleSystemsVisibility(CharacterInstance c, bool visible)
        {
            for (int i = 0; i < this.m_followParticleSystems.Count; i++)
            {
                PoolableParticleSystem system = this.m_followParticleSystems[i];
                bool flag = false;
                if (system.FollowTm == c.PhysicsBody.Transform)
                {
                    flag = true;
                }
                else
                {
                    CharacterView view = PlayerView.Binder.RoomView.getCharacterViewForCharacter(c);
                    if ((view != null) && (system.FollowTm == view.Transform))
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    if (visible)
                    {
                        GameObjectExtensions.SetLayerRecursively(system.gameObject, system.OriginalLayer);
                    }
                    else
                    {
                        GameObjectExtensions.SetLayerRecursively(system.gameObject, Layers.TRANSPARENT_FX);
                    }
                }
            }
        }

        public void stopFollowEffect(PoolableParticleSystem pps)
        {
            if (pps != null)
            {
                pps.FollowTm = null;
                pps.stopAndFadeOutParticles();
                this.m_followParticleSystems.Remove(pps);
                if (LINKED_DESTRUCTION_EFFECTS.ContainsKey(pps.EffectType))
                {
                    this.playEffectStatic(pps.Tm.position, LINKED_DESTRUCTION_EFFECTS[pps.EffectType], -1f, false, 1f, null);
                }
            }
        }

        public Transform Tm
        {
            [CompilerGenerated]
            get
            {
                return this.<Tm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Tm>k__BackingField = value;
            }
        }
    }
}

