namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class CustomScrollRect : ScrollRect
    {
        private Vector2 m_prevNormalizedPos = Vector2.zero;
        private RectTransform m_rectTm;

        public event Scrolled OnScrolled;

        protected override void Awake()
        {
            base.Awake();
            this.m_rectTm = base.GetComponent<RectTransform>();
        }

        protected override void Start()
        {
            base.Start();
            base.normalizedPosition = new Vector2(0.5f, 0f);
        }

        protected void Update()
        {
            if (base.normalizedPosition != this.m_prevNormalizedPos)
            {
                if (this.OnScrolled != null)
                {
                    Vector2 vector = base.normalizedPosition - this.m_prevNormalizedPos;
                    float deltaY = vector.y * this.m_rectTm.rect.height;
                    this.OnScrolled(deltaY);
                }
                this.m_prevNormalizedPos = base.normalizedPosition;
            }
        }

        public delegate void Scrolled(float deltaY);
    }
}

