namespace PlayerView
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class MaskedProgressBar : MonoBehaviour
    {
        public RectTransform Background;
        public RectTransform Foreground;
        public float ForegroundMargin;
        public RectTransform ForegroundMask;

        public void refresh()
        {
            Slider slider = GameObjectExtensions.AddOrGetComponent<Slider>(base.gameObject);
            slider.normalizedValue = 1f;
            if (this.Background != null)
            {
                this.Background.anchorMin = Vector2.zero;
                this.Background.anchorMax = Vector2.one;
                this.Background.pivot = (Vector2) (Vector2.one * 0.5f);
                this.Background.localRotation = Quaternion.identity;
                this.Background.localScale = Vector3.one;
            }
            if (this.ForegroundMask != null)
            {
                slider.fillRect = this.ForegroundMask;
                this.ForegroundMask.pivot = (Vector2) (Vector2.one * 0.5f);
                this.ForegroundMask.localRotation = Quaternion.identity;
                this.ForegroundMask.localScale = Vector3.one;
                this.ForegroundMask.anchoredPosition3D = Vector3.zero;
                this.ForegroundMask.sizeDelta = Vector2.zero;
                GameObjectExtensions.AddOrGetComponent<Mask>(this.ForegroundMask.gameObject).showMaskGraphic = false;
                Image image = GameObjectExtensions.AddOrGetComponent<Image>(this.ForegroundMask.gameObject);
                image.sprite = null;
                image.color = new Color(0.9882353f, 0.2588235f, 0.8470588f, 0.3843137f);
            }
            if (this.Foreground != null)
            {
                this.Foreground.anchorMin = Vector2.zero;
                this.Foreground.anchorMax = Vector2.one;
                this.Foreground.pivot = new Vector2(0f, 0.5f);
                this.Foreground.localRotation = Quaternion.identity;
                this.Foreground.localScale = Vector3.one;
                this.Foreground.anchoredPosition3D = new Vector3(this.ForegroundMargin, 0f, 0f);
                RectTransformExtensions.SetTop(this.Foreground, -this.ForegroundMargin);
                RectTransformExtensions.SetBottom(this.Foreground, this.ForegroundMargin);
                LayoutElement element = GameObjectExtensions.AddOrGetComponent<LayoutElement>(this.Foreground.gameObject);
                if ((this.ForegroundMask != null) && (this.ForegroundMask.rect.width > 0f))
                {
                    element.minWidth = this.ForegroundMask.rect.width - (this.ForegroundMargin * 2f);
                }
                ContentSizeFitter fitter = GameObjectExtensions.AddOrGetComponent<ContentSizeFitter>(this.Foreground.gameObject);
                fitter.horizontalFit = ContentSizeFitter.FitMode.MinSize;
                fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
                GameObjectExtensions.AddOrGetComponent<Image>(this.Foreground.gameObject);
            }
        }
    }
}

