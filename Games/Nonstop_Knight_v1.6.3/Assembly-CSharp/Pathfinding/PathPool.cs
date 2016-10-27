namespace Pathfinding
{
    using System;
    using System.Collections.Generic;

    public static class PathPool
    {
        private static readonly Dictionary<Type, Stack<Path>> pool = new Dictionary<Type, Stack<Path>>();
        private static readonly Dictionary<Type, int> totalCreated = new Dictionary<Type, int>();

        public static T GetPath<T>() where T: Path, new()
        {
            Dictionary<Type, Stack<Path>> pool = PathPool.pool;
            lock (pool)
            {
                T local;
                Stack<Path> stack;
                if (PathPool.pool.TryGetValue(typeof(T), out stack) && (stack.Count > 0))
                {
                    local = stack.Pop() as T;
                }
                else
                {
                    Dictionary<Type, int> dictionary2;
                    Type type;
                    local = Activator.CreateInstance<T>();
                    if (!totalCreated.ContainsKey(typeof(T)))
                    {
                        totalCreated[typeof(T)] = 0;
                    }
                    int num = dictionary2[type];
                    (dictionary2 = totalCreated)[type = typeof(T)] = num + 1;
                }
                local.pooled = false;
                local.Reset();
                return local;
            }
        }

        public static int GetSize(Type type)
        {
            Stack<Path> stack;
            if (pool.TryGetValue(type, out stack))
            {
                return stack.Count;
            }
            return 0;
        }

        public static int GetTotalCreated(Type type)
        {
            int num;
            if (totalCreated.TryGetValue(type, out num))
            {
                return num;
            }
            return 0;
        }

        public static void Pool(Path path)
        {
            Dictionary<Type, Stack<Path>> pool = PathPool.pool;
            lock (pool)
            {
                Stack<Path> stack;
                if (path.pooled)
                {
                    throw new ArgumentException("The path is already pooled.");
                }
                if (!PathPool.pool.TryGetValue(path.GetType(), out stack))
                {
                    stack = new Stack<Path>();
                    PathPool.pool[path.GetType()] = stack;
                }
                path.pooled = true;
                path.OnEnterPool();
                stack.Push(path);
            }
        }
    }
}

