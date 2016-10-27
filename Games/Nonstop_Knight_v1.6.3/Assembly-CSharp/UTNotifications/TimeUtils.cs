namespace UTNotifications
{
    using System;

    public sealed class TimeUtils
    {
        public static int DaysToSeconds(int days)
        {
            return (days * 0x15180);
        }

        public static int HoursToSeconds(int hours)
        {
            return (hours * 0xe10);
        }

        public static int MinutesToSeconds(int minutes)
        {
            return (minutes * 60);
        }

        public static int ToSecondsFromNow(DateTime dateTime)
        {
            TimeSpan span = (TimeSpan) (dateTime - DateTime.Now);
            return (int) span.TotalSeconds;
        }

        public static int WeeksToSeconds(int weeks)
        {
            return (weeks * 0x93a80);
        }
    }
}

