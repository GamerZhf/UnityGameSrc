using System;
using UnityEngine;

[Serializable]
public class FootstepHandler : MonoBehaviour
{
    public AudioSource audioSource;
    public FootType footType;
    private PhysicMaterial physicMaterial;

    public override void Main()
    {
    }

    public override void OnCollisionEnter(Collision collisionInfo)
    {
        this.physicMaterial = collisionInfo.collider.sharedMaterial;
    }

    public override void OnFootstep()
    {
        if (this.audioSource.enabled)
        {
            AudioClip clip = null;
            switch (this.footType)
            {
            }
            this.audioSource.pitch = UnityEngine.Random.Range((float) 0.98f, (float) 1.02f);
            this.audioSource.PlayOneShot(clip, UnityEngine.Random.Range((float) 0.8f, (float) 1.2f));
        }
    }
}

