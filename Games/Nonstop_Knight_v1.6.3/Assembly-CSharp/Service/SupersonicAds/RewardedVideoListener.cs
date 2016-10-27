namespace Service.SupersonicAds
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    internal class RewardedVideoListener
    {
        [CompilerGenerated]
        private bool <InitSuccess>k__BackingField;
        [CompilerGenerated]
        private bool <VideoAvailable>k__BackingField;
        [CompilerGenerated]
        private bool <VideoIsPlaying>k__BackingField;
        private readonly VideoEndedDelegate endedCallback;
        private readonly VideoFinishedDelegate rewardedCallback;

        public RewardedVideoListener(VideoEndedDelegate _endedCallback, VideoFinishedDelegate _rewardedCallback)
        {
            this.endedCallback = _endedCallback;
            this.rewardedCallback = _rewardedCallback;
            SupersonicEvents.onRewardedVideoInitSuccessEvent += new Action(this.OnVideoInitSuccess);
            SupersonicEvents.onRewardedVideoInitFailEvent += new Action<SupersonicError>(this.OnVideoInitFail);
            SupersonicEvents.onRewardedVideoAdOpenedEvent += new Action(this.OnVideoStartPlaying);
            SupersonicEvents.onRewardedVideoAdRewardedEvent += new Action<SupersonicPlacement>(this.OnRewarded);
            SupersonicEvents.onRewardedVideoAdClosedEvent += new Action(this.OnVideoEndPlaying);
            SupersonicEvents.onVideoAvailabilityChangedEvent += new Action<bool>(this.OnVideoAvailabilityChanged);
        }

        private void OnEnded()
        {
            if (this.endedCallback != null)
            {
                this.endedCallback();
            }
        }

        private void OnRewarded(SupersonicPlacement _result)
        {
            Debug.Log("--- OnRewarded ---");
            this.OnRewarded(true, _result.getRewardAmount(), _result.getRewardName());
        }

        private void OnRewarded(bool _success, [Optional, DefaultParameterValue(-1)] int _amount, [Optional, DefaultParameterValue(null)] string _name)
        {
            if (this.rewardedCallback != null)
            {
                this.rewardedCallback(_success, _amount, _name);
            }
        }

        private void OnVideoAvailabilityChanged(bool _available)
        {
            Debug.Log("--- OnVideoAvailabilityChanged ---" + _available);
            this.VideoAvailable = _available;
        }

        private void OnVideoEndPlaying()
        {
            Debug.Log("--- OnVideoEndPlaying ---");
            this.VideoIsPlaying = false;
            this.OnEnded();
        }

        private void OnVideoInitFail(SupersonicError _error)
        {
            this.InitSuccess = false;
            Debug.Log("--- Init rewarded video failed ---");
            Debug.Log(_error.ToString());
        }

        private void OnVideoInitSuccess()
        {
            Debug.Log("--- OnVideoInitSuccess ---");
            this.InitSuccess = true;
        }

        private void OnVideoStartPlaying()
        {
            Debug.Log("--- OnVideoStartPlaying ---");
            this.VideoIsPlaying = true;
        }

        public void ShowRewardedVideo()
        {
            Supersonic.Agent.showRewardedVideo();
        }

        public bool InitSuccess
        {
            [CompilerGenerated]
            get
            {
                return this.<InitSuccess>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<InitSuccess>k__BackingField = value;
            }
        }

        public bool VideoAvailable
        {
            [CompilerGenerated]
            get
            {
                return this.<VideoAvailable>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<VideoAvailable>k__BackingField = value;
            }
        }

        public bool VideoIsPlaying
        {
            [CompilerGenerated]
            get
            {
                return this.<VideoIsPlaying>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<VideoIsPlaying>k__BackingField = value;
            }
        }

        public delegate void VideoEndedDelegate();

        public delegate void VideoFinishedDelegate(bool _success, int _amount, string _name);
    }
}

