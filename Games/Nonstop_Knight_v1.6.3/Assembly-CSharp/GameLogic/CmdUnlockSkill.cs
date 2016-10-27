namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class CmdUnlockSkill : ICommand
    {
        private Player m_player;
        private SkillType m_skillType;

        public CmdUnlockSkill(Player player, SkillType skillType)
        {
            this.m_player = player;
            this.m_skillType = skillType;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator8E iteratore = new <executeRoutine>c__Iterator8E();
            iteratore.<>f__this = this;
            return iteratore;
        }

        public static void ExecuteStatic(Player player, SkillType skillType, [Optional, DefaultParameterValue(false)] bool cheated)
        {
            CharacterInstance activeCharacter = player.ActiveCharacter;
            if (player.PendingSkillUnlocks.Contains(skillType))
            {
                player.PendingSkillUnlocks.Remove(skillType);
            }
            else if (Application.isEditor && !cheated)
            {
                UnityEngine.Debug.LogWarning("Unlocking skill without pending flag: " + skillType);
            }
            if (!player.UnlockedSkills.Contains(skillType))
            {
                player.UnlockedSkills.Add(skillType);
            }
            SkillInstance item = activeCharacter.getSkillInstance(skillType);
            if (item != null)
            {
                item.Rank = 1;
            }
            else
            {
                SkillInstance instance3 = new SkillInstance();
                instance3.Rank = 1;
                instance3.SkillType = skillType;
                item = instance3;
                activeCharacter.SkillInstances.Add(item);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator8E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdUnlockSkill <>f__this;

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
                    CmdUnlockSkill.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_skillType, false);
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

