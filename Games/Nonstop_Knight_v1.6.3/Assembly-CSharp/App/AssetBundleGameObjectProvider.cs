namespace App
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class AssetBundleGameObjectProvider : GameObjectProvider
    {
        public AssetBundleGameObjectProvider(string resourceName, Transform objectPoolParentTm, [Optional, DefaultParameterValue(-1)] int layer)
        {
            base.m_resourceName = resourceName;
            base.m_layer = layer;
            base.m_prototype = Binder.AssetBundleLoader.loadPrefab(base.m_resourceName);
            base.m_objectPoolParentTm = objectPoolParentTm;
        }
    }
}

