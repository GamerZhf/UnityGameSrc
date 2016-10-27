namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class PromotionCardProvider : IInstanceProvider<PromotionCard>
    {
        private GameObjectProvider m_gop;

        public PromotionCardProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public PromotionCard instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            PromotionCard component = obj2.GetComponent<PromotionCard>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(PromotionCard obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(PromotionCard obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

