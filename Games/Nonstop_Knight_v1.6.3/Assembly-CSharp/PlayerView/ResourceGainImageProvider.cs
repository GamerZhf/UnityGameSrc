namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class ResourceGainImageProvider : IInstanceProvider<ResourceGainImage>
    {
        private GameObjectProvider m_gop;

        public ResourceGainImageProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public ResourceGainImage instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            ResourceGainImage component = obj2.GetComponent<ResourceGainImage>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(ResourceGainImage obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(ResourceGainImage obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

