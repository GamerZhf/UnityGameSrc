namespace Service.SupersonicAds
{
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class SupersonicManager : MonoBehaviour
    {
        [CompilerGenerated]
        private bool <Initialized>k__BackingField;
        private static SupersonicManager instance;
        private bool rewarded;
        private object rewardedCustomData;
        private readonly RewardedVideoListener rewardedVideoListener;
        private Action videoEndedDelegate;
        private ShowAddDelegate videoRewardedDelegate;
        private const float WAIT_FOR_PLAYING_VIDEO_SEC = 1f;
        private const float WAIT_FOR_REWARD_TIMEOUT_SEC = 3f;

        private SupersonicManager()
        {
            this.rewardedVideoListener = new RewardedVideoListener(new RewardedVideoListener.VideoEndedDelegate(this.OnVideoEnded), new RewardedVideoListener.VideoFinishedDelegate(this.OnRewardedVideoFinished));
        }

        protected void Awake()
        {
            Supersonic.Agent.start();
        }

        public void Init(string _appKey, string _userId)
        {
            if (!this.Initialized)
            {
                Supersonic.Agent.initRewardedVideo(_appKey, _userId);
                this.Initialized = true;
            }
        }

        public bool IsAvailable()
        {
            return Supersonic.Agent.isRewardedVideoAvailable();
        }

        public bool IsVideoReady()
        {
            return this.rewardedVideoListener.VideoAvailable;
        }

        public bool IsVideoShowing()
        {
            return this.rewardedVideoListener.VideoIsPlaying;
        }

        protected void OnApplicationPause(bool _isPaused)
        {
            if (_isPaused)
            {
                Supersonic.Agent.onPause();
            }
            else
            {
                Supersonic.Agent.onResume();
            }
        }

        private void OnRewardedVideoFinished(bool _success, int _amount, string _name)
        {
            this.rewarded = true;
            UnityEngine.Debug.Log(string.Concat(new object[] { "SupersonicManager::OnRewardedVideoFinished - success:", _success, " amount:", _amount, " name:", _name, " customDelegate set:", this.videoRewardedDelegate != null }));
            if (this.videoRewardedDelegate != null)
            {
                this.videoRewardedDelegate(new VideoResult(_success, _amount, _name, this.rewardedCustomData));
            }
        }

        [DebuggerHidden]
        private IEnumerator OnRewardTimeout()
        {
            <OnRewardTimeout>c__Iterator2B iteratorb = new <OnRewardTimeout>c__Iterator2B();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        private void OnVideoEnded()
        {
            UnityUtils.StartCoroutine(this, this.OnRewardTimeout());
            if (this.videoEndedDelegate != null)
            {
                this.videoEndedDelegate();
            }
            UnityEngine.Debug.Log("SupersonicManager::OnVideoEnded - timeout for reward:" + 3f);
        }

        [DebuggerHidden]
        private IEnumerator PlayVideoDelayed()
        {
            <PlayVideoDelayed>c__Iterator2A iteratora = new <PlayVideoDelayed>c__Iterator2A();
            iteratora.<>f__this = this;
            return iteratora;
        }

        public void ShowAdd(ShowAddDelegate _onRewarded, Action _onVideoEnded, [Optional, DefaultParameterValue(null)] object _rewardedCustomData)
        {
            if (!this.IsAvailable() || !this.Initialized)
            {
                _onRewarded(new VideoResult(false, PlayVideoError.SdkNotReady));
            }
            else if (this.rewardedVideoListener.VideoIsPlaying)
            {
                _onRewarded(new VideoResult(false, PlayVideoError.VideoStillPlaying));
            }
            else if (!Binder.ServiceWatchdog.IsOnline || (Application.internetReachability == NetworkReachability.NotReachable))
            {
                _onRewarded(new VideoResult(false, PlayVideoError.NoNetwork));
            }
            else
            {
                this.rewarded = false;
                this.rewardedCustomData = _rewardedCustomData;
                this.videoRewardedDelegate = _onRewarded;
                this.videoEndedDelegate = _onVideoEnded;
                UnityUtils.StartCoroutine(this, this.PlayVideoDelayed());
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

        public static SupersonicManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject target = new GameObject("SupersonicManager");
                    instance = target.AddComponent<SupersonicManager>();
                    target.AddComponent<SupersonicEvents>();
                    UnityEngine.Object.DontDestroyOnLoad(target);
                }
                return instance;
            }
        }

        [CompilerGenerated]
        private sealed class <OnRewardTimeout>c__Iterator2B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal SupersonicManager <>f__this;
            internal IEnumerator <ie>__0;

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
                        UnityEngine.Debug.Log("SupersonicManager::OnRewardTimeout - WaitForSeconds:" + 3f);
                        this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(3f);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00C9;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    return true;
                }
                UnityEngine.Debug.Log("SupersonicManager::OnRewardTimeout - did we get rewarded and should not finish with timeout?:" + this.<>f__this.rewarded);
                if (!this.<>f__this.rewarded)
                {
                    this.<>f__this.OnRewardedVideoFinished(false, -1, "timeout");
                    this.$PC = -1;
                }
            Label_00C9:
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
        private sealed class <PlayVideoDelayed>c__Iterator2A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal SupersonicManager <>f__this;
            internal IEnumerator <ie>__0;

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
                        UnityEngine.Debug.Log("SupersonicManager::OnRewardTimeout - WaitForSeconds:" + 3f);
                        this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(1f);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0093;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    return true;
                }
                this.<>f__this.rewardedVideoListener.ShowRewardedVideo();
                this.$PC = -1;
            Label_0093:
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

        public delegate void ShowAddDelegate(VideoResult _result);
    }
}

