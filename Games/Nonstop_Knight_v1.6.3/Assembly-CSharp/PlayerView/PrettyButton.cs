namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class PrettyButton : MonoBehaviour
    {
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public Image Bg;
        public UnityEngine.UI.Button Button;
        public UnityEngine.UI.Text CornerText;
        public Image Icon;
        public Image Icon2;
        public UnityEngine.UI.Text Text;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            if (this.Button == null)
            {
                this.Button = base.transform.GetComponent<UnityEngine.UI.Button>();
            }
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

