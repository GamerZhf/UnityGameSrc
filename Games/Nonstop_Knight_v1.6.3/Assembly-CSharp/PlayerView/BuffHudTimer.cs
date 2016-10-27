namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class BuffHudTimer : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private GameLogic.Buff <Buff>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public UnityEngine.UI.Image CooldownOverlay;
        public UnityEngine.UI.Image Image;
        public Text Number;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
        }

        public void cleanUpForReuse()
        {
            this.Buff = null;
        }

        public void initialize(GameLogic.Buff buff)
        {
            this.Buff = buff;
            this.Image.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", buff.HudSprite);
            this.CooldownOverlay.fillAmount = 0f;
            this.Number.text = string.Empty;
            this.Number.enabled = this.Buff.HudShowStacked || this.Buff.HudShowModifier;
        }

        protected void Update()
        {
            if (this.Buff != null)
            {
                this.CooldownOverlay.fillAmount = this.Buff.getNormalizedProgress(Time.time);
                string str = null;
                if (this.Buff.HudShowStacked)
                {
                    if (this.Buff.HudShowModifier)
                    {
                        float num = GameLogic.Binder.BuffSystem.getTotalBuffModifierFromSource(this.Buff.Character, this.Buff.Source);
                        if (num > 0f)
                        {
                            str = ((num * 100f)).ToString("+0;-0") + "%";
                        }
                    }
                    else
                    {
                        int num2 = GameLogic.Binder.BuffSystem.getNumberOfBuffsFromSource(this.Buff.Character, this.Buff.Source);
                        if (num2 > 1)
                        {
                            str = "x" + num2;
                        }
                    }
                }
                else if (this.Buff.HudShowModifier)
                {
                    str = ((this.Buff.TotalModifier * 100f)).ToString("+0;-0") + "%";
                }
                if (!string.IsNullOrEmpty(str))
                {
                    this.Number.enabled = true;
                    this.Number.text = str;
                }
                else
                {
                    this.Number.enabled = false;
                }
            }
        }

        public GameLogic.Buff Buff
        {
            [CompilerGenerated]
            get
            {
                return this.<Buff>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Buff>k__BackingField = value;
            }
        }

        public RectTransform RectTm
        {
            [CompilerGenerated]
            get
            {
                return this.<RectTm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RectTm>k__BackingField = value;
            }
        }
    }
}

