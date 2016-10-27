namespace PlayerView
{
    using App;
    using Service;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class TournamentPlayerCell : MonoBehaviour
    {
        public Image BgImage;
        public Text ContributionText;
        public GameObject GoldenBg;
        public UnityEngine.UI.LayoutElement LayoutElement;
        private TournamentEntry m_tournamentEntry;
        public Text NameText;
        public Image TopContributorImage;

        protected void Awake()
        {
        }

        public void initialize(TournamentEntry tournamentEntry, bool stripedRow, int contributorRank)
        {
            if (this.BgImage != null)
            {
                this.BgImage.color = !stripedRow ? ConfigUi.LIST_CELL_REGULAR_COLOR : ConfigUi.LIST_CELL_STRIPED_COLOR;
            }
            this.m_tournamentEntry = tournamentEntry;
            this.NameText.text = tournamentEntry.PlayerDisplayName;
            if (this.TopContributorImage != null)
            {
                if (contributorRank == 1)
                {
                    this.TopContributorImage.enabled = true;
                    this.TopContributorImage.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_contributor_gold");
                }
                else if (contributorRank == 2)
                {
                    this.TopContributorImage.enabled = true;
                    this.TopContributorImage.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_contributor_silver");
                }
                else if (contributorRank == 3)
                {
                    this.TopContributorImage.enabled = true;
                    this.TopContributorImage.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_mini_contributor_bronze");
                }
                else
                {
                    this.TopContributorImage.enabled = false;
                }
            }
            this.refresh();
        }

        public void refresh()
        {
            if (this.m_tournamentEntry != null)
            {
                this.ContributionText.text = this.m_tournamentEntry.Contribution.ToString();
            }
        }
    }
}

