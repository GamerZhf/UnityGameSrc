namespace GameLogic
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class PhysicsUtil
    {
        public static void ApplyDrag(ref Vector3 vel, float dragPerSecond, float dt)
        {
            float magnitude = vel.magnitude;
            float num2 = magnitude * dragPerSecond;
            float num3 = magnitude - (num2 * dt);
            vel = Vector3.ClampMagnitude(vel, Mathf.Clamp(num3, 0f, float.MaxValue));
        }

        public static float DistBetween(CharacterInstance a, CharacterInstance b)
        {
            return Vector3.Distance(a.PhysicsBody.Transform.position, b.PhysicsBody.Transform.position);
        }

        public static float DistBetween(CharacterInstance a, Vector3 worldPt)
        {
            return Vector3.Distance(a.PhysicsBody.Transform.position, worldPt);
        }

        public static bool IsOnSurface(Vector3 pos, float distDown, [Optional, DefaultParameterValue(-1)] int layerMask)
        {
            if (layerMask != -1)
            {
                return Physics.Raycast(pos, Vector3.down, distDown, layerMask);
            }
            return Physics.Raycast(pos, Vector3.down, distDown);
        }

        public static bool IsOnSurface(Transform tm, Collider collider, [Optional, DefaultParameterValue(0.1f)] float maxDist, [Optional, DefaultParameterValue(-1)] int layerMask)
        {
            return IsOnSurface(tm.position, collider.bounds.extents.y + maxDist, layerMask);
        }

        public static bool IsOnSurfaceSphereCast(Vector3 pos, float distDown, float sphereRadius, [Optional, DefaultParameterValue(-1)] int layerMask)
        {
            RaycastHit hit;
            if (layerMask != -1)
            {
                return Physics.SphereCast(pos, sphereRadius, Vector3.down, out hit, distDown, layerMask);
            }
            return Physics.SphereCast(pos, sphereRadius, Vector3.down, out hit, distDown, layerMask);
        }
    }
}

