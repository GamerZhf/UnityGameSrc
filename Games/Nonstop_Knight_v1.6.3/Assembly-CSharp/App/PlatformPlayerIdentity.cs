namespace App
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.SocialPlatforms;

    public class PlatformPlayerIdentity : IUserProfile
    {
        [CompilerGenerated]
        private string <id>k__BackingField;
        [CompilerGenerated]
        private Texture2D <image>k__BackingField;
        [CompilerGenerated]
        private bool <isFriend>k__BackingField;
        [CompilerGenerated]
        private UserState <state>k__BackingField;
        [CompilerGenerated]
        private string <userName>k__BackingField;

        public string getPrettyName()
        {
            return this.userName.Substring(0, Mathf.Min(this.userName.Length, 0x20));
        }

        public string id
        {
            [CompilerGenerated]
            get
            {
                return this.<id>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<id>k__BackingField = value;
            }
        }

        public Texture2D image
        {
            [CompilerGenerated]
            get
            {
                return this.<image>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<image>k__BackingField = value;
            }
        }

        public bool isFriend
        {
            [CompilerGenerated]
            get
            {
                return this.<isFriend>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<isFriend>k__BackingField = value;
            }
        }

        public UserState state
        {
            [CompilerGenerated]
            get
            {
                return this.<state>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<state>k__BackingField = value;
            }
        }

        public string userName
        {
            [CompilerGenerated]
            get
            {
                return this.<userName>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<userName>k__BackingField = value;
            }
        }
    }
}

