namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class SkillHudButtonProvider : IInstanceProvider<SkillHudButton>
    {
        private GameObjectProvider m_gop;

        public SkillHudButtonProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public SkillHudButton instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            SkillHudButton component = obj2.GetComponent<SkillHudButton>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(SkillHudButton obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(SkillHudButton obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

