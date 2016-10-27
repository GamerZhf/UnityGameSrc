namespace PlayerView
{
    using App;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class FixedMenuShadow2 : Shadow
    {
        public static float Alpha = 0.1568628f;
        public static Vector2 FixedDistance = new Vector2(4f, -6f);

        protected override void Awake()
        {
            base.Awake();
            base.effectColor = new Color(0f, 0f, 0f, Alpha);
            base.effectDistance = FixedDistance;
            base.useGraphicAlpha = false;
            base.enabled = ConfigDevice.DeviceQuality() >= DeviceQualityType.High;
        }
    }
}

