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

    public class CmdClaimAchievement : ICommand
    {
        private bool m_cheated;
        private string m_id;
        private Player m_player;
        private int m_tier;

        public CmdClaimAchievement(Player player, string id, int tier, [Optional, DefaultParameterValue(false)] bool cheated)
        {
            this.m_player = player;
            this.m_id = id;
            this.m_tier = tier;
            this.m_cheated = cheated;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator95 iterator = new <executeRoutine>c__Iterator95();
            iterator.<>f__this = this;
            return iterator;
        }

        public static double ExecuteStatic(Player player, string id, int tier, [Optional, DefaultParameterValue(false)] bool cheated)
        {
            if (!player.Achievements.Claimed.ContainsKey(id))
            {
                player.Achievements.Claimed.Add(id, tier);
            }
            else
            {
                if (tier <= player.Achievements.Claimed[id])
                {
                    UnityEngine.Debug.LogWarning("Re-completing achievement '" + id + "' with same or lower tier.");
                }
                player.Achievements.Claimed[id] = tier;
            }
            CmdMarkAchievementAsNotified.ExecuteStatic(player, id, tier);
            ConfigAchievements.SharedData data = ConfigAchievements.SHARED_DATA[id];
            double amount = App.Binder.ConfigMeta.ACHIEVEMENT_TIER_DIAMOND_REWARDS[tier];
            bool visualizationManuallyControlled = !cheated;
            CmdGainResources.ExecuteStatic(player, data.RewardResource, amount, visualizationManuallyControlled, string.Empty, null);
            GameLogic.Binder.EventBus.AchievementClaimed(player, id, tier);
            return amount;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator95 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdClaimAchievement <>f__this;

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
                    CmdClaimAchievement.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_id, this.<>f__this.m_tier, this.<>f__this.m_cheated);
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

