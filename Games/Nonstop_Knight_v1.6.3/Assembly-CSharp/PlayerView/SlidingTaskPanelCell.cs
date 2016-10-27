namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class SlidingTaskPanelCell : MonoBehaviour
    {
        public Image Bg;
        public UnityEngine.UI.Button Button;
        public static Color DISABLED_COLOR = new Color(1f, 1f, 1f, 0.5f);
        public Image Icon;
        private System.Action m_clickCallback;
        public GameObject Notifier;
        public UnityEngine.UI.Text Text;

        public void initialize(Sprite icon, string text, bool stripedRow, System.Action clickCallback)
        {
            this.Icon.sprite = icon;
            this.Text.text = text;
            this.Bg.color = !stripedRow ? ConfigUi.LIST_CELL_REGULAR_COLOR : ConfigUi.LIST_CELL_STRIPED_COLOR;
            this.m_clickCallback = clickCallback;
            this.setNotifierState(false);
            this.setInteractable(true);
        }

        public void onClick()
        {
            this.m_clickCallback();
        }

        public void setInteractable(bool interactable)
        {
            this.Button.interactable = interactable;
            this.Icon.material = !interactable ? PlayerView.Binder.DisabledUiMaterial : null;
            this.Icon.color = !interactable ? DISABLED_COLOR : Color.white;
            this.Text.color = !interactable ? DISABLED_COLOR : Color.white;
        }

        public void setNotifierState(bool notify)
        {
            this.Notifier.SetActive(notify);
            this.Icon.gameObject.SetActive(!notify);
        }
    }
}

