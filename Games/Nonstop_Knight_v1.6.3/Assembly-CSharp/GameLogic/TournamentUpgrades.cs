namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class TournamentUpgrades : QuickLookableCharacterStatModifier, IJsonData
    {
        [CompilerGenerated]
        private GameLogic.Player <Player>k__BackingField;
        public Dictionary<string, TournamentUpgradeInstance> EpicUpgrades = new Dictionary<string, TournamentUpgradeInstance>();
        [JsonIgnore]
        public Dictionary<string, TournamentUpgradeInstance> ExternalEpicUpgrades = new Dictionary<string, TournamentUpgradeInstance>();
        [JsonIgnore]
        public Dictionary<string, TournamentUpgradeInstance> ExternalNormalUpgrades = new Dictionary<string, TournamentUpgradeInstance>();
        private Dictionary<PerkInstance, TournamentUpgrade> m_perkInstanceToUpgradeMap = new Dictionary<PerkInstance, TournamentUpgrade>();
        private List<string> m_uniqueUpgrades = new List<string>();
        public Dictionary<string, TournamentUpgradeInstance> NormalUpgrades = new Dictionary<string, TournamentUpgradeInstance>();
        private static List<string> sm_tempStringList = new List<string>();

        public void addEpicUpgradeToQuickLookup(TournamentUpgradeInstance tui, [Optional, DefaultParameterValue(true)] bool triggerQuickLookupRefresh)
        {
            if (!this.m_perkInstanceToUpgradeMap.ContainsKey(tui.SharedPerkInstance))
            {
                TournamentUpgrade upgrade = App.Binder.ConfigMeta.TOURNAMENT_UPGRADES[tui.TournamentUpgradeId];
                this.m_perkInstanceToUpgradeMap.Add(tui.SharedPerkInstance, upgrade);
            }
            if (!this.m_uniqueUpgrades.Contains(tui.TournamentUpgradeId))
            {
                this.m_uniqueUpgrades.Add(tui.TournamentUpgradeId);
            }
            base.addQuickLookupPerkInstance(tui.SharedPerkInstance, triggerQuickLookupRefresh);
        }

        private void addExternalUpgradesToQuickLookup([Optional, DefaultParameterValue(true)] bool triggerQuickLookupRefresh)
        {
            foreach (KeyValuePair<string, TournamentUpgradeInstance> pair in this.ExternalNormalUpgrades)
            {
                this.addNormalUpgradeToQuickLookup(pair.Value, false);
            }
            foreach (KeyValuePair<string, TournamentUpgradeInstance> pair2 in this.ExternalEpicUpgrades)
            {
                this.addEpicUpgradeToQuickLookup(pair2.Value, false);
            }
            if (triggerQuickLookupRefresh)
            {
                base.refreshQuickLookup();
            }
        }

        public void addNormalUpgradeToQuickLookup(TournamentUpgradeInstance tui, [Optional, DefaultParameterValue(true)] bool triggerQuickLookupRefresh)
        {
            if (!this.m_perkInstanceToUpgradeMap.ContainsKey(tui.SharedPerkInstance))
            {
                TournamentUpgrade upgrade = App.Binder.ConfigMeta.TOURNAMENT_UPGRADES[tui.TournamentUpgradeId];
                this.m_perkInstanceToUpgradeMap.Add(tui.SharedPerkInstance, upgrade);
            }
            if (!this.m_uniqueUpgrades.Contains(tui.TournamentUpgradeId))
            {
                this.m_uniqueUpgrades.Add(tui.TournamentUpgradeId);
            }
            base.addQuickLookupPerkInstance(tui.SharedPerkInstance, triggerQuickLookupRefresh);
        }

        private void addPlayerUpgradesToQuickLookup([Optional, DefaultParameterValue(true)] bool triggerQuickLookupRefresh)
        {
            foreach (KeyValuePair<string, TournamentUpgradeInstance> pair in this.NormalUpgrades)
            {
                this.addNormalUpgradeToQuickLookup(pair.Value, false);
            }
            foreach (KeyValuePair<string, TournamentUpgradeInstance> pair2 in this.EpicUpgrades)
            {
                this.addEpicUpgradeToQuickLookup(pair2.Value, false);
            }
            if (triggerQuickLookupRefresh)
            {
                base.refreshQuickLookup();
            }
        }

        private void clear()
        {
            base.QuickLookupPerkInstances.Clear();
            this.ExternalNormalUpgrades.Clear();
            this.ExternalEpicUpgrades.Clear();
            this.m_perkInstanceToUpgradeMap.Clear();
            this.m_uniqueUpgrades.Clear();
        }

        protected override IBuffIconProvider getBuffIconProvideForPerkInstance(PerkInstance perkInstance)
        {
            return this.m_perkInstanceToUpgradeMap[perkInstance];
        }

        public int getNumEpicUpgrades(string id)
        {
            int num = 0;
            if (this.EpicUpgrades.ContainsKey(id))
            {
                num += this.EpicUpgrades[id].TotalCount;
            }
            if (this.ExternalEpicUpgrades.ContainsKey(id))
            {
                num += this.ExternalEpicUpgrades[id].TotalCount;
            }
            return num;
        }

        public int getNumNormalUpgrades(string id)
        {
            int num = 0;
            if (this.NormalUpgrades.ContainsKey(id))
            {
                num += this.NormalUpgrades[id].TotalCount;
            }
            if (this.ExternalNormalUpgrades.ContainsKey(id))
            {
                num += this.ExternalNormalUpgrades[id].TotalCount;
            }
            return num;
        }

        public float getTotalEpicModifierForUpgrade(string id)
        {
            float num = 0f;
            if (this.EpicUpgrades.ContainsKey(id))
            {
                num += this.EpicUpgrades[id].TotalModifier;
            }
            if (this.ExternalEpicUpgrades.ContainsKey(id))
            {
                num += this.ExternalEpicUpgrades[id].TotalModifier;
            }
            return num;
        }

        public float getTotalNormalModifierForUpgrade(string id)
        {
            float num = 0f;
            if (this.NormalUpgrades.ContainsKey(id))
            {
                num += this.NormalUpgrades[id].TotalModifier;
            }
            if (this.ExternalNormalUpgrades.ContainsKey(id))
            {
                num += this.ExternalNormalUpgrades[id].TotalModifier;
            }
            return num;
        }

        public List<string> getUniqueUpgradesOwned()
        {
            return this.m_uniqueUpgrades;
        }

        public void initializeWithExternalUpgrades(List<TournamentEntry> tournamentEntries)
        {
            this.clear();
            this.addPlayerUpgradesToQuickLookup(false);
            for (int i = 0; i < tournamentEntries.Count; i++)
            {
                TournamentEntry entry = tournamentEntries[i];
                if (entry.PlayerId != this.Player._id)
                {
                    foreach (KeyValuePair<string, TournamentUpgradeInstance> pair in entry.NormalUpgrades)
                    {
                        if (!this.ExternalNormalUpgrades.ContainsKey(pair.Key))
                        {
                            this.ExternalNormalUpgrades.Add(pair.Key, new TournamentUpgradeInstance(pair.Value));
                        }
                        else
                        {
                            TournamentUpgradeInstance local1 = this.ExternalNormalUpgrades[pair.Key];
                            local1.TotalCount += pair.Value.TotalCount;
                            TournamentUpgradeInstance local2 = this.ExternalNormalUpgrades[pair.Key];
                            local2.TotalModifier += pair.Value.TotalModifier;
                        }
                    }
                    foreach (KeyValuePair<string, TournamentUpgradeInstance> pair2 in entry.EpicUpgrades)
                    {
                        if (!this.ExternalEpicUpgrades.ContainsKey(pair2.Key))
                        {
                            this.ExternalEpicUpgrades.Add(pair2.Key, new TournamentUpgradeInstance(pair2.Value));
                        }
                        else
                        {
                            TournamentUpgradeInstance local3 = this.ExternalEpicUpgrades[pair2.Key];
                            local3.TotalCount += pair2.Value.TotalCount;
                            TournamentUpgradeInstance local4 = this.ExternalEpicUpgrades[pair2.Key];
                            local4.TotalModifier += pair2.Value.TotalModifier;
                        }
                    }
                }
            }
            this.addExternalUpgradesToQuickLookup(false);
            base.refreshQuickLookup();
        }

        public void postDeserializeInitialization()
        {
            sm_tempStringList.Clear();
            foreach (KeyValuePair<string, TournamentUpgradeInstance> pair in this.NormalUpgrades)
            {
                string key = pair.Key;
                TournamentUpgradeInstance instance = pair.Value;
                if ((string.IsNullOrEmpty(key) || (instance.TotalCount <= 0)) || !App.Binder.ConfigMeta.TOURNAMENT_UPGRADES.ContainsKey(key))
                {
                    sm_tempStringList.Add(key);
                }
            }
            for (int i = 0; i < sm_tempStringList.Count; i++)
            {
                this.NormalUpgrades.Remove(sm_tempStringList[i]);
            }
            sm_tempStringList.Clear();
            foreach (KeyValuePair<string, TournamentUpgradeInstance> pair2 in this.EpicUpgrades)
            {
                string str2 = pair2.Key;
                TournamentUpgradeInstance instance2 = pair2.Value;
                if ((string.IsNullOrEmpty(str2) || (instance2.TotalCount <= 0)) || !App.Binder.ConfigMeta.TOURNAMENT_UPGRADES.ContainsKey(str2))
                {
                    sm_tempStringList.Add(str2);
                }
            }
            for (int j = 0; j < sm_tempStringList.Count; j++)
            {
                this.EpicUpgrades.Remove(sm_tempStringList[j]);
            }
            sm_tempStringList.Clear();
            this.clear();
            this.addPlayerUpgradesToQuickLookup(true);
        }

        [JsonIgnore]
        public GameLogic.Player Player
        {
            [CompilerGenerated]
            get
            {
                return this.<Player>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Player>k__BackingField = value;
            }
        }
    }
}

