using SupersonicJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SupersonicConfig
{
    private static AndroidJavaObject _androidBridge;
    private static readonly string AndroidBridge = "com.supersonic.unity.androidbridge.AndroidBridge";
    private static SupersonicConfig mInstance;
    private const string unsupportedPlatformStr = "Unsupported Platform";

    public SupersonicConfig()
    {
        using (AndroidJavaClass class2 = new AndroidJavaClass(AndroidBridge))
        {
            _androidBridge = class2.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
        }
    }

    public void setClientSideCallbacks(bool status)
    {
        object[] args = new object[] { status };
        _androidBridge.Call("setSupersonicClientSideCallbacks", args);
    }

    public void setItemCount(int count)
    {
        object[] args = new object[] { count };
        _androidBridge.Call("setSupersonicItemCount", args);
    }

    public void setItemName(string name)
    {
        object[] args = new object[] { name };
        _androidBridge.Call("setSupersonicItemName", args);
    }

    public void setLanguage(string language)
    {
        object[] args = new object[] { language };
        _androidBridge.Call("setSupersonicLanguage", args);
    }

    public void setMaxVideoLength(int length)
    {
        object[] args = new object[] { length };
        _androidBridge.Call("setSupersonicMaxVideoLength", args);
    }

    public void setOfferwallCustomParams(Dictionary<string, string> owCustomParams)
    {
        string str = Json.Serialize(owCustomParams);
        object[] args = new object[] { str };
        _androidBridge.Call("setSupersonicOfferwallCustomParams", args);
    }

    public void setPrivateKey(string key)
    {
        object[] args = new object[] { key };
        _androidBridge.Call("setSupersonicPrivateKey", args);
    }

    public void setRewardedVideoCustomParams(Dictionary<string, string> rvCustomParams)
    {
        string str = Json.Serialize(rvCustomParams);
        object[] args = new object[] { str };
        _androidBridge.Call("setSupersonicRewardedVideoCustomParams", args);
    }

    public static SupersonicConfig Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new SupersonicConfig();
            }
            return mInstance;
        }
    }
}

