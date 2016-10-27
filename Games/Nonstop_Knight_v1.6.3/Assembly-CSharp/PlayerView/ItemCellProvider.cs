namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class ItemCellProvider : IInstanceProvider<ItemCell>
    {
        private GameObjectProvider m_gop;

        public ItemCellProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public ItemCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            ItemCell component = obj2.GetComponent<ItemCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(ItemCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(ItemCell obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

