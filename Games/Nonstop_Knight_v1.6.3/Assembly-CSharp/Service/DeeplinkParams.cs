namespace Service
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class DeeplinkParams : CustomParams
    {
        [CompilerGenerated]
        private string <OpenURL>k__BackingField;
        private const string PARAM_URL = "open-url";

        public DeeplinkParams(Dictionary<string, object> promoParams) : base(promoParams)
        {
            if (promoParams != null)
            {
                object obj2;
                promoParams.TryGetValue("open-url", out obj2);
                if (obj2 != null)
                {
                    this.OpenURL = obj2 as string;
                }
            }
        }

        public override bool Validate()
        {
            bool flag = !string.IsNullOrEmpty(this.OpenURL);
            if (!flag)
            {
                Debug.LogWarning("Promotion Validation failed: open-url must be set");
            }
            return (flag && base.Validate());
        }

        public string OpenURL
        {
            [CompilerGenerated]
            get
            {
                return this.<OpenURL>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<OpenURL>k__BackingField = value;
            }
        }
    }
}

