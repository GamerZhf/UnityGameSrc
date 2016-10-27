namespace Facebook.Unity.Canvas
{
    using Facebook.Unity;
    using System;
    using UnityEngine;

    internal class CanvasFacebookGameObject : FacebookGameObject, ICanvasFacebookCallbackHandler, IFacebookCallbackHandler
    {
        protected override void OnAwake()
        {
            GameObject obj2 = new GameObject("FacebookJsBridge");
            obj2.AddComponent<JsBridge>();
            obj2.transform.parent = base.gameObject.transform;
        }

        public void OnFacebookAuthResponseChange(string message)
        {
            this.CanvasFacebookImpl.OnFacebookAuthResponseChange(message);
        }

        public void OnHideUnity(bool hide)
        {
            this.CanvasFacebookImpl.OnHideUnity(hide);
        }

        public void OnPayComplete(string result)
        {
            this.CanvasFacebookImpl.OnPayComplete(result);
        }

        public void OnUrlResponse(string message)
        {
            this.CanvasFacebookImpl.OnUrlResponse(message);
        }

        protected ICanvasFacebookImplementation CanvasFacebookImpl
        {
            get
            {
                return (ICanvasFacebookImplementation) base.Facebook;
            }
        }
    }
}

