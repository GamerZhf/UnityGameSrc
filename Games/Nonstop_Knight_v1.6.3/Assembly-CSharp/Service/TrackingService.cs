namespace Service
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class TrackingService : MonoBehaviour
    {
        private static readonly DateTime FirstOfJanuary1970 = new DateTime(0x7b2, 1, 1);
        private Queue<TrackingEventData> m_batch = new Queue<TrackingEventData>();
        private int m_dropCounter;
        private TrackingEventData m_dropEvent;
        private DateTime? m_lastFail;
        private DateTime m_lastSaved;
        private bool m_networkState;
        private double m_offlineMinutes;
        private bool m_queueChanged;

        public TrackingService()
        {
            this.Load();
        }

        public void AddEvent(TrackingEvent _trackingEvent)
        {
            if (!ConfigApp.CHEAT_MARKETING_MODE_ENABLED)
            {
                Service.Binder.EventBus.TrackingEvent(_trackingEvent);
                _trackingEvent.Payload["$utc-stamp"] = Convert.ToUInt64(DateTime.UtcNow.Subtract(FirstOfJanuary1970).TotalMilliseconds);
                this.m_batch.Enqueue(_trackingEvent.GetData());
                if (this.m_batch.Count > ConfigService.TRACKING_QUEUE_SIZE)
                {
                    this.m_batch.Dequeue();
                    this.m_dropCounter++;
                }
                this.m_queueChanged = true;
            }
        }

        public void CheckAndSave([Optional, DefaultParameterValue(false)] bool force)
        {
            if (this.m_queueChanged)
            {
                DateTime now = DateTime.Now;
                if (!force)
                {
                    TimeSpan span = (TimeSpan) (now - this.m_lastSaved);
                    if (span.Seconds <= ConfigService.TRACKING_QUEUE_SAVE_INTERVAL)
                    {
                        return;
                    }
                }
                this.Save();
            }
        }

        private Request<string> CreateTrackingRequest(List<TrackingEventData> list)
        {
            return Request<string>.PostUrl(Service.Binder.SessionData.TrackingUrl, list, false);
        }

        private void Load()
        {
            string json = IOUtil.LoadFromPersistentStorage("trackinq_queue.json");
            if (json != null)
            {
                try
                {
                    Log("Try to read queue");
                    QueueWrapper wrapper = JsonUtils.Deserialize<QueueWrapper>(json, true);
                    if (wrapper != null)
                    {
                        this.m_offlineMinutes = wrapper.offlineMinutes;
                        this.m_dropCounter = wrapper.dropCounter;
                        this.m_batch = new Queue<TrackingEventData>(wrapper.Queue);
                    }
                    Log("Tracking Queue loaded #" + this.m_batch.Count);
                }
                catch (Exception exception)
                {
                    UnityEngine.Debug.LogError(exception);
                }
            }
            this.m_lastSaved = DateTime.Now;
            this.m_queueChanged = false;
        }

        private static void Log(string str)
        {
            Service.Binder.Logger.Log(str);
        }

        private List<TrackingEventData> PrepareWithSession()
        {
            List<TrackingEventData> list = new List<TrackingEventData>();
            if (this.m_dropCounter > 0)
            {
                double num = !this.m_lastFail.HasValue ? 0.0 : (DateTime.Now - this.m_lastFail.Value).TotalMinutes;
                TrackingEvent event2 = new TrackingEvent("data_discarded");
                event2.Payload["events_dropped"] = this.m_dropCounter;
                event2.Payload["time_played_offline"] = (int) (this.m_offlineMinutes + num);
                TrackingEventData item = event2.GetData();
                item.sid = Service.Binder.SessionData.SessionId;
                item.appversion = Service.Binder.SessionData.ClientVersion;
                list.Add(item);
            }
            foreach (TrackingEventData data2 in this.m_batch)
            {
                if (data2.sid == null)
                {
                    data2.sid = Service.Binder.SessionData.SessionId;
                    data2.appversion = Service.Binder.SessionData.ClientVersion;
                }
                list.Add(data2);
            }
            return list;
        }

        public void Reset()
        {
            this.m_batch.Clear();
            this.Save();
        }

        private void Save()
        {
            QueueWrapper dataObject = new QueueWrapper();
            dataObject.Queue = new List<TrackingEventData>(this.m_batch);
            dataObject.offlineMinutes = this.m_offlineMinutes;
            dataObject.dropCounter = this.m_dropCounter;
            if (IOUtil.SaveToPersistentStorage(JsonUtils.Serialize(dataObject), "trackinq_queue.json", ConfigApp.PersistentStorageEncryptionEnabled, true))
            {
                this.m_queueChanged = false;
                Log("Tracking Queue saved #" + this.m_batch.Count);
            }
            else
            {
                Log("Failed to save Tracking Queue");
            }
            this.m_lastSaved = DateTime.Now;
        }

        [DebuggerHidden]
        public IEnumerator SendBatch()
        {
            <SendBatch>c__Iterator238 iterator = new <SendBatch>c__Iterator238();
            iterator.<>f__this = this;
            return iterator;
        }

        private void Start()
        {
        }

        public void StartTrackingService()
        {
            Service.Binder.TaskManager.StartTask(this.SendBatch(), null);
        }

        [CompilerGenerated]
        private sealed class <SendBatch>c__Iterator238 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TrackingService <>f__this;
            internal List<TrackingEventData> <eventList>__1;
            internal IEnumerator <ie>__3;
            internal Request<string> <req>__2;
            internal IEnumerator <warmupDelayIe>__0;

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
                        this.<warmupDelayIe>__0 = TimeUtil.WaitForUnscaledSeconds((float) ConfigService.TRACKING_INITIAL_WARMUP_DELAY_SECONDS);
                        break;

                    case 1:
                        break;

                    case 2:
                        if (!this.<req>__2.Success)
                        {
                            this.<>f__this.m_offlineMinutes += !this.<>f__this.m_lastFail.HasValue ? 0.0 : (DateTime.Now - this.<>f__this.m_lastFail.Value).TotalMinutes;
                            this.<>f__this.m_lastFail = new DateTime?(DateTime.Now);
                        }
                        else
                        {
                            this.<>f__this.m_batch.Clear();
                            this.<>f__this.m_queueChanged = true;
                            this.<>f__this.m_dropCounter = 0;
                            this.<>f__this.m_lastFail = null;
                            this.<>f__this.m_offlineMinutes = 0.0;
                            this.<>f__this.CheckAndSave(true);
                        }
                        goto Label_026D;

                    case 3:
                        goto Label_02AC;

                    default:
                        goto Label_02C8;
                }
                if (this.<warmupDelayIe>__0.MoveNext())
                {
                    this.$current = this.<warmupDelayIe>__0.Current;
                    this.$PC = 1;
                    goto Label_02CA;
                }
            Label_006C:
                if ((!Service.Binder.ServiceWatchdog.IsOnline || (this.<>f__this.m_batch.Count <= 0)) || !Service.Binder.SessionData.IsLoggedIn)
                {
                    if (this.<>f__this.m_batch.Count > 0)
                    {
                        this.<>f__this.m_offlineMinutes += !this.<>f__this.m_lastFail.HasValue ? 0.0 : (DateTime.Now - this.<>f__this.m_lastFail.Value).TotalMinutes;
                        this.<>f__this.m_lastFail = new DateTime?(DateTime.Now);
                    }
                }
                else
                {
                    this.<eventList>__1 = this.<>f__this.PrepareWithSession();
                    TrackingService.Log("Send batch " + this.<>f__this.m_batch.Count);
                    this.<req>__2 = this.<>f__this.CreateTrackingRequest(this.<eventList>__1);
                    this.$current = this.<req>__2.Task;
                    this.$PC = 2;
                    goto Label_02CA;
                }
            Label_026D:
                this.<>f__this.CheckAndSave(false);
                this.<ie>__3 = TimeUtil.WaitForUnscaledSeconds((float) ConfigService.TRACKING_QUEUE_FLUSH_INTERVAL);
            Label_02AC:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 3;
                    goto Label_02CA;
                }
                goto Label_006C;
                this.$PC = -1;
            Label_02C8:
                return false;
            Label_02CA:
                return true;
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

        private class QueueWrapper
        {
            public int dropCounter;
            public double offlineMinutes;
            public List<TrackingEventData> Queue;
        }
    }
}

