namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdAssignActiveSkill : ICommand
    {
        private CharacterInstance m_character;
        private SkillType m_skillType;

        public CmdAssignActiveSkill(CharacterInstance character, SkillType skillType)
        {
            this.m_character = character;
            this.m_skillType = skillType;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator90 iterator = new <executeRoutine>c__Iterator90();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(CharacterInstance character, SkillType skillType)
        {
            SkillInstance item = character.getSkillInstance(skillType);
            if (item == null)
            {
                UnityEngine.Debug.LogWarning(string.Concat(new object[] { "Assigning skill '", skillType, "' to character '", character.Name, "' even though the skill has not been unlocked for the character." }));
                SkillInstance instance2 = new SkillInstance();
                instance2.Rank = 1;
                instance2.SkillType = skillType;
                item = instance2;
                character.SkillInstances.Add(item);
            }
            int group = ConfigSkills.SHARED_DATA[skillType].Group;
            for (int i = character.ActiveSkillTypes.Count - 1; i >= 0; i--)
            {
                SkillType type = character.ActiveSkillTypes[i];
                if ((type != SkillType.NONE) && (ConfigSkills.SHARED_DATA[type].Group == group))
                {
                    character.ActiveSkillTypes.Remove(type);
                }
            }
            character.ActiveSkillTypes.Add(skillType);
            CmdInspectSkill.ExecuteStatic(item);
            GameLogic.Binder.EventBus.CharacterSkillsChanged(character);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator90 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdAssignActiveSkill <>f__this;

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
                    CmdAssignActiveSkill.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_skillType);
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

