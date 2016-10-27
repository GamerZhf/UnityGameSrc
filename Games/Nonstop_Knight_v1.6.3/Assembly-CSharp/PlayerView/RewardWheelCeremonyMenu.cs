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
    using UnityEngine.UI;

    public class RewardWheelCeremonyMenu : Menu
    {
        public MenuOverlay BackgroundOverlay;
        public Image Banderoll;
        public GameObject ItemRoot;
        private Coroutine m_finalizationRoutine;
        private List<RewardCarousel> m_rewardWheelCells = new List<RewardCarousel>();
        private Coroutine m_rewardWheelSpinRoutine;
        private bool m_rewardWheelStopButtonClicked;
        private Coroutine m_tapToOpenTimerRoutine;
        private Dictionary<GameObject, TransformAnimation> m_transformAnimations;
        public Image Ray1;
        public Image Ray2;
        public Text RewardTitle;
        public RectTransform RewardWheelCellRoot;
        public GameObject RewardWheelRoot;
        public GameObject RewardWheelStopButton;
        public Text TapToOpenText;
        public MenuTreasureChest TreasureChest;

        private bool allChestsOpen()
        {
            return this.TreasureChest.IsOpen;
        }

        private void animatedObjectScaling(GameObject go, Vector3 targetLocalScale)
        {
            go.SetActive(true);
            TransformAnimation animation = this.m_transformAnimations[go];
            TransformAnimationTask animationTask = new TransformAnimationTask(animation.transform, 0.3f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
            animationTask.scale(targetLocalScale, true, ConfigUi.MENU_EASING_OUT);
            animation.addTask(animationTask);
        }

        [DebuggerHidden]
        private IEnumerator finalizationRoutine(Reward ltd)
        {
            <finalizationRoutine>c__Iterator174 iterator = new <finalizationRoutine>c__Iterator174();
            iterator.ltd = ltd;
            iterator.<$>ltd = ltd;
            iterator.<>f__this = this;
            return iterator;
        }

        private RewardCarousel getRewardWheelCellFarthestRight()
        {
            float minValue = float.MinValue;
            RewardCarousel carousel = null;
            for (int i = 0; i < this.m_rewardWheelCells.Count; i++)
            {
                RewardCarousel carousel2 = this.m_rewardWheelCells[i];
                if (carousel2.RectTm.anchoredPosition.x > minValue)
                {
                    minValue = carousel2.RectTm.anchoredPosition.x;
                    carousel = carousel2;
                }
            }
            return carousel;
        }

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator171 iterator = new <hideRoutine>c__Iterator171();
            iterator.instant = instant;
            iterator.<$>instant = instant;
            iterator.<>f__this = this;
            return iterator;
        }

        protected override void onAwake()
        {
            this.m_transformAnimations = new Dictionary<GameObject, TransformAnimation>();
            GameObject[] objArray1 = new GameObject[] { this.Banderoll.gameObject, this.RewardTitle.gameObject, this.TapToOpenText.gameObject, this.Ray1.gameObject, this.Ray2.gameObject, this.RewardWheelRoot, this.RewardWheelStopButton };
            foreach (GameObject obj2 in objArray1)
            {
                TransformAnimation component = obj2.GetComponent<TransformAnimation>();
                if (component == null)
                {
                    component = obj2.AddComponent<TransformAnimation>();
                }
                if (!this.m_transformAnimations.ContainsKey(obj2))
                {
                    this.m_transformAnimations.Add(obj2, component);
                }
            }
            TransformAnimation animation2 = GameObjectExtensions.AddOrGetComponent<TransformAnimation>(this.ItemRoot.gameObject);
            this.m_transformAnimations.Add(this.ItemRoot, animation2);
            foreach (KeyValuePair<GameObject, TransformAnimation> pair in this.m_transformAnimations)
            {
                pair.Value.transform.localScale = Vector3.zero;
            }
            for (int i = 0; i < 8; i++)
            {
                GameObject obj3 = App.Binder.AssetBundleLoader.instantiatePrefab("Prefabs/Menu/RewardWheelCell");
                obj3.transform.name = "RewardWheelCell-" + i;
                obj3.transform.SetParent(this.RewardWheelCellRoot, false);
                obj3.transform.localScale = Vector3.one;
                this.m_rewardWheelCells.Add(obj3.GetComponent<RewardCarousel>());
            }
        }

        public void onChestOpenButtonClicked(MenuTreasureChest treasureChest)
        {
            if (!UnityUtils.CoroutineRunning(ref this.m_finalizationRoutine))
            {
                TransformAnimation transformAnimation = treasureChest.TransformAnimation;
                transformAnimation.transform.localScale = (Vector3) (Vector3.one * 1.1f);
                TransformAnimationTask animationTask = new TransformAnimationTask(transformAnimation.transform, 0.3f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                animationTask.scale(Vector3.one, true, Easing.Function.OUT_BOUNCE);
                transformAnimation.addTask(animationTask);
                if (this.allChestsOpen())
                {
                    this.m_finalizationRoutine = UnityUtils.StartCoroutine(this, this.finalizationRoutine(treasureChest.Reward));
                }
            }
        }

        public void onRewardWheelStopButtonClicked()
        {
            this.RewardWheelStopButton.SetActive(false);
            this.m_rewardWheelStopButtonClicked = true;
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator16F iteratorf = new <preShowRoutine>c__Iterator16F();
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        [DebuggerHidden]
        private IEnumerator rewardWheelSpinRoutine()
        {
            <rewardWheelSpinRoutine>c__Iterator173 iterator = new <rewardWheelSpinRoutine>c__Iterator173();
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator170 iterator = new <showRoutine>c__Iterator170();
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        private IEnumerator tapToOpenTimerRoutine()
        {
            <tapToOpenTimerRoutine>c__Iterator172 iterator = new <tapToOpenTimerRoutine>c__Iterator172();
            iterator.<>f__this = this;
            return iterator;
        }

        [ContextMenu("testInit")]
        private void testInit()
        {
            GameObject obj2 = ResourceUtil.Instantiate<GameObject>("Prefabs/Menu/MenuCamera");
            obj2.transform.SetParent(base.transform, false);
            Camera component = obj2.GetComponent<Camera>();
            component.depth = 5f;
            component.name = "MenuCamera";
            component.enabled = true;
            obj2.transform.localPosition = new Vector3(0f, 0f, -50f);
            component.orthographicSize = 1100f;
            base.GetComponent<Canvas>().worldCamera = component;
            PlayerView.Binder.SpriteResources = new SpriteResources();
            GameLogic.Binder.ItemResources = new ItemResources();
            GameState state = new GameState();
            GameLogic.Binder.GameState = state;
            ItemInstance item = new ItemInstance("Weapon001", 1, 0, 0, null);
            Reward reward2 = new Reward();
            List<ItemInstance> list = new List<ItemInstance>();
            list.Add(item);
            reward2.ItemDrops = list;
            reward2.ChestType = ChestType.Vendor;
            Reward ltd = reward2;
            base.StartCoroutine(this.testRoutine(ltd));
        }

        [ContextMenu("testReset")]
        private void testReset()
        {
            this.m_rewardWheelStopButtonClicked = false;
            UnityUtils.StopCoroutine(this, ref this.m_rewardWheelSpinRoutine);
            this.m_rewardWheelSpinRoutine = UnityUtils.StartCoroutine(this, this.rewardWheelSpinRoutine());
        }

        [DebuggerHidden]
        private IEnumerator testRoutine(Reward ltd)
        {
            <testRoutine>c__Iterator175 iterator = new <testRoutine>c__Iterator175();
            iterator.ltd = ltd;
            iterator.<$>ltd = ltd;
            iterator.<>f__this = this;
            return iterator;
        }

        public override bool IsOverlayMenu
        {
            get
            {
                return true;
            }
        }

        public override PlayerView.MenuType MenuType
        {
            get
            {
                return PlayerView.MenuType.RewardWheelCeremonyMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <finalizationRoutine>c__Iterator174 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Reward <$>ltd;
            internal RewardWheelCeremonyMenu <>f__this;
            internal IEnumerator <ie>__1;
            internal Player <player>__0;
            internal Reward ltd;

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
                        this.<player>__0 = GameLogic.Binder.GameState.Player;
                        this.<>f__this.TapToOpenText.gameObject.SetActive(false);
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdConsumeReward(this.<player>__0, this.ltd, true, string.Empty), 0f);
                        this.$PC = 1;
                        goto Label_019C;

                    case 1:
                        this.<>f__this.BackgroundOverlay.fadeToTransparent(0.3f, Easing.Function.LINEAR);
                        this.<>f__this.animatedObjectScaling(this.<>f__this.RewardWheelRoot, Vector3.zero);
                        this.<>f__this.animatedObjectScaling(this.<>f__this.RewardTitle.gameObject, Vector3.zero);
                        this.<>f__this.animatedObjectScaling(this.<>f__this.Ray1.gameObject, Vector3.zero);
                        this.<>f__this.animatedObjectScaling(this.<>f__this.Ray2.gameObject, Vector3.zero);
                        this.<>f__this.animatedObjectScaling(this.<>f__this.Banderoll.gameObject, Vector3.zero);
                        this.<ie>__1 = TimeUtil.WaitForUnscaledSeconds(1.2f);
                        break;

                    case 2:
                        break;

                    default:
                        goto Label_019A;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 2;
                    goto Label_019C;
                }
                this.<>f__this.m_finalizationRoutine = null;
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                goto Label_019A;
                this.$PC = -1;
            Label_019A:
                return false;
            Label_019C:
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
        private sealed class <hideRoutine>c__Iterator171 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal Dictionary<GameObject, TransformAnimation>.Enumerator <$s_434>__0;
            internal RewardWheelCeremonyMenu <>f__this;
            internal KeyValuePair<GameObject, TransformAnimation> <kv>__1;
            internal TransformAnimation <ta>__2;
            internal TransformAnimationTask <tt>__3;
            internal bool instant;

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
                        UnityUtils.StopCoroutine(this.<>f__this, ref this.<>f__this.m_rewardWheelSpinRoutine);
                        UnityUtils.StopCoroutine(this.<>f__this, ref this.<>f__this.m_tapToOpenTimerRoutine);
                        if (this.instant)
                        {
                            this.<>f__this.BackgroundOverlay.fadeToTransparent(0f, Easing.Function.LINEAR);
                            goto Label_016E;
                        }
                        this.<>f__this.BackgroundOverlay.fadeToTransparent(ConfigUi.POPUP_TRANSITION_DURATION_OUT, Easing.Function.LINEAR);
                        this.<$s_434>__0 = this.<>f__this.m_transformAnimations.GetEnumerator();
                        try
                        {
                            while (this.<$s_434>__0.MoveNext())
                            {
                                this.<kv>__1 = this.<$s_434>__0.Current;
                                this.<ta>__2 = this.<kv>__1.Value;
                                this.<tt>__3 = new TransformAnimationTask(this.<ta>__2.transform, ConfigUi.POPUP_TRANSITION_DURATION_OUT, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                                this.<tt>__3.scale(Vector3.zero, true, ConfigUi.MENU_EASING_IN);
                                this.<ta>__2.addTask(this.<tt>__3);
                            }
                        }
                        finally
                        {
                            this.<$s_434>__0.Dispose();
                        }
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_016E;
                }
                if (this.<>f__this.BackgroundOverlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                goto Label_016E;
                this.$PC = -1;
            Label_016E:
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
        private sealed class <preShowRoutine>c__Iterator16F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Dictionary<GameObject, TransformAnimation>.Enumerator <$s_433>__0;
            internal RewardWheelCeremonyMenu <>f__this;
            internal KeyValuePair<GameObject, TransformAnimation> <kv>__1;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    this.<$s_433>__0 = this.<>f__this.m_transformAnimations.GetEnumerator();
                    try
                    {
                        while (this.<$s_433>__0.MoveNext())
                        {
                            this.<kv>__1 = this.<$s_433>__0.Current;
                            this.<kv>__1.Value.transform.localScale = Vector3.zero;
                        }
                    }
                    finally
                    {
                        this.<$s_433>__0.Dispose();
                    }
                    this.<>f__this.m_rewardWheelStopButtonClicked = false;
                }
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
        private sealed class <rewardWheelSpinRoutine>c__Iterator173 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal RewardWheelCeremonyMenu <>f__this;
            internal float <currentOffsetX>__4;
            internal float <d>__11;
            internal float <deceleration>__8;
            internal Vector3[] <fca>__1;
            internal int <i>__12;
            internal int <i>__5;
            internal int <i>__9;
            internal Vector3[] <rootFca>__2;
            internal RewardCarousel <rwc>__10;
            internal RewardCarousel <rwc>__13;
            internal RewardCarousel <rwc>__6;
            internal RewardCarousel <stopOnCell>__3;
            internal Vector2 <velocity>__7;
            internal float <xOffset>__0;

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
                        this.<xOffset>__0 = this.<>f__this.m_rewardWheelCells[0].RectTm.sizeDelta.x;
                        this.<fca>__1 = new Vector3[4];
                        this.<rootFca>__2 = new Vector3[4];
                        this.<stopOnCell>__3 = null;
                        this.<currentOffsetX>__4 = this.<xOffset>__0;
                        this.<i>__5 = 0;
                        while (this.<i>__5 < this.<>f__this.m_rewardWheelCells.Count)
                        {
                            this.<rwc>__6 = this.<>f__this.m_rewardWheelCells[this.<i>__5];
                            this.<rwc>__6.gameObject.SetActive(true);
                            if (this.<i>__5 == 0)
                            {
                                this.<rwc>__6.RectTm.anchoredPosition = Vector2.zero;
                            }
                            else if ((this.<i>__5 % 2) == 0)
                            {
                                this.<rwc>__6.RectTm.anchoredPosition = new Vector2(-this.<currentOffsetX>__4, 0f);
                                this.<currentOffsetX>__4 += this.<xOffset>__0;
                            }
                            else
                            {
                                this.<rwc>__6.RectTm.anchoredPosition = new Vector2(this.<currentOffsetX>__4, 0f);
                            }
                            this.<i>__5++;
                        }
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_043C;

                    case 1:
                        this.<velocity>__7 = new Vector2(1100f, 0f);
                        this.<deceleration>__8 = 0f;
                        goto Label_03EB;

                    case 2:
                        goto Label_03EB;

                    default:
                        goto Label_043A;
                }
            Label_02AF:
                this.<velocity>__7.x = Mathf.Clamp(this.<velocity>__7.x + (this.<deceleration>__8 * Time.unscaledDeltaTime), 0f, float.MaxValue);
            Label_02E1:
                this.<i>__12 = 0;
                while (this.<i>__12 < this.<>f__this.m_rewardWheelCells.Count)
                {
                    this.<rwc>__13 = this.<>f__this.m_rewardWheelCells[this.<i>__12];
                    RectTransform rectTm = this.<rwc>__13.RectTm;
                    rectTm.anchoredPosition -= (Vector2) (this.<velocity>__7 * Time.unscaledDeltaTime);
                    this.<rwc>__13.RectTm.GetWorldCorners(this.<fca>__1);
                    if (this.<fca>__1[2].x < this.<rootFca>__2[0].x)
                    {
                        this.<rwc>__13.RectTm.anchoredPosition = new Vector2(this.<>f__this.getRewardWheelCellFarthestRight().RectTm.anchoredPosition.x + this.<xOffset>__0, 0f);
                    }
                    this.<i>__12++;
                }
                this.$current = null;
                this.$PC = 2;
                goto Label_043C;
            Label_03EB:
                if (this.<velocity>__7.x > 0f)
                {
                    this.<>f__this.RewardWheelRoot.GetComponent<RectTransform>().GetWorldCorners(this.<rootFca>__2);
                    if (!this.<>f__this.m_rewardWheelStopButtonClicked)
                    {
                        goto Label_02E1;
                    }
                    if (this.<stopOnCell>__3 == null)
                    {
                        this.<i>__9 = 0;
                        while (this.<i>__9 < this.<>f__this.m_rewardWheelCells.Count)
                        {
                            this.<rwc>__10 = this.<>f__this.m_rewardWheelCells[this.<i>__9];
                            this.<rwc>__10.RectTm.GetWorldCorners(this.<fca>__1);
                            if (this.<fca>__1[0].x >= this.<rootFca>__2[2].x)
                            {
                                this.<stopOnCell>__3 = this.<rwc>__10;
                                this.<d>__11 = this.<rwc>__10.RectTm.anchoredPosition.x;
                                this.<deceleration>__8 = -1210000f / (2f * this.<d>__11);
                                break;
                            }
                            this.<i>__9++;
                        }
                    }
                    goto Label_02AF;
                }
                this.<>f__this.m_finalizationRoutine = UnityUtils.StartCoroutine(this.<>f__this, this.<>f__this.finalizationRoutine(null));
                this.<>f__this.m_rewardWheelSpinRoutine = null;
                goto Label_043A;
                this.$PC = -1;
            Label_043A:
                return false;
            Label_043C:
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
        private sealed class <showRoutine>c__Iterator170 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal RewardWheelCeremonyMenu <>f__this;
            internal TransformAnimation <ta>__0;
            internal TransformAnimation <ta>__2;
            internal TransformAnimation <ta>__4;
            internal TransformAnimation <ta>__6;
            internal TransformAnimation <ta>__8;
            internal TransformAnimationTask <tt>__1;
            internal TransformAnimationTask <tt>__3;
            internal TransformAnimationTask <tt>__5;
            internal TransformAnimationTask <tt>__7;
            internal TransformAnimationTask <tt>__9;

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
                        this.<>f__this.BackgroundOverlay.fadeToBlack(0.25f, ConfigUi.MENU_BACKGROUND_ALPHA, Easing.Function.LINEAR);
                        this.<ta>__0 = this.<>f__this.m_transformAnimations[this.<>f__this.Banderoll.gameObject];
                        this.<ta>__0.transform.localScale = new Vector3(0f, 0.5f, 1f);
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_03AB;

                    case 1:
                        this.<tt>__1 = new TransformAnimationTask(this.<ta>__0.transform, 0.345f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__1.scale(Vector3.one, true, Easing.Function.OUT_BOUNCE);
                        this.<ta>__0.addTask(this.<tt>__1);
                        this.<ta>__2 = this.<>f__this.m_transformAnimations[this.<>f__this.RewardTitle.gameObject];
                        this.<ta>__2.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 2;
                        goto Label_03AB;

                    case 2:
                        this.<tt>__3 = new TransformAnimationTask(this.<ta>__2.transform, 0.24f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__3.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__2.addTask(this.<tt>__3);
                        this.<ta>__4 = this.<>f__this.m_transformAnimations[this.<>f__this.Ray1.gameObject];
                        this.<ta>__4.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 3;
                        goto Label_03AB;

                    case 3:
                        this.<tt>__5 = new TransformAnimationTask(this.<ta>__4.transform, 0.525f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__5.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__4.addTask(this.<tt>__5);
                        this.<ta>__6 = this.<>f__this.m_transformAnimations[this.<>f__this.Ray2.gameObject];
                        this.<ta>__6.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 4;
                        goto Label_03AB;

                    case 4:
                        this.<tt>__7 = new TransformAnimationTask(this.<ta>__6.transform, 0.525f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__7.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__6.addTask(this.<tt>__7);
                        this.$current = new WaitForSeconds(0.45f);
                        this.$PC = 5;
                        goto Label_03AB;

                    case 5:
                        this.<ta>__8 = this.<>f__this.m_transformAnimations[this.<>f__this.ItemRoot];
                        this.<ta>__8.transform.localScale = (Vector3) (Vector3.one * 5f);
                        this.$current = null;
                        this.$PC = 6;
                        goto Label_03AB;

                    case 6:
                        this.<tt>__9 = new TransformAnimationTask(this.<ta>__8.transform, 0.3f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__9.scale(Vector3.one, true, Easing.Function.OUT_BOUNCE);
                        this.<ta>__8.addTask(this.<tt>__9);
                        this.$current = null;
                        this.$PC = 7;
                        goto Label_03AB;

                    case 7:
                        this.<>f__this.m_tapToOpenTimerRoutine = UnityUtils.StartCoroutine(this.<>f__this, this.<>f__this.tapToOpenTimerRoutine());
                        break;

                    default:
                        break;
                        this.$PC = -1;
                        break;
                }
                return false;
            Label_03AB:
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
        private sealed class <tapToOpenTimerRoutine>c__Iterator172 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal RewardWheelCeremonyMenu <>f__this;
            internal TransformAnimation <ta>__0;
            internal TransformAnimationTask <tt>__1;

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
                        this.$current = new WaitForSeconds(1.2f);
                        this.$PC = 1;
                        goto Label_0118;

                    case 1:
                        if (this.<>f__this.allChestsOpen())
                        {
                            break;
                        }
                        this.<>f__this.TapToOpenText.gameObject.SetActive(true);
                        this.<ta>__0 = this.<>f__this.m_transformAnimations[this.<>f__this.TapToOpenText.gameObject];
                        this.<ta>__0.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 2;
                        goto Label_0118;

                    case 2:
                        this.<tt>__1 = new TransformAnimationTask(this.<ta>__0.transform, 0.3f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__1.scale(Vector3.one, true, ConfigUi.MENU_EASING_OUT);
                        this.<ta>__0.addTask(this.<tt>__1);
                        break;

                    default:
                        goto Label_0116;
                }
                this.<>f__this.m_tapToOpenTimerRoutine = null;
                goto Label_0116;
                this.$PC = -1;
            Label_0116:
                return false;
            Label_0118:
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
        private sealed class <testRoutine>c__Iterator175 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Reward <$>ltd;
            internal RewardWheelCeremonyMenu <>f__this;
            internal IEnumerator <ie>__0;
            internal Reward ltd;

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
                        this.<ie>__0 = this.<>f__this.preShowRoutine(MenuContentType.NONE, this.ltd);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00A9;

                    default:
                        goto Label_00C5;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    goto Label_00C7;
                }
                this.<ie>__0 = this.<>f__this.showRoutine(MenuContentType.NONE, this.ltd);
            Label_00A9:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 2;
                    goto Label_00C7;
                }
                goto Label_00C5;
                this.$PC = -1;
            Label_00C5:
                return false;
            Label_00C7:
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

