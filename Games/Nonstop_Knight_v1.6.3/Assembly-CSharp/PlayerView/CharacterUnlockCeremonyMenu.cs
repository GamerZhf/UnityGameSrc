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

    public class CharacterUnlockCeremonyMenu : Menu
    {
        public RawImage Avatar;
        public MenuOverlay BackgroundOverlay;
        public Image Banderoll;
        public GameObject Button;
        public Text DescriptionText;
        private CharacterInstance m_character;
        private Dictionary<GameObject, TransformAnimation> m_transformAnimations = new Dictionary<GameObject, TransformAnimation>();
        public Text Name;
        public Image Ray1;
        public Image Ray2;

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator113 iterator = new <hideRoutine>c__Iterator113();
            iterator.instant = instant;
            iterator.<$>instant = instant;
            iterator.<>f__this = this;
            return iterator;
        }

        protected override void onAwake()
        {
            this.Ray1.GetComponent<RotateMenuTransformAnimation>().initialize();
            this.Ray2.GetComponent<RotateMenuTransformAnimation>().initialize();
            GameObject[] objArray1 = new GameObject[] { this.Banderoll.gameObject, this.Button, this.Ray1.gameObject, this.Ray2.gameObject, this.DescriptionText.gameObject, this.Avatar.gameObject };
            foreach (GameObject obj2 in objArray1)
            {
                if (obj2.gameObject.GetComponent<TransformAnimation>() == null)
                {
                    TransformAnimation animation = obj2.gameObject.AddComponent<TransformAnimation>();
                    this.m_transformAnimations.Add(obj2, animation);
                }
            }
            foreach (KeyValuePair<GameObject, TransformAnimation> pair in this.m_transformAnimations)
            {
                pair.Value.transform.localScale = Vector3.zero;
            }
        }

        public void onButtonClicked()
        {
            PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator111 iterator = new <preShowRoutine>c__Iterator111();
            iterator.parameter = parameter;
            iterator.<$>parameter = parameter;
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator112 iterator = new <showRoutine>c__Iterator112();
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
                return PlayerView.MenuType.CharacterUnlockCeremonyMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator113 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal Dictionary<GameObject, TransformAnimation>.Enumerator <$s_401>__0;
            internal CharacterUnlockCeremonyMenu <>f__this;
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
                        this.<$s_401>__0 = this.<>f__this.m_transformAnimations.GetEnumerator();
                        try
                        {
                            while (this.<$s_401>__0.MoveNext())
                            {
                                this.<kv>__1 = this.<$s_401>__0.Current;
                                this.<ta>__2 = this.<kv>__1.Value;
                                this.<tt>__3 = new TransformAnimationTask(this.<ta>__2.transform, ConfigUi.POPUP_TRANSITION_DURATION_OUT, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                                this.<tt>__3.scale(Vector3.zero, true, ConfigUi.MENU_EASING_IN);
                                this.<ta>__2.addTask(this.<tt>__3);
                            }
                        }
                        finally
                        {
                            this.<$s_401>__0.Dispose();
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
        private sealed class <preShowRoutine>c__Iterator111 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal Dictionary<GameObject, TransformAnimation>.Enumerator <$s_400>__0;
            internal CharacterUnlockCeremonyMenu <>f__this;
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
                    this.<>f__this.m_character = (CharacterInstance) this.parameter;
                    this.<>f__this.Name.text = StringExtensions.ToUpperLoca(this.<>f__this.m_character.Name);
                    this.<>f__this.Avatar.texture = PlayerView.Binder.MenuSystem.CharacterMenuCamera.RenderTexture;
                    this.<>f__this.Avatar.enabled = true;
                    this.<$s_400>__0 = this.<>f__this.m_transformAnimations.GetEnumerator();
                    try
                    {
                        while (this.<$s_400>__0.MoveNext())
                        {
                            this.<kv>__1 = this.<$s_400>__0.Current;
                            this.<kv>__1.Value.transform.localScale = Vector3.zero;
                        }
                    }
                    finally
                    {
                        this.<$s_400>__0.Dispose();
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
        private sealed class <showRoutine>c__Iterator112 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterUnlockCeremonyMenu <>f__this;
            internal RectTransform <avatarRectTm>__4;
            internal TransformAnimation <ta>__0;
            internal TransformAnimation <ta>__11;
            internal TransformAnimation <ta>__2;
            internal TransformAnimation <ta>__5;
            internal TransformAnimation <ta>__7;
            internal TransformAnimation <ta>__9;
            internal TransformAnimationTask <tt>__1;
            internal TransformAnimationTask <tt>__10;
            internal TransformAnimationTask <tt>__12;
            internal TransformAnimationTask <tt>__3;
            internal TransformAnimationTask <tt>__6;
            internal TransformAnimationTask <tt>__8;

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
                        goto Label_04AE;

                    case 1:
                        this.<ta>__0 = this.<>f__this.m_transformAnimations[this.<>f__this.Banderoll.gameObject];
                        this.<ta>__0.transform.localScale = new Vector3(0f, 0.5f, 1f);
                        this.$current = null;
                        this.$PC = 2;
                        goto Label_04AE;

                    case 2:
                        this.<tt>__1 = new TransformAnimationTask(this.<ta>__0.transform, 0.345f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__1.scale(Vector3.one, true, Easing.Function.OUT_BOUNCE);
                        this.<ta>__0.addTask(this.<tt>__1);
                        this.$current = new WaitForSeconds(0.6f);
                        this.$PC = 3;
                        goto Label_04AE;

                    case 3:
                        this.<ta>__2 = this.<>f__this.m_transformAnimations[this.<>f__this.DescriptionText.gameObject];
                        this.<ta>__2.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 4;
                        goto Label_04AE;

                    case 4:
                        this.<tt>__3 = new TransformAnimationTask(this.<ta>__2.transform, 0.24f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__3.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__2.addTask(this.<tt>__3);
                        this.$current = new WaitForSeconds(0.6f);
                        this.$PC = 5;
                        goto Label_04AE;

                    case 5:
                        this.<avatarRectTm>__4 = this.<>f__this.Avatar.GetComponent<RectTransform>();
                        this.<avatarRectTm>__4.anchoredPosition = new Vector2(0f, this.<avatarRectTm>__4.anchoredPosition.y);
                        this.<ta>__5 = this.<>f__this.m_transformAnimations[this.<>f__this.Avatar.gameObject];
                        this.<ta>__5.transform.localScale = (Vector3) (Vector3.one * 5f);
                        this.$current = null;
                        this.$PC = 6;
                        goto Label_04AE;

                    case 6:
                        this.<tt>__6 = new TransformAnimationTask(this.<ta>__5.transform, 0.3f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__6.scale(Vector3.one, true, Easing.Function.OUT_BOUNCE);
                        this.<ta>__5.addTask(this.<tt>__6);
                        this.<ta>__7 = this.<>f__this.m_transformAnimations[this.<>f__this.Ray1.gameObject];
                        this.<ta>__7.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 7;
                        goto Label_04AE;

                    case 7:
                        this.<tt>__8 = new TransformAnimationTask(this.<ta>__7.transform, 0.525f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__8.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__7.addTask(this.<tt>__8);
                        this.<ta>__9 = this.<>f__this.m_transformAnimations[this.<>f__this.Ray2.gameObject];
                        this.<ta>__9.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 8;
                        goto Label_04AE;

                    case 8:
                        this.<tt>__10 = new TransformAnimationTask(this.<ta>__9.transform, 0.525f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__10.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__9.addTask(this.<tt>__10);
                        this.$current = new WaitForSeconds(1.35f);
                        this.$PC = 9;
                        goto Label_04AE;

                    case 9:
                        this.<ta>__11 = this.<>f__this.m_transformAnimations[this.<>f__this.Button];
                        this.<ta>__11.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 10;
                        goto Label_04AE;

                    case 10:
                        this.<tt>__12 = new TransformAnimationTask(this.<ta>__11.transform, 0.3f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__12.scale(Vector3.one, true, ConfigUi.MENU_EASING_OUT);
                        this.<ta>__11.addTask(this.<tt>__12);
                        break;

                    default:
                        break;
                        this.$PC = -1;
                        break;
                }
                return false;
            Label_04AE:
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

