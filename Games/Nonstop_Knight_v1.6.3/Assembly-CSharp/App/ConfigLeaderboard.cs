namespace App
{
    using GameLogic;
    using System;

    public static class ConfigLeaderboard
    {
        public static LeaderboardEntry[] DUMMY_PLAYERS;
        public static LeaderboardEntry[] DUMMY_PLAYERS_TESTING;
        public static int MAX_NUM_VISIBLE_LEADERBOARD_CELLS = 100;

        static ConfigLeaderboard()
        {
            LeaderboardEntry[] entryArray1 = new LeaderboardEntry[9];
            LeaderboardEntry entry = new LeaderboardEntry();
            entry.UserId = "dummy01";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_01;
            entry.HighestFloor = 590;
            entry.AvatarSpriteId = "sprite_knight_paladin_256";
            entryArray1[0] = entry;
            entry = new LeaderboardEntry();
            entry.UserId = "dummy02";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_02;
            entry.HighestFloor = 0x1c0;
            entry.AvatarSpriteId = "sprite_knight_fiery_256";
            entryArray1[1] = entry;
            entry = new LeaderboardEntry();
            entry.UserId = "dummy03";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_03;
            entry.HighestFloor = 0x121;
            entry.AvatarSpriteId = "sprite_knight_spartan_256";
            entryArray1[2] = entry;
            entry = new LeaderboardEntry();
            entry.UserId = "dummy04";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_04;
            entry.HighestFloor = 0x7c;
            entry.AvatarSpriteId = "sprite_knight_crown_256";
            entryArray1[3] = entry;
            entry = new LeaderboardEntry();
            entry.UserId = "dummy05";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_05;
            entry.HighestFloor = 90;
            entry.AvatarSpriteId = "sprite_knight_spiky_256";
            entryArray1[4] = entry;
            entry = new LeaderboardEntry();
            entry.UserId = "dummy06";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_06;
            entry.HighestFloor = 0x37;
            entry.AvatarSpriteId = "sprite_knight_epic_256";
            entryArray1[5] = entry;
            entry = new LeaderboardEntry();
            entry.UserId = "dummy07";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_07;
            entry.HighestFloor = 0x20;
            entry.AvatarSpriteId = "sprite_knight_bullhorns_256";
            entryArray1[6] = entry;
            entry = new LeaderboardEntry();
            entry.UserId = "dummy08";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_08;
            entry.HighestFloor = 0x12;
            entry.AvatarSpriteId = "sprite_knight_spitfire_256";
            entryArray1[7] = entry;
            entry = new LeaderboardEntry();
            entry.UserId = "dummy09";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_09;
            entry.HighestFloor = 10;
            entry.AvatarSpriteId = "sprite_knight_buckethead_256";
            entryArray1[8] = entry;
            DUMMY_PLAYERS = entryArray1;
            LeaderboardEntry[] entryArray2 = new LeaderboardEntry[9];
            entry = new LeaderboardEntry();
            entry.UserId = "dummy01";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_01;
            entry.HighestFloor = 11;
            entry.AvatarSpriteId = "sprite_knight_paladin_256";
            entryArray2[0] = entry;
            entry = new LeaderboardEntry();
            entry.UserId = "dummy02";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_02;
            entry.HighestFloor = 10;
            entry.AvatarSpriteId = "sprite_knight_fiery_256";
            entryArray2[1] = entry;
            entry = new LeaderboardEntry();
            entry.UserId = "dummy03";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_03;
            entry.HighestFloor = 9;
            entry.AvatarSpriteId = "sprite_knight_spartan_256";
            entryArray2[2] = entry;
            entry = new LeaderboardEntry();
            entry.UserId = "dummy04";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_04;
            entry.HighestFloor = 8;
            entry.AvatarSpriteId = "sprite_knight_crown_256";
            entryArray2[3] = entry;
            entry = new LeaderboardEntry();
            entry.UserId = "dummy05";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_05;
            entry.HighestFloor = 7;
            entry.AvatarSpriteId = "sprite_knight_spiky_256";
            entryArray2[4] = entry;
            entry = new LeaderboardEntry();
            entry.UserId = "dummy06";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_06;
            entry.HighestFloor = 6;
            entry.AvatarSpriteId = "sprite_knight_epic_256";
            entryArray2[5] = entry;
            entry = new LeaderboardEntry();
            entry.UserId = "dummy07";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_07;
            entry.HighestFloor = 5;
            entry.AvatarSpriteId = "sprite_knight_bullhorns_256";
            entryArray2[6] = entry;
            entry = new LeaderboardEntry();
            entry.UserId = "dummy08";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_08;
            entry.HighestFloor = 4;
            entry.AvatarSpriteId = "sprite_knight_spitfire_256";
            entryArray2[7] = entry;
            entry = new LeaderboardEntry();
            entry.UserId = "dummy09";
            entry.Dummy = true;
            entry.Name = ConfigLoca.LEADERBOARD_DUMMY_KNIGHT_09;
            entry.HighestFloor = 3;
            entry.AvatarSpriteId = "sprite_knight_buckethead_256";
            entryArray2[8] = entry;
            DUMMY_PLAYERS_TESTING = entryArray2;
        }

        public static bool IsValidLeaderboardName(string name)
        {
            if (name == null)
            {
                return false;
            }
            string self = name.Trim();
            if (self == string.Empty)
            {
                return false;
            }
            return (StringExtensions.ToLowerLoca(_.L(ConfigLoca.HERO_KNIGHT, null, false)) != StringExtensions.ToLowerLoca(self));
        }
    }
}

