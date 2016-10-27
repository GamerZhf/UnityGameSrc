﻿namespace UnityStandardAssets.ImageEffects
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode, AddComponentMenu("Image Effects/Color Adjustments/Contrast Stretch")]
    public class ContrastStretch : MonoBehaviour
    {
        public float adaptationSpeed = 0.02f;
        private RenderTexture[] adaptRenderTex = new RenderTexture[2];
        private int curAdaptIndex;
        public float limitMaximum = 0.6f;
        public float limitMinimum = 0.2f;
        private Material m_materialAdapt;
        private Material m_materialApply;
        private Material m_materialLum;
        private Material m_materialReduce;
        public Shader shaderAdapt;
        public Shader shaderApply;
        public Shader shaderLum;
        public Shader shaderReduce;

        private void CalculateAdaptation(Texture curTexture)
        {
            int curAdaptIndex = this.curAdaptIndex;
            this.curAdaptIndex = (this.curAdaptIndex + 1) % 2;
            float num2 = 1f - Mathf.Pow(1f - this.adaptationSpeed, 30f * Time.deltaTime);
            num2 = Mathf.Clamp(num2, 0.01f, 1f);
            this.materialAdapt.SetTexture("_CurTex", curTexture);
            this.materialAdapt.SetVector("_AdaptParams", new Vector4(num2, this.limitMinimum, this.limitMaximum, 0f));
            Graphics.SetRenderTarget(this.adaptRenderTex[this.curAdaptIndex]);
            GL.Clear(false, true, Color.black);
            Graphics.Blit(this.adaptRenderTex[curAdaptIndex], this.adaptRenderTex[this.curAdaptIndex], this.materialAdapt);
        }

        private void OnDisable()
        {
            for (int i = 0; i < 2; i++)
            {
                UnityEngine.Object.DestroyImmediate(this.adaptRenderTex[i]);
                this.adaptRenderTex[i] = null;
            }
            if (this.m_materialLum != null)
            {
                UnityEngine.Object.DestroyImmediate(this.m_materialLum);
            }
            if (this.m_materialReduce != null)
            {
                UnityEngine.Object.DestroyImmediate(this.m_materialReduce);
            }
            if (this.m_materialAdapt != null)
            {
                UnityEngine.Object.DestroyImmediate(this.m_materialAdapt);
            }
            if (this.m_materialApply != null)
            {
                UnityEngine.Object.DestroyImmediate(this.m_materialApply);
            }
        }

        private void OnEnable()
        {
            for (int i = 0; i < 2; i++)
            {
                if (this.adaptRenderTex[i] == null)
                {
                    this.adaptRenderTex[i] = new RenderTexture(1, 1, 0);
                    this.adaptRenderTex[i].hideFlags = HideFlags.HideAndDontSave;
                }
            }
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            RenderTexture temporary = RenderTexture.GetTemporary(source.width / 1, source.height / 1);
            Graphics.Blit(source, temporary, this.materialLum);
            while ((temporary.width > 1) || (temporary.height > 1))
            {
                int width = temporary.width / 2;
                if (width < 1)
                {
                    width = 1;
                }
                int height = temporary.height / 2;
                if (height < 1)
                {
                    height = 1;
                }
                RenderTexture dest = RenderTexture.GetTemporary(width, height);
                Graphics.Blit(temporary, dest, this.materialReduce);
                RenderTexture.ReleaseTemporary(temporary);
                temporary = dest;
            }
            this.CalculateAdaptation(temporary);
            this.materialApply.SetTexture("_AdaptTex", this.adaptRenderTex[this.curAdaptIndex]);
            Graphics.Blit(source, destination, this.materialApply);
            RenderTexture.ReleaseTemporary(temporary);
        }

        private void Start()
        {
            if (!SystemInfo.supportsImageEffects)
            {
                base.enabled = false;
            }
            else if ((!this.shaderAdapt.isSupported || !this.shaderApply.isSupported) || (!this.shaderLum.isSupported || !this.shaderReduce.isSupported))
            {
                base.enabled = false;
            }
        }

        protected Material materialAdapt
        {
            get
            {
                if (this.m_materialAdapt == null)
                {
                    this.m_materialAdapt = new Material(this.shaderAdapt);
                    this.m_materialAdapt.hideFlags = HideFlags.HideAndDontSave;
                }
                return this.m_materialAdapt;
            }
        }

        protected Material materialApply
        {
            get
            {
                if (this.m_materialApply == null)
                {
                    this.m_materialApply = new Material(this.shaderApply);
                    this.m_materialApply.hideFlags = HideFlags.HideAndDontSave;
                }
                return this.m_materialApply;
            }
        }

        protected Material materialLum
        {
            get
            {
                if (this.m_materialLum == null)
                {
                    this.m_materialLum = new Material(this.shaderLum);
                    this.m_materialLum.hideFlags = HideFlags.HideAndDontSave;
                }
                return this.m_materialLum;
            }
        }

        protected Material materialReduce
        {
            get
            {
                if (this.m_materialReduce == null)
                {
                    this.m_materialReduce = new Material(this.shaderReduce);
                    this.m_materialReduce.hideFlags = HideFlags.HideAndDontSave;
                }
                return this.m_materialReduce;
            }
        }
    }
}

