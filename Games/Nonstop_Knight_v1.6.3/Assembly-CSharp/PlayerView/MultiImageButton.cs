namespace PlayerView
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class MultiImageButton : Button
    {
        private Graphic[] m_graphics;

        private void colorTween(Color targetColor, bool instant)
        {
            for (int i = 0; i < this.Graphics.Length; i++)
            {
                this.Graphics[i].CrossFadeColor(targetColor, instant ? 0f : base.colors.fadeDuration, true, true);
            }
        }

        protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
        {
            Color normalColor;
            switch (state)
            {
                case Selectable.SelectionState.Normal:
                    normalColor = base.colors.normalColor;
                    break;

                case Selectable.SelectionState.Highlighted:
                    normalColor = base.colors.highlightedColor;
                    break;

                case Selectable.SelectionState.Pressed:
                    normalColor = base.colors.pressedColor;
                    break;

                case Selectable.SelectionState.Disabled:
                    normalColor = base.colors.disabledColor;
                    break;

                default:
                    normalColor = Color.black;
                    break;
            }
            if (base.gameObject.activeInHierarchy)
            {
                if (base.transition == Selectable.Transition.ColorTint)
                {
                    this.colorTween((Color) (normalColor * base.colors.colorMultiplier), instant);
                }
                else
                {
                    base.DoStateTransition(state, instant);
                }
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

