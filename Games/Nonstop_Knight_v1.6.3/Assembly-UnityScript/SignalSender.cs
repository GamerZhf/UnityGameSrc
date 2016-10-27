using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class SignalSender
{
    private bool hasFired;
    public bool onlyOnce;
    public ReceiverItem[] receivers;

    public override void SendSignals(MonoBehaviour sender)
    {
        if (!this.hasFired || !this.onlyOnce)
        {
            for (int i = 0; i < Extensions.get_length(this.receivers); i++)
            {
                sender.StartCoroutine(this.receivers[i].SendWithDelay(sender));
            }
            this.hasFired = true;
        }
    }
}

