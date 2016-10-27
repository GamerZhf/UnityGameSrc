namespace Appboy.Models.InAppMessage
{
    using Appboy;
    using Appboy.Models;
    using Appboy.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class InAppMessageBase : IInAppMessage
    {
        private bool _clickLogged;
        private int _duration;
        private bool _impressionLogged;
        protected string _jsonString;
        [CompilerGenerated]
        private Color? <BackgroundColor>k__BackingField;
        [CompilerGenerated]
        private Dictionary<string, string> <Extras>k__BackingField;
        [CompilerGenerated]
        private string <Icon>k__BackingField;
        [CompilerGenerated]
        private Color? <IconBackgroundColor>k__BackingField;
        [CompilerGenerated]
        private Color? <IconColor>k__BackingField;
        [CompilerGenerated]
        private string <ImageURI>k__BackingField;
        [CompilerGenerated]
        private ClickAction <InAppClickAction>k__BackingField;
        [CompilerGenerated]
        private DismissType <InAppDismissType>k__BackingField;
        [CompilerGenerated]
        private string <Message>k__BackingField;
        [CompilerGenerated]
        private Color? <TextColor>k__BackingField;
        [CompilerGenerated]
        private string <URI>k__BackingField;
        private const int DefaultDuration = 0x1388;

        protected InAppMessageBase()
        {
        }

        public InAppMessageBase(JSONClass json)
        {
            if (json == null)
            {
                throw new ArgumentNullException("The JSON Class passed to InAppMessage constructor is null.");
            }
            this._jsonString = json.ToString();
            this.Message = (string) json["message"];
            if (json["extras"] != null)
            {
                this.Extras = JsonUtils.JSONClassToDictionary(json["extras"].AsObject);
            }
            this.InAppClickAction = (ClickAction) ((int) EnumUtils.TryParse(typeof(ClickAction), (string) json["click_action"], true, ClickAction.NEWS_FEED));
            this.URI = (string) json["uri"];
            this.ImageURI = (string) json["image_url"];
            if ((this.InAppClickAction == ClickAction.URI) && (this.URI == null))
            {
                Debug.Log("The click action cannot be set to URI because the uri is null. Setting click action to NONE.");
                this.InAppClickAction = ClickAction.NONE;
            }
            this.InAppDismissType = (DismissType) ((int) EnumUtils.TryParse(typeof(DismissType), (string) json["message_close"], true, DismissType.AUTO_DISMISS));
            this.Duration = json["duration"].AsInt;
            this.BackgroundColor = ColorUtils.HexToColor((string) json["bg_color"]);
            this.TextColor = ColorUtils.HexToColor((string) json["text_color"]);
            this.Icon = (string) json["icon"];
            this.IconColor = ColorUtils.HexToColor((string) json["icon_color"]);
            this.IconBackgroundColor = ColorUtils.HexToColor((string) json["icon_bg_color"]);
        }

        public void LogClicked()
        {
            if (!this._clickLogged)
            {
                this._clickLogged = true;
                AppboyBinding.LogInAppMessageClicked(this._jsonString);
            }
            else
            {
                Debug.Log("The in-app message already logged a click.");
            }
        }

        public void LogImpression()
        {
            if (!this._impressionLogged)
            {
                this._impressionLogged = true;
                AppboyBinding.LogInAppMessageImpression(this._jsonString);
            }
            else
            {
                Debug.Log("The in-app message already logged an impression.");
            }
        }

        public bool SetInAppClickAction(ClickAction clickAction)
        {
            if (clickAction != ClickAction.URI)
            {
                this.InAppClickAction = clickAction;
                this.URI = null;
                return true;
            }
            Debug.LogError("A non-null URI is required in order to set the InAppClickAction to URI.");
            return false;
        }

        public bool SetInAppClickAction(ClickAction clickAction, string uri)
        {
            if ((uri != null) && (clickAction == ClickAction.URI))
            {
                this.InAppClickAction = clickAction;
                this.URI = uri;
                return true;
            }
            return this.SetInAppClickAction(clickAction);
        }

        public override string ToString()
        {
            object[] args = new object[] { base.GetType().Name, this.Message, this.InAppClickAction, this.URI, this.InAppDismissType, this.Duration, CollectionUtils.DictionaryToString(this.Extras), this.BackgroundColor, this.TextColor, this.Icon, this.IconColor, this.IconBackgroundColor, this.ImageURI };
            return string.Format("{0}: Message={1}, InAppClickAction={2}, URI={3}, InAppDismissType={4}, Duration={5}, Extras={6}, BackgroundColor={7}TextColor={8}, Icon={9}, IconColor={10}, IconBackgroundColor={11}, ImageURI={12}", args);
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

        public int Duration
        {
            get
            {
                return this._duration;
            }
            set
            {
                this._duration = (value > 0) ? value : 0x1388;
            }
        }

        public Dictionary<string, string> Extras
        {
            [CompilerGenerated]
            get
            {
                return this.<Extras>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Extras>k__BackingField = value;
            }
        }

        public string Icon
        {
            [CompilerGenerated]
            get
            {
                return this.<Icon>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Icon>k__BackingField = value;
            }
        }

        public Color? IconBackgroundColor
        {
            [CompilerGenerated]
            get
            {
                return this.<IconBackgroundColor>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<IconBackgroundColor>k__BackingField = value;
            }
        }

        public Color? IconColor
        {
            [CompilerGenerated]
            get
            {
                return this.<IconColor>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<IconColor>k__BackingField = value;
            }
        }

        public string ImageURI
        {
            [CompilerGenerated]
            get
            {
                return this.<ImageURI>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ImageURI>k__BackingField = value;
            }
        }

        public ClickAction InAppClickAction
        {
            [CompilerGenerated]
            get
            {
                return this.<InAppClickAction>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<InAppClickAction>k__BackingField = value;
            }
        }

        public DismissType InAppDismissType
        {
            [CompilerGenerated]
            get
            {
                return this.<InAppDismissType>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<InAppDismissType>k__BackingField = value;
            }
        }

        public string Message
        {
            [CompilerGenerated]
            get
            {
                return this.<Message>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Message>k__BackingField = value;
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

