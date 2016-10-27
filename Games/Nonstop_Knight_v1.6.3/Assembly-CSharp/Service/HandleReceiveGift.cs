namespace Service
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;

    [InboxCommand(InboxCommandIdType.GiftShopProduct)]
    public class HandleReceiveGift : IInboxCommandHandler
    {
        private readonly Reward m_reward;

        public HandleReceiveGift(Dictionary<string, object> parameters)
        {
            Reward reward = new Reward();
            reward.RewardSource = RewardSourceType.FacebookFriend;
            reward.RewardSourceId = parameters["senderFBid"].ToString();
            this.m_reward = reward;
            string id = parameters["shopEntryId"].ToString();
            if (ConfigShops.GetShopEntry(id) == null)
            {
                id = "PetBoxSmall";
            }
            this.m_reward.addShopEntryDrop(GameLogic.Binder.GameState.Player, id, false);
        }

        public override void Execute()
        {
            GameLogic.Binder.GameState.Player.UnclaimedRewards.Add(this.m_reward);
        }
    }
}

