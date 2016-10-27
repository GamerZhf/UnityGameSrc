namespace Android
{
    using System;
    using UnityEngine;

    public class AndroidApplication
    {
        public static void Suspend()
        {
            try
            {
                AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                object[] args = new object[] { true };
                class2.GetStatic<AndroidJavaObject>("currentActivity").Call<bool>("moveTaskToBack", args);
            }
            catch (Exception exception)
            {
                Debug.LogError("Couldn't suspend application, falling back to Application.Quit()");
                Debug.LogException(exception);
                Application.Quit();
            }
        }
    }
}

