namespace Service
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Events
    {
        public delegate void AppboyIngameMessageReady();

        public delegate void ContentReady();

        public delegate void IapShopInitialized();

        public delegate void IapShopProductsUpdated();

        public delegate void IapShopPurchase(PremiumProduct product);

        public delegate void IapShopStateChanged();

        public delegate void InboxCommands(List<InboxCommand> inboxCommands);

        public delegate void LocalTournamentViewsRefreshed();

        public delegate void NetworkStateChanged(bool isOnline);

        public delegate void NewContentAvailable();

        public delegate void PlayerLoggedIn();

        public delegate void PlayerRegistered();

        public delegate void PromotionAction(string name, string action);

        public delegate void PromotionsAvailable();

        public delegate void RemoteTournamentViewsUpdated(Dictionary<string, List<TournamentLogEvent>> eventsToRemove);

        public delegate void RequireClientUpdate(string storeDeeplink);

        public delegate void ServerTimeSynced();

        public delegate void TournamentCardsReceived(string tournamentId);

        public delegate void TournamentLogUpdated();

        public delegate void TrackingSendEvent(TrackingEvent tEvent);
    }
}

