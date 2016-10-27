namespace PlayerView
{
    using App;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class PoolableAudioSourceProvider : ITypedInstanceProvider<PoolableAudioSource, AudioSourceType>
    {
        private Dictionary<AudioSourceType, GameObjectProvider> m_audioSourceTypeGops;
        private int m_layer;
        private Transform m_objectPoolParentTm;

        public PoolableAudioSourceProvider(int layer, Transform objectPoolParentTm)
        {
            this.m_layer = layer;
            this.m_objectPoolParentTm = objectPoolParentTm;
            this.m_audioSourceTypeGops = new Dictionary<AudioSourceType, GameObjectProvider>(new AudioSourceTypeBoxAvoidanceComparer());
        }

        public PoolableAudioSource instantiate(AudioSourceType ast)
        {
            PoolableAudioSource source;
            if (!this.m_audioSourceTypeGops.ContainsKey(ast))
            {
                this.m_audioSourceTypeGops.Add(ast, new GameObjectProvider("Prefabs/AudioSources/" + ast, this.m_objectPoolParentTm, this.m_layer));
            }
            GameObject obj2 = this.m_audioSourceTypeGops[ast].instantiate();
            PoolableAudioSource component = obj2.GetComponent<PoolableAudioSource>();
            if (component != null)
            {
                source = component;
            }
            else
            {
                source = obj2.AddComponent<PoolableAudioSource>();
            }
            source.AudioSourceType = ast;
            obj2.SetActive(false);
            return source;
        }

        public void onDestroy(PoolableAudioSource obj)
        {
            this.m_audioSourceTypeGops[obj.AudioSourceType].onDestroy(obj.gameObject);
        }

        public void onReturn(PoolableAudioSource obj)
        {
            this.m_audioSourceTypeGops[obj.AudioSourceType].onReturn(obj.gameObject);
        }

        public void reset()
        {
            foreach (KeyValuePair<AudioSourceType, GameObjectProvider> pair in this.m_audioSourceTypeGops)
            {
                pair.Value.onReset();
            }
        }
    }
}

