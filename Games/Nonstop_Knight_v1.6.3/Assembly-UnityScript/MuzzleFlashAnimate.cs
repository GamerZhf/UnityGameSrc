using System;
using UnityEngine;

[Serializable]
public class MuzzleFlashAnimate : MonoBehaviour
{
    public override void Main()
    {
    }

    public override void Update()
    {
        float num;
        Vector3 vector;
        this.transform.localScale = (Vector3) (Vector3.one * UnityEngine.Random.Range((float) 0.5f, (float) 1.5f));
        float single1 = num = UnityEngine.Random.Range((float) 0, 90f);
        Vector3 vector1 = vector = this.transform.localEulerAngles;
        float single2 = vector.z = num;
        Vector3 vector3 = this.transform.localEulerAngles = vector;
    }
}

