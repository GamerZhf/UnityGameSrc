namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class DragDropSource : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
    {
        [CompilerGenerated]
        private GameObject <DragObject>k__BackingField;
        public Vector3 DragObjectLocalScale = ((Vector3) (Vector3.one * 0.75f));
        public bool DragOnSurfaces = true;
        private RectTransform m_dragPlane;
        private Canvas m_parentCanvas;

        public event DragEnded OnDragEnded;

        public event DragStarted OnDragStarted;

        protected void Awake()
        {
            this.onAwake();
        }

        protected virtual bool dragAllowed()
        {
            return true;
        }

        protected virtual void onAwake()
        {
        }

        protected virtual void onBeginDrag(GameObject dragObject)
        {
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (this.dragAllowed())
            {
                if (this.m_parentCanvas == null)
                {
                    this.m_parentCanvas = base.transform.GetComponentInParent<Canvas>();
                    if (this.m_parentCanvas == null)
                    {
                        Debug.LogError("Parent Canvas not found.");
                        return;
                    }
                }
                this.DragObject = new GameObject("DraggedObject");
                this.DragObject.transform.SetParent(this.m_parentCanvas.transform, false);
                this.DragObject.transform.SetAsLastSibling();
                this.DragObject.transform.localScale = this.DragObjectLocalScale;
                Image image = this.DragObject.AddComponent<Image>();
                this.DragObject.AddComponent<CanvasGroup>().blocksRaycasts = false;
                image.sprite = base.GetComponent<Image>().sprite;
                image.SetNativeSize();
                if (this.DragOnSurfaces)
                {
                    this.m_dragPlane = base.transform as RectTransform;
                }
                else
                {
                    this.m_dragPlane = this.m_parentCanvas.transform as RectTransform;
                }
                this.setDraggedPosition(eventData);
                this.onBeginDrag(this.DragObject);
                if (this.OnDragStarted != null)
                {
                    this.OnDragStarted(eventData);
                }
            }
        }

        public void OnDrag(PointerEventData data)
        {
            if ((data != null) && (this.DragObject != null))
            {
                this.setDraggedPosition(data);
            }
        }

        protected virtual void onEndDrag(GameObject dragObject)
        {
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.onEndDrag(this.DragObject);
            if (this.OnDragEnded != null)
            {
                this.OnDragEnded(eventData);
            }
            if (this.DragObject != null)
            {
                UnityEngine.Object.Destroy(this.DragObject);
            }
        }

        private void setDraggedPosition(PointerEventData data)
        {
            Vector3 vector;
            if ((this.DragOnSurfaces && (data.pointerEnter != null)) && (data.pointerEnter.transform is RectTransform))
            {
                this.m_dragPlane = data.pointerEnter.transform as RectTransform;
            }
            RectTransform component = this.DragObject.GetComponent<RectTransform>();
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.m_dragPlane, data.position, data.pressEventCamera, out vector))
            {
                component.position = vector;
                component.rotation = this.m_dragPlane.rotation;
            }
        }

        protected GameObject DragObject
        {
            [CompilerGenerated]
            get
            {
                return this.<DragObject>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<DragObject>k__BackingField = value;
            }
        }

        public delegate void DragEnded(PointerEventData pointerEventData);

        public delegate void DragStarted(PointerEventData pointerEventData);
    }
}

