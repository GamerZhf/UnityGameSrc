namespace GameLogic
{
    using App;
    using Pathfinding.Serialization.JsonFx;
    using PlayerView;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class DungeonMood : IJsonData
    {
        public Color AmbientLightColor;
        public Color BackgroundColor;
        public Color DirectionalLightColor;
        public float DirectionalLightIntensity;
        public Vector2 DirectionalLightOrientation;
        public List<Color> DungeonLightColors;
        public float DungeonLightIntensity;
        public float FloorAndDecoMoodContribution;
        public Color FogColor;
        public Color HeroLightColor;
        public float HeroLightIntensity;
        public float HeroLightRange;
        public float HorizontalFogEndTerm;
        public float HorizontalFogStartTerm;
        [JsonIgnore]
        public string Id;
        public Color PropColor;
        public EffectType Weather;

        public DungeonMood()
        {
            this.Id = string.Empty;
            this.AmbientLightColor = Color.black;
            this.DirectionalLightColor = Color.white;
            this.DirectionalLightOrientation = new Vector2(45f, -30f);
            this.FogColor = new Color(0.1098039f, 0.6745098f, 0.6078432f, 1f);
            this.BackgroundColor = new Color(0.1176471f, 0.345098f, 0.3372549f, 1f);
            this.HeroLightColor = new Color(0.9411765f, 0.5019608f, 0.5019608f, 1f);
            this.HeroLightIntensity = 2.5f;
            this.HeroLightRange = 13f;
            this.HorizontalFogStartTerm = 12f;
            this.HorizontalFogEndTerm = 28f;
            this.FloorAndDecoMoodContribution = 1f;
            List<Color> list = new List<Color>();
            list.Add(new Color(1f, 0.5647059f, 0f, 1f));
            list.Add(new Color(1f, 0.5647059f, 0f, 1f));
            list.Add(new Color(1f, 0.5647059f, 0f, 1f));
            this.DungeonLightColors = list;
            this.DungeonLightIntensity = 1f;
            this.PropColor = new Color(1f, 1f, 1f, 1f);
        }

        public DungeonMood(DungeonMood prototype)
        {
            this.Id = string.Empty;
            this.AmbientLightColor = Color.black;
            this.DirectionalLightColor = Color.white;
            this.DirectionalLightOrientation = new Vector2(45f, -30f);
            this.FogColor = new Color(0.1098039f, 0.6745098f, 0.6078432f, 1f);
            this.BackgroundColor = new Color(0.1176471f, 0.345098f, 0.3372549f, 1f);
            this.HeroLightColor = new Color(0.9411765f, 0.5019608f, 0.5019608f, 1f);
            this.HeroLightIntensity = 2.5f;
            this.HeroLightRange = 13f;
            this.HorizontalFogStartTerm = 12f;
            this.HorizontalFogEndTerm = 28f;
            this.FloorAndDecoMoodContribution = 1f;
            List<Color> list = new List<Color>();
            list.Add(new Color(1f, 0.5647059f, 0f, 1f));
            list.Add(new Color(1f, 0.5647059f, 0f, 1f));
            list.Add(new Color(1f, 0.5647059f, 0f, 1f));
            this.DungeonLightColors = list;
            this.DungeonLightIntensity = 1f;
            this.PropColor = new Color(1f, 1f, 1f, 1f);
            this.copyFrom(prototype);
        }

        public void copyFrom(DungeonMood another)
        {
            this.Id = another.Id;
            this.AmbientLightColor = another.AmbientLightColor;
            this.DirectionalLightColor = another.DirectionalLightColor;
            this.DirectionalLightIntensity = another.DirectionalLightIntensity;
            this.DirectionalLightOrientation = another.DirectionalLightOrientation;
            this.FogColor = another.FogColor;
            this.BackgroundColor = another.BackgroundColor;
            this.HeroLightColor = another.HeroLightColor;
            this.HeroLightIntensity = another.HeroLightIntensity;
            this.HeroLightRange = another.HeroLightRange;
            this.HorizontalFogStartTerm = another.HorizontalFogStartTerm;
            this.HorizontalFogEndTerm = another.HorizontalFogEndTerm;
            this.FloorAndDecoMoodContribution = another.FloorAndDecoMoodContribution;
            this.DungeonLightColors = new List<Color>(another.DungeonLightColors);
            this.DungeonLightIntensity = another.DungeonLightIntensity;
            this.Weather = another.Weather;
            this.PropColor = another.PropColor;
        }

        public void postDeserializeInitialization()
        {
        }

        public void refreshSceneDirectionalLight()
        {
            Light[] lightArray = UnityEngine.Object.FindObjectsOfType<Light>();
            List<Light> list = new List<Light>();
            for (int i = 0; i < lightArray.Length; i++)
            {
                if (!lightArray[i].name.StartsWith("MenuDirectional") && (lightArray[i].type == LightType.Directional))
                {
                    list.Add(lightArray[i]);
                }
            }
            Light light = list[0];
            light.gameObject.SetActive(true);
            light.enabled = true;
            light.color = this.DirectionalLightColor;
            light.intensity = this.DirectionalLightIntensity;
            light.transform.position = new Vector3(0f, 50f, 0f);
            light.transform.rotation = Quaternion.Euler(new Vector3(this.DirectionalLightOrientation.x, this.DirectionalLightOrientation.y, 0f));
        }
    }
}

