namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class MenuTreasureChestProvider : IInstanceProvider<MenuTreasureChest>
    {
        private GameObjectProvider m_gop;

        public MenuTreasureChestProvider(string resourceName, int layer)
        {
            this.m_gop = new GameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public MenuTreasureChest instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            MenuTreasureChest component = obj2.GetComponent<MenuTreasureChest>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(MenuTreasureChest obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(MenuTreasureChest obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

