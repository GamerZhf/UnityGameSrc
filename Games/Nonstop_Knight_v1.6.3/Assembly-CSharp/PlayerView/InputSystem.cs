namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class InputSystem : MonoBehaviour
    {
        [CompilerGenerated]
        private TouchKit <TouchKit>k__BackingField;
        public const float DRAG_THRESHOLD_INCHES = 0.1f;
        private int m_backButtonState;
        private readonly Stack<System.Action> m_backNavigationStack = new Stack<System.Action>();
        private Requirement[] m_layerToRequirement = new Requirement[Enum.GetValues(typeof(Layer)).Length];
        private EventSystem m_unityEventSystem;
        private bool m_usesTouch;

        protected void Awake()
        {
            this.m_unityEventSystem = base.GetComponent<EventSystem>();
            this.m_usesTouch = this.UnityEventSystem.currentInputModule is TouchInputModule;
            float num = (Screen.dpi != 0f) ? Screen.dpi : 72f;
            this.m_unityEventSystem.pixelDragThreshold = Mathf.FloorToInt(num * 0.1f);
            this.TouchKit = base.gameObject.AddComponent<TouchKit>();
            this.TouchKit.autoScaleRectsAndDistances = true;
            this.TouchKit.shouldAutoUpdateTouches = true;
            this.TouchKit.maxTouchesToProcess = 1;
            this.TouchKit.drawDebugBoundaryFrames = !ConfigApp.ProductionBuild;
            this.TouchKit.drawTouches = !ConfigApp.ProductionBuild;
            this.TouchKit.simulateTouches = Application.isEditor;
            this.TouchKit.simulateMultitouch = Application.isEditor;
            TKSwipeRecognizer recognizer = new TKSwipeRecognizer(0.2f);
            recognizer.gestureRecognizedEvent += delegate (TKSwipeRecognizer r) {
                if (this.InputEnabled && (PlayerView.Binder.EventBus != null))
                {
                    PlayerView.Binder.EventBus.GestureSwipeRecognized(r);
                }
            };
            TouchKit.addGestureRecognizer(recognizer);
            TKPanRecognizer recognizer2 = new TKPanRecognizer(0.05f);
            recognizer2.gestureRecognizedEvent += delegate (TKPanRecognizer r) {
                if (this.InputEnabled && (PlayerView.Binder.EventBus != null))
                {
                    PlayerView.Binder.EventBus.GesturePanRecognized(r);
                }
            };
            recognizer2.gestureCompleteEvent += delegate (TKPanRecognizer r) {
                if (this.InputEnabled && (PlayerView.Binder.EventBus != null))
                {
                    PlayerView.Binder.EventBus.GesturePanCompleted(r);
                }
            };
            TouchKit.addGestureRecognizer(recognizer2);
            TKTapRecognizer recognizer3 = new TKTapRecognizer(0.4f, 0.5f);
            recognizer3.gestureRecognizedEvent += delegate (TKTapRecognizer r) {
                if (this.InputEnabled && (PlayerView.Binder.EventBus != null))
                {
                    PlayerView.Binder.EventBus.GestureTapRecognized(r);
                }
            };
            TouchKit.addGestureRecognizer(recognizer3);
        }

        public Requirement getInputRequirement(Layer layer)
        {
            return this.m_layerToRequirement[(int) layer];
        }

        public RectTransform getRectTransformUnderMouse()
        {
            PointerEventData eventData = new PointerEventData(this.m_unityEventSystem);
            eventData.position = this.getTouchOrMouseCurrentScreenPos();
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            this.m_unityEventSystem.RaycastAll(eventData, raycastResults);
            if (raycastResults.Count > 0)
            {
                RaycastResult result = raycastResults[0];
                return result.gameObject.GetComponent<RectTransform>();
            }
            return null;
        }

        public Vector2 getTouchOrMouseCurrentScreenPos()
        {
            if (!this.UsesTouch)
            {
                return Input.mousePosition;
            }
            if (Input.touchCount > 0)
            {
                return Input.GetTouch(0).position;
            }
            return Vector2.zero;
        }

        public bool hasActiveTouch()
        {
            if (this.UsesTouch)
            {
                return (Input.touchCount > 0);
            }
            return Input.GetMouseButton(0);
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameplayStateChangeStarted -= new GameLogic.Events.GameplayStateChangeStarted(this.onGameplayStateChangeStarted);
            GameLogic.Binder.EventBus.OnGameplayStateChanged -= new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            PlayerView.Binder.EventBus.OnMenuChangeStarted -= new PlayerView.Events.MenuChangeStarted(this.onMenuChangeStarted);
            PlayerView.Binder.EventBus.OnMenuChanged -= new PlayerView.Events.MenuChanged(this.onMenuChanged);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameplayStateChangeStarted += new GameLogic.Events.GameplayStateChangeStarted(this.onGameplayStateChangeStarted);
            GameLogic.Binder.EventBus.OnGameplayStateChanged += new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            PlayerView.Binder.EventBus.OnMenuChangeStarted += new PlayerView.Events.MenuChangeStarted(this.onMenuChangeStarted);
            PlayerView.Binder.EventBus.OnMenuChanged += new PlayerView.Events.MenuChanged(this.onMenuChanged);
            this.m_backNavigationStack.Clear();
        }

        private void onGameplayStateChanged(GameplayState previousState, GameplayState currentState)
        {
            this.setInputRequirement(Layer.GameplayTransition, Requirement.Neutral);
        }

        private void onGameplayStateChangeStarted(GameplayState fromState, GameplayState targetState, float transitionDelay)
        {
            this.setInputRequirement(Layer.GameplayTransition, Requirement.MustBeDisabled);
        }

        private void onMenuChanged(Menu sourceMenu, Menu targetMenu)
        {
            this.setInputRequirement(Layer.MenuTransition, Requirement.Neutral);
        }

        private void onMenuChangeStarted(MenuType sourceMenuType, MenuType targetMenuType)
        {
            this.setInputRequirement(Layer.MenuTransition, Requirement.MustBeDisabled);
        }

        public void PopBackNavigationListener()
        {
            this.m_backNavigationStack.Pop();
        }

        public void PushBackNavigationListener(System.Action listener)
        {
            this.m_backNavigationStack.Push(listener);
        }

        private void refreshInputStatus()
        {
            for (int i = 0; i < this.m_layerToRequirement.Length; i++)
            {
                switch (this.m_layerToRequirement[i])
                {
                    case Requirement.MustBeEnabled:
                        this.InputEnabled = true;
                        return;

                    case Requirement.MustBeDisabled:
                        this.InputEnabled = false;
                        return;
                }
            }
            this.InputEnabled = true;
        }

        public void setInputRequirement(Layer layer, Requirement requirement)
        {
            Requirement requirement2 = this.m_layerToRequirement[(int) layer];
            if (requirement != requirement2)
            {
                this.m_layerToRequirement[(int) layer] = requirement;
                this.refreshInputStatus();
            }
        }

        public bool touchOnValidSpotForGestureStart()
        {
            RectTransform transform = PlayerView.Binder.InputSystem.getRectTransformUnderMouse();
            if ((transform != null) && (transform != PlayerView.Binder.DungeonHud.Vignetting.rectTransform))
            {
                return false;
            }
            return true;
        }

        protected void Update()
        {
            if ((this.m_backButtonState == 0) && Input.GetKeyDown(KeyCode.Escape))
            {
                this.m_backButtonState = 1;
            }
            else if ((this.m_backButtonState == 1) && Input.GetKeyUp(KeyCode.Escape))
            {
                this.m_backButtonState = 0;
                if (this.InputEnabled && (this.m_backNavigationStack.Count > 0))
                {
                    this.m_backNavigationStack.Peek()();
                }
                else
                {
                    PlayerView.Binder.EventBus.NavigateBack();
                }
            }
        }

        public bool InputEnabled
        {
            get
            {
                return this.m_unityEventSystem.enabled;
            }
            set
            {
                this.m_unityEventSystem.enabled = value;
            }
        }

        public TouchKit TouchKit
        {
            [CompilerGenerated]
            get
            {
                return this.<TouchKit>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<TouchKit>k__BackingField = value;
            }
        }

        public EventSystem UnityEventSystem
        {
            get
            {
                return this.m_unityEventSystem;
            }
        }

        public bool UsesTouch
        {
            get
            {
                return this.m_usesTouch;
            }
        }

        public enum Layer
        {
            TechPopup,
            LocalNotificationDialog,
            ContextTutorial,
            FtueTutorial,
            MenuTransition,
            MenuHeroNamingPopup,
            LocationEndTransition,
            GameplayTransition
        }

        public enum Requirement
        {
            Neutral,
            MustBeEnabled,
            MustBeDisabled
        }
    }
}

