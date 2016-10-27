namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class OptionsContent : MenuContent
    {
        public Text BuildInfoVersion;
        public GameObject CombatStatsDividerRoot;
        public GameObject CombatStatsRoot;
        public Text CombatStatsTitle;
        public Toggle CombatStatsToggle;
        public Text CombatStatsToggleText;
        public Text CommunityTitle;
        public Text FacebookButtonText;
        public GameObject FacebookConnectButtonOffRoot;
        public GameObject FacebookConnectButtonOnRoot;
        public Text FacebookConnectButtonText;
        public Text FacebookConnectTitle;
        public Text FacebookDescription;
        public Text ForumsButtonText;
        public Text ForumsDescription;
        public Image GooglePlayAchievementsButtonIcon;
        public Text GooglePlayAchievementsButtonText;
        public GameObject GooglePlayConnectButtonOffRoot;
        public GameObject GooglePlayConnectButtonOnRoot;
        public Text GooglePlayConnectButtonText;
        public Text GooglePlayConnectTitle;
        public Image GooglePlayLeaderboardsButtonIcon;
        public Text GooglePlayLeaderboardsButtonText;
        public GameObject GooglePlayRoot;
        public GameObject GooglePlaySeparator;
        public GameObject GooglePlaySettingsRoot;
        public Text MusicTitle;
        public Toggle MusicToggle;
        public Text MusicToggleText;
        public Text PrivacyButtonText;
        public GameObject PrivacyRoot;
        public GameObject PrivacySeparator;
        public Text PrivacyText;
        public Text SettingsTitle;
        public Text SnapchatButtonText;
        public Text SnapchatDescription;
        public Text SoundTitle;
        public Toggle SoundToggle;
        public Text SoundToggleText;
        public Text SupportButtonText;
        public Text SupportTitle;
        public Text TwitterButtonText;
        public Text TwitterDescription;
        public Text UserHandle;

        protected override void onAwake()
        {
            this.CommunityTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.COMMUNITY_DIVIDER_TITLE, null, false));
            this.ForumsButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.COMMUNITY_FORUMS, null, false));
            this.FacebookButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.COMMUNITY_FACEBOOK, null, false));
            this.TwitterButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.COMMUNITY_TWITTER, null, false));
            this.SnapchatButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.COMMUNITY_SNAPCHAT, null, false));
            this.ForumsDescription.text = _.L(ConfigLoca.COMMUNITY_FORUMS_DESCRIPTION, null, false);
            this.FacebookDescription.text = _.L(ConfigLoca.COMMUNITY_FACEBOOK_DESCRIPTION, null, false);
            this.TwitterDescription.text = _.L(ConfigLoca.COMMUNITY_TWITTER_DESCRIPTION, null, false);
            this.SnapchatDescription.text = _.L(ConfigLoca.COMMUNITY_SNAPCHAT_DESCRIPTION, null, false);
            this.SettingsTitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_SETTINGS_BUTTON_TEXT, null, false));
            this.MusicTitle.text = _.L(ConfigLoca.OPTIONS_MUSIC, null, false);
            this.SoundTitle.text = _.L(ConfigLoca.OPTIONS_SFX, null, false);
            this.SupportTitle.text = _.L(ConfigLoca.OPTIONS_SUPPORT, null, false);
            this.SupportButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.OPTIONS_SUPPORT_BUTTON_TEXT, null, false));
            this.FacebookConnectTitle.text = _.L(ConfigLoca.FACEBOOK_TITLE, null, false);
            this.GooglePlayConnectTitle.text = _.L(ConfigLoca.GOOGLE_PLAY_TITLE, null, false);
            this.PrivacyText.text = _.L(ConfigLoca.OPTIONS_PRIVACY, null, false);
            this.PrivacyButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.OPTIONS_PRIVACY_BUTTON_TEXT, null, false));
            this.CombatStatsTitle.text = _.L(ConfigLoca.OPTIONS_COMBAT_STATS, null, false);
            this.GooglePlayAchievementsButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.GOOGLE_PLAY_ACHIEVEMENTS_TITLE, null, false));
            this.GooglePlayLeaderboardsButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.GOOGLE_PLAY_LEADERBOARD_TITLE, null, false));
            this.GooglePlayRoot.SetActive(true);
            this.GooglePlaySeparator.SetActive(true);
            this.GooglePlaySettingsRoot.SetActive(true);
            this.PrivacyRoot.SetActive(true);
            this.PrivacySeparator.SetActive(true);
        }

        protected override void onCleanup()
        {
        }

        public void onCombatStatToggleClicked(bool state)
        {
            Player player = GameLogic.Binder.GameState.Player;
            player.Preferences.CombatStatsEnabled = state;
            base.refresh();
            PlayerView.Binder.DungeonHud.CombatStats.gameObject.SetActive(player.Preferences.CombatStatsEnabled);
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
            App.Binder.EventBus.OnPlatformConnectStateChanged -= new App.Events.PlatformConnectStateChanged(this.onPlatformConnectStateChanged);
        }

        protected void OnEnable()
        {
            App.Binder.EventBus.OnPlatformConnectStateChanged += new App.Events.PlatformConnectStateChanged(this.onPlatformConnectStateChanged);
        }

        public void onFacebookButtonClicked()
        {
            Application.OpenURL(App.Binder.ConfigMeta.FACEBOOK_URL);
        }

        public void onFacebookConnectButtonClicked()
        {
            if (!Service.Binder.FacebookAdapter.RequiresUserConnect())
            {
                Service.Binder.FacebookAdapter.Logout("options");
                Service.Binder.TaskManager.StartTask(this.refreshDelayed(0.3f), null);
            }
            else if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                FacebookConnectPopupContent.InputParameters parameters2 = new FacebookConnectPopupContent.InputParameters();
                parameters2.context = "options";
                parameters2.CompletionCallback = delegate (bool success) {
                    if (success)
                    {
                        GameLogic.Binder.LeaderboardSystem.initialize();
                        base.refresh();
                    }
                };
                FacebookConnectPopupContent.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TechPopupMenu, MenuContentType.FacebookConnectPopupContent, parameter, 0f, false, true);
            }
        }

        public void onForumsButtonClicked()
        {
            Application.OpenURL(App.Binder.ConfigMeta.FORUMS_URL);
        }

        public void onGooglePlayAchievementsButtonClicked()
        {
            App.Binder.SocialSystem.ShowAchievements();
        }

        public void onGooglePlayConnectButtonClicked()
        {
            if (App.Binder.SocialSystem.IsConnected())
            {
                App.Binder.SocialSystem.Disconnect();
            }
            else
            {
                App.Binder.SocialSystem.Connect();
            }
        }

        public void onGooglePlayLeaderboardsButtonClicked()
        {
            App.Binder.SocialSystem.ShowLeaderboards();
        }

        public void onMusicToggleClicked(bool state)
        {
            GameLogic.Binder.GameState.Player.MusicEnabled = state;
            base.refresh();
        }

        private void onPlatformConnectStateChanged(PlatformConnectType _connectType)
        {
            base.refresh();
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.BuildInfoVersion.text = App.Binder.BuildResources.getBuildInfoDescription();
            if (!ConfigApp.ProductionBuild)
            {
                this.BuildInfoVersion.text = this.BuildInfoVersion.text + " " + App.Binder.LocaSystem.GetLocaKey();
            }
            Player player = GameLogic.Binder.GameState.Player;
            this.MusicToggle.isOn = player.MusicEnabled;
            this.SoundToggle.isOn = player.SoundEnabled;
            this.CombatStatsRoot.SetActive(App.Binder.ConfigMeta.COMBAT_STATS_ENABLED);
            this.CombatStatsDividerRoot.SetActive(this.CombatStatsRoot.activeSelf);
            this.CombatStatsToggle.isOn = player.Preferences.CombatStatsEnabled;
            if (string.IsNullOrEmpty(player.ServerStats.UserHandle))
            {
                this.UserHandle.text = "-";
            }
            else
            {
                this.UserHandle.text = player.ServerStats.UserHandle;
            }
        }

        public void onPrivacyButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Application.OpenURL(App.Binder.ConfigMeta.PRIVACY_URL);
            }
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_TITLE_MORE, null, false)), string.Empty, string.Empty);
            this.MusicToggleText.text = !this.MusicToggle.isOn ? StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_BUTTON_TOGGLE_OFF, null, false)) : StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_BUTTON_TOGGLE_ON, null, false));
            this.SoundToggleText.text = !this.SoundToggle.isOn ? StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_BUTTON_TOGGLE_OFF, null, false)) : StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_BUTTON_TOGGLE_ON, null, false));
            this.CombatStatsToggleText.text = !this.CombatStatsToggle.isOn ? StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_BUTTON_TOGGLE_OFF, null, false)) : StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_BUTTON_TOGGLE_ON, null, false));
            bool flag = !Service.Binder.FacebookAdapter.RequiresUserConnect();
            this.FacebookConnectButtonOnRoot.SetActive(flag);
            this.FacebookConnectButtonOffRoot.SetActive(!flag);
            this.FacebookConnectButtonText.text = !flag ? StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_CONNECT, null, false)) : StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_STATUS_CONNECTED, null, false));
            bool flag2 = App.Binder.SocialSystem.IsConnected();
            this.GooglePlaySettingsRoot.SetActive(flag2);
            this.GooglePlayLeaderboardsButtonIcon.gameObject.SetActive(false);
            this.GooglePlayAchievementsButtonIcon.gameObject.SetActive(false);
            this.GooglePlayConnectButtonOnRoot.SetActive(flag2);
            this.GooglePlayConnectButtonOffRoot.SetActive(!flag2);
            this.GooglePlayLeaderboardsButtonIcon.gameObject.SetActive(true);
            this.GooglePlayAchievementsButtonIcon.gameObject.SetActive(true);
            this.GooglePlayConnectButtonText.text = !flag2 ? _.L(ConfigLoca.UI_PROMPT_CONNECT, null, false).ToUpper() : _.L(ConfigLoca.UI_STATUS_CONNECTED, null, false).ToUpper();
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator158 iterator = new <onShow>c__Iterator158();
            iterator.<>f__this = this;
            return iterator;
        }

        public void onSnapchatButtonClicked()
        {
            Application.OpenURL(App.Binder.ConfigMeta.SNAPCHAT_URL);
        }

        public void onSoundToggleClicked(bool state)
        {
            GameLogic.Binder.GameState.Player.SoundEnabled = state;
            base.refresh();
        }

        public void onSupportButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Application.OpenURL(App.Binder.ConfigMeta.SUPPORT_URL);
            }
        }

        public void onTwitterButtonClicked()
        {
            Application.OpenURL(App.Binder.ConfigMeta.TWITTER_URL);
        }

        [DebuggerHidden]
        private IEnumerator refreshDelayed(float seconds)
        {
            <refreshDelayed>c__Iterator157 iterator = new <refreshDelayed>c__Iterator157();
            iterator.seconds = seconds;
            iterator.<$>seconds = seconds;
            iterator.<>f__this = this;
            return iterator;
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.OptionsContent;
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator158 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal OptionsContent <>f__this;

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
                    this.<>f__this.refresh();
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
        private sealed class <refreshDelayed>c__Iterator157 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal float <$>seconds;
            internal OptionsContent <>f__this;
            internal IEnumerator <ie>__0;
            internal float seconds;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(this.seconds);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0076;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    return true;
                }
                this.<>f__this.refresh();
                this.$PC = -1;
            Label_0076:
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

