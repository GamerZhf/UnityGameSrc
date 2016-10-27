namespace Appboy.Models.InAppMessage
{
    using Appboy.Models;
    using System;
    using System.Collections.Generic;

    public interface IInAppMessage
    {
        void LogClicked();
        void LogImpression();
        bool SetInAppClickAction(ClickAction clickAction);
        bool SetInAppClickAction(ClickAction clickAction, string uri);

        Color? BackgroundColor { get; set; }

        int Duration { get; set; }

        Dictionary<string, string> Extras { get; set; }

        string Icon { get; set; }

        Color? IconBackgroundColor { get; set; }

        Color? IconColor { get; set; }

        string ImageURI { get; set; }

        ClickAction InAppClickAction { get; }

        DismissType InAppDismissType { get; set; }

        string Message { get; set; }

        Color? TextColor { get; set; }

        string URI { get; }
    }
}

