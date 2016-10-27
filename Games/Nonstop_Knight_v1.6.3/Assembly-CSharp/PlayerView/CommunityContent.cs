namespace PlayerView
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class CommunityContent : MenuContent
    {
        public Text DividerTitle1;
        public Text FacebookButtonText;
        public Text FacebookDescription;
        public Text ForumsButtonText;
        public Text ForumsDescription;
        public Text SnapchatButtonText;
        public Text SnapchatDescription;
        public Text TwitterButtonText;
        public Text TwitterDescription;

        protected override void onAwake()
        {
            this.DividerTitle1.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.COMMUNITY_DIVIDER_TITLE, null, false));
            this.ForumsButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.COMMUNITY_FORUMS, null, false));
            this.FacebookButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.COMMUNITY_FACEBOOK, null, false));
            this.TwitterButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.COMMUNITY_TWITTER, null, false));
            this.SnapchatButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.COMMUNITY_SNAPCHAT, null, false));
            this.ForumsDescription.text = _.L(ConfigLoca.COMMUNITY_FORUMS_DESCRIPTION, null, false);
            this.FacebookDescription.text = _.L(ConfigLoca.COMMUNITY_FACEBOOK_DESCRIPTION, null, false);
            this.TwitterDescription.text = _.L(ConfigLoca.COMMUNITY_TWITTER_DESCRIPTION, null, false);
            this.SnapchatDescription.text = _.L(ConfigLoca.COMMUNITY_SNAPCHAT_DESCRIPTION, null, false);
        }

        protected override void onCleanup()
        {
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

        public void onFacebookButtonClicked()
        {
            Application.OpenURL(App.Binder.ConfigMeta.FACEBOOK_URL);
        }

        public void onForumsButtonClicked()
        {
            Application.OpenURL(App.Binder.ConfigMeta.FORUMS_URL);
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_COMMUNITY_BUTTON_TEXT, null, false)), string.Empty, string.Empty);
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator115 iterator = new <onShow>c__Iterator115();
            iterator.<>f__this = this;
            return iterator;
        }

        public void onSnapchatButtonClicked()
        {
            Application.OpenURL(App.Binder.ConfigMeta.SNAPCHAT_URL);
        }

        public void onTwitterButtonClicked()
        {
            Application.OpenURL(App.Binder.ConfigMeta.TWITTER_URL);
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.CommunityContent;
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator115 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CommunityContent <>f__this;

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
    }
}

