namespace App
{
    using GameLogic;
    using PlayerView;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class Splash : MonoBehaviour
    {
        [CompilerGenerated]
        private UnityEngine.Canvas <Canvas>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        public Text BuildInfoVersion;
        public CanvasGroupAlphaFading CanvasGroup;
        public GameObject ImageRoot;
        private Coroutine m_animationRoutine;
        public AnimatedProgressBar ProgressBar;
        public App.ServerSelectPopup ServerSelectPopup;
        public GameObject ServerSelectRoot;

        public Coroutine animateToBlack()
        {
            base.gameObject.SetActive(true);
            UnityUtils.StopCoroutine(this, ref this.m_animationRoutine);
            this.m_animationRoutine = UnityUtils.StartCoroutine(this, this.animationRoutine(true, ConfigUi.FADE_TO_BLACK_DURATION * 2f));
            return this.m_animationRoutine;
        }

        public Coroutine animateToTransparent()
        {
            UnityUtils.StopCoroutine(this, ref this.m_animationRoutine);
            if (!base.gameObject.activeSelf)
            {
                this.setVisible(false);
                return null;
            }
            this.m_animationRoutine = UnityUtils.StartCoroutine(this, this.animationRoutine(false, ConfigUi.FADE_TO_BLACK_DURATION * 2f));
            return this.m_animationRoutine;
        }

        [DebuggerHidden]
        private IEnumerator animationRoutine(bool toBlack, float duration)
        {
            <animationRoutine>c__Iterator34 iterator = new <animationRoutine>c__Iterator34();
            iterator.toBlack = toBlack;
            iterator.duration = duration;
            iterator.<$>toBlack = toBlack;
            iterator.<$>duration = duration;
            iterator.<>f__this = this;
            return iterator;
        }

        protected void Awake()
        {
            this.Tm = base.transform;
            this.Canvas = base.GetComponent<UnityEngine.Canvas>();
            if (ConfigApp.ProductionBuild)
            {
                this.BuildInfoVersion.enabled = false;
            }
            else
            {
                this.BuildInfoVersion.text = App.Binder.BuildResources.getBuildInfoDescription();
            }
        }

        public void onOfflineButtonClicked()
        {
            this.ServerSelectPopup.selectServer(ConfigService.OFFLINE_SERVER_ENTRY.Id);
        }

        [DebuggerHidden]
        public IEnumerator selectDevServerRoutine(Player player, ServerEntry[] collection)
        {
            <selectDevServerRoutine>c__Iterator35 iterator = new <selectDevServerRoutine>c__Iterator35();
            iterator.collection = collection;
            iterator.player = player;
            iterator.<$>collection = collection;
            iterator.<$>player = player;
            iterator.<>f__this = this;
            return iterator;
        }

        public void setVisible(bool visible)
        {
            if (visible)
            {
                this.ProgressBar.setNormalizedValue(0f);
            }
            UnityUtils.StopCoroutine(this, ref this.m_animationRoutine);
            this.CanvasGroup.setTransparent(!visible);
            base.gameObject.SetActive(visible);
            if ((ConfigDevice.DeviceQuality() <= DeviceQualityType.Low) && (PlayerView.Binder.SpriteResources != null))
            {
                if (visible && !PlayerView.Binder.SpriteResources.atlasLoaded("Splash"))
                {
                    PlayerView.Binder.SpriteResources.loadAtlas("Splash");
                }
                else if (!visible && PlayerView.Binder.SpriteResources.atlasLoaded("Splash"))
                {
                    PlayerView.Binder.SpriteResources.releaseAtlas("Splash");
                }
            }
        }

        protected void Start()
        {
            this.ServerSelectRoot.SetActive(false);
        }

        public void updateProgressBar(float interval)
        {
            this.ProgressBar.animateToNormalizedValue(interval, 0.4f, null, null, 0f);
        }

        public UnityEngine.Canvas Canvas
        {
            [CompilerGenerated]
            get
            {
                return this.<Canvas>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Canvas>k__BackingField = value;
            }
        }

        public bool IsAnimating
        {
            get
            {
                return UnityUtils.CoroutineRunning(ref this.m_animationRoutine);
            }
        }

        public bool IsVisible
        {
            get
            {
                return base.gameObject.activeSelf;
            }
        }

        public Transform Tm
        {
            [CompilerGenerated]
            get
            {
                return this.<Tm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Tm>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <animationRoutine>c__Iterator34 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal float <$>duration;
            internal bool <$>toBlack;
            internal Splash <>f__this;
            internal float duration;
            internal bool toBlack;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (((ConfigDevice.DeviceQuality() <= DeviceQualityType.Low) && (PlayerView.Binder.SpriteResources != null)) && (this.toBlack && !PlayerView.Binder.SpriteResources.atlasLoaded("Splash")))
                        {
                            PlayerView.Binder.SpriteResources.loadAtlas("Splash");
                        }
                        if (!this.toBlack)
                        {
                            this.$current = this.<>f__this.CanvasGroup.animateToTransparent(this.duration, 0f);
                            this.$PC = 2;
                        }
                        else
                        {
                            this.$current = this.<>f__this.CanvasGroup.animateToBlack(this.duration, 0f);
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                    case 2:
                        this.<>f__this.m_animationRoutine = null;
                        if (!this.toBlack)
                        {
                            this.<>f__this.gameObject.SetActive(false);
                        }
                        if (((ConfigDevice.DeviceQuality() <= DeviceQualityType.Low) && (PlayerView.Binder.SpriteResources != null)) && (!this.toBlack && PlayerView.Binder.SpriteResources.atlasLoaded("Splash")))
                        {
                            PlayerView.Binder.SpriteResources.releaseAtlas("Splash");
                            break;
                            this.$PC = -1;
                        }
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <selectDevServerRoutine>c__Iterator35 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ServerEntry[] <$>collection;
            internal Player <$>player;
            internal Splash <>f__this;
            internal ServerEntry[] collection;
            internal Player player;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<>f__this.ServerSelectPopup.initialize(this.collection);
                        this.<>f__this.ServerSelectRoot.SetActive(true);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00B2;
                }
                if (this.<>f__this.ServerSelectPopup.SelectedServerId == null)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.player.Preferences.DevServerId = this.<>f__this.ServerSelectPopup.SelectedServerId;
                this.<>f__this.ServerSelectRoot.SetActive(false);
                goto Label_00B2;
                this.$PC = -1;
            Label_00B2:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

