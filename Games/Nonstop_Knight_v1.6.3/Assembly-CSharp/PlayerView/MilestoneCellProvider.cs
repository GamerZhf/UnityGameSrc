namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class MilestoneCellProvider : IInstanceProvider<MilestoneCell>
    {
        private GameObjectProvider m_gop;

        public MilestoneCellProvider(string resourceName, int layer)
        {
            this.m_gop = new GameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public MilestoneCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            MilestoneCell component = obj2.GetComponent<MilestoneCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(MilestoneCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(MilestoneCell obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

