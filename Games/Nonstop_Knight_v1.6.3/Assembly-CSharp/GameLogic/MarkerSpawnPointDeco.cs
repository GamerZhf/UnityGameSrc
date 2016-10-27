namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class MarkerSpawnPointDeco : GameLogic.AbstractMarker
    {
        [CompilerGenerated]
        private DungeonDeco <ActiveDeco>k__BackingField;
        [CompilerGenerated]
        private bool <Loaded>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        public DungeonDecoCategoryType DecoCategoryType;
        public string FixedDecoId = string.Empty;

        protected void Awake()
        {
            this.Tm = base.transform;
            this.unload();
            if (Application.isPlaying)
            {
                TransformExtensions.DestroyChildren(this.Tm);
            }
            this.setFixedValues();
        }

        public void load(DungeonThemeType theme, DungeonMood mood, [Optional, DefaultParameterValue(null)] Dictionary<DungeonDecoLayerType, string> layers)
        {
            this.unload();
            if (this.DecoCategoryType != DungeonDecoCategoryType.NONE)
            {
                if (!DungeonDecoResources.ThemeLoaded(theme))
                {
                    DungeonDecoResources.LoadTheme(theme);
                }
                if (!string.IsNullOrEmpty(this.FixedDecoId))
                {
                    this.spawnDeco(this.FixedDecoId, mood, layers);
                }
                else
                {
                    RaycastHit hit;
                    bool flag = true;
                    if (Application.isPlaying)
                    {
                        flag = UnityEngine.Random.Range((float) 0f, (float) 1f) <= ConfigDungeons.DUNGEON_DECO_CATEGORY_SPAWN_PROBABILITY[this.DecoCategoryType];
                    }
                    if (flag && Physics.Raycast(this.Tm.position + ((Vector3) (Vector3.up * 999f)), Vector3.down, out hit, float.MaxValue, Layers.GroundLayerMask))
                    {
                        DungeonBlock component = hit.collider.GetComponent<DungeonBlock>();
                        if ((component != null) && component.DontSpawnDecosAbove)
                        {
                            flag = false;
                        }
                    }
                    if ((flag && (mood != null)) && ((mood.DungeonLightIntensity <= 0f) && (this.DecoCategoryType == DungeonDecoCategoryType.FloorDeco1B)))
                    {
                        flag = false;
                    }
                    if (flag)
                    {
                        this.spawnDeco(DungeonDecoResources.GetRandomDeco(theme, this.DecoCategoryType), mood, layers);
                    }
                }
                this.Loaded = true;
            }
        }

        private void setFixedValues()
        {
            base.shape = GameLogic.AbstractMarker.Shape.SPHERE;
            base.color = Color.magenta;
            base.customSize = true;
            base.customSphereRadius = 0.35f;
        }

        private void spawnDeco(string decoPrefabId, DungeonMood mood, [Optional, DefaultParameterValue(null)] Dictionary<DungeonDecoLayerType, string> layers)
        {
            if (Application.isPlaying)
            {
                this.ActiveDeco = ResourceUtil.Instantiate<GameObject>(decoPrefabId).GetComponent<DungeonDeco>();
                this.ActiveDeco.refreshLights(LangUtil.GetRandomValueFromList<Color>(mood.DungeonLightColors), mood.DungeonLightIntensity);
            }
            this.ActiveDeco.refreshRotation();
            this.ActiveDeco.refreshLayers(layers);
            this.ActiveDeco.transform.SetParent(this.Tm, false);
            this.ActiveDeco.transform.localPosition = Vector3.zero;
            this.ActiveDeco.gameObject.SetActive(true);
            if (Application.isPlaying)
            {
                this.ActiveDeco.transform.SetParent(GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.LayoutRoot.transform, true);
            }
        }

        public void unload()
        {
            if (this.ActiveDeco != null)
            {
                if (Application.isPlaying)
                {
                    UnityEngine.Object.Destroy(this.ActiveDeco.gameObject);
                }
                else
                {
                    UnityEngine.Object.DestroyImmediate(this.ActiveDeco.gameObject);
                }
                this.ActiveDeco = null;
            }
            if (!Application.isPlaying)
            {
                TransformExtensions.DestroyChildrenImmediate(this.Tm);
            }
            this.Loaded = false;
        }

        public DungeonDeco ActiveDeco
        {
            [CompilerGenerated]
            get
            {
                return this.<ActiveDeco>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ActiveDeco>k__BackingField = value;
            }
        }

        public bool Loaded
        {
            [CompilerGenerated]
            get
            {
                return this.<Loaded>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Loaded>k__BackingField = value;
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

