﻿namespace Pathfinding.Util
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class StackPool<T>
    {
        private static readonly List<Stack<T>> pool;

        static StackPool()
        {
            StackPool<T>.pool = new List<Stack<T>>();
        }

        public static Stack<T> Claim()
        {
            if (StackPool<T>.pool.Count > 0)
            {
                Stack<T> stack = StackPool<T>.pool[StackPool<T>.pool.Count - 1];
                StackPool<T>.pool.RemoveAt(StackPool<T>.pool.Count - 1);
                return stack;
            }
            return new Stack<T>();
        }

        public static void Clear()
        {
            StackPool<T>.pool.Clear();
        }

        public static int GetSize()
        {
            return StackPool<T>.pool.Count;
        }

        public static void Release(Stack<T> stack)
        {
            for (int i = 0; i < StackPool<T>.pool.Count; i++)
            {
                if (StackPool<T>.pool[i] == stack)
                {
                    Debug.LogError("The Stack is released even though it is inside the pool");
                }
            }
            stack.Clear();
            StackPool<T>.pool.Add(stack);
        }

        public static void Warmup(int count)
        {
            Stack<T>[] stackArray = new Stack<T>[count];
            for (int i = 0; i < count; i++)
            {
                stackArray[i] = StackPool<T>.Claim();
            }
            for (int j = 0; j < count; j++)
            {
                StackPool<T>.Release(stackArray[j]);
            }
        }
    }
}

