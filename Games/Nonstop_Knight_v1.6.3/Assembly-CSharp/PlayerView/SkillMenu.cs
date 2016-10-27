namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class SkillMenu : Menu
    {
        [CompilerGenerated]
        private SkillToggle <DraggedSkillToggle>k__BackingField;
        public GridLayoutGroup ActiveSkillToggleGrid;
        private int m_defaultHighlightedSkillIndex;
        private SkillToggle m_highlightedSkillToggle;
        private ToggleGroup m_mainToggleGroup;
        private List<SkillSlotOLD> m_skillSlots = new List<SkillSlotOLD>();
        private List<SkillToggle> m_skillToggles = new List<SkillToggle>();
        public Text SkillDescriptionCooldown;
        public Text SkillDescriptionLevel;
        public Text SkillDescriptionName;
        public Text SkillDescriptionNextRank;
        public Text SkillDescriptionNextRankTitle;
        public Text SkillDescriptionText;
        public Text SkillDescriptionUnlockedAtLevel;
        public Text SkillPointsAvailable;
        public Text SkillPointsRefresh;
        public GridLayoutGroup SkillToggleGrid;
        public Button SkillUpgradeButton;
        public Text SkillUpgradeCostCoins;
        public Text SkillUpgradeCostSkillPoints;

        private void cleanup()
        {
            for (int i = this.m_skillToggles.Count - 1; i >= 0; i--)
            {
                SkillToggle item = this.m_skillToggles[i];
                this.m_skillToggles.Remove(item);
                PlayerView.Binder.SkillTogglePool.returnObject(item);
            }
            this.m_highlightedSkillToggle = null;
            this.m_defaultHighlightedSkillIndex = 0;
        }

        private string getFormattedNextRankText(SkillInstance skillInstance)
        {
            StringBuilder builder = new StringBuilder();
            return builder.ToString();
        }

        private string getFormattedSkillDescription(SkillInstance skillInstance)
        {
            SkillType skillType = skillInstance.SkillType;
            StringBuilder builder = new StringBuilder(ConfigSkills.SHARED_DATA[skillType].Description);
            return builder.ToString();
        }

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator17E iteratore = new <hideRoutine>c__Iterator17E();
            iteratore.<>f__this = this;
            return iteratore;
        }

        protected override void onAwake()
        {
            TransformExtensions.DestroyChildren(this.SkillToggleGrid.transform);
            TransformExtensions.DestroyChildren(this.ActiveSkillToggleGrid.transform);
            this.m_mainToggleGroup = this.SkillToggleGrid.GetComponent<ToggleGroup>();
            for (int i = 0; i < ConfigSkills.SkillGroupCount; i++)
            {
                GameObject obj2 = App.Binder.AssetBundleLoader.instantiatePrefab("Prefabs/Menu/SkillSlotOLD");
                obj2.transform.SetParent(this.ActiveSkillToggleGrid.transform);
                obj2.transform.localScale = Vector3.one;
                SkillSlotOLD component = obj2.GetComponent<SkillSlotOLD>();
                component.initialize(this, i);
                this.m_skillSlots.Add(component);
            }
            this.refreshSkillDescriptionPanel();
        }

        public void onBackButtonClicked()
        {
            if (ConfigUi.VERTICAL_ITEM_MENU_ENABLED)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.NONE, MenuContentType.NONE, null, 0f, false, true);
            }
            else
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.MultiHeroMenu, MenuContentType.NONE, null, 0f, false, true);
            }
        }

        private void onCharacterSkillRankUpped(CharacterInstance character, SkillInstance si)
        {
            int siblingIndex = this.m_highlightedSkillToggle.transform.GetSiblingIndex();
            this.cleanup();
            this.m_defaultHighlightedSkillIndex = siblingIndex;
            base.StartCoroutine(this.showRoutine(MenuContentType.NONE, null));
        }

        private void onCharacterSkillsChanged(CharacterInstance character)
        {
            int siblingIndex = this.m_highlightedSkillToggle.transform.GetSiblingIndex();
            this.cleanup();
            this.m_defaultHighlightedSkillIndex = siblingIndex;
            base.StartCoroutine(this.showRoutine(MenuContentType.NONE, null));
        }

        protected void OnDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnCharacterSkillRankUpped -= new GameLogic.Events.CharacterSkillRankUpped(this.onCharacterSkillRankUpped);
            eventBus.OnCharacterSkillsChanged -= new GameLogic.Events.CharacterSkillsChanged(this.onCharacterSkillsChanged);
        }

        protected void OnEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnCharacterSkillRankUpped += new GameLogic.Events.CharacterSkillRankUpped(this.onCharacterSkillRankUpped);
            eventBus.OnCharacterSkillsChanged += new GameLogic.Events.CharacterSkillsChanged(this.onCharacterSkillsChanged);
        }

        public void onSkillToggleValueChanged(bool toggled)
        {
            this.refreshSkillDescriptionPanel();
        }

        protected override void onUpdate(float dt)
        {
            this.refreshSkillPointsAvailable();
        }

        public void onUpgradeButtonClicked()
        {
        }

        private void refreshSkillDescriptionPanel()
        {
            if (GameLogic.Binder.GameState.Player != null)
            {
                CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
                this.m_highlightedSkillToggle = null;
                if (this.m_mainToggleGroup.AnyTogglesOn())
                {
                    IEnumerator<Toggle> enumerator = this.m_mainToggleGroup.ActiveToggles().GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            this.m_highlightedSkillToggle = enumerator.Current.GetComponent<SkillToggle>();
                            SkillInstance skillInstance = this.m_highlightedSkillToggle.SkillInstance;
                            ConfigSkills.SharedData data = ConfigSkills.SHARED_DATA[skillInstance.SkillType];
                            this.SkillDescriptionCooldown.text = "Cooldown: " + activeCharacter.getSkillCooldown(skillInstance.SkillType) + "s";
                            this.SkillDescriptionName.text = data.Name;
                            this.SkillDescriptionText.text = this.getFormattedSkillDescription(skillInstance);
                            if ((this.m_highlightedSkillToggle.Owner.Rank >= data.UnlockRank) && (skillInstance.Rank < App.Binder.ConfigMeta.GLOBAL_LEVEL_CAP))
                            {
                                this.SkillDescriptionLevel.text = skillInstance.Rank + " / " + App.Binder.ConfigMeta.GLOBAL_LEVEL_CAP;
                                this.SkillDescriptionNextRankTitle.text = "Next rank:";
                                this.SkillDescriptionNextRank.text = this.getFormattedNextRankText(skillInstance);
                                if (skillInstance.Rank < this.m_highlightedSkillToggle.Owner.Rank)
                                {
                                    this.SkillUpgradeButton.gameObject.SetActive(true);
                                    int num = 0;
                                    this.SkillUpgradeCostCoins.text = num.ToString();
                                    this.SkillUpgradeCostSkillPoints.text = 0.ToString();
                                    this.SkillUpgradeButton.interactable = GameLogic.Binder.GameState.Player.getResourceAmount(ResourceType.Coin) >= num;
                                    this.SkillDescriptionUnlockedAtLevel.text = string.Empty;
                                }
                                else
                                {
                                    this.SkillUpgradeButton.gameObject.SetActive(false);
                                    this.SkillDescriptionUnlockedAtLevel.gameObject.SetActive(true);
                                    this.SkillDescriptionUnlockedAtLevel.text = "<color=red>Level " + (skillInstance.Rank + 1) + " required!</color>";
                                }
                            }
                            else
                            {
                                this.SkillDescriptionUnlockedAtLevel.gameObject.SetActive(true);
                                this.SkillUpgradeButton.gameObject.SetActive(false);
                                this.SkillDescriptionNextRankTitle.text = string.Empty;
                                this.SkillDescriptionNextRank.text = string.Empty;
                                if (skillInstance.Rank < App.Binder.ConfigMeta.GLOBAL_LEVEL_CAP)
                                {
                                    this.SkillDescriptionLevel.text = string.Empty;
                                    this.SkillDescriptionUnlockedAtLevel.text = "<color=red>Unlocked at Level " + data.UnlockRank + "</color>";
                                }
                                else
                                {
                                    this.SkillDescriptionLevel.text = skillInstance.Rank + " / " + App.Binder.ConfigMeta.GLOBAL_LEVEL_CAP;
                                    this.SkillDescriptionUnlockedAtLevel.text = "<color=orange>Fully upgraded!</color>";
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (enumerator == null)
                        {
                        }
                        enumerator.Dispose();
                    }
                }
                else
                {
                    this.SkillDescriptionCooldown.text = string.Empty;
                    this.SkillDescriptionLevel.text = string.Empty;
                    this.SkillDescriptionName.text = string.Empty;
                    this.SkillDescriptionNextRankTitle.text = string.Empty;
                    this.SkillDescriptionNextRank.text = string.Empty;
                    this.SkillDescriptionText.text = string.Empty;
                    this.SkillDescriptionUnlockedAtLevel.text = string.Empty;
                    this.SkillUpgradeButton.gameObject.SetActive(false);
                }
            }
        }

        private void refreshSkillPointsAvailable()
        {
            this.SkillPointsAvailable.text = "CHANGE ME";
            int num = 0;
            int num2 = num / 60;
            this.SkillPointsRefresh.text = num2.ToString("00") + ":" + ((num - (num2 * 60))).ToString("00");
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator17D iteratord = new <showRoutine>c__Iterator17D();
            iteratord.<>f__this = this;
            return iteratord;
        }

        public SkillToggle DraggedSkillToggle
        {
            [CompilerGenerated]
            get
            {
                return this.<DraggedSkillToggle>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<DraggedSkillToggle>k__BackingField = value;
            }
        }

        public override PlayerView.MenuType MenuType
        {
            get
            {
                return PlayerView.MenuType.SkillMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator17E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal SkillMenu <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    this.<>f__this.cleanup();
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
        private sealed class <showRoutine>c__Iterator17D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal SkillMenu <>f__this;
            internal int <group>__5;
            internal int <i>__2;
            internal CharacterInstance <pc>__1;
            internal Player <player>__0;
            internal SkillInstance <skillInstance>__3;
            internal SkillInstance <skillInstance>__6;
            internal SkillToggle <toggle>__4;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    this.<player>__0 = GameLogic.Binder.GameState.Player;
                    this.<pc>__1 = this.<player>__0.ActiveCharacter;
                    this.<i>__2 = 0;
                    while (this.<i>__2 < this.<pc>__1.SkillInstances.Count)
                    {
                        this.<skillInstance>__3 = this.<pc>__1.SkillInstances[this.<i>__2];
                        this.<toggle>__4 = PlayerView.Binder.SkillTogglePool.getObject();
                        this.<toggle>__4.transform.SetParent(this.<>f__this.SkillToggleGrid.transform);
                        this.<toggle>__4.transform.localScale = Vector3.one;
                        this.<toggle>__4.transform.localPosition = Vector3.zero;
                        this.<toggle>__4.initialize(this.<>f__this, this.<pc>__1, this.<skillInstance>__3);
                        this.<toggle>__4.Toggle.group = this.<>f__this.m_mainToggleGroup;
                        this.<>f__this.m_skillToggles.Add(this.<toggle>__4);
                        this.<toggle>__4.Toggle.onValueChanged.AddListener(new UnityAction<bool>(this.<>f__this.onSkillToggleValueChanged));
                        this.<toggle>__4.gameObject.SetActive(true);
                        this.<i>__2++;
                    }
                    this.<group>__5 = 0;
                    while (this.<group>__5 < ConfigSkills.SkillGroupCount)
                    {
                        this.<skillInstance>__6 = this.<pc>__1.getActiveSkillInstanceForGroup(this.<group>__5);
                        this.<>f__this.m_skillSlots[this.<group>__5].refreshContent(this.<skillInstance>__6);
                        this.<group>__5++;
                    }
                    this.<>f__this.m_mainToggleGroup.SetAllTogglesOff();
                    if (this.<>f__this.m_skillToggles.Count > 0)
                    {
                        this.<>f__this.m_skillToggles[this.<>f__this.m_defaultHighlightedSkillIndex].Toggle.isOn = true;
                    }
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
    }
}

