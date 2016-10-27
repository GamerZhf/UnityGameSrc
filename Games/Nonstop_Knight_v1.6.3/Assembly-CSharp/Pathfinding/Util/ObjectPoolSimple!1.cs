namespace Pathfinding.Util
{
    using System;
    using System.Collections.Generic;

    public static class ObjectPoolSimple<T> where T: class, new()
    {
        private static readonly HashSet<T> inPool;
        private static List<T> pool;

        static ObjectPoolSimple()
        {
            ObjectPoolSimple<T>.pool = new List<T>();
            ObjectPoolSimple<T>.inPool = new HashSet<T>();
        }

        public static T Claim()
        {
            List<T> pool = ObjectPoolSimple<T>.pool;
            lock (pool)
            {
                if (ObjectPoolSimple<T>.pool.Count > 0)
                {
                    T item = ObjectPoolSimple<T>.pool[ObjectPoolSimple<T>.pool.Count - 1];
                    ObjectPoolSimple<T>.pool.RemoveAt(ObjectPoolSimple<T>.pool.Count - 1);
                    ObjectPoolSimple<T>.inPool.Remove(item);
                    return item;
                }
                return Activator.CreateInstance<T>();
            }
        }

        public static void Clear()
        {
            List<T> pool = ObjectPoolSimple<T>.pool;
            lock (pool)
            {
                ObjectPoolSimple<T>.pool.Clear();
            }
        }

        public static int GetSize()
        {
            return ObjectPoolSimple<T>.pool.Count;
        }

        public static void Release(ref T obj)
        {
            List<T> pool = ObjectPoolSimple<T>.pool;
            lock (pool)
            {
                ObjectPoolSimple<T>.pool.Add(obj);
            }
            obj = null;
        }
    }
}

