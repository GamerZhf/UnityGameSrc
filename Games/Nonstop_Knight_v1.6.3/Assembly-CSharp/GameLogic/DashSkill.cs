namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class DashSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorCC rcc = new <ExecuteRoutine>c__IteratorCC();
            rcc.c = c;
            rcc.executionStats = executionStats;
            rcc.<$>c = c;
            rcc.<$>executionStats = executionStats;
            return rcc;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorCC : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal SkillExecutionStats <$>executionStats;
            internal Room <activeRoom>__0;
            internal CharacterInstance <closestEnemy>__2;
            internal double <damage>__9;
            internal Vector3 <dashDir>__4;
            internal float <distToStopPoint>__3;
            internal float <distTraveled>__6;
            internal float <maxDistTraveled>__1;
            internal Vector3 <pushDirectionXz>__10;
            internal bool <skillMidpointReached>__7;
            internal CharacterInstance <targetCharacter>__8;
            internal Vector3 <vel>__5;
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
                        this.<activeRoom>__0 = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom;
                        this.<maxDistTraveled>__1 = ConfigSkills.Dash.MaxDistanceTraveled;
                        this.<closestEnemy>__2 = this.<activeRoom>__0.getClosestEnemyCharacter(this.c, true);
                        if ((this.<closestEnemy>__2 == null) || (PhysicsUtil.DistBetween(this.c, this.<closestEnemy>__2) > (this.<maxDistTraveled>__1 * 1.25f)))
                        {
                            CmdSetCharacterTarget.ExecuteStatic(this.c, null, Vector3.zero);
                            break;
                        }
                        CmdSetCharacterTarget.ExecuteStatic(this.c, this.<closestEnemy>__2, Vector3.zero);
                        break;

                    case 1:
                        goto Label_023A;

                    default:
                        goto Label_03C7;
                }
                this.<distToStopPoint>__3 = this.<maxDistTraveled>__1;
                if (this.c.TargetCharacter != null)
                {
                    this.<dashDir>__4 = Vector3Extensions.ToXzVector3(this.c.TargetCharacter.PhysicsBody.Transform.position - this.c.PhysicsBody.Transform.position).normalized;
                    this.<distToStopPoint>__3 = PhysicsUtil.DistBetween(this.c, this.c.TargetCharacter);
                    this.executionStats.EnemiesAround = 1;
                }
                else
                {
                    this.<dashDir>__4 = this.c.PhysicsBody.Transform.forward;
                }
                this.c.PhysicsBody.gameObject.layer = Layers.IGNORE_CHARACTERS;
                this.<vel>__5 = (Vector3) (this.<dashDir>__4 * ConfigSkills.Dash.MovementForce);
                this.<distTraveled>__6 = 0f;
                this.<skillMidpointReached>__7 = false;
            Label_023A:
                while ((this.<distTraveled>__6 < this.<maxDistTraveled>__1) && !this.<activeRoom>__0.characterWithinAttackDistance(this.c, this.c.TargetCharacter))
                {
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, this.<vel>__5, this.<vel>__5.normalized);
                    this.<distTraveled>__6 += this.<vel>__5.magnitude * Time.fixedDeltaTime;
                    if (!this.<skillMidpointReached>__7 && (Mathf.Clamp01(this.<distTraveled>__6 / this.<distToStopPoint>__3) >= ConfigSkills.Dash.NormalizedExecutionMidpoint))
                    {
                        GameLogic.Binder.EventBus.CharacterSkillExecutionMidpoint(this.c, SkillType.Dash, this.executionStats);
                        this.<skillMidpointReached>__7 = true;
                    }
                    this.$current = new WaitForFixedUpdate();
                    this.$PC = 1;
                    return true;
                }
                CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, Vector3.zero, this.c.Facing);
                if (this.<activeRoom>__0.characterWithinAttackDistance(this.c, this.c.TargetCharacter))
                {
                    this.<targetCharacter>__8 = this.c.TargetCharacter;
                    this.<damage>__9 = this.c.SkillDamage(true) * ConfigSkills.Dash.DamagePct;
                    CmdDealDamageToCharacter.ExecuteStatic(this.c, this.<targetCharacter>__8, this.<damage>__9, false, DamageType.Magic, SkillType.Dash);
                    if (this.<targetCharacter>__8.IsDead)
                    {
                        this.executionStats.KillCount++;
                    }
                    if ((ConfigSkills.Dash.PushForce > 0f) && !this.<targetCharacter>__8.IsDead)
                    {
                        this.<pushDirectionXz>__10 = Vector3Extensions.ToXzVector3(this.c.TargetCharacter.PhysicsBody.Transform.position - this.c.PhysicsBody.Transform.position).normalized;
                        GameLogic.Binder.CommandProcessor.executeCharacterSpecific(this.c.TargetCharacter, new CmdPushCharacter(this.c.TargetCharacter, (Vector3) (this.<pushDirectionXz>__10 * ConfigSkills.Dash.PushForce)), 0f);
                    }
                }
                this.c.resetAttackTimer();
                goto Label_03C7;
                this.$PC = -1;
            Label_03C7:
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

