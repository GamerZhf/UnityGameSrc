namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;

    public class DungeonEncounterCellProvider : IInstanceProvider<DungeonEncounterCell>
    {
        private GameObjectProvider m_gop;

        public DungeonEncounterCellProvider(string resourceName, int layer)
        {
            this.m_gop = new GameObjectProvider(resourceName, App.Binder.PersistentObjectRootTm, layer);
        }

        public DungeonEncounterCell instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            DungeonEncounterCell component = obj2.GetComponent<DungeonEncounterCell>();
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(DungeonEncounterCell obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(DungeonEncounterCell obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

