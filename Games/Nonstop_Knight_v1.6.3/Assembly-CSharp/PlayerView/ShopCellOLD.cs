namespace PlayerView
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class ShopCellOLD : MonoBehaviour
    {
        public UnityEngine.UI.Button Button;
        public Image ButtonImage;
        public Text CornerText;
        public Image CostIcon;
        public Text CostText;
        public Image Icon;
        public string ShopBundleId;

        protected void Awake()
        {
            this.refresh();
        }

        public void refresh()
        {
        }
    }
}

