namespace Appboy.Models.InAppMessage
{
    using Appboy.Utilities;
    using System;

    public class InAppMessageFull : InAppMessageImmersiveBase
    {
        public InAppMessageFull()
        {
        }

        public InAppMessageFull(JSONClass json) : base(json)
        {
        }
    }
}

