namespace Service
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;

    [InboxCommand(InboxCommandIdType.RewardPet)]
    public class HandleRewardPet : IInboxCommandHandler
    {
        private int m_amount;
        private string m_id;

        public HandleRewardPet(Dictionary<string, object> parameters)
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
                object obj3 = null;
                parameters.TryGetValue("Amount", out obj3);
                if (obj3 != null)
                {
                    int.TryParse(obj3.ToString(), out this.m_amount);
                }
            }
        }

        public override void Execute()
        {
            Reward item = new Reward();
            item.Pets = new List<PetReward>();
            if (App.Binder.ConfigMeta.IsActivePetId(this.m_id))
            {
                item.ChestType = ChestType.ServerGift;
                List<string> list = new List<string>();
                list.Add("PetBundleSmall");
                item.ShopEntryDrops = list;
                PetReward reward2 = new PetReward();
                reward2.PetId = this.m_id;
                reward2.Amount = this.m_amount;
                item.Pets.Add(reward2);
                item.Sprite = "icon_" + this.m_id.ToLower();
            }
            List<Reward> rewards = new List<Reward>();
            rewards.Add(item);
            RewardHelper.ClaimReward(rewards, true);
        }
    }
}

