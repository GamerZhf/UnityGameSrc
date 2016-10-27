namespace App
{
    using GameLogic;
    using System;
    using System.Collections.Generic;

    public static class ConfigLeagues
    {
        public static LeagueData[] Leagues;

        static ConfigLeagues()
        {
            LeagueData[] dataArray1 = new LeagueData[7];
            LeagueData data = new LeagueData();
            data.Id = "League001";
            data.RewardChestType = ChestType.Basic001;
            dataArray1[0] = data;
            data = new LeagueData();
            data.Id = "League002";
            data.CrownRequirement = 100.0;
            data.RewardChestType = ChestType.Special001;
            dataArray1[1] = data;
            data = new LeagueData();
            data.Id = "League003";
            data.CrownRequirement = 3000.0;
            data.RewardChestType = ChestType.Special002;
            dataArray1[2] = data;
            data = new LeagueData();
            data.Id = "League004";
            data.CrownRequirement = 120000.0;
            data.RewardChestType = ChestType.Special003;
            dataArray1[3] = data;
            data = new LeagueData();
            data.Id = "League005";
            data.CrownRequirement = 30000000.0;
            data.RewardChestType = ChestType.Special004;
            dataArray1[4] = data;
            data = new LeagueData();
            data.Id = "League006";
            data.CrownRequirement = 7500000000;
            data.RewardChestType = ChestType.Special005;
            dataArray1[5] = data;
            data = new LeagueData();
            data.Id = "League007";
            data.CrownRequirement = 1500000000000;
            data.RewardChestType = ChestType.Special006;
            dataArray1[6] = data;
            Leagues = dataArray1;
        }

        public static List<ChestType> GetUnlockedChestTypes(double crowns)
        {
            List<ChestType> list = new List<ChestType>(Leagues.Length);
            for (int i = 0; i < Leagues.Length; i++)
            {
                if (crowns >= Leagues[i].CrownRequirement)
                {
                    list.Add(Leagues[i].RewardChestType);
                }
            }
            return list;
        }

        public static bool HasPlayerReachedLeague(double crowns, LeagueData league)
        {
            return (crowns >= league.CrownRequirement);
        }
    }
}

