using Boo.Lang;
using Boo.Lang.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class EffectSequencer : MonoBehaviour
{
    public ExplosionPart[] ambientEmitters;
    public ExplosionPart[] explosionEmitters;
    public ExplosionPart[] miscSpecialEffects;
    public ExplosionPart[] smokeEmitters;

    public override IEnumerator InstantiateDelayed(ExplosionPart go)
    {
        return new $InstantiateDelayed$78(go, this).GetEnumerator();
    }

    public override void Main()
    {
    }

    public override IEnumerator Start()
    {
        return new $Start$69(this).GetEnumerator();
    }

    [Serializable, CompilerGenerated]
    internal sealed class $InstantiateDelayed$78 : GenericGenerator<WaitForSeconds>
    {
        internal ExplosionPart $go$81;
        internal EffectSequencer $self_$82;

        public $InstantiateDelayed$78(ExplosionPart go, EffectSequencer self_)
        {
            this.$go$81 = go;
            this.$self_$82 = self_;
        }

        public override IEnumerator<WaitForSeconds> GetEnumerator()
        {
            return new $(this.$go$81, this.$self_$82);
        }

        [Serializable, CompilerGenerated]
        internal sealed class $ : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
        {
            internal ExplosionPart $go$79;
            internal EffectSequencer $self_$80;

            public $(ExplosionPart go, EffectSequencer self_)
            {
                this.$go$79 = go;
                this.$self_$80 = self_;
            }

            public override bool MoveNext()
            {
                // This item is obfuscated and can not be translated.
                switch (base._state)
                {
                    case 2:
                        UnityEngine.Object.Instantiate(this.$go$79.gameObject, this.$self_$80.transform.position + ((Vector3) (Vector3.up * this.$go$79.yOffset)), this.$self_$80.transform.rotation);
                        this.YieldDefault(1);
                        break;
                }
                return false;
            }
        }
    }

    [Serializable, CompilerGenerated]
    internal sealed class $Start$69 : GenericGenerator<object>
    {
        internal EffectSequencer $self_$77;

        public $Start$69(EffectSequencer self_)
        {
            this.$self_$77 = self_;
        }

        public override IEnumerator<object> GetEnumerator()
        {
            return new $(this.$self_$77);
        }

        [Serializable, CompilerGenerated]
        internal sealed class $ : GenericGeneratorEnumerator<object>, IEnumerator
        {
            internal IEnumerator $$iterator$65$72;
            internal IEnumerator $$iterator$66$73;
            internal IEnumerator $$iterator$67$74;
            internal IEnumerator $$iterator$68$75;
            internal ExplosionPart $go$70;
            internal float $maxTime$71;
            internal EffectSequencer $self_$76;

            public $(EffectSequencer self_)
            {
                this.$self_$76 = self_;
            }

            public override bool MoveNext()
            {
                // This item is obfuscated and can not be translated.
                switch (base._state)
                {
                    case 1:
                        break;

                    case 2:
                        this.$$iterator$68$75 = this.$self_$76.miscSpecialEffects.GetEnumerator();
                        while (this.$$iterator$68$75.MoveNext())
                        {
                            object current = this.$$iterator$68$75.Current;
                            if (!(current is ExplosionPart))
                            {
                            }
                            this.$go$70 = (ExplosionPart) RuntimeServices.Coerce(current, typeof(ExplosionPart));
                            this.$self_$76.StartCoroutine_Auto(this.$self_$76.InstantiateDelayed(this.$go$70));
                            if (this.$go$70.gameObject.GetComponent<ParticleEmitter>() != null)
                            {
                                this.$maxTime$71 = Mathf.Max(this.$maxTime$71, this.$go$70.delay + this.$go$70.gameObject.GetComponent<ParticleEmitter>().maxEnergy);
                            }
                        }
                        UnityEngine.Object.Destroy(this.$self_$76.gameObject, this.$maxTime$71 + 0.5f);
                        this.YieldDefault(1);
                        break;

                    default:
                        this.$go$70 = null;
                        this.$maxTime$71 = 0;
                        this.$$iterator$65$72 = this.$self_$76.ambientEmitters.GetEnumerator();
                        while (this.$$iterator$65$72.MoveNext())
                        {
                            object obj1 = this.$$iterator$65$72.Current;
                            if (!(obj1 is ExplosionPart))
                            {
                            }
                            this.$go$70 = (ExplosionPart) RuntimeServices.Coerce(obj1, typeof(ExplosionPart));
                            this.$self_$76.StartCoroutine_Auto(this.$self_$76.InstantiateDelayed(this.$go$70));
                            if (this.$go$70.gameObject.GetComponent<ParticleEmitter>() != null)
                            {
                                this.$maxTime$71 = Mathf.Max(this.$maxTime$71, this.$go$70.delay + this.$go$70.gameObject.GetComponent<ParticleEmitter>().maxEnergy);
                            }
                        }
                        this.$$iterator$66$73 = this.$self_$76.explosionEmitters.GetEnumerator();
                        while (this.$$iterator$66$73.MoveNext())
                        {
                            object obj2 = this.$$iterator$66$73.Current;
                            if (!(obj2 is ExplosionPart))
                            {
                            }
                            this.$go$70 = (ExplosionPart) RuntimeServices.Coerce(obj2, typeof(ExplosionPart));
                            this.$self_$76.StartCoroutine_Auto(this.$self_$76.InstantiateDelayed(this.$go$70));
                            if (this.$go$70.gameObject.GetComponent<ParticleEmitter>() != null)
                            {
                                this.$maxTime$71 = Mathf.Max(this.$maxTime$71, this.$go$70.delay + this.$go$70.gameObject.GetComponent<ParticleEmitter>().maxEnergy);
                            }
                        }
                        this.$$iterator$67$74 = this.$self_$76.smokeEmitters.GetEnumerator();
                        while (this.$$iterator$67$74.MoveNext())
                        {
                            object obj3 = this.$$iterator$67$74.Current;
                            if (!(obj3 is ExplosionPart))
                            {
                            }
                            this.$go$70 = (ExplosionPart) RuntimeServices.Coerce(obj3, typeof(ExplosionPart));
                            this.$self_$76.StartCoroutine_Auto(this.$self_$76.InstantiateDelayed(this.$go$70));
                            if (this.$go$70.gameObject.GetComponent<ParticleEmitter>() != null)
                            {
                                this.$maxTime$71 = Mathf.Max(this.$maxTime$71, this.$go$70.delay + this.$go$70.gameObject.GetComponent<ParticleEmitter>().maxEnergy);
                            }
                        }
                        if ((this.$self_$76.GetComponent<AudioSource>() != null) && (this.$self_$76.GetComponent<AudioSource>().clip != null))
                        {
                            this.$maxTime$71 = Mathf.Max(this.$maxTime$71, this.$self_$76.GetComponent<AudioSource>().clip.length);
                        }
                        break;
                }
                return false;
            }
        }
    }
}

