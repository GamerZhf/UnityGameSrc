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

    public class PlayerLevelUpCeremonyMenu : Menu
    {
        public MenuOverlay BackgroundOverlay;
        public Image Banderoll;
        public GameObject Button;
        public Text DescriptionText;
        public GameObject DiamondRewardRoot;
        public Text DiamondRewardText;
        public GameObject EnergyRewardRoot;
        public Text EnergyRewardText;
        private double m_diamondCount;
        private Coroutine m_rewardRoutine;
        private Dictionary<GameObject, TransformAnimation> m_transformAnimations = new Dictionary<GameObject, TransformAnimation>();
        public Image Ray1;
        public Image Ray2;
        public Text RewardTitle;

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator160 iterator = new <hideRoutine>c__Iterator160();
            iterator.instant = instant;
            iterator.<$>instant = instant;
            iterator.<>f__this = this;
            return iterator;
        }

        protected override void onAwake()
        {
            this.Ray1.GetComponent<RotateMenuTransformAnimation>().initialize();
            this.Ray2.GetComponent<RotateMenuTransformAnimation>().initialize();
            GameObject[] objArray1 = new GameObject[] { this.Banderoll.gameObject, this.Button, this.Ray1.gameObject, this.Ray2.gameObject, this.DescriptionText.gameObject, this.RewardTitle.gameObject };
            foreach (GameObject obj2 in objArray1)
            {
                if (obj2.gameObject.GetComponent<TransformAnimation>() == null)
                {
                    TransformAnimation animation = obj2.gameObject.AddComponent<TransformAnimation>();
                    this.m_transformAnimations.Add(obj2, animation);
                }
            }
            if (this.DiamondRewardRoot.gameObject.GetComponent<TransformAnimation>() == null)
            {
                TransformAnimation animation2 = this.DiamondRewardRoot.gameObject.AddComponent<TransformAnimation>();
                this.m_transformAnimations.Add(this.DiamondRewardRoot, animation2);
            }
            if (this.EnergyRewardRoot.gameObject.GetComponent<TransformAnimation>() == null)
            {
                TransformAnimation animation3 = this.EnergyRewardRoot.gameObject.AddComponent<TransformAnimation>();
                this.m_transformAnimations.Add(this.EnergyRewardRoot, animation3);
            }
            foreach (KeyValuePair<GameObject, TransformAnimation> pair in this.m_transformAnimations)
            {
                pair.Value.transform.localScale = Vector3.zero;
            }
        }

        public void onButtonClicked()
        {
            if (!UnityUtils.CoroutineRunning(ref this.m_rewardRoutine))
            {
                this.m_rewardRoutine = UnityUtils.StartCoroutine(this, this.rewardRoutine());
            }
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator15E iteratore = new <preShowRoutine>c__Iterator15E();
            iteratore.<>f__this = this;
            return iteratore;
        }

        [DebuggerHidden]
        private IEnumerator rewardRoutine()
        {
            <rewardRoutine>c__Iterator161 iterator = new <rewardRoutine>c__Iterator161();
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator15F iteratorf = new <showRoutine>c__Iterator15F();
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        [ContextMenu("test()")]
        private void test()
        {
            base.StopAllCoroutines();
            base.Awake();
            base.StartCoroutine(this.showRoutine(MenuContentType.NONE, null));
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
                return PlayerView.MenuType.PlayerLevelUpCeremonyMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator160 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal Dictionary<GameObject, TransformAnimation>.Enumerator <$s_422>__0;
            internal PlayerLevelUpCeremonyMenu <>f__this;
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
                        if (this.instant)
                        {
                            this.<>f__this.BackgroundOverlay.fadeToTransparent(0f, Easing.Function.LINEAR);
                            goto Label_0142;
                        }
                        this.<>f__this.BackgroundOverlay.fadeToTransparent(ConfigUi.POPUP_TRANSITION_DURATION_OUT, Easing.Function.LINEAR);
                        this.<$s_422>__0 = this.<>f__this.m_transformAnimations.GetEnumerator();
                        try
                        {
                            while (this.<$s_422>__0.MoveNext())
                            {
                                this.<kv>__1 = this.<$s_422>__0.Current;
                                this.<ta>__2 = this.<kv>__1.Value;
                                this.<tt>__3 = new TransformAnimationTask(this.<ta>__2.transform, ConfigUi.POPUP_TRANSITION_DURATION_OUT, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                                this.<tt>__3.scale(Vector3.zero, true, ConfigUi.MENU_EASING_IN);
                                this.<ta>__2.addTask(this.<tt>__3);
                            }
                        }
                        finally
                        {
                            this.<$s_422>__0.Dispose();
                        }
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0142;
                }
                if (this.<>f__this.BackgroundOverlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                goto Label_0142;
                this.$PC = -1;
            Label_0142:
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
        private sealed class <preShowRoutine>c__Iterator15E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Dictionary<GameObject, TransformAnimation>.Enumerator <$s_421>__2;
            internal PlayerLevelUpCeremonyMenu <>f__this;
            internal KeyValuePair<GameObject, TransformAnimation> <kv>__3;
            internal int <levelUpCount>__1;
            internal Player <player>__0;

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
                    this.<player>__0 = GameLogic.Binder.GameState.Player;
                    this.<levelUpCount>__1 = 1;
                    this.<>f__this.m_diamondCount = 5.0;
                    this.<>f__this.DescriptionText.text = "CONGRATULATIONS,\nYOU ARE NOW LEVEL " + this.<player>__0.Rank + "!";
                    this.<>f__this.DiamondRewardText.text = this.<>f__this.m_diamondCount.ToString();
                    this.<$s_421>__2 = this.<>f__this.m_transformAnimations.GetEnumerator();
                    try
                    {
                        while (this.<$s_421>__2.MoveNext())
                        {
                            this.<kv>__3 = this.<$s_421>__2.Current;
                            this.<kv>__3.Value.transform.localScale = Vector3.zero;
                        }
                    }
                    finally
                    {
                        this.<$s_421>__2.Dispose();
                    }
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
        private sealed class <rewardRoutine>c__Iterator161 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal PlayerLevelUpCeremonyMenu <>f__this;
            internal Player <player>__0;

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
                    this.<player>__0 = GameLogic.Binder.GameState.Player;
                    CmdGainResources.ExecuteStatic(this.<player>__0, ResourceType.Diamond, this.<>f__this.m_diamondCount, false, string.Empty, null);
                    GameLogic.Binder.CommandProcessor.execute(new CmdEndGameplay(GameLogic.Binder.GameState.ActiveDungeon, false), 0f);
                    this.<>f__this.m_rewardRoutine = null;
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
        private sealed class <showRoutine>c__Iterator15F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal PlayerLevelUpCeremonyMenu <>f__this;
            internal int <deltaMaxEnergy>__2;
            internal RectTransform <diamondRectTm>__10;
            internal RectTransform <energyRectTm>__13;
            internal bool <energyRewardVisible>__3;
            internal Player <player>__0;
            internal int <prevLevel>__1;
            internal TransformAnimation <ta>__11;
            internal TransformAnimation <ta>__14;
            internal TransformAnimation <ta>__16;
            internal TransformAnimation <ta>__18;
            internal TransformAnimation <ta>__20;
            internal TransformAnimation <ta>__4;
            internal TransformAnimation <ta>__6;
            internal TransformAnimation <ta>__8;
            internal TransformAnimationTask <tt>__12;
            internal TransformAnimationTask <tt>__15;
            internal TransformAnimationTask <tt>__17;
            internal TransformAnimationTask <tt>__19;
            internal TransformAnimationTask <tt>__21;
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
                        this.<player>__0 = GameLogic.Binder.GameState.Player;
                        this.<prevLevel>__1 = this.<player>__0.Rank - 1;
                        this.<deltaMaxEnergy>__2 = GameLogic.Binder.LevelUpRules.getMaxEnergyForLevel(this.<player>__0.Rank) - GameLogic.Binder.LevelUpRules.getMaxEnergyForLevel(this.<prevLevel>__1);
                        this.<energyRewardVisible>__3 = this.<deltaMaxEnergy>__2 > 0;
                        this.<>f__this.BackgroundOverlay.fadeToBlack(0.25f, ConfigUi.MENU_BACKGROUND_ALPHA, Easing.Function.LINEAR);
                        this.$current = new WaitForSeconds(0.6f);
                        this.$PC = 1;
                        goto Label_06E4;

                    case 1:
                        this.<ta>__4 = this.<>f__this.m_transformAnimations[this.<>f__this.Banderoll.gameObject];
                        this.<ta>__4.transform.localScale = new Vector3(0f, 0.5f, 1f);
                        this.$current = null;
                        this.$PC = 2;
                        goto Label_06E4;

                    case 2:
                        this.<tt>__5 = new TransformAnimationTask(this.<ta>__4.transform, 0.345f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__5.scale(Vector3.one, true, Easing.Function.OUT_BOUNCE);
                        this.<ta>__4.addTask(this.<tt>__5);
                        this.$current = new WaitForSeconds(0.6f);
                        this.$PC = 3;
                        goto Label_06E4;

                    case 3:
                        this.<ta>__6 = this.<>f__this.m_transformAnimations[this.<>f__this.DescriptionText.gameObject];
                        this.<ta>__6.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 4;
                        goto Label_06E4;

                    case 4:
                        this.<tt>__7 = new TransformAnimationTask(this.<ta>__6.transform, 0.24f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__7.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__6.addTask(this.<tt>__7);
                        this.<ta>__8 = this.<>f__this.m_transformAnimations[this.<>f__this.RewardTitle.gameObject];
                        this.<ta>__8.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 5;
                        goto Label_06E4;

                    case 5:
                        this.<tt>__9 = new TransformAnimationTask(this.<ta>__8.transform, 0.24f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__9.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__8.addTask(this.<tt>__9);
                        this.$current = new WaitForSeconds(0.6f);
                        this.$PC = 6;
                        goto Label_06E4;

                    case 6:
                        this.<diamondRectTm>__10 = this.<>f__this.DiamondRewardRoot.GetComponent<RectTransform>();
                        if (!this.<energyRewardVisible>__3)
                        {
                            this.<diamondRectTm>__10.anchoredPosition = new Vector2(0f, this.<diamondRectTm>__10.anchoredPosition.y);
                            break;
                        }
                        this.<diamondRectTm>__10.anchoredPosition = new Vector2(-130f, this.<diamondRectTm>__10.anchoredPosition.y);
                        break;

                    case 7:
                        this.<tt>__12 = new TransformAnimationTask(this.<ta>__11.transform, 0.3f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__12.scale(Vector3.one, true, Easing.Function.OUT_BOUNCE);
                        this.<ta>__11.addTask(this.<tt>__12);
                        if (!this.<energyRewardVisible>__3)
                        {
                            goto Label_04FC;
                        }
                        this.<>f__this.EnergyRewardText.text = "MAX\n+" + this.<deltaMaxEnergy>__2;
                        this.<energyRectTm>__13 = this.<>f__this.EnergyRewardRoot.GetComponent<RectTransform>();
                        this.<energyRectTm>__13.anchoredPosition = new Vector2(130f, this.<energyRectTm>__13.anchoredPosition.y);
                        this.<ta>__14 = this.<>f__this.m_transformAnimations[this.<>f__this.EnergyRewardRoot];
                        this.<ta>__14.transform.localScale = (Vector3) (Vector3.one * 5f);
                        this.$current = null;
                        this.$PC = 8;
                        goto Label_06E4;

                    case 8:
                        this.<tt>__15 = new TransformAnimationTask(this.<ta>__14.transform, 0.3f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__15.scale(Vector3.one, true, Easing.Function.OUT_BOUNCE);
                        this.<ta>__14.addTask(this.<tt>__15);
                        goto Label_04FC;

                    case 9:
                        this.<tt>__17 = new TransformAnimationTask(this.<ta>__16.transform, 0.525f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__17.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__16.addTask(this.<tt>__17);
                        this.<ta>__18 = this.<>f__this.m_transformAnimations[this.<>f__this.Ray2.gameObject];
                        this.<ta>__18.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 10;
                        goto Label_06E4;

                    case 10:
                        this.<tt>__19 = new TransformAnimationTask(this.<ta>__18.transform, 0.525f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__19.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__18.addTask(this.<tt>__19);
                        this.$current = new WaitForSeconds(1.35f);
                        this.$PC = 11;
                        goto Label_06E4;

                    case 11:
                        this.<ta>__20 = this.<>f__this.m_transformAnimations[this.<>f__this.Button];
                        this.<ta>__20.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 12;
                        goto Label_06E4;

                    case 12:
                        this.<tt>__21 = new TransformAnimationTask(this.<ta>__20.transform, 0.3f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__21.scale(Vector3.one, true, ConfigUi.MENU_EASING_OUT);
                        this.<ta>__20.addTask(this.<tt>__21);
                        goto Label_06E2;

                    default:
                        goto Label_06E2;
                }
                this.<ta>__11 = this.<>f__this.m_transformAnimations[this.<>f__this.DiamondRewardRoot];
                this.<ta>__11.transform.localScale = (Vector3) (Vector3.one * 5f);
                this.$current = null;
                this.$PC = 7;
                goto Label_06E4;
            Label_04FC:
                this.<ta>__16 = this.<>f__this.m_transformAnimations[this.<>f__this.Ray1.gameObject];
                this.<ta>__16.transform.localScale = Vector3.zero;
                this.$current = null;
                this.$PC = 9;
                goto Label_06E4;
                this.$PC = -1;
            Label_06E2:
                return false;
            Label_06E4:
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

