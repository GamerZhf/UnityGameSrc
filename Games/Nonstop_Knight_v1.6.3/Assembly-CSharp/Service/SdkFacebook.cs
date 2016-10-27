namespace Service
{
    using Facebook.Unity;
    using System;
    using System.Collections.Generic;

    public class SdkFacebook : ISdkWrapper
    {
        public void Event(ESdkEvent eventName, object param)
        {
            string str;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            ESdkEvent event2 = eventName;
            if (event2 == ESdkEvent.LevelUp)
            {
                str = "fb_mobile_level_achieved";
                parameters.Add("fb_level", param);
            }
            else if (event2 == ESdkEvent.Tuturial)
            {
                str = "fb_mobile_tutorial_completion";
            }
            else
            {
                str = eventName.ToString();
            }
            FB.LogAppEvent(str, null, parameters);
        }

        public void Purchase(PremiumProduct product)
        {
            FB.LogPurchase(product.price, product.currencyCode, null);
        }

        public void SessionEnd()
        {
        }

        public void SessionStart(string trUserId)
        {
        }
    }
}

