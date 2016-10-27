namespace PlayerView
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class GroundView : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
    {
        private RoomView m_roomView;

        public void initialize(RoomView roomView)
        {
            this.m_roomView = roomView;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            this.m_roomView.onGroundClick(eventData.pointerPressRaycast.worldPosition);
        }
    }
}

