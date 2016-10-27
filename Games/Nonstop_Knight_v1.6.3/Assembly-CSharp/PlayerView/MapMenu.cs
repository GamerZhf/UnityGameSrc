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

    public class MapMenu : Menu
    {
        private List<MapNode> m_mapNodes = new List<MapNode>();
        private Coroutine m_nodeUnlockVisualizationRoutine;
        private float m_scrollRectVertPosNorm;
        public CustomScrollRect ScrollRect;

        private int getLastCompletedMapNodexIndex()
        {
            Player player = GameLogic.Binder.GameState.Player;
            for (int i = 0; i < this.m_mapNodes.Count; i++)
            {
                if (!this.m_mapNodes[i].IsDungeonNode && !player.hasClaimedReward(this.m_mapNodes[i].RewardId))
                {
                    return ((i <= 0) ? 0 : i);
                }
            }
            return 0;
        }

        private float getNormalizedVerticalScrollPositionForMapNode(MapNode mn)
        {
            float height = this.ScrollRect.content.rect.height;
            float num2 = (height * 0.5f) + (mn.RectTm.anchoredPosition.y * 1.18f);
            return Mathf.Clamp01(num2 / height);
        }

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator134 iterator = new <hideRoutine>c__Iterator134();
            iterator.<>f__this = this;
            return iterator;
        }

        private void instantCenterOnLastCompletedNode()
        {
            Player player = GameLogic.Binder.GameState.Player;
            MapNode mn = null;
            for (int i = 0; i < this.m_mapNodes.Count; i++)
            {
                MapNode node2 = this.m_mapNodes[i];
                bool flag = false;
                if (!node2.IsDungeonNode)
                {
                    flag = player.hasClaimedReward(node2.RewardId);
                }
                if ((mn == null) && !flag)
                {
                    mn = node2;
                    break;
                }
            }
            if (mn != null)
            {
                this.setScrollRectVerticalPosNormalized(this.getNormalizedVerticalScrollPositionForMapNode(mn));
            }
        }

        protected void LateUpdate()
        {
            if (this.m_scrollRectVertPosNorm > -1f)
            {
                this.ScrollRect.verticalNormalizedPosition = this.m_scrollRectVertPosNorm;
                this.m_scrollRectVertPosNorm = -1f;
            }
        }

        private bool mapNodeIsPendingForUnlockVisualization(MapNode mn)
        {
            Player player = GameLogic.Binder.GameState.Player;
            int num = this.getLastCompletedMapNodexIndex();
            return ((num != player.LastVisualizedMapNodeUnlockingIndex) && (this.m_mapNodes[num] == mn));
        }

        [DebuggerHidden]
        private IEnumerator nodeUnlockVisualizationRoutine(MapNode mn)
        {
            <nodeUnlockVisualizationRoutine>c__Iterator135 iterator = new <nodeUnlockVisualizationRoutine>c__Iterator135();
            iterator.mn = mn;
            iterator.<$>mn = mn;
            iterator.<>f__this = this;
            return iterator;
        }

        protected override void onAwake()
        {
            Transform transform = TransformExtensions.FindChildRecursively(base.transform, "MapNodes");
            for (int i = 0; i < transform.childCount; i++)
            {
                MapNode component = transform.GetChild(i).GetComponent<MapNode>();
                component.initialize(component.name, string.Empty);
                this.m_mapNodes.Add(component);
            }
        }

        public void onLevelButtonClicked(string dungeonId)
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                if (ConfigUi.MAP_PREDUNGEON_TOOLTIP_ENABLED)
                {
                    PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.PreDungeonMenu, MenuContentType.DungeonTooltip, dungeonId, 0f, false, true);
                }
                else
                {
                    PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.StackedPopupMenu, MenuContentType.DungeonPopupContent, dungeonId, 0f, false, true);
                }
            }
        }

        protected override void onRefresh()
        {
            this.refreshMapNodes();
            Player player = GameLogic.Binder.GameState.Player;
            int num = this.getLastCompletedMapNodexIndex();
            if (num != player.LastVisualizedMapNodeUnlockingIndex)
            {
                UnityUtils.StopCoroutine(this, ref this.m_nodeUnlockVisualizationRoutine);
                this.m_nodeUnlockVisualizationRoutine = UnityUtils.StartCoroutine(this, this.nodeUnlockVisualizationRoutine(this.m_mapNodes[num]));
            }
        }

        public void onRewardButtonClicked(string rewardId)
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.MiniPopupMenu, MenuContentType.MilestoneRewardMiniPopup, rewardId, 0f, false, true);
            }
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator132 iterator = new <preShowRoutine>c__Iterator132();
            iterator.<>f__this = this;
            return iterator;
        }

        private void refreshMapNodes()
        {
            Player player = GameLogic.Binder.GameState.Player;
            for (int i = 0; i < this.m_mapNodes.Count; i++)
            {
                MapNode mn = this.m_mapNodes[i];
                bool unlocked = false;
                if (this.mapNodeIsPendingForUnlockVisualization(mn))
                {
                    unlocked = false;
                }
                else if (mn.IsDungeonNode)
                {
                    if (i == 0)
                    {
                        unlocked = true;
                    }
                    else
                    {
                        MapNode node2 = this.m_mapNodes[i - 1];
                        if (!node2.IsDungeonNode)
                        {
                            unlocked = player.hasClaimedReward(node2.RewardId);
                        }
                    }
                }
                mn.refresh(this, unlocked);
            }
        }

        private void setScrollRectVerticalPosNormalized(float v)
        {
            this.m_scrollRectVertPosNorm = v;
            this.ScrollRect.verticalNormalizedPosition = v;
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator133 iterator = new <showRoutine>c__Iterator133();
            iterator.<>f__this = this;
            return iterator;
        }

        public override PlayerView.MenuType MenuType
        {
            get
            {
                return PlayerView.MenuType.MapMenu;
            }
        }

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator134 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MapMenu <>f__this;

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
                    this.<>f__this.instantCenterOnLastCompletedNode();
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

        [CompilerGenerated]
        private sealed class <nodeUnlockVisualizationRoutine>c__Iterator135 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MapNode <$>mn;
            internal MapMenu <>f__this;
            internal float <easedV>__4;
            internal Player <player>__0;
            internal ManualTimer <scrollTimer>__3;
            internal float <startNormalizedVertPos>__1;
            internal float <targetNormalizedVertPos>__2;
            internal MapNode mn;

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
                        this.<player>__0 = GameLogic.Binder.GameState.Player;
                        if (this.mn == null)
                        {
                            goto Label_0196;
                        }
                        this.<startNormalizedVertPos>__1 = Mathf.Clamp01(this.<>f__this.ScrollRect.verticalNormalizedPosition);
                        this.<targetNormalizedVertPos>__2 = this.<>f__this.getNormalizedVerticalScrollPositionForMapNode(this.mn);
                        this.<scrollTimer>__3 = new ManualTimer(1f);
                        break;

                    case 1:
                        break;

                    case 2:
                        this.mn.OpenEffect.Stop(true);
                        this.mn.OpenEffect.Clear(true);
                        this.<>f__this.onRewardButtonClicked(this.mn.RewardId);
                        goto Label_0196;

                    default:
                        goto Label_01CF;
                }
                if (!this.<scrollTimer>__3.Idle)
                {
                    this.<easedV>__4 = Easing.Apply(this.<scrollTimer>__3.normalizedProgress(), Easing.Function.SMOOTHSTEP);
                    this.<>f__this.setScrollRectVerticalPosNormalized(this.<startNormalizedVertPos>__1 + ((this.<targetNormalizedVertPos>__2 - this.<startNormalizedVertPos>__1) * this.<easedV>__4));
                    this.<scrollTimer>__3.tick(Time.unscaledDeltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01D1;
                }
                this.mn.OpenEffect.Play(true);
                this.mn.refresh(this.<>f__this, true);
                if (!this.mn.IsDungeonNode)
                {
                    this.mn.Button.interactable = false;
                    this.$current = new WaitForSeconds(1.2f);
                    this.$PC = 2;
                    goto Label_01D1;
                }
            Label_0196:
                this.<player>__0.LastVisualizedMapNodeUnlockingIndex = this.<>f__this.m_mapNodes.IndexOf(this.mn);
                this.<>f__this.m_nodeUnlockVisualizationRoutine = null;
                goto Label_01CF;
                this.$PC = -1;
            Label_01CF:
                return false;
            Label_01D1:
                return true;
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
        private sealed class <preShowRoutine>c__Iterator132 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MapMenu <>f__this;

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
                    this.<>f__this.refreshMapNodes();
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

        [CompilerGenerated]
        private sealed class <showRoutine>c__Iterator133 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MapMenu <>f__this;
            internal int <lastCompletedMapNodexIdx>__1;
            internal Player <player>__0;

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
                    this.<player>__0 = GameLogic.Binder.GameState.Player;
                    this.<lastCompletedMapNodexIdx>__1 = this.<>f__this.getLastCompletedMapNodexIndex();
                    if (this.<lastCompletedMapNodexIdx>__1 != this.<player>__0.LastVisualizedMapNodeUnlockingIndex)
                    {
                        UnityUtils.StopCoroutine(this.<>f__this, ref this.<>f__this.m_nodeUnlockVisualizationRoutine);
                        this.<>f__this.m_nodeUnlockVisualizationRoutine = UnityUtils.StartCoroutine(this.<>f__this, this.<>f__this.nodeUnlockVisualizationRoutine(this.<>f__this.m_mapNodes[this.<lastCompletedMapNodexIdx>__1]));
                    }
                    else
                    {
                        this.<>f__this.instantCenterOnLastCompletedNode();
                    }
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

