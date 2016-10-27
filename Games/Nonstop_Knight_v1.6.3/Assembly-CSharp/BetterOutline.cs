using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/ToJ Effects/Better Outline", 0), RequireComponent(typeof(Text))]
public class BetterOutline : Shadow
{
    protected BetterOutline()
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
            int count = verts.Count;
            int start = 0;
            int num3 = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if ((i != 0) && (j != 0))
                    {
                        start = num3;
                        num3 = verts.Count;
                        this.ApplyShadowZeroAlloc(verts, base.effectColor, start, verts.Count, (i * base.effectDistance.x) * 0.707f, (j * base.effectDistance.y) * 0.707f);
                    }
                }
            }
            start = num3;
            num3 = verts.Count;
            this.ApplyShadowZeroAlloc(verts, base.effectColor, start, verts.Count, -base.effectDistance.x, 0f);
            start = num3;
            num3 = verts.Count;
            this.ApplyShadowZeroAlloc(verts, base.effectColor, start, verts.Count, base.effectDistance.x, 0f);
            start = num3;
            num3 = verts.Count;
            this.ApplyShadowZeroAlloc(verts, base.effectColor, start, verts.Count, 0f, -base.effectDistance.y);
            start = num3;
            num3 = verts.Count;
            this.ApplyShadowZeroAlloc(verts, base.effectColor, start, verts.Count, 0f, base.effectDistance.y);
            if (base.GetComponent<Text>().material.shader == Shader.Find("Text Effects/Fancy Text"))
            {
                for (int k = 0; k < (verts.Count - count); k++)
                {
                    UIVertex vertex = verts[k];
                    vertex.uv1 = new Vector2(0f, 0f);
                    verts[k] = vertex;
                }
            }
        }
    }
}

