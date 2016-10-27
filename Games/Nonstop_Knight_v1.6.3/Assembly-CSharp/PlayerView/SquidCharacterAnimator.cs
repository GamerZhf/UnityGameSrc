namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class SquidCharacterAnimator : AbstractCharacterAnimator
    {
        private float m_nextMenuAnimTime;

        protected override Dictionary<AbstractCharacterAnimator.Action, AbstractCharacterAnimator.ActionParameters> getActionClipLinks()
        {
            Dictionary<AbstractCharacterAnimator.Action, AbstractCharacterAnimator.ActionParameters> dictionary = new Dictionary<AbstractCharacterAnimator.Action, AbstractCharacterAnimator.ActionParameters>();
            string[] names = new string[] { "AttackRanged1" };
            dictionary.Add(AbstractCharacterAnimator.Action.ATTACK_RANGED, new AbstractCharacterAnimator.ActionParameters(names, 0.1f, 0.8f, 0f));
            return dictionary;
        }

        protected override Dictionary<AbstractCharacterAnimator.State, AbstractCharacterAnimator.StateParameters> getStateClipLinks()
        {
            Dictionary<AbstractCharacterAnimator.State, AbstractCharacterAnimator.StateParameters> dictionary = new Dictionary<AbstractCharacterAnimator.State, AbstractCharacterAnimator.StateParameters>();
            string[] names = new string[] { "Fly" };
            dictionary.Add(AbstractCharacterAnimator.State.IDLE, new AbstractCharacterAnimator.StateParameters(names, 0.2f));
            return dictionary;
        }

        protected override void onAwake()
        {
        }

        private void onCharacterRangedAttackStarted(CharacterInstance sourceCharacter, Vector3 targetWorldPt)
        {
            if (sourceCharacter == base.CharacterView.Character)
            {
                base.startAction(AbstractCharacterAnimator.Action.ATTACK_RANGED, base.CharacterView.Character.getAttackDuration() * 0.6f, true, -1, -1f);
            }
        }

        protected override void onDisable()
        {
            GameLogic.Binder.EventBus.OnCharacterRangedAttackStarted -= new GameLogic.Events.CharacterRangedAttackStarted(this.onCharacterRangedAttackStarted);
        }

        protected override void onEnable()
        {
            GameLogic.Binder.EventBus.OnCharacterRangedAttackStarted += new GameLogic.Events.CharacterRangedAttackStarted(this.onCharacterRangedAttackStarted);
        }

        protected override bool onGetTargetAnimationState(out AbstractCharacterAnimator.State state, out float speed)
        {
            speed = 1.25f;
            state = AbstractCharacterAnimator.State.IDLE;
            return true;
        }

        protected override void onUpdate(float dt)
        {
            if (base.CharacterView.IsMenuView)
            {
                if (this.m_nextMenuAnimTime == 0f)
                {
                    this.m_nextMenuAnimTime = Time.unscaledTime + ConfigUi.PET_POPUP_ANIMATION_INTERVAL_INITIAL.getRandom();
                }
                else if (Time.unscaledTime >= this.m_nextMenuAnimTime)
                {
                    base.startAction(AbstractCharacterAnimator.Action.ATTACK_RANGED, 1f, true, -1, -1f);
                    this.m_nextMenuAnimTime = Time.unscaledTime + ConfigUi.PET_POPUP_ANIMATION_INTERVAL_STAY.getRandom();
                }
            }
        }
    }
}

