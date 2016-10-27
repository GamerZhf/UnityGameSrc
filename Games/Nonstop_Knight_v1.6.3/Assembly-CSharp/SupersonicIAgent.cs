using System;

public interface SupersonicIAgent
{
    string getAdvertiserId();
    void getOfferwallCredits();
    SupersonicPlacement getPlacementInfo(string name);
    void initInterstitial(string appKey, string userId);
    void initOfferwall(string appKey, string userId);
    void initRewardedVideo(string appKey, string userId);
    bool isInterstitialPlacementCapped(string placementName);
    bool isInterstitialReady();
    bool isOfferwallAvailable();
    bool isRewardedVideoAvailable();
    bool isRewardedVideoPlacementCapped(string placementName);
    void loadInterstitial();
    void onPause();
    void onResume();
    void reportAppStarted();
    void setAge(int age);
    void setGender(string gender);
    void setMediationSegment(string segment);
    void shouldTrackNetworkState(bool track);
    void showInterstitial();
    void showInterstitial(string placementName);
    void showOfferwall();
    void showOfferwall(string placementName);
    void showRewardedVideo();
    void showRewardedVideo(string placementName);
    void start();
    void validateIntegration();
}

