namespace Appboy.Models.InAppMessage
{
    using Appboy.Utilities;
    using System;

    public class InAppMessageFactory
    {
        public static IInAppMessage BuildInAppMessage(string inAppMessageJSONString)
        {
            JSONClass json = InAppMessageConstants.JSONObjectFromString(inAppMessageJSONString);
            if (json == null)
            {
                return null;
            }
            switch (((InAppMessageType) ((int) EnumUtils.TryParse(typeof(InAppMessageType), (string) json["type"], true, InAppMessageType.SLIDEUP))))
            {
                case InAppMessageType.FULL:
                    return new InAppMessageFull(json);

                case InAppMessageType.MODAL:
                    return new InAppMessageModal(json);
            }
            return new InAppMessageSlideup(json);
        }
    }
}

