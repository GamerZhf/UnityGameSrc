namespace Appboy.Models.InAppMessage
{
    using System;
    using System.Collections.Generic;

    public interface IInAppMessageImmersive
    {
        void LogButtonClicked(int buttonID);

        List<InAppMessageButton> Buttons { get; set; }

        Color? CloseButtonColor { get; set; }

        string Header { get; set; }

        Color? HeaderTextColor { get; set; }
    }
}

