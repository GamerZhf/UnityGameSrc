namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class DungeonBoostViewProvider : ITypedInstanceProvider<DungeonBoostView, DungeonBoostType>
    {
        private string m_categoryId;
        private Dictionary<DungeonBoostType, GameObjectDirectoryProvider> m_gops;
        private int m_layer;
        private Transform m_objectPoolParentTm;

        public DungeonBoostViewProvider(int layer, Transform objectPoolParentTm, string categoryId)
        {
            this.m_layer = layer;
            this.m_objectPoolParentTm = objectPoolParentTm;
            this.m_gops = new Dictionary<DungeonBoostType, GameObjectDirectoryProvider>(new DungeonBoostTypeBoxAvoidanceComparer());
            this.m_categoryId = categoryId;
        }

        public DungeonBoostView instantiate(DungeonBoostType type)
        {
            if (!this.m_gops.ContainsKey(type))
            {
                object[] objArray1 = new object[] { "Prefabs/DungeonBoosts/", this.m_categoryId, "/", type };
                this.m_gops.Add(type, new GameObjectDirectoryProvider(string.Concat(objArray1), this.m_objectPoolParentTm, this.m_layer));
            }
            GameObject obj2 = this.m_gops[type].instantiate();
            DungeonBoostView component = obj2.GetComponent<DungeonBoostView>();
            if (component == null)
            {
                component = obj2.AddComponent<DungeonBoostView>();
            }
            component.Type = type;
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(DungeonBoostView obj)
        {
            this.m_gops[obj.Type].onDestroy(obj.gameObject);
        }

        public void onReturn(DungeonBoostView obj)
        {
            this.m_gops[obj.Type].onReturn(obj.gameObject);
        }

        public void reset()
        {
            foreach (KeyValuePair<DungeonBoostType, GameObjectDirectoryProvider> pair in this.m_gops)
            {
                pair.Value.onReset();
            }
        }
    }
}

