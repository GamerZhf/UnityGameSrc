namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class StatusIndicatorProvider : IInstanceProvider<StatusIndicator>
    {
        private GameObjectProvider m_gop;

        public StatusIndicatorProvider(string resourceName, int layer)
        {
            this.m_gop = new GameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public StatusIndicator instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            StatusIndicator component = obj2.GetComponent<StatusIndicator>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(StatusIndicator obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(StatusIndicator obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

