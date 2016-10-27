namespace Service
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class LoggingService : MonoBehaviour
    {
        private Coroutine batchSendCoroutine;
        private float lastBatchSendTimestamp;
        public const int LOG_MESSAGE_BATCH_SIZE = 5;
        public const int LOG_MESSAGE_INTERVAL_SECONDS = 5;
        public const int LOG_MESSAGE_MAX_SIZE = 10;
        public int LogLevel = 2;
        private readonly List<string> logMessages = new List<string>();

        private int LogTypeToLevel(LogType _type)
        {
            switch (_type)
            {
                case LogType.Error:
                    return 2;

                case LogType.Assert:
                    return 4;

                case LogType.Warning:
                    return 1;

                case LogType.Log:
                    return 0;

                case LogType.Exception:
                    return 3;
            }
            return 5;
        }

        protected void OnDisable()
        {
            Application.logMessageReceived -= new Application.LogCallback(this.OnLogMessageReceived);
            if (this.batchSendCoroutine != null)
            {
                base.StopCoroutine(this.batchSendCoroutine);
                this.batchSendCoroutine = null;
            }
        }

        protected void OnEnable()
        {
            Application.logMessageReceived += new Application.LogCallback(this.OnLogMessageReceived);
        }

        private void OnLogMessageReceived(string _logMessage, string _stackTrace, LogType _type)
        {
            int logLevel = this.LogLevel;
            Player player = GameLogic.Binder.GameState.Player;
            if ((player != null) && (player.ServerStats.LogLevel > -1))
            {
                logLevel = player.ServerStats.LogLevel;
            }
            if (this.LogTypeToLevel(_type) >= logLevel)
            {
                object[] args = new object[] { DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffff", CultureInfo.InvariantCulture), ConfigApp.BundleVersion, Application.platform, _type, _stackTrace, _logMessage };
                this.logMessages.Add(string.Format("[clienttime={0} version={1} platform={2} loglevel={3} stacktrace={4}] {5}", args));
                if (this.logMessages.Count > 10)
                {
                    this.logMessages.RemoveAt(0);
                }
            }
        }

        [DebuggerHidden]
        private IEnumerator SendLogMessageBatch()
        {
            <SendLogMessageBatch>c__Iterator219 iterator = new <SendLogMessageBatch>c__Iterator219();
            iterator.<>f__this = this;
            return iterator;
        }

        protected void Update()
        {
            if ((((this.batchSendCoroutine == null) && (this.logMessages.Count != 0)) && (((Service.Binder.PlayerService != null) && (Service.Binder.SessionData != null)) && Service.Binder.SessionData.IsLoggedIn)) && ((this.logMessages.Count >= 5) || ((Time.unscaledTime - this.lastBatchSendTimestamp) >= 5f)))
            {
                this.batchSendCoroutine = base.StartCoroutine(this.SendLogMessageBatch());
            }
        }

        [CompilerGenerated]
        private sealed class <SendLogMessageBatch>c__Iterator219 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal LoggingService <>f__this;
            internal List<string> <batchMessages>__0;
            internal int <index>__2;
            internal Request<string> <resp>__1;

            private void <>__Finally0()
            {
                this.<>f__this.batchSendCoroutine = null;
            }

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>__Finally0();
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.$current = new WaitForEndOfFrame();
                        this.$PC = 1;
                        goto Label_0189;

                    case 1:
                        num = 0xfffffffd;
                        break;

                    case 2:
                        break;

                    default:
                        goto Label_0187;
                }
                try
                {
                    switch (num)
                    {
                        case 2:
                            if (!this.<resp>__1.Success)
                            {
                                break;
                            }
                            this.<>f__this.lastBatchSendTimestamp = Time.unscaledTime;
                            goto Label_0180;

                        default:
                            if (((Service.Binder.PlayerService == null) || (Service.Binder.SessionData == null)) || (!Service.Binder.SessionData.IsLoggedIn || !Service.Binder.ServiceWatchdog.IsOnline))
                            {
                                goto Label_0180;
                            }
                            this.<batchMessages>__0 = new List<string>(this.<>f__this.logMessages);
                            this.<>f__this.logMessages.Clear();
                            this.<resp>__1 = Request<string>.Post("/player/{sessionId}/log", this.<batchMessages>__0);
                            this.$current = this.<resp>__1.Task;
                            this.$PC = 2;
                            flag = true;
                            goto Label_0189;
                    }
                    this.<index>__2 = this.<batchMessages>__0.Count - 1;
                    while ((this.<index>__2 >= 0) && (this.<>f__this.logMessages.Count < 10))
                    {
                        this.<>f__this.logMessages.Insert(0, this.<batchMessages>__0[this.<index>__2]);
                        this.<index>__2--;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.<>__Finally0();
                }
            Label_0180:
                this.$PC = -1;
            Label_0187:
                return false;
            Label_0189:
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
    }
}

