namespace Appboy
{
    using Appboy.Models;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class AppboyBinding : MonoBehaviour
    {
        private static AndroidJavaObject appboy;
        private static AndroidJavaObject appboyUnityActivity;
        public const string Version = "1.2.1";

        public static void AddToCustomUserAttributeArray(string key, string value)
        {
            object[] args = new object[] { key, value };
            GetCurrentUser().Call<bool>("addToCustomAttributeArray", args);
        }

        public static void ChangeUser(string userId)
        {
            object[] args = new object[] { userId };
            Appboy.Call<AndroidJavaObject>("changeUser", args);
        }

        public static void ClearPushMessage(int notificationId)
        {
            object[] args = new object[] { notificationId };
            AppboyUnityActivity.Call("clearNotification", args);
        }

        private static AndroidJavaObject GetCurrentUser()
        {
            return Appboy.Call<AndroidJavaObject>("getCurrentUser", new object[0]);
        }

        public static void IncrementCustomUserAttribute(string key)
        {
            IncrementCustomUserAttribute(key, 1);
        }

        public static void IncrementCustomUserAttribute(string key, int incrementValue)
        {
            object[] args = new object[] { key, incrementValue };
            GetCurrentUser().Call<bool>("incrementCustomUserAttribute", args);
        }

        public static void LogCustomEvent(string eventName)
        {
            object[] args = new object[] { eventName };
            Appboy.Call<bool>("logCustomEvent", args);
        }

        public static void LogFeedbackDisplayed()
        {
            Appboy.Call<bool>("logFeedbackDisplayed", new object[0]);
        }

        public static void LogFeedDisplayed()
        {
            Appboy.Call<bool>("logFeedDisplayed", new object[0]);
        }

        public static void LogInAppMessageButtonClicked(string inAppMessageJSONString, int buttonID)
        {
            object[] args = new object[] { inAppMessageJSONString, buttonID };
            AppboyUnityActivity.Call("logInAppMessageButtonClick", args);
        }

        public static void LogInAppMessageClicked(string inAppMessageJSONString)
        {
            object[] args = new object[] { inAppMessageJSONString };
            AppboyUnityActivity.Call("logInAppMessageClick", args);
        }

        public static void LogInAppMessageImpression(string inAppMessageJSONString)
        {
            object[] args = new object[] { inAppMessageJSONString };
            AppboyUnityActivity.Call("logInAppMessageImpression", args);
        }

        public static void LogPurchase(string productId, string currencyCode, decimal price)
        {
            LogPurchase(productId, currencyCode, price, 1);
        }

        public static void LogPurchase(string productId, string currencyCode, decimal price, int quantity)
        {
            object[] args = new object[] { price.ToString() };
            AndroidJavaObject obj2 = new AndroidJavaObject("java.math.BigDecimal", args);
            object[] objArray2 = new object[] { productId, currencyCode, obj2, quantity };
            Appboy.Call<bool>("logPurchase", objArray2);
        }

        [Obsolete("LogSlideupClicked is deprecated, please use LogInAppMessageClicked instead.")]
        public static void LogSlideupClicked(string slideupJSONString)
        {
            object[] args = new object[] { slideupJSONString };
            AppboyUnityActivity.Call("logInAppMessageClick", args);
        }

        [Obsolete("LogSlideupImpression is deprecated, please use LogInAppMessageImpression instead.")]
        public static void LogSlideupImpression(string slideupJSONString)
        {
            object[] args = new object[] { slideupJSONString };
            AppboyUnityActivity.Call("logInAppMessageImpression", args);
        }

        public static void RemoveFromCustomUserAttributeArray(string key, string value)
        {
            object[] args = new object[] { key, value };
            GetCurrentUser().Call<bool>("removeFromCustomAttributeArray", args);
        }

        public static void RequestFeedRefresh()
        {
            Appboy.Call("requestFeedRefresh", new object[0]);
        }

        public static void RequestFeedRefreshFromCache()
        {
            Appboy.Call("requestFeedRefreshFromCache", new object[0]);
        }

        public static void RequestInAppMessage()
        {
            Appboy.Call("requestInAppMessageRefresh", new object[0]);
        }

        public static void RequestSlideup()
        {
            Appboy.Call("requestInAppMessageRefresh", new object[0]);
        }

        public static void SetCustomUserAttribute(string key, bool value)
        {
            object[] args = new object[] { key, value };
            GetCurrentUser().Call<bool>("setCustomUserAttribute", args);
        }

        public static void SetCustomUserAttribute(string key, int value)
        {
            object[] args = new object[] { key, value };
            GetCurrentUser().Call<bool>("setCustomUserAttribute", args);
        }

        public static void SetCustomUserAttribute(string key, float value)
        {
            object[] args = new object[] { key, value };
            GetCurrentUser().Call<bool>("setCustomUserAttribute", args);
        }

        public static void SetCustomUserAttribute(string key, string value)
        {
            object[] args = new object[] { key, value };
            GetCurrentUser().Call<bool>("setCustomUserAttribute", args);
        }

        public static void SetCustomUserAttributeArray(string key, List<string> array, int size)
        {
            if (array == null)
            {
                object[] args = new object[2];
                args[0] = key;
                GetCurrentUser().Call<bool>("setCustomAttributeArray", args);
            }
            else
            {
                object[] objArray2 = new object[] { key, array.ToArray() };
                GetCurrentUser().Call<bool>("setCustomAttributeArray", objArray2);
            }
        }

        public static void SetCustomUserAttributeToNow(string key)
        {
            object[] args = new object[] { key };
            GetCurrentUser().Call<bool>("setCustomUserAttributeToNow", args);
        }

        public static void SetCustomUserAttributeToSecondsFromEpoch(string key, long secondsFromEpoch)
        {
            object[] args = new object[] { key, secondsFromEpoch };
            GetCurrentUser().Call<bool>("setCustomUserAttributeToSecondsFromEpoch", args);
        }

        public static void SetUserAvatarImageURL(string imageURL)
        {
            object[] args = new object[] { imageURL };
            GetCurrentUser().Call<bool>("setAvatarImageUrl", args);
        }

        public static void SetUserBio(string bio)
        {
            object[] args = new object[] { bio };
            GetCurrentUser().Call<bool>("setBio", args);
        }

        public static void SetUserCountry(string country)
        {
            object[] args = new object[] { country };
            GetCurrentUser().Call<bool>("setCountry", args);
        }

        public static void SetUserDateOfBirth(int year, int month, int day)
        {
            using (AndroidJavaClass class2 = new AndroidJavaClass("com.appboy.enums.Month"))
            {
                AndroidJavaObject @static;
                switch (month)
                {
                    case 1:
                        @static = class2.GetStatic<AndroidJavaObject>("JANUARY");
                        break;

                    case 2:
                        @static = class2.GetStatic<AndroidJavaObject>("FEBRUARY");
                        break;

                    case 3:
                        @static = class2.GetStatic<AndroidJavaObject>("MARCH");
                        break;

                    case 4:
                        @static = class2.GetStatic<AndroidJavaObject>("APRIL");
                        break;

                    case 5:
                        @static = class2.GetStatic<AndroidJavaObject>("MAY");
                        break;

                    case 6:
                        @static = class2.GetStatic<AndroidJavaObject>("JUNE");
                        break;

                    case 7:
                        @static = class2.GetStatic<AndroidJavaObject>("JULY");
                        break;

                    case 8:
                        @static = class2.GetStatic<AndroidJavaObject>("AUGUST");
                        break;

                    case 9:
                        @static = class2.GetStatic<AndroidJavaObject>("SEPTEMBER");
                        break;

                    case 10:
                        @static = class2.GetStatic<AndroidJavaObject>("OCTOBER");
                        break;

                    case 11:
                        @static = class2.GetStatic<AndroidJavaObject>("NOVEMBER");
                        break;

                    case 12:
                        @static = class2.GetStatic<AndroidJavaObject>("DECEMBER");
                        break;

                    default:
                        Debug.Log("Month must be in range from 1-12");
                        return;
                }
                object[] args = new object[] { year, @static, day };
                GetCurrentUser().Call<bool>("setDateOfBirth", args);
            }
        }

        public static void SetUserEmail(string email)
        {
            object[] args = new object[] { email };
            GetCurrentUser().Call<bool>("setEmail", args);
        }

        public static void SetUserEmailNotificationSubscriptionType(AppboyNotificationSubscriptionType emailNotificationSubscriptionType)
        {
            using (AndroidJavaClass class2 = new AndroidJavaClass("com.appboy.enums.NotificationSubscriptionType"))
            {
                switch (emailNotificationSubscriptionType)
                {
                    case AppboyNotificationSubscriptionType.OPTED_IN:
                    {
                        object[] args = new object[] { class2.GetStatic<AndroidJavaObject>("OPTED_IN") };
                        GetCurrentUser().Call<bool>("setEmailNotificationSubscriptionType", args);
                        return;
                    }
                    case AppboyNotificationSubscriptionType.SUBSCRIBED:
                    {
                        object[] objArray2 = new object[] { class2.GetStatic<AndroidJavaObject>("SUBSCRIBED") };
                        GetCurrentUser().Call<bool>("setEmailNotificationSubscriptionType", objArray2);
                        return;
                    }
                    case AppboyNotificationSubscriptionType.UNSUBSCRIBED:
                    {
                        object[] objArray3 = new object[] { class2.GetStatic<AndroidJavaObject>("UNSUBSCRIBED") };
                        GetCurrentUser().Call<bool>("setEmailNotificationSubscriptionType", objArray3);
                        return;
                    }
                }
                Debug.Log("Unknown notification subscription type received: " + emailNotificationSubscriptionType);
            }
        }

        public static void SetUserFirstName(string firstName)
        {
            object[] args = new object[] { firstName };
            GetCurrentUser().Call<bool>("setFirstName", args);
        }

        public static void SetUserGender(Gender gender)
        {
            using (AndroidJavaClass class2 = new AndroidJavaClass("com.appboy.enums.Gender"))
            {
                object[] objArray2;
                Gender gender2 = gender;
                if (gender2 != Gender.Male)
                {
                    if (gender2 == Gender.Female)
                    {
                        goto Label_0048;
                    }
                    goto Label_0071;
                }
                object[] args = new object[] { class2.GetStatic<AndroidJavaObject>("MALE") };
                GetCurrentUser().Call<bool>("setGender", args);
                return;
            Label_0048:
                objArray2 = new object[] { class2.GetStatic<AndroidJavaObject>("FEMALE") };
                GetCurrentUser().Call<bool>("setGender", objArray2);
                return;
            Label_0071:
                Debug.Log("Unknown gender received: " + gender);
            }
        }

        public static void SetUserHomeCity(string city)
        {
            object[] args = new object[] { city };
            GetCurrentUser().Call<bool>("setHomeCity", args);
        }

        public static void SetUserIsSubscribedToEmails(bool isSubscribedToEmails)
        {
            object[] args = new object[] { isSubscribedToEmails };
            GetCurrentUser().Call<bool>("setIsSubscribedToEmails", args);
        }

        public static void SetUserLastName(string lastName)
        {
            object[] args = new object[] { lastName };
            GetCurrentUser().Call<bool>("setLastName", args);
        }

        public static void SetUserPhoneNumber(string phoneNumber)
        {
            object[] args = new object[] { phoneNumber };
            GetCurrentUser().Call<bool>("setPhoneNumber", args);
        }

        public static void SetUserPushNotificationSubscriptionType(AppboyNotificationSubscriptionType pushNotificationSubscriptionType)
        {
            using (AndroidJavaClass class2 = new AndroidJavaClass("com.appboy.enums.NotificationSubscriptionType"))
            {
                switch (pushNotificationSubscriptionType)
                {
                    case AppboyNotificationSubscriptionType.OPTED_IN:
                    {
                        object[] args = new object[] { class2.GetStatic<AndroidJavaObject>("OPTED_IN") };
                        GetCurrentUser().Call<bool>("setPushNotificationSubscriptionType", args);
                        return;
                    }
                    case AppboyNotificationSubscriptionType.SUBSCRIBED:
                    {
                        object[] objArray2 = new object[] { class2.GetStatic<AndroidJavaObject>("SUBSCRIBED") };
                        GetCurrentUser().Call<bool>("setPushNotificationSubscriptionType", objArray2);
                        return;
                    }
                    case AppboyNotificationSubscriptionType.UNSUBSCRIBED:
                    {
                        object[] objArray3 = new object[] { class2.GetStatic<AndroidJavaObject>("UNSUBSCRIBED") };
                        GetCurrentUser().Call<bool>("setPushNotificationSubscriptionType", objArray3);
                        return;
                    }
                }
                Debug.Log("Unknown notification subscription type received: " + pushNotificationSubscriptionType);
            }
        }

        private void Start()
        {
            Debug.Log("Starting Appboy binding for Android clients.");
        }

        public static void SubmitFeedback(string replyToEmail, string message, bool isReportingABug)
        {
            object[] args = new object[] { replyToEmail, message, isReportingABug };
            Appboy.Call<bool>("submitFeedback", args);
        }

        public static void UnsetCustomUserAttribute(string key)
        {
            object[] args = new object[] { key };
            GetCurrentUser().Call<bool>("unsetCustomUserAttribute", args);
        }

        public static AndroidJavaObject Appboy
        {
            get
            {
                if (appboy == null)
                {
                    using (AndroidJavaClass class2 = new AndroidJavaClass("com.appboy.Appboy"))
                    {
                        object[] args = new object[] { AppboyUnityActivity };
                        appboy = class2.CallStatic<AndroidJavaObject>("getInstance", args);
                    }
                }
                return appboy;
            }
        }

        public static AndroidJavaObject AppboyUnityActivity
        {
            get
            {
                if (appboyUnityActivity == null)
                {
                    using (AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        appboyUnityActivity = class2.GetStatic<AndroidJavaObject>("currentActivity");
                    }
                }
                return appboyUnityActivity;
            }
        }
    }
}

