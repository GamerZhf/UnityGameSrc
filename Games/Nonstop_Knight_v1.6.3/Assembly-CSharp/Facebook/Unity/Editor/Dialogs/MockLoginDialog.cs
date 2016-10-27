namespace Facebook.Unity.Editor.Dialogs
{
    using Facebook.MiniJSON;
    using Facebook.Unity;
    using Facebook.Unity.Editor;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class MockLoginDialog : EditorFacebookMockDialog
    {
        private string accessToken = string.Empty;

        protected override void DoGui()
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("User Access Token:", new GUILayoutOption[0]);
            GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.MinWidth(400f) };
            this.accessToken = GUILayout.TextField(this.accessToken, GUI.skin.textArea, options);
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);
            if (GUILayout.Button("Find Access Token", new GUILayoutOption[0]))
            {
                Application.OpenURL(string.Format("https://developers.facebook.com/tools/accesstoken/?app_id={0}", FB.AppId));
            }
            GUILayout.Space(20f);
        }

        protected override void SendSuccessResult()
        {
            if (string.IsNullOrEmpty(this.accessToken))
            {
                this.SendErrorResult("Empty Access token string");
            }
            else
            {
                FB.API("/me?fields=id&access_token=" + this.accessToken, HttpMethod.GET, delegate (IGraphResult graphResult) {
                    <SendSuccessResult>c__AnonStorey267 storey = new <SendSuccessResult>c__AnonStorey267 {
                        <>f__this = this
                    };
                    if (!string.IsNullOrEmpty(graphResult.Error))
                    {
                        this.SendErrorResult("Graph API error: " + graphResult.Error);
                    }
                    else
                    {
                        storey.facebookID = graphResult.ResultDictionary["id"] as string;
                        FB.API("/me/permissions?access_token=" + this.accessToken, HttpMethod.GET, new FacebookDelegate<IGraphResult>(storey.<>m__83), (IDictionary<string, string>) null);
                    }
                }, (IDictionary<string, string>) null);
            }
        }

        protected override string DialogTitle
        {
            get
            {
                return "Mock Login Dialog";
            }
        }

        [CompilerGenerated]
        private sealed class <SendSuccessResult>c__AnonStorey267
        {
            internal MockLoginDialog <>f__this;
            internal string facebookID;

            internal void <>m__83(IGraphResult permResult)
            {
                if (!string.IsNullOrEmpty(permResult.Error))
                {
                    this.<>f__this.SendErrorResult("Graph API error: " + permResult.Error);
                }
                else
                {
                    List<string> permissions = new List<string>();
                    List<string> list2 = new List<string>();
                    List<object> list3 = permResult.ResultDictionary["data"] as List<object>;
                    foreach (Dictionary<string, object> dictionary in list3)
                    {
                        if ((dictionary["status"] as string) == "granted")
                        {
                            permissions.Add(dictionary["permission"] as string);
                        }
                        else
                        {
                            list2.Add(dictionary["permission"] as string);
                        }
                    }
                    AccessToken token = new AccessToken(this.<>f__this.accessToken, this.facebookID, DateTime.Now.AddDays(60.0), permissions, new DateTime?(DateTime.Now));
                    IDictionary<string, object> dictionary2 = (IDictionary<string, object>) Json.Deserialize(token.ToJson());
                    dictionary2.Add("granted_permissions", permissions);
                    dictionary2.Add("declined_permissions", list2);
                    if (!string.IsNullOrEmpty(this.<>f__this.CallbackID))
                    {
                        dictionary2["callback_id"] = this.<>f__this.CallbackID;
                    }
                    if (this.<>f__this.Callback != null)
                    {
                        this.<>f__this.Callback(Json.Serialize(dictionary2));
                    }
                }
            }
        }
    }
}

