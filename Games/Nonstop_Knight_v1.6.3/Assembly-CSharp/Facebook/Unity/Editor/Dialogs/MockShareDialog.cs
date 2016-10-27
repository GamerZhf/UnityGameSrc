namespace Facebook.Unity.Editor.Dialogs
{
    using Facebook.MiniJSON;
    using Facebook.Unity;
    using Facebook.Unity.Editor;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;
    using UnityEngine;

    internal class MockShareDialog : EditorFacebookMockDialog
    {
        [CompilerGenerated]
        private string <SubTitle>k__BackingField;

        protected override void DoGui()
        {
        }

        private string GenerateFakePostID()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(AccessToken.CurrentAccessToken.UserId);
            builder.Append('_');
            for (int i = 0; i < 0x11; i++)
            {
                builder.Append(UnityEngine.Random.Range(0, 10));
            }
            return builder.ToString();
        }

        protected override void SendCancelResult()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary["cancelled"] = "true";
            if (!string.IsNullOrEmpty(base.CallbackID))
            {
                dictionary["callback_id"] = base.CallbackID;
            }
            base.Callback(Json.Serialize(dictionary));
        }

        protected override void SendSuccessResult()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            if (FB.IsLoggedIn)
            {
                dictionary["postId"] = this.GenerateFakePostID();
            }
            else
            {
                dictionary["did_complete"] = true;
            }
            if (!string.IsNullOrEmpty(base.CallbackID))
            {
                dictionary["callback_id"] = base.CallbackID;
            }
            if (base.Callback != null)
            {
                base.Callback(Json.Serialize(dictionary));
            }
        }

        protected override string DialogTitle
        {
            get
            {
                return ("Mock " + this.SubTitle + " Dialog");
            }
        }

        public string SubTitle
        {
            [CompilerGenerated]
            private get
            {
                return this.<SubTitle>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<SubTitle>k__BackingField = value;
            }
        }
    }
}

