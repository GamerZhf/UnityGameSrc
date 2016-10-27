namespace Pathfinding.Examples
{
    using Pathfinding;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_procedural_world.php")]
    public class ProceduralWorld : MonoBehaviour
    {
        public ProceduralPrefab[] prefabs;
        public int range = 1;
        public bool staticBatching;
        public int subTiles = 20;
        public Transform target;
        private Queue<IEnumerator> tileGenerationQueue = new Queue<IEnumerator>();
        private Dictionary<Int2, ProceduralTile> tiles = new Dictionary<Int2, ProceduralTile>();
        public float tileSize = 100f;

        [DebuggerHidden]
        private IEnumerator GenerateTiles()
        {
            <GenerateTiles>c__IteratorF rf = new <GenerateTiles>c__IteratorF();
            rf.<>f__this = this;
            return rf;
        }

        private void Start()
        {
            this.Update();
            AstarPath.active.Scan();
            base.StartCoroutine(this.GenerateTiles());
        }

        private void Update()
        {
            Int2 num = new Int2(Mathf.RoundToInt((this.target.position.x - (this.tileSize * 0.5f)) / this.tileSize), Mathf.RoundToInt((this.target.position.z - (this.tileSize * 0.5f)) / this.tileSize));
            this.range = (this.range >= 1) ? this.range : 1;
            bool flag = true;
            while (flag)
            {
                flag = false;
                foreach (KeyValuePair<Int2, ProceduralTile> pair in this.tiles)
                {
                    if ((Mathf.Abs((int) (pair.Key.x - num.x)) > this.range) || (Mathf.Abs((int) (pair.Key.y - num.y)) > this.range))
                    {
                        pair.Value.Destroy();
                        this.tiles.Remove(pair.Key);
                        flag = true;
                        continue;
                    }
                }
            }
            for (int i = num.x - this.range; i <= (num.x + this.range); i++)
            {
                for (int k = num.y - this.range; k <= (num.y + this.range); k++)
                {
                    if (!this.tiles.ContainsKey(new Int2(i, k)))
                    {
                        ProceduralTile tile = new ProceduralTile(this, i, k);
                        IEnumerator item = tile.Generate();
                        item.MoveNext();
                        this.tileGenerationQueue.Enqueue(item);
                        this.tiles.Add(new Int2(i, k), tile);
                    }
                }
            }
            for (int j = num.x - 1; j <= (num.x + 1); j++)
            {
                for (int m = num.y - 1; m <= (num.y + 1); m++)
                {
                    this.tiles[new Int2(j, m)].ForceFinish();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GenerateTiles>c__IteratorF : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ProceduralWorld <>f__this;
            internal IEnumerator <generator>__0;

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
                    case 2:
                        if (this.<>f__this.tileGenerationQueue.Count > 0)
                        {
                            this.<generator>__0 = this.<>f__this.tileGenerationQueue.Dequeue();
                            this.$current = this.<>f__this.StartCoroutine(this.<generator>__0);
                            this.$PC = 1;
                            goto Label_0095;
                        }
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0093;
                }
                this.$current = null;
                this.$PC = 2;
                goto Label_0095;
                this.$PC = -1;
            Label_0093:
                return false;
            Label_0095:
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

        [Serializable]
        public class ProceduralPrefab
        {
            public float density;
            public float perlin;
            public Vector2 perlinOffset = Vector2.zero;
            public float perlinPower = 1f;
            public float perlinScale = 1f;
            public GameObject prefab;
            public float random = 1f;
            public bool singleFixed;
        }

        private class ProceduralTile
        {
            [CompilerGenerated]
            private bool <destroyed>k__BackingField;
            private IEnumerator ie;
            private System.Random rnd;
            private Transform root;
            private bool staticBatching;
            private ProceduralWorld world;
            private int x;
            private int z;

            public ProceduralTile(ProceduralWorld world, int x, int z)
            {
                this.x = x;
                this.z = z;
                this.world = world;
                this.rnd = new System.Random((x * 0x2717) ^ (z * 0x8ca7));
            }

            public void Destroy()
            {
                if (this.root != null)
                {
                    UnityEngine.Debug.Log(string.Concat(new object[] { "Destroying tile ", this.x, ", ", this.z }));
                    UnityEngine.Object.Destroy(this.root.gameObject);
                    this.root = null;
                }
                this.ie = null;
            }

            public void ForceFinish()
            {
                while (((this.ie != null) && (this.root != null)) && this.ie.MoveNext())
                {
                }
                this.ie = null;
            }

            [DebuggerHidden]
            public IEnumerator Generate()
            {
                <Generate>c__Iterator10 iterator = new <Generate>c__Iterator10();
                iterator.<>f__this = this;
                return iterator;
            }

            [DebuggerHidden]
            private IEnumerator InternalGenerate()
            {
                <InternalGenerate>c__Iterator11 iterator = new <InternalGenerate>c__Iterator11();
                iterator.<>f__this = this;
                return iterator;
            }

            private Vector3 RandomInside()
            {
                Vector3 vector = new Vector3();
                vector.x = (this.x + ((float) this.rnd.NextDouble())) * this.world.tileSize;
                vector.z = (this.z + ((float) this.rnd.NextDouble())) * this.world.tileSize;
                return vector;
            }

            private Vector3 RandomInside(float px, float pz)
            {
                Vector3 vector = new Vector3();
                vector.x = (px + (((float) this.rnd.NextDouble()) / ((float) this.world.subTiles))) * this.world.tileSize;
                vector.z = (pz + (((float) this.rnd.NextDouble()) / ((float) this.world.subTiles))) * this.world.tileSize;
                return vector;
            }

            private Quaternion RandomYRot()
            {
                return Quaternion.Euler(360f * ((float) this.rnd.NextDouble()), 0f, 360f * ((float) this.rnd.NextDouble()));
            }

            public bool destroyed
            {
                [CompilerGenerated]
                get
                {
                    return this.<destroyed>k__BackingField;
                }
                [CompilerGenerated]
                private set
                {
                    this.<destroyed>k__BackingField = value;
                }
            }

            [CompilerGenerated]
            private sealed class <Generate>c__Iterator10 : IEnumerator, IDisposable, IEnumerator<object>
            {
                internal object $current;
                internal int $PC;
                internal ProceduralWorld.ProceduralTile <>f__this;
                internal GameObject <rt>__0;

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
                        {
                            this.<>f__this.ie = this.<>f__this.InternalGenerate();
                            object[] objArray1 = new object[] { "Tile ", this.<>f__this.x, " ", this.<>f__this.z };
                            this.<rt>__0 = new GameObject(string.Concat(objArray1));
                            this.<>f__this.root = this.<rt>__0.transform;
                            break;
                        }
                        case 1:
                            break;

                        default:
                            goto Label_010E;
                    }
                    if (((this.<>f__this.ie != null) && (this.<>f__this.root != null)) && this.<>f__this.ie.MoveNext())
                    {
                        this.$current = this.<>f__this.ie.Current;
                        this.$PC = 1;
                        return true;
                    }
                    this.<>f__this.ie = null;
                    this.$PC = -1;
                Label_010E:
                    return false;
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

            [CompilerGenerated]
            private sealed class <InternalGenerate>c__Iterator11 : IEnumerator, IDisposable, IEnumerator<object>
            {
                internal object $current;
                internal int $PC;
                internal ProceduralWorld.ProceduralTile <>f__this;
                internal int <count>__16;
                internal int <counter>__0;
                internal float <density>__14;
                internal float[,] <ditherMap>__1;
                internal float <fcount>__15;
                internal int <i>__2;
                internal int <j>__17;
                internal GameObject <ob>__19;
                internal GameObject <ob>__5;
                internal Vector3 <p>__18;
                internal Vector3 <p>__4;
                internal float <perl>__13;
                internal ProceduralWorld.ProceduralPrefab <pref>__3;
                internal float <px>__11;
                internal float <pz>__12;
                internal float <subSize>__6;
                internal int <sx>__7;
                internal int <sx>__9;
                internal int <sz>__10;
                internal int <sz>__8;

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
                            UnityEngine.Debug.Log(string.Concat(new object[] { "Generating tile ", this.<>f__this.x, ", ", this.<>f__this.z }));
                            this.<counter>__0 = 0;
                            this.<ditherMap>__1 = new float[this.<>f__this.world.subTiles + 2, this.<>f__this.world.subTiles + 2];
                            this.<i>__2 = 0;
                            while (this.<i>__2 < this.<>f__this.world.prefabs.Length)
                            {
                                this.<pref>__3 = this.<>f__this.world.prefabs[this.<i>__2];
                                if (this.<pref>__3.singleFixed)
                                {
                                    this.<p>__4 = new Vector3((this.<>f__this.x + 0.5f) * this.<>f__this.world.tileSize, 0f, (this.<>f__this.z + 0.5f) * this.<>f__this.world.tileSize);
                                    this.<ob>__5 = UnityEngine.Object.Instantiate(this.<pref>__3.prefab, this.<p>__4, Quaternion.identity) as GameObject;
                                    this.<ob>__5.transform.parent = this.<>f__this.root;
                                }
                                else
                                {
                                    this.<subSize>__6 = this.<>f__this.world.tileSize / ((float) this.<>f__this.world.subTiles);
                                    this.<sx>__7 = 0;
                                    while (this.<sx>__7 < this.<>f__this.world.subTiles)
                                    {
                                        this.<sz>__8 = 0;
                                        while (this.<sz>__8 < this.<>f__this.world.subTiles)
                                        {
                                            this.<ditherMap>__1[this.<sx>__7 + 1, this.<sz>__8 + 1] = 0f;
                                            this.<sz>__8++;
                                        }
                                        this.<sx>__7++;
                                    }
                                    this.<sx>__9 = 0;
                                    while (this.<sx>__9 < this.<>f__this.world.subTiles)
                                    {
                                        this.<sz>__10 = 0;
                                        while (this.<sz>__10 < this.<>f__this.world.subTiles)
                                        {
                                            this.<px>__11 = this.<>f__this.x + (((float) this.<sx>__9) / ((float) this.<>f__this.world.subTiles));
                                            this.<pz>__12 = this.<>f__this.z + (((float) this.<sz>__10) / ((float) this.<>f__this.world.subTiles));
                                            this.<perl>__13 = Mathf.Pow(Mathf.PerlinNoise((this.<px>__11 + this.<pref>__3.perlinOffset.x) * this.<pref>__3.perlinScale, (this.<pz>__12 + this.<pref>__3.perlinOffset.y) * this.<pref>__3.perlinScale), this.<pref>__3.perlinPower);
                                            this.<density>__14 = (this.<pref>__3.density * Mathf.Lerp(1f, this.<perl>__13, this.<pref>__3.perlin)) * Mathf.Lerp(1f, (float) this.<>f__this.rnd.NextDouble(), this.<pref>__3.random);
                                            this.<fcount>__15 = ((this.<subSize>__6 * this.<subSize>__6) * this.<density>__14) + this.<ditherMap>__1[this.<sx>__9 + 1, this.<sz>__10 + 1];
                                            this.<count>__16 = Mathf.RoundToInt(this.<fcount>__15);
                                            float single1 = this.<ditherMap>__1[(this.<sx>__9 + 1) + 1, this.<sz>__10 + 1];
                                            single1[0] += 0.4375f * (this.<fcount>__15 - this.<count>__16);
                                            float single2 = this.<ditherMap>__1[(this.<sx>__9 + 1) - 1, (this.<sz>__10 + 1) + 1];
                                            single2[0] += 0.1875f * (this.<fcount>__15 - this.<count>__16);
                                            float single3 = this.<ditherMap>__1[this.<sx>__9 + 1, (this.<sz>__10 + 1) + 1];
                                            single3[0] += 0.3125f * (this.<fcount>__15 - this.<count>__16);
                                            float single4 = this.<ditherMap>__1[(this.<sx>__9 + 1) + 1, (this.<sz>__10 + 1) + 1];
                                            single4[0] += 0.0625f * (this.<fcount>__15 - this.<count>__16);
                                            this.<j>__17 = 0;
                                            while (this.<j>__17 < this.<count>__16)
                                            {
                                                this.<p>__18 = this.<>f__this.RandomInside(this.<px>__11, this.<pz>__12);
                                                this.<ob>__19 = UnityEngine.Object.Instantiate(this.<pref>__3.prefab, this.<p>__18, this.<>f__this.RandomYRot()) as GameObject;
                                                this.<ob>__19.transform.parent = this.<>f__this.root;
                                                this.<counter>__0++;
                                                if ((this.<counter>__0 % 2) == 0)
                                                {
                                                    this.$current = null;
                                                    this.$PC = 1;
                                                    goto Label_0614;
                                                }
                                            Label_050E:
                                                this.<j>__17++;
                                            }
                                            this.<sz>__10++;
                                        }
                                        this.<sx>__9++;
                                    }
                                }
                                this.<i>__2++;
                            }
                            this.<ditherMap>__1 = null;
                            this.$current = null;
                            this.$PC = 2;
                            goto Label_0614;

                        case 1:
                            goto Label_050E;

                        case 2:
                            this.$current = null;
                            this.$PC = 3;
                            goto Label_0614;

                        case 3:
                            if (Application.HasProLicense() && this.<>f__this.world.staticBatching)
                            {
                                StaticBatchingUtility.Combine(this.<>f__this.root.gameObject);
                            }
                            this.$PC = -1;
                            break;
                    }
                    return false;
                Label_0614:
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
}

