namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class ItemCardProvider : IInstanceProvider<ItemCard>
    {
        private GameObjectProvider m_gop;

        public ItemCardProvider(string resourceName, int layer)
        {
            this.m_gop = new GameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public ItemCard instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            ItemCard component = obj2.GetComponent<ItemCard>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(ItemCard obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(ItemCard obj)
        {
            obj.Button.onClick.RemoveAllListeners();
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

