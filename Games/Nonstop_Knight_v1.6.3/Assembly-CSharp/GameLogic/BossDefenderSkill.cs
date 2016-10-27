namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BossDefenderSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorC2 rc = new <ExecuteRoutine>c__IteratorC2();
            rc.c = c;
            rc.<$>c = c;
            return rc;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorC2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal Buff <buff>__1;
            internal float <durationSeconds>__0;
            internal IEnumerator <ie>__2;
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
                    {
                        CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, Vector3.zero, this.c.Facing);
                        this.<durationSeconds>__0 = ConfigSkills.BossDefender.Duration;
                        Buff buff = new Buff();
                        buff.BaseStat1 = BaseStatProperty.UniversalArmorBonus;
                        buff.Modifier = ConfigSkills.BossDefender.ArmorBonus;
                        buff.DurationSeconds = this.<durationSeconds>__0;
                        this.<buff>__1 = buff;
                        GameLogic.Binder.BuffSystem.startBuff(this.c, this.<buff>__1);
                        this.<ie>__2 = TimeUtil.WaitForFixedSeconds(this.<durationSeconds>__0);
                        break;
                    }
                    case 1:
                        break;

                    default:
                        goto Label_00D8;
                }
                if (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 1;
                    return true;
                }
                goto Label_00D8;
                this.$PC = -1;
            Label_00D8:
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

