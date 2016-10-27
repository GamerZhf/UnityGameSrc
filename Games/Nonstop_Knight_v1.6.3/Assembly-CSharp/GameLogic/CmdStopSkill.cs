namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdStopSkill : ICommand
    {
        private CharacterInstance m_character;
        private SkillType m_skillType;

        public CmdStopSkill(CharacterInstance character, SkillType skillType)
        {
            this.m_character = character;
            this.m_skillType = skillType;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator70 iterator = new <executeRoutine>c__Iterator70();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(CharacterInstance c, SkillType skillType)
        {
            Coroutine commandCoroutine = c.SkillRoutines[skillType];
            GameLogic.Binder.CommandProcessor.stopCommand(c.PhysicsBody, ref commandCoroutine);
            c.SkillRoutines.Remove(skillType);
            if (ConfigSkills.SHARED_DATA[skillType].ExternalControl)
            {
                c.assignToDefaultLayer();
                c.ExternallyControlled = false;
            }
            GameLogic.Binder.EventBus.CharacterSkillStopped(c, skillType);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator70 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdStopSkill <>f__this;

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
                    CmdStopSkill.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_skillType);
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

