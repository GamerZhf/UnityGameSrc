namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class FloaterText : MonoBehaviour
    {
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        [CompilerGenerated]
        private TransformAnimation <TransformAnimation>k__BackingField;
        public const int AUDIO_TRIGGER_KILLS_THRESHOLD = 3;
        public UnityEngine.CanvasGroup CanvasGroup;
        public UnityEngine.UI.Text CoinGain;
        private ManualTimer m_exitTimer = new ManualTimer(ConfigUi.FLOATER_EXIT_DURATION);
        private Coroutine m_masterRoutine;
        private UnityEngine.UI.Text m_multikill10Text;
        private UnityEngine.UI.Text m_multikill3Text;
        private UnityEngine.UI.Text m_multikill4Text;
        private UnityEngine.UI.Text m_multikill5Text;
        private UnityEngine.UI.Text m_multikill6Text;
        private UnityEngine.UI.Text m_multikill7Text;
        private UnityEngine.UI.Text m_multikill8Text;
        private UnityEngine.UI.Text m_multikill9Text;
        private ManualTimer m_stayTimer = new ManualTimer(ConfigUi.FLOATER_STAY_DURATION);
        private TransformAnimationTask m_taTask;
        private int m_visibleKillCount;
        public GameObject Multikill10;
        public GameObject Multikill3;
        public GameObject Multikill4;
        public GameObject Multikill5;
        public GameObject Multikill6;
        public GameObject Multikill7;
        public GameObject Multikill8;
        public GameObject Multikill9;
        public UnityEngine.UI.Text Text;

        protected void Awake()
        {
            this.Tm = base.transform;
            this.TransformAnimation = base.gameObject.AddComponent<TransformAnimation>();
            this.m_taTask = new TransformAnimationTask(this.Tm, ConfigUi.FLOATER_ENTRY_DURATION, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
            this.m_taTask.scale(Vector3.one, true, Easing.Function.OUT_BACK);
            this.m_multikill3Text = this.Multikill3.GetComponent<UnityEngine.UI.Text>();
            this.m_multikill4Text = this.Multikill4.GetComponent<UnityEngine.UI.Text>();
            this.m_multikill5Text = this.Multikill5.GetComponent<UnityEngine.UI.Text>();
            this.m_multikill6Text = this.Multikill6.GetComponent<UnityEngine.UI.Text>();
            this.m_multikill7Text = this.Multikill7.GetComponent<UnityEngine.UI.Text>();
            this.m_multikill8Text = this.Multikill8.GetComponent<UnityEngine.UI.Text>();
            this.m_multikill9Text = this.Multikill9.GetComponent<UnityEngine.UI.Text>();
            this.m_multikill10Text = this.Multikill10.GetComponent<UnityEngine.UI.Text>();
            this.m_multikill3Text.text = _.L(ConfigLoca.MULTIKILL3_ANNOUNCEMENT, null, false);
            this.m_multikill4Text.text = _.L(ConfigLoca.MULTIKILL4_ANNOUNCEMENT, null, false);
            this.m_multikill5Text.text = _.L(ConfigLoca.MULTIKILL5_ANNOUNCEMENT, null, false);
            this.m_multikill6Text.text = _.L(ConfigLoca.MULTIKILL6_ANNOUNCEMENT, null, false);
            this.m_multikill7Text.text = _.L(ConfigLoca.MULTIKILL7_ANNOUNCEMENT, null, false);
            this.m_multikill8Text.text = _.L(ConfigLoca.MULTIKILL8_ANNOUNCEMENT, null, false);
            this.m_multikill9Text.text = _.L(ConfigLoca.MULTIKILL9_ANNOUNCEMENT, null, false);
            this.m_multikill10Text.text = _.L(ConfigLoca.MULTIKILL10_ANNOUNCEMENT, null, false);
        }

        [DebuggerHidden]
        private IEnumerator masterRoutine()
        {
            <masterRoutine>c__IteratorFE rfe = new <masterRoutine>c__IteratorFE();
            rfe.<>f__this = this;
            return rfe;
        }

        public void prewarm()
        {
            base.StartCoroutine(this.prewarmRoutine());
        }

        [DebuggerHidden]
        private IEnumerator prewarmRoutine()
        {
            <prewarmRoutine>c__IteratorFF rff = new <prewarmRoutine>c__IteratorFF();
            rff.<>f__this = this;
            return rff;
        }

        public void showOrRefresh(int killCount)
        {
            bool flag = this.m_exitTimer.timeElapsed() > 0f;
            if (flag || (killCount >= this.m_visibleKillCount))
            {
                this.Text.text = _.L(ConfigLoca.MULTIKILL_NUM_ENEMIES_KILLED, new <>__AnonType4<int>(killCount), false);
                this.m_visibleKillCount = killCount;
                ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                CharacterInstance primaryPlayerCharacter = activeDungeon.PrimaryPlayerCharacter;
                double baseAmount = 0.0;
                this.Multikill3.SetActive(false);
                this.Multikill4.SetActive(false);
                this.Multikill5.SetActive(false);
                this.Multikill6.SetActive(false);
                this.Multikill7.SetActive(false);
                this.Multikill8.SetActive(false);
                this.Multikill9.SetActive(false);
                this.Multikill10.SetActive(false);
                if (this.m_visibleKillCount >= 10)
                {
                    this.Multikill10.SetActive(true);
                }
                else if (this.m_visibleKillCount >= 9)
                {
                    this.Multikill9.SetActive(true);
                }
                else if (this.m_visibleKillCount >= 8)
                {
                    this.Multikill8.SetActive(true);
                }
                else if (this.m_visibleKillCount >= 7)
                {
                    this.Multikill7.SetActive(true);
                }
                else if (this.m_visibleKillCount >= 6)
                {
                    this.Multikill6.SetActive(true);
                }
                else if (this.m_visibleKillCount >= 5)
                {
                    this.Multikill5.SetActive(true);
                }
                else if (this.m_visibleKillCount >= 4)
                {
                    this.Multikill4.SetActive(true);
                }
                else if (this.m_visibleKillCount >= 3)
                {
                    this.Multikill3.SetActive(true);
                }
                else
                {
                    UnityEngine.Debug.LogWarning("Trying to show float text with fewer than 3 simulatenous kills.");
                }
                if (this.m_visibleKillCount >= 3)
                {
                    PlayerView.Binder.AudioSystem.playSfxIfNotAlreadyPlaying(AudioSourceType.SfxUi_NotificationGreat);
                }
                baseAmount = App.Binder.ConfigMeta.MultikillCoinGainCurve(this.m_visibleKillCount, activeDungeon.Floor);
                baseAmount = CharacterStatModifierUtil.ApplyCoinBonuses(primaryPlayerCharacter, GameLogic.CharacterType.UNSPECIFIED, baseAmount, false);
                this.CoinGain.text = MenuHelpers.BigValueToString(baseAmount);
                this.m_stayTimer.reset();
                if (!UnityUtils.CoroutineRunning(ref this.m_masterRoutine))
                {
                    this.m_masterRoutine = UnityUtils.StartCoroutine(this, this.masterRoutine());
                }
                else if (flag)
                {
                    UnityUtils.StopCoroutine(this, ref this.m_masterRoutine);
                    this.m_masterRoutine = UnityUtils.StartCoroutine(this, this.masterRoutine());
                }
            }
        }

        protected void Update()
        {
        }

        public bool IsVisible
        {
            get
            {
                return UnityUtils.CoroutineRunning(ref this.m_masterRoutine);
            }
        }

        public Transform Tm
        {
            [CompilerGenerated]
            get
            {
                return this.<Tm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Tm>k__BackingField = value;
            }
        }

        public TransformAnimation TransformAnimation
        {
            [CompilerGenerated]
            get
            {
                return this.<TransformAnimation>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<TransformAnimation>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <masterRoutine>c__IteratorFE : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal FloaterText <>f__this;

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
                        this.<>f__this.CanvasGroup.alpha = 1f;
                        this.<>f__this.Tm.localScale = Vector3.zero;
                        this.<>f__this.m_taTask.reset();
                        this.<>f__this.TransformAnimation.addTask(this.<>f__this.m_taTask);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00DA;

                    case 3:
                        goto Label_0153;

                    default:
                        goto Label_01B1;
                }
                if (this.<>f__this.TransformAnimation.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01B3;
                }
            Label_00DA:
                while (!this.<>f__this.m_stayTimer.Idle)
                {
                    this.<>f__this.m_stayTimer.tick(Time.deltaTime);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01B3;
                }
                this.<>f__this.m_exitTimer.reset();
            Label_0153:
                while (!this.<>f__this.m_exitTimer.Idle)
                {
                    this.<>f__this.CanvasGroup.alpha = 1f - this.<>f__this.m_exitTimer.normalizedProgress();
                    this.<>f__this.m_exitTimer.tick(Time.deltaTime);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_01B3;
                }
                this.<>f__this.CanvasGroup.alpha = 0f;
                this.<>f__this.m_exitTimer.reset();
                this.<>f__this.m_visibleKillCount = 0;
                this.<>f__this.m_masterRoutine = null;
                goto Label_01B1;
                this.$PC = -1;
            Label_01B1:
                return false;
            Label_01B3:
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

        [CompilerGenerated]
        private sealed class <prewarmRoutine>c__IteratorFF : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal FloaterText <>f__this;

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
                        this.<>f__this.Text.text = _.L(ConfigLoca.MULTIKILL_NUM_ENEMIES_KILLED, new <>__AnonType4<int>(10), false);
                        this.<>f__this.Multikill3.SetActive(true);
                        this.<>f__this.Multikill4.SetActive(true);
                        this.<>f__this.Multikill5.SetActive(true);
                        this.<>f__this.Multikill6.SetActive(true);
                        this.<>f__this.Multikill7.SetActive(true);
                        this.<>f__this.Multikill8.SetActive(true);
                        this.<>f__this.Multikill9.SetActive(true);
                        this.<>f__this.Multikill10.SetActive(true);
                        this.$current = null;
                        this.$PC = 1;
                        return true;

                    case 1:
                        this.<>f__this.Multikill3.SetActive(false);
                        this.<>f__this.Multikill4.SetActive(false);
                        this.<>f__this.Multikill5.SetActive(false);
                        this.<>f__this.Multikill6.SetActive(false);
                        this.<>f__this.Multikill7.SetActive(false);
                        this.<>f__this.Multikill8.SetActive(false);
                        this.<>f__this.Multikill9.SetActive(false);
                        this.<>f__this.Multikill10.SetActive(false);
                        break;

                    default:
                        break;
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
    }
}

