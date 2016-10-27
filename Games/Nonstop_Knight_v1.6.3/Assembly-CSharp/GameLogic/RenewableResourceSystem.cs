namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class RenewableResourceSystem : MonoBehaviour, IRenewableResourceSystem
    {
        private Dictionary<ResourceType, Coroutine> m_collectionRoutines = new Dictionary<ResourceType, Coroutine>();
        private Dictionary<ResourceType, PersistentTimer> m_timers = new Dictionary<ResourceType, PersistentTimer>();
        public static Dictionary<ResourceType, int> RENEWABLE_RESOURCE_TIMERS;

        static RenewableResourceSystem()
        {
            Dictionary<ResourceType, int> dictionary = new Dictionary<ResourceType, int>();
            dictionary.Add(ResourceType.Energy, 900);
            RENEWABLE_RESOURCE_TIMERS = dictionary;
        }

        protected void Awake()
        {
            this.m_collectionRoutines.Add(ResourceType.Energy, null);
            this.m_timers.Add(ResourceType.Energy, new PersistentTimer((float) RENEWABLE_RESOURCE_TIMERS[ResourceType.Energy], 0L));
        }

        [DebuggerHidden]
        private IEnumerator collectRoutine(ResourceType resourceType)
        {
            <collectRoutine>c__Iterator8F iteratorf = new <collectRoutine>c__Iterator8F();
            iteratorf.resourceType = resourceType;
            iteratorf.<$>resourceType = resourceType;
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        protected void FixedUpdate()
        {
            foreach (KeyValuePair<ResourceType, PersistentTimer> pair in this.m_timers)
            {
                ResourceType key = pair.Key;
                if ((pair.Value.timeRemainingSeconds() <= 0f) && (this.m_collectionRoutines[key] == null))
                {
                    this.m_collectionRoutines[key] = UnityUtils.StartCoroutine(this, this.collectRoutine(key));
                }
            }
        }

        public float getSecondsUntilRefresh(ResourceType resourceType)
        {
            if (!this.m_timers.ContainsKey(resourceType))
            {
                return 0f;
            }
            return this.m_timers[resourceType].timeRemainingSeconds();
        }

        protected void OnDisable()
        {
            Binder.EventBus.OnGameStateInitialized -= new Events.GameStateInitialized(this.onGameStateInitialized);
        }

        protected void OnEnable()
        {
            Binder.EventBus.OnGameStateInitialized += new Events.GameStateInitialized(this.onGameStateInitialized);
        }

        private void onGameStateInitialized()
        {
            this.m_collectionRoutines[ResourceType.Energy] = UnityUtils.StartCoroutine(this, this.collectRoutine(ResourceType.Energy));
        }

        [CompilerGenerated]
        private sealed class <collectRoutine>c__Iterator8F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ResourceType <$>resourceType;
            internal RenewableResourceSystem <>f__this;
            internal Player <player>__0;
            internal ResourceType resourceType;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<player>__0 = Binder.GameState.Player;
                        this.$current = Binder.CommandProcessor.execute(new CmdCollectRenewableResources(this.<player>__0, this.resourceType), 0f);
                        this.$PC = 1;
                        return true;

                    case 1:
                        this.<>f__this.m_timers[this.resourceType].set(this.<player>__0.RenewableResourceTimestamps[this.resourceType.ToString()]);
                        this.<>f__this.m_collectionRoutines[this.resourceType] = null;
                        break;

                    default:
                        break;
                        this.$PC = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

