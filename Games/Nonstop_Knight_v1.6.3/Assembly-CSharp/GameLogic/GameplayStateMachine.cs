namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameplayStateMachine : UnityEngine.MonoBehaviour, IGameplayStateMachine
    {
        private FiniteStateMachine m_fsm;
        private Dictionary<GameplayState, FiniteStateMachine.State> m_fsmStates = new Dictionary<GameplayState, FiniteStateMachine.State>();

        protected void Awake()
        {
            this.m_fsm = new FiniteStateMachine(this);
            this.m_fsmStates[GameplayState.START_CEREMONY_STEP1] = new GameplayStartCeremonyStep1State();
            this.m_fsmStates[GameplayState.START_CEREMONY_STEP2] = new GameplayStartCeremonyStep2State();
            this.m_fsmStates[GameplayState.WAITING] = new GameplayWaitingState();
            this.m_fsmStates[GameplayState.ACTION] = new GameplayActionState();
            this.m_fsmStates[GameplayState.BOSS_START] = new GameplayBossStartState();
            this.m_fsmStates[GameplayState.BOSS_FIGHT] = new GameplayBossFightState();
            this.m_fsmStates[GameplayState.END_CEREMONY] = new GameplayEndCeremonyState();
            this.m_fsmStates[GameplayState.ENDED] = new GameplayEndedState();
            this.m_fsmStates[GameplayState.REVIVAL] = new GameplayRevivalState();
            this.m_fsmStates[GameplayState.RETIREMENT] = new GameplayRetirementState();
            this.m_fsmStates[GameplayState.ROOM_COMPLETION] = new RoomCompletionState();
            Binder.EventBus.OnGameplayStateChanged += new Events.GameplayStateChanged(this.onGameplayStateChanged);
        }

        private void changeFsmState(GameplayState newState)
        {
            if ((this.m_fsm.CurrentState != null) && (this.m_fsm.CurrentState.ID == newState))
            {
                Debug.LogError("Trying to switch into same FSM state: " + newState);
            }
            else
            {
                this.m_fsm.changeState(this.m_fsmStates[newState], 0f);
            }
        }

        protected void OnDisable()
        {
            Binder.EventBus.OnGameplayStateChanged -= new Events.GameplayStateChanged(this.onGameplayStateChanged);
        }

        private void onGameplayStateChanged(GameplayState previousState, GameplayState currentState)
        {
            this.changeFsmState(currentState);
        }

        protected void Update()
        {
            this.m_fsm.update(Time.deltaTime);
        }

        public UnityEngine.MonoBehaviour MonoBehaviour
        {
            get
            {
                return this;
            }
        }
    }
}

