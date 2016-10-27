namespace PlayerView
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class MenuParallaxBackground : MonoBehaviour
    {
        private List<MenuParallaxLayer> m_parallaxLayers = new List<MenuParallaxLayer>();
        public CustomScrollRect ScrollRect;

        private void onScrollRectTranslated(float deltaY)
        {
            for (int i = 0; i < this.m_parallaxLayers.Count; i++)
            {
                this.m_parallaxLayers[i].move(deltaY);
            }
        }

        [ContextMenu("refresh()")]
        private void refresh()
        {
            if (this.ScrollRect == null)
            {
                this.ScrollRect = base.gameObject.GetComponentInParent<CustomScrollRect>();
            }
            if (this.ScrollRect != null)
            {
                this.ScrollRect.OnScrolled += new CustomScrollRect.Scrolled(this.onScrollRectTranslated);
            }
            this.setLayers();
        }

        public void reset()
        {
            for (int i = 0; i < this.m_parallaxLayers.Count; i++)
            {
                this.m_parallaxLayers[i].reset();
            }
        }

        private void setLayers()
        {
            this.m_parallaxLayers.Clear();
            for (int i = 0; i < base.transform.childCount; i++)
            {
                MenuParallaxLayer component = base.transform.GetChild(i).GetComponent<MenuParallaxLayer>();
                if (component != null)
                {
                    this.m_parallaxLayers.Add(component);
                }
            }
        }

        protected void Start()
        {
            this.refresh();
        }
    }
}

