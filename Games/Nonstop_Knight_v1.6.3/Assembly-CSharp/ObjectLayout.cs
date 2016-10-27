using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ObjectLayout : MonoBehaviour
{
    [CompilerGenerated]
    private Transform <Tm>k__BackingField;
    public string Id;
    private Layout m_layout;
    public string ResourcePath;

    protected void Awake()
    {
        this.Tm = base.transform;
    }

    public void clear()
    {
        this.destroyLayoutObjects();
        this.m_layout = null;
    }

    private bool deserialize(string id)
    {
        string path = this.ResourcePath + "/" + id;
        TextAsset asset = ResourceUtil.LoadSafe<TextAsset>(path, false);
        if (asset == null)
        {
            Debug.LogError("Object layout not found: " + path);
            return false;
        }
        this.m_layout = JsonUtils.Deserialize<Layout>(asset.text, true);
        return true;
    }

    private void destroyLayoutObjects()
    {
        foreach (Transform transform in TransformExtensions.GetChildren(base.transform, false))
        {
            UnityEngine.Object.DestroyImmediate(transform.gameObject);
        }
    }

    private void instantiateLayoutObjects()
    {
        foreach (LayoutObject obj2 in this.m_layout.Objects)
        {
            GameObject obj3 = null;
            if (Application.isPlaying)
            {
                obj3 = ResourceUtil.Instantiate<GameObject>(obj2.PrefabId);
            }
            if (obj3 != null)
            {
                Transform transform = obj3.transform;
                transform.parent = base.transform;
                transform.localPosition = obj2.Position;
                transform.localRotation = obj2.Rotation;
                transform.localScale = obj2.Scale;
            }
        }
    }

    public void load([Optional, DefaultParameterValue(null)] Layout preloadedLayout)
    {
        this.clear();
        if (preloadedLayout != null)
        {
            this.m_layout = preloadedLayout;
        }
        else
        {
            this.deserialize(this.Id);
        }
        if (this.m_layout != null)
        {
            this.instantiateLayoutObjects();
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

    public class Layout
    {
        public string Id;
        public List<ObjectLayout.LayoutObject> Objects = new List<ObjectLayout.LayoutObject>();
    }

    public class LayoutObject
    {
        public Vector3 Position;
        public string PrefabId;
        public Quaternion Rotation;
        public Vector3 Scale;
    }
}

