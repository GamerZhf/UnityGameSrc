namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class PetRowCell : MonoBehaviour
    {
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public RectTransform GridTm;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
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

