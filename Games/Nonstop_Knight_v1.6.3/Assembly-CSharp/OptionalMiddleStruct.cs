using System;
using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public struct OptionalMiddleStruct
{
    public string name;
    public Color color;
    public float drag;
}

