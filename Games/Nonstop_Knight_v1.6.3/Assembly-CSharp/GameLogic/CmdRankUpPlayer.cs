namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [ConsoleCommand("rankup")]
    public class CmdRankUpPlayer : ICommand
    {
        private bool m_cheated;
        private Player m_player;

        public CmdRankUpPlayer(string[] serialized)
        {
            this.m_player = GameLogic.Binder.GameState.Player;
            this.m_cheated = true;
        }

        public CmdRankUpPlayer(Player player, [Optional, DefaultParameterValue(false)] bool cheated)
        {
            this.m_player = player;
            this.m_cheated = cheated;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorAB rab = new <executeRoutine>c__IteratorAB();
            rab.<>f__this = this;
            return rab;
        }

        public static void ExecuteStatic(Player player, [Optional, DefaultParameterValue(false)] bool cheated)
        {
            double num = App.Binder.ConfigMeta.XpRequiredForRankUp(player.Rank);
            CmdGainResources.ExecuteStatic(player, ResourceType.Xp, -num, false, string.Empty, null);
            player.Rank = Mathf.Min(player.Rank + 1, App.Binder.ConfigMeta.XP_LEVEL_CAP);
            bool flag = false;
            for (int i = 0; i < ConfigSkills.ALL_HERO_SKILLS.Count; i++)
            {
                SkillType item = ConfigSkills.ALL_HERO_SKILLS[i];
                ConfigSkills.SharedData data = ConfigSkills.SHARED_DATA[item];
                if (((player.Rank == data.UnlockRank) && !player.PendingSkillUnlocks.Contains(item)) && !player.hasUnlockedSkill(item))
                {
                    player.PendingSkillUnlocks.Add(item);
                    flag = true;
                    break;
                }
            }
            bool flag2 = false;
            for (int j = 0; j < ConfigRunestones.RUNESTONES.Length; j++)
            {
                ConfigRunestones.SharedData data2 = ConfigRunestones.RUNESTONES[j];
                if (((player.Rank == data2.UnlockRank) && !player.PendingRankUpRunestoneUnlocks.Contains(data2.Id)) && !player.Runestones.ownsRunestone(data2.Id))
                {
                    player.PendingRankUpRunestoneUnlocks.Add(data2.Id);
                    flag2 = true;
                    break;
                }
            }
            if (!flag && !flag2)
            {
                player.NumPendingRankUpCeremonies++;
            }
            GameLogic.Binder.EventBus.PlayerRankUpped(player, cheated);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorAB : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdRankUpPlayer <>f__this;

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
                    CmdRankUpPlayer.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_cheated);
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

