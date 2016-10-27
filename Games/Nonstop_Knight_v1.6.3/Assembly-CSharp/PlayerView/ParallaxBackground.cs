namespace PlayerView
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [ExecuteInEditMode]
    public class ParallaxBackground : MonoBehaviour
    {
        private List<ParallaxLayer> m_parallaxLayers = new List<ParallaxLayer>();
        public PlayerView.ParallaxCamera ParallaxCamera;

        [ContextMenu("initialize()")]
        public void initialize()
        {
            this.initialize(null, true);
        }

        public void initialize(PlayerView.ParallaxCamera pxCamera, bool layersVisible)
        {
            this.ParallaxCamera = pxCamera;
            if (this.ParallaxCamera == null)
            {
                this.ParallaxCamera = Camera.main.GetComponent<PlayerView.ParallaxCamera>();
            }
            if (this.ParallaxCamera != null)
            {
                this.ParallaxCamera.OnCameraTranslated = new PlayerView.ParallaxCamera.ParallaxCameraTranslateCallback(this.onParallaxCameraTranslated);
            }
            this.initializeLayers(layersVisible);
        }

        private void initializeLayers(bool layersVisible)
        {
            this.m_parallaxLayers.Clear();
            for (int i = 0; i < base.transform.childCount; i++)
            {
                ParallaxLayer component = base.transform.GetChild(i).GetComponent<ParallaxLayer>();
                if (component != null)
                {
                    if (!layersVisible)
                    {
                        component.gameObject.SetActive(false);
                    }
                    else
                    {
                        component.gameObject.SetActive(true);
                        component.initialize();
                        this.m_parallaxLayers.Add(component);
                    }
                }
            }
        }

        private void onParallaxCameraTranslated(Vector3 delta)
        {
            for (int i = 0; i < this.m_parallaxLayers.Count; i++)
            {
                this.m_parallaxLayers[i].move(delta);
            }
        }

        public void setMaterialColor(Color materialColor)
        {
            for (int i = 0; i < this.m_parallaxLayers.Count; i++)
            {
                this.m_parallaxLayers[i].setMaterialColor(materialColor);
            }
        }
    }
}

