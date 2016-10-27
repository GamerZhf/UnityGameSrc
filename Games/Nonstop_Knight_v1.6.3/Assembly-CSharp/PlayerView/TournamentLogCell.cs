namespace PlayerView
{
    using App;
    using Service;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class TournamentLogCell : MonoBehaviour
    {
        public Image BgImage;
        public Image LogIcon;
        public Text LogText;

        public void initialize(bool striped)
        {
            this.BgImage.color = !striped ? ConfigUi.LIST_CELL_STRIPED_COLOR : ConfigUi.LIST_CELL_REGULAR_COLOR;
        }

        public void refresh(TournamentLogEvent logEvent)
        {
            this.LogText.text = logEvent.LogDisplayText;
            this.LogIcon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", logEvent.IconIdentifier);
        }
    }
}

