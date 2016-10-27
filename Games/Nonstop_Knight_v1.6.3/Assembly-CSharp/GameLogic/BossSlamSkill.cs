namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BossSlamSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorC5 rc = new <ExecuteRoutine>c__IteratorC5();
            rc.c = c;
            rc.<$>c = c;
            return rc;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorC5 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal Vector3 <dashDir>__0;
            internal double <hpBeforeWait>__3;
            internal IEnumerator <ie>__4;
            internal Vector3 <vel>__1;
            internal float <velSqrMag>__2;
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
                        this.<dashDir>__0 = -this.c.PhysicsBody.Transform.forward;
                        this.c.PhysicsBody.gameObject.layer = Layers.IGNORE_CHARACTERS;
                        this.<vel>__1 = (Vector3) (this.<dashDir>__0 * ConfigSkills.BossSlam.MovementForce);
                        this.<velSqrMag>__2 = this.<vel>__1.sqrMagnitude;
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_017C;

                    default:
                        goto Label_01B0;
                }
                if (this.<velSqrMag>__2 > 25f)
                {
                    this.<velSqrMag>__2 = this.<vel>__1.sqrMagnitude;
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, this.<vel>__1, this.c.Facing);
                    PhysicsUtil.ApplyDrag(ref this.<vel>__1, ConfigSkills.BossSlam.DragPerSecond, Time.fixedDeltaTime);
                    this.$current = new WaitForFixedUpdate();
                    this.$PC = 1;
                    goto Label_01B2;
                }
                CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, Vector3.zero, this.c.Facing);
                if (ConfigSkills.BossSlam.PostLeapWaitTime <= 0f)
                {
                    goto Label_018C;
                }
                this.<hpBeforeWait>__3 = this.c.CurrentHp;
                this.<ie>__4 = TimeUtil.WaitForFixedSeconds(ConfigSkills.BossSlam.PostLeapWaitTime);
            Label_017C:
                while (this.<ie>__4.MoveNext())
                {
                    if (this.c.CurrentHp < this.<hpBeforeWait>__3)
                    {
                        break;
                    }
                    this.$current = this.<ie>__4.Current;
                    this.$PC = 2;
                    goto Label_01B2;
                }
            Label_018C:
                GameLogic.Binder.SkillSystem.activateSkill(this.c, SkillType.Slam, 0f, null);
                goto Label_01B0;
                this.$PC = -1;
            Label_01B0:
                return false;
            Label_01B2:
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

