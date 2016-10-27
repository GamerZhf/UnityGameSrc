namespace Service
{
    using System;
    using System.Collections.Generic;

    public class PromotionResponse
    {
        public List<RemotePromotion> promotions;

        public PromotionResponse()
        {
            this.promotions = new List<RemotePromotion>();
        }

        public PromotionResponse(List<RemotePromotion> _promotions)
        {
            this.promotions = _promotions;
        }
    }
}

