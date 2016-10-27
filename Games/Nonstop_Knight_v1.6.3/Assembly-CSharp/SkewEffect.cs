using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text)), AddComponentMenu("UI/ToJ Effects/Skew Effect", 5)]
public class SkewEffect : BaseVertexEffect
{
    [SerializeField]
    private Vector2 m_LowerLeftOffset = Vector2.zero;
    [SerializeField]
    private Vector2 m_LowerRightOffset = Vector2.zero;
    [SerializeField]
    private Vector2 m_UpperLeftOffset = Vector2.zero;
    [SerializeField]
    private Vector2 m_UpperRightOffset = Vector2.zero;

    protected SkewEffect()
    {
    }

    public override void ModifyVertices(List<UIVertex> verts)
    {
        if (this.IsActive() && (verts.Count != 0))
        {
            UIVertex vertex2 = verts[0];
            Vector2 position = vertex2.position;
            UIVertex vertex3 = verts[verts.Count - 1];
            Vector2 vector2 = vertex3.position;
            for (int i = 0; i < verts.Count; i++)
            {
                UIVertex vertex4 = verts[i];
                if (vertex4.position.x < position.x)
                {
                    UIVertex vertex5 = verts[i];
                    position.x = vertex5.position.x;
                }
                UIVertex vertex6 = verts[i];
                if (vertex6.position.y > position.y)
                {
                    UIVertex vertex7 = verts[i];
                    position.y = vertex7.position.y;
                }
                UIVertex vertex8 = verts[i];
                if (vertex8.position.x > vector2.x)
                {
                    UIVertex vertex9 = verts[i];
                    vector2.x = vertex9.position.x;
                }
                UIVertex vertex10 = verts[i];
                if (vertex10.position.y < vector2.y)
                {
                    UIVertex vertex11 = verts[i];
                    vector2.y = vertex11.position.y;
                }
            }
            float num2 = position.y - vector2.y;
            float num3 = vector2.x - position.x;
            for (int j = 0; j < verts.Count; j++)
            {
                UIVertex vertex = verts[j];
                float num5 = (vertex.position.y - vector2.y) / num2;
                float num6 = 1f - num5;
                float num7 = (vector2.x - vertex.position.x) / num3;
                float num8 = 1f - num7;
                Vector3 zero = Vector3.zero;
                zero.y = (((this.upperLeftOffset.y * num5) + (this.lowerLeftOffset.y * num6)) * num7) + (((this.upperRightOffset.y * num5) + (this.lowerRightOffset.y * num6)) * num8);
                zero.x = (((this.upperLeftOffset.x * num7) + (this.upperRightOffset.x * num8)) * num5) + (((this.lowerLeftOffset.x * num7) + (this.lowerRightOffset.x * num8)) * num6);
                vertex.position += zero;
                verts[j] = vertex;
            }
        }
    }

    public Vector2 lowerLeftOffset
    {
        get
        {
            return this.m_LowerLeftOffset;
        }
        set
        {
            this.m_LowerLeftOffset = value;
            if (base.graphic != null)
            {
                base.graphic.SetVerticesDirty();
            }
        }
    }

    public Vector2 lowerRightOffset
    {
        get
        {
            return this.m_LowerRightOffset;
        }
        set
        {
            this.m_LowerRightOffset = value;
            if (base.graphic != null)
            {
                base.graphic.SetVerticesDirty();
            }
        }
    }

    public Vector2 upperLeftOffset
    {
        get
        {
            return this.m_UpperLeftOffset;
        }
        set
        {
            this.m_UpperLeftOffset = value;
            if (base.graphic != null)
            {
                base.graphic.SetVerticesDirty();
            }
        }
    }

    public Vector2 upperRightOffset
    {
        get
        {
            return this.m_UpperRightOffset;
        }
        set
        {
            this.m_UpperRightOffset = value;
            if (base.graphic != null)
            {
                base.graphic.SetVerticesDirty();
            }
        }
    }
}

