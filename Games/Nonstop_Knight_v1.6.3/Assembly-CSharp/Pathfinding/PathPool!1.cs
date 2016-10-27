namespace Pathfinding
{
    using Pathfinding.Util;
    using System;

    [Obsolete("Generic version is now obsolete to trade an extremely tiny performance decrease for a large decrease in boilerplate for Path classes")]
    public static class PathPool<T> where T: Path, new()
    {
        [Obsolete("Use PathPool.GetPath<T> instead of PathPool<T>.GetPath")]
        public static T GetPath()
        {
            return PathPool.GetPath<T>();
        }

        public static int GetSize()
        {
            return PathPool.GetSize(typeof(T));
        }

        public static int GetTotalCreated()
        {
            return PathPool.GetTotalCreated(typeof(T));
        }

        public static void Recycle(T path)
        {
            PathPool.Pool(path);
        }

        public static void Warmup(int count, int length)
        {
            ListPool<GraphNode>.Warmup(count, length);
            ListPool<Vector3>.Warmup(count, length);
            Path[] o = new Path[count];
            for (int i = 0; i < count; i++)
            {
                o[i] = PathPool<T>.GetPath();
                o[i].Claim(o);
            }
            for (int j = 0; j < count; j++)
            {
                o[j].Release(o, false);
            }
        }
    }
}

