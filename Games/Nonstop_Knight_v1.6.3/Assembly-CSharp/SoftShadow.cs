using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/ToJ Effects/Soft Shadow", 3), RequireComponent(typeof(Text))]
public class SoftShadow : Shadow
{
    [SerializeField]
    private float m_BlurSpread = 1f;

    protected SoftShadow()
    {
    }

    protected void ApplyShadowZeroAlloc(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
    {
        int num = verts.Count * 2;
        if (verts.Capacity < num)
        {
            verts.Capacity = num;
        }
        for (int i = start; i < end; i++)
        {
            UIVertex item = verts[i];
            verts.Add(item);
            Vector3 position = item.position;
            position.x += x;
            position.y += y;
            item.position = position;
            Color32 color2 = color;
            if (base.useGraphicAlpha)
            {
                UIVertex vertex2 = verts[i];
                color2.a = (byte) ((color2.a * vertex2.color.a) / 0xff);
            }
            item.color = color2;
            verts[i] = item;
        }
    }

    public override void ModifyVertices(List<UIVertex> verts)
    {
        if (this.IsActive())
        {
            bool flag = false;
            int count = verts.Count;
            Text component = base.GetComponent<Text>();
            List<UIVertex> range = new List<UIVertex>();
            if (flag)
            {
                range = verts.GetRange(count - (component.cachedTextGenerator.characterCountVisible * 6), component.cachedTextGenerator.characterCountVisible * 6);
            }
            else
            {
                range = verts;
            }
            Color effectColor = base.effectColor;
            effectColor.a /= 4f;
            int start = 0;
            int num3 = range.Count;
            this.ApplyShadowZeroAlloc(range, effectColor, start, range.Count, base.effectDistance.x, base.effectDistance.y);
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if ((i != 0) || (j != 0))
                    {
                        start = num3;
                        num3 = range.Count;
                        this.ApplyShadowZeroAlloc(range, effectColor, start, range.Count, base.effectDistance.x + (i * this.blurSpread), base.effectDistance.y + (j * this.blurSpread));
                    }
                }
            }
            if (flag)
            {
                range.RemoveRange(range.Count - (component.cachedTextGenerator.characterCountVisible * 6), component.cachedTextGenerator.characterCountVisible * 6);
                range.AddRange(verts);
            }
            if (component.material.shader == Shader.Find("Text Effects/Fancy Text"))
            {
                for (int k = 0; k < (range.Count - count); k++)
                {
                    UIVertex vertex = range[k];
                    vertex.uv1 = new Vector2(0f, 0f);
                    range[k] = vertex;
                }
            }
        }
    }

    public float blurSpread
    {
        get
        {
            return this.m_BlurSpread;
        }
        set
        {
            this.m_BlurSpread = value;
            if (base.graphic != null)
            {
                base.graphic.SetVerticesDirty();
            }
        }
    }
}

