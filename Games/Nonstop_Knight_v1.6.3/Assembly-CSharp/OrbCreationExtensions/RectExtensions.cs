namespace OrbCreationExtensions
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Extension]
    public static class RectExtensions
    {
        [Extension]
        public static float MaxExtents(Bounds b)
        {
            return Mathf.Max(Mathf.Max(b.extents.x, b.extents.y), b.extents.z);
        }

        [Extension]
        public static float MaxSize(Bounds b)
        {
            return Mathf.Max(Mathf.Max(b.size.x, b.size.y), b.size.z);
        }

        [Extension]
        public static float MinExtents(Bounds b)
        {
            return Mathf.Min(Mathf.Min(b.extents.x, b.extents.y), b.extents.z);
        }

        [Extension]
        public static float MinSize(Bounds b)
        {
            return Mathf.Min(Mathf.Min(b.size.x, b.size.y), b.size.z);
        }

        [Extension]
        public static bool MouseInRect(Rect rect)
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition.y = Screen.height - mousePosition.y;
            return MouseInRect(rect, mousePosition);
        }

        [Extension]
        public static bool MouseInRect(Rect rect, Rect parentRect)
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition.y = Screen.height - mousePosition.y;
            return MouseInRect(rect, parentRect, mousePosition);
        }

        [Extension]
        public static bool MouseInRect(Rect rect, Vector2 point)
        {
            return rect.Contains(point);
        }

        [Extension]
        public static bool MouseInRect(Rect rect, Rect parentRect1, Rect parentRect2)
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition.y = Screen.height - mousePosition.y;
            return MouseInRect(rect, parentRect1, parentRect2, mousePosition);
        }

        [Extension]
        public static bool MouseInRect(Rect rect, Rect parentRect, Vector2 point)
        {
            rect.x += parentRect.x;
            rect.y += parentRect.y;
            return MouseInRect(rect, point);
        }

        [Extension]
        public static bool MouseInRect(Rect rect, Rect parentRect1, Rect parentRect2, Vector2 point)
        {
            rect.x += parentRect1.x;
            rect.y += parentRect1.y;
            rect.x += parentRect2.x;
            rect.y += parentRect2.y;
            return MouseInRect(rect, point);
        }

        [Extension]
        public static Vector2 RelativeMousePosInRect(Rect rect)
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition.y = Screen.height - mousePosition.y;
            return RelativeMousePosInRect(rect, mousePosition);
        }

        [Extension]
        public static Vector2 RelativeMousePosInRect(Rect rect, Vector2 point)
        {
            Vector2 vector = new Vector2(-1f, -1f);
            if (rect.Contains(point))
            {
                vector.x = point.x - rect.x;
                if (rect.width > 0f)
                {
                    vector.x = Mathf.Abs((float) (vector.x / rect.width));
                }
                vector.y = point.y - rect.y;
                if (rect.height > 0f)
                {
                    vector.y = 1f - Mathf.Abs((float) (vector.y / rect.height));
                }
            }
            return vector;
        }

        [Extension]
        public static Rect RelativeRectInImage(Rect r, Texture2D img)
        {
            return new Rect(r.x / ((float) img.width), 1f - ((r.y + r.height) / ((float) img.height)), r.width / ((float) img.width), r.height / ((float) img.height));
        }
    }
}

