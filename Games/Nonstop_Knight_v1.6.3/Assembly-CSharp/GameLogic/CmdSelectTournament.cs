namespace GameLogic
{
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdSelectTournament : ICommand
    {
        private Player m_player;
        private string m_tournamentId;

        public CmdSelectTournament(Player player, string tournamentId)
        {
            this.m_player = player;
            this.m_tournamentId = tournamentId;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorB5 rb = new <executeRoutine>c__IteratorB5();
            rb.<>f__this = this;
            return rb;
        }

        public static void ExecuteStatic(Player player, string tournamentId)
        {
            TournamentInstance selectedTournament = player.Tournaments.SelectedTournament;
            TournamentInstance instance2 = null;
            if (string.IsNullOrEmpty(tournamentId))
            {
                player.Tournaments.LastUnselectedTournamentId = player.Tournaments.SelectedTournamentId;
                player.Tournaments.SelectedTournamentId = null;
                player.Tournaments.SelectedTournament = null;
                if ((selectedTournament != null) && (selectedTournament.CurrentState == TournamentInstance.State.CLEARED_FOR_REMOVAL))
                {
                    for (int i = player.UnclaimedRewards.Count - 1; i >= 0; i--)
                    {
                        Reward reward = player.UnclaimedRewards[i];
                        if (reward.ChestType == ChestType.TournamentCards)
                        {
                            player.UnclaimedRewards.RemoveAt(i);
                        }
                        else if ((reward.RewardSource == RewardSourceType.TournamentDungeonDrop) && (reward.RewardSourceId == selectedTournament.TournamentId))
                        {
                            player.UnclaimedRewards.RemoveAt(i);
                        }
                    }
                }
            }
            else
            {
                TournamentInstance instance3 = player.Tournaments.getInstanceOrNull(tournamentId);
                if (instance3 == null)
                {
                    UnityEngine.Debug.LogError("Trying to select tournament without instance data: " + tournamentId);
                    return;
                }
                player.Tournaments.SelectedTournamentId = tournamentId;
                player.Tournaments.SelectedTournament = instance3;
                instance2 = instance3;
            }
            GameLogic.Binder.EventBus.TournamentSelected(player, instance2, selectedTournament);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorB5 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdSelectTournament <>f__this;

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
                    CmdSelectTournament.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_tournamentId);
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

