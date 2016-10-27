namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class HeroStatCell : MonoBehaviour
    {
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public Image Bg;
        public Text ThreeColumns1;
        public Text ThreeColumns2;
        public Text ThreeColumns3;
        public GameObject ThreeColumnsRoot;
        public Text TwoColumns1;
        public Text TwoColumns2;
        public GameObject TwoColumnsRoot;

        public void setEmpty(bool striped)
        {
            this.TwoColumnsRoot.SetActive(false);
            this.ThreeColumnsRoot.SetActive(false);
            this.Bg.enabled = striped;
        }

        public void setThreeColumnLayout(string column1, string column2, string column3, bool striped)
        {
            this.TwoColumnsRoot.SetActive(false);
            this.ThreeColumnsRoot.SetActive(true);
            this.ThreeColumns1.text = column1;
            this.ThreeColumns2.text = column2;
            this.ThreeColumns3.text = column3;
            this.Bg.enabled = striped;
        }

        public void setTwoColumnLayout(string column1, string column2, bool striped)
        {
            this.TwoColumnsRoot.SetActive(true);
            this.ThreeColumnsRoot.SetActive(false);
            this.TwoColumns1.text = column1;
            this.TwoColumns2.text = column2;
            this.Bg.enabled = striped;
        }

        public RectTransform RectTm
        {
            [CompilerGenerated]
            get
            {
                return this.<RectTm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RectTm>k__BackingField = value;
            }
        }
    }
}

