namespace Service
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AppboyIOSBridge
    {
        [CompilerGenerated]
        private AppboyNativeBridge <Bridge>k__BackingField;
        private const string GO_NAME = "AppboyIOSBridge";

        public AppboyIOSBridge()
        {
            Debug.Log("----- Init Appboy native bridge -----");
            GameObject target = GameObject.Find("AppboyIOSBridge");
            if (target == null)
            {
                target = new GameObject("AppboyIOSBridge");
                target.AddComponent<AppboyNativeBridge>();
            }
            this.Bridge = target.GetComponent<AppboyNativeBridge>();
            UnityEngine.Object.DontDestroyOnLoad(target);
        }

        public AppboyNativeBridge Bridge
        {
            [CompilerGenerated]
            get
            {
                return this.<Bridge>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Bridge>k__BackingField = value;
            }
        }
    }
}

