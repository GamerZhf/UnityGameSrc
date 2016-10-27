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

    public class SlidingAdventurePanel : Menu, ISlidingPanel
    {
        public const int ADVENTURE_SUB_TAB_AUGMENTATIONS_INDEX = 1;
        public const int ADVENTURE_SUB_TAB_INFO_INDEX = 0;
        public const int ADVENTURE_SUB_TAB_SHOP_INDEX = 2;
        public IconWithText AdventureAscendButton;
        public RectTransform AdventureAugmentationGridTm;
        public Text AdventureBannerDescription;
        public Text AdventureBannerTitle;
        public GameObject AdventureContentRoot;
        public List<IconWithText> AdventureInfoCells;
        public GameObject AdventureMilestoneCellPrototype;
        public List<IconWithText> AdventureMilestoneRewards;
        public Text AdventureRewardStash;
        public ScrollRect AdventureScrollRect;
        public List<IconWithText> AdventureSubTabButtons;
        public List<RectTransform> AdventureSubTabContentRootTms;
        public Text AdventureSubTabTitle;
        public Text AdventureSubTitleTokensAmount;
        public GameObject AdventureSubTitleTokensRoot;
        public GameObject AugmentationCardPrototype;
        public GameObject AugmentationCellPrototype;
        public PrettyButton ConfirmationPanelConfirmPopupButton;
        public Text ConfirmationPanelConfirmPopupDescription;
        public GameObject ConfirmationPanelConfirmPopupRoot;
        public Text ConfirmationPanelConfirmPopupTitle;
        public Text ConfirmationPanelInfoPopupDescription;
        public Text ConfirmationPanelInfoPopupDescriptionTime;
        public GameObject ConfirmationPanelInfoPopupRoot;
        public GameObject ConfirmationPanelInfoPopupSpinner;
        public Text ConfirmationPanelInfoPopupTitle;
        public GameObject ConfirmationPanelRoot;
        public PrettyButton DonateButton;
        public IconWithText DonateCell;
        private int m_activeAdventureSubTabIdx = -1;
        private Sprite m_activeTabBgSprite;
        private int m_activeTabIdx = -1;
        private int m_activeTournamentSubTabIdx = -1;
        private TournamentView m_activeTournamentView;
        private List<TournamentMilestoneCell> m_adventureMilestoneCells;
        private List<Card> m_augCards;
        private List<PlayerAugmentationCell> m_augCells;
        private Sprite m_goldenTabBgSprite;
        private Sprite m_inactiveTabBgSprite;
        private InputParameters m_inputParams;
        private List<TournamentLogCell> m_tournamentLogCells;
        private List<TournamentMilestoneCell> m_tournamentMilestoneCells;
        private List<TournamentPlayerCell> m_tournamentPlayerCells;
        private Coroutine m_tournamentTabReconstructRoutine;
        private List<TournamentUpgradeCell> m_tournamentUpgradeCells;
        public const int MAIN_TAB_ADVENTURE_INDEX = 0;
        public const int MAIN_TAB_TOURNAMENTS_INDEX = 1;
        public TournamentMilestoneRewardCell MilestoneCardReward;
        public Text MilestoneHeaderText;
        public TournamentMilestoneRewardCell MilestoneMainReward;
        public TournamentMilestoneRewardCell MilestoneTopContributorReward;
        public const int NUM_VISIBLE_ADVENTURE_MILESTONES = 14;
        public OffscreenOpenClose PanelRoot;
        private static List<int> sm_tempIntList = new List<int>();
        private static List<PlayerAugEntry> sm_tempPlayerAugList = new List<PlayerAugEntry>();
        public List<IconWithText> TabButtons;
        public List<GameObject> TabContentRoots;
        public IconWithText TouranmentUpgradeInfoCell;
        public const int TOURNAMENT_SUB_TAB_INFO_INDEX = 0;
        public const int TOURNAMENT_SUB_TAB_JOURNAL_INDEX = 3;
        public const int TOURNAMENT_SUB_TAB_LEADERBOARD_INDEX = 1;
        public const int TOURNAMENT_SUB_TAB_UPGRADES_INDEX = 2;
        public Text TournamentBannerDescription;
        public Text TournamentBannerTitle;
        public GameObject TournamentContentRoot;
        public List<IconWithText> TournamentInfoCells;
        public GameObject TournamentLogCellPrototype;
        public GameObject TournamentMilestoneCellPrototype;
        public AnimatedProgressBar TournamentMilestoneProgressBar;
        public Text TournamentMilestoneProgressBarText;
        public GameObject TournamentPlayerCellPrototype;
        public List<IconWithText> TournamentSubTabButtons;
        public Text TournamentSubTabTitle;
        public Text TournamentTimeRemainingText;
        public GameObject TournamentUpgradeCellPrototype;
        public RectTransform VerticalGroupTm;

        private TournamentMilestoneCell addAdventureMilestoneCell()
        {
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.AdventureMilestoneCellPrototype);
            obj2.transform.SetParent(this.AdventureSubTabContentRootTms[0], false);
            TournamentMilestoneCell component = obj2.GetComponent<TournamentMilestoneCell>();
            this.m_adventureMilestoneCells.Add(component);
            obj2.SetActive(false);
            return component;
        }

        private Card addAugmentationCard()
        {
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.AugmentationCardPrototype);
            obj2.transform.SetParent(this.AdventureSubTabContentRootTms[2], false);
            Card component = obj2.GetComponent<Card>();
            this.m_augCards.Add(component);
            obj2.SetActive(false);
            return component;
        }

        private TournamentLogCell addTournamentLogCell()
        {
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.TournamentLogCellPrototype);
            obj2.transform.SetParent(this.VerticalGroupTm, false);
            TournamentLogCell component = obj2.GetComponent<TournamentLogCell>();
            this.m_tournamentLogCells.Add(component);
            obj2.SetActive(false);
            return component;
        }

        private TournamentMilestoneCell addTournamentMilestoneCell()
        {
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.TournamentMilestoneCellPrototype);
            obj2.transform.SetParent(this.VerticalGroupTm, false);
            TournamentMilestoneCell component = obj2.GetComponent<TournamentMilestoneCell>();
            this.m_tournamentMilestoneCells.Add(component);
            obj2.SetActive(false);
            return component;
        }

        private TournamentPlayerCell addTournamentPlayerCell()
        {
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.TournamentPlayerCellPrototype);
            obj2.transform.SetParent(this.VerticalGroupTm, false);
            TournamentPlayerCell component = obj2.GetComponent<TournamentPlayerCell>();
            this.m_tournamentPlayerCells.Add(component);
            obj2.SetActive(false);
            return component;
        }

        private TournamentUpgradeCell addTournamentUpgradeCell()
        {
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.TournamentUpgradeCellPrototype);
            obj2.transform.SetParent(this.VerticalGroupTm, false);
            TournamentUpgradeCell component = obj2.GetComponent<TournamentUpgradeCell>();
            this.m_tournamentUpgradeCells.Add(component);
            obj2.SetActive(false);
            return component;
        }

        public bool canBeOpened()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            PlayerView.MenuType type = PlayerView.Binder.MenuSystem.topmostActiveMenuType();
            return (((!PlayerView.Binder.SlidingAdventurePanelController.PanningActive && !PlayerView.Binder.MenuSystem.InTransition) && (PlayerView.Binder.DungeonHud.AdventureButton.Button.interactable && PlayerView.Binder.DungeonHud.IsOpen)) && (((activeDungeon != null) && (activeDungeon.CurrentGameplayState != GameplayState.RETIREMENT)) && (!activeDungeon.isTutorialDungeon() && (type == PlayerView.MenuType.NONE))));
        }

        private void cleanupAdventureSubTab()
        {
            for (int i = 0; i < this.AdventureSubTabContentRootTms.Count; i++)
            {
                this.AdventureSubTabContentRootTms[i].gameObject.SetActive(false);
            }
            for (int j = this.m_augCells.Count - 1; j >= 0; j--)
            {
                this.m_augCells[j].gameObject.SetActive(false);
            }
            for (int k = this.m_augCards.Count - 1; k >= 0; k--)
            {
                this.m_augCards[k].gameObject.SetActive(false);
            }
            for (int m = 0; m < this.m_adventureMilestoneCells.Count; m++)
            {
                this.m_adventureMilestoneCells[m].gameObject.SetActive(false);
            }
            this.AdventureSubTitleTokensRoot.SetActive(false);
        }

        private void cleanupCells()
        {
            this.cleanupSharedCells();
            this.cleanupAdventureSubTab();
            this.cleanupTournamentSubTab();
        }

        private void cleanupSharedCells()
        {
        }

        private void cleanupTournamentSubTab()
        {
            for (int i = 0; i < this.m_tournamentPlayerCells.Count; i++)
            {
                this.m_tournamentPlayerCells[i].gameObject.SetActive(false);
            }
            for (int j = 0; j < this.m_tournamentUpgradeCells.Count; j++)
            {
                this.m_tournamentUpgradeCells[j].gameObject.SetActive(false);
            }
            for (int k = 0; k < this.m_tournamentLogCells.Count; k++)
            {
                this.m_tournamentLogCells[k].gameObject.SetActive(false);
            }
            for (int m = 0; m < this.m_tournamentMilestoneCells.Count; m++)
            {
                this.m_tournamentMilestoneCells[m].gameObject.SetActive(false);
            }
            for (int n = 0; n < this.TournamentInfoCells.Count; n++)
            {
                this.TournamentInfoCells[n].gameObject.SetActive(false);
            }
            this.DonateCell.gameObject.SetActive(false);
            this.TouranmentUpgradeInfoCell.gameObject.SetActive(false);
        }

        private void fillAdventureMilestoneCell(Player player, int rowIdx, int startFloor, int milestoneFloor, int currentFloor, int highestFloorReached, TournamentMilestoneCell cell)
        {
            cell.Text.text = milestoneFloor.ToString();
            PlayerView.Binder.AdventureMilestones.fillCells(player, startFloor, milestoneFloor, cell.Rewards, false, false);
            cell.Bg.color = ((rowIdx % 2) != 0) ? ConfigUi.LIST_CELL_STRIPED_COLOR : ConfigUi.LIST_CELL_REGULAR_COLOR;
            if (currentFloor < milestoneFloor)
            {
                cell.CanvasGroup.alpha = 0.33f;
                if (highestFloorReached < milestoneFloor)
                {
                    cell.Tickmark.enabled = false;
                }
                else
                {
                    cell.Tickmark.material = PlayerView.Binder.DisabledUiMaterial;
                    cell.Tickmark.enabled = true;
                }
            }
            else
            {
                cell.CanvasGroup.alpha = 1f;
                cell.Tickmark.material = null;
                cell.Tickmark.enabled = true;
            }
        }

        public int getActiveAdventureSubTabIndex()
        {
            return this.m_activeAdventureSubTabIdx;
        }

        public int getActiveTabIndex()
        {
            return this.m_activeTabIdx;
        }

        public Button getContentTargetButton(ContentTarget target)
        {
            switch (target)
            {
                case ContentTarget.AdventureTabButton:
                    return this.TabButtons[0].Button;

                case ContentTarget.TournamentTabButton:
                    return this.TabButtons[1].Button;

                case ContentTarget.AscendButton:
                    return this.AdventureAscendButton.Button;

                case ContentTarget.ConfirmationButton:
                    return this.ConfirmationPanelConfirmPopupButton.Button;

                case ContentTarget.AdventureShopSubTab:
                    return this.AdventureSubTabButtons[2].Button;

                case ContentTarget.FirstAugmentationCell:
                    return this.m_augCards[0].Button;
            }
            return null;
        }

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator185 iterator = new <hideRoutine>c__Iterator185();
            iterator.instant = instant;
            iterator.<$>instant = instant;
            iterator.<>f__this = this;
            return iterator;
        }

        private void markActiveTabContentAsInspected()
        {
        }

        public void onAdventureSubTabButtonClicked(int idx)
        {
            if (this.m_activeAdventureSubTabIdx != idx)
            {
                PlayerView.Binder.AudioSystem.playSfxGrp(AudioGroupType.SfxGrpUi_BookTurn, (float) 0f);
                this.setActiveAdventureSubTabIndex(idx);
            }
        }

        public void onAscendButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.ThinPopupMenu, MenuContentType.AscendPopupContent, null, 0f, false, true);
            }
        }

        private void onAugCardClicked(Card card)
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                MiniPopupMenu.InputParameters parameters3;
                if (card.ActiveContent.Obj is PlayerAugmentation)
                {
                    Player player = GameLogic.Binder.GameState.Player;
                    PlayerAugmentation augmentation = (PlayerAugmentation) card.ActiveContent.Obj;
                    if (player.Augmentations.canBuy(augmentation.Id))
                    {
                        parameters3 = new MiniPopupMenu.InputParameters();
                        parameters3.MenuContentParams = card.ActiveContent.Obj;
                        MiniPopupMenu.InputParameters parameter = parameters3;
                        PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.MiniPopupMenu, MenuContentType.VendorMiniPopupContent, parameter, 0f, false, true);
                    }
                }
                else if (card.ActiveContent.Obj is ShopPurchaseController)
                {
                    ShopPurchaseController controller = (ShopPurchaseController) card.ActiveContent.Obj;
                    if (controller.isPurchaseable())
                    {
                        bool flag = controller.getRefShopEntry().Type == ShopEntryType.IapDiamonds;
                        if (App.Binder.ConfigMeta.DISABLE_VENDOR_ADS_CONFIRMATION_POPUP && controller.payWithAd())
                        {
                            flag = true;
                        }
                        if (flag)
                        {
                            controller.purchase(1);
                        }
                        else
                        {
                            parameters3 = new MiniPopupMenu.InputParameters();
                            parameters3.MenuContentParams = card.ActiveContent.Obj;
                            MiniPopupMenu.InputParameters parameters2 = parameters3;
                            PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.MiniPopupMenu, MenuContentType.VendorMiniPopupContent, parameters2, 0f, false, true);
                        }
                    }
                }
            }
        }

        protected override void onAwake()
        {
            this.m_activeTabBgSprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "uix_tab_active_0");
            this.m_inactiveTabBgSprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "uix_tab_inactive_0");
            this.m_goldenTabBgSprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "uix_button_selected");
            this.TabButtons[0].Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_ADVENTURE_TITLE, null, false));
            this.TabButtons[1].Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_TOURNAMENT_TITLE, null, false));
            this.AdventureBannerTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_ADVENTURE_TITLE, null, false));
            this.AdventureBannerDescription.text = _.L(ConfigLoca.ADVPANEL_ADVENTURE_DESCRIPTION, null, false);
            this.AdventureRewardStash.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ASCEND_REWARD_STASH, null, false));
            this.AdventureAscendButton.Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_ASCEND, null, false));
            this.AdventureInfoCells[0].Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_ADVENTURE_INFO1_TITLE, null, false));
            this.AdventureInfoCells[0].Text2.text = _.L(ConfigLoca.ADVPANEL_ADVENTURE_INFO1_DESCRIPTION, null, false);
            this.AdventureInfoCells[1].Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_TOURNAMENT_INFO4_TITLE, null, false));
            this.TournamentBannerTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_TOURNAMENT_TITLE, null, false));
            this.TournamentBannerDescription.text = _.L(ConfigLoca.ADVPANEL_TOURNAMENT_DESCRIPTION, null, false);
            this.TournamentInfoCells[0].Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_TOURNAMENT_INFO1_TITLE, null, false));
            this.TournamentInfoCells[0].Text2.text = _.L(ConfigLoca.ADVPANEL_TOURNAMENT_INFO1_DESCRIPTION, null, false);
            this.TournamentInfoCells[1].Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_TOURNAMENT_INFO2_TITLE, null, false));
            this.TournamentInfoCells[1].Text2.text = _.L(ConfigLoca.ADVPANEL_TOURNAMENT_INFO2_DESCRIPTION, null, false);
            this.TournamentInfoCells[2].Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_TOURNAMENT_INFO3_TITLE, null, false));
            this.TournamentInfoCells[3].Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_TOURNAMENT_INFO4_TITLE, null, false));
            this.DonateCell.Text.text = _.L(ConfigLoca.ADVPANEL_DONATE_DESCRIPTION, null, false);
            this.TouranmentUpgradeInfoCell.Text.text = _.L(ConfigLoca.BH_CARD_CEREMONY_FOOTNOTE, null, false);
            List<PerkType> list = GameLogic.Binder.PlayerAugmentationResources.getUsedPerkTypes();
            this.m_augCells = new List<PlayerAugmentationCell>(list.Count);
            for (int i = 0; i < this.m_augCells.Capacity; i++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.AugmentationCellPrototype);
                obj2.name = this.AugmentationCellPrototype.name + "-" + i;
                obj2.transform.SetParent(this.AdventureSubTabContentRootTms[1], false);
                this.m_augCells.Add(obj2.GetComponent<PlayerAugmentationCell>());
                obj2.SetActive(false);
            }
            this.AugmentationCellPrototype.SetActive(false);
            this.m_augCards = new List<Card>(6);
            for (int j = 0; j < this.m_augCards.Capacity; j++)
            {
                this.addAugmentationCard();
            }
            this.AugmentationCardPrototype.SetActive(false);
            for (int k = 0; k < this.AdventureSubTabButtons.Count; k++)
            {
                this.AdventureSubTabButtons[k].Background.sprite = this.m_inactiveTabBgSprite;
            }
            for (int m = 0; m < this.TournamentSubTabButtons.Count; m++)
            {
                this.TournamentSubTabButtons[m].Background.sprite = this.m_inactiveTabBgSprite;
            }
            this.m_tournamentPlayerCells = new List<TournamentPlayerCell>(50);
            for (int n = 0; n < this.m_tournamentPlayerCells.Capacity; n++)
            {
                this.addTournamentPlayerCell();
            }
            this.TournamentPlayerCellPrototype.SetActive(false);
            this.m_tournamentUpgradeCells = new List<TournamentUpgradeCell>(8);
            for (int num6 = 0; num6 < this.m_tournamentUpgradeCells.Capacity; num6++)
            {
                this.addTournamentUpgradeCell();
            }
            this.TournamentUpgradeCellPrototype.SetActive(false);
            this.m_tournamentLogCells = new List<TournamentLogCell>(0x19);
            for (int num7 = 0; num7 < this.m_tournamentLogCells.Capacity; num7++)
            {
                this.addTournamentLogCell();
            }
            this.TournamentLogCellPrototype.gameObject.SetActive(false);
            this.m_tournamentMilestoneCells = new List<TournamentMilestoneCell>(10);
            for (int num8 = 0; num8 < this.m_tournamentMilestoneCells.Capacity; num8++)
            {
                this.addTournamentMilestoneCell();
            }
            this.TournamentMilestoneCellPrototype.SetActive(false);
            this.m_adventureMilestoneCells = new List<TournamentMilestoneCell>(14);
            for (int num9 = 0; num9 < this.m_adventureMilestoneCells.Capacity; num9++)
            {
                this.addAdventureMilestoneCell();
            }
            this.AdventureMilestoneCellPrototype.SetActive(false);
        }

        public void onCloseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        public void onConfirmationPanelButtonClicked()
        {
            this.switchAdventure();
        }

        public void onDonateButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && (this.m_activeTournamentView != null))
            {
                MiniPopupMenu.InputParameters parameters2 = new MiniPopupMenu.InputParameters();
                parameters2.MenuContentParams = this.m_activeTournamentView;
                MiniPopupMenu.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.MiniPopupMenu, MenuContentType.VendorMiniPopupContent, parameter, 0f, false, true);
            }
        }

        private void onGestureTapRecognized(TKTapRecognizer r)
        {
            if (((!PlayerView.Binder.SlidingAdventurePanelController.PanningActive && PlayerView.Binder.InputSystem.InputEnabled) && (!PlayerView.Binder.MenuSystem.InTransition && (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == PlayerView.MenuType.SlidingAdventurePanel))) && PlayerView.Binder.InputSystem.touchOnValidSpotForGestureStart())
            {
                this.onCloseButtonClicked();
            }
        }

        private void onLocalTournamentViewsRefreshed()
        {
            if (this.m_activeTabIdx == 1)
            {
                UnityUtils.StopCoroutine(this, ref this.m_tournamentTabReconstructRoutine);
                this.m_tournamentTabReconstructRoutine = UnityUtils.StartCoroutine(this, this.reconstructTabTournamentRoutine());
                this.onRefresh();
            }
        }

        private void onPlayerAugmentationGained(Player player, string id)
        {
            if ((this.m_activeTabIdx == 0) && ((this.m_activeAdventureSubTabIdx == 1) || (this.m_activeAdventureSubTabIdx == 2)))
            {
                this.reconstructSubContentAdventure();
            }
        }

        protected override void onRefresh()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if ((this.m_activeTabIdx != 1) && PlayerView.Binder.NotificationSystem.NotifyAdventurePanelTournamentTab)
            {
                this.TabButtons[1].Notifier.SetActive(true);
                this.TabButtons[1].Background.sprite = this.m_goldenTabBgSprite;
            }
            else
            {
                this.TabButtons[1].Notifier.SetActive(false);
                this.TabButtons[1].Background.sprite = (this.m_activeTabIdx != 1) ? this.m_inactiveTabBgSprite : this.m_activeTabBgSprite;
            }
            if (this.m_activeTabIdx == 0)
            {
                int num = player.getLastCompletedFloor(false) + 1;
                int floor = player.getGatedRetirementMinFloor();
                if (num < floor)
                {
                    this.AdventureAscendButton.Text2.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ASCEND_UNLOCKED_AT_FLOOR, new <>__AnonType15<int>(floor), false));
                    this.AdventureAscendButton.Icon.gameObject.SetActive(true);
                    this.AdventureAscendButton.Text.gameObject.SetActive(false);
                    this.AdventureAscendButton.Text2.gameObject.SetActive(true);
                    this.AdventureAscendButton.Button.interactable = false;
                }
                else
                {
                    this.AdventureAscendButton.Icon.gameObject.SetActive(false);
                    this.AdventureAscendButton.Text.gameObject.SetActive(true);
                    this.AdventureAscendButton.Text2.gameObject.SetActive(false);
                    this.AdventureAscendButton.Button.interactable = true;
                }
                this.AdventureAscendButton.Background.material = !this.AdventureAscendButton.Button.interactable ? PlayerView.Binder.DisabledUiMaterial : null;
                if (this.m_activeAdventureSubTabIdx == 2)
                {
                    player.Notifiers.markAllAugNotificationsThatWeCanPurchaseAsInspected();
                    PlayerView.Binder.DungeonHud.refreshAdventureButton();
                    for (int i = 0; i < this.m_augCards.Count; i++)
                    {
                        Card card = this.m_augCards[i];
                        PlayerAugmentation augmentation = (PlayerAugmentation) card.ActiveContent.Obj;
                        bool interactable = !player.Tournaments.hasTournamentSelected() && player.Augmentations.canBuy(augmentation.Id);
                        this.m_augCards[i].refresh(card.ActiveContent.Text, card.ActiveContent.PriceText, card.ActiveContent.PriceIcon, interactable, !interactable);
                    }
                }
                if (App.Binder.ConfigMeta.SELL_AUGMENTATIONS_IN_ADVENTURE_PANEL && !player.Notifiers.AugmentationShopInspected)
                {
                    this.AdventureSubTabButtons[2].Notifier.SetActive(true);
                    this.AdventureSubTabButtons[2].Background.sprite = this.m_goldenTabBgSprite;
                }
                else
                {
                    this.AdventureSubTabButtons[2].Notifier.SetActive(false);
                    this.AdventureSubTabButtons[2].Background.sprite = (this.m_activeAdventureSubTabIdx != 2) ? this.m_inactiveTabBgSprite : this.m_activeTabBgSprite;
                }
            }
            else if (this.m_activeTabIdx == 1)
            {
                if (this.m_activeTournamentSubTabIdx == 1)
                {
                    for (int j = 0; j < this.m_tournamentPlayerCells.Count; j++)
                    {
                        this.m_tournamentPlayerCells[j].refresh();
                    }
                }
                else if (this.m_activeTournamentSubTabIdx == 2)
                {
                    if (this.m_activeTournamentView != null)
                    {
                        int num5 = this.m_activeTournamentView.Instance.getDonationsRemaining();
                        if (num5 > 0)
                        {
                            this.DonateButton.Text.text = this.m_activeTournamentView.Instance.getDonationPrice().ToString("0");
                            this.DonateButton.Button.interactable = num5 > 0;
                            this.DonateButton.Bg.material = !this.DonateButton.Button.interactable ? PlayerView.Binder.DisabledUiMaterial : null;
                            this.DonateCell.gameObject.SetActive(true);
                        }
                        else
                        {
                            this.DonateCell.gameObject.SetActive(false);
                        }
                    }
                }
                else if ((this.m_activeTournamentSubTabIdx == 3) && (this.m_activeTournamentView != null))
                {
                    List<TournamentLogEvent> displayableEvents = this.m_activeTournamentView.Log.GetDisplayableEvents();
                    for (int k = 0; k < this.m_tournamentLogCells.Count; k++)
                    {
                        TournamentLogCell cell = this.m_tournamentLogCells[k];
                        if (k < displayableEvents.Count)
                        {
                            cell.refresh(displayableEvents[displayableEvents.Count - (k + 1)]);
                            cell.gameObject.SetActive(true);
                        }
                        else
                        {
                            cell.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        private void onResourcesGained(Player player, ResourceType resourceType, double amount, bool visualizationManuallyControlled, string analyticsSourceId, Vector3? worldPt)
        {
            if (resourceType == ResourceType.Diamond)
            {
                this.onRefresh();
            }
        }

        public void onTabButtonClicked(int idx)
        {
            if (this.m_activeTabIdx != idx)
            {
                PlayerView.Binder.AudioSystem.playSfxGrp(AudioGroupType.SfxGrpUi_BookTurn, (float) 0f);
                this.setActiveTabIndex(idx);
            }
        }

        private void onTournamentDonationMade(Player player, TournamentInstance tournament, int count, double totalPrice)
        {
            this.onRefresh();
        }

        public void onTournamentSubTabButtonClicked(int idx)
        {
            if (this.m_activeTournamentSubTabIdx != idx)
            {
                PlayerView.Binder.AudioSystem.playSfxGrp(AudioGroupType.SfxGrpUi_BookTurn, (float) 0f);
                this.setActiveTournamentSubTabIndex(idx);
            }
        }

        protected override void onUpdate(float dt)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (this.ConfirmationPanelRoot.activeInHierarchy && this.ConfirmationPanelConfirmPopupButton.gameObject.activeInHierarchy)
            {
                this.ConfirmationPanelConfirmPopupButton.Button.interactable = ((((activeDungeon != null) && (activeDungeon.ActiveRoom != null)) && ((activeDungeon.CurrentGameplayState == GameplayState.ACTION) && !activeDungeon.ActiveRoom.MainBossSummoned)) && (!activeDungeon.WildBossMode && !PlayerView.Binder.MenuSystem.InTransition)) && !PlayerView.Binder.TransitionSystem.InCriticalTransition;
                this.ConfirmationPanelConfirmPopupButton.Bg.material = !this.ConfirmationPanelConfirmPopupButton.Button.interactable ? PlayerView.Binder.DisabledUiMaterial : null;
            }
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator183 iterator = new <preShowRoutine>c__Iterator183();
            iterator.parameter = parameter;
            iterator.<$>parameter = parameter;
            iterator.<>f__this = this;
            return iterator;
        }

        private void reconstructContent()
        {
            for (int i = 0; i < this.TabContentRoots.Count; i++)
            {
                this.TabContentRoots[i].SetActive(i == this.m_activeTabIdx);
            }
            UnityUtils.StopCoroutine(this, ref this.m_tournamentTabReconstructRoutine);
            if (this.m_activeTabIdx == 0)
            {
                this.reconstructTabAdventure();
            }
            else if (this.m_activeTabIdx == 1)
            {
                this.m_tournamentTabReconstructRoutine = UnityUtils.StartCoroutine(this, this.reconstructTabTournamentRoutine());
            }
            this.onRefresh();
        }

        private void reconstructSubContentAdventure()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.cleanupAdventureSubTab();
            this.AdventureScrollRect.content = this.AdventureSubTabContentRootTms[this.m_activeAdventureSubTabIdx];
            this.AdventureSubTabContentRootTms[this.m_activeAdventureSubTabIdx].gameObject.SetActive(true);
            this.AdventureScrollRect.verticalNormalizedPosition = 1f;
            if (this.m_activeAdventureSubTabIdx == 0)
            {
                this.AdventureSubTabTitle.text = "INFO";
                int highestFloorReached = player.getHighestFloorReached();
                int currentFloor = player.getCurrentFloor(true);
                sm_tempIntList.Clear();
                int floor = 1;
                int num5 = -1;
                while (floor < currentFloor)
                {
                    AdventureMilestones.MilestoneData data = PlayerView.Binder.AdventureMilestones.getNextMilestoneData(player, floor);
                    if (!sm_tempIntList.Contains(data.Floor))
                    {
                        sm_tempIntList.Add(data.Floor);
                    }
                    if (num5 != data.Floor)
                    {
                        floor = data.Floor + 1;
                        num5 = data.Floor;
                    }
                }
                int rowIdx = 0;
                for (int i = Mathf.Max((sm_tempIntList.Count - 7) - 1, 0); i < sm_tempIntList.Count; i++)
                {
                    TournamentMilestoneCell cell = this.m_adventureMilestoneCells[rowIdx];
                    floor = sm_tempIntList[i];
                    this.fillAdventureMilestoneCell(player, rowIdx, floor, floor, currentFloor, highestFloorReached, cell);
                    cell.gameObject.SetActive(true);
                    rowIdx++;
                }
                if (sm_tempIntList.Count > 0)
                {
                    floor = sm_tempIntList[sm_tempIntList.Count - 1] + 1;
                }
                else
                {
                    floor = currentFloor + 1;
                }
                for (int j = rowIdx; j < this.m_adventureMilestoneCells.Count; j++)
                {
                    TournamentMilestoneCell cell2 = this.m_adventureMilestoneCells[j];
                    int milestoneFloor = PlayerView.Binder.AdventureMilestones.getNextMilestoneData(player, floor).Floor;
                    this.fillAdventureMilestoneCell(player, j, floor, milestoneFloor, currentFloor, highestFloorReached, cell2);
                    cell2.gameObject.SetActive(true);
                    floor = milestoneFloor + 1;
                }
            }
            else if (this.m_activeAdventureSubTabIdx == 1)
            {
                this.AdventureSubTabTitle.text = "KNIGHT UPGRADES";
                List<PerkType> list = GameLogic.Binder.PlayerAugmentationResources.getUsedPerkTypes();
                PlayerAugmentations source = player.Augmentations;
                sm_tempPlayerAugList.Clear();
                for (int k = 0; k < list.Count; k++)
                {
                    PerkType perkType = list[k];
                    ConfigPerks.SharedData data2 = ConfigPerks.SHARED_DATA[perkType];
                    List<KeyValuePair<PerkInstance, BuffSource>> perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(source, perkType);
                    float num12 = 0f;
                    for (int num13 = 0; num13 < perkInstancesOfType.Count; num13++)
                    {
                        KeyValuePair<PerkInstance, BuffSource> pair = perkInstancesOfType[num13];
                        num12 += pair.Key.Modifier;
                    }
                    if (num12 != 0f)
                    {
                        string str;
                        if (perkType == PerkType.CoinBonusStart)
                        {
                            str = MenuHelpers.BigValueToString((double) num12);
                        }
                        else
                        {
                            str = MenuHelpers.BigModifierToString(num12, true);
                        }
                        PlayerAugEntry item = new PlayerAugEntry();
                        item.Title = StringExtensions.ToLowerLoca(_.L(data2.ShortDescription, null, false));
                        item.Text = str;
                        item.Icon = PlayerView.Binder.SpriteResources.getSprite(data2.Sprite);
                        item.CornerText = "x" + perkInstancesOfType.Count.ToString();
                        item.Count = perkInstancesOfType.Count;
                        item.OrderNo = k;
                        sm_tempPlayerAugList.Add(item);
                    }
                }
                sm_tempPlayerAugList.Sort(new Comparison<PlayerAugEntry>(PlayerAugEntry.SortByCount));
                for (int m = 0; m < sm_tempPlayerAugList.Count; m++)
                {
                    PlayerAugEntry entry2 = sm_tempPlayerAugList[m];
                    PlayerAugmentationCell cell3 = this.m_augCells[m];
                    cell3.Title.text = entry2.Title;
                    cell3.Text.text = entry2.Text;
                    cell3.Icon.sprite = entry2.Icon;
                    cell3.CornerText.text = entry2.CornerText;
                    cell3.Bg.color = ((m % 2) != 0) ? ConfigUi.LIST_CELL_STRIPED_COLOR : ConfigUi.LIST_CELL_REGULAR_COLOR;
                    cell3.gameObject.SetActive(true);
                }
                for (int n = sm_tempPlayerAugList.Count; n < this.m_augCells.Count; n++)
                {
                    this.m_augCells[n].gameObject.SetActive(false);
                }
            }
            else if (this.m_activeAdventureSubTabIdx == 2)
            {
                this.AdventureSubTabTitle.text = "SHOP";
                this.AdventureSubTitleTokensAmount.text = MenuHelpers.BigValueToString(player.getResourceAmount(ResourceType.Token));
                this.AdventureSubTitleTokensRoot.SetActive(true);
                int num16 = 0;
                List<PlayerAugmentation> list3 = GameLogic.Binder.PlayerAugmentationResources.getOrderedList();
                for (int num17 = 0; num17 < list3.Count; num17++)
                {
                    if (num16 >= 6)
                    {
                        break;
                    }
                    PlayerAugmentation augmentation = list3[num17];
                    if (!player.Augmentations.hasAugmentation(augmentation.Id))
                    {
                        Card card = this.m_augCards[num16++];
                        string str2 = null;
                        SpriteAtlasEntry sprite = null;
                        if (augmentation.PerkInstance != null)
                        {
                            ConfigPerks.SharedData data3 = ConfigPerks.SHARED_DATA[augmentation.PerkInstance.Type];
                            if (augmentation.PerkInstance.Type == PerkType.CoinBonusStart)
                            {
                                str2 = MenuHelpers.BigValueToString((double) augmentation.PerkInstance.Modifier) + "\n" + StringExtensions.ToUpperLoca(_.L(data3.ShortDescription, null, false));
                            }
                            else
                            {
                                str2 = MenuHelpers.BigModifierToString(augmentation.PerkInstance.Modifier, true) + "\n" + StringExtensions.ToUpperLoca(_.L(data3.ShortDescription, null, false));
                            }
                            sprite = data3.Sprite;
                        }
                        Card.Content content2 = new Card.Content();
                        content2.Obj = augmentation;
                        content2.Text = str2;
                        content2.Sprite = sprite;
                        content2.Grayscale = true;
                        content2.PriceText = MenuHelpers.BigValueToString(App.Binder.ConfigMeta.GetAugmentationPrice(augmentation.Id));
                        content2.PriceIcon = ConfigUi.RESOURCE_TYPE_SPRITES[ResourceType.Token];
                        Card.Content content = content2;
                        card.initialize(content, new Action<Card>(this.onAugCardClicked));
                        card.gameObject.SetActive(true);
                    }
                }
                for (int num18 = num16; num18 < this.m_augCards.Count; num18++)
                {
                    this.m_augCards[num18].gameObject.SetActive(false);
                }
            }
            this.onRefresh();
        }

        private void reconstructSubContentTournament()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.cleanupTournamentSubTab();
            if (this.m_activeTournamentSubTabIdx == 1)
            {
                this.TournamentSubTabTitle.text = "LEADERBOARD";
                for (int i = 0; i < this.m_activeTournamentView.TournamentEntries.Count; i++)
                {
                    while (i >= this.m_tournamentPlayerCells.Count)
                    {
                        this.addTournamentPlayerCell();
                    }
                    TournamentPlayerCell cell = this.m_tournamentPlayerCells[i];
                    TournamentEntry tournamentEntry = this.m_activeTournamentView.TournamentEntries[i];
                    cell.initialize(tournamentEntry, (i % 2) != 0, i + 1);
                    switch (this.m_activeTournamentView.getLeaderboardRanking(tournamentEntry))
                    {
                        case 0:
                            cell.TopContributorImage.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_contributor_gold");
                            cell.TopContributorImage.enabled = true;
                            break;

                        case 1:
                            cell.TopContributorImage.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_contributor_silver");
                            cell.TopContributorImage.enabled = true;
                            break;

                        case 2:
                            cell.TopContributorImage.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_contributor_bronze");
                            cell.TopContributorImage.enabled = true;
                            break;

                        default:
                            cell.TopContributorImage.enabled = false;
                            break;
                    }
                    cell.GoldenBg.SetActive(tournamentEntry == this.m_activeTournamentView.PlayerEntry);
                    cell.gameObject.SetActive(true);
                }
                for (int j = this.m_activeTournamentView.TournamentEntries.Count; j < this.m_tournamentPlayerCells.Count; j++)
                {
                    this.m_tournamentPlayerCells[j].gameObject.SetActive(false);
                }
            }
            else if (this.m_activeTournamentSubTabIdx == 2)
            {
                <reconstructSubContentTournament>c__AnonStorey2EE storeyee = new <reconstructSubContentTournament>c__AnonStorey2EE();
                this.TournamentSubTabTitle.text = "UPGRADES";
                TournamentUpgrades upgrades = this.m_activeTournamentView.Instance.Upgrades;
                List<string> list = upgrades.getUniqueUpgradesOwned();
                storeyee.sortValues = new Dictionary<string, int>();
                for (int k = 0; k < list.Count; k++)
                {
                    string key = list[k];
                    storeyee.sortValues.Add(key, (((((upgrades.getNumEpicUpgrades(key) * 3) + upgrades.getNumNormalUpgrades(key)) * 10) + upgrades.getNumEpicUpgrades(key)) * 10) + (key.GetHashCode() % 0x3e8));
                }
                list.Sort(new Comparison<string>(storeyee.<>m__19A));
                int num5 = 0;
                for (int m = 0; m < list.Count; m++)
                {
                    string id = list[m];
                    TournamentUpgrade upgrade = App.Binder.ConfigMeta.TOURNAMENT_UPGRADES[id];
                    while (m >= this.m_tournamentUpgradeCells.Count)
                    {
                        this.addTournamentUpgradeCell();
                    }
                    TournamentUpgradeCell cell2 = this.m_tournamentUpgradeCells[m];
                    ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[upgrade.PerkType];
                    cell2.Title.text = StringExtensions.ToLowerLoca(_.L(data.ShortDescription, null, false));
                    float num7 = upgrades.getTotalNormalModifierForUpgrade(id);
                    float num8 = upgrades.getTotalEpicModifierForUpgrade(id);
                    float num9 = num7 + num8;
                    cell2.Text.text = MenuHelpers.BigModifierToString(num9, true);
                    cell2.NormalCount.text = "\x00d7" + upgrades.getNumNormalUpgrades(id).ToString();
                    cell2.EpicCount.text = "\x00d7" + upgrades.getNumEpicUpgrades(id).ToString();
                    cell2.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(data.Sprite);
                    cell2.Bg.color = ((m % 2) != 0) ? ConfigUi.LIST_CELL_STRIPED_COLOR : ConfigUi.LIST_CELL_REGULAR_COLOR;
                    cell2.gameObject.SetActive(true);
                    num5++;
                    if (num5 <= 3)
                    {
                        this.DonateCell.transform.SetSiblingIndex(cell2.transform.GetSiblingIndex());
                    }
                }
                for (int n = list.Count; n < this.m_tournamentUpgradeCells.Count; n++)
                {
                    this.m_tournamentUpgradeCells[n].gameObject.SetActive(false);
                }
                this.TouranmentUpgradeInfoCell.gameObject.SetActive(true);
                this.TouranmentUpgradeInfoCell.Background.color = ((num5 % 2) != 0) ? ConfigUi.LIST_CELL_STRIPED_COLOR : ConfigUi.LIST_CELL_REGULAR_COLOR;
                this.TouranmentUpgradeInfoCell.transform.SetAsLastSibling();
                num5++;
            }
            else if (this.m_activeTournamentSubTabIdx == 3)
            {
                this.TournamentSubTabTitle.text = "JOURNAL";
                for (int num11 = 0; num11 < this.m_tournamentLogCells.Count; num11++)
                {
                    bool striped = (num11 % 2) == 0;
                    this.m_tournamentLogCells[num11].initialize(striped);
                }
            }
            else if (this.m_activeTournamentSubTabIdx == 0)
            {
                this.TournamentSubTabTitle.text = "INFO";
                for (int num12 = 0; num12 < this.TournamentInfoCells.Count; num12++)
                {
                    this.TournamentInfoCells[num12].gameObject.SetActive(true);
                }
                string str3 = string.Empty;
                DungeonRuleset rulesetForId = ConfigDungeonModifiers.GetRulesetForId(this.m_activeTournamentView.TournamentInfo.TournamentRulesetId);
                if ((rulesetForId != null) && (rulesetForId.DungeonModifiers.Count > 0))
                {
                    for (int num13 = 0; num13 < rulesetForId.DungeonModifiers.Count; num13++)
                    {
                        DungeonModifierType type = rulesetForId.DungeonModifiers[num13];
                        DungeonModifier modifier = ConfigDungeonModifiers.MODIFIERS[type];
                        if (!string.IsNullOrEmpty(modifier.Description))
                        {
                            str3 = str3 + ("- " + _.L(modifier.Description, null, false) + "\n");
                        }
                    }
                }
                else
                {
                    str3 = "- " + _.L(ConfigLoca.BH_ANNOUNCEMENT_NO_ADDED_DIFFICULTY, null, false);
                }
                this.TournamentInfoCells[2].Text2.text = str3;
                long totalContribution = this.m_activeTournamentView.GetTotalContribution();
                for (int num15 = 0; num15 < this.m_activeTournamentView.TournamentInfo.RewardMilestones.Count; num15++)
                {
                    while (num15 >= this.m_tournamentMilestoneCells.Count)
                    {
                        this.addTournamentMilestoneCell();
                    }
                    TournamentMilestoneCell cell3 = this.m_tournamentMilestoneCells[num15];
                    RewardMilestone milestone = this.m_activeTournamentView.TournamentInfo.RewardMilestones[num15];
                    cell3.Text.text = milestone.Threshold.ToString();
                    cell3.Rewards[0].Text.text = "\x00d7" + milestone.CardPackAmount;
                    cell3.Rewards[0].gameObject.SetActive(true);
                    cell3.Rewards[1].Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(ConfigUi.GetFloaterSpriteForShopEntry(milestone.MainReward.Id));
                    ShopEntry shopEntry = ConfigShops.GetShopEntry(milestone.MainReward.Id);
                    if (shopEntry.Type == ShopEntryType.TokenBundle)
                    {
                        double v = ConfigShops.CalculateTokenBundleSize(player, shopEntry.Id);
                        cell3.Rewards[1].Text.text = "\x00d7" + MenuHelpers.BigValueToString(v);
                    }
                    else
                    {
                        cell3.Rewards[1].Text.text = "\x00d7" + milestone.MainReward.Amount;
                    }
                    cell3.Rewards[1].gameObject.SetActive(true);
                    cell3.Rewards[2].Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(ConfigUi.GetFloaterSpriteForShopEntry(milestone.MainReward.Id));
                    RewardMilestone.Entry entry3 = milestone.ContributorRewards[milestone.ContributorRewards.Count - 1];
                    Character character = GameLogic.Binder.CharacterResources.getResource(entry3.Id);
                    cell3.Rewards[2].Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(character.AvatarSprite);
                    cell3.Rewards[2].Text.text = "\x00d7?";
                    cell3.Rewards[2].gameObject.SetActive(true);
                    cell3.Bg.color = ((num15 % 2) != 0) ? ConfigUi.LIST_CELL_STRIPED_COLOR : ConfigUi.LIST_CELL_REGULAR_COLOR;
                    if (totalContribution < milestone.Threshold)
                    {
                        cell3.CanvasGroup.alpha = 0.33f;
                        cell3.Tickmark.enabled = false;
                    }
                    else
                    {
                        cell3.CanvasGroup.alpha = 1f;
                        cell3.Tickmark.enabled = true;
                    }
                    cell3.gameObject.SetActive(true);
                }
                for (int num17 = this.m_activeTournamentView.TournamentInfo.RewardMilestones.Count; num17 < this.m_tournamentMilestoneCells.Count; num17++)
                {
                    this.m_tournamentMilestoneCells[num17].gameObject.SetActive(false);
                }
            }
            this.onRefresh();
        }

        public void reconstructTabAdventure()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (player.Tournaments.hasTournamentSelected())
            {
                this.ConfirmationPanelConfirmPopupTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_CONFIRMATION_ADVENTURE_CONTINUE, null, false));
                this.ConfirmationPanelConfirmPopupDescription.text = _.L(ConfigLoca.ADVPANEL_CONFIRMATION_DESCRIPTION2, null, false);
                this.ConfirmationPanelConfirmPopupButton.Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_CONTINUE, null, false));
                this.ConfirmationPanelInfoPopupSpinner.SetActive(false);
                this.ConfirmationPanelInfoPopupRoot.SetActive(false);
                this.ConfirmationPanelConfirmPopupRoot.SetActive(true);
                this.ConfirmationPanelRoot.SetActive(true);
                this.AdventureContentRoot.SetActive(false);
            }
            else
            {
                double num;
                double num2;
                double num3;
                this.ConfirmationPanelRoot.SetActive(false);
                this.AdventureContentRoot.SetActive(true);
                double v = player.getTotalTokensFromRetirement(out num, out num2, out num3);
                if (v > 0.0)
                {
                    this.AdventureMilestoneRewards[0].Text.text = "\x00d7" + MenuHelpers.BigValueToString(v);
                    this.AdventureMilestoneRewards[0].CanvasGroup.alpha = 1f;
                }
                else
                {
                    this.AdventureMilestoneRewards[0].Text.text = "\x00d70";
                    this.AdventureMilestoneRewards[0].CanvasGroup.alpha = 0.333f;
                }
                int num5 = 0;
                for (int i = 0; i < player.UnclaimedRewards.Count; i++)
                {
                    if (player.UnclaimedRewards[i].ChestType == ChestType.RewardBoxCommon)
                    {
                        num5++;
                    }
                }
                if (num5 > 0)
                {
                    this.AdventureMilestoneRewards[1].Text.text = "\x00d7" + num5;
                    this.AdventureMilestoneRewards[1].CanvasGroup.alpha = 1f;
                }
                else
                {
                    this.AdventureMilestoneRewards[1].Text.text = "\x00d70";
                    this.AdventureMilestoneRewards[1].CanvasGroup.alpha = 0.333f;
                }
                Reward reward = player.getFirstUnclaimedRetirementTriggerChest();
                int num7 = (reward == null) ? 0 : reward.FrenzyPotions;
                if (num7 > 0)
                {
                    this.AdventureMilestoneRewards[2].Text.text = "\x00d7" + num7;
                    this.AdventureMilestoneRewards[2].CanvasGroup.alpha = 1f;
                }
                else
                {
                    this.AdventureMilestoneRewards[2].Text.text = "\x00d70";
                    this.AdventureMilestoneRewards[2].CanvasGroup.alpha = 0.333f;
                }
                player.Notifiers.HeroRetirementsInspected = true;
                if (App.Binder.ConfigMeta.SELL_AUGMENTATIONS_IN_ADVENTURE_PANEL)
                {
                    this.AdventureSubTabButtons[2].gameObject.SetActive(true);
                }
                else
                {
                    this.AdventureSubTabButtons[2].gameObject.SetActive(false);
                    if (this.m_activeAdventureSubTabIdx == 2)
                    {
                        this.m_activeAdventureSubTabIdx = 0;
                    }
                    if (this.m_inputParams.OverrideOpenAdventureSubTabIndex.HasValue && (this.m_inputParams.OverrideOpenAdventureSubTabIndex.Value == 2))
                    {
                        this.m_inputParams.OverrideOpenAdventureSubTabIndex = 0;
                    }
                }
                if (this.m_inputParams.OverrideOpenAdventureSubTabIndex.HasValue)
                {
                    this.setActiveAdventureSubTabIndex(this.m_inputParams.OverrideOpenAdventureSubTabIndex.Value);
                }
                else if (this.m_activeAdventureSubTabIdx == -1)
                {
                    this.setActiveAdventureSubTabIndex(0);
                }
                else
                {
                    this.setActiveAdventureSubTabIndex(this.m_activeAdventureSubTabIdx);
                }
            }
        }

        [DebuggerHidden]
        private IEnumerator reconstructTabTournamentRoutine()
        {
            <reconstructTabTournamentRoutine>c__Iterator186 iterator = new <reconstructTabTournamentRoutine>c__Iterator186();
            iterator.<>f__this = this;
            return iterator;
        }

        private void refreshActiveTournamentView()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.m_activeTournamentView = null;
            if (player.Tournaments.hasTournamentSelected())
            {
                this.m_activeTournamentView = Service.Binder.TournamentSystem.GetTournamentView(player.Tournaments.SelectedTournamentId);
            }
            else if (player.Tournaments.LastUnselectedTournamentId != null)
            {
                this.m_activeTournamentView = Service.Binder.TournamentSystem.GetTournamentView(player.Tournaments.LastUnselectedTournamentId);
            }
            else
            {
                this.m_activeTournamentView = Service.Binder.TournamentSystem.NextAvailableViewOrNull();
            }
        }

        public void setActiveAdventureSubTabIndex(int idx)
        {
            if (this.m_activeAdventureSubTabIdx != idx)
            {
                if (this.m_activeAdventureSubTabIdx != -1)
                {
                    this.AdventureSubTabButtons[this.m_activeAdventureSubTabIdx].Background.sprite = this.m_inactiveTabBgSprite;
                }
                this.m_activeAdventureSubTabIdx = idx;
                this.AdventureSubTabButtons[this.m_activeAdventureSubTabIdx].Background.sprite = this.m_activeTabBgSprite;
            }
            this.reconstructSubContentAdventure();
        }

        public void setActiveTabIndex(int idx)
        {
            if (this.m_activeTabIdx != idx)
            {
                this.markActiveTabContentAsInspected();
                if (this.m_activeTabIdx != -1)
                {
                    this.TabButtons[this.m_activeTabIdx].Background.sprite = this.m_inactiveTabBgSprite;
                }
                this.m_activeTabIdx = idx;
                this.TabButtons[this.m_activeTabIdx].Background.sprite = this.m_activeTabBgSprite;
            }
            this.setTabInteractable(this.m_activeTabIdx, true);
            this.reconstructContent();
        }

        public void setActiveTournamentSubTabIndex(int idx)
        {
            if (this.m_activeTournamentSubTabIdx != idx)
            {
                if (this.m_activeTournamentSubTabIdx != -1)
                {
                    this.TournamentSubTabButtons[this.m_activeTournamentSubTabIdx].Background.sprite = this.m_inactiveTabBgSprite;
                }
                this.m_activeTournamentSubTabIdx = idx;
                this.TournamentSubTabButtons[this.m_activeTournamentSubTabIdx].Background.sprite = this.m_activeTabBgSprite;
            }
            this.reconstructSubContentTournament();
        }

        public void setTabInteractable(int idx, bool interactable)
        {
            if (interactable)
            {
                this.TabButtons[idx].Button.interactable = true;
                this.TabButtons[idx].Background.material = null;
            }
            else
            {
                this.TabButtons[idx].Button.interactable = false;
                this.TabButtons[idx].Background.material = PlayerView.Binder.DisabledUiMaterial;
            }
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator184 iterator = new <showRoutine>c__Iterator184();
            iterator.<>f__this = this;
            return iterator;
        }

        public void switchAdventure()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition && !PlayerView.Binder.TransitionSystem.InCriticalTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                if (player.Tournaments.hasTournamentSelected())
                {
                    PlayerView.Binder.TransitionSystem.switchAdventure(null, StringExtensions.ToUpperLoca(_.L(ConfigLoca.BH_SWITCH_TO_ADVENTURE, null, false)));
                }
                else if (this.m_activeTournamentView != null)
                {
                    if (!player.Tournaments.tournamentIsActive(this.m_activeTournamentView.TournamentInfo.Id))
                    {
                        Service.Binder.TournamentSystem.JoinTournament(this.m_activeTournamentView.TournamentInfo.Id);
                    }
                    PlayerView.Binder.TransitionSystem.switchAdventure(this.m_activeTournamentView.TournamentInfo.Id, StringExtensions.ToUpperLoca(_.L(ConfigLoca.BH_SWITCH_TO_BOSS_HUNT, null, false)));
                }
            }
        }

        public override PlayerView.MenuType MenuType
        {
            get
            {
                return PlayerView.MenuType.SlidingAdventurePanel;
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
        private sealed class <hideRoutine>c__Iterator185 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal SlidingAdventurePanel <>f__this;
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
                        this.<>f__this.cleanupCells();
                        GameLogic.Binder.EventBus.OnResourcesGained -= new GameLogic.Events.ResourcesGained(this.<>f__this.onResourcesGained);
                        GameLogic.Binder.EventBus.OnTournamentDonationMade -= new GameLogic.Events.TournamentDonationMade(this.<>f__this.onTournamentDonationMade);
                        GameLogic.Binder.EventBus.OnPlayerAugmentationGained -= new GameLogic.Events.PlayerAugmentationGained(this.<>f__this.onPlayerAugmentationGained);
                        PlayerView.Binder.EventBus.OnGestureTapRecognized -= new PlayerView.Events.GestureTapRecognized(this.<>f__this.onGestureTapRecognized);
                        Service.Binder.EventBus.OnLocalTournamentViewsRefreshed -= new Service.Events.LocalTournamentViewsRefreshed(this.<>f__this.onLocalTournamentViewsRefreshed);
                        if (PlayerView.Binder.SlidingAdventurePanelController.PanningActive)
                        {
                            goto Label_0173;
                        }
                        this.<duration>__0 = !this.instant ? ConfigUi.SLIDING_PANEL_EXIT_DURATION : 0f;
                        this.<>f__this.PanelRoot.close(this.<duration>__0, !PlayerView.Binder.SlidingAdventurePanelController.LastClosingTriggeredFromSwipe ? Easing.Function.IN_CUBIC : Easing.Function.OUT_CUBIC, 0f);
                        if (this.instant)
                        {
                            goto Label_015C;
                        }
                        PlayerView.Binder.AudioSystem.playSfxGrp(AudioGroupType.SfxGrpUi_BookClose, (float) 0f);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0173;
                }
                if (this.<>f__this.PanelRoot.Animating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
            Label_015C:
                PlayerView.Binder.SlidingAdventurePanelController.LastClosingTriggeredFromSwipe = false;
                goto Label_0173;
                this.$PC = -1;
            Label_0173:
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
        private sealed class <preShowRoutine>c__Iterator183 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal SlidingAdventurePanel <>f__this;
            internal Player <player>__0;
            internal object parameter;

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
                    if (this.parameter != null)
                    {
                        this.<>f__this.m_inputParams = (SlidingAdventurePanel.InputParameters) this.parameter;
                    }
                    else
                    {
                        this.<>f__this.m_inputParams = new SlidingAdventurePanel.InputParameters();
                    }
                    GameLogic.Binder.EventBus.OnResourcesGained += new GameLogic.Events.ResourcesGained(this.<>f__this.onResourcesGained);
                    GameLogic.Binder.EventBus.OnTournamentDonationMade += new GameLogic.Events.TournamentDonationMade(this.<>f__this.onTournamentDonationMade);
                    GameLogic.Binder.EventBus.OnPlayerAugmentationGained += new GameLogic.Events.PlayerAugmentationGained(this.<>f__this.onPlayerAugmentationGained);
                    PlayerView.Binder.EventBus.OnGestureTapRecognized += new PlayerView.Events.GestureTapRecognized(this.<>f__this.onGestureTapRecognized);
                    Service.Binder.EventBus.OnLocalTournamentViewsRefreshed += new Service.Events.LocalTournamentViewsRefreshed(this.<>f__this.onLocalTournamentViewsRefreshed);
                    this.<>f__this.PanelRoot.close(0f, Easing.Function.LINEAR, 0f);
                    if (this.<>f__this.m_inputParams.OverrideOpenTabIndex.HasValue)
                    {
                        this.<>f__this.setActiveTabIndex(this.<>f__this.m_inputParams.OverrideOpenTabIndex.Value);
                    }
                    else if (this.<>f__this.m_activeTabIdx == -1)
                    {
                        this.<player>__0 = GameLogic.Binder.GameState.Player;
                        if (this.<player>__0.Tournaments.hasTournamentSelected())
                        {
                            this.<>f__this.setActiveTabIndex(1);
                        }
                        else
                        {
                            this.<>f__this.setActiveTabIndex(0);
                        }
                    }
                    else
                    {
                        this.<>f__this.setActiveTabIndex(this.<>f__this.m_activeTabIdx);
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

        [CompilerGenerated]
        private sealed class <reconstructSubContentTournament>c__AnonStorey2EE
        {
            internal Dictionary<string, int> sortValues;

            internal int <>m__19A(string a, string b)
            {
                int num = this.sortValues[b];
                return num.CompareTo(this.sortValues[a]);
            }
        }

        [CompilerGenerated]
        private sealed class <reconstructTabTournamentRoutine>c__Iterator186 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            private static Comparison<RewardMilestone> <>f__am$cache13;
            internal SlidingAdventurePanel <>f__this;
            internal double <amount>__7;
            internal RewardMilestone <nextMilestone>__5;
            internal Character <pet>__10;
            internal Character <pet>__13;
            internal Player <player>__0;
            internal bool <playerHasCompletedAtLeastOneTournament>__2;
            internal bool <playerHasJoinedToActiveTournament>__1;
            internal int <playerRanking>__8;
            internal int <ranking>__11;
            internal RewardMilestone.Entry <rewardEntry>__9;
            internal long <secondsUntilEnd>__15;
            internal ShopEntry <shopEntry>__6;
            internal RewardMilestone.Entry <topReward>__12;
            internal long <totalContribution>__4;
            internal TournamentInfo <tournamentInfo>__3;
            internal float <v>__14;

            private static int <>m__19B(RewardMilestone rm1, RewardMilestone rm2)
            {
                return rm1.Threshold.CompareTo(rm2.Threshold);
            }

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
                        this.<>f__this.cleanupCells();
                        if (this.<player>__0.Tournaments.tournamentsUnlocked())
                        {
                            if (Service.Binder.TournamentSystem.Synchronized)
                            {
                                goto Label_01FC;
                            }
                            this.<>f__this.ConfirmationPanelInfoPopupDescription.gameObject.SetActive(false);
                            this.<>f__this.ConfirmationPanelInfoPopupSpinner.SetActive(true);
                            this.<>f__this.ConfirmationPanelConfirmPopupRoot.SetActive(false);
                            this.<>f__this.ConfirmationPanelInfoPopupRoot.SetActive(true);
                            this.<>f__this.ConfirmationPanelRoot.SetActive(true);
                            this.<>f__this.TournamentContentRoot.SetActive(false);
                            break;
                        }
                        this.<>f__this.ConfirmationPanelInfoPopupTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_CONFIRMATION_BH_LOCKED, null, false));
                        this.<>f__this.ConfirmationPanelInfoPopupDescription.text = _.L(ConfigLoca.ADVPANEL_CONFIRMATION_BH_NEXT_NOTIFIED, null, false);
                        this.<>f__this.ConfirmationPanelInfoPopupDescription.gameObject.SetActive(true);
                        this.<>f__this.ConfirmationPanelInfoPopupSpinner.SetActive(false);
                        this.<>f__this.ConfirmationPanelRoot.SetActive(true);
                        this.<>f__this.ConfirmationPanelConfirmPopupRoot.SetActive(false);
                        this.<>f__this.ConfirmationPanelInfoPopupRoot.SetActive(true);
                        this.<>f__this.TournamentContentRoot.SetActive(false);
                        this.<>f__this.m_tournamentTabReconstructRoutine = null;
                        goto Label_0E7A;

                    case 1:
                        break;

                    default:
                        goto Label_0E7A;
                }
                while (!Service.Binder.TournamentSystem.Synchronized)
                {
                    if (Service.Binder.ServiceWatchdog.IsOnline)
                    {
                        this.<>f__this.ConfirmationPanelInfoPopupTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_CONFIRMATION_BH_LOADING, null, false));
                    }
                    else
                    {
                        this.<>f__this.ConfirmationPanelInfoPopupTitle.text = _.L(ConfigLoca.UI_STATUS_CONNECTING, null, false);
                    }
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
            Label_01FC:
                this.<>f__this.refreshActiveTournamentView();
                if ((this.<>f__this.m_activeTournamentView != null) && PlayerView.Binder.NotificationSystem.NotifyAdventurePanelTournamentTab)
                {
                    this.<player>__0.Tournaments.markTournamentAsNotified(this.<>f__this.m_activeTournamentView.TournamentInfo.Id);
                }
                PlayerView.Binder.NotificationSystem.NotifyAdventurePanelTournamentTab = false;
                this.<playerHasJoinedToActiveTournament>__1 = (this.<>f__this.m_activeTournamentView != null) && this.<>f__this.m_activeTournamentView.PlayerHasJoined;
                this.<playerHasCompletedAtLeastOneTournament>__2 = this.<player>__0.Tournaments.CompletedTournamentHistory.Count > 0;
                if (((this.<>f__this.m_activeTournamentView != null) && (this.<>f__this.m_activeTournamentView.ServerJoinStatus == TournamentViewRemote.Status.OK)) && !this.<player>__0.Tournaments.hasTournamentSelected())
                {
                    if (this.<playerHasJoinedToActiveTournament>__1)
                    {
                        this.<>f__this.ConfirmationPanelConfirmPopupTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_CONFIRMATION_BH_CONTINUE, null, false));
                        this.<>f__this.ConfirmationPanelConfirmPopupButton.Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_CONTINUE, null, false));
                        this.<>f__this.ConfirmationPanelConfirmPopupDescription.text = _.L(ConfigLoca.ADVPANEL_CONFIRMATION_DESCRIPTION1, null, false);
                    }
                    else
                    {
                        this.<>f__this.ConfirmationPanelConfirmPopupTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_CONFIRMATION_BH_JOIN, null, false));
                        this.<>f__this.ConfirmationPanelConfirmPopupButton.Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_JOIN, null, false));
                        this.<>f__this.ConfirmationPanelConfirmPopupDescription.text = _.L(ConfigLoca.ADVPANEL_CONFIRMATION_DESCRIPTION1, null, false);
                    }
                    this.<>f__this.ConfirmationPanelConfirmPopupRoot.SetActive(true);
                    this.<>f__this.ConfirmationPanelInfoPopupRoot.SetActive(false);
                    this.<>f__this.ConfirmationPanelRoot.SetActive(true);
                    this.<>f__this.TournamentContentRoot.SetActive(false);
                    this.<>f__this.m_tournamentTabReconstructRoutine = null;
                }
                else if (!this.<player>__0.Tournaments.hasTournamentSelected() && ((this.<>f__this.m_activeTournamentView == null) || (this.<>f__this.m_activeTournamentView.ServerJoinStatus == TournamentViewRemote.Status.TooEarlyForJoin)))
                {
                    if (this.<playerHasCompletedAtLeastOneTournament>__2)
                    {
                        this.<>f__this.ConfirmationPanelInfoPopupTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_CONFIRMATION_BH_COMPLETED, null, false));
                    }
                    else
                    {
                        this.<>f__this.ConfirmationPanelInfoPopupTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_CONFIRMATION_BH_NOT_AVAILABLE, null, false));
                    }
                    if ((this.<>f__this.m_activeTournamentView != null) && (this.<>f__this.m_activeTournamentView.ServerJoinStatus == TournamentViewRemote.Status.TooEarlyForJoin))
                    {
                        this.<>f__this.ConfirmationPanelInfoPopupDescription.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_CONFIRMATION_BH_AVAILABLE_IN, null, false));
                        this.<>f__this.ConfirmationPanelInfoPopupDescription.gameObject.SetActive(true);
                        this.<>f__this.ConfirmationPanelInfoPopupDescriptionTime.text = MenuHelpers.SecondsToStringDaysHoursMinutes(this.<>f__this.m_activeTournamentView.TournamentInfo.getSecondsUntilAvailable(), false);
                        this.<>f__this.ConfirmationPanelInfoPopupDescriptionTime.gameObject.SetActive(true);
                    }
                    else
                    {
                        this.<>f__this.ConfirmationPanelInfoPopupTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ADVPANEL_CONFIRMATION_BH_NOT_AVAILABLE, null, false));
                        this.<>f__this.ConfirmationPanelInfoPopupDescription.text = _.L(ConfigLoca.ADVPANEL_CONFIRMATION_BH_NEXT_NOTIFIED, null, false);
                        this.<>f__this.ConfirmationPanelInfoPopupDescription.gameObject.SetActive(true);
                        this.<>f__this.ConfirmationPanelInfoPopupDescriptionTime.gameObject.SetActive(false);
                    }
                    this.<>f__this.ConfirmationPanelInfoPopupSpinner.SetActive(false);
                    this.<>f__this.ConfirmationPanelRoot.SetActive(true);
                    this.<>f__this.ConfirmationPanelConfirmPopupRoot.SetActive(false);
                    this.<>f__this.ConfirmationPanelInfoPopupRoot.SetActive(true);
                    this.<>f__this.TournamentContentRoot.SetActive(false);
                    this.<>f__this.m_tournamentTabReconstructRoutine = null;
                }
                else
                {
                    this.<>f__this.ConfirmationPanelRoot.SetActive(false);
                    this.<>f__this.TournamentContentRoot.SetActive(true);
                    this.<tournamentInfo>__3 = this.<>f__this.m_activeTournamentView.TournamentInfo;
                    if (<>f__am$cache13 == null)
                    {
                        <>f__am$cache13 = new Comparison<RewardMilestone>(SlidingAdventurePanel.<reconstructTabTournamentRoutine>c__Iterator186.<>m__19B);
                    }
                    this.<tournamentInfo>__3.RewardMilestones.Sort(<>f__am$cache13);
                    this.<totalContribution>__4 = this.<>f__this.m_activeTournamentView.GetTotalContribution();
                    this.<nextMilestone>__5 = this.<>f__this.m_activeTournamentView.getNextMilestone();
                    if (this.<nextMilestone>__5 == null)
                    {
                        this.<>f__this.MilestoneHeaderText.text = _.L(ConfigLoca.ADVPANEL_ALL_MILESTONES_COMPLETED, new <>__AnonType23<int>(this.<>f__this.m_activeTournamentView.getMilestoneCount()), false);
                        this.<>f__this.MilestoneCardReward.gameObject.SetActive(false);
                        this.<>f__this.MilestoneMainReward.gameObject.SetActive(false);
                        this.<>f__this.MilestoneTopContributorReward.gameObject.SetActive(false);
                        this.<>f__this.TournamentMilestoneProgressBar.setNormalizedValue(1f);
                        this.<>f__this.TournamentMilestoneProgressBarText.text = this.<totalContribution>__4 + " / " + this.<tournamentInfo>__3.RewardMilestones[this.<tournamentInfo>__3.RewardMilestones.Count - 1].Threshold;
                    }
                    else
                    {
                        this.<>f__this.MilestoneHeaderText.text = _.L(ConfigLoca.ADVPANEL_MILESTONE_TITLE, new <>__AnonType22<int, int>(this.<>f__this.m_activeTournamentView.getMilestoneNumber(this.<nextMilestone>__5), this.<>f__this.m_activeTournamentView.getMilestoneCount()), false);
                        if (this.<nextMilestone>__5.CardPackAmount > 0)
                        {
                            this.<>f__this.MilestoneCardReward.Amount.text = "\x00d7" + this.<nextMilestone>__5.CardPackAmount;
                            this.<>f__this.MilestoneCardReward.CanvasGroup.alpha = 1f;
                        }
                        else
                        {
                            this.<>f__this.MilestoneCardReward.Amount.text = "\x00d70";
                            this.<>f__this.MilestoneCardReward.CanvasGroup.alpha = 0.333f;
                        }
                        this.<>f__this.MilestoneCardReward.gameObject.SetActive(true);
                        this.<>f__this.MilestoneMainReward.RewardIcon.sprite = PlayerView.Binder.SpriteResources.getSprite(ConfigUi.GetFloaterSpriteForShopEntry(this.<nextMilestone>__5.MainReward.Id));
                        this.<shopEntry>__6 = ConfigShops.GetShopEntry(this.<nextMilestone>__5.MainReward.Id);
                        if (this.<shopEntry>__6.Type == ShopEntryType.TokenBundle)
                        {
                            this.<amount>__7 = ConfigShops.CalculateTokenBundleSize(this.<player>__0, this.<shopEntry>__6.Id);
                        }
                        else
                        {
                            this.<amount>__7 = this.<nextMilestone>__5.MainReward.Amount;
                        }
                        if (this.<amount>__7 > 0.0)
                        {
                            this.<>f__this.MilestoneMainReward.Amount.text = "\x00d7" + MenuHelpers.BigValueToString(this.<amount>__7);
                            this.<>f__this.MilestoneMainReward.CanvasGroup.alpha = 1f;
                        }
                        else
                        {
                            this.<>f__this.MilestoneMainReward.Amount.text = "\x00d70";
                            this.<>f__this.MilestoneMainReward.CanvasGroup.alpha = 0.333f;
                        }
                        this.<>f__this.MilestoneMainReward.gameObject.SetActive(true);
                        this.<playerRanking>__8 = this.<>f__this.m_activeTournamentView.getLeaderboardRanking(this.<>f__this.m_activeTournamentView.PlayerEntry);
                        if ((this.<playerRanking>__8 == -1) || (this.<playerRanking>__8 >= this.<nextMilestone>__5.ContributorRewards.Count))
                        {
                            if (this.<nextMilestone>__5.ContributorRewards.Count > 0)
                            {
                                this.<topReward>__12 = this.<nextMilestone>__5.ContributorRewards[0];
                                this.<pet>__13 = GameLogic.Binder.CharacterResources.getResource(this.<topReward>__12.Id);
                                this.<>f__this.MilestoneTopContributorReward.RewardIcon.sprite = PlayerView.Binder.SpriteResources.getSprite(this.<pet>__13.AvatarSprite);
                                this.<>f__this.MilestoneTopContributorReward.Amount.text = "\x00d70";
                                this.<>f__this.MilestoneTopContributorReward.BonusIcon.material = PlayerView.Binder.DisabledUiMaterial;
                                this.<>f__this.MilestoneTopContributorReward.CanvasGroup.alpha = 0.333f;
                            }
                            else
                            {
                                this.<>f__this.MilestoneTopContributorReward.gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            this.<rewardEntry>__9 = this.<nextMilestone>__5.ContributorRewards[this.<playerRanking>__8];
                            if (!GameLogic.Binder.CharacterResources.hasCharacter(this.<rewardEntry>__9.Id))
                            {
                                UnityEngine.Debug.LogError("Tournament milestone contributor reward id is assumed to be a pet id: " + this.<rewardEntry>__9.Id);
                                this.<>f__this.MilestoneTopContributorReward.gameObject.SetActive(false);
                            }
                            else
                            {
                                this.<pet>__10 = GameLogic.Binder.CharacterResources.getResource(this.<rewardEntry>__9.Id);
                                this.<>f__this.MilestoneTopContributorReward.RewardIcon.sprite = PlayerView.Binder.SpriteResources.getSprite(this.<pet>__10.AvatarSprite);
                                this.<>f__this.MilestoneTopContributorReward.Amount.text = "\x00d7" + this.<rewardEntry>__9.Amount;
                                this.<ranking>__11 = this.<>f__this.m_activeTournamentView.getLeaderboardRanking(this.<>f__this.m_activeTournamentView.PlayerEntry);
                                switch (this.<ranking>__11)
                                {
                                    case 0:
                                        this.<>f__this.MilestoneTopContributorReward.BonusIcon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_contributor_gold");
                                        break;

                                    case 1:
                                        this.<>f__this.MilestoneTopContributorReward.BonusIcon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_contributor_silver");
                                        break;

                                    case 2:
                                        this.<>f__this.MilestoneTopContributorReward.BonusIcon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_contributor_bronze");
                                        break;

                                    default:
                                        this.<>f__this.MilestoneTopContributorReward.BonusIcon.sprite = null;
                                        break;
                                }
                                this.<>f__this.MilestoneTopContributorReward.BonusIcon.material = null;
                                this.<>f__this.MilestoneTopContributorReward.CanvasGroup.alpha = 1f;
                                this.<>f__this.MilestoneTopContributorReward.gameObject.SetActive(true);
                            }
                        }
                        this.<v>__14 = Mathf.Clamp01(((float) this.<totalContribution>__4) / ((float) this.<nextMilestone>__5.Threshold));
                        this.<>f__this.TournamentMilestoneProgressBar.setNormalizedValue(this.<v>__14);
                        this.<>f__this.TournamentMilestoneProgressBarText.text = this.<totalContribution>__4 + " / " + this.<nextMilestone>__5.Threshold;
                    }
                    this.<secondsUntilEnd>__15 = this.<>f__this.m_activeTournamentView.GetSecondsUntilEnd();
                    this.<>f__this.TournamentTimeRemainingText.text = (this.<secondsUntilEnd>__15 < 0L) ? "-" : MenuHelpers.SecondsToStringHoursMinutes(this.<secondsUntilEnd>__15);
                    if (this.<>f__this.m_inputParams.OverrideOpenTournamentSubTabIndex.HasValue)
                    {
                        this.<>f__this.setActiveTournamentSubTabIndex(this.<>f__this.m_inputParams.OverrideOpenTournamentSubTabIndex.Value);
                    }
                    else if ((this.<>f__this.m_activeTournamentView.Instance != null) && (this.<>f__this.m_activeTournamentView.Instance.LastAdventurePanelSubTabIdx == -1))
                    {
                        this.<>f__this.setActiveTournamentSubTabIndex(0);
                        this.<>f__this.m_activeTournamentView.Instance.LastAdventurePanelSubTabIdx = 0;
                    }
                    else if (this.<>f__this.m_activeTournamentSubTabIdx == -1)
                    {
                        this.<>f__this.setActiveTournamentSubTabIndex(1);
                    }
                    else
                    {
                        this.<>f__this.setActiveTournamentSubTabIndex(this.<>f__this.m_activeTournamentSubTabIdx);
                    }
                    this.<>f__this.m_tournamentTabReconstructRoutine = null;
                    this.$PC = -1;
                }
            Label_0E7A:
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
        private sealed class <showRoutine>c__Iterator184 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal SlidingAdventurePanel <>f__this;

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
                        if (PlayerView.Binder.SlidingAdventurePanelController.PanningActive)
                        {
                            goto Label_0095;
                        }
                        this.<>f__this.PanelRoot.open(ConfigUi.SLIDING_PANEL_ENTRY_DURATION, Easing.Function.OUT_CUBIC, 0f);
                        PlayerView.Binder.AudioSystem.playSfxGrp(AudioGroupType.SfxGrpUi_BookOpen, (float) 0f);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0095;
                }
                if (this.<>f__this.PanelRoot.Animating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                goto Label_0095;
                this.$PC = -1;
            Label_0095:
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

        public enum ContentTarget
        {
            UNSPECIFIED,
            AdventureTabButton,
            TournamentTabButton,
            AscendButton,
            ConfirmationButton,
            AdventureShopSubTab,
            FirstAugmentationCell
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public int? OverrideOpenTabIndex;
            public int? OverrideOpenAdventureSubTabIndex;
            public int? OverrideOpenTournamentSubTabIndex;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PlayerAugEntry
        {
            public Sprite Icon;
            public string Title;
            public string Text;
            public string CornerText;
            [CompilerGenerated]
            private int <OrderNo>k__BackingField;
            [CompilerGenerated]
            private int <Count>k__BackingField;
            public int OrderNo
            {
                [CompilerGenerated]
                get
                {
                    return this.<OrderNo>k__BackingField;
                }
                [CompilerGenerated]
                set
                {
                    this.<OrderNo>k__BackingField = value;
                }
            }
            public int Count
            {
                [CompilerGenerated]
                get
                {
                    return this.<Count>k__BackingField;
                }
                [CompilerGenerated]
                set
                {
                    this.<Count>k__BackingField = value;
                }
            }
            public static int SortByCount(SlidingAdventurePanel.PlayerAugEntry x, SlidingAdventurePanel.PlayerAugEntry y)
            {
                int num = x.Count.CompareTo(y.Count);
                if (num != 0)
                {
                    return -num;
                }
                return x.OrderNo.CompareTo(y.OrderNo);
            }
        }
    }
}

