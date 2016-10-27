using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable, RequireComponent(typeof(PerFrameRaycast))]
public class LaserScope : MonoBehaviour
{
    private float aniDir = 1f;
    private float aniTime;
    private LineRenderer lRenderer;
    public float maxWidth = 0.5f;
    public float minWidth = 0.2f;
    public float noiseSize = 1f;
    public GameObject pointer;
    public float pulseSpeed = 1.5f;
    private PerFrameRaycast raycast;
    public float scrollSpeed = 0.5f;

    public override IEnumerator ChoseNewAnimationTargetCoroutine()
    {
        return new $ChoseNewAnimationTargetCoroutine$83(this).GetEnumerator();
    }

    public override void Main()
    {
    }

    public override void Start()
    {
        this.lRenderer = ((LineRenderer) this.gameObject.GetComponent(typeof(LineRenderer))) as LineRenderer;
        this.aniTime = 0;
        this.StartCoroutine_Auto(this.ChoseNewAnimationTargetCoroutine());
        this.raycast = this.GetComponent<PerFrameRaycast>();
    }

    public override void Update()
    {
        float num3;
        Vector2 vector;
        float single1 = num3 = this.GetComponent<Renderer>().material.mainTextureOffset.x + ((Time.deltaTime * this.aniDir) * this.scrollSpeed);
        Vector2 vector1 = vector = this.GetComponent<Renderer>().material.mainTextureOffset;
        float single2 = vector.x = num3;
        Vector2 vector10 = this.GetComponent<Renderer>().material.mainTextureOffset = vector;
        this.GetComponent<Renderer>().material.SetTextureOffset("_NoiseTex", new Vector2((-Time.time * this.aniDir) * this.scrollSpeed, (float) 0));
        float b = Mathf.PingPong(Time.time * this.pulseSpeed, 1f);
        b = Mathf.Max(this.minWidth, b) * this.maxWidth;
        this.lRenderer.SetWidth(b, b);
        RaycastHit hitInfo = this.raycast.GetHitInfo();
        if (hitInfo.transform != null)
        {
            float num4;
            Vector2 vector2;
            this.lRenderer.SetPosition(1, (Vector3) (hitInfo.distance * Vector3.forward));
            float single3 = num4 = 0.1f * hitInfo.distance;
            Vector2 vector11 = vector2 = this.GetComponent<Renderer>().material.mainTextureScale;
            float single4 = vector2.x = num4;
            Vector2 vector12 = this.GetComponent<Renderer>().material.mainTextureScale = vector2;
            this.GetComponent<Renderer>().material.SetTextureScale("_NoiseTex", new Vector2((0.1f * hitInfo.distance) * this.noiseSize, this.noiseSize));
            if (this.pointer != null)
            {
                float num5;
                Vector3 vector3;
                this.pointer.GetComponent<Renderer>().enabled = true;
                this.pointer.transform.position = hitInfo.point + ((Vector3) ((this.transform.position - hitInfo.point) * 0.01f));
                this.pointer.transform.rotation = Quaternion.LookRotation(hitInfo.normal, this.transform.up);
                float single5 = num5 = 90f;
                Vector3 vector13 = vector3 = this.pointer.transform.eulerAngles;
                float single6 = vector3.x = num5;
                Vector3 vector14 = this.pointer.transform.eulerAngles = vector3;
            }
        }
        else
        {
            float num6;
            Vector2 vector4;
            if (this.pointer != null)
            {
                this.pointer.GetComponent<Renderer>().enabled = false;
            }
            float num2 = 200f;
            this.lRenderer.SetPosition(1, (Vector3) (num2 * Vector3.forward));
            float single7 = num6 = 0.1f * num2;
            Vector2 vector15 = vector4 = this.GetComponent<Renderer>().material.mainTextureScale;
            float single8 = vector4.x = num6;
            Vector2 vector16 = this.GetComponent<Renderer>().material.mainTextureScale = vector4;
            this.GetComponent<Renderer>().material.SetTextureScale("_NoiseTex", new Vector2((0.1f * num2) * this.noiseSize, this.noiseSize));
        }
    }

    [Serializable, CompilerGenerated]
    internal sealed class $ChoseNewAnimationTargetCoroutine$83 : GenericGenerator<WaitForSeconds>
    {
        internal LaserScope $self_$85;

        public $ChoseNewAnimationTargetCoroutine$83(LaserScope self_)
        {
            this.$self_$85 = self_;
        }

        public override IEnumerator<WaitForSeconds> GetEnumerator()
        {
            return new $(this.$self_$85);
        }

        [Serializable, CompilerGenerated]
        internal sealed class $ : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
        {
            internal LaserScope $self_$84;

            public $(LaserScope self_)
            {
                this.$self_$84 = self_;
            }

            public override bool MoveNext()
            {
                // This item is obfuscated and can not be translated.
                switch (base._state)
                {
                    case 1:
                        break;

                    case 2:
                        this.$self_$84.minWidth = (this.$self_$84.minWidth * 0.8f) + (UnityEngine.Random.Range((float) 0.1f, (float) 1f) * 0.2f);
                        break;

                    default:
                        this.$self_$84.aniDir = (this.$self_$84.aniDir * 0.9f) + (UnityEngine.Random.Range((float) 0.5f, (float) 1.5f) * 0.1f);
                        break;
                        this.YieldDefault(1);
                        break;
                }
                return false;
            }
        }
    }
}

