namespace PlayerView
{
    using GameLogic;
    using Service.SupersonicAds;
    using System;
    using UnityEngine;

    public interface IEventBus
    {
        event PlayerView.Events.AdWatched OnAdWatched;

        event PlayerView.Events.AdWatchStarted OnAdWatchStarted;

        event PlayerView.Events.AppboyAction OnAppboyAction;

        event PlayerView.Events.DungeonDropViewResourcesCollected OnDungeonDropViewResourcesCollected;

        event PlayerView.Events.DungeonHudProgressBarHidingStarted OnDungeonHudProgressBarHidingStarted;

        event PlayerView.Events.DungeonHudProgressBarShowingStarted OnDungeonHudProgressBarShowingStarted;

        event PlayerView.Events.FlyToHudStarted OnFlyToHudStarted;

        event PlayerView.Events.GesturePanCompleted OnGesturePanCompleted;

        event PlayerView.Events.GesturePanRecognized OnGesturePanRecognized;

        event PlayerView.Events.GestureSwipeRecognized OnGestureSwipeRecognized;

        event PlayerView.Events.GestureTapRecognized OnGestureTapRecognized;

        event PlayerView.Events.MenuChanged OnMenuChanged;

        event PlayerView.Events.MenuChangeStarted OnMenuChangeStarted;

        event PlayerView.Events.MenuContentChanged OnMenuContentChanged;

        event PlayerView.Events.MenuShowStarted OnMenuShowStarted;

        event PlayerView.Events.NavigateBack OnNavigateBack;

        event PlayerView.Events.ShopEntryPurchased OnShopEntryPurchased;

        event PlayerView.Events.SpecialOfferAdAccepted OnSpecialOfferAdAccepted;

        event PlayerView.Events.SpecialOfferAdOffered OnSpecialOfferAdOffered;

        event PlayerView.Events.SpecialOfferAdRejected OnSpecialOfferAdRejected;

        event PlayerView.Events.SpecialOfferAdWatched OnSpecialOfferAdWatched;

        void AdWatched(VideoResult result);
        void AdWatchStarted(string adZoneId, Reward reward);
        void AppboyAction(string action, string campaignId);
        void DungeonDropViewResourcesCollected(ResourceType resourceType, double amount, Vector3 worldPos);
        void DungeonHudProgressBarHidingStarted();
        void DungeonHudProgressBarShowingStarted();
        void FlyToHudStarted(Vector2 sourceScreenPos);
        void GesturePanCompleted(TKPanRecognizer r);
        void GesturePanRecognized(TKPanRecognizer r);
        void GestureSwipeRecognized(TKSwipeRecognizer r);
        void GestureTapRecognized(TKTapRecognizer r);
        void MenuChanged(Menu sourceMenu, Menu targetMenu);
        void MenuChangeStarted(MenuType sourceMenuType, MenuType targetMenuType);
        void MenuContentChanged(GameObject parent);
        void MenuShowStarted(MenuType targetMenuType);
        void NavigateBack();
        void ShopEntryPurchased(Player player, ShopEntryInstance shopEntryInstance, ShopEntry shopEntry, ResourceType spentResource, double price, int numPurchases);
        void SpecialOfferAdAccepted(Reward reward);
        void SpecialOfferAdOffered(Reward reward);
        void SpecialOfferAdRejected(Reward reward);
        void SpecialOfferAdWatched(Reward reward);
    }
}

