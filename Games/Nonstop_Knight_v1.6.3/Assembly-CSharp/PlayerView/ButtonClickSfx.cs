namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class ButtonClickSfx : MonoBehaviour
    {
        [CompilerGenerated]
        private UnityEngine.UI.Button <Button>k__BackingField;

        protected void Awake()
        {
            this.Button = base.GetComponent<UnityEngine.UI.Button>();
        }

        private void onClick()
        {
            Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_ButtonGeneric, (float) 0f);
        }

        protected void OnDisable()
        {
            if (this.Button != null)
            {
                this.Button.onClick.RemoveListener(new UnityAction(this.onClick));
            }
        }

        protected void OnEnable()
        {
            if (this.Button != null)
            {
                this.Button.onClick.AddListener(new UnityAction(this.onClick));
            }
        }

        public UnityEngine.UI.Button Button
        {
            [CompilerGenerated]
            get
            {
                return this.<Button>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Button>k__BackingField = value;
            }
        }
    }
}

