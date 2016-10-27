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

    public class RewardCeremonyMenu : Menu
    {
        [CompilerGenerated]
        private List<MenuTreasureChest> <Chests>k__BackingField;
        [CompilerGenerated]
        private InputParameters <Params>k__BackingField;
        public MenuOverlay BackgroundOverlay;
        public Image Banderoll;
        public CanvasGroupAlphaFading BanderollTextCanvasGroup;
        public const float CHEST_ANCHORED_OFFSET_X = 0f;
        public Text ChestsRemainingText;
        public CanvasGroupAlphaFading DescriptionCanvasGroup;
        public List<Text> Descriptions;
        public const float EXTRA_WAIT_TIME = 1.5f;
        private bool m_clickFlag;
        private bool m_finalizationCompletedSuccessfully;
        private Coroutine m_finalizationRoutine;
        private bool m_finalizationTriggered;
        private OrderedDict<Reward, bool> m_queuedRewards = new OrderedDict<Reward, bool>();
        private bool m_resourceFlyToHudAllowed;
        private Dictionary<GameObject, TransformAnimation> m_transformAnimations;
        public const int MAX_FLY_TO_HUD_PER_CATEGORY = 4;
        public const int MAX_NUM_REWARDS_STACKS = 1;
        public Text NewBountiesDescription;
        public GameObject NewBountiesRoot;
        public Text NextLeaderboardTargetDescription;
        public LeaderboardImage NextLeaderboardTargetImage;
        public GameObject NextLeaderboardTargetRoot;
        public Image Ray1;
        public Image Ray2;
        private static List<Reward> sm_tempRewards = new List<Reward>(4);
        public Button TapAnywhereButton;
        public Text Title;
        public RectTransform TreasureChestGridTm;

        private MenuTreasureChest addMenuTreasureChestToGrid(Reward reward, bool openAtStart, float anchoredOffsetX, [Optional, DefaultParameterValue(null)] LeaderboardEntry lbe)
        {
            MenuTreasureChest item = PlayerView.Binder.MenuTreasureChestPool.getObject();
            item.RectTm.SetParent(this.TreasureChestGridTm, false);
            item.RectTm.anchoredPosition = new Vector2(anchoredOffsetX, item.RectTm.anchoredPosition.y);
            item.initialize(this, base.Canvas, new Action<MenuTreasureChest>(this.onChestClicked), reward, openAtStart, this.Params.CeremonyEntry, lbe, this.Params.ConsumeRewardsAfterChestOpen);
            this.Chests.Add(item);
            item.gameObject.SetActive(false);
            return item;
        }

        private bool allChestsClaimed()
        {
            if (this.m_queuedRewards.Count > 0)
            {
                return false;
            }
            for (int i = 0; i < this.Chests.Count; i++)
            {
                if (!this.Chests[i].RewardClaimed)
                {
                    return false;
                }
            }
            return true;
        }

        private void animatedObjectScaling(GameObject go, Vector3 targetLocalScale, float duration)
        {
            go.SetActive(true);
            TransformAnimation animation = this.m_transformAnimations[go];
            TransformAnimationTask animationTask = new TransformAnimationTask(animation.transform, duration, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
            animationTask.scale(targetLocalScale, true, ConfigUi.MENU_EASING_OUT);
            animation.addTask(animationTask);
        }

        private void cleanupCells()
        {
            for (int i = this.Chests.Count - 1; i >= 0; i--)
            {
                this.removeMenuTreasureChest(this.Chests[i]);
            }
        }

        [ContextMenu("editorTest")]
        private void editorTest()
        {
            base.GetComponent<Canvas>().worldCamera = PlayerView.Binder.MenuSystem.MenuCamera;
            base.GraphicRaycaster.enabled = true;
            Reward singleReward = new Reward();
            singleReward.ChestType = ChestType.TournamentCards;
            singleReward.TournamentUpgradeReward = new TournamentUpgradeReward();
            TournamentUpgradeReward.Entry item = new TournamentUpgradeReward.Entry();
            item.TourmanentUpgradeId = "Tug001";
            singleReward.TournamentUpgradeReward.Choices.Add(item);
            item = new TournamentUpgradeReward.Entry();
            item.TourmanentUpgradeId = "Tug002";
            singleReward.TournamentUpgradeReward.Choices.Add(item);
            item = new TournamentUpgradeReward.Entry();
            item.TourmanentUpgradeId = "Tug003";
            item.IsEpicUpgrade = true;
            singleReward.TournamentUpgradeReward.Choices.Add(item);
            base.StartCoroutine(this.editorTestRoutine(singleReward, null));
        }

        [DebuggerHidden]
        private IEnumerator editorTestRoutine(Reward singleReward, Dictionary<Reward, bool> multiRewards)
        {
            <editorTestRoutine>c__Iterator16D iteratord = new <editorTestRoutine>c__Iterator16D();
            iteratord.singleReward = singleReward;
            iteratord.multiRewards = multiRewards;
            iteratord.<$>singleReward = singleReward;
            iteratord.<$>multiRewards = multiRewards;
            iteratord.<>f__this = this;
            return iteratord;
        }

        [DebuggerHidden]
        private IEnumerator finalizationRoutine(MenuTreasureChest lastChest)
        {
            <finalizationRoutine>c__Iterator16C iteratorc = new <finalizationRoutine>c__Iterator16C();
            iteratorc.lastChest = lastChest;
            iteratorc.<$>lastChest = lastChest;
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator16A iteratora = new <hideRoutine>c__Iterator16A();
            iteratora.instant = instant;
            iteratora.<$>instant = instant;
            iteratora.<>f__this = this;
            return iteratora;
        }

        private bool isMultiRewardCeremony()
        {
            return (this.Params.MultiRewards != null);
        }

        private bool isSingleRewardMultiPartChest()
        {
            return (((this.Params.SingleReward != null) && this.Params.SingleReward.isWrappedInsideChest()) && ConfigUi.CHEST_BLUEPRINTS[this.Params.SingleReward.getVisualChestType()].isMultiPart());
        }

        private bool isSingleRewardSinglePartChest()
        {
            return (((this.Params.SingleReward != null) && this.Params.SingleReward.isWrappedInsideChest()) && !ConfigUi.CHEST_BLUEPRINTS[this.Params.SingleReward.getVisualChestType()].isMultiPart());
        }

        protected override void onAwake()
        {
            this.m_transformAnimations = new Dictionary<GameObject, TransformAnimation>();
            List<GameObject> list2 = new List<GameObject>();
            list2.Add(this.Banderoll.gameObject);
            list2.Add(this.Ray1.gameObject);
            list2.Add(this.Ray2.gameObject);
            list2.Add(this.ChestsRemainingText.gameObject);
            list2.Add(this.NextLeaderboardTargetRoot);
            list2.Add(this.NewBountiesRoot);
            List<GameObject> list = list2;
            for (int i = 0; i < this.Descriptions.Count; i++)
            {
                list.Add(this.Descriptions[i].gameObject);
            }
            for (int j = 0; j < list.Count; j++)
            {
                GameObject self = list[j];
                TransformAnimation animation = GameObjectExtensions.AddOrGetComponent<TransformAnimation>(self);
                if (!this.m_transformAnimations.ContainsKey(self))
                {
                    this.m_transformAnimations.Add(self, animation);
                }
            }
            foreach (KeyValuePair<GameObject, TransformAnimation> pair in this.m_transformAnimations)
            {
                pair.Value.transform.localScale = Vector3.zero;
            }
            this.Chests = new List<MenuTreasureChest>(8);
        }

        public void onChestClicked(MenuTreasureChest treasureChest)
        {
            this.m_clickFlag = true;
            if (!PlayerView.Binder.MenuSystem.InTransition && !this.m_finalizationTriggered)
            {
                base.StartCoroutine(this.postChestOpenRoutine(treasureChest));
            }
        }

        protected void OnDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            if (eventBus != null)
            {
                eventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            }
        }

        protected void OnEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            if (eventBus != null)
            {
                eventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            }
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            if (PlayerView.Binder.MenuSystem.menuTypeInActiveStack(this.MenuType))
            {
                this.m_resourceFlyToHudAllowed = false;
            }
        }

        public void onTapAnywhereButtonClicked()
        {
            this.m_clickFlag = true;
            if (this.Chests != null)
            {
                for (int i = 0; i < this.Chests.Count; i++)
                {
                    if (!this.Chests[i].usesCardChoiceFlow())
                    {
                        this.Chests[i].onClick();
                    }
                }
            }
        }

        [DebuggerHidden]
        private IEnumerator postChestOpenRoutine(MenuTreasureChest chest)
        {
            <postChestOpenRoutine>c__Iterator16B iteratorb = new <postChestOpenRoutine>c__Iterator16B();
            iteratorb.chest = chest;
            iteratorb.<$>chest = chest;
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator168 iterator = new <preShowRoutine>c__Iterator168();
            iterator.parameter = parameter;
            iterator.<$>parameter = parameter;
            iterator.<>f__this = this;
            return iterator;
        }

        private void refreshChestsRemainingText()
        {
            if (this.isMultiRewardCeremony() && (this.Params.MultiRewards.Count > 1))
            {
                this.ChestsRemainingText.text = _.L(ConfigLoca.CEREMONY_REWARDS_REMAINING, new <>__AnonType4<string>(MenuHelpers.ColoredText(this.m_queuedRewards.Count)), false);
                this.ChestsRemainingText.enabled = true;
            }
            else
            {
                this.ChestsRemainingText.enabled = false;
            }
        }

        private void removeMenuTreasureChest(MenuTreasureChest mtc)
        {
            for (int i = this.Chests.Count - 1; i >= 0; i--)
            {
                if (this.Chests[i] == mtc)
                {
                    this.Chests.Remove(mtc);
                    PlayerView.Binder.MenuTreasureChestPool.returnObject(mtc);
                }
            }
        }

        private bool rewardContentStaysOnScreenUntilClicked()
        {
            if ((this.Params.SingleReward != null) && (((this.Params.SingleReward.getVisualChestType() == ChestType.RewardBoxMulti) || (this.Params.SingleReward.getVisualChestType() == ChestType.LootBoxBossHunt)) || (this.Params.SingleReward.getVisualChestType() == ChestType.PetBoxSmall)))
            {
                return true;
            }
            if (this.Params.MultiRewards != null)
            {
                foreach (KeyValuePair<Reward, bool> pair in this.Params.MultiRewards)
                {
                    if ((pair.Key.getVisualChestType() == ChestType.RewardBoxMulti) || (pair.Key.getVisualChestType() == ChestType.PetBoxSmall))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator169 iterator = new <showRoutine>c__Iterator169();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void StartCardPackCeremony(string localizedTitle, string localizedDescription, Reward reward, [Optional, DefaultParameterValue(false)] bool closeAllMenusBefore)
        {
            InputParameters parameters2 = new InputParameters();
            parameters2.Title = localizedTitle;
            parameters2.Description = localizedDescription;
            parameters2.Description6 = _.L(ConfigLoca.BH_CARD_CEREMONY_FOOTNOTE, null, false);
            parameters2.SingleReward = reward;
            parameters2.SingleRewardOpenAtStart = true;
            parameters2.ConsumeRewardsAfterChestOpen = true;
            parameters2.HideBackground = true;
            InputParameters parameter = parameters2;
            if (closeAllMenusBefore)
            {
                PlayerView.Binder.MenuSystem.closeAllMenusAndTransitionToNewMenu(PlayerView.MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter);
            }
            else
            {
                PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.RewardCeremonyMenu, MenuContentType.NONE, parameter, 0f, false, true);
            }
        }

        public List<MenuTreasureChest> Chests
        {
            [CompilerGenerated]
            get
            {
                return this.<Chests>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Chests>k__BackingField = value;
            }
        }

        public override bool IsOverlayMenu
        {
            get
            {
                return true;
            }
        }

        public override PlayerView.MenuType MenuType
        {
            get
            {
                return PlayerView.MenuType.RewardCeremonyMenu;
            }
        }

        public InputParameters Params
        {
            [CompilerGenerated]
            get
            {
                return this.<Params>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Params>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <editorTestRoutine>c__Iterator16D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Dictionary<Reward, bool> <$>multiRewards;
            internal Reward <$>singleReward;
            internal RewardCeremonyMenu <>f__this;
            internal IEnumerator <ie>__1;
            internal RewardCeremonyMenu.InputParameters <ip>__0;
            internal Dictionary<Reward, bool> multiRewards;
            internal Reward singleReward;

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
                    {
                        RewardCeremonyMenu.InputParameters parameters = new RewardCeremonyMenu.InputParameters();
                        parameters.Title = "VICTORY!";
                        parameters.Description = "Increase your group's power!";
                        parameters.Description5 = "Pick one";
                        parameters.SingleReward = this.singleReward;
                        parameters.MultiRewards = this.multiRewards;
                        parameters.DisableFlyToHud = true;
                        parameters.HideBackground = true;
                        this.<ip>__0 = parameters;
                        this.<ie>__1 = this.<>f__this.preShowRoutine(MenuContentType.NONE, this.<ip>__0);
                        break;
                    }
                    case 1:
                        break;

                    case 2:
                        goto Label_0110;

                    default:
                        goto Label_012C;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    goto Label_012E;
                }
                this.<ie>__1 = this.<>f__this.showRoutine(MenuContentType.NONE, this.<ip>__0);
            Label_0110:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 2;
                    goto Label_012E;
                }
                goto Label_012C;
                this.$PC = -1;
            Label_012C:
                return false;
            Label_012E:
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
        private sealed class <finalizationRoutine>c__Iterator16C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuTreasureChest <$>lastChest;
            internal RewardCeremonyMenu <>f__this;
            internal bool <extraWait>__0;
            internal int <i>__2;
            internal IEnumerator <ie>__1;
            internal IEnumerator <ie>__3;
            internal IEnumerator <ie>__4;
            internal MenuTreasureChest lastChest;

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
                        this.<>f__this.m_clickFlag = false;
                        if (!this.<>f__this.rewardContentStaysOnScreenUntilClicked())
                        {
                            goto Label_0089;
                        }
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00F5;

                    case 3:
                        this.<>f__this.BackgroundOverlay.fadeToTransparent(0.3f, Easing.Function.LINEAR);
                        this.<>f__this.DescriptionCanvasGroup.animateToTransparent(0.3f, 0f);
                        this.<>f__this.animatedObjectScaling(this.<>f__this.Ray1.gameObject, Vector3.zero, 0.3f);
                        this.<>f__this.animatedObjectScaling(this.<>f__this.Ray2.gameObject, Vector3.zero, 0.3f);
                        this.<>f__this.animatedObjectScaling(this.<>f__this.Banderoll.gameObject, Vector3.zero, 0.3f);
                        if (this.<>f__this.Params.DisableFlyToHud)
                        {
                            goto Label_02E7;
                        }
                        RewardCeremonyMenu.sm_tempRewards.Clear();
                        this.<i>__2 = 0;
                        while (this.<i>__2 < this.<>f__this.Chests.Count)
                        {
                            RewardCeremonyMenu.sm_tempRewards.Add(this.<>f__this.Chests[this.<i>__2].Reward);
                            this.<i>__2++;
                        }
                        this.<ie>__3 = PlayerView.Binder.DungeonHud.flyToHud(RewardCeremonyMenu.sm_tempRewards, RectTransformUtility.WorldToScreenPoint(this.<>f__this.Canvas.worldCamera, this.<>f__this.TreasureChestGridTm.position), true, this.<>f__this.m_resourceFlyToHudAllowed || this.<>f__this.Params.ConsumeRewardsAfterChestOpen);
                        goto Label_02D7;

                    case 4:
                        goto Label_02D7;

                    case 5:
                        goto Label_0344;

                    case 6:
                        goto Label_036C;

                    default:
                        goto Label_03B1;
                }
                if (!this.<>f__this.m_clickFlag && !this.lastChest.FinalizationRoutineSkippedByPlayer)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_03B3;
                }
            Label_0089:
                this.<extraWait>__0 = this.<>f__this.isMultiRewardCeremony() || !this.<>f__this.Params.SingleRewardOpenAtStart;
                if (!this.<extraWait>__0)
                {
                    goto Label_0125;
                }
                this.<ie>__1 = TimeUtil.WaitForUnscaledSeconds(1.5f);
            Label_00F5:
                while ((this.<ie>__1.MoveNext() && !this.<>f__this.m_clickFlag) && !this.lastChest.FinalizationRoutineSkippedByPlayer)
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 2;
                    goto Label_03B3;
                }
            Label_0125:
                this.$current = null;
                this.$PC = 3;
                goto Label_03B3;
            Label_02D7:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 4;
                    goto Label_03B3;
                }
            Label_02E7:
                if ((!this.<extraWait>__0 || this.<>f__this.m_clickFlag) || this.lastChest.FinalizationRoutineSkippedByPlayer)
                {
                    goto Label_036C;
                }
                this.<ie>__4 = TimeUtil.WaitForUnscaledSeconds(1.5f);
            Label_0344:
                while (this.<ie>__4.MoveNext())
                {
                    this.$current = this.<ie>__4.Current;
                    this.$PC = 5;
                    goto Label_03B3;
                }
            Label_036C:
                while (this.<>f__this.BackgroundOverlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 6;
                    goto Label_03B3;
                }
                this.<>f__this.m_finalizationRoutine = null;
                this.<>f__this.m_finalizationCompletedSuccessfully = true;
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                goto Label_03B1;
                this.$PC = -1;
            Label_03B1:
                return false;
            Label_03B3:
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
        private sealed class <hideRoutine>c__Iterator16A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal Dictionary<GameObject, TransformAnimation>.Enumerator <$s_429>__0;
            internal RewardCeremonyMenu <>f__this;
            internal KeyValuePair<GameObject, TransformAnimation> <kv>__1;
            internal TransformAnimation <ta>__2;
            internal TransformAnimationTask <tt>__3;
            internal bool instant;

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
                        UnityUtils.StopCoroutine(this.<>f__this, ref this.<>f__this.m_finalizationRoutine);
                        if (this.instant)
                        {
                            this.<>f__this.BackgroundOverlay.fadeToTransparent(0f, Easing.Function.LINEAR);
                            goto Label_0171;
                        }
                        if (!this.<>f__this.BackgroundOverlay.IsAnimating)
                        {
                            this.<>f__this.BackgroundOverlay.fadeToTransparent(ConfigUi.POPUP_TRANSITION_DURATION_OUT, Easing.Function.LINEAR);
                        }
                        this.<$s_429>__0 = this.<>f__this.m_transformAnimations.GetEnumerator();
                        try
                        {
                            while (this.<$s_429>__0.MoveNext())
                            {
                                this.<kv>__1 = this.<$s_429>__0.Current;
                                this.<ta>__2 = this.<kv>__1.Value;
                                if (!this.<ta>__2.HasTasks)
                                {
                                    this.<tt>__3 = new TransformAnimationTask(this.<ta>__2.transform, ConfigUi.POPUP_TRANSITION_DURATION_OUT, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                                    this.<tt>__3.scale(Vector3.zero, true, ConfigUi.MENU_EASING_IN);
                                    this.<ta>__2.addTask(this.<tt>__3);
                                }
                            }
                        }
                        finally
                        {
                            this.<$s_429>__0.Dispose();
                        }
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_01E2;
                }
                if (this.<>f__this.BackgroundOverlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
            Label_0171:
                this.<>f__this.cleanupCells();
                this.<>f__this.m_queuedRewards.clear();
                if (!this.<>f__this.m_finalizationCompletedSuccessfully)
                {
                    PlayerView.Binder.DungeonHud.resetResourceBar();
                }
                if (this.<>f__this.Params.HideCallback != null)
                {
                    this.<>f__this.Params.HideCallback();
                    goto Label_01E2;
                    this.$PC = -1;
                }
            Label_01E2:
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
        private sealed class <postChestOpenRoutine>c__Iterator16B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuTreasureChest <$>chest;
            internal RewardCeremonyMenu <>f__this;
            internal float <displayRewardContentForSeconds>__0;
            internal MenuTreasureChest <newChest>__3;
            internal bool <openAtStart>__2;
            internal Reward <reward>__1;
            internal TransformAnimation <ta>__4;
            internal TransformAnimationTask <tt>__5;
            internal MenuTreasureChest chest;

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
                        if (this.<>f__this.allChestsClaimed() && !this.<>f__this.m_finalizationTriggered)
                        {
                            this.<>f__this.m_finalizationRoutine = UnityUtils.StartCoroutine(this.<>f__this, this.<>f__this.finalizationRoutine(this.chest));
                            this.<>f__this.m_finalizationTriggered = true;
                        }
                        this.<displayRewardContentForSeconds>__0 = 0f;
                        if (this.<>f__this.rewardContentStaysOnScreenUntilClicked())
                        {
                            this.<displayRewardContentForSeconds>__0 = float.MaxValue;
                        }
                        else if ((this.<>f__this.isMultiRewardCeremony() || this.<>f__this.isSingleRewardMultiPartChest()) || !this.<>f__this.Params.SingleRewardOpenAtStart)
                        {
                            this.<displayRewardContentForSeconds>__0 = 1.5f;
                        }
                        this.<>f__this.StartCoroutine(this.chest.finalizationRoutine(this.<displayRewardContentForSeconds>__0));
                        if (this.chest.usesPetAnimationFlow())
                        {
                            this.<>f__this.DescriptionCanvasGroup.setTransparent(true);
                        }
                        break;

                    case 1:
                        break;

                    case 2:
                        this.<tt>__5 = new TransformAnimationTask(this.<ta>__4.transform, 0.3f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__5.scale(Vector3.one, true, Easing.Function.OUT_BOUNCE);
                        this.<ta>__4.addTask(this.<tt>__5);
                        this.<newChest>__3.Interactable = true;
                        goto Label_0306;

                    case 3:
                        goto Label_0306;

                    default:
                        goto Label_0322;
                }
                while (this.chest.FinalizationRunning)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_0324;
                }
                if (this.<>f__this.Params.ConsumeRewardsAfterChestOpen)
                {
                    CmdConsumeReward.ExecuteStatic(GameLogic.Binder.GameState.Player, this.chest.Reward, true, string.Empty);
                }
                if ((this.<>f__this.m_queuedRewards.Count <= 0) || (this.<>f__this.m_queuedRewards.Count <= 0))
                {
                    goto Label_0322;
                }
                this.<>f__this.removeMenuTreasureChest(this.chest);
                this.<reward>__1 = this.<>f__this.m_queuedRewards.keyAt(0);
                this.<openAtStart>__2 = this.<>f__this.m_queuedRewards.valueAt(0);
                this.<>f__this.m_queuedRewards.remove(this.<reward>__1);
                this.<>f__this.refreshChestsRemainingText();
                this.<newChest>__3 = this.<>f__this.addMenuTreasureChestToGrid(this.<reward>__1, this.<openAtStart>__2, 0f, null);
                this.<newChest>__3.gameObject.SetActive(true);
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_RewardSlam, (float) 0f);
                this.<ta>__4 = this.<newChest>__3.TransformAnimation;
                this.<ta>__4.transform.localScale = (Vector3) (Vector3.one * 5f);
                this.$current = null;
                this.$PC = 2;
                goto Label_0324;
            Label_0306:
                if (this.<ta>__4.HasTasks)
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0324;
                }
                goto Label_0322;
                this.$PC = -1;
            Label_0322:
                return false;
            Label_0324:
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
        private sealed class <preShowRoutine>c__Iterator168 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal Dictionary<Reward, bool>.Enumerator <$s_427>__1;
            internal Dictionary<GameObject, TransformAnimation>.Enumerator <$s_428>__9;
            internal RewardCeremonyMenu <>f__this;
            internal int <counter>__0;
            internal bool <doShowNewBounties>__8;
            internal bool <doShowNextLeaderboardEntryTarget>__6;
            internal KeyValuePair<GameObject, TransformAnimation> <kv>__10;
            internal KeyValuePair<Reward, bool> <kv>__2;
            internal float <offsetX>__5;
            internal bool <openAtStart>__4;
            internal Reward <reward>__3;
            internal LeaderboardEntry <target>__7;
            internal object parameter;

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
                    this.<>f__this.Params = (RewardCeremonyMenu.InputParameters) this.parameter;
                    this.<>f__this.Title.text = this.<>f__this.Params.Title;
                    this.<>f__this.DescriptionCanvasGroup.setTransparent(false);
                    this.<>f__this.Descriptions[0].text = this.<>f__this.Params.Description;
                    this.<>f__this.Descriptions[1].text = this.<>f__this.Params.Description2;
                    this.<>f__this.Descriptions[2].text = this.<>f__this.Params.Description3;
                    this.<>f__this.Descriptions[3].text = this.<>f__this.Params.Description4;
                    this.<>f__this.Descriptions[4].text = this.<>f__this.Params.Description5;
                    this.<>f__this.Descriptions[5].text = this.<>f__this.Params.Description6;
                    this.<>f__this.m_queuedRewards.clear();
                    if (this.<>f__this.Params.HideBackground)
                    {
                        this.<>f__this.Ray1.enabled = false;
                        this.<>f__this.Ray2.enabled = false;
                    }
                    else if (this.<>f__this.Params.MultiRewards != null)
                    {
                        this.<>f__this.Ray1.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_menu_swirl");
                        this.<>f__this.Ray2.sprite = this.<>f__this.Ray1.sprite;
                        this.<>f__this.Ray1.enabled = true;
                        this.<>f__this.Ray2.enabled = true;
                    }
                    else
                    {
                        this.<>f__this.Ray1.sprite = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "fx_haloshine");
                        this.<>f__this.Ray2.sprite = this.<>f__this.Ray1.sprite;
                        this.<>f__this.Ray1.enabled = true;
                        this.<>f__this.Ray2.enabled = true;
                    }
                    if (this.<>f__this.Params.SingleReward != null)
                    {
                        this.<>f__this.TreasureChestGridTm.localScale = Vector3.one;
                        this.<>f__this.addMenuTreasureChestToGrid(this.<>f__this.Params.SingleReward, this.<>f__this.Params.SingleRewardOpenAtStart, 0f, this.<>f__this.Params.BeatenLeaderboardEntry);
                    }
                    else
                    {
                        this.<>f__this.TreasureChestGridTm.localScale = Vector3.one;
                        this.<counter>__0 = 0;
                        this.<$s_427>__1 = this.<>f__this.Params.MultiRewards.GetEnumerator();
                        try
                        {
                            while (this.<$s_427>__1.MoveNext())
                            {
                                this.<kv>__2 = this.<$s_427>__1.Current;
                                this.<reward>__3 = this.<kv>__2.Key;
                                this.<openAtStart>__4 = this.<kv>__2.Value;
                                if (this.<counter>__0 < 1)
                                {
                                    this.<offsetX>__5 = 0f + (0f * this.<counter>__0);
                                    this.<>f__this.addMenuTreasureChestToGrid(this.<reward>__3, this.<openAtStart>__4, this.<offsetX>__5, null);
                                    this.<counter>__0++;
                                }
                                else
                                {
                                    this.<>f__this.m_queuedRewards.add(this.<reward>__3, this.<openAtStart>__4);
                                }
                            }
                        }
                        finally
                        {
                            this.<$s_427>__1.Dispose();
                        }
                    }
                    this.<doShowNextLeaderboardEntryTarget>__6 = this.<>f__this.Params.NextLeaderboardEntryTarget != null;
                    this.<>f__this.NextLeaderboardTargetRoot.SetActive(this.<doShowNextLeaderboardEntryTarget>__6);
                    if (this.<doShowNextLeaderboardEntryTarget>__6)
                    {
                        this.<target>__7 = this.<>f__this.Params.NextLeaderboardEntryTarget;
                        this.<>f__this.NextLeaderboardTargetImage.refresh(this.<target>__7.AvatarSpriteId, this.<target>__7.ImageTexture);
                        this.<>f__this.NextLeaderboardTargetDescription.text = _.L(ConfigLoca.CEREMONY_NEXT_TARGET, new <>__AnonType21<string, string>(MenuHelpers.ColoredText(this.<target>__7.HighestFloor), MenuHelpers.ColoredText(this.<target>__7.getPrettyName())), false);
                    }
                    this.<doShowNewBounties>__8 = this.<>f__this.Params.NumNewBountiesAvailable > 0;
                    this.<>f__this.NewBountiesRoot.SetActive(this.<doShowNewBounties>__8);
                    if (this.<doShowNewBounties>__8)
                    {
                        this.<>f__this.NewBountiesDescription.text = _.L(ConfigLoca.NOTIFICATIONS_NEW_BOUNTY, null, false);
                    }
                    this.<>f__this.refreshChestsRemainingText();
                    this.<$s_428>__9 = this.<>f__this.m_transformAnimations.GetEnumerator();
                    try
                    {
                        while (this.<$s_428>__9.MoveNext())
                        {
                            this.<kv>__10 = this.<$s_428>__9.Current;
                            this.<kv>__10.Value.transform.localScale = Vector3.zero;
                        }
                    }
                    finally
                    {
                        this.<$s_428>__9.Dispose();
                    }
                    this.<>f__this.TapAnywhereButton.enabled = false;
                    this.<>f__this.m_resourceFlyToHudAllowed = true;
                    this.<>f__this.m_finalizationTriggered = false;
                    this.<>f__this.m_finalizationCompletedSuccessfully = false;
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
        private sealed class <showRoutine>c__Iterator169 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal RewardCeremonyMenu <>f__this;
            internal bool <cardChoiceFlowActive>__6;
            internal int <i>__2;
            internal int <i>__20;
            internal int <i>__21;
            internal int <i>__7;
            internal IEnumerator <ie>__5;
            internal TransformAnimation <ta>__0;
            internal TransformAnimation <ta>__10;
            internal TransformAnimation <ta>__12;
            internal TransformAnimation <ta>__14;
            internal TransformAnimation <ta>__16;
            internal TransformAnimation <ta>__18;
            internal TransformAnimation <ta>__3;
            internal TransformAnimation <ta>__8;
            internal TransformAnimationTask <tt>__1;
            internal TransformAnimationTask <tt>__11;
            internal TransformAnimationTask <tt>__13;
            internal TransformAnimationTask <tt>__15;
            internal TransformAnimationTask <tt>__17;
            internal TransformAnimationTask <tt>__19;
            internal TransformAnimationTask <tt>__4;
            internal TransformAnimationTask <tt>__9;
            internal bool <usesCardChoiceFlow>__22;

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
                        this.<>f__this.BackgroundOverlay.fadeToBlack(0.25f, ConfigUi.MENU_BACKGROUND_ALPHA, Easing.Function.LINEAR);
                        PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_FlagAppear, (float) 0f);
                        PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_RewardSlam, (float) 0.5f);
                        this.<ta>__0 = this.<>f__this.m_transformAnimations[this.<>f__this.Banderoll.gameObject];
                        this.<ta>__0.transform.localScale = new Vector3(0f, 0.5f, 1f);
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_089D;

                    case 1:
                        this.<tt>__1 = new TransformAnimationTask(this.<ta>__0.transform, 0.345f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__1.scale(Vector3.one, true, Easing.Function.OUT_BOUNCE);
                        this.<ta>__0.addTask(this.<tt>__1);
                        this.<i>__2 = 0;
                        break;

                    case 2:
                        this.<tt>__4 = new TransformAnimationTask(this.<ta>__3.transform, 0.24f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__4.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__3.addTask(this.<tt>__4);
                        this.<i>__2++;
                        break;

                    case 3:
                        goto Label_0238;

                    case 4:
                        this.<tt>__9 = new TransformAnimationTask(this.<ta>__8.transform, 0.3f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__9.scale(Vector3.one, true, Easing.Function.OUT_BOUNCE);
                        this.<ta>__8.addTask(this.<tt>__9);
                        this.<ie>__5 = TimeUtil.WaitForUnscaledSeconds(0.05f);
                        goto Label_03BE;

                    case 5:
                        goto Label_03BE;

                    case 6:
                        this.<tt>__11 = new TransformAnimationTask(this.<ta>__10.transform, 0.24f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__11.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__10.addTask(this.<tt>__11);
                        if (this.<cardChoiceFlowActive>__6)
                        {
                            goto Label_04D8;
                        }
                        this.<ie>__5 = TimeUtil.WaitForUnscaledSeconds(0.2f);
                        goto Label_04C8;

                    case 7:
                        goto Label_04C8;

                    case 8:
                        this.<tt>__13 = new TransformAnimationTask(this.<ta>__12.transform, 0.45f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__13.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__12.addTask(this.<tt>__13);
                        this.<ta>__14 = this.<>f__this.m_transformAnimations[this.<>f__this.Ray2.gameObject];
                        this.<ta>__14.transform.localScale = Vector3.zero;
                        this.$current = null;
                        this.$PC = 9;
                        goto Label_089D;

                    case 9:
                        this.<tt>__15 = new TransformAnimationTask(this.<ta>__14.transform, 0.45f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__15.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__14.addTask(this.<tt>__15);
                        if (this.<cardChoiceFlowActive>__6)
                        {
                            goto Label_064F;
                        }
                        this.<ie>__5 = TimeUtil.WaitForUnscaledSeconds(0.2f);
                        goto Label_063F;

                    case 10:
                        goto Label_063F;

                    case 11:
                        this.<tt>__17 = new TransformAnimationTask(this.<ta>__16.transform, 0.3f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__17.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__16.addTask(this.<tt>__17);
                        goto Label_06F7;

                    case 12:
                        this.<tt>__19 = new TransformAnimationTask(this.<ta>__18.transform, 0.3f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<tt>__19.scale(Vector3.one, true, Easing.Function.OUT_BACK);
                        this.<ta>__18.addTask(this.<tt>__19);
                        goto Label_07A0;

                    case 13:
                        this.<i>__21 = 0;
                        while (this.<i>__21 < this.<>f__this.Chests.Count)
                        {
                            this.<usesCardChoiceFlow>__22 = this.<>f__this.Chests[this.<i>__21].usesCardChoiceFlow();
                            this.<>f__this.Chests[this.<i>__21].Interactable = !this.<usesCardChoiceFlow>__22;
                            this.<i>__21++;
                        }
                        this.<>f__this.TapAnywhereButton.enabled = true;
                        goto Label_089B;

                    default:
                        goto Label_089B;
                }
                if (this.<i>__2 < this.<>f__this.Descriptions.Count)
                {
                    this.<ta>__3 = this.<>f__this.m_transformAnimations[this.<>f__this.Descriptions[this.<i>__2].gameObject];
                    this.<ta>__3.transform.localScale = Vector3.zero;
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_089D;
                }
                this.<ie>__5 = TimeUtil.WaitForUnscaledSeconds(0.45f);
            Label_0238:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 3;
                    goto Label_089D;
                }
                this.<cardChoiceFlowActive>__6 = false;
                this.<i>__7 = 0;
                while (this.<i>__7 < this.<>f__this.Chests.Count)
                {
                    this.<>f__this.Chests[this.<i>__7].gameObject.SetActive(true);
                    if (this.<>f__this.Chests[this.<i>__7].usesCardChoiceFlow())
                    {
                        this.<>f__this.Chests[this.<i>__7].TransformAnimation.RectTm.localScale = Vector3.one;
                        this.<cardChoiceFlowActive>__6 = true;
                        goto Label_03CE;
                    }
                    this.<>f__this.Chests[this.<i>__7].gameObject.SetActive(true);
                    this.<ta>__8 = this.<>f__this.Chests[this.<i>__7].TransformAnimation;
                    this.<ta>__8.transform.localScale = (Vector3) (Vector3.one * 5f);
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_089D;
                Label_03A1:
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 5;
                    goto Label_089D;
                Label_03BE:
                    if (this.<ie>__5.MoveNext())
                    {
                        goto Label_03A1;
                    }
                Label_03CE:
                    this.<i>__7++;
                }
                this.<ta>__10 = this.<>f__this.m_transformAnimations[this.<>f__this.ChestsRemainingText.gameObject];
                this.<ta>__10.transform.localScale = Vector3.zero;
                this.$current = null;
                this.$PC = 6;
                goto Label_089D;
            Label_04C8:
                if (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 7;
                    goto Label_089D;
                }
            Label_04D8:
                this.<ta>__12 = this.<>f__this.m_transformAnimations[this.<>f__this.Ray1.gameObject];
                this.<ta>__12.transform.localScale = Vector3.zero;
                this.$current = null;
                this.$PC = 8;
                goto Label_089D;
            Label_063F:
                if (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 10;
                    goto Label_089D;
                }
            Label_064F:
                if (this.<>f__this.Params.NextLeaderboardEntryTarget != null)
                {
                    this.<ta>__16 = this.<>f__this.m_transformAnimations[this.<>f__this.NextLeaderboardTargetRoot];
                    this.<ta>__16.transform.localScale = Vector3.zero;
                    this.$current = null;
                    this.$PC = 11;
                    goto Label_089D;
                }
            Label_06F7:
                if (this.<>f__this.Params.NumNewBountiesAvailable > 0)
                {
                    this.<ta>__18 = this.<>f__this.m_transformAnimations[this.<>f__this.NewBountiesRoot];
                    this.<ta>__18.transform.localScale = Vector3.zero;
                    this.$current = null;
                    this.$PC = 12;
                    goto Label_089D;
                }
            Label_07A0:
                this.<i>__20 = 0;
                while (this.<i>__20 < this.<>f__this.Chests.Count)
                {
                    this.<>f__this.Chests[this.<i>__20].onShow();
                    this.<i>__20++;
                }
                this.$current = null;
                this.$PC = 13;
                goto Label_089D;
                this.$PC = -1;
            Label_089B:
                return false;
            Label_089D:
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

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParameters
        {
            public RewardCeremonyEntry CeremonyEntry;
            public string Title;
            public string Description;
            public string Description2;
            public string Description3;
            public string Description4;
            public string Description5;
            public string Description6;
            public Reward SingleReward;
            public bool SingleRewardOpenAtStart;
            public Dictionary<Reward, bool> MultiRewards;
            public LeaderboardEntry BeatenLeaderboardEntry;
            public LeaderboardEntry NextLeaderboardEntryTarget;
            public System.Action HideCallback;
            public bool ConsumeRewardsAfterChestOpen;
            public bool DisableFlyToHud;
            public bool HideBackground;
            public int NumNewBountiesAvailable;
        }
    }
}

