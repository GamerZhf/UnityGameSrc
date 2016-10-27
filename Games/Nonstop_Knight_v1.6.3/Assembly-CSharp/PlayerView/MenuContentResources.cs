namespace PlayerView
{
    using App;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class MenuContentResources : MonoBehaviour
    {
        private Dictionary<MenuContentType, GameObject> m_sharedMenuContents = new Dictionary<MenuContentType, GameObject>();

        protected void Awake()
        {
            this.instantiateSharedMenuContent(MenuContentType.ReviveMiniPopup);
            this.instantiateSharedMenuContent(MenuContentType.ItemInfoContent);
            this.instantiateSharedMenuContent(MenuContentType.SpecialOfferAdPromptMiniPopup);
            this.instantiateSharedMenuContent(MenuContentType.IapMiniPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.PendingPurchasesPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.AscendPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.ShopOfferMiniPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.InfoTooltip);
            this.instantiateSharedMenuContent(MenuContentType.SellConfirmationTooltip);
            this.instantiateSharedMenuContent(MenuContentType.OptionsContent);
            this.instantiateSharedMenuContent(MenuContentType.RunestoneInfoContent);
            this.instantiateSharedMenuContent(MenuContentType.HeroPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.LeaderboardPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.SkillPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.ChestGalleryContent);
            this.instantiateSharedMenuContent(MenuContentType.VendorPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.CheatPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.RequiredClientUpdateContent);
            this.instantiateSharedMenuContent(MenuContentType.FacebookConnectPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.HeroNamingPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.FrenzyMiniPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.StarterBundlePopupContent);
            this.instantiateSharedMenuContent(MenuContentType.AppboyPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.PlayerProgressRestorePopupContent);
            this.instantiateSharedMenuContent(MenuContentType.SkillInfoContent);
            this.instantiateSharedMenuContent(MenuContentType.RateGameMiniPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.ConfirmationPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.TechPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.BossPotionMiniPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.PromotionPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.PromotionIAPPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.PetPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.MissionsPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.VendorMiniPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.AchievementPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.SendGiftPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.PromotionEventPopupContent);
            this.instantiateSharedMenuContent(MenuContentType.BannerPopupContent);
        }

        public GameObject getSharedResource(MenuContentType menuContentType)
        {
            return this.m_sharedMenuContents[menuContentType];
        }

        private GameObject instantiateMenuContent(MenuContentType menuContentType)
        {
            GameObject obj2 = App.Binder.AssetBundleLoader.instantiatePrefab("Prefabs/MenuContents/" + menuContentType);
            obj2.name = menuContentType.ToString();
            obj2.transform.SetParent(base.transform, false);
            obj2.SetActive(false);
            return obj2;
        }

        public GameObject instantiateResource(MenuContentType menuContentType)
        {
            return this.instantiateMenuContent(menuContentType);
        }

        private void instantiateSharedMenuContent(MenuContentType menuContentType)
        {
            this.m_sharedMenuContents.Add(menuContentType, this.instantiateMenuContent(menuContentType));
        }
    }
}

