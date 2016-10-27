namespace Service
{
    using System;

    public class RemoteNotificationPlayerData
    {
        public bool HasLoggedInSinceLastNotification;
        public string Locale;
        public RemoteNotificationProvider Provider;
        public string RegistrationId;

        public RemoteNotificationPlayerData()
        {
        }

        public RemoteNotificationPlayerData(string registrationId, string locale, RemoteNotificationProvider provider)
        {
            this.RegistrationId = registrationId;
            this.Locale = locale;
            this.Provider = provider;
        }
    }
}

