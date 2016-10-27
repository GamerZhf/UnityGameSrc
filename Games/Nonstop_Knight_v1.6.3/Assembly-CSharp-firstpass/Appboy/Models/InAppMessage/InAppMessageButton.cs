namespace Appboy.Models.InAppMessage
{
    using Appboy.Models;
    using Appboy.Utilities;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class InAppMessageButton
    {
        [CompilerGenerated]
        private Color? <BackgroundColor>k__BackingField;
        [CompilerGenerated]
        private ClickAction <ButtonClickAction>k__BackingField;
        [CompilerGenerated]
        private int <ButtonID>k__BackingField;
        [CompilerGenerated]
        private string <Text>k__BackingField;
        [CompilerGenerated]
        private Color? <TextColor>k__BackingField;
        [CompilerGenerated]
        private string <URI>k__BackingField;

        public InAppMessageButton()
        {
        }

        public InAppMessageButton(JSONClass json)
        {
            this.ButtonID = json["id"].AsInt;
            this.Text = (string) json["text"];
            this.TextColor = ColorUtils.HexToColor((string) json["text_color"]);
            this.BackgroundColor = ColorUtils.HexToColor((string) json["bg_color"]);
            this.URI = (string) json["uri"];
            this.ButtonClickAction = (ClickAction) ((int) EnumUtils.TryParse(typeof(ClickAction), (string) json["click_action"], true, ClickAction.NEWS_FEED));
            if ((this.ButtonClickAction == ClickAction.URI) && (this.URI == null))
            {
                Debug.Log("The click action cannot be set to URI because the uri is null. Setting click action to NONE.");
                this.ButtonClickAction = ClickAction.NONE;
            }
        }

        public bool SetButtonClickAction(ClickAction clickAction)
        {
            if (clickAction != ClickAction.URI)
            {
                this.ButtonClickAction = clickAction;
                this.URI = null;
                return true;
            }
            Debug.LogError("A non-null URI is required in order to set the ButtonClickAction to URI.");
            return false;
        }

        public bool SetButtonClickAction(ClickAction clickAction, string uri)
        {
            if ((uri != null) && (clickAction == ClickAction.URI))
            {
                this.ButtonClickAction = clickAction;
                this.URI = uri;
                return true;
            }
            return this.SetButtonClickAction(clickAction);
        }

        public override string ToString()
        {
            object[] args = new object[] { this.ButtonID, this.Text, this.URI, this.ButtonClickAction, this.TextColor, this.BackgroundColor };
            return string.Format("In-App Message Button: ButtonID={0}, Text={1}, URI={2}, ButtonClickAction={3}, TextColor={4}, BackgroundColor={5}", args);
        }

        public Color? BackgroundColor
        {
            [CompilerGenerated]
            get
            {
                return this.<BackgroundColor>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<BackgroundColor>k__BackingField = value;
            }
        }

        public ClickAction ButtonClickAction
        {
            [CompilerGenerated]
            get
            {
                return this.<ButtonClickAction>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ButtonClickAction>k__BackingField = value;
            }
        }

        public int ButtonID
        {
            [CompilerGenerated]
            get
            {
                return this.<ButtonID>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ButtonID>k__BackingField = value;
            }
        }

        public string Text
        {
            [CompilerGenerated]
            get
            {
                return this.<Text>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Text>k__BackingField = value;
            }
        }

        public Color? TextColor
        {
            [CompilerGenerated]
            get
            {
                return this.<TextColor>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<TextColor>k__BackingField = value;
            }
        }

        public string URI
        {
            [CompilerGenerated]
            get
            {
                return this.<URI>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<URI>k__BackingField = value;
            }
        }
    }
}

