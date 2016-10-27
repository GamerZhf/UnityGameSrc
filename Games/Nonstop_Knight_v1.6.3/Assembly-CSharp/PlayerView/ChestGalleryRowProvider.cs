namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class ChestGalleryRowProvider : IInstanceProvider<ChestGalleryRow>
    {
        private GameObjectProvider m_gop;

        public ChestGalleryRowProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public ChestGalleryRow instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            ChestGalleryRow component = obj2.GetComponent<ChestGalleryRow>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(ChestGalleryRow obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(ChestGalleryRow obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

