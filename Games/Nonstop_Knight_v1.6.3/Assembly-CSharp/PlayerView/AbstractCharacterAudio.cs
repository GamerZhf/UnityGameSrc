namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class AbstractCharacterAudio : MonoBehaviour
    {
        [CompilerGenerated]
        private AudioGroupType <AggroAudioGroupType>k__BackingField;
        [CompilerGenerated]
        private List<AudioSourceType> <AggroAudioSourceTypes>k__BackingField;
        [CompilerGenerated]
        private PlayerView.CharacterView <CharacterView>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;

        protected AbstractCharacterAudio()
        {
        }

        protected void Awake()
        {
            this.Tm = base.transform;
        }

        public void cleanup()
        {
            this.onCleanup();
        }

        public void initialize(PlayerView.CharacterView characterView)
        {
            this.CharacterView = characterView;
            this.onInitialize();
        }

        private void onCharacterMeleeAttackStarted(CharacterInstance sourceCharacter, CharacterInstance targetCharacter)
        {
            if (sourceCharacter == this.CharacterView.Character)
            {
                this.playAggro();
            }
        }

        private void onCharacterRangedAttackStarted(CharacterInstance sourceCharacter, Vector3 targetWorldPt)
        {
            if (sourceCharacter == this.CharacterView.Character)
            {
                this.playAggro();
            }
        }

        private void onCharacterTargetUpdated(CharacterInstance character, CharacterInstance oldTarget)
        {
            if ((character == this.CharacterView.Character) && (this.CharacterView.Character.TargetCharacter != null))
            {
                this.playAggro();
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
            GameLogic.Binder.EventBus.OnCharacterTargetUpdated -= new GameLogic.Events.CharacterTargetUpdated(this.onCharacterTargetUpdated);
            GameLogic.Binder.EventBus.OnCharacterMeleeAttackStarted -= new GameLogic.Events.CharacterMeleeAttackStarted(this.onCharacterMeleeAttackStarted);
            GameLogic.Binder.EventBus.OnCharacterRangedAttackStarted -= new GameLogic.Events.CharacterRangedAttackStarted(this.onCharacterRangedAttackStarted);
            this.onDisable();
        }

        protected virtual void onEnable()
        {
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnCharacterTargetUpdated += new GameLogic.Events.CharacterTargetUpdated(this.onCharacterTargetUpdated);
            GameLogic.Binder.EventBus.OnCharacterMeleeAttackStarted += new GameLogic.Events.CharacterMeleeAttackStarted(this.onCharacterMeleeAttackStarted);
            GameLogic.Binder.EventBus.OnCharacterRangedAttackStarted += new GameLogic.Events.CharacterRangedAttackStarted(this.onCharacterRangedAttackStarted);
            this.onEnable();
        }

        protected virtual void onInitialize()
        {
        }

        protected virtual void onUpdate(float dt)
        {
        }

        private void playAggro()
        {
            AudioSystem.PlaybackParameters parameters3;
            float num2 = 1f;
            float num3 = 1f;
            if (this.CharacterView.Character.IsBoss)
            {
                if (this.CharacterView.Character.Type == GameLogic.CharacterType.Yeti)
                {
                    num2 = 0.9f;
                    num3 = 1.55f;
                }
                else
                {
                    num2 = 0.6f;
                    num3 = 1.33f;
                }
            }
            if (this.AggroAudioSourceTypes != null)
            {
                AudioSourceType randomValueFromList = LangUtil.GetRandomValueFromList<AudioSourceType>(this.AggroAudioSourceTypes);
                int num4 = PlayerView.Binder.AudioSystem.numberOfSfxAudioSourcePlaying(randomValueFromList);
                bool flag = UnityEngine.Random.Range(0, num4 + 4) == 0;
                if (this.CharacterView.Character.IsBoss)
                {
                    flag = true;
                }
                if (flag)
                {
                    parameters3 = new AudioSystem.PlaybackParameters();
                    parameters3.VolumeMultiplier = num3;
                    parameters3.PitchMin = 0.8f * num2;
                    parameters3.PitchMax = 1.2f * num2;
                    parameters3.DelayMax = 0.2f;
                    parameters3.FixedWorldPt = this.CharacterView.Character.PhysicsBody.Transform.position;
                    AudioSystem.PlaybackParameters pp = parameters3;
                    PlayerView.Binder.AudioSystem.playSfx(randomValueFromList, pp);
                }
            }
            if (this.AggroAudioGroupType != AudioGroupType.UNSPECIFIED)
            {
                AudioGroupType aggroAudioGroupType = this.AggroAudioGroupType;
                int num5 = PlayerView.Binder.AudioSystem.numberOfSfxAudioGroupPlaying(aggroAudioGroupType);
                bool flag2 = UnityEngine.Random.Range(0, num5 + 4) == 0;
                if (this.CharacterView.Character.IsBoss)
                {
                    flag2 = true;
                }
                if (flag2)
                {
                    parameters3 = new AudioSystem.PlaybackParameters();
                    parameters3.VolumeMultiplier = num3;
                    parameters3.PitchMin = 0.9f * num2;
                    parameters3.PitchMax = 1.1f * num2;
                    parameters3.DelayMax = 0.2f;
                    parameters3.FixedWorldPt = this.CharacterView.Character.PhysicsBody.Transform.position;
                    AudioSystem.PlaybackParameters parameters2 = parameters3;
                    PlayerView.Binder.AudioSystem.playSfxGrpLimitPolyphony(aggroAudioGroupType, parameters2, 2);
                }
            }
        }

        protected void Update()
        {
            if (this.CharacterView.Character != null)
            {
                this.onUpdate(Time.deltaTime);
            }
        }

        public AudioGroupType AggroAudioGroupType
        {
            [CompilerGenerated]
            get
            {
                return this.<AggroAudioGroupType>k__BackingField;
            }
            [CompilerGenerated]
            protected set
            {
                this.<AggroAudioGroupType>k__BackingField = value;
            }
        }

        public List<AudioSourceType> AggroAudioSourceTypes
        {
            [CompilerGenerated]
            get
            {
                return this.<AggroAudioSourceTypes>k__BackingField;
            }
            [CompilerGenerated]
            protected set
            {
                this.<AggroAudioSourceTypes>k__BackingField = value;
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

        public Transform Tm
        {
            [CompilerGenerated]
            get
            {
                return this.<Tm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Tm>k__BackingField = value;
            }
        }
    }
}

