namespace Appboy
{
    using Appboy.Models;
    using Appboy.Models.Cards;
    using Appboy.Models.InAppMessage;
    using Appboy.Utilities;
    using System;
    using UnityEngine;

    public class AppboyBindingTester : MonoBehaviour
    {
        private void FeedReceivedCallback(string message)
        {
            Debug.Log("FeedReceivedCallback message: " + message);
            Feed feed = new Feed(message);
            Debug.Log("Feed received: " + feed);
            foreach (Card card in feed.Cards)
            {
                Debug.Log("Card: " + card);
            }
        }

        private void InAppMessageReceivedCallback(string message)
        {
            Debug.Log("InAppMessageReceivedCallback message: " + message);
            IInAppMessage message2 = InAppMessageFactory.BuildInAppMessage(message);
            Debug.Log("In-app message received: " + message2);
            if (message2 is IInAppMessageImmersive)
            {
                IInAppMessageImmersive immersive = message2 as IInAppMessageImmersive;
                if ((immersive.Buttons == null) || (immersive.Buttons.Count <= 0))
                {
                }
            }
        }

        private void PushNotificationOpenedCallback(string message)
        {
            Debug.Log("PushNotificationOpenedCallback message: " + message);
            Debug.Log("Push Notification opened: " + new PushNotification(message));
        }

        private void PushNotificationOpenedCallbackForiOS(string message)
        {
            JSONClass json = (JSONClass) JSON.Parse(message);
            Debug.Log("Push opened Notification event: " + new ApplePushNotification(json));
        }

        private void PushNotificationReceivedCallback(string message)
        {
            Debug.Log("PushNotificationReceivedCallback message: " + message);
            Debug.Log("Push Notification received: " + new PushNotification(message));
        }

        private void PushNotificationReceivedCallbackForiOS(string message)
        {
            JSONClass json = (JSONClass) JSON.Parse(message);
            Debug.Log("Push received Notification event: " + new ApplePushNotification(json));
        }

        private void Start()
        {
        }
    }
}

