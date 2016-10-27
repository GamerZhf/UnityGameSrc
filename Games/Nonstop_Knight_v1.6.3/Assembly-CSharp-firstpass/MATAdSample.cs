using MATSDK;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MATAdSample : MonoBehaviour
{
    private int bottomMargin = 5;
    private string MAT_ADVERTISER_ID;
    private string MAT_CONVERSION_KEY;
    private string MAT_PACKAGE_NAME;
    private int numButtons;
    private Vector2 scrollPosition = Vector2.zero;
    private int topMargin = 5;

    private void Awake()
    {
        this.MAT_ADVERTISER_ID = "877";
        this.MAT_CONVERSION_KEY = "40c19f41ef0ec2d433f595f0880d39b9";
        this.MAT_PACKAGE_NAME = "edu.self.AtomicDodgeBallLite";
        MonoBehaviour.print("Awake called: " + this.MAT_ADVERTISER_ID + ", " + this.MAT_CONVERSION_KEY);
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        style.fontSize = 50;
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;
        float height = Screen.height / 10;
        GUI.Label(new Rect(10f, (float) this.topMargin, (float) (Screen.width - 20), height), "MAT Unity Test App", style);
        GUI.skin.button.fontSize = 40;
        GUI.skin.verticalScrollbar.fixedWidth = Screen.width * 0.05f;
        GUI.skin.verticalScrollbarThumb.fixedWidth = Screen.width * 0.05f;
        float num2 = ((float) (this.numButtons * 0.125)) * Screen.height;
        this.scrollPosition = GUI.BeginScrollView(new Rect(0.1f * Screen.width, 0.15f * Screen.height, Screen.width - (0.1f * Screen.width), ((Screen.height - (0.15f * Screen.height)) - this.topMargin) - this.bottomMargin), this.scrollPosition, new Rect(0.1f * Screen.width, 0.15f * Screen.height, Screen.width - (0.1f * Screen.width), num2));
        int num3 = 0;
        float top = ((float) (0.15000000596046448 + (num3 * 0.125))) * Screen.height;
        Rect position = new Rect(0.1f * Screen.width, top, 0.8f * Screen.width, 0.1f * Screen.height);
        if (GUI.Button(position, "Start MAT SDK"))
        {
            MonoBehaviour.print("Start MAT SDK clicked");
            MATBinding.Init(this.MAT_ADVERTISER_ID, this.MAT_CONVERSION_KEY);
            MATBinding.SetPackageName(this.MAT_PACKAGE_NAME);
        }
        num3++;
        top = ((float) (0.15000000596046448 + (num3 * 0.125))) * Screen.height;
        position = new Rect(0.1f * Screen.width, top, 0.8f * Screen.width, 0.1f * Screen.height);
        if (GUI.Button(position, "Set Delegate"))
        {
            MonoBehaviour.print("Set Delegate clicked");
            MATBinding.SetDelegate(true);
        }
        num3++;
        top = ((float) (0.15000000596046448 + (num3 * 0.125))) * Screen.height;
        position = new Rect(0.1f * Screen.width, top, 0.8f * Screen.width, 0.1f * Screen.height);
        if (GUI.Button(position, "Show Banner"))
        {
            MonoBehaviour.print("Show Banner");
            MATBinding.ShowBanner("place1");
        }
        num3++;
        top = ((float) (0.15000000596046448 + (num3 * 0.125))) * Screen.height;
        position = new Rect(0.1f * Screen.width, top, 0.8f * Screen.width, 0.1f * Screen.height);
        if (GUI.Button(position, "Hide Banner"))
        {
            MonoBehaviour.print("Hide Banner");
            MATBinding.HideBanner();
        }
        num3++;
        top = ((float) (0.15000000596046448 + (num3 * 0.125))) * Screen.height;
        position = new Rect(0.1f * Screen.width, top, 0.8f * Screen.width, 0.1f * Screen.height);
        if (GUI.Button(position, "Destroy Banner"))
        {
            MonoBehaviour.print("Destroy Banner");
            MATBinding.DestroyBanner();
        }
        num3++;
        top = ((float) (0.15000000596046448 + (num3 * 0.125))) * Screen.height;
        position = new Rect(0.1f * Screen.width, top, 0.8f * Screen.width, 0.1f * Screen.height);
        if (GUI.Button(position, "Cache Interstitial"))
        {
            MATAdMetadata metadata = new MATAdMetadata();
            metadata.setBirthDate(new DateTime?(DateTime.Today.AddYears(-45)));
            metadata.setGender(MATAdGender.FEMALE);
            metadata.setLocation(120.8f, 234.5f, 579.6f);
            metadata.setDebugMode(true);
            HashSet<string> keywords = new HashSet<string>();
            keywords.Add("pro");
            keywords.Add("evening");
            metadata.setKeywords(keywords);
            Dictionary<string, string> customTargets = new Dictionary<string, string>();
            customTargets.Add("type", "game");
            customTargets.Add("subtype1", "adventure");
            customTargets.Add("subtype2", "action");
            metadata.setCustomTargets(customTargets);
            MATBinding.CacheInterstitial("place1", metadata);
        }
        num3++;
        top = ((float) (0.15000000596046448 + (num3 * 0.125))) * Screen.height;
        position = new Rect(0.1f * Screen.width, top, 0.8f * Screen.width, 0.1f * Screen.height);
        if (GUI.Button(position, "Show Interstitial"))
        {
            MonoBehaviour.print("Show Interstitial");
            MATBinding.ShowInterstitial("place1");
        }
        num3++;
        top = ((float) (0.15000000596046448 + (num3 * 0.125))) * Screen.height;
        position = new Rect(0.1f * Screen.width, top, 0.8f * Screen.width, 0.1f * Screen.height);
        if (GUI.Button(position, "Destroy Interstitial"))
        {
            MonoBehaviour.print("Destroy Interstitial");
            MATBinding.DestroyInterstitial();
        }
        GUI.EndScrollView();
        this.numButtons = num3 + 1;
        MATBinding.LayoutBanner();
    }
}

