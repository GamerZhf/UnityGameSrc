namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectCenterer : MonoBehaviour
    {
        [CompilerGenerated]
        private RectTransform <TargetRectTm>k__BackingField;
        private RectTransform m_ContentRectTransform;
        private RectTransform m_RectTransform;
        private ScrollRect m_ScrollRect;
        public float ScrollSpeed = 10f;

        private void Awake()
        {
            this.m_ScrollRect = base.GetComponent<ScrollRect>();
            this.m_RectTransform = base.GetComponent<RectTransform>();
            this.m_ContentRectTransform = this.m_ScrollRect.content;
        }

        private void LateUpdate()
        {
            this.updateScrollToSelected();
        }

        private void updateScrollToSelected()
        {
            if ((this.TargetRectTm != null) && (this.TargetRectTm.parent == this.m_ContentRectTransform.transform))
            {
                Vector3 vector = this.m_RectTransform.localPosition - this.TargetRectTm.localPosition;
                float num = this.m_ContentRectTransform.rect.height - this.m_RectTransform.rect.height;
                float num2 = this.m_ContentRectTransform.rect.height - vector.y;
                float num3 = this.m_ScrollRect.normalizedPosition.y * num;
                float num4 = (num3 - (this.TargetRectTm.rect.height / 2f)) + this.m_RectTransform.rect.height;
                float num5 = num3 + (this.TargetRectTm.rect.height / 2f);
                if (num2 > num4)
                {
                    float num6 = num2 - num4;
                    float num7 = num3 + num6;
                    float y = num7 / num;
                    this.m_ScrollRect.normalizedPosition = Vector2.Lerp(this.m_ScrollRect.normalizedPosition, new Vector2(0f, y), this.ScrollSpeed * Time.deltaTime);
                }
                else if (num2 < num5)
                {
                    float num9 = num2 - num5;
                    float num10 = num3 + num9;
                    float num11 = num10 / num;
                    this.m_ScrollRect.normalizedPosition = Vector2.Lerp(this.m_ScrollRect.normalizedPosition, new Vector2(0f, num11), this.ScrollSpeed * Time.deltaTime);
                }
            }
        }

        public RectTransform TargetRectTm
        {
            [CompilerGenerated]
            get
            {
                return this.<TargetRectTm>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<TargetRectTm>k__BackingField = value;
            }
        }
    }
}

