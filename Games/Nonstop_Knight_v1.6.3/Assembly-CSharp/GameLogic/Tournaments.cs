namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Tournaments : IJsonData
    {
        [CompilerGenerated]
        private GameLogic.Player <Player>k__BackingField;
        [CompilerGenerated]
        private TournamentInstance <SelectedTournament>k__BackingField;
        public Dictionary<string, TournamentInstance> ActiveInstances = new Dictionary<string, TournamentInstance>();
        public List<string> CompletedTournamentHistory = new List<string>();
        public string LastUnselectedTournamentId;
        public long LatestCompletedTournamentStartTime = -1L;
        public List<string> NotifiedTournamentHistory = new List<string>();
        public string SelectedTournamentId;

        public TournamentInstance getInstanceOrNull(string id)
        {
            TournamentInstance instance;
            this.ActiveInstances.TryGetValue(id, out instance);
            return instance;
        }

        public bool hasTournamentBeenNotified(string id)
        {
            return this.NotifiedTournamentHistory.Contains(id);
        }

        public bool hasTournamentSelected()
        {
            return (this.SelectedTournament != null);
        }

        public void markTournamentAsNotified(string id)
        {
            if (!this.NotifiedTournamentHistory.Contains(id))
            {
                this.NotifiedTournamentHistory.Add(id);
            }
        }

        public bool playerHasCompletedTournament(string id)
        {
            return this.CompletedTournamentHistory.Contains(id);
        }

        public void postDeserializeInitialization()
        {
            if (!string.IsNullOrEmpty(this.SelectedTournamentId))
            {
                this.SelectedTournament = this.getInstanceOrNull(this.SelectedTournamentId);
            }
            else
            {
                this.SelectedTournament = null;
            }
            foreach (KeyValuePair<string, TournamentInstance> pair in this.ActiveInstances)
            {
                TournamentInstance instance = pair.Value;
                instance.Player = this.Player;
                instance.postDeserializeInitialization();
            }
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, TournamentInstance> pair2 in this.ActiveInstances)
            {
                string key = pair2.Key;
                if (pair2.Value.CurrentState == TournamentInstance.State.CLEARED_FOR_REMOVAL)
                {
                    list.Add(key);
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == this.SelectedTournamentId)
                {
                    this.SelectedTournamentId = null;
                }
                this.ActiveInstances.Remove(list[i]);
            }
            if (!string.IsNullOrEmpty(this.LastUnselectedTournamentId) && !this.ActiveInstances.ContainsKey(this.LastUnselectedTournamentId))
            {
                this.LastUnselectedTournamentId = null;
            }
        }

        public bool tournamentIsActive(string tournamentId)
        {
            return (this.getInstanceOrNull(tournamentId) != null);
        }

        public bool tournamentsUnlocked()
        {
            return (this.Player.CumulativeRetiredHeroStats.HeroesRetired >= 3);
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

        [JsonIgnore]
        public TournamentInstance SelectedTournament
        {
            [CompilerGenerated]
            get
            {
                return this.<SelectedTournament>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<SelectedTournament>k__BackingField = value;
            }
        }
    }
}

