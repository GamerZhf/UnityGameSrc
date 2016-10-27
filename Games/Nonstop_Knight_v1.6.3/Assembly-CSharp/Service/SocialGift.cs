namespace Service
{
    using GameLogic;
    using System;
    using System.Runtime.InteropServices;

    public static class SocialGift
    {
        public const string DefaultGiftId = "PetBoxSmall";

        public static SocialInboxCommand CreateFacebookGiftCommand(string senderFBid, string targetPlayerFBid, [Optional, DefaultParameterValue("PetBoxSmall")] string giftId)
        {
            SocialInboxCommand command2 = new SocialInboxCommand();
            command2.socialPlatform = PlatformConnectType.Facebook;
            command2.targetSocialid = targetPlayerFBid;
            command2.CommandId = InboxCommandIdType.GiftShopProduct;
            InboxCommandParameterType type = new InboxCommandParameterType();
            type.Add("shopEntryId", giftId);
            type.Add("rewardSource", RewardSourceType.FacebookFriend.ToString());
            type.Add("senderFBid", senderFBid);
            command2.Parameters = type;
            return command2;
        }
    }
}

