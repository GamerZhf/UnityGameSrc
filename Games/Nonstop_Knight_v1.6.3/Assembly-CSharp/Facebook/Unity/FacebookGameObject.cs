namespace Facebook.Unity
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal abstract class FacebookGameObject : MonoBehaviour, IFacebookCallbackHandler
    {
        [CompilerGenerated]
        private IFacebookImplementation <Facebook>k__BackingField;

        protected FacebookGameObject()
        {
        }

        public void Awake()
        {
            UnityEngine.Object.DontDestroyOnLoad(this);
            AccessToken.CurrentAccessToken = null;
            this.OnAwake();
        }

        public void OnAppRequestsComplete(string message)
        {
            this.Facebook.OnAppRequestsComplete(message);
        }

        protected virtual void OnAwake()
        {
        }

        public void OnGetAppLinkComplete(string message)
        {
            this.Facebook.OnGetAppLinkComplete(message);
        }

        public void OnGroupCreateComplete(string message)
        {
            this.Facebook.OnGroupCreateComplete(message);
        }

        public void OnGroupJoinComplete(string message)
        {
            this.Facebook.OnGroupJoinComplete(message);
        }

        public void OnInitComplete(string message)
        {
            this.Facebook.OnInitComplete(message);
        }

        public void OnLoginComplete(string message)
        {
            this.Facebook.OnLoginComplete(message);
        }

        public void OnLogoutComplete(string message)
        {
            this.Facebook.OnLogoutComplete(message);
        }

        public void OnShareLinkComplete(string message)
        {
            this.Facebook.OnShareLinkComplete(message);
        }

        public IFacebookImplementation Facebook
        {
            [CompilerGenerated]
            get
            {
                return this.<Facebook>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Facebook>k__BackingField = value;
            }
        }
    }
}

