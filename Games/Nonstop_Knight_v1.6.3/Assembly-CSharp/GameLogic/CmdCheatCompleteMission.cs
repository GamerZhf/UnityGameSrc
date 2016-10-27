namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ConsoleCommand("completemission")]
    public class CmdCheatCompleteMission : ICommand
    {
        private bool m_forceStartMission;
        private MissionInstance m_mission;
        private Player m_player;

        public CmdCheatCompleteMission(string[] serialized)
        {
            this.m_player = Binder.GameState.Player;
            if (serialized[0] == "event")
            {
                this.m_forceStartMission = true;
                this.m_mission = this.m_player.PromotionEvents.getNewestEventInstance().Missions.Instances[int.Parse(serialized[1])];
            }
            else
            {
                this.m_forceStartMission = false;
                this.m_mission = this.m_player.Missions.Instances[int.Parse(serialized[0])];
            }
        }

        public CmdCheatCompleteMission(Player player, MissionInstance mission, [Optional, DefaultParameterValue(false)] bool forceStartMission)
        {
            this.m_player = player;
            this.m_mission = mission;
            this.m_forceStartMission = forceStartMission;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator94 iterator = new <executeRoutine>c__Iterator94();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(Player player, MissionInstance mission, [Optional, DefaultParameterValue(false)] bool forceStartMission)
        {
            mission.ForceCompleted = true;
            if (forceStartMission && mission.OnCooldown)
            {
                CmdStartMission.ExecuteStatic(player, mission);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator94 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdCheatCompleteMission <>f__this;

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
                    CmdCheatCompleteMission.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_mission, this.<>f__this.m_forceStartMission);
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

