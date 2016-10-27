namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class HeroCell : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private GameLogic.Character <Character>k__BackingField;
        [CompilerGenerated]
        private GameLogic.CharacterInstance <CharacterInstance>k__BackingField;
        public Image Bg;
        public Button HeroButton;
        public Image Icon;
        public Image LevelIcon;
        public Text LevelText;
        private Material m_greyscaleMaterial;
        public AnimatedProgressBar ProgressBar;
        public Button UpgradeButton;

        protected void Awake()
        {
            this.m_greyscaleMaterial = Resources.Load<Material>("Materials/image_greyscale");
        }

        public void cleanUpForReuse()
        {
        }

        public void initialize(GameLogic.CharacterInstance characterInstance, GameLogic.Character character)
        {
            this.CharacterInstance = characterInstance;
            this.Character = character;
            if ((characterInstance == null) && (character == null))
            {
                this.UpgradeButton.gameObject.SetActive(false);
                this.ProgressBar.gameObject.SetActive(true);
                this.ProgressBar.setNormalizedValue(0f);
                this.HeroButton.interactable = false;
                this.Bg.material = this.m_greyscaleMaterial;
                this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "avatar_hero001");
                this.Icon.material = this.m_greyscaleMaterial;
                this.LevelIcon.material = this.m_greyscaleMaterial;
                this.LevelText.text = string.Empty;
            }
            else if (characterInstance != null)
            {
                this.UpgradeButton.gameObject.SetActive(true);
                this.ProgressBar.gameObject.SetActive(false);
                this.HeroButton.interactable = true;
                this.Bg.material = null;
                this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", characterInstance.Character.AvatarSpriteId);
                this.Icon.material = null;
                this.LevelIcon.material = null;
                this.LevelText.text = characterInstance.Rank.ToString();
            }
            else
            {
                this.UpgradeButton.gameObject.SetActive(false);
                this.ProgressBar.gameObject.SetActive(true);
                this.HeroButton.interactable = true;
                this.Bg.material = this.m_greyscaleMaterial;
                this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", character.AvatarSpriteId);
                this.Icon.material = this.m_greyscaleMaterial;
                this.LevelIcon.material = this.m_greyscaleMaterial;
                this.LevelText.text = string.Empty;
            }
        }

        public void onButtonClicked()
        {
        }

        public GameLogic.Character Character
        {
            [CompilerGenerated]
            get
            {
                return this.<Character>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Character>k__BackingField = value;
            }
        }

        public GameLogic.CharacterInstance CharacterInstance
        {
            [CompilerGenerated]
            get
            {
                return this.<CharacterInstance>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CharacterInstance>k__BackingField = value;
            }
        }
    }
}

