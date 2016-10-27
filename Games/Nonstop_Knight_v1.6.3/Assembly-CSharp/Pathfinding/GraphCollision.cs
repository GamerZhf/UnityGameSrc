namespace Pathfinding
{
    using Pathfinding.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class GraphCollision
    {
        public bool collisionCheck = true;
        public float collisionOffset;
        public float diameter = 1f;
        private float finalRadius;
        private float finalRaycastRadius;
        public float fromHeight = 100f;
        public float height = 2f;
        public bool heightCheck = true;
        public LayerMask heightMask = -1;
        public LayerMask mask;
        public const float RaycastErrorMargin = 0.005f;
        public RayDirection rayDirection = RayDirection.Both;
        public bool thickRaycast;
        public float thickRaycastDiameter = 1f;
        public ColliderType type = ColliderType.Capsule;
        public bool unwalkableWhenNoGround = true;
        public Vector3 up;
        private Vector3 upheight;
        public bool use2D;

        public bool Check(Vector3 position)
        {
            if (!this.collisionCheck)
            {
                return true;
            }
            if (this.use2D)
            {
                switch (this.type)
                {
                    case ColliderType.Sphere:
                        return (Physics2D.OverlapCircle(position, this.finalRadius, (int) this.mask) == null);

                    case ColliderType.Capsule:
                        throw new Exception("Capsule mode cannot be used with 2D since capsules don't exist in 2D. Please change the Physics Testing -> Collider Type setting.");
                }
                return (Physics2D.OverlapPoint(position, (int) this.mask) == null);
            }
            position += (Vector3) (this.up * this.collisionOffset);
            switch (this.type)
            {
                case ColliderType.Sphere:
                    return !Physics.CheckSphere(position, this.finalRadius, (int) this.mask);

                case ColliderType.Capsule:
                    return !Physics.CheckCapsule(position, position + this.upheight, this.finalRadius, (int) this.mask);

                default:
                    switch (this.rayDirection)
                    {
                        case RayDirection.Up:
                            return !Physics.Raycast(position, this.up, this.height, (int) this.mask);

                        case RayDirection.Both:
                            return (!Physics.Raycast(position, this.up, this.height, (int) this.mask) && !Physics.Raycast(position + this.upheight, -this.up, this.height, (int) this.mask));
                    }
                    break;
            }
            return !Physics.Raycast(position + this.upheight, -this.up, this.height, (int) this.mask);
        }

        public Vector3 CheckHeight(Vector3 position)
        {
            RaycastHit hit;
            bool flag;
            return this.CheckHeight(position, out hit, out flag);
        }

        public Vector3 CheckHeight(Vector3 position, out RaycastHit hit, out bool walkable)
        {
            walkable = true;
            if (!this.heightCheck || this.use2D)
            {
                hit = new RaycastHit();
                return position;
            }
            if (this.thickRaycast)
            {
                Ray ray = new Ray(position + ((Vector3) (this.up * this.fromHeight)), -this.up);
                if (Physics.SphereCast(ray, this.finalRaycastRadius, out hit, this.fromHeight + 0.005f, (int) this.heightMask))
                {
                    return VectorMath.ClosestPointOnLine(ray.origin, ray.origin + ray.direction, hit.point);
                }
                walkable &= !this.unwalkableWhenNoGround;
                return position;
            }
            if (Physics.Raycast(position + ((Vector3) (this.up * this.fromHeight)), -this.up, out hit, this.fromHeight + 0.005f, (int) this.heightMask))
            {
                return hit.point;
            }
            walkable &= !this.unwalkableWhenNoGround;
            return position;
        }

        public RaycastHit[] CheckHeightAll(Vector3 position)
        {
            bool flag;
            RaycastHit hit2;
            if (!this.heightCheck || this.use2D)
            {
                RaycastHit hit = new RaycastHit();
                hit.point = position;
                hit.distance = 0f;
                return new RaycastHit[] { hit };
            }
            if (this.thickRaycast)
            {
                return new RaycastHit[0];
            }
            List<RaycastHit> list = new List<RaycastHit>();
            Vector3 origin = position + ((Vector3) (this.up * this.fromHeight));
            Vector3 zero = Vector3.zero;
            int num = 0;
        Label_0080:
            this.Raycast(origin, out hit2, out flag);
            if (hit2.transform != null)
            {
                if ((hit2.point != zero) || (list.Count == 0))
                {
                    origin = hit2.point - ((Vector3) (this.up * 0.005f));
                    zero = hit2.point;
                    num = 0;
                    list.Add(hit2);
                    goto Label_0080;
                }
                origin -= (Vector3) (this.up * 0.001f);
                num++;
                if (num <= 10)
                {
                    goto Label_0080;
                }
                Debug.LogError(string.Concat(new object[] { "Infinite Loop when raycasting. Please report this error (arongranberg.com)\n", origin, " : ", zero }));
            }
            return list.ToArray();
        }

        public void DeserializeSettings(GraphSerializationContext ctx)
        {
            this.type = (ColliderType) ctx.reader.ReadInt32();
            this.diameter = ctx.reader.ReadSingle();
            this.height = ctx.reader.ReadSingle();
            this.collisionOffset = ctx.reader.ReadSingle();
            this.rayDirection = (RayDirection) ctx.reader.ReadInt32();
            this.mask = ctx.reader.ReadInt32();
            this.heightMask = ctx.reader.ReadInt32();
            this.fromHeight = ctx.reader.ReadSingle();
            this.thickRaycast = ctx.reader.ReadBoolean();
            this.thickRaycastDiameter = ctx.reader.ReadSingle();
            this.unwalkableWhenNoGround = ctx.reader.ReadBoolean();
            this.use2D = ctx.reader.ReadBoolean();
            this.collisionCheck = ctx.reader.ReadBoolean();
            this.heightCheck = ctx.reader.ReadBoolean();
        }

        public void Initialize(Matrix4x4 matrix, float scale)
        {
            this.up = matrix.MultiplyVector(Vector3.up);
            this.upheight = (Vector3) (this.up * this.height);
            this.finalRadius = (this.diameter * scale) * 0.5f;
            this.finalRaycastRadius = (this.thickRaycastDiameter * scale) * 0.5f;
        }

        public Vector3 Raycast(Vector3 origin, out RaycastHit hit, out bool walkable)
        {
            walkable = true;
            if (!this.heightCheck || this.use2D)
            {
                hit = new RaycastHit();
                return (origin - ((Vector3) (this.up * this.fromHeight)));
            }
            if (this.thickRaycast)
            {
                Ray ray = new Ray(origin, -this.up);
                if (Physics.SphereCast(ray, this.finalRaycastRadius, out hit, this.fromHeight + 0.005f, (int) this.heightMask))
                {
                    return VectorMath.ClosestPointOnLine(ray.origin, ray.origin + ray.direction, hit.point);
                }
                walkable &= !this.unwalkableWhenNoGround;
            }
            else
            {
                if (Physics.Raycast(origin, -this.up, out hit, this.fromHeight + 0.005f, (int) this.heightMask))
                {
                    return hit.point;
                }
                walkable &= !this.unwalkableWhenNoGround;
            }
            return (origin - ((Vector3) (this.up * this.fromHeight)));
        }

        public void SerializeSettings(GraphSerializationContext ctx)
        {
            ctx.writer.Write((int) this.type);
            ctx.writer.Write(this.diameter);
            ctx.writer.Write(this.height);
            ctx.writer.Write(this.collisionOffset);
            ctx.writer.Write((int) this.rayDirection);
            ctx.writer.Write((int) this.mask);
            ctx.writer.Write((int) this.heightMask);
            ctx.writer.Write(this.fromHeight);
            ctx.writer.Write(this.thickRaycast);
            ctx.writer.Write(this.thickRaycastDiameter);
            ctx.writer.Write(this.unwalkableWhenNoGround);
            ctx.writer.Write(this.use2D);
            ctx.writer.Write(this.collisionCheck);
            ctx.writer.Write(this.heightCheck);
        }
    }
}

