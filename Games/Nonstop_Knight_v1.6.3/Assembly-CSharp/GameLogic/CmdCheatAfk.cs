namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [ConsoleCommand("afk")]
    public class CmdCheatAfk : ICommand
    {
        private int m_afkMinutes;

        public CmdCheatAfk(int afkMinutes)
        {
            this.m_afkMinutes = afkMinutes;
        }

        public CmdCheatAfk(string[] serialized)
        {
            this.m_afkMinutes = int.Parse(serialized[0]);
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator7F iteratorf = new <executeRoutine>c__Iterator7F();
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        public static void ExecuteStatic(int afkMinutes)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            Player player = GameLogic.Binder.GameState.Player;
            if (((activeDungeon != null) && (activeDungeon.ActiveRoom != null)) && ((player != null) && (activeDungeon.CurrentGameplayState == GameplayState.ACTION)))
            {
                long numSeconds = afkMinutes * 60L;
                player.addPassiveProgress(numSeconds);
                player.Vendor.LastRefreshTimestamp -= numSeconds;
                for (int i = 0; i < player.Missions.Instances.Count; i++)
                {
                    if (player.Missions.Instances[i].OnCooldown)
                    {
                        MissionInstance local1 = player.Missions.Instances[i];
                        local1.CooldownStartTimestamp -= numSeconds;
                    }
                }
                CmdSavePlayerDataToPersistentStorage.ExecuteStatic(player);
                App.Binder.AppContext.hardReset(null);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator7F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdCheatAfk <>f__this;

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
                    CmdCheatAfk.ExecuteStatic(this.<>f__this.m_afkMinutes);
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

