namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class ChestGalleryContent : MenuContent
    {
        public const int BASIC_TAB_INDEX = 0;
        public const int EVENT_TAB_INDEX = 2;
        private List<ChestGalleryRow> m_chestGalleryRows = new List<ChestGalleryRow>(ConfigMeta.BOSS_CHESTS.Count);
        public UnityEngine.UI.ScrollRect ScrollRect;
        public const int SPECIAL_TAB_INDEX = 1;
        public RectTransform VerticalGroup;

        private void cleanupChestGalleryRows()
        {
            for (int i = 0; i < this.m_chestGalleryRows.Count; i++)
            {
                PlayerView.Binder.ChestGalleryRowPool.returnObject(this.m_chestGalleryRows[i]);
            }
            this.m_chestGalleryRows.Clear();
        }

        private ChestType[] getActiveChestTypes(int index)
        {
            switch (index)
            {
                case 0:
                    return ConfigMeta.BOSS_CHESTS_BASIC;

                case 1:
                    return ConfigMeta.BOSS_CHESTS_SPECIAL;

                case 2:
                    return ConfigMeta.BOSS_CHESTS_EVENT;
            }
            return null;
        }

        public override string getTitleForTab(int index)
        {
            switch (index)
            {
                case 0:
                    return StringExtensions.ToUpperLoca(_.L(ConfigLoca.CHEST_GALLERY_BASIC_TITLE, null, false));

                case 1:
                    return StringExtensions.ToUpperLoca(_.L(ConfigLoca.CHEST_GALLERY_SPECIAL_TITLE, null, false));

                case 2:
                    return ((GameLogic.Binder.GameState.Player.getNumEventChestTypesEnabled() <= 0) ? null : StringExtensions.ToUpperLoca(_.L(ConfigLoca.CHEST_GALLERY_EVENT_TITLE, null, false)));
            }
            return null;
        }

        protected override void onAwake()
        {
        }

        protected override void onCleanup()
        {
            this.cleanupChestGalleryRows();
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_CODEX_CHESTS_BUTTON_TEXT, null, false)), string.Empty, string.Empty);
            this.reconstructContent();
            base.refresh();
        }

        protected override void onRefresh()
        {
            for (int i = 0; i < this.m_chestGalleryRows.Count; i++)
            {
                this.m_chestGalleryRows[i].refresh();
            }
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator114 iterator = new <onShow>c__Iterator114();
            iterator.<>f__this = this;
            return iterator;
        }

        public override void onTabButtonClicked(int index)
        {
            this.reconstructContent();
            base.refresh();
        }

        private void reconstructContent()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.ScrollRect.verticalNormalizedPosition = 1f;
            this.cleanupChestGalleryRows();
            ChestType[] typeArray = this.getActiveChestTypes(((StackedPopupMenu) base.m_contentMenu).Smcc.getActiveTabIndex());
            if (typeArray != null)
            {
                for (int i = 0; i < typeArray.Length; i++)
                {
                    ChestType chestType = typeArray[i];
                    if (player.isChestEnabled(chestType))
                    {
                        ChestGalleryRow item = PlayerView.Binder.ChestGalleryRowPool.getObject();
                        item.transform.SetParent(this.VerticalGroup, false);
                        item.gameObject.SetActive(true);
                        item.initialize(chestType);
                        this.m_chestGalleryRows.Add(item);
                    }
                }
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.ChestGalleryContent;
            }
        }

        public override bool UsesTabs
        {
            get
            {
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator114 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ChestGalleryContent <>f__this;

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
                    this.<>f__this.ScrollRect.verticalNormalizedPosition = 1f;
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

