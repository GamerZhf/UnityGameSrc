using MATSDK;
using System;
using UnityEngine;

public class MATSample : MonoBehaviour
{
    private string MAT_ADVERTISER_ID;
    private string MAT_CONVERSION_KEY;
    private string MAT_PACKAGE_NAME;

    private void Awake()
    {
        this.MAT_ADVERTISER_ID = "877";
        this.MAT_CONVERSION_KEY = "8c14d6bbe466b65211e781d62e301eec";
        this.MAT_PACKAGE_NAME = "com.hasoffers.unitytestapp";
        MonoBehaviour.print("Awake called: " + this.MAT_ADVERTISER_ID + ", " + this.MAT_CONVERSION_KEY);
    }

    public static string getSampleiTunesIAPReceipt()
    {
        return "dGhpcyBpcyBhIHNhbXBsZSBpb3MgYXBwIHN0b3JlIHJlY2VpcHQ=";
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        style.fontSize = 50;
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(10f, 5f, (float) (Screen.width - 20), (float) (Screen.height / 10)), "MAT Unity Test App", style);
        GUI.skin.button.fontSize = 40;
        if (GUI.Button(new Rect(10f, (float) (Screen.height / 10), (float) (Screen.width - 20), (float) (Screen.height / 10)), "Start MAT SDK"))
        {
            MonoBehaviour.print("Start MAT SDK clicked");
            MATBinding.Init(this.MAT_ADVERTISER_ID, this.MAT_CONVERSION_KEY);
            MATBinding.SetPackageName(this.MAT_PACKAGE_NAME);
            MATBinding.SetFacebookEventLogging(true, false);
            MATBinding.CheckForDeferredDeeplink();
            MATBinding.AutomateIapEventMeasurement(true);
        }
        else if (GUI.Button(new Rect(10f, (float) ((2 * Screen.height) / 10), (float) (Screen.width - 20), (float) (Screen.height / 10)), "Set Delegate"))
        {
            MonoBehaviour.print("Set Delegate clicked");
            MATBinding.SetDelegate(true);
        }
        else if (GUI.Button(new Rect(10f, (float) ((3 * Screen.height) / 10), (float) (Screen.width - 20), (float) (Screen.height / 10)), "Enable Debug Mode"))
        {
            MonoBehaviour.print("Enable Debug Mode clicked");
            MATBinding.SetDebugMode(true);
        }
        else if (GUI.Button(new Rect(10f, (float) ((4 * Screen.height) / 10), (float) (Screen.width - 20), (float) (Screen.height / 10)), "Allow Duplicates"))
        {
            MonoBehaviour.print("Allow Duplicates clicked");
            MATBinding.SetAllowDuplicates(true);
        }
        else if (GUI.Button(new Rect(10f, (float) ((5 * Screen.height) / 10), (float) (Screen.width - 20), (float) (Screen.height / 10)), "Measure Session"))
        {
            MonoBehaviour.print("Measure Session clicked");
            MATBinding.MeasureSession();
        }
        else if (GUI.Button(new Rect(10f, (float) ((6 * Screen.height) / 10), (float) (Screen.width - 20), (float) (Screen.height / 10)), "Measure Event"))
        {
            MonoBehaviour.print("Measure Event clicked");
            MATBinding.MeasureEvent("evt11");
        }
        else if (GUI.Button(new Rect(10f, (float) ((7 * Screen.height) / 10), (float) (Screen.width - 20), (float) (Screen.height / 10)), "Measure Event With Event Items"))
        {
            MonoBehaviour.print("Measure Event With Event Items clicked");
            MATItem item = new MATItem();
            item.name = "subitem1";
            item.unitPrice = 5.0;
            item.quantity = 5;
            item.revenue = 3.0;
            item.attribute2 = "attrValue2";
            item.attribute3 = "attrValue3";
            item.attribute4 = "attrValue4";
            item.attribute5 = "attrValue5";
            MATItem item2 = new MATItem();
            item2.name = "subitem2";
            item2.unitPrice = 1.0;
            item2.quantity = 3;
            item2.revenue = 1.5;
            item2.attribute1 = "attrValue1";
            item2.attribute3 = "attrValue3";
            MATItem[] itemArray = new MATItem[] { item, item2 };
            MATEvent tuneEvent = new MATEvent("purchase");
            tuneEvent.revenue = 10.0;
            tuneEvent.currencyCode = "AUD";
            tuneEvent.advertiserRefId = "ref222";
            tuneEvent.attribute1 = "test_attribute1";
            tuneEvent.attribute2 = "test_attribute2";
            tuneEvent.attribute3 = "test_attribute3";
            tuneEvent.attribute4 = "test_attribute4";
            tuneEvent.attribute5 = "test_attribute5";
            tuneEvent.contentType = "test_contentType";
            tuneEvent.contentId = "test_contentId";
            tuneEvent.date1 = new DateTime?(DateTime.UtcNow);
            DateTime time2 = new DateTime(2, 1, 1);
            tuneEvent.date2 = new DateTime?(DateTime.UtcNow.Add(new TimeSpan(time2.Ticks)));
            tuneEvent.level = 3;
            tuneEvent.quantity = 2;
            tuneEvent.rating = 4.5;
            tuneEvent.searchString = "test_searchString";
            tuneEvent.eventItems = itemArray;
            tuneEvent.transactionState = 1;
            MATBinding.MeasureEvent(tuneEvent);
        }
        else if (GUI.Button(new Rect(10f, (float) ((8 * Screen.height) / 10), (float) (Screen.width - 20), (float) (Screen.height / 10)), "Test Setter Methods"))
        {
            MonoBehaviour.print("Test Setter Methods clicked");
            MATBinding.SetAge(0x22);
            MATBinding.SetAllowDuplicates(true);
            MATBinding.SetAppAdTracking(true);
            MATBinding.SetDebugMode(true);
            MATBinding.SetExistingUser(false);
            MATBinding.SetFacebookUserId("temp_facebook_user_id");
            MATBinding.SetGender(0);
            MATBinding.SetGoogleUserId("temp_google_user_id");
            MATBinding.SetLocation(111.0, 222.0, 333.0);
            MATBinding.SetPayingUser(true);
            MATBinding.SetPhoneNumber("111-222-3333");
            MATBinding.SetTwitterUserId("twitter_user_id");
            MATBinding.SetUserId("temp_user_id");
            MATBinding.SetUserName("temp_user_name");
            MATBinding.SetUserEmail("tempuser@tempcompany.com");
            MATBinding.SetDeepLink("myapp://myval1/myval2");
            MATBinding.SetAndroidId("111111111111");
            MATBinding.SetDeviceId("123456789123456");
            MATBinding.SetGoogleAdvertisingId("12345678-1234-1234-1234-123456789012", true);
            MATBinding.SetMacAddress("AA:BB:CC:DD:EE:FF");
            MATBinding.SetCurrencyCode("CAD");
            MATBinding.SetTRUSTeId("1234567890");
            MATPreloadData preloadData = new MATPreloadData("1122334455");
            preloadData.advertiserSubAd = "some_adv_sub_ad_id";
            preloadData.publisherSub3 = "some_pub_sub3";
            MATBinding.SetPreloadedApp(preloadData);
        }
        else if (GUI.Button(new Rect(10f, (float) ((9 * Screen.height) / 10), (float) (Screen.width - 20), (float) (Screen.height / 10)), "Test Getter Methods"))
        {
            MonoBehaviour.print("Test Getter Methods clicked");
            MonoBehaviour.print("isPayingUser = " + MATBinding.GetIsPayingUser());
            MonoBehaviour.print("tuneId     = " + MATBinding.GetTuneId());
            MonoBehaviour.print("openLogId = " + MATBinding.GetOpenLogId());
        }
    }
}

