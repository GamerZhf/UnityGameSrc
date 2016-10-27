namespace App
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AssetBundleLoader : MonoBehaviour, IAssetBundleLoader
    {
        [CompilerGenerated]
        private bool <AssetBundlesAvailable>k__BackingField;
        [CompilerGenerated]
        private bool <Loaded>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.Object[] <Prefabs>k__BackingField;
        [CompilerGenerated]
        private Sprite[] <Sprites>k__BackingField;

        public GameObject instantiatePrefab(string id)
        {
            if (!this.AssetBundlesAvailable)
            {
                return ResourceUtil.Instantiate<GameObject>(id);
            }
            id = Path.GetFileNameWithoutExtension(id);
            for (int i = 0; i < this.Prefabs.Length; i++)
            {
                UnityEngine.Object original = this.Prefabs[i];
                if (original.name == id)
                {
                    GameObject obj3 = (GameObject) UnityEngine.Object.Instantiate(original);
                    obj3.name = original.name;
                    return obj3;
                }
            }
            return null;
        }

        private WWW loadAssetBundleAsync(string id)
        {
            string[] textArray1 = new string[] { "file://", Application.streamingAssetsPath, "/AssetBundles/", ConfigDevice.GetAssetBundlePlatformKey(), "/", id };
            string url = string.Concat(textArray1);
            if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Low)
            {
                url = url + ".sd";
            }
            else
            {
                url = url + ".hd";
            }
            return WWW.LoadFromCacheOrDownload(url, 1);
        }

        private AssetBundle loadAssetBundleSync(string id)
        {
            string[] textArray1 = new string[] { Application.streamingAssetsPath, "/AssetBundles/", ConfigDevice.GetAssetBundlePlatformKey(), "/", id };
            string path = string.Concat(textArray1);
            if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Low)
            {
                path = path + ".sd";
            }
            else
            {
                path = path + ".hd";
            }
            return AssetBundle.CreateFromFile(path);
        }

        public GameObject loadPrefab(string id)
        {
            if (!this.AssetBundlesAvailable)
            {
                return Resources.Load<GameObject>(id);
            }
            id = Path.GetFileNameWithoutExtension(id);
            for (int i = 0; i < this.Prefabs.Length; i++)
            {
                UnityEngine.Object obj2 = this.Prefabs[i];
                if (obj2.name == id)
                {
                    return (GameObject) obj2;
                }
            }
            return null;
        }

        [DebuggerHidden]
        protected IEnumerator Start()
        {
            <Start>c__Iterator30 iterator = new <Start>c__Iterator30();
            iterator.<>f__this = this;
            return iterator;
        }

        public bool AssetBundlesAvailable
        {
            [CompilerGenerated]
            get
            {
                return this.<AssetBundlesAvailable>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<AssetBundlesAvailable>k__BackingField = value;
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

        public UnityEngine.Object[] Prefabs
        {
            [CompilerGenerated]
            get
            {
                return this.<Prefabs>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Prefabs>k__BackingField = value;
            }
        }

        public Sprite[] Sprites
        {
            [CompilerGenerated]
            get
            {
                return this.<Sprites>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Sprites>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <Start>c__Iterator30 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal AssetBundleLoader <>f__this;
            internal AssetBundle <ab>__1;
            internal AssetBundle <ab>__3;
            internal bool <assetBundleMissing>__0;
            internal WWW <www>__2;
            internal WWW <www>__4;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (ConfigApp.AssetBundlesEnabled)
                        {
                            this.<assetBundleMissing>__0 = false;
                            if (Application.isEditor)
                            {
                                this.<ab>__1 = this.<>f__this.loadAssetBundleSync("sprites");
                                if (this.<ab>__1 != null)
                                {
                                    this.<>f__this.Sprites = this.<ab>__1.LoadAllAssets<Sprite>();
                                }
                                else
                                {
                                    this.<assetBundleMissing>__0 = true;
                                }
                                break;
                            }
                            this.<www>__2 = this.<>f__this.loadAssetBundleAsync("sprites");
                            this.$current = this.<www>__2;
                            this.$PC = 1;
                            goto Label_01C3;
                        }
                        this.<>f__this.AssetBundlesAvailable = false;
                        this.<>f__this.Loaded = true;
                        goto Label_01C1;

                    case 1:
                        this.<>f__this.Sprites = this.<www>__2.assetBundle.LoadAllAssets<Sprite>();
                        break;

                    case 2:
                        this.<>f__this.Prefabs = this.<www>__4.assetBundle.LoadAllAssets<UnityEngine.Object>();
                        goto Label_0195;

                    default:
                        goto Label_01C1;
                }
                if (Application.isEditor)
                {
                    this.<ab>__3 = this.<>f__this.loadAssetBundleSync("prefabs");
                    if (this.<ab>__3 != null)
                    {
                        this.<>f__this.Prefabs = this.<ab>__3.LoadAllAssets<UnityEngine.Object>();
                    }
                    else
                    {
                        this.<assetBundleMissing>__0 = true;
                    }
                }
                else
                {
                    this.<www>__4 = this.<>f__this.loadAssetBundleAsync("prefabs");
                    this.$current = this.<www>__4;
                    this.$PC = 2;
                    goto Label_01C3;
                }
            Label_0195:
                this.<>f__this.AssetBundlesAvailable = !this.<assetBundleMissing>__0;
                this.<>f__this.Loaded = true;
                goto Label_01C1;
                this.$PC = -1;
            Label_01C1:
                return false;
            Label_01C3:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

