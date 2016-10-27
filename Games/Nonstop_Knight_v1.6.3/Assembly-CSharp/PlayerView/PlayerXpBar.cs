namespace PlayerView
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
    using UnityEngine.UI;

    public class PlayerXpBar : MonoBehaviour
    {
        [NonSerialized]
        public float AnimationDuration = 0.13f;
        [NonSerialized]
        public Easing.Function DefaultEasing = Easing.Function.OUT_QUAD;
        private int m_currentRank;
        private double m_currentXp;
        private Coroutine m_masterRoutine;
        private List<double> m_queue = new List<double>(20);
        private ManualTimer m_timer = new ManualTimer();
        public Text PlayerRank;
        public ParticleSystem PlayerRankBlingEffect;
        public GameObject PlayerRankForegroundDefault;
        public GameObject PlayerRankForegroundMaxLevel;
        public Transform PlayerRankIconTm;
        public PulsatingGraphic PlayerRankPulseGraphic;
        public ScalePulse PlayerRankScalePulse;
        public AnimatedProgressBar ProgressBar;

        protected void Awake()
        {
        }

        private float getNormalizedProgressTowardsNextRank(double xp)
        {
            return Mathf.Clamp01((float) (xp / App.Binder.ConfigMeta.XpRequiredForRankUp(this.m_currentRank)));
        }

        public void initialize(Player player)
        {
            this.set(player.Rank, player.getResourceAmount(ResourceType.Xp), true);
        }

        public bool isAnimating()
        {
            return ((this.m_queue.Count > 0) || !this.m_timer.Idle);
        }

        [DebuggerHidden]
        private IEnumerator masterRoutine()
        {
            <masterRoutine>c__Iterator100 iterator = new <masterRoutine>c__Iterator100();
            iterator.<>f__this = this;
            return iterator;
        }

        protected void OnDisable()
        {
            UnityUtils.StopCoroutine(this, ref this.m_masterRoutine);
        }

        protected void OnEnable()
        {
            this.restartMasterRoutine();
        }

        public void queue(double xpAmount)
        {
            this.m_queue.Add(xpAmount);
        }

        private void restartMasterRoutine()
        {
            UnityUtils.StopCoroutine(this, ref this.m_masterRoutine);
            if (base.gameObject.activeInHierarchy)
            {
                this.m_masterRoutine = UnityUtils.StartCoroutine(this, this.masterRoutine());
            }
        }

        public void set(int rank, double xp, [Optional, DefaultParameterValue(true)] bool clearQueue)
        {
            if (clearQueue)
            {
                if (this.m_queue.Count > 0)
                {
                }
                this.m_queue.Clear();
                this.restartMasterRoutine();
            }
            this.m_currentRank = rank;
            this.m_currentXp = MathUtil.Clamp(xp, 0.0, double.MaxValue);
            this.PlayerRank.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.DHUD_PLAYER_LEVEL, new <>__AnonType8<int>(this.m_currentRank), false));
            this.ProgressBar.setNormalizedValue(this.getNormalizedProgressTowardsNextRank(this.m_currentXp));
            bool flag = this.m_currentRank >= App.Binder.ConfigMeta.XP_LEVEL_CAP;
            this.PlayerRankForegroundDefault.SetActive(!flag);
            this.PlayerRankForegroundMaxLevel.SetActive(flag);
        }

        [CompilerGenerated]
        private sealed class <masterRoutine>c__Iterator100 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal PlayerXpBar <>f__this;
            internal double <delta>__4;
            internal float <easedV>__5;
            internal int <i>__1;
            internal float <normalizedProgressTowardsNextRank>__7;
            internal double <requiredXp>__8;
            internal double <source>__2;
            internal double <target>__3;
            internal double <totalInQueue>__0;
            internal double <xp>__6;

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
                        goto Label_02CB;

                    case 2:
                        break;
                        this.$PC = -1;
                        goto Label_031C;

                    default:
                        goto Label_031C;
                }
                this.<totalInQueue>__0 = 0.0;
                this.<i>__1 = 0;
                while (this.<i>__1 < this.<>f__this.m_queue.Count)
                {
                    this.<totalInQueue>__0 += this.<>f__this.m_queue[this.<i>__1];
                    this.<i>__1++;
                }
                this.<>f__this.m_queue.Clear();
                if (this.<totalInQueue>__0 == 0.0)
                {
                    goto Label_02FD;
                }
                this.<>f__this.m_timer.set(this.<>f__this.AnimationDuration);
                this.<source>__2 = this.<>f__this.m_currentXp;
                this.<target>__3 = this.<>f__this.m_currentXp + this.<totalInQueue>__0;
                this.<delta>__4 = this.<target>__3 - this.<source>__2;
            Label_02CB:
                while (!this.<>f__this.m_timer.Idle)
                {
                    this.<easedV>__5 = Easing.Apply(this.<>f__this.m_timer.normalizedProgress(), this.<>f__this.DefaultEasing);
                    this.<xp>__6 = this.<source>__2 + (this.<delta>__4 * this.<easedV>__5);
                    this.<normalizedProgressTowardsNextRank>__7 = this.<>f__this.getNormalizedProgressTowardsNextRank(this.<xp>__6);
                    if (this.<normalizedProgressTowardsNextRank>__7 >= 1f)
                    {
                        this.<requiredXp>__8 = App.Binder.ConfigMeta.XpRequiredForRankUp(this.<>f__this.m_currentRank);
                        this.<source>__2 = MathUtil.Clamp(this.<source>__2 - this.<requiredXp>__8, 0.0, double.MaxValue);
                        this.<target>__3 = MathUtil.Clamp(this.<target>__3 - this.<requiredXp>__8, 0.0, double.MaxValue);
                        this.<xp>__6 = MathUtil.Clamp(this.<xp>__6 - this.<requiredXp>__8, 0.0, double.MaxValue);
                        this.<>f__this.m_currentRank = Mathf.Min(this.<>f__this.m_currentRank + 1, App.Binder.ConfigMeta.XP_LEVEL_CAP);
                        this.<>f__this.PlayerRankBlingEffect.Play(true);
                        PlayerView.Binder.AudioSystem.playSfx(!GameLogic.Binder.FrenzySystem.isFrenzyActive() ? AudioSourceType.SfxUi_PlayerRankUp : AudioSourceType.SfxUi_PlayerRankUpFrenzy, (float) 0f);
                        PlayerView.Binder.DungeonHud.TaskPanel.refreshTaskPanelUnclaimedLevelUpRewards();
                        PlayerView.Binder.DungeonHud.applyTutorialRestrictions();
                    }
                    this.<>f__this.set(this.<>f__this.m_currentRank, this.<xp>__6, false);
                    this.<>f__this.m_timer.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_031E;
                }
                this.<>f__this.set(this.<>f__this.m_currentRank, this.<target>__3, false);
            Label_02FD:
                this.$current = null;
                this.$PC = 2;
                goto Label_031E;
            Label_031C:
                return false;
            Label_031E:
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

