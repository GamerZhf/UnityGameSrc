namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class AchievementPopupContent : MenuContent
    {
        [CompilerGenerated]
        private List<AchievementCell> <AchievementCells>k__BackingField;
        public PrettyButton GooglePlayButton;
        public Text GooglePlayConnectTitle;
        public GameObject GooglePlayExternalCellRoot;
        private bool m_platformAchievementPopupPending;
        private bool m_queuedRefresh;
        public UnityEngine.UI.ScrollRect ScrollRect;
        public RectTransform ScrollRectTm;
        public Text Subtitle;
        public RectTransform VerticalGroup;

        private void addAchivementCellToList(string id, int tier)
        {
            AchievementCell item = PlayerView.Binder.AchievementCellPool.getObject();
            item.transform.SetParent(this.VerticalGroup, false);
            bool stripedRow = (this.VerticalGroup.childCount % 2) == 1;
            item.initialize(id, tier, stripedRow);
            this.AchievementCells.Add(item);
            item.gameObject.SetActive(true);
        }

        private void cleanupCells()
        {
            for (int i = this.AchievementCells.Count - 1; i >= 0; i--)
            {
                AchievementCell item = this.AchievementCells[i];
                this.AchievementCells.Remove(item);
                PlayerView.Binder.AchievementCellPool.returnObject(item);
            }
        }

        public AchievementCell getFirstAchievementCellWithInteractableButton()
        {
            for (int i = 0; i < this.AchievementCells.Count; i++)
            {
                if (this.AchievementCells[i].CellButton.ButtonScript.interactable)
                {
                    return this.AchievementCells[i];
                }
            }
            return null;
        }

        private void onAchievementClaimed(Player player, string achievementId, int tier)
        {
            this.onRefresh();
        }

        protected override void onAwake()
        {
            this.AchievementCells = new List<AchievementCell>();
            this.Subtitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_ACHIEVEMENTS_BUTTON_TEXT, null, false));
            this.GooglePlayConnectTitle.text = _.L(ConfigLoca.GOOGLE_PLAY_TITLE, null, false);
            this.GooglePlayButton.Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.GOOGLE_PLAY_ACHIEVEMENTS_TITLE, null, false));
            this.GooglePlayExternalCellRoot.SetActive(true);
        }

        protected override void onCleanup()
        {
            this.cleanupCells();
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnAchievementClaimed -= new GameLogic.Events.AchievementClaimed(this.onAchievementClaimed);
            GameLogic.Binder.EventBus.OnResourcesGained -= new GameLogic.Events.ResourcesGained(this.onResourcesGained);
            App.Binder.EventBus.OnPlatformConnectStateChanged -= new App.Events.PlatformConnectStateChanged(this.onPlatformConnectStateChanged);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnAchievementClaimed += new GameLogic.Events.AchievementClaimed(this.onAchievementClaimed);
            GameLogic.Binder.EventBus.OnResourcesGained += new GameLogic.Events.ResourcesGained(this.onResourcesGained);
            App.Binder.EventBus.OnPlatformConnectStateChanged += new App.Events.PlatformConnectStateChanged(this.onPlatformConnectStateChanged);
        }

        public void onGooglePlayAchievementsButtonClicked()
        {
            if (!App.Binder.SocialSystem.IsConnected())
            {
                this.m_platformAchievementPopupPending = true;
                App.Binder.SocialSystem.Connect();
            }
            else
            {
                App.Binder.SocialSystem.ShowAchievements();
            }
        }

        private void onPlatformConnectStateChanged(PlatformConnectType connectType)
        {
            if (this.m_platformAchievementPopupPending && App.Binder.SocialSystem.IsConnected())
            {
                App.Binder.SocialSystem.ShowAchievements();
            }
            this.m_platformAchievementPopupPending = false;
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.m_platformAchievementPopupPending = false;
            this.cleanupCells();
            for (int i = 0; i < ConfigAchievements.ACHIEVEMENT_IDS.Count; i++)
            {
                string id = ConfigAchievements.ACHIEVEMENT_IDS[i];
                this.addAchivementCellToList(id, player.Achievements.getCurrentTier(id));
            }
            this.onRefresh();
        }

        protected override void onRefresh()
        {
            Player player = GameLogic.Binder.GameState.Player;
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_ACHIEVEMENTS_BUTTON_TEXT, null, false)), string.Empty, string.Empty);
            base.m_contentMenu.refreshTabs();
            for (int i = 0; i < this.AchievementCells.Count; i++)
            {
                string achievementId = this.AchievementCells[i].AchievementId;
                this.AchievementCells[i].refresh(true, player.Achievements.getCurrentTier(achievementId));
            }
            this.m_queuedRefresh = false;
        }

        private void onResourcesGained(Player player, ResourceType type, double amount, bool instant, string analyticsSourceId, Vector3? worldPt)
        {
            if (amount > 0.0)
            {
                this.m_queuedRefresh = true;
            }
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator106 iterator = new <onShow>c__Iterator106();
            iterator.param = param;
            iterator.<$>param = param;
            iterator.<>f__this = this;
            return iterator;
        }

        protected void Update()
        {
            if (this.m_queuedRefresh)
            {
                this.onRefresh();
            }
        }

        public List<AchievementCell> AchievementCells
        {
            [CompilerGenerated]
            get
            {
                return this.<AchievementCells>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<AchievementCells>k__BackingField = value;
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.AchievementPopupContent;
            }
        }

        public override string TabTitle
        {
            get
            {
                return StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_ACHIEVEMENTS_BUTTON_TEXT, null, false));
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator106 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>param;
            internal AchievementPopupContent <>f__this;
            internal int <achievementTier>__3;
            internal AchievementCell <cell>__6;
            internal float <cellCenterPos>__7;
            internal string <centerOnAchievementId>__1;
            internal int <i>__5;
            internal AchievementPopupContent.InputParameters <inputParams>__2;
            internal Player <player>__0;
            internal int <targetCellIdx>__4;
            internal float <totalContentHeight>__8;
            internal float <viewRectHeight>__9;
            internal object param;

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
                    this.<player>__0 = GameLogic.Binder.GameState.Player;
                    this.<centerOnAchievementId>__1 = null;
                    if (this.param != null)
                    {
                        this.<inputParams>__2 = (AchievementPopupContent.InputParameters) this.param;
                        if (this.<inputParams>__2.CenterOnAchievementId != null)
                        {
                            this.<centerOnAchievementId>__1 = this.<inputParams>__2.CenterOnAchievementId;
                        }
                    }
                    if (string.IsNullOrEmpty(this.<centerOnAchievementId>__1))
                    {
                        this.<centerOnAchievementId>__1 = this.<player>__0.Achievements.getFirstCompletableAchievement(out this.<achievementTier>__3);
                    }
                    this.<targetCellIdx>__4 = -1;
                    if (!string.IsNullOrEmpty(this.<centerOnAchievementId>__1))
                    {
                        this.<i>__5 = 0;
                        while (this.<i>__5 < this.<>f__this.AchievementCells.Count)
                        {
                            if (this.<>f__this.AchievementCells[this.<i>__5].AchievementId == this.<centerOnAchievementId>__1)
                            {
                                this.<targetCellIdx>__4 = this.<i>__5;
                                break;
                            }
                            this.<i>__5++;
                        }
                    }
                    if (this.<targetCellIdx>__4 != -1)
                    {
                        Canvas.ForceUpdateCanvases();
                        this.<cell>__6 = this.<>f__this.AchievementCells[this.<targetCellIdx>__4];
                        this.<cellCenterPos>__7 = -this.<cell>__6.RectTm.localPosition.y;
                        this.<totalContentHeight>__8 = this.<>f__this.VerticalGroup.rect.height;
                        this.<viewRectHeight>__9 = this.<>f__this.ScrollRectTm.rect.height;
                        this.<>f__this.ScrollRect.verticalNormalizedPosition = UiUtil.CalculateScrollRectVerticalNormalizedPosition(this.<cellCenterPos>__7, this.<totalContentHeight>__8, this.<viewRectHeight>__9);
                    }
                    else
                    {
                        this.<>f__this.ScrollRect.verticalNormalizedPosition = 1f;
                    }
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
        public struct InputParameters
        {
            public string CenterOnAchievementId;
        }
    }
}

