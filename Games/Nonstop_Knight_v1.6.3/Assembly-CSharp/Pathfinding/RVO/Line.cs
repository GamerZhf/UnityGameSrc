namespace Pathfinding.RVO
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct Line
    {
        public Vector2 point;
        public Vector2 dir;
    }
}

