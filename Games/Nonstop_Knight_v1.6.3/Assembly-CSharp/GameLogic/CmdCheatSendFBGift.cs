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

    [ConsoleCommand("sendfbgift")]
    public class CmdCheatSendFBGift : ICommand
    {
        private readonly SocialInboxCommand m_inboxCmd;

        public CmdCheatSendFBGift(SocialInboxCommand command)
        {
            this.m_inboxCmd = command;
        }

        public CmdCheatSendFBGift(string[] serialized)
        {
            this.m_inboxCmd = null;
            foreach (KeyValuePair<string, FbPlatformUser> pair in Service.Binder.FacebookAdapter.Friends)
            {
                UnityEngine.Debug.Log(string.Format("Friend id: {0} username: {1} imageUrl: {2}", pair.Key, pair.Value.userName, pair.Value.ImageUrl));
            }
            string targetPlayerFBid = !(serialized[0] != "D") ? GameLogic.Binder.GameState.Player.SocialData.FacebookId : serialized[0];
            string id = !(serialized[1] != "D") ? "PetBoxSmall" : serialized[1];
            if (ConfigShops.GetShopEntry(id) == null)
            {
                UnityEngine.Debug.LogError("Invalid shop entry id");
            }
            else
            {
                string facebookId = GameLogic.Binder.GameState.Player.SocialData.FacebookId;
                if (facebookId == null)
                {
                    UnityEngine.Debug.LogError("Cannot send facebook gift when not logged into Facebook");
                }
                else
                {
                    this.m_inboxCmd = SocialGift.CreateFacebookGiftCommand(facebookId, targetPlayerFBid, "PetBoxSmall");
                }
            }
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator40 iterator = new <executeRoutine>c__Iterator40();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(SocialInboxCommand inboxCmd)
        {
            Service.Binder.PlayerService.SendSocialInboxCommand(inboxCmd);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator40 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdCheatSendFBGift <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if ((this.$PC == 0) && (this.<>f__this.m_inboxCmd != null))
                {
                    CmdCheatSendFBGift.ExecuteStatic(this.<>f__this.m_inboxCmd);
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

