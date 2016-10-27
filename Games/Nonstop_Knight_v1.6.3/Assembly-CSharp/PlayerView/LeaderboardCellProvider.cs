namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class LeaderboardCellProvider : IInstanceProvider<LeaderboardCell>
    {
        private GameObjectProvider m_gop;

        public LeaderboardCellProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public LeaderboardCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            LeaderboardCell component = obj2.GetComponent<LeaderboardCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(LeaderboardCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(LeaderboardCell obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

