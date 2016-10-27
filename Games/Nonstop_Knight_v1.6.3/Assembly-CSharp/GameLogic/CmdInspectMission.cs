namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdInspectMission : ICommand
    {
        private MissionInstance m_mission;
        private Player m_player;

        public CmdInspectMission(Player player, MissionInstance mission)
        {
            this.m_player = player;
            this.m_mission = mission;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorA3 ra = new <executeRoutine>c__IteratorA3();
            ra.<>f__this = this;
            return ra;
        }

        public static void ExecuteStatic(Player player, MissionInstance mission)
        {
            if (!mission.Inspected)
            {
                mission.Inspected = true;
                Binder.EventBus.MissionInspected(player, mission);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorA3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdInspectMission <>f__this;

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
                    CmdInspectMission.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_mission);
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

