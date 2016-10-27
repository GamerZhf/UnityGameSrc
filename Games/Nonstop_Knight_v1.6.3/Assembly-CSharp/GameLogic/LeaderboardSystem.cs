namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class LeaderboardSystem : MonoBehaviour, ILeaderboardSystem
    {
        [CompilerGenerated]
        private bool <Initialized>k__BackingField;
        private bool m_initializationRoutineRunning;
        private List<ILeaderboardView> m_leaderboardViews = new List<ILeaderboardView>();

        private ILeaderboardView createLeaderboardView(LeaderboardType type)
        {
            if (type == LeaderboardType.Royal)
            {
                return new LeaderboardViewFacebook();
            }
            return new LeaderboardView(type);
        }

        public int getLeaderboardIndexForPlayer(LeaderboardType leaderboardType, Player player)
        {
            return this.getLeaderboardView(leaderboardType).getLeaderboardIndexForPlayer(player);
        }

        public int getLeaderboardRankForPlayer(LeaderboardType leaderboardType, Player player)
        {
            return this.getLeaderboardView(leaderboardType).getLeaderboardRankForPlayer(player);
        }

        private ILeaderboardView getLeaderboardView(LeaderboardType type)
        {
            for (int i = 0; i < this.m_leaderboardViews.Count; i++)
            {
                if (this.m_leaderboardViews[i].Type == type)
                {
                    return this.m_leaderboardViews[i];
                }
            }
            return null;
        }

        public LeaderboardEntry getNextLeaderboardTargetForPlayer(LeaderboardType leaderboardType, Player player)
        {
            return this.getLeaderboardView(leaderboardType).getNextLeaderboardTargetForPlayer(player);
        }

        public List<LeaderboardEntry> getSortedLeaderboardEntries(LeaderboardType leaderboardType)
        {
            return this.getLeaderboardView(leaderboardType).SortedLeaderboardEntries;
        }

        [DebuggerHidden]
        private IEnumerator initializationRoutine()
        {
            <initializationRoutine>c__Iterator7E iteratore = new <initializationRoutine>c__Iterator7E();
            iteratore.<>f__this = this;
            return iteratore;
        }

        public void initialize()
        {
            if (!this.m_initializationRoutineRunning)
            {
                UnityUtils.StartCoroutine(this, this.initializationRoutine());
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            GameLogic.Binder.EventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnPlayerActiveCharacterSwitched -= new GameLogic.Events.PlayerActiveCharacterSwitched(this.onPlayerActiveCharacterSwitched);
            App.Binder.EventBus.OnLeaderboardLoaded -= new App.Events.LeaderboardLoaded(this.onLeaderboardLoaded);
            App.Binder.EventBus.OnPlayerScoreUpdated -= new App.Events.PlayerScoreUpdated(this.onPlayerScoreUpdated);
            GameLogic.Binder.EventBus.OnPlayerRenamed -= new GameLogic.Events.PlayerRenamed(this.onPlayerRenamed);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            GameLogic.Binder.EventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnPlayerActiveCharacterSwitched += new GameLogic.Events.PlayerActiveCharacterSwitched(this.onPlayerActiveCharacterSwitched);
            App.Binder.EventBus.OnLeaderboardLoaded += new App.Events.LeaderboardLoaded(this.onLeaderboardLoaded);
            App.Binder.EventBus.OnPlayerScoreUpdated += new App.Events.PlayerScoreUpdated(this.onPlayerScoreUpdated);
            GameLogic.Binder.EventBus.OnPlayerRenamed += new GameLogic.Events.PlayerRenamed(this.onPlayerRenamed);
        }

        private void onGameplayStarted(ActiveDungeon ad)
        {
            this.refreshLeaderboards();
        }

        private void onGameStateInitialized()
        {
            this.initialize();
        }

        private void onLeaderboardLoaded(LeaderboardType lbType)
        {
            ILeaderboardView view = this.getLeaderboardView(lbType);
            if (view != null)
            {
                view.NotifyLeaderboardLoaded();
            }
        }

        private void onPlayerActiveCharacterSwitched(CharacterInstance activeCharacter)
        {
            for (int i = 0; i < this.m_leaderboardViews.Count; i++)
            {
                this.m_leaderboardViews[i].NotifyCharacterUpdated(activeCharacter);
            }
        }

        private void onPlayerRenamed(Player player)
        {
            for (int i = 0; i < this.m_leaderboardViews.Count; i++)
            {
                this.m_leaderboardViews[i].NotifyPlayerRenamed(player);
            }
        }

        private void onPlayerScoreUpdated(LeaderboardType lbType, int newScore)
        {
            ILeaderboardView view = this.getLeaderboardView(lbType);
            if (view != null)
            {
                view.NotifyPlayerScoreUpdated(newScore);
            }
        }

        public void refreshLeaderboards()
        {
            if (this.Initialized)
            {
                for (int i = 0; i < this.m_leaderboardViews.Count; i++)
                {
                    this.m_leaderboardViews[i].refresh();
                }
            }
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

        [CompilerGenerated]
        private sealed class <initializationRoutine>c__Iterator7E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal IEnumerator <$s_331>__0;
            internal LeaderboardSystem <>f__this;
            internal int <i>__3;
            internal LeaderboardType <lbType>__1;
            internal ILeaderboardView <lbView>__2;

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
                        this.<>f__this.m_initializationRoutineRunning = true;
                        this.<>f__this.Initialized = false;
                        this.<>f__this.m_leaderboardViews.Clear();
                        this.<$s_331>__0 = Enum.GetValues(typeof(LeaderboardType)).GetEnumerator();
                        try
                        {
                            while (this.<$s_331>__0.MoveNext())
                            {
                                this.<lbType>__1 = (LeaderboardType) ((int) this.<$s_331>__0.Current);
                                this.<lbView>__2 = this.<>f__this.createLeaderboardView(this.<lbType>__1);
                                this.<lbView>__2.initialize();
                                this.<>f__this.m_leaderboardViews.Add(this.<lbView>__2);
                            }
                        }
                        finally
                        {
                            IDisposable disposable = this.<$s_331>__0 as IDisposable;
                            if (disposable == null)
                            {
                            }
                            disposable.Dispose();
                        }
                        this.<i>__3 = 0;
                        goto Label_0134;

                    case 1:
                        break;

                    default:
                        goto Label_017E;
                }
            Label_0106:
                if (!this.<>f__this.m_leaderboardViews[this.<i>__3].Initialized)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<i>__3++;
            Label_0134:
                if (this.<i>__3 < this.<>f__this.m_leaderboardViews.Count)
                {
                    goto Label_0106;
                }
                this.<>f__this.Initialized = true;
                this.<>f__this.refreshLeaderboards();
                this.<>f__this.m_initializationRoutineRunning = false;
                goto Label_017E;
                this.$PC = -1;
            Label_017E:
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

