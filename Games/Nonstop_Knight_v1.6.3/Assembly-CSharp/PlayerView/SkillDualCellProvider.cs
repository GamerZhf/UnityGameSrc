namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class SkillDualCellProvider : IInstanceProvider<SkillDualCell>
    {
        private GameObjectProvider m_gop;

        public SkillDualCellProvider(string resourceName, int layer)
        {
            this.m_gop = new AssetBundleGameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public SkillDualCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            SkillDualCell component = obj2.GetComponent<SkillDualCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(SkillDualCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(SkillDualCell obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

