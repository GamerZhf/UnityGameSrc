namespace GameLogic
{
    using Service;
    using System;
    using System.Collections.Generic;

    public class DungeonEventRules : CsvResources<string, DungeonEvent>
    {
        private Dictionary<string, List<DungeonEvent>> m_events = new Dictionary<string, List<DungeonEvent>>();

        public DungeonEventRules()
        {
            string[,] strArray = CsvUtils.Deserialize(ResourceUtil.LoadSafe<TextAsset>("Rules/Dungeon-Events", false).text);
            for (int i = 0; i < strArray.GetLength(1); i++)
            {
                if (strArray[0, i] != null)
                {
                    DungeonEvent item = new DungeonEvent();
                    int num2 = 0;
                    item.DungeonId = strArray[num2++, i];
                    item.Mon = base.parseBool(strArray[num2++, i]);
                    item.Tue = base.parseBool(strArray[num2++, i]);
                    item.Wed = base.parseBool(strArray[num2++, i]);
                    item.Thu = base.parseBool(strArray[num2++, i]);
                    item.Fri = base.parseBool(strArray[num2++, i]);
                    item.Sat = base.parseBool(strArray[num2++, i]);
                    item.Sun = base.parseBool(strArray[num2++, i]);
                    item.CoinMultiplier = base.parseFloat(strArray[num2++, i]);
                    item.XpMultiplier = base.parseFloat(strArray[num2++, i]);
                    item.DropChanceMultiplier = base.parseFloat(strArray[num2++, i]);
                    if (!this.m_events.ContainsKey(item.DungeonId))
                    {
                        this.m_events.Add(item.DungeonId, new List<DungeonEvent>());
                    }
                    this.m_events[item.DungeonId].Add(item);
                }
            }
        }

        public DungeonEvent getActiveEventForDungeon(string dungeonId)
        {
            DungeonEvent event2 = null;
            if (this.m_events.ContainsKey(dungeonId))
            {
                for (int i = 0; i < this.m_events[dungeonId].Count; i++)
                {
                    DungeonEvent event3 = this.m_events[dungeonId][i];
                    DayOfWeek dayOfWeek = TimeUtil.UnixTimestampToDateTime(Service.Binder.ServerTime.GameTime).DayOfWeek;
                    if (event3.activeOnWeekday(dayOfWeek))
                    {
                        event2 = event3;
                    }
                }
            }
            return event2;
        }
    }
}

