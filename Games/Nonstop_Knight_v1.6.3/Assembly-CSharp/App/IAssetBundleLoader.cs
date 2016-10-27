namespace App
{
    using System;
    using UnityEngine;

    public interface IAssetBundleLoader
    {
        GameObject instantiatePrefab(string id);
        GameObject loadPrefab(string id);

        bool AssetBundlesAvailable { get; }

        bool Loaded { get; }

        UnityEngine.Object[] Prefabs { get; }

        Sprite[] Sprites { get; }
    }
}

