namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class LeaguePlayerCellProvider : IInstanceProvider<LeaguePlayerCell>
    {
        private GameObjectProvider m_gop;

        public LeaguePlayerCellProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public LeaguePlayerCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            LeaguePlayerCell component = obj2.GetComponent<LeaguePlayerCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(LeaguePlayerCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(LeaguePlayerCell obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

