namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class HeroNamingPopupContent : MenuContent
    {
        public GameObject CloseButtonRoot;
        public GameObject DescriptionRoot;
        public Text DescriptionText;
        public InputField Input;
        public Text InputText;
        private string m_customName;
        private bool m_doAutoFocus;
        private const int NUM_RENAMES_ALLOWED = 1;
        public Button OkButton;
        public Image OkButtonBg;
        public Text OkButtonText;
        public Text RTLText;
        public Text Title;
        public Text TypeName;
        public GameObject WarningRoot;
        public Text WarningText;

        private void completeRenaming()
        {
            CmdRenamePlayer.ExecuteStatic(GameLogic.Binder.GameState.Player, this.m_customName.Trim());
            PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
        }

        protected void LateUpdate()
        {
            if (this.m_doAutoFocus && (EventSystem.current != null))
            {
                this.Input.ActivateInputField();
                this.m_doAutoFocus = false;
            }
        }

        protected override void onAwake()
        {
            this.DescriptionText.text = _.L(ConfigLoca.HERONAMING_DESCRIPTION, null, false);
            this.TypeName.text = "<" + _.L(ConfigLoca.HERONAMING_TYPE_HERE, null, false) + ">";
            this.OkButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_OK, null, false));
            this.Input.onValidateInput = new InputField.OnValidateInput(this.onValidateInput);
            this.Input.shouldHideMobileInput = false;
        }

        public override bool onBackgroundOverlayClicked()
        {
            if (PlayerView.Binder.MenuSystem.InTransition)
            {
                return false;
            }
            this.onCustomCloseButtonClicked();
            return true;
        }

        protected override void onCleanup()
        {
            PlayerView.Binder.InputSystem.setInputRequirement(InputSystem.Layer.MenuHeroNamingPopup, InputSystem.Requirement.Neutral);
        }

        public void onCustomCloseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        public void onInputEndEdit(string str)
        {
            this.updateInput(this.Input.text);
        }

        public void onInputValueChange(string str)
        {
            this.updateInput(this.Input.text);
        }

        public void onOkButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Player player = GameLogic.Binder.GameState.Player;
                if (player.SocialData.HeroNamingCount > 1)
                {
                    this.onCustomCloseButtonClicked();
                }
                else if (player.SocialData.HeroNamingCount == 1)
                {
                    ConfirmationPopupContent.InputParameters parameters;
                    ConfirmationPopupContent.InputParameters parameters2;
                    if (ConfigDevice.IsAndroid())
                    {
                        parameters2 = new ConfirmationPopupContent.InputParameters();
                        parameters2.TitleText = _.L(ConfigLoca.HERONAMING_TITLE, null, false);
                        parameters2.DescriptionText = this.m_customName;
                        parameters2.LeftButtonText = _.L(ConfigLoca.UI_PROMPT_CANCEL, null, false);
                        parameters2.RightButtonText = _.L(ConfigLoca.UI_PROMPT_CONFIRM, null, false);
                        parameters2.RightButtonCallback = new System.Action(this.completeRenaming);
                        parameters2.NavigateBackEqualsToLeftButton = true;
                        parameters = parameters2;
                    }
                    else
                    {
                        parameters2 = new ConfirmationPopupContent.InputParameters();
                        parameters2.TitleText = _.L(ConfigLoca.HERONAMING_TITLE, null, false);
                        parameters2.DescriptionText = this.m_customName;
                        parameters2.LeftButtonText = _.L(ConfigLoca.UI_PROMPT_CONFIRM, null, false);
                        parameters2.RightButtonText = _.L(ConfigLoca.UI_PROMPT_CANCEL, null, false);
                        parameters2.LeftButtonCallback = new System.Action(this.completeRenaming);
                        parameters = parameters2;
                    }
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.ConfirmationPopupContent, parameters, 0f, false, true);
                }
                else
                {
                    this.completeRenaming();
                }
            }
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            LocaSystem locaSystem = App.Binder.LocaSystem;
            bool flag = locaSystem.IsRightToLeft(locaSystem.selectedLanguage);
            this.RTLText.enabled = flag;
            Color color = this.InputText.color;
            color.a = !flag ? color.a : 0f;
            this.InputText.color = color;
            this.Title.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HERONAMING_TITLE, null, false));
            Player player = GameLogic.Binder.GameState.Player;
            if (!player.SocialData.HasGivenCustomName)
            {
                this.Input.text = string.Empty;
            }
            else if (!string.IsNullOrEmpty(player.SocialData.Name))
            {
                Debug.Log(string.Concat(new object[] { "is ", player.SocialData.Name, " arabic letters? - ", App.Binder.LocaSystem.IsArabic(player.SocialData.Name) }));
                if (App.Binder.LocaSystem.IsArabic(player.SocialData.Name))
                {
                    this.Input.text = RTLConverter.Reverse(player.SocialData.Name);
                }
                else
                {
                    this.Input.text = player.SocialData.Name;
                }
            }
            else
            {
                this.Input.text = string.Empty;
            }
            bool flag2 = player.SocialData.HeroNamingCount <= 1;
            this.DescriptionRoot.SetActive(flag2);
            this.Input.interactable = flag2;
            this.WarningRoot.SetActive(player.SocialData.HeroNamingCount > 0);
            if (player.SocialData.HeroNamingCount == 1)
            {
                this.WarningText.text = _.L(ConfigLoca.HERONAMING_WARNING_ONE_REMAINING, null, false);
            }
            else if (!flag2)
            {
                this.WarningText.text = _.L(ConfigLoca.HERONAMING_WARNING_NONE_REMAINING, null, false);
            }
            this.validateCustomName(player);
            this.m_doAutoFocus = true;
            PlayerView.Binder.InputSystem.setInputRequirement(InputSystem.Layer.MenuHeroNamingPopup, InputSystem.Requirement.MustBeEnabled);
        }

        protected override void onRefresh()
        {
        }

        private char onValidateInput(string text, int charIndex, char addedChar)
        {
            UnicodeCategory unicodeCategory = char.GetUnicodeCategory(addedChar);
            return ((((unicodeCategory != UnicodeCategory.OtherNotAssigned) && (unicodeCategory != UnicodeCategory.OtherSymbol)) && (unicodeCategory != UnicodeCategory.Surrogate)) ? addedChar : '\0');
        }

        private void updateInput(string newInput)
        {
            Player player = GameLogic.Binder.GameState.Player;
            LocaSystem locaSystem = App.Binder.LocaSystem;
            if (locaSystem.IsRightToLeft(locaSystem.selectedLanguage))
            {
                newInput = RTLConverter.Reverse(newInput);
                this.RTLText.text = newInput;
            }
            this.m_customName = newInput;
            this.validateCustomName(player);
        }

        private void validateCustomName(Player player)
        {
            bool flag = (player.SocialData.HeroNamingCount > 1) || (ConfigLeaderboard.IsValidLeaderboardName(this.m_customName) && (this.m_customName != player.SocialData.Name));
            this.OkButton.interactable = flag;
            this.OkButtonBg.material = !flag ? PlayerView.Binder.DisabledUiMaterial : null;
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.HeroNamingPopupContent;
            }
        }
    }
}

