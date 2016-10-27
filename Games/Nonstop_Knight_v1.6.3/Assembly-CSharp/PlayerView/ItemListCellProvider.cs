namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class ItemListCellProvider : IInstanceProvider<ItemListCell>
    {
        private GameObjectProvider m_gop;

        public ItemListCellProvider(string resourceName, int layer)
        {
            this.m_gop = new GameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public ItemListCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            ItemListCell component = obj2.GetComponent<ItemListCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(ItemListCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(ItemListCell obj)
        {
            obj.Button.onClick.RemoveAllListeners();
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

