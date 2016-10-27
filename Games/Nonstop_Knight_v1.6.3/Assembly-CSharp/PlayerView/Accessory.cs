namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using Xft;

    public class Accessory : MonoBehaviour, IFlashable
    {
        [CompilerGenerated]
        private ParticleSystem[] <ParticleSystems>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        [CompilerGenerated]
        private AccessoryType <Type>k__BackingField;
        private Color m_flashMaterialColor;
        private List<FlashMaterial> m_flashMaterials;
        private bool m_flashMaterialsEnabled;
        public UnityEngine.Renderer Renderer;
        public XWeaponTrail Trail;

        protected void Awake()
        {
            this.Tm = base.transform;
            this.ParticleSystems = this.Tm.GetComponentsInChildren<ParticleSystem>();
        }

        public void initializeFlashMaterials()
        {
            this.m_flashMaterials = new List<FlashMaterial>();
            FlashMaterial item = new FlashMaterial();
            item.Renderer = this.Renderer;
            item.FlashMaterials = new Material[this.Renderer.sharedMaterials.Length];
            for (int i = 0; i < this.Renderer.sharedMaterials.Length; i++)
            {
                Material material2 = UnityEngine.Object.Instantiate<Material>(this.Renderer.sharedMaterials[i]);
                material2.name = material2.name + "-INSTANCE";
                material2.shader = Shader.Find("CUSTOM/Opaque_Monochrome");
                item.FlashMaterials[i] = material2;
            }
            this.m_flashMaterials.Add(item);
        }

        protected void LateUpdate()
        {
            if (this.m_flashMaterialsEnabled)
            {
                for (int i = 0; i < this.m_flashMaterials.Count; i++)
                {
                    for (int j = 0; j < this.m_flashMaterials[i].FlashMaterials.Length; j++)
                    {
                        this.m_flashMaterials[i].FlashMaterials[j].color = this.m_flashMaterialColor;
                    }
                }
            }
        }

        public void setFlashMaterialColor(Color color)
        {
            this.m_flashMaterialColor = color;
        }

        public void setFlashMaterialsEnabled(bool enabled)
        {
            if (this.m_flashMaterialsEnabled != enabled)
            {
                for (int i = 0; i < this.m_flashMaterials.Count; i++)
                {
                    FlashMaterial material = this.m_flashMaterials[i];
                    if (enabled)
                    {
                        material.MaterialsBeforeFlash = material.Renderer.sharedMaterials;
                        material.Renderer.sharedMaterials = material.FlashMaterials;
                    }
                    else
                    {
                        material.Renderer.sharedMaterials = material.MaterialsBeforeFlash;
                    }
                }
                this.m_flashMaterialsEnabled = enabled;
            }
        }

        public void setVisibility(bool visible)
        {
            this.Renderer.enabled = visible;
            if (this.ParticleSystems != null)
            {
                for (int i = 0; i < this.ParticleSystems.Length; i++)
                {
                    if (visible)
                    {
                        GameObjectExtensions.SetLayerRecursively(this.ParticleSystems[i].gameObject, Layers.CHARACTER_VIEWS);
                    }
                    else
                    {
                        GameObjectExtensions.SetLayerRecursively(this.ParticleSystems[i].gameObject, Layers.TRANSPARENT_FX);
                    }
                }
            }
        }

        public ParticleSystem[] ParticleSystems
        {
            [CompilerGenerated]
            get
            {
                return this.<ParticleSystems>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ParticleSystems>k__BackingField = value;
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

        public AccessoryType Type
        {
            [CompilerGenerated]
            get
            {
                return this.<Type>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Type>k__BackingField = value;
            }
        }

        private class FlashMaterial
        {
            public Material[] FlashMaterials;
            public Material[] MaterialsBeforeFlash;
            public UnityEngine.Renderer Renderer;
        }
    }
}

