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

    public class MenuSystem : MonoBehaviour
    {
        [CompilerGenerated]
        private PlayerView.CharacterMenuCamera <CharacterMenuCamera>k__BackingField;
        [CompilerGenerated]
        private bool <InTransition>k__BackingField;
        [CompilerGenerated]
        private Camera <MenuCamera>k__BackingField;
        [CompilerGenerated]
        private MenuCharacterView <MenuHero>k__BackingField;
        [CompilerGenerated]
        private MenuCharacterView <MenuPet>k__BackingField;
        [CompilerGenerated]
        private Material <ReferenceMenuLightingMaterial>k__BackingField;
        [CompilerGenerated]
        private MenuType <TransitioningToMenuType>k__BackingField;
        private Stack<Menu> m_activeMenuStack = new Stack<Menu>();
        private CharacterView m_menuHeroCharacterView;
        private CharacterView m_menuPetCharacterView;
        private List<Menu> m_menus = new List<Menu>();
        private List<PrewarmedMenuEntry> m_preWarmedMenus = new List<PrewarmedMenuEntry>();

        protected void Awake()
        {
            GameObject obj2 = ResourceUtil.Instantiate<GameObject>("Prefabs/Menu/MenuCamera");
            obj2.transform.SetParent(base.transform, false);
            this.MenuCamera = obj2.GetComponent<Camera>();
            this.MenuCamera.depth = 5f;
            this.MenuCamera.name = "MenuCamera";
            this.MenuCamera.enabled = false;
            obj2 = ResourceUtil.Instantiate<GameObject>("Prefabs/Menu/CharacterMenuCamera");
            obj2.transform.SetParent(base.transform, false);
            this.CharacterMenuCamera = obj2.GetComponent<PlayerView.CharacterMenuCamera>();
            this.CharacterMenuCamera.Camera.enabled = false;
            GameObject obj3 = new GameObject("HeroCharacterMenuView");
            obj3.transform.SetParent(base.transform, false);
            this.MenuHero = obj3.AddComponent<MenuCharacterView>();
            obj3 = new GameObject("PetCharacterMenuView");
            obj3.transform.SetParent(base.transform, false);
            this.MenuPet = obj3.AddComponent<MenuCharacterView>();
            this.m_menus.Add(this.createMenuObject<SpeechBubbleMenu>("Prefabs/Menu/SpeechBubbleMenu"));
            this.m_menus.Add(this.createMenuObject<MiniPopupMenu>("Prefabs/Menu/MiniPopupMenu"));
            this.m_menus.Add(this.createMenuObject<TooltipMenu>("Prefabs/Menu/TooltipMenu"));
            this.m_menus.Add(this.createMenuObject<SlidingInventoryMenu>("Prefabs/Menu/SlidingInventoryMenu"));
            this.m_menus.Add(this.createMenuObject<FullscreenAdMenu>("Prefabs/Menu/FullscreenAdMenu"));
            this.m_menus.Add(this.createMenuObject<RewardCeremonyMenu>("Prefabs/Menu/RewardCeremonyMenu"));
            this.m_menus.Add(this.createMenuObject<ThinPopupMenu>("Prefabs/Menu/ThinPopupMenu"));
            this.m_menus.Add(this.createMenuObject<TechPopupMenu>("Prefabs/Menu/TechPopupMenu"));
            this.m_menus.Add(this.createMenuObject<StackedPopupMenu>("Prefabs/Menu/StackedPopupMenu"));
            this.m_menus.Add(this.createMenuObject<MessagePopupMenu>("Prefabs/Menu/MessagePopupMenu"));
            this.m_menus.Add(this.createMenuObject<LocationEndCeremonyMenu>("Prefabs/Menu/LocationEndCeremonyMenu"));
            this.m_menus.Add(this.createMenuObject<SlidingTaskPanel>("Prefabs/Menu/SlidingTaskPanel"));
            this.m_menus.Add(this.createMenuObject<SlidingAdventurePanel>("Prefabs/Menu/SlidingAdventurePanel"));
            this.ReferenceMenuLightingMaterial = new Material(Shader.Find("CUSTOM/Menu_Character_DiffuseWrap_Backfaces"));
            this.ReferenceMenuLightingMaterial.SetColor("_AmbientColor", (Color) (Color.white * 0.4f));
            this.ReferenceMenuLightingMaterial.SetColor("_LightColor", (Color) (Color.white * 0.6f));
            this.ReferenceMenuLightingMaterial.SetVector("_LightPosWorldSpace", (Vector4) (Quaternion.Euler(14.8f, 233.5f, 0f) * Vector3.one));
        }

        public Coroutine closeAllMenusAndTransitionToNewMenu(MenuType menuType, MenuContentType menuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            return base.StartCoroutine(this.closeAllMenusAndTransitionToNewMenuRoutine(menuType, menuContentType, parameter));
        }

        [DebuggerHidden]
        private IEnumerator closeAllMenusAndTransitionToNewMenuRoutine(MenuType menuType, MenuContentType menuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <closeAllMenusAndTransitionToNewMenuRoutine>c__Iterator13E iteratore = new <closeAllMenusAndTransitionToNewMenuRoutine>c__Iterator13E();
            iteratore.menuType = menuType;
            iteratore.menuContentType = menuContentType;
            iteratore.parameter = parameter;
            iteratore.<$>menuType = menuType;
            iteratore.<$>menuContentType = menuContentType;
            iteratore.<$>parameter = parameter;
            iteratore.<>f__this = this;
            return iteratore;
        }

        private T createMenuObject<T>(string resourceLocation) where T: Menu
        {
            GameObject obj2 = App.Binder.AssetBundleLoader.instantiatePrefab(resourceLocation);
            obj2.transform.SetParent(base.transform, false);
            T component = obj2.GetComponent<T>();
            component.Canvas.enabled = false;
            return component;
        }

        private T createPrewarmedMenuObject<T>(string resourceLocation, List<MenuContentType> menuContentTypes, List<object> contentParams) where T: Menu
        {
            GameObject obj2 = App.Binder.AssetBundleLoader.instantiatePrefab(resourceLocation);
            obj2.transform.SetParent(base.transform, false);
            for (int i = 0; i < menuContentTypes.Count; i++)
            {
                obj2.name = obj2.name + ("+" + menuContentTypes[i]);
            }
            T component = obj2.GetComponent<T>();
            component.Canvas.enabled = false;
            component.preWarm(menuContentTypes, contentParams);
            return component;
        }

        [ContextMenu("returnToPreviousMenu")]
        private void editorReturnToPreviousMenu()
        {
            if (!this.InTransition)
            {
                this.returnToPreviousMenu(true);
            }
        }

        public Menu getSharedMenuObject(MenuType menuType)
        {
            for (int i = 0; i < this.m_menus.Count; i++)
            {
                if (this.m_menus[i].MenuType == menuType)
                {
                    return this.m_menus[i];
                }
            }
            return null;
        }

        [DebuggerHidden]
        private IEnumerator initializationRoutine()
        {
            <initializationRoutine>c__Iterator141 iterator = new <initializationRoutine>c__Iterator141();
            iterator.<>f__this = this;
            return iterator;
        }

        public void initializeMenuHero(CharacterInstance character)
        {
            if (this.m_menuHeroCharacterView != null)
            {
                PlayerView.Binder.PersistentCharacterViewPool.returnObject(this.m_menuHeroCharacterView, this.m_menuHeroCharacterView.CharacterPrefab);
                this.m_menuHeroCharacterView = null;
            }
            this.m_menuHeroCharacterView = PlayerView.Binder.PersistentCharacterViewPool.getObject(character.Prefab);
            this.m_menuHeroCharacterView.name = "MenuHeroCharacterView";
            this.m_menuHeroCharacterView.transform.SetParent(base.transform, true);
            this.m_menuHeroCharacterView.transform.localScale = Vector3.one;
            this.m_menuHeroCharacterView.transform.position = new Vector3(999f, 999f, 0f);
            this.m_menuHeroCharacterView.initialize(character, true);
            this.m_menuHeroCharacterView.gameObject.SetActive(false);
            this.MenuHero.setTarget(this.m_menuHeroCharacterView);
        }

        public void initializeMenuPet(CharacterPrefab prefab)
        {
            if (this.m_menuPetCharacterView != null)
            {
                PlayerView.Binder.PersistentCharacterViewPool.returnObject(this.m_menuPetCharacterView, this.m_menuPetCharacterView.CharacterPrefab);
                this.m_menuPetCharacterView = null;
            }
            this.m_menuPetCharacterView = PlayerView.Binder.PersistentCharacterViewPool.getObject(prefab);
            this.m_menuPetCharacterView.name = "MenuPetCharacterView";
            this.m_menuPetCharacterView.transform.SetParent(base.transform, true);
            this.m_menuPetCharacterView.transform.localScale = Vector3.one;
            this.m_menuPetCharacterView.transform.position = new Vector3(999f, 999f, 0f);
            this.m_menuPetCharacterView.initialize(null, true);
            this.m_menuPetCharacterView.gameObject.SetActive(false);
            this.MenuPet.setTarget(this.m_menuPetCharacterView);
        }

        public Coroutine instantCloseAllMenus()
        {
            return base.StartCoroutine(this.instantCloseAllMenusRoutine());
        }

        [DebuggerHidden]
        private IEnumerator instantCloseAllMenusRoutine()
        {
            <instantCloseAllMenusRoutine>c__Iterator139 iterator = new <instantCloseAllMenusRoutine>c__Iterator139();
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        private IEnumerator instantPopAllActiveMenus()
        {
            <instantPopAllActiveMenus>c__Iterator140 iterator = new <instantPopAllActiveMenus>c__Iterator140();
            iterator.<>f__this = this;
            return iterator;
        }

        public bool menuTypeInActiveStack(MenuType menuType)
        {
            foreach (Menu menu in this.m_activeMenuStack)
            {
                if (menu.MenuType == menuType)
                {
                    return true;
                }
            }
            return false;
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            GameLogic.Binder.EventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnGameplayEndingStarted -= new GameLogic.Events.GameplayEndingStarted(this.onGameplayEndingStarted);
            GameLogic.Binder.EventBus.OnGameplayEnded -= new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            PlayerView.Binder.EventBus.OnMenuChangeStarted -= new PlayerView.Events.MenuChangeStarted(this.onMenuChangeStarted);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            GameLogic.Binder.EventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnGameplayEndingStarted += new GameLogic.Events.GameplayEndingStarted(this.onGameplayEndingStarted);
            GameLogic.Binder.EventBus.OnGameplayEnded += new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            PlayerView.Binder.EventBus.OnMenuChangeStarted += new PlayerView.Events.MenuChangeStarted(this.onMenuChangeStarted);
        }

        private void onGameplayEnded(ActiveDungeon activeDungeon)
        {
        }

        private void onGameplayEndingStarted(ActiveDungeon activeDungeon)
        {
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            if (ConfigUi.TIME_SLOWDOWN_DURING_MENUS_ENABLED && (this.topmostActiveMenuType() == MenuType.NONE))
            {
                GameLogic.Binder.TimeSystem.gameplaySlowdown(false);
            }
        }

        private void onGameStateInitialized()
        {
            base.StartCoroutine(this.initializationRoutine());
        }

        private void onMenuChangeStarted(MenuType sourceMenuType, MenuType targetMenu)
        {
            if (ConfigUi.TIME_SLOWDOWN_DURING_MENUS_ENABLED)
            {
                if (((targetMenu != MenuType.NONE) && (targetMenu != MenuType.SlidingTaskPanel)) && (targetMenu != MenuType.SlidingAdventurePanel))
                {
                    GameLogic.Binder.TimeSystem.gameplaySlowdown(true);
                }
                else
                {
                    GameLogic.Binder.TimeSystem.gameplaySlowdown(false);
                }
            }
        }

        [DebuggerHidden]
        private IEnumerator popMenu(bool instant)
        {
            <popMenu>c__Iterator13F iteratorf = new <popMenu>c__Iterator13F();
            iteratorf.instant = instant;
            iteratorf.<$>instant = instant;
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        public Coroutine returnToPreviousMenu([Optional, DefaultParameterValue(true)] bool closeTooltipMenu)
        {
            if (this.InTransition)
            {
                UnityEngine.Debug.LogWarning("Menu transition already ongoing, skipping new returnToPreviousMenu()");
                return null;
            }
            return base.StartCoroutine(this.returnToPreviousMenuRoutine(closeTooltipMenu));
        }

        [DebuggerHidden]
        private IEnumerator returnToPreviousMenuRoutine([Optional, DefaultParameterValue(true)] bool closeTooltipMenu)
        {
            <returnToPreviousMenuRoutine>c__Iterator13A iteratora = new <returnToPreviousMenuRoutine>c__Iterator13A();
            iteratora.closeTooltipMenu = closeTooltipMenu;
            iteratora.<$>closeTooltipMenu = closeTooltipMenu;
            iteratora.<>f__this = this;
            return iteratora;
        }

        public Menu topmostActiveMenu()
        {
            if (this.m_activeMenuStack.Count > 0)
            {
                return this.m_activeMenuStack.Peek();
            }
            return null;
        }

        public MenuType topmostActiveMenuType()
        {
            if (this.m_activeMenuStack.Count > 0)
            {
                return this.m_activeMenuStack.Peek().MenuType;
            }
            return MenuType.NONE;
        }

        [DebuggerHidden]
        private IEnumerator transitionRoutine(MenuType targetMenuType, MenuContentType targetMenuContentType, object parameter, float delay, bool closeTopmostActiveMenuBeforeTransition, bool closeTopmostTooltipMenuBeforeTransition)
        {
            <transitionRoutine>c__Iterator13B iteratorb = new <transitionRoutine>c__Iterator13B();
            iteratorb.closeTopmostActiveMenuBeforeTransition = closeTopmostActiveMenuBeforeTransition;
            iteratorb.targetMenuType = targetMenuType;
            iteratorb.closeTopmostTooltipMenuBeforeTransition = closeTopmostTooltipMenuBeforeTransition;
            iteratorb.delay = delay;
            iteratorb.targetMenuContentType = targetMenuContentType;
            iteratorb.parameter = parameter;
            iteratorb.<$>closeTopmostActiveMenuBeforeTransition = closeTopmostActiveMenuBeforeTransition;
            iteratorb.<$>targetMenuType = targetMenuType;
            iteratorb.<$>closeTopmostTooltipMenuBeforeTransition = closeTopmostTooltipMenuBeforeTransition;
            iteratorb.<$>delay = delay;
            iteratorb.<$>targetMenuContentType = targetMenuContentType;
            iteratorb.<$>parameter = parameter;
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        public Coroutine transitionToMenu(MenuType menuType, [Optional, DefaultParameterValue(0)] MenuContentType menuContentType, [Optional, DefaultParameterValue(null)] object parameter, [Optional, DefaultParameterValue(0f)] float delay, [Optional, DefaultParameterValue(false)] bool closeTopmostActiveMenuBeforeTransition, [Optional, DefaultParameterValue(true)] bool closeTopmostTooltipMenuBeforeTransition)
        {
            if (this.InTransition)
            {
                UnityEngine.Debug.LogWarning("Menu transition already ongoing, skipping new transitionToMenu()");
                return null;
            }
            return base.StartCoroutine(this.transitionRoutine(menuType, menuContentType, parameter, delay, closeTopmostActiveMenuBeforeTransition, closeTopmostTooltipMenuBeforeTransition));
        }

        public Coroutine waitAndCloseAllMenus()
        {
            return base.StartCoroutine(this.waitAndCloseAllMenusRoutine());
        }

        [DebuggerHidden]
        private IEnumerator waitAndCloseAllMenusRoutine()
        {
            <waitAndCloseAllMenusRoutine>c__Iterator138 iterator = new <waitAndCloseAllMenusRoutine>c__Iterator138();
            iterator.<>f__this = this;
            return iterator;
        }

        public Coroutine waitAndTransitionToNewMenu(MenuType menuType, MenuContentType menuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            return base.StartCoroutine(this.waitAndTransitionToNewMenuRoutine(menuType, menuContentType, parameter));
        }

        [DebuggerHidden]
        private IEnumerator waitAndTransitionToNewMenuRoutine(MenuType menuType, MenuContentType menuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <waitAndTransitionToNewMenuRoutine>c__Iterator13C iteratorc = new <waitAndTransitionToNewMenuRoutine>c__Iterator13C();
            iteratorc.menuType = menuType;
            iteratorc.menuContentType = menuContentType;
            iteratorc.parameter = parameter;
            iteratorc.<$>menuType = menuType;
            iteratorc.<$>menuContentType = menuContentType;
            iteratorc.<$>parameter = parameter;
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        public Coroutine waitForMenuToBeClosed(MenuType menuType)
        {
            return base.StartCoroutine(this.waitForMenuToBeClosedRoutine(menuType));
        }

        [DebuggerHidden]
        private IEnumerator waitForMenuToBeClosedRoutine(MenuType menuType)
        {
            <waitForMenuToBeClosedRoutine>c__Iterator13D iteratord = new <waitForMenuToBeClosedRoutine>c__Iterator13D();
            iteratord.menuType = menuType;
            iteratord.<$>menuType = menuType;
            iteratord.<>f__this = this;
            return iteratord;
        }

        public PlayerView.CharacterMenuCamera CharacterMenuCamera
        {
            [CompilerGenerated]
            get
            {
                return this.<CharacterMenuCamera>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CharacterMenuCamera>k__BackingField = value;
            }
        }

        public bool InTransition
        {
            [CompilerGenerated]
            get
            {
                return this.<InTransition>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<InTransition>k__BackingField = value;
            }
        }

        public Camera MenuCamera
        {
            [CompilerGenerated]
            get
            {
                return this.<MenuCamera>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<MenuCamera>k__BackingField = value;
            }
        }

        public MenuCharacterView MenuHero
        {
            [CompilerGenerated]
            get
            {
                return this.<MenuHero>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<MenuHero>k__BackingField = value;
            }
        }

        public MenuCharacterView MenuPet
        {
            [CompilerGenerated]
            get
            {
                return this.<MenuPet>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<MenuPet>k__BackingField = value;
            }
        }

        public Material ReferenceMenuLightingMaterial
        {
            [CompilerGenerated]
            get
            {
                return this.<ReferenceMenuLightingMaterial>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ReferenceMenuLightingMaterial>k__BackingField = value;
            }
        }

        public MenuType TransitioningToMenuType
        {
            [CompilerGenerated]
            get
            {
                return this.<TransitioningToMenuType>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<TransitioningToMenuType>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <closeAllMenusAndTransitionToNewMenuRoutine>c__Iterator13E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuContentType <$>menuContentType;
            internal MenuType <$>menuType;
            internal object <$>parameter;
            internal MenuSystem <>f__this;
            internal MenuContentType menuContentType;
            internal MenuType menuType;
            internal object parameter;

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
                        this.$current = this.<>f__this.waitAndCloseAllMenus();
                        this.$PC = 1;
                        goto Label_0086;

                    case 1:
                        this.$current = this.<>f__this.transitionToMenu(this.menuType, this.menuContentType, this.parameter, 0f, false, true);
                        this.$PC = 2;
                        goto Label_0086;

                    case 2:
                        break;
                        this.$PC = -1;
                        break;
                }
                return false;
            Label_0086:
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
        private sealed class <initializationRoutine>c__Iterator141 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuSystem <>f__this;
            internal int <i>__1;
            internal int <i>__2;
            internal Stopwatch <watch>__0;

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
                    this.<watch>__0 = DebugUtil.StartStopwatch();
                    this.<>f__this.initializeMenuHero(GameLogic.Binder.GameState.Player.ActiveCharacter);
                    this.<>f__this.MenuCamera.enabled = true;
                    this.<i>__1 = 0;
                    while (this.<i>__1 < this.<>f__this.m_menus.Count)
                    {
                        this.<>f__this.m_menus[this.<i>__1].initialize(this.<>f__this.MenuCamera);
                        this.<>f__this.m_menus[this.<i>__1].Canvas.enabled = false;
                        this.<i>__1++;
                    }
                    this.<i>__2 = 0;
                    while (this.<i>__2 < this.<>f__this.m_preWarmedMenus.Count)
                    {
                        MenuSystem.PrewarmedMenuEntry entry = this.<>f__this.m_preWarmedMenus[this.<i>__2];
                        entry.Menu.initialize(this.<>f__this.MenuCamera);
                        MenuSystem.PrewarmedMenuEntry entry2 = this.<>f__this.m_preWarmedMenus[this.<i>__2];
                        entry2.Menu.Canvas.enabled = false;
                        this.<i>__2++;
                    }
                    PlayerView.Binder.TransitionSystem.enqueueGameStart();
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
        private sealed class <instantCloseAllMenusRoutine>c__Iterator139 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuSystem <>f__this;
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
                    case 1:
                        if (this.<>f__this.InTransition)
                        {
                            this.$current = null;
                            this.$PC = 1;
                            goto Label_0099;
                        }
                        this.<ie>__0 = this.<>f__this.instantPopAllActiveMenus();
                        break;

                    case 2:
                        break;

                    default:
                        goto Label_0097;
                }
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 2;
                    goto Label_0099;
                }
                this.$PC = -1;
            Label_0097:
                return false;
            Label_0099:
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
        private sealed class <instantPopAllActiveMenus>c__Iterator140 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuSystem <>f__this;
            internal int <i>__0;
            internal IEnumerator <ie>__1;

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
                        this.<i>__0 = this.<>f__this.m_activeMenuStack.Count - 1;
                        goto Label_0090;

                    case 1:
                        break;

                    default:
                        goto Label_00A8;
                }
            Label_0072:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    return true;
                }
                this.<i>__0--;
            Label_0090:
                if (this.<i>__0 >= 0)
                {
                    this.<ie>__1 = this.<>f__this.popMenu(true);
                    goto Label_0072;
                }
                goto Label_00A8;
                this.$PC = -1;
            Label_00A8:
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
        private sealed class <popMenu>c__Iterator13F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal MenuSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal Menu <menu>__0;
            internal bool instant;

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
                        if (this.<>f__this.m_activeMenuStack.Count <= 0)
                        {
                            goto Label_00C4;
                        }
                        this.<menu>__0 = this.<>f__this.m_activeMenuStack.Pop();
                        this.<menu>__0.GraphicRaycaster.enabled = false;
                        this.<ie>__1 = this.<menu>__0.hideRoutine(this.instant);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00C4;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    return true;
                }
                this.<menu>__0.Canvas.enabled = false;
                goto Label_00C4;
                this.$PC = -1;
            Label_00C4:
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
        private sealed class <returnToPreviousMenuRoutine>c__Iterator13A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>closeTooltipMenu;
            internal MenuSystem <>f__this;
            internal IEnumerator <ie>__0;
            internal IEnumerator <ie>__5;
            internal IEnumerator <ie>__6;
            internal Menu <sourceMenu>__1;
            internal MenuType <sourceMenuType>__2;
            internal Menu[] <stackContent>__4;
            internal Menu <targetMenu>__3;
            internal bool closeTooltipMenu;

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
                        this.<>f__this.InTransition = true;
                        if (((this.<>f__this.m_activeMenuStack.Count <= 0) || !this.closeTooltipMenu) || (this.<>f__this.m_activeMenuStack.Peek().MenuType != MenuType.TooltipMenu))
                        {
                            goto Label_00B6;
                        }
                        this.<ie>__0 = this.<>f__this.popMenu(false);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_01A9;

                    case 3:
                        goto Label_0224;

                    default:
                        goto Label_0262;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    goto Label_0264;
                }
            Label_00B6:
                this.<sourceMenu>__1 = null;
                this.<sourceMenuType>__2 = MenuType.NONE;
                if (this.<>f__this.m_activeMenuStack.Count > 0)
                {
                    this.<sourceMenu>__1 = this.<>f__this.m_activeMenuStack.Peek();
                    this.<sourceMenuType>__2 = this.<sourceMenu>__1.MenuType;
                }
                this.<targetMenu>__3 = null;
                this.<>f__this.TransitioningToMenuType = MenuType.NONE;
                if (this.<>f__this.m_activeMenuStack.Count < 2)
                {
                    PlayerView.Binder.EventBus.MenuChangeStarted(this.<sourceMenuType>__2, MenuType.NONE);
                    this.<ie>__6 = this.<>f__this.popMenu(false);
                    goto Label_0224;
                }
                this.<stackContent>__4 = this.<>f__this.m_activeMenuStack.ToArray();
                this.<>f__this.TransitioningToMenuType = this.<stackContent>__4[1].MenuType;
                PlayerView.Binder.EventBus.MenuChangeStarted(this.<sourceMenuType>__2, this.<stackContent>__4[1].MenuType);
                this.<ie>__5 = this.<>f__this.popMenu(false);
            Label_01A9:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 2;
                    goto Label_0264;
                }
                this.<targetMenu>__3 = this.<>f__this.m_activeMenuStack.Peek();
                this.<targetMenu>__3.refresh();
                goto Label_0234;
            Label_0224:
                while (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 3;
                    goto Label_0264;
                }
            Label_0234:
                this.<>f__this.InTransition = false;
                PlayerView.Binder.EventBus.MenuChanged(this.<sourceMenu>__1, this.<targetMenu>__3);
                goto Label_0262;
                this.$PC = -1;
            Label_0262:
                return false;
            Label_0264:
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
        private sealed class <transitionRoutine>c__Iterator13B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>closeTopmostActiveMenuBeforeTransition;
            internal bool <$>closeTopmostTooltipMenuBeforeTransition;
            internal float <$>delay;
            internal object <$>parameter;
            internal MenuContentType <$>targetMenuContentType;
            internal MenuType <$>targetMenuType;
            internal MenuSystem <>f__this;
            internal int <i>__2;
            internal IEnumerator <ie>__0;
            internal IEnumerator <ie>__10;
            internal IEnumerator <ie>__11;
            internal IEnumerator <ie>__12;
            internal IEnumerator <ie>__7;
            internal IEnumerator <ie>__8;
            internal IEnumerator <ie>__9;
            internal Menu <sourceMenu>__4;
            internal MenuType <sourceMenuType>__5;
            internal Menu <targetMenu>__1;
            internal bool <targetMenuIsOverlayMenu>__3;
            internal bool <transitioningBetweenTooltipMenus>__6;
            internal bool closeTopmostActiveMenuBeforeTransition;
            internal bool closeTopmostTooltipMenuBeforeTransition;
            internal float delay;
            internal object parameter;
            internal MenuContentType targetMenuContentType;
            internal MenuType targetMenuType;

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
                        if (this.<>f__this.m_activeMenuStack.Count <= 0)
                        {
                            goto Label_0137;
                        }
                        if (!this.closeTopmostActiveMenuBeforeTransition)
                        {
                            if (this.targetMenuType == MenuType.TooltipMenu)
                            {
                                goto Label_0137;
                            }
                            if (this.closeTopmostTooltipMenuBeforeTransition && (this.<>f__this.m_activeMenuStack.Peek().MenuType == MenuType.TooltipMenu))
                            {
                                this.$current = this.<>f__this.returnToPreviousMenu(false);
                                this.$PC = 2;
                                goto Label_05F1;
                            }
                            break;
                        }
                        this.$current = this.<>f__this.returnToPreviousMenu(true);
                        this.$PC = 1;
                        goto Label_05F1;

                    case 1:
                    case 3:
                        goto Label_0137;

                    case 2:
                        break;

                    case 4:
                        goto Label_0197;

                    case 5:
                        goto Label_0336;

                    case 6:
                        goto Label_0385;

                    case 7:
                        goto Label_03EA;

                    case 8:
                        goto Label_045A;

                    case 9:
                        goto Label_0483;

                    case 10:
                        goto Label_052A;

                    case 11:
                        goto Label_0589;

                    case 12:
                        goto Label_05B2;

                    default:
                        goto Label_05EF;
                }
                if ((this.<>f__this.m_activeMenuStack.Count > 0) && (this.<>f__this.m_activeMenuStack.Peek().MenuType == this.targetMenuType))
                {
                    this.$current = this.<>f__this.returnToPreviousMenu(true);
                    this.$PC = 3;
                    goto Label_05F1;
                }
            Label_0137:
                this.<>f__this.InTransition = true;
                this.<>f__this.TransitioningToMenuType = this.targetMenuType;
                if (this.delay <= 0f)
                {
                    goto Label_01A7;
                }
                this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(this.delay);
            Label_0197:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 4;
                    goto Label_05F1;
                }
            Label_01A7:
                this.<targetMenu>__1 = null;
                this.<i>__2 = 0;
                while (this.<i>__2 < this.<>f__this.m_menus.Count)
                {
                    if (this.<>f__this.m_menus[this.<i>__2].MenuType == this.targetMenuType)
                    {
                        this.<targetMenu>__1 = this.<>f__this.m_menus[this.<i>__2];
                        break;
                    }
                    this.<i>__2++;
                }
                this.<targetMenuIsOverlayMenu>__3 = (this.<targetMenu>__1 != null) && this.<targetMenu>__1.IsOverlayMenu;
                this.<sourceMenu>__4 = (this.<>f__this.m_activeMenuStack.Count <= 0) ? null : this.<>f__this.m_activeMenuStack.Peek();
                this.<sourceMenuType>__5 = (this.<sourceMenu>__4 == null) ? MenuType.NONE : this.<sourceMenu>__4.MenuType;
                this.<transitioningBetweenTooltipMenus>__6 = (this.<sourceMenuType>__5 == MenuType.TooltipMenu) && (this.targetMenuType == MenuType.TooltipMenu);
                PlayerView.Binder.EventBus.MenuChangeStarted(this.<sourceMenuType>__5, this.targetMenuType);
                if ((!this.closeTopmostTooltipMenuBeforeTransition || this.<transitioningBetweenTooltipMenus>__6) || (this.<sourceMenuType>__5 != MenuType.TooltipMenu))
                {
                    goto Label_0346;
                }
                this.<ie>__7 = this.<>f__this.popMenu(false);
            Label_0336:
                while (this.<ie>__7.MoveNext())
                {
                    this.$current = this.<ie>__7.Current;
                    this.$PC = 5;
                    goto Label_05F1;
                }
            Label_0346:
                if (!this.<transitioningBetweenTooltipMenus>__6)
                {
                    if ((this.<sourceMenu>__4 == null) || this.<targetMenuIsOverlayMenu>__3)
                    {
                        goto Label_03FA;
                    }
                    this.<ie>__9 = this.<>f__this.popMenu(false);
                    goto Label_03EA;
                }
                this.<ie>__8 = this.<>f__this.popMenu(true);
            Label_0385:
                while (this.<ie>__8.MoveNext())
                {
                    this.$current = this.<ie>__8.Current;
                    this.$PC = 6;
                    goto Label_05F1;
                }
                goto Label_03FA;
            Label_03EA:
                while (this.<ie>__9.MoveNext())
                {
                    this.$current = this.<ie>__9.Current;
                    this.$PC = 7;
                    goto Label_05F1;
                }
            Label_03FA:
                if ((this.<targetMenu>__1 == null) || this.<targetMenu>__1.PreWarmed)
                {
                    goto Label_0483;
                }
                this.<ie>__10 = this.<targetMenu>__1.preShowRoutine(this.targetMenuContentType, this.parameter);
            Label_045A:
                while (this.<ie>__10.MoveNext())
                {
                    this.$current = this.<ie>__10.Current;
                    this.$PC = 8;
                    goto Label_05F1;
                }
            Label_0483:
                while (PlayerView.Binder.ScreenTransitionEffect.Animating)
                {
                    this.$current = null;
                    this.$PC = 9;
                    goto Label_05F1;
                }
                if (this.<targetMenu>__1 == null)
                {
                    this.<ie>__12 = this.<>f__this.instantPopAllActiveMenus();
                    goto Label_0589;
                }
                this.<targetMenu>__1.Canvas.enabled = true;
                this.<targetMenu>__1.RectTm.localScale = Vector3.one;
                this.<targetMenu>__1.GraphicRaycaster.enabled = true;
                PlayerView.Binder.EventBus.MenuShowStarted(this.targetMenuType);
                this.<ie>__11 = this.<targetMenu>__1.showRoutine(this.targetMenuContentType, this.parameter);
            Label_052A:
                while (this.<ie>__11.MoveNext())
                {
                    this.$current = this.<ie>__11.Current;
                    this.$PC = 10;
                    goto Label_05F1;
                }
                this.<>f__this.m_activeMenuStack.Push(this.<targetMenu>__1);
                goto Label_05B2;
            Label_0589:
                while (this.<ie>__12.MoveNext())
                {
                    this.$current = this.<ie>__12.Current;
                    this.$PC = 11;
                    goto Label_05F1;
                }
            Label_05B2:
                while (PlayerView.Binder.ScreenTransitionEffect.Animating)
                {
                    this.$current = null;
                    this.$PC = 12;
                    goto Label_05F1;
                }
                this.<>f__this.InTransition = false;
                PlayerView.Binder.EventBus.MenuChanged(this.<sourceMenu>__4, this.<targetMenu>__1);
                goto Label_05EF;
                this.$PC = -1;
            Label_05EF:
                return false;
            Label_05F1:
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
        private sealed class <waitAndCloseAllMenusRoutine>c__Iterator138 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuSystem <>f__this;

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
                    case 1:
                        if (this.<>f__this.InTransition)
                        {
                            this.$current = null;
                            this.$PC = 1;
                            goto Label_0088;
                        }
                        break;

                    case 2:
                        break;

                    default:
                        goto Label_0086;
                }
                while (this.<>f__this.topmostActiveMenuType() != MenuType.NONE)
                {
                    this.$current = PlayerView.Binder.MenuSystem.returnToPreviousMenu(false);
                    this.$PC = 2;
                    goto Label_0088;
                }
                this.$PC = -1;
            Label_0086:
                return false;
            Label_0088:
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
        private sealed class <waitAndTransitionToNewMenuRoutine>c__Iterator13C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuContentType <$>menuContentType;
            internal MenuType <$>menuType;
            internal object <$>parameter;
            internal MenuSystem <>f__this;
            internal MenuContentType menuContentType;
            internal MenuType menuType;
            internal object parameter;

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
                    case 1:
                        if (this.<>f__this.InTransition || (((this.<>f__this.topmostActiveMenuType() != MenuType.NONE) && (this.<>f__this.topmostActiveMenuType() != MenuType.SlidingInventoryMenu)) && ((this.<>f__this.topmostActiveMenuType() != MenuType.SlidingTaskPanel) && (this.<>f__this.topmostActiveMenuType() != MenuType.SlidingAdventurePanel))))
                        {
                            this.$current = null;
                            this.$PC = 1;
                        }
                        else
                        {
                            this.$current = this.<>f__this.transitionToMenu(this.menuType, this.menuContentType, this.parameter, 0f, false, true);
                            this.$PC = 2;
                        }
                        return true;

                    case 2:
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

        [CompilerGenerated]
        private sealed class <waitForMenuToBeClosedRoutine>c__Iterator13D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuType <$>menuType;
            internal MenuSystem <>f__this;
            internal MenuType menuType;

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
                    case 1:
                        if (this.<>f__this.InTransition || (this.<>f__this.topmostActiveMenuType() == this.menuType))
                        {
                            this.$current = null;
                            this.$PC = 1;
                            return true;
                        }
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

        [StructLayout(LayoutKind.Sequential)]
        private struct PrewarmedMenuEntry
        {
            public PlayerView.MenuType MenuType;
            public List<MenuContentType> MenuContentTypes;
            public PlayerView.Menu Menu;
        }
    }
}

