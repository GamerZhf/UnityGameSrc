namespace Pathfinding.Util
{
    using System;
    using System.Collections.Generic;

    public static class ListPool<T>
    {
        private static readonly HashSet<List<T>> inPool;
        private const int MaxCapacitySearchLength = 8;
        private static readonly List<List<T>> pool;

        static ListPool()
        {
            ListPool<T>.pool = new List<List<T>>();
            ListPool<T>.inPool = new HashSet<List<T>>();
        }

        public static List<T> Claim()
        {
            List<List<T>> pool = ListPool<T>.pool;
            lock (pool)
            {
                if (ListPool<T>.pool.Count > 0)
                {
                    List<T> item = ListPool<T>.pool[ListPool<T>.pool.Count - 1];
                    ListPool<T>.pool.RemoveAt(ListPool<T>.pool.Count - 1);
                    ListPool<T>.inPool.Remove(item);
                    return item;
                }
                return new List<T>();
            }
        }

        public static List<T> Claim(int capacity)
        {
            List<List<T>> pool = ListPool<T>.pool;
            lock (pool)
            {
                List<T> item = null;
                int num = -1;
                for (int i = 0; (i < ListPool<T>.pool.Count) && (i < 8); i++)
                {
                    List<T> list3 = ListPool<T>.pool[(ListPool<T>.pool.Count - 1) - i];
                    if (list3.Capacity >= capacity)
                    {
                        ListPool<T>.pool.RemoveAt((ListPool<T>.pool.Count - 1) - i);
                        ListPool<T>.inPool.Remove(list3);
                        return list3;
                    }
                    if ((item == null) || (list3.Capacity > item.Capacity))
                    {
                        item = list3;
                        num = (ListPool<T>.pool.Count - 1) - i;
                    }
                }
                if (item == null)
                {
                    item = new List<T>(capacity);
                }
                else
                {
                    item.Capacity = capacity;
                    ListPool<T>.pool[num] = ListPool<T>.pool[ListPool<T>.pool.Count - 1];
                    ListPool<T>.pool.RemoveAt(ListPool<T>.pool.Count - 1);
                    ListPool<T>.inPool.Remove(item);
                }
                return item;
            }
        }

        public static void Clear()
        {
            List<List<T>> pool = ListPool<T>.pool;
            lock (pool)
            {
                ListPool<T>.pool.Clear();
            }
        }

        public static int GetSize()
        {
            return ListPool<T>.pool.Count;
        }

        public static void Release(List<T> list)
        {
            list.Clear();
            List<List<T>> pool = ListPool<T>.pool;
            lock (pool)
            {
                ListPool<T>.pool.Add(list);
            }
        }

        public static void Warmup(int count, int size)
        {
            List<List<T>> pool = ListPool<T>.pool;
            lock (pool)
            {
                List<T>[] listArray = new List<T>[count];
                for (int i = 0; i < count; i++)
                {
                    listArray[i] = ListPool<T>.Claim(size);
                }
                for (int j = 0; j < count; j++)
                {
                    ListPool<T>.Release(listArray[j]);
                }
            }
        }
    }
}

