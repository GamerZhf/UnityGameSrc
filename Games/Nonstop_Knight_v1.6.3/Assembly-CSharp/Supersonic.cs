using System;
using UnityEngine;

public class Supersonic : SupersonicIAgent
{
    private SupersonicIAgent _platformAgent = new AndroidAgent();
    public const string GENDER_FEMALE = "female";
    public const string GENDER_MALE = "male";
    public const string GENDER_UNKNOWN = "unknown";
    private static Supersonic mInstance;
    private const string UNITY_PLUGIN_VERSION = "6.4.17";

    private Supersonic()
    {
    }

    public string getAdvertiserId()
    {
        return this._platformAgent.getAdvertiserId();
    }

    public void getOfferwallCredits()
    {
        this._platformAgent.getOfferwallCredits();
    }

    public SupersonicPlacement getPlacementInfo(string placementName)
    {
        return this._platformAgent.getPlacementInfo(placementName);
    }

    public void initInterstitial(string appKey, string userId)
    {
        this._platformAgent.initInterstitial(appKey, userId);
    }

    public void initOfferwall(string appKey, string userId)
    {
        this._platformAgent.initOfferwall(appKey, userId);
    }

    public void initRewardedVideo(string appKey, string userId)
    {
        this._platformAgent.initRewardedVideo(appKey, userId);
    }

    public bool isInterstitialPlacementCapped(string placementName)
    {
        return this._platformAgent.isInterstitialPlacementCapped(placementName);
    }

    public bool isInterstitialReady()
    {
        return this._platformAgent.isInterstitialReady();
    }

    public bool isOfferwallAvailable()
    {
        return this._platformAgent.isOfferwallAvailable();
    }

    public bool isRewardedVideoAvailable()
    {
        return this._platformAgent.isRewardedVideoAvailable();
    }

    public bool isRewardedVideoPlacementCapped(string placementName)
    {
        return this._platformAgent.isRewardedVideoPlacementCapped(placementName);
    }

    public void loadInterstitial()
    {
        this._platformAgent.loadInterstitial();
    }

    public void onPause()
    {
        this._platformAgent.onPause();
    }

    public void onResume()
    {
        this._platformAgent.onResume();
    }

    public static string pluginVersion()
    {
        return "6.4.17";
    }

    public void reportAppStarted()
    {
        this._platformAgent.reportAppStarted();
    }

    public void setAge(int age)
    {
        this._platformAgent.setAge(age);
    }

    public void setGender(string gender)
    {
        if (gender.Equals("male"))
        {
            this._platformAgent.setGender("male");
        }
        else if (gender.Equals("female"))
        {
            this._platformAgent.setGender("female");
        }
        else if (gender.Equals("unknown"))
        {
            this._platformAgent.setGender("unknown");
        }
    }

    public void setMediationSegment(string segment)
    {
        this._platformAgent.setMediationSegment(segment);
    }

    public void shouldTrackNetworkState(bool track)
    {
        this._platformAgent.shouldTrackNetworkState(track);
    }

    public void showInterstitial()
    {
        this._platformAgent.showInterstitial();
    }

    public void showInterstitial(string placementName)
    {
        this._platformAgent.showInterstitial(placementName);
    }

    public void showOfferwall()
    {
        this._platformAgent.showOfferwall();
    }

    public void showOfferwall(string placementName)
    {
        this._platformAgent.showOfferwall(placementName);
    }

    public void showRewardedVideo()
    {
        this._platformAgent.showRewardedVideo();
    }

    public void showRewardedVideo(string placementName)
    {
        this._platformAgent.showRewardedVideo(placementName);
    }

    public void start()
    {
        this._platformAgent.start();
    }

    public static string unityVersion()
    {
        return Application.unityVersion;
    }

    public void validateIntegration()
    {
        this._platformAgent.validateIntegration();
    }

    public static Supersonic Agent
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new Supersonic();
            }
            return mInstance;
        }
    }
}

