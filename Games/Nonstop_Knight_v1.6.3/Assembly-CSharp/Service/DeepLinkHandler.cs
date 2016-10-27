namespace Service
{
    using PlayerView;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class DeepLinkHandler
    {
        [CompilerGenerated]
        private static Action<bool> <>f__am$cache1;
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map8;
        public const string DEEPLINK_FACEBOOK_CONNECT = ":facebook:connect:";
        public const string DEEPLINK_URL_ASCEND = ":inapp:ascend:";
        public const string DEEPLINK_URL_INVENTORY = ":inapp:inventory:";
        public const string DEEPLINK_URL_INVENTORY_ARMORS = ":inapp:inventory:armors:";
        public const string DEEPLINK_URL_INVENTORY_CLOAKS = ":inapp:inventory:cloaks:";
        public const string DEEPLINK_URL_INVENTORY_CONSUMABLE = ":inapp:inventory:consumable:";
        public const string DEEPLINK_URL_INVENTORY_WEAPONS = ":inapp:inventory:weapons:";
        public const string DEEPLINK_URL_KNIGHT = ":inapp:knight:";
        public const string DEEPLINK_URL_KNIGHT_ACHIEVEMENTS = ":inapp:knight:achievements:";
        public const string DEEPLINK_URL_KNIGHT_BOUNTIES = ":inapp:knight:bounties:";
        public const string DEEPLINK_URL_KNIGHT_CHESTS = ":inapp:knight:chests:";
        public const string DEEPLINK_URL_KNIGHT_COMMUNITY = ":inapp:knight:community:";
        public const string DEEPLINK_URL_KNIGHT_LEADERBOARD_FRIENDS = ":inapp:knight:leaderboard:";
        public const string DEEPLINK_URL_KNIGHT_LEADERBOARD_LOCAL = ":inapp:knight:leaderboard:local:";
        public const string DEEPLINK_URL_KNIGHT_UPGRADES = ":inapp:knight:upgrades:";
        public const string DEEPLINK_URL_PETS = ":inapp:pets:";
        public const string DEEPLINK_URL_SHOP = ":inapp:shop:";
        public const string DEEPLINK_URL_SHOP_DRAGON = ":inapp:shop:dragon:";
        public const string DEEPLINK_URL_SHOP_GEMS = ":inapp:shop:gems:";
        public const string DEEPLINK_URL_SHOP_UPGRADES = ":inapp:shop:upgrades:";
        public const string DEEPLINK_URL_SHOP_VENDOR = ":inapp:shop:vendor:";
        public const string DEEPLINK_URL_SKILLS = ":inapp:skills:";
        public const string DEEPLINK_URL_SLIDING_SIDEMENU = ":inapp:slidingside:";

        public static void ExecuteDeepLink(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
            else
            {
                string key = url;
                if (key != null)
                {
                    int num;
                    if (<>f__switch$map8 == null)
                    {
                        Dictionary<string, int> dictionary = new Dictionary<string, int>(0x17);
                        dictionary.Add(":inapp:shop:", 0);
                        dictionary.Add(":inapp:shop:vendor:", 1);
                        dictionary.Add(":inapp:shop:upgrades:", 2);
                        dictionary.Add(":inapp:shop:gems:", 3);
                        dictionary.Add(":inapp:ascend:", 4);
                        dictionary.Add(":inapp:inventory:", 5);
                        dictionary.Add(":inapp:inventory:weapons:", 6);
                        dictionary.Add(":inapp:inventory:armors:", 7);
                        dictionary.Add(":inapp:inventory:cloaks:", 8);
                        dictionary.Add(":inapp:inventory:consumable:", 9);
                        dictionary.Add(":inapp:shop:dragon:", 10);
                        dictionary.Add(":inapp:knight:", 11);
                        dictionary.Add(":inapp:knight:leaderboard:", 12);
                        dictionary.Add(":inapp:knight:leaderboard:local:", 13);
                        dictionary.Add(":inapp:knight:chests:", 14);
                        dictionary.Add(":inapp:knight:achievements:", 15);
                        dictionary.Add(":inapp:knight:upgrades:", 0x10);
                        dictionary.Add(":inapp:knight:community:", 0x11);
                        dictionary.Add(":inapp:skills:", 0x12);
                        dictionary.Add(":inapp:slidingside:", 0x13);
                        dictionary.Add(":inapp:pets:", 20);
                        dictionary.Add(":inapp:knight:bounties:", 0x15);
                        dictionary.Add(":facebook:connect:", 0x16);
                        <>f__switch$map8 = dictionary;
                    }
                    if (<>f__switch$map8.TryGetValue(key, out num))
                    {
                        VendorPopupContent.InputParameters parameters10;
                        SlidingInventoryMenu.InputParameters parameters11;
                        switch (num)
                        {
                            case 0:
                                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.StackedPopupMenu, MenuContentType.VendorPopupContent, null, 0f, true, true);
                                return;

                            case 1:
                            {
                                VendorPopupContent component = PlayerView.Binder.MenuContentResources.getSharedResource(MenuContentType.VendorPopupContent).GetComponent<VendorPopupContent>();
                                parameters10 = new VendorPopupContent.InputParameters();
                                parameters10.CenterOnRectTm = component.VendorGridTm;
                                VendorPopupContent.InputParameters parameter = parameters10;
                                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.StackedPopupMenu, MenuContentType.VendorPopupContent, parameter, 0f, true, true);
                                return;
                            }
                            case 2:
                            {
                                VendorPopupContent content2 = PlayerView.Binder.MenuContentResources.getSharedResource(MenuContentType.VendorPopupContent).GetComponent<VendorPopupContent>();
                                parameters10 = new VendorPopupContent.InputParameters();
                                parameters10.CenterOnRectTm = content2.AugmentationGridTm;
                                VendorPopupContent.InputParameters parameters2 = parameters10;
                                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.StackedPopupMenu, MenuContentType.VendorPopupContent, parameters2, 0f, true, true);
                                return;
                            }
                            case 3:
                            {
                                VendorPopupContent content3 = PlayerView.Binder.MenuContentResources.getSharedResource(MenuContentType.VendorPopupContent).GetComponent<VendorPopupContent>();
                                parameters10 = new VendorPopupContent.InputParameters();
                                parameters10.CenterOnRectTm = content3.GemGridTm;
                                VendorPopupContent.InputParameters parameters3 = parameters10;
                                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.StackedPopupMenu, MenuContentType.VendorPopupContent, parameters3, 0f, true, true);
                                return;
                            }
                            case 4:
                                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.AscendPopupContent, null, 0f, true, true);
                                return;

                            case 5:
                                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.SlidingInventoryMenu, MenuContentType.NONE, null, 0f, true, true);
                                return;

                            case 6:
                            {
                                parameters11 = new SlidingInventoryMenu.InputParameters();
                                parameters11.OverrideOpenTabIndex = 0;
                                SlidingInventoryMenu.InputParameters parameters4 = parameters11;
                                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.SlidingInventoryMenu, MenuContentType.NONE, parameters4, 0f, true, true);
                                return;
                            }
                            case 7:
                            {
                                parameters11 = new SlidingInventoryMenu.InputParameters();
                                parameters11.OverrideOpenTabIndex = 1;
                                SlidingInventoryMenu.InputParameters parameters5 = parameters11;
                                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.SlidingInventoryMenu, MenuContentType.NONE, parameters5, 0f, true, true);
                                return;
                            }
                            case 8:
                            {
                                parameters11 = new SlidingInventoryMenu.InputParameters();
                                parameters11.OverrideOpenTabIndex = 2;
                                SlidingInventoryMenu.InputParameters parameters6 = parameters11;
                                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.SlidingInventoryMenu, MenuContentType.NONE, parameters6, 0f, true, true);
                                return;
                            }
                            case 9:
                            {
                                parameters11 = new SlidingInventoryMenu.InputParameters();
                                parameters11.OverrideOpenTabIndex = 3;
                                SlidingInventoryMenu.InputParameters parameters7 = parameters11;
                                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.SlidingInventoryMenu, MenuContentType.NONE, parameters7, 0f, true, true);
                                return;
                            }
                            case 10:
                                if (Service.Binder.ShopManager.StarterBundleAvailable() && Service.Binder.ShopManager.StarterBundleVisible())
                                {
                                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.StarterBundlePopupContent, null, 0f, false, true);
                                }
                                return;

                            case 11:
                                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.StackedPopupMenu, MenuContentType.HeroPopupContent, null);
                                return;

                            case 12:
                                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.StackedPopupMenu, MenuContentType.LeaderboardPopupContent, null);
                                return;

                            case 13:
                            {
                                LeaderboardPopupContent.InputParams params2 = new LeaderboardPopupContent.InputParams();
                                params2.OverrideTab = 1;
                                LeaderboardPopupContent.InputParams @params = params2;
                                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.StackedPopupMenu, MenuContentType.LeaderboardPopupContent, @params);
                                return;
                            }
                            case 14:
                                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.StackedPopupMenu, MenuContentType.ChestGalleryContent, null);
                                return;

                            case 15:
                                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.StackedPopupMenu, MenuContentType.AchievementPopupContent, null);
                                return;

                            case 0x10:
                            {
                                SlidingAdventurePanel.InputParameters parameters12 = new SlidingAdventurePanel.InputParameters();
                                parameters12.OverrideOpenTabIndex = 0;
                                parameters12.OverrideOpenAdventureSubTabIndex = 1;
                                SlidingAdventurePanel.InputParameters parameters8 = parameters12;
                                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.SlidingAdventurePanel, MenuContentType.NONE, parameters8);
                                return;
                            }
                            case 0x11:
                                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.StackedPopupMenu, MenuContentType.OptionsContent, null);
                                return;

                            case 0x12:
                                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.StackedPopupMenu, MenuContentType.SkillPopupContent, null);
                                return;

                            case 0x13:
                                if (!PlayerView.Binder.MenuSystem.InTransition)
                                {
                                    PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.SlidingTaskPanel, MenuContentType.NONE, null, 0f, false, true);
                                    return;
                                }
                                return;

                            case 20:
                                if (!PlayerView.Binder.MenuSystem.InTransition)
                                {
                                    PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.StackedPopupMenu, MenuContentType.PetPopupContent, null);
                                    return;
                                }
                                return;

                            case 0x15:
                                if (!PlayerView.Binder.MenuSystem.InTransition)
                                {
                                    PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.StackedPopupMenu, MenuContentType.MissionsPopupContent, null);
                                    return;
                                }
                                return;

                            case 0x16:
                                if (!PlayerView.Binder.MenuSystem.InTransition)
                                {
                                    FacebookConnectPopupContent.InputParameters parameters13 = new FacebookConnectPopupContent.InputParameters();
                                    parameters13.context = "promo";
                                    if (<>f__am$cache1 == null)
                                    {
                                        <>f__am$cache1 = delegate (bool success) {
                                            if (success)
                                            {
                                            }
                                        };
                                    }
                                    parameters13.CompletionCallback = <>f__am$cache1;
                                    FacebookConnectPopupContent.InputParameters parameters9 = parameters13;
                                    PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(MenuType.TechPopupMenu, MenuContentType.FacebookConnectPopupContent, parameters9);
                                    return;
                                }
                                return;
                        }
                    }
                }
                Application.OpenURL(url);
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }
    }
}

