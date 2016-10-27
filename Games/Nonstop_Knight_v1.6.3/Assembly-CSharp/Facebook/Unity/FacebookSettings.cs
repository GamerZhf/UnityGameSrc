namespace Facebook.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class FacebookSettings : ScriptableObject
    {
        [SerializeField]
        private List<string> appIds;
        [SerializeField]
        private List<string> appLabels;
        [SerializeField]
        private List<UrlSchemes> appLinkSchemes;
        [SerializeField]
        private bool cookie;
        private const string FacebookSettingsAssetExtension = ".asset";
        private const string FacebookSettingsAssetName = "FacebookSettings";
        private const string FacebookSettingsPath = "FacebookSDK/SDK/Resources";
        [SerializeField]
        private bool frictionlessRequests;
        private static FacebookSettings instance;
        [SerializeField]
        private string iosURLSuffix;
        [SerializeField]
        private bool logging;
        [SerializeField]
        private int selectedAppIndex;
        [SerializeField]
        private bool status;
        [SerializeField]
        private bool xfbml;

        public FacebookSettings()
        {
            List<string> list = new List<string>();
            list.Add("0");
            this.appIds = list;
            list = new List<string>();
            list.Add("App Name");
            this.appLabels = list;
            this.cookie = true;
            this.logging = true;
            this.status = true;
            this.frictionlessRequests = true;
            this.iosURLSuffix = string.Empty;
            List<UrlSchemes> list2 = new List<UrlSchemes>();
            list2.Add(new UrlSchemes(null));
            this.appLinkSchemes = list2;
        }

        private static void DirtyEditor()
        {
        }

        public static void SettingsChanged()
        {
            DirtyEditor();
        }

        public static string AppId
        {
            get
            {
                return AppIds[SelectedAppIndex];
            }
        }

        public static List<string> AppIds
        {
            get
            {
                return Instance.appIds;
            }
            set
            {
                if (Instance.appIds != value)
                {
                    Instance.appIds = value;
                    DirtyEditor();
                }
            }
        }

        public static List<string> AppLabels
        {
            get
            {
                return Instance.appLabels;
            }
            set
            {
                if (Instance.appLabels != value)
                {
                    Instance.appLabels = value;
                    DirtyEditor();
                }
            }
        }

        public static List<UrlSchemes> AppLinkSchemes
        {
            get
            {
                return Instance.appLinkSchemes;
            }
            set
            {
                if (Instance.appLinkSchemes != value)
                {
                    Instance.appLinkSchemes = value;
                    DirtyEditor();
                }
            }
        }

        public static string ChannelUrl
        {
            get
            {
                return "/channel.html";
            }
        }

        public static bool Cookie
        {
            get
            {
                return Instance.cookie;
            }
            set
            {
                if (Instance.cookie != value)
                {
                    Instance.cookie = value;
                    DirtyEditor();
                }
            }
        }

        public static bool FrictionlessRequests
        {
            get
            {
                return Instance.frictionlessRequests;
            }
            set
            {
                if (Instance.frictionlessRequests != value)
                {
                    Instance.frictionlessRequests = value;
                    DirtyEditor();
                }
            }
        }

        private static FacebookSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load("FacebookSettings") as FacebookSettings;
                    if (instance == null)
                    {
                        instance = ScriptableObject.CreateInstance<FacebookSettings>();
                    }
                }
                return instance;
            }
        }

        public static string IosURLSuffix
        {
            get
            {
                return Instance.iosURLSuffix;
            }
            set
            {
                if (Instance.iosURLSuffix != value)
                {
                    Instance.iosURLSuffix = value;
                    DirtyEditor();
                }
            }
        }

        public static bool IsValidAppId
        {
            get
            {
                return (((AppId != null) && (AppId.Length > 0)) && !AppId.Equals("0"));
            }
        }

        public static bool Logging
        {
            get
            {
                return Instance.logging;
            }
            set
            {
                if (Instance.logging != value)
                {
                    Instance.logging = value;
                    DirtyEditor();
                }
            }
        }

        public static int SelectedAppIndex
        {
            get
            {
                return Instance.selectedAppIndex;
            }
            set
            {
                if (Instance.selectedAppIndex != value)
                {
                    Instance.selectedAppIndex = value;
                    DirtyEditor();
                }
            }
        }

        public static bool Status
        {
            get
            {
                return Instance.status;
            }
            set
            {
                if (Instance.status != value)
                {
                    Instance.status = value;
                    DirtyEditor();
                }
            }
        }

        public static bool Xfbml
        {
            get
            {
                return Instance.xfbml;
            }
            set
            {
                if (Instance.xfbml != value)
                {
                    Instance.xfbml = value;
                    DirtyEditor();
                }
            }
        }

        [Serializable]
        public class UrlSchemes
        {
            [SerializeField]
            private List<string> list;

            public UrlSchemes([Optional, DefaultParameterValue(null)] List<string> schemes)
            {
                this.list = (schemes != null) ? schemes : new List<string>();
            }

            public List<string> Schemes
            {
                get
                {
                    return this.list;
                }
                set
                {
                    this.list = value;
                }
            }
        }
    }
}

