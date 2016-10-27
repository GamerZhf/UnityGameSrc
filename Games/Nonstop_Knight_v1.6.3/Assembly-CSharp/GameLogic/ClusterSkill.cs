namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ClusterSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorCB rcb = new <ExecuteRoutine>c__IteratorCB();
            rcb.c = c;
            rcb.<$>c = c;
            return rcb;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorCB : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal float <angle>__5;
            internal float <angleInterval>__7;
            internal float <easedV>__4;
            internal int <i>__6;
            internal IEnumerator <ie>__12;
            internal float <normalizedBreakTime>__3;
            internal int <numProjectilesThrown>__2;
            internal Projectile <p>__10;
            internal Projectile.ProjectileProperties <props>__11;
            internal Vector3 <spawnPt>__9;
            internal ManualTimer <spinTimer>__0;
            internal Vector3 <startForward>__1;
            internal Vector3 <throwDir>__8;
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
                        CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, Vector3.zero, this.c.Facing);
                        this.<spinTimer>__0 = new ManualTimer(ConfigSkills.Cluster.SpinDuration);
                        this.<startForward>__1 = this.c.PhysicsBody.Transform.forward;
                        this.<numProjectilesThrown>__2 = 0;
                        this.<normalizedBreakTime>__3 = 0.75f;
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_035F;

                    default:
                        goto Label_037B;
                }
                if (!this.<spinTimer>__0.Idle)
                {
                    this.<easedV>__4 = Easing.Apply(this.<spinTimer>__0.normalizedProgress(), Easing.Function.OUT_CUBIC);
                    this.<angle>__5 = 360f * this.<easedV>__4;
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, this.c.Velocity, (Vector3) (Quaternion.Euler(0f, -this.<angle>__5, 0f) * this.<startForward>__1));
                    this.<spinTimer>__0.tick(Time.fixedDeltaTime);
                    if ((this.<numProjectilesThrown>__2 == 0) && (this.<angle>__5 > 0f))
                    {
                        this.<i>__6 = 0;
                        while (this.<i>__6 < ConfigSkills.Cluster.NumProjectiles)
                        {
                            this.<angleInterval>__7 = ConfigSkills.Cluster.ThrowArc / ((float) ConfigSkills.Cluster.NumProjectiles);
                            this.<throwDir>__8 = (Vector3) (Quaternion.AngleAxis((ConfigSkills.Cluster.ThrowArc * 0.5f) - (this.<angleInterval>__7 * this.<i>__6), Vector3.up) * this.<startForward>__1);
                            this.<spawnPt>__9 = this.c.PhysicsBody.Transform.position + ((Vector3) (this.<throwDir>__8 * 1f));
                            this.<spawnPt>__9.y = 1.5f;
                            this.<throwDir>__8.y += ConfigSkills.Cluster.VerticalLift;
                            this.<p>__10 = GameLogic.Binder.ProjectilePool.getObject();
                            this.<p>__10.gameObject.SetActive(true);
                            Projectile.ProjectileProperties properties = new Projectile.ProjectileProperties();
                            properties.Type = ProjectileType.Cluster;
                            properties.Radius = ConfigSkills.Cluster.ProjectileRadius;
                            properties.Speed = UnityEngine.Random.Range(ConfigSkills.Cluster.ProjectileSpeedMin, ConfigSkills.Cluster.ProjectileSpeedMax);
                            properties.DamagePerHit = MathUtil.ClampMin(this.c.SkillDamage(true), 1.0) * ConfigSkills.Cluster.ProjectileDamagePct;
                            properties.DestroyAfterContact = true;
                            properties.MaxLifetime = 5f;
                            properties.UsesGravity = true;
                            properties.GravityFactor = ConfigSkills.Cluster.ProjectileGravityFactor;
                            properties.CollideWithGround = true;
                            properties.DamageType = DamageType.Magic;
                            this.<props>__11 = properties;
                            this.<p>__10.spawn(this.c, this.<spawnPt>__9, this.<throwDir>__8, this.<props>__11);
                            this.<numProjectilesThrown>__2++;
                            this.<i>__6++;
                        }
                    }
                    if (this.<spinTimer>__0.normalizedProgress() < this.<normalizedBreakTime>__3)
                    {
                        this.$current = new WaitForFixedUpdate();
                        this.$PC = 1;
                        goto Label_037D;
                    }
                }
                if (ConfigSkills.Cluster.WaitAfterSpin <= 0f)
                {
                    goto Label_037B;
                }
                CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, Vector3.zero, this.c.Facing);
                this.<ie>__12 = TimeUtil.WaitForFixedSeconds(ConfigSkills.Cluster.WaitAfterSpin);
            Label_035F:
                while (this.<ie>__12.MoveNext())
                {
                    this.$current = this.<ie>__12.Current;
                    this.$PC = 2;
                    goto Label_037D;
                }
                goto Label_037B;
                this.$PC = -1;
            Label_037B:
                return false;
            Label_037D:
                return true;
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

