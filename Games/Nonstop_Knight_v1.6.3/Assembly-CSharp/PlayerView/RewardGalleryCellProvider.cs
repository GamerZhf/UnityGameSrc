namespace PlayerView
{
    using App;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class RewardGalleryCellProvider : ITypedInstanceProvider<RewardGalleryCell, RewardGalleryCellType>
    {
        private Dictionary<RewardGalleryCellType, GameObjectProvider> m_gops;
        private Transform m_objectPoolParentTm;

        public RewardGalleryCellProvider(Transform objectPoolParentTm)
        {
            this.m_objectPoolParentTm = objectPoolParentTm;
            this.m_gops = new Dictionary<RewardGalleryCellType, GameObjectProvider>(new RewardGalleryCellTypeBoxAvoidanceComparer());
        }

        public RewardGalleryCell instantiate(RewardGalleryCellType type)
        {
            if (!this.m_gops.ContainsKey(type))
            {
                this.m_gops.Add(type, new GameObjectProvider("Prefabs/Menu/" + type, this.m_objectPoolParentTm, -1));
            }
            GameObject obj2 = this.m_gops[type].instantiate();
            RewardGalleryCell component = obj2.GetComponent<RewardGalleryCell>();
            component.Type = type;
            obj2.SetActive(false);
            return component;
        }

        public void onDestroy(RewardGalleryCell obj)
        {
            this.m_gops[obj.Type].onDestroy(obj.gameObject);
        }

        public void onReturn(RewardGalleryCell obj)
        {
            this.m_gops[obj.Type].onReturn(obj.gameObject);
        }

        public void reset()
        {
            foreach (KeyValuePair<RewardGalleryCellType, GameObjectProvider> pair in this.m_gops)
            {
                pair.Value.onReset();
            }
        }
    }
}

