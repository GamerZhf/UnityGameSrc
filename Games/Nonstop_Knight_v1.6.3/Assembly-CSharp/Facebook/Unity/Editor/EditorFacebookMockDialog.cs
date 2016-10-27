namespace Facebook.Unity.Editor
{
    using Facebook.MiniJSON;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal abstract class EditorFacebookMockDialog : MonoBehaviour
    {
        [CompilerGenerated]
        private OnComplete <Callback>k__BackingField;
        [CompilerGenerated]
        private string <CallbackID>k__BackingField;
        private Rect modalRect;
        private GUIStyle modalStyle;

        protected EditorFacebookMockDialog()
        {
        }

        protected abstract void DoGui();
        public void OnGUI()
        {
            GUI.ModalWindow(this.GetHashCode(), this.modalRect, new GUI.WindowFunction(this.OnGUIDialog), this.DialogTitle, this.modalStyle);
        }

        private void OnGUIDialog(int windowId)
        {
            GUILayout.Space(10f);
            GUILayout.BeginVertical(new GUILayoutOption[0]);
            GUILayout.Label("Warning! Mock dialog responses will NOT match production dialogs", new GUILayoutOption[0]);
            GUILayout.Label("Test your app on one of the supported platforms", new GUILayoutOption[0]);
            this.DoGui();
            GUILayout.EndVertical();
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.FlexibleSpace();
            GUIContent content = new GUIContent("Send Success");
            if (GUI.Button(GUILayoutUtility.GetRect(content, GUI.skin.button), content))
            {
                this.SendSuccessResult();
                UnityEngine.Object.Destroy(this);
            }
            GUIContent content2 = new GUIContent("Send Cancel");
            if (GUI.Button(GUILayoutUtility.GetRect(content2, GUI.skin.button), content2, GUI.skin.button))
            {
                this.SendCancelResult();
                UnityEngine.Object.Destroy(this);
            }
            GUIContent content3 = new GUIContent("Send Error");
            if (GUI.Button(GUILayoutUtility.GetRect(content2, GUI.skin.button), content3, GUI.skin.button))
            {
                this.SendErrorResult("Error: Error button pressed");
                UnityEngine.Object.Destroy(this);
            }
            GUILayout.EndHorizontal();
        }

        protected virtual void SendCancelResult()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary["cancelled"] = true;
            if (!string.IsNullOrEmpty(this.CallbackID))
            {
                dictionary["callback_id"] = this.CallbackID;
            }
            this.Callback(Json.Serialize(dictionary));
        }

        protected virtual void SendErrorResult(string errorMessage)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary["error"] = errorMessage;
            if (!string.IsNullOrEmpty(this.CallbackID))
            {
                dictionary["callback_id"] = this.CallbackID;
            }
            this.Callback(Json.Serialize(dictionary));
        }

        protected abstract void SendSuccessResult();
        public void Start()
        {
            this.modalRect = new Rect(10f, 10f, (float) (Screen.width - 20), (float) (Screen.height - 20));
            Texture2D textured = new Texture2D(1, 1);
            textured.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f, 1f));
            textured.Apply();
            this.modalStyle = new GUIStyle();
            this.modalStyle.normal.background = textured;
        }

        public OnComplete Callback
        {
            [CompilerGenerated]
            protected get
            {
                return this.<Callback>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Callback>k__BackingField = value;
            }
        }

        public string CallbackID
        {
            [CompilerGenerated]
            protected get
            {
                return this.<CallbackID>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<CallbackID>k__BackingField = value;
            }
        }

        protected abstract string DialogTitle { get; }

        public delegate void OnComplete(string result);
    }
}

