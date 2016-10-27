namespace PlayerView
{
    using App;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class PoolableParticleSystemProvider : ITypedInstanceProvider<PoolableParticleSystem, EffectType>
    {
        private Dictionary<EffectType, GameObjectProvider> m_effectTypeGops;
        private Transform m_objectPoolParentTm;

        public PoolableParticleSystemProvider(Transform objectPoolParentTm)
        {
            this.m_objectPoolParentTm = objectPoolParentTm;
            this.m_effectTypeGops = new Dictionary<EffectType, GameObjectProvider>(new EffectTypeBoxAvoidanceComparer());
        }

        public PoolableParticleSystem instantiate(EffectType effectType)
        {
            PoolableParticleSystem system;
            if (!this.m_effectTypeGops.ContainsKey(effectType))
            {
                this.m_effectTypeGops.Add(effectType, new GameObjectProvider("Prefabs/ParticleEffects/" + effectType, this.m_objectPoolParentTm, -1));
            }
            GameObject obj2 = this.m_effectTypeGops[effectType].instantiate();
            PoolableParticleSystem component = obj2.GetComponent<PoolableParticleSystem>();
            if (component != null)
            {
                system = component;
            }
            else
            {
                system = obj2.AddComponent<PoolableParticleSystem>();
            }
            system.EffectType = effectType;
            obj2.SetActive(false);
            return system;
        }

        public void onDestroy(PoolableParticleSystem obj)
        {
            this.m_effectTypeGops[obj.EffectType].onDestroy(obj.gameObject);
        }

        public void onReturn(PoolableParticleSystem obj)
        {
            this.m_effectTypeGops[obj.EffectType].onReturn(obj.gameObject);
        }

        public void reset()
        {
            foreach (KeyValuePair<EffectType, GameObjectProvider> pair in this.m_effectTypeGops)
            {
                pair.Value.onReset();
            }
        }
    }
}

