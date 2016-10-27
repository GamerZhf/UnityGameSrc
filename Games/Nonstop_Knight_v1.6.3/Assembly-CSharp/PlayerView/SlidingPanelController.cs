namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class SlidingPanelController : MonoBehaviour
    {
        [CompilerGenerated]
        private bool <LastClosingTriggeredFromSwipe>k__BackingField;
        [CompilerGenerated]
        private ISlidingPanel <SlidingPanel>k__BackingField;
        private int m_numFramesPanned;
        private PanDirection m_panDirection;
        private float m_panelOffset;
        private Vector2? m_slidingPanelPanStartPoint;

        public bool canBeClosed()
        {
            return ((!this.PanningActive && !PlayerView.Binder.MenuSystem.InTransition) && (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == this.SlidingPanel.MenuType));
        }

        public void initialize(ISlidingPanel slidingPanel, float panelOffset, PanDirection panDirection)
        {
            this.SlidingPanel = slidingPanel;
            this.m_panelOffset = panelOffset;
            this.m_panDirection = panDirection;
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            PlayerView.Binder.EventBus.OnGesturePanRecognized -= new PlayerView.Events.GesturePanRecognized(this.onGesturePanRecognized);
            PlayerView.Binder.EventBus.OnGesturePanCompleted -= new PlayerView.Events.GesturePanCompleted(this.onGesturePanCompleted);
            PlayerView.Binder.EventBus.OnMenuChanged -= new PlayerView.Events.MenuChanged(this.onMenuChanged);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            PlayerView.Binder.EventBus.OnGesturePanRecognized += new PlayerView.Events.GesturePanRecognized(this.onGesturePanRecognized);
            PlayerView.Binder.EventBus.OnGesturePanCompleted += new PlayerView.Events.GesturePanCompleted(this.onGesturePanCompleted);
            PlayerView.Binder.EventBus.OnMenuChanged += new PlayerView.Events.MenuChanged(this.onMenuChanged);
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            if (this.PanningActive)
            {
                this.m_slidingPanelPanStartPoint = null;
                if (!PlayerView.Binder.MenuSystem.InTransition && (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == this.SlidingPanel.MenuType))
                {
                    PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                }
            }
        }

        private void onGesturePanCompleted(TKPanRecognizer r)
        {
            if (this.PanningActive)
            {
                this.m_slidingPanelPanStartPoint = null;
                if (((this.m_panDirection == PanDirection.Right) && (r.deltaTranslation.x < 0f)) || ((this.m_panDirection == PanDirection.Left) && (r.deltaTranslation.x > 0f)))
                {
                    if (!PlayerView.Binder.MenuSystem.InTransition && (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == this.SlidingPanel.MenuType))
                    {
                        this.LastClosingTriggeredFromSwipe = true;
                        PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                    }
                }
                else
                {
                    if (this.SlidingPanel.MenuType == MenuType.SlidingTaskPanel)
                    {
                        GameLogic.Binder.GameState.Player.TrackingData.NumMainMenuOpensSwipe++;
                    }
                    this.SlidingPanel.Panel.open(ConfigUi.SLIDING_PANEL_ENTRY_DURATION, Easing.Function.OUT_CUBIC, 0f);
                    if (this.m_panDirection == PanDirection.Right)
                    {
                        PlayerView.Binder.DungeonHud.shiftRootPanelToRight(Easing.Function.OUT_CUBIC);
                    }
                    else if (this.m_panDirection == PanDirection.Left)
                    {
                        PlayerView.Binder.DungeonHud.shiftRootPanelToLeft(Easing.Function.OUT_CUBIC);
                    }
                    PlayerView.Binder.RoomView.RoomCamera.refreshCameraOffset(this.SlidingPanel.MenuType);
                }
            }
            this.m_numFramesPanned = 0;
        }

        private void onGesturePanRecognized(TKPanRecognizer r)
        {
            Vector2 screenPoint = r.touchLocation();
            if (this.PanningActive)
            {
                Vector2 vector2;
                Vector2 vector4;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(this.SlidingPanel.Panel.RectTm, screenPoint, PlayerView.Binder.MenuSystem.MenuCamera, out vector2);
                Vector2 vector3 = screenPoint - r.deltaTranslation;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(this.SlidingPanel.Panel.RectTm, vector3, PlayerView.Binder.MenuSystem.MenuCamera, out vector4);
                Vector2 vector5 = vector2 - vector4;
                float x = vector5.x;
                float num2 = this.SlidingPanel.Panel.RectTm.anchoredPosition.x + x;
                if (this.m_panDirection == PanDirection.Left)
                {
                    num2 = Mathf.Clamp(num2, this.SlidingPanel.Panel.OriginalAnchoredPos.x - this.SlidingPanel.Panel.OriginalPixelWidth, this.SlidingPanel.Panel.OriginalAnchoredPos.x);
                }
                else if (this.m_panDirection == PanDirection.Right)
                {
                    num2 = Mathf.Clamp(num2, this.SlidingPanel.Panel.OriginalAnchoredPos.x, this.SlidingPanel.Panel.OriginalAnchoredPos.x + this.SlidingPanel.Panel.OriginalPixelWidth);
                }
                this.SlidingPanel.Panel.RectTm.anchoredPosition = new Vector2(num2, this.SlidingPanel.Panel.RectTm.anchoredPosition.y);
                float num3 = PlayerView.Binder.DungeonHud.RootPanelTa.RectTm.anchoredPosition.x + x;
                if (this.m_panDirection == PanDirection.Left)
                {
                    num3 = Mathf.Clamp(num3, PlayerView.Binder.DungeonHud.PanelOriginalPosX - this.SlidingPanel.Panel.OriginalPixelWidth, PlayerView.Binder.DungeonHud.PanelOriginalPosX);
                }
                else if (this.m_panDirection == PanDirection.Right)
                {
                    num3 = Mathf.Clamp(num3, PlayerView.Binder.DungeonHud.PanelOriginalPosX, PlayerView.Binder.DungeonHud.PanelOriginalPosX + this.SlidingPanel.Panel.OriginalPixelWidth);
                }
                PlayerView.Binder.DungeonHud.RootPanelTa.RectTm.anchoredPosition = new Vector2(num3, PlayerView.Binder.DungeonHud.RootPanelTa.RectTm.anchoredPosition.y);
                float num4 = 0f;
                if (this.m_panDirection == PanDirection.Left)
                {
                    num4 = Mathf.Clamp01((this.SlidingPanel.Panel.OriginalAnchoredPos.x - this.SlidingPanel.Panel.RectTm.anchoredPosition.x) / this.SlidingPanel.Panel.OriginalPixelWidth);
                }
                else if (this.m_panDirection == PanDirection.Right)
                {
                    num4 = Mathf.Clamp01((this.SlidingPanel.Panel.RectTm.anchoredPosition.x - this.SlidingPanel.Panel.OriginalAnchoredPos.x) / this.SlidingPanel.Panel.OriginalPixelWidth);
                }
                PlayerView.Binder.RoomView.RoomCamera.Offset = PlayerView.Binder.RoomView.RoomCamera.DefaultOffset - PlayerView.Binder.RoomView.RoomCamera.Transform.TransformVector(this.m_panelOffset * num4, 0f, 0f);
            }
            else if (((this.m_numFramesPanned == 0) && !PlayerView.Binder.SlidingTaskPanelController.PanningActive) && (!PlayerView.Binder.SlidingAdventurePanelController.PanningActive && PlayerView.Binder.InputSystem.touchOnValidSpotForGestureStart()))
            {
                if (this.SlidingPanel.canBeOpened())
                {
                    float num5 = Mathf.Clamp01(screenPoint.x / ((float) Screen.width));
                    if (((this.m_panDirection == PanDirection.Right) && (num5 <= 0.5f)) || ((this.m_panDirection == PanDirection.Left) && (num5 > 0.5f)))
                    {
                        this.m_slidingPanelPanStartPoint = new Vector2?(r.startPoint);
                        PlayerView.Binder.MenuSystem.transitionToMenu(this.SlidingPanel.MenuType, MenuContentType.NONE, null, 0f, false, true);
                        PlayerView.Binder.DungeonHud.closeSlidingPanelArrowRoot(ConfigUi.SLIDING_PANEL_ENTRY_DURATION, Easing.Function.OUT_CUBIC);
                    }
                }
                else if (this.canBeClosed())
                {
                    this.m_slidingPanelPanStartPoint = new Vector2?(r.startPoint);
                    PlayerView.Binder.DungeonHud.closeSlidingPanelArrowRoot(ConfigUi.SLIDING_PANEL_ENTRY_DURATION, Easing.Function.OUT_CUBIC);
                }
            }
            this.m_numFramesPanned++;
        }

        private void onMenuChanged(Menu sourceMenu, Menu targetMenu)
        {
            if (((sourceMenu != null) && (sourceMenu.MenuType == this.SlidingPanel.MenuType)) && this.PanningActive)
            {
                this.m_slidingPanelPanStartPoint = null;
                if (PlayerView.Binder.MenuSystem.menuTypeInActiveStack(this.SlidingPanel.MenuType))
                {
                    this.SlidingPanel.Panel.open(ConfigUi.SLIDING_PANEL_ENTRY_DURATION, Easing.Function.OUT_CUBIC, 0f);
                    if (this.m_panDirection == PanDirection.Right)
                    {
                        PlayerView.Binder.DungeonHud.shiftRootPanelToRight(Easing.Function.OUT_CUBIC);
                    }
                    else if (this.m_panDirection == PanDirection.Left)
                    {
                        PlayerView.Binder.DungeonHud.shiftRootPanelToLeft(Easing.Function.OUT_CUBIC);
                    }
                }
                else
                {
                    this.SlidingPanel.Panel.close(ConfigUi.SLIDING_PANEL_EXIT_DURATION, Easing.Function.IN_CUBIC, 0f);
                    PlayerView.Binder.DungeonHud.shiftRootPanelToCenter(Easing.Function.IN_CUBIC);
                }
            }
        }

        public bool LastClosingTriggeredFromSwipe
        {
            [CompilerGenerated]
            get
            {
                return this.<LastClosingTriggeredFromSwipe>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<LastClosingTriggeredFromSwipe>k__BackingField = value;
            }
        }

        public bool PanningActive
        {
            get
            {
                return this.m_slidingPanelPanStartPoint.HasValue;
            }
        }

        public ISlidingPanel SlidingPanel
        {
            [CompilerGenerated]
            get
            {
                return this.<SlidingPanel>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<SlidingPanel>k__BackingField = value;
            }
        }

        public enum PanDirection
        {
            Right,
            Left
        }
    }
}

