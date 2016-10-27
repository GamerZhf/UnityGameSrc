namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class BannerPopupContent : MenuContent
    {
        public TransformAnimation BgTa;
        public CanvasGroupAlphaFading ContentGroup;
        public Text Description;
        private InputParameters m_inputParams;
        public List<IconWithText> Rewards;
        public GameObject RewardsRoot;
        public Image Shield;
        public Text Title;

        protected override void onAwake()
        {
        }

        protected override void onCleanup()
        {
        }

        [DebuggerHidden]
        protected override IEnumerator onHide()
        {
            <onHide>c__Iterator108 iterator = new <onHide>c__Iterator108();
            iterator.<>f__this = this;
            return iterator;
        }

        public void onOverlayClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.m_inputParams = (InputParameters) param;
            this.Shield.sprite = this.m_inputParams.Shield;
            this.Title.text = this.m_inputParams.Title;
            this.Description.text = this.m_inputParams.Description;
            if (this.m_inputParams.Rewards != null)
            {
                if (this.m_inputParams.Rewards.Count > this.Rewards.Count)
                {
                    UnityEngine.Debug.LogWarning("Cannot visualize all rewards");
                }
                for (int i = 0; i < this.Rewards.Count; i++)
                {
                    IconWithText text = this.Rewards[i];
                    if (i >= this.m_inputParams.Rewards.Count)
                    {
                        text.gameObject.SetActive(false);
                    }
                    else
                    {
                        RewardContent content = this.m_inputParams.Rewards[i];
                        text.Icon.sprite = content.Icon;
                        RewardContent content2 = this.m_inputParams.Rewards[i];
                        text.Text.text = content2.Text;
                        text.gameObject.SetActive(true);
                    }
                }
                this.RewardsRoot.SetActive(true);
            }
            else if (this.m_inputParams.FillRewardsFromAdventureMilestoneFloor > 0)
            {
                PlayerView.Binder.AdventureMilestones.fillCells(GameLogic.Binder.GameState.Player, this.m_inputParams.FillRewardsFromAdventureMilestoneFloor, this.m_inputParams.FillRewardsFromAdventureMilestoneFloor, this.Rewards, false, false);
                this.RewardsRoot.SetActive(true);
            }
            else
            {
                this.RewardsRoot.SetActive(false);
            }
            this.BgTa.Tm.localScale = new Vector3(1f, 0f, 1f);
            this.ContentGroup.setTransparent(true);
        }

        protected override void onRefresh()
        {
        }

        [DebuggerHidden]
        protected override IEnumerator onShow([Optional, DefaultParameterValue(null)] object param)
        {
            <onShow>c__Iterator107 iterator = new <onShow>c__Iterator107();
            iterator.<>f__this = this;
            return iterator;
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.BannerPopupContent;
            }
        }

        public override bool ForceShowGameplayLayerBehind
        {
            get
            {
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <onHide>c__Iterator108 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal BannerPopupContent <>f__this;
            internal TransformAnimation <ta>__0;
            internal TransformAnimationTask <tt>__1;

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
                        this.<>f__this.ContentGroup.animateToTransparent(0.15f, 0f);
                        this.<ta>__0 = this.<>f__this.BgTa;
                        this.<tt>__1 = new TransformAnimationTask(this.<ta>__0.Tm, 0.2f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__1.scale(new Vector3(1f, 0f, 1f), true, Easing.Function.IN_BACK);
                        this.<ta>__0.addTask(this.<tt>__1);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_011A;
                }
                if (this.<>f__this.ContentGroup.IsAnimating || this.<>f__this.BgTa.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                if (this.<>f__this.m_inputParams.HideCallback != null)
                {
                    this.<>f__this.m_inputParams.HideCallback();
                    goto Label_011A;
                    this.$PC = -1;
                }
            Label_011A:
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
        private sealed class <onShow>c__Iterator107 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal BannerPopupContent <>f__this;
            internal TransformAnimation <ta>__0;
            internal TransformAnimationTask <tt>__1;

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
                        this.<ta>__0 = this.<>f__this.BgTa;
                        this.<tt>__1 = new TransformAnimationTask(this.<ta>__0.Tm, 0.35f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__1.scale(Vector3.one, true, Easing.Function.OUT_BOUNCE);
                        this.<ta>__0.addTask(this.<tt>__1);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00DC;
                }
                if (this.<ta>__0.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 1;
                }
                else
                {
                    this.$current = this.<>f__this.ContentGroup.animateToBlack(0.3f, 0f);
                    this.$PC = 2;
                }
                return true;
                this.$PC = -1;
            Label_00DC:
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
            public Sprite Shield;
            public string Title;
            public string Description;
            public List<BannerPopupContent.RewardContent> Rewards;
            public int FillRewardsFromAdventureMilestoneFloor;
            public System.Action HideCallback;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RewardContent
        {
            public Sprite Icon;
            public string Text;
        }
    }
}

