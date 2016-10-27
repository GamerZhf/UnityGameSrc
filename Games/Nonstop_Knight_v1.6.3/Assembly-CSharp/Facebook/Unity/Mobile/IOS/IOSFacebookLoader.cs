namespace Facebook.Unity.Mobile.IOS
{
    using Facebook.Unity;

    internal class IOSFacebookLoader : FB.CompiledFacebookLoader
    {
        protected override FacebookGameObject FBGameObject
        {
            get
            {
                IOSFacebookGameObject component = ComponentFactory.GetComponent<IOSFacebookGameObject>(ComponentFactory.IfNotExist.AddNew);
                if (component.Facebook == null)
                {
                    component.Facebook = new IOSFacebook();
                }
                return component;
            }
        }
    }
}

