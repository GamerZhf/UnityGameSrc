namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class DungeonCellProvider : IInstanceProvider<DungeonCell>
    {
        private GameObjectProvider m_gop;

        public DungeonCellProvider(string resourceName, int layer)
        {
            this.m_gop = new GameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public DungeonCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            DungeonCell component = obj2.GetComponent<DungeonCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(DungeonCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(DungeonCell obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

