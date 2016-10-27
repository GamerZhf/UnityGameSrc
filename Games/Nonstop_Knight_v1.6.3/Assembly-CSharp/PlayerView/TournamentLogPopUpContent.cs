namespace PlayerView
{
    using GameLogic;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class TournamentLogPopUpContent : MenuContent
    {
        [CompilerGenerated]
        private List<LeaderboardCell> <LeaderboardCells>k__BackingField;
        public Text LoadingText;
        public RectTransform LogCellGroupRoot;
        public GameObject LogCellPrototype;
        private List<TournamentLogCell> m_logCells;
        private Coroutine m_reconstructRoutine;

        protected override void onAwake()
        {
            this.m_logCells = new List<TournamentLogCell>(0x19);
            for (int i = 0; i < this.m_logCells.Capacity; i++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.LogCellPrototype);
                obj2.transform.SetParent(this.LogCellGroupRoot, false);
                TournamentLogCell component = obj2.GetComponent<TournamentLogCell>();
                this.m_logCells.Add(component);
                component.initialize((i % 2) == 0);
                obj2.SetActive(false);
            }
            this.LogCellPrototype.gameObject.SetActive(false);
            this.LoadingText.gameObject.SetActive(true);
        }

        protected override void onCleanup()
        {
            UnityUtils.StopCoroutine(this, ref this.m_reconstructRoutine);
        }

        protected void OnDisable()
        {
            Service.Binder.EventBus.OnTournamentLogUpdated -= new Service.Events.TournamentLogUpdated(this.reconstructContent);
        }

        protected void OnEnable()
        {
            Service.Binder.EventBus.OnTournamentLogUpdated += new Service.Events.TournamentLogUpdated(this.reconstructContent);
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle("HUNT JOURNAL", string.Empty, string.Empty);
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator1A2 iteratora = new <onShow>c__Iterator1A2();
            iteratora.<>f__this = this;
            return iteratora;
        }

        private void reconstructContent()
        {
            UnityUtils.StopCoroutine(this, ref this.m_reconstructRoutine);
            this.m_reconstructRoutine = UnityUtils.StartCoroutine(this, this.reconstructRoutine());
        }

        [DebuggerHidden]
        private IEnumerator reconstructRoutine()
        {
            <reconstructRoutine>c__Iterator1A3 iteratora = new <reconstructRoutine>c__Iterator1A3();
            iteratora.<>f__this = this;
            return iteratora;
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.TournamentLogPopUpContent;
            }
        }

        public List<LeaderboardCell> LeaderboardCells
        {
            [CompilerGenerated]
            get
            {
                return this.<LeaderboardCells>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<LeaderboardCells>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator1A2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TournamentLogPopUpContent <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    this.<>f__this.reconstructContent();
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
        private sealed class <reconstructRoutine>c__Iterator1A3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TournamentLogPopUpContent <>f__this;
            internal int <i>__1;
            internal List<TournamentLogEvent> <logEvents>__0;

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
                        this.<>f__this.LoadingText.gameObject.SetActive(true);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_017F;
                }
                if (!GameLogic.Binder.GameState.Player.Tournaments.hasTournamentSelected())
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<logEvents>__0 = Service.Binder.TournamentSystem.GetTournamentView(GameLogic.Binder.GameState.Player.Tournaments.SelectedTournamentId).Log.GetDisplayableEvents();
                this.<i>__1 = 0;
                while (this.<i>__1 < this.<>f__this.m_logCells.Count)
                {
                    if (this.<i>__1 < this.<logEvents>__0.Count)
                    {
                        this.<>f__this.m_logCells[this.<i>__1].gameObject.SetActive(true);
                        this.<>f__this.m_logCells[this.<i>__1].refresh(this.<logEvents>__0[this.<logEvents>__0.Count - (1 + this.<i>__1)]);
                    }
                    else
                    {
                        this.<>f__this.m_logCells[this.<i>__1].gameObject.SetActive(false);
                    }
                    this.<i>__1++;
                }
                this.<>f__this.LoadingText.gameObject.SetActive(false);
                this.$PC = -1;
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

