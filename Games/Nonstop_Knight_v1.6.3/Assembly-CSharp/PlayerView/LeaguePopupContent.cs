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
    using UnityEngine.UI;

    public class LeaguePopupContent : MenuContent
    {
        [CompilerGenerated]
        private List<LeaguePlayerCell> <LeaguePlayerCells>k__BackingField;
        [CompilerGenerated]
        private List<LeagueTitleCell> <LeagueTitleCells>k__BackingField;
        private double m_previousActivePlayerCrownCount;
        public UnityEngine.UI.ScrollRect ScrollRect;
        public RectTransform ScrollRectTm;
        public RectTransform VerticalGroup;

        private void addLeaguePlayerCellToList(LeaderboardEntry lbe, int rank)
        {
            LeaguePlayerCell item = PlayerView.Binder.LeaguePlayerCellPool.getObject();
            item.transform.SetParent(this.VerticalGroup, false);
            bool stripedRow = (this.VerticalGroup.childCount % 2) == 0;
            item.initialize(rank, stripedRow);
            bool selected = lbe.UserId == "unauthenticated_local_player_leaderboard_user";
            item.refresh(lbe, null, selected);
            this.LeaguePlayerCells.Add(item);
            item.gameObject.SetActive(true);
        }

        private void addLeagueTitleCellToList(LeagueData leagueData)
        {
            LeagueTitleCell item = PlayerView.Binder.LeagueTitleCellPool.getObject();
            item.transform.SetParent(this.VerticalGroup, false);
            bool stripedRow = (this.VerticalGroup.childCount % 2) == 0;
            item.initialize(leagueData, stripedRow);
            this.LeagueTitleCells.Add(item);
            item.gameObject.SetActive(true);
        }

        private void cleanupCells()
        {
            for (int i = this.LeaguePlayerCells.Count - 1; i >= 0; i--)
            {
                LeaguePlayerCell item = this.LeaguePlayerCells[i];
                this.LeaguePlayerCells.Remove(item);
                PlayerView.Binder.LeaguePlayerCellPool.returnObject(item);
            }
            for (int j = this.LeagueTitleCells.Count - 1; j >= 0; j--)
            {
                LeagueTitleCell cell2 = this.LeagueTitleCells[j];
                this.LeagueTitleCells.Remove(cell2);
                PlayerView.Binder.LeagueTitleCellPool.returnObject(cell2);
            }
        }

        private LeaguePlayerCell getActivePlayerCell()
        {
            for (int i = 0; i < this.LeaguePlayerCells.Count; i++)
            {
                if (this.LeaguePlayerCells[i].LeaderboardEntry.UserId == "unauthenticated_local_player_leaderboard_user")
                {
                    return this.LeaguePlayerCells[i];
                }
            }
            return null;
        }

        private void onActiveLeagueChestTypeChanged(Player player)
        {
            this.reconstructContent();
        }

        protected override void onAwake()
        {
            this.LeaguePlayerCells = new List<LeaguePlayerCell>(PlayerView.Binder.LeaguePlayerCellPool.InitialCapacity);
            this.LeagueTitleCells = new List<LeagueTitleCell>(PlayerView.Binder.LeagueTitleCellPool.InitialCapacity);
        }

        protected override void onCleanup()
        {
            this.cleanupCells();
        }

        public void onCustomCloseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnActiveLeagueChestTypeChanged -= new GameLogic.Events.ActiveLeagueChestTypeChanged(this.onActiveLeagueChestTypeChanged);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnActiveLeagueChestTypeChanged += new GameLogic.Events.ActiveLeagueChestTypeChanged(this.onActiveLeagueChestTypeChanged);
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.reconstructContent();
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle("LEAGUES", string.Empty, string.Empty);
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator12C iteratorc = new <onShow>c__Iterator12C();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        private void reconstructContent()
        {
            this.cleanupCells();
            List<LeaderboardEntry> list = new List<LeaderboardEntry>();
            for (int i = 0; i < ConfigLeaderboard.DUMMY_PLAYERS.Length; i++)
            {
                list.Add(ConfigLeaderboard.DUMMY_PLAYERS[i]);
            }
            LeaderboardEntry entry2 = new LeaderboardEntry();
            entry2.Name = "YOU";
            entry2.UserId = "unauthenticated_local_player_leaderboard_user";
            LeaderboardEntry item = entry2;
            item.setDefaultPlayerHeroAvatarSprite();
            list.Add(item);
            int num2 = 1;
            int index = ConfigLeagues.Leagues.Length - 1;
            for (int j = 0; j < list.Count; j++)
            {
                if ((index >= 0) && !ConfigLeagues.HasPlayerReachedLeague(0.0, ConfigLeagues.Leagues[index]))
                {
                    this.addLeagueTitleCellToList(ConfigLeagues.Leagues[index]);
                    index--;
                }
                this.addLeaguePlayerCellToList(list[j], num2++);
            }
            this.addLeagueTitleCellToList(ConfigLeagues.Leagues[0]);
            base.refresh();
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.LeaguePopupContent;
            }
        }

        public List<LeaguePlayerCell> LeaguePlayerCells
        {
            [CompilerGenerated]
            get
            {
                return this.<LeaguePlayerCells>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<LeaguePlayerCells>k__BackingField = value;
            }
        }

        public List<LeagueTitleCell> LeagueTitleCells
        {
            [CompilerGenerated]
            get
            {
                return this.<LeagueTitleCells>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<LeagueTitleCells>k__BackingField = value;
            }
        }

        public override MenuContent.TabSpriteParameters TabSprite
        {
            get
            {
                MenuContent.TabSpriteParameters parameters = new MenuContent.TabSpriteParameters();
                parameters.SpriteAtlasId = "Menu";
                parameters.SpriteId = "icon_trophy_floater";
                return parameters;
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator12C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal LeaguePopupContent <>f__this;
            internal LeaguePlayerCell <activePlayerCell>__4;
            internal float <cellCenterPos>__1;
            internal LeaguePlayerCell <centerOnCell>__0;
            internal double <crowns>__6;
            internal Player <player>__5;
            internal float <totalContentHeight>__2;
            internal float <viewRectHeight>__3;

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
                    this.<centerOnCell>__0 = this.<>f__this.getActivePlayerCell();
                    if (this.<centerOnCell>__0 != null)
                    {
                        Canvas.ForceUpdateCanvases();
                        this.<cellCenterPos>__1 = -this.<centerOnCell>__0.RectTm.localPosition.y;
                        this.<totalContentHeight>__2 = this.<>f__this.VerticalGroup.rect.height;
                        this.<viewRectHeight>__3 = this.<>f__this.ScrollRectTm.rect.height;
                        this.<>f__this.ScrollRect.verticalNormalizedPosition = UiUtil.CalculateScrollRectVerticalNormalizedPosition(this.<cellCenterPos>__1, this.<totalContentHeight>__2, this.<viewRectHeight>__3);
                    }
                    else
                    {
                        this.<>f__this.ScrollRect.verticalNormalizedPosition = 1f;
                    }
                    this.<activePlayerCell>__4 = this.<>f__this.getActivePlayerCell();
                    this.<player>__5 = GameLogic.Binder.GameState.Player;
                    this.<crowns>__6 = this.<player>__5.getResourceAmount(ResourceType.Crown);
                    if (this.<>f__this.m_previousActivePlayerCrownCount != this.<crowns>__6)
                    {
                        this.<activePlayerCell>__4.Flash.show(0.4f, Easing.Function.SMOOTHSTEP, Easing.Function.SMOOTHSTEP, 0.2f);
                        this.<>f__this.m_previousActivePlayerCrownCount = this.<crowns>__6;
                    }
                    else
                    {
                        this.<activePlayerCell>__4.Flash.hide();
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
    }
}

