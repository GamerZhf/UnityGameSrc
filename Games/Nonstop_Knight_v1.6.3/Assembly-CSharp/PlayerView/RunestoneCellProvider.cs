namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class RunestoneCellProvider : IInstanceProvider<RunestoneCell>
    {
        private GameObjectProvider m_gop;

        public RunestoneCellProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public RunestoneCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            RunestoneCell component = obj2.GetComponent<RunestoneCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(RunestoneCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(RunestoneCell obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

