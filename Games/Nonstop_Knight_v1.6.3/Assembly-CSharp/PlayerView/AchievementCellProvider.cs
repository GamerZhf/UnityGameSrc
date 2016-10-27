namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class AchievementCellProvider : IInstanceProvider<AchievementCell>
    {
        private GameObjectProvider m_gop;

        public AchievementCellProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public AchievementCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            AchievementCell component = obj2.GetComponent<AchievementCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(AchievementCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(AchievementCell obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

