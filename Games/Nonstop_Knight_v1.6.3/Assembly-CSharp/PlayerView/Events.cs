namespace PlayerView
{
    using GameLogic;
    using Service.SupersonicAds;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Events
    {
        public delegate void AdWatched(VideoResult result);

        public delegate void AdWatchStarted(string adZoneId, Reward reward);

        public delegate void AppboyAction(string action, string campaignId);

        public delegate void DungeonDropViewResourcesCollected(ResourceType resourceType, double amount, Vector3 worldPos);

        public delegate void DungeonHudProgressBarHidingStarted();

        public delegate void DungeonHudProgressBarShowingStarted();

        public delegate void FlyToHudStarted(Vector2 sourceScreenPos);

        public delegate void GesturePanCompleted(TKPanRecognizer r);

        public delegate void GesturePanRecognized(TKPanRecognizer r);

        public delegate void GestureSwipeRecognized(TKSwipeRecognizer r);

        public delegate void GestureTapRecognized(TKTapRecognizer r);

        public delegate void MenuChanged(Menu sourceMenu, Menu targetMenu);

        public delegate void MenuChangeStarted(MenuType sourceMenuType, MenuType targetMenuType);

        public delegate void MenuContentChanged(GameObject parent);

        public delegate void MenuShowStarted(MenuType targetMenuType);

        public delegate void NavigateBack();

        public delegate void ShopEntryPurchased(Player player, ShopEntryInstance shopEntryInstance, ShopEntry shopEntry, ResourceType spentResource, double price, int numPurchases);

        public delegate void SpecialOfferAdAccepted(Reward reward);

        public delegate void SpecialOfferAdOffered(Reward reward);

        public delegate void SpecialOfferAdRejected(Reward reward);

        public delegate void SpecialOfferAdWatched(Reward reward);
    }
}

