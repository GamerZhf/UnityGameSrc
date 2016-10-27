namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class HpIndicatorProvider : IInstanceProvider<HpIndicator>
    {
        private GameObjectProvider m_gop;

        public HpIndicatorProvider(string resourceName, int layer)
        {
            this.m_gop = new GameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public HpIndicator instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            HpIndicator component = obj2.GetComponent<HpIndicator>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(HpIndicator obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(HpIndicator obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

