namespace Area730.Notifications
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class DataHolder : ScriptableObject
    {
        public List<NotificationInstance> notifications = new List<NotificationInstance>();
        public string unityClass = "com.unity3d.player.UnityPlayerNativeActivity";

        public void print()
        {
            foreach (NotificationInstance instance in this.notifications)
            {
                instance.Print();
            }
        }
    }
}

