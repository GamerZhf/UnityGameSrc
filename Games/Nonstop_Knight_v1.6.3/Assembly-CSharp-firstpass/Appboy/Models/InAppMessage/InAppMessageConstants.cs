namespace Appboy.Models.InAppMessage
{
    using Appboy.Utilities;
    using System;
    using UnityEngine;

    public class InAppMessageConstants
    {
        public const string BackgroundColorKey = "bg_color";
        public const string ButtonBackgroundColorKey = "bg_color";
        public const string ButtonClickActionKey = "click_action";
        public const string ButtonIDKey = "id";
        public const string ButtonsKey = "btns";
        public const string ButtonTextColorKey = "text_color";
        public const string ButtonTextKey = "text";
        public const string ButtonURIKey = "uri";
        public const string ClickActionKey = "click_action";
        public const string CloseButtonColorKey = "close_btn_color";
        public const string DismissTypeKey = "message_close";
        public const string DurationKey = "duration";
        public const string ExtrasKey = "extras";
        public const string HeaderKey = "header";
        public const string HeaderTextColorKey = "header_text_color";
        public const string HideChevronKey = "hide_chevron";
        public const string IconBackgroundColorKey = "icon_bg_color";
        public const string IconColorKey = "icon_color";
        public const string IconKey = "icon";
        public const string ImageURLKey = "image_url";
        public const string MessageKey = "message";
        public const string SlideFromKey = "slide_from";
        public const string TextColorKey = "text_color";
        public const string TypeKey = "type";
        public const string URIKey = "uri";

        public static JSONClass JSONObjectFromString(string JSONString)
        {
            if (string.IsNullOrEmpty(JSONString))
            {
                Debug.LogError("Slideup JSON Message cannot be null or empty.");
            }
            JSONClass class2 = null;
            try
            {
                class2 = (JSONClass) JSON.Parse(JSONString);
            }
            catch
            {
                Debug.LogError(string.Format("Cannot parse in-app message JSON message {0}.", JSONString));
            }
            return class2;
        }
    }
}

