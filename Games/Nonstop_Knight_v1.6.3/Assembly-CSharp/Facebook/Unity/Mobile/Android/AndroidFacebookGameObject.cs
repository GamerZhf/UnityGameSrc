namespace Facebook.Unity.Mobile.Android
{
    using Facebook.Unity.Mobile;
    using System;
    using UnityEngine;

    internal class AndroidFacebookGameObject : MobileFacebookGameObject
    {
        protected override void OnAwake()
        {
            AndroidJNIHelper.debug = Debug.isDebugBuild;
        }
    }
}

