namespace PlayerView
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class TechPopupContent : MenuContent
    {
        public GameObject Button;
        public Text ButtonTextComp;
        public Text Message;

        protected override void onAwake()
        {
        }

        public void onButtonClicked()
        {
            if (!Binder.MenuSystem.InTransition)
            {
                Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        protected override void onCleanup()
        {
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            InputParameters parameters = (InputParameters) param;
            this.Message.text = parameters.Message;
            if (!string.IsNullOrEmpty(parameters.Title))
            {
                base.m_contentMenu.refreshTitle(parameters.Title, string.Empty, string.Empty);
            }
            if (!string.IsNullOrEmpty(parameters.ButtonText))
            {
                this.ButtonTextComp.text = parameters.ButtonText;
            }
        }

        protected override void onRefresh()
        {
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.TechPopupContent;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public string Title;
            public string Message;
            public string ButtonText;
        }
    }
}

