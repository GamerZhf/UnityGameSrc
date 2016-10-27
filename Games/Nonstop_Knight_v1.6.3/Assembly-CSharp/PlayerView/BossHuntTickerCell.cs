namespace PlayerView
{
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class BossHuntTickerCell : MonoBehaviour, IEventSystemHandler, IPointerClickHandler, IPoolable
    {
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public CanvasGroupAlphaFading AlphaFadingRoot;
        public Image ContributorRankIcon;
        private float m_fadeInDuration = 0.2f;
        private float m_fadeOutDuration = 2f;
        private float m_fadeOutStartDelay = 8f;
        private Coroutine m_fadeRoutine;
        private float m_floatSpeed = 650f;
        private float m_intervalDistance = 100f;
        private bool m_readyToDie;
        private readonly float m_startX;
        private readonly float m_startY;
        private float m_targetY;
        public Image TickerIcon;
        public Text TickerTextLeft;
        public Text TickerTextRight;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
        }

        public void BumpToSlot(int slot)
        {
            this.m_targetY = this.m_startY + (this.m_intervalDistance * slot);
        }

        public void cleanUpForReuse()
        {
            UnityUtils.StopCoroutine(this, ref this.m_fadeRoutine);
            if (this.AlphaFadingRoot != null)
            {
                this.AlphaFadingRoot.setTransparent(true);
            }
            this.TickerTextLeft.text = null;
            this.TickerTextRight.text = null;
            this.TickerIcon.sprite = null;
            this.ContributorRankIcon.sprite = null;
            this.m_targetY = float.MinValue;
            base.gameObject.SetActive(false);
        }

        [DebuggerHidden]
        private IEnumerator fadeRoutine()
        {
            <fadeRoutine>c__IteratorED red = new <fadeRoutine>c__IteratorED();
            red.<>f__this = this;
            return red;
        }

        protected void FixedUpdate()
        {
            Vector2 anchoredPosition = this.RectTm.anchoredPosition;
            if (anchoredPosition.y < this.m_targetY)
            {
                this.RectTm.anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + (this.m_floatSpeed * Time.deltaTime));
            }
        }

        public void Initialize(TournamentLogEvent logEvent, RectTransform tickerRoot)
        {
            UnityUtils.StopCoroutine(this, ref this.m_fadeRoutine);
            base.gameObject.SetActive(true);
            if (!string.IsNullOrEmpty(logEvent.TickerLeftText) && (logEvent.TickerLeftText.Length > 0x10))
            {
                this.TickerTextLeft.text = logEvent.TickerLeftText.Substring(0, 14) + "..";
            }
            else
            {
                this.TickerTextLeft.text = logEvent.TickerLeftText;
            }
            this.TickerTextRight.text = logEvent.TickerRightText;
            if (logEvent.IconIdentifier != null)
            {
                this.TickerIcon.enabled = true;
                this.TickerIcon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", logEvent.IconIdentifier);
            }
            else
            {
                this.TickerIcon.enabled = false;
            }
            if (logEvent.ContributorIconIdentifier != null)
            {
                this.ContributorRankIcon.enabled = true;
                this.ContributorRankIcon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", logEvent.ContributorIconIdentifier);
            }
            else
            {
                this.ContributorRankIcon.enabled = false;
            }
            this.AlphaFadingRoot.setTransparent(true);
            this.RectTm.SetParent(tickerRoot);
            this.RectTm.anchoredPosition = new Vector2(this.m_startX, this.m_startY);
            this.m_targetY = this.RectTm.anchoredPosition.y + this.m_intervalDistance;
            this.m_fadeRoutine = UnityUtils.StartCoroutine(this, this.fadeRoutine());
            this.m_readyToDie = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                if ((PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.SlidingTaskPanel) || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.SlidingAdventurePanel))
                {
                    PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                }
                else
                {
                    SlidingAdventurePanel.InputParameters parameters2 = new SlidingAdventurePanel.InputParameters();
                    parameters2.OverrideOpenTabIndex = 1;
                    parameters2.OverrideOpenTournamentSubTabIndex = 3;
                    SlidingAdventurePanel.InputParameters parameter = parameters2;
                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.SlidingAdventurePanel, MenuContentType.NONE, parameter, 0f, false, true);
                }
            }
        }

        public void PreMatureDestroy()
        {
            if (this.AlphaFadingRoot.TimeRemaining > this.m_fadeInDuration)
            {
                UnityUtils.StopCoroutine(this, ref this.m_fadeRoutine);
                this.m_fadeRoutine = UnityUtils.StartCoroutine(this, this.preMatureDestroyRoutine());
            }
        }

        [DebuggerHidden]
        private IEnumerator preMatureDestroyRoutine()
        {
            <preMatureDestroyRoutine>c__IteratorEC rec = new <preMatureDestroyRoutine>c__IteratorEC();
            rec.<>f__this = this;
            return rec;
        }

        public bool ReadyToDie
        {
            get
            {
                return this.m_readyToDie;
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

        [CompilerGenerated]
        private sealed class <fadeRoutine>c__IteratorED : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal BossHuntTickerCell <>f__this;

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
                        this.<>f__this.AlphaFadingRoot.animateToAlpha(1f, this.<>f__this.m_fadeInDuration, 0f);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00B7;

                    default:
                        goto Label_00DF;
                }
                if (this.<>f__this.AlphaFadingRoot.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_00E1;
                }
                this.<>f__this.AlphaFadingRoot.animateToTransparent(this.<>f__this.m_fadeOutDuration, this.<>f__this.m_fadeOutStartDelay);
            Label_00B7:
                while (this.<>f__this.AlphaFadingRoot.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_00E1;
                }
                this.<>f__this.m_readyToDie = true;
                this.$PC = -1;
            Label_00DF:
                return false;
            Label_00E1:
                return true;
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
        private sealed class <preMatureDestroyRoutine>c__IteratorEC : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal BossHuntTickerCell <>f__this;

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
                        this.<>f__this.AlphaFadingRoot.animateToTransparent(this.<>f__this.m_fadeInDuration, 0f);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0082;
                }
                if (this.<>f__this.AlphaFadingRoot.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<>f__this.m_readyToDie = true;
                this.$PC = -1;
            Label_0082:
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

