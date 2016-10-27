namespace Service
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UTNotifications;

    public class RemoteNotificationSystem : MonoBehaviour
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map9;

        public void OnDisable()
        {
            Manager.Instance.OnSendRegistrationId -= new Manager.OnSendRegistrationIdHandler(this.onReceiveNotificationRegistrationId);
            Manager.Instance.OnNotificationsReceived -= new Manager.OnNotificationsReceivedHandler(this.onReceiveNotifications);
        }

        public void OnEnable()
        {
            Manager.Instance.OnSendRegistrationId += new Manager.OnSendRegistrationIdHandler(this.onReceiveNotificationRegistrationId);
            Manager.Instance.OnNotificationsReceived += new Manager.OnNotificationsReceivedHandler(this.onReceiveNotifications);
        }

        private void onReceiveNotificationRegistrationId(string providerName, string registrationId)
        {
            Debug.Log(string.Format("UTNotification Registration Succesful, provider: {0} registration: {1}", providerName, registrationId));
            RemoteNotificationProvider provider = (RemoteNotificationProvider) ((int) Enum.Parse(typeof(RemoteNotificationProvider), providerName));
            Player player = GameLogic.Binder.GameState.Player;
            player.RemoteNotificationPlayerData.RegistrationId = registrationId;
            player.RemoteNotificationPlayerData.Locale = App.Binder.LocaSystem.DisplayLanguage;
            player.RemoteNotificationPlayerData.Provider = provider;
            player.RemoteNotificationPlayerData.HasLoggedInSinceLastNotification = true;
        }

        private void onReceiveNotifications(IList<ReceivedNotification> notifications)
        {
            IEnumerator<ReceivedNotification> enumerator = notifications.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    string str;
                    int num;
                    ReceivedNotification current = enumerator.Current;
                    current.userData.TryGetValue("nskclientaction", out str);
                    string key = str;
                    if (key == null)
                    {
                        return;
                    }
                    if (<>f__switch$map9 == null)
                    {
                        Dictionary<string, int> dictionary2 = new Dictionary<string, int>(2);
                        dictionary2.Add("updateplayer", 0);
                        dictionary2.Add("updatetournament", 1);
                        <>f__switch$map9 = dictionary2;
                    }
                    if (!<>f__switch$map9.TryGetValue(key, out num))
                    {
                        return;
                    }
                    if (num == 0)
                    {
                        Debug.Log("Received updateplayer clientaction from remote notification, flagging player for sync.");
                        Service.Binder.PlayerService.FlagForSync();
                    }
                    else
                    {
                        if (num != 1)
                        {
                            return;
                        }
                        Debug.Log("Received updatetournament clientaction from remote notification!");
                        Service.Binder.TournamentSystem.ForceUpsert();
                        continue;
                    }
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
        }
    }
}

