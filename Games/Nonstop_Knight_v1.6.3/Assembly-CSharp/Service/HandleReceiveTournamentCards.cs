namespace Service
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [InboxCommand(InboxCommandIdType.RewardTournamentCardPack)]
    public class HandleReceiveTournamentCards : IInboxCommandHandler
    {
        private readonly List<Reward> m_rewards;

        public HandleReceiveTournamentCards(Dictionary<string, object> parameters)
        {
            <HandleReceiveTournamentCards>c__AnonStorey2F1 storeyf = new <HandleReceiveTournamentCards>c__AnonStorey2F1();
            this.m_rewards = new List<Reward>();
            storeyf.senderId = parameters["senderId"].ToString();
            string str = parameters["logEventId"].ToString();
            string tournamentId = parameters["tournamentId"].ToString();
            object obj2 = null;
            int num = !parameters.TryGetValue("count", out obj2) ? 1 : ((int) obj2);
            int numCompletedMilestones = !parameters.TryGetValue("numMilestonesCompleted", out obj2) ? 0 : ((int) obj2);
            TournamentView tournamentView = Service.Binder.TournamentSystem.GetTournamentView(tournamentId);
            if (tournamentView == null)
            {
                Debug.LogError("Received cards for a tournament that is not in the tournament views, discarding cards.");
            }
            else
            {
                TournamentEntry entry = tournamentView.TournamentEntries.Find(new Predicate<TournamentEntry>(storeyf.<>m__19F));
                string str3 = (entry == null) ? "A party member" : entry.PlayerDisplayName;
                for (int i = 0; i < num; i++)
                {
                    Reward reward2 = new Reward();
                    reward2.RewardSource = RewardSourceType.TournamentPartyMember;
                    reward2.RewardSourceId = str3;
                    Reward item = reward2;
                    item.ChestType = ChestType.TournamentCards;
                    item.TournamentUpgradeReward = TournamentUpgradeReward.GenerateReward(numCompletedMilestones);
                    this.m_rewards.Add(item);
                }
                TournamentLogEvent event3 = new TournamentLogEvent();
                event3.Id = str;
                event3.PlayerId = storeyf.senderId;
                event3.Type = TournamentLogEvent.LogEventType.CardPackDonated;
                event3.AdditionalData = num.ToString();
                TournamentLogEvent logEvent = event3;
                Service.Binder.TournamentSystem.RegisterPriorityLogEvent(logEvent, tournamentId);
                Service.Binder.EventBus.TournamentCardsReceived(tournamentId);
            }
        }

        public override void Execute()
        {
            Player player = GameLogic.Binder.GameState.Player;
            for (int i = 0; i < this.m_rewards.Count; i++)
            {
                player.UnclaimedRewards.Add(this.m_rewards[i]);
            }
        }

        [CompilerGenerated]
        private sealed class <HandleReceiveTournamentCards>c__AnonStorey2F1
        {
            internal string senderId;

            internal bool <>m__19F(TournamentEntry e)
            {
                return (e.PlayerId == this.senderId);
            }
        }
    }
}

