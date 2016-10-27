namespace PlayerView
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class UiUtil
    {
        public static float CalculateScrollRectVerticalNormalizedPosition(float targetCenterPos, float totalContentHeight, float viewRectHeight)
        {
            float num = viewRectHeight * 0.5f;
            float num3 = Mathf.Clamp01(Mathf.Clamp((targetCenterPos - viewRectHeight) + num, 0f, float.MaxValue) / (totalContentHeight - viewRectHeight));
            return (1f - num3);
        }

        public static void LimitToParentBounds(RectTransform rectTm, RectTransform parentRectTm, [Optional, DefaultParameterValue(0f)] float marginX, [Optional, DefaultParameterValue(0f)] float marginY)
        {
            Vector2 vector2;
            Vector2 vector = (Vector2) (rectTm.sizeDelta * 0.5f);
            Rect rect = parentRectTm.rect;
            vector2.x = Mathf.Clamp(rectTm.anchoredPosition.x, (-(rect.width * 0.5f) + marginX) + vector.x, ((rect.width * 0.5f) - marginX) - vector.x);
            vector2.y = Mathf.Clamp(rectTm.anchoredPosition.y, (-(rect.height * 0.5f) + marginY) + vector.y, ((rect.height * 0.5f) - marginY) - vector.y);
            rectTm.anchoredPosition = vector2;
        }
    }
}

