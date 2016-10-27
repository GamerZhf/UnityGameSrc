namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ChargeSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorC9 rc = new <ExecuteRoutine>c__IteratorC9();
            rc.c = c;
            rc.executionStats = executionStats;
            rc.<$>c = c;
            rc.<$>executionStats = executionStats;
            return rc;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorC9 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal SkillExecutionStats <$>executionStats;
            internal Vector3 <chargeDirXz>__0;
            internal double <damage>__5;
            internal float <newSpeed>__8;
            internal float <pointBlankRange>__3;
            internal Vector3 <pushDirectionXz>__6;
            internal CharacterInstance <targetCharacter>__4;
            internal float <timeElapsed>__2;
            internal Vector3 <v>__7;
            internal Vector3 <vel>__1;
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
                        if (this.c.TargetCharacter != null)
                        {
                            this.c.PhysicsBody.gameObject.layer = Layers.IGNORE_CHARACTERS;
                            this.<chargeDirXz>__0 = Vector3Extensions.ToXzVector3(this.c.TargetCharacter.PhysicsBody.Transform.position - this.c.PhysicsBody.Transform.position).normalized;
                            this.executionStats.EnemiesAround = 1;
                            this.<vel>__1 = (Vector3) (this.<chargeDirXz>__0 * ConfigSkills.Charge.MovementForce);
                            this.<timeElapsed>__2 = 0f;
                            this.<pointBlankRange>__3 = this.c.Radius * 3f;
                            break;
                        }
                        goto Label_03EB;

                    case 1:
                        break;

                    case 2:
                        goto Label_03AF;

                    default:
                        goto Label_03EB;
                }
                while (((this.<timeElapsed>__2 < 2f) && (this.c.TargetCharacter != null)) && (!this.c.TargetCharacter.IsDead && (PhysicsUtil.DistBetween(this.c, this.c.TargetCharacter) > this.<pointBlankRange>__3)))
                {
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, this.<vel>__1, this.<vel>__1.normalized);
                    this.<timeElapsed>__2 += Time.fixedDeltaTime * Time.timeScale;
                    this.$current = new WaitForFixedUpdate();
                    this.$PC = 1;
                    goto Label_03ED;
                }
                CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, Vector3.zero, this.c.Facing);
                this.c.assignToDefaultLayer();
                if ((this.c.TargetCharacter == null) || this.c.TargetCharacter.IsDead)
                {
                    goto Label_03EB;
                }
                GameLogic.Binder.EventBus.CharacterSkillExecutionMidpoint(this.c, SkillType.Charge, this.executionStats);
                this.<targetCharacter>__4 = this.c.TargetCharacter;
                this.<damage>__5 = this.c.SkillDamage(true) * ConfigSkills.Charge.DamagePct;
                if (this.c.IsPet)
                {
                    this.<damage>__5 = this.c.OwningPlayer.ActiveCharacter.DamagePerHit(false);
                }
                CmdDealDamageToCharacter.ExecuteStatic(this.c, this.<targetCharacter>__4, this.<damage>__5, false, DamageType.Magic, SkillType.Charge);
                if (this.<targetCharacter>__4.IsDead)
                {
                    this.executionStats.KillCount++;
                }
                if ((ConfigSkills.Charge.PushForce > 0f) && !this.<targetCharacter>__4.IsDead)
                {
                    this.<pushDirectionXz>__6 = Vector3Extensions.ToXzVector3(this.c.TargetCharacter.PhysicsBody.Transform.position - this.c.PhysicsBody.Transform.position).normalized;
                    GameLogic.Binder.CommandProcessor.executeCharacterSpecific(this.c.TargetCharacter, new CmdPushCharacter(this.c.TargetCharacter, (Vector3) (this.<pushDirectionXz>__6 * ConfigSkills.Charge.PushForce)), 0f);
                }
                this.<v>__7 = this.<vel>__1;
            Label_03AF:
                while (this.<v>__7.sqrMagnitude > 500f)
                {
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, this.<v>__7, this.<v>__7.normalized);
                    this.<newSpeed>__8 = this.<v>__7.magnitude - ((500f * Time.fixedDeltaTime) * Time.timeScale);
                    this.<v>__7 = Vector3.ClampMagnitude(this.<v>__7, Mathf.Clamp(this.<newSpeed>__8, 0f, float.MaxValue));
                    this.$current = new WaitForFixedUpdate();
                    this.$PC = 2;
                    goto Label_03ED;
                }
                CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, Vector3.zero, this.c.Facing);
                goto Label_03EB;
                this.$PC = -1;
            Label_03EB:
                return false;
            Label_03ED:
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

