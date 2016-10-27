using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Extension]
public static class Vector3Extensions
{
    [Extension]
    public static float SignedAngle(Vector3 self, Vector3 to)
    {
        Vector3 rhs = Vector3.Cross(Vector3.up, self);
        float num = Vector3.Angle(to, self);
        return (Mathf.Sign(Vector3.Dot(to, rhs)) * num);
    }

    [Extension]
    public static Vector2 ToXzVector2(Vector3 self)
    {
        return new Vector2(self.x, self.z);
    }

    [Extension]
    public static Vector3 ToXzVector3(Vector3 self)
    {
        return new Vector3(self.x, 0f, self.z);
    }

    [Extension]
    public static float XzDistanceTo(Vector3 self, Vector3 to)
    {
        return Vector2.Distance(ToXzVector2(self), ToXzVector2(to));
    }

    [Extension]
    public static Vector3 XzVector3DirectionTo(Vector3 self, Vector3 to)
    {
        return ToXzVector3(to - self).normalized;
    }
}

