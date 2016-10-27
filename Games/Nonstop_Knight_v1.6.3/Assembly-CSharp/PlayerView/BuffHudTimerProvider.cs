namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class BuffHudTimerProvider : IInstanceProvider<BuffHudTimer>
    {
        private GameObjectProvider m_gop;

        public BuffHudTimerProvider(string resourceName, int layer)
        {
            this.m_gop = new GameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public BuffHudTimer instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            BuffHudTimer component = obj2.GetComponent<BuffHudTimer>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(BuffHudTimer obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(BuffHudTimer obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

