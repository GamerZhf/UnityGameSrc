namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class MenuHudTop : MonoBehaviour
    {
        [CompilerGenerated]
        private PlayerView.ResourceGainVisualizer <ResourceGainVisualizer>k__BackingField;
        public UnityEngine.Canvas Canvas;
        public Text CoinsText;
        public Image DiamondsIcon;
        public Text DiamondsText;
        public Text EnergyRefreshText;
        public Text EnergyText;
        public GameObject FpsCounter;
        public Image LevelRadialProgress;
        public Text LevelText;
        public AnimatedProgressBar LevelXpSlider;
        private Coroutine m_elementPulsatingRoutine;
        public RectTransform TopPanelTm;

        protected void Awake()
        {
            GameObjectExtensions.AddOrGetComponent<TransformAnimation>(this.DiamondsIcon.gameObject);
            this.ResourceGainVisualizer = base.gameObject.AddComponent<PlayerView.ResourceGainVisualizer>();
        }

        [DebuggerHidden]
        private IEnumerator elementPulsatingRoutine(TransformAnimation ta)
        {
            <elementPulsatingRoutine>c__Iterator136 iterator = new <elementPulsatingRoutine>c__Iterator136();
            iterator.ta = ta;
            iterator.<$>ta = ta;
            return iterator;
        }

        public void initialize(Camera canvasCamera)
        {
            this.Canvas.worldCamera = canvasCamera;
            this.ResourceGainVisualizer.initialize(canvasCamera);
        }

        public void onCoinsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TooltipMenu, MenuContentType.ResourceTooltip, ResourceType.Coin, 0f, false, true);
            }
        }

        protected void OnDisable()
        {
            PlayerView.Binder.EventBus.OnMenuChanged -= new PlayerView.Events.MenuChanged(this.onMenuChanged);
        }

        protected void OnEnable()
        {
            PlayerView.Binder.EventBus.OnMenuChanged += new PlayerView.Events.MenuChanged(this.onMenuChanged);
            this.TopPanelTm.gameObject.SetActive(true);
        }

        public void onEnergyButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TooltipMenu, MenuContentType.ResourceTooltip, ResourceType.Energy, 0f, false, true);
            }
        }

        public void onLevelButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TooltipMenu, MenuContentType.LevelTooltip, null, 0f, false, true);
            }
        }

        private void onMenuChanged(Menu sourceMenu, Menu targetMenu)
        {
            if (targetMenu != null)
            {
                this.TopPanelTm.gameObject.SetActive(true);
            }
            else
            {
                this.TopPanelTm.gameObject.SetActive(false);
            }
        }

        public void onOptionsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.MiniPopupMenu, MenuContentType.CheatPopupContent, null, 0f, false, true);
            }
        }

        public void pulsateElement(TransformAnimation element)
        {
            if (!UnityUtils.CoroutineRunning(ref this.m_elementPulsatingRoutine))
            {
                this.m_elementPulsatingRoutine = UnityUtils.StartCoroutine(this, this.elementPulsatingRoutine(element));
            }
        }

        private void refresh()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.LevelText.text = player.Rank.ToString();
            this.LevelRadialProgress.fillAmount = GameLogic.Binder.LevelUpRules.getNormalizedProgressTowardsNextLevel(player.Rank, player.getResourceAmount(ResourceType.Xp));
            int num = GameLogic.Binder.LevelUpRules.getMaxEnergyForLevel(player.Rank);
            this.EnergyText.text = player.getResourceAmount(ResourceType.Energy) + "/" + num;
            if (player.getResourceAmount(ResourceType.Energy) < num)
            {
                int num2 = 0;
                int num3 = num2 / 60;
                int num4 = num2 - (num3 * 60);
                string[] textArray1 = new string[] { "(", num3.ToString("00"), ":", num4.ToString("00"), ")" };
                this.EnergyRefreshText.text = string.Concat(textArray1);
            }
            else
            {
                this.EnergyRefreshText.text = "(MAX)";
            }
            this.CoinsText.text = player.getResourceAmount(ResourceType.Coin).ToString();
            this.DiamondsText.text = player.getResourceAmount(ResourceType.Diamond).ToString();
        }

        protected void Update()
        {
            if (!this.ResourceGainVisualizer.HasActiveAnimations)
            {
                this.refresh();
            }
        }

        public PlayerView.ResourceGainVisualizer ResourceGainVisualizer
        {
            [CompilerGenerated]
            get
            {
                return this.<ResourceGainVisualizer>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ResourceGainVisualizer>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <elementPulsatingRoutine>c__Iterator136 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TransformAnimation <$>ta;
            internal TransformAnimationTask <tt>__0;
            internal TransformAnimation ta;

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
                        this.ta.stopAll();
                        this.ta.transform.localScale = Vector3.one;
                        this.<tt>__0 = new TransformAnimationTask(this.ta.transform, 0.1f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__0.scale((Vector3) (Vector3.one * 1.5f), true, Easing.Function.SMOOTHSTEP);
                        this.ta.addTask(this.<tt>__0);
                        this.<tt>__0 = new TransformAnimationTask(this.ta.transform, 0.1f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__0.scale(Vector3.one, true, Easing.Function.SMOOTHSTEP);
                        this.ta.addTask(this.<tt>__0);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0109;
                }
                if (this.ta.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                goto Label_0109;
                this.$PC = -1;
            Label_0109:
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

