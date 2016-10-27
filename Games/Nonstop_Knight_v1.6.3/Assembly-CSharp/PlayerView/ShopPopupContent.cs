namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class ShopPopupContent : MenuContent
    {
        private InputParameters m_params;
        private List<ShopCell> m_shopCells = new List<ShopCell>();
        public UnityEngine.UI.ScrollRect ScrollRect;
        public RectTransform VerticalGroup;

        private void addShopCellToList(ShopEntry shopEntry)
        {
            ShopCell item = PlayerView.Binder.ShopCellPool.getObject();
            item.transform.SetParent(this.VerticalGroup, false);
            bool flag = (this.VerticalGroup.childCount % 2) == 0;
            item.initialize(shopEntry, null, PathToShopType.Vendor, !flag ? ConfigUi.LIST_CELL_REGULAR_COLOR : ConfigUi.LIST_CELL_STRIPED_COLOR, null, null);
            this.m_shopCells.Add(item);
            item.gameObject.SetActive(true);
        }

        protected override void onAwake()
        {
        }

        protected override void onCleanup()
        {
        }

        public override bool onCloseButtonClicked()
        {
            if (this.m_params.CloseCallback != null)
            {
                this.m_params.CloseCallback();
                return true;
            }
            return false;
        }

        protected void OnDisable()
        {
        }

        protected void OnEnable()
        {
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.m_params = (InputParameters) param;
            this.ScrollRect.verticalNormalizedPosition = 1f;
            this.onRefresh();
        }

        protected override void onPreWarm([Optional, DefaultParameterValue(null)] object param)
        {
        }

        protected override void onRefresh()
        {
            GameLogic.Binder.GameState.Player.Notifiers.ShopInspected = true;
            base.m_contentMenu.refreshTitle("GEM SHOP", string.Empty, string.Empty);
            for (int i = 0; i < this.m_shopCells.Count; i++)
            {
                this.m_shopCells[i].refresh(this.m_params.PathToShop, this.m_params.CloseCallback, this.m_params.PurchaseCallback);
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.ShopPopupContent;
            }
        }

        public override MenuContent.TabSpriteParameters TabSprite
        {
            get
            {
                MenuContent.TabSpriteParameters parameters = new MenuContent.TabSpriteParameters();
                parameters.SpriteAtlasId = "DungeonHud";
                parameters.SpriteId = "icon_gem_floater";
                parameters.SpriteSize = new Vector2(100f, 100f);
                return parameters;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public PathToShopType PathToShop;
            public System.Action CloseCallback;
            public Action<ShopEntry, PurchaseResult> PurchaseCallback;
        }
    }
}

