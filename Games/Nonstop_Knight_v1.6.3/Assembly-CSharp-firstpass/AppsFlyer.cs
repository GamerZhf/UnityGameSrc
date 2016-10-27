using System;
using System.Collections.Generic;
using UnityEngine;

public class AppsFlyer : MonoBehaviour
{
    private static AndroidJavaClass cls_AppsFlyer = new AndroidJavaClass("com.appsflyer.AppsFlyerLib");
    private static AndroidJavaClass cls_AppsFlyerHelper = new AndroidJavaClass("com.appsflyer.AppsFlyerUnityHelper");

    public static void createValidateInAppListener(string aObject, string callbackMethod, string callbackFailedMethod)
    {
        MonoBehaviour.print("AF.cs createValidateInAppListener called");
        using (AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj2 = class2.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                object[] args = new object[] { obj2, aObject, callbackMethod, callbackFailedMethod };
                cls_AppsFlyerHelper.CallStatic("createValidateInAppListener", args);
            }
        }
    }

    public static string getAppsFlyerId()
    {
        string str;
        using (AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj2 = class2.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                object[] args = new object[] { obj2 };
                str = cls_AppsFlyer.CallStatic<string>("getAppsFlyerUID", args);
            }
        }
        return str;
    }

    public static void getConversionData()
    {
    }

    public static void handleOpenUrl(string url, string sourceApplication, string annotation)
    {
    }

    public static void loadConversionData(string callbackObject, string callbackMethod, string callbackFailedMethod)
    {
        using (AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj2 = class2.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                object[] args = new object[] { obj2, callbackObject, callbackMethod, callbackFailedMethod };
                cls_AppsFlyerHelper.CallStatic("createConversionDataListener", args);
            }
        }
    }

    public static void setAppID(string appleAppId)
    {
    }

    public static void setAppsFlyerKey(string key)
    {
        object[] args = new object[] { key };
        cls_AppsFlyer.CallStatic("setAppsFlyerKey", args);
    }

    public static void setCollectAndroidID(bool shouldCollect)
    {
        MonoBehaviour.print("AF.cs setCollectAndroidID");
        object[] args = new object[] { shouldCollect };
        cls_AppsFlyer.CallStatic("setCollectAndroidID", args);
    }

    public static void setCollectIMEI(bool shouldCollect)
    {
        object[] args = new object[] { shouldCollect };
        cls_AppsFlyer.CallStatic("setCollectIMEI", args);
    }

    public static void setCurrencyCode(string currencyCode)
    {
        object[] args = new object[] { currencyCode };
        cls_AppsFlyer.CallStatic("setCurrencyCode", args);
    }

    public static void setCustomerUserID(string customerUserID)
    {
        object[] args = new object[] { customerUserID };
        cls_AppsFlyer.CallStatic("setAppUserId", args);
    }

    public static void setIsDebug(bool isDebug)
    {
    }

    public static void setIsSandbox(bool isSandbox)
    {
    }

    public static void trackAppLaunch()
    {
        using (AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj2 = class2.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                object[] args = new object[] { obj2 };
                cls_AppsFlyer.CallStatic("sendTracking", args);
            }
        }
    }

    public static void trackEvent(string eventName, string eventValue)
    {
        using (AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj2 = class2.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                object[] args = new object[] { obj2, eventName, eventValue };
                cls_AppsFlyer.CallStatic("sendTrackingWithEvent", args);
            }
        }
    }

    public static void trackRichEvent(string eventName, Dictionary<string, string> eventValues)
    {
        using (AndroidJavaObject obj2 = new AndroidJavaObject("java.util.HashMap", new object[0]))
        {
            IntPtr methodID = AndroidJNIHelper.GetMethodID(obj2.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
            object[] args = new object[2];
            foreach (KeyValuePair<string, string> pair in eventValues)
            {
                object[] objArray1 = new object[] { pair.Key };
                using (AndroidJavaObject obj3 = new AndroidJavaObject("java.lang.String", objArray1))
                {
                    object[] objArray2 = new object[] { pair.Value };
                    using (AndroidJavaObject obj4 = new AndroidJavaObject("java.lang.String", objArray2))
                    {
                        args[0] = obj3;
                        args[1] = obj4;
                        AndroidJNI.CallObjectMethod(obj2.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(args));
                    }
                }
            }
            using (AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject obj5 = class2.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    object[] objArray3 = new object[] { obj5, eventName, obj2 };
                    cls_AppsFlyer.CallStatic("trackEvent", objArray3);
                }
            }
        }
    }

    public static void validateReceipt(string publicKey, string purchaseData, string signature)
    {
        MonoBehaviour.print("AF.cs validateReceipt pk = " + publicKey + " data = " + purchaseData + "sig = " + signature);
        using (AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject obj2 = class2.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                MonoBehaviour.print("inside cls_activity");
                object[] args = new object[] { obj2, publicKey, purchaseData, signature };
                cls_AppsFlyer.CallStatic("validateAndTrackInAppPurchase", args);
            }
        }
    }
}

