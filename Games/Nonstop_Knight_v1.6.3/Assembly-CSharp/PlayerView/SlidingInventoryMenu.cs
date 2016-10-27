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

    public class SlidingInventoryMenu : Menu
    {
        public const int ARMORS_TAB_INDEX = 1;
        public const int CLOAKS_TAB_INDEX = 2;
        public Button CloseButton;
        public RectTransform GridGroup;
        public Text InfoBarTitle;
        private int m_activeTabIdx = -1;
        private List<ItemCell> m_itemCells = new List<ItemCell>();
        private List<PotionCell> m_potionCells = new List<PotionCell>();
        private bool m_queuedReconstruct;
        private bool m_queuedRefresh;
        private List<ItemInstance> m_tempItemInstanceList;
        public OffscreenOpenClose PanelRoot;
        public const int POTION_IDX_BOSS = 0;
        public const int POTION_IDX_FRENZY = 1;
        public const int POTION_IDX_REVIVE = 2;
        public GameObject PotionCellPrototype;
        public const int POTIONS_TAB_INDEX = 3;
        public GameObject PowerRoot;
        public Text PowerText;
        public UnityEngine.UI.ScrollRect ScrollRect;
        public GameObject SortButtonRoot;
        public List<IconWithText> TabButtons;
        public List<PulsatingGraphic> TabUnlockNotifierPulsating;
        public List<GameObject> TabUnlockNotifiers;
        public List<PulsatingGraphic> TabUpgradeNotifierPulsating;
        public List<GameObject> TabUpgradeNotifiers;
        public Text TokenCount;
        public GameObject TokenRoot;
        public RectTransform VerticalGroup;
        public const int WEAPONS_TAB_INDEX = 0;

        private void addItemCell(ItemInstance ii)
        {
            ItemCell item = PlayerView.Binder.ItemCellPool.getObject();
            item.transform.SetParent(this.VerticalGroup, false);
            bool stripedRow = (this.VerticalGroup.childCount % 2) != 0;
            item.initialize(ii, stripedRow);
            this.m_itemCells.Add(item);
            item.gameObject.SetActive(true);
        }

        private PotionCell addPotionCell(PotionType potionType)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.PotionCellPrototype);
            obj2.name = this.PotionCellPrototype.name + "-" + potionType;
            obj2.transform.SetParent(this.VerticalGroup, false);
            PotionCell component = obj2.GetComponent<PotionCell>();
            bool stripedRow = (this.VerticalGroup.childCount % 2) == 0;
            component.initialize(potionType, stripedRow);
            this.m_potionCells.Add(component);
            obj2.SetActive(true);
            return component;
        }

        public int getCellIdxForPotion(PotionType potionType)
        {
            switch (potionType)
            {
                case PotionType.Revive:
                    return 2;

                case PotionType.Frenzy:
                    return 1;

                case PotionType.Boss:
                    return 0;
            }
            return -1;
        }

        private ItemType getItemTypeForTab(int tabIdx)
        {
            switch (tabIdx)
            {
                case 0:
                    return ItemType.Weapon;

                case 1:
                    return ItemType.Armor;

                case 2:
                    return ItemType.Cloak;
            }
            return ItemType.UNSPECIFIED;
        }

        public RectTransform getPotionCellButtonTm(PotionType potionType)
        {
            return this.m_potionCells[this.getCellIdxForPotion(potionType)].CellButton.GetComponent<RectTransform>();
        }

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator189 iterator = new <hideRoutine>c__Iterator189();
            iterator.instant = instant;
            iterator.<$>instant = instant;
            iterator.<>f__this = this;
            return iterator;
        }

        public bool isPotionUsageForceDisabled()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            return ((((PlayerView.Binder.TransitionSystem.InCriticalTransition || GameLogic.Binder.FrenzySystem.isFrenzyActive()) || ((activeDungeon.ActiveRoom == null) || activeDungeon.ActiveRoom.MainBossSummoned)) || activeDungeon.WildBossMode) || (activeDungeon.CurrentGameplayState != GameplayState.ACTION));
        }

        public bool isTabInteractable(int idx)
        {
            return this.TabButtons[idx].Button.interactable;
        }

        private static int ItemInstanceCompareByLevel(ItemInstance x, ItemInstance y)
        {
            if (x == y)
            {
                return 0;
            }
            CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
            if (activeCharacter.isItemInstanceEquipped(x))
            {
                return -1;
            }
            if (activeCharacter.isItemInstanceEquipped(y))
            {
                return 1;
            }
            if (!x.InspectedByPlayer || !y.InspectedByPlayer)
            {
                if (!x.InspectedByPlayer && y.InspectedByPlayer)
                {
                    return -1;
                }
                if (!y.InspectedByPlayer && x.InspectedByPlayer)
                {
                    return 1;
                }
                return -x.Rarity.CompareTo(y.Rarity);
            }
            int num = x.LevelPlusRank.CompareTo(y.LevelPlusRank);
            if (num != 0)
            {
                return -num;
            }
            num = x.Rarity.CompareTo(y.Rarity);
            if (num != 0)
            {
                return -num;
            }
            return x.Item.Id.CompareTo(y.Item.Id);
        }

        private static int ItemInstanceCompareByRarity(ItemInstance x, ItemInstance y)
        {
            if (x == y)
            {
                return 0;
            }
            CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
            if (activeCharacter.isItemInstanceEquipped(x))
            {
                return -1;
            }
            if (activeCharacter.isItemInstanceEquipped(y))
            {
                return 1;
            }
            int num = x.Rarity.CompareTo(y.Rarity);
            if (num != 0)
            {
                return -num;
            }
            num = x.LevelPlusRank.CompareTo(y.LevelPlusRank);
            if (num != 0)
            {
                return -num;
            }
            return x.Item.Id.CompareTo(y.Item.Id);
        }

        private void markActiveTabContentAsInspected()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (this.m_activeTabIdx == 3)
            {
                player.Notifiers.PotionsInspected = true;
            }
            else
            {
                player.Notifiers.markAllNotificationsForItemsThatWeCanUpgradeAsInspected(this.getItemTypeForTab(this.m_activeTabIdx));
            }
        }

        protected override void onAwake()
        {
            this.m_tempItemInstanceList = new List<ItemInstance>(0x20);
            for (int i = 0; i < 0x20; i++)
            {
                this.addItemCell(null);
            }
            this.addPotionCell(PotionType.Boss);
            this.addPotionCell(PotionType.Frenzy);
            this.addPotionCell(PotionType.Revive).refresh(0, false);
            this.PotionCellPrototype.SetActive(false);
            for (int j = 0; j < this.TabUpgradeNotifiers.Count; j++)
            {
                if (this.TabUpgradeNotifiers[j] != null)
                {
                    this.TabUpgradeNotifiers[j].SetActive(false);
                }
            }
            for (int k = 0; k < this.TabUpgradeNotifierPulsating.Count; k++)
            {
                if (this.TabUpgradeNotifierPulsating[k] != null)
                {
                    this.TabUpgradeNotifierPulsating[k].enabled = false;
                }
            }
            for (int m = 0; m < this.TabUnlockNotifiers.Count; m++)
            {
                if (this.TabUnlockNotifiers[m] != null)
                {
                    this.TabUnlockNotifiers[m].SetActive(false);
                }
            }
            for (int n = 0; n < this.TabUnlockNotifierPulsating.Count; n++)
            {
                if (this.TabUnlockNotifierPulsating[n] != null)
                {
                    this.TabUnlockNotifierPulsating[n].enabled = false;
                }
            }
        }

        public void onCloseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            this.m_queuedRefresh = true;
        }

        private void onGestureTapRecognized(TKTapRecognizer r)
        {
            if ((PlayerView.Binder.InputSystem.InputEnabled && !PlayerView.Binder.MenuSystem.InTransition) && ((PlayerView.Binder.MenuSystem.topmostActiveMenuType() == PlayerView.MenuType.SlidingInventoryMenu) && PlayerView.Binder.InputSystem.touchOnValidSpotForGestureStart()))
            {
                this.onCloseButtonClicked();
            }
        }

        private void onItemEquipped(CharacterInstance character, ItemInstance itemInstance, ItemInstance replacedItemInstance)
        {
            character.OwningPlayer.Notifiers.markItemNotificationsAsInspected(itemInstance);
            this.m_queuedRefresh = true;
        }

        private void onItemInspected(ItemInstance itemInstance)
        {
            this.m_queuedRefresh = true;
        }

        private void onItemRankUpped(CharacterInstance character, ItemInstance itemInstance, int rankUpCount, bool free)
        {
            Player owningPlayer = character.OwningPlayer;
            double coins = owningPlayer.getResourceAmount(ResourceType.Coin);
            if (!owningPlayer.canUpgradeItemInstance(itemInstance, coins))
            {
                owningPlayer.Notifiers.markItemNotificationsAsNonInspected(itemInstance);
            }
            this.m_queuedRefresh = true;
        }

        private void onItemSold(CharacterInstance character, ItemInstance itemInstance, double amount, RectTransform flyToHudOrigin)
        {
            this.m_queuedReconstruct = true;
        }

        private void onItemUnlocked(CharacterInstance character, ItemInstance itemInstance)
        {
            this.m_queuedRefresh = true;
        }

        private void onPotionsGained(CharacterInstance character, PotionType potionType, int amount)
        {
            this.m_queuedRefresh = true;
        }

        protected override void onRefresh()
        {
            this.m_queuedRefresh = false;
            if (this.m_activeTabIdx != 3)
            {
                for (int i = 0; i < this.m_itemCells.Count; i++)
                {
                    this.m_itemCells[i].refresh();
                }
            }
            this.refreshNotifiers();
        }

        private void onResourcesGained(Player player, ResourceType resourceType, double amount, bool instant, string analyticsSourceId, Vector3? worldPt)
        {
            if ((amount < 0.0) && (resourceType == ResourceType.Coin))
            {
                ItemType itemType = this.getItemTypeForTab(this.m_activeTabIdx);
                player.Notifiers.markAllNotificationsForItemsThatWeCannotUpgradeAsNonInspected(itemType);
            }
            this.m_queuedRefresh = true;
        }

        private void onRewardConsumed(Player player, Reward reward)
        {
            if (reward.ItemDrops.Count > 0)
            {
                this.m_queuedReconstruct = true;
            }
            else
            {
                this.m_queuedRefresh = true;
            }
        }

        public void onSortButtonClicked()
        {
            Player player = GameLogic.Binder.GameState.Player;
            player.Preferences.SlidingMenuSortingOrder = ++player.Preferences.SlidingMenuSortingOrder % 2;
            this.ScrollRect.verticalNormalizedPosition = 1f;
            this.reconstructContent();
        }

        public void onTabButtonClicked(int idx)
        {
            if (this.m_activeTabIdx != idx)
            {
                PlayerView.Binder.AudioSystem.playSfxGrp(AudioGroupType.SfxGrpUi_BookTurn, (float) 0f);
                this.setActiveTabIndex(idx);
            }
        }

        protected override void onUpdate(float dt)
        {
            if (this.m_queuedReconstruct)
            {
                this.reconstructContent();
            }
            if (this.m_queuedRefresh)
            {
                this.onRefresh();
            }
            if (this.m_activeTabIdx == 3)
            {
                Player player = GameLogic.Binder.GameState.Player;
                CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
                bool flag = this.isPotionUsageForceDisabled();
                this.m_potionCells[0].refresh(activeCharacter.Inventory.BossPotions, !flag && (activeCharacter.Inventory.BossPotions > 0));
                this.m_potionCells[1].refresh(activeCharacter.Inventory.FrenzyPotions, (!flag && (activeCharacter.Inventory.FrenzyPotions > 0)) && !player.BossTrain.Active);
                this.m_potionCells[2].refresh(activeCharacter.Inventory.RevivePotions, false);
            }
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator187 iterator = new <preShowRoutine>c__Iterator187();
            iterator.parameter = parameter;
            iterator.<$>parameter = parameter;
            iterator.<>f__this = this;
            return iterator;
        }

        private void reconstructContent()
        {
            this.m_queuedReconstruct = false;
            Player player = GameLogic.Binder.GameState.Player;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            if (this.m_activeTabIdx == 3)
            {
                this.InfoBarTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.CONSUMABLES, null, false));
                this.SortButtonRoot.SetActive(false);
                for (int i = 0; i < this.ItemCells.Count; i++)
                {
                    this.ItemCells[i].gameObject.SetActive(false);
                }
                for (int j = 0; j < this.m_potionCells.Count; j++)
                {
                    this.m_potionCells[j].gameObject.SetActive(true);
                }
                this.onRefresh();
            }
            else
            {
                ItemType type = this.getItemTypeForTab(this.m_activeTabIdx);
                switch (type)
                {
                    case ItemType.Weapon:
                        this.InfoBarTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ITEMS_WEAPONS, null, false));
                        break;

                    case ItemType.Armor:
                        this.InfoBarTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ITEMS_ARMORS, null, false));
                        break;

                    case ItemType.Cloak:
                        this.InfoBarTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ITEMS_CLOAKS, null, false));
                        break;

                    default:
                        this.InfoBarTitle.text = "CHANGE ME";
                        break;
                }
                this.SortButtonRoot.SetActive(true);
                for (int k = 0; k < this.m_potionCells.Count; k++)
                {
                    this.m_potionCells[k].gameObject.SetActive(false);
                }
                this.m_tempItemInstanceList.Clear();
                List<ItemSlot> list = activeCharacter.getItemSlots(false);
                for (int m = 0; m < list.Count; m++)
                {
                    ItemInstance itemInstance = list[m].ItemInstance;
                    if ((itemInstance != null) && (itemInstance.Item.Type == type))
                    {
                        this.m_tempItemInstanceList.Add(itemInstance);
                    }
                }
                List<ItemInstance> list2 = activeCharacter.getItemInstances(false);
                for (int n = 0; n < list2.Count; n++)
                {
                    ItemInstance item = list2[n];
                    if (item.Item.Type == type)
                    {
                        this.m_tempItemInstanceList.Add(item);
                    }
                }
                if (player.Preferences.SlidingMenuSortingOrder == 0)
                {
                    this.m_tempItemInstanceList.Sort(new Comparison<ItemInstance>(SlidingInventoryMenu.ItemInstanceCompareByRarity));
                }
                else
                {
                    this.m_tempItemInstanceList.Sort(new Comparison<ItemInstance>(SlidingInventoryMenu.ItemInstanceCompareByLevel));
                }
                while (this.m_tempItemInstanceList.Count > this.ItemCells.Count)
                {
                    this.addItemCell(null);
                }
                for (int num6 = 0; num6 < this.m_tempItemInstanceList.Count; num6++)
                {
                    ItemCell cell = this.ItemCells[num6];
                    cell.initialize(this.m_tempItemInstanceList[num6], cell.StripedRow);
                    cell.enabled = true;
                    cell.gameObject.SetActive(true);
                }
                for (int num7 = this.m_tempItemInstanceList.Count; num7 < this.ItemCells.Count; num7++)
                {
                    this.ItemCells[num7].gameObject.SetActive(false);
                }
            }
            this.ScrollRect.verticalNormalizedPosition = 1f;
            this.refreshNotifiers();
        }

        private void refreshNotifiers()
        {
            Player player = GameLogic.Binder.GameState.Player;
            for (int i = 0; i < this.TabUpgradeNotifiers.Count; i++)
            {
                GameObject obj2 = this.TabUpgradeNotifiers[i];
                if (obj2 != null)
                {
                    obj2.SetActive(player.Notifiers.getNumberOfGreenItemNotifications(this.getItemTypeForTab(i)) > 0);
                }
            }
            for (int j = 0; j < this.TabUpgradeNotifierPulsating.Count; j++)
            {
                PulsatingGraphic mb = this.TabUpgradeNotifierPulsating[j];
                if (mb != null)
                {
                    UnityUtils.SetEnabledStateIfDifferent(mb, this.TabUpgradeNotifiers[j].activeSelf);
                }
            }
            for (int k = 0; k < this.TabUnlockNotifiers.Count; k++)
            {
                GameObject obj3 = this.TabUnlockNotifiers[k];
                if (obj3 != null)
                {
                    if (k == 3)
                    {
                        obj3.SetActive(!player.Notifiers.PotionsInspected);
                    }
                    else
                    {
                        ItemType itemType = this.getItemTypeForTab(k);
                        obj3.SetActive(player.Notifiers.getNumberOfGoldItemNotifications(itemType) > 0);
                    }
                }
            }
            for (int m = 0; m < this.TabUnlockNotifierPulsating.Count; m++)
            {
                PulsatingGraphic graphic2 = this.TabUnlockNotifierPulsating[m];
                if (graphic2 != null)
                {
                    UnityUtils.SetEnabledStateIfDifferent(graphic2, this.TabUnlockNotifiers[m].activeSelf);
                }
            }
        }

        public void setActiveTabIndex(int idx)
        {
            if (this.m_activeTabIdx != idx)
            {
                this.markActiveTabContentAsInspected();
                if (this.m_activeTabIdx != -1)
                {
                    this.TabButtons[this.m_activeTabIdx].Background.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "uix_tab_inactive_0");
                }
                this.m_activeTabIdx = idx;
                this.TabButtons[this.m_activeTabIdx].Background.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "uix_tab_active_0");
            }
            this.setTabInteractable(this.m_activeTabIdx, true);
            if (this.m_activeTabIdx == 3)
            {
                this.markActiveTabContentAsInspected();
            }
            this.reconstructContent();
        }

        public void setTabInteractable(int idx, bool interactable)
        {
            if (interactable)
            {
                this.TabButtons[idx].Button.interactable = true;
                this.TabButtons[idx].CanvasGroup.alpha = 1f;
                this.TabButtons[idx].Icon.material = null;
                this.TabButtons[idx].Background.material = null;
            }
            else
            {
                this.TabButtons[idx].Button.interactable = false;
                this.TabButtons[idx].CanvasGroup.alpha = 0.75f;
                this.TabButtons[idx].Icon.material = PlayerView.Binder.DisabledUiMaterial;
                this.TabButtons[idx].Background.material = PlayerView.Binder.DisabledUiMaterial;
            }
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator188 iterator = new <showRoutine>c__Iterator188();
            iterator.<>f__this = this;
            return iterator;
        }

        public int ActiveTabIndex
        {
            get
            {
                return this.m_activeTabIdx;
            }
        }

        public List<ItemCell> ItemCells
        {
            get
            {
                return this.m_itemCells;
            }
        }

        public override PlayerView.MenuType MenuType
        {
            get
            {
                return PlayerView.MenuType.SlidingInventoryMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator189 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal SlidingInventoryMenu <>f__this;
            internal float <duration>__0;
            internal int <i>__1;
            internal int <i>__2;
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
                        this.<duration>__0 = !this.instant ? ConfigUi.SLIDING_PANEL_EXIT_DURATION : 0f;
                        this.<>f__this.PanelRoot.close(this.<duration>__0, Easing.Function.IN_CUBIC, 0f);
                        if (this.instant)
                        {
                            goto Label_00A6;
                        }
                        PlayerView.Binder.AudioSystem.playSfxGrp(AudioGroupType.SfxGrpUi_BookClose, (float) 0f);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0263;
                }
                if (this.<>f__this.PanelRoot.Animating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
            Label_00A6:
                this.<>f__this.markActiveTabContentAsInspected();
                this.<i>__1 = 0;
                while (this.<i>__1 < this.<>f__this.ItemCells.Count)
                {
                    this.<>f__this.ItemCells[this.<i>__1].enabled = false;
                    this.<i>__1++;
                }
                this.<i>__2 = 0;
                while (this.<i>__2 < this.<>f__this.TabButtons.Count)
                {
                    this.<>f__this.setTabInteractable(this.<i>__2, true);
                    this.<i>__2++;
                }
                GameLogic.Binder.EventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.<>f__this.onGameplayStarted);
                GameLogic.Binder.EventBus.OnItemUnlocked -= new GameLogic.Events.ItemUnlocked(this.<>f__this.onItemUnlocked);
                GameLogic.Binder.EventBus.OnItemEquipped -= new GameLogic.Events.ItemEquipped(this.<>f__this.onItemEquipped);
                GameLogic.Binder.EventBus.OnItemRankUpped -= new GameLogic.Events.ItemRankUpped(this.<>f__this.onItemRankUpped);
                GameLogic.Binder.EventBus.OnItemInspected -= new GameLogic.Events.ItemInspected(this.<>f__this.onItemInspected);
                GameLogic.Binder.EventBus.OnItemSold -= new GameLogic.Events.ItemSold(this.<>f__this.onItemSold);
                GameLogic.Binder.EventBus.OnResourcesGained -= new GameLogic.Events.ResourcesGained(this.<>f__this.onResourcesGained);
                GameLogic.Binder.EventBus.OnRewardConsumed -= new GameLogic.Events.RewardConsumed(this.<>f__this.onRewardConsumed);
                GameLogic.Binder.EventBus.OnPotionsGained -= new GameLogic.Events.PotionsGained(this.<>f__this.onPotionsGained);
                PlayerView.Binder.EventBus.OnGestureTapRecognized -= new PlayerView.Events.GestureTapRecognized(this.<>f__this.onGestureTapRecognized);
                goto Label_0263;
                this.$PC = -1;
            Label_0263:
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
        private sealed class <preShowRoutine>c__Iterator187 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal SlidingInventoryMenu <>f__this;
            internal SlidingInventoryMenu.InputParameters <ip>__0;
            internal Player <player>__1;
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
                        this.<ip>__0 = (SlidingInventoryMenu.InputParameters) this.parameter;
                    }
                    else
                    {
                        this.<ip>__0 = new SlidingInventoryMenu.InputParameters();
                    }
                    this.<player>__1 = GameLogic.Binder.GameState.Player;
                    this.<>f__this.m_potionCells[this.<>f__this.getCellIdxForPotion(PotionType.Boss)].Description.text = _.L(ConfigLoca.MINIPOPUP_BOSS_POTION_DESCRIPTION, new <>__AnonType9<int>(App.Binder.ConfigMeta.BOSS_POTION_NUM_BOSSES), false);
                    GameLogic.Binder.EventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.<>f__this.onGameplayStarted);
                    GameLogic.Binder.EventBus.OnItemUnlocked += new GameLogic.Events.ItemUnlocked(this.<>f__this.onItemUnlocked);
                    GameLogic.Binder.EventBus.OnItemEquipped += new GameLogic.Events.ItemEquipped(this.<>f__this.onItemEquipped);
                    GameLogic.Binder.EventBus.OnItemRankUpped += new GameLogic.Events.ItemRankUpped(this.<>f__this.onItemRankUpped);
                    GameLogic.Binder.EventBus.OnItemInspected += new GameLogic.Events.ItemInspected(this.<>f__this.onItemInspected);
                    GameLogic.Binder.EventBus.OnItemSold += new GameLogic.Events.ItemSold(this.<>f__this.onItemSold);
                    GameLogic.Binder.EventBus.OnResourcesGained += new GameLogic.Events.ResourcesGained(this.<>f__this.onResourcesGained);
                    GameLogic.Binder.EventBus.OnRewardConsumed += new GameLogic.Events.RewardConsumed(this.<>f__this.onRewardConsumed);
                    GameLogic.Binder.EventBus.OnPotionsGained += new GameLogic.Events.PotionsGained(this.<>f__this.onPotionsGained);
                    PlayerView.Binder.EventBus.OnGestureTapRecognized += new PlayerView.Events.GestureTapRecognized(this.<>f__this.onGestureTapRecognized);
                    if (!this.<player>__1.hasCompletedTutorial("TUT051A"))
                    {
                        this.<>f__this.setActiveTabIndex(0);
                        this.<>f__this.setTabInteractable(1, false);
                        this.<>f__this.setTabInteractable(2, false);
                    }
                    else if (!this.<player>__1.hasCompletedTutorial("TUT052A"))
                    {
                        if (this.<>f__this.m_activeTabIdx == -1)
                        {
                            this.<>f__this.setActiveTabIndex(0);
                        }
                        else
                        {
                            this.<>f__this.setActiveTabIndex(this.<>f__this.m_activeTabIdx);
                        }
                        this.<>f__this.setTabInteractable(2, false);
                    }
                    else if (this.<ip>__0.OverrideOpenTabIndex.HasValue)
                    {
                        this.<>f__this.setActiveTabIndex(this.<ip>__0.OverrideOpenTabIndex.Value);
                    }
                    else if (this.<>f__this.m_activeTabIdx == -1)
                    {
                        this.<>f__this.setActiveTabIndex(0);
                    }
                    else
                    {
                        this.<>f__this.setActiveTabIndex(this.<>f__this.m_activeTabIdx);
                    }
                    this.<>f__this.setTabInteractable(3, this.<player>__1.potionsUnlocked());
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
        private sealed class <showRoutine>c__Iterator188 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal SlidingInventoryMenu <>f__this;

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
                        this.<>f__this.PanelRoot.open(ConfigUi.SLIDING_PANEL_ENTRY_DURATION, Easing.Function.OUT_CUBIC, 0f);
                        PlayerView.Binder.AudioSystem.playSfxGrp(AudioGroupType.SfxGrpUi_BookOpen, (float) 0f);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0086;
                }
                if (this.<>f__this.PanelRoot.Animating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                goto Label_0086;
                this.$PC = -1;
            Label_0086:
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
            public int? OverrideOpenTabIndex;
        }
    }
}

