namespace Appboy.Models.InAppMessage
{
    using Appboy;
    using Appboy.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class InAppMessageImmersiveBase : InAppMessageBase, IInAppMessageImmersive
    {
        private bool _buttonClickLogged;
        [CompilerGenerated]
        private List<InAppMessageButton> <Buttons>k__BackingField;
        [CompilerGenerated]
        private Color? <CloseButtonColor>k__BackingField;
        [CompilerGenerated]
        private string <Header>k__BackingField;
        [CompilerGenerated]
        private Color? <HeaderTextColor>k__BackingField;

        protected InAppMessageImmersiveBase()
        {
        }

        public InAppMessageImmersiveBase(JSONClass json) : base(json)
        {
            this.Header = (string) json["header"];
            this.HeaderTextColor = ColorUtils.HexToColor((string) json["header_text_color"]);
            this.CloseButtonColor = ColorUtils.HexToColor((string) json["close_btn_color"]);
            if (json["btns"] != null)
            {
                this.Buttons = new List<InAppMessageButton>();
                JSONArray array = (JSONArray) JSON.Parse(json["btns"].ToString());
                Debug.Log(string.Format("parse in-app message with {0} buttons", array.Count));
                for (int i = 0; i < array.Count; i++)
                {
                    JSONClass asObject = array[i].AsObject;
                    try
                    {
                        Debug.Log(string.Format("Button no. {0} json string is {1}", i, asObject));
                        InAppMessageButton item = new InAppMessageButton(asObject);
                        if (item != null)
                        {
                            this.Buttons.Add(item);
                        }
                    }
                    catch
                    {
                        Debug.Log(string.Format("Unable to parse button from {0}", asObject));
                    }
                }
            }
        }

        public void LogButtonClicked(int buttonID)
        {
            if (!this._buttonClickLogged)
            {
                this._buttonClickLogged = true;
                AppboyBinding.LogInAppMessageButtonClicked(base._jsonString, buttonID);
            }
            else
            {
                Debug.Log("The in-app message already log a button clicked.");
            }
        }

        public override string ToString()
        {
            object[] args = new object[] { base.ToString(), this.Header, this.HeaderTextColor, this.CloseButtonColor, CollectionUtils.ListToString<InAppMessageButton>(this.Buttons) };
            return string.Format("{0}, Header={1}, HeaderTextColor={2}, CloseButtonColor={3}, Buttons{4}", args);
        }

        public List<InAppMessageButton> Buttons
        {
            [CompilerGenerated]
            get
            {
                return this.<Buttons>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Buttons>k__BackingField = value;
            }
        }

        public Color? CloseButtonColor
        {
            [CompilerGenerated]
            get
            {
                return this.<CloseButtonColor>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<CloseButtonColor>k__BackingField = value;
            }
        }

        public string Header
        {
            [CompilerGenerated]
            get
            {
                return this.<Header>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Header>k__BackingField = value;
            }
        }

        public Color? HeaderTextColor
        {
            [CompilerGenerated]
            get
            {
                return this.<HeaderTextColor>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<HeaderTextColor>k__BackingField = value;
            }
        }
    }
}

