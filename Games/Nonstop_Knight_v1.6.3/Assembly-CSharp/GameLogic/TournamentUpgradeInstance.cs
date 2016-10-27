namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using System;

    public class TournamentUpgradeInstance
    {
        private PerkInstance m_sharedPerkInstance;
        public int TotalCount;
        public float TotalModifier;
        public string TournamentUpgradeId;

        public TournamentUpgradeInstance()
        {
        }

        public TournamentUpgradeInstance(TournamentUpgradeInstance another)
        {
            this.TournamentUpgradeId = another.TournamentUpgradeId;
            this.TotalCount = another.TotalCount;
            this.TotalModifier = another.TotalModifier;
        }

        public TournamentUpgradeInstance(string id, int totalCount, float totalModifier)
        {
            this.TournamentUpgradeId = id;
            this.TotalCount = totalCount;
            this.TotalModifier = totalModifier;
        }

        public void increaseTotalCount(int amount)
        {
            this.TotalCount += amount;
        }

        public void increaseTotalModifier(float amount)
        {
            this.TotalModifier += amount;
            PerkInstance sharedPerkInstance = this.SharedPerkInstance;
            if (sharedPerkInstance != null)
            {
                sharedPerkInstance.Modifier += amount;
            }
        }

        [JsonIgnore]
        public PerkInstance SharedPerkInstance
        {
            get
            {
                if (this.m_sharedPerkInstance == null)
                {
                    TournamentUpgrade tournamentUpgrade = App.Binder.ConfigMeta.GetTournamentUpgrade(this.TournamentUpgradeId);
                    if ((tournamentUpgrade != null) && (tournamentUpgrade.PerkType != PerkType.NONE))
                    {
                        this.m_sharedPerkInstance = new PerkInstance(tournamentUpgrade.PerkType, this.TotalModifier);
                    }
                }
                return this.m_sharedPerkInstance;
            }
        }
    }
}

