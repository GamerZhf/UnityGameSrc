namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class LeagueTitleCellProvider : IInstanceProvider<LeagueTitleCell>
    {
        private GameObjectProvider m_gop;

        public LeagueTitleCellProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public LeagueTitleCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            LeagueTitleCell component = obj2.GetComponent<LeagueTitleCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(LeagueTitleCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(LeagueTitleCell obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

