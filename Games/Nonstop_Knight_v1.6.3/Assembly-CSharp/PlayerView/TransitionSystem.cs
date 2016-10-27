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
    using UnityEngine;

    public class TransitionSystem : MonoBehaviour
    {
        private Coroutine m_criticalTransitionRoutine;
        private Coroutine m_harmlessTransitionRoutine;
        private bool m_lastRoomFailedDuringThisSession;
        private Stopwatch m_loadStopwatch = new Stopwatch();
        private Queue<IEnumerator> m_queuedCriticalTransitions = new Queue<IEnumerator>();

        public void abortActiveTransitions()
        {
            UnityUtils.StopCoroutine(this, ref this.m_criticalTransitionRoutine);
            UnityUtils.StopCoroutine(this, ref this.m_harmlessTransitionRoutine);
        }

        protected void Awake()
        {
        }

        [DebuggerHidden]
        private IEnumerator bossFightRoutine()
        {
            <bossFightRoutine>c__Iterator1AA iteratoraa = new <bossFightRoutine>c__Iterator1AA();
            iteratoraa.<>f__this = this;
            return iteratoraa;
        }

        [DebuggerHidden]
        private IEnumerator bossRewardVisualizationRoutine(ActiveDungeon.BossRewards bossRewards, Vector3 killWorldPt, int floor, bool wildBoss)
        {
            <bossRewardVisualizationRoutine>c__Iterator1AD iteratorad = new <bossRewardVisualizationRoutine>c__Iterator1AD();
            iteratorad.bossRewards = bossRewards;
            iteratorad.killWorldPt = killWorldPt;
            iteratorad.wildBoss = wildBoss;
            iteratorad.floor = floor;
            iteratorad.<$>bossRewards = bossRewards;
            iteratorad.<$>killWorldPt = killWorldPt;
            iteratorad.<$>wildBoss = wildBoss;
            iteratorad.<$>floor = floor;
            return iteratorad;
        }

        [DebuggerHidden]
        private IEnumerator bossStartRoutine()
        {
            <bossStartRoutine>c__Iterator1A9 iteratora = new <bossStartRoutine>c__Iterator1A9();
            iteratora.<>f__this = this;
            return iteratora;
        }

        [DebuggerHidden]
        private IEnumerator dungeonReloadRoutine()
        {
            <dungeonReloadRoutine>c__Iterator1B8 iteratorb = new <dungeonReloadRoutine>c__Iterator1B8();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        [DebuggerHidden]
        private IEnumerator endActiveTournamentRoutine()
        {
            <endActiveTournamentRoutine>c__Iterator1B5 iteratorb = new <endActiveTournamentRoutine>c__Iterator1B5();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        private void enqueueCriticalTransition(IEnumerator routine)
        {
            this.m_queuedCriticalTransitions.Enqueue(routine);
        }

        public void enqueueDungeonReload()
        {
            this.enqueueCriticalTransition(this.dungeonReloadRoutine());
        }

        public void enqueueGameStart()
        {
            this.enqueueCriticalTransition(this.gameStartRoutine());
        }

        public void enqueueRevivePrimaryPlayerCharacter(float normalizedHpGain, bool free)
        {
            this.enqueueCriticalTransition(this.reviveRoutine(normalizedHpGain, free));
        }

        [DebuggerHidden]
        private IEnumerator floorTransitionRoutine()
        {
            <floorTransitionRoutine>c__Iterator1B4 iteratorb = new <floorTransitionRoutine>c__Iterator1B4();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        [DebuggerHidden]
        private IEnumerator frenzyActivationRoutine()
        {
            <frenzyActivationRoutine>c__Iterator1B9 iteratorb = new <frenzyActivationRoutine>c__Iterator1B9();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        [DebuggerHidden]
        private IEnumerator frenzyDectivationRoutine()
        {
            <frenzyDectivationRoutine>c__Iterator1BA iteratorba = new <frenzyDectivationRoutine>c__Iterator1BA();
            iteratorba.<>f__this = this;
            return iteratorba;
        }

        [DebuggerHidden]
        private IEnumerator gameStartRoutine()
        {
            <gameStartRoutine>c__Iterator1B6 iteratorb = new <gameStartRoutine>c__Iterator1B6();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        [DebuggerHidden]
        private IEnumerator heroDeathRoutine()
        {
            <heroDeathRoutine>c__Iterator1AE iteratorae = new <heroDeathRoutine>c__Iterator1AE();
            iteratorae.<>f__this = this;
            return iteratorae;
        }

        private void onCharacterKilled(CharacterInstance c, CharacterInstance killer, bool critted, SkillType fromSkill)
        {
            if (c.IsWildBoss)
            {
                ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                if (activeDungeon.WildBossMode && (activeDungeon.ActiveRoom.numberOfWildBossesAlive() == 0))
                {
                    bool escaped = killer == null;
                    this.enqueueCriticalTransition(this.wildBossOverRoutine(escaped));
                    Service.Binder.TrackingSystem.sendBossFightEvent(GameLogic.Binder.GameState.Player, c.CharacterId, c.IsEliteBoss, c.IsWildBoss, true, Room.BossSummonMethod.WildBoss);
                    activeDungeon.WildBossMode = false;
                    activeDungeon.WildBossEscapeTriggered = false;
                }
            }
        }

        private void onCharacterTargetUpdated(CharacterInstance character, CharacterInstance oldTarget)
        {
            if ((character.IsWildBoss && (character.TargetCharacter != null)) && character.TargetCharacter.IsPrimaryPlayerCharacter)
            {
                ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                if (!activeDungeon.WildBossMode)
                {
                    this.enqueueCriticalTransition(this.wildBossSpottedRoutine(character));
                    activeDungeon.WildBossMode = true;
                    activeDungeon.WildBossEscapeTriggered = false;
                }
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnRoomCompleted -= new GameLogic.Events.RoomCompleted(this.onRoomCompleted);
            GameLogic.Binder.EventBus.OnGameplayStateChanged -= new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            GameLogic.Binder.EventBus.OnFrenzyActivated -= new GameLogic.Events.FrenzyActivated(this.onFrenzyActivated);
            GameLogic.Binder.EventBus.OnFrenzyDeactivated -= new GameLogic.Events.FrenzyDeactivated(this.onFrenzyDeactivated);
            GameLogic.Binder.EventBus.OnCharacterTargetUpdated -= new GameLogic.Events.CharacterTargetUpdated(this.onCharacterTargetUpdated);
            GameLogic.Binder.EventBus.OnCharacterKilled -= new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnRoomCompleted += new GameLogic.Events.RoomCompleted(this.onRoomCompleted);
            GameLogic.Binder.EventBus.OnGameplayStateChanged += new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            GameLogic.Binder.EventBus.OnFrenzyActivated += new GameLogic.Events.FrenzyActivated(this.onFrenzyActivated);
            GameLogic.Binder.EventBus.OnFrenzyDeactivated += new GameLogic.Events.FrenzyDeactivated(this.onFrenzyDeactivated);
            GameLogic.Binder.EventBus.OnCharacterTargetUpdated += new GameLogic.Events.CharacterTargetUpdated(this.onCharacterTargetUpdated);
            GameLogic.Binder.EventBus.OnCharacterKilled += new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
        }

        private void onFrenzyActivated()
        {
            this.startHarmlessTransition(this.frenzyActivationRoutine());
        }

        private void onFrenzyDeactivated()
        {
            this.startHarmlessTransition(this.frenzyDectivationRoutine());
        }

        private void onGameplayStateChanged(GameplayState previousState, GameplayState currentState)
        {
            if (currentState == GameplayState.BOSS_START)
            {
                this.enqueueCriticalTransition(this.bossStartRoutine());
            }
            else if (currentState == GameplayState.BOSS_FIGHT)
            {
                this.enqueueCriticalTransition(this.bossFightRoutine());
            }
            if (!GameLogic.Binder.GameState.ActiveDungeon.isTutorialDungeon())
            {
                if (currentState == GameplayState.START_CEREMONY_STEP1)
                {
                    this.enqueueCriticalTransition(this.startCeremony1Routine());
                }
                else if (currentState == GameplayState.START_CEREMONY_STEP2)
                {
                    this.enqueueCriticalTransition(this.startCeremony2Routine());
                }
            }
        }

        private void onRoomCompleted(Room room)
        {
            switch (room.EndCondition)
            {
                case RoomEndCondition.NORMAL_COMPLETION:
                case RoomEndCondition.FRENZY_COMPLETION:
                    this.m_lastRoomFailedDuringThisSession = false;
                    this.enqueueCriticalTransition(this.roomCompletionRoutine());
                    break;

                case RoomEndCondition.FAIL:
                    this.m_lastRoomFailedDuringThisSession = true;
                    this.enqueueCriticalTransition(this.heroDeathRoutine());
                    break;

                case RoomEndCondition.TOURNAMENT_END:
                    this.m_lastRoomFailedDuringThisSession = false;
                    this.enqueueCriticalTransition(this.endActiveTournamentRoutine());
                    break;
            }
        }

        public void retire()
        {
            this.abortActiveTransitions();
            this.enqueueCriticalTransition(this.retirementRoutine());
        }

        [DebuggerHidden]
        private IEnumerator retirementRoutine()
        {
            <retirementRoutine>c__Iterator1B2 iteratorb = new <retirementRoutine>c__Iterator1B2();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        [DebuggerHidden]
        private IEnumerator reviveRoutine(float normalizedHpGain, bool free)
        {
            <reviveRoutine>c__Iterator1B7 iteratorb = new <reviveRoutine>c__Iterator1B7();
            iteratorb.free = free;
            iteratorb.normalizedHpGain = normalizedHpGain;
            iteratorb.<$>free = free;
            iteratorb.<$>normalizedHpGain = normalizedHpGain;
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        [DebuggerHidden]
        private IEnumerator roomCompletionRoutine()
        {
            <roomCompletionRoutine>c__Iterator1AF iteratoraf = new <roomCompletionRoutine>c__Iterator1AF();
            iteratoraf.<>f__this = this;
            return iteratoraf;
        }

        [DebuggerHidden]
        private IEnumerator startCeremony1Routine()
        {
            <startCeremony1Routine>c__Iterator1B0 iteratorb = new <startCeremony1Routine>c__Iterator1B0();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        [DebuggerHidden]
        private IEnumerator startCeremony2Routine()
        {
            <startCeremony2Routine>c__Iterator1B1 iteratorb = new <startCeremony2Routine>c__Iterator1B1();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        private void startHarmlessTransition(IEnumerator routine)
        {
            UnityUtils.StopCoroutine(this, ref this.m_harmlessTransitionRoutine);
            this.m_harmlessTransitionRoutine = UnityUtils.StartCoroutine(this, routine);
        }

        public void switchAdventure(string tournamentId, string locationChangeCustomText)
        {
            this.abortActiveTransitions();
            this.enqueueCriticalTransition(this.switchAdventureRoutine(tournamentId, locationChangeCustomText));
        }

        [DebuggerHidden]
        private IEnumerator switchAdventureRoutine(string tournamentId, string locationChangeCustomText)
        {
            <switchAdventureRoutine>c__Iterator1B3 iteratorb = new <switchAdventureRoutine>c__Iterator1B3();
            iteratorb.locationChangeCustomText = locationChangeCustomText;
            iteratorb.tournamentId = tournamentId;
            iteratorb.<$>locationChangeCustomText = locationChangeCustomText;
            iteratorb.<$>tournamentId = tournamentId;
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        protected void Update()
        {
            if ((this.m_queuedCriticalTransitions.Count > 0) && !this.InCriticalTransition)
            {
                UnityUtils.StopCoroutine(this, ref this.m_harmlessTransitionRoutine);
                this.m_criticalTransitionRoutine = UnityUtils.StartCoroutine(this, this.m_queuedCriticalTransitions.Dequeue());
            }
        }

        [DebuggerHidden]
        private IEnumerator wildBossOverRoutine(bool escaped)
        {
            <wildBossOverRoutine>c__Iterator1AC iteratorac = new <wildBossOverRoutine>c__Iterator1AC();
            iteratorac.escaped = escaped;
            iteratorac.<$>escaped = escaped;
            iteratorac.<>f__this = this;
            return iteratorac;
        }

        [DebuggerHidden]
        private IEnumerator wildBossSpottedRoutine(CharacterInstance wildBoss)
        {
            <wildBossSpottedRoutine>c__Iterator1AB iteratorab = new <wildBossSpottedRoutine>c__Iterator1AB();
            iteratorab.wildBoss = wildBoss;
            iteratorab.<$>wildBoss = wildBoss;
            iteratorab.<>f__this = this;
            return iteratorab;
        }

        public bool InCriticalTransition
        {
            get
            {
                return UnityUtils.CoroutineRunning(ref this.m_criticalTransitionRoutine);
            }
        }

        [CompilerGenerated]
        private sealed class <bossFightRoutine>c__Iterator1AA : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TransitionSystem <>f__this;
            internal ActiveDungeon <ad>__0;
            internal CharacterInstance <boss>__1;
            internal IEnumerator <ie>__2;

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
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00C8;

                    default:
                        goto Label_010B;
                }
                if (this.<ad>__0.ActiveRoom.ActiveCharacters.Count == 0)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_010D;
                }
                this.<boss>__1 = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.getFirstAliveBoss();
                PlayerView.Binder.DungeonHud.FloorProgressionRibbon.refreshBossBar(this.<boss>__1);
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(0.1f);
            Label_00C8:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 2;
                    goto Label_010D;
                }
                GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.ACTION, 0f), 0f);
                this.<>f__this.m_criticalTransitionRoutine = null;
                goto Label_010B;
                this.$PC = -1;
            Label_010B:
                return false;
            Label_010D:
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
        private sealed class <bossRewardVisualizationRoutine>c__Iterator1AD : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ActiveDungeon.BossRewards <$>bossRewards;
            internal int <$>floor;
            internal Vector3 <$>killWorldPt;
            internal bool <$>wildBoss;
            internal int <coinDropCount>__2;
            internal double <coinsPerDrop>__4;
            internal bool <frenzyActive>__1;
            internal int <i>__10;
            internal int <i>__5;
            internal int <i>__9;
            internal Sprite <overrideSprite>__8;
            internal Player <player>__0;
            internal Reward <tempReward>__7;
            internal Vector3 <worldPos>__6;
            internal double <xpPerDrop>__3;
            internal ActiveDungeon.BossRewards bossRewards;
            internal int floor;
            internal Vector3 killWorldPt;
            internal bool wildBoss;

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
                        this.<frenzyActive>__1 = GameLogic.Binder.FrenzySystem.isFrenzyActive();
                        this.<coinDropCount>__2 = this.bossRewards.CoinDropCount;
                        this.<xpPerDrop>__3 = this.bossRewards.XpPerDrop;
                        this.<coinsPerDrop>__4 = this.bossRewards.CoinsPerDrop;
                        this.<i>__5 = 0;
                        break;

                    case 1:
                    case 2:
                        PlayerView.Binder.DungeonHud.flyToHudXpGain(this.<xpPerDrop>__3, this.<worldPos>__6, false, false);
                        this.<i>__5++;
                        break;

                    case 3:
                        goto Label_01C3;

                    case 4:
                        goto Label_01EA;

                    case 5:
                        goto Label_027D;

                    case 6:
                        goto Label_02A4;

                    case 7:
                        goto Label_0303;

                    case 8:
                        goto Label_037F;

                    case 9:
                        goto Label_043D;

                    default:
                        goto Label_0472;
                }
                if (this.<i>__5 < this.<coinDropCount>__2)
                {
                    this.<worldPos>__6 = this.killWorldPt + new Vector3(UnityEngine.Random.Range((float) -0.75f, (float) 0.75f), 0f, UnityEngine.Random.Range((float) -0.75f, (float) 0.75f));
                    PlayerView.Binder.DungeonHud.flyToHudCoinGain(this.<coinsPerDrop>__4, this.<worldPos>__6, false);
                    if (this.<frenzyActive>__1)
                    {
                        this.$current = null;
                        this.$PC = 1;
                    }
                    else
                    {
                        this.$current = new WaitForSeconds(ConfigGameplay.BOSS_DELAY_BETWEEN_SHOWERS);
                        this.$PC = 2;
                    }
                    goto Label_0474;
                }
                if (this.bossRewards.Tokens > 0.0)
                {
                    PlayerView.Binder.DungeonHud.flyToHudTokenGain(this.bossRewards.Tokens, this.killWorldPt, false, true);
                    if (!this.<frenzyActive>__1)
                    {
                        this.$current = new WaitForSeconds(1f);
                        this.$PC = 3;
                        goto Label_0474;
                    }
                }
            Label_01C3:
                if (!this.<frenzyActive>__1)
                {
                    this.$current = new WaitForSeconds(0.5f);
                    this.$PC = 4;
                    goto Label_0474;
                }
            Label_01EA:
                if (this.bossRewards.FrenzyPotions > 0)
                {
                    Reward reward = new Reward();
                    reward.ChestType = ChestType.RetirementTrigger;
                    reward.FrenzyPotions = this.bossRewards.FrenzyPotions;
                    this.<tempReward>__7 = reward;
                    this.<overrideSprite>__8 = PlayerView.Binder.SpriteResources.getSprite("Menu", "icon_bottle_frenzy_floater");
                    PlayerView.Binder.RoomView.visualizeChestDrop(this.<tempReward>__7, this.killWorldPt, this.<overrideSprite>__8);
                    if (!this.<frenzyActive>__1)
                    {
                        this.$current = new WaitForSeconds(1f);
                        this.$PC = 5;
                        goto Label_0474;
                    }
                }
            Label_027D:
                if (!this.<frenzyActive>__1)
                {
                    this.$current = new WaitForSeconds(0.5f);
                    this.$PC = 6;
                    goto Label_0474;
                }
            Label_02A4:
                if (this.bossRewards.RiggedRewards != null)
                {
                    this.<i>__9 = 0;
                    while (this.<i>__9 < this.bossRewards.RiggedRewards.Count)
                    {
                        PlayerView.Binder.RoomView.visualizeChestDrop(this.bossRewards.RiggedRewards[this.<i>__9], this.killWorldPt, null);
                        this.$current = new WaitForSeconds(1f);
                        this.$PC = 7;
                        goto Label_0474;
                    Label_0303:
                        this.<i>__9++;
                    }
                }
                if (this.bossRewards.AdditionalDrop != null)
                {
                    PlayerView.Binder.RoomView.visualizeChestDrop(this.bossRewards.AdditionalDrop, this.killWorldPt, null);
                    if (!this.<frenzyActive>__1)
                    {
                        this.$current = new WaitForSeconds(1f);
                        this.$PC = 8;
                        goto Label_0474;
                    }
                }
            Label_037F:
                if ((this.bossRewards.MainDrops != null) && ((this.wildBoss || this.<frenzyActive>__1) || (this.<player>__0.BossTrain.Active || this.<player>__0.autoSummonBossInFloor(this.floor))))
                {
                    this.<i>__10 = 0;
                    while (this.<i>__10 < this.bossRewards.MainDrops.Count)
                    {
                        PlayerView.Binder.RoomView.visualizeChestDrop(this.bossRewards.MainDrops[this.<i>__10], this.killWorldPt, null);
                        if (this.<i>__10 < (this.bossRewards.MainDrops.Count - 1))
                        {
                            this.$current = new WaitForSeconds(1f);
                            this.$PC = 9;
                            goto Label_0474;
                        }
                    Label_043D:
                        this.<i>__10++;
                    }
                    goto Label_0472;
                    this.$PC = -1;
                }
            Label_0472:
                return false;
            Label_0474:
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
        private sealed class <bossStartRoutine>c__Iterator1A9 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TransitionSystem <>f__this;
            internal ActiveDungeon <ad>__0;
            internal string <bossTitle>__3;
            internal IEnumerator <ie>__2;
            internal Player <player>__1;

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
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        this.<ie>__2 = PlayerView.Binder.RoomView.RoomCamera.dimLightsRoutine(true, 1f);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00CC;

                    case 3:
                        goto Label_020E;

                    case 4:
                        goto Label_027C;

                    default:
                        goto Label_02BF;
                }
                if (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 1;
                    goto Label_02C1;
                }
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(0.5f);
            Label_00CC:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 2;
                    goto Label_02C1;
                }
                if (this.<ad>__0.isEliteBossFloor())
                {
                    this.<bossTitle>__3 = _.L(ConfigLoca.ANNOUNCEMENT_ELITE_BOSS, null, false);
                }
                else
                {
                    this.<bossTitle>__3 = _.L(ConfigLoca.ANNOUNCEMENT_BOSS, null, false);
                }
                if (this.<player>__1.BossTrain.Active)
                {
                    this.<bossTitle>__3 = this.<bossTitle>__3 + string.Concat(new object[] { " ", this.<player>__1.BossTrain.NumBossFloorsCompleted + 1, "/", this.<player>__1.BossTrain.ChargesTotal });
                }
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_BannerAnnounceBoss, (float) 0f);
                PlayerView.Binder.DungeonHud.AnnouncementBanner.show(this.<bossTitle>__3, GameLogic.Binder.CharacterResources.getResource(this.<ad>__0.BossId).Name, true, 0f, 1f, null);
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(0.1f);
            Label_020E:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 3;
                    goto Label_02C1;
                }
                GameLogic.Binder.SkillSystem.endSkillCooldownTimers();
                CmdGainHp.ExecuteStatic(this.<ad>__0.PrimaryPlayerCharacter, this.<ad>__0.PrimaryPlayerCharacter.MaxLife(true), true);
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(2f);
            Label_027C:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 4;
                    goto Label_02C1;
                }
                GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.BOSS_FIGHT, 0f), 0f);
                this.<>f__this.m_criticalTransitionRoutine = null;
                goto Label_02BF;
                this.$PC = -1;
            Label_02BF:
                return false;
            Label_02C1:
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
        private sealed class <dungeonReloadRoutine>c__Iterator1B8 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal int <_targetFloor>__4;
            internal TransitionSystem <>f__this;
            internal ActiveDungeon <ad>__0;
            internal bool <bossTrainWasActive>__3;
            internal int <i>__8;
            internal CharacterInstance <pc>__2;
            internal Player <player>__1;
            internal string <targetDungeonId>__5;
            internal int <targetFloor>__6;
            internal CharacterView <view>__7;

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
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        this.<pc>__2 = this.<ad>__0.PrimaryPlayerCharacter;
                        this.<bossTrainWasActive>__3 = this.<player>__1.BossTrain.Active;
                        CmdEndBossTrain.ExecuteStatic(this.<player>__1);
                        if (!this.<bossTrainWasActive>__3)
                        {
                            PlayerView.Binder.DungeonHud.hideHpBars();
                            this.$current = PlayerView.Binder.DungeonHud.animateOverlay(true, 0.5f, new Color?(Color.black));
                            this.$PC = 4;
                        }
                        else
                        {
                            this.$current = PlayerView.Binder.ScreenTransitionEffect.fadeToBlack(0f);
                            this.$PC = 1;
                        }
                        goto Label_0429;

                    case 1:
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.END_CEREMONY, 0f), 0f);
                        this.$PC = 2;
                        goto Label_0429;

                    case 2:
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdEndGameplay(this.<ad>__0, false), 0f);
                        this.$PC = 3;
                        goto Label_0429;

                    case 3:
                        if (this.<player>__1.BossTrain.NumBossFloorsCompleted <= 0)
                        {
                            this.<player>__1.setLastCompletedFloor(Mathf.Clamp(this.<player>__1.BossTrain.ActivationFloor - 1, 0, 0x7fffffff), false);
                            break;
                        }
                        this.<player>__1.setLastCompletedFloor(Mathf.Clamp(this.<player>__1.getLastCompletedFloor(false) - 2, 0, 0x7fffffff), false);
                        break;

                    case 4:
                        PlayerView.Binder.RoomView.RoomCamera.setActiveCameraModeTarget(null);
                        GameLogic.Binder.DeathSystem.killAllNonPersistentCharacters(true, true);
                        goto Label_0230;

                    case 5:
                        goto Label_0230;

                    case 6:
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdStartGameplay(this.<targetDungeonId>__5, this.<targetFloor>__6, true, null), 0f);
                        this.$PC = 7;
                        goto Label_0429;

                    case 7:
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<pc>__2 = this.<ad>__0.PrimaryPlayerCharacter;
                        PlayerView.Binder.RoomView.RoomCamera.setActiveCameraModeTarget(this.<pc>__2);
                        PlayerView.Binder.RoomView.RoomCamera.Transform.position = this.<pc>__2.PhysicsBody.Transform.position + PlayerView.Binder.RoomView.RoomCamera.Offset;
                        PlayerView.Binder.RoomView.RoomCamera.FovAnimator.setFov(PlayerView.Binder.RoomView.RoomCamera.FovAnimator.DefaultFov);
                        PlayerView.Binder.RoomView.RoomCamera.transitionToMood(this.<ad>__0.Mood, 0f);
                        this.<view>__7 = PlayerView.Binder.RoomView.getCharacterViewForCharacter(this.<pc>__2);
                        this.<i>__8 = 0;
                        while (this.<i>__8 < 120)
                        {
                            this.<view>__7.Animator.Update();
                            this.<i>__8++;
                        }
                        PlayerView.Binder.DungeonHud.animateOverlay(false, 1f, null);
                        GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.START_CEREMONY_STEP1, 0f), 0f);
                        this.<>f__this.m_criticalTransitionRoutine = null;
                        goto Label_0427;

                    default:
                        goto Label_0427;
                }
                this.<_targetFloor>__4 = this.<player>__1.getLastCompletedFloor(false) + 1;
                GameLogic.Binder.CommandProcessor.execute(new CmdStartGameplay(ConfigDungeons.GetDungeonIdForFloor(this.<_targetFloor>__4), this.<_targetFloor>__4, false, null), 0f);
                this.<>f__this.m_criticalTransitionRoutine = null;
                goto Label_0427;
            Label_0230:
                if (GameLogic.Binder.EventBus.ProcessingQueuedEvents)
                {
                    this.$current = null;
                    this.$PC = 5;
                }
                else
                {
                    CmdSavePlayerDataToPersistentStorage.ExecuteStatic(this.<player>__1);
                    this.<targetDungeonId>__5 = this.<ad>__0.Dungeon.Id;
                    this.<targetFloor>__6 = this.<ad>__0.Floor;
                    this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdEndGameplay(this.<ad>__0, true), 0f);
                    this.$PC = 6;
                }
                goto Label_0429;
                this.$PC = -1;
            Label_0427:
                return false;
            Label_0429:
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
        private sealed class <endActiveTournamentRoutine>c__Iterator1B5 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TransitionSystem <>f__this;
            internal ActiveDungeon <ad>__0;

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
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.WAITING, 0f), 0f);
                        this.$PC = 1;
                        goto Label_00FA;

                    case 1:
                        if (this.<ad>__0.ActiveTournament == null)
                        {
                            goto Label_00C3;
                        }
                        break;

                    case 2:
                        break;

                    default:
                        goto Label_00F8;
                }
                if (PlayerView.Binder.MenuSystem.InTransition || ((PlayerView.Binder.MenuSystem.topmostActiveMenu() != null) && (PlayerView.Binder.MenuSystem.topmostActiveMenu().activeContentType() == MenuContentType.BannerPopupContent)))
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_00FA;
                }
            Label_00C3:
                this.<>f__this.switchAdventure(null, StringExtensions.ToUpperLoca(_.L(ConfigLoca.BH_SWITCH_TO_ADVENTURE, null, false)));
                this.<>f__this.m_criticalTransitionRoutine = null;
                goto Label_00F8;
                this.$PC = -1;
            Label_00F8:
                return false;
            Label_00FA:
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
        private sealed class <floorTransitionRoutine>c__Iterator1B4 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TransitionSystem <>f__this;
            internal ActiveDungeon <ad>__1;
            internal Transform <camTm>__11;
            internal bool <frenzyActive>__3;
            internal DungeonMood <frenzyMood>__4;
            internal IEnumerator <ie>__16;
            internal LocationEndCeremonyMenu.InputParams <ip>__14;
            internal LocationEndCeremonyMenu <lecm>__13;
            internal CharacterInstance <pc>__2;
            internal Player <player>__0;
            internal float <referenceDuration>__5;
            internal bool <showLocationEndCeremony>__9;
            internal Coroutine <spinRoutine>__10;
            internal Dungeon <targetDungeon>__8;
            internal string <targetDungeonId>__7;
            internal int <targetFloor>__6;
            internal Vector3 <targetLocalPos>__15;
            internal TransformAnimationTask <tt>__12;

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
                        this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<pc>__2 = this.<player>__0.ActiveCharacter;
                        this.<frenzyActive>__3 = GameLogic.Binder.FrenzySystem.isFrenzyActive();
                        this.<frenzyMood>__4 = GameLogic.Binder.DungeonMoodResources.getMood("MoodFrenzy");
                        this.<referenceDuration>__5 = !this.<frenzyActive>__3 ? 0.8f : 0.3f;
                        if (this.<player>__0.BossTrain.Active && (this.<player>__0.BossTrain.ChargesRemaining <= 0))
                        {
                            CmdEndBossTrain.ExecuteStatic(this.<player>__0);
                        }
                        PlayerView.Binder.DungeonHud.FloorProgressionRibbon.refreshBossBar(null);
                        if (this.<ad>__1.Dungeon.Id.StartsWith("Test"))
                        {
                            this.<targetFloor>__6 = 1;
                            this.<targetDungeonId>__7 = this.<ad>__1.Dungeon.Id;
                        }
                        else if (this.<player>__0.BossTrain.Active)
                        {
                            this.<targetFloor>__6 = this.<player>__0.BossTrain.PendingJumpToFloorWithBoss;
                            this.<targetDungeonId>__7 = ConfigDungeons.GetDungeonIdForFloor(this.<targetFloor>__6);
                            this.<player>__0.setLastCompletedFloor(Mathf.Clamp(this.<targetFloor>__6 - 1, 0, 0x7fffffff), false);
                        }
                        else
                        {
                            this.<targetFloor>__6 = this.<player>__0.getLastCompletedFloor(false) + 1;
                            if (this.<ad>__1.isTutorialDungeon())
                            {
                                this.<targetFloor>__6 = 1;
                            }
                            this.<targetDungeonId>__7 = ConfigDungeons.GetDungeonIdForFloor(this.<targetFloor>__6);
                        }
                        this.<targetDungeon>__8 = GameLogic.Binder.DungeonResources.getResource(this.<targetDungeonId>__7);
                        this.<showLocationEndCeremony>__9 = this.<ad>__1.Dungeon.Theme != this.<targetDungeon>__8.Theme;
                        this.$current = new WaitForSeconds(this.<referenceDuration>__5 * 0.25f);
                        this.$PC = 1;
                        goto Label_0CB3;

                    case 1:
                        if (!this.<showLocationEndCeremony>__9)
                        {
                            goto Label_034A;
                        }
                        this.$current = new WaitForSeconds(this.<referenceDuration>__5 * 0.5f);
                        this.$PC = 2;
                        goto Label_0CB3;

                    case 2:
                        PlayerView.Binder.DungeonHud.openCloseSkillBar(false, ConfigUi.MENU_TRANSITION_DURATION);
                        PlayerView.Binder.DungeonHud.LeftPanel.close(ConfigUi.MENU_TRANSITION_DURATION, ConfigUi.MENU_EASING_IN, 0f);
                        PlayerView.Binder.DungeonHud.RightPanel.close(ConfigUi.MENU_TRANSITION_DURATION, ConfigUi.MENU_EASING_IN, 0f);
                        PlayerView.Binder.DungeonHud.BossHuntTicker.gameObject.SetActive(false);
                        if (!PlayerView.Binder.MenuSystem.InTransition && !PlayerView.Binder.MenuSystem.menuTypeInActiveStack(MenuType.RewardCeremonyMenu))
                        {
                            break;
                        }
                        this.$current = null;
                        this.$PC = 3;
                        goto Label_0CB3;

                    case 3:
                        break;

                    case 4:
                        PlayerView.Binder.EffectSystem.playEffectStatic(this.<pc>__2.getGroundLevelWorldPos(), EffectType.Teleport, -1f, false, 1f, null);
                        this.$current = new WaitForSeconds(0.1f);
                        this.$PC = 5;
                        goto Label_0CB3;

                    case 5:
                        this.<pc>__2.ExternallyControlled = false;
                        UnityUtils.StopCoroutine(this.<>f__this, ref this.<spinRoutine>__10);
                        PlayerView.Binder.RoomView.getCharacterViewForCharacter(this.<pc>__2).setVisibility(false);
                        goto Label_044E;

                    case 6:
                        goto Label_044E;

                    case 7:
                    case 8:
                        while (PlayerView.Binder.MenuSystem.InTransition)
                        {
                            this.$current = null;
                            this.$PC = 8;
                            goto Label_0CB3;
                        }
                        CmdSavePlayerDataToPersistentStorage.ExecuteStatic(this.<player>__0);
                        if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Low)
                        {
                            Resources.UnloadUnusedAssets();
                        }
                        PlayerView.Binder.AudioSystem.playSfx(!this.<frenzyActive>__3 ? AudioSourceType.SfxGameplay_NextFloorClouds : AudioSourceType.SfxGameplay_NextFloorCloudsShort, (float) 0f);
                        this.<camTm>__11 = PlayerView.Binder.RoomView.RoomCamera.Transform;
                        this.<tt>__12 = new TransformAnimationTask(this.<camTm>__11, this.<referenceDuration>__5, 0f, TimeUtil.DeltaTimeType.DELTA_TIME);
                        this.<tt>__12.translate(this.<camTm>__11.localPosition + ((Vector3) (Vector3.up * -20f)), true, Easing.Function.IN_EXPO);
                        PlayerView.Binder.RoomView.RoomCamera.TransformAnimation.stopAll();
                        PlayerView.Binder.RoomView.RoomCamera.TransformAnimation.addTask(this.<tt>__12);
                        PlayerView.Binder.DungeonHud.FloorTransitionFx.gameObject.SetActive(false);
                        PlayerView.Binder.DungeonHud.FloorTransitionFx.startDelay = !this.<frenzyActive>__3 ? 0.3f : 0f;
                        PlayerView.Binder.DungeonHud.FloorTransitionFx.gameObject.SetActive(true);
                        PlayerView.Binder.DungeonHud.Vignetting.CrossFadeAlpha(0f, this.<referenceDuration>__5, false);
                        if (this.<frenzyActive>__3)
                        {
                            PlayerView.Binder.DungeonHud.animateOverlay(true, this.<referenceDuration>__5, new Color?(this.<frenzyMood>__4.BackgroundColor));
                        }
                        else
                        {
                            PlayerView.Binder.DungeonHud.animateOverlay(true, this.<referenceDuration>__5, new Color?(this.<ad>__1.Mood.BackgroundColor));
                        }
                        this.$current = new WaitForSeconds(this.<referenceDuration>__5);
                        this.$PC = 9;
                        goto Label_0CB3;

                    case 9:
                    case 10:
                        if (!GameLogic.Binder.DeathSystem.allQueuedCharactersDestroyed())
                        {
                            this.$current = null;
                            this.$PC = 10;
                        }
                        else if (!this.<showLocationEndCeremony>__9)
                        {
                            this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdEndGameplay(this.<ad>__1, true), 0f);
                            this.$PC = 0x12;
                        }
                        else
                        {
                            PlayerView.Binder.DungeonHud.FloorProgressionRibbon.initialize(this.<targetFloor>__6);
                            this.$current = new WaitForSeconds(0.2f);
                            this.$PC = 11;
                        }
                        goto Label_0CB3;

                    case 11:
                        this.<lecm>__13 = (LocationEndCeremonyMenu) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.LocationEndCeremonyMenu);
                        this.<ip>__14 = new LocationEndCeremonyMenu.InputParams();
                        this.$current = PlayerView.Binder.MenuSystem.waitAndTransitionToNewMenu(MenuType.LocationEndCeremonyMenu, MenuContentType.NONE, this.<ip>__14);
                        this.$PC = 12;
                        goto Label_0CB3;

                    case 12:
                        this.$current = new WaitForSeconds(0.6f);
                        this.$PC = 13;
                        goto Label_0CB3;

                    case 13:
                        PlayerView.Binder.DungeonHud.FloorTransitionFx.gameObject.SetActive(false);
                        Service.Binder.TrackingSystem.sendAreaChangeEvent(this.<player>__0, this.<ad>__1.Dungeon.Theme.ToString(), this.<targetDungeon>__8.Theme.ToString(), "started", 0L);
                        this.<>f__this.m_loadStopwatch.Reset();
                        this.<>f__this.m_loadStopwatch.Start();
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdEndGameplay(this.<ad>__1, false), 0f);
                        this.$PC = 14;
                        goto Label_0CB3;

                    case 14:
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdStartGameplay(this.<targetDungeonId>__7, this.<targetFloor>__6, false, new CmdStartGameplay.ProgressCallback(this.<lecm>__13.onLoadProgress)), 0f);
                        this.$PC = 15;
                        goto Label_0CB3;

                    case 15:
                        this.<>f__this.m_loadStopwatch.Stop();
                        Service.Binder.TrackingSystem.sendAreaChangeEvent(this.<player>__0, this.<ad>__1.Dungeon.Theme.ToString(), this.<targetDungeon>__8.Theme.ToString(), "completed", this.<>f__this.m_loadStopwatch.ElapsedMilliseconds);
                        PlayerView.Binder.RoomView.getCharacterViewForCharacter(this.<pc>__2).setVisibility(false);
                        PlayerView.Binder.RoomView.RoomCamera.FovAnimator.setFov(PlayerView.Binder.RoomView.RoomCamera.FovAnimator.DefaultFov);
                        this.$current = PlayerView.Binder.DungeonHud.animateOverlay(true, 0f, new Color?(this.<ad>__1.Mood.BackgroundColor));
                        this.$PC = 0x10;
                        goto Label_0CB3;

                    case 0x10:
                        PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxGameplay_NextFloorClouds, (float) 0f);
                        PlayerView.Binder.DungeonHud.FloorTransitionFx.gameObject.SetActive(true);
                        this.$current = new WaitForSeconds(0.4f);
                        this.$PC = 0x11;
                        goto Label_0CB3;

                    case 0x11:
                        PlayerView.Binder.MenuSystem.waitAndCloseAllMenus();
                        PlayerView.Binder.InputSystem.setInputRequirement(InputSystem.Layer.LocationEndTransition, InputSystem.Requirement.Neutral);
                        goto Label_09CB;

                    case 0x12:
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdStartGameplay(this.<targetDungeonId>__7, this.<targetFloor>__6, true, null), 0f);
                        this.$PC = 0x13;
                        goto Label_0CB3;

                    case 0x13:
                        goto Label_09CB;

                    case 20:
                        PlayerView.Binder.DungeonHud.Vignetting.color = Color.white;
                        PlayerView.Binder.RoomView.RoomCamera.refreshDefaultOrientation();
                        PlayerView.Binder.RoomView.RoomCamera.setActiveCameraModeTarget(this.<pc>__2);
                        PlayerView.Binder.RoomView.RoomCamera.getActiveCameraMode().update(1000f);
                        PlayerView.Binder.RoomView.RoomCamera.setActiveCameraModeTarget(null);
                        this.<targetLocalPos>__15 = this.<camTm>__11.localPosition;
                        this.<camTm>__11.localPosition -= (Vector3) (Vector3.up * -20f);
                        this.<tt>__12.reset();
                        this.<tt>__12.translate(this.<targetLocalPos>__15, true, Easing.Function.OUT_EXPO);
                        PlayerView.Binder.RoomView.RoomCamera.TransformAnimation.stopAll();
                        PlayerView.Binder.RoomView.RoomCamera.TransformAnimation.addTask(this.<tt>__12);
                        PlayerView.Binder.DungeonHud.animateOverlay(false, this.<referenceDuration>__5, null);
                        if (this.<frenzyActive>__3)
                        {
                            PlayerView.Binder.RoomView.RoomCamera.transitionToMood(this.<frenzyMood>__4, this.<referenceDuration>__5);
                        }
                        else
                        {
                            PlayerView.Binder.RoomView.RoomCamera.transitionToMood(this.<ad>__1.Mood, this.<referenceDuration>__5);
                        }
                        this.$current = new WaitForSeconds(this.<referenceDuration>__5);
                        this.$PC = 0x15;
                        goto Label_0CB3;

                    case 0x15:
                        PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxGameplay_NextFloorArrive, (float) 0f);
                        PlayerView.Binder.RoomView.RoomCamera.setActiveCameraModeTarget(this.<pc>__2);
                        PlayerView.Binder.EffectSystem.playEffectStatic(this.<pc>__2.getGroundLevelWorldPos(), EffectType.Teleport, -1f, false, 1f, null);
                        this.$current = new WaitForSeconds(0.1f);
                        this.$PC = 0x16;
                        goto Label_0CB3;

                    case 0x16:
                        PlayerView.Binder.RoomView.getCharacterViewForCharacter(this.<pc>__2).setVisibility(true);
                        this.<pc>__2.ExternallyControlled = true;
                        this.<ie>__16 = this.<pc>__2.PhysicsBody.spinAroundRoutine(2, this.<referenceDuration>__5 * 0.5f, Easing.Function.OUT_CUBIC);
                        goto Label_0C42;

                    case 0x17:
                        goto Label_0C42;

                    default:
                        goto Label_0CB1;
                }
                PlayerView.Binder.InputSystem.setInputRequirement(InputSystem.Layer.LocationEndTransition, InputSystem.Requirement.MustBeDisabled);
                PlayerView.Binder.MenuSystem.waitAndCloseAllMenus();
            Label_034A:
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxGameplay_NextFloorVanish, (float) 0f);
                this.<spinRoutine>__10 = null;
                this.<pc>__2.ExternallyControlled = true;
                this.<spinRoutine>__10 = UnityUtils.StartCoroutine(this.<>f__this, this.<pc>__2.PhysicsBody.spinAroundRoutine(2, this.<referenceDuration>__5 * 0.5f, Easing.Function.IN_CUBIC));
                this.$current = new WaitForSeconds(this.<referenceDuration>__5 * 0.4f);
                this.$PC = 4;
                goto Label_0CB3;
            Label_044E:
                if (GameLogic.Binder.EventBus.ProcessingQueuedEvents)
                {
                    this.$current = null;
                    this.$PC = 6;
                }
                else
                {
                    GameLogic.Binder.DeathSystem.killAllNonPersistentCharacters(true, true);
                    this.$current = new WaitForSeconds(this.<referenceDuration>__5 * 0.65f);
                    this.$PC = 7;
                }
                goto Label_0CB3;
            Label_09CB:
                this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                this.$current = new WaitForSeconds(this.<referenceDuration>__5 * 0.5f);
                this.$PC = 20;
                goto Label_0CB3;
            Label_0C42:
                if (this.<ie>__16.MoveNext())
                {
                    this.$current = this.<ie>__16.Current;
                    this.$PC = 0x17;
                    goto Label_0CB3;
                }
                this.<pc>__2.ExternallyControlled = false;
                PlayerView.Binder.DungeonHud.FloorTransitionFx.gameObject.SetActive(false);
                if (!this.<showLocationEndCeremony>__9)
                {
                    GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.START_CEREMONY_STEP1, 0f), 0f);
                }
                this.<>f__this.m_criticalTransitionRoutine = null;
                goto Label_0CB1;
                this.$PC = -1;
            Label_0CB1:
                return false;
            Label_0CB3:
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
        private sealed class <frenzyActivationRoutine>c__Iterator1B9 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TransitionSystem <>f__this;
            internal DungeonMood <frenzyMood>__0;
            internal IEnumerator <ie>__1;

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
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.WAITING, 0f), 0f);
                        this.$PC = 1;
                        goto Label_018F;

                    case 1:
                        GameLogic.Binder.DeathSystem.killAllNonPersistentCharacters(false, false);
                        this.<frenzyMood>__0 = GameLogic.Binder.DungeonMoodResources.getMood("MoodFrenzy");
                        this.$current = PlayerView.Binder.RoomView.RoomCamera.transitionToMood(this.<frenzyMood>__0, 1f);
                        this.$PC = 2;
                        goto Label_018F;

                    case 2:
                        PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_BannerAnnounceBoss, (float) 0f);
                        PlayerView.Binder.DungeonHud.AnnouncementBanner.show(_.L(ConfigLoca.ANNOUNCEMENT_FRENZY_1, null, false), _.L(ConfigLoca.ANNOUNCEMENT_FRENZY_2, null, false), true, 0f, 1f, null);
                        this.<ie>__1 = TimeUtil.WaitForUnscaledSeconds(2.1f);
                        break;

                    case 3:
                        break;

                    case 4:
                        this.<>f__this.m_harmlessTransitionRoutine = null;
                        goto Label_018D;

                    default:
                        goto Label_018D;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 3;
                }
                else
                {
                    PlayerView.Binder.DungeonHud.FrenzyBar.gameObject.SetActive(true);
                    this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.ACTION, 0f), 0f);
                    this.$PC = 4;
                }
                goto Label_018F;
                this.$PC = -1;
            Label_018D:
                return false;
            Label_018F:
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
        private sealed class <frenzyDectivationRoutine>c__Iterator1BA : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TransitionSystem <>f__this;
            internal ActiveDungeon <ad>__0;

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
                    this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                    if (((this.<ad>__0 == null) || (this.<ad>__0.ActiveRoom == null)) || (this.<ad>__0.CurrentGameplayState == GameplayState.RETIREMENT))
                    {
                        this.<>f__this.m_harmlessTransitionRoutine = null;
                    }
                    else
                    {
                        PlayerView.Binder.DungeonHud.FrenzyBar.gameObject.SetActive(false);
                        PlayerView.Binder.RoomView.RoomCamera.transitionToMood(this.<ad>__0.Mood, 1f);
                        this.<>f__this.m_harmlessTransitionRoutine = null;
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

        [CompilerGenerated]
        private sealed class <gameStartRoutine>c__Iterator1B6 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TransitionSystem <>f__this;
            internal int <floor>__1;
            internal Player <player>__0;
            internal int <startAtFloor>__2;

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
                    if (App.Binder.AppContext.StartupMode == AppContext.AppStartupMode.DEFAULT)
                    {
                        if (!ConfigApp.CHEAT_SKIP_TUTORIALS && !this.<player>__0.hasCompletedTutorial("TUT003C"))
                        {
                            GameLogic.Binder.CommandProcessor.execute(new CmdStartGameplay(ConfigTutorials.TUTORIAL_DUNGEON_ID, 0, false, null), 0f);
                        }
                        else
                        {
                            this.<floor>__1 = this.<player>__0.getLastCompletedFloor(false) + 1;
                            this.<floor>__1 += this.<player>__0.getPassiveProgress(this.<floor>__1, this.<player>__0.UnclaimedPassiveRewardableSeconds, false).NumFloorCompletions;
                            this.<startAtFloor>__2 = Mathf.Clamp(this.<floor>__1, 1, 0x7fffffff);
                            GameLogic.Binder.CommandProcessor.execute(new CmdStartGameplay(ConfigDungeons.GetDungeonIdForFloor(this.<startAtFloor>__2), this.<startAtFloor>__2, false, null), 0f);
                        }
                    }
                    else if (App.Binder.AppContext.StartupMode == AppContext.AppStartupMode.DEBUG_DUNGEON)
                    {
                        GameLogic.Binder.CommandProcessor.execute(new CmdStartGameplay("Debug1", 1, false, null), 0f);
                    }
                    this.<>f__this.m_criticalTransitionRoutine = null;
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
        private sealed class <heroDeathRoutine>c__Iterator1AE : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TransitionSystem <>f__this;
            internal ActiveDungeon <ad>__0;
            internal int <floor>__1;
            internal IEnumerator <ie>__4;
            internal bool <inWildBossModeAndBossNotEscaping>__6;
            internal CharacterInstance <pc>__5;
            internal Player <player>__2;
            internal Room <room>__3;
            internal int <secondsRemaining>__7;

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
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<floor>__1 = this.<ad>__0.Floor;
                        this.<player>__2 = GameLogic.Binder.GameState.Player;
                        this.<room>__3 = this.<ad>__0.ActiveRoom;
                        GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.REVIVAL, 0f), 0f);
                        GameLogic.Binder.TimeSystem.gameplaySlowdown(true);
                        PlayerView.Binder.RoomView.RoomCamera.FovAnimator.animate(48f, 4f, 0f);
                        PlayerView.Binder.DungeonHud.openCloseSkillBar(false, ConfigUi.MENU_TRANSITION_DURATION);
                        this.<ie>__4 = TimeUtil.WaitForUnscaledSeconds(1f);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_0128;

                    case 3:
                        goto Label_028B;

                    case 4:
                        goto Label_031C;

                    case 5:
                        goto Label_03C7;

                    default:
                        goto Label_045C;
                }
                if (this.<ie>__4.MoveNext())
                {
                    this.$current = this.<ie>__4.Current;
                    this.$PC = 1;
                    goto Label_045E;
                }
            Label_0128:
                while (PlayerView.Binder.MenuSystem.InTransition)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_045E;
                }
                this.<pc>__5 = this.<player>__2.ActiveCharacter;
                this.<inWildBossModeAndBossNotEscaping>__6 = this.<ad>__0.WildBossMode && !this.<ad>__0.WildBossEscapeTriggered;
                if ((this.<room>__3.MainBossSummoned || this.<inWildBossModeAndBossNotEscaping>__6) && (!this.<ad>__0.SecondChanceUsed && (this.<pc>__5.getPerkInstanceCount(PerkType.SecondChance) > 0)))
                {
                    this.<ad>__0.SecondChanceUsed = true;
                    PlayerView.Binder.DungeonHud.showInfoCombatText(this.<pc>__5.PositionAtTimeOfDeath, StringExtensions.ToUpperLoca(_.L(ConfigLoca.COMBAT_TEXT_SECOND_CHANCE, null, false)));
                    this.<>f__this.m_criticalTransitionRoutine = null;
                    this.<>f__this.enqueueRevivePrimaryPlayerCharacter(this.<pc>__5.getGenericModifierForPerkType(PerkType.SecondChance), true);
                    goto Label_045C;
                }
                if ((!this.<room>__3.MainBossSummoned && !this.<inWildBossModeAndBossNotEscaping>__6) || (((this.<ad>__0.ActiveTournament == null) && (this.<floor>__1 < App.Binder.ConfigMeta.REVIVE_UNLOCK_FLOOR)) && !this.<player>__2.hasRetired()))
                {
                    PlayerView.Binder.DungeonHud.KnockedDownText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.DHUD_KNOCKED_DOWN, null, false));
                    PlayerView.Binder.DungeonHud.YoureDeadText.animateToBlack(2f, 0f);
                    this.<ie>__4 = TimeUtil.WaitForUnscaledSeconds(2f);
                    goto Label_031C;
                }
                this.<ie>__4 = TimeUtil.WaitForUnscaledSeconds(2f);
            Label_028B:
                while (this.<ie>__4.MoveNext())
                {
                    this.$current = this.<ie>__4.Current;
                    this.$PC = 3;
                    goto Label_045E;
                }
                PlayerView.Binder.MenuSystem.waitAndTransitionToNewMenu(MenuType.MiniPopupMenu, MenuContentType.ReviveMiniPopup, null);
                goto Label_0444;
            Label_031C:
                while (this.<ie>__4.MoveNext())
                {
                    this.$current = this.<ie>__4.Current;
                    this.$PC = 4;
                    goto Label_045E;
                }
                PlayerView.Binder.DungeonHud.YoureDeadText2Text.text = 3.ToString();
                PlayerView.Binder.DungeonHud.YoureDeadText2.animateToBlack(2f, 0f);
                this.<secondsRemaining>__7 = 3;
                while (this.<secondsRemaining>__7 > 0)
                {
                    PlayerView.Binder.AudioSystem.playSfxGrp(AudioGroupType.SfxGrpUi_ClockTick, (float) 0f);
                    PlayerView.Binder.DungeonHud.YoureDeadText2Text.text = this.<secondsRemaining>__7.ToString();
                    this.<ie>__4 = TimeUtil.WaitForUnscaledSeconds(1f);
                Label_03C7:
                    while (this.<ie>__4.MoveNext())
                    {
                        this.$current = this.<ie>__4.Current;
                        this.$PC = 5;
                        goto Label_045E;
                    }
                    this.<secondsRemaining>__7--;
                }
                PlayerView.Binder.DungeonHud.YoureDeadText2Text.text = string.Empty;
                PlayerView.Binder.DungeonHud.YoureDeadText.animateToTransparent(ConfigUi.POPUP_TRANSITION_DURATION_IN, 0f);
                PlayerView.Binder.DungeonHud.YoureDeadText2.animateToTransparent(ConfigUi.POPUP_TRANSITION_DURATION_IN, 0f);
                this.<>f__this.enqueueDungeonReload();
            Label_0444:
                this.<>f__this.m_criticalTransitionRoutine = null;
                goto Label_045C;
                this.$PC = -1;
            Label_045C:
                return false;
            Label_045E:
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
        private sealed class <retirementRoutine>c__Iterator1B2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TransitionSystem <>f__this;
            internal ActiveDungeon <ad>__1;
            internal Transform <camTm>__8;
            internal bool <frenzyActive>__4;
            internal DungeonMood <frenzyMood>__5;
            internal CharacterView <heroView>__3;
            internal IEnumerator <ie>__6;
            internal LocationEndCeremonyMenu.InputParams <ip>__13;
            internal LocationEndCeremonyMenu <lecm>__12;
            internal CharacterInstance <pc>__2;
            internal Player <player>__0;
            internal Coroutine <spinRoutine>__7;
            internal int <startFloor>__14;
            internal ManualTimer <timer>__10;
            internal TransformAnimationTask <tt>__9;
            internal float <v>__11;

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
                        this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<pc>__2 = this.<player>__0.ActiveCharacter;
                        this.<heroView>__3 = PlayerView.Binder.RoomView.getCharacterViewForCharacter(this.<pc>__2);
                        this.<frenzyActive>__4 = GameLogic.Binder.FrenzySystem.isFrenzyActive();
                        this.<frenzyMood>__5 = GameLogic.Binder.DungeonMoodResources.getMood("MoodFrenzy");
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.RETIREMENT, 0f), 0f);
                        this.$PC = 1;
                        goto Label_07BC;

                    case 1:
                        this.$current = PlayerView.Binder.MenuSystem.waitAndCloseAllMenus();
                        this.$PC = 2;
                        goto Label_07BC;

                    case 2:
                        PlayerView.Binder.DungeonHud.BossHuntTicker.gameObject.SetActive(false);
                        break;

                    case 3:
                        break;

                    case 4:
                        goto Label_0176;

                    case 5:
                        goto Label_01B7;

                    case 6:
                        goto Label_0248;

                    case 7:
                        goto Label_02AE;

                    case 8:
                        goto Label_0319;

                    case 9:
                        goto Label_0416;

                    case 10:
                        goto Label_04ED;

                    case 11:
                        goto Label_05B8;

                    case 12:
                        goto Label_05E1;

                    case 13:
                        PlayerView.Binder.DungeonHud.RetirementFx.gameObject.SetActive(false);
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdEndGameplay(GameLogic.Binder.GameState.ActiveDungeon, false), 0f);
                        this.$PC = 14;
                        goto Label_07BC;

                    case 14:
                        CmdRetire.ExecuteStatic(this.<player>__0);
                        CmdSavePlayerDataToPersistentStorage.ExecuteStatic(this.<player>__0);
                        this.<startFloor>__14 = this.<player>__0.getLastCompletedFloor(false) + 1;
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdStartGameplay(ConfigDungeons.GetDungeonIdForFloor(this.<startFloor>__14), this.<startFloor>__14, false, new CmdStartGameplay.ProgressCallback(this.<lecm>__12.onLoadProgress)), 0f);
                        this.$PC = 15;
                        goto Label_07BC;

                    case 15:
                        this.$current = new WaitForSeconds(0.4f);
                        this.$PC = 0x10;
                        goto Label_07BC;

                    case 0x10:
                        PlayerView.Binder.MenuSystem.waitAndCloseAllMenus();
                        this.$current = new WaitForSeconds(0.4f);
                        this.$PC = 0x11;
                        goto Label_07BC;

                    case 0x11:
                        this.<>f__this.m_criticalTransitionRoutine = null;
                        goto Label_07BA;

                    default:
                        goto Label_07BA;
                }
                if (PlayerView.Binder.DungeonHud.Animating)
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_07BC;
                }
                PlayerView.Binder.DungeonHud.setElementVisibility(false, false);
            Label_0176:
                while (PlayerView.Binder.DungeonHud.Animating)
                {
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_07BC;
                }
                this.<ie>__6 = TimeUtil.WaitForUnscaledSeconds(0.4f);
            Label_01B7:
                while (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 5;
                    goto Label_07BC;
                }
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxGameplay_NextFloorVanish, (float) 0f);
                this.<spinRoutine>__7 = null;
                this.<pc>__2.ExternallyControlled = true;
                this.<spinRoutine>__7 = UnityUtils.StartCoroutine(this.<>f__this, this.<pc>__2.PhysicsBody.spinAroundRoutine(2, 0.4f, Easing.Function.IN_CUBIC));
                this.<ie>__6 = TimeUtil.WaitForUnscaledSeconds(0.3f);
            Label_0248:
                while (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 6;
                    goto Label_07BC;
                }
                PlayerView.Binder.EffectSystem.playEffectStatic(this.<pc>__2.getGroundLevelWorldPos(), EffectType.Teleport, -1f, false, 1f, null);
                this.<ie>__6 = TimeUtil.WaitForUnscaledSeconds(0.1f);
            Label_02AE:
                while (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 7;
                    goto Label_07BC;
                }
                this.<pc>__2.ExternallyControlled = false;
                UnityUtils.StopCoroutine(this.<>f__this, ref this.<spinRoutine>__7);
                this.<heroView>__3.setVisibility(false);
                this.<ie>__6 = TimeUtil.WaitForUnscaledSeconds(0.5f);
            Label_0319:
                while (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 8;
                    goto Label_07BC;
                }
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxGameplay_NextFloorClouds, (float) 0f);
                this.<camTm>__8 = PlayerView.Binder.RoomView.RoomCamera.Transform;
                this.<tt>__9 = new TransformAnimationTask(this.<camTm>__8, 1.8f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                this.<tt>__9.translate(this.<camTm>__8.localPosition + ((Vector3) (Vector3.up * 25f)), true, Easing.Function.IN_CUBIC);
                PlayerView.Binder.RoomView.RoomCamera.TransformAnimation.stopAll();
                PlayerView.Binder.RoomView.RoomCamera.TransformAnimation.addTask(this.<tt>__9);
                PlayerView.Binder.DungeonHud.Vignetting.CrossFadeAlpha(0f, 0.6f, true);
                this.<ie>__6 = TimeUtil.WaitForUnscaledSeconds(0.6f);
            Label_0416:
                while (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 9;
                    goto Label_07BC;
                }
                PlayerView.Binder.DungeonHud.RetirementFx.gravityModifier = 0f;
                PlayerView.Binder.DungeonHud.RetirementFx.emissionRate = 20f;
                PlayerView.Binder.DungeonHud.RetirementFx.gameObject.SetActive(true);
                if (this.<frenzyActive>__4)
                {
                    PlayerView.Binder.DungeonHud.animateOverlay(true, 1.2f, new Color?(this.<frenzyMood>__5.BackgroundColor));
                }
                else
                {
                    PlayerView.Binder.DungeonHud.animateOverlay(true, 1.2f, new Color?(this.<ad>__1.Mood.BackgroundColor));
                }
                this.<ie>__6 = TimeUtil.WaitForUnscaledSeconds(0.4f);
            Label_04ED:
                while (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 10;
                    goto Label_07BC;
                }
                this.<timer>__10 = new ManualTimer(2.5f);
            Label_05B8:
                while (!this.<timer>__10.Idle)
                {
                    this.<v>__11 = this.<timer>__10.normalizedProgress();
                    PlayerView.Binder.DungeonHud.RetirementFx.gravityModifier = 30000f * this.<v>__11;
                    PlayerView.Binder.DungeonHud.RetirementFx.emissionRate = 20f + (40f * this.<v>__11);
                    if (!PlayerView.Binder.ScreenTransitionEffect.Animating && (this.<v>__11 >= 0.75f))
                    {
                        PlayerView.Binder.ScreenTransitionEffect.fadeToWhite(1f, 0f);
                    }
                    this.<timer>__10.tick(Time.deltaTime);
                    this.$current = null;
                    this.$PC = 11;
                    goto Label_07BC;
                }
            Label_05E1:
                while (PlayerView.Binder.ScreenTransitionEffect.Animating)
                {
                    this.$current = null;
                    this.$PC = 12;
                    goto Label_07BC;
                }
                PlayerView.Binder.DungeonHud.animateOverlay(true, 0f, new Color?(Color.white));
                PlayerView.Binder.ScreenTransitionEffect.fadeFromWhite(0f, 0f);
                this.<heroView>__3.Transform.localScale = Vector3.one;
                PlayerView.Binder.DungeonHud.Vignetting.color = Color.white;
                this.<lecm>__12 = (LocationEndCeremonyMenu) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.LocationEndCeremonyMenu);
                LocationEndCeremonyMenu.InputParams @params = new LocationEndCeremonyMenu.InputParams();
                @params.IsRetirement = true;
                this.<ip>__13 = @params;
                this.$current = PlayerView.Binder.MenuSystem.waitAndTransitionToNewMenu(MenuType.LocationEndCeremonyMenu, MenuContentType.NONE, this.<ip>__13);
                this.$PC = 13;
                goto Label_07BC;
                this.$PC = -1;
            Label_07BA:
                return false;
            Label_07BC:
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
        private sealed class <reviveRoutine>c__Iterator1B7 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>free;
            internal float <$>normalizedHpGain;
            internal TransitionSystem <>f__this;
            internal ActiveDungeon <ad>__0;
            internal CharacterInstance <c>__5;
            internal double <diamondPrice>__3;
            internal float <effectiveRadius>__6;
            internal List<CharacterInstance> <enemiesAround>__8;
            internal CharacterInstance <enemy>__10;
            internal int <i>__4;
            internal int <i>__9;
            internal IEnumerator <ie>__12;
            internal CharacterInstance <pc>__2;
            internal Player <player>__1;
            internal Vector3 <pushDirectionXz>__11;
            internal float <pushForce>__7;
            internal bool free;
            internal float normalizedHpGain;

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
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        this.<pc>__2 = this.<player>__1.ActiveCharacter;
                        PlayerView.Binder.RoomView.RoomCamera.FovAnimator.animate(PlayerView.Binder.RoomView.RoomCamera.FovAnimator.DefaultFov, 0.2f, 0f);
                        if (this.free)
                        {
                            goto Label_013F;
                        }
                        if (this.<pc>__2.Inventory.RevivePotions <= 0)
                        {
                            this.<diamondPrice>__3 = this.<ad>__0.getCurrentRevivePrice();
                            if (this.<diamondPrice>__3 > this.<player>__1.getResourceAmount(ResourceType.Diamond))
                            {
                                UnityEngine.Debug.LogWarning("Reviving without enough diamonds.");
                            }
                            CmdGainResources.ExecuteStatic(this.<player>__1, ResourceType.Diamond, -this.<diamondPrice>__3, false, "TRACKING_ID_REVIVE", null);
                            break;
                        }
                        CmdGainPotions.ExecuteStatic(this.<pc>__2, PotionType.Revive, -1);
                        this.<player>__1.TrackingData.FreeRevivesUsed++;
                        break;

                    case 1:
                        goto Label_01B2;

                    case 2:
                        goto Label_032F;

                    case 3:
                        PlayerView.Binder.DungeonHud.setElementVisibility(true, false);
                        GameLogic.Binder.TimeSystem.gameplaySlowdown(false);
                        this.<>f__this.m_criticalTransitionRoutine = null;
                        goto Label_039A;

                    default:
                        goto Label_039A;
                }
                this.<ad>__0.NumberOfPaidRevivesUsed++;
            Label_013F:
                GameLogic.Binder.SkillSystem.endSkillCooldownTimers();
                this.<i>__4 = 0;
                while (this.<i>__4 < GameLogic.Binder.GameState.PersistentCharacters.Count)
                {
                    this.<c>__5 = GameLogic.Binder.GameState.PersistentCharacters[this.<i>__4];
                    if (this.<c>__5.IsDead)
                    {
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdReviveCharacter(this.<c>__5, this.normalizedHpGain), 0f);
                        this.$PC = 1;
                        goto Label_039C;
                    }
                Label_01B2:
                    this.<i>__4++;
                }
                this.<effectiveRadius>__6 = ConfigSkills.Leap.PushAndBlastRadius * ConfigSkills.Leap.PostBlastRadius;
                this.<pushForce>__7 = ConfigSkills.Leap.PostBlastPushForce;
                this.<enemiesAround>__8 = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.getEnemyCharactersWithinRadius(this.<pc>__2.PhysicsBody.Transform.position, this.<effectiveRadius>__6, this.<pc>__2);
                this.<i>__9 = 0;
                while (this.<i>__9 < this.<enemiesAround>__8.Count)
                {
                    this.<enemy>__10 = this.<enemiesAround>__8[this.<i>__9];
                    this.<pushDirectionXz>__11 = Vector3Extensions.ToXzVector3(this.<enemy>__10.PhysicsBody.Transform.position - this.<pc>__2.PhysicsBody.Transform.position).normalized;
                    GameLogic.Binder.CommandProcessor.executeCharacterSpecific(this.<enemy>__10, new CmdPushCharacter(this.<enemy>__10, (Vector3) (this.<pushDirectionXz>__11 * this.<pushForce>__7)), 0f);
                    this.<i>__9++;
                }
                this.<ad>__0.ActiveRoom.CompletionTriggered = false;
                this.<ie>__12 = TimeUtil.WaitForUnscaledSeconds(0.333f);
            Label_032F:
                while (this.<ie>__12.MoveNext())
                {
                    this.$current = this.<ie>__12.Current;
                    this.$PC = 2;
                    goto Label_039C;
                }
                this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.ACTION, 0f), 0f);
                this.$PC = 3;
                goto Label_039C;
                this.$PC = -1;
            Label_039A:
                return false;
            Label_039C:
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
        private sealed class <roomCompletionRoutine>c__Iterator1AF : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TransitionSystem <>f__this;
            internal ActiveDungeon <ad>__1;
            internal Reward <bossReward>__7;
            internal Dictionary<Reward, bool> <bossRewards>__3;
            internal int <i>__4;
            internal IEnumerator <ie>__2;
            internal RewardCeremonyMenu.InputParameters <ip>__6;
            internal Player <player>__0;
            internal Reward <reward>__5;

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
                        this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.END_CEREMONY, 0f), 0f);
                        this.$PC = 1;
                        goto Label_03CF;

                    case 1:
                        if (this.<ad>__1.VisualizableBossRewards == null)
                        {
                            goto Label_00F2;
                        }
                        this.<ie>__2 = this.<>f__this.bossRewardVisualizationRoutine(this.<ad>__1.VisualizableBossRewards, this.<ad>__1.LastBossKillWorldPt, this.<ad>__1.Floor, false);
                        break;

                    case 2:
                        break;

                    case 3:
                        goto Label_0124;

                    case 4:
                    case 5:
                        if (PlayerView.Binder.MenuSystem.InTransition || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.RewardCeremonyMenu))
                        {
                            this.$current = null;
                            this.$PC = 5;
                            goto Label_03CF;
                        }
                        goto Label_039F;

                    default:
                        goto Label_03CD;
                }
                if (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 2;
                    goto Label_03CF;
                }
            Label_00F2:
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(0.5f);
            Label_0124:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 3;
                    goto Label_03CF;
                }
                if (((this.<ad>__1.isBossFloor() && !GameLogic.Binder.FrenzySystem.isFrenzyActive()) && (!this.<player>__0.BossTrain.Active && !this.<player>__0.autoSummonBossInFloor(this.<ad>__1.Floor))) && (this.<player>__0.getNumberOfUnclaimedBossChests() > 0))
                {
                    this.<bossRewards>__3 = new Dictionary<Reward, bool>();
                    this.<i>__4 = this.<player>__0.UnclaimedRewards.Count - 1;
                    while (this.<i>__4 >= 0)
                    {
                        this.<reward>__5 = this.<player>__0.UnclaimedRewards[this.<i>__4];
                        if (this.<player>__0.isClaimableReward(this.<reward>__5) && ConfigMeta.IsBossChest(this.<reward>__5.ChestType))
                        {
                            CmdConsumeReward.ExecuteStatic(this.<player>__0, this.<reward>__5, true, "TRACKING_ID_GAMEPLAY_LOOT_GAIN");
                            this.<bossRewards>__3.Add(this.<reward>__5, false);
                        }
                        this.<i>__4--;
                    }
                    RewardCeremonyMenu.InputParameters parameters = new RewardCeremonyMenu.InputParameters();
                    parameters.Title = StringExtensions.ToUpperLoca(_.L(ConfigUi.CeremonyEntries.BOSS_VICTORY.Title, null, false));
                    this.<ip>__6 = parameters;
                    if (this.<bossRewards>__3.Count > 1)
                    {
                        this.<ip>__6.Description = _.L(ConfigLoca.CEREMONY_DESCRIPTION_BOSS_VICTORY_MULTIPLE, null, false);
                        this.<ip>__6.MultiRewards = this.<bossRewards>__3;
                    }
                    else if (this.<bossRewards>__3.Count == 1)
                    {
                        this.<bossReward>__7 = LangUtil.GetFirstKeyFromDict<Reward, bool>(this.<bossRewards>__3);
                        this.<ip>__6.Description = MenuHelpers.GetFormattedDescriptionColored(_.L(ConfigUi.CeremonyEntries.BOSS_VICTORY.Description, null, false), "$ChestName$", _.L(ConfigUi.CHEST_BLUEPRINTS[this.<bossReward>__7.getVisualChestType()].Name, null, false));
                        this.<ip>__6.SingleReward = this.<bossReward>__7;
                        this.<ip>__6.SingleRewardOpenAtStart = false;
                    }
                    this.$current = PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.RewardCeremonyMenu, MenuContentType.NONE, this.<ip>__6, 0f, false, true);
                    this.$PC = 4;
                    goto Label_03CF;
                }
            Label_039F:
                this.<>f__this.enqueueCriticalTransition(this.<>f__this.floorTransitionRoutine());
                this.<>f__this.m_criticalTransitionRoutine = null;
                goto Label_03CD;
                this.$PC = -1;
            Label_03CD:
                return false;
            Label_03CF:
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
        private sealed class <startCeremony1Routine>c__Iterator1B0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TransitionSystem <>f__this;
            internal ActiveDungeon <ad>__0;
            internal IEnumerator <ie>__2;
            internal Player <player>__1;

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
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_0123;

                    case 3:
                        goto Label_014B;

                    case 4:
                        GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.START_CEREMONY_STEP2, 0f), 0f);
                        this.<>f__this.m_criticalTransitionRoutine = null;
                        goto Label_01C1;

                    default:
                        goto Label_01C1;
                }
                if (!PlayerView.Binder.RoomView.GameplayPrewarmingCompleted)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01C3;
                }
                if (this.<ad>__0.SeamlessTransition)
                {
                    goto Label_014B;
                }
                App.Binder.AppContext.Splash.animateToTransparent();
                PlayerView.Binder.RoomView.RoomCamera.FovAnimator.animate(PlayerView.Binder.RoomView.RoomCamera.FovAnimator.DefaultFov, ConfigUi.FADE_TO_BLACK_DURATION, 0f);
                PlayerView.Binder.DungeonHud.animateOverlay(false, ConfigUi.FADE_TO_BLACK_DURATION, null);
                PlayerView.Binder.ScreenTransitionEffect.fadeFromBlack(0f);
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(ConfigUi.FADE_TO_BLACK_DURATION);
            Label_0123:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 2;
                    goto Label_01C3;
                }
            Label_014B:
                while (PlayerView.Binder.TutorialSystem.StartCeremonyBlocking)
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_01C3;
                }
                if (this.<player>__1.hasCompletedTutorial("TUT004A"))
                {
                    PlayerView.Binder.DungeonHud.setElementVisibility(true, false);
                }
                this.$current = null;
                this.$PC = 4;
                goto Label_01C3;
                this.$PC = -1;
            Label_01C1:
                return false;
            Label_01C3:
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
        private sealed class <startCeremony2Routine>c__Iterator1B1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TransitionSystem <>f__this;
            internal ActiveDungeon <ad>__0;
            internal Player <player>__1;

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
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        if (!this.<player>__1.BossTrain.Active || !this.<ad>__0.isBossFloor())
                        {
                            if (!this.<player>__1.getLastBossEncounterFailed(false) || (this.<player>__1.getLastBossEncounterFailed(false) && !this.<>f__this.m_lastRoomFailedDuringThisSession))
                            {
                                break;
                            }
                            goto Label_00F1;
                        }
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_0126;

                    case 1:
                        GameLogic.Binder.CommandProcessor.execute(new CmdStartBossFight(Room.BossSummonMethod.BossTrain), 0f);
                        goto Label_010C;

                    case 2:
                        break;

                    default:
                        goto Label_0124;
                }
                while (PlayerView.Binder.AnnouncementSystem.PendingBlockingAnnouncements)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0126;
                }
            Label_00F1:
                GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.ACTION, 0f), 0f);
            Label_010C:
                this.<>f__this.m_criticalTransitionRoutine = null;
                goto Label_0124;
                this.$PC = -1;
            Label_0124:
                return false;
            Label_0126:
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
        private sealed class <switchAdventureRoutine>c__Iterator1B3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal string <$>locationChangeCustomText;
            internal string <$>tournamentId;
            internal TransitionSystem <>f__this;
            internal CharacterView <heroView>__2;
            internal IEnumerator <ie>__3;
            internal LocationEndCeremonyMenu.InputParams <ip>__6;
            internal LocationEndCeremonyMenu <lecm>__5;
            internal CharacterInstance <pc>__1;
            internal Player <player>__0;
            internal Coroutine <spinRoutine>__4;
            internal int <startFloor>__7;
            internal string locationChangeCustomText;
            internal string tournamentId;

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
                        this.<pc>__1 = this.<player>__0.ActiveCharacter;
                        this.<heroView>__2 = PlayerView.Binder.RoomView.getCharacterViewForCharacter(this.<pc>__1);
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.RETIREMENT, 0f), 0f);
                        this.$PC = 1;
                        goto Label_0535;

                    case 1:
                        this.$current = PlayerView.Binder.MenuSystem.waitAndCloseAllMenus();
                        this.$PC = 2;
                        goto Label_0535;

                    case 2:
                        PlayerView.Binder.DungeonHud.BossHuntTicker.gameObject.SetActive(false);
                        break;

                    case 3:
                        break;

                    case 4:
                        goto Label_0135;

                    case 5:
                        goto Label_0176;

                    case 6:
                        goto Label_0207;

                    case 7:
                        goto Label_026D;

                    case 8:
                        goto Label_02D8;

                    case 9:
                        goto Label_0330;

                    case 10:
                        PlayerView.Binder.DungeonHud.RetirementFx.gameObject.SetActive(false);
                        GameLogic.Binder.FrenzySystem.deactivateFrenzy();
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdEndGameplay(GameLogic.Binder.GameState.ActiveDungeon, false), 0f);
                        this.$PC = 11;
                        goto Label_0535;

                    case 11:
                        CmdSelectTournament.ExecuteStatic(this.<player>__0, this.tournamentId);
                        CmdSavePlayerDataToPersistentStorage.ExecuteStatic(this.<player>__0);
                        this.<startFloor>__7 = Mathf.Max(this.<player>__0.getLastCompletedFloor(false) + 1, 1);
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdStartGameplay(ConfigDungeons.GetDungeonIdForFloor(this.<startFloor>__7), this.<startFloor>__7, false, new CmdStartGameplay.ProgressCallback(this.<lecm>__5.onLoadProgress)), 0f);
                        this.$PC = 12;
                        goto Label_0535;

                    case 12:
                        GameLogic.Binder.HeroStatRecordingSystem.RealtimeCombatStats.reset();
                        this.$current = new WaitForSeconds(0.4f);
                        this.$PC = 13;
                        goto Label_0535;

                    case 13:
                        PlayerView.Binder.MenuSystem.waitAndCloseAllMenus();
                        this.$current = new WaitForSeconds(0.4f);
                        this.$PC = 14;
                        goto Label_0535;

                    case 14:
                        this.<>f__this.m_criticalTransitionRoutine = null;
                        goto Label_0533;

                    default:
                        goto Label_0533;
                }
                if (PlayerView.Binder.DungeonHud.Animating)
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0535;
                }
                PlayerView.Binder.DungeonHud.setElementVisibility(false, false);
            Label_0135:
                while (PlayerView.Binder.DungeonHud.Animating)
                {
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_0535;
                }
                this.<ie>__3 = TimeUtil.WaitForUnscaledSeconds(0.4f);
            Label_0176:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 5;
                    goto Label_0535;
                }
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxGameplay_NextFloorVanish, (float) 0f);
                this.<spinRoutine>__4 = null;
                this.<pc>__1.ExternallyControlled = true;
                this.<spinRoutine>__4 = UnityUtils.StartCoroutine(this.<>f__this, this.<pc>__1.PhysicsBody.spinAroundRoutine(2, 0.4f, Easing.Function.IN_CUBIC));
                this.<ie>__3 = TimeUtil.WaitForUnscaledSeconds(0.3f);
            Label_0207:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 6;
                    goto Label_0535;
                }
                PlayerView.Binder.EffectSystem.playEffectStatic(this.<pc>__1.getGroundLevelWorldPos(), EffectType.Teleport, -1f, false, 1f, null);
                this.<ie>__3 = TimeUtil.WaitForUnscaledSeconds(0.1f);
            Label_026D:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 7;
                    goto Label_0535;
                }
                this.<pc>__1.ExternallyControlled = false;
                UnityUtils.StopCoroutine(this.<>f__this, ref this.<spinRoutine>__4);
                this.<heroView>__2.setVisibility(false);
                this.<ie>__3 = TimeUtil.WaitForUnscaledSeconds(0.5f);
            Label_02D8:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 8;
                    goto Label_0535;
                }
                PlayerView.Binder.DungeonHud.Vignetting.CrossFadeAlpha(0f, 1f, false);
                PlayerView.Binder.ScreenTransitionEffect.fadeToWhite(1f, 0f);
            Label_0330:
                while (PlayerView.Binder.ScreenTransitionEffect.Animating)
                {
                    this.$current = null;
                    this.$PC = 9;
                    goto Label_0535;
                }
                PlayerView.Binder.DungeonHud.animateOverlay(true, 0f, new Color?(Color.white));
                PlayerView.Binder.ScreenTransitionEffect.fadeFromWhite(0f, 0f);
                this.<heroView>__2.Transform.localScale = Vector3.one;
                PlayerView.Binder.DungeonHud.Vignetting.color = Color.white;
                this.<lecm>__5 = (LocationEndCeremonyMenu) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.LocationEndCeremonyMenu);
                LocationEndCeremonyMenu.InputParams @params = new LocationEndCeremonyMenu.InputParams();
                @params.CustomText = this.locationChangeCustomText;
                this.<ip>__6 = @params;
                this.$current = PlayerView.Binder.MenuSystem.waitAndTransitionToNewMenu(MenuType.LocationEndCeremonyMenu, MenuContentType.NONE, this.<ip>__6);
                this.$PC = 10;
                goto Label_0535;
                this.$PC = -1;
            Label_0533:
                return false;
            Label_0535:
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
        private sealed class <wildBossOverRoutine>c__Iterator1AC : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>escaped;
            internal TransitionSystem <>f__this;
            internal ActiveDungeon <ad>__0;
            internal bool <frenzyActive>__1;
            internal bool <fromAction>__2;
            internal IEnumerator <ie>__3;
            internal bool escaped;

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
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<frenzyActive>__1 = GameLogic.Binder.FrenzySystem.isFrenzyActive();
                        this.<fromAction>__2 = this.<ad>__0.CurrentGameplayState == GameplayState.ACTION;
                        if (this.<fromAction>__2 && !this.<frenzyActive>__1)
                        {
                            GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.WAITING, 0f), 0f);
                        }
                        GameLogic.Binder.DeathSystem.killAllNonPersistentCharacters(false, false);
                        if (this.<ad>__0.VisualizableBossRewards == null)
                        {
                            goto Label_014A;
                        }
                        this.<ie>__3 = this.<>f__this.bossRewardVisualizationRoutine(this.<ad>__0.VisualizableBossRewards, this.<ad>__0.LastBossKillWorldPt, this.<ad>__0.Floor, true);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_014A;

                    case 3:
                        goto Label_01D7;

                    case 4:
                        GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.ACTION, 0f), 0f);
                        goto Label_0234;

                    case 5:
                        goto Label_0266;

                    default:
                        goto Label_0298;
                }
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 1;
                    goto Label_029A;
                }
                this.<ad>__0.VisualizableBossRewards = null;
                if (!this.<frenzyActive>__1)
                {
                    this.$current = new WaitForSeconds(0.5f);
                    this.$PC = 2;
                    goto Label_029A;
                }
            Label_014A:
                if (this.escaped)
                {
                    PlayerView.Binder.DungeonHud.KnockedDownText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.ANNOUNCEMENT_WILD_BOSS_ESCAPED, null, false));
                    PlayerView.Binder.DungeonHud.YoureDeadText.animateToBlack(1f, 0f);
                }
                if (this.<frenzyActive>__1)
                {
                    goto Label_01E7;
                }
                this.<ie>__3 = PlayerView.Binder.RoomView.RoomCamera.dimLightsRoutine(false, 1f);
            Label_01D7:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 3;
                    goto Label_029A;
                }
            Label_01E7:
                if (this.<fromAction>__2 && !this.<frenzyActive>__1)
                {
                    this.$current = new WaitForSeconds(0.5f);
                    this.$PC = 4;
                    goto Label_029A;
                }
            Label_0234:
                if (!this.escaped)
                {
                    goto Label_0280;
                }
                if (this.<frenzyActive>__1)
                {
                    this.$current = new WaitForSeconds(1f);
                    this.$PC = 5;
                    goto Label_029A;
                }
            Label_0266:
                PlayerView.Binder.DungeonHud.YoureDeadText.animateToTransparent(0.5f, 0f);
            Label_0280:
                this.<>f__this.m_criticalTransitionRoutine = null;
                goto Label_0298;
                this.$PC = -1;
            Label_0298:
                return false;
            Label_029A:
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
        private sealed class <wildBossSpottedRoutine>c__Iterator1AB : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>wildBoss;
            internal TransitionSystem <>f__this;
            internal ActiveDungeon <ad>__0;
            internal bool <fromAction>__1;
            internal IEnumerator <ie>__2;
            internal CharacterInstance wildBoss;

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
                        if (!GameLogic.Binder.FrenzySystem.isFrenzyActive())
                        {
                            this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                            this.<fromAction>__1 = this.<ad>__0.CurrentGameplayState == GameplayState.ACTION;
                            if (this.<fromAction>__1)
                            {
                                GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.WAITING, 0f), 0f);
                            }
                            this.<ie>__2 = PlayerView.Binder.RoomView.RoomCamera.dimLightsRoutine(true, 1f);
                            break;
                        }
                        this.<>f__this.m_criticalTransitionRoutine = null;
                        goto Label_0295;

                    case 1:
                        break;

                    case 2:
                        goto Label_0116;

                    case 3:
                        goto Label_01C4;

                    case 4:
                        goto Label_0232;

                    default:
                        goto Label_0295;
                }
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 1;
                    goto Label_0297;
                }
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(0.5f);
            Label_0116:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 2;
                    goto Label_0297;
                }
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxUi_BannerAnnounceBoss, (float) 0f);
                PlayerView.Binder.DungeonHud.AnnouncementBanner.show(!this.wildBoss.IsEliteBoss ? _.L(ConfigLoca.ANNOUNCEMENT_WILD_BOSS, null, false) : _.L(ConfigLoca.ANNOUNCEMENT_ELITE_WILD_BOSS, null, false), this.wildBoss.Name, true, 0f, 1f, null);
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(0.1f);
            Label_01C4:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 3;
                    goto Label_0297;
                }
                GameLogic.Binder.SkillSystem.endSkillCooldownTimers();
                CmdGainHp.ExecuteStatic(this.<ad>__0.PrimaryPlayerCharacter, this.<ad>__0.PrimaryPlayerCharacter.MaxLife(true), true);
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(2f);
            Label_0232:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 4;
                    goto Label_0297;
                }
                if (this.<fromAction>__1)
                {
                    GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.ACTION, 0f), 0f);
                }
                PlayerView.Binder.DungeonHud.FloorProgressionRibbon.refreshBossBar(this.wildBoss);
                this.<>f__this.m_criticalTransitionRoutine = null;
                goto Label_0295;
                this.$PC = -1;
            Label_0295:
                return false;
            Label_0297:
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
    }
}

