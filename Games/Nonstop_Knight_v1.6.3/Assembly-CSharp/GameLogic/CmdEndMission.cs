namespace GameLogic
{
    using App;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [ConsoleCommand("endmission")]
    public class CmdEndMission : ICommand
    {
        private MissionInstance m_mission;
        private Player m_player;
        private bool m_success;

        public CmdEndMission(string[] serialized)
        {
            this.m_player = GameLogic.Binder.GameState.Player;
            this.m_mission = this.m_player.Missions.Instances[int.Parse(serialized[0])];
        }

        public CmdEndMission(Player player, MissionInstance mission, bool success)
        {
            this.m_player = player;
            this.m_mission = mission;
            this.m_success = success;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator99 iterator = new <executeRoutine>c__Iterator99();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(Player player, MissionInstance mission, bool success)
        {
            if (mission.MissionType == MissionType.Adventure)
            {
                player.HasUnlockedMissions = true;
                mission.Inspected = true;
                mission.CooldownStartTimestamp = Service.Binder.ServerTime.GameTime;
                mission.CooldownDurationSeconds = Math.Min((long) (ConfigMissions.NUM_ACTIVE_MISSIONS * App.Binder.ConfigMeta.MISSION_BASE_COOLDOWN_SECONDS), (long) (player.Missions.getMaxRemainingCooldownSeconds() + App.Binder.ConfigMeta.MISSION_BASE_COOLDOWN_SECONDS));
                mission.OnCooldown = true;
            }
            GameLogic.Binder.EventBus.MissionEnded(player, mission, success);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator99 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdEndMission <>f__this;

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
                    CmdEndMission.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_mission, this.<>f__this.m_success);
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

