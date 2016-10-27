namespace Service
{
    using System;

    public class ServerTime
    {
        private long clientTimeSync;
        private long serverTimeSync;

        public void SyncTime(long serverTime)
        {
            this.serverTimeSync = serverTime;
            this.clientTimeSync = this.ClientTime;
        }

        public long ClientTime
        {
            get
            {
                return TimeUtil.DateTimeToUnixTimestamp(DateTime.UtcNow);
            }
        }

        public long GameTime
        {
            get
            {
                if ((ConfigService.USE_SERVER_TIME_AS_GAME_TIME && (Binder.ServiceWatchdog != null)) && Binder.ServiceWatchdog.IsOnline)
                {
                    return this.Now;
                }
                return this.ClientTime;
            }
        }

        public long Now
        {
            get
            {
                return (this.ClientTime - (this.clientTimeSync - this.serverTimeSync));
            }
        }
    }
}

