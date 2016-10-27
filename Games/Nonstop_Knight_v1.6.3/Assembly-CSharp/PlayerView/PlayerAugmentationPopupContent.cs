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

    public class PlayerAugmentationPopupContent : MenuContent
    {
        public GameObject AugmentationCellPrototype;
        public GameObject EmptyNote;
        public Text EmptyNoteText;
        private List<PlayerAugmentationCell> m_augCells = new List<PlayerAugmentationCell>();
        public UnityEngine.UI.ScrollRect ScrollRect;
        public Text Subtitle;
        public RectTransform VerticalGroup;

        protected override void onAwake()
        {
            this.EmptyNoteText.text = _.L(ConfigLoca.AUGMENTATIONS_BUY_FROM_SHOP, null, false);
            this.Subtitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.VENDOR_TITLE_AUGMENTATIONS, null, false));
            List<PerkType> list = GameLogic.Binder.PlayerAugmentationResources.getUsedPerkTypes();
            this.m_augCells = new List<PlayerAugmentationCell>(list.Count);
            for (int i = 0; i < this.m_augCells.Capacity; i++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.AugmentationCellPrototype);
                obj2.name = this.AugmentationCellPrototype.name + "-" + i;
                obj2.transform.SetParent(this.VerticalGroup);
                this.m_augCells.Add(obj2.GetComponent<PlayerAugmentationCell>());
                obj2.SetActive(false);
            }
            this.AugmentationCellPrototype.SetActive(false);
        }

        protected override void onCleanup()
        {
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnPlayerAugmentationGained -= new GameLogic.Events.PlayerAugmentationGained(this.onPlayerAugmentationGained);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnPlayerAugmentationGained += new GameLogic.Events.PlayerAugmentationGained(this.onPlayerAugmentationGained);
        }

        private void onPlayerAugmentationGained(Player player, string id)
        {
            this.reconstructContent();
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.reconstructContent();
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_AUGMENTATIONS_BUTTON_TEXT, null, false)), string.Empty, string.Empty);
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator15D iteratord = new <onShow>c__Iterator15D();
            iteratord.<>f__this = this;
            return iteratord;
        }

        private void reconstructContent()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (player.Tournaments.hasTournamentSelected())
            {
                this.EmptyNoteText.text = "Knight upgrades are\ndisabled during boss hunt!";
            }
            else
            {
                this.EmptyNoteText.text = _.L(ConfigLoca.AUGMENTATIONS_BUY_FROM_SHOP, null, false);
            }
            List<PerkType> list = GameLogic.Binder.PlayerAugmentationResources.getUsedPerkTypes();
            int num = 0;
            PlayerAugmentationCell cell = null;
            for (int i = 0; i < this.m_augCells.Count; i++)
            {
                PlayerAugmentationCell cell2 = this.m_augCells[i];
                PerkType perkType = list[i];
                ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[perkType];
                List<KeyValuePair<PerkInstance, BuffSource>> perkInstancesOfType = CharacterStatModifierUtil.GetPerkInstancesOfType(player.Augmentations, perkType);
                float m = 0f;
                for (int j = 0; j < perkInstancesOfType.Count; j++)
                {
                    KeyValuePair<PerkInstance, BuffSource> pair = perkInstancesOfType[j];
                    m += pair.Key.Modifier;
                }
                if (m == 0f)
                {
                    cell2.gameObject.SetActive(false);
                }
                else
                {
                    string str;
                    cell2.gameObject.SetActive(true);
                    num++;
                    cell = cell2;
                    if (perkType == PerkType.CoinBonusStart)
                    {
                        str = MenuHelpers.BigValueToString((double) m);
                    }
                    else
                    {
                        str = MenuHelpers.BigModifierToString(m, true);
                    }
                    cell2.Title.text = StringExtensions.ToUpperLoca(_.L(data.ShortDescription, null, false));
                    cell2.Text.text = str;
                    cell2.Icon.sprite = PlayerView.Binder.SpriteResources.getSprite(data.Sprite);
                    cell2.CornerText.text = "x" + perkInstancesOfType.Count.ToString();
                    cell2.Divider.SetActive(true);
                }
            }
            if (cell != null)
            {
                cell.Divider.SetActive(false);
            }
            this.EmptyNote.SetActive(num == 0);
            this.onRefresh();
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.PlayerAugmentationPopupContent;
            }
        }

        public override string TabTitle
        {
            get
            {
                return StringExtensions.ToUpperLoca(_.L(ConfigLoca.HEROVIEW_AUGMENTATIONS_BUTTON_TEXT, null, false));
            }
        }

        [CompilerGenerated]
        private sealed class <onShow>c__Iterator15D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal PlayerAugmentationPopupContent <>f__this;

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

