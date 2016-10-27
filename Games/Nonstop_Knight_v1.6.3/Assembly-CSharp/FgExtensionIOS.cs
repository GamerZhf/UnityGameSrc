using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class FgExtensionIOS : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern float FG_SetIconBadgeNumber(int count);
    public static void SetIconBadgeNumber(int count)
    {
    }
}

