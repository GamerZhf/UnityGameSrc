namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [ExecuteInEditMode]
    public class DungeonBlock : MonoBehaviour
    {
        [CompilerGenerated]
        private GameObject <LoadedChildInstance>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.MeshFilter <MeshFilter>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.MeshRenderer <MeshRenderer>k__BackingField;
        [CompilerGenerated]
        private bool <RoomBuilderContentHidden>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        public DungeonBlockType BlockType;
        public bool DontSpawnDecosAbove;
        public const string INSTANCE_POSTFIX = "-BLOCKINSTANCE";
        public bool LockRotation;

        protected void Awake()
        {
            this.Tm = base.transform;
            this.MeshRenderer = base.GetComponent<UnityEngine.MeshRenderer>();
            this.MeshFilter = base.GetComponent<UnityEngine.MeshFilter>();
            this.unload();
        }

        private void hideRoomBuilderContent()
        {
            if (!this.RoomBuilderContentHidden)
            {
                if (Application.isPlaying)
                {
                    Component[] components = base.GetComponents(typeof(Component));
                    for (int i = components.Length - 1; i >= 0; i--)
                    {
                        Component component = components[i];
                        if (!(component is Transform) && !(component is DungeonBlock))
                        {
                            UnityEngine.Object.Destroy(component);
                        }
                    }
                    TransformExtensions.DestroyChildren(base.transform);
                }
                else
                {
                    Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
                    for (int j = 0; j < componentsInChildren.Length; j++)
                    {
                        componentsInChildren[j].enabled = false;
                    }
                }
                this.RoomBuilderContentHidden = false;
            }
        }

        public void load(DungeonThemeType themeType, bool isTutorialDungeon)
        {
            this.unload();
            if (!this.RoomBuilderContentHidden)
            {
                this.hideRoomBuilderContent();
            }
            if (this.BlockType != DungeonBlockType.UNSPECIFIED)
            {
                if (!GameLogic.Binder.DungeonBlockResources.themeLoaded(themeType))
                {
                    GameLogic.Binder.DungeonBlockResources.loadTheme(themeType);
                }
                string keyFromDictionaryWithWeights = LangUtil.GetKeyFromDictionaryWithWeights<string>(ConfigDungeons.DUNGEON_BLOCK_POOLS[themeType][this.BlockType], null);
                if (isTutorialDungeon)
                {
                    int num2 = 0;
                    while ((GameLogic.Binder.DungeonBlockResources.getDungeonBlockPrototype(keyFromDictionaryWithWeights).GetComponent<DungeonObstacle>() != null) && (num2 < 0x3e8))
                    {
                        uint? nullable2 = null;
                        keyFromDictionaryWithWeights = LangUtil.GetKeyFromDictionaryWithWeights<string>(ConfigDungeons.DUNGEON_BLOCK_POOLS[themeType][this.BlockType], nullable2);
                        num2++;
                    }
                    if (num2 >= 0x3e8)
                    {
                        Debug.LogError("Cannot spawn non-obstacle block in tutorial dungeon, skipping block..");
                        return;
                    }
                }
                GameObject obj2 = GameLogic.Binder.DungeonBlockResources.instantiateDungeonBlock(keyFromDictionaryWithWeights);
                obj2.name = obj2.name + "-BLOCKINSTANCE";
                obj2.transform.SetParent(base.transform, false);
                obj2.transform.localPosition = Vector3.zero;
                obj2.transform.localScale = Vector3.one;
                obj2.transform.localRotation = Quaternion.identity;
                this.LoadedChildInstance = obj2.gameObject;
                DungeonBlock component = obj2.GetComponent<DungeonBlock>();
                this.LockRotation = component.LockRotation;
                this.DontSpawnDecosAbove = component.DontSpawnDecosAbove;
                if (Application.isPlaying)
                {
                    UnityEngine.Object.Destroy(component);
                }
                else
                {
                    UnityEngine.Object.DestroyImmediate(component);
                }
                if (!this.LockRotation)
                {
                    obj2.transform.Rotate(Vector3.up, LangUtil.GetRandomValueFromList<float>(ConfigDungeons.DUNGEON_BLOCK_RANDOMIZED_STARTING_ROTATION), Space.World);
                }
            }
        }

        public void showRoomBuilderContent()
        {
            if (!Application.isPlaying)
            {
                Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].enabled = true;
                }
            }
        }

        public void unload()
        {
            if (base.gameObject != null)
            {
                if (this.LoadedChildInstance != null)
                {
                    if (Application.isPlaying)
                    {
                        UnityEngine.Object.Destroy(this.LoadedChildInstance);
                    }
                    else
                    {
                        UnityEngine.Object.DestroyImmediate(this.LoadedChildInstance);
                    }
                    this.LoadedChildInstance = null;
                }
                Transform[] componentsInChildren = base.gameObject.GetComponentsInChildren<Transform>(true);
                List<GameObject> list = null;
                for (int i = componentsInChildren.Length - 1; i >= 0; i--)
                {
                    if (componentsInChildren[i].name.EndsWith("-BLOCKINSTANCE"))
                    {
                        if (list == null)
                        {
                            list = new List<GameObject>();
                        }
                        list.Add(componentsInChildren[i].gameObject);
                    }
                }
                if (list != null)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (Application.isPlaying)
                        {
                            UnityEngine.Object.Destroy(list[j].gameObject);
                        }
                        else
                        {
                            UnityEngine.Object.DestroyImmediate(list[j].gameObject);
                        }
                    }
                }
            }
        }

        public GameObject LoadedChildInstance
        {
            [CompilerGenerated]
            get
            {
                return this.<LoadedChildInstance>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<LoadedChildInstance>k__BackingField = value;
            }
        }

        public UnityEngine.MeshFilter MeshFilter
        {
            [CompilerGenerated]
            get
            {
                return this.<MeshFilter>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<MeshFilter>k__BackingField = value;
            }
        }

        public UnityEngine.MeshRenderer MeshRenderer
        {
            [CompilerGenerated]
            get
            {
                return this.<MeshRenderer>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<MeshRenderer>k__BackingField = value;
            }
        }

        public bool RoomBuilderContentHidden
        {
            [CompilerGenerated]
            get
            {
                return this.<RoomBuilderContentHidden>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RoomBuilderContentHidden>k__BackingField = value;
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

