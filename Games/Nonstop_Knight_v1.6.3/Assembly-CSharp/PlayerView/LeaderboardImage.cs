namespace PlayerView
{
    using GameLogic;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class LeaderboardImage : MonoBehaviour
    {
        public RawImage AvatarRaw;
        public Image AvatarSprite;

        public void refresh(LeaderboardEntry lbe)
        {
            this.refresh(lbe.AvatarSpriteId, lbe.ImageTexture);
        }

        public void refresh(string avatarSpriteId, Texture2D rawTexture)
        {
            if (string.IsNullOrEmpty(avatarSpriteId) && (rawTexture == null))
            {
                this.AvatarSprite.enabled = true;
                this.AvatarRaw.enabled = false;
                this.AvatarSprite.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_knight_unknown_256");
            }
            else if (((rawTexture != null) && (rawTexture.width > 0x31)) && (rawTexture.height > 0x31))
            {
                this.AvatarSprite.enabled = false;
                this.AvatarRaw.enabled = true;
                this.AvatarRaw.texture = rawTexture;
            }
            else
            {
                this.AvatarSprite.enabled = true;
                this.AvatarRaw.enabled = false;
                this.AvatarSprite.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", !string.IsNullOrEmpty(avatarSpriteId) ? avatarSpriteId : "sprite_knight_unknown_256");
            }
        }
    }
}

