namespace App
{
    using Service;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class ServerSelectCell : MonoBehaviour
    {
        [CompilerGenerated]
        private string <ServerId>k__BackingField;
        public Image Bg;
        public Text Description;
        public Text Title;
        public Text Title2;

        protected void Awake()
        {
        }

        public void initialize(ServerEntry serverEntry, bool stripedRow)
        {
            this.Bg.color = !stripedRow ? ConfigUi.LIST_CELL_REGULAR_COLOR : ConfigUi.LIST_CELL_STRIPED_COLOR;
            this.Description.text = serverEntry.Url;
            this.ServerId = serverEntry.Id;
            this.Title.text = serverEntry.Name;
            this.Title2.text = serverEntry.Description;
        }

        public void onButtonClicked()
        {
            ServerSelectPopup.Root.selectServer(this.ServerId);
        }

        public string ServerId
        {
            [CompilerGenerated]
            get
            {
                return this.<ServerId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ServerId>k__BackingField = value;
            }
        }
    }
}

