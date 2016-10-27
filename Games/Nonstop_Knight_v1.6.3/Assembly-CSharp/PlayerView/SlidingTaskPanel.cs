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

    public class SlidingTaskPanel : Menu, ISlidingPanel
    {
        public GameObject CellPrototype;
        private float m_coreCellHeight;
        private float m_milestoneCellHeight;
        private List<TaskPanelMilestoneCell> m_milestoneCells;
        private List<MilestoneData> m_milestoneDataList = new List<MilestoneData>(1);
        private float m_milestoneDividerHeight;
        private float m_missionCellHeight;
        private List<MissionCell> m_missionCells;
        private float m_missionDividerHeight;
        private float m_nextMissionRefreshTime;
        private Dictionary<MenuContentType, SlidingTaskPanelCell> m_slidingTaskPanelCells = new Dictionary<MenuContentType, SlidingTaskPanelCell>();
        private List<MissionInstance> m_sortedMissionInstanceList = new List<MissionInstance>(ConfigMissions.NUM_ACTIVE_MISSIONS);
        public Image MasterCooldownBg;
        public RectTransform MasterCooldownRootTm;
        public Text MasterCooldownSubtitle;
        public Text MasterCooldownText;
        public Text MenuSubtitle;
        public LayoutElement MilestoneDivider;
        public GameObject MilestonePrototype;
        public const int MILESTONES_TOTAL = 1;
        public Text MilestoneSubtitle;
        public GameObject MissionCellPrototype;
        public LayoutElement MissionDivider;
        public Text MissionTitle;
        public OffscreenOpenClose PanelRoot;
        public UnityEngine.UI.ScrollRect ScrollRect;
        public RectTransform VerticalGroup;

        private SlidingTaskPanelCell addCell(MenuContentType menuContentType, Sprite icon, string text, System.Action clickCallback)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.CellPrototype);
            obj2.name = this.CellPrototype.name + "-" + text;
            obj2.transform.SetParent(this.VerticalGroup, false);
            obj2.transform.SetAsLastSibling();
            SlidingTaskPanelCell component = obj2.GetComponent<SlidingTaskPanelCell>();
            bool stripedRow = (this.VerticalGroup.childCount % 2) == 0;
            component.initialize(icon, text, stripedRow, clickCallback);
            this.m_slidingTaskPanelCells.Add(menuContentType, component);
            obj2.SetActive(true);
            return component;
        }

        public bool canBeOpened()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            PlayerView.MenuType type = PlayerView.Binder.MenuSystem.topmostActiveMenuType();
            return (((!PlayerView.Binder.SlidingTaskPanelController.PanningActive && !PlayerView.Binder.MenuSystem.InTransition) && (PlayerView.Binder.DungeonHud.MenuButton.Button.interactable && PlayerView.Binder.DungeonHud.IsOpen)) && (((activeDungeon != null) && (activeDungeon.CurrentGameplayState != GameplayState.RETIREMENT)) && (!activeDungeon.isTutorialDungeon() && (type == PlayerView.MenuType.NONE))));
        }

        public Button getSubMenuButton(MenuContentType subMenuContent)
        {
            return this.m_slidingTaskPanelCells[subMenuContent].Button;
        }

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator18C iteratorc = new <hideRoutine>c__Iterator18C();
            iteratorc.instant = instant;
            iteratorc.<$>instant = instant;
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        private void onAchievementsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.StackedPopupMenu, MenuContentType.AchievementPopupContent, null, 0f, false, true);
            }
        }

        protected override void onAwake()
        {
            this.addCell(MenuContentType.HeroPopupContent, PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "icon_helmet"), StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_TITLE, null, false)), new System.Action(this.onKnightButtonClicked));
            this.addCell(MenuContentType.LeaderboardPopupContent, PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_trophy_floater"), StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_LEADERBOARD_BUTTON_TEXT, null, false)), new System.Action(this.onLeaderdboardButtonClicked));
            this.addCell(MenuContentType.MissionsPopupContent, PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_bounty001_floater"), StringExtensions.ToUpperLoca(_.L(ConfigLoca.DHUD_BUTTON_MISSIONS, null, false)), new System.Action(this.onBountiesButtonClicked));
            this.addCell(MenuContentType.AchievementPopupContent, PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_prizeribbon_floater"), StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_ACHIEVEMENTS_BUTTON_TEXT, null, false)), new System.Action(this.onAchievementsButtonClicked));
            this.addCell(MenuContentType.ChestGalleryContent, PlayerView.Binder.SpriteResources.getSprite("Menu", "floater_chest_wooden"), StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_CODEX_CHESTS_BUTTON_TEXT, null, false)), new System.Action(this.onChestsButtonClicked));
            this.addCell(MenuContentType.OptionsContent, PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_bubble_tutorial"), StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_TITLE_MORE, null, false)), new System.Action(this.onMoreButtonClicked));
            this.m_coreCellHeight = this.CellPrototype.GetComponent<LayoutElement>().minHeight;
            this.CellPrototype.SetActive(false);
            for (int i = 0; i < 1; i++)
            {
                this.m_milestoneDataList.Add(new MilestoneData());
            }
            this.m_milestoneCells = new List<TaskPanelMilestoneCell>(1);
            this.m_milestoneDividerHeight = this.MilestoneDivider.minHeight;
            this.m_milestoneCellHeight = this.MilestonePrototype.GetComponent<LayoutElement>().minHeight;
            for (int j = 0; j < this.m_milestoneCells.Capacity; j++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.MilestonePrototype);
                obj2.name = this.MilestonePrototype.name + "-" + j;
                obj2.transform.SetParent(this.VerticalGroup, false);
                obj2.transform.SetSiblingIndex((this.MilestoneSubtitle.rectTransform.parent.GetSiblingIndex() + 1) + j);
                this.m_milestoneCells.Add(obj2.GetComponent<TaskPanelMilestoneCell>());
                obj2.SetActive(true);
            }
            this.MilestonePrototype.SetActive(false);
            this.m_missionCells = new List<MissionCell>(ConfigMissions.NUM_ACTIVE_MISSIONS);
            this.m_missionDividerHeight = this.MissionDivider.minHeight;
            this.m_missionCellHeight = this.MissionCellPrototype.GetComponent<LayoutElement>().minHeight;
            for (int k = 0; k < ConfigMissions.NUM_ACTIVE_MISSIONS; k++)
            {
                GameObject obj3 = UnityEngine.Object.Instantiate<GameObject>(this.MissionCellPrototype);
                obj3.name = this.MissionCellPrototype.name + "-" + k;
                obj3.transform.SetParent(this.VerticalGroup, false);
                obj3.transform.SetSiblingIndex(this.MissionTitle.rectTransform.parent.GetSiblingIndex() + 1);
                this.m_missionCells.Add(obj3.GetComponent<MissionCell>());
                obj3.SetActive(true);
            }
            this.MissionCellPrototype.SetActive(false);
            this.MissionTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.MISSIONS_QUICKVIEW_TITLE, null, false));
            this.MilestoneSubtitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_LEADERBOARD_BUTTON_TEXT, null, false));
            this.MenuSubtitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.DHUD_BUTTON_MENU, null, false));
        }

        public void onBountiesButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.StackedPopupMenu, MenuContentType.MissionsPopupContent, null, 0f, false, true);
            }
        }

        public void onBountiesQuickViewClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.StackedPopupMenu, MenuContentType.MissionsPopupContent, null, 0f, false, true);
            }
        }

        private void onChestsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.StackedPopupMenu, MenuContentType.ChestGalleryContent, null, 0f, false, true);
            }
        }

        public void onCloseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        private void onGestureTapRecognized(TKTapRecognizer r)
        {
            if (((!PlayerView.Binder.SlidingTaskPanelController.PanningActive && PlayerView.Binder.InputSystem.InputEnabled) && (!PlayerView.Binder.MenuSystem.InTransition && (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == PlayerView.MenuType.SlidingTaskPanel))) && PlayerView.Binder.InputSystem.touchOnValidSpotForGestureStart())
            {
                this.onCloseButtonClicked();
            }
        }

        private void onKnightButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.StackedPopupMenu, MenuContentType.HeroPopupContent, null, 0f, false, true);
            }
        }

        public void onLeaderdboardButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.StackedPopupMenu, MenuContentType.LeaderboardPopupContent, null, 0f, false, true);
            }
        }

        private void onMoreButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.StackedPopupMenu, MenuContentType.OptionsContent, null, 0f, false, true);
            }
        }

        private void onPetsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.StackedPopupMenu, MenuContentType.PetPopupContent, null, 0f, false, true);
            }
        }

        protected override void onRefresh()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.m_slidingTaskPanelCells[MenuContentType.MissionsPopupContent].setInteractable(player.HasUnlockedMissions);
            this.refreshMilestoneQuickView();
            this.refreshMissionQuickView();
        }

        private void onSkillsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.StackedPopupMenu, MenuContentType.SkillPopupContent, null, 0f, false, true);
            }
        }

        protected override void onUpdate(float dt)
        {
            this.refreshNotifiers();
            if (Time.unscaledTime >= this.m_nextMissionRefreshTime)
            {
                this.refreshMissionQuickView();
                this.m_nextMissionRefreshTime = Time.unscaledTime + UnityEngine.Random.Range((float) 0.5f, (float) 1f);
            }
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator18A iteratora = new <preShowRoutine>c__Iterator18A();
            iteratora.<>f__this = this;
            return iteratora;
        }

        public void refreshMilestoneQuickView()
        {
            int num = 0;
            this.refreshNextMilestonesForPlayer();
            bool flag = false;
            int num2 = 0;
            for (int i = 0; i < this.m_milestoneCells.Count; i++)
            {
                TaskPanelMilestoneCell cell = this.m_milestoneCells[i];
                if ((i >= this.m_milestoneDataList.Count) || (num >= App.Binder.ConfigMeta.NUM_VISIBLE_MENU_MILESTONES))
                {
                    cell.gameObject.SetActive(false);
                }
                else
                {
                    MilestoneData data = this.m_milestoneDataList[num2++];
                    if (data.Floor == 0x7fffffff)
                    {
                        cell.gameObject.SetActive(false);
                    }
                    else
                    {
                        cell.gameObject.SetActive(true);
                        num++;
                        cell.Bg.color = !flag ? ConfigUi.LIST_CELL_REGULAR_COLOR : ConfigUi.LIST_CELL_STRIPED_COLOR;
                        flag = !flag;
                        cell.Floor.text = _.L(ConfigLoca.UI_PROMPT_FLOOR_X, new <>__AnonType15<int>(data.Floor), false);
                        if (data.LeaderboardEntry != null)
                        {
                            cell.Text.text = data.LeaderboardEntry.getPrettyName();
                            if (data.LeaderboardEntry.isDummy())
                            {
                                cell.Icon.refresh(data.LeaderboardEntry);
                            }
                            else
                            {
                                List<LeaderboardImage> lbImages = new List<LeaderboardImage>();
                                lbImages.Add(cell.Icon);
                                List<LeaderboardEntry> lbEntries = new List<LeaderboardEntry>();
                                lbEntries.Add(data.LeaderboardEntry);
                                Service.Binder.FacebookAdapter.PopulateImages(lbImages, lbEntries);
                            }
                        }
                        else
                        {
                            cell.Text.text = data.Text;
                            cell.Icon.AvatarRaw.enabled = false;
                            cell.Icon.AvatarSprite.enabled = true;
                            cell.Icon.AvatarSprite.sprite = data.Sprite;
                        }
                    }
                }
            }
            this.MilestoneDivider.minHeight = this.m_milestoneDividerHeight + ((App.Binder.ConfigMeta.NUM_VISIBLE_MENU_MILESTONES - num) * this.m_milestoneCellHeight);
        }

        public void refreshMissionQuickView()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.m_sortedMissionInstanceList.Clear();
            for (int i = 0; i < player.Missions.Instances.Count; i++)
            {
                this.m_sortedMissionInstanceList.Add(player.Missions.Instances[i]);
            }
            this.m_sortedMissionInstanceList.Sort(new Comparison<MissionInstance>(MissionInstance.CompareByCooldown));
            bool flag = false;
            int num2 = 0;
            for (int j = 0; j < this.m_missionCells.Count; j++)
            {
                MissionCell cell = this.m_missionCells[j];
                if (j < this.m_sortedMissionInstanceList.Count)
                {
                    MissionInstance instance = this.m_sortedMissionInstanceList[j];
                    if (string.IsNullOrEmpty(instance.MissionId) || (instance.getRemainingCooldownSeconds() > 0L))
                    {
                        cell.gameObject.SetActive(false);
                    }
                    else
                    {
                        double num4;
                        cell.gameObject.SetActive(true);
                        num2++;
                        ConfigMissions.Mission missionData = ConfigMissions.GetMissionData(instance.MissionId);
                        cell.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(missionData.IconFloater);
                        float v = instance.getMissionProgress(player, out num4);
                        num4 = (num4 <= instance.Requirement) ? num4 : instance.Requirement;
                        object[] objArray1 = new object[] { _.L(missionData.Title, null, false), " ", num4, "/", instance.Requirement };
                        cell.Description.text = string.Concat(objArray1);
                        cell.ProgressBarText.text = null;
                        cell.ProgressBar.setNormalizedValue(v);
                        cell.Bg.color = !flag ? ConfigUi.LIST_CELL_REGULAR_COLOR : ConfigUi.LIST_CELL_STRIPED_COLOR;
                        flag = !flag;
                    }
                }
                else
                {
                    cell.gameObject.SetActive(false);
                }
            }
            if (!player.HasUnlockedMissions)
            {
                this.MissionTitle.text = string.Empty;
                this.MasterCooldownRootTm.gameObject.SetActive(false);
            }
            else if (player.Missions.hasAllMissionsOnCooldown())
            {
                this.MissionTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.MISSIONS_QUICKVIEW_NO_BOUNTIES_TITLE, null, false));
                this.MasterCooldownRootTm.gameObject.SetActive(true);
                num2++;
                this.MasterCooldownText.text = MenuHelpers.SecondsToStringHoursMinutes(player.Missions.getMinRemainingCooldownSeconds());
                this.MasterCooldownRootTm.SetSiblingIndex(this.MissionTitle.rectTransform.parent.GetSiblingIndex() + 1);
                this.MasterCooldownBg.color = !flag ? ConfigUi.LIST_CELL_REGULAR_COLOR : ConfigUi.LIST_CELL_STRIPED_COLOR;
            }
            else
            {
                this.MissionTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.MISSIONS_QUICKVIEW_TITLE, null, false));
                this.MasterCooldownRootTm.gameObject.SetActive(false);
            }
            this.MissionDivider.minHeight = this.m_missionDividerHeight + ((ConfigMissions.NUM_ACTIVE_MISSIONS - num2) * this.m_missionCellHeight);
            this.MissionDivider.minHeight += Mathf.Max((float) (this.m_milestoneCellHeight * (3 - App.Binder.ConfigMeta.NUM_VISIBLE_MENU_MILESTONES)), (float) 0f);
            this.MissionDivider.minHeight -= this.m_coreCellHeight;
        }

        private void refreshNextMilestonesForPlayer()
        {
            Player player = GameLogic.Binder.GameState.Player;
            for (int i = 0; i < this.m_milestoneDataList.Count; i++)
            {
                this.m_milestoneDataList[i].clear();
            }
            int num2 = 0;
            LeaderboardEntry entry = GameLogic.Binder.LeaderboardSystem.getNextLeaderboardTargetForPlayer(LeaderboardType.Royal, player);
            if (entry != null)
            {
                MilestoneData data = this.m_milestoneDataList[num2++];
                data.Floor = entry.HighestFloor;
                data.LeaderboardEntry = entry;
            }
            this.m_milestoneDataList.Sort(new Comparison<MilestoneData>(MilestoneData.SortByFloor));
        }

        private void refreshNotifiers()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (!App.Binder.ConfigMeta.NOTIFY_NEW_MISSIONS_DURING_WELCOME_BACK)
            {
                this.m_slidingTaskPanelCells[MenuContentType.MissionsPopupContent].setNotifierState(player.HasUnlockedMissions && player.Missions.hasUninspectedMissions());
            }
            if (App.Binder.ConfigMeta.SHOW_SHOP_IN_SLIDING_MENU)
            {
                this.m_slidingTaskPanelCells[MenuContentType.VendorPopupContent].setNotifierState(PlayerView.Binder.DungeonHud.shouldNotifyShop());
            }
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator18B iteratorb = new <showRoutine>c__Iterator18B();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        public override PlayerView.MenuType MenuType
        {
            get
            {
                return PlayerView.MenuType.SlidingTaskPanel;
            }
        }

        public OffscreenOpenClose Panel
        {
            get
            {
                return this.PanelRoot;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator18C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal SlidingTaskPanel <>f__this;
            internal float <duration>__0;
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
                        PlayerView.Binder.EventBus.OnGestureTapRecognized -= new PlayerView.Events.GestureTapRecognized(this.<>f__this.onGestureTapRecognized);
                        if (PlayerView.Binder.SlidingTaskPanelController.PanningActive)
                        {
                            goto Label_00FC;
                        }
                        this.<duration>__0 = !this.instant ? ConfigUi.SLIDING_PANEL_EXIT_DURATION : 0f;
                        this.<>f__this.PanelRoot.close(this.<duration>__0, !PlayerView.Binder.SlidingTaskPanelController.LastClosingTriggeredFromSwipe ? Easing.Function.IN_CUBIC : Easing.Function.OUT_CUBIC, 0f);
                        if (this.instant)
                        {
                            goto Label_00E5;
                        }
                        PlayerView.Binder.AudioSystem.playSfxGrp(AudioGroupType.SfxGrpUi_BookClose, (float) 0f);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00FC;
                }
                if (this.<>f__this.PanelRoot.Animating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
            Label_00E5:
                PlayerView.Binder.SlidingTaskPanelController.LastClosingTriggeredFromSwipe = false;
                goto Label_00FC;
                this.$PC = -1;
            Label_00FC:
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
        private sealed class <preShowRoutine>c__Iterator18A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Dictionary<MenuContentType, SlidingTaskPanelCell>.Enumerator <$s_442>__1;
            internal SlidingTaskPanel <>f__this;
            internal KeyValuePair<MenuContentType, SlidingTaskPanelCell> <kv>__2;
            internal bool <striped>__0;

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
                    PlayerView.Binder.EventBus.OnGestureTapRecognized += new PlayerView.Events.GestureTapRecognized(this.<>f__this.onGestureTapRecognized);
                    this.<>f__this.PanelRoot.close(0f, Easing.Function.LINEAR, 0f);
                    this.<striped>__0 = false;
                    this.<$s_442>__1 = this.<>f__this.m_slidingTaskPanelCells.GetEnumerator();
                    try
                    {
                        while (this.<$s_442>__1.MoveNext())
                        {
                            this.<kv>__2 = this.<$s_442>__1.Current;
                            if (this.<kv>__2.Value.gameObject.activeSelf)
                            {
                                this.<kv>__2.Value.Bg.color = !this.<striped>__0 ? ConfigUi.LIST_CELL_REGULAR_COLOR : ConfigUi.LIST_CELL_STRIPED_COLOR;
                                this.<striped>__0 = !this.<striped>__0;
                            }
                        }
                    }
                    finally
                    {
                        this.<$s_442>__1.Dispose();
                    }
                    this.<>f__this.onRefresh();
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
        private sealed class <showRoutine>c__Iterator18B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal SlidingTaskPanel <>f__this;

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
                        if (PlayerView.Binder.SlidingTaskPanelController.PanningActive)
                        {
                            goto Label_0089;
                        }
                        this.<>f__this.PanelRoot.open(ConfigUi.SLIDING_PANEL_ENTRY_DURATION, Easing.Function.OUT_CUBIC, 0f);
                        PlayerView.Binder.AudioSystem.playSfxGrp(AudioGroupType.SfxGrpUi_BookOpen, (float) 0f);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00AA;
                }
                if (this.<>f__this.PanelRoot.Animating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
            Label_0089:
                this.<>f__this.ScrollRect.verticalNormalizedPosition = 1f;
                goto Label_00AA;
                this.$PC = -1;
            Label_00AA:
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

        private class MilestoneData
        {
            public int Floor = 0x7fffffff;
            public GameLogic.LeaderboardEntry LeaderboardEntry;
            public UnityEngine.Sprite Sprite;
            public string Text;

            public void clear()
            {
                this.Floor = 0x7fffffff;
                this.Text = null;
                this.LeaderboardEntry = null;
                this.Sprite = null;
            }

            public static int SortByFloor(SlidingTaskPanel.MilestoneData x, SlidingTaskPanel.MilestoneData y)
            {
                int num = x.Floor.CompareTo(y.Floor);
                if (num != 0)
                {
                    return num;
                }
                if (!string.IsNullOrEmpty(x.Text) && !string.IsNullOrEmpty(y.Text))
                {
                    num = x.Text.CompareTo(y.Text);
                    if (num != 0)
                    {
                        return num;
                    }
                }
                return x.GetHashCode().CompareTo(y.GetHashCode());
            }
        }
    }
}

