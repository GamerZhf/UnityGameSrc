namespace App
{
    using GameLogic;
    using PlayerView;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class AppContext : Context, ICoroutineExecutor
    {
        [CompilerGenerated]
        private App.Splash <Splash>k__BackingField;
        public string DebugStartupDungeonId;
        private Coroutine m_hardResetRoutine;
        private bool m_initialFocusFlag;
        private bool m_initialPauseFlag;
        private long m_lastApplicationPauseTimestamp;
        private DeviceQualityType m_lastDeviceQualityType;
        private List<Context> m_otherContextRoots = new List<Context>();
        private bool m_watchedVideoAd;
        private QuitConfirmationPopup quitConfirmationPopup;
        public AppStartupMode StartupMode;

        Coroutine ICoroutineExecutor.StartCoroutine(IEnumerator routine)
        {
            return base.StartCoroutine(routine);
        }

        void ICoroutineExecutor.StopAllCoroutines()
        {
            base.StopAllCoroutines();
        }

        private static void ConfigureLibGC()
        {
        }

        [DebuggerHidden]
        private IEnumerator contextInitRoutine(bool createContexts, [Optional, DefaultParameterValue(null)] Player preloadedPlayer)
        {
            <contextInitRoutine>c__Iterator2D iteratord = new <contextInitRoutine>c__Iterator2D();
            iteratord.createContexts = createContexts;
            iteratord.preloadedPlayer = preloadedPlayer;
            iteratord.<$>createContexts = createContexts;
            iteratord.<$>preloadedPlayer = preloadedPlayer;
            iteratord.<>f__this = this;
            return iteratord;
        }

        private T createContextRoot<T>() where T: Component
        {
            GameObject target = new GameObject(typeof(T).ToString());
            UnityEngine.Object.DontDestroyOnLoad(target);
            return target.AddComponent<T>();
        }

        [ContextMenu("dispatchNavigateBack()")]
        protected void dispatchNavigateBack()
        {
            PlayerView.Binder.EventBus.NavigateBack();
        }

        [ContextMenu("hardReset()")]
        public void hardReset([Optional, DefaultParameterValue(null)] Player preloadedPlayer)
        {
            Assert.IsTrue_Release(!UnityUtils.CoroutineRunning(ref this.m_hardResetRoutine), "Hard-reset routine already running.");
            UnityUtils.StopCoroutine(this, ref this.m_hardResetRoutine);
            this.m_hardResetRoutine = UnityUtils.StartCoroutine(this, this.hardResetRoutine(preloadedPlayer));
        }

        [DebuggerHidden]
        private IEnumerator hardResetRoutine([Optional, DefaultParameterValue(null)] Player preloadedPlayer)
        {
            <hardResetRoutine>c__Iterator2E iteratore = new <hardResetRoutine>c__Iterator2E();
            iteratore.preloadedPlayer = preloadedPlayer;
            iteratore.<$>preloadedPlayer = preloadedPlayer;
            iteratore.<>f__this = this;
            return iteratore;
        }

        [DebuggerHidden]
        protected override IEnumerator mapBindings(bool allocatePersistentObjectPools)
        {
            <mapBindings>c__Iterator2C iteratorc = new <mapBindings>c__Iterator2C();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        public void OnApplicationFocus(bool focused)
        {
            if (Application.isEditor && !this.m_initialFocusFlag)
            {
                this.m_initialFocusFlag = true;
            }
            else if (Application.isEditor && ConfigApp.CHEAT_EDITOR_APP_FOCUS_SIMULATES_PAUSING)
            {
                this.OnApplicationPause(!focused);
            }
        }

        public void OnApplicationPause(bool paused)
        {
            if (Application.isEditor && !this.m_initialPauseFlag)
            {
                this.m_initialPauseFlag = true;
            }
            else if (this.systemsShouldReactToApplicationPause())
            {
                if (paused)
                {
                    this.m_lastApplicationPauseTimestamp = Service.Binder.ServerTime.GameTime;
                    if ((Service.Binder.AdsSystem == null) || !Service.Binder.AdsSystem.isShowing())
                    {
                        this.Splash.setVisible(true);
                    }
                }
                else
                {
                    long num = Service.Binder.ServerTime.GameTime - this.m_lastApplicationPauseTimestamp;
                    long num2 = !this.m_watchedVideoAd ? App.Binder.ConfigMeta.PASSIVE_COIN_GAIN_CEREMONY_COOLDOWN_SECONDS : App.Binder.ConfigMeta.ANDROID_VIDEO_AD_SESSION_TIMEOUT_SECONDS;
                    this.m_watchedVideoAd = false;
                    if (num > num2)
                    {
                        this.hardReset(null);
                    }
                    else
                    {
                        this.Splash.setVisible(false);
                    }
                }
            }
        }

        protected override void onAwake()
        {
            if (ConfigApp.ProductionBuild || ConfigApp.IsStableBuild())
            {
                if ((!ConfigApp.MigratePlayerProgressUponDataModelVersionChange || !ConfigApp.LocalNotificationsEnabled) || ((!ConfigApp.PersistentStorageEncryptionEnabled || !ConfigApp.SocialSystemEnabled) || ConfigApp.AssetBundlesEnabled))
                {
                    UnityEngine.Debug.LogError("Cannot start a production build with incorrect ConfigApp flags");
                    return;
                }
                if (ResourceUtil.LoadUnsafe<TextAsset>("RemoteContent/current_v" + ConfigApp.ProductionBuildDefaultRemoteContentVersion) == null)
                {
                    UnityEngine.Debug.LogError("No default remote content version JSON found: " + ConfigApp.ProductionBuildDefaultRemoteContentVersion);
                    return;
                }
                if (ConfigPromotionEvents.DEBUG_EVENT_ENABLED)
                {
                    UnityEngine.Debug.LogError("Cannot start a production build with the debug promotion event enabled");
                    return;
                }
            }
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            Screen.orientation = ScreenOrientation.Portrait;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;
            Screen.orientation = ScreenOrientation.AutoRotation;
            if (ConfigApp.CHEAT_MARKETING_MODE_ENABLED)
            {
                QualitySettings.antiAliasing = 8;
            }
            else
            {
                QualitySettings.antiAliasing = 0;
            }
            ConfigureLibGC();
            Layers.Initialize();
            this.refreshDeviceQuality();
            Screen.sleepTimeout = -1;
            this.m_lastApplicationPauseTimestamp = Service.Binder.ServerTime.GameTime;
            this.preMapBindings();
            this.quitConfirmationPopup = new QuitConfirmationPopup(this);
            this.Splash = App.Binder.AssetBundleLoader.instantiatePrefab("Prefabs/UI/Splash").GetComponent<App.Splash>();
            this.Splash.transform.SetParent(base.Tm, false);
            this.Splash.setVisible(true);
            base.StartCoroutine(this.contextInitRoutine(true, null));
        }

        protected override void onCleanup()
        {
        }

        private void onGameplayStarted(ActiveDungeon ad)
        {
            Screen.sleepTimeout = -2;
            this.Splash.updateProgressBar(1f);
            UnityEngine.Debug.Log("FETCHING STARTUP PARAMS");
            string[] startupParamsOrEmpty = StartUpParamsHelper.GetStartupParamsOrEmpty();
            for (int i = 0; i < startupParamsOrEmpty.Length; i++)
            {
                UnityEngine.Debug.Log(string.Concat(new object[] { "STARTUP PARAM ", i, ": ", startupParamsOrEmpty[i] }));
            }
        }

        private void onNavigateBack()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                MenuType type = PlayerView.Binder.MenuSystem.topmostActiveMenuType();
                if (PlayerView.Binder.InputSystem.InputEnabled && (type != MenuType.NONE))
                {
                    PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                }
                else
                {
                    this.onQuitRequested();
                }
            }
        }

        protected override void onPostInitialize()
        {
            GameLogic.Binder.EventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            App.Binder.EventBus.OnApplicationQuitRequested -= new App.Events.ApplicationQuitRequested(this.onQuitRequested);
            App.Binder.EventBus.OnApplicationQuitRequested += new App.Events.ApplicationQuitRequested(this.onQuitRequested);
            PlayerView.Binder.EventBus.OnNavigateBack -= new PlayerView.Events.NavigateBack(this.onNavigateBack);
            PlayerView.Binder.EventBus.OnNavigateBack += new PlayerView.Events.NavigateBack(this.onNavigateBack);
        }

        private void onQuitRequested()
        {
            this.quitConfirmationPopup.Show();
        }

        private void preMapBindings()
        {
            ConfigPerks.PreInitialize();
            App.Binder.AppContext = this;
            App.Binder.AppboyIOSBridge = new AppboyIOSBridge();
            App.Binder.BuildResources = new BuildResources();
            App.Binder.PersistentObjectRootTm = new GameObject("PersistentObjectRoot").transform;
            UnityEngine.Object.DontDestroyOnLoad(App.Binder.PersistentObjectRootTm);
            App.Binder.DynamicObjectRootTm = new GameObject("DynamicObjectRoot").transform;
            UnityEngine.Object.DontDestroyOnLoad(App.Binder.DynamicObjectRootTm);
            App.Binder.EventBus = new App.EventBus();
            App.Binder.AssetBundleLoader = base.createPersistentGameObject<AssetBundleLoader>(base.Tm);
            App.Binder.LocaSystem = new LocaSystem();
        }

        private void refreshDeviceQuality()
        {
            if (this.m_lastDeviceQualityType != ConfigDevice.DeviceQuality())
            {
                if (ConfigDevice.DeviceQuality() == DeviceQualityType.Low)
                {
                    Application.targetFrameRate = 30;
                    Time.fixedDeltaTime = 0.04f;
                    Time.maximumDeltaTime = 0.3f;
                }
                else if (ConfigDevice.DeviceQuality() == DeviceQualityType.Med)
                {
                    Application.targetFrameRate = 60;
                    Time.fixedDeltaTime = 0.02f;
                    Time.maximumDeltaTime = 0.2f;
                }
                else
                {
                    Application.targetFrameRate = 60;
                    Time.fixedDeltaTime = 0.02f;
                    Time.maximumDeltaTime = 0.2f;
                }
                this.m_lastDeviceQualityType = ConfigDevice.DeviceQuality();
            }
        }

        public void startVideoAd()
        {
            this.m_watchedVideoAd = true;
        }

        [ContextMenu("stressTest()")]
        private void stressTest()
        {
            base.StartCoroutine(this.stressTestRoutine());
        }

        [DebuggerHidden]
        private IEnumerator stressTestRoutine()
        {
            <stressTestRoutine>c__Iterator2F iteratorf = new <stressTestRoutine>c__Iterator2F();
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        public bool systemsShouldReactToApplicationPause()
        {
            if (Application.isEditor && !this.m_initialPauseFlag)
            {
                return false;
            }
            if (this.Splash == null)
            {
                return false;
            }
            if (UnityUtils.CoroutineRunning(ref this.m_hardResetRoutine))
            {
                return false;
            }
            if ((Service.Binder.ShopManager != null) && Service.Binder.ShopManager.IsPurchasing())
            {
                return false;
            }
            if ((App.Binder.NotificationRegister != null) && App.Binder.NotificationRegister.AskingInputFromPlayer)
            {
                return false;
            }
            return (((GameLogic.Binder.GameState != null) && (GameLogic.Binder.GameState.Player != null)) && ((GameLogic.Binder.GameState.ActiveDungeon != null) && (GameLogic.Binder.GameState.ActiveDungeon.CurrentGameplayState != GameplayState.UNDETERMINED)));
        }

        public App.Splash Splash
        {
            [CompilerGenerated]
            get
            {
                return this.<Splash>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Splash>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <contextInitRoutine>c__Iterator2D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>createContexts;
            internal Player <$>preloadedPlayer;
            internal AppContext <>f__this;
            internal Context <context>__2;
            internal Stopwatch <debugStopwatch>__0;
            internal Exception <e>__10;
            internal int <i>__11;
            internal int <i>__4;
            internal int <i>__6;
            internal IEnumerator <ie>__3;
            internal Stopwatch <loadStopwatch>__1;
            internal Transform[] <ourChildTms>__5;
            internal Player <player>__7;
            internal string <selectedLanguage>__8;
            internal SystemLanguage <systemLanguage>__9;
            internal bool createContexts;
            internal Player preloadedPlayer;

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
                        this.<debugStopwatch>__0 = DebugUtil.StartStopwatch();
                        this.<loadStopwatch>__1 = DebugUtil.StartStopwatch();
                        if (!this.createContexts)
                        {
                            this.<i>__4 = 0;
                            while (this.<i>__4 < this.<>f__this.m_otherContextRoots.Count)
                            {
                                this.<>f__this.m_otherContextRoots[this.<i>__4].gameObject.SetActive(true);
                                this.<ie>__3 = this.<>f__this.m_otherContextRoots[this.<i>__4].initialize(false);
                            Label_03F7:
                                while (this.<ie>__3.MoveNext())
                                {
                                    this.$current = this.<ie>__3.Current;
                                    this.$PC = 12;
                                    goto Label_077C;
                                }
                                this.<i>__4++;
                            }
                            this.<ourChildTms>__5 = TransformExtensions.GetChildren(this.<>f__this.Tm, false);
                            this.<i>__6 = 0;
                            while (this.<i>__6 < this.<ourChildTms>__5.Length)
                            {
                                this.<ourChildTms>__5[this.<i>__6].gameObject.SetActive(true);
                                this.<i>__6++;
                            }
                            goto Label_048C;
                        }
                        this.<>f__this.Splash.updateProgressBar(0.1f);
                        this.<context>__2 = UnityUtils.InstantiateGameObjectWithComponent<GameLogicContext>(null);
                        this.<ie>__3 = this.<context>__2.initialize(true);
                        break;

                    case 1:
                        break;

                    case 2:
                        if (ConfigApp.ProductionBuild)
                        {
                            goto Label_018F;
                        }
                        this.<context>__2 = UnityUtils.InstantiateGameObjectWithComponent<DebugViewContext>(null);
                        this.<ie>__3 = this.<context>__2.initialize(true);
                        goto Label_0156;

                    case 3:
                        goto Label_0156;

                    case 4:
                        goto Label_018F;

                    case 5:
                        goto Label_01E4;

                    case 6:
                        goto Label_024A;

                    case 7:
                        this.<>f__this.Splash.updateProgressBar(0.3f);
                        this.<context>__2 = UnityUtils.InstantiateGameObjectWithComponent<AiViewContext>(null);
                        this.<ie>__3 = this.<context>__2.initialize(true);
                        goto Label_02D8;

                    case 8:
                        goto Label_02D8;

                    case 9:
                        this.<ie>__3 = this.<>f__this.initialize(true);
                        goto Label_0347;

                    case 10:
                        goto Label_0347;

                    case 11:
                        this.<>f__this.Splash.updateProgressBar(0.4f);
                        goto Label_048C;

                    case 12:
                        goto Label_03F7;

                    case 13:
                        this.<>f__this.Splash.updateProgressBar(0.5f);
                        if (ConfigApp.ProductionBuild || !string.IsNullOrEmpty(this.<player>__7.Preferences.DevServerId))
                        {
                            goto Label_065E;
                        }
                        this.$current = this.<>f__this.StartCoroutine(this.<>f__this.Splash.selectDevServerRoutine(this.<player>__7, ConfigService.DevServerSelectionList));
                        this.$PC = 14;
                        goto Label_077C;

                    case 14:
                        goto Label_065E;

                    case 15:
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdInitializeGameState(this.<player>__7), 0f);
                        this.$PC = 0x10;
                        goto Label_077C;

                    case 0x10:
                        this.<>f__this.Splash.updateProgressBar(0.764f);
                        if (this.createContexts)
                        {
                            Service.Binder.TrackingSystem.sendGameLoadEvent(this.<player>__7, "completed", this.<loadStopwatch>__1.ElapsedMilliseconds);
                            App.Binder.NotificationRegister = new NotificationRegister();
                        }
                        this.$PC = -1;
                        goto Label_077A;

                    default:
                        goto Label_077A;
                }
                if (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 1;
                }
                else
                {
                    this.<>f__this.m_otherContextRoots.Add(this.<context>__2);
                    this.$current = null;
                    this.$PC = 2;
                }
                goto Label_077C;
            Label_0156:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 3;
                    goto Label_077C;
                }
                this.<>f__this.m_otherContextRoots.Add(this.<context>__2);
                this.$current = null;
                this.$PC = 4;
                goto Label_077C;
            Label_018F:
                this.<>f__this.Splash.updateProgressBar(0.2f);
                this.<context>__2 = UnityUtils.InstantiateGameObjectWithComponent<ServiceContext>(null);
                this.<ie>__3 = this.<context>__2.initialize(true);
            Label_01E4:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 5;
                    goto Label_077C;
                }
                this.<>f__this.m_otherContextRoots.Add(this.<context>__2);
                this.<context>__2 = UnityUtils.InstantiateGameObjectWithComponent<PlayerViewContext>(null);
                this.<ie>__3 = this.<context>__2.initialize(true);
            Label_024A:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 6;
                    goto Label_077C;
                }
                this.<>f__this.m_otherContextRoots.Add(this.<context>__2);
                this.$current = null;
                this.$PC = 7;
                goto Label_077C;
            Label_02D8:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 8;
                    goto Label_077C;
                }
                this.<>f__this.m_otherContextRoots.Add(this.<context>__2);
                this.$current = null;
                this.$PC = 9;
                goto Label_077C;
            Label_0347:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 10;
                    goto Label_077C;
                }
                this.$current = null;
                this.$PC = 11;
                goto Label_077C;
            Label_048C:
                this.<player>__7 = null;
                if (this.preloadedPlayer != null)
                {
                    this.<player>__7 = this.preloadedPlayer;
                }
                else
                {
                    this.<player>__7 = PlayerLoader.LoadExistingOrCreateNew();
                }
                if (!ConfigApp.ProductionBuild)
                {
                    this.<selectedLanguage>__8 = App.Binder.LocaSystem.selectedLanguage.ToString();
                    if ((this.<player>__7.Preferences.DevLanguageId != null) && (this.<player>__7.Preferences.DevLanguageId != this.<selectedLanguage>__8))
                    {
                        try
                        {
                            this.<systemLanguage>__9 = (SystemLanguage) ((int) Enum.Parse(typeof(SystemLanguage), this.<player>__7.Preferences.DevLanguageId));
                            UnityEngine.Debug.Log("Overriding LocaSystem selected language from " + this.<selectedLanguage>__8 + " to " + this.<player>__7.Preferences.DevLanguageId + ". Hard-resetting to take effect...");
                            App.Binder.LocaSystem.Initialize(this.<systemLanguage>__9);
                            this.<>f__this.hardReset(null);
                            goto Label_077A;
                        }
                        catch (Exception exception)
                        {
                            this.<e>__10 = exception;
                            UnityEngine.Debug.LogException(this.<e>__10);
                        }
                    }
                }
                this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdInitializePlayer(this.<player>__7), 0f);
                this.$PC = 13;
                goto Label_077C;
            Label_065E:
                this.<>f__this.Splash.updateProgressBar(0.618f);
                this.<i>__11 = 0;
                while (this.<i>__11 < this.<>f__this.m_otherContextRoots.Count)
                {
                    this.<>f__this.m_otherContextRoots[this.<i>__11].postInitialize();
                    this.<i>__11++;
                }
                this.<>f__this.postInitialize();
                this.$current = this.<>f__this.StartCoroutine(Service.Binder.ServiceContext.startupRoutine(this.<player>__7));
                this.$PC = 15;
                goto Label_077C;
            Label_077A:
                return false;
            Label_077C:
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
        private sealed class <hardResetRoutine>c__Iterator2E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Player <$>preloadedPlayer;
            internal AppContext <>f__this;
            internal Transform <childTm>__5;
            internal int <i>__0;
            internal int <i>__1;
            internal int <i>__2;
            internal int <i>__4;
            internal IEnumerator <ie>__6;
            internal Transform[] <ourChildTms>__3;
            internal Player preloadedPlayer;

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
                        this.<>f__this.Splash.setVisible(true);
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_026C;

                    case 1:
                        this.<i>__0 = 0;
                        while (this.<i>__0 < this.<>f__this.m_otherContextRoots.Count)
                        {
                            this.<>f__this.m_otherContextRoots[this.<i>__0].forceResetObjectPools();
                            this.<i>__0++;
                        }
                        TransformExtensions.DestroyChildren(App.Binder.DynamicObjectRootTm);
                        this.<i>__1 = this.<>f__this.m_otherContextRoots.Count - 1;
                        while (this.<i>__1 >= 0)
                        {
                            this.<>f__this.m_otherContextRoots[this.<i>__1].cleanup();
                            TransformExtensions.DestroyChildren(this.<>f__this.m_otherContextRoots[this.<i>__1].Tm);
                            this.<i>__1--;
                        }
                        this.<i>__2 = 0;
                        while (this.<i>__2 < this.<>f__this.m_otherContextRoots.Count)
                        {
                            this.<>f__this.m_otherContextRoots[this.<i>__2].gameObject.SetActive(false);
                            this.<i>__2++;
                        }
                        this.<ourChildTms>__3 = TransformExtensions.GetChildren(this.<>f__this.Tm, false);
                        this.<i>__4 = 0;
                        while (this.<i>__4 < this.<ourChildTms>__3.Length)
                        {
                            this.<childTm>__5 = this.<ourChildTms>__3[this.<i>__4];
                            if (this.<childTm>__5 != this.<>f__this.Splash.Tm)
                            {
                                this.<childTm>__5.gameObject.SetActive(false);
                            }
                            this.<i>__4++;
                        }
                        GC.Collect();
                        this.<>f__this.refreshDeviceQuality();
                        this.<ie>__6 = this.<>f__this.contextInitRoutine(false, this.preloadedPlayer);
                        break;

                    case 2:
                        break;

                    default:
                        goto Label_026A;
                }
                while (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 2;
                    goto Label_026C;
                }
                this.<>f__this.m_hardResetRoutine = null;
                goto Label_026A;
                this.$PC = -1;
            Label_026A:
                return false;
            Label_026C:
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
        private sealed class <mapBindings>c__Iterator2C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal AppContext <>f__this;

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
                    if (ConfigApp.LocalNotificationsEnabled)
                    {
                        App.Binder.LocalNotificationSystem = this.<>f__this.createPersistentGameObject<LocalNotificationSystem>(this.<>f__this.Tm);
                    }
                    App.Binder.SocialSystem = this.<>f__this.createPersistentGameObject<GooglePlaySocialSystem>(this.<>f__this.Tm);
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
        private sealed class <stressTestRoutine>c__Iterator2F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal AppContext <>f__this;
            internal int <allObjects>__1;
            internal string <debugStr>__3;
            internal int <i>__0;
            internal int <objPoolChildren>__2;

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
                        this.<i>__0 = 0;
                        goto Label_01DD;

                    case 1:
                        break;

                    case 2:
                        goto Label_008A;

                    case 3:
                        goto Label_00B7;

                    case 4:
                        this.<i>__0++;
                        goto Label_01DD;

                    default:
                        goto Label_01F6;
                }
            Label_0051:
                if (UnityUtils.CoroutineRunning(ref this.<>f__this.m_hardResetRoutine))
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01F8;
                }
                this.<>f__this.hardReset(null);
            Label_008A:
                while (UnityUtils.CoroutineRunning(ref this.<>f__this.m_hardResetRoutine))
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01F8;
                }
            Label_00B7:
                while (((GameLogic.Binder.GameState == null) || (GameLogic.Binder.GameState.ActiveDungeon == null)) || (GameLogic.Binder.GameState.ActiveDungeon.CurrentGameplayState != GameplayState.START_CEREMONY_STEP1))
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_01F8;
                }
                this.<allObjects>__1 = UnityEngine.Object.FindObjectsOfType<Transform>().Length;
                this.<objPoolChildren>__2 = App.Binder.PersistentObjectRootTm.GetComponentsInChildren<Transform>(true).Length;
                object[] objArray1 = new object[] { "All objects: ", this.<allObjects>__1, "\n", "Obj pool children: ", this.<objPoolChildren>__2, "\n", "Profiler.usedHeapSize: ", Profiler.usedHeapSize, "\n", "Profiler.GetMonoHeapSize: ", Profiler.GetMonoHeapSize(), "\n", "Profiler.GetMonoUsedSize: ", Profiler.GetMonoUsedSize() };
                this.<debugStr>__3 = string.Concat(objArray1);
                UnityEngine.Debug.Log("MEMORY STATS:\n" + this.<debugStr>__3);
                this.$current = null;
                this.$PC = 4;
                goto Label_01F8;
            Label_01DD:
                if (this.<i>__0 < 10)
                {
                    goto Label_0051;
                }
                goto Label_01F6;
                this.$PC = -1;
            Label_01F6:
                return false;
            Label_01F8:
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

        public enum AppStartupMode
        {
            DEFAULT,
            DEBUG_DUNGEON
        }
    }
}

