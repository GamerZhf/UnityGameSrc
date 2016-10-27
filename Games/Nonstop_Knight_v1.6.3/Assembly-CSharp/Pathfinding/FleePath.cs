namespace Pathfinding
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class FleePath : RandomPath
    {
        public static FleePath Construct(Vector3 start, Vector3 avoid, int searchLength, [Optional, DefaultParameterValue(null)] OnPathDelegate callback)
        {
            FleePath path = PathPool.GetPath<FleePath>();
            path.Setup(start, avoid, searchLength, callback);
            return path;
        }

        protected void Setup(Vector3 start, Vector3 avoid, int searchLength, OnPathDelegate callback)
        {
            base.Setup(start, searchLength, callback);
            base.aim = avoid - start;
            base.aim = (Vector3) (base.aim * 10f);
            base.aim = start - base.aim;
        }
    }
}

