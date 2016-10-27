namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class ExplosionSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats, [Optional, DefaultParameterValue(0.25f)] float damagePct)
        {
            <ExecuteRoutine>c__IteratorCF rcf = new <ExecuteRoutine>c__IteratorCF();
            rcf.c = c;
            rcf.executionStats = executionStats;
            rcf.damagePct = damagePct;
            rcf.<$>c = c;
            rcf.<$>executionStats = executionStats;
            rcf.<$>damagePct = damagePct;
            return rcf;
        }

        public static void ExecuteStatic(CharacterInstance c, Vector3 position, [Optional, DefaultParameterValue(null)] SkillExecutionStats executionStats, [Optional, DefaultParameterValue(0.25f)] float damagePct)
        {
            if ((c == null) || !c.IsDead)
            {
                List<CharacterInstance> list = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.getCharactersWithinRadius(position, ConfigSkills.Explosion.Radius);
                for (int i = 0; i < list.Count; i++)
                {
                    CharacterInstance character = list[i];
                    Vector3 normalized = Vector3Extensions.ToXzVector3(character.PhysicsBody.Transform.position - position).normalized;
                    GameLogic.Binder.CommandProcessor.executeCharacterSpecific(character, new CmdPushCharacter(character, (Vector3) (normalized * ConfigSkills.Explosion.PushForce)), 0f);
                    if (!character.IsSupport)
                    {
                        CharacterInstance sourceCharacter = (character != c) ? c : null;
                        double baseAmount = character.MaxLife(true) * damagePct;
                        CmdDealDamageToCharacter.ExecuteStatic(sourceCharacter, character, baseAmount, false, DamageType.UNSPECIFIED, SkillType.Explosion);
                    }
                    if (character.IsDead && (executionStats != null))
                    {
                        executionStats.KillCount++;
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorCF : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal float <$>damagePct;
            internal SkillExecutionStats <$>executionStats;
            internal CharacterInstance c;
            internal float damagePct;
            internal SkillExecutionStats executionStats;

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
                    ExplosionSkill.ExecuteStatic(this.c, this.c.PhysicsBody.Transform.position, this.executionStats, this.damagePct);
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

