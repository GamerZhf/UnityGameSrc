using System;
using UnityEngine;

[Serializable]
public class AI : MonoBehaviour
{
    public MonoBehaviour behaviourOnLostTrack;
    public MonoBehaviour behaviourOnSpotted;
    private Transform character;
    private bool insideInterestArea = true;
    private Transform player;
    public AudioClip soundOnSpotted;

    public override void Awake()
    {
        this.character = this.transform;
        this.player = GameObject.FindWithTag("Player").transform;
    }

    public override bool CanSeePlayer()
    {
        Vector3 direction = this.player.position - this.character.position;
        RaycastHit hitInfo = new RaycastHit();
        Physics.Raycast(this.character.position, direction, out hitInfo, direction.magnitude);
        return ((hitInfo.collider != null) && (hitInfo.collider.transform == this.player));
    }

    public override void Main()
    {
    }

    public override void OnEnable()
    {
        this.behaviourOnLostTrack.enabled = true;
        this.behaviourOnSpotted.enabled = false;
    }

    public override void OnEnterInterestArea()
    {
        this.insideInterestArea = true;
    }

    public override void OnExitInterestArea()
    {
        this.insideInterestArea = false;
        this.OnLostTrack();
    }

    public override void OnLostTrack()
    {
        if (!this.behaviourOnLostTrack.enabled)
        {
            this.behaviourOnLostTrack.enabled = true;
            this.behaviourOnSpotted.enabled = false;
        }
    }

    public override void OnSpotted()
    {
        if (this.insideInterestArea && !this.behaviourOnSpotted.enabled)
        {
            this.behaviourOnSpotted.enabled = true;
            this.behaviourOnLostTrack.enabled = false;
            if ((this.GetComponent<AudioSource>() != null) && (this.soundOnSpotted != null))
            {
                this.GetComponent<AudioSource>().clip = this.soundOnSpotted;
                this.GetComponent<AudioSource>().Play();
            }
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if ((other.transform == this.player) && this.CanSeePlayer())
        {
            this.OnSpotted();
        }
    }
}

