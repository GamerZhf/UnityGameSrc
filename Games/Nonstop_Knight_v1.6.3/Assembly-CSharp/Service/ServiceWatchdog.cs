namespace Service
{
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class ServiceWatchdog : MonoBehaviour
    {
        private bool m_online;
        private bool m_shopUnlocked;
        private float nextLocalLbUpdate = (Time.realtimeSinceStartup + ConfigService.LEADERBOARD_LOCAL_REFRESH);
        private float nextSocialLbUpdate = (Time.realtimeSinceStartup + ConfigService.LEADERBOARD_SOCIAL_REFRESH);

        private void CheckShopInit()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if ((!this.m_shopUnlocked && (player != null)) && ((player.shopUnlocked() && Service.Binder.PlayerService.IsLoggedIn) && this.IsOnline))
            {
                Service.Binder.TaskManager.StartTask(Service.Binder.ShopManager.Initialize(), null);
                this.m_shopUnlocked = true;
            }
        }

        private void Log(string _msg)
        {
            Service.Binder.Logger.Log(_msg);
        }

        protected void OnApplicationPause(bool paused)
        {
            if (paused)
            {
                Service.Binder.EventBus.NetworkStateChanged(false);
            }
        }

        private void OnDisable()
        {
            Service.Binder.EventBus.OnNetworkStateChanged -= new Service.Events.NetworkStateChanged(this.onNetworkStateChanged);
            Service.Binder.EventBus.OnPlayerLoggedIn -= new Service.Events.PlayerLoggedIn(this.CheckShopInit);
            Service.Binder.EventBus.OnPlayerRegistered -= new Service.Events.PlayerRegistered(this.CheckShopInit);
            GameLogic.Binder.EventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.CheckShopInit);
            GameLogic.Binder.EventBus.OnPlayerRetired -= new GameLogic.Events.PlayerRetired(this.TriggerCheckShop2);
            GameLogic.Binder.EventBus.OnTutorialCompleted -= new GameLogic.Events.TutorialCompleted(this.TriggerCheckShop);
            GameLogic.Binder.EventBus.OnRoomCompleted -= new GameLogic.Events.RoomCompleted(this.TriggerCheckShop2);
        }

        private void OnEnable()
        {
            Service.Binder.EventBus.OnNetworkStateChanged += new Service.Events.NetworkStateChanged(this.onNetworkStateChanged);
            Service.Binder.EventBus.OnPlayerLoggedIn += new Service.Events.PlayerLoggedIn(this.CheckShopInit);
            Service.Binder.EventBus.OnPlayerRegistered += new Service.Events.PlayerRegistered(this.CheckShopInit);
            GameLogic.Binder.EventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.CheckShopInit);
            GameLogic.Binder.EventBus.OnPlayerRetired += new GameLogic.Events.PlayerRetired(this.TriggerCheckShop2);
            GameLogic.Binder.EventBus.OnTutorialCompleted += new GameLogic.Events.TutorialCompleted(this.TriggerCheckShop);
            GameLogic.Binder.EventBus.OnRoomCompleted += new GameLogic.Events.RoomCompleted(this.TriggerCheckShop2);
        }

        private void onNetworkStateChanged(bool online)
        {
            this.Log("network state changed:" + online);
            this.m_online = online;
            if (this.IsOnline && (Service.Binder.ShopManager.State == ShopManagerState.Unavailable))
            {
                this.Log("Initialize Shop");
                Service.Binder.TaskManager.StartTask(Service.Binder.ShopManager.Initialize(), null);
            }
        }

        public bool OnWatchdogException(Exception ex)
        {
            UnityEngine.Debug.Log(ex);
            return true;
        }

        public void StartServiceWatchdog()
        {
            Service.Binder.TaskManager.StartTask(this.Watchdog(), new TaskManager.ExceptionActionDelegate(this.OnWatchdogException));
        }

        private void TriggerCheckShop([Optional, DefaultParameterValue(null)] object dummyObj, [Optional, DefaultParameterValue(null)] object dummyObj2)
        {
            this.CheckShopInit();
        }

        private void TriggerCheckShop2([Optional, DefaultParameterValue(null)] object dummyObj, [Optional, DefaultParameterValue(0)] int dummyObj2)
        {
            this.CheckShopInit();
        }

        [DebuggerHidden]
        private IEnumerator Watchdog()
        {
            <Watchdog>c__Iterator234 iterator = new <Watchdog>c__Iterator234();
            iterator.<>f__this = this;
            return iterator;
        }

        public bool IsOnline
        {
            get
            {
                return this.m_online;
            }
        }

        [CompilerGenerated]
        private sealed class <Watchdog>c__Iterator234 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ServiceWatchdog <>f__this;
            internal float <checkWait>__3;
            internal List<string> <friends>__6;
            internal IEnumerator <ie>__4;
            internal float <maxRetryWait>__2;
            internal Request<string> <req>__5;
            internal float <retryWait>__1;
            internal float <retryWaitInitial>__0;

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
                        this.<retryWaitInitial>__0 = 2f;
                        this.<retryWait>__1 = this.<retryWaitInitial>__0;
                        this.<maxRetryWait>__2 = 30f;
                        this.<checkWait>__3 = 10f;
                        this.<ie>__4 = TimeUtil.WaitForUnscaledSeconds(0.1f);
                        break;

                    case 1:
                        break;

                    case 2:
                        if (this.<req>__5.Success)
                        {
                            goto Label_0181;
                        }
                        this.<>f__this.Log("next offline status ping: " + this.<retryWait>__1);
                        this.<ie>__4 = TimeUtil.WaitForUnscaledSeconds(this.<checkWait>__3);
                        goto Label_014F;

                    case 3:
                        goto Label_014F;

                    case 4:
                        this.<>f__this.nextSocialLbUpdate = Time.realtimeSinceStartup + ConfigService.LEADERBOARD_SOCIAL_REFRESH;
                        goto Label_0209;

                    case 5:
                        this.<>f__this.nextLocalLbUpdate = Time.realtimeSinceStartup + ConfigService.LEADERBOARD_LOCAL_REFRESH;
                        goto Label_0261;

                    case 6:
                        goto Label_02A0;

                    default:
                        goto Label_02BC;
                }
                if (this.<ie>__4.MoveNext())
                {
                    this.$current = this.<ie>__4.Current;
                    this.$PC = 1;
                    goto Label_02BE;
                }
            Label_00A4:
                this.<>f__this.CheckShopInit();
                if (this.<>f__this.m_online)
                {
                    goto Label_0181;
                }
                this.<req>__5 = Request<string>.Alive("/alive");
                this.$current = this.<req>__5.Task;
                this.$PC = 2;
                goto Label_02BE;
            Label_014F:
                if (this.<ie>__4.MoveNext())
                {
                    this.$current = this.<ie>__4.Current;
                    this.$PC = 3;
                    goto Label_02BE;
                }
                this.<retryWait>__1 = Mathf.Min(this.<maxRetryWait>__2, this.<retryWait>__1 * 1.9f);
                goto Label_00A4;
            Label_0181:
                if ((this.<>f__this.m_online && (this.<>f__this.nextSocialLbUpdate < Time.realtimeSinceStartup)) && Service.Binder.FacebookAdapter.IsLoggedIn())
                {
                    this.<friends>__6 = new List<string>(Service.Binder.FacebookAdapter.Friends.Keys);
                    this.$current = Service.Binder.LeaderboardService.LoadFriendsLeaderboard(PlatformConnectType.Facebook, this.<friends>__6);
                    this.$PC = 4;
                    goto Label_02BE;
                }
            Label_0209:
                if (this.<>f__this.m_online && (this.<>f__this.nextLocalLbUpdate < Time.realtimeSinceStartup))
                {
                    this.$current = Service.Binder.LeaderboardService.LoadLocalLeaderboard();
                    this.$PC = 5;
                    goto Label_02BE;
                }
            Label_0261:
                this.<retryWait>__1 = this.<retryWaitInitial>__0;
                this.<ie>__4 = TimeUtil.WaitForUnscaledSeconds(this.<checkWait>__3);
            Label_02A0:
                while (this.<ie>__4.MoveNext())
                {
                    this.$current = this.<ie>__4.Current;
                    this.$PC = 6;
                    goto Label_02BE;
                }
                goto Label_00A4;
                this.$PC = -1;
            Label_02BC:
                return false;
            Label_02BE:
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

