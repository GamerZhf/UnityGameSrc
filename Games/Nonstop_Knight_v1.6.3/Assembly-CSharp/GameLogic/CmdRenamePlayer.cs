namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [ConsoleCommand("rename")]
    public class CmdRenamePlayer : ICommand
    {
        private string m_newName;
        private Player m_player;

        public CmdRenamePlayer(string[] serialized)
        {
            this.m_player = Binder.GameState.Player;
            this.m_newName = serialized[0];
        }

        public CmdRenamePlayer(Player player, string newName)
        {
            this.m_player = player;
            this.m_newName = newName;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorAF raf = new <executeRoutine>c__IteratorAF();
            raf.<>f__this = this;
            return raf;
        }

        public static void ExecuteStatic(Player player, string newName)
        {
            if (player.SocialData.Name != newName)
            {
                player.SocialData.HeroNamingCount++;
            }
            player.SocialData.Name = newName;
            player.SocialData.HasGivenCustomName = true;
            player.SocialData.ShowInLocalLeaderboards = newName != _.L(ConfigLoca.HERO_KNIGHT, null, false);
            if (Binder.EventBus != null)
            {
                Binder.EventBus.PlayerRenamed(player);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorAF : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdRenamePlayer <>f__this;

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
                    CmdRenamePlayer.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_newName);
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

