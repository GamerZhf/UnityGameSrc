namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class SkillCellProvider : IInstanceProvider<SkillCell>
    {
        private GameObjectProvider m_gop;

        public SkillCellProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public SkillCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            SkillCell component = obj2.GetComponent<SkillCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(SkillCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(SkillCell obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

