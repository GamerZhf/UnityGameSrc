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

    public class ItemUnlockCeremonyMenu : Menu
    {
        public MenuOverlay BackgroundOverlay;
        public Image Banderoll;
        public GameObject Button;
        public Image ItemBorders;
        public Image ItemIcon;
        public Text ItemLevel;
        public Text ItemName;
        public GameObject ItemRoot;
        private List<Vector3> m_originalStarLocalPositions = new List<Vector3>();
        private ItemInstance m_rewardItemInstance;
        private Dictionary<GameObject, TransformAnimation> m_transformAnimations = new Dictionary<GameObject, TransformAnimation>();
        public Image Ray1;
        public Image Ray2;
        public List<Image> Stars = new List<Image>();
        public Text TitleText;

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator129 iterator = new <hideRoutine>c__Iterator129();
            iterator.instant = instant;
            iterator.<$>instant = instant;
            iterator.<>f__this = this;
            return iterator;
        }

        protected override void onAwake()
        {
            this.Ray1.GetComponent<RotateMenuTransformAnimation>().initialize();
            this.Ray2.GetComponent<RotateMenuTransformAnimation>().initialize();
            GameObject[] objArray1 = new GameObject[] { this.Banderoll.gameObject, this.Button, this.Ray1.gameObject, this.Ray2.gameObject, this.TitleText.gameObject };
            foreach (GameObject obj2 in objArray1)
            {
                if (obj2.gameObject.GetComponent<TransformAnimation>() == null)
                {
                    TransformAnimation animation = obj2.gameObject.AddComponent<TransformAnimation>();
                    this.m_transformAnimations.Add(obj2, animation);
                }
            }
            if (this.ItemRoot.gameObject.GetComponent<TransformAnimation>() == null)
            {
                TransformAnimation animation2 = this.ItemRoot.gameObject.AddComponent<TransformAnimation>();
                this.m_transformAnimations.Add(this.ItemRoot, animation2);
            }
            for (int i = 0; i < this.Stars.Count; i++)
            {
                if (this.Stars[i].gameObject.GetComponent<TransformAnimation>() == null)
                {
                    TransformAnimation animation3 = this.Stars[i].gameObject.AddComponent<TransformAnimation>();
                    this.m_transformAnimations.Add(this.Stars[i].gameObject, animation3);
                }
            }
            foreach (KeyValuePair<GameObject, TransformAnimation> pair in this.m_transformAnimations)
            {
                pair.Value.transform.localScale = Vector3.zero;
            }
            for (int j = 0; j < this.Stars.Count; j++)
            {
                this.m_originalStarLocalPositions.Add(this.Stars[j].transform.localPosition);
            }
        }

        public void onButtonClicked()
        {
            PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator127 iterator = new <preShowRoutine>c__Iterator127();
            iterator.parameter = parameter;
            iterator.<$>parameter = parameter;
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator128 iterator = new <showRoutine>c__Iterator128();
            iterator.<>f__this = this;
            return iterator;
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
                return PlayerView.MenuType.ItemUnlockCeremonyMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator129 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal Dictionary<GameObject, TransformAnimation>.Enumerator <$s_412>__0;
            internal ItemUnlockCeremonyMenu <>f__this;
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
                            this.<>f__this.BackgroundOverlay.setTransparent(true);
                            goto Label_013D;
                        }
                        this.<>f__this.BackgroundOverlay.fadeToTransparent(ConfigUi.POPUP_TRANSITION_DURATION_OUT, Easing.Function.LINEAR);
                        this.<$s_412>__0 = this.<>f__this.m_transformAnimations.GetEnumerator();
                        try
                        {
                            while (this.<$s_412>__0.MoveNext())
                            {
                                this.<kv>__1 = this.<$s_412>__0.Current;
                                this.<ta>__2 = this.<kv>__1.Value;
                                this.<tt>__3 = new TransformAnimationTask(this.<ta>__2.transform, ConfigUi.POPUP_TRANSITION_DURATION_OUT, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                                this.<tt>__3.scale(Vector3.zero, true, ConfigUi.MENU_EASING_IN);
                                this.<ta>__2.addTask(this.<tt>__3);
                            }
                        }
                        finally
                        {
                            this.<$s_412>__0.Dispose();
                        }
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_013D;
                }
                if (this.<>f__this.BackgroundOverlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                goto Label_013D;
                this.$PC = -1;
            Label_013D:
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
        private sealed class <preShowRoutine>c__Iterator127 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal Dictionary<GameObject, TransformAnimation>.Enumerator <$s_411>__0;
            internal ItemUnlockCeremonyMenu <>f__this;
            internal KeyValuePair<GameObject, TransformAnimation> <kv>__1;
            internal object parameter;

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
                    this.<>f__this.m_rewardItemInstance = (ItemInstance) this.parameter;
                    this.<>f__this.ItemIcon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", this.<>f__this.m_rewardItemInstance.Item.SpriteId);
                    this.<>f__this.ItemName.text = StringExtensions.ToUpperLoca(this.<>f__this.m_rewardItemInstance.Item.Name);
                    this.<>f__this.ItemLevel.text = "CHANGE ME";
                    MenuHelpers.RefreshStarContainer(this.<>f__this.Stars, this.<>f__this.m_originalStarLocalPositions, this.<>f__this.m_rewardItemInstance.Rarity, false);
                    this.<$s_411>__0 = this.<>f__this.m_transformAnimations.GetEnumerator();
                    try
                    {
                        while (this.<$s_411>__0.MoveNext())
                        {
                            this.<kv>__1 = this.<$s_411>__0.Current;
                            this.<kv>__1.Value.transform.localScale = Vector3.zero;
                        }
                    }
                    finally
                    {
                        this.<$s_411>__0.Dispose();
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
        private sealed class <showRoutine>c__Iterator128 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ItemUnlockCeremonyMenu <>f__this;
            internal TransformAnimation <ta>__0;
            internal TransformAnimation <ta>__10;
            internal TransformAnimation <ta>__2;
            internal TransformAnimation <ta>__4;
            internal TransformAnimation <ta>__6;
            internal TransformAnimation <ta>__8;
            internal TransformAnimationTask <tt>__1;
            internal TransformAnimationTask <tt>__11;
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
                        this.$current = new WaitForSeconds(0.6f);
                        this.$PC = 1;
                        goto Label_046B;

                    case 1:
                        this.<ta>__0 = this.<>f__this.m_transformAnimations[this.<>f__this.Banderoll.gameObject];
                        this.<ta>__0.transform.localScale = new Vector3(0f, 0.5f, 1f);
                        this.$current = null;
                        this.$PC = 2;
                        goto Label_046B;

                    case 2:
                        this.<tt>__1 = new TransformAnimationTask(this.<ta>__0.transform, 0.345f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__1.scale(Vector3.one, true, Easing.Function.OUT_BOUNCE);
                        this.<ta>__0.addTask(this.<tt>__1);
                        this.$current = new WaitForSeconds(0.6f);
                        this.$PC = 3;
                        goto Label_046B;

                    case 3:
                        this.<ta>__2 = this.<>f__this.m_transformAnimations[this.<>f__this.TitleText.gameObject];
                        this.<ta>__2.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 4;
                        goto Label_046B;

                    case 4:
                        this.<tt>__3 = new TransformAnimationTask(this.<ta>__2.transform, 0.24f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__3.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__2.addTask(this.<tt>__3);
                        this.$current = new WaitForSeconds(0.6f);
                        this.$PC = 5;
                        goto Label_046B;

                    case 5:
                        this.<ta>__4 = this.<>f__this.m_transformAnimations[this.<>f__this.ItemRoot];
                        this.<ta>__4.transform.localScale = (Vector3) (Vector3.one * 5f);
                        this.$current = null;
                        this.$PC = 6;
                        goto Label_046B;

                    case 6:
                        this.<tt>__5 = new TransformAnimationTask(this.<ta>__4.transform, 0.3f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__5.scale(Vector3.one, true, Easing.Function.OUT_BOUNCE);
                        this.<ta>__4.addTask(this.<tt>__5);
                        this.<ta>__6 = this.<>f__this.m_transformAnimations[this.<>f__this.Ray1.gameObject];
                        this.<ta>__6.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 7;
                        goto Label_046B;

                    case 7:
                        this.<tt>__7 = new TransformAnimationTask(this.<ta>__6.transform, 0.525f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__7.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__6.addTask(this.<tt>__7);
                        this.<ta>__8 = this.<>f__this.m_transformAnimations[this.<>f__this.Ray2.gameObject];
                        this.<ta>__8.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 8;
                        goto Label_046B;

                    case 8:
                        this.<tt>__9 = new TransformAnimationTask(this.<ta>__8.transform, 0.525f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__9.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__8.addTask(this.<tt>__9);
                        this.$current = new WaitForSeconds(0.3f);
                        this.$PC = 9;
                        goto Label_046B;

                    case 9:
                        this.<ta>__10 = this.<>f__this.m_transformAnimations[this.<>f__this.Button];
                        this.<ta>__10.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 10;
                        goto Label_046B;

                    case 10:
                        this.<tt>__11 = new TransformAnimationTask(this.<ta>__10.transform, 0.3f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__11.scale(Vector3.one, true, ConfigUi.MENU_EASING_OUT);
                        this.<ta>__10.addTask(this.<tt>__11);
                        break;

                    default:
                        break;
                        this.$PC = -1;
                        break;
                }
                return false;
            Label_046B:
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

