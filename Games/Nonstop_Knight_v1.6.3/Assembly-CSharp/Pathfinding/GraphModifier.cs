namespace Pathfinding
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [ExecuteInEditMode]
    public abstract class GraphModifier : MonoBehaviour
    {
        private GraphModifier next;
        private GraphModifier prev;
        private static GraphModifier root;
        [SerializeField, HideInInspector]
        protected ulong uniqueID;
        protected static Dictionary<ulong, GraphModifier> usedIDs = new Dictionary<ulong, GraphModifier>();

        protected GraphModifier()
        {
        }

        private void AddToLinkedList()
        {
            if (root == null)
            {
                root = this;
            }
            else
            {
                this.next = root;
                root.prev = this;
                root = this;
            }
        }

        protected virtual void Awake()
        {
            this.ConfigureUniqueID();
        }

        private void ConfigureUniqueID()
        {
            GraphModifier modifier;
            if (usedIDs.TryGetValue(this.uniqueID, out modifier) && (modifier != this))
            {
                this.Reset();
            }
            usedIDs[this.uniqueID] = this;
        }

        public static void FindAllModifiers()
        {
            GraphModifier[] modifierArray = UnityEngine.Object.FindObjectsOfType(typeof(GraphModifier)) as GraphModifier[];
            for (int i = 0; i < modifierArray.Length; i++)
            {
                if (modifierArray[i].enabled)
                {
                    modifierArray[i].OnEnable();
                }
            }
        }

        protected static List<T> GetModifiersOfType<T>() where T: GraphModifier
        {
            GraphModifier root = GraphModifier.root;
            List<T> list = new List<T>();
            while (root != null)
            {
                T item = root as T;
                if (item != null)
                {
                    list.Add(item);
                }
                root = root.next;
            }
            return list;
        }

        protected virtual void OnDestroy()
        {
            usedIDs.Remove(this.uniqueID);
        }

        protected virtual void OnDisable()
        {
            this.RemoveFromLinkedList();
        }

        protected virtual void OnEnable()
        {
            this.RemoveFromLinkedList();
            this.AddToLinkedList();
            this.ConfigureUniqueID();
        }

        public virtual void OnGraphsPostUpdate()
        {
        }

        public virtual void OnGraphsPreUpdate()
        {
        }

        public virtual void OnLatePostScan()
        {
        }

        public virtual void OnPostCacheLoad()
        {
        }

        public virtual void OnPostScan()
        {
        }

        public virtual void OnPreScan()
        {
        }

        private void RemoveFromLinkedList()
        {
            if (root == this)
            {
                root = this.next;
                if (root != null)
                {
                    root.prev = null;
                }
            }
            else
            {
                if (this.prev != null)
                {
                    this.prev.next = this.next;
                }
                if (this.next != null)
                {
                    this.next.prev = this.prev;
                }
            }
            this.prev = null;
            this.next = null;
        }

        private void Reset()
        {
            this.uniqueID = (ulong) (UnityEngine.Random.Range(0, 0x7fffffff) | (UnityEngine.Random.Range(0, 0x7fffffff) << 0x20));
            usedIDs[this.uniqueID] = this;
        }

        public static void TriggerEvent(EventType type)
        {
            if (!Application.isPlaying)
            {
                FindAllModifiers();
            }
            GraphModifier root = GraphModifier.root;
            switch (type)
            {
                case EventType.PostScan:
                    while (root != null)
                    {
                        root.OnPostScan();
                        root = root.next;
                    }
                    return;

                case EventType.PreScan:
                    while (root != null)
                    {
                        root.OnPreScan();
                        root = root.next;
                    }
                    return;

                case EventType.LatePostScan:
                    while (root != null)
                    {
                        root.OnLatePostScan();
                        root = root.next;
                    }
                    return;

                case EventType.PreUpdate:
                    while (root != null)
                    {
                        root.OnGraphsPreUpdate();
                        root = root.next;
                    }
                    return;

                case EventType.PostUpdate:
                    while (root != null)
                    {
                        root.OnGraphsPostUpdate();
                        root = root.next;
                    }
                    break;

                case EventType.PostCacheLoad:
                    while (root != null)
                    {
                        root.OnPostCacheLoad();
                        root = root.next;
                    }
                    break;
            }
        }

        public enum EventType
        {
            LatePostScan = 4,
            PostCacheLoad = 0x20,
            PostScan = 1,
            PostUpdate = 0x10,
            PreScan = 2,
            PreUpdate = 8
        }
    }
}

