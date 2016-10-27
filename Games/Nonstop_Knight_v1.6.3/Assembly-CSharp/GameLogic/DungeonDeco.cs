namespace GameLogic
{
    using App;
    using PlayerView;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class DungeonDeco : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private string <DecoPrefabId>k__BackingField;
        [CompilerGenerated]
        private DropLight[] <DropLights>k__BackingField;
        [CompilerGenerated]
        private DungeonLight[] <DungeonLights>k__BackingField;
        [CompilerGenerated]
        private DungeonDecoLayerObject[] <LayerObjects>k__BackingField;
        [CompilerGenerated]
        private ParticleSystem[] <ParticleSystems>k__BackingField;
        [CompilerGenerated]
        private Renderer[] <Renderers>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        private Color m_activeColor;
        private float m_activeIntensity;
        private Dictionary<DungeonDecoLayerType, List<DungeonDecoLayerObject>> m_layerObjectLookup = new Dictionary<DungeonDecoLayerType, List<DungeonDecoLayerObject>>(new DungeonDecoLayerTypeBoxAvoidanceComparer());
        private Quaternion m_origLocalRotation;
        public int RandomRotationGranularity;

        protected void Awake()
        {
            this.refresh();
        }

        public void cleanUpForReuse()
        {
        }

        public Color getActiveColor()
        {
            return this.m_activeColor;
        }

        public float getActiveIntensity()
        {
            return this.m_activeIntensity;
        }

        private void refresh()
        {
            this.Tm = base.transform;
            this.DungeonLights = this.Tm.GetComponentsInChildren<DungeonLight>();
            this.DropLights = this.Tm.GetComponentsInChildren<DropLight>();
            this.ParticleSystems = this.Tm.GetComponentsInChildren<ParticleSystem>();
            this.Renderers = this.Tm.GetComponentsInChildren<Renderer>();
            this.LayerObjects = this.Tm.GetComponentsInChildren<DungeonDecoLayerObject>();
            this.m_layerObjectLookup.Clear();
            for (int i = 0; i < this.LayerObjects.Length; i++)
            {
                DungeonDecoLayerObject item = this.LayerObjects[i];
                if (!this.m_layerObjectLookup.ContainsKey(item.Layer))
                {
                    this.m_layerObjectLookup.Add(item.Layer, new List<DungeonDecoLayerObject>());
                }
                this.m_layerObjectLookup[item.Layer].Add(item);
            }
            if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Med)
            {
                for (int j = 0; j < this.Renderers.Length; j++)
                {
                    if (this.Renderers[j].sharedMaterial.shader.name == "CUSTOM/Deco_Prop")
                    {
                        this.Renderers[j].sharedMaterial.shader = Shader.Find("CUSTOM/Deco_Prop_Opaque");
                    }
                }
            }
            else if (ConfigDevice.IsAndroid())
            {
                for (int k = 0; k < this.Renderers.Length; k++)
                {
                    if (this.Renderers[k].sharedMaterial.shader.name == "CUSTOM/Deco_Prop")
                    {
                        this.Renderers[k].sharedMaterial.shader = Shader.Find("CUSTOM/Deco_Prop-ANDROID");
                    }
                }
            }
            this.m_origLocalRotation = this.Tm.localRotation;
        }

        public void refreshLayers(Dictionary<DungeonDecoLayerType, string> layers)
        {
            if (layers == null)
            {
                for (int i = 0; i < this.LayerObjects.Length; i++)
                {
                    this.LayerObjects[i].gameObject.SetActive(false);
                }
            }
            else
            {
                for (int j = 0; j < this.LayerObjects.Length; j++)
                {
                    DungeonDecoLayerObject obj2 = this.LayerObjects[j];
                    if (layers.ContainsKey(obj2.Layer))
                    {
                        obj2.gameObject.SetActive(true);
                        obj2.setMaterial(layers[obj2.Layer]);
                    }
                    else
                    {
                        obj2.gameObject.SetActive(false);
                    }
                }
            }
        }

        public void refreshLights(Color color, float intensity)
        {
            if (this.Tm == null)
            {
                this.refresh();
            }
            for (int i = 0; i < this.DungeonLights.Length; i++)
            {
                DungeonLight light = this.DungeonLights[i];
                if (!light.LockColor)
                {
                    light.Color = color;
                }
                if (!light.LockIntensity)
                {
                    light.LightIntensity = intensity;
                    light.setVisible(intensity > 0f);
                }
            }
            for (int j = 0; j < this.DropLights.Length; j++)
            {
                DropLight light2 = this.DropLights[j];
                if (!light2.LockColor)
                {
                    light2.Color = color;
                }
                if (!light2.LockIntensity)
                {
                    light2.LightIntensity = intensity;
                    light2.setVisible(intensity > 0f);
                }
            }
            for (int k = 0; k < this.ParticleSystems.Length; k++)
            {
                ParticleSystem system = this.ParticleSystems[k];
                if (intensity > 0f)
                {
                    system.gameObject.SetActive(true);
                }
                else
                {
                    system.gameObject.SetActive(false);
                }
            }
            this.m_activeColor = color;
            this.m_activeIntensity = intensity;
        }

        public void refreshRotation()
        {
            if (this.Tm == null)
            {
                this.refresh();
            }
            if (this.RandomRotationGranularity <= 0)
            {
                if (this.RandomRotationGranularity < 0)
                {
                    Debug.LogError("Only positive rotation values supported: " + base.gameObject.name);
                }
            }
            else
            {
                float y = UnityEngine.Random.Range(0, (360 / this.RandomRotationGranularity) + 1) * this.RandomRotationGranularity;
                this.Tm.localRotation = Quaternion.Euler(0f, y, 0f) * this.m_origLocalRotation;
            }
        }

        public string DecoPrefabId
        {
            [CompilerGenerated]
            get
            {
                return this.<DecoPrefabId>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<DecoPrefabId>k__BackingField = value;
            }
        }

        public DropLight[] DropLights
        {
            [CompilerGenerated]
            get
            {
                return this.<DropLights>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<DropLights>k__BackingField = value;
            }
        }

        public DungeonLight[] DungeonLights
        {
            [CompilerGenerated]
            get
            {
                return this.<DungeonLights>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<DungeonLights>k__BackingField = value;
            }
        }

        public DungeonDecoLayerObject[] LayerObjects
        {
            [CompilerGenerated]
            get
            {
                return this.<LayerObjects>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<LayerObjects>k__BackingField = value;
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
            private set
            {
                this.<ParticleSystems>k__BackingField = value;
            }
        }

        public Renderer[] Renderers
        {
            [CompilerGenerated]
            get
            {
                return this.<Renderers>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Renderers>k__BackingField = value;
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

