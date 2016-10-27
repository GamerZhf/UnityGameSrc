namespace PlayerView
{
    using App;
    using GameLogic;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class TournamentMilestoneTooltip : MenuContent
    {
        [CompilerGenerated]
        private Service.TournamentView <TournamentView>k__BackingField;
        public RectTransform ArrowRectTm;
        public GameObject CellPrototype;
        private Vector3 m_arrowDefaultLocalPos;
        private Coroutine m_arrowRefreshRoutine;
        private List<TournamentMilestoneRewardCell> m_cells;
        public UnityEngine.UI.ScrollRect ScrollRect;
        public RectTransform VerticalGroupTm;

        private TournamentMilestoneRewardCell addCell()
        {
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.CellPrototype);
            obj2.transform.SetParent(this.VerticalGroupTm, false);
            TournamentMilestoneRewardCell component = obj2.GetComponent<TournamentMilestoneRewardCell>();
            this.m_cells.Add(component);
            obj2.SetActive(false);
            return component;
        }

        [DebuggerHidden]
        private IEnumerator arrowRefreshRoutine()
        {
            <arrowRefreshRoutine>c__Iterator1A5 iteratora = new <arrowRefreshRoutine>c__Iterator1A5();
            iteratora.<>f__this = this;
            return iteratora;
        }

        private void cleanupCells()
        {
            for (int i = 0; i < this.m_cells.Count; i++)
            {
                this.m_cells[i].gameObject.SetActive(false);
            }
        }

        protected override void onAwake()
        {
            this.m_arrowDefaultLocalPos = this.ArrowRectTm.localPosition;
            this.m_cells = new List<TournamentMilestoneRewardCell>(5);
            for (int i = 0; i < this.m_cells.Capacity; i++)
            {
                this.addCell();
            }
            this.CellPrototype.SetActive(false);
        }

        protected override void onCleanup()
        {
            UnityUtils.StopCoroutine(this, ref this.m_arrowRefreshRoutine);
            this.cleanupCells();
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.TournamentView = (Service.TournamentView) param;
            this.reconstructContent();
            this.m_arrowRefreshRoutine = UnityUtils.StartCoroutine(this, this.arrowRefreshRoutine());
        }

        protected override void onRefresh()
        {
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator1A4 iteratora = new <onShow>c__Iterator1A4();
            iteratora.<>f__this = this;
            return iteratora;
        }

        private void reconstructContent()
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.cleanupCells();
            long totalContribution = this.TournamentView.GetTotalContribution();
            for (int i = 0; i < this.TournamentView.TournamentInfo.RewardMilestones.Count; i++)
            {
                while (i >= this.m_cells.Count)
                {
                    this.addCell();
                }
                TournamentMilestoneRewardCell cell = this.m_cells[i];
                RewardMilestone milestone = this.TournamentView.TournamentInfo.RewardMilestones[i];
                cell.Description.text = "Beat " + milestone.Threshold + " bosses";
                cell.RewardIcon.sprite = PlayerView.Binder.SpriteResources.getSprite(ConfigUi.GetFloaterSpriteForShopEntry(milestone.MainReward.Id));
                ShopEntry shopEntry = ConfigShops.GetShopEntry(milestone.MainReward.Id);
                if (shopEntry.Type == ShopEntryType.TokenBundle)
                {
                    double v = ConfigShops.CalculateTokenBundleSize(player, shopEntry.Id);
                    cell.Amount.text = MenuHelpers.BigValueToString(v);
                    RectTransformExtensions.SetSize(cell.RewardIcon.rectTransform, (Vector2) (Vector2.one * 70f));
                }
                else
                {
                    cell.Amount.text = milestone.MainReward.Amount + "x";
                    RectTransformExtensions.SetSize(cell.RewardIcon.rectTransform, (Vector2) (Vector2.one * 80f));
                }
                cell.Bg.color = ((i % 2) != 0) ? ConfigUi.LIST_CELL_REGULAR_COLOR : new Color(0f, 0f, 0f, 0f);
                if ((i > 0) && (totalContribution < milestone.Threshold))
                {
                    cell.CanvasGroup.alpha = 0.33f;
                }
                else
                {
                    cell.CanvasGroup.alpha = 1f;
                }
                cell.gameObject.SetActive(true);
            }
            for (int j = this.TournamentView.TournamentInfo.RewardMilestones.Count; j < this.m_cells.Count; j++)
            {
                this.m_cells[j].gameObject.SetActive(false);
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.TournamentMilestoneTooltip;
            }
        }

        public Service.TournamentView TournamentView
        {
            [CompilerGenerated]
            get
            {
                return this.<TournamentView>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<TournamentView>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <arrowRefreshRoutine>c__Iterator1A5 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TournamentMilestoneTooltip <>f__this;
            internal TooltipMenu <tooltipMenu>__0;

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
                        this.<tooltipMenu>__0 = (TooltipMenu) this.<>f__this.m_contentMenu;
                        break;

                    case 1:
                        break;
                        this.$PC = -1;
                        goto Label_0077;

                    default:
                        goto Label_0077;
                }
                this.<tooltipMenu>__0.refreshArrowPosition(this.<>f__this.ArrowRectTm, this.<>f__this.m_arrowDefaultLocalPos);
                this.$current = null;
                this.$PC = 1;
                return true;
            Label_0077:
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
        private sealed class <onShow>c__Iterator1A4 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TournamentMilestoneTooltip <>f__this;

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
                    UnityUtils.StopCoroutine(this.<>f__this, ref this.<>f__this.m_arrowRefreshRoutine);
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

