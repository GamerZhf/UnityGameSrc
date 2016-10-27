namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class ShopCellProvider : IInstanceProvider<ShopCell>
    {
        private GameObjectProvider m_gop;

        public ShopCellProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public ShopCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            ShopCell component = obj2.GetComponent<ShopCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(ShopCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(ShopCell obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

