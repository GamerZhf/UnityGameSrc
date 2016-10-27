namespace PlayerView
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class MultiImageToggle : Toggle
    {
        private Graphic[] m_graphics;
        private bool m_previouslyOn;

        private void colorTween(Color targetColor, bool instant)
        {
            for (int i = 0; i < this.Graphics.Length; i++)
            {
                this.Graphics[i].CrossFadeColor(targetColor, instant ? 0f : base.colors.fadeDuration, true, true);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.m_previouslyOn = false;
            this.refresh(true);
        }

        private void refresh(bool instant)
        {
            Color pressedColor;
            if (base.isOn)
            {
                pressedColor = base.colors.pressedColor;
            }
            else
            {
                pressedColor = base.colors.normalColor;
            }
            if (base.gameObject.activeInHierarchy && (base.transition == Selectable.Transition.ColorTint))
            {
                this.colorTween((Color) (pressedColor * base.colors.colorMultiplier), instant);
            }
            this.m_previouslyOn = base.isOn;
        }

        protected void Update()
        {
            if (base.isOn != this.m_previouslyOn)
            {
                this.refresh(false);
            }
        }

        protected Graphic[] Graphics
        {
            get
            {
                if (this.m_graphics == null)
                {
                    this.m_graphics = base.transform.GetComponentsInChildren<Graphic>();
                }
                return this.m_graphics;
            }
        }
    }
}

