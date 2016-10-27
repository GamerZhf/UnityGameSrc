namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using UnityEngine;

    public class AreaEffectProvider : ITypedInstanceProvider<AreaEffect, AreaEffectType>
    {
        private GameObjectProvider m_gop;
        private static int sm_spawnCounter;

        public AreaEffectProvider(int layer)
        {
            this.m_gop = new GameObjectProvider(string.Empty, App.Binder.PersistentObjectRootTm, layer);
        }

        public AreaEffect instantiate(AreaEffectType areaEffectType)
        {
            GameObject obj2 = this.m_gop.instantiate();
            obj2.name = "AreaEffect_" + sm_spawnCounter++;
            AreaEffect effect = null;
            switch (areaEffectType)
            {
                case AreaEffectType.RockConcentration:
                    effect = obj2.AddComponent<RockConcentrationAreaEffect>();
                    break;

                case AreaEffectType.RockRing:
                    effect = obj2.AddComponent<RockRingAreaEffect>();
                    break;

                case AreaEffectType.RockRingSector:
                    effect = obj2.AddComponent<RockRingSectorAreaEffect>();
                    break;

                default:
                    Debug.LogError("Unsupported AreaEffectType: " + areaEffectType);
                    break;
            }
            obj2.SetActive(false);
            return effect;
        }

        public void onDestroy(AreaEffect obj)
        {
            this.m_gop.onDestroy(obj.gameObject);
        }

        public void onReturn(AreaEffect obj)
        {
            this.m_gop.onReturn(obj.gameObject);
        }

        public void reset()
        {
            this.m_gop.onReset();
        }
    }
}

