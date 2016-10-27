namespace GameLogic
{
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdDonateToTournamentBucket : ICommand
    {
        private int m_count;
        private Player m_player;
        private double m_totalPrice;
        private string m_tournamentId;

        public CmdDonateToTournamentBucket(Player player, string tournamentId, int count, double totalPrice)
        {
            this.m_player = player;
            this.m_tournamentId = tournamentId;
            this.m_count = count;
            this.m_totalPrice = totalPrice;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorDE rde = new <executeRoutine>c__IteratorDE();
            rde.<>f__this = this;
            return rde;
        }

        public static void ExecuteStatic(Player player, string tournamentId, int count, double totalPrice)
        {
            TournamentInstance tournament = player.Tournaments.getInstanceOrNull(tournamentId);
            if (tournament == null)
            {
                UnityEngine.Debug.LogError("Player has no active tournament with id: " + tournamentId);
            }
            else
            {
                TournamentView tournamentView = Service.Binder.TournamentSystem.GetTournamentView(tournamentId);
                if (tournamentView == null)
                {
                    UnityEngine.Debug.LogError("Cannot get tournament view for id: " + tournamentId);
                }
                else
                {
                    if (player.getResourceAmount(ResourceType.Diamond) >= totalPrice)
                    {
                        TournamentLogEvent.LogEventType multiCardPackDonated;
                        CmdGainResources.ExecuteStatic(player, ResourceType.Diamond, -totalPrice, false, string.Empty, null);
                        Dictionary<string, object> customParams = new Dictionary<string, object>();
                        customParams.Add("count", count);
                        if (count > 1)
                        {
                            multiCardPackDonated = TournamentLogEvent.LogEventType.MultiCardPackDonated;
                        }
                        else
                        {
                            multiCardPackDonated = TournamentLogEvent.LogEventType.CardPackDonated;
                        }
                        int numMilestonesClaimed = tournamentView.Instance.NumMilestonesClaimed;
                        customParams.Add("numMilestonesCompleted", numMilestonesClaimed);
                        TournamentLogEvent logEvent = Service.Binder.TournamentSystem.LogTournamentEvent(tournamentId, multiCardPackDonated, false, count.ToString());
                        Service.Binder.TournamentService.StartBroadcastToBucket(BroadcastKey.GiftCards, tournamentId, logEvent, customParams);
                        tournament.NumDonationsMade += count;
                        for (int i = 0; i < count; i++)
                        {
                            Reward reward2 = new Reward();
                            reward2.RewardSource = RewardSourceType.SelfReward;
                            reward2.RewardSourceId = player.SocialData.Name;
                            Reward item = reward2;
                            item.ChestType = ChestType.TournamentCards;
                            item.TournamentUpgradeReward = TournamentUpgradeReward.GenerateReward(numMilestonesClaimed);
                            player.UnclaimedRewards.Add(item);
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("Not enough diamonds to donate to bucket");
                    }
                    GameLogic.Binder.EventBus.TournamentDonationMade(player, tournament, count, totalPrice);
                }
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorDE : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdDonateToTournamentBucket <>f__this;

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
                    CmdDonateToTournamentBucket.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_tournamentId, this.<>f__this.m_count, this.<>f__this.m_totalPrice);
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

