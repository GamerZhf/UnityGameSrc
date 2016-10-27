namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class BossHuntTickerCellProvider : IInstanceProvider<BossHuntTickerCell>
    {
        private GameObjectProvider m_gop;

        public BossHuntTickerCellProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public BossHuntTickerCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            BossHuntTickerCell component = obj2.GetComponent<BossHuntTickerCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(BossHuntTickerCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(BossHuntTickerCell obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

