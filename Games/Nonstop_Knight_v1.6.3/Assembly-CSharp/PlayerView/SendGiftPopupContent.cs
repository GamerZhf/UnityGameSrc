namespace PlayerView
{
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class SendGiftPopupContent : MenuContent
    {
        public Text FbButtonText;
        public Text FbDescription;
        public GameObject FbRoot;
        public GameObject FriendGiftCellPrototype;
        public Transform FriendsRootTm;
        public Text Header;
        private readonly List<FriendGiftCell> m_friends = new List<FriendGiftCell>(3);
        private Coroutine m_reconstructRoutine;
        private const int MAX_FRIENDS_VISIBLE = 3;

        private void cleanupCells()
        {
            foreach (FriendGiftCell cell in this.m_friends)
            {
                cell.cleanUp();
            }
        }

        protected override void onAwake()
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.FriendGiftCellPrototype);
                obj2.name = this.FriendGiftCellPrototype.name + "-" + i;
                obj2.transform.SetParent(this.FriendsRootTm, false);
                this.m_friends.Add(obj2.GetComponent<FriendGiftCell>());
                obj2.SetActive(false);
            }
            this.FriendGiftCellPrototype.SetActive(false);
            this.Header.text = "SEND GIFTS";
            this.FbDescription.text = "CONNECT TO SEND FREE GIFTS TO YOUR FRIENDS";
            this.FbButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_CONNECT, null, false));
        }

        protected override void onCleanup()
        {
            UnityUtils.StopCoroutine(this, ref this.m_reconstructRoutine);
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

        public void onFacebookConnectClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                FacebookConnectPopupContent.InputParameters parameters2 = new FacebookConnectPopupContent.InputParameters();
                parameters2.context = "gifting";
                parameters2.CompletionCallback = delegate (bool success) {
                    if (success)
                    {
                        this.FbRoot.SetActive(false);
                        this.reconstructContent();
                    }
                };
                FacebookConnectPopupContent.InputParameters parameter = parameters2;
                PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.TechPopupMenu, MenuContentType.FacebookConnectPopupContent, parameter, 0f, false, true);
            }
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.reconstructContent();
        }

        protected override void onRefresh()
        {
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            return new <onShow>c__Iterator17B();
        }

        private void reconstructContent()
        {
            this.cleanupCells();
            if (Service.Binder.FacebookAdapter.RequiresUserConnect())
            {
                this.FbRoot.SetActive(true);
            }
            else
            {
                this.FbRoot.SetActive(false);
                List<FbPlatformUser> source = Enumerable.ToList<FbPlatformUser>(MathUtil.GetRandomSubSet<FbPlatformUser>(Service.Binder.FacebookAdapter.GetFriends(), 3));
                List<LeaderboardImage> lbImages = new List<LeaderboardImage>();
                for (int i = 0; i < Enumerable.Count<FbPlatformUser>(source); i++)
                {
                    this.m_friends[i].initialize((i % 2) == 0, source[i]);
                    this.m_friends[i].gameObject.SetActive(true);
                    lbImages.Add(this.m_friends[i].LeaderboardImage);
                }
                Service.Binder.FacebookAdapter.PopulateImages(lbImages, source);
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.SendGiftPopupContent;
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator17B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;

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
                    PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_Popup, (float) 0f);
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

