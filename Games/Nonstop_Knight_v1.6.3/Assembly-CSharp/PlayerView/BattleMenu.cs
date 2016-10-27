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

    public class BattleMenu : Menu
    {
        public CustomScrollRect ContentRoot;
        private Coroutine m_dungeonRankUpRoutine;
        private List<MapCell> m_mapCells = new List<MapCell>();
        public VerticalLayoutGroup MapCellRoot;
        public CameraScrollRect MapScrollRect;
        public MenuOverlay Overlay;

        private void addDungeonCell(string dungeonId)
        {
            MapCell item = PlayerView.Binder.MapCellPool.getObject();
            item.transform.SetParent(this.MapCellRoot.transform, false);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;
            item.gameObject.SetActive(true);
            item.initialize(this, dungeonId);
            this.m_mapCells.Add(item);
        }

        private void centerOnDungeon(string dungeonId)
        {
            MapCell cell = this.getMapCellForDungeonId(dungeonId);
            if (cell != null)
            {
                this.MapScrollRect.setCameraPosY(cell.transform.position.y);
            }
            else
            {
                this.MapScrollRect.setCameraPosY(float.MinValue);
            }
        }

        private void centerOnLastPlayedMapCell()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.centerOnDungeon(player.getLastCompletedFloor(false).ToString());
        }

        private bool dungeonExplored(int dungeonNumber)
        {
            return GameLogic.Binder.GameState.Player.hasExploredDungeon(dungeonNumber.ToString());
        }

        [DebuggerHidden]
        private IEnumerator dungeonRankUpRoutine()
        {
            <dungeonRankUpRoutine>c__Iterator10F iteratorf = new <dungeonRankUpRoutine>c__Iterator10F();
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        public int getDistanceToLastExploredDungeon(MapCell mc)
        {
            MapCell cell = this.getLastExploredMapCell();
            if (cell != null)
            {
                return (mc.DungeonNumber - cell.DungeonNumber);
            }
            return 0;
        }

        public MapCell getFirstUnexploredMapCell()
        {
            int dungeonNumber = this.m_mapCells[0].DungeonNumber;
            MapCell cell = null;
            Player player = GameLogic.Binder.GameState.Player;
            for (int i = this.m_mapCells.Count - 1; i >= 0; i--)
            {
                MapCell cell2 = this.m_mapCells[i];
                if (!player.hasExploredDungeon(cell2.DungeonId) && (cell2.DungeonNumber < dungeonNumber))
                {
                    dungeonNumber = cell2.DungeonNumber;
                    cell = cell2;
                }
            }
            return cell;
        }

        public MapCell getLastExploredMapCell()
        {
            int dungeonNumber = 0;
            MapCell cell = null;
            Player player = GameLogic.Binder.GameState.Player;
            for (int i = this.m_mapCells.Count - 1; i >= 0; i--)
            {
                MapCell cell2 = this.m_mapCells[i];
                if (player.hasExploredDungeon(cell2.DungeonId) && (cell2.DungeonNumber > dungeonNumber))
                {
                    dungeonNumber = cell2.DungeonNumber;
                    cell = cell2;
                }
            }
            return cell;
        }

        private MapCell getMapCellForDungeonId(string dungeonId)
        {
            for (int i = 0; i < this.m_mapCells.Count; i++)
            {
                if (this.m_mapCells[i].DungeonId == dungeonId)
                {
                    return this.m_mapCells[i];
                }
            }
            return null;
        }

        public MapCell getNextMapCell(MapCell from)
        {
            int index = this.m_mapCells.IndexOf(from);
            if (index == 0)
            {
                return null;
            }
            return this.m_mapCells[index - 1];
        }

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator10E iteratore = new <hideRoutine>c__Iterator10E();
            iteratore.instant = instant;
            iteratore.<$>instant = instant;
            iteratore.<>f__this = this;
            return iteratore;
        }

        protected override void onAwake()
        {
            if ((PlayerView.Binder.MenuHudTop != null) && (PlayerView.Binder.MenuHudBottom != null))
            {
                RectTransform transform = this.ContentRoot.GetComponent<RectTransform>();
                float num = ((float) Screen.width) / ((float) Screen.height);
                float num2 = 0.5097f + (0.8716f * num);
                transform.offsetMin = new Vector2(0f, PlayerView.Binder.MenuHudBottom.BottomPanelTm.sizeDelta.y * num2);
                transform.offsetMax = new Vector2(0f, -PlayerView.Binder.MenuHudTop.TopPanelTm.sizeDelta.y * num2);
            }
            RectTransform component = this.Overlay.GetComponent<RectTransform>();
            component.sizeDelta = new Vector2(component.sizeDelta.x, 999999f);
            this.addDungeonCell("1");
        }

        private void onDiamondRewardVisualizationCompeleted(int count)
        {
            TransformAnimation component = PlayerView.Binder.MenuHudTop.DiamondsIcon.GetComponent<TransformAnimation>();
            PlayerView.Binder.MenuHudTop.pulsateElement(component);
            int num = int.Parse(PlayerView.Binder.MenuHudTop.DiamondsText.text);
            PlayerView.Binder.MenuHudTop.DiamondsText.text = (num + count).ToString();
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnDungeonExplored -= new GameLogic.Events.DungeonExplored(this.onDungeonExplored);
        }

        private void onDungeonExplored(CharacterInstance character, string dungeonId)
        {
            MapCell from = this.getMapCellForDungeonId(dungeonId);
            from.refresh(true, 0f);
            from.DynamicContent.LockedContentRoot.SetActive(true);
            from.DynamicContent.openLockedContent(ConfigUi.MAP_EXPLORE_ANIMATION_DURATION);
            from.LockedContentRoot.gameObject.SetActive(true);
            from.LockedContentRoot.setTransparent(false);
            from.DynamicContent.setColorTint(true, 0f);
            from.DynamicContent.setColorTint(false, ConfigUi.MAP_EXPLORE_ANIMATION_DURATION * 2f);
            from.LockedContentRoot.animateToTransparent(ConfigUi.MAP_EXPLORE_ANIMATION_DURATION, 0f);
            from.LockedContentRoot.CanvasGroup.interactable = false;
            from.LockedContentRoot.CanvasGroup.blocksRaycasts = false;
            MapCell cell2 = this.getNextMapCell(from);
            if (cell2 != null)
            {
                cell2.refresh(false, 0f);
                cell2.LockedContentRoot.setTransparent(true);
                cell2.LockedContentRoot.animateToBlack(ConfigUi.MAP_EXPLORE_ANIMATION_DURATION * 2f, 0f);
            }
            this.refreshMapCells();
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnDungeonExplored += new GameLogic.Events.DungeonExplored(this.onDungeonExplored);
        }

        protected override void onInitialize()
        {
            this.MapScrollRect.initialize(base.RectTm, base.Canvas.worldCamera);
        }

        protected override void onRefresh()
        {
            this.refreshMapCells();
        }

        protected override void onUpdate(float dt)
        {
            if (this.MapScrollRect.Initialized && ((PlayerView.Binder.MenuSystem.InTransition && (PlayerView.Binder.MenuSystem.TransitioningToMenuType == this.MenuType)) || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == this.MenuType)))
            {
                this.MapScrollRect.update(dt);
            }
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator10C iteratorc = new <preShowRoutine>c__Iterator10C();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        public bool previousDungeonExplored(MapCell mc)
        {
            return ((mc.DungeonNumber <= 1) || this.dungeonExplored(mc.DungeonNumber - 1));
        }

        private void refreshMapCells()
        {
            Player player = GameLogic.Binder.GameState.Player;
            for (int i = this.m_mapCells.Count - 1; i >= 0; i--)
            {
                MapCell cell = this.m_mapCells[i];
                if (!cell.IsAnimating)
                {
                    bool previousAreaCompleted = false;
                    if (i == (this.m_mapCells.Count - 1))
                    {
                        previousAreaCompleted = true;
                    }
                    float normalizedProgressBarValue = 0.5f;
                    if (player.PendingDungeonCompletionRewards.Contains(cell.DungeonId))
                    {
                        cell.refresh(previousAreaCompleted, normalizedProgressBarValue);
                    }
                    else
                    {
                        cell.refresh(previousAreaCompleted, normalizedProgressBarValue);
                    }
                }
            }
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator10D iteratord = new <showRoutine>c__Iterator10D();
            iteratord.<>f__this = this;
            return iteratord;
        }

        public override PlayerView.MenuType MenuType
        {
            get
            {
                return PlayerView.MenuType.BattleMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <dungeonRankUpRoutine>c__Iterator10F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal BattleMenu <>f__this;
            internal string <dungeonId>__2;
            internal int <i>__1;
            internal MapCell <mc>__3;
            internal Player <player>__0;

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
                        this.<player>__0 = GameLogic.Binder.GameState.Player;
                        this.<i>__1 = this.<player>__0.PendingDungeonCompletionRewards.Count - 1;
                        goto Label_0121;

                    case 1:
                        break;

                    default:
                        goto Label_0145;
                }
            Label_0113:
                this.<i>__1--;
            Label_0121:
                if (this.<i>__1 >= 0)
                {
                    this.<dungeonId>__2 = this.<player>__0.PendingDungeonCompletionRewards[this.<i>__1];
                    this.<mc>__3 = this.<>f__this.getMapCellForDungeonId(this.<dungeonId>__2);
                    if (this.<mc>__3 != null)
                    {
                        if (ConfigUi.MAP_ALL_PROGRESS_BARS_ENABLED)
                        {
                            this.<mc>__3.refresh(true, 0f);
                        }
                        else
                        {
                            this.<mc>__3.refresh(true, -1f);
                        }
                    }
                    this.<player>__0.PendingDungeonCompletionRewards.Remove(this.<dungeonId>__2);
                    if (this.<i>__1 < (this.<player>__0.PendingDungeonCompletionRewards.Count - 1))
                    {
                        this.$current = new WaitForSeconds(2f);
                        this.$PC = 1;
                        return true;
                    }
                    goto Label_0113;
                }
                this.<>f__this.m_dungeonRankUpRoutine = null;
                goto Label_0145;
                this.$PC = -1;
            Label_0145:
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
        private sealed class <hideRoutine>c__Iterator10E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal BattleMenu <>f__this;
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
                        if (ConfigUi.MENU_TO_MENU_FADE_TO_BLACK_TOTAL_DURATION <= 0f)
                        {
                            goto Label_00C2;
                        }
                        this.<>f__this.Overlay.setTransparent(true);
                        this.<>f__this.Overlay.fadeToBlack(!this.instant ? (ConfigUi.MENU_TO_MENU_FADE_TO_BLACK_TOTAL_DURATION * 0.5f) : 0f, 1f, Easing.Function.LINEAR);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00C2;
                }
                if (this.<>f__this.Overlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<>f__this.Overlay.setTransparent(false);
                goto Label_00C2;
                this.$PC = -1;
            Label_00C2:
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
        private sealed class <preShowRoutine>c__Iterator10C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal BattleMenu <>f__this;

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
                    this.<>f__this.refreshMapCells();
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
        private sealed class <showRoutine>c__Iterator10D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal BattleMenu <>f__this;
            internal Player <player>__0;

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
                        if (this.<>f__this.Canvas.renderMode != RenderMode.WorldSpace)
                        {
                            this.<>f__this.Canvas.renderMode = RenderMode.WorldSpace;
                            this.<>f__this.MapCellRoot.transform.SetParent(this.<>f__this.transform, true);
                            this.<>f__this.ContentRoot.vertical = false;
                            this.<>f__this.ContentRoot.horizontal = false;
                        }
                        this.<>f__this.MapCellRoot.gameObject.SetActive(false);
                        this.<>f__this.MapCellRoot.gameObject.SetActive(true);
                        this.<>f__this.MapScrollRect.reset();
                        this.<>f__this.MapScrollRect.BoundsMin = this.<>f__this.m_mapCells[this.<>f__this.m_mapCells.Count - 1].RectTm.position + ((Vector3) (this.<>f__this.m_mapCells[this.<>f__this.m_mapCells.Count - 1].RectTm.rect.size * 0.6f));
                        this.<>f__this.MapScrollRect.BoundsMax = this.<>f__this.m_mapCells[0].RectTm.position - ((Vector3) (this.<>f__this.m_mapCells[0].RectTm.rect.size * 0.25f));
                        this.<player>__0 = GameLogic.Binder.GameState.Player;
                        if (this.<player>__0.PendingDungeonCompletionRewards.Count == 0)
                        {
                            this.<>f__this.centerOnLastPlayedMapCell();
                        }
                        if (ConfigUi.MENU_TO_MENU_FADE_TO_BLACK_TOTAL_DURATION <= 0f)
                        {
                            goto Label_024A;
                        }
                        this.<>f__this.Overlay.setTransparent(false);
                        this.<>f__this.Overlay.fadeToTransparent(ConfigUi.MENU_TO_MENU_FADE_TO_BLACK_TOTAL_DURATION * 0.5f, Easing.Function.LINEAR);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_02D1;
                        this.$PC = -1;
                        goto Label_02D1;

                    default:
                        goto Label_02D1;
                }
                while (this.<>f__this.Overlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_02D3;
                }
            Label_024A:
                this.<>f__this.Overlay.setTransparent(true);
                if (this.<player>__0.PendingDungeonCompletionRewards.Count > 0)
                {
                    UnityUtils.StopCoroutine(this.<>f__this, ref this.<>f__this.m_dungeonRankUpRoutine);
                    this.<>f__this.m_dungeonRankUpRoutine = UnityUtils.StartCoroutine(this.<>f__this, this.<>f__this.dungeonRankUpRoutine());
                    this.$current = this.<>f__this.m_dungeonRankUpRoutine;
                    this.$PC = 2;
                    goto Label_02D3;
                }
            Label_02D1:
                return false;
            Label_02D3:
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

