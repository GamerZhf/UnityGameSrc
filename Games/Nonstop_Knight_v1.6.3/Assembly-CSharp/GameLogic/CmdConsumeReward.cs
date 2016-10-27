namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class CmdConsumeReward : ICommand
    {
        private Player m_player;
        private bool m_removeFromUnclaimedRewards;
        private Reward m_reward;
        private string m_trackingId;

        public CmdConsumeReward(Player player, Reward reward, bool removeFromUnclaimedRewards, [Optional, DefaultParameterValue("")] string trackingId)
        {
            this.m_player = player;
            this.m_reward = reward;
            this.m_removeFromUnclaimedRewards = removeFromUnclaimedRewards;
            this.m_trackingId = trackingId;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator97 iterator = new <executeRoutine>c__Iterator97();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(Player player, Reward reward, bool removeFromUnclaimedRewards, [Optional, DefaultParameterValue("")] string trackingId)
        {
            if (player != null)
            {
                if (removeFromUnclaimedRewards && player.UnclaimedRewards.Contains(reward))
                {
                    player.UnclaimedRewards.Remove(reward);
                }
                double amount = reward.getTotalCoinAmount();
                if (amount > 0.0)
                {
                    CmdGainResources.ExecuteStatic(player, ResourceType.Coin, amount, true, trackingId, null);
                }
                double num2 = reward.getTotalDiamondAmount();
                if (num2 > 0.0)
                {
                    CmdGainResources.ExecuteStatic(player, ResourceType.Diamond, num2, true, trackingId, null);
                }
                double num3 = reward.getTotalTokenAmount();
                if (num3 > 0.0)
                {
                    CmdGainResources.ExecuteStatic(player, ResourceType.Token, num3, true, trackingId, null);
                }
                for (int i = 0; i < reward.ItemDrops.Count; i++)
                {
                    CmdGainItemInstance.ExecuteStatic(player.ActiveCharacter, reward.ItemDrops[i], trackingId);
                }
                for (int j = 0; j < reward.RunestoneDrops.Count; j++)
                {
                    string runestoneId = reward.RunestoneDrops[j];
                    if (ConfigMeta.RUNESTONE_DUPLICATES_AUTO_CONVERTED_INTO_TOKENS && player.Runestones.ownsRunestone(runestoneId))
                    {
                        ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(runestoneId);
                        double num6 = App.Binder.ConfigMeta.RunestoneTokenGainCurve(runestoneData.Rarity);
                        Vector3? worldPt = null;
                        CmdGainResources.ExecuteStatic(player, ResourceType.Token, num6, true, trackingId, worldPt);
                        reward.RunestoneAtIndexConvertedIntoTokens = new List<int>();
                        reward.RunestoneAtIndexConvertedIntoTokens.Add(j);
                    }
                    else
                    {
                        CmdGainRunestone.ExecuteStatic(player, reward.RunestoneDrops[j], false);
                    }
                }
                if (reward.Boost != BoostType.UNSPECIFIED)
                {
                    CmdActivateBoost.ExecuteStatic(player, reward.Boost, trackingId);
                }
                if (reward.Revives > 0)
                {
                    CmdGainPotions.ExecuteStatic(player.ActiveCharacter, PotionType.Revive, reward.Revives);
                }
                if ((reward.ShopEntryId == ConfigShops.IAP_STARTER_BUNDLE_ID) && reward.isEmpty())
                {
                    player.HasPurchasedStarterBundle = true;
                    CmdGainPet.ExecuteStatic(player, "Pet001", App.Binder.ConfigMeta.PetRequiredDuplicatesForLevelUp(1), false);
                }
                if (reward.FrenzyPotions > 0)
                {
                    CmdGainPotions.ExecuteStatic(player.ActiveCharacter, PotionType.Frenzy, reward.FrenzyPotions);
                }
                double num7 = reward.getTotalDustAmount();
                if (num7 > 0.0)
                {
                    CmdGainResources.ExecuteStatic(player, ResourceType.Dust, num7, true, trackingId, null);
                }
                if (reward.XpPotions > 0)
                {
                    double num8 = 0.0;
                    for (int m = 0; m < reward.XpPotions; m++)
                    {
                        double num10 = App.Binder.ConfigMeta.XpRequiredForRankUp(player.Rank);
                        num8 += Math.Floor(num10 * App.Binder.ConfigMeta.XP_GAIN_PER_POTION);
                    }
                    CmdGainResources.ExecuteStatic(player, ResourceType.Xp, num8, true, string.Empty, null);
                }
                if (reward.BossPotions > 0)
                {
                    CmdGainPotions.ExecuteStatic(player.ActiveCharacter, PotionType.Boss, reward.BossPotions);
                }
                for (int k = 0; k < reward.Pets.Count; k++)
                {
                    PetReward reward2 = reward.Pets[k];
                    string petId = reward2.PetId;
                    int num12 = reward2.Amount;
                    PetInstance instance = player.Pets.getPetInstance(petId);
                    if ((instance != null) && instance.isAtMaxLevel())
                    {
                        double num13 = ConfigShops.CalculateCoinBundleSize(player, "CoinBundlePetConversion", num12);
                        Vector3? nullable7 = null;
                        CmdGainResources.ExecuteStatic(player, ResourceType.Coin, num13, true, trackingId, nullable7);
                        reward.Pets[k].ConvertIntoShopEntryId = "CoinBundlePetConversion";
                    }
                    else
                    {
                        CmdGainPet.ExecuteStatic(player, petId, num12, false);
                    }
                }
                if (reward.MegaBoxes > 0)
                {
                    for (int n = 0; n < reward.MegaBoxes; n++)
                    {
                        Reward reward4 = new Reward();
                        reward4.ChestType = ChestType.RewardBoxMulti;
                        Reward reward3 = reward4;
                        CmdRollChestLootTable.ExecuteStatic(reward3.ChestType, player, false, ref reward3, null);
                        player.UnclaimedRewards.Add(reward3);
                    }
                }
                if (reward.TournamentUpgradeReward != null)
                {
                    for (int num15 = 0; num15 < reward.TournamentUpgradeReward.Choices.Count; num15++)
                    {
                        TournamentUpgradeReward.Entry entry = reward.TournamentUpgradeReward.Choices[num15];
                        if (entry.Selected)
                        {
                            CmdGainTournamentUpgrade.ExecuteStatic(player, entry.TourmanentUpgradeId, entry.IsEpicUpgrade, reward.TournamentUpgradeReward.NumMilestonesCompleted);
                        }
                    }
                }
                GameLogic.Binder.EventBus.RewardConsumed(player, reward);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator97 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdConsumeReward <>f__this;

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
                    CmdConsumeReward.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_reward, this.<>f__this.m_removeFromUnclaimedRewards, this.<>f__this.m_trackingId);
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

