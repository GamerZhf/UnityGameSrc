namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class TurboClick : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler, IPointerExitHandler
    {
        [CompilerGenerated]
        private int <ClickCounter>k__BackingField;
        public const float CLICK_INTERVAL = 0.12f;
        public const float CLICK_INTERVAL_MIN = 0.0005f;
        public const float CLICK_INTERVAL_MODIFICATON_PER_CLICK = -0.02f;
        public const float INITIAL_DELAY = 0.15f;
        private Button m_button;
        private bool m_dragging;
        private float m_nextTurboClickTime;

        public event ClickCallback OnClick;

        protected void Awake()
        {
            this.m_button = base.GetComponent<Button>();
        }

        protected void OnDisable()
        {
            this.m_dragging = false;
            this.m_nextTurboClickTime = 0f;
        }

        protected void OnEnable()
        {
            this.m_dragging = false;
            this.m_nextTurboClickTime = 0f;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (this.m_button != null)
            {
                this.m_dragging = true;
                this.m_nextTurboClickTime = Time.unscaledTime + 0.15f;
                this.ClickCounter = 0;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (this.m_dragging)
            {
                this.m_nextTurboClickTime = Time.unscaledTime + 0.15f;
                this.ClickCounter = 0;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (this.m_dragging)
            {
                this.m_nextTurboClickTime = 0f;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (this.m_button != null)
            {
                this.m_dragging = false;
                this.m_nextTurboClickTime = 0f;
            }
        }

        protected void Update()
        {
            if ((this.m_dragging && (this.m_button != null)) && ((this.m_nextTurboClickTime > 0f) && (Time.unscaledTime >= this.m_nextTurboClickTime)))
            {
                if (this.OnClick != null)
                {
                    this.OnClick();
                }
                this.ClickCounter++;
                float num = Mathf.Max((float) (0.12f + (this.ClickCounter * -0.02f)), (float) 0.0005f);
                this.m_nextTurboClickTime = Time.unscaledTime + num;
            }
        }

        public int ClickCounter
        {
            [CompilerGenerated]
            get
            {
                return this.<ClickCounter>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ClickCounter>k__BackingField = value;
            }
        }

        public delegate void ClickCallback();
    }
}

