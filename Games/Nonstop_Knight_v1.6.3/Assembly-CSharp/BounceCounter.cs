using System;
using UnityEngine;

public class BounceCounter : MonoBehaviour
{
    public UnityDataConnector conn;
    public bool trackBounces = true;

    private void OnCollisionEnter(Collision c)
    {
        if (this.trackBounces)
        {
            this.conn.SaveDataOnTheCloud(base.gameObject.name, c.relativeVelocity.magnitude);
        }
    }
}

