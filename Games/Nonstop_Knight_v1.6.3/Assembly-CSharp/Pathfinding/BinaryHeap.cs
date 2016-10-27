namespace Pathfinding
{
    using System;
    using System.Runtime.InteropServices;

    public class BinaryHeap
    {
        private const int D = 4;
        public float growthFactor = 2f;
        private Tuple[] heap;
        public int numberOfItems;
        private const bool SortGScores = true;

        public BinaryHeap(int capacity)
        {
            capacity = RoundUpToNextMultipleMod1(capacity);
            this.heap = new Tuple[capacity];
            this.numberOfItems = 0;
        }

        public void Add(PathNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (this.numberOfItems == this.heap.Length)
            {
                this.Expand();
            }
            int numberOfItems = this.numberOfItems;
            uint f = node.F;
            uint g = node.G;
            while (numberOfItems != 0)
            {
                int index = (numberOfItems - 1) / 4;
                if ((f >= this.heap[index].F) && ((f != this.heap[index].F) || (g <= this.heap[index].node.G)))
                {
                    break;
                }
                this.heap[numberOfItems] = this.heap[index];
                numberOfItems = index;
            }
            this.heap[numberOfItems] = new Tuple(f, node);
            this.numberOfItems++;
        }

        public void Clear()
        {
            this.numberOfItems = 0;
        }

        private void Expand()
        {
            int num = RoundUpToNextMultipleMod1(Math.Max(this.heap.Length + 4, (int) Math.Round((double) (this.heap.Length * this.growthFactor))));
            if (num > 0x40000)
            {
                throw new Exception("Binary Heap Size really large (2^18). A heap size this large is probably the cause of pathfinding running in an infinite loop. \nRemove this check (in BinaryHeap.cs) if you are sure that it is not caused by a bug");
            }
            Tuple[] tupleArray = new Tuple[num];
            for (int i = 0; i < this.heap.Length; i++)
            {
                tupleArray[i] = this.heap[i];
            }
            this.heap = tupleArray;
        }

        internal PathNode GetNode(int i)
        {
            return this.heap[i].node;
        }

        public void Rebuild()
        {
            for (int i = 2; i < this.numberOfItems; i++)
            {
                int index = i;
                Tuple tuple = this.heap[i];
                uint f = tuple.F;
                while (index != 1)
                {
                    int num4 = index / 4;
                    if (f >= this.heap[num4].F)
                    {
                        break;
                    }
                    this.heap[index] = this.heap[num4];
                    this.heap[num4] = tuple;
                    index = num4;
                }
            }
        }

        public PathNode Remove()
        {
            int num3;
            this.numberOfItems--;
            PathNode node = this.heap[0].node;
            Tuple tuple = this.heap[this.numberOfItems];
            uint g = tuple.node.G;
            int index = 0;
        Label_0046:
            num3 = index;
            uint f = tuple.F;
            int num5 = (num3 * 4) + 1;
            if (num5 <= this.numberOfItems)
            {
                uint num6 = this.heap[num5].F;
                uint num7 = this.heap[num5 + 1].F;
                uint num8 = this.heap[num5 + 2].F;
                uint num9 = this.heap[num5 + 3].F;
                if ((num5 < this.numberOfItems) && ((num6 < f) || ((num6 == f) && (this.heap[num5].node.G < g))))
                {
                    f = num6;
                    index = num5;
                }
                if (((num5 + 1) < this.numberOfItems) && ((num7 < f) || ((num7 == f) && (this.heap[num5 + 1].node.G < ((index != num3) ? this.heap[index].node.G : g)))))
                {
                    f = num7;
                    index = num5 + 1;
                }
                if (((num5 + 2) < this.numberOfItems) && ((num8 < f) || ((num8 == f) && (this.heap[num5 + 2].node.G < ((index != num3) ? this.heap[index].node.G : g)))))
                {
                    f = num8;
                    index = num5 + 2;
                }
                if (((num5 + 3) < this.numberOfItems) && ((num9 < f) || ((num9 == f) && (this.heap[num5 + 3].node.G < ((index != num3) ? this.heap[index].node.G : g)))))
                {
                    index = num5 + 3;
                }
            }
            if (num3 != index)
            {
                this.heap[num3] = this.heap[index];
                goto Label_0046;
            }
            this.heap[index] = tuple;
            return node;
        }

        private static int RoundUpToNextMultipleMod1(int v)
        {
            return (v + ((4 - ((v - 1) % 4)) % 4));
        }

        internal void SetF(int i, uint f)
        {
            this.heap[i].F = f;
        }

        private void Validate()
        {
            for (int i = 1; i < this.numberOfItems; i++)
            {
                int index = (i - 1) / 4;
                if (this.heap[index].F > this.heap[i].F)
                {
                    object[] objArray1 = new object[] { "Invalid state at ", i, ":", index, " ( ", this.heap[index].F, " > ", this.heap[i].F, " ) " };
                    throw new Exception(string.Concat(objArray1));
                }
            }
        }

        public bool isEmpty
        {
            get
            {
                return (this.numberOfItems <= 0);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Tuple
        {
            public uint F;
            public PathNode node;
            public Tuple(uint f, PathNode node)
            {
                this.F = f;
                this.node = node;
            }
        }
    }
}

