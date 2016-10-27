using System;
using UnityEngine;

[Serializable]
public class HealthFlash : MonoBehaviour
{
    private float healthBlink = 1f;
    public Material healthMaterial;
    private float oneOverMaxHealth = 0.5f;
    public Health playerHealth;

    public override void Main()
    {
    }

    public override void Start()
    {
        this.oneOverMaxHealth = 1f / this.playerHealth.maxHealth;
    }

    public override void Update()
    {
        float num = this.playerHealth.health * this.oneOverMaxHealth;
        this.healthMaterial.SetFloat("_SelfIllumination", (num * 2f) * this.healthBlink);
        if (num < 0.45f)
        {
            this.healthBlink = Mathf.PingPong(Time.time * 6f, 2f);
        }
        else
        {
            this.healthBlink = 1f;
        }
    }
}

