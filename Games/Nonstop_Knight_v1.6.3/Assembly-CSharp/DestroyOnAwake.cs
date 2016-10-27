using System;
using UnityEngine;

public class DestroyOnAwake : MonoBehaviour
{
    protected void Awake()
    {
        UnityEngine.Object.Destroy(base.gameObject);
    }
}

