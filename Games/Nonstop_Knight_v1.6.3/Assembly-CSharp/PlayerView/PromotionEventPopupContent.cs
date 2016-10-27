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

    public class PromotionEventPopupContent : MenuContent
    {
        public GameObject Contents;
        public const int DESCRIPTION_ROWS_MAX = 5;
        public GameObject DescriptionRowPrototype;
        public List<GameObject> DescriptionRowsSection;
        public RectTransform DescriptionRowsTm;
        public Text DescriptionTitle;
        public Text FlavorText;
        public MenuDragHandler HeaderAvatarDragHandler;
        public RawImage HeaderHeroAvatar;
        public RawImage HeaderPetAvatar;
        private ConfigPromotionEvents.Event m_contentData;
        private PromotionEventInstance m_contentInstance;
        private ConfigPromotionEvents.Event m_contentLastData;
        private List<IconWithText> m_descriptionRows;
        private CharacterInstance m_heroAvatarCharacterInstance;
        private bool m_isInitialRefresh;
        private string m_lastActiveIapPlacementPromotionId;
        private List<MissionCell> m_missionCells = new List<MissionCell>(4);
        private List<PromotionCard> m_promotionCards = new List<PromotionCard>(1);
        public Text MissionsBigPrizeDescription;
        public Image MissionsBigPrizeIcon;
        public AnimatedProgressBar MissionsBigPrizeProgressBar;
        public Text MissionsBigPrizeProgressBarTitle;
        public List<GameObject> MissionsSection;
        public Text MissionsTitle;
        public RectTransform MissionsTm;
        public UnityEngine.UI.ScrollRect ScrollRect;
        public RectTransform SpecialOfferItems;
        public List<GameObject> SpecialOfferSection;
        public Text SpecialOfferTitle;
        public Text Subtitle;
        public GameObject TimerRoot;
        public Text TimerText;
        public Text Title;

        private void cleanupHeader()
        {
            if (this.m_heroAvatarCharacterInstance != null)
            {
                GameLogic.Binder.CharacterPool.returnObject(this.m_heroAvatarCharacterInstance);
                this.m_heroAvatarCharacterInstance = null;
            }
            if (PlayerView.Binder.MenuSystem.MenuHero.CharacterView != null)
            {
                PlayerView.Binder.MenuSystem.MenuHero.CharacterView.gameObject.SetActive(false);
            }
            if (PlayerView.Binder.MenuSystem.MenuPet.CharacterView != null)
            {
                PlayerView.Binder.MenuSystem.MenuPet.CharacterView.gameObject.SetActive(false);
            }
            this.HeaderHeroAvatar.enabled = false;
            this.HeaderHeroAvatar.texture = null;
            this.HeaderPetAvatar.enabled = false;
            this.HeaderPetAvatar.texture = null;
            PlayerView.Binder.MenuSystem.CharacterMenuCamera.Camera.enabled = false;
            if (PlayerView.Binder.MenuSystem.CharacterMenuCamera.Target != null)
            {
                MenuCharacterView target = PlayerView.Binder.MenuSystem.CharacterMenuCamera.Target;
                this.HeaderAvatarDragHandler.OnDragged -= new MenuDragHandler.Dragged(target.OnDrag);
            }
        }

        private void cleanupMissions()
        {
            for (int i = 0; i < this.m_missionCells.Count; i++)
            {
                PlayerView.Binder.MissionCellPool.returnObject(this.m_missionCells[i]);
            }
            this.m_missionCells.Clear();
        }

        private void cleanupSpecialOffer()
        {
            for (int i = 0; i < this.m_promotionCards.Count; i++)
            {
                PlayerView.Binder.PromotionCardAugmentationPool.returnObject(this.m_promotionCards[i]);
            }
            this.m_promotionCards.Clear();
        }

        private ItemInstance getHeroAvatarItemInstance(Player player, ItemType itemType, Dictionary<ItemType, string> itemOverrides)
        {
            ItemInstance instance = player.ActiveCharacter.getEquippedItemOfType(itemType);
            if (!itemOverrides.ContainsKey(itemType))
            {
                return instance;
            }
            string str = itemOverrides[itemType];
            if (string.IsNullOrEmpty(str))
            {
                return instance;
            }
            if (!GameLogic.Binder.ItemResources.containsResource(str))
            {
                return instance;
            }
            return ItemInstance.Create(GameLogic.Binder.ItemResources.getResource(str), player, -1);
        }

        protected override void onAwake()
        {
            this.DescriptionTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PROMOTION_EVENT_DESCRIPTION_TITLE, null, false));
            this.MissionsTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PROMOTION_EVENT_MISSIONS_TITLE, null, false));
            this.SpecialOfferTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PROMOTION_EVENT_SPECIAL_OFFER_TITLE, null, false));
            this.m_descriptionRows = new List<IconWithText>(5);
            for (int i = 0; i < 5; i++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.DescriptionRowPrototype);
                obj2.name = this.DescriptionRowPrototype.name + "-" + i;
                obj2.transform.SetParent(this.DescriptionRowsTm);
                obj2.transform.SetSiblingIndex(i);
                obj2.SetActive(false);
                this.m_descriptionRows.Add(obj2.GetComponent<IconWithText>());
            }
            this.DescriptionRowPrototype.SetActive(false);
        }

        protected override void onCleanup()
        {
            this.cleanupHeader();
            this.cleanupMissions();
            this.cleanupSpecialOffer();
            this.m_contentData = null;
            this.m_contentLastData = null;
            this.m_contentInstance = null;
            this.m_isInitialRefresh = false;
            this.m_lastActiveIapPlacementPromotionId = null;
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnMissionStarted -= new GameLogic.Events.MissionStarted(this.onMissionStarted);
            GameLogic.Binder.EventBus.OnPromotionEventStarted -= new GameLogic.Events.PromotionEventStarted(this.onPromotionEventStarted);
            GameLogic.Binder.EventBus.OnPromotionEventEnded -= new GameLogic.Events.PromotionEventEnded(this.onPromotionEventEnded);
            GameLogic.Binder.EventBus.OnPromotionEventRefreshed -= new GameLogic.Events.PromotionEventRefreshed(this.onPromotionEventRefreshed);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnMissionStarted += new GameLogic.Events.MissionStarted(this.onMissionStarted);
            GameLogic.Binder.EventBus.OnPromotionEventStarted += new GameLogic.Events.PromotionEventStarted(this.onPromotionEventStarted);
            GameLogic.Binder.EventBus.OnPromotionEventEnded += new GameLogic.Events.PromotionEventEnded(this.onPromotionEventEnded);
            GameLogic.Binder.EventBus.OnPromotionEventRefreshed += new GameLogic.Events.PromotionEventRefreshed(this.onPromotionEventRefreshed);
        }

        private void onMissionStarted(Player player, MissionInstance mission)
        {
            this.refreshMissions();
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            Player player = GameLogic.Binder.GameState.Player;
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.DHUD_BUTTON_PROMOTION, null, false)), string.Empty, string.Empty);
            if (player.PromotionEvents.Instances.Count > 1)
            {
                UnityEngine.Debug.LogWarning(player.PromotionEvents.Instances.Count + " promotion events. The UI only supports showing one at a time.");
            }
            foreach (KeyValuePair<string, PromotionEventInstance> pair in player.PromotionEvents.Instances)
            {
                if (!pair.Value.Inspected)
                {
                    CmdInspectPromotionEvent.ExecuteStatic(player, pair.Key);
                }
            }
            this.m_isInitialRefresh = true;
            base.refresh();
        }

        private void onPromotionEventEnded(Player player, string promotionId)
        {
            base.refresh();
        }

        private void onPromotionEventRefreshed(Player player, string promotionId)
        {
            base.refresh();
        }

        private void onPromotionEventStarted(Player player, string promotionId)
        {
            base.refresh();
        }

        protected override void onRefresh()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.m_contentInstance = player.PromotionEvents.getNewestEventInstance();
            bool flag = this.m_contentInstance != null;
            this.Contents.SetActive(flag);
            if (flag)
            {
                this.m_contentData = this.m_contentInstance.getData();
                this.refreshHeader();
                this.refreshInfo();
                this.refreshMissions();
                this.refreshSpecialOffer();
                this.m_contentLastData = this.m_contentData;
                this.m_isInitialRefresh = false;
            }
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator165 iterator = new <onShow>c__Iterator165();
            iterator.<>f__this = this;
            return iterator;
        }

        public void onVendorIAPButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.StarterBundlePopupContent, null, 0f, false, true);
            }
        }

        private void refreshHeader()
        {
            if ((this.m_contentData != null) && ((this.m_isInitialRefresh || (this.m_contentLastData == null)) || !this.m_contentData.Header.Equals(this.m_contentLastData.Header)))
            {
                this.cleanupHeader();
                switch (this.m_contentData.Header.Type)
                {
                    case PromotionEventHeaderType.HeroAvatar:
                    {
                        Player player = GameLogic.Binder.GameState.Player;
                        this.m_heroAvatarCharacterInstance = GameLogic.Binder.CharacterPool.getObject();
                        this.m_heroAvatarCharacterInstance.CharacterId = player.ActiveCharacter.CharacterId;
                        this.m_heroAvatarCharacterInstance.Character = player.ActiveCharacter.Character;
                        this.m_heroAvatarCharacterInstance.ItemSlots.Clear();
                        ItemSlot item = new ItemSlot();
                        item.CompatibleItemType = ItemType.Weapon;
                        item.ItemInstance = this.getHeroAvatarItemInstance(player, ItemType.Weapon, this.m_contentData.Header.Items);
                        this.m_heroAvatarCharacterInstance.ItemSlots.Add(item);
                        item = new ItemSlot();
                        item.CompatibleItemType = ItemType.Armor;
                        item.ItemInstance = this.getHeroAvatarItemInstance(player, ItemType.Armor, this.m_contentData.Header.Items);
                        this.m_heroAvatarCharacterInstance.ItemSlots.Add(item);
                        item = new ItemSlot();
                        item.CompatibleItemType = ItemType.Cloak;
                        item.ItemInstance = this.getHeroAvatarItemInstance(player, ItemType.Cloak, this.m_contentData.Header.Items);
                        this.m_heroAvatarCharacterInstance.ItemSlots.Add(item);
                        PlayerView.Binder.MenuSystem.initializeMenuHero(this.m_heroAvatarCharacterInstance);
                        break;
                    }
                    case PromotionEventHeaderType.PetAvatar:
                        PlayerView.Binder.MenuSystem.initializeMenuPet(this.m_contentData.Header.CharacterPrefab);
                        break;
                }
                if ((this.m_contentData.Header.Type == PromotionEventHeaderType.HeroAvatar) || (this.m_contentData.Header.Type == PromotionEventHeaderType.PetAvatar))
                {
                    MenuCharacterView view = (this.m_contentData.Header.Type != PromotionEventHeaderType.HeroAvatar) ? PlayerView.Binder.MenuSystem.MenuPet : PlayerView.Binder.MenuSystem.MenuHero;
                    view.CharacterView.gameObject.SetActive(true);
                    view.CharacterView.setVisibility(true);
                    view.CharacterView.Transform.rotation = Quaternion.Euler(0f, -20f, 0f);
                    PlayerView.Binder.MenuSystem.CharacterMenuCamera.Target = view;
                    RawImage image = (this.m_contentData.Header.Type != PromotionEventHeaderType.HeroAvatar) ? this.HeaderPetAvatar : this.HeaderHeroAvatar;
                    image.enabled = true;
                    image.texture = PlayerView.Binder.MenuSystem.CharacterMenuCamera.RenderTexture;
                    PlayerView.Binder.MenuSystem.CharacterMenuCamera.Camera.enabled = true;
                    MenuCharacterView target = PlayerView.Binder.MenuSystem.CharacterMenuCamera.Target;
                    this.HeaderAvatarDragHandler.OnDragged += new MenuDragHandler.Dragged(target.OnDrag);
                }
            }
        }

        private void refreshInfo()
        {
            if (this.m_contentData != null)
            {
                this.Title.text = string.IsNullOrEmpty(this.m_contentData.Info.Title) ? "???" : StringExtensions.ToUpperLoca(this.m_contentData.Info.Title);
                this.Subtitle.text = string.IsNullOrEmpty(this.m_contentData.Info.Subtitle) ? "???" : this.m_contentData.Info.Subtitle;
                this.FlavorText.text = string.IsNullOrEmpty(this.m_contentData.Info.Flavor) ? "???" : this.m_contentData.Info.Flavor;
                this.TimerRoot.SetActive(!this.m_contentData.HideTimer);
                bool flag = (this.m_contentData.Info.DescriptionRows != null) && (this.m_contentData.Info.DescriptionRows.Count > 0);
                for (int i = 0; i < this.DescriptionRowsSection.Count; i++)
                {
                    this.DescriptionRowsSection[i].SetActive(flag);
                }
                if (flag)
                {
                    for (int j = 0; (j < this.m_contentData.Info.DescriptionRows.Count) && (j < 5); j++)
                    {
                        ConfigPromotionEvents.EventInfo.DescriptionRow row = this.m_contentData.Info.DescriptionRows[j];
                        IconWithText text = this.m_descriptionRows[j];
                        text.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(row.Icon);
                        text.Text.text = row.Text;
                        text.gameObject.SetActive(true);
                    }
                    for (int k = this.m_contentData.Info.DescriptionRows.Count; k < 5; k++)
                    {
                        this.m_descriptionRows[k].gameObject.SetActive(false);
                    }
                }
            }
        }

        private void refreshMissions()
        {
            Player player = GameLogic.Binder.GameState.Player;
            bool flag = this.m_contentInstance.Missions.Instances.Count > 0;
            for (int i = 0; i < this.MissionsSection.Count; i++)
            {
                this.MissionsSection[i].SetActive(flag);
            }
            this.cleanupMissions();
            if (flag)
            {
                this.MissionsBigPrizeIcon.sprite = PlayerView.Binder.SpriteResources.getSprite(this.m_contentData.Missions.BigPrizeSprite);
                int amount = this.m_contentInstance.Missions.getNumCompletedMissionsRequiredForBigPrize();
                this.MissionsBigPrizeDescription.text = (this.m_contentData.Missions.BigPrizeDescription == null) ? _.L(ConfigLoca.PROMOTION_EVENT_MISSIONS_BIG_PRIZE, new <>__AnonType9<int>(amount), false) : this.m_contentData.Missions.BigPrizeDescription.Replace("$Amount$", amount.ToString());
                for (int j = 0; j < this.m_contentInstance.Missions.Instances.Count; j++)
                {
                    MissionInstance mission = this.m_contentInstance.Missions.Instances[j];
                    MissionCell item = PlayerView.Binder.MissionCellPool.getObject();
                    item.transform.SetParent(this.MissionsTm, false);
                    item.gameObject.SetActive(true);
                    item.refresh(player, mission, j < (this.m_contentInstance.Missions.Instances.Count - 1), this.m_contentData.Missions.getTitleOverride(mission.MissionId), this.m_contentData.Missions.getDescriptionOverride(mission.MissionId));
                    if (!mission.OnCooldown)
                    {
                        CmdInspectMission.ExecuteStatic(player, mission);
                    }
                    this.m_missionCells.Add(item);
                }
            }
        }

        private void refreshSpecialOffer()
        {
            if (this.m_contentData != null)
            {
                RemotePromotion promotion = Service.Binder.PromotionManager.GetPromotion(this.m_contentData.IapPlacementPromotionId, EPromotionType.IapPlacement);
                bool flag = (promotion != null) && promotion.State.Active;
                for (int i = 0; i < this.SpecialOfferSection.Count; i++)
                {
                    this.SpecialOfferSection[i].SetActive(flag);
                }
                string str = !flag ? null : this.m_contentData.IapPlacementPromotionId;
                if (str != this.m_lastActiveIapPlacementPromotionId)
                {
                    this.cleanupSpecialOffer();
                    if (flag)
                    {
                        PromotionCard.Content content2 = new PromotionCard.Content();
                        content2.Promotion = promotion;
                        content2.RemoteTexture = promotion.ParsedCustomParams.ShopBannerImage;
                        PromotionCard.Content content = content2;
                        PromotionCard item = PlayerView.Binder.PromotionCardAugmentationPool.getObject();
                        item.transform.SetParent(this.SpecialOfferItems, false);
                        item.initialize(content, null);
                        this.m_promotionCards.Add(item);
                        item.gameObject.SetActive(true);
                    }
                }
                this.m_lastActiveIapPlacementPromotionId = str;
            }
        }

        protected void Update()
        {
            if (((this.m_contentInstance != null) && (this.m_contentData != null)) && !this.m_contentData.HideTimer)
            {
                this.TimerText.text = _.L(ConfigLoca.PROMOTION_TIMER, null, false) + " " + MenuHelpers.ColoredText(this.m_contentInstance.getTimerString());
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.PromotionEventPopupContent;
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator165 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal PromotionEventPopupContent <>f__this;

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
                    this.<>f__this.ScrollRect.verticalNormalizedPosition = 1f;
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

