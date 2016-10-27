namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class FireballSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank)
        {
            <ExecuteRoutine>c__IteratorD0 rd = new <ExecuteRoutine>c__IteratorD0();
            rd.c = c;
            rd.<$>c = c;
            return rd;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorD0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal double <damage>__1;
            internal List<CharacterInstance> <enemiesAroundTargetedPoint>__2;
            internal CharacterInstance <enemy>__4;
            internal int <i>__3;
            internal Vector3 <pushDirectionXz>__5;
            internal Vector3 <targetWorldPos>__0;
            internal CharacterInstance c;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    this.<targetWorldPos>__0 = this.c.ManualTargetPos;
                    this.<damage>__1 = this.c.SkillDamage(true) * ConfigSkills.Fireball.DamagePct;
                    this.<enemiesAroundTargetedPoint>__2 = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.getEnemyCharactersWithinRadius(this.<targetWorldPos>__0, ConfigSkills.Fireball.Radius, this.c);
                    this.<i>__3 = 0;
                    while (this.<i>__3 < this.<enemiesAroundTargetedPoint>__2.Count)
                    {
                        this.<enemy>__4 = this.<enemiesAroundTargetedPoint>__2[this.<i>__3];
                        this.<pushDirectionXz>__5 = Vector3Extensions.ToXzVector3(this.<enemy>__4.PhysicsBody.Transform.position - this.c.PhysicsBody.Transform.position).normalized;
                        GameLogic.Binder.CommandProcessor.executeCharacterSpecific(this.<enemy>__4, new CmdPushCharacter(this.<enemy>__4, (Vector3) (this.<pushDirectionXz>__5 * ConfigSkills.Fireball.PushForce)), 0f);
                        CmdDealDamageToCharacter.ExecuteStatic(this.c, this.<enemy>__4, this.<damage>__1, false, DamageType.Magic, SkillType.Fireball);
                        this.<i>__3++;
                    }
                }
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

