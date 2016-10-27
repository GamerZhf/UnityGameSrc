namespace GameLogic
{
    using Pathfinding.Serialization.JsonFx;
    using System;

    public class PlayerTrackingData
    {
        public int ActiveFloorCompletions;
        public int ArmorLevelups;
        public int BossFightsLost;
        public int CloakLevelups;
        public double CoinsEarnedActive;
        public double CoinsEarnedPassive;
        public double DiamondsEarned;
        public long FpsTotalFrames;
        public float FpsTotalSeconds;
        public int FreeRevivesUsed;
        public int ItemsGained;
        public int KilledByMinions;
        public int LastFailedBossFightFloor;
        public int LostToSameBossInARowCount;
        public int NumMainMenuOpensArrowButton;
        public int NumMainMenuOpensSwipe;
        public int NumMainMenuOpensTopButton;
        public int PassiveFloorCompletions;
        [JsonIgnore]
        public int PerSessionArmorLevelups;
        [JsonIgnore]
        public int PerSessionCloakLevelups;
        [JsonIgnore]
        public int PerSessionLeaderboardOpenings;
        [JsonIgnore]
        public int PerSessionShopOpenings;
        [JsonIgnore]
        public bool PerSessionWasOutperformed;
        [JsonIgnore]
        public int PerSessionWeaponLevelups;
        public double TokensEarned;
        public int WeaponLevelups;

        public void clearPlayerProgressFields()
        {
            this.TokensEarned = 0.0;
            this.DiamondsEarned = 0.0;
            this.ItemsGained = 0;
            this.WeaponLevelups = 0;
            this.ArmorLevelups = 0;
            this.CloakLevelups = 0;
            this.KilledByMinions = 0;
            this.BossFightsLost = 0;
            this.ActiveFloorCompletions = 0;
            this.PassiveFloorCompletions = 0;
            this.CoinsEarnedActive = 0.0;
            this.CoinsEarnedPassive = 0.0;
            this.FreeRevivesUsed = 0;
            this.FpsTotalFrames = 0L;
            this.FpsTotalSeconds = 0f;
            this.NumMainMenuOpensTopButton = 0;
            this.NumMainMenuOpensArrowButton = 0;
            this.NumMainMenuOpensSwipe = 0;
        }
    }
}

