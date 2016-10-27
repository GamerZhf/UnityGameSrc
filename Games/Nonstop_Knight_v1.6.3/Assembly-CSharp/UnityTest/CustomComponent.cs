namespace UnityTest
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CustomComponent : MonoBehaviour
    {
        [CompilerGenerated]
        private float <MyFloatProp>k__BackingField;
        public float MyFloatField = 3f;

        public float MyFloatProp
        {
            [CompilerGenerated]
            get
            {
                return this.<MyFloatProp>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<MyFloatProp>k__BackingField = value;
            }
        }
    }
}

