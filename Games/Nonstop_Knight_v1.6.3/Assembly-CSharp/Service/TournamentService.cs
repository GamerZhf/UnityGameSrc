namespace Service
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class TournamentService
    {
        private Dictionary<string, TournamentViewRemote> m_availableTournaments = new Dictionary<string, TournamentViewRemote>();
        private bool m_tournamentsLoaded;
        public static readonly string TOURNAMENT_CACHE_PATH = "cached_tournament_remoteviews.json";

        [DebuggerHidden]
        private IEnumerator BroadcastToBucket(BroadcastKey broadcastKey, string tournamentId, TournamentLogEvent logEvent, Dictionary<string, object> customParams)
        {
            <BroadcastToBucket>c__Iterator236 iterator = new <BroadcastToBucket>c__Iterator236();
            iterator.broadcastKey = broadcastKey;
            iterator.logEvent = logEvent;
            iterator.customParams = customParams;
            iterator.tournamentId = tournamentId;
            iterator.<$>broadcastKey = broadcastKey;
            iterator.<$>logEvent = logEvent;
            iterator.<$>customParams = customParams;
            iterator.<$>tournamentId = tournamentId;
            return iterator;
        }

        public List<TournamentEntry> GetTournamentEntries(string tournamentId)
        {
            TournamentViewRemote remote;
            this.m_availableTournaments.TryGetValue(tournamentId, out remote);
            return ((remote == null) ? new List<TournamentEntry>() : remote.Entries);
        }

        public void LoadFromCache()
        {
            try
            {
                string json = IOUtil.LoadFromPersistentStorage(TOURNAMENT_CACHE_PATH);
                this.m_availableTournaments = JsonUtils.Deserialize<Dictionary<string, TournamentViewRemote>>(json, true);
                UnityEngine.Debug.Log("Loaded cached tournaments");
            }
            catch (Exception)
            {
                UnityEngine.Debug.Log("No cached tournaments");
            }
        }

        public void StartBroadcastToBucket(BroadcastKey key, string tournamentId, [Optional, DefaultParameterValue(null)] TournamentLogEvent logEvent, [Optional, DefaultParameterValue(null)] Dictionary<string, object> customParams)
        {
            Service.Binder.TaskManager.StartTask(this.BroadcastToBucket(key, tournamentId, logEvent, customParams), null);
        }

        public void StartUpdateRemoteViews(Dictionary<string, TournamentContribution> contributions)
        {
            Service.Binder.TaskManager.StartTask(this.UpdateRemoteTournamentViews(contributions), null);
        }

        [DebuggerHidden]
        private IEnumerator UpdateRemoteTournamentViews(Dictionary<string, TournamentContribution> contributions)
        {
            <UpdateRemoteTournamentViews>c__Iterator237 iterator = new <UpdateRemoteTournamentViews>c__Iterator237();
            iterator.contributions = contributions;
            iterator.<$>contributions = contributions;
            iterator.<>f__this = this;
            return iterator;
        }

        public void WriteCache()
        {
            IOUtil.SaveToPersistentStorage(JsonUtils.Serialize(this.m_availableTournaments), TOURNAMENT_CACHE_PATH, ConfigApp.PersistentStorageEncryptionEnabled, true);
        }

        public Dictionary<string, TournamentViewRemote> AvailableTournaments
        {
            get
            {
                return this.m_availableTournaments;
            }
        }

        public bool RemoteViewsLoaded
        {
            get
            {
                return this.m_tournamentsLoaded;
            }
        }

        [CompilerGenerated]
        private sealed class <BroadcastToBucket>c__Iterator236 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal BroadcastKey <$>broadcastKey;
            internal Dictionary<string, object> <$>customParams;
            internal TournamentLogEvent <$>logEvent;
            internal string <$>tournamentId;
            internal TournamentBroadcastRequest <request>__0;
            internal Request<string> <resp>__1;
            internal BroadcastKey broadcastKey;
            internal Dictionary<string, object> customParams;
            internal TournamentLogEvent logEvent;
            internal string tournamentId;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                    case 1:
                        if (!Service.Binder.PlayerService.IsLoggedIn || !Service.Binder.ServiceWatchdog.IsOnline)
                        {
                            this.$current = null;
                            this.$PC = 1;
                        }
                        else
                        {
                            TournamentBroadcastRequest request = new TournamentBroadcastRequest();
                            request.Key = this.broadcastKey;
                            request.LogEvent = this.logEvent;
                            request.CustomParams = this.customParams;
                            this.<request>__0 = request;
                            this.<resp>__1 = Request<string>.Post("/player/{sessionId}/tournament/" + WWW.EscapeURL(this.tournamentId) + "/broadcast", this.<request>__0);
                            this.$current = this.<resp>__1.Task;
                            this.$PC = 2;
                        }
                        return true;

                    case 2:
                        if (this.<resp>__1.Success)
                        {
                            UnityEngine.Debug.Log(string.Format("Succesfully broadcasted to tournament: {0} with key: {1}", this.tournamentId, this.broadcastKey));
                        }
                        else
                        {
                            UnityEngine.Debug.Log(string.Format("Failed broadcasting to bucket, msg: {0}", this.<resp>__1.ErrorMsg));
                        }
                        this.$PC = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateRemoteTournamentViews>c__Iterator237 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Dictionary<string, TournamentContribution> <$>contributions;
            internal List<TournamentViewRemote>.Enumerator <$s_530>__2;
            internal Dictionary<string, TournamentContribution>.Enumerator <$s_531>__5;
            internal TournamentService <>f__this;
            internal KeyValuePair<string, TournamentContribution> <contribution>__6;
            internal Dictionary<string, List<TournamentLogEvent>> <loggedEvents>__4;
            internal TournamentContributionRequest <request>__0;
            internal Request<TournamentResponse> <resp>__1;
            internal TournamentViewRemote <view>__3;
            internal Dictionary<string, TournamentContribution> contributions;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                    {
                        TournamentContributionRequest request = new TournamentContributionRequest();
                        request.ContributionsPerTournament = this.contributions;
                        this.<request>__0 = request;
                        this.<resp>__1 = Request<TournamentResponse>.Post("/player/{sessionId}/tournament", this.<request>__0);
                        this.$current = this.<resp>__1.Task;
                        this.$PC = 1;
                        return true;
                    }
                    case 1:
                        if (!this.<resp>__1.Success)
                        {
                            UnityEngine.Debug.Log(string.Format("Failed updating remote tournament views, msg: {0}", this.<resp>__1.ErrorMsg));
                            break;
                        }
                        this.<>f__this.m_availableTournaments.Clear();
                        this.<$s_530>__2 = this.<resp>__1.Result.RemoteTournamentViews.GetEnumerator();
                        try
                        {
                            while (this.<$s_530>__2.MoveNext())
                            {
                                this.<view>__3 = this.<$s_530>__2.Current;
                                this.<>f__this.m_availableTournaments[this.<view>__3.TournamentInfo.Id] = this.<view>__3;
                            }
                        }
                        finally
                        {
                            this.<$s_530>__2.Dispose();
                        }
                        this.<loggedEvents>__4 = new Dictionary<string, List<TournamentLogEvent>>();
                        this.<$s_531>__5 = this.contributions.GetEnumerator();
                        try
                        {
                            while (this.<$s_531>__5.MoveNext())
                            {
                                this.<contribution>__6 = this.<$s_531>__5.Current;
                                this.<loggedEvents>__4.Add(this.<contribution>__6.Key, this.<contribution>__6.Value.LogEvents);
                            }
                        }
                        finally
                        {
                            this.<$s_531>__5.Dispose();
                        }
                        Service.Binder.EventBus.TournamentRemoteViewsUpdated(this.<loggedEvents>__4);
                        this.<>f__this.WriteCache();
                        break;

                    default:
                        goto Label_01C9;
                }
                this.$PC = -1;
            Label_01C9:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

