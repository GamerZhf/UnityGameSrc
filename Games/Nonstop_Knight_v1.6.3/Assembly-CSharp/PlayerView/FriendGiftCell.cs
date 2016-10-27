namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class FriendGiftCell : MonoBehaviour
    {
        [CompilerGenerated]
        private FbPlatformUser <FbUser>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public CanvasGroup AlphaGroup;
        public Image BgImage;
        public UnityEngine.UI.LayoutElement LayoutElement;
        public PlayerView.LeaderboardImage LeaderboardImage;
        public Text NameText;
        public CellButton SendGiftButton;

        protected void Awake()
        {
        }

        public void cleanUp()
        {
            this.LeaderboardImage.refresh(null, null);
            this.FbUser = null;
            this.NameText.text = null;
            base.gameObject.SetActive(false);
        }

        public void initialize(bool stripedRow, FbPlatformUser FbUser)
        {
            this.SendGiftButton.setCellButtonStyle(CellButtonType.Unlock, "SEND");
            this.FbUser = FbUser;
            this.NameText.text = FbUser.userName;
            if (this.BgImage != null)
            {
                this.BgImage.color = !stripedRow ? ConfigUi.LIST_CELL_REGULAR_COLOR : ConfigUi.LIST_CELL_STRIPED_COLOR;
            }
            this.NameText.text = this.FbUser.getPrettyName();
            base.gameObject.SetActive(true);
        }

        public void onSendButtonClicked()
        {
            if (this.SendGiftButton.ActiveType == CellButtonType.Unlock)
            {
                string facebookId = GameLogic.Binder.GameState.Player.SocialData.FacebookId;
                if (this.FbUser.id == null)
                {
                }
                SocialInboxCommand inboxCommand = SocialGift.CreateFacebookGiftCommand(facebookId, facebookId, "PetBoxSmall");
                Service.Binder.PlayerService.SendSocialInboxCommand(inboxCommand);
                this.SendGiftButton.setCellButtonStyle(CellButtonType.UnlockLocked, "SENT");
            }
        }

        public void refresh()
        {
        }

        public FbPlatformUser FbUser
        {
            [CompilerGenerated]
            get
            {
                return this.<FbUser>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<FbUser>k__BackingField = value;
            }
        }

        public RectTransform RectTm
        {
            [CompilerGenerated]
            get
            {
                return this.<RectTm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RectTm>k__BackingField = value;
            }
        }
    }
}

