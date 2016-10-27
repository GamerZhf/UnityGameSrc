namespace PlayerView
{
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class FacebookConnectPopupContent : MenuContent
    {
        public GameObject Button;
        public UnityEngine.UI.Text ButtonText;
        private InputParameters m_inputParams;
        private Coroutine m_simulatedConnectionRoutine;
        public UnityEngine.UI.Text Text;

        [DebuggerHidden]
        private IEnumerator connectAndLoad()
        {
            <connectAndLoad>c__Iterator116 iterator = new <connectAndLoad>c__Iterator116();
            iterator.<>f__this = this;
            return iterator;
        }

        protected override void onAwake()
        {
        }

        public void onButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                if (this.m_inputParams.CompletionCallback != null)
                {
                    this.m_inputParams.CompletionCallback(false);
                }
            }
        }

        protected override void onCleanup()
        {
        }

        private void onConnect(bool success)
        {
            UnityEngine.Debug.Log("Facebook connect:" + success);
            PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            if (this.m_inputParams.CompletionCallback != null)
            {
                this.m_inputParams.CompletionCallback(true);
            }
        }

        private void onConnectFailed()
        {
            this.showFailedMessage();
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.m_inputParams = (InputParameters) param;
            base.m_contentMenu.setCloseButtonVisibility(false);
            this.Button.gameObject.SetActive(false);
            this.ButtonText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.UI_PROMPT_CONTINUE, null, false));
            this.Text.text = _.L(ConfigLoca.UI_STATUS_CONNECTING, null, false);
            this.onRefresh();
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle("FACEBOOK", string.Empty, string.Empty);
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator117 iterator = new <onShow>c__Iterator117();
            iterator.<>f__this = this;
            return iterator;
        }

        private void showFailedMessage()
        {
            this.Text.text = _.L(ConfigLoca.FACEBOOK_CONNECT_FAILED, null, false);
            this.Button.gameObject.SetActive(true);
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.FacebookConnectPopupContent;
            }
        }

        [CompilerGenerated]
        private sealed class <connectAndLoad>c__Iterator116 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal FacebookConnectPopupContent <>f__this;
            internal FacebookAdapter <fb>__0;

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
                        this.<fb>__0 = Service.Binder.FacebookAdapter;
                        this.$current = this.<fb>__0.Login(this.<>f__this.m_inputParams.context);
                        this.$PC = 1;
                        return true;

                    case 1:
                        if (this.<fb>__0.IsLoggedIn())
                        {
                            UnityEngine.Debug.Log("Facebook connect success");
                        }
                        PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                        if (this.<>f__this.m_inputParams.CompletionCallback != null)
                        {
                            this.<>f__this.m_inputParams.CompletionCallback(true);
                        }
                        this.$PC = -1;
                        break;
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
        private sealed class <onShow>c__Iterator117 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal FacebookConnectPopupContent <>f__this;

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
                    Service.Binder.TaskManager.StartTask(this.<>f__this.connectAndLoad(), null);
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

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public Action<bool> CompletionCallback;
            public string context;
        }
    }
}

