namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public abstract class AbstractCharacterAnimator : MonoBehaviour
    {
        [CompilerGenerated]
        private Transform <AnimationTm>k__BackingField;
        [CompilerGenerated]
        private PlayerView.CharacterView <CharacterView>k__BackingField;
        [CompilerGenerated]
        private bool <UseUnscaledTime>k__BackingField;
        private Dictionary<Action, List<string>> m_actionClipNames;
        private Dictionary<Action, float> m_actionDetransitionPercentages;
        private float m_actionDuration;
        private float m_actionNormalizedDetransitionTime;
        private Dictionary<Action, float> m_actionRandomDelays;
        private float m_actionTime;
        private Dictionary<Action, float> m_actionTransitionDurations;
        private Dictionary<State, List<string>> m_activeStateClips = new Dictionary<State, List<string>>();
        private Animation m_animation;
        private List<string> m_animationStateNames = new List<string>();
        private List<AnimationState> m_animationStates = new List<AnimationState>();
        protected Action m_currentAction;
        private DelayedAction m_delayedAction;
        private Dictionary<State, float> m_stateTransitionDurations;
        protected State m_targetState;
        private float m_transitionDuration;
        private Dictionary<string, float> m_transitionStartingWeight;
        private Dictionary<string, float> m_transitionTargetWeight;
        private float m_transitionTime;

        public event AnimationActionTriggered OnAnimationActionTriggered;

        public event AnimationStateChanged OnAnimationStateChanged;

        protected AbstractCharacterAnimator()
        {
        }

        protected void Awake()
        {
            this.m_animation = this.getAnimationComponent();
            this.AnimationTm = this.m_animation.transform;
            this.m_activeStateClips = new Dictionary<State, List<string>>();
            this.m_actionClipNames = new Dictionary<Action, List<string>>();
            this.m_actionTransitionDurations = new Dictionary<Action, float>();
            this.m_actionDetransitionPercentages = new Dictionary<Action, float>();
            this.m_actionRandomDelays = new Dictionary<Action, float>();
            this.m_stateTransitionDurations = new Dictionary<State, float>();
            foreach (KeyValuePair<State, StateParameters> pair in this.getStateClipLinks())
            {
                StateParameters parameters = pair.Value;
                this.registerState(pair.Key, parameters.names, parameters.transitionDuration);
            }
            foreach (KeyValuePair<Action, ActionParameters> pair2 in this.getActionClipLinks())
            {
                ActionParameters parameters2 = pair2.Value;
                this.registerAction(pair2.Key, parameters2.names, parameters2.transitionDuration, parameters2.detransitionPercentage, parameters2.randomDelay);
            }
            this.m_transitionStartingWeight = new Dictionary<string, float>();
            this.m_transitionTargetWeight = new Dictionary<string, float>();
            for (int i = 0; i < this.m_animationStateNames.Count; i++)
            {
                string key = this.m_animationStateNames[i];
                if (!this.m_transitionStartingWeight.ContainsKey(key))
                {
                    this.m_transitionStartingWeight.Add(key, 0f);
                }
                if (!this.m_transitionTargetWeight.ContainsKey(key))
                {
                    this.m_transitionTargetWeight.Add(key, 0f);
                }
            }
            this.onAwake();
        }

        public void cleanup()
        {
            this.onCleanup();
        }

        private void clearTargetWeights()
        {
            for (int i = 0; i < this.m_animationStateNames.Count; i++)
            {
                string str = this.m_animationStateNames[i];
                this.m_transitionTargetWeight[str] = 0f;
            }
        }

        private void copyStartingWeights()
        {
            for (int i = 0; i < this.m_animationStates.Count; i++)
            {
                AnimationState state = this.m_animationStates[i];
                this.m_transitionStartingWeight[this.m_animationStateNames[i]] = state.weight;
            }
        }

        public void freeze()
        {
            for (int i = 0; i < this.m_animationStates.Count; i++)
            {
                this.m_animationStates[i].enabled = false;
            }
        }

        protected virtual Dictionary<Action, ActionParameters> getActionClipLinks()
        {
            return new Dictionary<Action, ActionParameters>();
        }

        public float getActionDuration(Action action)
        {
            return this.m_animation[this.m_actionClipNames[action][0]].length;
        }

        protected virtual Animation getAnimationComponent()
        {
            return base.GetComponentInChildren<Animation>();
        }

        private float getDeltaTime()
        {
            if (this.UseUnscaledTime)
            {
                return (Time.deltaTime / Time.timeScale);
            }
            return Time.deltaTime;
        }

        protected virtual State getMenuViewDefaultState()
        {
            return State.IDLE;
        }

        protected virtual Dictionary<State, StateParameters> getStateClipLinks()
        {
            return new Dictionary<State, StateParameters>();
        }

        private bool getTargetAnimationState(out State state, out float speed)
        {
            speed = 1f;
            state = State.IDLE;
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (this.CharacterView.IsMenuView)
            {
                state = this.CharacterView.Animator.getMenuViewDefaultState();
                return true;
            }
            if (((this.CharacterView == null) || (this.CharacterView.Character == null)) || (activeDungeon == null))
            {
                state = State.IDLE;
                return true;
            }
            if (this.CharacterView.Character.IsDead)
            {
                return false;
            }
            if (this.CharacterView.Character.Stunned)
            {
                return false;
            }
            if (this.CharacterView.Character.isAttacking() && (this.m_currentAction != Action.NONE))
            {
                return false;
            }
            return this.onGetTargetAnimationState(out state, out speed);
        }

        public bool hasAction(Action action)
        {
            return this.m_actionClipNames.ContainsKey(action);
        }

        public bool hasState(State state)
        {
            return this.m_activeStateClips.ContainsKey(state);
        }

        public void initialize(PlayerView.CharacterView characterView)
        {
            this.CharacterView = characterView;
            this.onInitialize();
        }

        protected virtual void onAwake()
        {
        }

        private void onCharacterInterrupted(CharacterInstance c, bool stopSkills)
        {
            if (!this.CharacterView.IsMenuView && (c == this.CharacterView.Character))
            {
                this.stopAction();
            }
        }

        private void onCharacterStunConditionChanged(CharacterInstance c)
        {
            if ((c == this.CharacterView.Character) && c.Stunned)
            {
                this.freeze();
            }
        }

        protected virtual void onCleanup()
        {
        }

        protected virtual void onDisable()
        {
        }

        protected void OnDisable()
        {
            if (this.m_animation != null)
            {
                this.m_animation.enabled = false;
            }
            GameLogic.Binder.EventBus.OnCharacterStunConditionChanged -= new GameLogic.Events.CharacterStunConditionChanged(this.onCharacterStunConditionChanged);
            this.onDisable();
        }

        protected virtual void onEnable()
        {
        }

        protected void OnEnable()
        {
            this.m_targetState = State.NONE;
            this.stopAction();
            this.m_animation.enabled = true;
            this.setDefaultPose();
            GameLogic.Binder.EventBus.OnCharacterStunConditionChanged += new GameLogic.Events.CharacterStunConditionChanged(this.onCharacterStunConditionChanged);
            this.onEnable();
        }

        protected virtual bool onGetTargetAnimationState(out State state, out float speed)
        {
            speed = 1f;
            state = State.IDLE;
            return true;
        }

        protected virtual void onInitialize()
        {
        }

        protected virtual void onUpdate(float dt)
        {
        }

        private void registerAction(Action action, string[] clipNames, float transitionDuration, float detransitionPercentage, float delay)
        {
            this.m_actionClipNames[action] = new List<string>();
            for (int i = 0; i < clipNames.Length; i++)
            {
                string name = clipNames[i];
                if (this.m_animation.GetClip(name) != null)
                {
                    AnimationState item = this.m_animation[name];
                    item.wrapMode = WrapMode.ClampForever;
                    this.m_animationStates.Add(item);
                    this.m_animationStateNames.Add(name);
                    this.m_actionTransitionDurations[action] = transitionDuration;
                    this.m_actionClipNames[action].Add(name);
                }
            }
            this.m_actionDetransitionPercentages[action] = detransitionPercentage;
            this.m_actionRandomDelays[action] = delay;
        }

        private void registerState(State state, string[] clipNames, float transitionDuration)
        {
            this.m_activeStateClips[state] = new List<string>(clipNames);
            for (int i = 0; i < clipNames.Length; i++)
            {
                string name = clipNames[i];
                if (this.m_animation.GetClip(name) != null)
                {
                    AnimationState item = this.m_animation[name];
                    item.wrapMode = WrapMode.Loop;
                    this.m_animationStates.Add(item);
                    this.m_animationStateNames.Add(name);
                    this.m_stateTransitionDurations[state] = transitionDuration;
                }
            }
        }

        private void resetTransitionTimer(float duration)
        {
            this.m_transitionTime = 0f;
            this.m_transitionDuration = duration;
        }

        private void setDefaultPose()
        {
            if ((this.m_activeStateClips.Count != 0) && (this.m_activeStateClips[State.IDLE].Count != 0))
            {
                string name = this.m_activeStateClips[1][0];
                if (this.m_animation.GetClip(name) != null)
                {
                    this.m_animation[name].enabled = true;
                    this.m_animation[name].normalizedTime = 0.3f;
                    this.m_animation[name].speed = 1f;
                    this.m_transitionTargetWeight[name] = 1f;
                    this.updateWeights(1f);
                }
            }
        }

        public void startAction(Action action, float duration, [Optional, DefaultParameterValue(true)] bool overrideSelf, [Optional, DefaultParameterValue(-1)] int clipIndex, [Optional, DefaultParameterValue(-1f)] float singleFrameTargetNormalizedTime)
        {
            if (!this.m_actionDetransitionPercentages.ContainsKey(action))
            {
                Debug.LogWarning(this.m_animation.name + " doesn't have action " + action);
            }
            else
            {
                float normalizedDetransitionTime = this.m_actionDetransitionPercentages[action];
                bool overrideOthers = true;
                if (this.m_actionRandomDelays[action] > 0f)
                {
                    this.m_delayedAction = new DelayedAction(UnityEngine.Random.value * this.m_actionRandomDelays[action], action, normalizedDetransitionTime, overrideSelf, overrideOthers, duration, clipIndex, singleFrameTargetNormalizedTime);
                }
                else
                {
                    this.startAction(action, normalizedDetransitionTime, overrideSelf, overrideOthers, duration, clipIndex, singleFrameTargetNormalizedTime);
                }
            }
        }

        private void startAction(Action action, float normalizedDetransitionTime, bool overrideSelf, bool overrideOthers, float duration, [Optional, DefaultParameterValue(-1)] int clipIndex, [Optional, DefaultParameterValue(-1f)] float singleFrameTargetNormalizedTime)
        {
            if (((this.m_currentAction != action) || overrideSelf) && ((this.m_currentAction == Action.NONE) || overrideOthers))
            {
                string name = string.Empty;
                if (clipIndex == -1)
                {
                    name = LangUtil.GetRandomValueFromList<string>(this.m_actionClipNames[action]);
                }
                else
                {
                    name = this.m_actionClipNames[action][clipIndex];
                }
                if (this.m_animation.GetClip(name) == null)
                {
                    Debug.LogError("Clip doesn't exist: " + name);
                }
                else
                {
                    float num;
                    this.m_currentAction = action;
                    if (singleFrameTargetNormalizedTime != -1f)
                    {
                        num = duration;
                    }
                    else
                    {
                        num = this.m_actionTransitionDurations[action];
                    }
                    this.resetTransitionTimer(num);
                    this.copyStartingWeights();
                    this.clearTargetWeights();
                    this.m_transitionTargetWeight[name] = 1f;
                    if (!this.m_animation.enabled)
                    {
                        Debug.LogWarning("Animation component was disabled, enabling it");
                        this.m_animation.enabled = true;
                    }
                    this.m_animation[name].enabled = true;
                    if (singleFrameTargetNormalizedTime != -1f)
                    {
                        this.m_animation[name].time = singleFrameTargetNormalizedTime;
                        this.m_animation[name].speed = 0f;
                    }
                    else
                    {
                        float num2 = this.m_animation[name].length / duration;
                        if (this.UseUnscaledTime)
                        {
                            num2 /= Time.timeScale;
                        }
                        this.m_animation[name].time = 0f;
                        this.m_animation[name].speed = num2;
                    }
                    this.m_actionTime = 0f;
                    this.m_actionNormalizedDetransitionTime = normalizedDetransitionTime;
                    this.m_actionDuration = duration;
                    this.m_targetState = State.NONE;
                    if (this.OnAnimationActionTriggered != null)
                    {
                        this.OnAnimationActionTriggered(action);
                    }
                }
            }
        }

        private float stateTransitionDuration(State state)
        {
            return this.m_stateTransitionDurations[state];
        }

        public void stopAction()
        {
            this.m_currentAction = Action.NONE;
        }

        public void Update()
        {
            if (this.m_animation.gameObject.activeSelf && this.m_animation.enabled)
            {
                float dt = this.getDeltaTime();
                if (this.m_delayedAction != null)
                {
                    this.m_delayedAction.delay -= dt;
                    if (this.m_delayedAction.delay <= 0f)
                    {
                        this.startAction(this.m_delayedAction.action, this.m_delayedAction.normalizedDetransitionTime, this.m_delayedAction.overrideSelf, this.m_delayedAction.overrideOthers, this.m_delayedAction.duration, this.m_delayedAction.clipIndex, this.m_delayedAction.singleFrameTargetNormalizedTime);
                        this.m_delayedAction = null;
                    }
                }
                if (this.m_currentAction != Action.NONE)
                {
                    if (this.NormalizedTransitionTime == 1f)
                    {
                        if (this.m_actionTime > (this.m_actionDuration * this.m_actionNormalizedDetransitionTime))
                        {
                            this.m_currentAction = Action.NONE;
                        }
                        this.m_actionTime += dt;
                    }
                }
                else
                {
                    State state;
                    float num2;
                    bool flag = this.getTargetAnimationState(out state, out num2);
                    if (this.UseUnscaledTime)
                    {
                        num2 /= Time.timeScale;
                    }
                    if (flag && (this.m_targetState != state))
                    {
                        this.m_targetState = state;
                        this.copyStartingWeights();
                        this.clearTargetWeights();
                        this.resetTransitionTimer(this.stateTransitionDuration(state));
                        float num3 = UnityEngine.Random.Range((float) 0f, (float) 1f);
                        for (int i = 0; i < this.m_activeStateClips[state].Count; i++)
                        {
                            string name = this.m_activeStateClips[state][i];
                            if (this.m_animation.GetClip(name) == null)
                            {
                                Debug.LogError("Clip doesn't exist: " + name);
                            }
                            else
                            {
                                this.m_animation[name].enabled = true;
                                this.m_animation[name].normalizedTime = num3;
                                this.m_animation[name].speed = num2;
                                this.m_transitionTargetWeight[name] = 1f;
                            }
                        }
                        if (this.OnAnimationStateChanged != null)
                        {
                            this.OnAnimationStateChanged(state);
                        }
                    }
                }
                this.updateWeights(this.NormalizedTransitionTime);
                this.m_transitionTime += dt;
                this.onUpdate(dt);
            }
        }

        private void updateWeights(float t)
        {
            for (int i = 0; i < this.m_animationStates.Count; i++)
            {
                AnimationState state = this.m_animationStates[i];
                float num2 = this.m_transitionStartingWeight[this.m_animationStateNames[i]];
                float num3 = this.m_transitionTargetWeight[this.m_animationStateNames[i]];
                state.weight = num2 + (t * (num3 - num2));
            }
        }

        public Transform AnimationTm
        {
            [CompilerGenerated]
            get
            {
                return this.<AnimationTm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<AnimationTm>k__BackingField = value;
            }
        }

        public PlayerView.CharacterView CharacterView
        {
            [CompilerGenerated]
            get
            {
                return this.<CharacterView>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CharacterView>k__BackingField = value;
            }
        }

        public Action CurrentAction
        {
            get
            {
                return this.m_currentAction;
            }
        }

        public float NormalizedTransitionTime
        {
            get
            {
                if (this.m_transitionDuration == 0f)
                {
                    return 1f;
                }
                return Mathf.Clamp01(this.m_transitionTime / this.m_transitionDuration);
            }
        }

        public State TargetState
        {
            get
            {
                return this.m_targetState;
            }
        }

        public bool UseUnscaledTime
        {
            [CompilerGenerated]
            get
            {
                return this.<UseUnscaledTime>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<UseUnscaledTime>k__BackingField = value;
            }
        }

        public enum Action
        {
            NONE,
            ATTACK_MELEE,
            ATTACK_RANGED,
            HIT,
            DEATH,
            SKILL_LEAP,
            SKILL_BLAST,
            SKILL_CLONE,
            SPIKES_IN,
            CHEER,
            TUTORIAL_ENTER,
            IDLE_ACT
        }

        protected class ActionParameters
        {
            public float detransitionPercentage;
            public string[] names;
            public float randomDelay;
            public float transitionDuration;

            public ActionParameters(string[] names, float transitionDuration, float detransitionPercentage, float randomDelay)
            {
                this.names = names;
                this.transitionDuration = transitionDuration;
                this.detransitionPercentage = detransitionPercentage;
                this.randomDelay = randomDelay;
            }
        }

        public delegate void AnimationActionTriggered(AbstractCharacterAnimator.Action action);

        public delegate void AnimationStateChanged(AbstractCharacterAnimator.State newState);

        private class DelayedAction
        {
            public AbstractCharacterAnimator.Action action;
            public int clipIndex;
            public float delay;
            public float duration;
            public float normalizedDetransitionTime;
            public bool overrideOthers;
            public bool overrideSelf;
            public float singleFrameTargetNormalizedTime;

            public DelayedAction(float delay, AbstractCharacterAnimator.Action action, float normalizedDetransitionTime, bool overrideSelf, bool overrideOthers, float duration, int clipIndex, float singleFrameTargetNormalizedTime)
            {
                this.delay = delay;
                this.action = action;
                this.normalizedDetransitionTime = normalizedDetransitionTime;
                this.overrideSelf = overrideSelf;
                this.overrideOthers = overrideOthers;
                this.duration = duration;
                this.clipIndex = clipIndex;
                this.singleFrameTargetNormalizedTime = singleFrameTargetNormalizedTime;
            }
        }

        public enum State
        {
            NONE,
            IDLE,
            IDLE_MENU,
            RUN,
            LAY_STILL,
            TUTORIAL_STAY
        }

        protected class StateParameters
        {
            public string[] names;
            public float transitionDuration;

            public StateParameters(string[] names, float transitionDuration)
            {
                this.names = names;
                this.transitionDuration = transitionDuration;
            }
        }
    }
}

