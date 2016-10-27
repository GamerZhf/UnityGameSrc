namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class RunestoneRowCell : MonoBehaviour
    {
        [CompilerGenerated]
        private RectTransform <Tm>k__BackingField;
        public Image Background;
        public Image FirstColImage;
        public GridLayoutGroup GridRoot;

        protected void Awake()
        {
            this.Tm = base.GetComponent<RectTransform>();
        }

        public RectTransform Tm
        {
            [CompilerGenerated]
            get
            {
                return this.<Tm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Tm>k__BackingField = value;
            }
        }
    }
}

