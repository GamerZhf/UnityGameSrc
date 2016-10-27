using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputFieldCaretFix : MonoBehaviour, IEventSystemHandler, ISelectHandler
{
    private bool m_isFixed;
    public Vector2 offset;

    public void OnSelect(BaseEventData eventData)
    {
        if (!this.m_isFixed)
        {
            RectTransform component = base.transform.Find(base.gameObject.name + " Input Caret").GetComponent<RectTransform>();
            component.anchoredPosition += this.offset;
            this.m_isFixed = true;
        }
    }
}

