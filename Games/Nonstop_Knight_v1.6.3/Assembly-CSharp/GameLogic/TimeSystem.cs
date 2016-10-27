namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TimeSystem : MonoBehaviour, ITimeSystem
    {
        public const float FROZEN_TIMESCALE = 0.001f;
        public const float GAMEPLAY_SLOWDOWN_TIMESCALE = 0.7f;
        private bool m_gameplaySlowdownTimeFlag;
        private bool m_pauseTimeFreezeFlag;
        private bool m_refreshPriorities;
        private float m_speedCheatTargetTimescale = 1f;
        private long m_systemClockLastFrame;
        private Coroutine m_timeFreezeRoutine;
        private bool m_tutorialTimeFreezeFlag;

        protected void Awake()
        {
            this.m_systemClockLastFrame = TimeUtil.DateTimeToUnixTimestamp(DateTime.UtcNow);
        }

        public void gameplaySlowdown(bool enabled)
        {
            if (this.m_gameplaySlowdownTimeFlag != enabled)
            {
                this.m_gameplaySlowdownTimeFlag = enabled;
                this.m_refreshPriorities = true;
                GameLogic.Binder.EventBus.GameplayTimeSlowdownToggled(enabled);
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            UnityUtils.StopCoroutine(this, ref this.m_timeFreezeRoutine);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            this.m_timeFreezeRoutine = UnityUtils.StartCoroutine(this, this.timeScaleManipulationRoutine());
        }

        private void onGameStateInitialized()
        {
            Player player = GameLogic.Binder.GameState.Player;
            long timeOffsetSeconds = this.m_systemClockLastFrame - player.LastSerializationTimestamp;
            if (timeOffsetSeconds < 0L)
            {
                GameLogic.Binder.EventBus.SuspectedSystemClockCheat(timeOffsetSeconds);
            }
        }

        public void pause(bool paused)
        {
            if (this.m_pauseTimeFreezeFlag != paused)
            {
                this.m_pauseTimeFreezeFlag = paused;
                this.m_refreshPriorities = true;
                GameLogic.Binder.EventBus.PauseToggled(paused);
            }
        }

        public bool paused()
        {
            return this.m_pauseTimeFreezeFlag;
        }

        private void setTimeScale(float timescale)
        {
            if (timescale != Time.timeScale)
            {
                GameLogic.Binder.EventBus.TimescaleChangeStarted(timescale);
                Time.timeScale = timescale;
                GameLogic.Binder.EventBus.TimescaleChanged(timescale);
            }
        }

        public void speedCheat(float targetTimescale)
        {
            this.m_speedCheatTargetTimescale = targetTimescale;
            this.m_refreshPriorities = true;
        }

        [DebuggerHidden]
        private IEnumerator timeScaleManipulationRoutine()
        {
            <timeScaleManipulationRoutine>c__Iterator77 iterator = new <timeScaleManipulationRoutine>c__Iterator77();
            iterator.<>f__this = this;
            return iterator;
        }

        public void tutorialSlowdown(bool enabled)
        {
            if (this.m_tutorialTimeFreezeFlag != enabled)
            {
                this.m_tutorialTimeFreezeFlag = enabled;
                this.m_refreshPriorities = true;
            }
        }

        public bool tutorialSlowdownEnabled()
        {
            return this.m_tutorialTimeFreezeFlag;
        }

        protected void Update()
        {
            if (GameLogic.Binder.GameState.Player != null)
            {
                long num = TimeUtil.DateTimeToUnixTimestamp(DateTime.UtcNow);
                long timeOffsetSeconds = num - this.m_systemClockLastFrame;
                if (timeOffsetSeconds < 0L)
                {
                    GameLogic.Binder.EventBus.SuspectedSystemClockCheat(timeOffsetSeconds);
                }
                this.m_systemClockLastFrame = num;
            }
        }

        [CompilerGenerated]
        private sealed class <timeScaleManipulationRoutine>c__Iterator77 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TimeSystem <>f__this;
            internal int <frameCount>__0;
            internal int <frameCount>__3;
            internal int <i>__2;
            internal int <i>__5;
            internal float <vPerStep>__1;
            internal float <vPerStep>__4;

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
                        goto Label_006D;

                    case 2:
                        goto Label_00F8;

                    case 3:
                        goto Label_013F;

                    case 4:
                        goto Label_01CA;

                    case 5:
                        goto Label_0211;

                    case 6:
                        break;
                        this.$PC = -1;
                        goto Label_02B1;

                    default:
                        goto Label_02B1;
                }
                if (!this.<>f__this.m_pauseTimeFreezeFlag)
                {
                    goto Label_007D;
                }
                this.<>f__this.setTimeScale(0.001f);
            Label_006D:
                while (this.<>f__this.m_pauseTimeFreezeFlag)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_02B3;
                }
            Label_007D:
                if (!this.<>f__this.m_tutorialTimeFreezeFlag)
                {
                    goto Label_014F;
                }
                this.<frameCount>__0 = ConfigTutorials.TUTORIAL_TIME_SLOWDOWN_FRAME_COUNT;
                this.<vPerStep>__1 = 1f / ((float) this.<frameCount>__0);
                this.<i>__2 = 1;
                while (this.<i>__2 <= this.<frameCount>__0)
                {
                    this.<>f__this.setTimeScale(Mathf.Clamp((float) (1f - (this.<i>__2 * this.<vPerStep>__1)), (float) 0.001f, (float) 1f));
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_02B3;
                Label_00F8:
                    this.<i>__2++;
                }
                this.<>f__this.setTimeScale(0.001f);
            Label_013F:
                while (this.<>f__this.m_tutorialTimeFreezeFlag)
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_02B3;
                }
            Label_014F:
                if (!this.<>f__this.m_gameplaySlowdownTimeFlag)
                {
                    goto Label_0231;
                }
                this.<frameCount>__3 = ConfigUi.MENU_TIME_TIME_SLOWDOWN_FRAME_COUNT;
                this.<vPerStep>__4 = 1f / ((float) this.<frameCount>__3);
                this.<i>__5 = 1;
                while (this.<i>__5 <= this.<frameCount>__3)
                {
                    this.<>f__this.setTimeScale(Mathf.Clamp((float) (1f - (this.<i>__5 * this.<vPerStep>__4)), (float) 0.7f, (float) 1f));
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_02B3;
                Label_01CA:
                    this.<i>__5++;
                }
                this.<>f__this.setTimeScale(0.7f);
            Label_0211:
                while (this.<>f__this.m_gameplaySlowdownTimeFlag && !this.<>f__this.m_refreshPriorities)
                {
                    this.$current = null;
                    this.$PC = 5;
                    goto Label_02B3;
                }
            Label_0231:
                if ((this.<>f__this.m_speedCheatTargetTimescale != 1f) && (Time.timeScale != this.<>f__this.m_speedCheatTargetTimescale))
                {
                    this.<>f__this.setTimeScale(this.<>f__this.m_speedCheatTargetTimescale);
                }
                else
                {
                    this.<>f__this.setTimeScale(1f);
                }
                this.<>f__this.m_refreshPriorities = false;
                this.$current = null;
                this.$PC = 6;
                goto Label_02B3;
            Label_02B1:
                return false;
            Label_02B3:
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

