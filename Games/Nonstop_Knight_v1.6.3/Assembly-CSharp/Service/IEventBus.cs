namespace Service
{
    using System;
    using System.Collections.Generic;

    public interface IEventBus
    {
        event Service.Events.AppboyIngameMessageReady OnAppboyIngameMessageReady;

        event Service.Events.ContentReady OnContentReady;

        event Service.Events.IapShopInitialized OnIapShopInitialized;

        event Service.Events.IapShopProductsUpdated OnIapShopProductsUpdated;

        event Service.Events.IapShopPurchase OnIapShopPurchase;

        event Service.Events.IapShopStateChanged OnIapShopStateChanged;

        event Service.Events.InboxCommands OnInboxCommands;

        event Service.Events.LocalTournamentViewsRefreshed OnLocalTournamentViewsRefreshed;

        event Service.Events.NetworkStateChanged OnNetworkStateChanged;

        event Service.Events.NewContentAvailable OnNewContentAvailable;

        event Service.Events.PlayerLoggedIn OnPlayerLoggedIn;

        event Service.Events.PlayerRegistered OnPlayerRegistered;

        event Service.Events.PromotionAction OnPromotionAction;

        event Service.Events.PromotionsAvailable OnPromotionsAvailable;

        event Service.Events.ServerTimeSynced OnServerTimeSynced;

        event Service.Events.TournamentCardsReceived OnTournamentCardsReceived;

        event Service.Events.TournamentLogUpdated OnTournamentLogUpdated;

        event Events.RemoteTournamentViewsUpdated OnTournamentRemoteViewsUpdated;

        event Events.TrackingSendEvent OnTrackingEvent;

        void AppboyIngameMessageReady();
        void ContentReady();
        void IapShopInitialized();
        void IapShopProductsUpdated();
        void IapShopPurchase(PremiumProduct product);
        void IapShopStateChanged();
        void InboxCommands(List<InboxCommand> inboxCommands);
        void LocalTournamentViewsRefreshed();
        void NetworkStateChanged(bool online);
        void NewContentAvailable();
        void PlayerLoggedIn();
        void PlayerRegistered();
        void PromotionAction(string name, string action);
        void PromotionsAvailable();
        void ServerTimeSynced();
        void TournamentCardsReceived(string tournamentId);
        void TournamentLogUpdated();
        void TournamentRemoteViewsUpdated(Dictionary<string, List<TournamentLogEvent>> eventsToRemove);
        void TrackingEvent(Service.TrackingEvent tEvent);
    }
}

