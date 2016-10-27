namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using PlayerView;
    using Service;
    using System;
    using System.Runtime.CompilerServices;

    public class PromotionEventInstance : IJsonData
    {
        [CompilerGenerated]
        private GameLogic.PromotionEvents <PromotionEvents>k__BackingField;
        public long EndTimestamp;
        public string Id;
        public bool Inspected;
        public GameLogic.Missions Missions = new GameLogic.Missions(MissionType.PromotionEvent);
        public long StartTimestamp;

        public ConfigPromotionEvents.Event getData()
        {
            return Service.Binder.PromotionEventSystem.getPromotionEventData(this.Id);
        }

        public string getTimerString()
        {
            return MenuHelpers.SecondsToStringDaysHoursMinutesSeconds(MathUtil.Clamp((long) (this.EndTimestamp - Service.Binder.ServerTime.GameTime), (long) 0L, (long) 0x7fffffffffffffffL), true);
        }

        public void postDeserializeInitialization()
        {
            this.Missions.Player = this.PromotionEvents.Player;
            this.Missions.postDeserializeInitialization();
        }

        [JsonIgnore]
        public GameLogic.PromotionEvents PromotionEvents
        {
            [CompilerGenerated]
            get
            {
                return this.<PromotionEvents>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<PromotionEvents>k__BackingField = value;
            }
        }
    }
}

