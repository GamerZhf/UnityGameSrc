namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class MapCell : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private PlayerView.BattleMenu <BattleMenu>k__BackingField;
        [CompilerGenerated]
        private State <CurrentState>k__BackingField;
        [CompilerGenerated]
        private string <DungeonId>k__BackingField;
        [CompilerGenerated]
        private int <DungeonNumber>k__BackingField;
        [CompilerGenerated]
        private MapCellDynamicContent <DynamicContent>k__BackingField;
        [CompilerGenerated]
        private float <NormalizedProgressBarValue>k__BackingField;
        [CompilerGenerated]
        private bool <PreviousAreaCompleted>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public Image AreaRequirementIcon;
        public Text AreaRequirementText;
        public Image BottomClouds;
        public Image ButtonIcon;
        public ParticleSystem DiamondRewardEffect;
        public static Color DisableFontColor = new Color(0.7372549f, 0.7372549f, 0.7372549f, 1f);
        public GameObject DynamicContentRoot;
        public static Color EnabledFontColor = new Color(0.9254902f, 0.9254902f, 0.8941177f, 1f);
        public Text EventInfoText;
        public Button ExploreButton;
        public Image ExploreButtonImage;
        public Text ExploreCost;
        public Image ExploreCostIcon;
        public Image LevelRequirementIcon;
        public Text LevelRequirementText;
        public CanvasGroupAlphaFading LockedContentRoot;
        public Text LockedName;
        private Sprite m_checkboxSprite;
        private Material m_greyscaleMaterial;
        private Sprite m_tickedCheckboxSprite;
        public GameObject OpenContentRoot;
        public Text OpenName;
        public Button PlayButton;
        public AnimatedProgressBar ProgressBar;
        public GameObject ProgressBarRoot;
        public static Color TickedCheckboxColor = new Color(0.6156863f, 1f, 0f, 1f);
        public GameObject TopClouds;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            this.m_greyscaleMaterial = Resources.Load<Material>("Materials/image_greyscale");
            this.m_checkboxSprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_ui_checkbox");
            this.m_tickedCheckboxSprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_ui_checkbox_checked");
        }

        public void cleanUpForReuse()
        {
        }

        public void initialize(PlayerView.BattleMenu bm, string dungeonId)
        {
            this.BattleMenu = bm;
            this.DungeonId = dungeonId;
            this.DungeonNumber = int.Parse(dungeonId);
            Dungeon dungeon = GameLogic.Binder.DungeonResources.getResource(this.DungeonId);
            GameObject obj2 = App.Binder.AssetBundleLoader.instantiatePrefab("Prefabs/MapStyles/" + dungeon.MapStyle);
            obj2.transform.SetParent(this.DynamicContentRoot.transform, false);
            this.DynamicContent = obj2.GetComponent<MapCellDynamicContent>();
            this.DynamicContent.reset();
            int num = int.Parse(dungeonId);
            this.DynamicContent.BackgroundCanvas.overrideSorting = true;
            this.DynamicContent.BackgroundCanvas.sortingOrder = -999999 + num;
        }

        public void onExploreButtonClicked()
        {
            <onExploreButtonClicked>c__AnonStorey2EC storeyec = new <onExploreButtonClicked>c__AnonStorey2EC();
            storeyec.<>f__this = this;
            Player player = GameLogic.Binder.GameState.Player;
            storeyec.pc = player.ActiveCharacter;
            double num2 = MathUtil.ClampMin(GameLogic.Binder.DungeonResources.getResource(this.DungeonId).ExploreCost - player.getResourceAmount(ResourceType.Coin), 0.0);
            if (num2 > 0.0)
            {
                OutOfMaterialsMiniPopupContent.InputParams params2 = new OutOfMaterialsMiniPopupContent.InputParams();
                Dictionary<ResourceType, double> dictionary = new Dictionary<ResourceType, double>();
                dictionary.Add(ResourceType.Coin, num2);
                params2.MissingResources = dictionary;
                params2.SuccessCallback = new System.Action(storeyec.<>m__192);
                OutOfMaterialsMiniPopupContent.InputParams parameter = params2;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.MiniPopupMenu, MenuContentType.OutOfMaterialsMiniPopup, parameter, 0f, false, true);
            }
            else
            {
                GameLogic.Binder.CommandProcessor.execute(new CmdExploreDungeon(storeyec.pc, this.DungeonId), 0f);
            }
        }

        public void onPlayButtonClicked()
        {
            if (ConfigUi.MAP_PREDUNGEON_TOOLTIP_ENABLED)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.PreDungeonMenu, MenuContentType.DungeonTooltip, this.DungeonId, 0f, false, true);
            }
            else
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.StackedPopupMenu, MenuContentType.DungeonPopupContent, this.DungeonId, 0f, false, true);
            }
        }

        public void refresh(bool previousAreaCompleted, float normalizedProgressBarValue)
        {
            this.PreviousAreaCompleted = previousAreaCompleted;
            this.NormalizedProgressBarValue = normalizedProgressBarValue;
            Dungeon dungeon = GameLogic.Binder.DungeonResources.getResource(this.DungeonId);
            Player player = GameLogic.Binder.GameState.Player;
            bool flag = player.hasExploredDungeon(this.DungeonId);
            int num = this.BattleMenu.getDistanceToLastExploredDungeon(this);
            bool flag2 = this.BattleMenu.previousDungeonExplored(this) && !flag;
            bool flag3 = flag || (num <= ConfigUi.MAP_AREAS_VISIBLE_BEFORE_CLOUDS);
            if (flag)
            {
                this.CurrentState = State.Open;
            }
            else
            {
                this.CurrentState = State.Locked;
            }
            this.BottomClouds.gameObject.SetActive(this.DungeonId == "1");
            this.TopClouds.SetActive(this.BattleMenu.getNextMapCell(this) == null);
            this.DynamicContent.setColorTint(this.CurrentState == State.Locked, 0f);
            this.LockedContentRoot.CanvasGroup.interactable = true;
            this.LockedContentRoot.CanvasGroup.blocksRaycasts = true;
            switch (this.CurrentState)
            {
                case State.Open:
                {
                    this.OpenContentRoot.SetActive(true);
                    this.DynamicContent.OpenContentRoot.SetActive(true);
                    this.LockedContentRoot.setTransparent(false);
                    this.LockedContentRoot.gameObject.SetActive(false);
                    this.DynamicContent.LockedContentRoot.SetActive(false);
                    this.OpenContentRoot.transform.position = this.DynamicContent.UiAnchor.position;
                    this.OpenName.text = StringExtensions.ToUpperLoca(dungeon.Name);
                    this.PlayButton.interactable = true;
                    if (player.floorCompleted(0))
                    {
                        this.ButtonIcon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_map2_node_inactive");
                    }
                    else
                    {
                        this.ButtonIcon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_map2_node_active");
                    }
                    if (normalizedProgressBarValue == -1f)
                    {
                        this.ProgressBarRoot.SetActive(false);
                    }
                    else
                    {
                        this.ProgressBarRoot.SetActive(true);
                        this.ProgressBar.setNormalizedValue(normalizedProgressBarValue);
                    }
                    DungeonEvent event2 = GameLogic.Binder.DungeonEventRules.getActiveEventForDungeon(this.DungeonId);
                    if (event2 != null)
                    {
                        string str = string.Empty;
                        if (event2.CoinMultiplier != 1f)
                        {
                            str = str + event2.CoinMultiplier + "X COINS";
                        }
                        if (event2.XpMultiplier != 1f)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                str = str + ", ";
                            }
                            str = str + event2.XpMultiplier + "X EXPERIENCE";
                        }
                        if (event2.DropChanceMultiplier != 1f)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                str = str + ", ";
                            }
                            str = str + event2.DropChanceMultiplier + "X DROP-CHANCE";
                        }
                        str = str + "!";
                        DateTime time = TimeUtil.UnixTimestampToDateTime(Service.Binder.ServerTime.GameTime);
                        this.EventInfoText.text = StringExtensions.ToUpperLoca(time.DayOfWeek.ToString()) + " EVENT: " + str;
                    }
                    else
                    {
                        this.EventInfoText.text = string.Empty;
                    }
                    break;
                }
                case State.Locked:
                    this.LockedContentRoot.setTransparent(false);
                    this.OpenContentRoot.SetActive(false);
                    this.DynamicContent.reset();
                    this.DynamicContent.OpenContentRoot.SetActive(true);
                    this.DynamicContent.LockedContentRoot.SetActive(true);
                    if (flag3)
                    {
                        this.LockedContentRoot.gameObject.SetActive(true);
                        this.LockedName.text = StringExtensions.ToUpperLoca(dungeon.Name);
                        this.AreaRequirementText.gameObject.SetActive(!previousAreaCompleted);
                        int levelRequirement = dungeon.LevelRequirement;
                        this.LevelRequirementText.text = "PLAYER LEVEL " + levelRequirement;
                        if (player.Rank >= levelRequirement)
                        {
                            this.LevelRequirementText.color = EnabledFontColor;
                            this.LevelRequirementIcon.sprite = this.m_tickedCheckboxSprite;
                            this.LevelRequirementIcon.color = TickedCheckboxColor;
                        }
                        else
                        {
                            this.LevelRequirementText.color = DisableFontColor;
                            this.LevelRequirementIcon.sprite = this.m_checkboxSprite;
                            this.LevelRequirementIcon.color = Color.white;
                        }
                        if (flag2)
                        {
                            this.ExploreButton.gameObject.SetActive(true);
                            this.ExploreCost.text = dungeon.ExploreCost.ToString();
                            if (previousAreaCompleted && (player.Rank >= levelRequirement))
                            {
                                this.ExploreButton.interactable = true;
                                this.ExploreButtonImage.material = null;
                                this.ExploreCostIcon.material = null;
                            }
                            else
                            {
                                this.ExploreButton.interactable = false;
                                this.ExploreButtonImage.material = this.m_greyscaleMaterial;
                                this.ExploreCostIcon.material = this.m_greyscaleMaterial;
                            }
                        }
                        else
                        {
                            this.ExploreButton.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        this.LockedContentRoot.gameObject.SetActive(false);
                    }
                    break;
            }
        }

        public PlayerView.BattleMenu BattleMenu
        {
            [CompilerGenerated]
            get
            {
                return this.<BattleMenu>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<BattleMenu>k__BackingField = value;
            }
        }

        public State CurrentState
        {
            [CompilerGenerated]
            get
            {
                return this.<CurrentState>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CurrentState>k__BackingField = value;
            }
        }

        public string DungeonId
        {
            [CompilerGenerated]
            get
            {
                return this.<DungeonId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<DungeonId>k__BackingField = value;
            }
        }

        public int DungeonNumber
        {
            [CompilerGenerated]
            get
            {
                return this.<DungeonNumber>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<DungeonNumber>k__BackingField = value;
            }
        }

        public MapCellDynamicContent DynamicContent
        {
            [CompilerGenerated]
            get
            {
                return this.<DynamicContent>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<DynamicContent>k__BackingField = value;
            }
        }

        public bool IsAnimating
        {
            get
            {
                return (this.DynamicContent.IsAnimating || this.LockedContentRoot.IsAnimating);
            }
        }

        public float NormalizedProgressBarValue
        {
            [CompilerGenerated]
            get
            {
                return this.<NormalizedProgressBarValue>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<NormalizedProgressBarValue>k__BackingField = value;
            }
        }

        public bool PreviousAreaCompleted
        {
            [CompilerGenerated]
            get
            {
                return this.<PreviousAreaCompleted>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PreviousAreaCompleted>k__BackingField = value;
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
        private sealed class <onExploreButtonClicked>c__AnonStorey2EC
        {
            internal MapCell <>f__this;
            internal CharacterInstance pc;

            internal void <>m__192()
            {
                GameLogic.Binder.CommandProcessor.execute(new CmdExploreDungeon(this.pc, this.<>f__this.DungeonId), 0f);
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        public enum State
        {
            Open,
            Locked
        }
    }
}

