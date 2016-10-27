using System;
using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public struct TKRect
{
    public float x;
    public float y;
    public float width;
    public float height;
    public TKRect(float x, float y, float width, float height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.updateRectWithRuntimeScaleModifier();
    }

    public TKRect(float width, float height, Vector2 center)
    {
        this.width = width;
        this.height = height;
        this.x = center.x - (width / 2f);
        this.y = center.y - (height / 2f);
        this.updateRectWithRuntimeScaleModifier();
    }

    public float xMin
    {
        get
        {
            return this.x;
        }
    }
    public float xMax
    {
        get
        {
            return (this.x + this.width);
        }
    }
    public float yMin
    {
        get
        {
            return this.y;
        }
    }
    public float yMax
    {
        get
        {
            return (this.y + this.height);
        }
    }
    public Vector2 center
    {
        get
        {
            return new Vector2(this.x + (this.width / 2f), this.y + (this.height / 2f));
        }
    }
    private void updateRectWithRuntimeScaleModifier()
    {
        Vector2 runtimeScaleModifier = TouchKit.instance.runtimeScaleModifier;
        this.x *= runtimeScaleModifier.x;
        this.y *= runtimeScaleModifier.y;
        this.width *= runtimeScaleModifier.x;
        this.height *= runtimeScaleModifier.y;
    }

    public TKRect copyWithExpansion(float allSidesExpansion)
    {
        return this.copyWithExpansion(allSidesExpansion, allSidesExpansion);
    }

    public TKRect copyWithExpansion(float xExpansion, float yExpansion)
    {
        xExpansion *= TouchKit.instance.runtimeScaleModifier.x;
        yExpansion *= TouchKit.instance.runtimeScaleModifier.y;
        TKRect rect = new TKRect();
        rect.x = this.x - xExpansion;
        rect.y = this.y - yExpansion;
        rect.width = this.width + (xExpansion * 2f);
        rect.height = this.height + (yExpansion * 2f);
        return rect;
    }

    public bool contains(Vector2 point)
    {
        return (((this.x <= point.x) && (this.y <= point.y)) && ((this.xMax >= point.x) && (this.yMax >= point.y)));
    }

    public override string ToString()
    {
        object[] args = new object[] { this.x, this.xMax, this.y, this.yMax, this.width, this.height, this.center };
        return string.Format("TKRect: x: {0}, xMax: {1}, y: {2}, yMax: {3}, width: {4}, height: {5}, center: {6}", args);
    }
}

