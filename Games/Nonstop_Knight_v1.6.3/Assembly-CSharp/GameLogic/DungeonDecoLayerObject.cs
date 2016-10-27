namespace GameLogic
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class DungeonDecoLayerObject : MonoBehaviour
    {
        [CompilerGenerated]
        private UnityEngine.Renderer <Renderer>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        public DungeonDecoLayerType Layer;

        protected void Awake()
        {
            this.refresh();
        }

        public void refresh()
        {
            this.Tm = base.transform;
            this.Renderer = base.GetComponent<UnityEngine.Renderer>();
        }

        public void setMaterial(string id)
        {
            if (this.Tm == null)
            {
                this.refresh();
            }
            if (!string.IsNullOrEmpty(id))
            {
                this.Renderer.sharedMaterial = ResourceUtil.Instantiate<Material>(string.Concat(new object[] { "Materials/DungeonDecoLayers/", this.Layer, "/", id }));
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

        public Transform Tm
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

