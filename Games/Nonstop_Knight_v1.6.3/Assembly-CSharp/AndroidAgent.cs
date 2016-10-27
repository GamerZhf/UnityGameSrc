using SupersonicJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AndroidAgent : SupersonicIAgent
{
    private static AndroidJavaObject _androidBridge;
    private static readonly string AndroidBridge = "com.supersonic.unity.androidbridge.AndroidBridge";
    private const string PLACEMENT_NAME = "placement_name";
    private const string REWARD_AMOUNT = "reward_amount";
    private const string REWARD_NAME = "reward_name";

    public AndroidAgent()
    {
        Debug.Log("AndroidAgent ctr");
    }

    public string getAdvertiserId()
    {
        return this.getBridge().Call<string>("getAdvertiserId", new object[0]);
    }

    private AndroidJavaObject getBridge()
    {
        if (_androidBridge == null)
        {
            using (AndroidJavaClass class2 = new AndroidJavaClass(AndroidBridge))
            {
                _androidBridge = class2.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
            }
        }
        return _androidBridge;
    }

    public void getOfferwallCredits()
    {
        this.getBridge().Call("getOfferwallCredits", new object[0]);
    }

    public SupersonicPlacement getPlacementInfo(string placementName)
    {
        object[] args = new object[] { placementName };
        string json = this.getBridge().Call<string>("getPlacementInfo", args);
        SupersonicPlacement placement = null;
        if (json != null)
        {
            Dictionary<string, object> dictionary = Json.Deserialize(json) as Dictionary<string, object>;
            string pName = dictionary["placement_name"].ToString();
            string rName = dictionary["reward_name"].ToString();
            int rAmount = Convert.ToInt32(dictionary["reward_amount"].ToString());
            placement = new SupersonicPlacement(pName, rName, rAmount);
        }
        return placement;
    }

    public void initInterstitial(string appKey, string userId)
    {
        object[] args = new object[] { appKey, userId };
        this.getBridge().Call("initInterstitial", args);
    }

    public void initOfferwall(string appKey, string userId)
    {
        object[] args = new object[] { appKey, userId };
        this.getBridge().Call("initOfferwall", args);
    }

    public void initRewardedVideo(string appKey, string userId)
    {
        object[] args = new object[] { appKey, userId };
        this.getBridge().Call("initRewardedVideo", args);
    }

    public bool isInterstitialPlacementCapped(string placementName)
    {
        object[] args = new object[] { placementName };
        return this.getBridge().Call<bool>("isInterstitialPlacementCapped", args);
    }

    public bool isInterstitialReady()
    {
        return this.getBridge().Call<bool>("isInterstitialReady", new object[0]);
    }

    public bool isOfferwallAvailable()
    {
        return this.getBridge().Call<bool>("isOfferwallAvailable", new object[0]);
    }

    public bool isRewardedVideoAvailable()
    {
        return this.getBridge().Call<bool>("isRewardedVideoAvailable", new object[0]);
    }

    public bool isRewardedVideoPlacementCapped(string placementName)
    {
        object[] args = new object[] { placementName };
        return this.getBridge().Call<bool>("isRewardedVideoPlacementCapped", args);
    }

    public void loadInterstitial()
    {
        this.getBridge().Call("loadInterstitial", new object[0]);
    }

    public void onPause()
    {
        this.getBridge().Call("onPause", new object[0]);
    }

    public void onResume()
    {
        this.getBridge().Call("onResume", new object[0]);
    }

    public void reportAppStarted()
    {
        this.getBridge().Call("reportAppStarted", new object[0]);
    }

    public void setAge(int age)
    {
        object[] args = new object[] { age };
        this.getBridge().Call("setAge", args);
    }

    public void setGender(string gender)
    {
        object[] args = new object[] { gender };
        this.getBridge().Call("setGender", args);
    }

    public void setMediationSegment(string segment)
    {
        object[] args = new object[] { segment };
        this.getBridge().Call("setMediationSegment", args);
    }

    public void shouldTrackNetworkState(bool track)
    {
        object[] args = new object[] { track };
        this.getBridge().Call("shouldTrackNetworkState", args);
    }

    public void showInterstitial()
    {
        this.getBridge().Call("showInterstitial", new object[0]);
    }

    public void showInterstitial(string placementName)
    {
        object[] args = new object[] { placementName };
        this.getBridge().Call("showInterstitial", args);
    }

    public void showOfferwall()
    {
        this.getBridge().Call("showOfferwall", new object[0]);
    }

    public void showOfferwall(string placementName)
    {
        object[] args = new object[] { placementName };
        this.getBridge().Call("showOfferwall", args);
    }

    public void showRewardedVideo()
    {
        this.getBridge().Call("showRewardedVideo", new object[0]);
    }

    public void showRewardedVideo(string placementName)
    {
        object[] args = new object[] { placementName };
        this.getBridge().Call("showRewardedVideo", args);
    }

    public void start()
    {
        Debug.Log("Android started");
        object[] args = new object[] { "Unity", Supersonic.pluginVersion(), Supersonic.unityVersion() };
        this.getBridge().Call("setPluginData", args);
        Debug.Log("Android started - ended");
    }

    public void validateIntegration()
    {
        this.getBridge().Call("validateIntegration", new object[0]);
    }
}

