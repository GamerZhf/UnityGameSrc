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

    public class PetPopupContent : MenuContent
    {
        [CompilerGenerated]
        private List<PerkCell> <ContextInfoPerkCells>k__BackingField;
        public static Color COLOR_PERK_LOCKED = new Color(0.5490196f, 0.627451f, 0.7254902f, 0.5882353f);
        public RawImage ContextInfoAvatar;
        public MenuDragHandler ContextInfoAvatarDragHandler;
        public Transform ContextInfoCellTm;
        public Text ContextInfoFlavorText;
        public Text ContextInfoLevelText;
        public GameObject ContextInfoPerkCellPrototype;
        public RectTransform ContextInfoPerkRootTm;
        public Text ContextInfoPerkTitle;
        public Text ContextInfoPetPower;
        public Text ContextInfoPowerText;
        public Text ContextInfoPowerTitle;
        public AnimatedProgressBar ContextInfoProgressBar;
        public GameObject ContextInfoProgressBarFgDefault;
        public GameObject ContextInfoProgressBarFgFull;
        public Text ContextInfoProgressBarText;
        public List<Image> ContextInfoStars = new List<Image>();
        public Text ContextInfoTitle;
        public RectTransform ContextInfoTooltipButtonTm;
        public ParticleSystem ContextInfoUpgradeParticleEffect;
        public PrettyButton GiftButton;
        public Text GiftDescription;
        private Character m_highlightedCharacter;
        private List<PetCell> m_petCells = new List<PetCell>();
        public Text NoPetText;
        public Transform PetGridTm;
        public RectTransform PetRowRootTm;
        public Text PetSubtitleText;
        public PrettyButton PetUnequipButton;
        public RectTransform PetUnequipButtonRootTm;
        public UnityEngine.UI.ScrollRect ScrollRect;
        public RectTransform VerticalGroup;

        private void addPetCell(string characterId)
        {
            PetCell.Content content = new PetCell.Content();
            Character character = GameLogic.Binder.CharacterResources.getResource(characterId);
            PetInstance instance = GameLogic.Binder.GameState.Player.Pets.getPetInstance(characterId);
            content.Obj = character;
            content.Sprite = character.AvatarSprite;
            content.ShowHiddenStars = true;
            content.Interactable = true;
            if (instance != null)
            {
                content.StarRank = instance.Level;
                if (instance.Level >= 1)
                {
                    content.Grayscale = false;
                }
                else
                {
                    content.Grayscale = true;
                }
            }
            else
            {
                content.StarRank = 0;
                content.Grayscale = true;
            }
            PetCell item = PlayerView.Binder.PetCellPool.getObject();
            item.transform.SetParent(this.PetGridTm, false);
            item.initialize(content, new Action<PetCell>(this.onCellClicked));
            this.m_petCells.Add(item);
            item.gameObject.SetActive(true);
        }

        private void cleanupCells()
        {
            for (int i = this.m_petCells.Count - 1; i >= 0; i--)
            {
                PetCell item = this.m_petCells[i];
                this.m_petCells.Remove(item);
                PlayerView.Binder.PetCellPool.returnObject(item);
            }
        }

        private void closeHighlightedContextInfo()
        {
            this.ContextInfoCellTm.gameObject.SetActive(false);
            this.m_highlightedCharacter = null;
        }

        private bool highlightPerkAsLegendary(PerkType perkType)
        {
            if ((perkType != PerkType.ChesterChestDrop) && (perkType != PerkType.AllyHeal))
            {
                return (ConfigPerks.SHARED_DATA[perkType].LinkedToRunestone != null);
            }
            return true;
        }

        public void onAuxButtonClicked()
        {
            if (this.m_highlightedCharacter != null)
            {
                this.closeHighlightedContextInfo();
            }
        }

        protected override void onAwake()
        {
            this.PetSubtitleText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PET_COLLECTION, null, false));
            this.PetUnequipButton.Text.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_UNEQUIP, null, false));
            this.ContextInfoPerkTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PET_PERKS, null, false));
            this.ContextInfoPowerTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PET_POWER, null, false));
            this.NoPetText.text = _.L(ConfigLoca.PET_PERKS_TOOLTIP, null, false);
            this.ContextInfoPerkCells = new List<PerkCell>(5);
            for (int i = 0; i < 5; i++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.ContextInfoPerkCellPrototype);
                obj2.name = this.ContextInfoPerkCellPrototype.name + "-" + i;
                obj2.transform.SetParent(this.ContextInfoPerkRootTm, false);
                this.ContextInfoPerkCells.Add(obj2.GetComponent<PerkCell>());
                obj2.SetActive(true);
            }
            this.ContextInfoPerkCellPrototype.SetActive(false);
        }

        private void onCellClicked(PetCell cell)
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (cell.ActiveContent.Obj == this.m_highlightedCharacter)
            {
                PetInstance instance = player.Pets.getPetInstance(this.m_highlightedCharacter);
                if ((instance != null) && (instance.Level >= 1))
                {
                    if (!player.Pets.isPetSelected(this.m_highlightedCharacter.Id))
                    {
                        CmdSelectPet.ExecuteStatic(player, this.m_highlightedCharacter.Id);
                    }
                    CmdInspectPet.ExecuteStatic(player, this.m_highlightedCharacter.Id, false);
                }
            }
            else
            {
                this.openContextInfo((Character) cell.ActiveContent.Obj);
                this.onRefresh();
            }
        }

        protected override void onCleanup()
        {
            this.cleanupCells();
            PlayerView.Binder.MenuSystem.CharacterMenuCamera.Camera.enabled = false;
            MenuCharacterView target = PlayerView.Binder.MenuSystem.CharacterMenuCamera.Target;
            this.ContextInfoAvatarDragHandler.OnDragged -= new MenuDragHandler.Dragged(target.OnDrag);
            if (PlayerView.Binder.MenuSystem.MenuPet.CharacterView != null)
            {
                PlayerView.Binder.MenuSystem.MenuPet.CharacterView.gameObject.SetActive(false);
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnPetGained -= new GameLogic.Events.PetGained(this.onPetGained);
            GameLogic.Binder.EventBus.OnPetSelected -= new GameLogic.Events.PetSelected(this.onPetSelected);
            GameLogic.Binder.EventBus.OnPetLevelUpped -= new GameLogic.Events.PetLevelUpped(this.onPetLevelUpped);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnPetGained += new GameLogic.Events.PetGained(this.onPetGained);
            GameLogic.Binder.EventBus.OnPetSelected += new GameLogic.Events.PetSelected(this.onPetSelected);
            GameLogic.Binder.EventBus.OnPetLevelUpped += new GameLogic.Events.PetLevelUpped(this.onPetLevelUpped);
        }

        private void onPetGained(Player player, string petId, bool cheated)
        {
            this.reconstructContent();
        }

        private void onPetLevelUpped(Player player, string petId, bool cheated)
        {
            this.ContextInfoUpgradeParticleEffect.Play();
            this.onRefresh();
        }

        private void onPetSelected(Player player, PetInstance pet)
        {
            this.onRefresh();
        }

        public void onPetUnequipButtonClicked()
        {
            CmdSelectPet.ExecuteStatic(GameLogic.Binder.GameState.Player, null);
            this.refreshUnequipButtons();
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.refreshUnequipButtons();
            PlayerView.Binder.MenuSystem.CharacterMenuCamera.Target = PlayerView.Binder.MenuSystem.MenuPet;
            PlayerView.Binder.MenuSystem.CharacterMenuCamera.Camera.enabled = true;
            MenuCharacterView menuPet = PlayerView.Binder.MenuSystem.MenuPet;
            this.ContextInfoAvatarDragHandler.OnDragged += new MenuDragHandler.Dragged(menuPet.OnDrag);
            PetInstance instance = GameLogic.Binder.GameState.Player.Pets.getSelectedPetInstance();
            if (instance != null)
            {
                this.m_highlightedCharacter = instance.Character;
            }
            this.reconstructContent();
        }

        protected override void onRefresh()
        {
            Player player = GameLogic.Binder.GameState.Player;
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_PETS_BUTTON_TEXT, null, false)), string.Empty, string.Empty);
            this.NoPetText.gameObject.SetActive(this.m_highlightedCharacter == null);
            for (int i = 0; i < this.m_petCells.Count; i++)
            {
                Character character = (Character) this.m_petCells[i].ActiveContent.Obj;
                PetInstance instance = player.Pets.getPetInstance(character);
                bool highlighted = this.m_highlightedCharacter == character;
                bool selected = player.Pets.isPetSelected(character);
                if ((instance != null) && (instance.Level > 0))
                {
                    bool notify = !instance.InspectedByPlayer;
                    this.m_petCells[i].refresh(highlighted, selected, notify, -1f, null);
                }
                else if ((instance != null) && (instance.Level == 0))
                {
                    float progressBarNormalizeValue = instance.getNormalizedProgressTowardsLevelUp();
                    string progressBarText = instance.Duplicates + " / " + App.Binder.ConfigMeta.PetRequiredDuplicatesForLevelUp(instance.Level + 1);
                    this.m_petCells[i].refresh(highlighted, selected, false, progressBarNormalizeValue, progressBarText);
                }
                else
                {
                    string str2 = "0 / " + App.Binder.ConfigMeta.PetRequiredDuplicatesForLevelUp(1);
                    this.m_petCells[i].refresh(highlighted, selected, false, 0f, str2);
                }
            }
            this.refreshUnequipButtons();
            this.refreshContextInfo();
        }

        public void onSendGiftButtonClicked()
        {
            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.SendGiftPopupContent, null, 0f, false, true);
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator15C iteratorc = new <onShow>c__Iterator15C();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        public void onTooltipButtonClicked()
        {
            TooltipMenu.InputParameters parameters2 = new TooltipMenu.InputParameters();
            parameters2.CenterOnTm = this.ContextInfoTooltipButtonTm;
            parameters2.MenuContentParams = _.L(ConfigLoca.PET_PERKS_TOOLTIP, null, false);
            TooltipMenu.InputParameters parameter = parameters2;
            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TooltipMenu, MenuContentType.InfoTooltip, parameter, 0f, false, true);
        }

        private void openContextInfo(Character character)
        {
            if (character != null)
            {
                Player player = GameLogic.Binder.GameState.Player;
                this.m_highlightedCharacter = character;
                PetInstance instance = player.Pets.getPetInstance(character.Id);
                if ((instance != null) && (instance.Level >= 1))
                {
                    if (!player.Pets.isPetSelected(character.Id))
                    {
                        CmdSelectPet.ExecuteStatic(player, character.Id);
                    }
                    CmdInspectPet.ExecuteStatic(player, character.Id, false);
                }
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_ButtonTab, (float) 0f);
                this.refreshContextInfo();
                this.ContextInfoCellTm.gameObject.SetActive(true);
            }
        }

        private void reconstructContent()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.cleanupCells();
            for (int i = 0; i < App.Binder.ConfigMeta.PETS.Count; i++)
            {
                PetConfig config = App.Binder.ConfigMeta.PETS[i];
                if (config.Enabled)
                {
                    this.addPetCell(config.Id);
                }
            }
            this.PetUnequipButtonRootTm.SetAsLastSibling();
            bool flag = false;
            if (this.m_highlightedCharacter != null)
            {
                flag = true;
            }
            else if (player.Pets.isPetSelected())
            {
                this.m_highlightedCharacter = player.Pets.getSelectedPetInstance().Character;
                flag = true;
            }
            this.ContextInfoCellTm.gameObject.SetActive(flag);
            this.onRefresh();
        }

        private void refreshContextInfo()
        {
            if (this.m_highlightedCharacter != null)
            {
                PetInstance instance = GameLogic.Binder.GameState.Player.Pets.getPetInstance(this.m_highlightedCharacter);
                bool flag = (instance != null) && (instance.Level > 0);
                this.ContextInfoTitle.text = StringExtensions.ToUpperLoca(this.m_highlightedCharacter.Name);
                this.ContextInfoFlavorText.text = this.m_highlightedCharacter.FlavorText;
                MenuHelpers.RefreshStarContainerWithBackground(this.ContextInfoStars, !flag ? 0 : instance.Level, true);
                if (flag && (instance.SpawnedCharacterInstance != null))
                {
                    this.ContextInfoPetPower.text = MenuHelpers.BigValueToString(instance.SpawnedCharacterInstance.DamagePerHit(false));
                    this.ContextInfoPetPower.gameObject.SetActive(true);
                }
                else
                {
                    this.ContextInfoPetPower.gameObject.SetActive(false);
                }
                float v = 0f;
                if (instance != null)
                {
                    if (instance.isAtMaxLevel())
                    {
                        this.ContextInfoProgressBarText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_MAX, null, false));
                    }
                    else
                    {
                        this.ContextInfoProgressBarText.text = instance.Duplicates + " / " + App.Binder.ConfigMeta.PetRequiredDuplicatesForLevelUp(instance.Level + 1);
                    }
                    v = instance.getNormalizedProgressTowardsLevelUp();
                }
                else
                {
                    this.ContextInfoProgressBarText.text = "0 / " + App.Binder.ConfigMeta.PetRequiredDuplicatesForLevelUp(1);
                }
                this.ContextInfoProgressBar.setNormalizedValue(v);
                this.ContextInfoProgressBarFgDefault.SetActive(v < 1f);
                this.ContextInfoProgressBarFgFull.SetActive(v >= 1f);
                PlayerView.Binder.MenuSystem.initializeMenuPet(this.m_highlightedCharacter.Prefab);
                PlayerView.Binder.MenuSystem.MenuPet.CharacterView.gameObject.SetActive(true);
                PlayerView.Binder.MenuSystem.MenuPet.CharacterView.setVisibility(true);
                PlayerView.Binder.MenuSystem.MenuPet.CharacterView.Transform.rotation = Quaternion.Euler(0f, MathUtil.RandomSign() * UnityEngine.Random.Range((float) 15f, (float) 30f), 0f);
                this.ContextInfoAvatar.texture = PlayerView.Binder.MenuSystem.CharacterMenuCamera.RenderTexture;
                this.ContextInfoAvatar.enabled = true;
                if (this.m_highlightedCharacter.FixedPerks != null)
                {
                    this.ContextInfoPerkCells[0].Text.text = MenuHelpers.GetPetAttackTypeDescription(this.m_highlightedCharacter);
                    this.ContextInfoPerkCells[0].Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_weapon");
                    this.ContextInfoPerkCells[0].Icon.color = !flag ? COLOR_PERK_LOCKED : Color.white;
                    this.ContextInfoPerkCells[0].Text.color = !flag ? COLOR_PERK_LOCKED : Color.white;
                    int num2 = this.m_highlightedCharacter.FixedPerks.count();
                    for (int i = 1; i < this.ContextInfoPerkCells.Count; i++)
                    {
                        PerkCell cell = this.ContextInfoPerkCells[i];
                        int idx = i - 1;
                        if (idx < this.m_highlightedCharacter.FixedPerks.count())
                        {
                            int rankReq = this.m_highlightedCharacter.FixedPerks.Entries[idx].RankReq;
                            PerkInstance instance2 = this.m_highlightedCharacter.FixedPerks.getPerkInstanceAtIndex(idx);
                            ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[instance2.Type];
                            if (data.LinkedToRunestone != null)
                            {
                                ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(data.LinkedToRunestone);
                                cell.Text.text = _.L(ConfigLoca.PET_PERK_DESCRIPTION, new <>__AnonType0<string, string>(_.L(ConfigPerks.SHARED_DATA[runestoneData.PerkInstance.Type].ShortDescription, null, true), _.L(ConfigSkills.SHARED_DATA[runestoneData.LinkedToSkill].Name, null, true)), false);
                            }
                            else
                            {
                                cell.Text.text = MenuHelpers.GetFormattedPerkDescription(instance2.Type, instance2.Modifier, data.DurationSeconds, data.Threshold, 0f, false);
                            }
                            cell.Icon.sprite = !this.highlightPerkAsLegendary(instance2.Type) ? PlayerView.Binder.SpriteResources.getSprite("Menu", data.SmallSprite) : PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_perk_legendary");
                            if (flag && (instance.Level >= rankReq))
                            {
                                cell.Icon.color = Color.white;
                                if (this.highlightPerkAsLegendary(instance2.Type))
                                {
                                    cell.Text.color = ConfigUi.LEGENDARY_PERK_COLOR;
                                }
                                else
                                {
                                    cell.Text.color = cell.Icon.color;
                                }
                            }
                            else
                            {
                                cell.Icon.color = COLOR_PERK_LOCKED;
                                if (this.highlightPerkAsLegendary(instance2.Type))
                                {
                                    cell.Text.color = ConfigUi.LEGENDARY_PERK_COLOR_DIMMED;
                                }
                                else
                                {
                                    cell.Text.color = cell.Icon.color;
                                }
                            }
                            cell.gameObject.SetActive(true);
                            cell.Divider.SetActive(i < this.m_highlightedCharacter.FixedPerks.count());
                        }
                        else
                        {
                            cell.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < this.ContextInfoPerkCells.Count; j++)
                    {
                        this.ContextInfoPerkCells[j].gameObject.SetActive(false);
                    }
                }
            }
        }

        private void refreshUnequipButtons()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.PetUnequipButton.Button.interactable = player.Pets.isPetSelected();
            this.PetUnequipButton.Bg.material = !this.PetUnequipButton.Button.interactable ? PlayerView.Binder.DisabledUiMaterial : null;
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.PetPopupContent;
            }
        }

        public List<PerkCell> ContextInfoPerkCells
        {
            [CompilerGenerated]
            get
            {
                return this.<ContextInfoPerkCells>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ContextInfoPerkCells>k__BackingField = value;
            }
        }

        public override string TabTitle
        {
            get
            {
                return StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_PETS_BUTTON_TEXT, null, false));
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator15C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal PetPopupContent <>f__this;
            internal PetCell <cell>__3;
            internal PetCell <cell>__6;
            internal float <cellCenterPos>__7;
            internal int <centerOnTargetCellIdx>__1;
            internal int <i>__2;
            internal int <i>__5;
            internal Player <player>__0;
            internal string <selectedPetId>__4;
            internal float <totalContentHeight>__8;
            internal float <viewRectHeight>__9;

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
                    this.<centerOnTargetCellIdx>__1 = -1;
                    this.<i>__2 = 0;
                    while (this.<i>__2 < this.<>f__this.m_petCells.Count)
                    {
                        this.<cell>__3 = this.<>f__this.m_petCells[this.<i>__2];
                        if (this.<cell>__3.ActiveContent.Interactable && this.<cell>__3.ActiveContent.Notify)
                        {
                            this.<centerOnTargetCellIdx>__1 = this.<i>__2;
                            break;
                        }
                        this.<i>__2++;
                    }
                    if ((this.<centerOnTargetCellIdx>__1 == -1) && this.<player>__0.Pets.isPetSelected())
                    {
                        this.<selectedPetId>__4 = this.<player>__0.Pets.getSelectedPetInstance().CharacterId;
                        this.<i>__5 = 0;
                        while (this.<i>__5 < this.<>f__this.m_petCells.Count)
                        {
                            if (((Character) this.<>f__this.m_petCells[this.<i>__5].ActiveContent.Obj).Id == this.<selectedPetId>__4)
                            {
                                this.<centerOnTargetCellIdx>__1 = this.<i>__5;
                                break;
                            }
                            this.<i>__5++;
                        }
                    }
                    if (this.<centerOnTargetCellIdx>__1 != -1)
                    {
                        Canvas.ForceUpdateCanvases();
                        this.<cell>__6 = this.<>f__this.m_petCells[this.<centerOnTargetCellIdx>__1];
                        this.<cellCenterPos>__7 = -this.<cell>__6.RectTm.localPosition.y;
                        this.<totalContentHeight>__8 = this.<>f__this.VerticalGroup.rect.height;
                        this.<viewRectHeight>__9 = this.<>f__this.ScrollRect.GetComponent<RectTransform>().rect.height;
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
    }
}

