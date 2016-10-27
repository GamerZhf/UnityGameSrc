namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [ExecuteInEditMode]
    public class SortingLayerSetter : MonoBehaviour
    {
        [CompilerGenerated]
        private UnityEngine.Renderer <Renderer>k__BackingField;
        public bool ApplyToChildren;
        public string SortingLayerName;
        public int SortingOrder;

        protected void Awake()
        {
            this.Renderer = base.GetComponent<UnityEngine.Renderer>();
            this.refresh();
        }

        protected void LateUpdate()
        {
            if ((this.Renderer.sortingLayerName != this.SortingLayerName) || (this.Renderer.sortingOrder != this.SortingOrder))
            {
                this.refresh();
            }
        }

        private void refresh()
        {
            UnityEngine.Renderer component = base.GetComponent<UnityEngine.Renderer>();
            if (component != null)
            {
                component.sortingLayerName = this.SortingLayerName;
                component.sortingOrder = this.SortingOrder;
            }
            if (this.ApplyToChildren)
            {
                UnityEngine.Renderer[] componentsInChildren = base.GetComponentsInChildren<UnityEngine.Renderer>();
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].sortingLayerName = this.SortingLayerName;
                    componentsInChildren[i].sortingOrder = this.SortingOrder;
                }
            }
        }

        public UnityEngine.Renderer Renderer
        {
            [CompilerGenerated]
            get
            {
                return this.<Renderer>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Renderer>k__BackingField = value;
            }
        }
    }
}

