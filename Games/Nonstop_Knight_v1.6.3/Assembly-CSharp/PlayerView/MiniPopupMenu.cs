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

    public class MiniPopupMenu : Menu
    {
        public Text AdditionalShopButtonText;
        public MenuOverlay BackgroundOverlay;
        public Button CloseButton;
        public RectTransform ContentAreaTm;
        public Text Description;
        public RectTransform DisabledContentTm;
        public InputParameters InputParams;
        private MenuContent m_content;
        private Sprite m_defaultMainButtonBgSprite;
        private TransformAnimation m_panelTransformAnimation;
        private List<RewardGalleryCell> m_rewardGalleryCells = new List<RewardGalleryCell>(1);
        public PrettyButton MainButton;
        public IconWithText MainButtonVisualsDual;
        public IconWithText MainButtonVisualsSingle;
        public CanvasGroup PanelRoot;
        public RectTransform RewardTm;
        public GameObject ShopButton;
        public Image TitleIcon;
        public Text TitleText;

        public override MenuContent activeContentObject()
        {
            return this.m_content;
        }

        public override MenuContentType activeContentType()
        {
            if (this.m_content != null)
            {
                return this.m_content.ContentType;
            }
            return MenuContentType.NONE;
        }

        private void addRewardGalleryCellToGrid(RewardGalleryCell.Content content)
        {
            RewardGalleryCell item = PlayerView.Binder.RewardGalleryCellPool.getObject(content.Type);
            item.transform.SetParent(this.RewardTm, false);
            item.initialize(content, null);
            this.m_rewardGalleryCells.Add(item);
            item.gameObject.SetActive(true);
        }

        private void cleanupCells()
        {
            for (int i = this.m_rewardGalleryCells.Count - 1; i >= 0; i--)
            {
                RewardGalleryCell item = this.m_rewardGalleryCells[i];
                this.m_rewardGalleryCells.Remove(item);
                PlayerView.Binder.RewardGalleryCellPool.returnObject(item, item.Type);
            }
        }

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator152 iterator = new <hideRoutine>c__Iterator152();
            iterator.instant = instant;
            iterator.<$>instant = instant;
            iterator.<>f__this = this;
            return iterator;
        }

        protected override void onAwake()
        {
            this.m_panelTransformAnimation = this.PanelRoot.gameObject.AddComponent<TransformAnimation>();
            this.m_defaultMainButtonBgSprite = this.MainButton.Bg.sprite;
            this.BackgroundOverlay.fadeToTransparent(0f, Easing.Function.LINEAR);
            this.AdditionalShopButtonText.text = _.L(ConfigLoca.VENDOR_VISIT_SHOP, null, false);
        }

        public void onBackButtonClicked()
        {
        }

        public void onBackgroundOverlayClicked()
        {
            if ((this.CloseButton.enabled && !this.m_content.onBackgroundOverlayClicked()) && !PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        public void onCloseButtonClicked()
        {
            if (this.CloseButton.enabled && !this.m_content.onCloseButtonClicked())
            {
                if (this.InputParams.CloseCallback != null)
                {
                    this.InputParams.CloseCallback();
                }
                else if (!PlayerView.Binder.MenuSystem.InTransition)
                {
                    PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                }
            }
        }

        public void onMainButtonClicked()
        {
            if (!this.m_content.onMainButtonClicked() && !PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        public void onVisitShopButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                VendorPopupContent component = PlayerView.Binder.MenuContentResources.getSharedResource(MenuContentType.VendorPopupContent).GetComponent<VendorPopupContent>();
                VendorPopupContent.InputParameters parameters2 = new VendorPopupContent.InputParameters();
                parameters2.PathToShop = this.InputParams.PathToShop;
                parameters2.CloseCallback = this.InputParams.CloseCallback;
                parameters2.PurchaseCallback = this.InputParams.PurchaseCallback;
                parameters2.CenterOnRectTm = component.GemGridTm;
                VendorPopupContent.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.StackedPopupMenu, MenuContentType.VendorPopupContent, parameter, 0f, true, true);
            }
        }

        public void populateLayout(MiniPopupEntry mpe, bool mainButtonInteractable, RewardGalleryCell.Content rewardContent, [Optional, DefaultParameterValue(null)] string overrideButtonText, [Optional, DefaultParameterValue(null)] Sprite overrideButtonSprite, [Optional, DefaultParameterValue(null)] string overrideDescriptionTextLocalized)
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.cleanupCells();
            if (overrideDescriptionTextLocalized != null)
            {
                this.Description.text = overrideDescriptionTextLocalized;
            }
            else
            {
                this.Description.text = _.L(mpe.DescriptionText, null, false);
            }
            this.ShopButton.SetActive(mpe.ShowAdditionalShopButton && player.shopUnlocked());
            this.setCloseButtonVisibility(!mpe.HideCloseButton);
            this.TitleText.text = StringExtensions.ToUpperLoca(_.L(mpe.TitleText, null, false));
            if (mpe.TitleIcon != null)
            {
                this.TitleIcon.enabled = true;
                this.TitleIcon.sprite = PlayerView.Binder.SpriteResources.getSprite(mpe.TitleIcon);
            }
            else
            {
                this.TitleIcon.enabled = false;
            }
            if (rewardContent != null)
            {
                this.addRewardGalleryCellToGrid(rewardContent);
            }
            if ((overrideButtonSprite != null) && (!string.IsNullOrEmpty(overrideButtonText) || !string.IsNullOrEmpty(mpe.ButtonText)))
            {
                this.MainButtonVisualsSingle.gameObject.SetActive(false);
                this.MainButtonVisualsDual.gameObject.SetActive(true);
                this.MainButtonVisualsDual.Icon.sprite = overrideButtonSprite;
                if (!string.IsNullOrEmpty(overrideButtonText))
                {
                    this.MainButtonVisualsDual.Text.text = StringExtensions.ToUpperLoca(overrideButtonText);
                }
                else if (!string.IsNullOrEmpty(mpe.ButtonText))
                {
                    this.MainButtonVisualsDual.Text.text = StringExtensions.ToUpperLoca(_.L(mpe.ButtonText, null, false));
                }
                else
                {
                    this.MainButtonVisualsDual.Text.text = null;
                }
            }
            else
            {
                this.MainButtonVisualsSingle.gameObject.SetActive(true);
                this.MainButtonVisualsDual.gameObject.SetActive(false);
                if (overrideButtonSprite != null)
                {
                    this.MainButtonVisualsSingle.Icon.enabled = true;
                    this.MainButtonVisualsSingle.Text.enabled = false;
                    this.MainButtonVisualsSingle.Icon.sprite = overrideButtonSprite;
                }
                else
                {
                    this.MainButtonVisualsSingle.Icon.enabled = false;
                    this.MainButtonVisualsSingle.Text.enabled = true;
                    if (!string.IsNullOrEmpty(overrideButtonText))
                    {
                        this.MainButtonVisualsSingle.Text.text = StringExtensions.ToUpperLoca(overrideButtonText);
                    }
                    else if (!string.IsNullOrEmpty(mpe.ButtonText))
                    {
                        this.MainButtonVisualsSingle.Text.text = StringExtensions.ToUpperLoca(_.L(mpe.ButtonText, null, false));
                    }
                    else
                    {
                        this.MainButtonVisualsSingle.Text.text = null;
                    }
                }
            }
            this.MainButton.Button.interactable = mainButtonInteractable;
            if (mpe.ButtonBackground != null)
            {
                this.MainButton.Bg.sprite = PlayerView.Binder.SpriteResources.getSprite(mpe.ButtonBackground);
            }
            else
            {
                this.MainButton.Bg.sprite = this.m_defaultMainButtonBgSprite;
            }
            this.MainButton.Bg.material = !this.MainButton.Button.interactable ? PlayerView.Binder.DisabledUiMaterial : null;
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator150 iterator = new <preShowRoutine>c__Iterator150();
            iterator.parameter = parameter;
            iterator.targetMenuContentType = targetMenuContentType;
            iterator.<$>parameter = parameter;
            iterator.<$>targetMenuContentType = targetMenuContentType;
            iterator.<>f__this = this;
            return iterator;
        }

        public override void refreshTitle(string title, string additionalText1, string additionalText2)
        {
            this.TitleText.text = StringExtensions.ToUpperLoca(title);
        }

        public override void setCloseButtonVisibility(bool visible)
        {
            this.CloseButton.enabled = visible;
            this.CloseButton.targetGraphic.enabled = visible;
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator151 iterator = new <showRoutine>c__Iterator151();
            iterator.parameter = parameter;
            iterator.<$>parameter = parameter;
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
                return PlayerView.MenuType.MiniPopupMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator152 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal MiniPopupMenu <>f__this;
            internal float <easedV>__2;
            internal ManualTimer <timer>__1;
            internal TransformAnimationTask <tt>__0;
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
                        if (!this.instant)
                        {
                            this.<tt>__0 = new TransformAnimationTask(this.<>f__this.m_panelTransformAnimation.transform, ConfigUi.POPUP_TRANSITION_DURATION_OUT, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                            this.<tt>__0.scale((Vector3) (Vector3.one * ConfigUi.POPUP_TRANSITION_SCALE), true, ConfigUi.POPUP_EASING_OUT);
                            this.<>f__this.m_panelTransformAnimation.addTask(this.<tt>__0);
                            this.<>f__this.BackgroundOverlay.fadeToTransparent(ConfigUi.POPUP_TRANSITION_DURATION_OUT, Easing.Function.LINEAR);
                            this.<timer>__1 = new ManualTimer(ConfigUi.POPUP_TRANSITION_DURATION_OUT);
                            break;
                        }
                        this.<>f__this.BackgroundOverlay.setTransparent(true);
                        goto Label_0168;

                    case 1:
                        break;

                    case 2:
                        goto Label_0153;

                    default:
                        goto Label_01E0;
                }
                while (!this.<timer>__1.Idle)
                {
                    this.<easedV>__2 = Easing.Apply(this.<timer>__1.normalizedProgress(), Easing.Function.LINEAR);
                    this.<>f__this.PanelRoot.alpha = 1f - this.<easedV>__2;
                    this.<timer>__1.tick(Time.deltaTime / Time.timeScale);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01E2;
                }
            Label_0153:
                while (this.<>f__this.BackgroundOverlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01E2;
                }
            Label_0168:
                this.<>f__this.PanelRoot.transform.localScale = Vector3.zero;
                this.<>f__this.cleanupCells();
                this.<>f__this.m_content.cleanup();
                this.<>f__this.m_content.transform.SetParent(this.<>f__this.DisabledContentTm, false);
                this.<>f__this.m_content.gameObject.SetActive(false);
                goto Label_01E0;
                this.$PC = -1;
            Label_01E0:
                return false;
            Label_01E2:
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
        private sealed class <preShowRoutine>c__Iterator150 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal MenuContentType <$>targetMenuContentType;
            internal MiniPopupMenu <>f__this;
            internal GameObject <contentObj>__0;
            internal object parameter;
            internal MenuContentType targetMenuContentType;

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
                    if (this.parameter != null)
                    {
                        this.<>f__this.InputParams = (MiniPopupMenu.InputParameters) this.parameter;
                    }
                    else
                    {
                        this.<>f__this.InputParams = new MiniPopupMenu.InputParameters();
                    }
                    this.<>f__this.MainButton.gameObject.SetActive(true);
                    this.<contentObj>__0 = PlayerView.Binder.MenuContentResources.getSharedResource(this.targetMenuContentType);
                    this.<contentObj>__0.transform.SetParent(this.<>f__this.ContentAreaTm, false);
                    this.<>f__this.m_content = this.<contentObj>__0.GetComponent<MenuContent>();
                    this.<>f__this.m_content.preShow(this.<>f__this, this.<>f__this.InputParams.MenuContentParams);
                    this.<contentObj>__0.SetActive(true);
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
        private sealed class <showRoutine>c__Iterator151 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal MiniPopupMenu <>f__this;
            internal float <easedV>__4;
            internal IEnumerator <ie>__0;
            internal bool <instant>__1;
            internal ManualTimer <timer>__3;
            internal TransformAnimationTask <tt>__2;
            internal object parameter;

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
                        PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_Popup, (float) 0f);
                        this.<>f__this.PanelRoot.transform.localScale = Vector3.one;
                        this.<ie>__0 = this.<>f__this.m_content.show(this.parameter);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_01FF;

                    default:
                        goto Label_0230;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    goto Label_0232;
                }
                this.<instant>__1 = false;
                if (this.<instant>__1)
                {
                    this.<>f__this.PanelRoot.transform.localScale = Vector3.one;
                    this.<>f__this.BackgroundOverlay.fadeToBlack(0f, ConfigUi.MENU_BACKGROUND_ALPHA, Easing.Function.LINEAR);
                    this.<>f__this.PanelRoot.alpha = 1f;
                    goto Label_0230;
                }
                this.<>f__this.m_panelTransformAnimation.transform.localScale = (Vector3) (Vector3.one * ConfigUi.POPUP_TRANSITION_SCALE);
                this.<tt>__2 = new TransformAnimationTask(this.<>f__this.m_panelTransformAnimation.transform, ConfigUi.POPUP_TRANSITION_DURATION_IN, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__2.scale(Vector3.one, true, ConfigUi.MENU_EASING_OUT);
                this.<>f__this.m_panelTransformAnimation.addTask(this.<tt>__2);
                this.<>f__this.BackgroundOverlay.fadeToBlack(ConfigUi.POPUP_TRANSITION_DURATION_IN, ConfigUi.MENU_BACKGROUND_ALPHA, Easing.Function.LINEAR);
                this.<timer>__3 = new ManualTimer(ConfigUi.POPUP_TRANSITION_DURATION_IN);
            Label_01FF:
                while (!this.<timer>__3.Idle)
                {
                    this.<easedV>__4 = Easing.Apply(this.<timer>__3.normalizedProgress(), Easing.Function.LINEAR);
                    this.<>f__this.PanelRoot.alpha = this.<easedV>__4;
                    this.<timer>__3.tick(Time.deltaTime / Time.timeScale);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0232;
                }
                this.<>f__this.PanelRoot.alpha = 1f;
                goto Label_0230;
                this.$PC = -1;
            Label_0230:
                return false;
            Label_0232:
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

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public PathToShopType PathToShop;
            public System.Action CloseCallback;
            public Action<ShopEntry, PurchaseResult> PurchaseCallback;
            public object MenuContentParams;
        }
    }
}

