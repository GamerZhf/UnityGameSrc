namespace Appboy.Models.InAppMessage
{
    using Appboy.Utilities;
    using System;

    public class InAppMessageModal : InAppMessageImmersiveBase
    {
        public InAppMessageModal()
        {
        }

        public InAppMessageModal(JSONClass json) : base(json)
        {
        }
    }
}

