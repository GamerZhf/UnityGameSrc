namespace GameLogic
{
    using App;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdRetire : ICommand
    {
        private Player m_player;

        public CmdRetire(Player player)
        {
            this.m_player = player;
        }

        public CmdRetire(string[] serialized)
        {
            this.m_player = GameLogic.Binder.GameState.Player;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorB1 rb = new <executeRoutine>c__IteratorB1();
            rb.<>f__this = this;
            return rb;
        }

        public static void ExecuteStatic(Player player)
        {
            double num2;
            double num3;
            double num4;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            CharacterInstance.HighestLevelItemInfo highestLevelItemOwnedAtFloorStart = activeCharacter.HighestLevelItemOwnedAtFloorStart;
            highestLevelItemOwnedAtFloorStart.Rank = (int) Math.Floor(highestLevelItemOwnedAtFloorStart.Rank * App.Binder.ConfigMeta.PREASCENSION_LEVELMULTIPLIER);
            int retirementFloor = player.getLastCompletedFloor(true) + 1;
            double num5 = player.getTotalTokensFromRetirement(out num2, out num3, out num4);
            player.CumulativeRetiredHeroStats.add(player.ActiveCharacter.HeroStats);
            player.CumulativeRetiredHeroStats.HeroesRetired++;
            player.CumulativeRetiredHeroStats.HighestTokenGainWithRetirement = Math.Max(player.CumulativeRetiredHeroStats.HighestTokenGainWithRetirement, num5);
            Player player2 = JsonUtils.Deserialize<Player>(ResourceUtil.LoadSafe<TextAsset>("Players/humanPlayer1", false).text, true);
            double amount = player.getStartingCoins();
            int num7 = player.getStartingFloor();
            player.setResourceAmount(ResourceType.Coin, amount);
            player.setLastCompletedFloor(Mathf.Max(num7 - 1, 0), true);
            player.UnclaimedPassiveRewardableSeconds = 0L;
            player.LastPassiveRewardClaimTimestamp = Service.Binder.ServerTime.GameTime;
            player.setMinionsKilledSinceLastRoomCompletion(0, true);
            player.setLastBossEncounterFailed(false, true);
            player.LastMysteryChestDropTimestamp = 0L;
            player.BossTrain.Active = false;
            player.rerollRetirementRandomSeed();
            player.MysteryChestsWithCoinsConsumed = 0;
            for (int i = player.UnclaimedRewards.Count - 1; i >= 0; i--)
            {
                Reward reward = player.UnclaimedRewards[i];
                if (!ConfigMeta.IsRetirementChest(reward.ChestType))
                {
                    player.UnclaimedRewards.Remove(reward);
                }
            }
            double num9 = activeCharacter.getTotalEquipmentTokenValue();
            List<SkillInstance> list = new List<SkillInstance>(activeCharacter.SkillInstances);
            List<SkillType> list2 = new List<SkillType>(activeCharacter.ActiveSkillTypes);
            Inventory inventory = activeCharacter.Inventory;
            string characterId = activeCharacter.CharacterId;
            activeCharacter.copyFrom(player2.ActiveCharacter);
            activeCharacter.SkillInstances = list;
            activeCharacter.ActiveSkillTypes = list2;
            activeCharacter.CharacterId = characterId;
            inventory.ItemInstances = activeCharacter.Inventory.ItemInstances;
            activeCharacter.Inventory = inventory;
            Reward item = player.getFirstUnclaimedRetirementTriggerChest();
            if (item == null)
            {
                Reward reward5 = new Reward();
                reward5.ChestType = ChestType.RetirementTrigger;
                item = reward5;
                player.UnclaimedRewards.Add(item);
            }
            if (num9 > 0.0)
            {
                item.addResourceDrop(ResourceType.Token, num9);
            }
            double num10 = item.getTotalTokenAmount();
            item.TokenDrops.Clear();
            item.TokenDrops.Add(Math.Floor(num10 * num4));
            for (int j = 0; j < activeCharacter.ItemSlots.Count; j++)
            {
                ItemInstance itemInstance = activeCharacter.ItemSlots[j].ItemInstance;
                if (itemInstance != null)
                {
                    itemInstance.Rank = Mathf.Max((num7 * App.Binder.ConfigMeta.LEVEL_TO_FLOOR_MULTIPLIER) - itemInstance.Level, 0);
                }
            }
            for (int k = 0; k < player.UnclaimedRewards.Count; k++)
            {
                Reward reward3 = player.UnclaimedRewards[k];
                if (ConfigMeta.IsRetirementChest(reward3.ChestType))
                {
                    for (int n = 0; n < reward3.ItemDrops.Count; n++)
                    {
                        ItemInstance instance3 = reward3.ItemDrops[n];
                        instance3.Rank = Mathf.Max((num7 * App.Binder.ConfigMeta.LEVEL_TO_FLOOR_MULTIPLIER) - instance3.Level, 0);
                    }
                }
            }
            for (int m = 0; m < player.UnclaimedRewards.Count; m++)
            {
                Reward reward4 = player.UnclaimedRewards[m];
                if (ConfigMeta.IsRetirementChest(reward4.ChestType) && (reward4.ChestType != ChestType.RetirementTrigger))
                {
                    if (reward4.getTotalCoinAmount() > 0.0)
                    {
                        string shopEntryId = reward4.ShopEntryId;
                        if (string.IsNullOrEmpty(reward4.ShopEntryId) || !App.Binder.ConfigMeta.VENDOR_COIN_BUNDLES.ContainsKey(reward4.ShopEntryId))
                        {
                            shopEntryId = "CoinBundleSmall";
                        }
                        reward4.CoinDrops.Clear();
                        CharacterInstance.HighestLevelItemInfo info = !App.Binder.ConfigMeta.COINBUNDLES_USE_PREASCENSIONITEMS ? activeCharacter.HighestLevelItemOwnedAtFloorStart : highestLevelItemOwnedAtFloorStart;
                        double num15 = App.Binder.ConfigMeta.CoinBundleSize(info, player, shopEntryId, 0.0);
                        reward4.CoinDrops.Add(num15);
                    }
                    if (reward4.getTotalTokenAmount() > 0.0)
                    {
                        string str3 = reward4.ShopEntryId;
                        if (string.IsNullOrEmpty(reward4.ShopEntryId) || !App.Binder.ConfigMeta.VENDOR_TOKEN_BUNDLES.ContainsKey(reward4.ShopEntryId))
                        {
                            str3 = "TokenBundleSmall";
                        }
                        reward4.TokenDrops.Clear();
                        double num16 = App.Binder.ConfigMeta.TokenBundleSize(player, str3);
                        reward4.TokenDrops.Add(num16);
                    }
                }
            }
            activeCharacter.postDeserializeInitialization();
            player.PendingPostRetirementTokenRewardCeremony = true;
            player.PendingPostRetirementGiftRewardCeremony = true;
            player.HighestFloorRewardClaimedDuringThisRun = false;
            GameLogic.Binder.EventBus.PlayerRetired(player, retirementFloor);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorB1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdRetire <>f__this;

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
                    CmdRetire.ExecuteStatic(this.<>f__this.m_player);
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

