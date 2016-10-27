namespace GameLogic
{
    using App;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [ConsoleCommand("tug")]
    public class CmdGainTournamentUpgrade : ICommand
    {
        private bool m_epicVersion;
        private string m_id;
        private int m_numMilestonesCompleted;
        private Player m_player;

        public CmdGainTournamentUpgrade(string[] serialized)
        {
            this.m_player = GameLogic.Binder.GameState.Player;
            this.m_id = LangUtil.FirstLetterToUpper(serialized[0]);
            this.m_epicVersion = UnityEngine.Random.Range((float) 0f, (float) 1f) <= 0.5f;
            this.m_numMilestonesCompleted = 1;
        }

        public CmdGainTournamentUpgrade(Player player, string id, bool epicVersion, int numMilestonesCompleted)
        {
            this.m_player = player;
            this.m_id = id;
            this.m_epicVersion = epicVersion;
            this.m_numMilestonesCompleted = numMilestonesCompleted;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorA1 ra = new <executeRoutine>c__IteratorA1();
            ra.<>f__this = this;
            return ra;
        }

        public static void ExecuteStatic(Player player, string id, bool epicVersion, int numMilestonesCompleted)
        {
            TournamentInstance selectedTournament = player.Tournaments.SelectedTournament;
            if (selectedTournament != null)
            {
                float totalModifier = App.Binder.ConfigMeta.GetTournamentUpgradeModifier(id, epicVersion, numMilestonesCompleted);
                if (epicVersion)
                {
                    if (!selectedTournament.Upgrades.EpicUpgrades.ContainsKey(id))
                    {
                        TournamentUpgradeInstance instance2 = new TournamentUpgradeInstance(id, 1, totalModifier);
                        selectedTournament.Upgrades.EpicUpgrades.Add(id, instance2);
                        selectedTournament.Upgrades.addEpicUpgradeToQuickLookup(instance2, true);
                    }
                    else
                    {
                        selectedTournament.Upgrades.EpicUpgrades[id].increaseTotalCount(1);
                        selectedTournament.Upgrades.EpicUpgrades[id].increaseTotalModifier(totalModifier);
                    }
                }
                else if (!selectedTournament.Upgrades.NormalUpgrades.ContainsKey(id))
                {
                    TournamentUpgradeInstance instance3 = new TournamentUpgradeInstance(id, 1, totalModifier);
                    selectedTournament.Upgrades.NormalUpgrades.Add(id, instance3);
                    selectedTournament.Upgrades.addNormalUpgradeToQuickLookup(instance3, true);
                }
                else
                {
                    selectedTournament.Upgrades.NormalUpgrades[id].increaseTotalCount(1);
                    selectedTournament.Upgrades.NormalUpgrades[id].increaseTotalModifier(totalModifier);
                }
                GameLogic.Binder.EventBus.TournamentUpgradeGained(player, id, epicVersion, numMilestonesCompleted);
            }
            else if (Application.isEditor)
            {
                UnityEngine.Debug.LogWarning("Trying to consume tournament upgrade reward without selected tournament: " + id);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorA1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdGainTournamentUpgrade <>f__this;

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
                    CmdGainTournamentUpgrade.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_id, this.<>f__this.m_epicVersion, this.<>f__this.m_numMilestonesCompleted);
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

