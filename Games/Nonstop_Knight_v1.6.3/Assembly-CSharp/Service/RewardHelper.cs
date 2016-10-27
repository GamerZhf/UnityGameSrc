namespace Service
{
    using App;
    using GameLogic;
    using PlayerView;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class RewardHelper
    {
        public static void ClaimReward(List<Reward> rewards, [Optional, DefaultParameterValue(true)] bool showCeremony)
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (rewards.Count > 1)
            {
                player.UnclaimedRewards.AddRange(rewards);
            }
            else if (rewards.Count == 1)
            {
                Reward reward = rewards[0];
                CmdConsumeReward.ExecuteStatic(player, reward, true, string.Empty);
                CmdSavePlayerDataToPersistentStorage.ExecuteStatic(player);
                if (reward.MegaBoxes <= 0)
                {
                    if (showCeremony)
                    {
                        RewardCeremonyMenu.InputParameters parameters2 = new RewardCeremonyMenu.InputParameters();
                        parameters2.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.SHOP_PURCHASE.Title, null, false));
                        parameters2.Description = _.L(ConfigUi.CeremonyEntries.SHOP_PURCHASE.Description, null, false);
                        parameters2.SingleRewardOpenAtStart = ConfigUi.CeremonyEntries.SHOP_PURCHASE.ChestOpenAtStart;
                        parameters2.SingleReward = rewards[0];
                        RewardCeremonyMenu.InputParameters parameter = parameters2;
                        PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, true, true);
                    }
                    else
                    {
                        PlayerView.Binder.DungeonHud.resetResourceBar();
                    }
                }
            }
        }
    }
}

