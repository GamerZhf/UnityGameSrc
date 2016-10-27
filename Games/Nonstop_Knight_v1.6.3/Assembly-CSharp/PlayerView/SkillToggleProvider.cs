namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class SkillToggleProvider : IInstanceProvider<SkillToggle>
    {
        private GameObjectProvider m_gop;

        public SkillToggleProvider(string resourceName, int layer)
        {
            this.m_gop = new GameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public SkillToggle instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            SkillToggle component = obj2.GetComponent<SkillToggle>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(SkillToggle obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(SkillToggle obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

