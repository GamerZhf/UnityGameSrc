namespace Service
{
    using GameLogic;
    using System;
    using System.Collections.Generic;

    [InboxCommand(InboxCommandIdType.RewardItem)]
    public class HandleRewardShopEntry : IInboxCommandHandler
    {
        private string m_id;

        public HandleRewardShopEntry(Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                object obj2 = null;
                parameters.TryGetValue("Id", out obj2);
                if (obj2 != null)
                {
                    this.m_id = obj2 as string;
                    this.m_id = LangUtil.FirstLetterToUpper(this.m_id);
                }
            }
        }

        public override void Execute()
        {
            Reward item = new Reward();
            item.ChestType = ChestType.ServerGift;
            item.addShopEntryDrop(GameLogic.Binder.GameState.Player, this.m_id, false);
            List<Reward> rewards = new List<Reward>();
            rewards.Add(item);
            RewardHelper.ClaimReward(rewards, true);
        }
    }
}

