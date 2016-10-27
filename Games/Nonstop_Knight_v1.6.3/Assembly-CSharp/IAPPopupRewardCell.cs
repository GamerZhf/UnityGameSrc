using App;
using GameLogic;
using PlayerView;
using System;
using UnityEngine;
using UnityEngine.UI;

public class IAPPopupRewardCell : MonoBehaviour
{
    public Text Description;
    public Image Icon;

    private string GetRewardEntryDropTitle(Reward reward)
    {
        if (reward.getTotalDiamondAmount() > 0.0)
        {
            return _.L(ConfigLoca.RESOURCES_DIAMONDS_MULTILINE, new <>__AnonType9<string>(reward.getTotalDiamondAmount().ToString("0")), false);
        }
        if (reward.getTotalCoinAmount() > 0.0)
        {
            return _.L(ConfigLoca.RESOURCES_COINS_MULTILINE, new <>__AnonType9<string>(MenuHelpers.BigValueToString(reward.getTotalCoinAmount())), false);
        }
        if (reward.getTotalTokenAmount() > 0.0)
        {
            return _.L(ConfigLoca.RESOURCES_TOKENS_MULTILINE, new <>__AnonType9<string>(MenuHelpers.BigValueToString(reward.getTotalTokenAmount())), false);
        }
        if (reward.FrenzyPotions > 0)
        {
            return _.L(ConfigLoca.VENDOR_FRENZY_POTIONS, new <>__AnonType9<string>(MenuHelpers.BigValueToString((double) reward.FrenzyPotions)), false);
        }
        if (reward.Revives > 0)
        {
            return _.L(ConfigLoca.VENDOR_REVIVES, new <>__AnonType9<string>(MenuHelpers.BigValueToString((double) reward.Revives)), false);
        }
        if (reward.MegaBoxes > 0)
        {
            int megaBoxes = reward.MegaBoxes;
            return ((megaBoxes != 1) ? _.L(ConfigLoca.VENDOR_MEGA_BOX_BUNDLE_PLURAL, new <>__AnonType9<int>(megaBoxes), false) : _.L(ConfigLoca.VENDOR_MEGA_BOX_BUNDLE, new <>__AnonType9<int>(megaBoxes), false));
        }
        if ((reward.Pets != null) && (reward.Pets.Count > 0))
        {
            PetReward reward2 = reward.Pets[0];
            object[] objArray1 = new object[] { reward2.Amount, "\n", GameLogic.Binder.CharacterResources.getResource(reward2.PetId).Name, " shards" };
            return string.Concat(objArray1);
        }
        return "???";
    }

    public void InitForReward(Reward reward)
    {
        SpriteAtlasEntry atlasEntry = new SpriteAtlasEntry("Menu", reward.Sprite);
        this.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(atlasEntry);
        this.Description.text = StringExtensions.ToUpperLoca(this.GetRewardEntryDropTitle(reward));
    }
}

