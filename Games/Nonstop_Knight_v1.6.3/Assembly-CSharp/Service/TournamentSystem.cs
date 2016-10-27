namespace Service
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class TournamentSystem : MonoBehaviour, ITournamentSystem
    {
        [CompilerGenerated]
        private bool <Initialized>k__BackingField;
        [CompilerGenerated]
        private bool <Synchronized>k__BackingField;
        private float m_nextUpsert = Time.realtimeSinceStartup;
        private Dictionary<string, TournamentView> m_tournamentViews = new Dictionary<string, TournamentView>();
        private Coroutine m_upsertRoutine;

        public int CountUnclaimedMilestones()
        {
            int num = 0;
            foreach (KeyValuePair<string, TournamentView> pair in this.m_tournamentViews)
            {
                num += pair.Value.CountUnclaimedMilestones();
            }
            return num;
        }

        public void ForceUpsert()
        {
            this.m_nextUpsert = 0f;
        }

        public TournamentView GetTournamentView(string tournamentId)
        {
            TournamentView view;
            this.m_tournamentViews.TryGetValue(tournamentId, out view);
            return view;
        }

        public RewardMilestone GetUnclaimedRewardMilestoneWithLowestCompletion(out string tournamentId)
        {
            RewardMilestone milestone = null;
            float maxValue = float.MaxValue;
            tournamentId = null;
            foreach (KeyValuePair<string, TournamentView> pair in this.m_tournamentViews)
            {
                RewardMilestone milestoneWithLowestCompletion = pair.Value.GetMilestoneWithLowestCompletion(ref maxValue, ref tournamentId);
                if (milestoneWithLowestCompletion != null)
                {
                    milestone = milestoneWithLowestCompletion;
                }
                else
                {
                    milestone = milestone;
                }
            }
            return milestone;
        }

        public void Initialize()
        {
            if (!this.Initialized)
            {
                Service.Binder.TournamentService.LoadFromCache();
                this.RefreshTournamentViews();
                if (this.m_upsertRoutine != null)
                {
                    UnityUtils.StopCoroutine(this, ref this.m_upsertRoutine);
                }
                this.m_upsertRoutine = UnityUtils.StartCoroutine(this, this.upsertRoutine());
                if (!ConfigApp.ProductionBuild && (Service.Binder.ServerSelection.SelectedServer == ConfigService.OFFLINE_SERVER_ENTRY))
                {
                    this.Synchronized = true;
                }
                this.Initialized = true;
            }
        }

        public void JoinTournament(string tournamentId)
        {
            if (!this.Initialized || !this.Synchronized)
            {
                UnityEngine.Debug.LogError("Trying to join a tournament when TournamentSystem not fully initialized and synchronized");
            }
            else
            {
                TournamentView view;
                this.m_tournamentViews.TryGetValue(tournamentId, out view);
                if (view == null)
                {
                    UnityEngine.Debug.LogError("Cannot join a non-existent tournament:" + tournamentId);
                }
                else
                {
                    TournamentInstance instance = CmdJoinTournament.ExecuteStatic(GameLogic.Binder.GameState.Player, view.TournamentInfo);
                    if (instance != null)
                    {
                        view.JoinTournament(instance);
                        this.LogTournamentEvent(tournamentId, TournamentLogEvent.LogEventType.PlayerJoined, true, null);
                        this.ForceUpsert();
                    }
                }
            }
        }

        public TournamentLogEvent LogTournamentEvent(string tournamentId, TournamentLogEvent.LogEventType type, [Optional, DefaultParameterValue(true)] bool sendToServer, [Optional, DefaultParameterValue(null)] string additionalData)
        {
            TournamentView view;
            if (!this.m_tournamentViews.TryGetValue(tournamentId, out view))
            {
                UnityEngine.Debug.LogError("Trying to log to a non existing tournamentview");
                return null;
            }
            TournamentLogEvent event3 = new TournamentLogEvent();
            event3.Type = type;
            event3.Id = Guid.NewGuid().ToString();
            event3.AdditionalData = additionalData;
            event3.PlayerId = GameLogic.Binder.GameState.Player._id;
            TournamentLogEvent item = event3;
            if (sendToServer)
            {
                view.Instance.OutgoingLogQueue.Add(item);
            }
            view.Log.RegisterPriorityEvent(item);
            return item;
        }

        public TournamentView NextAvailableViewOrNull()
        {
            Player player = GameLogic.Binder.GameState.Player;
            List<TournamentView> views = new List<TournamentView>(this.m_tournamentViews.Values);
            if (views.Count == 0)
            {
                return null;
            }
            return TournamentView.getNextTournamentView(views, player);
        }

        private void onBossKilled(CharacterInstance killed, CharacterInstance killer, bool critted, SkillType skill)
        {
            if ((killed.IsBoss && (killer != null)) && (killer.IsPlayerCharacter && (GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.numberOfBossesAlive() <= 0)))
            {
                string selectedTournamentId = GameLogic.Binder.GameState.Player.Tournaments.SelectedTournamentId;
                if (selectedTournamentId != null)
                {
                    this.LogTournamentEvent(selectedTournamentId, !killed.IsEliteBoss ? TournamentLogEvent.LogEventType.BossKilled : TournamentLogEvent.LogEventType.EliteBossKilled, true, killed.CharacterId);
                }
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            GameLogic.Binder.EventBus.OnCharacterKilled -= new GameLogic.Events.CharacterKilled(this.onBossKilled);
            GameLogic.Binder.EventBus.OnTournamentUpgradeGained -= new GameLogic.Events.TournamentUpgradeGained(this.onTournamentUpgradeGained);
            Service.Binder.EventBus.OnTournamentRemoteViewsUpdated -= new Service.Events.RemoteTournamentViewsUpdated(this.onTournamentRemoteViewsUpdated);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            GameLogic.Binder.EventBus.OnCharacterKilled += new GameLogic.Events.CharacterKilled(this.onBossKilled);
            GameLogic.Binder.EventBus.OnTournamentUpgradeGained += new GameLogic.Events.TournamentUpgradeGained(this.onTournamentUpgradeGained);
            Service.Binder.EventBus.OnTournamentRemoteViewsUpdated += new Service.Events.RemoteTournamentViewsUpdated(this.onTournamentRemoteViewsUpdated);
        }

        private void onGameStateInitialized()
        {
            this.Initialize();
        }

        private void onTournamentRemoteViewsUpdated(Dictionary<string, List<TournamentLogEvent>> logEventsToRemove)
        {
            foreach (KeyValuePair<string, List<TournamentLogEvent>> pair in logEventsToRemove)
            {
                TournamentView view;
                if (!this.m_tournamentViews.TryGetValue(pair.Key, out view))
                {
                    UnityEngine.Debug.LogError("Trying to remove log events from tournament not in views");
                }
                else
                {
                    view.Instance.ClearLogEvents(pair.Value);
                }
            }
            this.RefreshTournamentViews();
            this.Synchronized = true;
        }

        private void onTournamentUpgradeGained(Player player, string id, bool epicVersion, int numMilestonesCompleted)
        {
            if (this.Initialized)
            {
                this.LogTournamentEvent(player.Tournaments.SelectedTournamentId, !epicVersion ? TournamentLogEvent.LogEventType.CardSelected : TournamentLogEvent.LogEventType.EpicCardSelected, true, id);
            }
        }

        private void RefreshTournamentViews()
        {
            Player player = GameLogic.Binder.GameState.Player;
            List<string> list = new List<string>();
            foreach (TournamentViewRemote remote in Service.Binder.TournamentService.AvailableTournaments.Values)
            {
                list.Add(remote.TournamentInfo.Id);
                if (!this.m_tournamentViews.ContainsKey(remote.TournamentInfo.Id))
                {
                    TournamentView view = new TournamentView(remote.TournamentInfo, remote.Log, remote.status, player.Tournaments.getInstanceOrNull(remote.TournamentInfo.Id), remote.Entries, remote.BucketStartTime);
                    this.m_tournamentViews.Add(remote.TournamentInfo.Id, view);
                }
                else
                {
                    this.m_tournamentViews[remote.TournamentInfo.Id].UpdateEntries(remote.Entries);
                    this.m_tournamentViews[remote.TournamentInfo.Id].UpdateLog(remote.Log);
                    this.m_tournamentViews[remote.TournamentInfo.Id].BucketStartTime = remote.BucketStartTime;
                    this.m_tournamentViews[remote.TournamentInfo.Id].BucketId = remote.BucketId;
                    this.m_tournamentViews[remote.TournamentInfo.Id].ServerJoinStatus = remote.status;
                }
                TournamentView view2 = this.m_tournamentViews[remote.TournamentInfo.Id];
                switch (remote.status)
                {
                    case TournamentViewRemote.Status.OK:
                    {
                        if ((view2.Instance != null) && (view2.Instance.CurrentState == TournamentInstance.State.PENDING_JOIN_CONFIRMATION))
                        {
                            view2.Instance.CurrentState = TournamentInstance.State.ACTIVE;
                        }
                        continue;
                    }
                    case TournamentViewRemote.Status.TooEarlyForJoin:
                    {
                        if ((view2.Instance != null) && (view2.Instance.CurrentState == TournamentInstance.State.PENDING_JOIN_CONFIRMATION))
                        {
                            view2.Instance.CurrentState = TournamentInstance.State.ERROR_JOIN_TOO_EARLY;
                        }
                        continue;
                    }
                    case TournamentViewRemote.Status.TooLateForJoin:
                    {
                        if ((view2.Instance != null) && (view2.Instance.CurrentState == TournamentInstance.State.PENDING_JOIN_CONFIRMATION))
                        {
                            view2.Instance.CurrentState = TournamentInstance.State.ERROR_JOIN_TOO_LATE;
                        }
                        continue;
                    }
                    case TournamentViewRemote.Status.BucketTimeEnded:
                    {
                        if (view2.Instance != null)
                        {
                            if (view2.Instance.CurrentState != TournamentInstance.State.PENDING_JOIN_CONFIRMATION)
                            {
                                break;
                            }
                            view2.Instance.CurrentState = TournamentInstance.State.ERROR_JOIN_TOO_LATE;
                        }
                        continue;
                    }
                    default:
                    {
                        continue;
                    }
                }
                if (view2.Instance.CurrentState == TournamentInstance.State.ACTIVE)
                {
                    view2.Instance.CurrentState = TournamentInstance.State.PENDING_END_ANNOUNCEMENT;
                }
            }
            List<string> list2 = new List<string>();
            foreach (KeyValuePair<string, TournamentView> pair in this.m_tournamentViews)
            {
                if (!list.Contains(pair.Value.TournamentInfo.Id))
                {
                    if (pair.Value.PlayerHasJoined)
                    {
                        pair.Value.Instance.CurrentState = TournamentInstance.State.ERROR_EXPIRED;
                        pair.Value.ServerJoinStatus = TournamentViewRemote.Status.Expired;
                    }
                    else
                    {
                        list2.Add(pair.Key);
                    }
                }
            }
            foreach (KeyValuePair<string, TournamentView> pair2 in this.m_tournamentViews)
            {
                if ((pair2.Value.Instance != null) && (pair2.Value.Instance.CurrentState == TournamentInstance.State.CLEARED_FOR_REMOVAL))
                {
                    list2.Add(pair2.Key);
                }
            }
            for (int i = 0; i < list2.Count; i++)
            {
                this.m_tournamentViews.Remove(list2[i]);
                player.Tournaments.ActiveInstances.Remove(list2[i]);
            }
            Service.Binder.EventBus.LocalTournamentViewsRefreshed();
        }

        public void RegisterPriorityLogEvent(TournamentLogEvent logEvent, string tournamentId)
        {
            TournamentView view;
            if (!this.m_tournamentViews.TryGetValue(tournamentId, out view))
            {
                UnityEngine.Debug.LogError(string.Format("Trying to log to a non existing tournamentview with id: {0}", tournamentId));
            }
            view.Log.RegisterPriorityEvent(logEvent);
        }

        [DebuggerHidden]
        private IEnumerator upsertRoutine()
        {
            <upsertRoutine>c__IteratorDF rdf = new <upsertRoutine>c__IteratorDF();
            rdf.<>f__this = this;
            return rdf;
        }

        public bool Initialized
        {
            [CompilerGenerated]
            get
            {
                return this.<Initialized>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Initialized>k__BackingField = value;
            }
        }

        public bool Synchronized
        {
            [CompilerGenerated]
            get
            {
                return this.<Synchronized>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Synchronized>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <upsertRoutine>c__IteratorDF : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Dictionary<string, TournamentView>.Enumerator <$s_370>__1;
            internal TournamentSystem <>f__this;
            internal Dictionary<string, TournamentContribution> <collectedContributions>__0;
            internal KeyValuePair<string, TournamentView> <view>__2;

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
                        break;

                    case 1:
                        break;
                        this.$PC = -1;
                        goto Label_017F;

                    default:
                        goto Label_017F;
                }
                if ((Service.Binder.PlayerService.IsLoggedIn && Service.Binder.ServiceWatchdog.IsOnline) && (this.<>f__this.m_nextUpsert < Time.realtimeSinceStartup))
                {
                    this.<collectedContributions>__0 = new Dictionary<string, TournamentContribution>();
                    this.<$s_370>__1 = this.<>f__this.m_tournamentViews.GetEnumerator();
                    try
                    {
                        while (this.<$s_370>__1.MoveNext())
                        {
                            this.<view>__2 = this.<$s_370>__1.Current;
                            if (this.<view>__2.Value.Instance != null)
                            {
                                this.<view>__2.Value.RefreshPlayerEntry();
                                if (this.<view>__2.Value.PlayerEntry != null)
                                {
                                    this.<collectedContributions>__0[this.<view>__2.Key] = new TournamentContribution(this.<view>__2.Value.PlayerEntry, new List<TournamentLogEvent>(this.<view>__2.Value.Instance.OutgoingLogQueue));
                                }
                            }
                        }
                    }
                    finally
                    {
                        this.<$s_370>__1.Dispose();
                    }
                    Service.Binder.TournamentService.StartUpdateRemoteViews(this.<collectedContributions>__0);
                    this.<>f__this.m_nextUpsert = Time.realtimeSinceStartup + ConfigService.TOURNAMENT_UPSERT_REFRESH;
                }
                this.$current = new WaitForSeconds(0.5f);
                this.$PC = 1;
                return true;
            Label_017F:
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

