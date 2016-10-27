using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class Health : MonoBehaviour
{
    private float colliderRadiusHeuristic = 1f;
    private ParticleEmitter damageEffect;
    public bool damageEffectCentered = true;
    private float damageEffectCenterYOffset;
    public float damageEffectMultiplier = 1f;
    public Transform damageEffectTransform;
    public GameObject damagePrefab;
    public SignalSender damageSignals;
    public bool dead;
    public SignalSender dieSignals;
    public float health = 100f;
    public bool invincible;
    private float lastDamageTime;
    public float maxHealth = 100f;
    public float regenerateSpeed;
    private GameObject scorchMark;
    public GameObject scorchMarkPrefab;

    public override void Awake()
    {
        this.enabled = false;
        if (this.damagePrefab != null)
        {
            if (this.damageEffectTransform == null)
            {
                this.damageEffectTransform = this.transform;
            }
            GameObject obj2 = Spawner.Spawn(this.damagePrefab, Vector3.zero, Quaternion.identity);
            obj2.transform.parent = this.damageEffectTransform;
            obj2.transform.localPosition = Vector3.zero;
            this.damageEffect = obj2.GetComponent<ParticleEmitter>();
            Vector2 vector = new Vector2(this.GetComponent<Collider>().bounds.extents.x, this.GetComponent<Collider>().bounds.extents.z);
            this.colliderRadiusHeuristic = vector.magnitude * 0.5f;
            this.damageEffectCenterYOffset = this.GetComponent<Collider>().bounds.extents.y;
        }
        if (this.scorchMarkPrefab != null)
        {
            this.scorchMark = (GameObject) UnityEngine.Object.Instantiate(this.scorchMarkPrefab, Vector3.zero, Quaternion.identity);
            this.scorchMark.SetActive(false);
        }
    }

    public override void Main()
    {
    }

    public override void OnDamage(float amount, Vector3 fromDirection)
    {
        if ((!this.invincible && !this.dead) && (amount > 0))
        {
            this.health -= amount;
            this.damageSignals.SendSignals(this);
            this.lastDamageTime = Time.time;
            if (this.regenerateSpeed > 0)
            {
                this.enabled = true;
            }
            if (this.damageEffect != null)
            {
                this.damageEffect.transform.rotation = Quaternion.LookRotation(fromDirection, Vector3.up);
                if (!this.damageEffectCentered)
                {
                    Vector3 vector = fromDirection;
                    vector.y = 0;
                    this.damageEffect.transform.position = (Vector3) ((this.transform.position + (Vector3.up * this.damageEffectCenterYOffset)) + (this.colliderRadiusHeuristic * vector));
                }
                this.damageEffect.Emit();
            }
            if (this.health <= 0)
            {
                this.health = 0;
                this.dead = true;
                this.dieSignals.SendSignals(this);
                this.enabled = false;
                if (this.scorchMark != null)
                {
                    float num;
                    Vector3 vector3;
                    this.scorchMark.SetActive(true);
                    Vector3 vector2 = this.GetComponent<Collider>().ClosestPointOnBounds(this.transform.position - (Vector3.up * 100));
                    this.scorchMark.transform.position = vector2 + ((Vector3) (Vector3.up * 0.1f));
                    float single1 = num = UnityEngine.Random.Range((float) 0, 90f);
                    Vector3 vector1 = vector3 = this.scorchMark.transform.eulerAngles;
                    float single2 = vector3.y = num;
                    Vector3 vector5 = this.scorchMark.transform.eulerAngles = vector3;
                }
            }
        }
    }

    public override void OnEnable()
    {
        this.StartCoroutine_Auto(this.Regenerate());
    }

    public override IEnumerator Regenerate()
    {
        return new $Regenerate$91(this).GetEnumerator();
    }

    [Serializable, CompilerGenerated]
    internal sealed class $Regenerate$91 : GenericGenerator<WaitForSeconds>
    {
        internal Health $self_$93;

        public $Regenerate$91(Health self_)
        {
            this.$self_$93 = self_;
        }

        public override IEnumerator<WaitForSeconds> GetEnumerator()
        {
            return new $(this.$self_$93);
        }

        [Serializable, CompilerGenerated]
        internal sealed class $ : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
        {
            internal Health $self_$92;

            public $(Health self_)
            {
                this.$self_$92 = self_;
            }

            public override bool MoveNext()
            {
                // This item is obfuscated and can not be translated.
                switch (base._state)
                {
                    case 1:
                        goto Label_00E4;

                    case 2:
                        goto Label_0078;

                    case 3:
                        break;

                    default:
                        if (this.$self_$92.regenerateSpeed <= 0)
                        {
                            goto Label_00DB;
                        }
                        break;
                }
                while (this.$self_$92.enabled)
                {
                    if (Time.time > (this.$self_$92.lastDamageTime + 3))
                    {
                        this.$self_$92.health += this.$self_$92.regenerateSpeed;
                    }
                    goto Label_00E4;
                Label_0078:
                    if (this.$self_$92.health >= this.$self_$92.maxHealth)
                    {
                        this.$self_$92.health = this.$self_$92.maxHealth;
                        this.$self_$92.enabled = false;
                    }
                    goto Label_00E4;
                }
            Label_00DB:
                this.YieldDefault(1);
            Label_00E4:
                return false;
            }
        }
    }
}

