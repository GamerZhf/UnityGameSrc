namespace Facebook.Unity.Editor
{
    using Facebook.Unity;

    internal class EditorFacebookLoader : FB.CompiledFacebookLoader
    {
        protected override FacebookGameObject FBGameObject
        {
            get
            {
                EditorFacebookGameObject component = ComponentFactory.GetComponent<EditorFacebookGameObject>(ComponentFactory.IfNotExist.AddNew);
                component.Facebook = new EditorFacebook();
                return component;
            }
        }
    }
}

