namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class DungeonDropView : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private GameLogic.Reward <Reward>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        [CompilerGenerated]
        private TransformAnimation <TransformAnimation>k__BackingField;
        public SpriteRenderer Chest2dRenderer;
        public SpriteRenderer ChestBeamFrontRenderer;
        public SpriteRenderer ChestBeamRenderer;
        public ParticleSystem ChestGlowParticleSystem;
        public MeshRenderer ChestShadow;
        public ParticleSystem CoinFxParticleSystem;
        public SpriteRenderer CoinGlowRenderer;
        public List<SpriteRenderer> CoinRenderers = new List<SpriteRenderer>();
        public ParticleSystem DiamondFxParticleSystem;
        public SpriteRenderer DiamondGlowRenderer;
        public List<SpriteRenderer> DiamondRenderers = new List<SpriteRenderer>();
        private const float DROP_APEX_HEIGHT = 3f;
        private const float DROP_DURATION = 0.4f;
        private List<RendererData> m_coinRenderers = new List<RendererData>();
        private List<RendererData> m_diamondRenderers = new List<RendererData>();
        private Coroutine m_dropRoutine;
        private int m_originalChestSpriteSortingOrder;
        private List<RendererData> m_xpRenderers = new List<RendererData>();
        public ParticleSystem XpFxParticleSystem;
        public SpriteRenderer XpGlowRenderer;
        public List<SpriteRenderer> XpRenderers = new List<SpriteRenderer>();

        protected void Awake()
        {
            this.Tm = base.transform;
            this.TransformAnimation = GameObjectExtensions.AddOrGetComponent<TransformAnimation>(base.gameObject);
            for (int i = 0; i < this.CoinRenderers.Count; i++)
            {
                RendererData item = new RendererData();
                item.Transform = this.CoinRenderers[i].transform;
                item.SpriterRenderer = this.CoinRenderers[i];
                item.TransformAnimation = GameObjectExtensions.AddOrGetComponent<TransformAnimation>(item.SpriterRenderer.gameObject);
                item.LocalScale = item.Transform.localScale;
                item.Billboard = false;
                this.m_coinRenderers.Add(item);
            }
            for (int j = 0; j < this.DiamondRenderers.Count; j++)
            {
                RendererData data2 = new RendererData();
                data2.Transform = this.DiamondRenderers[j].transform;
                data2.SpriterRenderer = this.DiamondRenderers[j];
                data2.TransformAnimation = GameObjectExtensions.AddOrGetComponent<TransformAnimation>(data2.SpriterRenderer.gameObject);
                data2.LocalScale = data2.Transform.localScale;
                data2.Billboard = false;
                this.m_diamondRenderers.Add(data2);
            }
            for (int k = 0; k < this.XpRenderers.Count; k++)
            {
                RendererData data3 = new RendererData();
                data3.Transform = this.XpRenderers[k].transform;
                data3.SpriterRenderer = this.XpRenderers[k];
                data3.TransformAnimation = GameObjectExtensions.AddOrGetComponent<TransformAnimation>(data3.SpriterRenderer.gameObject);
                data3.LocalScale = data3.Transform.localScale;
                data3.Billboard = true;
                this.m_xpRenderers.Add(data3);
            }
            this.m_originalChestSpriteSortingOrder = this.Chest2dRenderer.sortingOrder;
        }

        [DebuggerHidden]
        private IEnumerator chestDropRoutine([Optional, DefaultParameterValue(null)] Sprite overrideSprite)
        {
            <chestDropRoutine>c__IteratorF3 rf = new <chestDropRoutine>c__IteratorF3();
            rf.overrideSprite = overrideSprite;
            rf.<$>overrideSprite = overrideSprite;
            rf.<>f__this = this;
            return rf;
        }

        public void cleanUpForReuse()
        {
            UnityUtils.StopCoroutine(this, ref this.m_dropRoutine);
            this.Tm.localPosition = Vector3.zero;
            this.setChestVisualsEnabled(false);
            this.setCoinVisualsEnabled(false);
            this.setDiamondVisualsEnabled(false);
            this.setXpVisualsEnabled(false);
        }

        [DebuggerHidden]
        private IEnumerator coinDropRoutine()
        {
            <coinDropRoutine>c__IteratorF0 rf = new <coinDropRoutine>c__IteratorF0();
            rf.<>f__this = this;
            return rf;
        }

        [DebuggerHidden]
        private IEnumerator diamondDropRoutine()
        {
            <diamondDropRoutine>c__IteratorF1 rf = new <diamondDropRoutine>c__IteratorF1();
            rf.<>f__this = this;
            return rf;
        }

        private Vector3 getChestDropHudPosition(ChestType chestType)
        {
            Vector3 position;
            if (ConfigMeta.IsMysteryChest(chestType))
            {
                position = PlayerView.Binder.DungeonHud.TaskPanel.getTaskPanelItemWorldPosition(TaskPanelItemType.NormalChestDrop);
            }
            else if (ConfigMeta.IsRetirementChest(chestType))
            {
                position = PlayerView.Binder.DungeonHud.AdventureButton.Tm.position;
            }
            else
            {
                position = PlayerView.Binder.DungeonHud.TaskPanel.getTaskPanelItemWorldPosition(TaskPanelItemType.BossChestDrop);
            }
            position.z = 60f;
            position.y -= 60f;
            return position;
        }

        public void initialize(GameLogic.Reward reward)
        {
            this.Reward = reward;
            this.setDiamondVisualsEnabled(false);
            this.setCoinVisualsEnabled(false);
            this.setChestVisualsEnabled(false);
            this.setXpVisualsEnabled(false);
            this.Tm.localScale = Vector3.one;
        }

        [DebuggerHidden]
        private IEnumerator resourceGlowRoutine(SpriteRenderer glowRenderer, Color mainColor)
        {
            <resourceGlowRoutine>c__IteratorEF ref2 = new <resourceGlowRoutine>c__IteratorEF();
            ref2.mainColor = mainColor;
            ref2.glowRenderer = glowRenderer;
            ref2.<$>mainColor = mainColor;
            ref2.<$>glowRenderer = glowRenderer;
            return ref2;
        }

        [DebuggerHidden]
        private IEnumerator resourceUpAndDownAnimationRoutine(List<RendererData> renderers)
        {
            <resourceUpAndDownAnimationRoutine>c__IteratorEE ree = new <resourceUpAndDownAnimationRoutine>c__IteratorEE();
            ree.renderers = renderers;
            ree.<$>renderers = renderers;
            return ree;
        }

        private void setChestVisualsEnabled(bool enabled)
        {
            this.Chest2dRenderer.gameObject.SetActive(true);
            this.Chest2dRenderer.enabled = enabled;
            this.Chest2dRenderer.sortingOrder = this.m_originalChestSpriteSortingOrder;
            this.Chest2dRenderer.gameObject.layer = Layers.DEFAULT;
            this.ChestShadow.gameObject.SetActive(true);
            this.ChestShadow.enabled = enabled;
            this.ChestGlowParticleSystem.gameObject.SetActive(true);
            this.ChestGlowParticleSystem.Stop();
            this.ChestGlowParticleSystem.Clear();
            this.ChestBeamRenderer.gameObject.SetActive(true);
            this.ChestBeamRenderer.enabled = enabled;
            this.ChestBeamFrontRenderer.gameObject.SetActive(true);
            this.ChestBeamFrontRenderer.enabled = enabled;
        }

        private void setCoinVisualsEnabled(bool enabled)
        {
            for (int i = 0; i < this.CoinRenderers.Count; i++)
            {
                this.CoinRenderers[i].gameObject.SetActive(true);
                this.CoinRenderers[i].enabled = enabled;
                this.CoinRenderers[i].transform.localPosition = Vector3.zero;
                this.CoinRenderers[i].gameObject.layer = Layers.DEFAULT;
            }
            this.CoinGlowRenderer.gameObject.SetActive(true);
            this.CoinGlowRenderer.enabled = enabled;
            this.CoinFxParticleSystem.gameObject.SetActive(true);
            this.CoinFxParticleSystem.Stop();
            this.CoinFxParticleSystem.Clear();
        }

        private void setDiamondVisualsEnabled(bool enabled)
        {
            for (int i = 0; i < this.DiamondRenderers.Count; i++)
            {
                this.DiamondRenderers[i].gameObject.SetActive(true);
                this.DiamondRenderers[i].enabled = enabled;
                this.DiamondRenderers[i].transform.localPosition = Vector3.zero;
            }
            this.DiamondGlowRenderer.gameObject.SetActive(true);
            this.DiamondGlowRenderer.enabled = enabled;
            this.DiamondFxParticleSystem.gameObject.SetActive(true);
            this.DiamondFxParticleSystem.Stop();
            this.DiamondFxParticleSystem.Clear();
        }

        private void setXpVisualsEnabled(bool enabled)
        {
            for (int i = 0; i < this.XpRenderers.Count; i++)
            {
                this.XpRenderers[i].gameObject.SetActive(true);
                this.XpRenderers[i].enabled = enabled;
                this.XpRenderers[i].transform.localPosition = Vector3.zero;
            }
            this.XpGlowRenderer.gameObject.SetActive(true);
            this.XpGlowRenderer.enabled = enabled;
            this.XpFxParticleSystem.gameObject.SetActive(true);
            this.XpFxParticleSystem.Stop();
            this.XpFxParticleSystem.Clear();
        }

        public void startChestDropSequence([Optional, DefaultParameterValue(null)] Sprite overrideSprite)
        {
            UnityUtils.StopCoroutine(this, ref this.m_dropRoutine);
            this.m_dropRoutine = UnityUtils.StartCoroutine(this, this.chestDropRoutine(overrideSprite));
        }

        public void startCoinDropSequence()
        {
            UnityUtils.StopCoroutine(this, ref this.m_dropRoutine);
            this.m_dropRoutine = UnityUtils.StartCoroutine(this, this.coinDropRoutine());
        }

        public void startDiamondDropSequence()
        {
            UnityUtils.StopCoroutine(this, ref this.m_dropRoutine);
            this.m_dropRoutine = UnityUtils.StartCoroutine(this, this.diamondDropRoutine());
        }

        public void startXpDropSequence(Player player, double amount, Vector3 worldPt, float delay)
        {
            UnityUtils.StopCoroutine(this, ref this.m_dropRoutine);
            this.m_dropRoutine = UnityUtils.StartCoroutine(this, this.xpDropRoutine(amount, delay));
        }

        [DebuggerHidden]
        private IEnumerator xpDropRoutine(double amount, float delay)
        {
            <xpDropRoutine>c__IteratorF2 rf = new <xpDropRoutine>c__IteratorF2();
            rf.delay = delay;
            rf.amount = amount;
            rf.<$>delay = delay;
            rf.<$>amount = amount;
            rf.<>f__this = this;
            return rf;
        }

        public bool DropSequenceComplete
        {
            get
            {
                return !UnityUtils.CoroutineRunning(ref this.m_dropRoutine);
            }
        }

        public GameLogic.Reward Reward
        {
            [CompilerGenerated]
            get
            {
                return this.<Reward>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Reward>k__BackingField = value;
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

        public TransformAnimation TransformAnimation
        {
            [CompilerGenerated]
            get
            {
                return this.<TransformAnimation>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<TransformAnimation>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <chestDropRoutine>c__IteratorF3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Sprite <$>overrideSprite;
            internal DungeonDropView <>f__this;
            internal Color <c>__6;
            internal Color <c>__7;
            internal Vector2 <currentScreenPos>__8;
            internal Color <mainColor>__1;
            internal Vector3 <newWorldPos>__9;
            internal Vector3 <origLocalPos>__2;
            internal Vector3 <startLocalScale>__10;
            internal float <stayDuration>__0;
            internal Vector3 <targetLocalScale>__11;
            internal Vector3 <targetWorldPos>__12;
            internal ManualTimer <timer>__5;
            internal TransformAnimationTask <tt>__3;
            internal TransformAnimationTask <tt2>__4;
            internal Sprite overrideSprite;

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
                        this.<stayDuration>__0 = ConfigUi.CHEST_DROP_AUTO_COLLECT_WAIT;
                        this.<>f__this.setChestVisualsEnabled(true);
                        this.<>f__this.ChestShadow.enabled = false;
                        this.<>f__this.ChestBeamRenderer.enabled = false;
                        this.<>f__this.ChestBeamFrontRenderer.enabled = false;
                        PlayerView.Binder.DungeonHud.onDungeonDropViewChestDropStart(this.<>f__this.Reward);
                        if (!ConfigMeta.IsMysteryChest(this.<>f__this.Reward.ChestType))
                        {
                            this.<mainColor>__1 = ConfigUi.RARITY_SPRITE_COLORS[0];
                            break;
                        }
                        this.<mainColor>__1 = ConfigUi.RARITY_SPRITE_COLORS[2];
                        break;

                    case 1:
                        goto Label_033A;

                    case 2:
                        goto Label_0435;

                    case 3:
                        goto Label_047F;

                    case 4:
                        goto Label_0577;

                    case 5:
                        goto Label_0751;

                    case 6:
                        goto Label_07F9;

                    case 7:
                        goto Label_0886;

                    default:
                        goto Label_08B3;
                }
                this.<>f__this.ChestBeamRenderer.color = new Color(this.<mainColor>__1.r, this.<mainColor>__1.g, this.<mainColor>__1.b, 0f);
                this.<>f__this.ChestBeamFrontRenderer.color = new Color(1f, 1f, 1f, 0f);
                this.<>f__this.ChestGlowParticleSystem.transform.localScale = (Vector3) (Vector3.one * 1.5f);
                this.<>f__this.ChestGlowParticleSystem.startColor = new Color(this.<mainColor>__1.r, this.<mainColor>__1.g, this.<mainColor>__1.b, 0f);
                if (this.overrideSprite != null)
                {
                    this.<>f__this.Chest2dRenderer.sprite = this.overrideSprite;
                }
                else
                {
                    this.<>f__this.Chest2dRenderer.sprite = PlayerView.Binder.SpriteResources.getSprite(ConfigUi.CHEST_BLUEPRINTS[this.<>f__this.Reward.getVisualChestType()].DropSprite);
                }
                this.<>f__this.Chest2dRenderer.transform.localRotation = PlayerView.Binder.RoomView.RoomCamera.Transform.rotation;
                this.<origLocalPos>__2 = this.<>f__this.Tm.position;
                this.<tt>__3 = new TransformAnimationTask(this.<>f__this.Tm, 0.2f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__3.translate(this.<origLocalPos>__2 + ((Vector3) (Vector3.up * 3f)), false, Easing.Function.OUT_CUBIC);
                this.<tt>__3.scale(Vector3.one, true, Easing.Function.LINEAR);
                this.<>f__this.TransformAnimation.addTask(this.<tt>__3);
                this.<tt2>__4 = new TransformAnimationTask(this.<>f__this.Tm, 0.2f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt2>__4.translate(this.<origLocalPos>__2, false, Easing.Function.IN_CUBIC);
                this.<>f__this.TransformAnimation.addTask(this.<tt2>__4);
            Label_033A:
                while (this.<>f__this.TransformAnimation.HasTasks)
                {
                    if (this.<tt2>__4.Progress > 0.75f)
                    {
                        this.<>f__this.ChestShadow.enabled = true;
                    }
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_08B5;
                }
                this.<>f__this.ChestGlowParticleSystem.Play();
                this.<>f__this.ChestGlowParticleSystem.startColor = this.<mainColor>__1;
                this.<timer>__5 = new ManualTimer();
                this.<timer>__5.set(0.1f);
            Label_0435:
                while (!this.<timer>__5.Idle)
                {
                    this.<c>__6 = new Color(this.<mainColor>__1.r, this.<mainColor>__1.g, this.<mainColor>__1.b, this.<timer>__5.normalizedProgress());
                    this.<>f__this.ChestBeamRenderer.color = this.<c>__6;
                    this.<>f__this.ChestBeamFrontRenderer.color = new Color(1f, 1f, 1f, this.<c>__6.a);
                    this.<timer>__5.tick(Time.deltaTime);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_08B5;
                }
                this.<timer>__5.set(this.<stayDuration>__0);
            Label_047F:
                while (!this.<timer>__5.Idle)
                {
                    this.<timer>__5.tick(Time.deltaTime);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_08B5;
                }
                this.<timer>__5.set(0.1f);
            Label_0577:
                while (!this.<timer>__5.Idle)
                {
                    this.<c>__7 = new Color(this.<mainColor>__1.r, this.<mainColor>__1.g, this.<mainColor>__1.b, 1f - this.<timer>__5.normalizedProgress());
                    this.<c>__7.a *= 0.8f;
                    this.<>f__this.ChestGlowParticleSystem.startColor = this.<c>__7;
                    this.<>f__this.ChestBeamRenderer.color = this.<c>__7;
                    this.<>f__this.ChestBeamFrontRenderer.color = new Color(1f, 1f, 1f, this.<c>__7.a);
                    this.<timer>__5.tick(Time.deltaTime);
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_08B5;
                }
                this.<>f__this.ChestGlowParticleSystem.Stop();
                this.<>f__this.ChestBeamRenderer.enabled = false;
                this.<>f__this.ChestBeamFrontRenderer.enabled = false;
                this.<>f__this.Chest2dRenderer.transform.localRotation = Quaternion.identity;
                this.<>f__this.Tm.localRotation = Quaternion.identity;
                this.<>f__this.Chest2dRenderer.gameObject.layer = Layers.UI;
                this.<currentScreenPos>__8 = PlayerView.Binder.RoomView.RoomCamera.Camera.WorldToScreenPoint(this.<>f__this.Tm.position);
                this.<newWorldPos>__9 = PlayerView.Binder.DungeonHud.Canvas.worldCamera.ScreenToWorldPoint((Vector3) this.<currentScreenPos>__8);
                this.<newWorldPos>__9.z = 60f;
                this.<>f__this.Tm.position = this.<newWorldPos>__9;
                this.<>f__this.Tm.localScale = (Vector3) (Vector3.one * 200f);
                this.<startLocalScale>__10 = this.<>f__this.Tm.localScale;
                this.<targetLocalScale>__11 = this.<startLocalScale>__10;
                this.<targetWorldPos>__12 = this.<>f__this.getChestDropHudPosition(this.<>f__this.Reward.ChestType);
                this.<tt>__3 = new TransformAnimationTask(this.<>f__this.Tm, 0.5f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__3.translate(this.<targetWorldPos>__12, false, Easing.Function.IN_QUAD);
                this.<>f__this.TransformAnimation.addTask(this.<tt>__3);
                PlayerView.Binder.EventBus.FlyToHudStarted(this.<currentScreenPos>__8);
            Label_0751:
                while (this.<>f__this.TransformAnimation.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 5;
                    goto Label_08B5;
                }
                this.<>f__this.Tm.localRotation = Quaternion.identity;
                this.<>f__this.Chest2dRenderer.enabled = true;
                this.<tt>__3 = new TransformAnimationTask(this.<>f__this.Tm, 0.1f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__3.scale((Vector3) (this.<targetLocalScale>__11 * 1.5f), true, Easing.Function.OUT_CUBIC);
                this.<>f__this.TransformAnimation.addTask(this.<tt>__3);
            Label_07F9:
                while (this.<>f__this.TransformAnimation.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 6;
                    goto Label_08B5;
                }
                PlayerView.Binder.DungeonHud.onDungeonDropViewChestUiPulseMidway(this.<>f__this.Reward);
                this.<tt>__3 = new TransformAnimationTask(this.<>f__this.Tm, 0.1f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__3.scale(this.<targetLocalScale>__11, true, Easing.Function.SMOOTHSTEP);
                this.<>f__this.TransformAnimation.addTask(this.<tt>__3);
            Label_0886:
                while (this.<>f__this.TransformAnimation.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 7;
                    goto Label_08B5;
                }
                this.<>f__this.m_dropRoutine = null;
                goto Label_08B3;
                this.$PC = -1;
            Label_08B3:
                return false;
            Label_08B5:
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

        [CompilerGenerated]
        private sealed class <coinDropRoutine>c__IteratorF0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal DungeonDropView <>f__this;
            internal IEnumerator <ie>__0;

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
                        this.<>f__this.setCoinVisualsEnabled(true);
                        this.<ie>__0 = this.<>f__this.resourceUpAndDownAnimationRoutine(this.<>f__this.m_coinRenderers);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00D6;

                    case 3:
                        PlayerView.Binder.EventBus.DungeonDropViewResourcesCollected(ResourceType.Coin, this.<>f__this.Reward.getTotalCoinAmount(), this.<>f__this.Tm.position);
                        this.<>f__this.m_dropRoutine = null;
                        goto Label_0145;

                    default:
                        goto Label_0145;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    goto Label_0147;
                }
                this.<>f__this.CoinFxParticleSystem.Play();
                this.<ie>__0 = this.<>f__this.resourceGlowRoutine(this.<>f__this.CoinGlowRenderer, ConfigUi.COIN_COLOR);
            Label_00D6:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 2;
                    goto Label_0147;
                }
                this.$current = new WaitForSeconds(ConfigUi.COIN_DROP_AUTO_COLLECT_WAIT);
                this.$PC = 3;
                goto Label_0147;
                this.$PC = -1;
            Label_0145:
                return false;
            Label_0147:
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

        [CompilerGenerated]
        private sealed class <diamondDropRoutine>c__IteratorF1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal DungeonDropView <>f__this;
            internal IEnumerator <ie>__0;

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
                        this.<>f__this.setChestVisualsEnabled(false);
                        this.<>f__this.setDiamondVisualsEnabled(true);
                        this.<>f__this.setCoinVisualsEnabled(false);
                        this.<>f__this.setXpVisualsEnabled(false);
                        this.<ie>__0 = this.<>f__this.resourceUpAndDownAnimationRoutine(this.<>f__this.m_diamondRenderers);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00FA;

                    case 3:
                        PlayerView.Binder.EventBus.DungeonDropViewResourcesCollected(ResourceType.Diamond, this.<>f__this.Reward.getTotalDiamondAmount(), this.<>f__this.Tm.position);
                        this.<>f__this.m_dropRoutine = null;
                        goto Label_0169;

                    default:
                        goto Label_0169;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    goto Label_016B;
                }
                this.<>f__this.DiamondFxParticleSystem.Play();
                this.<ie>__0 = this.<>f__this.resourceGlowRoutine(this.<>f__this.DiamondGlowRenderer, ConfigUi.DIAMOND_COLOR);
            Label_00FA:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 2;
                    goto Label_016B;
                }
                this.$current = new WaitForSeconds(ConfigUi.DIAMOND_DROP_AUTO_COLLECT_WAIT);
                this.$PC = 3;
                goto Label_016B;
                this.$PC = -1;
            Label_0169:
                return false;
            Label_016B:
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

        [CompilerGenerated]
        private sealed class <resourceGlowRoutine>c__IteratorEF : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal SpriteRenderer <$>glowRenderer;
            internal Color <$>mainColor;
            internal Color <c>__1;
            internal ManualTimer <timer>__0;
            internal SpriteRenderer glowRenderer;
            internal Color mainColor;

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
                        this.glowRenderer.color = new Color(this.mainColor.r, this.mainColor.g, this.mainColor.b, 0f);
                        this.<timer>__0 = new ManualTimer();
                        this.<timer>__0.set(0.2f);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0116;
                }
                if (!this.<timer>__0.Idle)
                {
                    this.<c>__1 = new Color(this.mainColor.r, this.mainColor.g, this.mainColor.b, this.<timer>__0.normalizedProgress());
                    this.<c>__1.a *= 0.6f;
                    this.glowRenderer.color = this.<c>__1;
                    this.<timer>__0.tick(Time.deltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                goto Label_0116;
                this.$PC = -1;
            Label_0116:
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
        private sealed class <resourceUpAndDownAnimationRoutine>c__IteratorEE : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal List<DungeonDropView.RendererData> <$>renderers;
            internal int <i>__1;
            internal int <i>__8;
            internal DungeonDropView.RendererData <r>__2;
            internal float <rotYDeg>__0;
            internal Vector3 <targetLocalPos>__7;
            internal Transform <tm>__4;
            internal TransformAnimationTask <tt>__5;
            internal TransformAnimationTask <tt2>__6;
            internal TransformAnimation <tto>__3;
            internal List<DungeonDropView.RendererData> renderers;

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
                        this.<rotYDeg>__0 = PlayerView.Binder.RoomView.RoomCamera.Transform.rotation.eulerAngles.y;
                        this.<i>__1 = 0;
                        break;

                    case 1:
                        this.<i>__1++;
                        break;

                    case 2:
                        goto Label_0248;

                    default:
                        goto Label_0298;
                }
                if (this.<i>__1 < this.renderers.Count)
                {
                    this.<r>__2 = this.renderers[this.<i>__1];
                    this.<tto>__3 = this.<r>__2.TransformAnimation;
                    this.<tm>__4 = this.<r>__2.Transform;
                    if (this.<r>__2.Billboard)
                    {
                        this.<tm>__4.localRotation = PlayerView.Binder.RoomView.RoomCamera.Transform.rotation;
                    }
                    else
                    {
                        this.<tm>__4.localRotation = Quaternion.Euler(90f, this.<rotYDeg>__0, 0f);
                    }
                    this.<tm>__4.localScale = (Vector3) (Vector3.one * 0.1f);
                    this.<tt>__5 = new TransformAnimationTask(this.<tm>__4, 0.2f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    this.<tt>__5.translate(this.<tm>__4.localPosition + ((Vector3) (Vector3.up * 3f)), true, Easing.Function.OUT_CUBIC);
                    this.<tt>__5.scale(this.<r>__2.LocalScale, true, Easing.Function.LINEAR);
                    this.<tto>__3.addTask(this.<tt>__5);
                    this.<tt2>__6 = new TransformAnimationTask(this.<tm>__4, 0.2f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    this.<targetLocalPos>__7 = this.<tm>__4.localPosition;
                    this.<tt2>__6.translate(this.<targetLocalPos>__7, true, Easing.Function.IN_CUBIC);
                    this.<tto>__3.addTask(this.<tt2>__6);
                    this.$current = new WaitForSeconds(UnityEngine.Random.Range((float) 0.03f, (float) 0.05f));
                    this.$PC = 1;
                    goto Label_029A;
                }
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxGameplay_ResourceDrop, (float) 0.15f);
                this.<i>__8 = 0;
                goto Label_0276;
            Label_0248:
                if (this.renderers[this.<i>__8].TransformAnimation.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_029A;
                }
                this.<i>__8++;
            Label_0276:
                if (this.<i>__8 < this.renderers.Count)
                {
                    goto Label_0248;
                }
                goto Label_0298;
                this.$PC = -1;
            Label_0298:
                return false;
            Label_029A:
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

        [CompilerGenerated]
        private sealed class <xpDropRoutine>c__IteratorF2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal double <$>amount;
            internal float <$>delay;
            internal DungeonDropView <>f__this;
            internal IEnumerator <ie>__0;
            internal double amount;
            internal float delay;

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
                        if (this.delay <= 0f)
                        {
                            break;
                        }
                        this.$current = new WaitForSeconds(this.delay);
                        this.$PC = 1;
                        goto Label_0192;

                    case 1:
                        break;

                    case 2:
                        goto Label_00C8;

                    case 3:
                        goto Label_012B;

                    case 4:
                        PlayerView.Binder.EventBus.DungeonDropViewResourcesCollected(ResourceType.Xp, this.amount, this.<>f__this.Tm.position);
                        this.<>f__this.m_dropRoutine = null;
                        goto Label_0190;

                    default:
                        goto Label_0190;
                }
                this.<>f__this.setChestVisualsEnabled(false);
                this.<>f__this.setDiamondVisualsEnabled(false);
                this.<>f__this.setCoinVisualsEnabled(false);
                this.<>f__this.setXpVisualsEnabled(true);
                this.<ie>__0 = this.<>f__this.resourceUpAndDownAnimationRoutine(this.<>f__this.m_xpRenderers);
            Label_00C8:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 2;
                    goto Label_0192;
                }
                this.<>f__this.XpFxParticleSystem.Play();
                this.<ie>__0 = this.<>f__this.resourceGlowRoutine(this.<>f__this.XpGlowRenderer, ConfigUi.XP_COLOR);
            Label_012B:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 3;
                    goto Label_0192;
                }
                this.$current = new WaitForSeconds(ConfigUi.XP_DROP_AUTO_COLLECT_WAIT);
                this.$PC = 4;
                goto Label_0192;
                this.$PC = -1;
            Label_0190:
                return false;
            Label_0192:
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

        private class RendererData
        {
            public bool Billboard;
            public Vector3 LocalScale;
            public SpriteRenderer SpriterRenderer;
            public UnityEngine.Transform Transform;
            public TransformAnimation TransformAnimation;
        }
    }
}

