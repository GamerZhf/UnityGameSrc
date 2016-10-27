namespace Service
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ServiceContext : Context, ICoroutineExecutor
    {
        [CompilerGenerated]
        private float <TotalKbsReceived>k__BackingField;
        [CompilerGenerated]
        private float <TotalKbsSent>k__BackingField;

        Coroutine ICoroutineExecutor.StartCoroutine(IEnumerator routine)
        {
            return base.StartCoroutine(routine);
        }

        void ICoroutineExecutor.StopAllCoroutines()
        {
            base.StopAllCoroutines();
        }

        [DebuggerHidden]
        protected override IEnumerator mapBindings(bool allocatePersistentObjectPools)
        {
            <mapBindings>c__Iterator232 iterator = new <mapBindings>c__Iterator232();
            iterator.<>f__this = this;
            return iterator;
        }

        protected override void onCleanup()
        {
            Service.Binder.Logger.Log("Cleanup Service Context");
            Service.Binder.TaskManager.StopAllCoroutines();
            Service.Binder.ShopManager.UnInitialize();
            Service.Binder.PromotionManager.UnInitialize();
            Service.Binder.PromotionService.UnInitialize();
            base.destroyPersistentGameObject<PromotionEventSystem>((PromotionEventSystem) Service.Binder.PromotionEventSystem);
            Service.Binder.PromotionEventSystem = null;
            if (Service.Binder.NSKRTLManager != null)
            {
                Service.Binder.NSKRTLManager.UnInitialize();
            }
        }

        [DebuggerHidden]
        public IEnumerator startupRoutine(Player player)
        {
            <startupRoutine>c__Iterator233 iterator = new <startupRoutine>c__Iterator233();
            iterator.player = player;
            iterator.<$>player = player;
            iterator.<>f__this = this;
            return iterator;
        }

        [ContextMenu("testTriggerClientUpdate()")]
        public void testTriggerClientUpdate()
        {
            Service.Binder.PlayerService.PendingClientUpdateFromUrl = "http://test";
        }

        public float TotalKbsReceived
        {
            [CompilerGenerated]
            get
            {
                return this.<TotalKbsReceived>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<TotalKbsReceived>k__BackingField = value;
            }
        }

        public float TotalKbsSent
        {
            [CompilerGenerated]
            get
            {
                return this.<TotalKbsSent>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<TotalKbsSent>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <mapBindings>c__Iterator232 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ServiceContext <>f__this;

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
                        Service.Binder.LoggingService = this.<>f__this.createPersistentGameObject<LoggingService>(this.<>f__this.Tm);
                        Service.Binder.ServiceContext = this.<>f__this;
                        Service.Binder.ServerSelection = new ServerSelection();
                        Service.Binder.TaskManager = this.<>f__this.createPersistentGameObject<TaskManager>(this.<>f__this.Tm);
                        Service.Binder.EventBus = new Service.EventBus();
                        Service.Binder.SessionData = new SessionData();
                        Service.Binder.ContentService = new ContentService();
                        Service.Binder.InboxProcessor = this.<>f__this.createPersistentGameObject<InboxProcessor>(this.<>f__this.Tm);
                        Service.Binder.SdkController = new SdkController();
                        Service.Binder.TrackingService = this.<>f__this.createPersistentGameObject<TrackingService>(this.<>f__this.Tm);
                        Service.Binder.ServiceWatchdog = this.<>f__this.createPersistentGameObject<ServiceWatchdog>(this.<>f__this.Tm);
                        Service.Binder.ShopService = new ShopService();
                        Service.Binder.ShopManager = new ShopManager();
                        Service.Binder.FacebookAdapter = this.<>f__this.createPersistentGameObject<FacebookAdapter>(this.<>f__this.Tm);
                        Service.Binder.LeaderboardUpdateService = this.<>f__this.createPersistentGameObject<LeaderboardUpdateService>(this.<>f__this.Tm);
                        Service.Binder.LeaderboardService = new LeaderboardService();
                        Service.Binder.PromotionService = this.<>f__this.createPersistentGameObject<PromotionService>(this.<>f__this.Tm);
                        Service.Binder.AchievementThirdPartyService = new AchievementThirdPartyService();
                        Service.Binder.TournamentService = new TournamentService();
                        this.$current = null;
                        this.$PC = 1;
                        return true;

                    case 1:
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

        [CompilerGenerated]
        private sealed class <startupRoutine>c__Iterator233 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Player <$>player;
            internal ServiceContext <>f__this;
            internal Player player;

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
                    Service.Binder.TrackingSystem = this.<>f__this.createPersistentGameObject<TrackingSystem>(this.<>f__this.Tm);
                    Service.Binder.ServerSelection.Initialize(this.player);
                    Service.Binder.ContentService.InitializeContent();
                    Service.Binder.SdkController.Initialize();
                    Service.Binder.PromotionManager.Initialize();
                    Service.Binder.InboxProcessor.Initialize();
                    Service.Binder.PlayerService = this.<>f__this.createPersistentGameObject<PlayerService>(this.<>f__this.Tm);
                    Service.Binder.PlayerService.StartPlayerService();
                    Service.Binder.TrackingService.StartTrackingService();
                    Service.Binder.AdsSystem = this.<>f__this.createPersistentGameObject<AdsSystem>(this.<>f__this.Tm);
                    Service.Binder.TaskManager.StartTask(Service.Binder.FacebookAdapter.Initialize(), null);
                    App.Binder.AppboyIOSBridge.Bridge.RequestInAppMessageAsync();
                    Service.Binder.ServiceWatchdog.StartServiceWatchdog();
                    Service.Binder.PromotionService.Initialize();
                    Service.Binder.TournamentSystem = this.<>f__this.createPersistentGameObject<TournamentSystem>(this.<>f__this.Tm);
                    Service.Binder.RemoteNotificationSystem = this.<>f__this.createPersistentGameObject<RemoteNotificationSystem>(this.<>f__this.Tm);
                    Service.Binder.PromotionEventSystem = this.<>f__this.createPersistentGameObject<PromotionEventSystem>(this.<>f__this.Tm);
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

