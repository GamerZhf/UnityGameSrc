namespace PlayerView
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class FixedMenuShadowStars : Shadow
    {
        public static float Alpha = 0.2352941f;
        public static Vector2 FixedDistance = new Vector2(4f, -4f);

        protected override void Awake()
        {
            base.Awake();
            base.effectColor = new Color(0f, 0f, 0f, Alpha);
            base.effectDistance = FixedDistance;
            base.useGraphicAlpha = false;
        }
    }
}

