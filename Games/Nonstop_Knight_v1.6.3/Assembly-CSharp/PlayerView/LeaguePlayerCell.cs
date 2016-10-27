namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class LeaguePlayerCell : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private GameLogic.LeaderboardEntry <LeaderboardEntry>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        [CompilerGenerated]
        private bool <StripedRow>k__BackingField;
        public static Color ACTIVE_PLAYER_CELL_BG_COLOR = new Color(0.3843137f, 0.3764706f, 0.6627451f, 1f);
        public Image Bg;
        public ImageFlashEffect Flash;
        public PlayerView.LeaderboardImage LeaderboardImage;
        private Font m_defaultNameFont;
        public Text NameText;
        public Text NumberText;
        public Text RankText;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            this.m_defaultNameFont = this.NameText.font;
        }

        public void cleanUpForReuse()
        {
        }

        public void initialize(int rank, bool stripedRow)
        {
            this.RankText.text = rank + ".";
            this.StripedRow = stripedRow;
        }

        public void refresh(GameLogic.LeaderboardEntry lbe, Texture2D avatarTextureRaw, bool selected)
        {
            this.LeaderboardEntry = lbe;
            this.NameText.text = this.LeaderboardEntry.getPrettyName();
            this.NameText.font = this.m_defaultNameFont;
            this.NumberText.text = MenuHelpers.BigValueToString(0.0);
            this.LeaderboardImage.refresh(this.LeaderboardEntry.AvatarSpriteId, avatarTextureRaw);
            if (selected)
            {
                this.Bg.color = ACTIVE_PLAYER_CELL_BG_COLOR;
            }
            else
            {
                this.Bg.color = !this.StripedRow ? ConfigUi.LIST_CELL_REGULAR_COLOR : ConfigUi.LIST_CELL_STRIPED_COLOR;
            }
        }

        public GameLogic.LeaderboardEntry LeaderboardEntry
        {
            [CompilerGenerated]
            get
            {
                return this.<LeaderboardEntry>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<LeaderboardEntry>k__BackingField = value;
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

        public bool StripedRow
        {
            [CompilerGenerated]
            get
            {
                return this.<StripedRow>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<StripedRow>k__BackingField = value;
            }
        }
    }
}

