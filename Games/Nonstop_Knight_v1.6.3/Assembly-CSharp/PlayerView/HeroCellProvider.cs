namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class HeroCellProvider : IInstanceProvider<HeroCell>
    {
        private GameObjectProvider m_gop;

        public HeroCellProvider(string resourceName, int layer)
        {
            this.m_gop = new GameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public HeroCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            HeroCell component = obj2.GetComponent<HeroCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(HeroCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(HeroCell obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

