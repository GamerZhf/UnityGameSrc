namespace App
{
    using Android;
    using System;
    using UnityEngine;

    public static class ConfigDevice
    {
        private static readonly AndroidDevice androidDevice = new AndroidDevice();

        public static DeviceQualityType DeviceQuality()
        {
            if (ConfigApp.CHEAT_SIMULATE_DEVICE_QUALITY != DeviceQualityType.Auto)
            {
                return ConfigApp.CHEAT_SIMULATE_DEVICE_QUALITY;
            }
            if (androidDevice.Classification == AndroidDeviceClassification.LowEnd)
            {
                return DeviceQualityType.Low;
            }
            return DeviceQualityType.Med;
        }

        public static string GetAssetBundlePlatformKey()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.OSXEditor:
                    return "Standalone";

                case RuntimePlatform.IPhonePlayer:
                    return "iOS";

                case RuntimePlatform.Android:
                    return "Android";
            }
            return "UNSUPPORTED";
        }

        public static bool IsAndroid()
        {
            return (Application.platform == RuntimePlatform.Android);
        }
    }
}

