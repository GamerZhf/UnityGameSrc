namespace Facebook.Unity.Example
{
    using Facebook.Unity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    internal sealed class MainMenu : MenuBase
    {
        private void CallFBLogin()
        {
            List<string> permissions = new List<string>();
            permissions.Add("public_profile");
            permissions.Add("email");
            permissions.Add("user_friends");
            FB.LogInWithReadPermissions(permissions, new FacebookDelegate<ILoginResult>(this.HandleResult));
        }

        private void CallFBLoginForPublish()
        {
            List<string> permissions = new List<string>();
            permissions.Add("publish_actions");
            FB.LogInWithPublishPermissions(permissions, new FacebookDelegate<ILoginResult>(this.HandleResult));
        }

        private void CallFBLogout()
        {
            FB.LogOut();
        }

        protected override void GetGui()
        {
            bool enabled = GUI.enabled;
            if (base.Button("FB.Init"))
            {
                FB.Init(new InitDelegate(this.OnInitComplete), new HideUnityDelegate(this.OnHideUnity), null);
                base.Status = "FB.Init() called with " + FB.AppId;
            }
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUI.enabled = enabled && FB.IsInitialized;
            if (base.Button("Login"))
            {
                this.CallFBLogin();
                base.Status = "Login called";
            }
            GUI.enabled = FB.IsLoggedIn;
            if (base.Button("Get publish_actions"))
            {
                this.CallFBLoginForPublish();
                base.Status = "Login (for publish_actions) called";
            }
            if (base.Button("Logout"))
            {
                this.CallFBLogout();
                base.Status = "Logout called";
            }
            GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.MinWidth((float) ConsoleBase.MarginFix) };
            GUILayout.Label(GUIContent.none, options);
            GUILayout.EndHorizontal();
            GUI.enabled = enabled && FB.IsInitialized;
            if (base.Button("Share Dialog"))
            {
                base.SwitchMenu(typeof(DialogShare));
            }
            bool flag2 = GUI.enabled;
            GUI.enabled = (enabled && (AccessToken.CurrentAccessToken != null)) && Enumerable.Contains<string>(AccessToken.CurrentAccessToken.Permissions, "publish_actions");
            if (base.Button("Game Groups"))
            {
                base.SwitchMenu(typeof(GameGroups));
            }
            GUI.enabled = flag2;
            if (base.Button("App Requests"))
            {
                base.SwitchMenu(typeof(AppRequests));
            }
            if (base.Button("Graph Request"))
            {
                base.SwitchMenu(typeof(GraphRequest));
            }
            if (Constants.IsWeb && base.Button("Pay"))
            {
                base.SwitchMenu(typeof(Pay));
            }
            if (base.Button("App Events"))
            {
                base.SwitchMenu(typeof(AppEvents));
            }
            if (base.Button("App Links"))
            {
                base.SwitchMenu(typeof(AppLinks));
            }
            if (Constants.IsMobile && base.Button("App Invites"))
            {
                base.SwitchMenu(typeof(AppInvites));
            }
            if (Constants.IsMobile && base.Button("Access Token"))
            {
                base.SwitchMenu(typeof(AccessTokenMenu));
            }
            GUI.enabled = enabled;
        }

        private void OnHideUnity(bool isGameShown)
        {
            base.Status = "Success - Check log for details";
            base.LastResponse = string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown);
            LogView.AddLog("Is game shown: " + isGameShown);
        }

        private void OnInitComplete()
        {
            base.Status = "Success - Check log for details";
            base.LastResponse = "Success Response: OnInitComplete Called\n";
            LogView.AddLog(string.Format("OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'", FB.IsLoggedIn, FB.IsInitialized));
        }

        protected override bool ShowBackButton()
        {
            return false;
        }
    }
}

