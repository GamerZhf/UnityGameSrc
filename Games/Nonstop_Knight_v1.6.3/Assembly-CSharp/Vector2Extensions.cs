using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Extension]
public static class Vector2Extensions
{
    [Extension]
    public static Vector2 Rotate(Vector2 v, float degrees)
    {
        return (Vector2) (Quaternion.Euler(0f, 0f, degrees) * v);
    }
}

