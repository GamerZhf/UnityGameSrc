namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class MenuDragHandler : MonoBehaviour, IDragHandler, IEventSystemHandler
    {
        public event Dragged OnDragged;

        public void OnDrag(PointerEventData eventData)
        {
            if ((eventData != null) && (this.OnDragged != null))
            {
                this.OnDragged(eventData);
            }
        }

        public delegate void Dragged(PointerEventData eventData);
    }
}

