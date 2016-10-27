namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class LeapSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorD4 rd = new <ExecuteRoutine>c__IteratorD4();
            rd.c = c;
            rd.executionStats = executionStats;
            rd.<$>c = c;
            rd.<$>executionStats = executionStats;
            return rd;
        }

        public static void PreExecute(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            Vector3 forward = c.PhysicsBody.Transform.forward;
            float movementForce = ConfigSkills.Leap.MovementForce;
            CharacterInstance b = activeDungeon.ActiveRoom.getClosestEnemyCharacter(c, true);
            if ((b != null) && (PhysicsUtil.DistBetween(c, b) <= ConfigSkills.Leap.ClosestEnemyDistanceThreshold))
            {
                Vector3 vector2 = b.PhysicsBody.Transform.position - c.PhysicsBody.Transform.position;
                forward = Vector3Extensions.ToXzVector3(vector2.normalized);
            }
            executionStats.MovementDir = forward;
            executionStats.MovementForce = movementForce;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorD4 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal SkillExecutionStats <$>executionStats;
            internal ActiveDungeon <ad>__0;
            internal bool <blastExecuted>__5;
            internal float <blastRadius>__13;
            internal double <damage>__14;
            internal Vector3 <dashDir>__3;
            internal List<CharacterInstance> <enemiesAround>__15;
            internal List<CharacterInstance> <enemiesAroundUs>__9;
            internal HashSet<CharacterInstance> <enemiesPushedBack>__2;
            internal CharacterInstance <enemy>__11;
            internal CharacterInstance <enemy>__17;
            internal int <i>__10;
            internal int <i>__16;
            internal float <newSpeed>__8;
            internal float <pushAndBlastRadius>__1;
            internal Vector3 <pushDirectionXz>__12;
            internal Vector3 <pushDirectionXz>__18;
            internal Vector3 <vel>__4;
            internal float <velMagnitude>__7;
            internal float <velSqrMag>__6;
            internal CharacterInstance c;
            internal SkillExecutionStats executionStats;

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
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<pushAndBlastRadius>__1 = ConfigSkills.Leap.PushAndBlastRadius;
                        this.<enemiesPushedBack>__2 = new HashSet<CharacterInstance>();
                        this.<dashDir>__3 = this.executionStats.MovementDir;
                        if (this.c.getPerkInstanceCount(PerkType.SkillUpgradeLeap4) > 0)
                        {
                            CloneSkill.Summon(this.c, this.c.PhysicsBody.Transform.position, this.c.getSkillInstance(SkillType.Leap), ConfigSkills.Leap.UpgradedNumCloneInstancesAllowed);
                        }
                        this.c.PhysicsBody.gameObject.layer = Layers.IGNORE_CHARACTERS;
                        this.<vel>__4 = (Vector3) (this.<dashDir>__3 * this.executionStats.MovementForce);
                        this.<blastExecuted>__5 = false;
                        this.<velSqrMag>__6 = this.<vel>__4.sqrMagnitude;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0552;
                }
                if (this.<velSqrMag>__6 > 25f)
                {
                    if (!this.<blastExecuted>__5)
                    {
                        this.<velSqrMag>__6 = this.<vel>__4.sqrMagnitude;
                        CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, this.<vel>__4, this.<vel>__4.normalized);
                        if (this.executionStats.MovementLinearDeceleration != 0f)
                        {
                            this.<velMagnitude>__7 = this.<vel>__4.magnitude;
                            this.<newSpeed>__8 = this.<velMagnitude>__7 + (this.executionStats.MovementLinearDeceleration * Time.fixedDeltaTime);
                            this.<vel>__4 = Vector3.ClampMagnitude(this.<vel>__4, Mathf.Clamp(this.<newSpeed>__8, 0f, float.MaxValue));
                        }
                        else
                        {
                            PhysicsUtil.ApplyDrag(ref this.<vel>__4, ConfigSkills.Leap.DragPerSecond, Time.fixedDeltaTime);
                        }
                        this.<enemiesAroundUs>__9 = this.<ad>__0.ActiveRoom.getEnemyCharactersWithinRadius(this.c.PhysicsBody.Transform.position, this.<pushAndBlastRadius>__1, this.c);
                        this.<i>__10 = 0;
                        while (this.<i>__10 < this.<enemiesAroundUs>__9.Count)
                        {
                            this.<enemy>__11 = this.<enemiesAroundUs>__9[this.<i>__10];
                            if (!this.<enemiesPushedBack>__2.Contains(this.<enemy>__11))
                            {
                                this.<pushDirectionXz>__12 = Vector3Extensions.ToXzVector3(this.<enemy>__11.PhysicsBody.Transform.position - this.c.PhysicsBody.Transform.position).normalized;
                                GameLogic.Binder.CommandProcessor.executeCharacterSpecific(this.<enemy>__11, new CmdPushCharacter(this.<enemy>__11, (Vector3) ((this.<pushDirectionXz>__12 * ConfigSkills.Leap.PushForcePerVelocity) * this.<vel>__4.magnitude)), 0f);
                                this.<enemiesPushedBack>__2.Add(this.<enemy>__11);
                            }
                            this.<i>__10++;
                        }
                        if ((this.<velSqrMag>__6 < 30f) && !this.<blastExecuted>__5)
                        {
                            this.<blastRadius>__13 = this.<pushAndBlastRadius>__1 * ConfigSkills.Leap.PostBlastRadius;
                            CmdHitDestructibles.ExecuteStatic(this.c, this.c.PhysicsBody.Transform.position, this.<blastRadius>__13, SkillType.Leap);
                            this.<damage>__14 = MathUtil.ClampMin(this.c.SkillDamage(true), 1.0);
                            this.<damage>__14 *= !this.c.IsBoss ? ((double) ConfigSkills.Leap.DamagePct) : ((double) ConfigSkills.BossLeap.DamagePct);
                            this.<damage>__14 = CharacterStatModifierUtil.ApplySkillTypeDamageBonuses(this.c, SkillType.Leap, this.<damage>__14);
                            if (this.c.IsPet)
                            {
                                this.<damage>__14 = this.c.OwningPlayer.ActiveCharacter.DamagePerHit(false);
                            }
                            this.<enemiesAround>__15 = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.getEnemyCharactersWithinRadius(this.c.PhysicsBody.Transform.position, this.<blastRadius>__13, this.c);
                            this.<i>__16 = 0;
                            while (this.<i>__16 < this.<enemiesAround>__15.Count)
                            {
                                this.<enemy>__17 = this.<enemiesAround>__15[this.<i>__16];
                                this.<pushDirectionXz>__18 = Vector3Extensions.ToXzVector3(this.<enemy>__17.PhysicsBody.Transform.position - this.c.PhysicsBody.Transform.position).normalized;
                                GameLogic.Binder.CommandProcessor.executeCharacterSpecific(this.<enemy>__17, new CmdPushCharacter(this.<enemy>__17, (Vector3) (this.<pushDirectionXz>__18 * ConfigSkills.Leap.PostBlastPushForce)), 0f);
                                CmdDealDamageToCharacter.ExecuteStatic(this.c, this.<enemy>__17, this.<damage>__14, false, DamageType.Magic, SkillType.Leap);
                                if (this.<enemy>__17.IsDead)
                                {
                                    this.executionStats.KillCount++;
                                }
                                this.<i>__16++;
                            }
                            this.executionStats.EnemiesAround = this.<enemiesAround>__15.Count;
                            GameLogic.Binder.EventBus.CharacterSkillExecutionMidpoint(this.c, SkillType.Leap, this.executionStats);
                            this.<blastExecuted>__5 = true;
                        }
                        this.$current = new WaitForFixedUpdate();
                        this.$PC = 1;
                        return true;
                    }
                    goto Label_0552;
                    this.$PC = -1;
                }
            Label_0552:
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

