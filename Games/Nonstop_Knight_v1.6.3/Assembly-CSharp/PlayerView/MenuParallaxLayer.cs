namespace PlayerView
{
    using System;
    using UnityEngine;

    public class MenuParallaxLayer : MonoBehaviour
    {
        private Vector2 m_originalAnchoredPosition;
        private RectTransform m_rectTm;
        public float ParallaxFactor;

        protected void Awake()
        {
            this.m_rectTm = base.GetComponent<RectTransform>();
            this.m_originalAnchoredPosition = this.m_rectTm.anchoredPosition;
        }

        public void move(float deltaY)
        {
            Vector2 anchoredPosition = this.m_rectTm.anchoredPosition;
            anchoredPosition.y += deltaY * this.ParallaxFactor;
            this.m_rectTm.anchoredPosition = anchoredPosition;
        }

        public void reset()
        {
            this.m_rectTm.anchoredPosition = this.m_originalAnchoredPosition;
        }
    }
}

