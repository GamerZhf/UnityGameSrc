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
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class TutorialSystem : MonoBehaviour, ITutorialSystem
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map7;
        [CompilerGenerated]
        private string <ActiveContextTutorialId>k__BackingField;
        [CompilerGenerated]
        private string <ActiveFtueTutorialId>k__BackingField;
        [CompilerGenerated]
        private bool <StartCeremonyBlocking>k__BackingField;
        private Coroutine m_activeContextTutorialRoutine;
        private Coroutine m_activeFtueFunnelRoutine;
        private int m_characterKillCounter;
        private InputCaptureState m_contextTutorialInputCapture = new InputCaptureState(InputSystem.Layer.ContextTutorial);
        private InputCaptureState m_ftueInputCapture = new InputCaptureState(InputSystem.Layer.FtueTutorial);
        private InputCaptureState[] m_inputCapturePriority;
        private bool m_itemRankUpFlag;
        private bool m_retirementFlag;
        private bool m_skillActivationFlag;
        private bool m_skillChangeFlag;

        protected void Awake()
        {
            this.m_inputCapturePriority = new InputCaptureState[] { this.m_contextTutorialInputCapture, this.m_ftueInputCapture };
        }

        [DebuggerHidden]
        private IEnumerator closeAllMenusExcept(MenuType exceptMenuType)
        {
            <closeAllMenusExcept>c__Iterator1EB iteratoreb = new <closeAllMenusExcept>c__Iterator1EB();
            iteratoreb.exceptMenuType = exceptMenuType;
            iteratoreb.<$>exceptMenuType = exceptMenuType;
            return iteratoreb;
        }

        [DebuggerHidden]
        private IEnumerator contextTutorialRoutine(string tutorialId)
        {
            <contextTutorialRoutine>c__Iterator1BD iteratorbd = new <contextTutorialRoutine>c__Iterator1BD();
            iteratorbd.tutorialId = tutorialId;
            iteratorbd.<$>tutorialId = tutorialId;
            iteratorbd.<>f__this = this;
            return iteratorbd;
        }

        [DebuggerHidden]
        private IEnumerator CTUT001()
        {
            <CTUT001>c__Iterator1E3 iteratore = new <CTUT001>c__Iterator1E3();
            iteratore.<>f__this = this;
            return iteratore;
        }

        [DebuggerHidden]
        private IEnumerator CTUT002()
        {
            <CTUT002>c__Iterator1E4 iteratore = new <CTUT002>c__Iterator1E4();
            iteratore.<>f__this = this;
            return iteratore;
        }

        [DebuggerHidden]
        private IEnumerator CTUT003()
        {
            <CTUT003>c__Iterator1E5 iteratore = new <CTUT003>c__Iterator1E5();
            iteratore.<>f__this = this;
            return iteratore;
        }

        [DebuggerHidden]
        private IEnumerator CTUT004()
        {
            <CTUT004>c__Iterator1E6 iteratore = new <CTUT004>c__Iterator1E6();
            iteratore.<>f__this = this;
            return iteratore;
        }

        [DebuggerHidden]
        private IEnumerator CTUT005()
        {
            <CTUT005>c__Iterator1E7 iteratore = new <CTUT005>c__Iterator1E7();
            iteratore.<>f__this = this;
            return iteratore;
        }

        [DebuggerHidden]
        private IEnumerator CTUT006()
        {
            <CTUT006>c__Iterator1E8 iteratore = new <CTUT006>c__Iterator1E8();
            iteratore.<>f__this = this;
            return iteratore;
        }

        [DebuggerHidden]
        private IEnumerator CTUT007()
        {
            <CTUT007>c__Iterator1E9 iteratore = new <CTUT007>c__Iterator1E9();
            iteratore.<>f__this = this;
            return iteratore;
        }

        [DebuggerHidden]
        private IEnumerator ftueFunnelRoutine1()
        {
            <ftueFunnelRoutine1>c__Iterator1BB iteratorbb = new <ftueFunnelRoutine1>c__Iterator1BB();
            iteratorbb.<>f__this = this;
            return iteratorbb;
        }

        [DebuggerHidden]
        private IEnumerator ftueFunnelRoutine2()
        {
            <ftueFunnelRoutine2>c__Iterator1BC iteratorbc = new <ftueFunnelRoutine2>c__Iterator1BC();
            iteratorbc.<>f__this = this;
            return iteratorbc;
        }

        private string GetFixedTutorialSpeechBubbleString(string input)
        {
            LocaSystem locaSystem = App.Binder.LocaSystem;
            if (locaSystem.IsRightToLeft(locaSystem.selectedLanguage))
            {
                return input.Replace("\n", string.Empty);
            }
            return input;
        }

        private InputCaptureState getInputCaptureState(InputSystem.Layer layer)
        {
            if (layer == InputSystem.Layer.ContextTutorial)
            {
                return this.m_contextTutorialInputCapture;
            }
            return this.m_ftueInputCapture;
        }

        public bool isContextTutorialActive()
        {
            return !string.IsNullOrEmpty(this.ActiveContextTutorialId);
        }

        [DebuggerHidden]
        private IEnumerator navigateToAdventurePanel(InputCaptureState inputCaptureState, SlidingAdventurePanel.ContentTarget contentTarget, int tabIdx, [Optional, DefaultParameterValue(-1)] int adventureSubTaxIdx)
        {
            <navigateToAdventurePanel>c__Iterator1F1 iteratorf = new <navigateToAdventurePanel>c__Iterator1F1();
            iteratorf.inputCaptureState = inputCaptureState;
            iteratorf.tabIdx = tabIdx;
            iteratorf.adventureSubTaxIdx = adventureSubTaxIdx;
            iteratorf.contentTarget = contentTarget;
            iteratorf.<$>inputCaptureState = inputCaptureState;
            iteratorf.<$>tabIdx = tabIdx;
            iteratorf.<$>adventureSubTaxIdx = adventureSubTaxIdx;
            iteratorf.<$>contentTarget = contentTarget;
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        [DebuggerHidden]
        private IEnumerator navigateToAdventurePanelAdventureSubTab(InputCaptureState inputCaptureState, int adventureSubTabIdx, SlidingAdventurePanel.ContentTarget contentTarget)
        {
            <navigateToAdventurePanelAdventureSubTab>c__Iterator1F3 iteratorf = new <navigateToAdventurePanelAdventureSubTab>c__Iterator1F3();
            iteratorf.inputCaptureState = inputCaptureState;
            iteratorf.adventureSubTabIdx = adventureSubTabIdx;
            iteratorf.<$>inputCaptureState = inputCaptureState;
            iteratorf.<$>adventureSubTabIdx = adventureSubTabIdx;
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        [DebuggerHidden]
        private IEnumerator navigateToAdventurePanelTab(InputCaptureState inputCaptureState, int tabIdx)
        {
            <navigateToAdventurePanelTab>c__Iterator1F2 iteratorf = new <navigateToAdventurePanelTab>c__Iterator1F2();
            iteratorf.inputCaptureState = inputCaptureState;
            iteratorf.tabIdx = tabIdx;
            iteratorf.<$>inputCaptureState = inputCaptureState;
            iteratorf.<$>tabIdx = tabIdx;
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        [DebuggerHidden]
        private IEnumerator navigateToSlidingInventoryTabIfNotAlready(InputCaptureState inputCaptureState, int tabIdx)
        {
            <navigateToSlidingInventoryTabIfNotAlready>c__Iterator1EF iteratoref = new <navigateToSlidingInventoryTabIfNotAlready>c__Iterator1EF();
            iteratoref.inputCaptureState = inputCaptureState;
            iteratoref.tabIdx = tabIdx;
            iteratoref.<$>inputCaptureState = inputCaptureState;
            iteratoref.<$>tabIdx = tabIdx;
            iteratoref.<>f__this = this;
            return iteratoref;
        }

        [DebuggerHidden]
        private IEnumerator navigateToTaskPanelSubMenu(InputCaptureState inputCaptureState, MenuContentType subMenuContent)
        {
            <navigateToTaskPanelSubMenu>c__Iterator1F0 iteratorf = new <navigateToTaskPanelSubMenu>c__Iterator1F0();
            iteratorf.inputCaptureState = inputCaptureState;
            iteratorf.subMenuContent = subMenuContent;
            iteratorf.<$>inputCaptureState = inputCaptureState;
            iteratorf.<$>subMenuContent = subMenuContent;
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        private void onCharacterKilled(CharacterInstance character, CharacterInstance killer, bool critted, SkillType fromSkill)
        {
            if (!character.IsPlayerCharacter)
            {
                this.m_characterKillCounter++;
            }
        }

        private void onCharacterSkillActivated(CharacterInstance character, SkillType skillType, float buildupTime, SkillExecutionStats executionStats)
        {
            this.m_skillActivationFlag = true;
        }

        private void onCharacterSkillsChanged(CharacterInstance character)
        {
            this.m_skillChangeFlag = true;
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnGameplayStateChangeStarted -= new GameLogic.Events.GameplayStateChangeStarted(this.onGameplayStateChangeStarted);
            GameLogic.Binder.EventBus.OnGameplayEndingStarted -= new GameLogic.Events.GameplayEndingStarted(this.onGameplayEndingStarted);
            GameLogic.Binder.EventBus.OnCharacterKilled -= new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            GameLogic.Binder.EventBus.OnCharacterSkillActivated -= new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            GameLogic.Binder.EventBus.OnCharacterSkillsChanged -= new GameLogic.Events.CharacterSkillsChanged(this.onCharacterSkillsChanged);
            GameLogic.Binder.EventBus.OnItemRankUpped -= new GameLogic.Events.ItemRankUpped(this.onItemRankUpped);
            GameLogic.Binder.EventBus.OnPlayerRetired -= new GameLogic.Events.PlayerRetired(this.onPlayerRetired);
            GameLogic.Binder.EventBus.OnTournamentSelected -= new GameLogic.Events.TournamentSelected(this.onTournamentSelected);
            GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
            base.StopAllCoroutines();
            this.m_activeFtueFunnelRoutine = null;
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnGameplayStateChangeStarted += new GameLogic.Events.GameplayStateChangeStarted(this.onGameplayStateChangeStarted);
            GameLogic.Binder.EventBus.OnGameplayEndingStarted += new GameLogic.Events.GameplayEndingStarted(this.onGameplayEndingStarted);
            GameLogic.Binder.EventBus.OnCharacterKilled += new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            GameLogic.Binder.EventBus.OnCharacterSkillActivated += new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            GameLogic.Binder.EventBus.OnCharacterSkillsChanged += new GameLogic.Events.CharacterSkillsChanged(this.onCharacterSkillsChanged);
            GameLogic.Binder.EventBus.OnItemRankUpped += new GameLogic.Events.ItemRankUpped(this.onItemRankUpped);
            GameLogic.Binder.EventBus.OnPlayerRetired += new GameLogic.Events.PlayerRetired(this.onPlayerRetired);
            GameLogic.Binder.EventBus.OnTournamentSelected += new GameLogic.Events.TournamentSelected(this.onTournamentSelected);
        }

        private void onGameplayEndingStarted(ActiveDungeon ad)
        {
            this.stopActiveFtueTutorial();
        }

        private void onGameplayStarted(ActiveDungeon ad)
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.getInputCaptureState(InputSystem.Layer.FtueTutorial).setBlocking(false);
            this.m_characterKillCounter = 0;
            this.m_skillActivationFlag = false;
            this.m_itemRankUpFlag = false;
            this.m_skillChangeFlag = false;
            this.m_retirementFlag = false;
            if (!ConfigApp.CHEAT_SKIP_TUTORIALS)
            {
                if (ad.Dungeon.Id == ConfigTutorials.TUTORIAL_DUNGEON_ID)
                {
                    this.m_activeFtueFunnelRoutine = UnityUtils.StartCoroutine(this, this.ftueFunnelRoutine1());
                }
                else if ((!player.hasCompletedAllTutorialsInCategory(ConfigTutorials.CORE_LOOP_TUTORIALS) || !player.hasCompletedAllTutorialsInCategory(ConfigTutorials.SKILLS_TUTORIALS)) || ((!player.hasCompletedAllTutorialsInCategory(ConfigTutorials.AFK_TUTORIALS) || !player.hasCompletedAllTutorialsInCategory(ConfigTutorials.ASCEND_AND_TOKENS_TUTORIALS)) || !player.hasCompletedAllTutorialsInCategory(ConfigTutorials.MISSION_AND_PET_TUTORIALS)))
                {
                    this.m_activeFtueFunnelRoutine = UnityUtils.StartCoroutine(this, this.ftueFunnelRoutine2());
                }
            }
        }

        public void onGameplayStateChangeStarted(GameplayState fromState, GameplayState targetState, float transitionDelay)
        {
            if (targetState == GameplayState.RETIREMENT)
            {
                this.m_retirementFlag = true;
            }
            else if (targetState == GameplayState.ROOM_COMPLETION)
            {
                this.stopActiveFtueTutorial();
            }
        }

        private void onItemRankUpped(CharacterInstance character, ItemInstance itemInstance, int rankUpCount, bool free)
        {
            this.m_itemRankUpFlag = true;
        }

        private void onPlayerRetired(Player player, int retirementFloor)
        {
            this.m_retirementFlag = true;
        }

        private void onTournamentSelected(Player player, TournamentInstance selectedTournament, TournamentInstance unselectedTournament)
        {
            if (((selectedTournament != null) && (this.ActiveContextTutorialId != "CTUT006")) && !player.hasCompletedTutorial("CTUT006"))
            {
                player.CompletedTutorials.Add("CTUT006");
            }
        }

        [DebuggerHidden]
        private IEnumerator openMenuUnlessAlreadyOpen(MenuType menuType, MenuContentType contentType)
        {
            <openMenuUnlessAlreadyOpen>c__Iterator1EE iteratoree = new <openMenuUnlessAlreadyOpen>c__Iterator1EE();
            iteratoree.menuType = menuType;
            iteratoree.contentType = contentType;
            iteratoree.<$>menuType = menuType;
            iteratoree.<$>contentType = contentType;
            return iteratoree;
        }

        [DebuggerHidden]
        private IEnumerator showSpeechBubbleMenu(InputCaptureState inputCaptureState, string message, [Optional, DefaultParameterValue(false)] bool keepHudClosedAfter, [Optional, DefaultParameterValue(true)] bool freezeTime)
        {
            <showSpeechBubbleMenu>c__Iterator1EA iteratorea = new <showSpeechBubbleMenu>c__Iterator1EA();
            iteratorea.inputCaptureState = inputCaptureState;
            iteratorea.freezeTime = freezeTime;
            iteratorea.message = message;
            iteratorea.keepHudClosedAfter = keepHudClosedAfter;
            iteratorea.<$>inputCaptureState = inputCaptureState;
            iteratorea.<$>freezeTime = freezeTime;
            iteratorea.<$>message = message;
            iteratorea.<$>keepHudClosedAfter = keepHudClosedAfter;
            iteratorea.<>f__this = this;
            return iteratorea;
        }

        public void startContextTutorial(string tutorialId)
        {
            if (UnityUtils.CoroutineRunning(ref this.m_activeContextTutorialRoutine))
            {
                UnityEngine.Debug.LogWarning("Active context tutorial already running, skipping: " + tutorialId);
            }
            else
            {
                this.m_activeContextTutorialRoutine = UnityUtils.StartCoroutine(this, this.contextTutorialRoutine(tutorialId));
            }
        }

        private void stopActiveFtueTutorial()
        {
            UnityUtils.StopCoroutine(this, ref this.m_activeFtueFunnelRoutine);
            GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
            this.getInputCaptureState(InputSystem.Layer.FtueTutorial).setCapture(null);
        }

        [DebuggerHidden]
        private IEnumerator TUT000A()
        {
            return new <TUT000A>c__Iterator1BE();
        }

        [DebuggerHidden]
        private IEnumerator TUT001A()
        {
            <TUT001A>c__Iterator1BF iteratorbf = new <TUT001A>c__Iterator1BF();
            iteratorbf.<>f__this = this;
            return iteratorbf;
        }

        [DebuggerHidden]
        private IEnumerator TUT001B()
        {
            <TUT001B>c__Iterator1C0 iteratorc = new <TUT001B>c__Iterator1C0();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        [DebuggerHidden]
        private IEnumerator TUT001C()
        {
            <TUT001C>c__Iterator1C1 iteratorc = new <TUT001C>c__Iterator1C1();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        [DebuggerHidden]
        private IEnumerator TUT001D()
        {
            <TUT001D>c__Iterator1C2 iteratorc = new <TUT001D>c__Iterator1C2();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        [DebuggerHidden]
        private IEnumerator TUT001E()
        {
            <TUT001E>c__Iterator1C3 iteratorc = new <TUT001E>c__Iterator1C3();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        [DebuggerHidden]
        private IEnumerator TUT001F()
        {
            <TUT001F>c__Iterator1C4 iteratorc = new <TUT001F>c__Iterator1C4();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        [DebuggerHidden]
        private IEnumerator TUT002A()
        {
            <TUT002A>c__Iterator1C5 iteratorc = new <TUT002A>c__Iterator1C5();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        [DebuggerHidden]
        private IEnumerator TUT002B()
        {
            <TUT002B>c__Iterator1C6 iteratorc = new <TUT002B>c__Iterator1C6();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        [DebuggerHidden]
        private IEnumerator TUT003A()
        {
            <TUT003A>c__Iterator1C7 iteratorc = new <TUT003A>c__Iterator1C7();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        [DebuggerHidden]
        private IEnumerator TUT003B()
        {
            <TUT003B>c__Iterator1C8 iteratorc = new <TUT003B>c__Iterator1C8();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        [DebuggerHidden]
        private IEnumerator TUT003C()
        {
            <TUT003C>c__Iterator1C9 iteratorc = new <TUT003C>c__Iterator1C9();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        [DebuggerHidden]
        private IEnumerator TUT004A()
        {
            <TUT004A>c__Iterator1CA iteratorca = new <TUT004A>c__Iterator1CA();
            iteratorca.<>f__this = this;
            return iteratorca;
        }

        [DebuggerHidden]
        private IEnumerator TUT004B()
        {
            return new <TUT004B>c__Iterator1CB();
        }

        [DebuggerHidden]
        private IEnumerator TUT050A()
        {
            <TUT050A>c__Iterator1CC iteratorcc = new <TUT050A>c__Iterator1CC();
            iteratorcc.<>f__this = this;
            return iteratorcc;
        }

        [DebuggerHidden]
        private IEnumerator TUT050B()
        {
            return new <TUT050B>c__Iterator1CD();
        }

        [DebuggerHidden]
        private IEnumerator TUT050C()
        {
            <TUT050C>c__Iterator1CE iteratorce = new <TUT050C>c__Iterator1CE();
            iteratorce.<>f__this = this;
            return iteratorce;
        }

        [DebuggerHidden]
        private IEnumerator TUT050D()
        {
            <TUT050D>c__Iterator1CF iteratorcf = new <TUT050D>c__Iterator1CF();
            iteratorcf.<>f__this = this;
            return iteratorcf;
        }

        [DebuggerHidden]
        private IEnumerator TUT051A()
        {
            <TUT051A>c__Iterator1D0 iteratord = new <TUT051A>c__Iterator1D0();
            iteratord.<>f__this = this;
            return iteratord;
        }

        [DebuggerHidden]
        private IEnumerator TUT051B()
        {
            <TUT051B>c__Iterator1D1 iteratord = new <TUT051B>c__Iterator1D1();
            iteratord.<>f__this = this;
            return iteratord;
        }

        [DebuggerHidden]
        private IEnumerator TUT051C()
        {
            <TUT051C>c__Iterator1D2 iteratord = new <TUT051C>c__Iterator1D2();
            iteratord.<>f__this = this;
            return iteratord;
        }

        [DebuggerHidden]
        private IEnumerator TUT052A()
        {
            <TUT052A>c__Iterator1D3 iteratord = new <TUT052A>c__Iterator1D3();
            iteratord.<>f__this = this;
            return iteratord;
        }

        [DebuggerHidden]
        private IEnumerator TUT052B()
        {
            <TUT052B>c__Iterator1D4 iteratord = new <TUT052B>c__Iterator1D4();
            iteratord.<>f__this = this;
            return iteratord;
        }

        [DebuggerHidden]
        private IEnumerator TUT052C()
        {
            <TUT052C>c__Iterator1D5 iteratord = new <TUT052C>c__Iterator1D5();
            iteratord.<>f__this = this;
            return iteratord;
        }

        [DebuggerHidden]
        private IEnumerator TUT052D()
        {
            <TUT052D>c__Iterator1D6 iteratord = new <TUT052D>c__Iterator1D6();
            iteratord.<>f__this = this;
            return iteratord;
        }

        [DebuggerHidden]
        private IEnumerator TUT149A()
        {
            <TUT149A>c__Iterator1D7 iteratord = new <TUT149A>c__Iterator1D7();
            iteratord.<>f__this = this;
            return iteratord;
        }

        [DebuggerHidden]
        private IEnumerator TUT150D()
        {
            return new <TUT150D>c__Iterator1D8();
        }

        [DebuggerHidden]
        private IEnumerator TUT200A()
        {
            <TUT200A>c__Iterator1D9 iteratord = new <TUT200A>c__Iterator1D9();
            iteratord.<>f__this = this;
            return iteratord;
        }

        [DebuggerHidden]
        private IEnumerator TUT200B()
        {
            return new <TUT200B>c__Iterator1DA();
        }

        [DebuggerHidden]
        private IEnumerator TUT351A()
        {
            <TUT351A>c__Iterator1DB iteratordb = new <TUT351A>c__Iterator1DB();
            iteratordb.<>f__this = this;
            return iteratordb;
        }

        [DebuggerHidden]
        private IEnumerator TUT351B()
        {
            <TUT351B>c__Iterator1DC iteratordc = new <TUT351B>c__Iterator1DC();
            iteratordc.<>f__this = this;
            return iteratordc;
        }

        [DebuggerHidden]
        private IEnumerator TUT351C()
        {
            <TUT351C>c__Iterator1DD iteratordd = new <TUT351C>c__Iterator1DD();
            iteratordd.<>f__this = this;
            return iteratordd;
        }

        [DebuggerHidden]
        private IEnumerator TUT360A()
        {
            <TUT360A>c__Iterator1DE iteratorde = new <TUT360A>c__Iterator1DE();
            iteratorde.<>f__this = this;
            return iteratorde;
        }

        [DebuggerHidden]
        private IEnumerator TUT360B()
        {
            <TUT360B>c__Iterator1DF iteratordf = new <TUT360B>c__Iterator1DF();
            iteratordf.<>f__this = this;
            return iteratordf;
        }

        [DebuggerHidden]
        private IEnumerator TUT360C()
        {
            <TUT360C>c__Iterator1E0 iteratore = new <TUT360C>c__Iterator1E0();
            iteratore.<>f__this = this;
            return iteratore;
        }

        [DebuggerHidden]
        private IEnumerator TUT451A()
        {
            <TUT451A>c__Iterator1E1 iteratore = new <TUT451A>c__Iterator1E1();
            iteratore.<>f__this = this;
            return iteratore;
        }

        [DebuggerHidden]
        private IEnumerator TUT451B()
        {
            <TUT451B>c__Iterator1E2 iteratore = new <TUT451B>c__Iterator1E2();
            iteratore.<>f__this = this;
            return iteratore;
        }

        protected void Update()
        {
            InputCaptureState state = null;
            int num = 0;
            for (int i = 0; i < this.m_inputCapturePriority.Length; i++)
            {
                if (this.m_inputCapturePriority[i].RectTm != null)
                {
                    num++;
                    if (state == null)
                    {
                        state = this.m_inputCapturePriority[i];
                    }
                }
            }
            if (state != null)
            {
                if (Input.GetMouseButtonDown(0) || ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began)))
                {
                    Vector2 screenPoint = PlayerView.Binder.InputSystem.getTouchOrMouseCurrentScreenPos();
                    if (RectTransformUtility.RectangleContainsScreenPoint(state.RectTm, screenPoint, PlayerView.Binder.MenuSystem.MenuCamera))
                    {
                        bool inputEnabled = PlayerView.Binder.InputSystem.InputEnabled;
                        PlayerView.Binder.InputSystem.InputEnabled = true;
                        PointerEventData eventData = new PointerEventData(EventSystem.current);
                        ExecuteEvents.ExecuteHierarchy<IPointerEnterHandler>(state.RectTm.gameObject, eventData, ExecuteEvents.pointerEnterHandler);
                        ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(state.RectTm.gameObject, eventData, ExecuteEvents.pointerDownHandler);
                        PlayerView.Binder.InputSystem.InputEnabled = inputEnabled;
                    }
                }
                if (Input.GetMouseButtonUp(0) || ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended)))
                {
                    PointerEventData data2 = new PointerEventData(EventSystem.current);
                    ExecuteEvents.ExecuteHierarchy<IPointerUpHandler>(state.RectTm.gameObject, data2, ExecuteEvents.pointerUpHandler);
                    Vector2 vector2 = PlayerView.Binder.InputSystem.getTouchOrMouseCurrentScreenPos();
                    if (!state.IsSubmitRegistered && RectTransformUtility.RectangleContainsScreenPoint(state.RectTm, vector2, PlayerView.Binder.MenuSystem.MenuCamera))
                    {
                        state.IsSubmitRegistered = true;
                        ExecuteEvents.ExecuteHierarchy<ISubmitHandler>(state.RectTm.gameObject, data2, ExecuteEvents.submitHandler);
                    }
                }
            }
        }

        [DebuggerHidden]
        private IEnumerator walkPlayerOutOfMenu(InputCaptureState inputCaptureState, MenuType menuType, RectTransform inputCaptureTargetTm)
        {
            <walkPlayerOutOfMenu>c__Iterator1ED iteratored = new <walkPlayerOutOfMenu>c__Iterator1ED();
            iteratored.inputCaptureState = inputCaptureState;
            iteratored.inputCaptureTargetTm = inputCaptureTargetTm;
            iteratored.menuType = menuType;
            iteratored.<$>inputCaptureState = inputCaptureState;
            iteratored.<$>inputCaptureTargetTm = inputCaptureTargetTm;
            iteratored.<$>menuType = menuType;
            return iteratored;
        }

        [DebuggerHidden]
        private IEnumerator walkPlayerToMenuUnlessAlreadyOpen(InputCaptureState inputCaptureState, MenuType menuType, MenuContentType contentType, RectTransform inputCaptureTargetTm)
        {
            <walkPlayerToMenuUnlessAlreadyOpen>c__Iterator1EC iteratorec = new <walkPlayerToMenuUnlessAlreadyOpen>c__Iterator1EC();
            iteratorec.menuType = menuType;
            iteratorec.contentType = contentType;
            iteratorec.inputCaptureState = inputCaptureState;
            iteratorec.inputCaptureTargetTm = inputCaptureTargetTm;
            iteratorec.<$>menuType = menuType;
            iteratorec.<$>contentType = contentType;
            iteratorec.<$>inputCaptureState = inputCaptureState;
            iteratorec.<$>inputCaptureTargetTm = inputCaptureTargetTm;
            return iteratorec;
        }

        public string ActiveContextTutorialId
        {
            [CompilerGenerated]
            get
            {
                return this.<ActiveContextTutorialId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ActiveContextTutorialId>k__BackingField = value;
            }
        }

        public string ActiveFtueTutorialId
        {
            [CompilerGenerated]
            get
            {
                return this.<ActiveFtueTutorialId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ActiveFtueTutorialId>k__BackingField = value;
            }
        }

        public bool StartCeremonyBlocking
        {
            [CompilerGenerated]
            get
            {
                return this.<StartCeremonyBlocking>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<StartCeremonyBlocking>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <closeAllMenusExcept>c__Iterator1EB : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuType <$>exceptMenuType;
            internal MenuType exceptMenuType;

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
                    case 1:
                        if (PlayerView.Binder.MenuSystem.InTransition)
                        {
                            this.$current = null;
                            this.$PC = 1;
                        }
                        else
                        {
                            if (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == this.exceptMenuType)
                            {
                                break;
                            }
                            this.$current = PlayerView.Binder.MenuSystem.waitAndCloseAllMenus();
                            this.$PC = 2;
                        }
                        return true;

                    case 2:
                        break;

                    default:
                        break;
                        this.$PC = -1;
                        break;
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
        private sealed class <contextTutorialRoutine>c__Iterator1BD : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal string <$>tutorialId;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__2;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal Player <player>__1;
            internal string tutorialId;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.ContextTutorial);
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        this.<ie>__2 = null;
                        string tutorialId = this.tutorialId;
                        if (tutorialId != null)
                        {
                            int num2;
                            if (TutorialSystem.<>f__switch$map7 == null)
                            {
                                Dictionary<string, int> dictionary = new Dictionary<string, int>(7);
                                dictionary.Add("CTUT001", 0);
                                dictionary.Add("CTUT002", 1);
                                dictionary.Add("CTUT003", 2);
                                dictionary.Add("CTUT004", 3);
                                dictionary.Add("CTUT005", 4);
                                dictionary.Add("CTUT006", 5);
                                dictionary.Add("CTUT007", 6);
                                TutorialSystem.<>f__switch$map7 = dictionary;
                            }
                            if (TutorialSystem.<>f__switch$map7.TryGetValue(tutorialId, out num2))
                            {
                                switch (num2)
                                {
                                    case 0:
                                        this.<ie>__2 = this.<>f__this.CTUT001();
                                        goto Label_01BB;

                                    case 1:
                                        this.<ie>__2 = this.<>f__this.CTUT002();
                                        goto Label_01BB;

                                    case 2:
                                        this.<ie>__2 = this.<>f__this.CTUT003();
                                        goto Label_01BB;

                                    case 3:
                                        this.<ie>__2 = this.<>f__this.CTUT004();
                                        goto Label_01BB;

                                    case 4:
                                        this.<ie>__2 = this.<>f__this.CTUT005();
                                        goto Label_01BB;

                                    case 5:
                                        this.<ie>__2 = this.<>f__this.CTUT006();
                                        goto Label_01BB;

                                    case 6:
                                        this.<ie>__2 = this.<>f__this.CTUT007();
                                        goto Label_01BB;
                                }
                            }
                        }
                        UnityEngine.Debug.LogError("Unsupported context tutorial id: " + this.tutorialId);
                        this.<>f__this.m_activeContextTutorialRoutine = null;
                        goto Label_0261;
                    }
                    case 1:
                        goto Label_0204;

                    default:
                        goto Label_0261;
                }
            Label_01BB:
                this.<>f__this.ActiveContextTutorialId = this.tutorialId;
                GameLogic.Binder.EventBus.TutorialStarted(this.<player>__1, this.tutorialId);
            Label_0204:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 1;
                    return true;
                }
                this.<>f__this.ActiveContextTutorialId = null;
                CmdCompleteTutorial.ExecuteStatic(this.<player>__1, this.tutorialId);
                this.<inputCaptureState>__0.setCapture(null);
                this.<inputCaptureState>__0.setBlocking(false);
                this.<>f__this.m_activeContextTutorialRoutine = null;
                goto Label_0261;
                this.$PC = -1;
            Label_0261:
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
        private sealed class <CTUT001>c__Iterator1E3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal HeroPopupContent <heroPopup>__3;
            internal HeroPopupContent <heroPopupContent>__5;
            internal IEnumerator <ie>__2;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal SpeechBubbleMenu.InputParams <ip>__6;
            internal Player <player>__1;
            internal StackedPopupMenu <popup>__4;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.ContextTutorial);
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        this.<inputCaptureState>__0.setBlocking(true);
                        GameLogic.Binder.TimeSystem.tutorialSlowdown(true);
                        this.<ie>__2 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.HERONAMING_TUTORIAL, false, true);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_0115;

                    case 3:
                        goto Label_0183;

                    case 4:
                        goto Label_0215;

                    case 5:
                        goto Label_0276;

                    case 6:
                        goto Label_02B6;

                    case 7:
                        goto Label_031A;

                    case 8:
                        goto Label_0367;

                    case 9:
                        goto Label_03F2;

                    case 10:
                    case 11:
                        while (PlayerView.Binder.MenuSystem.InTransition || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.SpeechBubble))
                        {
                            this.<inputCaptureState>__0.setBlocking(false);
                            this.$current = null;
                            this.$PC = 11;
                            goto Label_0527;
                        }
                        this.$current = PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                        this.$PC = 12;
                        goto Label_0527;

                    case 12:
                        this.<ie>__2 = this.<>f__this.navigateToTaskPanelSubMenu(this.<inputCaptureState>__0, MenuContentType.LeaderboardPopupContent);
                        goto Label_04FE;

                    case 13:
                        goto Label_04FE;

                    default:
                        goto Label_0525;
                }
                if (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 1;
                    goto Label_0527;
                }
                this.<ie>__2 = this.<>f__this.navigateToTaskPanelSubMenu(this.<inputCaptureState>__0, MenuContentType.HeroPopupContent);
            Label_0115:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 2;
                    goto Label_0527;
                }
                this.<heroPopup>__3 = PlayerView.Binder.MenuContentResources.getSharedResource(MenuContentType.HeroPopupContent).GetComponent<HeroPopupContent>();
                this.<heroPopup>__3.ScrollRect.verticalNormalizedPosition = 1f;
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(0.25f);
            Label_0183:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 3;
                    goto Label_0527;
                }
                this.<inputCaptureState>__0.setCapture(this.<heroPopup>__3.HeroRenamingButton.GetComponent<RectTransform>());
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_0215:
                while (PlayerView.Binder.MenuSystem.InTransition || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() != MenuType.ThinPopupMenu))
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.<inputCaptureState>__0.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_0527;
                }
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(0.4f);
            Label_0276:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 5;
                    goto Label_0527;
                }
                this.<inputCaptureState>__0.setCapture(null);
            Label_02B6:
                while (PlayerView.Binder.MenuSystem.InTransition || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.ThinPopupMenu))
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    this.$current = null;
                    this.$PC = 6;
                    goto Label_0527;
                }
                if (!this.<player>__1.SocialData.HasGivenCustomName)
                {
                    CmdRenamePlayer.ExecuteStatic(this.<player>__1, _.L(ConfigLoca.HERO_KNIGHT, null, false));
                }
            Label_031A:
                while (PlayerView.Binder.MenuSystem.InTransition || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() != MenuType.StackedPopupMenu))
                {
                    this.$current = null;
                    this.$PC = 7;
                    goto Label_0527;
                }
                this.<popup>__4 = (StackedPopupMenu) PlayerView.Binder.MenuSystem.topmostActiveMenu();
            Label_0367:
                while (PlayerView.Binder.MenuSystem.InTransition || (this.<popup>__4.activeContentType() != MenuContentType.HeroPopupContent))
                {
                    this.$current = null;
                    this.$PC = 8;
                    goto Label_0527;
                }
                this.<heroPopupContent>__5 = (HeroPopupContent) this.<popup>__4.activeContentObject();
                this.<heroPopupContent>__5.ScrollRect.verticalNormalizedPosition = 1f;
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(0.2f);
            Label_03F2:
                while (this.<ie>__2.MoveNext())
                {
                    this.<inputCaptureState>__0.setBlocking(true);
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 9;
                    goto Label_0527;
                }
                SpeechBubbleMenu.InputParams @params = new SpeechBubbleMenu.InputParams();
                @params.Message = this.<>f__this.GetFixedTutorialSpeechBubbleString(_.L(ConfigLoca.HERONAMING_TUTORIAL2, null, false));
                this.<ip>__6 = @params;
                this.$current = PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.SpeechBubble, MenuContentType.NONE, this.<ip>__6, 0f, false, true);
                this.$PC = 10;
                goto Label_0527;
            Label_04FE:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 13;
                    goto Label_0527;
                }
                GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
                goto Label_0525;
                this.$PC = -1;
            Label_0525:
                return false;
            Label_0527:
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
        private sealed class <CTUT002>c__Iterator1E4 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal Button <firstSkillButton>__6;
            internal IEnumerator <ie>__2;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal Player <player>__1;
            internal SkillPopupContent <skillPopupContent>__5;
            internal StackedPopupMenu <stackPopup>__4;
            internal Menu <topmostMenu>__3;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.ContextTutorial);
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        if (this.<player>__1.UnlockedSkills.Contains(SkillType.Clone) && !this.<player>__1.ActiveCharacter.isSkillActive(SkillType.Clone))
                        {
                            this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(0.7f);
                            break;
                        }
                        goto Label_04A0;

                    case 1:
                        break;

                    case 2:
                        goto Label_011D;

                    case 3:
                        this.<inputCaptureState>__0.setCapture(PlayerView.Binder.DungeonHud.SkillsButton.Tm);
                        PlayerView.Binder.DungeonHud.TutorialCircle.Play();
                        goto Label_023C;

                    case 4:
                        goto Label_023C;

                    case 5:
                        goto Label_0298;

                    case 6:
                        goto Label_0364;

                    case 7:
                        goto Label_03C1;

                    case 8:
                        goto Label_041C;

                    case 9:
                        goto Label_0479;

                    default:
                        goto Label_04A0;
                }
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 1;
                    goto Label_04A2;
                }
                this.<inputCaptureState>__0.setBlocking(true);
                GameLogic.Binder.TimeSystem.tutorialSlowdown(true);
                PlayerView.Binder.DungeonHud.applyTutorialRestrictions();
                PlayerView.Binder.DungeonHud.setElementVisibility(true, false);
            Label_011D:
                while (PlayerView.Binder.DungeonHud.Animating)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_04A2;
                }
                this.<>f__this.m_skillChangeFlag = false;
                this.<topmostMenu>__3 = PlayerView.Binder.MenuSystem.topmostActiveMenu();
                this.<stackPopup>__4 = null;
                if (((this.<topmostMenu>__3 != null) && (this.<topmostMenu>__3.MenuType == MenuType.StackedPopupMenu)) && (((StackedPopupMenu) this.<topmostMenu>__3).activeContentType() == MenuContentType.SkillPopupContent))
                {
                    this.<stackPopup>__4 = (StackedPopupMenu) this.<topmostMenu>__3;
                    goto Label_02B9;
                }
                this.$current = PlayerView.Binder.MenuSystem.waitAndCloseAllMenus();
                this.$PC = 3;
                goto Label_04A2;
            Label_023C:
                if (PlayerView.Binder.MenuSystem.InTransition || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() != MenuType.StackedPopupMenu))
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.<inputCaptureState>__0.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_04A2;
                }
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                this.<stackPopup>__4 = (StackedPopupMenu) PlayerView.Binder.MenuSystem.topmostActiveMenu();
            Label_0298:
                while (PlayerView.Binder.MenuSystem.InTransition || (this.<stackPopup>__4.activeContentType() != MenuContentType.SkillPopupContent))
                {
                    this.$current = null;
                    this.$PC = 5;
                    goto Label_04A2;
                }
            Label_02B9:
                this.<skillPopupContent>__5 = (SkillPopupContent) this.<stackPopup>__4.activeContentObject();
                this.<firstSkillButton>__6 = this.<skillPopupContent>__5.getSkillCell(SkillType.Clone).Button;
                this.<inputCaptureState>__0.setCapture(this.<firstSkillButton>__6.GetComponent<RectTransform>());
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_0364:
                while (!this.<>f__this.m_skillChangeFlag)
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.<inputCaptureState>__0.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 6;
                    goto Label_04A2;
                }
                this.<>f__this.m_skillChangeFlag = false;
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(0.3f);
            Label_03C1:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 7;
                    goto Label_04A2;
                }
                this.<ie>__2 = this.<>f__this.walkPlayerOutOfMenu(this.<inputCaptureState>__0, MenuType.StackedPopupMenu, this.<stackPopup>__4.TitleCloseButton.GetComponent<RectTransform>());
            Label_041C:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 8;
                    goto Label_04A2;
                }
                this.<inputCaptureState>__0.setCapture(null);
                this.<ie>__2 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.CTUT002, false, true);
            Label_0479:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 9;
                    goto Label_04A2;
                }
                GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
                goto Label_04A0;
                this.$PC = -1;
            Label_04A0:
                return false;
            Label_04A2:
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
        private sealed class <CTUT003>c__Iterator1E5 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal double <coinCost>__5;
            internal ItemCell <firstItemCell>__8;
            internal IEnumerator <ie>__6;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal CharacterInstance <pc>__2;
            internal Player <player>__1;
            internal SlidingInventoryMenu <sm>__7;
            internal int <targetRank>__4;
            internal ItemInstance <weaponInstance>__3;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.ContextTutorial);
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        this.<pc>__2 = this.<player>__1.ActiveCharacter;
                        this.<weaponInstance>__3 = this.<pc>__2.getEquippedItemOfType(ItemType.Weapon);
                        this.<targetRank>__4 = this.<weaponInstance>__3.Rank + 1;
                        this.<coinCost>__5 = this.<pc>__2.getAdjustedItemUpgradeCost(this.<weaponInstance>__3.Item.Type, this.<player>__1.getRiggedItemLevel(this.<weaponInstance>__3), this.<targetRank>__4);
                        if (this.<player>__1.getResourceAmount(ResourceType.Coin) >= this.<coinCost>__5)
                        {
                            this.<inputCaptureState>__0.setBlocking(true);
                            GameLogic.Binder.TimeSystem.tutorialSlowdown(true);
                            this.<ie>__6 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.CTUT003, false, true);
                            break;
                        }
                        UnityEngine.Debug.LogError(string.Concat(new object[] { "Trying to run CTUT003 without enough coins to upgrade equipped weapon: ", this.<coinCost>__5, " - ", this.<player>__1.getResourceAmount(ResourceType.Coin) }));
                        goto Label_033C;

                    case 1:
                        break;

                    case 2:
                        goto Label_01DF;

                    case 3:
                        goto Label_0229;

                    case 4:
                        goto Label_02EE;

                    default:
                        goto Label_033C;
                }
                while (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 1;
                    goto Label_033E;
                }
                this.<>f__this.m_itemRankUpFlag = false;
                this.<ie>__6 = this.<>f__this.walkPlayerToMenuUnlessAlreadyOpen(this.<inputCaptureState>__0, MenuType.SlidingInventoryMenu, MenuContentType.NONE, PlayerView.Binder.DungeonHud.SkillHudButtons[3].RectTm);
            Label_01DF:
                while (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 2;
                    goto Label_033E;
                }
                this.<ie>__6 = this.<>f__this.navigateToSlidingInventoryTabIfNotAlready(this.<inputCaptureState>__0, 0);
            Label_0229:
                while (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 3;
                    goto Label_033E;
                }
                this.<sm>__7 = (SlidingInventoryMenu) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.SlidingInventoryMenu);
                this.<firstItemCell>__8 = this.<sm>__7.ItemCells[0];
                this.<inputCaptureState>__0.setCapture(this.<firstItemCell>__8.CellButton.ButtonScript.GetComponent<RectTransform>());
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_02EE:
                while (!this.<>f__this.m_itemRankUpFlag)
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.<inputCaptureState>__0.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_033E;
                }
                this.<>f__this.m_itemRankUpFlag = false;
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                this.<inputCaptureState>__0.setCapture(null);
                GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
                goto Label_033C;
                this.$PC = -1;
            Label_033C:
                return false;
            Label_033E:
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
        private sealed class <CTUT004>c__Iterator1E6 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal ActiveDungeon <ad>__1;
            internal IEnumerator <ie>__2;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.ContextTutorial);
                        this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                        if ((this.<ad>__1.CurrentGameplayState == GameplayState.ACTION) && PlayerView.Binder.DungeonHud.FloorProgressionRibbon.ChallengeButtonRoot.activeInHierarchy)
                        {
                            this.<inputCaptureState>__0.setBlocking(true);
                            GameLogic.Binder.TimeSystem.tutorialSlowdown(true);
                            this.<ie>__2 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.CTUT004, false, true);
                            break;
                        }
                        UnityEngine.Debug.LogError("Trying to run CTUT004 without capability of summoning boss");
                        goto Label_01CD;

                    case 1:
                        break;

                    case 2:
                        goto Label_017E;

                    default:
                        goto Label_01CD;
                }
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 1;
                    goto Label_01CF;
                }
                this.<inputCaptureState>__0.setCapture(PlayerView.Binder.DungeonHud.FloorProgressionRibbon.ChallengeButtonRoot.GetComponent<Button>().GetComponent<RectTransform>());
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_017E:
                while (this.<ad>__1.CurrentGameplayState != GameplayState.BOSS_START)
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.<inputCaptureState>__0.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01CF;
                }
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                this.<inputCaptureState>__0.setCapture(null);
                this.<inputCaptureState>__0.setBlocking(true);
                GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
                goto Label_01CD;
                this.$PC = -1;
            Label_01CD:
                return false;
            Label_01CF:
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
        private sealed class <CTUT005>c__Iterator1E7 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal SlidingInventoryMenu <slidinInventory>__2;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.ContextTutorial);
                        this.<inputCaptureState>__0.setBlocking(true);
                        GameLogic.Binder.TimeSystem.tutorialSlowdown(true);
                        this.<ie>__1 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.CTUT005, false, true);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00DC;

                    case 3:
                        goto Label_0181;

                    default:
                        goto Label_01C2;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    goto Label_01C4;
                }
                this.<ie>__1 = this.<>f__this.navigateToSlidingInventoryTabIfNotAlready(this.<inputCaptureState>__0, 3);
            Label_00DC:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 2;
                    goto Label_01C4;
                }
                this.<slidinInventory>__2 = (SlidingInventoryMenu) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.SlidingInventoryMenu);
                this.<inputCaptureState>__0.setCapture(this.<slidinInventory>__2.getPotionCellButtonTm(PotionType.Frenzy));
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_0181:
                while (!GameLogic.Binder.FrenzySystem.isFrenzyActive())
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.<inputCaptureState>__0.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_01C4;
                }
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                this.<inputCaptureState>__0.setCapture(null);
                GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
                goto Label_01C2;
                this.$PC = -1;
            Label_01C2:
                return false;
            Label_01C4:
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
        private sealed class <CTUT006>c__Iterator1E8 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.ContextTutorial);
                        this.<inputCaptureState>__0.setBlocking(true);
                        GameLogic.Binder.TimeSystem.tutorialSlowdown(true);
                        this.<ie>__1 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.CTUT006, false, true);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00E5;

                    default:
                        goto Label_0118;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    goto Label_011A;
                }
                PlayerView.Binder.NotificationSystem.NotifyAdventurePanelTournamentTab = true;
                this.<ie>__1 = this.<>f__this.navigateToAdventurePanel(this.<inputCaptureState>__0, SlidingAdventurePanel.ContentTarget.ConfirmationButton, 1, -1);
            Label_00E5:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 2;
                    goto Label_011A;
                }
                this.<inputCaptureState>__0.setBlocking(false);
                GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
                goto Label_0118;
                this.$PC = -1;
            Label_0118:
                return false;
            Label_011A:
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
        private sealed class <CTUT007>c__Iterator1E9 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.ContextTutorial);
                        this.<inputCaptureState>__0.setBlocking(true);
                        GameLogic.Binder.TimeSystem.tutorialSlowdown(true);
                        this.<ie>__1 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.CTUT007, false, true);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00C8;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    return true;
                }
                this.<inputCaptureState>__0.setBlocking(false);
                GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
                PlayerView.Binder.NotificationSystem.NotifyAdventurePanelTournamentTab = true;
                goto Label_00C8;
                this.$PC = -1;
            Label_00C8:
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
        private sealed class <ftueFunnelRoutine1>c__Iterator1BB : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal ActiveDungeon <ad>__1;
            internal int <i>__10;
            internal int <i>__4;
            internal int <i>__8;
            internal IEnumerator <ie>__6;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal KeyValuePair<string, TutorialSystem.TutorialRoutine> <kv>__9;
            internal CharacterInstance <pc>__3;
            internal Player <player>__2;
            internal Vector3 <toCamDirXz>__5;
            internal List<KeyValuePair<string, TutorialSystem.TutorialRoutine>> <tutorialRoutines>__7;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<player>__2 = GameLogic.Binder.GameState.Player;
                        this.<pc>__3 = this.<ad>__1.PrimaryPlayerCharacter;
                        PlayerView.Binder.DungeonHud.openCloseSkillBar(false, 0f);
                        PlayerView.Binder.DungeonHud.openCloseTopPanel(false, 0f);
                        this.<i>__4 = 0;
                        while (this.<i>__4 < PlayerView.Binder.DungeonHud.SkillHudButtons.Count)
                        {
                            PlayerView.Binder.DungeonHud.setSkillHudButtonSoftLock(this.<i>__4, true);
                            this.<i>__4++;
                        }
                        GameLogic.Binder.SkillSystem.endSkillCooldownTimers();
                        PlayerView.Binder.DungeonHud.FloorProgressionRibbon.BossSummoningLocked = true;
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.WAITING, 0f), 0f);
                        this.$PC = 1;
                        goto Label_05F0;

                    case 1:
                        this.<pc>__3.ExternallyControlled = true;
                        this.<toCamDirXz>__5 = Vector3Extensions.ToXzVector3(PlayerView.Binder.RoomView.RoomCamera.Transform.position - this.<pc>__3.PhysicsBody.Transform.position).normalized;
                        this.<toCamDirXz>__5 = (Vector3) (Quaternion.Euler(0f, UnityEngine.Random.Range((float) -30f, (float) 30f), 0f) * this.<toCamDirXz>__5);
                        CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.<pc>__3, Vector3.zero, this.<toCamDirXz>__5);
                        this.$current = null;
                        this.$PC = 2;
                        goto Label_05F0;

                    case 2:
                        this.<pc>__3.ExternallyControlled = false;
                        PlayerView.Binder.DungeonHud.animateOverlay(false, ConfigUi.FADE_TO_BLACK_DURATION, null);
                        App.Binder.AppContext.Splash.animateToTransparent();
                        PlayerView.Binder.ScreenTransitionEffect.fadeFromBlack(0f);
                        this.<ie>__6 = TimeUtil.WaitForUnscaledSeconds(ConfigUi.FADE_TO_BLACK_DURATION * 2f);
                        break;

                    case 3:
                        break;

                    case 4:
                    case 5:
                        goto Label_0478;

                    case 6:
                        goto Label_055F;

                    case 7:
                        this.<>f__this.m_activeFtueFunnelRoutine = null;
                        this.$PC = -1;
                        goto Label_05EE;

                    default:
                        goto Label_05EE;
                }
                if (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 3;
                    goto Label_05F0;
                }
                List<KeyValuePair<string, TutorialSystem.TutorialRoutine>> list = new List<KeyValuePair<string, TutorialSystem.TutorialRoutine>>();
                list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT000A", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT000A)));
                list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT001A", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT001A)));
                list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT001B", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT001B)));
                list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT001C", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT001C)));
                list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT001D", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT001D)));
                list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT001E", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT001E)));
                list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT001F", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT001F)));
                list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT002A", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT002A)));
                list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT002B", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT002B)));
                list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT003A", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT003A)));
                list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT003B", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT003B)));
                list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT003C", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT003C)));
                this.<tutorialRoutines>__7 = list;
                this.<i>__8 = 0;
                while (this.<i>__8 < this.<tutorialRoutines>__7.Count)
                {
                    this.<kv>__9 = this.<tutorialRoutines>__7[this.<i>__8];
                    if (!this.<player>__2.hasCompletedTutorial(this.<kv>__9.Key))
                    {
                        GameLogic.Binder.EventBus.TutorialStarted(this.<player>__2, this.<kv>__9.Key);
                    }
                    this.<>f__this.ActiveFtueTutorialId = this.<kv>__9.Key;
                    this.<ie>__6 = this.<kv>__9.Value();
                Label_0478:
                    if (GameLogic.Binder.TimeSystem.paused())
                    {
                        this.$current = null;
                        this.$PC = 4;
                        goto Label_05F0;
                    }
                    if (this.<ie>__6.MoveNext())
                    {
                        this.$current = this.<ie>__6.Current;
                        this.$PC = 5;
                        goto Label_05F0;
                    }
                    this.<inputCaptureState>__0.setCapture(null);
                    CmdCompleteTutorial.ExecuteStatic(this.<player>__2, this.<kv>__9.Key);
                    this.<i>__8++;
                }
                this.<inputCaptureState>__0.setBlocking(false);
                this.<ie>__6 = TimeUtil.WaitForUnscaledSeconds(0.25f);
            Label_055F:
                while (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 6;
                    goto Label_05F0;
                }
                this.<i>__10 = 0;
                while (this.<i>__10 < PlayerView.Binder.DungeonHud.SkillHudButtons.Count)
                {
                    PlayerView.Binder.DungeonHud.setSkillHudButtonSoftLock(this.<i>__10, false);
                    this.<i>__10++;
                }
                this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdCompleteRoom(RoomEndCondition.NORMAL_COMPLETION), 0f);
                this.$PC = 7;
                goto Label_05F0;
            Label_05EE:
                return false;
            Label_05F0:
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
        private sealed class <ftueFunnelRoutine2>c__Iterator1BC : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal ActiveDungeon <ad>__1;
            internal int <i>__4;
            internal IEnumerator <ie>__6;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal KeyValuePair<string, TutorialSystem.TutorialRoutine> <kv>__5;
            internal Player <player>__2;
            internal List<KeyValuePair<string, TutorialSystem.TutorialRoutine>> <tutorialRoutines>__3;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<player>__2 = GameLogic.Binder.GameState.Player;
                        List<KeyValuePair<string, TutorialSystem.TutorialRoutine>> list = new List<KeyValuePair<string, TutorialSystem.TutorialRoutine>>();
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT004A", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT004A)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT004B", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT004B)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT050A", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT050A)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT050B", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT050B)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT050C", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT050C)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT050D", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT050D)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT051A", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT051A)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT051B", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT051B)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT051C", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT051C)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT052A", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT052A)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT052B", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT052B)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT052C", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT052C)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT052D", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT052D)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT149A", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT149A)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT150D", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT150D)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT200A", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT200A)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT200B", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT200B)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT351A", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT351A)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT351B", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT351B)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT351C", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT351C)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT360A", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT360A)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT360B", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT360B)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT360C", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT360C)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT451A", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT451A)));
                        list.Add(new KeyValuePair<string, TutorialSystem.TutorialRoutine>("TUT451B", new TutorialSystem.TutorialRoutine(this.<>f__this.TUT451B)));
                        this.<tutorialRoutines>__3 = list;
                        this.<>f__this.StartCeremonyBlocking = true;
                        break;
                    }
                    case 1:
                        break;

                    case 2:
                        goto Label_040E;

                    case 3:
                        this.<kv>__5 = this.<tutorialRoutines>__3[this.<i>__4];
                        if (this.<player>__2.hasCompletedTutorial(this.<kv>__5.Key))
                        {
                            goto Label_054B;
                        }
                        GameLogic.Binder.EventBus.TutorialStarted(this.<player>__2, this.<kv>__5.Key);
                        this.<>f__this.ActiveFtueTutorialId = this.<kv>__5.Key;
                        this.<ie>__6 = this.<kv>__5.Value();
                        goto Label_04C6;

                    case 4:
                    case 5:
                        goto Label_04C6;

                    default:
                        goto Label_0593;
                }
                if (this.<ad>__1.CurrentGameplayState != GameplayState.START_CEREMONY_STEP1)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_0595;
                }
                this.<>f__this.StartCeremonyBlocking = false;
                this.<i>__4 = 0;
                goto Label_0559;
            Label_040E:
                if (!string.IsNullOrEmpty(this.<>f__this.ActiveContextTutorialId))
                {
                    this.$current = null;
                    this.$PC = 2;
                }
                else
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    this.$current = PlayerView.Binder.MenuSystem.waitForMenuToBeClosed(MenuType.RewardCeremonyMenu);
                    this.$PC = 3;
                }
                goto Label_0595;
            Label_04C6:
                if (GameLogic.Binder.TimeSystem.paused())
                {
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_0595;
                }
                if (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 5;
                    goto Label_0595;
                }
                this.<inputCaptureState>__0.setCapture(null);
                CmdCompleteTutorial.ExecuteStatic(this.<player>__2, this.<kv>__5.Key);
            Label_054B:
                this.<i>__4++;
            Label_0559:
                if (this.<i>__4 < this.<tutorialRoutines>__3.Count)
                {
                    goto Label_040E;
                }
                this.<inputCaptureState>__0.setBlocking(false);
                this.<>f__this.m_activeFtueFunnelRoutine = null;
                goto Label_0593;
                this.$PC = -1;
            Label_0593:
                return false;
            Label_0595:
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
        private sealed class <navigateToAdventurePanel>c__Iterator1F1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal int <$>adventureSubTaxIdx;
            internal SlidingAdventurePanel.ContentTarget <$>contentTarget;
            internal TutorialSystem.InputCaptureState <$>inputCaptureState;
            internal int <$>tabIdx;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal IEnumerator <ie>__2;
            internal IEnumerator <ie>__3;
            internal IEnumerator <ie>__4;
            internal SlidingAdventurePanel <sm>__0;
            internal int adventureSubTaxIdx;
            internal SlidingAdventurePanel.ContentTarget contentTarget;
            internal TutorialSystem.InputCaptureState inputCaptureState;
            internal int tabIdx;

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
                        this.<sm>__0 = (SlidingAdventurePanel) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.SlidingAdventurePanel);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00CF;

                    case 3:
                        goto Label_0134;

                    case 4:
                        goto Label_0190;

                    case 5:
                        goto Label_0228;

                    case 6:
                        goto Label_027A;

                    default:
                        goto Label_02A2;
                }
                if (PlayerView.Binder.MenuSystem.InTransition)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_02A4;
                }
                if (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.SlidingAdventurePanel)
                {
                    goto Label_00DF;
                }
                this.<ie>__1 = this.<>f__this.walkPlayerToMenuUnlessAlreadyOpen(this.inputCaptureState, MenuType.SlidingAdventurePanel, MenuContentType.NONE, PlayerView.Binder.DungeonHud.AdventureButton.Tm);
            Label_00CF:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 2;
                    goto Label_02A4;
                }
            Label_00DF:
                if (this.<sm>__0.getActiveTabIndex() == this.tabIdx)
                {
                    goto Label_0144;
                }
                this.<ie>__2 = this.<>f__this.navigateToAdventurePanelTab(this.inputCaptureState, this.tabIdx);
            Label_0134:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 3;
                    goto Label_02A4;
                }
            Label_0144:
                if (this.adventureSubTaxIdx == -1)
                {
                    goto Label_01A0;
                }
                this.<ie>__3 = this.<>f__this.navigateToAdventurePanelAdventureSubTab(this.inputCaptureState, 2, this.contentTarget);
            Label_0190:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 4;
                    goto Label_02A4;
                }
            Label_01A0:
                this.inputCaptureState.setCapture(this.<sm>__0.getContentTargetButton(this.contentTarget).GetComponent<RectTransform>());
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_0228:
                while (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.SlidingAdventurePanel)
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.inputCaptureState.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 5;
                    goto Label_02A4;
                }
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                this.<ie>__4 = TimeUtil.WaitForUnscaledSeconds(0.1f);
            Label_027A:
                while (this.<ie>__4.MoveNext())
                {
                    this.$current = this.<ie>__4.Current;
                    this.$PC = 6;
                    goto Label_02A4;
                }
                this.inputCaptureState.setCapture(null);
                goto Label_02A2;
                this.$PC = -1;
            Label_02A2:
                return false;
            Label_02A4:
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
        private sealed class <navigateToAdventurePanelAdventureSubTab>c__Iterator1F3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal int <$>adventureSubTabIdx;
            internal TutorialSystem.InputCaptureState <$>inputCaptureState;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal IEnumerator <ie>__2;
            internal SlidingAdventurePanel <sm>__0;
            internal int adventureSubTabIdx;
            internal TutorialSystem.InputCaptureState inputCaptureState;

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
                        this.<sm>__0 = (SlidingAdventurePanel) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.SlidingAdventurePanel);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00C7;

                    case 3:
                        goto Label_0169;

                    case 4:
                        goto Label_01C0;

                    default:
                        goto Label_01E8;
                }
                if (PlayerView.Binder.MenuSystem.InTransition)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01EA;
                }
                if (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.SlidingAdventurePanel)
                {
                    goto Label_00D7;
                }
                this.<ie>__1 = this.<>f__this.walkPlayerToMenuUnlessAlreadyOpen(this.inputCaptureState, MenuType.SlidingAdventurePanel, MenuContentType.NONE, PlayerView.Binder.DungeonHud.AdventureButton.Tm);
            Label_00C7:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 2;
                    goto Label_01EA;
                }
            Label_00D7:
                this.inputCaptureState.setCapture(this.<sm>__0.AdventureSubTabButtons[this.adventureSubTabIdx].Button.GetComponent<RectTransform>());
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_0169:
                while (this.<sm>__0.getActiveAdventureSubTabIndex() != this.adventureSubTabIdx)
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.inputCaptureState.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_01EA;
                }
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(0.1f);
            Label_01C0:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 4;
                    goto Label_01EA;
                }
                this.inputCaptureState.setCapture(null);
                goto Label_01E8;
                this.$PC = -1;
            Label_01E8:
                return false;
            Label_01EA:
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
        private sealed class <navigateToAdventurePanelTab>c__Iterator1F2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem.InputCaptureState <$>inputCaptureState;
            internal int <$>tabIdx;
            internal TutorialSystem <>f__this;
            internal SlidingAdventurePanel.ContentTarget <contentTarget>__2;
            internal IEnumerator <ie>__1;
            internal IEnumerator <ie>__3;
            internal SlidingAdventurePanel <sm>__0;
            internal TutorialSystem.InputCaptureState inputCaptureState;
            internal int tabIdx;

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
                        this.<sm>__0 = (SlidingAdventurePanel) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.SlidingAdventurePanel);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00C7;

                    case 3:
                        goto Label_0195;

                    case 4:
                        goto Label_01EC;

                    default:
                        goto Label_0214;
                }
                if (PlayerView.Binder.MenuSystem.InTransition)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_0216;
                }
                if (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.SlidingAdventurePanel)
                {
                    goto Label_00D7;
                }
                this.<ie>__1 = this.<>f__this.walkPlayerToMenuUnlessAlreadyOpen(this.inputCaptureState, MenuType.SlidingAdventurePanel, MenuContentType.NONE, PlayerView.Binder.DungeonHud.AdventureButton.Tm);
            Label_00C7:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 2;
                    goto Label_0216;
                }
            Label_00D7:
                this.<contentTarget>__2 = SlidingAdventurePanel.ContentTarget.UNSPECIFIED;
                if (this.tabIdx == 0)
                {
                    this.<contentTarget>__2 = SlidingAdventurePanel.ContentTarget.AdventureTabButton;
                }
                else if (this.tabIdx == 1)
                {
                    this.<contentTarget>__2 = SlidingAdventurePanel.ContentTarget.TournamentTabButton;
                }
                this.inputCaptureState.setCapture(this.<sm>__0.getContentTargetButton(this.<contentTarget>__2).GetComponent<RectTransform>());
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_0195:
                while (this.<sm>__0.getActiveTabIndex() != this.tabIdx)
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.inputCaptureState.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0216;
                }
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                this.<ie>__3 = TimeUtil.WaitForUnscaledSeconds(0.1f);
            Label_01EC:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 4;
                    goto Label_0216;
                }
                this.inputCaptureState.setCapture(null);
                goto Label_0214;
                this.$PC = -1;
            Label_0214:
                return false;
            Label_0216:
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
        private sealed class <navigateToSlidingInventoryTabIfNotAlready>c__Iterator1EF : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem.InputCaptureState <$>inputCaptureState;
            internal int <$>tabIdx;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__0;
            internal IEnumerator <ie>__2;
            internal SlidingInventoryMenu <sm>__1;
            internal TutorialSystem.InputCaptureState inputCaptureState;
            internal int tabIdx;

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
                    case 1:
                        if (PlayerView.Binder.MenuSystem.InTransition)
                        {
                            this.$current = null;
                            this.$PC = 1;
                            goto Label_0207;
                        }
                        if (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.SlidingInventoryMenu)
                        {
                            goto Label_00C6;
                        }
                        this.<ie>__0 = this.<>f__this.walkPlayerToMenuUnlessAlreadyOpen(this.inputCaptureState, MenuType.SlidingInventoryMenu, MenuContentType.NONE, PlayerView.Binder.DungeonHud.SkillHudButtons[3].RectTm);
                        break;

                    case 2:
                        break;

                    case 3:
                        goto Label_0192;

                    case 4:
                        goto Label_01E9;

                    default:
                        goto Label_0205;
                }
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 2;
                    goto Label_0207;
                }
            Label_00C6:
                this.<sm>__1 = (SlidingInventoryMenu) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.SlidingInventoryMenu);
                this.<sm>__1.setTabInteractable(this.tabIdx, true);
                if (this.<sm>__1.ActiveTabIndex == this.tabIdx)
                {
                    goto Label_0205;
                }
                this.inputCaptureState.setCapture(this.<sm>__1.TabButtons[this.tabIdx].GetComponent<RectTransform>());
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_0192:
                while (this.<sm>__1.ActiveTabIndex != this.tabIdx)
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.inputCaptureState.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0207;
                }
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(0.1f);
            Label_01E9:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 4;
                    goto Label_0207;
                }
                goto Label_0205;
                this.$PC = -1;
            Label_0205:
                return false;
            Label_0207:
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
        private sealed class <navigateToTaskPanelSubMenu>c__Iterator1F0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem.InputCaptureState <$>inputCaptureState;
            internal MenuContentType <$>subMenuContent;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__0;
            internal IEnumerator <ie>__3;
            internal SlidingTaskPanel <sm>__1;
            internal StackedPopupMenu <stackPopup>__2;
            internal TutorialSystem.InputCaptureState inputCaptureState;
            internal MenuContentType subMenuContent;

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
                    case 1:
                        if (PlayerView.Binder.MenuSystem.InTransition)
                        {
                            this.$current = null;
                            this.$PC = 1;
                            goto Label_0271;
                        }
                        if (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.SlidingTaskPanel)
                        {
                            goto Label_00C4;
                        }
                        this.<ie>__0 = this.<>f__this.walkPlayerToMenuUnlessAlreadyOpen(this.inputCaptureState, MenuType.SlidingTaskPanel, MenuContentType.NONE, PlayerView.Binder.DungeonHud.MenuButton.Tm);
                        break;

                    case 2:
                        break;

                    case 3:
                        goto Label_0194;

                    case 4:
                        goto Label_01F0;

                    case 5:
                        goto Label_0247;

                    default:
                        goto Label_026F;
                }
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 2;
                    goto Label_0271;
                }
            Label_00C4:
                this.<sm>__1 = (SlidingTaskPanel) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.SlidingTaskPanel);
                this.inputCaptureState.setCapture(this.<sm>__1.getSubMenuButton(this.subMenuContent).GetComponent<RectTransform>());
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_0194:
                while (PlayerView.Binder.MenuSystem.InTransition || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() != MenuType.StackedPopupMenu))
                {
                    if (this.inputCaptureState.IsSubmitRegistered)
                    {
                        this.inputCaptureState.setCapture(this.<sm>__1.getSubMenuButton(this.subMenuContent).GetComponent<RectTransform>());
                    }
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.inputCaptureState.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0271;
                }
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                this.<stackPopup>__2 = (StackedPopupMenu) PlayerView.Binder.MenuSystem.topmostActiveMenu();
            Label_01F0:
                while (PlayerView.Binder.MenuSystem.InTransition || (this.<stackPopup>__2.activeContentType() != this.subMenuContent))
                {
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_0271;
                }
                this.<ie>__3 = TimeUtil.WaitForUnscaledSeconds(0.1f);
            Label_0247:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 5;
                    goto Label_0271;
                }
                this.inputCaptureState.setCapture(null);
                goto Label_026F;
                this.$PC = -1;
            Label_026F:
                return false;
            Label_0271:
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
        private sealed class <openMenuUnlessAlreadyOpen>c__Iterator1EE : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuContentType <$>contentType;
            internal MenuType <$>menuType;
            internal Menu <topmost>__0;
            internal MenuContentType contentType;
            internal MenuType menuType;

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
                    case 1:
                        if (PlayerView.Binder.MenuSystem.InTransition)
                        {
                            this.$current = null;
                            this.$PC = 1;
                        }
                        else
                        {
                            this.<topmost>__0 = PlayerView.Binder.MenuSystem.topmostActiveMenu();
                            if (((this.<topmost>__0 != null) && (this.<topmost>__0.MenuType == this.menuType)) && (this.<topmost>__0.activeContentType() == this.contentType))
                            {
                                break;
                            }
                            this.$current = PlayerView.Binder.MenuSystem.waitAndCloseAllMenus();
                            this.$PC = 2;
                        }
                        goto Label_00FC;

                    case 2:
                        this.$current = PlayerView.Binder.MenuSystem.transitionToMenu(this.menuType, this.contentType, null, 0f, false, true);
                        this.$PC = 3;
                        goto Label_00FC;

                    case 3:
                        break;
                        this.$PC = -1;
                        break;
                }
                return false;
            Label_00FC:
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
        private sealed class <showSpeechBubbleMenu>c__Iterator1EA : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>freezeTime;
            internal TutorialSystem.InputCaptureState <$>inputCaptureState;
            internal bool <$>keepHudClosedAfter;
            internal string <$>message;
            internal TutorialSystem <>f__this;
            internal ActiveDungeon <ad>__0;
            internal IEnumerator <ie>__4;
            internal SpeechBubbleMenu.InputParams <ip>__2;
            internal LocaSystem <loca>__3;
            internal bool <tutorialSlowdownEnabledWhenEnteredThisFunction>__1;
            internal bool freezeTime;
            internal TutorialSystem.InputCaptureState inputCaptureState;
            internal bool keepHudClosedAfter;
            internal string message;

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
                        this.inputCaptureState.setBlocking(false);
                        this.$current = PlayerView.Binder.MenuSystem.waitForMenuToBeClosed(MenuType.RewardCeremonyMenu);
                        this.$PC = 1;
                        goto Label_02BE;

                    case 1:
                        this.<tutorialSlowdownEnabledWhenEnteredThisFunction>__1 = GameLogic.Binder.TimeSystem.tutorialSlowdownEnabled();
                        if (this.freezeTime && !this.<tutorialSlowdownEnabledWhenEnteredThisFunction>__1)
                        {
                            GameLogic.Binder.TimeSystem.tutorialSlowdown(true);
                        }
                        this.$current = PlayerView.Binder.MenuSystem.waitAndCloseAllMenus();
                        this.$PC = 2;
                        goto Label_02BE;

                    case 2:
                        PlayerView.Binder.DungeonHud.openCloseSkillBar(false, ConfigUi.MENU_TRANSITION_DURATION);
                        break;

                    case 3:
                        break;

                    case 4:
                    case 5:
                        if (PlayerView.Binder.MenuSystem.InTransition || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.SpeechBubble))
                        {
                            this.inputCaptureState.setBlocking(false);
                            this.$current = null;
                            this.$PC = 5;
                            goto Label_02BE;
                        }
                        this.inputCaptureState.setBlocking(true);
                        PlayerView.Binder.DungeonHud.applyTutorialRestrictions();
                        this.<ie>__4 = TimeUtil.WaitForUnscaledSeconds(0.5f);
                        goto Label_022C;

                    case 6:
                        goto Label_022C;

                    case 7:
                        goto Label_0280;

                    default:
                        goto Label_02BC;
                }
                if (PlayerView.Binder.DungeonHud.Animating)
                {
                    this.$current = null;
                    this.$PC = 3;
                }
                else
                {
                    SpeechBubbleMenu.InputParams @params = new SpeechBubbleMenu.InputParams();
                    @params.Message = this.<>f__this.GetFixedTutorialSpeechBubbleString(_.L(this.message, null, false));
                    this.<ip>__2 = @params;
                    this.<loca>__3 = App.Binder.LocaSystem;
                    if (this.<loca>__3.IsRightToLeft(this.<loca>__3.selectedLanguage))
                    {
                        char[] trimChars = new char[] { '\n' };
                        this.<ip>__2.Message = this.<ip>__2.Message.Trim(trimChars);
                    }
                    this.$current = PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.SpeechBubble, MenuContentType.NONE, this.<ip>__2, 0.3f, false, true);
                    this.$PC = 4;
                }
                goto Label_02BE;
            Label_022C:
                while (this.<ie>__4.MoveNext())
                {
                    this.$current = this.<ie>__4.Current;
                    this.$PC = 6;
                    goto Label_02BE;
                }
                if (this.keepHudClosedAfter || (this.<ad>__0.CurrentGameplayState == GameplayState.START_CEREMONY_STEP1))
                {
                    goto Label_028F;
                }
                PlayerView.Binder.DungeonHud.openCloseSkillBar(true, ConfigUi.MENU_TRANSITION_DURATION);
            Label_0280:
                while (PlayerView.Binder.DungeonHud.Animating)
                {
                    this.$current = null;
                    this.$PC = 7;
                    goto Label_02BE;
                }
            Label_028F:
                if (this.freezeTime && !this.<tutorialSlowdownEnabledWhenEnteredThisFunction>__1)
                {
                    GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
                    goto Label_02BC;
                    this.$PC = -1;
                }
            Label_02BC:
                return false;
            Label_02BE:
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
        private sealed class <TUT000A>c__Iterator1BE : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;

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
        private sealed class <TUT001A>c__Iterator1BF : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<ie>__1 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.TUT001A, false, false);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_008A;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    return true;
                }
                this.$PC = -1;
            Label_008A:
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
        private sealed class <TUT001B>c__Iterator1C0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal ActiveDungeon <ad>__1;
            internal IEnumerator <ie>__2;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal Room.Spawnpoint <spawnPt>__3;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<inputCaptureState>__0.setBlocking(true);
                        this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                        PlayerView.Binder.DungeonHud.openCloseCutsceneBorders(true, ConfigTutorials.CUTSCENE_BORDER_OPEN_DURATION);
                        this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(ConfigTutorials.CUTSCENE_BORDER_OPEN_DURATION);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00E3;

                    case 3:
                    case 4:
                        while (this.<ad>__1.CurrentGameplayState != GameplayState.ACTION)
                        {
                            this.$current = null;
                            this.$PC = 4;
                            goto Label_01B7;
                        }
                        goto Label_019D;

                    case 5:
                        goto Label_019D;

                    default:
                        goto Label_01B5;
                }
                if (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 1;
                    goto Label_01B7;
                }
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(0.5f);
            Label_00E3:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 2;
                    goto Label_01B7;
                }
                this.<spawnPt>__3 = this.<ad>__1.ActiveRoom.CharacterSpawnpoints[1];
                GameLogic.Binder.CharacterSpawningSystem.spawnRoomMinionHordeAtSpawnpoint(this.<ad>__1.ActiveRoom, this.<spawnPt>__3, 2, "Skeleton001");
                this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.ACTION, 0f), 0f);
                this.$PC = 3;
                goto Label_01B7;
            Label_019D:
                while (this.<>f__this.m_characterKillCounter < 2)
                {
                    this.$current = null;
                    this.$PC = 5;
                    goto Label_01B7;
                }
                this.$PC = -1;
            Label_01B5:
                return false;
            Label_01B7:
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
        private sealed class <TUT001C>c__Iterator1C1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal ActiveDungeon <ad>__1;
            internal IEnumerator <ie>__5;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal CharacterInstance <pc>__2;
            internal float <rangeThreshold>__4;
            internal Room.Spawnpoint <spawnPt>__3;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<pc>__2 = GameLogic.Binder.GameState.Player.ActiveCharacter;
                        this.<spawnPt>__3 = this.<ad>__1.ActiveRoom.CharacterSpawnpoints[2];
                        GameLogic.Binder.CharacterSpawningSystem.spawnRoomMinionHordeAtSpawnpoint(this.<ad>__1.ActiveRoom, this.<spawnPt>__3, 2, "Skeleton001");
                        this.<rangeThreshold>__4 = this.<pc>__2.AttackRange(true) * 3f;
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_018E;

                    case 3:
                        goto Label_01B6;

                    case 4:
                        goto Label_0208;

                    default:
                        goto Label_021F;
                }
                if (((this.<pc>__2.TargetCharacter == null) || this.<pc>__2.TargetCharacter.IsDead) || (Vector3.Distance(this.<pc>__2.PhysicsBody.Transform.position, this.<pc>__2.TargetCharacter.PhysicsBody.Transform.position) > this.<rangeThreshold>__4))
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_0221;
                }
                this.<inputCaptureState>__0.setBlocking(true);
                PlayerView.Binder.DungeonHud.openCloseCutsceneBorders(false, ConfigTutorials.CUTSCENE_BORDER_CLOSE_DURATION);
                GameLogic.Binder.TimeSystem.tutorialSlowdown(true);
                this.<ie>__5 = TimeUtil.WaitForUnscaledSeconds(0.25f);
            Label_018E:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 2;
                    goto Label_0221;
                }
            Label_01B6:
                while (PlayerView.Binder.DungeonHud.CutsceneTop.Animating)
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0221;
                }
                PlayerView.Binder.DungeonHud.setSkillHudButtonSoftLock(0, false);
                this.<ie>__5 = TimeUtil.WaitForUnscaledSeconds(0.5f);
            Label_0208:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 4;
                    goto Label_0221;
                }
                this.$PC = -1;
            Label_021F:
                return false;
            Label_0221:
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
        private sealed class <TUT001D>c__Iterator1C2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<inputCaptureState>__0.setBlocking(true);
                        this.<>f__this.m_skillActivationFlag = false;
                        PlayerView.Binder.DungeonHud.openCloseSkillBar(true, ConfigUi.MENU_TRANSITION_DURATION);
                        this.<ie>__1 = TimeUtil.WaitForUnscaledSeconds(ConfigUi.MENU_TRANSITION_DURATION);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00DF;

                    case 3:
                        goto Label_0167;

                    case 4:
                        goto Label_01D9;

                    case 5:
                        goto Label_0228;

                    default:
                        goto Label_023F;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    goto Label_0241;
                }
                this.<ie>__1 = TimeUtil.WaitForUnscaledSeconds(0.25f);
            Label_00DF:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 2;
                    goto Label_0241;
                }
                PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = PlayerView.Binder.DungeonHud.SkillButtonContainerTm.GetChild(0).position;
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_0167:
                while (!this.<>f__this.m_skillActivationFlag)
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0241;
                }
                this.<>f__this.m_skillActivationFlag = false;
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                this.<inputCaptureState>__0.setBlocking(true);
                PlayerView.Binder.DungeonHud.overrideSkillButtonInteractState(0, false);
                GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
            Label_01D9:
                while (this.<>f__this.m_characterKillCounter < 4)
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_0241;
                }
                this.<inputCaptureState>__0.setBlocking(true);
                this.<ie>__1 = TimeUtil.WaitForUnscaledSeconds(0.5f);
            Label_0228:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 5;
                    goto Label_0241;
                }
                this.$PC = -1;
            Label_023F:
                return false;
            Label_0241:
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
        private sealed class <TUT001E>c__Iterator1C3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal ActiveDungeon <ad>__1;
            internal IEnumerator <ie>__5;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal CharacterInstance <pc>__2;
            internal float <rangeThreshold>__4;
            internal Room.Spawnpoint <spawnPt>__3;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<pc>__2 = GameLogic.Binder.GameState.Player.ActiveCharacter;
                        this.<spawnPt>__3 = this.<ad>__1.ActiveRoom.CharacterSpawnpoints[3];
                        GameLogic.Binder.CharacterSpawningSystem.spawnRoomMinionHordeAtSpawnpoint(this.<ad>__1.ActiveRoom, this.<spawnPt>__3, 2, "Skeleton001");
                        this.<rangeThreshold>__4 = this.<pc>__2.AttackRange(true) * 4f;
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_017C;

                    case 3:
                        goto Label_01C9;

                    default:
                        goto Label_01E0;
                }
                if (((this.<pc>__2.TargetCharacter == null) || this.<pc>__2.TargetCharacter.IsDead) || (Vector3.Distance(this.<pc>__2.PhysicsBody.Transform.position, this.<pc>__2.TargetCharacter.PhysicsBody.Transform.position) > this.<rangeThreshold>__4))
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01E2;
                }
                this.<inputCaptureState>__0.setBlocking(true);
                PlayerView.Binder.DungeonHud.openCloseSkillBar(false, ConfigUi.MENU_TRANSITION_DURATION);
                GameLogic.Binder.TimeSystem.tutorialSlowdown(true);
            Label_017C:
                while (PlayerView.Binder.DungeonHud.SkillBarAnimating)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01E2;
                }
                PlayerView.Binder.DungeonHud.setSkillHudButtonSoftLock(1, false);
                this.<ie>__5 = TimeUtil.WaitForUnscaledSeconds(0.5f);
            Label_01C9:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 3;
                    goto Label_01E2;
                }
                this.$PC = -1;
            Label_01E0:
                return false;
            Label_01E2:
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
        private sealed class <TUT001F>c__Iterator1C4 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<inputCaptureState>__0.setBlocking(true);
                        this.<>f__this.m_skillActivationFlag = false;
                        PlayerView.Binder.DungeonHud.openCloseSkillBar(true, ConfigUi.MENU_TRANSITION_DURATION);
                        this.<ie>__1 = TimeUtil.WaitForUnscaledSeconds(ConfigUi.MENU_TRANSITION_DURATION);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00DF;

                    case 3:
                        goto Label_016C;

                    case 4:
                        goto Label_01DE;

                    case 5:
                        goto Label_022D;

                    default:
                        goto Label_0244;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    goto Label_0246;
                }
                this.<ie>__1 = TimeUtil.WaitForUnscaledSeconds(0.25f);
            Label_00DF:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 2;
                    goto Label_0246;
                }
                PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = PlayerView.Binder.DungeonHud.SkillButtonContainerTm.GetChild(1).position;
                PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(0f);
            Label_016C:
                while (!this.<>f__this.m_skillActivationFlag)
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0246;
                }
                this.<>f__this.m_skillActivationFlag = false;
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                this.<inputCaptureState>__0.setBlocking(true);
                PlayerView.Binder.DungeonHud.overrideSkillButtonInteractState(0, true);
                GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
            Label_01DE:
                while (this.<>f__this.m_characterKillCounter < 6)
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_0246;
                }
                this.<inputCaptureState>__0.setBlocking(true);
                this.<ie>__1 = TimeUtil.WaitForUnscaledSeconds(0.5f);
            Label_022D:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 5;
                    goto Label_0246;
                }
                this.$PC = -1;
            Label_0244:
                return false;
            Label_0246:
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
        private sealed class <TUT002A>c__Iterator1C5 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal ActiveDungeon <ad>__1;
            internal IEnumerator <ie>__3;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal Player <player>__2;
            internal Room.Spawnpoint <spawnPt>__4;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<inputCaptureState>__0.setBlocking(true);
                        this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<player>__2 = GameLogic.Binder.GameState.Player;
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.WAITING, 0f), 0f);
                        this.$PC = 1;
                        goto Label_034A;

                    case 1:
                    case 2:
                        if (this.<ad>__1.CurrentGameplayState != GameplayState.WAITING)
                        {
                            this.$current = null;
                            this.$PC = 2;
                            goto Label_034A;
                        }
                        this.<ie>__3 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.TUT002A, false, false);
                        break;

                    case 3:
                        break;

                    case 4:
                        goto Label_01B2;

                    case 5:
                        this.<spawnPt>__4 = this.<ad>__1.ActiveRoom.CharacterSpawnpoints[5];
                        GameLogic.Binder.CharacterSpawningSystem.spawnRoomMinionHordeAtSpawnpoint(this.<ad>__1.ActiveRoom, this.<spawnPt>__4, 5, "Skeleton001");
                        this.<ie>__3 = TimeUtil.WaitForUnscaledSeconds(0.5f);
                        goto Label_0281;

                    case 6:
                        goto Label_0281;

                    case 7:
                    case 8:
                        while (this.<ad>__1.CurrentGameplayState != GameplayState.ACTION)
                        {
                            this.<inputCaptureState>__0.setBlocking(false);
                            this.$current = null;
                            this.$PC = 8;
                            goto Label_034A;
                        }
                        this.<inputCaptureState>__0.setBlocking(true);
                        this.<ie>__3 = TimeUtil.WaitForUnscaledSeconds(0.1f);
                        goto Label_0331;

                    case 9:
                        goto Label_0331;

                    default:
                        goto Label_0348;
                }
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 3;
                    goto Label_034A;
                }
                this.<player>__2.setMinionsKilledSinceLastRoomCompletion(0, true);
                GameLogic.Binder.SkillSystem.endSkillCooldownTimers();
                PlayerView.Binder.DungeonHud.FloorProgressionRibbon.gameObject.SetActive(true);
                PlayerView.Binder.DungeonHud.FloorProgressionRibbon.refreshTimerProgressBar(true);
                PlayerView.Binder.DungeonHud.openCloseTopPanel(true, ConfigUi.MENU_TRANSITION_DURATION);
                PlayerView.Binder.DungeonHud.openCloseSkillBar(true, ConfigUi.MENU_TRANSITION_DURATION);
                this.<ie>__3 = TimeUtil.WaitForUnscaledSeconds(ConfigUi.MENU_TRANSITION_DURATION);
            Label_01B2:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 4;
                    goto Label_034A;
                }
                this.<spawnPt>__4 = this.<ad>__1.ActiveRoom.CharacterSpawnpoints[4];
                GameLogic.Binder.CharacterSpawningSystem.spawnRoomMinionHordeAtSpawnpoint(this.<ad>__1.ActiveRoom, this.<spawnPt>__4, 5, "Skeleton001");
                this.$current = null;
                this.$PC = 5;
                goto Label_034A;
            Label_0281:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 6;
                    goto Label_034A;
                }
                this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.ACTION, 0f), 0f);
                this.$PC = 7;
                goto Label_034A;
            Label_0331:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 9;
                    goto Label_034A;
                }
                this.$PC = -1;
            Label_0348:
                return false;
            Label_034A:
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
        private sealed class <TUT002B>c__Iterator1C6 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<inputCaptureState>__0.setBlocking(false);
                        PlayerView.Binder.DungeonHud.FloorProgressionRibbon.gameObject.SetActive(true);
                        PlayerView.Binder.DungeonHud.FloorProgressionRibbon.refreshTimerProgressBar(true);
                        this.<ie>__1 = TimeUtil.WaitForUnscaledSeconds(0.5f);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00AD;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    return true;
                }
                this.$PC = -1;
            Label_00AD:
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
        private sealed class <TUT003A>c__Iterator1C7 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal ActiveDungeon <ad>__1;
            internal IEnumerator <ie>__3;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal Player <player>__2;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<player>__2 = GameLogic.Binder.GameState.Player;
                        PlayerView.Binder.DungeonHud.openCloseTopPanel(true, ConfigUi.MENU_TRANSITION_DURATION);
                        this.<ie>__3 = TimeUtil.WaitForUnscaledSeconds(ConfigUi.MENU_TRANSITION_DURATION);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00D9;

                    case 3:
                    case 4:
                        while (this.<ad>__1.CurrentGameplayState != GameplayState.WAITING)
                        {
                            this.$current = null;
                            this.$PC = 4;
                            goto Label_01A9;
                        }
                        this.<ie>__3 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.TUT003A, false, false);
                        goto Label_0190;

                    case 5:
                        goto Label_0190;

                    default:
                        goto Label_01A7;
                }
                if (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 1;
                    goto Label_01A9;
                }
            Label_00D9:
                while (this.<player>__2.getMinionsKilledSinceLastRoomCompletion(true) < ConfigTutorials.TUTORIAL_REQUIRED_MINION_KILLS_UNTIL_BOSS_SUMMON)
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01A9;
                }
                this.<inputCaptureState>__0.setBlocking(true);
                this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.WAITING, 0f), 0f);
                this.$PC = 3;
                goto Label_01A9;
            Label_0190:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 5;
                    goto Label_01A9;
                }
                this.$PC = -1;
            Label_01A7:
                return false;
            Label_01A9:
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
        private sealed class <TUT003B>c__Iterator1C8 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal ActiveDungeon <ad>__1;
            internal IEnumerator <ie>__2;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<inputCaptureState>__0.setBlocking(true);
                        this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                        PlayerView.Binder.DungeonHud.FloorProgressionRibbon.BossSummoningLocked = false;
                        PlayerView.Binder.DungeonHud.setElementVisibility(true, false);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_011A;

                    case 3:
                        goto Label_016A;

                    case 4:
                        goto Label_01E8;

                    default:
                        goto Label_01FF;
                }
                if (PlayerView.Binder.DungeonHud.Animating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_0201;
                }
                PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = PlayerView.Binder.DungeonHud.FloorProgressionRibbon.ChallengeButtonRoot.transform.position;
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_011A:
                while (this.<ad>__1.CurrentGameplayState != GameplayState.BOSS_START)
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0201;
                }
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                this.<inputCaptureState>__0.setBlocking(true);
            Label_016A:
                while ((this.<ad>__1.BossesKilled == 0) || (this.<ad>__1.ActiveRoom.numberOfBossesAlive() > 0))
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0201;
                }
                this.<inputCaptureState>__0.setBlocking(true);
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.Music_JingleVictory, (float) 0f);
                PlayerView.Binder.AudioSystem.stopMusic();
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(1f);
            Label_01E8:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 4;
                    goto Label_0201;
                }
                this.$PC = -1;
            Label_01FF:
                return false;
            Label_0201:
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
        private sealed class <TUT003C>c__Iterator1C9 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<inputCaptureState>__0.setBlocking(true);
                        PlayerView.Binder.DungeonHud.openCloseTopPanel(false, ConfigUi.MENU_TRANSITION_DURATION);
                        this.<ie>__1 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.TUT003C, true, false);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00AB;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    return true;
                }
                goto Label_00AB;
                this.$PC = -1;
            Label_00AB:
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
        private sealed class <TUT004A>c__Iterator1CA : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<inputCaptureState>__0.setBlocking(true);
                        PlayerView.Binder.DungeonHud.openCloseSkillBar(false, 0f);
                        this.<ie>__1 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.TUT004A, false, true);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00C2;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    return true;
                }
                this.<inputCaptureState>__0.setBlocking(false);
                PlayerView.Binder.DungeonHud.openCloseTopPanel(true, ConfigUi.MENU_TRANSITION_DURATION);
                this.$PC = -1;
            Label_00C2:
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
        private sealed class <TUT004B>c__Iterator1CB : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;

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
        private sealed class <TUT050A>c__Iterator1CC : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal double <coinCost>__5;
            internal IEnumerator <ie>__6;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal CharacterInstance <pc>__2;
            internal Player <player>__1;
            internal int <targetRank>__4;
            internal ItemInstance <weaponInstance>__3;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        this.<pc>__2 = this.<player>__1.ActiveCharacter;
                        this.<weaponInstance>__3 = this.<pc>__2.getEquippedItemOfType(ItemType.Weapon);
                        this.<targetRank>__4 = this.<weaponInstance>__3.Rank + 1;
                        this.<coinCost>__5 = this.<pc>__2.getAdjustedItemUpgradeCost(this.<weaponInstance>__3.Item.Type, this.<player>__1.getRiggedItemLevel(this.<weaponInstance>__3), this.<targetRank>__4);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_013C;

                    default:
                        goto Label_0158;
                }
                if (this.<player>__1.getResourceAmount(ResourceType.Coin) < this.<coinCost>__5)
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_015A;
                }
                this.<inputCaptureState>__0.setBlocking(true);
                this.<ie>__6 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.TUT050A, false, true);
            Label_013C:
                while (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 2;
                    goto Label_015A;
                }
                goto Label_0158;
                this.$PC = -1;
            Label_0158:
                return false;
            Label_015A:
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
        private sealed class <TUT050B>c__Iterator1CD : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;

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
        private sealed class <TUT050C>c__Iterator1CE : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal ItemCell <firstItemCell>__4;
            internal IEnumerator <ie>__2;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal Player <player>__1;
            internal SlidingInventoryMenu <sm>__3;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        if (this.<player>__1.ActiveCharacter.getEquippedItemOfType(ItemType.Weapon).Rank <= 0)
                        {
                            this.<inputCaptureState>__0.setBlocking(true);
                            GameLogic.Binder.TimeSystem.tutorialSlowdown(true);
                            PlayerView.Binder.DungeonHud.setElementVisibility(true, false);
                            break;
                        }
                        goto Label_02BA;

                    case 1:
                        break;

                    case 2:
                        goto Label_011B;

                    case 3:
                        goto Label_01E0;

                    case 4:
                        goto Label_023D;

                    case 5:
                        goto Label_0298;

                    default:
                        goto Label_02BA;
                }
                while (PlayerView.Binder.DungeonHud.Animating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_02BC;
                }
                this.<>f__this.m_itemRankUpFlag = false;
                this.<ie>__2 = this.<>f__this.walkPlayerToMenuUnlessAlreadyOpen(this.<inputCaptureState>__0, MenuType.SlidingInventoryMenu, MenuContentType.NONE, PlayerView.Binder.DungeonHud.SkillHudButtons[3].RectTm);
            Label_011B:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 2;
                    goto Label_02BC;
                }
                this.<sm>__3 = (SlidingInventoryMenu) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.SlidingInventoryMenu);
                this.<firstItemCell>__4 = this.<sm>__3.ItemCells[0];
                this.<inputCaptureState>__0.setCapture(this.<firstItemCell>__4.CellButton.ButtonScript.GetComponent<RectTransform>());
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_01E0:
                while (!this.<>f__this.m_itemRankUpFlag)
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.<inputCaptureState>__0.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_02BC;
                }
                this.<>f__this.m_itemRankUpFlag = false;
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(0.1f);
            Label_023D:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 4;
                    goto Label_02BC;
                }
                this.<ie>__2 = this.<>f__this.walkPlayerOutOfMenu(this.<inputCaptureState>__0, MenuType.SlidingInventoryMenu, this.<sm>__3.CloseButton.GetComponent<RectTransform>());
            Label_0298:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 5;
                    goto Label_02BC;
                }
                GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
                this.$PC = -1;
            Label_02BA:
                return false;
            Label_02BC:
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
        private sealed class <TUT050D>c__Iterator1CF : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<ie>__1 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.TUT050D, false, true);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00AB;

                    default:
                        goto Label_00C6;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    goto Label_00C8;
                }
                PlayerView.Binder.DungeonHud.setElementVisibility(true, false);
            Label_00AB:
                while (PlayerView.Binder.DungeonHud.Animating)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_00C8;
                }
                goto Label_00C6;
                this.$PC = -1;
            Label_00C6:
                return false;
            Label_00C8:
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
        private sealed class <TUT051A>c__Iterator1D0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal ItemInstance <armorInstance>__3;
            internal double <coinCost>__5;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal CharacterInstance <pc>__2;
            internal Player <player>__1;
            internal int <targetRank>__4;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        this.<pc>__2 = this.<player>__1.ActiveCharacter;
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_011B;

                    default:
                        goto Label_014A;
                }
                if (this.<player>__1.getLastCompletedFloor(false) < 1)
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_014C;
                }
                this.<inputCaptureState>__0.setBlocking(true);
                this.<armorInstance>__3 = this.<pc>__2.getEquippedItemOfType(ItemType.Armor);
                this.<targetRank>__4 = this.<armorInstance>__3.Rank + 1;
                this.<coinCost>__5 = this.<pc>__2.getAdjustedItemUpgradeCost(this.<armorInstance>__3.Item.Type, this.<player>__1.getRiggedItemLevel(this.<armorInstance>__3), this.<targetRank>__4);
            Label_011B:
                while (this.<player>__1.getResourceAmount(ResourceType.Coin) < this.<coinCost>__5)
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_014C;
                }
                this.<inputCaptureState>__0.setBlocking(true);
                goto Label_014A;
                this.$PC = -1;
            Label_014A:
                return false;
            Label_014C:
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
        private sealed class <TUT051B>c__Iterator1D1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal ItemCell <firstItemCell>__4;
            internal IEnumerator <ie>__2;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal Player <player>__1;
            internal SlidingInventoryMenu <sm>__3;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        if (this.<player>__1.ActiveCharacter.getEquippedItemOfType(ItemType.Armor).Rank <= 0)
                        {
                            this.<ie>__2 = this.<>f__this.TUT051A();
                            break;
                        }
                        goto Label_0367;

                    case 1:
                        break;

                    case 2:
                        goto Label_00FA;

                    case 3:
                        goto Label_0166;

                    case 4:
                        goto Label_01B0;

                    case 5:
                        goto Label_0281;

                    case 6:
                        goto Label_02DE;

                    case 7:
                        goto Label_0345;

                    default:
                        goto Label_0367;
                }
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 1;
                    goto Label_0369;
                }
                this.<inputCaptureState>__0.setBlocking(true);
                GameLogic.Binder.TimeSystem.tutorialSlowdown(true);
                PlayerView.Binder.DungeonHud.setElementVisibility(true, false);
            Label_00FA:
                while (PlayerView.Binder.DungeonHud.Animating)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0369;
                }
                this.<>f__this.m_itemRankUpFlag = false;
                this.<ie>__2 = this.<>f__this.walkPlayerToMenuUnlessAlreadyOpen(this.<inputCaptureState>__0, MenuType.SlidingInventoryMenu, MenuContentType.NONE, PlayerView.Binder.DungeonHud.SkillHudButtons[3].RectTm);
            Label_0166:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 3;
                    goto Label_0369;
                }
                this.<ie>__2 = this.<>f__this.navigateToSlidingInventoryTabIfNotAlready(this.<inputCaptureState>__0, 1);
            Label_01B0:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 4;
                    goto Label_0369;
                }
                this.<sm>__3 = (SlidingInventoryMenu) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.SlidingInventoryMenu);
                this.<firstItemCell>__4 = this.<sm>__3.ItemCells[0];
                this.<firstItemCell>__4.setNumAllowedUpgrades(1);
                this.<inputCaptureState>__0.setCapture(this.<firstItemCell>__4.CellButton.ButtonScript.GetComponent<RectTransform>());
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_0281:
                while (!this.<>f__this.m_itemRankUpFlag)
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.<inputCaptureState>__0.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 5;
                    goto Label_0369;
                }
                this.<>f__this.m_itemRankUpFlag = false;
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(0.1f);
            Label_02DE:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 6;
                    goto Label_0369;
                }
                this.<firstItemCell>__4.setNumAllowedUpgrades(-1);
                this.<ie>__2 = this.<>f__this.walkPlayerOutOfMenu(this.<inputCaptureState>__0, MenuType.SlidingInventoryMenu, this.<sm>__3.CloseButton.GetComponent<RectTransform>());
            Label_0345:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 7;
                    goto Label_0369;
                }
                GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
                this.$PC = -1;
            Label_0367:
                return false;
            Label_0369:
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
        private sealed class <TUT051C>c__Iterator1D2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<ie>__1 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.TUT051C, false, true);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_008F;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    return true;
                }
                goto Label_008F;
                this.$PC = -1;
            Label_008F:
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
        private sealed class <TUT052A>c__Iterator1D3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0091;
                }
                if (this.<player>__1.getLastCompletedFloor(false) < 2)
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<inputCaptureState>__0.setBlocking(true);
                goto Label_0091;
                this.$PC = -1;
            Label_0091:
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
        private sealed class <TUT052B>c__Iterator1D4 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<ie>__1 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.TUT052B, false, true);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_008F;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    return true;
                }
                goto Label_008F;
                this.$PC = -1;
            Label_008F:
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
        private sealed class <TUT052C>c__Iterator1D5 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal Button <equipButton>__10;
            internal ItemInstance <equippedCloak>__4;
            internal int <i>__3;
            internal IEnumerator <ie>__5;
            internal ItemInstance <ii>__8;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal ItemCell <itemCell>__7;
            internal ItemInfoContent <itemInfo>__9;
            internal CharacterInstance <pc>__2;
            internal Player <player>__1;
            internal SlidingInventoryMenu <sm>__6;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        this.<pc>__2 = this.<player>__1.ActiveCharacter;
                        this.<i>__3 = this.<player>__1.UnclaimedRewards.Count - 1;
                        while (this.<i>__3 >= 0)
                        {
                            if (ConfigMeta.IsBossChest(this.<player>__1.UnclaimedRewards[this.<i>__3].ChestType))
                            {
                                CmdConsumeReward.ExecuteStatic(this.<player>__1, this.<player>__1.UnclaimedRewards[this.<i>__3], true, string.Empty);
                            }
                            this.<i>__3--;
                        }
                        this.<equippedCloak>__4 = this.<pc>__2.getEquippedItemOfType(ItemType.Cloak);
                        if ((this.<equippedCloak>__4 != null) && (this.<equippedCloak>__4.ItemId == ConfigTutorials.FIXED_TUTORIAL_CLOAK_ITEM_ID))
                        {
                            goto Label_056A;
                        }
                        this.<inputCaptureState>__0.setBlocking(true);
                        GameLogic.Binder.TimeSystem.tutorialSlowdown(true);
                        PlayerView.Binder.DungeonHud.setElementVisibility(true, false);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_01CA;

                    case 3:
                        goto Label_0214;

                    case 4:
                        goto Label_0348;

                    case 5:
                        goto Label_03A5;

                    case 6:
                        goto Label_0496;

                    case 7:
                        goto Label_04ED;

                    case 8:
                        goto Label_0548;

                    default:
                        goto Label_056A;
                }
                while (PlayerView.Binder.DungeonHud.Animating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_056C;
                }
                this.<ie>__5 = this.<>f__this.walkPlayerToMenuUnlessAlreadyOpen(this.<inputCaptureState>__0, MenuType.SlidingInventoryMenu, MenuContentType.NONE, PlayerView.Binder.DungeonHud.SkillHudButtons[3].RectTm);
            Label_01CA:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 2;
                    goto Label_056C;
                }
                this.<ie>__5 = this.<>f__this.navigateToSlidingInventoryTabIfNotAlready(this.<inputCaptureState>__0, 2);
            Label_0214:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 3;
                    goto Label_056C;
                }
                this.<sm>__6 = (SlidingInventoryMenu) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.SlidingInventoryMenu);
                this.<itemCell>__7 = null;
                if (this.<sm>__6.ItemCells.Count > 1)
                {
                    this.<itemCell>__7 = this.<sm>__6.ItemCells[1];
                }
                else if (this.<sm>__6.ItemCells.Count == 1)
                {
                    this.<itemCell>__7 = this.<sm>__6.ItemCells[0];
                }
                else
                {
                    UnityEngine.Debug.LogError("TUT052C: no ItemCells available");
                }
                this.<inputCaptureState>__0.setCapture(this.<itemCell>__7.CellButton.ButtonScript.GetComponent<RectTransform>());
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
                this.<ii>__8 = this.<itemCell>__7.ItemInstance;
            Label_0348:
                while (!this.<pc>__2.isItemInstanceEquipped(this.<ii>__8) && (PlayerView.Binder.MenuSystem.InTransition || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() != MenuType.ThinPopupMenu)))
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.<inputCaptureState>__0.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_056C;
                }
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
            Label_03A5:
                while (PlayerView.Binder.MenuSystem.InTransition || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() != MenuType.ThinPopupMenu))
                {
                    this.$current = null;
                    this.$PC = 5;
                    goto Label_056C;
                }
                this.<itemInfo>__9 = (ItemInfoContent) ((ThinPopupMenu) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.ThinPopupMenu)).activeContentObject();
                if (this.<itemInfo>__9 != null)
                {
                    this.<equipButton>__10 = this.<itemInfo>__9.EquipButton;
                    this.<inputCaptureState>__0.setCapture(this.<equipButton>__10.GetComponent<RectTransform>());
                }
                if (this.<itemInfo>__9 == null)
                {
                    goto Label_04BB;
                }
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_0496:
                while (!this.<pc>__2.isItemInstanceEquipped(this.<ii>__8))
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.<inputCaptureState>__0.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 6;
                    goto Label_056C;
                }
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
            Label_04BB:
                this.<ie>__5 = TimeUtil.WaitForUnscaledSeconds(0.3f);
            Label_04ED:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 7;
                    goto Label_056C;
                }
                this.<ie>__5 = this.<>f__this.walkPlayerOutOfMenu(this.<inputCaptureState>__0, MenuType.SlidingInventoryMenu, this.<sm>__6.CloseButton.GetComponent<RectTransform>());
            Label_0548:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 8;
                    goto Label_056C;
                }
                GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
                this.$PC = -1;
            Label_056A:
                return false;
            Label_056C:
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
        private sealed class <TUT052D>c__Iterator1D6 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<ie>__1 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.TUT052D, false, true);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00B9;

                    default:
                        goto Label_00D5;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    goto Label_00D7;
                }
                this.<ie>__1 = TimeUtil.WaitForUnscaledSeconds(1f);
            Label_00B9:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 2;
                    goto Label_00D7;
                }
                goto Label_00D5;
                this.$PC = -1;
            Label_00D5:
                return false;
            Label_00D7:
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
        private sealed class <TUT149A>c__Iterator1D7 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00A0;
                }
                if ((this.<player>__1.Rank < 2) && (this.<player>__1.getNumberOfUnclaimedLevelUpRewards() == 0))
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<inputCaptureState>__0.setBlocking(true);
                goto Label_00A0;
                this.$PC = -1;
            Label_00A0:
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
        private sealed class <TUT150D>c__Iterator1D8 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;

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
        private sealed class <TUT200A>c__Iterator1D9 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__2;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00CC;

                    default:
                        goto Label_00E8;
                }
                if ((this.<player>__1.getLastCompletedFloor(false) + 1) < 10)
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_00EA;
                }
                this.<inputCaptureState>__0.setBlocking(true);
                this.<ie>__2 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.TUT200A, false, true);
            Label_00CC:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 2;
                    goto Label_00EA;
                }
                goto Label_00E8;
                this.$PC = -1;
            Label_00E8:
                return false;
            Label_00EA:
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
        private sealed class <TUT200B>c__Iterator1DA : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;

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
        private sealed class <TUT351A>c__Iterator1DB : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal ActiveDungeon <ad>__1;
            internal IEnumerator <ie>__3;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal Player <player>__2;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<player>__2 = GameLogic.Binder.GameState.Player;
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00C3;

                    case 3:
                        this.<ie>__3 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.TUT351A, false, true);
                        goto Label_015E;

                    case 4:
                        goto Label_015E;

                    default:
                        goto Label_017A;
                }
                if (!this.<player>__2.canRetire())
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_017C;
                }
                this.<inputCaptureState>__0.setBlocking(true);
            Label_00C3:
                while (((this.<ad>__1.CurrentGameplayState != GameplayState.START_CEREMONY_STEP1) && (this.<ad>__1.CurrentGameplayState != GameplayState.START_CEREMONY_STEP2)) && (this.<ad>__1.CurrentGameplayState != GameplayState.ACTION))
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_017C;
                }
                this.<inputCaptureState>__0.setBlocking(true);
                this.$current = PlayerView.Binder.MenuSystem.waitAndCloseAllMenus();
                this.$PC = 3;
                goto Label_017C;
            Label_015E:
                if (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 4;
                    goto Label_017C;
                }
                goto Label_017A;
                this.$PC = -1;
            Label_017A:
                return false;
            Label_017C:
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
        private sealed class <TUT351B>c__Iterator1DC : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<inputCaptureState>__0.setBlocking(true);
                        GameLogic.Binder.TimeSystem.tutorialSlowdown(true);
                        PlayerView.Binder.DungeonHud.setElementVisibility(true, false);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00BD;

                    default:
                        goto Label_00E4;
                }
                if (PlayerView.Binder.DungeonHud.Animating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_00E6;
                }
                this.<ie>__1 = this.<>f__this.navigateToAdventurePanel(this.<inputCaptureState>__0, SlidingAdventurePanel.ContentTarget.AscendButton, 0, -1);
            Label_00BD:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 2;
                    goto Label_00E6;
                }
                GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
                goto Label_00E4;
                this.$PC = -1;
            Label_00E4:
                return false;
            Label_00E6:
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
        private sealed class <TUT351C>c__Iterator1DD : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal ActiveDungeon <ad>__3;
            internal AscendPopupContent <ascendPopup>__2;
            internal IEnumerator <ie>__1;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<inputCaptureState>__0.setBlocking(true);
                        this.<ie>__1 = this.<>f__this.openMenuUnlessAlreadyOpen(MenuType.ThinPopupMenu, MenuContentType.AscendPopupContent);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_0140;

                    case 3:
                        goto Label_018A;

                    default:
                        goto Label_022B;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    goto Label_022D;
                }
                this.<>f__this.m_retirementFlag = false;
                if (!App.Binder.ConfigMeta.RETIREMENT_FORCED_DURING_FTUE)
                {
                    goto Label_017A;
                }
                this.<ascendPopup>__2 = (AscendPopupContent) PlayerView.Binder.MenuSystem.topmostActiveMenu().activeContentObject();
                this.<inputCaptureState>__0.setCapture(this.<ascendPopup>__2.AscendButtonTm);
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_0140:
                while (!this.<>f__this.m_retirementFlag && !PlayerView.Binder.MenuSystem.InTransition)
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.<inputCaptureState>__0.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_022D;
                }
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                this.<inputCaptureState>__0.setCapture(null);
            Label_017A:
                this.<ad>__3 = GameLogic.Binder.GameState.ActiveDungeon;
            Label_018A:
                this.<inputCaptureState>__0.setBlocking(false);
                if (this.<>f__this.m_retirementFlag || (this.<ad>__3.CurrentGameplayState == GameplayState.RETIREMENT))
                {
                    UnityEngine.Debug.Log("Player chose to ascend during FTUE..");
                }
                else if ((!PlayerView.Binder.MenuSystem.InTransition && (PlayerView.Binder.MenuSystem.topmostActiveMenuType() != MenuType.ThinPopupMenu)) && (PlayerView.Binder.MenuSystem.topmostActiveMenuType() != MenuType.TooltipMenu))
                {
                    UnityEngine.Debug.Log("Player chose NOT to ascend during FTUE..");
                }
                else
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_022D;
                    this.$PC = -1;
                }
            Label_022B:
                return false;
            Label_022D:
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
        private sealed class <TUT360A>c__Iterator1DE : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__2;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00BD;

                    case 3:
                        goto Label_00FE;

                    case 4:
                        goto Label_014E;

                    default:
                        goto Label_016A;
                }
                if ((this.<player>__1.CumulativeRetiredHeroStats.HeroesRetired < 1) || this.<player>__1.PendingPostRetirementGiftRewardCeremony)
                {
                    this.<inputCaptureState>__0.setBlocking(false);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_016C;
                }
                this.<inputCaptureState>__0.setBlocking(true);
            Label_00BD:
                while (PlayerView.Binder.MenuSystem.InTransition)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_016C;
                }
                this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(0.2f);
            Label_00FE:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 3;
                    goto Label_016C;
                }
                this.<ie>__2 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.TUT360A, false, true);
            Label_014E:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 4;
                    goto Label_016C;
                }
                goto Label_016A;
                this.$PC = -1;
            Label_016A:
                return false;
            Label_016C:
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
        private sealed class <TUT360B>c__Iterator1DF : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__2;
            internal IEnumerator <ie>__3;
            internal IEnumerator <ie>__8;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal MiniPopupMenu <miniPopup>__6;
            internal Player <player>__1;
            internal VendorPopupContent <popupContent>__5;
            internal StackedPopupMenu <stackPopup>__4;
            internal StackedPopupMenu <stackPopup>__7;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        if (this.<player>__1.Augmentations.getTotalNumberOwned() <= 0)
                        {
                            this.<inputCaptureState>__0.setBlocking(true);
                            GameLogic.Binder.TimeSystem.tutorialSlowdown(true);
                            PlayerView.Binder.DungeonHud.setElementVisibility(true, false);
                            break;
                        }
                        goto Label_0425;

                    case 1:
                        break;

                    case 2:
                        goto Label_00DC;

                    case 3:
                        goto Label_0136;

                    case 4:
                        goto Label_0197;

                    case 5:
                        goto Label_0275;

                    case 6:
                        goto Label_0342;

                    case 7:
                        goto Label_03E2;

                    case 8:
                        GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
                        goto Label_0425;

                    default:
                        goto Label_0425;
                }
                while (PlayerView.Binder.DungeonHud.Animating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_0427;
                }
            Label_00DC:
                while (PlayerView.Binder.MenuSystem.InTransition)
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0427;
                }
                if (!App.Binder.ConfigMeta.SELL_AUGMENTATIONS_IN_ADVENTURE_PANEL)
                {
                    this.<ie>__3 = this.<>f__this.walkPlayerToMenuUnlessAlreadyOpen(this.<inputCaptureState>__0, MenuType.StackedPopupMenu, MenuContentType.VendorPopupContent, PlayerView.Binder.DungeonHud.VendorButton.Tm);
                    goto Label_0197;
                }
                this.<ie>__2 = this.<>f__this.navigateToAdventurePanel(this.<inputCaptureState>__0, SlidingAdventurePanel.ContentTarget.FirstAugmentationCell, 0, 2);
            Label_0136:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 3;
                    goto Label_0427;
                }
                goto Label_02A4;
            Label_0197:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 4;
                    goto Label_0427;
                }
                this.<stackPopup>__4 = (StackedPopupMenu) PlayerView.Binder.MenuSystem.topmostActiveMenu();
                this.<popupContent>__5 = (VendorPopupContent) this.<stackPopup>__4.activeContentObject();
                this.<popupContent>__5.centerOnTransform(this.<popupContent>__5.AugmentationGridTm);
                this.<inputCaptureState>__0.setCapture(this.<popupContent>__5.AugmentationGridTm.GetChild(0).GetComponentInChildren<Button>().GetComponent<RectTransform>());
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_0275:
                while (PlayerView.Binder.MenuSystem.InTransition || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() != MenuType.MiniPopupMenu))
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.<inputCaptureState>__0.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 5;
                    goto Label_0427;
                }
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
            Label_02A4:
                this.<miniPopup>__6 = (MiniPopupMenu) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.MiniPopupMenu);
                this.<inputCaptureState>__0.setCapture(this.<miniPopup>__6.MainButton.Button.GetComponent<RectTransform>());
                PlayerView.Binder.DungeonHud.TutorialCircle.Play();
            Label_0342:
                while (PlayerView.Binder.MenuSystem.InTransition || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.MiniPopupMenu))
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.<inputCaptureState>__0.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 6;
                    goto Label_0427;
                }
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                if (App.Binder.ConfigMeta.SELL_AUGMENTATIONS_IN_ADVENTURE_PANEL)
                {
                    goto Label_03F2;
                }
                this.<stackPopup>__7 = (StackedPopupMenu) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.StackedPopupMenu);
                this.<ie>__8 = this.<>f__this.walkPlayerOutOfMenu(this.<inputCaptureState>__0, MenuType.StackedPopupMenu, this.<stackPopup>__7.TitleCloseButton.GetComponent<RectTransform>());
            Label_03E2:
                while (this.<ie>__8.MoveNext())
                {
                    this.$current = this.<ie>__8.Current;
                    this.$PC = 7;
                    goto Label_0427;
                }
            Label_03F2:
                this.$current = PlayerView.Binder.MenuSystem.waitAndCloseAllMenus();
                this.$PC = 8;
                goto Label_0427;
                this.$PC = -1;
            Label_0425:
                return false;
            Label_0427:
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
        private sealed class <TUT360C>c__Iterator1E0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal IEnumerator <ie>__1;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00AD;

                    default:
                        goto Label_00C9;
                }
                if (PlayerView.Binder.MenuSystem.InTransition || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() != MenuType.NONE))
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_00CB;
                }
                this.<ie>__1 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.TUT360C, false, true);
            Label_00AD:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 2;
                    goto Label_00CB;
                }
                goto Label_00C9;
                this.$PC = -1;
            Label_00C9:
                return false;
            Label_00CB:
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
        private sealed class <TUT451A>c__Iterator1E1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal ActiveDungeon <ad>__1;
            internal IEnumerator <ie>__3;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;
            internal Player <player>__2;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<player>__2 = GameLogic.Binder.GameState.Player;
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00EE;

                    default:
                        goto Label_010A;
                }
                if ((!this.<player>__2.HasUnlockedMissions || PlayerView.Binder.MenuSystem.InTransition) || ((PlayerView.Binder.MenuSystem.topmostActiveMenuType() != MenuType.NONE) || (this.<ad>__1.CurrentGameplayState != GameplayState.ACTION)))
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_010C;
                }
                this.<ie>__3 = this.<>f__this.showSpeechBubbleMenu(this.<inputCaptureState>__0, ConfigLoca.TUT451A, false, true);
            Label_00EE:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 2;
                    goto Label_010C;
                }
                goto Label_010A;
                this.$PC = -1;
            Label_010A:
                return false;
            Label_010C:
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
        private sealed class <TUT451B>c__Iterator1E2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem <>f__this;
            internal ActiveDungeon <ad>__1;
            internal IEnumerator <ie>__2;
            internal TutorialSystem.InputCaptureState <inputCaptureState>__0;

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
                        this.<inputCaptureState>__0 = this.<>f__this.getInputCaptureState(InputSystem.Layer.FtueTutorial);
                        this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_00F9;

                    default:
                        goto Label_0120;
                }
                if ((!PlayerView.Binder.DungeonHud.MenuButton.Button.interactable || PlayerView.Binder.MenuSystem.InTransition) || ((PlayerView.Binder.MenuSystem.topmostActiveMenuType() != MenuType.NONE) || (this.<ad>__1.CurrentGameplayState != GameplayState.ACTION)))
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_0122;
                }
                this.<inputCaptureState>__0.setBlocking(true);
                GameLogic.Binder.TimeSystem.tutorialSlowdown(true);
                this.<ie>__2 = this.<>f__this.navigateToTaskPanelSubMenu(this.<inputCaptureState>__0, MenuContentType.MissionsPopupContent);
            Label_00F9:
                while (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 2;
                    goto Label_0122;
                }
                GameLogic.Binder.TimeSystem.tutorialSlowdown(false);
                goto Label_0120;
                this.$PC = -1;
            Label_0120:
                return false;
            Label_0122:
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
        private sealed class <walkPlayerOutOfMenu>c__Iterator1ED : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal TutorialSystem.InputCaptureState <$>inputCaptureState;
            internal RectTransform <$>inputCaptureTargetTm;
            internal MenuType <$>menuType;
            internal TutorialSystem.InputCaptureState inputCaptureState;
            internal RectTransform inputCaptureTargetTm;
            internal MenuType menuType;

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
                        this.inputCaptureState.setCapture(this.inputCaptureTargetTm);
                        PlayerView.Binder.DungeonHud.TutorialCircle.Play();
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00C9;
                }
                if (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == this.menuType)
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.inputCaptureState.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                goto Label_00C9;
                this.$PC = -1;
            Label_00C9:
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
        private sealed class <walkPlayerToMenuUnlessAlreadyOpen>c__Iterator1EC : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuContentType <$>contentType;
            internal TutorialSystem.InputCaptureState <$>inputCaptureState;
            internal RectTransform <$>inputCaptureTargetTm;
            internal MenuType <$>menuType;
            internal Menu <topmost>__0;
            internal MenuContentType contentType;
            internal TutorialSystem.InputCaptureState inputCaptureState;
            internal RectTransform inputCaptureTargetTm;
            internal MenuType menuType;

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
                    case 1:
                        if (PlayerView.Binder.MenuSystem.InTransition)
                        {
                            this.$current = null;
                            this.$PC = 1;
                        }
                        else
                        {
                            this.<topmost>__0 = PlayerView.Binder.MenuSystem.topmostActiveMenu();
                            if (((this.<topmost>__0 != null) && (this.<topmost>__0.MenuType == this.menuType)) && (this.<topmost>__0.activeContentType() == this.contentType))
                            {
                                goto Label_0175;
                            }
                            this.$current = PlayerView.Binder.MenuSystem.waitAndCloseAllMenus();
                            this.$PC = 2;
                        }
                        goto Label_0177;

                    case 2:
                        this.inputCaptureState.setCapture(this.inputCaptureTargetTm);
                        PlayerView.Binder.DungeonHud.TutorialCircle.Play();
                        break;

                    case 3:
                        break;

                    default:
                        goto Label_0175;
                }
                if (PlayerView.Binder.MenuSystem.InTransition || (PlayerView.Binder.MenuSystem.topmostActiveMenuType() != this.menuType))
                {
                    PlayerView.Binder.DungeonHud.TutorialCircle.transform.position = this.inputCaptureState.RectTm.position;
                    PlayerView.Binder.DungeonHud.TutorialCircle.Simulate(Time.deltaTime / Time.timeScale, true, false);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0177;
                }
                EffectUtils.ResetParticleSystem(PlayerView.Binder.DungeonHud.TutorialCircle);
                goto Label_0175;
                this.$PC = -1;
            Label_0175:
                return false;
            Label_0177:
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

        private class InputCaptureState
        {
            public bool IsBlocking;
            public bool IsSubmitRegistered;
            public PlayerView.InputSystem.Layer Layer;
            public RectTransform RectTm;

            public InputCaptureState(PlayerView.InputSystem.Layer layer)
            {
                this.Layer = layer;
            }

            private void refreshInputRequirement()
            {
                InputSystem.Requirement requirement = (!this.IsBlocking && (this.RectTm == null)) ? InputSystem.Requirement.Neutral : InputSystem.Requirement.MustBeDisabled;
                PlayerView.Binder.InputSystem.setInputRequirement(this.Layer, requirement);
            }

            public void setBlocking(bool blocking)
            {
                this.IsBlocking = blocking;
                this.refreshInputRequirement();
            }

            public void setCapture(RectTransform targetRectTm)
            {
                this.RectTm = targetRectTm;
                this.IsSubmitRegistered = false;
                this.refreshInputRequirement();
            }
        }

        public delegate IEnumerator TutorialRoutine();
    }
}

