namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdRankUpSkill : ICommand
    {
        private CharacterInstance m_character;
        private SkillInstance m_skillInstance;

        public CmdRankUpSkill(CharacterInstance character, SkillInstance skillInstance)
        {
            this.m_character = character;
            this.m_skillInstance = skillInstance;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorAD rad = new <executeRoutine>c__IteratorAD();
            rad.<>f__this = this;
            return rad;
        }

        public static void ExecuteStatic(CharacterInstance character, SkillInstance skillInstance)
        {
            Player owningPlayer = character.OwningPlayer;
            int rank = skillInstance.Rank + 1;
            skillInstance.Rank = rank;
            double num2 = App.Binder.ConfigMeta.SkillUpgradeDustCost(rank);
            CmdGainResources.ExecuteStatic(owningPlayer, ResourceType.Dust, -num2, false, string.Empty, null);
            GameLogic.Binder.EventBus.CharacterSkillRankUpped(character, skillInstance);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorAD : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdRankUpSkill <>f__this;

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
                    CmdRankUpSkill.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_skillInstance);
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

