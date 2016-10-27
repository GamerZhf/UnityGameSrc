namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class DungeonDropViewProvider : IInstanceProvider<DungeonDropView>
    {
        private GameObjectProvider m_gop;

        public DungeonDropViewProvider(string resourceName, int layer)
        {
            this.m_gop = new GameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public DungeonDropView instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            DungeonDropView component = obj2.GetComponent<DungeonDropView>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(DungeonDropView obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(DungeonDropView obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

