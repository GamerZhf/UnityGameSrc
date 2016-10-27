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

    public class LeaderboardPopupContent : MenuContent
    {
        [CompilerGenerated]
        private List<LeaderboardCell> <LeaderboardCells>k__BackingField;
        public Text FbButtonText;
        public Text FbDescription;
        public GameObject FbRoot;
        public Text LoadingText;
        public const int LOCAL_TAB_INDEX = 1;
        private Coroutine m_reconstructRoutine;
        public const int ROYAL_TAB_INDEX = 0;
        public UnityEngine.UI.ScrollRect ScrollRect;
        public RectTransform ScrollRectTm;
        public Text TitleHighestFloor;
        public Text TitleName;
        public Text TitleRank;
        public RectTransform VerticalGroup;

        private LeaderboardCell addLeaderboardCellForIndex(int lbIndex, int cellIndex, bool showAvatar, LeaderboardType lbType, ref List<LeaderboardEntry> incompleteUserProfileList, ref List<LeaderboardImage> incompleteUserProfileTargetImages)
        {
            ILeaderboardSystem leaderboardSystem = GameLogic.Binder.LeaderboardSystem;
            LeaderboardCell cell = this.addLeaderboardCellToList(showAvatar);
            LeaderboardEntry lbe = leaderboardSystem.getSortedLeaderboardEntries(lbType)[lbIndex];
            cell.refresh(cellIndex + 1, lbe, lbe.ImageTexture, showAvatar);
            if ((lbType == LeaderboardType.Royal) && (lbe.ImageTexture == null))
            {
                if (incompleteUserProfileList == null)
                {
                    incompleteUserProfileList = new List<LeaderboardEntry>();
                    incompleteUserProfileTargetImages = new List<LeaderboardImage>();
                }
                incompleteUserProfileList.Add(lbe);
                incompleteUserProfileTargetImages.Add(cell.LeaderboardImage);
            }
            return cell;
        }

        private LeaderboardCell addLeaderboardCellToList(bool showAvatar)
        {
            LeaderboardCell item = PlayerView.Binder.LeaderboardCellPool.getObject();
            item.transform.SetParent(this.VerticalGroup, false);
            bool stripedRow = (this.VerticalGroup.childCount % 2) == 0;
            float height = !showAvatar ? 100f : 140f;
            item.initialize(stripedRow, height);
            this.LeaderboardCells.Add(item);
            item.gameObject.SetActive(true);
            return item;
        }

        private void cleanupCells()
        {
            for (int i = this.LeaderboardCells.Count - 1; i >= 0; i--)
            {
                LeaderboardCell item = this.LeaderboardCells[i];
                this.LeaderboardCells.Remove(item);
                PlayerView.Binder.LeaderboardCellPool.returnObject(item);
            }
        }

        private LeaderboardType getActiveLeaderboardType()
        {
            int num = this.getStackedMenuController().getActiveTabIndex();
            LeaderboardType royal = LeaderboardType.Royal;
            switch (num)
            {
                case 0:
                    return LeaderboardType.Royal;

                case 1:
                    return LeaderboardType.Local;
            }
            UnityEngine.Debug.LogError("Unsupported tab index: " + num);
            return royal;
        }

        private StackedMenuContentController getStackedMenuController()
        {
            return ((StackedPopupMenu) base.m_contentMenu).Smcc;
        }

        public override string getTitleForTab(int idx)
        {
            switch (idx)
            {
                case 0:
                    return StringExtensions.ToUpperLoca(_.L(ConfigLoca.LEADERBOARD_ROYAL, null, false));

                case 1:
                    return StringExtensions.ToUpperLoca(_.L(ConfigLoca.LEADERBOARD_LOCAL, null, false));
            }
            return null;
        }

        protected override void onAwake()
        {
            this.LeaderboardCells = new List<LeaderboardCell>(ConfigLeaderboard.MAX_NUM_VISIBLE_LEADERBOARD_CELLS);
            this.LoadingText.gameObject.SetActive(true);
            this.LoadingText.text = _.L(ConfigLoca.UI_STATUS_LOADING, null, false);
            this.FbRoot.SetActive(false);
            this.FbDescription.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.FACEBOOK_CONNECT_TO_DESCRIPTION, null, false));
            this.FbButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_CONNECT, null, false));
            this.TitleRank.text = MenuHelpers.ColoredText(StringExtensions.ToUpperLoca(_.L(ConfigLoca.LEADERBOARD_TITLE_RANK, null, false)));
            this.TitleName.text = MenuHelpers.ColoredText(StringExtensions.ToUpperLoca(_.L(ConfigLoca.LEADERBOARD_TITLE_NAME, null, false)));
            this.TitleHighestFloor.text = MenuHelpers.ColoredText(StringExtensions.ToUpperLoca(_.L(ConfigLoca.LEADERBOARD_TITLE_HIGHEST_FLOOR, null, false)));
        }

        protected override void onCleanup()
        {
            UnityUtils.StopCoroutine(this, ref this.m_reconstructRoutine);
            this.cleanupCells();
        }

        public void onConnectToFacebookButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                FacebookConnectPopupContent.InputParameters parameters2 = new FacebookConnectPopupContent.InputParameters();
                parameters2.context = "leaderboard";
                parameters2.CompletionCallback = delegate (bool success) {
                    if (success)
                    {
                        GameLogic.Binder.LeaderboardSystem.initialize();
                        this.reconstructContent();
                    }
                };
                FacebookConnectPopupContent.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TechPopupMenu, MenuContentType.FacebookConnectPopupContent, parameter, 0f, false, true);
            }
        }

        protected void OnDisable()
        {
            App.Binder.EventBus.OnLeaderboardLoaded -= new App.Events.LeaderboardLoaded(this.onLeaderboardLoaded);
        }

        protected void OnEnable()
        {
            App.Binder.EventBus.OnLeaderboardLoaded += new App.Events.LeaderboardLoaded(this.onLeaderboardLoaded);
        }

        private void onLeaderboardLoaded(LeaderboardType lbType)
        {
            if (lbType == this.getActiveLeaderboardType())
            {
                this.reconstructContent();
            }
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            Player player = GameLogic.Binder.GameState.Player;
            player.TrackingData.PerSessionLeaderboardOpenings++;
            if (player.TrackingData.PerSessionLeaderboardOpenings == 1)
            {
                Service.Binder.TrackingSystem.sendCrmEvent(player, "crm_leaderboard");
            }
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_LEADERBOARD_BUTTON_TEXT, null, false)), string.Empty, string.Empty);
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator12A iteratora = new <onShow>c__Iterator12A();
            iteratora.param = param;
            iteratora.<$>param = param;
            iteratora.<>f__this = this;
            return iteratora;
        }

        public override void onTabButtonClicked(int idx)
        {
            this.reconstructContent();
        }

        private void reconstructContent()
        {
            UnityUtils.StopCoroutine(this, ref this.m_reconstructRoutine);
            this.m_reconstructRoutine = UnityUtils.StartCoroutine(this, this.reconstructRoutine());
        }

        [DebuggerHidden]
        private IEnumerator reconstructRoutine()
        {
            <reconstructRoutine>c__Iterator12B iteratorb = new <reconstructRoutine>c__Iterator12B();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.LeaderboardPopupContent;
            }
        }

        public List<LeaderboardCell> LeaderboardCells
        {
            [CompilerGenerated]
            get
            {
                return this.<LeaderboardCells>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<LeaderboardCells>k__BackingField = value;
            }
        }

        public override bool UsesTabs
        {
            get
            {
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator12A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>param;
            internal LeaderboardPopupContent <>f__this;
            internal LeaderboardPopupContent.InputParams <input>__0;
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
                    this.<input>__0 = this.param as LeaderboardPopupContent.InputParams;
                    if (this.<input>__0 != null)
                    {
                        this.<>f__this.getStackedMenuController().setActiveTabIndex(this.<input>__0.OverrideTab);
                    }
                    this.<>f__this.reconstructContent();
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
        private sealed class <reconstructRoutine>c__Iterator12B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal IEnumerator <$s_414>__12;
            internal LeaderboardPopupContent <>f__this;
            internal LeaderboardCell <cell>__11;
            internal float <cellCenterPos>__14;
            internal int <i>__10;
            internal IEnumerator <ie>__1;
            internal List<LeaderboardEntry> <incompleteUserProfileList>__4;
            internal List<LeaderboardImage> <incompleteUserProfileTargetImages>__5;
            internal ILeaderboardSystem <lbSystem>__0;
            internal LeaderboardType <lbType>__3;
            internal Player <player>__2;
            internal LeaderboardCell <playerCell>__7;
            internal int <playerIdx>__8;
            internal bool <showAvatar>__6;
            internal Transform <tf>__13;
            internal long <time>__9;
            internal float <totalContentHeight>__15;
            internal float <viewRectHeight>__16;

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
                        this.<lbSystem>__0 = GameLogic.Binder.LeaderboardSystem;
                        this.<>f__this.cleanupCells();
                        this.<>f__this.FbRoot.SetActive(false);
                        this.<>f__this.LoadingText.gameObject.SetActive(true);
                        goto Label_00A9;

                    case 1:
                        break;

                    case 2:
                        goto Label_0248;

                    default:
                        goto Label_0441;
                }
            Label_0099:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    goto Label_0443;
                }
            Label_00A9:
                if (!this.<lbSystem>__0.Initialized)
                {
                    this.<ie>__1 = TimeUtil.WaitForUnscaledSeconds(0.2f);
                    goto Label_0099;
                }
                this.<player>__2 = GameLogic.Binder.GameState.Player;
                this.<lbType>__3 = this.<>f__this.getActiveLeaderboardType();
                this.<incompleteUserProfileList>__4 = null;
                this.<incompleteUserProfileTargetImages>__5 = null;
                this.<showAvatar>__6 = this.<lbType>__3 == LeaderboardType.Royal;
                if (this.<showAvatar>__6)
                {
                    this.<>f__this.TitleName.rectTransform.anchoredPosition = new Vector2(300f, 0f);
                }
                else
                {
                    this.<>f__this.TitleName.rectTransform.anchoredPosition = new Vector2(200f, 0f);
                }
                this.<playerCell>__7 = null;
                this.<playerIdx>__8 = this.<lbSystem>__0.getLeaderboardIndexForPlayer(this.<lbType>__3, this.<player>__2);
                this.<time>__9 = TimeUtil.CurrentTimeInMilliseconds;
                this.<i>__10 = 0;
                while (this.<i>__10 < ConfigLeaderboard.MAX_NUM_VISIBLE_LEADERBOARD_CELLS)
                {
                    if (this.<i>__10 >= this.<lbSystem>__0.getSortedLeaderboardEntries(this.<lbType>__3).Count)
                    {
                        break;
                    }
                    this.<cell>__11 = this.<>f__this.addLeaderboardCellForIndex(this.<i>__10, this.<i>__10, this.<showAvatar>__6, this.<lbType>__3, ref this.<incompleteUserProfileList>__4, ref this.<incompleteUserProfileTargetImages>__5);
                    this.<cell>__11.gameObject.SetActive(false);
                    if (this.<i>__10 == this.<playerIdx>__8)
                    {
                        this.<playerCell>__7 = this.<cell>__11;
                    }
                    if ((TimeUtil.CurrentTimeInMilliseconds - this.<time>__9) > 200L)
                    {
                        this.<time>__9 = TimeUtil.CurrentTimeInMilliseconds;
                        this.$current = null;
                        this.$PC = 2;
                        goto Label_0443;
                    }
                Label_0248:
                    this.<i>__10++;
                }
                this.<>f__this.LoadingText.gameObject.SetActive(false);
                if (Service.Binder.FacebookAdapter.RequiresUserConnect())
                {
                    this.<>f__this.FbRoot.SetActive(true);
                    RectTransformExtensions.SetBottom(this.<>f__this.ScrollRectTm, 150f);
                }
                else
                {
                    this.<>f__this.FbRoot.SetActive(false);
                    RectTransformExtensions.SetBottom(this.<>f__this.ScrollRectTm, 0f);
                }
                this.<$s_414>__12 = this.<>f__this.VerticalGroup.transform.GetEnumerator();
                try
                {
                    while (this.<$s_414>__12.MoveNext())
                    {
                        this.<tf>__13 = (Transform) this.<$s_414>__12.Current;
                        this.<tf>__13.gameObject.SetActive(true);
                    }
                }
                finally
                {
                    IDisposable disposable = this.<$s_414>__12 as IDisposable;
                    if (disposable == null)
                    {
                    }
                    disposable.Dispose();
                }
                if (this.<incompleteUserProfileList>__4 != null)
                {
                    Service.Binder.FacebookAdapter.PopulateImages(this.<incompleteUserProfileTargetImages>__5, this.<incompleteUserProfileList>__4);
                }
                if (this.<playerCell>__7 != null)
                {
                    Canvas.ForceUpdateCanvases();
                    this.<cellCenterPos>__14 = -this.<playerCell>__7.RectTm.localPosition.y;
                    this.<totalContentHeight>__15 = this.<>f__this.VerticalGroup.rect.height;
                    this.<viewRectHeight>__16 = this.<>f__this.ScrollRectTm.rect.height;
                    this.<>f__this.ScrollRect.verticalNormalizedPosition = UiUtil.CalculateScrollRectVerticalNormalizedPosition(this.<cellCenterPos>__14, this.<totalContentHeight>__15, this.<viewRectHeight>__16);
                }
                else
                {
                    this.<>f__this.ScrollRect.verticalNormalizedPosition = 1f;
                }
                this.<>f__this.m_reconstructRoutine = null;
                this.<>f__this.onRefresh();
                this.$PC = -1;
            Label_0441:
                return false;
            Label_0443:
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

        public class InputParams
        {
            public int OverrideTab;
        }
    }
}

