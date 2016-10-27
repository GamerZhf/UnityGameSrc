namespace PlayerView
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class ResourceGainVisualizer : MonoBehaviour
    {
        [CompilerGenerated]
        private RectTransform <RootTm>k__BackingField;
        private List<ResourceAnimation> m_activeResourceAnimations = new List<ResourceAnimation>();
        private Camera m_canvasCam;
        private IObjectPool<ResourceAnimation> m_resourceAnimationPool;
        public const int MAX_CONCURRENT_ANIMATION_OBJECTS = 0x10;
        public const int MAX_SPRITES_PER_ANIMATION_OBJECT = 0x30;
        public const float REFERENCE_SCREEN_WIDTH_PIXELS = 1242f;

        public ResourceAnimation animate(List<Sprite> sprites, Sprite borders, Color color, bool grayscale, Vector2 sourceScreenPos, Vector2 targetScreenPos, double resourceAmount, int spriteAmount, float durationMin, float durationMax, float localScaleMin, float localScaleMax, float maxOffsetDistance, bool fromMenu, Action<int, int, double, bool> endCallback, [Optional, DefaultParameterValue(-1)] int siblingIndex)
        {
            spriteAmount = Mathf.Clamp(spriteAmount, 0, 0x30);
            ResourceAnimation item = this.m_resourceAnimationPool.getObject();
            for (int i = 0; i < spriteAmount; i++)
            {
                Vector3 vector;
                ResourceGainImage image = Binder.ResourceGainImagePool.getObject();
                image.transform.SetParent(this.RootTm);
                if (siblingIndex != -1)
                {
                    image.transform.SetSiblingIndex(siblingIndex);
                }
                else
                {
                    image.transform.SetAsLastSibling();
                }
                image.Image.sprite = LangUtil.GetRandomValueFromList<Sprite>(sprites);
                image.Borders.sprite = borders;
                image.Borders.enabled = borders != null;
                image.Image.color = color;
                image.Image.material = !grayscale ? null : Binder.DisabledUiMaterial;
                if (image.Borders != null)
                {
                    image.Borders.color = image.Image.color;
                    image.Borders.material = image.Image.material;
                }
                image.gameObject.SetActive(true);
                item.ImageTm[i] = image.transform;
                item.Image[i] = image;
                item.ImageTm[i].localScale = (Vector3) (Vector3.one * UnityEngine.Random.Range(localScaleMin, localScaleMax));
                item.SourceScreenPos[i] = sourceScreenPos;
                item.TargetScreenPos[i] = targetScreenPos;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(this.RootTm, sourceScreenPos, this.m_canvasCam, out vector);
                image.transform.position = vector;
                image.transform.localRotation = (spriteAmount <= 1) ? Quaternion.identity : Quaternion.Euler(0f, 0f, UnityEngine.Random.Range((float) -10f, (float) 10f));
                Vector2 v = sourceScreenPos - targetScreenPos;
                Vector2 zero = Vector2.zero;
                float num2 = (UnityEngine.Random.Range(0, 2) != 0) ? 1f : -1f;
                float max = maxOffsetDistance * (((float) Screen.width) / 1242f);
                zero = (Vector2) (Vector2Extensions.Rotate(v, num2 * 90f).normalized * UnityEngine.Random.Range(0f, max));
                zero.y = UnityEngine.Random.Range(zero.x, -zero.x);
                item.OffsetScreen[i] = zero;
                item.Lifetime[i] = UnityEngine.Random.Range(durationMin, durationMax);
                item.ElapsedTime[i] = 0f;
            }
            item.NumSprites = spriteAmount;
            item.TranslationEasingFunction = Easing.Function.IN_CUBIC;
            item.EndCallback = endCallback;
            item.FromMenu = fromMenu;
            MathUtil.DistributeValuesIntoChunksDouble(resourceAmount, spriteAmount, ref item.ResourceChunks);
            item.ResourceChunkIndex = 0;
            this.m_activeResourceAnimations.Add(item);
            return item;
        }

        public void emptyAllActiveAnimationResourceChunks()
        {
            for (int i = 0; i < this.m_activeResourceAnimations.Count; i++)
            {
                ResourceAnimation animation = this.m_activeResourceAnimations[i];
                for (int j = 0; j < animation.ResourceChunks.Count; j++)
                {
                    animation.ResourceChunks[j] = 0.0;
                }
            }
        }

        public void initialize(Camera canvasCam)
        {
            this.m_canvasCam = canvasCam;
            this.RootTm = base.GetComponent<RectTransform>();
            this.m_resourceAnimationPool = new ObjectPool<ResourceAnimation>(new ResourceAnimationProvider(), 0x10, ObjectPoolExpansionMethod.DOUBLE, true);
        }

        [ContextMenu("test()")]
        private void test()
        {
        }

        protected void Update()
        {
            for (int i = this.m_activeResourceAnimations.Count - 1; i >= 0; i--)
            {
                ResourceAnimation item = this.m_activeResourceAnimations[i];
                bool flag = true;
                int num2 = 0;
                double num3 = 0.0;
                for (int j = 0; j < item.NumSprites; j++)
                {
                    if (!item.CleanupFlag[j])
                    {
                        Vector3 vector2;
                        item.ElapsedTime[j] += Time.unscaledDeltaTime;
                        float v = Mathf.Clamp01(item.ElapsedTime[j] / item.Lifetime[j]);
                        float t = Easing.Apply(v, item.TranslationEasingFunction);
                        Vector2 screenPoint = Vector2.Lerp(item.SourceScreenPos[j], item.TargetScreenPos[j], t) + ((Vector2) (item.OffsetScreen[j] * Mathf.Sin(v * 3.141593f)));
                        RectTransformUtility.ScreenPointToWorldPointInRectangle(this.RootTm, screenPoint, this.m_canvasCam, out vector2);
                        item.ImageTm[j].position = vector2;
                        if (item.ElapsedTime[j] > item.Lifetime[j])
                        {
                            item.CleanupFlag[j] = true;
                            Binder.ResourceGainImagePool.returnObject(item.Image[j]);
                            item.Image[j] = null;
                            num2++;
                            num3 += item.ResourceChunks[item.ResourceChunkIndex++];
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                }
                if ((num2 > 0) && (item.EndCallback != null))
                {
                    item.EndCallback(num2, item.NumSprites, num3, item.FromMenu);
                }
                if (flag)
                {
                    this.m_activeResourceAnimations.Remove(item);
                    this.m_resourceAnimationPool.returnObject(item);
                }
            }
        }

        public bool HasActiveAnimations
        {
            get
            {
                return (this.m_activeResourceAnimations.Count > 0);
            }
        }

        public int NumActiveAnimations
        {
            get
            {
                return this.m_activeResourceAnimations.Count;
            }
        }

        public RectTransform RootTm
        {
            [CompilerGenerated]
            get
            {
                return this.<RootTm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RootTm>k__BackingField = value;
            }
        }

        public class ResourceAnimation : IPoolable
        {
            public bool[] CleanupFlag = new bool[0x30];
            public float[] ElapsedTime = new float[0x30];
            public Action<int, int, double, bool> EndCallback;
            public bool FromMenu;
            public ResourceGainImage[] Image = new ResourceGainImage[0x30];
            public Transform[] ImageTm = new Transform[0x30];
            public float[] Lifetime = new float[0x30];
            public int NumSprites;
            public Vector2[] OffsetScreen = new Vector2[0x30];
            public int ResourceChunkIndex;
            public List<double> ResourceChunks = new List<double>();
            public Vector2[] SourceScreenPos = new Vector2[0x30];
            public Vector2[] TargetScreenPos = new Vector2[0x30];
            public Easing.Function TranslationEasingFunction;

            public void cleanUpForReuse()
            {
                this.ResourceChunks.Clear();
                this.ResourceChunkIndex = 0;
                this.NumSprites = 0;
                for (int i = 0; i < 0x30; i++)
                {
                    this.ImageTm[i] = null;
                    this.CleanupFlag[i] = false;
                }
            }
        }

        private class ResourceAnimationProvider : IInstanceProvider<ResourceGainVisualizer.ResourceAnimation>
        {
            public ResourceGainVisualizer.ResourceAnimation instantiate()
            {
                return new ResourceGainVisualizer.ResourceAnimation();
            }

            public void onDestroy(ResourceGainVisualizer.ResourceAnimation obj)
            {
            }

            public void onReset()
            {
            }

            public void onReturn(ResourceGainVisualizer.ResourceAnimation obj)
            {
                for (int i = 0; i < 0x30; i++)
                {
                }
            }
        }
    }
}

