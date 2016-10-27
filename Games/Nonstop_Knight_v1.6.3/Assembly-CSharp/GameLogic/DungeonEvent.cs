namespace GameLogic
{
    using System;
    using UnityEngine;

    public class DungeonEvent
    {
        public float CoinMultiplier = 1f;
        public float DropChanceMultiplier = 1f;
        public string DungeonId;
        public bool Fri;
        public bool Mon;
        public bool Sat;
        public bool Sun;
        public bool Thu;
        public bool Tue;
        public bool Wed;
        public float XpMultiplier = 1f;

        public bool activeOnWeekday(DayOfWeek weekday)
        {
            switch (weekday)
            {
                case DayOfWeek.Sunday:
                    return this.Sun;

                case DayOfWeek.Monday:
                    return this.Mon;

                case DayOfWeek.Tuesday:
                    return this.Tue;

                case DayOfWeek.Wednesday:
                    return this.Wed;

                case DayOfWeek.Thursday:
                    return this.Thu;

                case DayOfWeek.Friday:
                    return this.Fri;

                case DayOfWeek.Saturday:
                    return this.Sat;
            }
            return false;
        }

        public double applyCoinMultiplier(double amount)
        {
            return (amount * this.CoinMultiplier);
        }

        public int applyDropChanceMultiplier(int amount)
        {
            return Mathf.RoundToInt(amount * this.DropChanceMultiplier);
        }

        public double applyXpMultiplier(double amount)
        {
            return (amount * this.XpMultiplier);
        }

        public TimeSpan getTimeRemaining()
        {
            DateTime now = DateTime.Now;
            return (TimeSpan) (DateTime.Today.AddDays(1.0) - now);
        }
    }
}

