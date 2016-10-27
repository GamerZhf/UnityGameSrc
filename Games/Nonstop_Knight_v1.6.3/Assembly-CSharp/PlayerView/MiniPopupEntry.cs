namespace PlayerView
{
    using App;
    using System;

    public class MiniPopupEntry
    {
        public SpriteAtlasEntry ButtonBackground;
        public string ButtonText;
        public string DescriptionText;
        public bool HideCloseButton;
        public bool ShowAdditionalShopButton;
        public SpriteAtlasEntry TitleIcon;
        public string TitleText;

        public MiniPopupEntry()
        {
        }

        public MiniPopupEntry(MiniPopupEntry another)
        {
            this.TitleText = another.TitleText;
            this.TitleIcon = another.TitleIcon;
            this.DescriptionText = another.DescriptionText;
            this.ButtonBackground = another.ButtonBackground;
            this.ButtonText = another.ButtonText;
            this.ShowAdditionalShopButton = another.ShowAdditionalShopButton;
            this.HideCloseButton = another.HideCloseButton;
        }
    }
}

