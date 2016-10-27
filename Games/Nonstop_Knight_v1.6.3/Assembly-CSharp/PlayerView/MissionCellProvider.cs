namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class MissionCellProvider : IInstanceProvider<MissionCell>
    {
        private GameObjectProvider m_gop;

        public MissionCellProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public MissionCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            MissionCell component = obj2.GetComponent<MissionCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(MissionCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(MissionCell obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

