namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class LeaderboardCell : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private GameLogic.LeaderboardEntry <LeaderboardEntry>k__BackingField;
        [CompilerGenerated]
        private int <LeaderboardRank>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public CanvasGroup AlphaGroup;
        public Image BgImage;
        public Text HighestFloorText;
        public Image IconSelectedBorder;
        public UnityEngine.UI.LayoutElement LayoutElement;
        public PlayerView.LeaderboardImage LeaderboardImage;
        private Font m_defaultNameFont;
        public Text NameText;
        public Text RankText;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            this.m_defaultNameFont = this.NameText.font;
        }

        public void cleanUpForReuse()
        {
        }

        public void initialize(bool stripedRow, float height)
        {
            if (this.BgImage != null)
            {
                this.BgImage.color = !stripedRow ? ConfigUi.LIST_CELL_REGULAR_COLOR : ConfigUi.LIST_CELL_STRIPED_COLOR;
            }
            this.LayoutElement.minHeight = height;
        }

        public void refresh(int leaderboardRank, GameLogic.LeaderboardEntry lbe, Texture2D avatarTextureRaw, bool showAvatar)
        {
            this.LeaderboardRank = leaderboardRank;
            this.LeaderboardEntry = lbe;
            this.NameText.text = this.LeaderboardEntry.getPrettyName();
            this.NameText.font = this.m_defaultNameFont;
            this.RankText.text = leaderboardRank + ".";
            this.HighestFloorText.text = this.LeaderboardEntry.HighestFloor.ToString();
            if (showAvatar)
            {
                this.LeaderboardImage.gameObject.SetActive(true);
                this.LeaderboardImage.refresh(this.LeaderboardEntry.AvatarSpriteId, avatarTextureRaw);
                this.NameText.rectTransform.anchoredPosition = new Vector2(300f, 0f);
            }
            else
            {
                this.LeaderboardImage.gameObject.SetActive(false);
                this.NameText.rectTransform.anchoredPosition = new Vector2(200f, 0f);
            }
            this.IconSelectedBorder.enabled = lbe.IsSelf;
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

        public int LeaderboardRank
        {
            [CompilerGenerated]
            get
            {
                return this.<LeaderboardRank>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<LeaderboardRank>k__BackingField = value;
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

