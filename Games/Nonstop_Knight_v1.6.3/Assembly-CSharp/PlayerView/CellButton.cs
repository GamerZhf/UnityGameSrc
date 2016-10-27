namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(RectTransform))]
    public class CellButton : MonoBehaviour
    {
        [CompilerGenerated]
        private CellButtonType <ActiveType>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public Text ButtonHeader;
        public Image ButtonIcon;
        public Image ButtonImage;
        public Text ButtonLabel;
        public Image ButtonLabelIcon;
        public GameObject ButtonLabelRoot;
        public Text ButtonPlainLabel;
        public GameObject ButtonPlainLabelRoot;
        public Button ButtonScript;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
        }

        public void setActive(bool value)
        {
            this.ButtonScript.enabled = value;
            this.ButtonImage.enabled = value;
            this.ButtonIcon.enabled = value;
            this.ButtonLabel.enabled = value;
            this.ButtonPlainLabel.enabled = value;
            this.ButtonLabelIcon.enabled = value;
        }

        public void setCellButtonStyle(CellButtonType type)
        {
            this.ActiveType = type;
            switch (type)
            {
                case CellButtonType.Upgrade:
                    this.ButtonScript.interactable = true;
                    this.ButtonImage.sprite = Binder.SpriteResources.getSprite("Menu", "uiz_button_green");
                    this.ButtonImage.material = null;
                    this.ButtonIcon.sprite = Binder.SpriteResources.getSprite("Menu", "icon_mini_upgrade");
                    this.ButtonIcon.color = new Color(0.3843137f, 0.6039216f, 0.07450981f, 1f);
                    this.ButtonIcon.material = null;
                    break;

                case CellButtonType.UpgradeDisabled:
                    this.ButtonScript.interactable = false;
                    this.ButtonImage.sprite = Binder.SpriteResources.getSprite("Menu", "uiz_button_blue");
                    this.ButtonImage.material = Binder.DisabledUiMaterial;
                    this.ButtonIcon.sprite = Binder.SpriteResources.getSprite("Menu", "icon_mini_upgrade");
                    this.ButtonIcon.color = new Color(0.3647059f, 0.4039216f, 0.4431373f, 1f);
                    this.ButtonIcon.material = Binder.DisabledUiMaterial;
                    break;

                case CellButtonType.Unlock:
                    this.ButtonScript.interactable = true;
                    this.ButtonImage.sprite = Binder.SpriteResources.getSprite("Menu", "uiz_button_gold");
                    this.ButtonImage.material = null;
                    this.ButtonIcon.sprite = Binder.SpriteResources.getSprite("Menu", "icon_mini_plus");
                    this.ButtonIcon.color = new Color(1f, 0.627451f, 0.03137255f, 1f);
                    this.ButtonIcon.material = null;
                    break;

                case CellButtonType.UnlockDisabled:
                    this.ButtonScript.interactable = false;
                    this.ButtonImage.sprite = Binder.SpriteResources.getSprite("Menu", "uiz_button_blue");
                    this.ButtonImage.material = Binder.DisabledUiMaterial;
                    this.ButtonIcon.sprite = Binder.SpriteResources.getSprite("Menu", "icon_mini_plus");
                    this.ButtonIcon.color = new Color(0.3647059f, 0.4039216f, 0.4431373f, 1f);
                    this.ButtonIcon.material = Binder.DisabledUiMaterial;
                    break;

                case CellButtonType.UnlockLocked:
                    this.ButtonScript.interactable = false;
                    this.ButtonImage.sprite = Binder.SpriteResources.getSprite("Menu", "uiz_button_blue");
                    this.ButtonImage.material = Binder.DisabledUiMaterial;
                    this.ButtonIcon.sprite = Binder.SpriteResources.getSprite("Menu", "icon_mini_lock");
                    this.ButtonIcon.color = new Color(0.3647059f, 0.4039216f, 0.4431373f, 1f);
                    this.ButtonIcon.material = null;
                    break;

                case CellButtonType.Selected:
                    this.ButtonScript.interactable = true;
                    this.ButtonImage.sprite = Binder.SpriteResources.getSprite("Menu", "uiz_button_green");
                    this.ButtonImage.material = null;
                    this.ButtonIcon.sprite = Binder.SpriteResources.getSprite("Menu", "icon_mini_checkmark");
                    this.ButtonIcon.color = new Color(0.3843137f, 0.6039216f, 0.07450981f, 1f);
                    this.ButtonIcon.material = null;
                    break;

                case CellButtonType.SelectedDisabled:
                    this.ButtonScript.interactable = false;
                    this.ButtonImage.sprite = Binder.SpriteResources.getSprite("Menu", "uiz_button_blue");
                    this.ButtonImage.material = Binder.DisabledUiMaterial;
                    this.ButtonIcon.sprite = Binder.SpriteResources.getSprite("Menu", "icon_mini_checkmark");
                    this.ButtonIcon.color = new Color(0.3647059f, 0.4039216f, 0.4431373f, 1f);
                    this.ButtonIcon.material = Binder.DisabledUiMaterial;
                    break;

                case CellButtonType.Select:
                    this.ButtonScript.interactable = true;
                    this.ButtonImage.sprite = Binder.SpriteResources.getSprite("Menu", "uiz_button_blue");
                    this.ButtonImage.material = null;
                    this.ButtonIcon.sprite = Binder.SpriteResources.getSprite("Menu", "icon_mini_plus");
                    this.ButtonIcon.color = new Color(0.3058824f, 0.3411765f, 0.5960785f, 1f);
                    this.ButtonIcon.material = null;
                    break;

                case CellButtonType.Evolve:
                    this.ButtonScript.interactable = true;
                    this.ButtonImage.sprite = Binder.SpriteResources.getSprite("Menu", "uiz_button_gold");
                    this.ButtonImage.material = null;
                    this.ButtonIcon.sprite = Binder.SpriteResources.getSprite("Menu", "icon_mini_plus");
                    this.ButtonIcon.color = new Color(1f, 0.627451f, 0.03137255f, 1f);
                    this.ButtonIcon.material = null;
                    break;

                case CellButtonType.Drink:
                    this.ButtonScript.interactable = true;
                    this.ButtonImage.sprite = Binder.SpriteResources.getSprite("Menu", "uiz_button_green");
                    this.ButtonImage.material = null;
                    this.ButtonIcon.sprite = Binder.SpriteResources.getSprite("Menu", "icon_mini_plus");
                    this.ButtonIcon.color = new Color(0.3843137f, 0.6039216f, 0.07450981f, 1f);
                    this.ButtonIcon.material = null;
                    break;

                case CellButtonType.DrinkDisabled:
                    this.ButtonScript.interactable = false;
                    this.ButtonImage.sprite = Binder.SpriteResources.getSprite("Menu", "uiz_button_blue");
                    this.ButtonImage.material = Binder.DisabledUiMaterial;
                    this.ButtonIcon.sprite = Binder.SpriteResources.getSprite("Menu", "icon_mini_plus");
                    this.ButtonIcon.color = new Color(0.3647059f, 0.4039216f, 0.4431373f, 1f);
                    this.ButtonIcon.material = Binder.DisabledUiMaterial;
                    break;
            }
        }

        public void setCellButtonStyle(CellButtonType type, string label)
        {
            this.ButtonLabelRoot.SetActive(false);
            this.ButtonPlainLabelRoot.SetActive(true);
            this.ButtonPlainLabel.text = label;
            this.setCellButtonStyle(type);
        }

        public void setCellButtonStyle(CellButtonType type, string label, Sprite labelIcon)
        {
            this.ButtonLabelRoot.SetActive(true);
            this.ButtonPlainLabelRoot.SetActive(false);
            this.ButtonLabel.text = label;
            this.ButtonLabel.rectTransform.anchoredPosition3D = new Vector3(20f, -40f, 0f);
            this.ButtonLabelIcon.enabled = true;
            this.ButtonLabelIcon.sprite = labelIcon;
            this.setCellButtonStyle(type);
        }

        public CellButtonType ActiveType
        {
            [CompilerGenerated]
            get
            {
                return this.<ActiveType>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ActiveType>k__BackingField = value;
            }
        }

        public RectTransform RectTm
        {
            [CompilerGenerated]
            get
            {
                return this.<RectTm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RectTm>k__BackingField = value;
            }
        }
    }
}

