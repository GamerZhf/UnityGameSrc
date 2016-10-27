namespace Service
{
    using App;
    using System;

    public interface IPromotionEventSystem
    {
        ConfigPromotionEvents.Event getPromotionEventData(string promotionId);
    }
}

