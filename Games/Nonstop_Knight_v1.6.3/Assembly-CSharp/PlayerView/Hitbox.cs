namespace PlayerView
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class Hitbox : Image
    {
        protected override void Awake()
        {
            base.Awake();
            this.refresh();
        }

        public void refresh()
        {
            base.sprite = null;
            base.overrideSprite = null;
            base.color = (Color) (Color.white * 0f);
            RectTransform component = base.GetComponent<RectTransform>();
            component.anchorMax = Vector2.one;
            component.anchorMin = Vector2.zero;
            component.pivot = (Vector2) (Vector2.one * 0.5f);
        }
    }
}

