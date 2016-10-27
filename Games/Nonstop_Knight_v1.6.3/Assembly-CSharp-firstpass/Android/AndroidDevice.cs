namespace Android
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AndroidDevice
    {
        [CompilerGenerated]
        private AndroidDeviceClassification <Classification>k__BackingField;

        public AndroidDevice()
        {
            this.Classification = AndroidDeviceClassification.Unknown;
            try
            {
                AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject obj3 = class2.GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getApplicationContext", new object[0]);
                AndroidJavaClass class3 = new AndroidJavaClass("com.facebook.device.yearclass.YearClass");
                object[] args = new object[] { obj3 };
                int num = class3.CallStatic<int>("get", args);
                if (num >= 0x7de)
                {
                    this.Classification = AndroidDeviceClassification.HighEnd;
                }
                else if (num > 0x7dc)
                {
                    this.Classification = AndroidDeviceClassification.MidRange;
                }
                else if (num > -1)
                {
                    this.Classification = AndroidDeviceClassification.LowEnd;
                }
                Debug.Log(string.Concat(new object[] { "AndroidDeviceClassification: ", num, ", ", this.Classification }));
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                Debug.LogWarning("Couldn't determine AndroidDeviceClassification");
            }
        }

        public AndroidDeviceClassification Classification
        {
            [CompilerGenerated]
            get
            {
                return this.<Classification>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Classification>k__BackingField = value;
            }
        }
    }
}

