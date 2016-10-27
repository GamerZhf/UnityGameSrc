namespace Pathfinding.Util
{
    using System;

    public static class ObjectPool<T> where T: class, IAstarPooledObject, new()
    {
        public static T Claim()
        {
            return ObjectPoolSimple<T>.Claim();
        }

        public static void Release(ref T obj)
        {
            T local = obj;
            ObjectPoolSimple<T>.Release(ref obj);
            local.OnEnterPool();
        }
    }
}

