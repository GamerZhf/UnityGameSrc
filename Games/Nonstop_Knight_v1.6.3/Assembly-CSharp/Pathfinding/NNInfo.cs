namespace Pathfinding
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct NNInfo
    {
        public readonly GraphNode node;
        public readonly Vector3 position;
        public NNInfo(NNInfoInternal internalInfo)
        {
            this.node = internalInfo.node;
            this.position = internalInfo.clampedPosition;
        }

        [Obsolete("This field has been renamed to 'position'")]
        public Vector3 clampedPosition
        {
            get
            {
                return this.position;
            }
        }
        public static explicit operator Vector3(NNInfo ob)
        {
            return ob.position;
        }

        public static explicit operator GraphNode(NNInfo ob)
        {
            return ob.node;
        }
    }
}

