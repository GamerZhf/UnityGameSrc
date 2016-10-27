namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class PotionCell : MonoBehaviour
    {
        [CompilerGenerated]
        private GameLogic.PotionType <PotionType>k__BackingField;
        public CanvasGroup AlphaGroup;
        public Image Bg;
        public PlayerView.CellButton CellButton;
        public Text Description;
        public Image Icon;
        public Image IconBackground;
        private int m_remaining;
        public Text Remaining;
        public Text Title;

        public void initialize(GameLogic.PotionType potionType, bool stripedRow)
        {
            this.PotionType = potionType;
            this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(ConfigUi.POTION_TYPE_SPRITES[potionType]);
            switch (potionType)
            {
                case GameLogic.PotionType.Revive:
                    this.Title.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.MINIPOPUP_REVIVE_TITLE, null, false));
                    this.Description.text = _.L(ConfigLoca.MINIPOPUP_REVIVE_DESCRIPTION, null, false);
                    this.CellButton.setCellButtonStyle(CellButtonType.DrinkDisabled, StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_DRINK, null, false)));
                    break;

                case GameLogic.PotionType.Frenzy:
                    this.Title.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.MINIPOPUP_FRENZY_TITLE, null, false));
                    this.Description.text = _.L(ConfigLoca.MINIPOPUP_FRENZY_DESCRIPTION, null, false);
                    this.CellButton.setCellButtonStyle(CellButtonType.DrinkDisabled, StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_DRINK, null, false)));
                    break;

                case GameLogic.PotionType.Boss:
                    this.Title.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.MINIPOPUP_BOSS_POTION_TITLE, null, false));
                    this.Description.text = _.L(ConfigLoca.MINIPOPUP_BOSS_POTION_DESCRIPTION, null, false);
                    this.CellButton.setCellButtonStyle(CellButtonType.DrinkDisabled, StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_USE, null, false)));
                    break;
            }
            this.Bg.color = !stripedRow ? ConfigUi.LIST_CELL_REGULAR_COLOR : ConfigUi.LIST_CELL_STRIPED_COLOR;
        }

        public void onClick()
        {
            if (this.CellButton.ActiveType == CellButtonType.Drink)
            {
                Player player = GameLogic.Binder.GameState.Player;
                CharacterInstance activeCharacter = player.ActiveCharacter;
                switch (this.PotionType)
                {
                    case GameLogic.PotionType.Frenzy:
                        if (activeCharacter.Inventory.FrenzyPotions > 0)
                        {
                            CmdGainPotions.ExecuteStatic(activeCharacter, GameLogic.PotionType.Frenzy, -1);
                            GameLogic.Binder.FrenzySystem.activateFrenzy();
                            this.m_remaining = Mathf.Max(this.m_remaining - 1, 0);
                            this.refresh(this.m_remaining, false);
                        }
                        break;

                    case GameLogic.PotionType.Boss:
                        if (activeCharacter.Inventory.BossPotions > 0)
                        {
                            CmdGainPotions.ExecuteStatic(activeCharacter, GameLogic.PotionType.Boss, -1);
                            CmdStartBossTrain.ExecuteStatic(GameLogic.Binder.GameState.ActiveDungeon, player, App.Binder.ConfigMeta.BOSS_POTION_NUM_BOSSES);
                            this.m_remaining = Mathf.Max(this.m_remaining - 1, 0);
                            this.refresh(this.m_remaining, false);
                        }
                        break;
                }
            }
        }

        public void refresh(int remaining, bool interactable)
        {
            this.m_remaining = remaining;
            this.Remaining.text = "x" + remaining;
            if (interactable)
            {
                this.CellButton.setCellButtonStyle(CellButtonType.Drink);
            }
            else
            {
                this.CellButton.setCellButtonStyle(CellButtonType.DrinkDisabled);
            }
        }

        public GameLogic.PotionType PotionType
        {
            [CompilerGenerated]
            get
            {
                return this.<PotionType>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PotionType>k__BackingField = value;
            }
        }
    }
}

