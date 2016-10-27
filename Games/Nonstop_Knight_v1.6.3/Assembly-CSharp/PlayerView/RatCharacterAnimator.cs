namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class RatCharacterAnimator : AbstractCharacterAnimator
    {
        protected override Dictionary<AbstractCharacterAnimator.Action, AbstractCharacterAnimator.ActionParameters> getActionClipLinks()
        {
            Dictionary<AbstractCharacterAnimator.Action, AbstractCharacterAnimator.ActionParameters> dictionary = new Dictionary<AbstractCharacterAnimator.Action, AbstractCharacterAnimator.ActionParameters>();
            string[] names = new string[] { "AttackMelee1", "AttackMelee2" };
            dictionary.Add(AbstractCharacterAnimator.Action.ATTACK_MELEE, new AbstractCharacterAnimator.ActionParameters(names, 0.1f, 0.8f, 0f));
            string[] textArray2 = new string[] { "AttackRanged1" };
            dictionary.Add(AbstractCharacterAnimator.Action.ATTACK_RANGED, new AbstractCharacterAnimator.ActionParameters(textArray2, 0.1f, 0.8f, 0f));
            return dictionary;
        }

        protected override Dictionary<AbstractCharacterAnimator.State, AbstractCharacterAnimator.StateParameters> getStateClipLinks()
        {
            Dictionary<AbstractCharacterAnimator.State, AbstractCharacterAnimator.StateParameters> dictionary = new Dictionary<AbstractCharacterAnimator.State, AbstractCharacterAnimator.StateParameters>();
            string[] names = new string[] { "Idle" };
            dictionary.Add(AbstractCharacterAnimator.State.IDLE, new AbstractCharacterAnimator.StateParameters(names, 0.2f));
            string[] textArray2 = new string[] { "Run" };
            dictionary.Add(AbstractCharacterAnimator.State.RUN, new AbstractCharacterAnimator.StateParameters(textArray2, 0.2f));
            return dictionary;
        }

        private void onCharacterMeleeAttackStarted(CharacterInstance sourceCharacter, CharacterInstance targetCharacter)
        {
            if (sourceCharacter == base.CharacterView.Character)
            {
                base.startAction(AbstractCharacterAnimator.Action.ATTACK_MELEE, base.CharacterView.Character.getAttackDuration() * 0.75f, true, -1, -1f);
            }
        }

        private void onCharacterRangedAttackStarted(CharacterInstance sourceCharacter, Vector3 targetWorldPt)
        {
            if (sourceCharacter == base.CharacterView.Character)
            {
                base.startAction(AbstractCharacterAnimator.Action.ATTACK_RANGED, 0.6f, true, -1, -1f);
            }
        }

        protected override void onDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnCharacterMeleeAttackStarted -= new GameLogic.Events.CharacterMeleeAttackStarted(this.onCharacterMeleeAttackStarted);
            eventBus.OnCharacterRangedAttackStarted -= new GameLogic.Events.CharacterRangedAttackStarted(this.onCharacterRangedAttackStarted);
        }

        protected override void onEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnCharacterMeleeAttackStarted += new GameLogic.Events.CharacterMeleeAttackStarted(this.onCharacterMeleeAttackStarted);
            eventBus.OnCharacterRangedAttackStarted += new GameLogic.Events.CharacterRangedAttackStarted(this.onCharacterRangedAttackStarted);
        }

        protected override bool onGetTargetAnimationState(out AbstractCharacterAnimator.State state, out float speed)
        {
            if ((base.CharacterView.Character.TargetCharacter == null) && (base.CharacterView.Character.ManualTargetPos == Vector3.zero))
            {
                speed = 1f;
                state = AbstractCharacterAnimator.State.IDLE;
                return true;
            }
            if (base.CharacterView.Character.Velocity != Vector3.zero)
            {
                speed = base.CharacterView.Character.MovementSpeed(true) * 0.2f;
                state = AbstractCharacterAnimator.State.RUN;
                return true;
            }
            return base.onGetTargetAnimationState(out state, out speed);
        }
    }
}

