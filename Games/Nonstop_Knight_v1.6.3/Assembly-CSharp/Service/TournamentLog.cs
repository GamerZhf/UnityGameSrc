namespace Service
{
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class TournamentLog
    {
        [CompilerGenerated]
        private List<TournamentLogEvent> <LogEvents>k__BackingField;
        public static readonly int LOG_MAX_SIZE = 10;
        [JsonIgnore]
        private LinkedList<TournamentLogEvent> m_logEventQueue;
        private TournamentView m_parent;
        [JsonIgnore]
        private LinkedList<TournamentLogEvent> m_priorityLogEventQueue;
        [JsonIgnore]
        public bool Synchronized;

        public TournamentLog()
        {
            this.m_logEventQueue = new LinkedList<TournamentLogEvent>();
            this.m_priorityLogEventQueue = new LinkedList<TournamentLogEvent>();
            this.LogEvents = new List<TournamentLogEvent>();
        }

        public TournamentLog(List<TournamentLogEvent> log)
        {
            this.m_logEventQueue = new LinkedList<TournamentLogEvent>();
            this.m_priorityLogEventQueue = new LinkedList<TournamentLogEvent>();
            this.LogEvents = log;
        }

        public List<TournamentLogEvent> GetDisplayableEvents()
        {
            List<TournamentLogEvent> list = new List<TournamentLogEvent>();
            for (int i = Math.Max(0, list.Count - LOG_MAX_SIZE); i < this.LogEvents.Count; i++)
            {
                this.LogEvents[i].PrepareForDisplay(this.m_parent);
                list.Add(this.LogEvents[i]);
            }
            return list;
        }

        public void Initialize(TournamentView parent)
        {
            this.m_parent = parent;
            if ((this.LogEvents.Count < LOG_MAX_SIZE) && ((this.LogEvents.Count == 0) || (this.LogEvents[0].Type != TournamentLogEvent.LogEventType.BossHuntStarted)))
            {
                TournamentLogEvent item = new TournamentLogEvent();
                item.Id = string.Empty;
                item.Type = TournamentLogEvent.LogEventType.BossHuntStarted;
                this.LogEvents.Insert(0, item);
            }
        }

        private TournamentLogEvent popEventQueue(LinkedList<TournamentLogEvent> queueToPop)
        {
            TournamentLogEvent item = null;
            LinkedListNode<TournamentLogEvent> first = queueToPop.First;
            if (first != null)
            {
                item = first.Value;
                queueToPop.RemoveFirst();
                this.LogEvents.Add(item);
                item.PrepareForDisplay(this.m_parent);
            }
            return item;
        }

        public TournamentLogEvent PopLogEventQueue()
        {
            return this.popEventQueue(this.m_logEventQueue);
        }

        public TournamentLogEvent PopPriorityLogEventQueue()
        {
            return this.popEventQueue(this.m_priorityLogEventQueue);
        }

        public void RegisterPriorityEvent(TournamentLogEvent logEvent)
        {
            for (int i = 0; i < this.LogEvents.Count; i++)
            {
                if (this.LogEvents[i].Id == logEvent.Id)
                {
                    return;
                }
            }
            for (LinkedListNode<TournamentLogEvent> node = this.m_logEventQueue.First; node != null; node = node.Next)
            {
                if (node.Value.Id == logEvent.Id)
                {
                    this.m_logEventQueue.Remove(node);
                }
            }
            this.m_priorityLogEventQueue.AddLast(logEvent);
        }

        public void UpdateLog(List<TournamentLogEvent> newEvents)
        {
            if ((newEvents != null) && (newEvents.Count != 0))
            {
                List<string> list = new List<string>();
                for (int i = 0; i < this.LogEvents.Count; i++)
                {
                    list.Add(this.LogEvents[i].Id);
                }
                foreach (TournamentLogEvent event2 in this.m_logEventQueue)
                {
                    list.Add(event2.Id);
                }
                foreach (TournamentLogEvent event3 in this.m_priorityLogEventQueue)
                {
                    list.Add(event3.Id);
                }
                List<TournamentLogEvent> list2 = new List<TournamentLogEvent>();
                for (int j = 0; j < newEvents.Count; j++)
                {
                    if (!list.Contains(newEvents[j].Id))
                    {
                        list2.Add(newEvents[j]);
                    }
                }
                if (Binder.TournamentSystem.Synchronized && this.Synchronized)
                {
                    for (int k = 0; k < list2.Count; k++)
                    {
                        if (list2[k].Type != TournamentLogEvent.LogEventType.CardPackDonated)
                        {
                            this.m_logEventQueue.AddLast(list2[k]);
                        }
                    }
                }
                else
                {
                    for (int m = 0; m < list2.Count; m++)
                    {
                        this.LogEvents.Add(list2[m]);
                    }
                    if (Binder.TournamentSystem.Synchronized)
                    {
                        this.Synchronized = true;
                    }
                }
            }
        }

        public List<TournamentLogEvent> LogEvents
        {
            [CompilerGenerated]
            get
            {
                return this.<LogEvents>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<LogEvents>k__BackingField = value;
            }
        }
    }
}

