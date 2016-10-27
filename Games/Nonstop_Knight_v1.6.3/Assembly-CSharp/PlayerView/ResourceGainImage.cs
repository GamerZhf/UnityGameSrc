namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class ResourceGainImage : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public UnityEngine.UI.Image Borders;
        public UnityEngine.UI.Image Image;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
        }

        public void cleanUpForReuse()
        {
            this.Image.sprite = null;
            this.Borders.sprite = null;
        }

        public RectTransform RectTm
        {
            [CompilerGenerated]
            get
            {
                return this.<RectTm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RectTm>k__BackingField = value;
            }
        }
    }
}

