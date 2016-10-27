namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class ShopCell : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private Color <BgColor>k__BackingField;
        [CompilerGenerated]
        private System.Action <CloseCallback>k__BackingField;
        [CompilerGenerated]
        private PathToShopType <PathToShop>k__BackingField;
        [CompilerGenerated]
        private Action<GameLogic.ShopEntry, PurchaseResult> <PurchaseCallback>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        [CompilerGenerated]
        private GameLogic.ShopEntry <ShopEntry>k__BackingField;
        [CompilerGenerated]
        private GameLogic.ShopEntryInstance <ShopEntryInstance>k__BackingField;
        public CanvasGroup AlphaGroup;
        public ParticleSystem BlingEffect;
        public UnityEngine.UI.Button Button;
        public Image ButtonImage;
        public Image CellBackground;
        public PlayerView.CellButton CellButton;
        public Text Description;
        public Image Icon;
        public Text IconLabel;
        private List<Vector3> m_originalStarLocalPositions = new List<Vector3>();
        public List<Image> Stars = new List<Image>();
        public Text Title;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            for (int i = 0; i < this.Stars.Count; i++)
            {
                this.m_originalStarLocalPositions.Add(this.Stars[i].transform.localPosition);
            }
        }

        public void cleanUpForReuse()
        {
            this.BlingEffect.Stop();
        }

        public void initialize(GameLogic.ShopEntry shopEntry, GameLogic.ShopEntryInstance shopEntryInstance, PathToShopType pathToShop, Color bgColor, [Optional, DefaultParameterValue(null)] System.Action closeCallback, [Optional, DefaultParameterValue(null)] Action<GameLogic.ShopEntry, PurchaseResult> purchaseCallback)
        {
        }

        public void onButtonClicked()
        {
        }

        public void refresh(PathToShopType pathToShop, [Optional, DefaultParameterValue(null)] System.Action closeCallback, [Optional, DefaultParameterValue(null)] Action<GameLogic.ShopEntry, PurchaseResult> purchaseCallback)
        {
        }

        public Color BgColor
        {
            [CompilerGenerated]
            get
            {
                return this.<BgColor>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<BgColor>k__BackingField = value;
            }
        }

        public System.Action CloseCallback
        {
            [CompilerGenerated]
            get
            {
                return this.<CloseCallback>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CloseCallback>k__BackingField = value;
            }
        }

        public PathToShopType PathToShop
        {
            [CompilerGenerated]
            get
            {
                return this.<PathToShop>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PathToShop>k__BackingField = value;
            }
        }

        public Action<GameLogic.ShopEntry, PurchaseResult> PurchaseCallback
        {
            [CompilerGenerated]
            get
            {
                return this.<PurchaseCallback>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PurchaseCallback>k__BackingField = value;
            }
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

        public GameLogic.ShopEntry ShopEntry
        {
            [CompilerGenerated]
            get
            {
                return this.<ShopEntry>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ShopEntry>k__BackingField = value;
            }
        }

        public GameLogic.ShopEntryInstance ShopEntryInstance
        {
            [CompilerGenerated]
            get
            {
                return this.<ShopEntryInstance>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ShopEntryInstance>k__BackingField = value;
            }
        }
    }
}

