namespace Facebook.Unity.Example
{
    using Facebook.Unity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    internal class AppRequests : MenuBase
    {
        private string[] actionTypeStrings = new string[] { "NONE", OGActionType.SEND.ToString(), OGActionType.ASKFOR.ToString(), OGActionType.TURN.ToString() };
        private string requestData = string.Empty;
        private string requestExcludes = string.Empty;
        private string requestFilter = string.Empty;
        private string requestMax = string.Empty;
        private string requestMessage = string.Empty;
        private string requestObjectID = string.Empty;
        private string requestTitle = string.Empty;
        private string requestTo = string.Empty;
        private int selectedAction;

        protected override void GetGui()
        {
            List<object> list3;
            if (base.Button("Select - Filter None"))
            {
                FacebookDelegate<IAppRequestResult> callback = new FacebookDelegate<IAppRequestResult>(this.HandleResult);
                FB.AppRequest("Test Message", null, null, null, null, string.Empty, string.Empty, callback);
            }
            if (base.Button("Select - Filter app_users"))
            {
                list3 = new List<object>();
                list3.Add("app_users");
                List<object> filters = list3;
                FB.AppRequest("Test Message", null, filters, null, 0, string.Empty, string.Empty, new FacebookDelegate<IAppRequestResult>(this.HandleResult));
            }
            if (base.Button("Select - Filter app_non_users"))
            {
                list3 = new List<object>();
                list3.Add("app_non_users");
                List<object> list2 = list3;
                FB.AppRequest("Test Message", null, list2, null, 0, string.Empty, string.Empty, new FacebookDelegate<IAppRequestResult>(this.HandleResult));
            }
            base.LabelAndTextField("Message: ", ref this.requestMessage);
            base.LabelAndTextField("To (optional): ", ref this.requestTo);
            base.LabelAndTextField("Filter (optional): ", ref this.requestFilter);
            base.LabelAndTextField("Exclude Ids (optional): ", ref this.requestExcludes);
            base.LabelAndTextField("Filters: ", ref this.requestExcludes);
            base.LabelAndTextField("Max Recipients (optional): ", ref this.requestMax);
            base.LabelAndTextField("Data (optional): ", ref this.requestData);
            base.LabelAndTextField("Title (optional): ", ref this.requestTitle);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.MaxWidth(200f * base.ScaleFactor) };
            GUILayout.Label("Request Action (optional): ", base.LabelStyle, options);
            GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.MinHeight(ConsoleBase.ButtonHeight * base.ScaleFactor), GUILayout.MaxWidth((float) (ConsoleBase.MainWindowWidth - 150)) };
            this.selectedAction = GUILayout.Toolbar(this.selectedAction, this.actionTypeStrings, base.ButtonStyle, optionArray2);
            GUILayout.EndHorizontal();
            base.LabelAndTextField("Request Object ID (optional): ", ref this.requestObjectID);
            if (base.Button("Custom App Request"))
            {
                OGActionType? selectedOGActionType = this.GetSelectedOGActionType();
                if (selectedOGActionType.HasValue)
                {
                    FB.AppRequest(this.requestMessage, selectedOGActionType.Value, this.requestObjectID, (this.requestTo == null) ? null : ((IEnumerable<string>) this.requestTo.Split(new char[] { ',' })), this.requestData, this.requestTitle, new FacebookDelegate<IAppRequestResult>(this.HandleResult));
                }
                else
                {
                    FB.AppRequest(this.requestMessage, !string.IsNullOrEmpty(this.requestTo) ? ((IEnumerable<string>) this.requestTo.Split(new char[] { ',' })) : null, !string.IsNullOrEmpty(this.requestFilter) ? Enumerable.ToList<object>(Enumerable.OfType<object>(this.requestFilter.Split(new char[] { ',' }))) : null, !string.IsNullOrEmpty(this.requestExcludes) ? ((IEnumerable<string>) this.requestExcludes.Split(new char[] { ',' })) : null, new int?(!string.IsNullOrEmpty(this.requestMax) ? int.Parse(this.requestMax) : 0), this.requestData, this.requestTitle, new FacebookDelegate<IAppRequestResult>(this.HandleResult));
                }
            }
        }

        private OGActionType? GetSelectedOGActionType()
        {
            string str = this.actionTypeStrings[this.selectedAction];
            if (str == OGActionType.SEND.ToString())
            {
                return 0;
            }
            if (str == OGActionType.ASKFOR.ToString())
            {
                return 1;
            }
            if (str == OGActionType.TURN.ToString())
            {
                return 2;
            }
            return null;
        }
    }
}

