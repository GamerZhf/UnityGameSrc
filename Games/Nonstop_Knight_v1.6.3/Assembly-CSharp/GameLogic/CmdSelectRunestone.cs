namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdSelectRunestone : ICommand
    {
        private Player m_player;
        private RunestoneInstance m_runestone;
        private SkillType m_skillType;
        private RunestoneSelectionSource m_source;

        public CmdSelectRunestone(Player player, SkillType skillType, RunestoneSelectionSource source, RunestoneInstance runestone)
        {
            this.m_player = player;
            this.m_skillType = skillType;
            this.m_source = source;
            this.m_runestone = runestone;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorB4 rb = new <executeRoutine>c__IteratorB4();
            rb.<>f__this = this;
            return rb;
        }

        public static void ExecuteStatic(Player player, SkillType skillType, RunestoneSelectionSource source, RunestoneInstance runestone)
        {
            for (int i = player.Runestones.SelectedRunestones.Count - 1; i >= 0; i--)
            {
                RunestoneSelection selection = player.Runestones.SelectedRunestones[i];
                if ((selection.Source == source) && (skillType == ConfigRunestones.GetSkillTypeForRunestone(selection.Id)))
                {
                    player.Runestones.SelectedRunestones.RemoveAt(i);
                }
            }
            if (runestone != null)
            {
                RunestoneSelection item = new RunestoneSelection();
                item.Id = runestone.Id;
                item.Source = source;
                player.Runestones.SelectedRunestones.Add(item);
            }
            GameLogic.Binder.EventBus.RunestoneSelected(player, runestone);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorB4 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdSelectRunestone <>f__this;

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
                    CmdSelectRunestone.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_skillType, this.<>f__this.m_source, this.<>f__this.m_runestone);
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

