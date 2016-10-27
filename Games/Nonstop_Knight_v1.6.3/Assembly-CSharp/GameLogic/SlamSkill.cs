namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class SlamSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorDB rdb = new <ExecuteRoutine>c__IteratorDB();
            rdb.c = c;
            rdb.<$>c = c;
            return rdb;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorDB : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal AreaEffect <areaEffect>__11;
            internal AreaEffect <areaEffect>__6;
            internal AreaEffect.AreaEffectProperties <areaEffectProps>__13;
            internal AreaEffect.AreaEffectProperties <areaEffectProps>__8;
            internal AreaEffectType <areaEffectType>__10;
            internal double <damagePerProjectileHit>__0;
            internal bool <hasQuakeRune>__1;
            internal bool <hasSplashRune>__2;
            internal bool <hasStunRune>__3;
            internal IEnumerator <ie>__17;
            internal Projectile <p>__15;
            internal Projectile.ProjectileProperties <projectileProps>__12;
            internal Projectile.ProjectileProperties <projectileProps>__7;
            internal Projectile.ProjectileProperties <props>__16;
            internal Vector3 <spawnPt>__14;
            internal Vector3 <spawnPt>__5;
            internal Vector3 <spawnPt>__9;
            internal float <stunDurationSeconds>__4;
            internal CharacterInstance c;

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
                        this.<damagePerProjectileHit>__0 = !this.c.IsPet ? this.c.SkillDamage(true) : this.c.OwningPlayer.ActiveCharacter.DamagePerHit(false);
                        this.<damagePerProjectileHit>__0 = MathUtil.ClampMin(this.<damagePerProjectileHit>__0, 1.0);
                        if (!this.c.IsBoss)
                        {
                            this.<damagePerProjectileHit>__0 *= ConfigSkills.Slam.DamagePct;
                            break;
                        }
                        this.<damagePerProjectileHit>__0 *= ConfigSkills.BossSlam.DamagePct;
                        break;

                    case 1:
                        goto Label_0575;

                    default:
                        goto Label_0591;
                }
                this.<damagePerProjectileHit>__0 = CharacterStatModifierUtil.ApplySkillTypeDamageBonuses(this.c, SkillType.Slam, this.<damagePerProjectileHit>__0);
                this.<hasQuakeRune>__1 = this.c.getPerkInstanceCount(PerkType.SkillUpgradeSlam2) > 0;
                this.<hasSplashRune>__2 = this.c.getPerkInstanceCount(PerkType.SkillUpgradeSlam4) > 0;
                this.<hasStunRune>__3 = this.c.getPerkInstanceCount(PerkType.SkillUpgradeSlam3) > 0;
                this.<stunDurationSeconds>__4 = !this.<hasStunRune>__3 ? 0f : ConfigPerks.SHARED_DATA[PerkType.SkillUpgradeSlam3].DurationSeconds;
                if (this.<hasQuakeRune>__1)
                {
                    this.<spawnPt>__5 = this.c.PhysicsBody.Transform.position + ((Vector3) (this.c.Facing * ConfigSkills.Slam.RockConcentrationCenterDistance));
                    this.<spawnPt>__5.y = 0f;
                    this.<areaEffect>__6 = GameLogic.Binder.AreaEffectPool.getObject(AreaEffectType.RockConcentration);
                    this.<areaEffect>__6.gameObject.SetActive(true);
                    Projectile.ProjectileProperties properties = new Projectile.ProjectileProperties();
                    properties.Type = ProjectileType.Slam;
                    properties.Radius = ConfigSkills.Slam.RockConcentrationProjectileRadius;
                    properties.Speed = 2f;
                    properties.DamagePerHit = this.<damagePerProjectileHit>__0 * ConfigSkills.Slam.RockConcentrationDamagePct;
                    properties.MaxLifetime = ConfigSkills.Slam.RockConcentrationProjectileLifetime;
                    properties.DamageType = DamageType.Magic;
                    properties.FromSkill = SkillType.Slam;
                    properties.StartSizeMultiplier = ConfigSkills.Slam.RockConcentrationProjectileVisualScale;
                    properties.StunDurationSeconds = this.<stunDurationSeconds>__4;
                    this.<projectileProps>__7 = properties;
                    AreaEffect.AreaEffectProperties properties2 = new AreaEffect.AreaEffectProperties();
                    properties2.MaxLifetime = ConfigPerks.SHARED_DATA[PerkType.SkillUpgradeSlam2].DurationSeconds;
                    properties2.CustomProperties = this.<projectileProps>__7;
                    this.<areaEffectProps>__8 = properties2;
                    this.<areaEffect>__6.spawn(this.c, this.<spawnPt>__5, this.<areaEffectProps>__8);
                }
                if (this.<hasSplashRune>__2)
                {
                    this.<spawnPt>__9 = this.c.PhysicsBody.Transform.position;
                    this.<spawnPt>__9.y = 0f;
                    this.<areaEffectType>__10 = !this.<hasQuakeRune>__1 ? AreaEffectType.RockRing : AreaEffectType.RockRingSector;
                    this.<areaEffect>__11 = GameLogic.Binder.AreaEffectPool.getObject(this.<areaEffectType>__10);
                    this.<areaEffect>__11.gameObject.SetActive(true);
                    Projectile.ProjectileProperties properties3 = new Projectile.ProjectileProperties();
                    properties3.Type = ProjectileType.Slam;
                    properties3.Radius = ConfigSkills.Slam.RockRingProjectileRadius;
                    properties3.Speed = 1f;
                    properties3.DamagePerHit = this.<damagePerProjectileHit>__0 * ConfigSkills.Slam.RockRingDamagePct;
                    properties3.MaxLifetime = ConfigSkills.Slam.RockRingProjectileLifetime;
                    properties3.DamageType = DamageType.Magic;
                    properties3.FromSkill = SkillType.Slam;
                    properties3.StartSizeMultiplier = ConfigSkills.Slam.RockRingProjectileVisualScale;
                    properties3.StunDurationSeconds = this.<stunDurationSeconds>__4;
                    this.<projectileProps>__12 = properties3;
                    if (this.c.IsPet)
                    {
                        this.<projectileProps>__12.MaxLifetime *= ConfigSkills.Slam.RockRingScalePet;
                        this.<projectileProps>__12.Radius *= ConfigSkills.Slam.RockRingScalePet;
                        this.<projectileProps>__12.StartSizeMultiplier *= ConfigSkills.Slam.RockRingScalePet;
                    }
                    AreaEffect.AreaEffectProperties properties4 = new AreaEffect.AreaEffectProperties();
                    properties4.MaxLifetime = ConfigPerks.SHARED_DATA[PerkType.SkillUpgradeSlam4].DurationSeconds;
                    properties4.CustomProperties = this.<projectileProps>__12;
                    this.<areaEffectProps>__13 = properties4;
                    this.<areaEffect>__11.spawn(this.c, this.<spawnPt>__9, this.<areaEffectProps>__13);
                }
                if (!this.<hasQuakeRune>__1 && !this.<hasSplashRune>__2)
                {
                    this.<spawnPt>__14 = this.c.PhysicsBody.Transform.position + ((Vector3) (this.c.Facing * 1.5f));
                    this.<spawnPt>__14.y = 0.1f;
                    this.<p>__15 = GameLogic.Binder.ProjectilePool.getObject();
                    this.<p>__15.gameObject.SetActive(true);
                    Projectile.ProjectileProperties properties5 = new Projectile.ProjectileProperties();
                    properties5.Type = ProjectileType.Slam;
                    properties5.Radius = ConfigSkills.Slam.Radius;
                    properties5.Speed = ConfigSkills.Slam.Speed;
                    properties5.DamagePerHit = this.<damagePerProjectileHit>__0;
                    properties5.MaxLifetime = 10f;
                    properties5.RequireGroundTouch = true;
                    properties5.DamageType = DamageType.Magic;
                    properties5.FromSkill = SkillType.Slam;
                    properties5.StunDurationSeconds = this.<stunDurationSeconds>__4;
                    this.<props>__16 = properties5;
                    this.<p>__15.spawn(this.c, this.<spawnPt>__14, this.c.Facing, this.<props>__16);
                }
                CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, Vector3.zero, this.c.Facing);
                this.<ie>__17 = TimeUtil.WaitForFixedSeconds(ConfigSkills.Slam.WaitBeforeMoving);
            Label_0575:
                while (this.<ie>__17.MoveNext())
                {
                    this.$current = this.<ie>__17.Current;
                    this.$PC = 1;
                    return true;
                }
                goto Label_0591;
                this.$PC = -1;
            Label_0591:
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

