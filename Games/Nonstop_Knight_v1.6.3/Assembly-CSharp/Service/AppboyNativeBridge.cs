namespace Service
{
    using App;
    using Appboy;
    using GameLogic;
    using PlayerView;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AppboyNativeBridge : MonoBehaviour
    {
        private Coroutine initRoutine;
        private List<AppboyIngameMessage> pendingMessages;

        public void AppboyFeedListener(object _data)
        {
            this.Log("------------------------------AppboyFeedListener, received data:");
            if (_data != null)
            {
                this.Log(_data.ToString());
            }
        }

        public void AppboyPushListener(object _data)
        {
            this.Log("------------------------------AppboyPushListener, received data:");
            if (_data != null)
            {
                this.Log(_data.ToString());
            }
        }

        public void AppboyPushOpenedListener(object _data)
        {
            this.Log("------------------------------AppboyPushOpenedListener, received data:");
            if (_data != null)
            {
                this.Log(_data.ToString());
            }
        }

        public void AppboySlideupListener(object _data)
        {
            this.Log("------------------------------AppboySlideupListener, received data:");
            if (_data != null)
            {
                this.Log(_data.ToString());
            }
            if (this.pendingMessages != null)
            {
                AppboyIngameMessage item = this.ParseData(_data as string);
                if (item != null)
                {
                    this.pendingMessages.Add(item);
                    item.LoadResources(this);
                }
            }
        }

        protected void Awake()
        {
            this.pendingMessages = new List<AppboyIngameMessage>();
        }

        private int GetForcedMessageIndex()
        {
            if (!this.HasNoMessage())
            {
                for (int i = 0; i < this.pendingMessages.Count; i++)
                {
                    if (this.IsValidForcedMessage(this.pendingMessages[i]))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public int GetPendingMessageCount()
        {
            int num = 0;
            for (int i = 0; i < this.pendingMessages.Count; i++)
            {
                if (this.IsValidMessage(this.pendingMessages[i]) && !this.IsValidForcedMessage(this.pendingMessages[i]))
                {
                    num++;
                }
            }
            return num;
        }

        public bool HasForcedMessages()
        {
            return (this.GetForcedMessageIndex() > -1);
        }

        private bool HasNoMessage()
        {
            return ((this.pendingMessages == null) || (this.pendingMessages.Count == 0));
        }

        private bool IsValidForcedMessage(AppboyIngameMessage message)
        {
            return (((message.extras != null) && !string.IsNullOrEmpty(message.extras.ForcedDisplay)) && message.ResourcesLoaded);
        }

        private bool IsValidMessage(AppboyIngameMessage message)
        {
            return ((message.extras != null) && message.ResourcesLoaded);
        }

        private void Log(string msg)
        {
            if (Service.Binder.Logger != null)
            {
                Service.Binder.Logger.Log(msg);
            }
        }

        public void OnApplicationPause(bool paused)
        {
            UnityEngine.Debug.Log("RequestInAppMessageRoutine, OnApplicationPause: " + paused);
            if (!paused)
            {
                this.RequestInAppMessageAsync();
            }
        }

        private AppboyIngameMessage ParseData(string _jsonString)
        {
            AppboyIngameMessage message = null;
            try
            {
                message = JsonUtils.Deserialize<AppboyIngameMessage>(_jsonString, true);
                message.JSONData = _jsonString;
            }
            catch (Exception exception)
            {
                this.Log("Could not deserialize received data");
                this.Log(exception.ToString());
            }
            return message;
        }

        public AppboyIngameMessage PopMessage()
        {
            if (!this.HasNoMessage())
            {
                AppboyIngameMessage message;
                int forcedMessageIndex = this.GetForcedMessageIndex();
                if (forcedMessageIndex > -1)
                {
                    message = this.pendingMessages[forcedMessageIndex];
                    this.pendingMessages.RemoveAt(forcedMessageIndex);
                    return message;
                }
                for (int i = 0; i < this.pendingMessages.Count; i++)
                {
                    if (this.IsValidMessage(this.pendingMessages[i]))
                    {
                        message = this.pendingMessages[i];
                        this.pendingMessages.RemoveAt(i);
                        return message;
                    }
                }
            }
            return null;
        }

        public void RequestInAppMessageAsync()
        {
            if (this.initRoutine != null)
            {
                base.StopCoroutine(this.initRoutine);
            }
            this.initRoutine = base.StartCoroutine(this.RequestInAppMessageRoutine());
        }

        [DebuggerHidden]
        private IEnumerator RequestInAppMessageRoutine()
        {
            <RequestInAppMessageRoutine>c__Iterator202 iterator = new <RequestInAppMessageRoutine>c__Iterator202();
            iterator.<>f__this = this;
            return iterator;
        }

        public void ShowNextMessage()
        {
            AppboyIngameMessage message = App.Binder.AppboyIOSBridge.Bridge.PopMessage();
            Texture2D textured = null;
            Texture2D texture = null;
            if (message.ImageLoader != null)
            {
                texture = message.ImageLoader.texture;
            }
            if (message.IconLoader != null)
            {
                textured = message.IconLoader.texture;
            }
            AppboyPopupContent.InputParameters parameter = new AppboyPopupContent.InputParameters();
            parameter.OriginalMessage = message;
            parameter.CampaignId = (message.extras == null) ? null : message.extras.CampaignId;
            parameter.DisposableIconTexture = textured;
            parameter.DisposableBackgroundTexture = texture;
            parameter.Headline = (message.extras == null) ? null : message.extras.MsgHeadline;
            parameter.Title = (message.extras == null) ? null : message.extras.MsgTitle;
            parameter.Message = message.GetMessage();
            int actionButtonCount = message.GetActionButtonCount();
            if (actionButtonCount == 1)
            {
                parameter.SingleButtonText = message.extras.ActBtn1;
                parameter.SingleButtonUrl = message.extras.OpenURL1;
            }
            else if (actionButtonCount > 1)
            {
                parameter.DualButtonLeftText = message.extras.ActBtn1;
                parameter.DualButtonLeftUrl = message.extras.OpenURL1;
                parameter.DualButtonRightText = message.extras.ActBtn2;
                parameter.DualButtonRightUrl = message.extras.OpenURL2;
            }
            PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.ThinPopupMenu, MenuContentType.AppboyPopupContent, parameter, 0f, false, true);
        }

        public void TestMessage()
        {
            this.AppboySlideupListener("{'icon_color':4278190080,'extras':{'MsgImage':'http://rr2-us-vir-1-content.flarecloud.net.s3.amazonaws.com/appboy/00_xpromoFH3/xpromo_v3.jpg','MsgIcon':'http://image005.flaticon.com/1/png/128/31/31181.png','MsgAppearance':'FullImage','ActBtn1':'btn1 text','OpenURL1':':inapp:knight:','MsgHeadline':'Custom Headline','MsgTitle':'Title of the fake Appboy message','MsgBody':'Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren.'},'slide_from':'bottom','message':'Thanks for playing(Message from Appboy that comes from campain settings and not from the custom params)','type':'slideup','message_close':'swipe','close_btn_color':4294967295,'text_color':4294967295,'click_action':'news_feed','icon_bg_color':4294967295,'campaign_id':'fake_test','duration':5000,'hide_chevron':false,'bg_color':4278190080}");
        }

        [CompilerGenerated]
        private sealed class <RequestInAppMessageRoutine>c__Iterator202 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal AppboyNativeBridge <>f__this;

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
                        UnityEngine.Debug.Log("RequestInAppMessageRoutine, waiting for Appboy beeing available");
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_00F4;

                    case 1:
                    case 2:
                        if (AppboyBinding.Appboy == null)
                        {
                            this.$current = null;
                            this.$PC = 2;
                            goto Label_00F4;
                        }
                        break;

                    case 3:
                        break;

                    default:
                        goto Label_00F2;
                }
                while ((GameLogic.Binder.GameState == null) || (GameLogic.Binder.GameState.Player == null))
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_00F4;
                }
                UnityEngine.Debug.Log("appboy: changing user " + GameLogic.Binder.GameState.Player.FgUserHandle);
                AppboyBinding.ChangeUser(GameLogic.Binder.GameState.Player.FgUserHandle);
                UnityEngine.Debug.Log("RequestInAppMessageRoutine, sending request...");
                AppboyBinding.RequestInAppMessage();
                this.<>f__this.initRoutine = null;
                goto Label_00F2;
                this.$PC = -1;
            Label_00F2:
                return false;
            Label_00F4:
                return true;
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

