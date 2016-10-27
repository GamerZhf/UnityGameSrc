namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using Service.SupersonicAds;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class FullscreenAdMenu : Menu
    {
        [CompilerGenerated]
        private InputParameters <Params>k__BackingField;
        public CanvasGroupAlphaFading AdCanvas;
        public TransformAnimation AdCloseButton;
        public Text AdLoadingText;
        public Text AdPlaceholderText;
        private const float ANIM_DURATION = 0.3f;
        public GameObject CanvasRoot;
        public GameObject InputBlockerRoot;
        private bool m_fakeAd;
        private VideoResult m_result;
        private bool m_videoEnded;
        private bool m_videoRewarded;

        [DebuggerHidden]
        private IEnumerator executeFinish()
        {
            <executeFinish>c__Iterator11A iteratora = new <executeFinish>c__Iterator11A();
            iteratora.<>f__this = this;
            return iteratora;
        }

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator11B iteratorb = new <hideRoutine>c__Iterator11B();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        private void onAdEnded()
        {
            this.m_videoEnded = true;
            this.tryToFinish();
        }

        private void onAdShowed(VideoResult result)
        {
            this.m_videoRewarded = true;
            this.m_result = result;
            if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Low)
            {
                PlayerView.Binder.SpriteResources.loadAtlas("Menu");
                PlayerView.Binder.SpriteResources.loadAtlas("DungeonHud");
            }
            switch (result.Error)
            {
                case PlayVideoError.None:
                    UnityEngine.Debug.Log("Ad show finished");
                    break;

                case PlayVideoError.SdkNotReady:
                case PlayVideoError.VideoStillPlaying:
                case PlayVideoError.NoNetwork:
                    UnityEngine.Debug.LogError("Ad show failed:" + result.Error);
                    break;
            }
            PlayerView.Binder.EventBus.AdWatched(result);
            if ((this.AdPlaceholderText != null) && (this.AdPlaceholderText.gameObject != null))
            {
                this.AdPlaceholderText.gameObject.SetActive(false);
                this.AdCloseButton.gameObject.SetActive(false);
            }
            this.tryToFinish();
        }

        private void onAdTimedOut()
        {
            this.m_videoEnded = true;
            AdsData data = new AdsData();
            data.AdReward = this.Params.Reward;
            data.AdCategory = this.Params.AdCategory;
            data.AdZone = this.Params.AdZoneId;
            this.onAdShowed(new VideoResult(App.Binder.ConfigMeta.REWARD_ON_VIDEO_AD_TIMOUT, 1, "timedOut", data));
        }

        protected override void onAwake()
        {
            this.AdCanvas.setTransparent(true);
        }

        public void onCloseAdButtonClicked()
        {
            this.m_videoEnded = true;
            bool flag = ((this.m_result != null) && this.m_result.Success) || this.m_fakeAd;
            AdsData data = new AdsData();
            data.AdReward = this.Params.Reward;
            data.AdCategory = this.Params.AdCategory;
            data.AdZone = this.Params.AdZoneId;
            this.onAdShowed(new VideoResult(flag, 1, "closedByAdButton", data));
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator118 iterator = new <preShowRoutine>c__Iterator118();
            iterator.parameter = parameter;
            iterator.<$>parameter = parameter;
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator119 iterator = new <showRoutine>c__Iterator119();
            iterator.<>f__this = this;
            return iterator;
        }

        private void tryToFinish()
        {
            UnityEngine.Debug.Log(string.Concat(new object[] { "try to finish m_videoEnded ", this.m_videoEnded, " m_videoRewarded ", this.m_videoRewarded }));
            if (this.m_videoEnded && this.m_videoRewarded)
            {
                Service.Binder.TaskManager.StartTask(this.executeFinish(), null);
            }
        }

        public override bool IsOverlayMenu
        {
            get
            {
                return true;
            }
        }

        public override PlayerView.MenuType MenuType
        {
            get
            {
                return PlayerView.MenuType.FullscreenAdMenu;
            }
        }

        public InputParameters Params
        {
            [CompilerGenerated]
            get
            {
                return this.<Params>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Params>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <executeFinish>c__Iterator11A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal FullscreenAdMenu <>f__this;
            internal IEnumerator <ie>__0;
            internal Player <player>__1;

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
                        this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(1f);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0139;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    return true;
                }
                if (this.<>f__this.Params.Reward.getTotalCoinAmount() > 0.0)
                {
                    this.<player>__1 = GameLogic.Binder.GameState.Player;
                    this.<player>__1.MysteryChestsWithCoinsConsumed++;
                }
                if ((this.<>f__this.Params.CompleteCallback != null) && (this.<>f__this.m_result != null))
                {
                    List<Reward> list = new List<Reward>();
                    list.Add(this.<>f__this.Params.Reward);
                    this.<>f__this.Params.CompleteCallback(list, this.<>f__this.m_result.Success, 1);
                }
                else
                {
                    PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                }
                this.$PC = -1;
            Label_0139:
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
        private sealed class <hideRoutine>c__Iterator11B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal FullscreenAdMenu <>f__this;

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
                    this.<>f__this.InputBlockerRoot.SetActive(false);
                    this.<>f__this.CanvasRoot.SetActive(false);
                    GameLogic.Binder.TimeSystem.pause(false);
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
        private sealed class <preShowRoutine>c__Iterator118 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal FullscreenAdMenu <>f__this;
            internal object parameter;

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
                    this.<>f__this.Params = (FullscreenAdMenu.InputParameters) this.parameter;
                    this.<>f__this.InputBlockerRoot.SetActive(true);
                    this.<>f__this.CanvasRoot.SetActive(true);
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
        private sealed class <showRoutine>c__Iterator119 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal FullscreenAdMenu <>f__this;
            internal ManualTimer <timeoutTimer>__0;
            internal ManualTimer <timer>__1;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                AdsData data;
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        GameLogic.Binder.TimeSystem.pause(true);
                        this.<>f__this.AdCanvas.setTransparent(true);
                        this.<>f__this.AdPlaceholderText.gameObject.SetActive(false);
                        this.<>f__this.AdCloseButton.gameObject.SetActive(false);
                        this.<>f__this.AdLoadingText.text = _.L(ConfigLoca.UI_STATUS_LOADING, null, false);
                        this.$current = this.<>f__this.AdCanvas.animateToBlack(0.3f, 0f);
                        this.$PC = 1;
                        goto Label_0432;

                    case 1:
                        if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Low)
                        {
                            PlayerView.Binder.SpriteResources.releaseAtlas("Menu");
                            PlayerView.Binder.SpriteResources.releaseAtlas("DungeonHud");
                        }
                        this.<>f__this.m_fakeAd = !AdsSystem.AdsSupported();
                        if (this.<>f__this.m_fakeAd)
                        {
                            PlayerView.Binder.EventBus.AdWatchStarted(this.<>f__this.Params.AdZoneId, this.<>f__this.Params.Reward);
                            this.<>f__this.AdLoadingText.text = string.Empty;
                            this.<>f__this.AdPlaceholderText.gameObject.SetActive(true);
                            this.<timer>__1 = new ManualTimer(1f);
                            goto Label_0372;
                        }
                        this.<timeoutTimer>__0 = new ManualTimer(10f);
                        break;

                    case 2:
                        break;

                    case 3:
                        goto Label_0372;

                    default:
                        goto Label_0430;
                }
                while (!Service.Binder.AdsSystem.adReady() && !this.<timeoutTimer>__0.Idle)
                {
                    this.<timeoutTimer>__0.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0432;
                }
                this.<>f__this.AdLoadingText.text = string.Empty;
                if (this.<timeoutTimer>__0.Idle)
                {
                    if (!ConfigApp.ProductionBuild)
                    {
                        UnityEngine.Debug.LogWarning("Ads timed out");
                    }
                    if (!App.Binder.ConfigMeta.REWARD_ON_VIDEO_AD_TIMOUT)
                    {
                        PlayerView.Binder.MenuSystem.instantCloseAllMenus();
                        this.<>f__this.m_videoEnded = true;
                    }
                    this.<>f__this.onAdTimedOut();
                }
                else
                {
                    this.<>f__this.m_videoEnded = false;
                    this.<>f__this.m_videoRewarded = false;
                    PlayerView.Binder.EventBus.AdWatchStarted(this.<>f__this.Params.AdZoneId, this.<>f__this.Params.Reward);
                    App.Binder.AppContext.startVideoAd();
                    data = new AdsData();
                    data.AdReward = this.<>f__this.Params.Reward;
                    data.AdZone = this.<>f__this.Params.AdZoneId;
                    data.AdCategory = this.<>f__this.Params.AdCategory;
                    SupersonicManager.Instance.ShowAdd(new SupersonicManager.ShowAddDelegate(this.<>f__this.onAdShowed), new System.Action(this.<>f__this.onAdEnded), data);
                }
                goto Label_0430;
            Label_0372:
                while (!this.<timer>__1.Idle)
                {
                    this.<>f__this.AdPlaceholderText.text = "<Video Ad>\n" + this.<timer>__1.timeRemaining().ToString("0") + "s";
                    this.<timer>__1.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0432;
                }
                this.<>f__this.AdPlaceholderText.text = "<Video Ad>\n";
                this.<>f__this.AdCloseButton.gameObject.SetActive(true);
                this.<>f__this.m_videoEnded = true;
                data = new AdsData();
                data.AdReward = this.<>f__this.Params.Reward;
                data.AdZone = this.<>f__this.Params.AdZoneId;
                data.AdCategory = this.<>f__this.Params.AdCategory;
                PlayerView.Binder.EventBus.AdWatched(new VideoResult(true, 1, "fake", data));
                goto Label_0430;
                this.$PC = -1;
            Label_0430:
                return false;
            Label_0432:
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

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public string AdZoneId;
            public AdsData.Category AdCategory;
            public GameLogic.Reward Reward;
            public Action<List<GameLogic.Reward>, bool, int> CompleteCallback;
        }
    }
}

