using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/ToJ Effects/Character Spacing", 7), RequireComponent(typeof(Text))]
public class CharacterSpacing : BaseVertexEffect
{
    [SerializeField]
    private float m_Offset;
    private const string REGEX_TAGS = "<b>|</b>|<i>|</i>|<size=.*?>|</size>|<color=.*?>|</color>|<material=.*?>|</material>";

    protected CharacterSpacing()
    {
    }

    private MatchCollection GetRegexMatchedTags(string text, out int lengthWithoutTags)
    {
        MatchCollection matchs = Regex.Matches(text, "<b>|</b>|<i>|</i>|<size=.*?>|</size>|<color=.*?>|</color>|<material=.*?>|</material>");
        lengthWithoutTags = 0;
        int num = 0;
        IEnumerator enumerator = matchs.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Match current = (Match) enumerator.Current;
                num += current.Length;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
        lengthWithoutTags = text.Length - num;
        return matchs;
    }

    public override void ModifyVertices(List<UIVertex> verts)
    {
        if (this.IsActive())
        {
            Text component = base.GetComponent<Text>();
            List<string> list = new List<string>();
            for (int i = 0; i < component.cachedTextGenerator.lineCount; i++)
            {
                UILineInfo info = component.cachedTextGenerator.lines[i];
                int startCharIdx = info.startCharIdx;
                int num3 = (i >= (component.cachedTextGenerator.lineCount - 1)) ? component.text.Length : component.cachedTextGenerator.lines[i + 1].startCharIdx;
                list.Add(component.text.Substring(startCharIdx, num3 - startCharIdx));
            }
            float num4 = (this.offset * component.fontSize) / 100f;
            float num5 = 0f;
            IEnumerator enumerator = null;
            Match current = null;
            if (((component.alignment == TextAnchor.LowerLeft) || (component.alignment == TextAnchor.MiddleLeft)) || (component.alignment == TextAnchor.UpperLeft))
            {
                num5 = 0f;
            }
            else if (((component.alignment == TextAnchor.LowerCenter) || (component.alignment == TextAnchor.MiddleCenter)) || (component.alignment == TextAnchor.UpperCenter))
            {
                num5 = 0.5f;
            }
            else if (((component.alignment == TextAnchor.LowerRight) || (component.alignment == TextAnchor.MiddleRight)) || (component.alignment == TextAnchor.UpperRight))
            {
                num5 = 1f;
            }
            bool flag = true;
            int num6 = 0;
            for (int j = 0; (j < list.Count) && flag; j++)
            {
                string text = list[j];
                int length = text.Length;
                if (length > (component.cachedTextGenerator.characterCountVisible - num6))
                {
                    length = component.cachedTextGenerator.characterCountVisible - num6;
                    text = text.Substring(0, length) + " ";
                    length++;
                }
                if (component.supportRichText)
                {
                    enumerator = this.GetRegexMatchedTags(text, out length).GetEnumerator();
                    current = null;
                    if (enumerator.MoveNext())
                    {
                        current = (Match) enumerator.Current;
                    }
                }
                bool flag2 = (list[j].Length > 0) && ((list[j][list[j].Length - 1] == ' ') || (list[j][list[j].Length - 1] == '\n'));
                float num9 = (-((length - 1) - (!flag2 ? 0 : 1)) * num4) * num5;
                float num10 = 0f;
                for (int k = 0; (k < text.Length) && flag; k++)
                {
                    if ((component.supportRichText && (current != null)) && (current.Index == k))
                    {
                        k += current.Length - 1;
                        num6 += current.Length - 1;
                        num10--;
                        current = null;
                        if (enumerator.MoveNext())
                        {
                            current = (Match) enumerator.Current;
                        }
                    }
                    if (((num6 * 4) + 4) >= verts.Count)
                    {
                        flag = false;
                        break;
                    }
                    for (int m = 0; m < 4; m++)
                    {
                        UIVertex vertex = verts[(num6 * 4) + m];
                        vertex.position += (Vector3) (Vector3.right * ((num4 * num10) + num9));
                        verts[(num6 * 4) + m] = vertex;
                    }
                    num6++;
                    num10++;
                }
            }
        }
    }

    public float offset
    {
        get
        {
            return this.m_Offset;
        }
        set
        {
            if (this.m_Offset != value)
            {
                this.m_Offset = value;
                if (base.graphic != null)
                {
                    base.graphic.SetVerticesDirty();
                }
            }
        }
    }
}

