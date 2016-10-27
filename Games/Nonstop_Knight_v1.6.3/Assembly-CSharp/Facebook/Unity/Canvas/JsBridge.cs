namespace Facebook.Unity.Canvas
{
    using Facebook.Unity;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    internal class JsBridge : MonoBehaviour
    {
        private ICanvasFacebookCallbackHandler facebook;

        public void OnAppRequestsComplete([Optional, DefaultParameterValue("")] string responseJsonData)
        {
            this.facebook.OnAppRequestsComplete(responseJsonData);
        }

        public void OnFacebookAuthResponseChange([Optional, DefaultParameterValue("")] string responseJsonData)
        {
            this.facebook.OnFacebookAuthResponseChange(responseJsonData);
        }

        public void OnFacebookFocus(string state)
        {
            this.facebook.OnHideUnity(state != "hide");
        }

        public void OnGroupCreateComplete([Optional, DefaultParameterValue("")] string responseJsonData)
        {
            this.facebook.OnGroupCreateComplete(responseJsonData);
        }

        public void OnInitComplete([Optional, DefaultParameterValue("")] string responseJsonData)
        {
            this.facebook.OnInitComplete(responseJsonData);
        }

        public void OnJoinGroupComplete([Optional, DefaultParameterValue("")] string responseJsonData)
        {
            this.facebook.OnGroupJoinComplete(responseJsonData);
        }

        public void OnLoginComplete([Optional, DefaultParameterValue("")] string responseJsonData)
        {
            this.facebook.OnLoginComplete(responseJsonData);
        }

        public void OnPayComplete([Optional, DefaultParameterValue("")] string responseJsonData)
        {
            this.facebook.OnPayComplete(responseJsonData);
        }

        public void OnShareLinkComplete([Optional, DefaultParameterValue("")] string responseJsonData)
        {
            this.facebook.OnShareLinkComplete(responseJsonData);
        }

        public void OnUrlResponse([Optional, DefaultParameterValue("")] string url)
        {
            this.facebook.OnUrlResponse(url);
        }

        public void Start()
        {
            this.facebook = ComponentFactory.GetComponent<CanvasFacebookGameObject>(ComponentFactory.IfNotExist.ReturnNull);
        }
    }
}

