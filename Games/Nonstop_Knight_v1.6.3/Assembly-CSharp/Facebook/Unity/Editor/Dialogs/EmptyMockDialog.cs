namespace Facebook.Unity.Editor.Dialogs
{
    using Facebook.MiniJSON;
    using Facebook.Unity.Editor;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class EmptyMockDialog : EditorFacebookMockDialog
    {
        [CompilerGenerated]
        private string <EmptyDialogTitle>k__BackingField;

        protected override void DoGui()
        {
        }

        protected override void SendSuccessResult()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary["did_complete"] = true;
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
                return this.EmptyDialogTitle;
            }
        }

        public string EmptyDialogTitle
        {
            [CompilerGenerated]
            get
            {
                return this.<EmptyDialogTitle>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<EmptyDialogTitle>k__BackingField = value;
            }
        }
    }
}

