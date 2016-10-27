namespace Facebook.Unity
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    internal class ComponentFactory
    {
        private static GameObject facebookGameObject;
        public const string GameObjectName = "UnityFacebookSDKPlugin";

        public static T AddComponent<T>() where T: MonoBehaviour
        {
            return FacebookGameObject.AddComponent<T>();
        }

        public static T GetComponent<T>([Optional, DefaultParameterValue(0)] IfNotExist ifNotExist) where T: MonoBehaviour
        {
            GameObject facebookGameObject = FacebookGameObject;
            T component = facebookGameObject.GetComponent<T>();
            if ((component == null) && (ifNotExist == IfNotExist.AddNew))
            {
                component = facebookGameObject.AddComponent<T>();
            }
            return component;
        }

        private static GameObject FacebookGameObject
        {
            get
            {
                if (facebookGameObject == null)
                {
                    facebookGameObject = new GameObject("UnityFacebookSDKPlugin");
                }
                return facebookGameObject;
            }
        }

        internal enum IfNotExist
        {
            AddNew,
            ReturnNull
        }
    }
}

