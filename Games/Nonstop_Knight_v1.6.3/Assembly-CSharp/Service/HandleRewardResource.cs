namespace Service
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;

    [InboxCommand(InboxCommandIdType.RewardResource)]
    public class HandleRewardResource : IInboxCommandHandler
    {
        private Reward reward;

        public HandleRewardResource(Dictionary<string, object> parameters)
        {
            Reward reward = new Reward();
            reward.ChestType = ChestType.ServerGift;
            this.reward = reward;
            try
            {
                this.reward.addResourceDrop((ResourceType) ((int) Enum.Parse(typeof(ResourceType), parameters["Resource"].ToString())), double.Parse(parameters["Amount"].ToString()));
            }
            catch (Exception)
            {
                this.reward.ShopEntryId = ConfigShops.IAP_STARTER_BUNDLE_ID;
            }
        }

        public override void Execute()
        {
            GameLogic.Binder.GameState.Player.UnclaimedRewards.Add(this.reward);
        }
    }
}

