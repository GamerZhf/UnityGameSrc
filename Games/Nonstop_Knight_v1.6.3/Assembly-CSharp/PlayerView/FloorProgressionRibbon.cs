namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class FloorProgressionRibbon : MonoBehaviour
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map5;
        [CompilerGenerated]
        private bool <BossSummoningLocked>k__BackingField;
        public GameObject AutoSummonBossesRoot;
        public Text AutoSummonBossesTitle;
        public Toggle AutoSummonBossesToggle;
        public Image Bg;
        public AnimatedProgressBar BossBar;
        public GameObject BossBarRoot;
        public Text BossBarSkill;
        public Text BossBarTitle;
        public GameObject ChallengeButtonAdventureContentRoot;
        public GameObject ChallengeButtonBossHuntContentRoot;
        public Text ChallengeButtonBossHuntText;
        public PulsatingGraphic ChallengeButtonPulsatingGraphic;
        public GameObject ChallengeButtonRoot;
        public Text ChallengeButtonText;
        public Text ChallengeDifficultyText;
        public List<IconWithText> FloorIcons;
        private Sprite m_bossSprite;
        private bool m_bossSummonClickFlag;
        private Sprite m_eliteBossSprite;
        private Color m_eliteFutureBorderColor;
        private Sprite m_goldenBorderSprite;
        private bool m_pendingBossHpRefresh;
        private bool m_pendingTimerProgressRefresh;
        private string m_prevDifficulty;
        private Sprite m_regularBorderSprite;
        public ParticleSystem TimerBarCompleteEffect;
        public GameObject TimerButtonRoot;
        public Text TimerButtonText;
        public AnimatedProgressBar TimerProgressBar;
        public ScalePulse TimerTextPulse;

        protected void Awake()
        {
            this.TimerButtonRoot.SetActive(false);
            this.ChallengeButtonRoot.SetActive(false);
            this.BossBarRoot.SetActive(false);
            this.ChallengeButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PROGR_RIBBON_BUTTON_BATTLE, null, false));
            this.ChallengeButtonBossHuntText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.PROGR_RIBBON_BUTTON_BATTLE, null, false));
            this.AutoSummonBossesTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.OPTIONS_AUTO_SUMMON_COMPLETED_BOSSES, null, false));
            this.m_regularBorderSprite = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "RoundFrame");
            this.m_goldenBorderSprite = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "RoundFrame_Golden");
            this.m_bossSprite = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "RoundFrame_BossPurple");
            this.m_eliteBossSprite = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "RoundFrame_BossElite");
            this.m_eliteFutureBorderColor = this.FloorIcons[2].Icon.color;
        }

        private void fillFloorIcon(int floor, IconWithText floorIcon, bool isInThePast, bool isInTheFuture)
        {
            floorIcon.gameObject.SetActive(true);
            floorIcon.Icon.sprite = this.getFloorBossSprite(floor);
            floorIcon.Icon.enabled = floorIcon.Icon.sprite != null;
            if (!isInThePast && (floorIcon.Icon.sprite == this.m_eliteBossSprite))
            {
                floorIcon.Background.sprite = this.m_goldenBorderSprite;
            }
            else
            {
                floorIcon.Background.sprite = this.m_regularBorderSprite;
            }
            if ((floorIcon.Icon.sprite == this.m_eliteBossSprite) && isInTheFuture)
            {
                floorIcon.Background.color = this.m_eliteFutureBorderColor;
            }
            else
            {
                floorIcon.Background.color = Color.white;
            }
            if (floorIcon.Icon.enabled)
            {
                floorIcon.Text.text = string.Empty;
            }
            else
            {
                floorIcon.Text.text = floor.ToString();
            }
        }

        private Sprite getFloorBossSprite(int floor)
        {
            if (ConfigDungeons.FloorHasEliteTag(floor))
            {
                return this.m_eliteBossSprite;
            }
            if (!ConfigDungeons.FloorHasBoss(floor))
            {
                return null;
            }
            if (GameLogic.Binder.GameState.Player.Tournaments.hasTournamentSelected())
            {
                return this.m_eliteBossSprite;
            }
            return this.m_bossSprite;
        }

        public void initialize(int floor)
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.m_bossSummonClickFlag = false;
            if (floor == 0)
            {
                this.Bg.enabled = false;
                this.FloorIcons[0].gameObject.SetActive(false);
                this.fillFloorIcon(0, this.FloorIcons[1], false, false);
                this.FloorIcons[2].gameObject.SetActive(false);
            }
            else
            {
                this.Bg.enabled = true;
                if (floor > 1)
                {
                    this.fillFloorIcon(floor - 1, this.FloorIcons[0], true, false);
                }
                else
                {
                    this.FloorIcons[0].gameObject.SetActive(false);
                }
                this.fillFloorIcon(floor, this.FloorIcons[1], false, false);
                this.fillFloorIcon(floor + 1, this.FloorIcons[2], false, true);
            }
            this.AutoSummonBossesRoot.SetActive(player.bossAutoSummonPossibleInFloor(floor));
            this.AutoSummonBossesToggle.isOn = player.Preferences.AutoSummonBosses;
            this.refreshDifficulty();
        }

        public void onAutoSummonBossesAuxButtonClicked()
        {
            Player player = GameLogic.Binder.GameState.Player;
            player.Preferences.AutoSummonBosses = !player.Preferences.AutoSummonBosses;
            this.AutoSummonBossesToggle.isOn = player.Preferences.AutoSummonBosses;
        }

        public void onAutoSummonBossesToggleClicked(bool state)
        {
            GameLogic.Binder.GameState.Player.Preferences.AutoSummonBosses = state;
        }

        public void onChallengeButtonClicked()
        {
            if (!this.m_bossSummonClickFlag)
            {
                ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                if (!PlayerView.Binder.MenuSystem.InTransition && ((activeDungeon.CurrentGameplayState == GameplayState.ACTION) || (activeDungeon.CurrentGameplayState == GameplayState.WAITING)))
                {
                    GameLogic.Binder.CommandProcessor.execute(new CmdStartBossFight(Room.BossSummonMethod.Player), 0f);
                    this.refresh();
                    this.m_bossSummonClickFlag = true;
                }
            }
        }

        private void onCharacterDealtDamage(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, Vector3 worldPos, double amount, bool critted, bool damageReduced, DamageType damageType, SkillType fromSkill)
        {
            if (targetCharacter.IsBoss)
            {
                this.m_pendingBossHpRefresh = true;
            }
        }

        private void onCharacterKilled(CharacterInstance target, CharacterInstance killer, bool critted, SkillType fromSkill)
        {
            if (target.IsBoss)
            {
                this.m_pendingBossHpRefresh = true;
            }
            if (!target.IsPlayerCharacter && (killer != null))
            {
                this.m_pendingTimerProgressRefresh = true;
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnGameplayStateChanged -= new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            GameLogic.Binder.EventBus.OnCharacterDealtDamage -= new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            GameLogic.Binder.EventBus.OnCharacterKilled -= new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            GameLogic.Binder.EventBus.OnPassiveProgress -= new GameLogic.Events.PassiveProgress(this.onPassiveProgress);
            GameLogic.Binder.EventBus.OnPlayerAugmentationGained -= new GameLogic.Events.PlayerAugmentationGained(this.onPlayerAugmentationGained);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnGameplayStateChanged += new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            GameLogic.Binder.EventBus.OnCharacterDealtDamage += new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            GameLogic.Binder.EventBus.OnCharacterKilled += new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            GameLogic.Binder.EventBus.OnPassiveProgress += new GameLogic.Events.PassiveProgress(this.onPassiveProgress);
            GameLogic.Binder.EventBus.OnPlayerAugmentationGained += new GameLogic.Events.PlayerAugmentationGained(this.onPlayerAugmentationGained);
        }

        private void onGameplayStarted(ActiveDungeon ad)
        {
            this.refreshTimerProgressBar(true);
            this.refreshChallengeButtonContent(ad.PrimaryPlayerCharacter.OwningPlayer);
        }

        private void onGameplayStateChanged(GameplayState previousState, GameplayState currentState)
        {
            this.refresh();
        }

        private void onPassiveProgress(Player player, int numPassiveMinionKills, int numPassiveFloorCompletions, int numPassiveBossKills)
        {
            this.m_pendingTimerProgressRefresh = true;
        }

        private void onPlayerAugmentationGained(Player player, string id)
        {
            this.m_pendingTimerProgressRefresh = true;
        }

        public void refresh()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if ((activeDungeon != null) && (activeDungeon.ActiveRoom != null))
            {
                Player player = GameLogic.Binder.GameState.Player;
                if ((GameLogic.Binder.FrenzySystem.isFrenzyActive() || player.BossTrain.Active) || (PlayerView.Binder.InputSystem.getInputRequirement(InputSystem.Layer.LocationEndTransition) == InputSystem.Requirement.MustBeDisabled))
                {
                    this.TimerButtonRoot.SetActive(false);
                    this.ChallengeButtonRoot.SetActive(false);
                }
                else if (activeDungeon.ActiveRoom.MainBossSummoned || activeDungeon.WildBossMode)
                {
                    this.TimerButtonRoot.SetActive(false);
                    this.ChallengeButtonRoot.SetActive(false);
                }
                else if (player.canManuallySummonFloorBoss(activeDungeon))
                {
                    bool flag = (this.BossSummoningLocked || ((activeDungeon.CurrentGameplayState != GameplayState.ACTION) && (activeDungeon.CurrentGameplayState != GameplayState.WAITING))) || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.StackedPopupMenu);
                    this.ChallengeButtonRoot.SetActive(!flag);
                    this.TimerButtonRoot.SetActive(false);
                    this.refreshBossBar(null);
                    this.refreshDifficulty();
                }
                else if (!this.TimerButtonRoot.activeInHierarchy)
                {
                    this.TimerButtonRoot.SetActive(true);
                    this.ChallengeButtonRoot.SetActive(false);
                    this.refreshBossBar(null);
                    this.refreshTimerProgressBar(true);
                }
            }
        }

        public void refreshBossBar(CharacterInstance boss)
        {
            if (boss != null)
            {
                this.BossBarRoot.SetActive(true);
                this.BossBarTitle.text = StringExtensions.ToUpperLoca(boss.Name);
                this.BossBarSkill.text = ConfigGameplay.GetBossAiDescription(boss.Character.BossAiBehaviour, boss.Character.BossAiParameters);
                if (boss.IsEliteBoss)
                {
                    PerkType bossPerkType = ConfigPerks.GetBossPerkType(GameLogic.Binder.GameState.Player, boss.Character.Id);
                    if (bossPerkType != PerkType.NONE)
                    {
                        this.BossBarSkill.text = this.BossBarSkill.text + "\n" + MenuHelpers.ColoredText(_.L(ConfigPerks.SHARED_DATA[bossPerkType].Description, null, false));
                    }
                }
                this.BossBar.setNormalizedValue(1f);
            }
            else
            {
                this.BossBarRoot.SetActive(false);
            }
        }

        public void refreshBossHp()
        {
            this.m_pendingBossHpRefresh = false;
            if (this.BossBar.gameObject.activeInHierarchy)
            {
                ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                float targetV = 0f;
                if (activeDungeon.ActiveRoom.numberOfBossesAlive() > 0)
                {
                    double num2 = activeDungeon.ActiveRoom.getCumulativeCurrentBossHp() / activeDungeon.ActiveRoom.getCumulativeMaxBossHp();
                    targetV = Mathf.Clamp((float) num2, 0.075f, 1f);
                }
                this.BossBar.animateToNormalizedValue(targetV, ConfigUi.HP_LOSS_INDICATOR_ANIMATION_SPEED, null, null, 0f);
            }
        }

        private void refreshChallengeButtonContent(Player player)
        {
            bool flag = player.Tournaments.hasTournamentSelected();
            this.ChallengeButtonAdventureContentRoot.SetActive(!flag);
            this.ChallengeButtonBossHuntContentRoot.SetActive(flag);
        }

        public void refreshDifficulty()
        {
            if (!GameLogic.Binder.GameState.ActiveDungeon.isBossFloor())
            {
                return;
            }
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            Player player = GameLogic.Binder.GameState.Player;
            Character referenceBoss = GameLogic.Binder.CharacterResources.getResource(activeDungeon.BossId);
            string str = ConfigGameplay.CalculateBossDifficulty(player, activeDungeon.Floor, activeDungeon.isEliteBossFloor(), referenceBoss, activeDungeon.getProgressDifficultyExponent());
            if (!(str != this.m_prevDifficulty))
            {
                return;
            }
            string key = str;
            if (key != null)
            {
                int num;
                if (<>f__switch$map5 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
                    dictionary.Add("impossible", 0);
                    dictionary.Add("very_hard", 1);
                    dictionary.Add("hard", 2);
                    <>f__switch$map5 = dictionary;
                }
                if (<>f__switch$map5.TryGetValue(key, out num))
                {
                    switch (num)
                    {
                        case 0:
                            this.ChallengeDifficultyText.text = StringExtensions.ToLowerLoca(_.L(ConfigLoca.PROGR_RIBBON_DIFFICULTY_IMPOSSIBLE, null, false));
                            this.ChallengeButtonPulsatingGraphic.enabled = false;
                            goto Label_0190;

                        case 1:
                            this.ChallengeDifficultyText.text = StringExtensions.ToLowerLoca(_.L(ConfigLoca.PROGR_RIBBON_DIFFICULTY_VERY_HARD, null, false));
                            this.ChallengeButtonPulsatingGraphic.enabled = false;
                            goto Label_0190;

                        case 2:
                            this.ChallengeDifficultyText.text = StringExtensions.ToLowerLoca(_.L(ConfigLoca.PROGR_RIBBON_DIFFICULTY_HARD, null, false));
                            this.ChallengeButtonPulsatingGraphic.enabled = false;
                            goto Label_0190;
                    }
                }
            }
            this.ChallengeDifficultyText.text = StringExtensions.ToLowerLoca(_.L(ConfigLoca.PROGR_RIBBON_DIFFICULTY_READY, null, false));
            this.ChallengeButtonPulsatingGraphic.enabled = true;
        Label_0190:
            this.m_prevDifficulty = str;
        }

        public void refreshTimerProgressBar([Optional, DefaultParameterValue(false)] bool instant)
        {
            this.m_pendingTimerProgressRefresh = false;
            if (this.TimerButtonRoot.activeInHierarchy)
            {
                ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                Player player = GameLogic.Binder.GameState.Player;
                int count = player.getRemainingMinionKillsUntilFloorCompletion(activeDungeon.Floor, activeDungeon.isTutorialDungeon(), player.getLastBossEncounterFailed(false));
                int num2 = player.getRequiredMinionKillsForFloorCompletion(activeDungeon.Floor, activeDungeon.isTutorialDungeon(), player.getLastBossEncounterFailed(false));
                if (count > 1)
                {
                    this.TimerButtonText.text = _.L(ConfigLoca.PROGR_RIBBON_KILL_ENEMIES, new <>__AnonType4<int>(count), false);
                }
                else if (count == 1)
                {
                    this.TimerButtonText.text = _.L(ConfigLoca.PROGR_RIBBON_KILL_ENEMY, null, false);
                }
                else
                {
                    this.TimerButtonText.text = _.L(ConfigLoca.PROGR_RIBBON_KILLED_ALL_ENEMIES, null, false);
                }
                float v = ((float) count) / ((float) num2);
                if (!instant && (v <= 0f))
                {
                    this.TimerBarCompleteEffect.Play();
                }
                if (v < 1f)
                {
                    v = Mathf.Clamp((float) (1f - v), (float) 0.1f, (float) 1f);
                }
                else
                {
                    v = Mathf.Clamp((float) (1f - v), (float) 0f, (float) 1f);
                }
                if (instant)
                {
                    this.TimerProgressBar.setNormalizedValue(v);
                }
                else
                {
                    this.TimerProgressBar.animateToNormalizedValue(v, 0.4f, null, null, 0f);
                }
            }
        }

        protected void Update()
        {
            this.refresh();
            if (this.m_pendingBossHpRefresh)
            {
                this.refreshBossHp();
            }
            if (this.m_pendingTimerProgressRefresh)
            {
                this.refreshTimerProgressBar(false);
            }
        }

        public bool BossSummoningLocked
        {
            [CompilerGenerated]
            get
            {
                return this.<BossSummoningLocked>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<BossSummoningLocked>k__BackingField = value;
            }
        }
    }
}

