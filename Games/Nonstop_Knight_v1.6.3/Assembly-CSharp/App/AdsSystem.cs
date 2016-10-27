namespace App
{
    using GameLogic;
    using PlayerView;
    using Service;
    using Service.SupersonicAds;
    using System;
    using UnityEngine;

    public class AdsSystem : MonoBehaviour, IAdsSystem
    {
        public static string ADS_DEFAULT_ZONE = "defaultZone";
        public static string ADS_VENDOR_ZONE = "vendorZone";

        public bool adReady()
        {
            if (Application.isEditor)
            {
                return true;
            }
            if (!AdsSupported())
            {
                return false;
            }
            return ((Service.Binder.ServiceWatchdog.IsOnline && (Application.internetReachability != NetworkReachability.NotReachable)) && SupersonicManager.Instance.IsVideoReady());
        }

        public static bool AdsSupported()
        {
            return ((Application.platform == RuntimePlatform.IPhonePlayer) || (Application.platform == RuntimePlatform.Android));
        }

        protected void Awake()
        {
        }

        protected void FixedUpdate()
        {
            if (GameLogic.Binder.GameState != null)
            {
                Player player = GameLogic.Binder.GameState.Player;
                if (player != null)
                {
                    long num = Service.Binder.ServerTime.GameTime - player.LastDailyAdCountResetTimestamp;
                    if (num > 0x15180L)
                    {
                        player.DailyAdCountMystery = 0;
                        player.DailyAdCountVendor = 0;
                        if (player.Tournaments.hasTournamentSelected())
                        {
                            player.Tournaments.SelectedTournament.DailyAdCountMystery = 0;
                            player.Tournaments.SelectedTournament.DailyAdCountCards = 0;
                        }
                        this.updateResetTimestamp(player);
                    }
                }
            }
        }

        private string GetAppKey()
        {
            string str = ConfigSdk.SUPERSONIC_DEV_IOS_KEY;
            return (!ConfigApp.ProductionBuild ? ConfigSdk.SUPERSONIC_DEV_ANDROID_KEY : ConfigSdk.SUPERSONIC_LIVE_ANDROID_KEY);
        }

        public bool initialized()
        {
            return (Application.isEditor || SupersonicManager.Instance.Initialized);
        }

        public bool isShowing()
        {
            return SupersonicManager.Instance.IsVideoShowing();
        }

        private void onAdWatched(VideoResult result)
        {
            if (result.Success)
            {
                AdsData customData = (AdsData) result.CustomData;
                Player player = GameLogic.Binder.GameState.Player;
                if (customData.AdCategory == AdsData.Category.VENDOR)
                {
                    player.DailyAdCountVendor++;
                }
                else if (customData.AdCategory == AdsData.Category.ADVENTURE_MYSTERY)
                {
                    player.DailyAdCountMystery++;
                }
                else if (customData.AdCategory == AdsData.Category.TOURNAMENT_MYSTERY)
                {
                    if (player.Tournaments.hasTournamentSelected())
                    {
                        TournamentInstance selectedTournament = player.Tournaments.SelectedTournament;
                        selectedTournament.DailyAdCountMystery++;
                    }
                }
                else if ((customData.AdCategory == AdsData.Category.TOURNAMENT_CARDS) && player.Tournaments.hasTournamentSelected())
                {
                    TournamentInstance instance2 = player.Tournaments.SelectedTournament;
                    instance2.DailyAdCountCards++;
                }
            }
        }

        protected void OnApplicationPause(bool paused)
        {
        }

        protected void OnDisable()
        {
            PlayerView.Binder.EventBus.OnAdWatched -= new PlayerView.Events.AdWatched(this.onAdWatched);
            Service.Binder.EventBus.OnPlayerRegistered -= new Service.Events.PlayerRegistered(this.OnFGUserHandleAvailable);
            Service.Binder.EventBus.OnPlayerLoggedIn -= new Service.Events.PlayerLoggedIn(this.OnFGUserHandleAvailable);
            GameLogic.Binder.EventBus.OnSuspectedSystemClockCheat -= new GameLogic.Events.SuspectedSystemClockCheat(this.onSuspectedSystemClockCheat);
        }

        protected void OnEnable()
        {
            PlayerView.Binder.EventBus.OnAdWatched += new PlayerView.Events.AdWatched(this.onAdWatched);
            Service.Binder.EventBus.OnPlayerRegistered += new Service.Events.PlayerRegistered(this.OnFGUserHandleAvailable);
            Service.Binder.EventBus.OnPlayerLoggedIn += new Service.Events.PlayerLoggedIn(this.OnFGUserHandleAvailable);
            GameLogic.Binder.EventBus.OnSuspectedSystemClockCheat += new GameLogic.Events.SuspectedSystemClockCheat(this.onSuspectedSystemClockCheat);
        }

        private void OnFGUserHandleAvailable()
        {
            SupersonicManager.Instance.Init(this.GetAppKey(), GameLogic.Binder.GameState.Player.FgUserHandle);
        }

        private void onSuspectedSystemClockCheat(long timeOffsetSeconds)
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.updateResetTimestamp(player);
        }

        private void updateResetTimestamp(Player player)
        {
            DateTime time = TimeUtil.UnixTimestampToDateTime(Service.Binder.ServerTime.GameTime);
            DateTime dateTime = new DateTime(time.Year, time.Month, time.Day);
            player.LastDailyAdCountResetTimestamp = TimeUtil.DateTimeToUnixTimestamp(dateTime);
        }
    }
}

