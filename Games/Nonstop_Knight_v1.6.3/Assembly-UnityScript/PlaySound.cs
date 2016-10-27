using System;
using UnityEngine;

[Serializable]
public class PlaySound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sound;

    public override void Awake()
    {
        if ((this.audioSource == null) && (this.GetComponent<AudioSource>() != null))
        {
            this.audioSource = this.GetComponent<AudioSource>();
        }
    }

    public override void Main()
    {
    }

    public override void OnSignal()
    {
        if (this.sound != null)
        {
            this.audioSource.clip = this.sound;
        }
        this.audioSource.Play();
    }
}

