namespace PlayerView
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class DragDropTarget : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IDropHandler
    {
        public Image ContainerImage;
        public Color HighlightColor = Color.green;
        private Color m_normalColor;
        public Image ReceivingImage;

        protected void Awake()
        {
            if (this.ContainerImage != null)
            {
                this.m_normalColor = this.ContainerImage.color;
            }
            this.onAwake();
        }

        protected virtual bool dropAllowed(GameObject dragObject)
        {
            return true;
        }

        private Sprite getSpriteFromDragObject(GameObject dragObject)
        {
            if (dragObject == null)
            {
                return null;
            }
            Image component = dragObject.GetComponent<Image>();
            if (component == null)
            {
                return null;
            }
            return component.sprite;
        }

        protected virtual void onAwake()
        {
        }

        protected virtual void onDrop(GameObject dragObject)
        {
        }

        public void OnDrop(PointerEventData data)
        {
            GameObject pointerDrag = data.pointerDrag;
            if (this.dropAllowed(pointerDrag))
            {
                this.ContainerImage.color = this.m_normalColor;
                if (this.ReceivingImage != null)
                {
                    Sprite sprite = this.getSpriteFromDragObject(pointerDrag);
                    if (sprite != null)
                    {
                        this.ReceivingImage.enabled = true;
                        this.ReceivingImage.overrideSprite = sprite;
                    }
                }
                this.onDrop(pointerDrag);
            }
        }

        protected virtual void onPointerEnter(GameObject dragObject)
        {
        }

        public void OnPointerEnter(PointerEventData data)
        {
            GameObject pointerDrag = data.pointerDrag;
            if ((this.ContainerImage != null) && (this.getSpriteFromDragObject(pointerDrag) != null))
            {
                this.ContainerImage.color = this.HighlightColor;
            }
            this.onPointerEnter(pointerDrag);
        }

        protected virtual void onPointerExit(GameObject dragObject)
        {
        }

        public void OnPointerExit(PointerEventData data)
        {
            if (this.ContainerImage != null)
            {
                this.ContainerImage.color = this.m_normalColor;
            }
            this.onPointerExit(data.pointerDrag);
        }
    }
}

