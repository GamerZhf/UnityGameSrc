namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class CombatTextProviderMenu : IInstanceProvider<CombatTextMenu>
    {
        private GameObjectProvider m_gop;

        public CombatTextProviderMenu(string resourceName, int layer)
        {
            this.m_gop = new GameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public CombatTextMenu instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            CombatTextMenu component = obj2.GetComponent<CombatTextMenu>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(CombatTextMenu obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(CombatTextMenu obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

