namespace Service
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class EventBus : IEventBus
    {
        private bool? cachedNetworkState;

        public event Service.Events.AppboyIngameMessageReady OnAppboyIngameMessageReady;

        public event Service.Events.ContentReady OnContentReady;

        public event Service.Events.IapShopInitialized OnIapShopInitialized;

        public event Service.Events.IapShopProductsUpdated OnIapShopProductsUpdated;

        public event Service.Events.IapShopPurchase OnIapShopPurchase;

        public event Service.Events.IapShopStateChanged OnIapShopStateChanged;

        public event Service.Events.InboxCommands OnInboxCommands;

        public event Service.Events.LocalTournamentViewsRefreshed OnLocalTournamentViewsRefreshed;

        public event Service.Events.NetworkStateChanged OnNetworkStateChanged;

        public event Service.Events.NewContentAvailable OnNewContentAvailable;

        public event Service.Events.PlayerLoggedIn OnPlayerLoggedIn;

        public event Service.Events.PlayerRegistered OnPlayerRegistered;

        public event Service.Events.PromotionAction OnPromotionAction;

        public event Service.Events.PromotionsAvailable OnPromotionsAvailable;

        public event Service.Events.ServerTimeSynced OnServerTimeSynced;

        public event Service.Events.TournamentCardsReceived OnTournamentCardsReceived;

        public event Service.Events.TournamentLogUpdated OnTournamentLogUpdated;

        public event Events.RemoteTournamentViewsUpdated OnTournamentRemoteViewsUpdated;

        public event Events.TrackingSendEvent OnTrackingEvent;

        public void AppboyIngameMessageReady()
        {
            if (this.OnAppboyIngameMessageReady != null)
            {
                this.OnAppboyIngameMessageReady();
            }
        }

        public void ContentReady()
        {
            if (this.OnContentReady != null)
            {
                this.OnContentReady();
            }
        }

        public void IapShopInitialized()
        {
            if (this.OnIapShopInitialized != null)
            {
                this.OnIapShopInitialized();
            }
        }

        public void IapShopProductsUpdated()
        {
            if (this.OnIapShopProductsUpdated != null)
            {
                this.OnIapShopProductsUpdated();
            }
        }

        public void IapShopPurchase(PremiumProduct product)
        {
            if (this.OnIapShopPurchase != null)
            {
                this.OnIapShopPurchase(product);
            }
        }

        public void IapShopStateChanged()
        {
            if (this.OnIapShopStateChanged != null)
            {
                this.OnIapShopStateChanged();
            }
        }

        public void InboxCommands(List<InboxCommand> inboxCommands)
        {
            if (this.OnInboxCommands != null)
            {
                this.OnInboxCommands(inboxCommands);
            }
        }

        public void LocalTournamentViewsRefreshed()
        {
            if (this.OnLocalTournamentViewsRefreshed != null)
            {
                this.OnLocalTournamentViewsRefreshed();
            }
        }

        public void NetworkStateChanged(bool online)
        {
            if (!this.cachedNetworkState.HasValue)
            {
                this.cachedNetworkState = new bool?(online);
                if (this.OnNetworkStateChanged != null)
                {
                    this.OnNetworkStateChanged(this.cachedNetworkState.Value);
                }
            }
            else if (this.cachedNetworkState.Value != online)
            {
                this.cachedNetworkState = new bool?(online);
                if (this.OnNetworkStateChanged != null)
                {
                    this.OnNetworkStateChanged(this.cachedNetworkState.Value);
                }
            }
        }

        public void NewContentAvailable()
        {
            if (this.OnNewContentAvailable != null)
            {
                this.OnNewContentAvailable();
            }
        }

        public void PlayerLoggedIn()
        {
            if (this.OnPlayerLoggedIn != null)
            {
                this.OnPlayerLoggedIn();
            }
        }

        public void PlayerRegistered()
        {
            if (this.OnPlayerRegistered != null)
            {
                this.OnPlayerRegistered();
            }
        }

        public void PromotionAction(string name, string action)
        {
            if (this.OnPromotionAction != null)
            {
                this.OnPromotionAction(name, action);
            }
        }

        public void PromotionsAvailable()
        {
            if (this.OnPromotionsAvailable != null)
            {
                this.OnPromotionsAvailable();
            }
        }

        public void ServerTimeSynced()
        {
            if (this.OnServerTimeSynced != null)
            {
                this.OnServerTimeSynced();
            }
        }

        public void TournamentCardsReceived(string tournamentId)
        {
            if (this.OnTournamentCardsReceived != null)
            {
                this.OnTournamentCardsReceived(tournamentId);
            }
        }

        public void TournamentLogUpdated()
        {
            if (this.OnTournamentLogUpdated != null)
            {
                this.OnTournamentLogUpdated();
            }
        }

        public void TournamentRemoteViewsUpdated(Dictionary<string, List<TournamentLogEvent>> eventsToRemove)
        {
            if (this.OnTournamentRemoteViewsUpdated != null)
            {
                this.OnTournamentRemoteViewsUpdated(eventsToRemove);
            }
        }

        public void TrackingEvent(Service.TrackingEvent tEvent)
        {
            if (this.OnTrackingEvent != null)
            {
                this.OnTrackingEvent(tEvent);
            }
        }
    }
}

