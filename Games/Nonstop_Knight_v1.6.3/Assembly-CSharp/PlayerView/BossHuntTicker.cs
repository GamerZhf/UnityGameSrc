namespace PlayerView
{
    using GameLogic;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BossHuntTicker : MonoBehaviour
    {
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        private float m_lastTickTime = Time.realtimeSinceStartup;
        private List<BossHuntTickerCell> m_tickerCells = new List<BossHuntTickerCell>();
        private Coroutine m_tickerRoutine;
        private int MAX_TICKER_CELLS = 3;
        private static readonly int NORMAL_TICKER_INTERVAL_MAX = 6;
        private static readonly int NORMAL_TICKER_INTERVAL_MIN = 2;
        private static readonly float TICKER_MIN_INTERVAL = 1f;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
        }

        protected void OnDisable()
        {
            UnityUtils.StopCoroutine(this, ref this.m_tickerRoutine);
            for (int i = 0; i < this.m_tickerCells.Count; i++)
            {
                this.m_tickerCells[i].cleanUpForReuse();
            }
            this.m_tickerCells.Clear();
        }

        protected void OnEnable()
        {
            UnityUtils.StopCoroutine(this, ref this.m_tickerRoutine);
            this.m_tickerRoutine = UnityUtils.StartCoroutine(this, this.TickerRoutine());
        }

        [DebuggerHidden]
        private IEnumerator TickerRoutine()
        {
            <TickerRoutine>c__IteratorF7 rf = new <TickerRoutine>c__IteratorF7();
            rf.<>f__this = this;
            return rf;
        }

        public RectTransform RectTm
        {
            [CompilerGenerated]
            get
            {
                return this.<RectTm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RectTm>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <TickerRoutine>c__IteratorF7 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal BossHuntTicker <>f__this;
            internal BossHuntTickerCell <cell>__6;
            internal int <i>__0;
            internal int <i>__7;
            internal int <i>__8;
            internal int <randomThreshold>__5;
            internal TournamentLogEvent <tickerLogEvent>__3;
            internal float <timeSinceLastTick>__4;
            internal string <tournamentId>__1;
            internal TournamentView <tournamentView>__2;

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
                        if ((Service.Binder.TournamentSystem == null) || !Service.Binder.TournamentSystem.Initialized)
                        {
                            this.$current = null;
                            this.$PC = 1;
                            goto Label_0387;
                        }
                        break;

                    case 2:
                        goto Label_016A;

                    case 3:
                        break;
                        this.$PC = -1;
                        goto Label_0385;

                    default:
                        goto Label_0385;
                }
                this.<i>__0 = 0;
                while (this.<i>__0 < this.<>f__this.m_tickerCells.Count)
                {
                    if (this.<>f__this.m_tickerCells[this.<i>__0].ReadyToDie)
                    {
                        this.<>f__this.m_tickerCells[this.<i>__0].cleanUpForReuse();
                        PlayerView.Binder.BossHuntTickerCellPool.returnObject(this.<>f__this.m_tickerCells[this.<i>__0]);
                        this.<>f__this.m_tickerCells.Remove(this.<>f__this.m_tickerCells[this.<i>__0]);
                    }
                    this.<i>__0++;
                }
                this.<tournamentId>__1 = GameLogic.Binder.GameState.Player.Tournaments.SelectedTournamentId;
                if (this.<tournamentId>__1 == null)
                {
                    goto Label_035D;
                }
                this.<tournamentView>__2 = Service.Binder.TournamentSystem.GetTournamentView(this.<tournamentId>__1);
            Label_016A:
                while (!this.<tournamentView>__2.FullyComposed)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0387;
                }
                this.<tickerLogEvent>__3 = this.<tournamentView>__2.Log.PopPriorityLogEventQueue();
                this.<timeSinceLastTick>__4 = Time.realtimeSinceStartup - this.<>f__this.m_lastTickTime;
                if ((this.<tickerLogEvent>__3 == null) && (this.<timeSinceLastTick>__4 > BossHuntTicker.NORMAL_TICKER_INTERVAL_MIN))
                {
                    this.<randomThreshold>__5 = MathUtil.RandomFromRange(BossHuntTicker.NORMAL_TICKER_INTERVAL_MIN, BossHuntTicker.NORMAL_TICKER_INTERVAL_MAX, (uint) UnityEngine.Random.Range(0, 0x3e8));
                    if (this.<timeSinceLastTick>__4 > this.<randomThreshold>__5)
                    {
                        this.<tickerLogEvent>__3 = this.<tournamentView>__2.Log.PopLogEventQueue();
                    }
                }
                if (this.<tickerLogEvent>__3 != null)
                {
                    this.<>f__this.m_lastTickTime = Time.realtimeSinceStartup;
                    this.<cell>__6 = PlayerView.Binder.BossHuntTickerCellPool.getObject();
                    this.<cell>__6.Initialize(this.<tickerLogEvent>__3, this.<>f__this.RectTm);
                    this.<>f__this.m_tickerCells.Insert(0, this.<cell>__6);
                    this.<i>__7 = 0;
                    while (this.<i>__7 < this.<>f__this.m_tickerCells.Count)
                    {
                        this.<>f__this.m_tickerCells[this.<i>__7].BumpToSlot(this.<i>__7 + 1);
                        this.<i>__7++;
                    }
                    if (this.<>f__this.m_tickerCells.Count > this.<>f__this.MAX_TICKER_CELLS)
                    {
                        this.<i>__8 = this.<>f__this.m_tickerCells.Count - 1;
                        while (this.<i>__8 >= this.<>f__this.MAX_TICKER_CELLS)
                        {
                            if (!this.<>f__this.m_tickerCells[this.<i>__8].ReadyToDie)
                            {
                                this.<>f__this.m_tickerCells[this.<i>__8].PreMatureDestroy();
                            }
                            this.<i>__8--;
                        }
                    }
                }
            Label_035D:
                this.$current = new WaitForSeconds(BossHuntTicker.TICKER_MIN_INTERVAL);
                this.$PC = 3;
                goto Label_0387;
            Label_0385:
                return false;
            Label_0387:
                return true;
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

