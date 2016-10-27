using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Extension]
public static class QuaternionExtensions
{
    [Extension]
    public static Quaternion RotateTowardsPoint(Quaternion self, Vector3 fromPt, Vector3 toPt, float t)
    {
        Quaternion quaternion;
        Vector3 forward = toPt - fromPt;
        if (forward != Vector3.zero)
        {
            quaternion = Quaternion.LookRotation(forward, Vector3.up);
        }
        else
        {
            quaternion = self;
        }
        return Quaternion.Lerp(self, quaternion, t);
    }
}

