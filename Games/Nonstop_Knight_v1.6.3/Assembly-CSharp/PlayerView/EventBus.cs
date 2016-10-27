namespace PlayerView
{
    using GameLogic;
    using Service.SupersonicAds;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class EventBus : PlayerView.IEventBus
    {
        public event PlayerView.Events.AdWatched OnAdWatched;

        public event PlayerView.Events.AdWatchStarted OnAdWatchStarted;

        public event PlayerView.Events.AppboyAction OnAppboyAction;

        public event PlayerView.Events.DungeonDropViewResourcesCollected OnDungeonDropViewResourcesCollected;

        public event PlayerView.Events.DungeonHudProgressBarHidingStarted OnDungeonHudProgressBarHidingStarted;

        public event PlayerView.Events.DungeonHudProgressBarShowingStarted OnDungeonHudProgressBarShowingStarted;

        public event PlayerView.Events.FlyToHudStarted OnFlyToHudStarted;

        public event PlayerView.Events.GesturePanCompleted OnGesturePanCompleted;

        public event PlayerView.Events.GesturePanRecognized OnGesturePanRecognized;

        public event PlayerView.Events.GestureSwipeRecognized OnGestureSwipeRecognized;

        public event PlayerView.Events.GestureTapRecognized OnGestureTapRecognized;

        public event PlayerView.Events.MenuChanged OnMenuChanged;

        public event PlayerView.Events.MenuChangeStarted OnMenuChangeStarted;

        public event PlayerView.Events.MenuContentChanged OnMenuContentChanged;

        public event PlayerView.Events.MenuShowStarted OnMenuShowStarted;

        public event PlayerView.Events.NavigateBack OnNavigateBack;

        public event PlayerView.Events.ShopEntryPurchased OnShopEntryPurchased;

        public event PlayerView.Events.SpecialOfferAdAccepted OnSpecialOfferAdAccepted;

        public event PlayerView.Events.SpecialOfferAdOffered OnSpecialOfferAdOffered;

        public event PlayerView.Events.SpecialOfferAdRejected OnSpecialOfferAdRejected;

        public event PlayerView.Events.SpecialOfferAdWatched OnSpecialOfferAdWatched;

        public void AdWatched(VideoResult result)
        {
            if (this.OnAdWatched != null)
            {
                this.OnAdWatched(result);
            }
        }

        public void AdWatchStarted(string adZoneId, Reward reward)
        {
            if (this.OnAdWatchStarted != null)
            {
                this.OnAdWatchStarted(adZoneId, reward);
            }
        }

        public void AppboyAction(string action, string campaignId)
        {
            if (this.OnAppboyAction != null)
            {
                this.OnAppboyAction(action, campaignId);
            }
        }

        public void DungeonDropViewResourcesCollected(ResourceType resourceType, double amount, Vector3 worldPos)
        {
            if (this.OnDungeonDropViewResourcesCollected != null)
            {
                this.OnDungeonDropViewResourcesCollected(resourceType, amount, worldPos);
            }
        }

        public void DungeonHudProgressBarHidingStarted()
        {
            if (this.OnDungeonHudProgressBarHidingStarted != null)
            {
                this.OnDungeonHudProgressBarHidingStarted();
            }
        }

        public void DungeonHudProgressBarShowingStarted()
        {
            if (this.OnDungeonHudProgressBarShowingStarted != null)
            {
                this.OnDungeonHudProgressBarShowingStarted();
            }
        }

        public void FlyToHudStarted(Vector2 sourceScreenPos)
        {
            if (this.OnFlyToHudStarted != null)
            {
                this.OnFlyToHudStarted(sourceScreenPos);
            }
        }

        public void GesturePanCompleted(TKPanRecognizer r)
        {
            if (this.OnGesturePanCompleted != null)
            {
                this.OnGesturePanCompleted(r);
            }
        }

        public void GesturePanRecognized(TKPanRecognizer r)
        {
            if (this.OnGesturePanRecognized != null)
            {
                this.OnGesturePanRecognized(r);
            }
        }

        public void GestureSwipeRecognized(TKSwipeRecognizer r)
        {
            if (this.OnGestureSwipeRecognized != null)
            {
                this.OnGestureSwipeRecognized(r);
            }
        }

        public void GestureTapRecognized(TKTapRecognizer r)
        {
            if (this.OnGestureTapRecognized != null)
            {
                this.OnGestureTapRecognized(r);
            }
        }

        public void MenuChanged(Menu sourceMenu, Menu targetMenu)
        {
            if (this.OnMenuChanged != null)
            {
                this.OnMenuChanged(sourceMenu, targetMenu);
            }
        }

        public void MenuChangeStarted(MenuType sourceMenuType, MenuType targetMenuType)
        {
            if (this.OnMenuChangeStarted != null)
            {
                this.OnMenuChangeStarted(sourceMenuType, targetMenuType);
            }
        }

        public void MenuContentChanged(GameObject content)
        {
            if (this.OnMenuContentChanged != null)
            {
                this.OnMenuContentChanged(content);
            }
        }

        public void MenuShowStarted(MenuType targetMenuType)
        {
            if (this.OnMenuShowStarted != null)
            {
                this.OnMenuShowStarted(targetMenuType);
            }
        }

        public void NavigateBack()
        {
            if (this.OnNavigateBack != null)
            {
                this.OnNavigateBack();
            }
        }

        public void ShopEntryPurchased(Player player, ShopEntryInstance shopEntryInstance, ShopEntry shopEntry, ResourceType spentResource, double price, int numPurchases)
        {
            if (this.OnShopEntryPurchased != null)
            {
                this.OnShopEntryPurchased(player, shopEntryInstance, shopEntry, spentResource, price, numPurchases);
            }
        }

        public void SpecialOfferAdAccepted(Reward reward)
        {
            if (this.OnSpecialOfferAdAccepted != null)
            {
                this.OnSpecialOfferAdAccepted(reward);
            }
        }

        public void SpecialOfferAdOffered(Reward reward)
        {
            if (this.OnSpecialOfferAdOffered != null)
            {
                this.OnSpecialOfferAdOffered(reward);
            }
        }

        public void SpecialOfferAdRejected(Reward reward)
        {
            if (this.OnSpecialOfferAdRejected != null)
            {
                this.OnSpecialOfferAdRejected(reward);
            }
        }

        public void SpecialOfferAdWatched(Reward reward)
        {
            if (this.OnSpecialOfferAdWatched != null)
            {
                this.OnSpecialOfferAdWatched(reward);
            }
        }
    }
}

