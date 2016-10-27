namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class AdventureMilestones
    {
        [CompilerGenerated]
        private List<MilestoneData> <SortedList>k__BackingField;
        public const int ADVENTURE_MILESTONES_TOTAL = 3;
        private int m_refreshBasedOnFloor = -1;

        public AdventureMilestones()
        {
            this.SortedList = new List<MilestoneData>(3);
            for (int i = 0; i < this.SortedList.Capacity; i++)
            {
                this.SortedList.Add(new MilestoneData());
            }
        }

        public void fillCells(Player player, int startFloor, int milestoneFloor, List<IconWithText> cells, bool useCanvasGroupInsteadOfRoot, bool skipCellIfRewardNotFound)
        {
            if (startFloor != this.m_refreshBasedOnFloor)
            {
                this.refresh(player, startFloor);
            }
            int num = 0;
            bool flag = false;
            for (int i = 0; i < this.SortedList.Count; i++)
            {
                MilestoneData data = this.SortedList[i];
                if ((data.Floor == milestoneFloor) && (data.GroupType == MilestoneData.Group.TokenMultiplier))
                {
                    IconWithText text = cells[num++];
                    text.Text.text = data.Text + "X";
                    text.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_tokenmultiplier");
                    if (useCanvasGroupInsteadOfRoot)
                    {
                        text.CanvasGroup.gameObject.SetActive(true);
                    }
                    else
                    {
                        text.gameObject.SetActive(true);
                    }
                    flag = true;
                }
            }
            if (!flag && skipCellIfRewardNotFound)
            {
                if (useCanvasGroupInsteadOfRoot)
                {
                    cells[num].CanvasGroup.gameObject.SetActive(false);
                }
                else
                {
                    cells[num].gameObject.SetActive(false);
                }
                num++;
            }
            int num3 = 0;
            for (int j = 0; j < this.SortedList.Count; j++)
            {
                MilestoneData data2 = this.SortedList[j];
                if ((data2.Floor == milestoneFloor) && (data2.GroupType == MilestoneData.Group.RewardBox))
                {
                    num3++;
                }
            }
            if (num3 > 0)
            {
                IconWithText text2 = cells[num++];
                text2.Text.text = "+" + num3;
                text2.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(ConfigUi.CHEST_BLUEPRINTS[ChestType.RewardBoxCommon].SoloSprite);
                if (useCanvasGroupInsteadOfRoot)
                {
                    text2.CanvasGroup.gameObject.SetActive(true);
                }
                else
                {
                    text2.gameObject.SetActive(true);
                }
            }
            if ((num3 == 0) && skipCellIfRewardNotFound)
            {
                if (useCanvasGroupInsteadOfRoot)
                {
                    cells[num].CanvasGroup.gameObject.SetActive(false);
                }
                else
                {
                    cells[num].gameObject.SetActive(false);
                }
                num++;
            }
            int num5 = 0;
            for (int k = 0; k < this.SortedList.Count; k++)
            {
                MilestoneData data3 = this.SortedList[k];
                if ((data3.Floor == milestoneFloor) && (data3.GroupType == MilestoneData.Group.FrenzyPotion))
                {
                    num5++;
                }
            }
            for (int m = 0; m < this.SortedList.Count; m++)
            {
                MilestoneData data4 = this.SortedList[m];
                if ((data4.Floor == milestoneFloor) && (data4.GroupType == MilestoneData.Group.FrenzyPotion))
                {
                    IconWithText text3 = cells[num++];
                    text3.Text.text = "+" + num5;
                    text3.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_bottle_frenzy_floater");
                    if (useCanvasGroupInsteadOfRoot)
                    {
                        text3.CanvasGroup.gameObject.SetActive(true);
                    }
                    else
                    {
                        text3.gameObject.SetActive(true);
                    }
                }
            }
            for (int n = num; n < cells.Count; n++)
            {
                if (useCanvasGroupInsteadOfRoot)
                {
                    cells[n].CanvasGroup.gameObject.SetActive(false);
                }
                else
                {
                    cells[n].gameObject.SetActive(false);
                }
            }
        }

        public bool floorHasMilestone(Player player, int floor)
        {
            if (floor != this.m_refreshBasedOnFloor)
            {
                this.refresh(player, floor);
            }
            for (int i = 0; i < this.SortedList.Count; i++)
            {
                if (this.SortedList[i].Floor == floor)
                {
                    return true;
                }
            }
            return false;
        }

        public MilestoneData getNextMilestoneData(Player player, int floor)
        {
            if (floor != this.m_refreshBasedOnFloor)
            {
                this.refresh(player, floor);
            }
            if (this.SortedList[0].isFilled())
            {
                return this.SortedList[0];
            }
            return null;
        }

        private void refresh(Player player, int fromFloor)
        {
            int num3;
            double num4;
            for (int i = 0; i < this.SortedList.Count; i++)
            {
                this.SortedList[i].clear();
            }
            int num2 = 0;
            player.getNextTokenRewardFloorMultiplierInfo(fromFloor, out num3, out num4);
            if (num3 > -1)
            {
                MilestoneData data = this.SortedList[num2++];
                data.Floor = num3;
                data.Text = num4.ToString("0.0");
                data.GroupType = MilestoneData.Group.TokenMultiplier;
            }
            int nextGuaranteedFrenzyPotionDropFloor = App.Binder.ConfigMeta.GetNextGuaranteedFrenzyPotionDropFloor(fromFloor - 1);
            if (nextGuaranteedFrenzyPotionDropFloor < 0x7fffffff)
            {
                MilestoneData data2 = this.SortedList[num2++];
                data2.Floor = nextGuaranteedFrenzyPotionDropFloor + 1;
                data2.GroupType = MilestoneData.Group.FrenzyPotion;
            }
            int nextGuaranteedRewardBoxDropFloor = App.Binder.ConfigMeta.GetNextGuaranteedRewardBoxDropFloor(fromFloor - 1);
            if (nextGuaranteedRewardBoxDropFloor < 0x7fffffff)
            {
                MilestoneData data3 = this.SortedList[num2++];
                data3.Floor = nextGuaranteedRewardBoxDropFloor + 1;
                data3.GroupType = MilestoneData.Group.RewardBox;
            }
            this.SortedList.Sort(new Comparison<MilestoneData>(MilestoneData.SortByFloor));
            this.m_refreshBasedOnFloor = fromFloor;
        }

        public List<MilestoneData> SortedList
        {
            [CompilerGenerated]
            get
            {
                return this.<SortedList>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<SortedList>k__BackingField = value;
            }
        }

        public class MilestoneData
        {
            public int Floor = 0x7fffffff;
            public Group GroupType;
            public string Text;

            public void clear()
            {
                this.Floor = 0x7fffffff;
                this.Text = null;
            }

            public bool isFilled()
            {
                return (this.Floor < 0x7fffffff);
            }

            public static int SortByFloor(AdventureMilestones.MilestoneData x, AdventureMilestones.MilestoneData y)
            {
                int num = x.Floor.CompareTo(y.Floor);
                if (num != 0)
                {
                    return num;
                }
                if (!string.IsNullOrEmpty(x.Text) && !string.IsNullOrEmpty(y.Text))
                {
                    num = x.Text.CompareTo(y.Text);
                    if (num != 0)
                    {
                        return num;
                    }
                }
                return x.GetHashCode().CompareTo(y.GetHashCode());
            }

            public enum Group
            {
                TokenMultiplier,
                RewardBox,
                FrenzyPotion
            }
        }
    }
}

