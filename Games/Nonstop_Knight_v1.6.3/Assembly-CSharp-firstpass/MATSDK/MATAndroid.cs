namespace MATSDK
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class MATAndroid
    {
        private AndroidJavaObject ajcAlliances;
        private AndroidJavaObject ajcCurrentActivity;
        public AndroidJavaObject ajcInstance;
        private AndroidJavaClass ajcMAT;
        private AndroidJavaClass ajcUnityPlayer;
        private static MATAndroid instance;

        private MATAndroid()
        {
        }

        public void CacheInterstitial(string placement)
        {
            object[] args = new object[] { placement };
            this.ajcAlliances.Call("cacheInterstitial", args);
        }

        public void CacheInterstitial(string placement, MATAdMetadata metadata)
        {
            AndroidJavaObject adMetadataJavaObject = this.GetAdMetadataJavaObject(metadata);
            object[] args = new object[] { placement, adMetadataJavaObject };
            this.ajcAlliances.Call("cacheInterstitial", args);
        }

        public void CheckForDeferredDeeplink()
        {
            AndroidJavaObject obj2 = new AndroidJavaObject("com.tune.unityutils.TUNEUnityDeeplinkListener", new object[0]);
            object[] args = new object[] { obj2 };
            this.ajcInstance.Call("checkForDeferredDeeplink", args);
        }

        public void DestroyBanner()
        {
            this.ajcAlliances.Call("destroyBanner", new object[0]);
        }

        public void DestroyInterstitial()
        {
            this.ajcAlliances.Call("destroyInterstitial", new object[0]);
        }

        private AndroidJavaObject GetAdMetadataJavaObject(MATAdMetadata metadata)
        {
            AndroidJavaObject @static;
            AndroidJavaObject obj2 = new AndroidJavaObject("com.tune.crosspromo.TuneAdMetadata", new object[0]);
            object[] args = new object[] { metadata.getDebugMode() };
            obj2 = obj2.Call<AndroidJavaObject>("withDebugMode", args);
            if (metadata.getGender() == MATAdGender.MALE)
            {
                @static = new AndroidJavaClass("com.mobileapptracker.MATGender").GetStatic<AndroidJavaObject>("MALE");
            }
            else if (metadata.getGender() == MATAdGender.FEMALE)
            {
                @static = new AndroidJavaClass("com.mobileapptracker.MATGender").GetStatic<AndroidJavaObject>("FEMALE");
            }
            else
            {
                @static = new AndroidJavaClass("com.mobileapptracker.MATGender").GetStatic<AndroidJavaObject>("UNKNOWN");
            }
            object[] objArray2 = new object[] { @static };
            obj2 = obj2.Call<AndroidJavaObject>("withGender", objArray2);
            if ((metadata.getLatitude() != 0.0) && (metadata.getLongitude() != 0.0))
            {
                double num = Convert.ToDouble(metadata.getLatitude());
                double num2 = Convert.ToDouble(metadata.getLongitude());
                object[] objArray3 = new object[] { num, num2 };
                obj2 = obj2.Call<AndroidJavaObject>("withLocation", objArray3);
            }
            if (metadata.getBirthDate().HasValue)
            {
                DateTime valueOrDefault = metadata.getBirthDate().GetValueOrDefault();
                int year = valueOrDefault.Year;
                int month = valueOrDefault.Month;
                int day = valueOrDefault.Day;
                object[] objArray4 = new object[] { year, month, day };
                obj2 = obj2.Call<AndroidJavaObject>("withBirthDate", objArray4);
            }
            if (metadata.getCustomTargets().Count != 0)
            {
                AndroidJavaObject obj4 = new AndroidJavaObject("java.util.HashMap", new object[0]);
                IntPtr methodID = AndroidJNIHelper.GetMethodID(obj4.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
                object[] objArray = new object[2];
                foreach (KeyValuePair<string, string> pair in metadata.getCustomTargets())
                {
                    object[] objArray5 = new object[] { pair.Key + string.Empty };
                    AndroidJavaObject obj5 = new AndroidJavaObject("java.lang.String", objArray5);
                    object[] objArray6 = new object[] { pair.Value + string.Empty };
                    AndroidJavaObject obj6 = new AndroidJavaObject("java.lang.String", objArray6);
                    objArray[0] = obj5;
                    objArray[1] = obj6;
                    AndroidJNI.CallObjectMethod(obj4.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(objArray));
                }
                object[] objArray7 = new object[] { obj4 };
                obj2 = obj2.Call<AndroidJavaObject>("withCustomTargets", objArray7);
            }
            if (metadata.getKeywords().Count == 0)
            {
                return obj2;
            }
            AndroidJavaObject obj7 = new AndroidJavaObject("java.util.HashSet", new object[0]);
            foreach (string str in metadata.getKeywords())
            {
                object[] objArray8 = new object[] { str };
                obj7.Call<bool>("add", objArray8);
            }
            object[] objArray9 = new object[] { obj7 };
            return obj2.Call<AndroidJavaObject>("withKeywords", objArray9);
        }

        public bool GetIsPayingUser()
        {
            return this.ajcInstance.Call<bool>("getIsPayingUser", new object[0]);
        }

        public string GetMatId()
        {
            return this.ajcInstance.Call<string>("getMatId", new object[0]);
        }

        public string GetOpenLogId()
        {
            return this.ajcInstance.Call<string>("getOpenLogId", new object[0]);
        }

        private AndroidJavaObject GetTuneEventJavaObject(MATEvent tuneEvent)
        {
            AndroidJavaObject obj2;
            if (tuneEvent.name == null)
            {
                object[] args = new object[] { tuneEvent.id };
                obj2 = new AndroidJavaObject("com.mobileapptracker.MATEvent", args);
            }
            else
            {
                object[] objArray2 = new object[] { tuneEvent.name };
                obj2 = new AndroidJavaObject("com.mobileapptracker.MATEvent", objArray2);
            }
            if (tuneEvent.revenue.HasValue)
            {
                object[] objArray3 = new object[] { tuneEvent.revenue };
                obj2 = obj2.Call<AndroidJavaObject>("withRevenue", objArray3);
            }
            if (tuneEvent.currencyCode != null)
            {
                object[] objArray4 = new object[] { tuneEvent.currencyCode };
                obj2 = obj2.Call<AndroidJavaObject>("withCurrencyCode", objArray4);
            }
            if (tuneEvent.advertiserRefId != null)
            {
                object[] objArray5 = new object[] { tuneEvent.advertiserRefId };
                obj2 = obj2.Call<AndroidJavaObject>("withAdvertiserRefId", objArray5);
            }
            if (tuneEvent.eventItems != null)
            {
                AndroidJavaObject obj3 = new AndroidJavaObject("java.util.ArrayList", new object[0]);
                foreach (MATItem item in tuneEvent.eventItems)
                {
                    object[] objArray6 = new object[] { item.name };
                    AndroidJavaObject obj4 = new AndroidJavaObject("com.mobileapptracker.MATEventItem", objArray6);
                    if (item.quantity.HasValue)
                    {
                        object[] objArray7 = new object[] { item.quantity };
                        obj4 = obj4.Call<AndroidJavaObject>("withQuantity", objArray7);
                    }
                    if (item.unitPrice.HasValue)
                    {
                        object[] objArray8 = new object[] { item.unitPrice };
                        obj4 = obj4.Call<AndroidJavaObject>("withUnitPrice", objArray8);
                    }
                    if (item.revenue.HasValue)
                    {
                        object[] objArray9 = new object[] { item.revenue };
                        obj4 = obj4.Call<AndroidJavaObject>("withRevenue", objArray9);
                    }
                    if (item.attribute1 != null)
                    {
                        object[] objArray10 = new object[] { item.attribute1 };
                        obj4 = obj4.Call<AndroidJavaObject>("withAttribute1", objArray10);
                    }
                    if (item.attribute2 != null)
                    {
                        object[] objArray11 = new object[] { item.attribute2 };
                        obj4 = obj4.Call<AndroidJavaObject>("withAttribute2", objArray11);
                    }
                    if (item.attribute3 != null)
                    {
                        object[] objArray12 = new object[] { item.attribute3 };
                        obj4 = obj4.Call<AndroidJavaObject>("withAttribute3", objArray12);
                    }
                    if (item.attribute4 != null)
                    {
                        object[] objArray13 = new object[] { item.attribute4 };
                        obj4 = obj4.Call<AndroidJavaObject>("withAttribute4", objArray13);
                    }
                    if (item.attribute5 != null)
                    {
                        object[] objArray14 = new object[] { item.attribute5 };
                        obj4 = obj4.Call<AndroidJavaObject>("withAttribute5", objArray14);
                    }
                    object[] objArray15 = new object[] { obj4 };
                    obj3.Call<bool>("add", objArray15);
                }
                object[] objArray16 = new object[] { obj3 };
                obj2 = obj2.Call<AndroidJavaObject>("withEventItems", objArray16);
            }
            if ((tuneEvent.receipt != null) && (tuneEvent.receiptSignature != null))
            {
                object[] objArray17 = new object[] { tuneEvent.receipt, tuneEvent.receiptSignature };
                obj2 = obj2.Call<AndroidJavaObject>("withReceipt", objArray17);
            }
            if (tuneEvent.contentType != null)
            {
                object[] objArray18 = new object[] { tuneEvent.contentType };
                obj2 = obj2.Call<AndroidJavaObject>("withContentType", objArray18);
            }
            if (tuneEvent.contentId != null)
            {
                object[] objArray19 = new object[] { tuneEvent.contentId };
                obj2 = obj2.Call<AndroidJavaObject>("withContentId", objArray19);
            }
            if (tuneEvent.level.HasValue)
            {
                object[] objArray20 = new object[] { tuneEvent.level };
                obj2 = obj2.Call<AndroidJavaObject>("withLevel", objArray20);
            }
            if (tuneEvent.quantity.HasValue)
            {
                object[] objArray21 = new object[] { tuneEvent.quantity };
                obj2 = obj2.Call<AndroidJavaObject>("withQuantity", objArray21);
            }
            if (tuneEvent.searchString != null)
            {
                object[] objArray22 = new object[] { tuneEvent.searchString };
                obj2 = obj2.Call<AndroidJavaObject>("withSearchString", objArray22);
            }
            if (tuneEvent.date1.HasValue)
            {
                TimeSpan span = new TimeSpan(tuneEvent.date1.Value.Ticks);
                double totalMilliseconds = span.TotalMilliseconds;
                DateTime time = new DateTime(0x7b2, 1, 1);
                TimeSpan span2 = new TimeSpan(time.Ticks);
                double num3 = totalMilliseconds - span2.TotalMilliseconds;
                object[] objArray23 = new object[] { num3 };
                long num4 = new AndroidJavaObject("java.lang.Double", objArray23).Call<long>("longValue", new object[0]);
                object[] objArray24 = new object[] { num4 };
                AndroidJavaObject obj6 = new AndroidJavaObject("java.util.Date", objArray24);
                object[] objArray25 = new object[] { obj6 };
                obj2 = obj2.Call<AndroidJavaObject>("withDate1", objArray25);
            }
            if (tuneEvent.date2.HasValue)
            {
                TimeSpan span3 = new TimeSpan(tuneEvent.date2.Value.Ticks);
                double num5 = span3.TotalMilliseconds;
                DateTime time2 = new DateTime(0x7b2, 1, 1);
                TimeSpan span4 = new TimeSpan(time2.Ticks);
                double num6 = num5 - span4.TotalMilliseconds;
                object[] objArray26 = new object[] { num6 };
                long num7 = new AndroidJavaObject("java.lang.Double", objArray26).Call<long>("longValue", new object[0]);
                object[] objArray27 = new object[] { num7 };
                AndroidJavaObject obj8 = new AndroidJavaObject("java.util.Date", objArray27);
                object[] objArray28 = new object[] { obj8 };
                obj2 = obj2.Call<AndroidJavaObject>("withDate2", objArray28);
            }
            if (tuneEvent.attribute1 != null)
            {
                object[] objArray29 = new object[] { tuneEvent.attribute1 };
                obj2 = obj2.Call<AndroidJavaObject>("withAttribute1", objArray29);
            }
            if (tuneEvent.attribute2 != null)
            {
                object[] objArray30 = new object[] { tuneEvent.attribute2 };
                obj2 = obj2.Call<AndroidJavaObject>("withAttribute2", objArray30);
            }
            if (tuneEvent.attribute3 != null)
            {
                object[] objArray31 = new object[] { tuneEvent.attribute3 };
                obj2 = obj2.Call<AndroidJavaObject>("withAttribute3", objArray31);
            }
            if (tuneEvent.attribute4 != null)
            {
                object[] objArray32 = new object[] { tuneEvent.attribute4 };
                obj2 = obj2.Call<AndroidJavaObject>("withAttribute4", objArray32);
            }
            if (tuneEvent.attribute5 != null)
            {
                object[] objArray33 = new object[] { tuneEvent.attribute5 };
                obj2 = obj2.Call<AndroidJavaObject>("withAttribute5", objArray33);
            }
            return obj2;
        }

        public void HideBanner()
        {
            this.ajcAlliances.Call("hideBanner", new object[0]);
        }

        public void Init(string advertiserId, string conversionKey)
        {
            if (this.ajcMAT == null)
            {
                this.ajcMAT = new AndroidJavaClass("com.mobileapptracker.MobileAppTracker");
                this.ajcUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                this.ajcCurrentActivity = this.ajcUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                object[] args = new object[] { this.ajcCurrentActivity, advertiserId, conversionKey };
                this.ajcInstance = this.ajcMAT.CallStatic<AndroidJavaObject>("init", args);
                object[] objArray2 = new object[] { "unity" };
                this.ajcInstance.Call("setPluginName", objArray2);
                this.ajcAlliances = new AndroidJavaObject("com.tune.unityutils.CrossPromoUnityPlugin", new object[0]);
            }
        }

        public void MeasureEvent(MATEvent tuneEvent)
        {
            AndroidJavaObject tuneEventJavaObject = this.GetTuneEventJavaObject(tuneEvent);
            object[] args = new object[] { tuneEventJavaObject };
            this.ajcInstance.Call("measureEvent", args);
        }

        public void MeasureEvent(int eventId)
        {
            object[] args = new object[] { eventId };
            this.ajcInstance.Call("measureEvent", args);
        }

        public void MeasureEvent(string eventName)
        {
            object[] args = new object[] { eventName };
            this.ajcInstance.Call("measureEvent", args);
        }

        public void MeasureSession()
        {
            object[] args = new object[] { this.ajcCurrentActivity };
            this.ajcInstance.Call("setReferralSources", args);
            this.ajcInstance.Call("measureSession", new object[0]);
        }

        public void SetAge(int age)
        {
            object[] args = new object[] { age };
            this.ajcInstance.Call("setAge", args);
        }

        public void SetAllowDuplicates(bool allow)
        {
            object[] args = new object[] { allow };
            this.ajcInstance.Call("setAllowDuplicates", args);
        }

        public void SetAndroidId(string androidId)
        {
            object[] args = new object[] { androidId };
            this.ajcInstance.Call("setAndroidId", args);
        }

        public void SetAndroidIdMd5(string androidIdMd5)
        {
            object[] args = new object[] { androidIdMd5 };
            this.ajcInstance.Call("setAndroidIdMd5", args);
        }

        public void SetAndroidIdSha1(string androidIdSha1)
        {
            object[] args = new object[] { androidIdSha1 };
            this.ajcInstance.Call("setAndroidIdSha1", args);
        }

        public void SetAndroidIdSha256(string androidIdSha256)
        {
            object[] args = new object[] { androidIdSha256 };
            this.ajcInstance.Call("setAndroidIdSha256", args);
        }

        public void SetAppAdTracking(bool adTrackingEnabled)
        {
            object[] args = new object[] { adTrackingEnabled };
            this.ajcInstance.Call("setAppAdTrackingEnabled", args);
        }

        public void SetCurrencyCode(string currencyCode)
        {
            object[] args = new object[] { currencyCode };
            this.ajcInstance.Call("setCurrencyCode", args);
        }

        public void SetDebugMode(bool debugMode)
        {
            object[] args = new object[] { debugMode };
            this.ajcInstance.Call("setDebugMode", args);
        }

        public void SetDeepLink(string deepLinkUrl)
        {
            object[] args = new object[] { deepLinkUrl };
            this.ajcInstance.Call("setReferralUrl", args);
        }

        public void SetDelegate(bool enable)
        {
            if (enable)
            {
                AndroidJavaObject obj2 = new AndroidJavaObject("com.tune.unityutils.TUNEUnityListener", new object[0]);
                object[] args = new object[] { obj2 };
                this.ajcInstance.Call("setMATResponse", args);
            }
        }

        public void SetDeviceId(string deviceId)
        {
            object[] args = new object[] { deviceId };
            this.ajcInstance.Call("setDeviceId", args);
        }

        public void SetEmailCollection(bool collectEmail)
        {
            object[] args = new object[] { collectEmail };
            this.ajcInstance.Call("setEmailCollection", args);
        }

        public void SetExistingUser(bool isExistingUser)
        {
            object[] args = new object[] { isExistingUser };
            this.ajcInstance.Call("setExistingUser", args);
        }

        public void SetFacebookEventLogging(bool fbEventLogging, bool limitEventAndDataUsage)
        {
            object[] args = new object[] { fbEventLogging, this.ajcCurrentActivity, limitEventAndDataUsage };
            this.ajcInstance.Call("setFacebookEventLogging", args);
        }

        public void SetFacebookUserId(string facebookUserId)
        {
            object[] args = new object[] { facebookUserId };
            this.ajcInstance.Call("setFacebookUserId", args);
        }

        public void SetGender(int gender)
        {
            object[] args = new object[] { gender };
            this.ajcInstance.Call("setGender", args);
        }

        public void SetGoogleAdvertisingId(string googleAid, bool isLATEnabled)
        {
            object[] args = new object[] { googleAid, isLATEnabled };
            this.ajcInstance.Call("setGoogleAdvertisingId", args);
        }

        public void SetGoogleUserId(string googleUserId)
        {
            object[] args = new object[] { googleUserId };
            this.ajcInstance.Call("setGoogleUserId", args);
        }

        public void SetLocation(double latitude, double longitude, double altitude)
        {
            object[] args = new object[] { latitude };
            this.ajcInstance.Call("setLatitude", args);
            object[] objArray2 = new object[] { longitude };
            this.ajcInstance.Call("setLongitude", objArray2);
            object[] objArray3 = new object[] { altitude };
            this.ajcInstance.Call("setAltitude", objArray3);
        }

        public void SetMacAddress(string macAddress)
        {
            object[] args = new object[] { macAddress };
            this.ajcInstance.Call("setMacAddress", args);
        }

        public void SetPackageName(string packageName)
        {
            object[] args = new object[] { packageName };
            this.ajcInstance.Call("setPackageName", args);
        }

        public void SetPayingUser(bool isPayingUser)
        {
            object[] args = new object[] { isPayingUser };
            this.ajcInstance.Call("setIsPayingUser", args);
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            object[] args = new object[] { phoneNumber };
            this.ajcInstance.Call("setPhoneNumber", args);
        }

        public void SetPreloadedApp(MATPreloadData preloadData)
        {
            object[] args = new object[] { preloadData.publisherId };
            AndroidJavaObject obj2 = new AndroidJavaObject("com.mobileapptracker.MATPreloadData", args);
            if (preloadData.offerId != null)
            {
                object[] objArray2 = new object[] { preloadData.offerId };
                obj2 = obj2.Call<AndroidJavaObject>("withOfferId", objArray2);
            }
            if (preloadData.agencyId != null)
            {
                object[] objArray3 = new object[] { preloadData.agencyId };
                obj2 = obj2.Call<AndroidJavaObject>("withAgencyId", objArray3);
            }
            if (preloadData.publisherReferenceId != null)
            {
                object[] objArray4 = new object[] { preloadData.publisherReferenceId };
                obj2 = obj2.Call<AndroidJavaObject>("withPublisherReferenceId", objArray4);
            }
            if (preloadData.publisherSub1 != null)
            {
                object[] objArray5 = new object[] { preloadData.publisherSub1 };
                obj2 = obj2.Call<AndroidJavaObject>("withPublisherSub1", objArray5);
            }
            if (preloadData.publisherSub2 != null)
            {
                object[] objArray6 = new object[] { preloadData.publisherSub2 };
                obj2 = obj2.Call<AndroidJavaObject>("withPublisherSub2", objArray6);
            }
            if (preloadData.publisherSub3 != null)
            {
                object[] objArray7 = new object[] { preloadData.publisherSub3 };
                obj2 = obj2.Call<AndroidJavaObject>("withPublisherSub3", objArray7);
            }
            if (preloadData.publisherSub4 != null)
            {
                object[] objArray8 = new object[] { preloadData.publisherSub4 };
                obj2 = obj2.Call<AndroidJavaObject>("withPublisherSub4", objArray8);
            }
            if (preloadData.publisherSub5 != null)
            {
                object[] objArray9 = new object[] { preloadData.publisherSub5 };
                obj2 = obj2.Call<AndroidJavaObject>("withPublisherSub5", objArray9);
            }
            if (preloadData.publisherSubAd != null)
            {
                object[] objArray10 = new object[] { preloadData.publisherSubAd };
                obj2 = obj2.Call<AndroidJavaObject>("withPublisherSubAd", objArray10);
            }
            if (preloadData.publisherSubAdgroup != null)
            {
                object[] objArray11 = new object[] { preloadData.publisherSubAdgroup };
                obj2 = obj2.Call<AndroidJavaObject>("withPublisherSubAdgroup", objArray11);
            }
            if (preloadData.publisherSubCampaign != null)
            {
                object[] objArray12 = new object[] { preloadData.publisherSubCampaign };
                obj2 = obj2.Call<AndroidJavaObject>("withPublisherSubCampaign", objArray12);
            }
            if (preloadData.publisherSubKeyword != null)
            {
                object[] objArray13 = new object[] { preloadData.publisherSubKeyword };
                obj2 = obj2.Call<AndroidJavaObject>("withPublisherSubKeyword", objArray13);
            }
            if (preloadData.publisherSubPublisher != null)
            {
                object[] objArray14 = new object[] { preloadData.publisherSubPublisher };
                obj2 = obj2.Call<AndroidJavaObject>("withPublisherSubPublisher", objArray14);
            }
            if (preloadData.publisherSubSite != null)
            {
                object[] objArray15 = new object[] { preloadData.publisherSubSite };
                obj2 = obj2.Call<AndroidJavaObject>("withPublisherSubSite", objArray15);
            }
            if (preloadData.advertiserSubAd != null)
            {
                object[] objArray16 = new object[] { preloadData.advertiserSubAd };
                obj2 = obj2.Call<AndroidJavaObject>("withAdvertiserSubAd", objArray16);
            }
            if (preloadData.advertiserSubAdgroup != null)
            {
                object[] objArray17 = new object[] { preloadData.advertiserSubAdgroup };
                obj2 = obj2.Call<AndroidJavaObject>("withAdvertiserSubAdgroup", objArray17);
            }
            if (preloadData.advertiserSubCampaign != null)
            {
                object[] objArray18 = new object[] { preloadData.advertiserSubCampaign };
                obj2 = obj2.Call<AndroidJavaObject>("withAdvertiserSubCampaign", objArray18);
            }
            if (preloadData.advertiserSubKeyword != null)
            {
                object[] objArray19 = new object[] { preloadData.advertiserSubKeyword };
                obj2 = obj2.Call<AndroidJavaObject>("withAdvertiserSubKeyword", objArray19);
            }
            if (preloadData.advertiserSubPublisher != null)
            {
                object[] objArray20 = new object[] { preloadData.advertiserSubPublisher };
                obj2 = obj2.Call<AndroidJavaObject>("withAdvertiserSubPublisher", objArray20);
            }
            if (preloadData.advertiserSubSite != null)
            {
                object[] objArray21 = new object[] { preloadData.advertiserSubSite };
                obj2 = obj2.Call<AndroidJavaObject>("withAdvertiserSubSite", objArray21);
            }
            object[] objArray22 = new object[] { obj2 };
            this.ajcInstance.Call("setPreloadedApp", objArray22);
        }

        public void SetSiteId(string siteId)
        {
            object[] args = new object[] { siteId };
            this.ajcInstance.Call("setSiteId", args);
        }

        public void SetTRUSTeId(string tpid)
        {
            object[] args = new object[] { tpid };
            this.ajcInstance.Call("setTRUSTeId", args);
        }

        public void SetTwitterUserId(string twitterUserId)
        {
            object[] args = new object[] { twitterUserId };
            this.ajcInstance.Call("setTwitterUserId", args);
        }

        public void SetUserEmail(string userEmail)
        {
            object[] args = new object[] { userEmail };
            this.ajcInstance.Call("setUserEmail", args);
        }

        public void SetUserId(string userId)
        {
            object[] args = new object[] { userId };
            this.ajcInstance.Call("setUserId", args);
        }

        public void SetUserName(string userName)
        {
            object[] args = new object[] { userName };
            this.ajcInstance.Call("setUserName", args);
        }

        public void ShowBanner(string placement)
        {
            object[] args = new object[] { placement };
            this.ajcAlliances.Call("showBanner", args);
        }

        public void ShowBanner(string placement, MATAdMetadata metadata)
        {
            AndroidJavaObject adMetadataJavaObject = this.GetAdMetadataJavaObject(metadata);
            object[] args = new object[] { placement, adMetadataJavaObject };
            this.ajcAlliances.Call("showBanner", args);
        }

        public void ShowBanner(string placement, MATAdMetadata metadata, MATBannerPosition position)
        {
            AndroidJavaObject @static;
            AndroidJavaObject adMetadataJavaObject = this.GetAdMetadataJavaObject(metadata);
            if (position == MATBannerPosition.TOP_CENTER)
            {
                @static = new AndroidJavaClass("com.tune.crosspromo.TuneBannerPosition").GetStatic<AndroidJavaObject>("TOP_CENTER");
            }
            else
            {
                @static = new AndroidJavaClass("com.tune.crosspromo.TuneBannerPosition").GetStatic<AndroidJavaObject>("BOTTOM_CENTER");
            }
            object[] args = new object[] { placement, adMetadataJavaObject, @static };
            this.ajcAlliances.Call("showBanner", args);
        }

        public void ShowInterstitial(string placement)
        {
            object[] args = new object[] { placement };
            this.ajcAlliances.Call("showInterstitial", args);
        }

        public void ShowInterstitial(string placement, MATAdMetadata metadata)
        {
            AndroidJavaObject adMetadataJavaObject = this.GetAdMetadataJavaObject(metadata);
            object[] args = new object[] { placement, adMetadataJavaObject };
            this.ajcAlliances.Call("showInterstitial", args);
        }

        public static MATAndroid Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MATAndroid();
                }
                return instance;
            }
        }
    }
}

