namespace GameLogic
{
    using App;
    using System;
    using UnityEngine;

    public class DungeonBoostProvider : IInstanceProvider<DungeonBoost>
    {
        private GameObjectProvider m_gop;
        private static int sm_spawnCounter;

        public DungeonBoostProvider(int layer)
        {
            this.m_gop = new GameObjectProvider(string.Empty, App.Binder.PersistentObjectRootTm, layer);
        }

        public DungeonBoost instantiate()
        {
            GameObject obj2 = this.m_gop.instantiate();
            obj2.name = "DungeonBoost_" + sm_spawnCounter++;
            DungeonBoost boost = obj2.AddComponent<DungeonBoost>();
            obj2.SetActive(false);
            return boost;
        }

        public void onDestroy(DungeonBoost obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReset()
        {
            this.m_gop.onReset();
        }

        public void onReturn(DungeonBoost obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }
    }
}

