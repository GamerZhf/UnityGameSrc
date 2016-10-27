namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class HeroStatsContent : MenuContent
    {
        private List<HeroStatCell> m_heroStatCells = new List<HeroStatCell>((((((HeroStats.STAT_HEADERS.Count + 1) + CharacterInstance.OFFENSE_STAT_HEADERS.Count) + 1) + CharacterInstance.DEFENSE_STAT_HEADERS.Count) + 1) + CharacterInstance.UTILITY_STAT_HEADERS.Count);
        public RectTransform VerticalGroupTm;

        private void cleanupCells()
        {
            for (int i = this.m_heroStatCells.Count - 1; i >= 0; i--)
            {
                HeroStatCell cell = this.m_heroStatCells[i];
                cell.gameObject.SetActive(false);
            }
        }

        protected override void onAwake()
        {
            for (int i = 0; i < this.m_heroStatCells.Capacity; i++)
            {
                GameObject obj2 = ResourceUtil.Instantiate<GameObject>("Prefabs/Menu/HeroStatCell");
                this.m_heroStatCells.Add(obj2.GetComponent<HeroStatCell>());
                obj2.transform.SetParent(this.VerticalGroupTm);
            }
        }

        protected override void onCleanup()
        {
            this.cleanupCells();
        }

        public void onCustomCloseButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        protected void OnDisable()
        {
        }

        protected void OnEnable()
        {
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            for (int i = 0; i < this.m_heroStatCells.Count; i++)
            {
                this.m_heroStatCells[i].gameObject.SetActive(true);
            }
            this.onRefresh();
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_STATS_BUTTON_TEXT, null, false)), string.Empty, string.Empty);
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
                string self = _.L(HeroStats.STAT_HEADERS[i], null, false);
                if (i == 0)
                {
                    self = MenuHelpers.ColoredText(StringExtensions.ToUpperLoca(self));
                }
                this.m_heroStatCells[num].setThreeColumnLayout(self, list[i], list2[i], (num % 2) != 0);
                num++;
            }
            this.m_heroStatCells[num].setEmpty((num % 2) != 0);
            num++;
            List<string> list3 = player.ActiveCharacter.offenseAttributesToRichTextFormattedStringList(string.Empty);
            for (int j = 0; j < CharacterInstance.OFFENSE_STAT_HEADERS.Count; j++)
            {
                string str2 = _.L(CharacterInstance.OFFENSE_STAT_HEADERS[j], null, false);
                if (j == 0)
                {
                    str2 = MenuHelpers.ColoredText(StringExtensions.ToUpperLoca(str2));
                }
                this.m_heroStatCells[num].setTwoColumnLayout(str2, list3[j], (num % 2) != 0);
                num++;
            }
            this.m_heroStatCells[num].setEmpty((num % 2) != 0);
            num++;
            List<string> list4 = player.ActiveCharacter.defenseAttributesToRichTextFormattedStringList(string.Empty);
            for (int k = 0; k < CharacterInstance.DEFENSE_STAT_HEADERS.Count; k++)
            {
                string str3 = _.L(CharacterInstance.DEFENSE_STAT_HEADERS[k], null, false);
                if (k == 0)
                {
                    str3 = MenuHelpers.ColoredText(StringExtensions.ToUpperLoca(str3));
                }
                this.m_heroStatCells[num].setTwoColumnLayout(str3, list4[k], (num % 2) != 0);
                num++;
            }
            this.m_heroStatCells[num].setEmpty((num % 2) != 0);
            num++;
            List<string> list5 = player.ActiveCharacter.utilityAttributesToRichTextFormattedStringList(string.Empty);
            for (int m = 0; m < CharacterInstance.UTILITY_STAT_HEADERS.Count; m++)
            {
                string str4 = _.L(CharacterInstance.UTILITY_STAT_HEADERS[m], null, false);
                if (m == 0)
                {
                    str4 = MenuHelpers.ColoredText(StringExtensions.ToUpperLoca(str4));
                }
                this.m_heroStatCells[num].setTwoColumnLayout(str4, list5[m], (num % 2) != 0);
                num++;
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.HeroStatsContent;
            }
        }
    }
}

