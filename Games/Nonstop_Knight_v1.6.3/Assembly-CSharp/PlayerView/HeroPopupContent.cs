namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class HeroPopupContent : MenuContent
    {
        public Text AchievementsButtonText;
        public AnimatedProgressBar AchievementsProgressBar;
        public Image AchievementsProgressBarImageFg;
        public Text AchievementsProgressBarText;
        public Text AchievementsText;
        public Text AtkText;
        public Text AtkTitle;
        public Text AugmentationsButtonText;
        public Text AugmentationsPurchased;
        public Text AugmentationsText;
        public CodexWidget CodexChests;
        public Text CommunityButtonText;
        public Text DefenseTitle;
        public Text DefText;
        public Text DefTitle;
        public Image GenderToggleImage;
        public Text HeaderTitle;
        public MenuDragHandler HeroAvatarDragHandler;
        public RawImage HeroAvatarImage;
        public Button HeroRenamingButton;
        public RectTransform LeaderboardButtonRectTm;
        public Text LeaderboardButtonText;
        public Text LeaderboardRank1;
        public Text LeaderboardRank2;
        public Text LeaderboardText;
        private List<HeroStatCell> m_heroStatCells = new List<HeroStatCell>(((HeroStats.STAT_HEADERS.Count + CharacterInstance.OFFENSE_STAT_HEADERS.Count) + CharacterInstance.DEFENSE_STAT_HEADERS.Count) + CharacterInstance.UTILITY_STAT_HEADERS.Count);
        private InputParameters? m_inputParams;
        public RectTransform MissionsButtonRectTm;
        public Text MissionsButtonText;
        public Text MissionsCount1;
        public Text MissionsCount2;
        public GameObject MissionsRoot;
        public GameObject MissionsRootSeparator;
        public Text MissionsText;
        public Text MoreTitle;
        public Text OffenseTitle;
        public UnityEngine.UI.ScrollRect ScrollRect;
        public Text SettingsButtonText;
        public Text SkillsButtonText;
        public GameObject SkillsNotifier;
        public AnimatedProgressBar SkillsProgressBar;
        public Image SkillsProgressBarImageFg;
        public Text SkillsProgressBarText;
        public Text SkillsText;
        public PrettyButton SkillsTitleButton;
        public Text SklText;
        public Text SklTitle;
        public Text StatsAllTimeSubtitle;
        public Text StatsButtonText;
        public Text StatsCurrentSubtitle;
        public Text StatsTitle;
        public Text UtilityTitle;

        public void onAchievementButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                this.showAchievementContent(false);
            }
        }

        public void onAugmentationsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                this.showAugmentationsContent(false);
            }
        }

        protected override void onAwake()
        {
            this.AtkTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_POWER_ATTACK, null, false));
            this.DefTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_POWER_DEFENSE, null, false));
            this.SklTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_POWER_SKILLDMG, null, false));
            this.StatsTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_STATS_BUTTON_TEXT, null, false));
            this.StatsCurrentSubtitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROSTATS_HEADER_CURRENT, null, false));
            this.StatsAllTimeSubtitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROSTATS_HEADER_ALL_TIME, null, false));
            this.OffenseTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ATTRIBUTES_OFFENSE_HEADER, null, false));
            this.DefenseTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ATTRIBUTES_DEFENSE_HEADER, null, false));
            this.UtilityTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ATTRIBUTES_UTILITY_HEADER, null, false));
            this.LeaderboardText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_LEADERBOARD_TEXT, null, false));
            this.LeaderboardButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_LEADERBOARD_BUTTON_TEXT, null, false));
            this.MissionsText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_MISSIONS_TEXT, null, false));
            this.MissionsButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_MISSIONS_BUTTON_TEXT, null, false));
            this.SkillsText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_SKILLS_TEXT, null, false));
            this.SkillsButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_SKILLS_BUTTON_TEXT, null, false));
            this.CodexChests.CodexText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_CODEX_CHESTS_TEXT, null, false));
            this.CodexChests.CodexButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_CODEX_CHESTS_BUTTON_TEXT, null, false));
            this.AchievementsText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_ACHIEVEMENTS_TEXT, null, false));
            this.AchievementsButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_ACHIEVEMENTS_BUTTON_TEXT, null, false));
            this.AugmentationsText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_AUGMENTATIONS_TEXT, null, false));
            this.AugmentationsButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_AUGMENTATIONS_BUTTON_TEXT, null, false));
            this.MoreTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_TITLE_MORE, null, false));
            this.StatsButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_STATS_BUTTON_TEXT, null, false));
            this.SettingsButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_SETTINGS_BUTTON_TEXT, null, false));
            int num = this.StatsTitle.rectTransform.parent.GetSiblingIndex() + 1;
            for (int i = 0; i < this.m_heroStatCells.Capacity; i++)
            {
                GameObject obj2 = ResourceUtil.Instantiate<GameObject>("Prefabs/Menu/HeroStatCell");
                this.m_heroStatCells.Add(obj2.GetComponent<HeroStatCell>());
                obj2.transform.SetParent(this.StatsTitle.rectTransform.parent.parent);
                switch (i)
                {
                    case 20:
                    case 0x1b:
                    case 0x1f:
                        num++;
                        break;
                }
                obj2.transform.SetSiblingIndex(num++);
            }
        }

        protected override void onCleanup()
        {
            for (int i = this.m_heroStatCells.Count - 1; i >= 0; i--)
            {
                HeroStatCell cell = this.m_heroStatCells[i];
                cell.gameObject.SetActive(false);
            }
            PlayerView.Binder.MenuSystem.CharacterMenuCamera.Camera.enabled = false;
            MenuCharacterView target = PlayerView.Binder.MenuSystem.CharacterMenuCamera.Target;
            this.HeroAvatarDragHandler.OnDragged -= new MenuDragHandler.Dragged(target.OnDrag);
            PlayerView.Binder.MenuSystem.MenuHero.CharacterView.gameObject.SetActive(false);
        }

        public void onCodexChestsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                this.showChestContent(false);
            }
        }

        public void onCodexRunestonesButtonClicked()
        {
            if (PlayerView.Binder.MenuSystem.InTransition)
            {
            }
        }

        public void onCommunityButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                this.showCommunityContent(false);
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnMissionStarted -= new GameLogic.Events.MissionStarted(this.onMissionStarted);
            GameLogic.Binder.EventBus.OnMissionEnded -= new GameLogic.Events.MissionEnded(this.onMissionEnded);
            GameLogic.Binder.EventBus.OnPlayerActiveCharacterSwitched -= new GameLogic.Events.PlayerActiveCharacterSwitched(this.onPlayerActiveCharacterSwitched);
            GameLogic.Binder.EventBus.OnPlayerRenamed -= new GameLogic.Events.PlayerRenamed(this.onPlayerRenamed);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnMissionStarted += new GameLogic.Events.MissionStarted(this.onMissionStarted);
            GameLogic.Binder.EventBus.OnMissionEnded += new GameLogic.Events.MissionEnded(this.onMissionEnded);
            GameLogic.Binder.EventBus.OnPlayerActiveCharacterSwitched += new GameLogic.Events.PlayerActiveCharacterSwitched(this.onPlayerActiveCharacterSwitched);
            GameLogic.Binder.EventBus.OnPlayerRenamed += new GameLogic.Events.PlayerRenamed(this.onPlayerRenamed);
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            this.refreshMissionFeatureAvailability(activeDungeon.PrimaryPlayerCharacter.OwningPlayer);
        }

        public void onGenderToggleClicked()
        {
            PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_ButtonTab, (float) 0f);
            Player player = GameLogic.Binder.GameState.Player;
            if (player.ActiveCharacter.CharacterId == "Hero001")
            {
                CmdSwitchActiveCharacter.ExecuteStatic(player, "Hero002");
            }
            else
            {
                CmdSwitchActiveCharacter.ExecuteStatic(player, "Hero001");
            }
        }

        public void onHeroNamingButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.HeroNamingPopupContent, null, 0f, false, true);
            }
        }

        public void onLeaderboardButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                this.showLeaderboardContent(false);
            }
        }

        private void onMissionEnded(Player player, MissionInstance mission, bool success)
        {
            this.refreshMissionCount(player);
        }

        public void onMissionsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                this.showMissionsContent(false);
            }
        }

        private void onMissionStarted(Player player, MissionInstance mission)
        {
            this.refreshMissionCount(player);
        }

        public void onOptionsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                ((StackedPopupMenu) base.m_contentMenu).Smcc.pushContent(MenuContentType.OptionsContent, null, false);
            }
        }

        private void onPlayerActiveCharacterSwitched(CharacterInstance activeCharacter)
        {
            this.refreshGenderToggle();
            PlayerView.Binder.MenuSystem.initializeMenuHero(activeCharacter);
            PlayerView.Binder.MenuSystem.MenuHero.CharacterView.gameObject.SetActive(true);
            PlayerView.Binder.MenuSystem.MenuHero.CharacterView.setVisibility(true);
        }

        private void onPlayerRenamed(Player player)
        {
            this.onRefresh();
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (param != null)
            {
                this.m_inputParams = new InputParameters?((InputParameters) param);
            }
            else
            {
                this.m_inputParams = null;
            }
            PlayerView.Binder.MenuSystem.initializeMenuHero(player.ActiveCharacter);
            PlayerView.Binder.MenuSystem.CharacterMenuCamera.Target = PlayerView.Binder.MenuSystem.MenuHero;
            PlayerView.Binder.MenuSystem.MenuHero.CharacterView.gameObject.SetActive(true);
            PlayerView.Binder.MenuSystem.MenuHero.CharacterView.setVisibility(true);
            PlayerView.Binder.MenuSystem.CharacterMenuCamera.Camera.enabled = true;
            MenuCharacterView target = PlayerView.Binder.MenuSystem.CharacterMenuCamera.Target;
            this.HeroAvatarDragHandler.OnDragged += new MenuDragHandler.Dragged(target.OnDrag);
            this.refreshGenderToggle();
            this.refreshMissionCount(player);
            this.refreshMissionFeatureAvailability(player);
            for (int i = 0; i < this.m_heroStatCells.Count; i++)
            {
                this.m_heroStatCells[i].gameObject.SetActive(true);
            }
            this.onRefresh();
            if (this.m_inputParams.HasValue && (this.m_inputParams.Value.DefaultMenuContent != MenuContentType.NONE))
            {
                ((StackedPopupMenu) base.m_contentMenu).Smcc.pushContent(this.m_inputParams.Value.DefaultMenuContent, null, false);
            }
        }

        protected override void onRefresh()
        {
            Player player = GameLogic.Binder.GameState.Player;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_TITLE, null, false)), string.Empty, string.Empty);
            if (!string.IsNullOrEmpty(player.SocialData.Name))
            {
                this.HeaderTitle.text = StringExtensions.ToUpperLoca(player.SocialData.Name);
            }
            else
            {
                this.HeaderTitle.text = StringExtensions.ToUpperLoca(player.ActiveCharacter.Name);
            }
            this.HeroAvatarImage.texture = PlayerView.Binder.MenuSystem.CharacterMenuCamera.RenderTexture;
            this.HeroAvatarImage.enabled = true;
            this.AtkText.text = MenuHelpers.BigValueToString(activeCharacter.DamagePerHit(false));
            this.DefText.text = MenuHelpers.BigValueToString(activeCharacter.MaxLife(false));
            this.SklText.text = MenuHelpers.BigValueToString(activeCharacter.SkillDamage(false));
            this.refreshHeroStats();
        }

        public void onRunestonesButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                ((StackedPopupMenu) base.m_contentMenu).Smcc.pushContent(MenuContentType.RunestoneGalleryContent, null, false);
            }
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator120 iterator = new <onShow>c__Iterator120();
            iterator.<>f__this = this;
            return iterator;
        }

        public void onSkillsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                ((StackedPopupMenu) base.m_contentMenu).Smcc.pushContent(MenuContentType.SkillPopupContent, null, false);
            }
        }

        public void onStatsButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                ((StackedPopupMenu) base.m_contentMenu).Smcc.pushContent(MenuContentType.HeroStatsContent, null, false);
            }
        }

        private void refreshGenderToggle()
        {
            if (this.GenderToggleImage != null)
            {
                if (GameLogic.Binder.GameState.Player.ActiveCharacter.CharacterId == "Hero001")
                {
                    this.GenderToggleImage.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "ui_button_gender_male");
                }
                else
                {
                    this.GenderToggleImage.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "ui_button_gender_female");
                }
            }
        }

        private void refreshHeroStats()
        {
            Player player = GameLogic.Binder.GameState.Player;
            int num = 0;
            HeroStats stats = new HeroStats(player.ActiveCharacter.HeroStats);
            double num2 = 0.0;
            Reward reward = player.getFirstUnclaimedRetirementTriggerChest();
            if (reward != null)
            {
                num2 += reward.getTotalTokenAmount();
            }
            stats.TokensEarned += Math.Floor((num2 + player.ActiveCharacter.getTotalEquipmentTokenValue()) * player.getActiveTokenRewardFloorMultiplier());
            HeroStats stats2 = new HeroStats(player.CumulativeRetiredHeroStats);
            stats2.add(player.ActiveCharacter.HeroStats);
            stats2.HighestFloor = player.CumulativeRetiredHeroStats.HighestFloor;
            List<string> list = stats.toRichTextFormattedStringList(StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROSTATS_HEADER_CURRENT, null, false)), false);
            List<string> list2 = stats2.toRichTextFormattedStringList(StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROSTATS_HEADER_ALL_TIME, null, false)), true);
            for (int i = 0; i < HeroStats.STAT_HEADERS.Count; i++)
            {
                string str = _.L(HeroStats.STAT_HEADERS[i], null, false);
                this.m_heroStatCells[num].setThreeColumnLayout(str, list[i], list2[i], (num % 2) != 0);
                num++;
            }
            List<string> list3 = player.ActiveCharacter.offenseAttributesToRichTextFormattedStringList(string.Empty);
            for (int j = 0; j < CharacterInstance.OFFENSE_STAT_HEADERS.Count; j++)
            {
                string str2 = _.L(CharacterInstance.OFFENSE_STAT_HEADERS[j], null, false);
                this.m_heroStatCells[num].setTwoColumnLayout(str2, list3[j], (num % 2) != 0);
                num++;
            }
            List<string> list4 = player.ActiveCharacter.defenseAttributesToRichTextFormattedStringList(string.Empty);
            for (int k = 0; k < CharacterInstance.DEFENSE_STAT_HEADERS.Count; k++)
            {
                string str3 = _.L(CharacterInstance.DEFENSE_STAT_HEADERS[k], null, false);
                this.m_heroStatCells[num].setTwoColumnLayout(str3, list4[k], (num % 2) != 0);
                num++;
            }
            List<string> list5 = player.ActiveCharacter.utilityAttributesToRichTextFormattedStringList(string.Empty);
            for (int m = 0; m < CharacterInstance.UTILITY_STAT_HEADERS.Count; m++)
            {
                string str4 = _.L(CharacterInstance.UTILITY_STAT_HEADERS[m], null, false);
                this.m_heroStatCells[num].setTwoColumnLayout(str4, list5[m], (num % 2) != 0);
                num++;
            }
        }

        private void refreshMissionCount(Player player)
        {
            int count = player.Missions.Instances.Count;
            if (count == 0)
            {
                this.MissionsCount1.text = "-";
                this.MissionsCount2.text = "/-";
            }
            else
            {
                this.MissionsCount1.text = player.Missions.getNumActiveMissions().ToString();
                this.MissionsCount2.text = "/" + count;
            }
        }

        private void refreshMissionFeatureAvailability(Player player)
        {
            this.MissionsRoot.SetActive(false);
            this.MissionsRootSeparator.SetActive(false);
        }

        private void SetDeepLink(MenuContentType deepLink)
        {
            switch (deepLink)
            {
                case MenuContentType.ChestGalleryContent:
                    this.showChestContent(true);
                    return;

                case MenuContentType.PlayerAugmentationPopupContent:
                    this.showAugmentationsContent(true);
                    return;

                case MenuContentType.AchievementPopupContent:
                    this.showAchievementContent(true);
                    break;

                case MenuContentType.LeaderboardPopupContent:
                    this.showLeaderboardContent(true);
                    break;

                case MenuContentType.CommunityContent:
                    this.showCommunityContent(true);
                    break;

                case MenuContentType.MissionsPopupContent:
                    this.showMissionsContent(true);
                    break;
            }
        }

        private void showAchievementContent([Optional, DefaultParameterValue(false)] bool instant)
        {
            ((StackedPopupMenu) base.m_contentMenu).Smcc.pushContent(MenuContentType.AchievementPopupContent, null, instant);
        }

        private void showAugmentationsContent([Optional, DefaultParameterValue(false)] bool instant)
        {
            ((StackedPopupMenu) base.m_contentMenu).Smcc.pushContent(MenuContentType.PlayerAugmentationPopupContent, null, instant);
        }

        private void showChestContent([Optional, DefaultParameterValue(false)] bool instant)
        {
            ((StackedPopupMenu) base.m_contentMenu).Smcc.pushContent(MenuContentType.ChestGalleryContent, null, instant);
        }

        private void showCommunityContent([Optional, DefaultParameterValue(false)] bool instant)
        {
            ((StackedPopupMenu) base.m_contentMenu).Smcc.pushContent(MenuContentType.OptionsContent, null, instant);
        }

        private void showLeaderboardContent([Optional, DefaultParameterValue(false)] bool instant)
        {
            ((StackedPopupMenu) base.m_contentMenu).Smcc.pushContent(MenuContentType.LeaderboardPopupContent, null, instant);
        }

        private void showMissionsContent([Optional, DefaultParameterValue(false)] bool instant)
        {
            ((StackedPopupMenu) base.m_contentMenu).Smcc.pushContent(MenuContentType.MissionsPopupContent, null, instant);
        }

        protected void Update()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (!GameLogic.Binder.LeaderboardSystem.Initialized)
            {
                this.LeaderboardRank1.text = "-";
                this.LeaderboardRank2.text = "/-";
            }
            else
            {
                this.LeaderboardRank1.text = GameLogic.Binder.LeaderboardSystem.getLeaderboardRankForPlayer(LeaderboardType.Royal, player).ToString();
                this.LeaderboardRank2.text = "/" + GameLogic.Binder.LeaderboardSystem.getSortedLeaderboardEntries(LeaderboardType.Royal).Count.ToString();
                if (GameLogic.Binder.LeaderboardSystem.getNextLeaderboardTargetForPlayer(LeaderboardType.Royal, player) != null)
                {
                }
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.HeroPopupContent;
            }
        }

        public override string TabTitle
        {
            get
            {
                return StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_TITLE, null, false));
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator120 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal HeroPopupContent <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (((this.$PC == 0) && this.<>f__this.m_inputParams.HasValue) && (this.<>f__this.m_inputParams.Value.DeepLink != MenuContentType.NONE))
                {
                    this.<>f__this.SetDeepLink(this.<>f__this.m_inputParams.Value.DeepLink);
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

        [Serializable]
        public class CodexWidget
        {
            public Text CodexButtonText;
            public GameObject CodexNotifier;
            public AnimatedProgressBar CodexProgressBar;
            public Image CodexProgressBarImageFg;
            public Text CodexProgressBarText;
            public Text CodexText;
            public PrettyButton CodexTitleButton;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public MenuContentType DefaultMenuContent;
            public MenuContentType DeepLink;
        }
    }
}

