namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class CombatTextProviderIngame : IInstanceProvider<CombatTextIngame>
    {
        private GameObjectProvider m_gop;

        public CombatTextProviderIngame(string resourceName, int layer)
        {
            this.m_gop = new GameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public CombatTextIngame instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            CombatTextIngame component = obj2.GetComponent<CombatTextIngame>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(CombatTextIngame obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(CombatTextIngame obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

