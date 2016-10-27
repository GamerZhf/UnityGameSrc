using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Extension]
public static class RectTransformExtensions
{
    [Extension]
    public static float GetHeight(RectTransform trans)
    {
        return trans.rect.height;
    }

    [Extension]
    public static Vector2 GetSize(RectTransform trans)
    {
        return trans.rect.size;
    }

    [Extension]
    public static float GetWidth(RectTransform trans)
    {
        return trans.rect.width;
    }

    [Extension]
    public static void SetBottom(RectTransform trans, float bottom)
    {
        trans.offsetMin = new Vector2(trans.offsetMin.x, bottom);
    }

    [Extension]
    public static void SetDefaultScale(RectTransform trans)
    {
        trans.localScale = new Vector3(1f, 1f, 1f);
    }

    [Extension]
    public static void SetHeight(RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(trans.rect.size.x, newSize));
    }

    [Extension]
    public static void SetLeftBottomPosition(RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    [Extension]
    public static void SetLeftTopPosition(RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    [Extension]
    public static void SetPivotAndAnchors(RectTransform trans, Vector2 aVec)
    {
        trans.pivot = aVec;
        trans.anchorMin = aVec;
        trans.anchorMax = aVec;
    }

    [Extension]
    public static void SetPositionOfPivot(RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
    }

    [Extension]
    public static void SetRightBottomPosition(RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    [Extension]
    public static void SetRightTopPosition(RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    [Extension]
    public static void SetSize(RectTransform trans, Vector2 newSize)
    {
        Vector2 size = trans.rect.size;
        Vector2 vector2 = newSize - size;
        trans.offsetMin -= new Vector2(vector2.x * trans.pivot.x, vector2.y * trans.pivot.y);
        trans.offsetMax += new Vector2(vector2.x * (1f - trans.pivot.x), vector2.y * (1f - trans.pivot.y));
    }

    [Extension]
    public static void SetTop(RectTransform trans, float top)
    {
        trans.offsetMax = new Vector2(trans.offsetMax.x, top);
    }

    [Extension]
    public static void SetWidth(RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(newSize, trans.rect.size.y));
    }
}

