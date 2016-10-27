namespace PlayerView
{
    using System;
    using UnityEngine;

    public class CullingDebugger : MonoBehaviour
    {
        protected void OnBecameInvisible()
        {
            Debug.Log("CULLED: " + base.name);
        }

        protected void OnBecameVisible()
        {
            Debug.Log("NON-CULLED: " + base.name);
        }
    }
}

