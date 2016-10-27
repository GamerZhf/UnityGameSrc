namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class HeroCharacterAnimator : AbstractCharacterAnimator
    {
        private int m_meleeAttackClipIdx;

        protected override Dictionary<AbstractCharacterAnimator.Action, AbstractCharacterAnimator.ActionParameters> getActionClipLinks()
        {
            Dictionary<AbstractCharacterAnimator.Action, AbstractCharacterAnimator.ActionParameters> dictionary = new Dictionary<AbstractCharacterAnimator.Action, AbstractCharacterAnimator.ActionParameters>();
            string[] names = new string[] { "Attack1", "Attack2", "Attack3", "Attack4" };
            dictionary.Add(AbstractCharacterAnimator.Action.ATTACK_MELEE, new AbstractCharacterAnimator.ActionParameters(names, 0.1f, 0.8f, 0f));
            string[] textArray2 = new string[] { "Attack4" };
            dictionary.Add(AbstractCharacterAnimator.Action.ATTACK_RANGED, new AbstractCharacterAnimator.ActionParameters(textArray2, 0.1f, 0.8f, 0f));
            string[] textArray3 = new string[] { "Leap" };
            dictionary.Add(AbstractCharacterAnimator.Action.SKILL_LEAP, new AbstractCharacterAnimator.ActionParameters(textArray3, 0.01f, 0.8f, 0f));
            string[] textArray4 = new string[] { "Cast" };
            dictionary.Add(AbstractCharacterAnimator.Action.SKILL_CLONE, new AbstractCharacterAnimator.ActionParameters(textArray4, 0.05f, 0.8f, 0f));
            string[] textArray5 = new string[] { "Death" };
            dictionary.Add(AbstractCharacterAnimator.Action.DEATH, new AbstractCharacterAnimator.ActionParameters(textArray5, 0.03f, 0.8f, 0f));
            string[] textArray6 = new string[] { "Cheer", "Cheer2", "Cheer3" };
            dictionary.Add(AbstractCharacterAnimator.Action.CHEER, new AbstractCharacterAnimator.ActionParameters(textArray6, 0.1f, 0.8f, 0f));
            string[] textArray7 = new string[] { "TutorialEnter" };
            dictionary.Add(AbstractCharacterAnimator.Action.TUTORIAL_ENTER, new AbstractCharacterAnimator.ActionParameters(textArray7, 0.1f, 0.8f, 0f));
            return dictionary;
        }

        protected override AbstractCharacterAnimator.State getMenuViewDefaultState()
        {
            return ((PlayerView.Binder.MenuSystem.topmostActiveMenuType() != MenuType.SpeechBubble) ? AbstractCharacterAnimator.State.IDLE : AbstractCharacterAnimator.State.TUTORIAL_STAY);
        }

        protected override Dictionary<AbstractCharacterAnimator.State, AbstractCharacterAnimator.StateParameters> getStateClipLinks()
        {
            Dictionary<AbstractCharacterAnimator.State, AbstractCharacterAnimator.StateParameters> dictionary = new Dictionary<AbstractCharacterAnimator.State, AbstractCharacterAnimator.StateParameters>();
            string[] names = new string[] { "Idle" };
            dictionary.Add(AbstractCharacterAnimator.State.IDLE, new AbstractCharacterAnimator.StateParameters(names, 0.2f));
            string[] textArray2 = new string[] { "Run" };
            dictionary.Add(AbstractCharacterAnimator.State.RUN, new AbstractCharacterAnimator.StateParameters(textArray2, 0.2f));
            string[] textArray3 = new string[] { "TutorialStay" };
            dictionary.Add(AbstractCharacterAnimator.State.TUTORIAL_STAY, new AbstractCharacterAnimator.StateParameters(textArray3, 0.2f));
            return dictionary;
        }

        protected override void onAwake()
        {
        }

        private void onCharacterKilled(CharacterInstance target, CharacterInstance killer, bool critted, SkillType fromSkill)
        {
            if (!base.CharacterView.IsMenuView)
            {
                if (((target == base.CharacterView.Character) && !target.IsSupport) && target.IsDead)
                {
                    base.startAction(AbstractCharacterAnimator.Action.DEATH, 0.6f, true, -1, -1f);
                }
                else if ((target.IsBoss && (killer != null)) && (!GameLogic.Binder.FrenzySystem.isFrenzyActive() && (GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.numberOfBossesAlive() == 0)))
                {
                    base.startAction(AbstractCharacterAnimator.Action.CHEER, 3.1f, true, -1, -1f);
                }
            }
        }

        private void onCharacterMeleeAttackStarted(CharacterInstance sourceCharacter, CharacterInstance targetCharacter)
        {
            if (!base.CharacterView.IsMenuView && (sourceCharacter == base.CharacterView.Character))
            {
                base.startAction(AbstractCharacterAnimator.Action.ATTACK_MELEE, base.CharacterView.Character.getAttackDuration() * 1.2f, true, this.m_meleeAttackClipIdx, -1f);
                if ((this.m_meleeAttackClipIdx % 2) == 0)
                {
                    this.m_meleeAttackClipIdx = (UnityEngine.Random.Range(0, 2) != 0) ? 3 : 1;
                }
                else
                {
                    this.m_meleeAttackClipIdx = (UnityEngine.Random.Range(0, 2) != 0) ? 2 : 0;
                }
            }
        }

        private void onCharacterRangedAttackStarted(CharacterInstance sourceCharacter, Vector3 targetWorldPt)
        {
            if (!base.CharacterView.IsMenuView && (sourceCharacter == base.CharacterView.Character))
            {
                base.startAction(AbstractCharacterAnimator.Action.ATTACK_RANGED, base.CharacterView.Character.getAttackDuration() * 0.4f, true, -1, -1f);
            }
        }

        private void onCharacterSkillActivated(CharacterInstance character, SkillType skillType, float buildupTime, SkillExecutionStats executionStats)
        {
            if (!base.CharacterView.IsMenuView && ((character == base.CharacterView.Character) && (skillType == SkillType.Slam)))
            {
                base.startAction(AbstractCharacterAnimator.Action.SKILL_LEAP, 0.4f, true, -1, -1f);
            }
        }

        private void onCharacterSkillBuildupCompleted(CharacterInstance c, SkillType skillType, SkillExecutionStats executionStats)
        {
            if (!base.CharacterView.IsMenuView && (c == base.CharacterView.Character))
            {
                if (skillType == SkillType.Leap)
                {
                    float duration = 0.66f;
                    if (executionStats.MovementDurationDynamic > 0f)
                    {
                        duration *= executionStats.MovementDurationDynamic / ConfigSkills.Leap.LeapDuration;
                    }
                    base.startAction(AbstractCharacterAnimator.Action.SKILL_LEAP, duration, true, -1, -1f);
                }
                else if (skillType == SkillType.Dash)
                {
                    base.startAction(AbstractCharacterAnimator.Action.ATTACK_MELEE, ConfigSkills.SHARED_DATA[SkillType.Dash].BuildupTime * 0.3f, true, 1, 1f);
                }
                else if (skillType == SkillType.Implosion)
                {
                    base.startAction(AbstractCharacterAnimator.Action.SKILL_LEAP, ConfigSkills.Implosion.LeapDuration * 1.11f, true, -1, -1f);
                }
                else if (skillType == SkillType.Clone)
                {
                    base.startAction(AbstractCharacterAnimator.Action.SKILL_CLONE, 0.5f, true, -1, -1f);
                }
            }
        }

        private void onCharacterSkillExecutionMidpoint(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats)
        {
            if (!base.CharacterView.IsMenuView && (character == base.CharacterView.Character))
            {
                if (skillType == SkillType.Dash)
                {
                    base.startAction(AbstractCharacterAnimator.Action.ATTACK_MELEE, 0.2f, true, 0, -1f);
                }
                else if ((skillType == SkillType.Omnislash) && (executionStats.EnemiesAround > 0))
                {
                    base.startAction(AbstractCharacterAnimator.Action.ATTACK_MELEE, 0.2f, true, 0, -1f);
                }
            }
        }

        protected override void onDisable()
        {
            GameLogic.Binder.EventBus.OnCharacterMeleeAttackStarted -= new GameLogic.Events.CharacterMeleeAttackStarted(this.onCharacterMeleeAttackStarted);
            GameLogic.Binder.EventBus.OnCharacterRangedAttackStarted -= new GameLogic.Events.CharacterRangedAttackStarted(this.onCharacterRangedAttackStarted);
            GameLogic.Binder.EventBus.OnCharacterSkillActivated -= new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            GameLogic.Binder.EventBus.OnCharacterSkillBuildupCompleted -= new GameLogic.Events.CharacterSkillBuildupCompleted(this.onCharacterSkillBuildupCompleted);
            GameLogic.Binder.EventBus.OnCharacterSkillExecutionMidpoint -= new GameLogic.Events.CharacterSkillExecutionMidpoint(this.onCharacterSkillExecutionMidpoint);
            GameLogic.Binder.EventBus.OnGameplayEnded -= new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            GameLogic.Binder.EventBus.OnCharacterKilled -= new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            PlayerView.Binder.EventBus.OnMenuShowStarted -= new PlayerView.Events.MenuShowStarted(this.onMenuShowStarted);
        }

        protected override void onEnable()
        {
            GameLogic.Binder.EventBus.OnCharacterMeleeAttackStarted += new GameLogic.Events.CharacterMeleeAttackStarted(this.onCharacterMeleeAttackStarted);
            GameLogic.Binder.EventBus.OnCharacterRangedAttackStarted += new GameLogic.Events.CharacterRangedAttackStarted(this.onCharacterRangedAttackStarted);
            GameLogic.Binder.EventBus.OnCharacterSkillActivated += new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            GameLogic.Binder.EventBus.OnCharacterSkillBuildupCompleted += new GameLogic.Events.CharacterSkillBuildupCompleted(this.onCharacterSkillBuildupCompleted);
            GameLogic.Binder.EventBus.OnCharacterSkillExecutionMidpoint += new GameLogic.Events.CharacterSkillExecutionMidpoint(this.onCharacterSkillExecutionMidpoint);
            GameLogic.Binder.EventBus.OnGameplayEnded += new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            GameLogic.Binder.EventBus.OnCharacterKilled += new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            PlayerView.Binder.EventBus.OnMenuShowStarted += new PlayerView.Events.MenuShowStarted(this.onMenuShowStarted);
        }

        private void onGameplayEnded(ActiveDungeon activeDungeon)
        {
            if (!base.CharacterView.IsMenuView)
            {
                base.stopAction();
            }
        }

        protected override bool onGetTargetAnimationState(out AbstractCharacterAnimator.State state, out float speed)
        {
            speed = 1f;
            state = AbstractCharacterAnimator.State.IDLE;
            if (base.CharacterView.Character.isAttacking())
            {
                return false;
            }
            if (base.CharacterView.Character.isExecutingSkill(SkillType.Whirlwind) || base.CharacterView.Character.isExecutingSkill(SkillType.Dash))
            {
                return false;
            }
            if ((base.CharacterView.Character.TargetCharacter == null) && (base.CharacterView.Character.ManualTargetPos == Vector3.zero))
            {
                state = AbstractCharacterAnimator.State.IDLE;
                return true;
            }
            if (base.CharacterView.Character.ExternallyControlled)
            {
                if (base.CharacterView.Character.SpinningAround)
                {
                    state = AbstractCharacterAnimator.State.RUN;
                    speed = 2f;
                    return true;
                }
                state = AbstractCharacterAnimator.State.IDLE;
                return true;
            }
            if (base.CharacterView.Character.Velocity != Vector3.zero)
            {
                speed = Mathf.Clamp(1.15f + ((base.CharacterView.Character.MovementSpeed(true) * 0.05f) * Easing.Apply(base.CharacterView.Character.RunAccelerationTimer.normalizedProgress(), ConfigGameplay.CHARACTER_FULLSPEED_ACCELERATION_EASING)), 0.1f, float.MaxValue);
                state = AbstractCharacterAnimator.State.RUN;
                return true;
            }
            return base.onGetTargetAnimationState(out state, out speed);
        }

        private void onMenuShowStarted(MenuType targetMenuType)
        {
            if (base.CharacterView.IsMenuView && (targetMenuType == MenuType.SpeechBubble))
            {
                base.startAction(AbstractCharacterAnimator.Action.TUTORIAL_ENTER, 1.8f, true, -1, -1f);
            }
        }
    }
}

