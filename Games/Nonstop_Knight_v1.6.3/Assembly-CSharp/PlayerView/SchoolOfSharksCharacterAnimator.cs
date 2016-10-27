namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class SchoolOfSharksCharacterAnimator : AbstractCharacterAnimator
    {
        private float m_nextMenuAnimTime;
        private Animation[] m_rootAnimations;
        private Vector3[] m_startingLocalPositions;

        protected override Dictionary<AbstractCharacterAnimator.Action, AbstractCharacterAnimator.ActionParameters> getActionClipLinks()
        {
            Dictionary<AbstractCharacterAnimator.Action, AbstractCharacterAnimator.ActionParameters> dictionary = new Dictionary<AbstractCharacterAnimator.Action, AbstractCharacterAnimator.ActionParameters>();
            string[] names = new string[] { "Attack" };
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
            this.m_rootAnimations = base.AnimationTm.parent.GetComponentsInChildren<Animation>();
            this.m_startingLocalPositions = new Vector3[this.m_rootAnimations.Length];
            for (int i = 0; i < this.m_rootAnimations.Length; i++)
            {
                this.m_startingLocalPositions[i] = this.m_rootAnimations[i].transform.localPosition;
            }
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
            speed = 1.5f;
            state = AbstractCharacterAnimator.State.IDLE;
            return true;
        }

        protected override void onUpdate(float dt)
        {
            for (int i = 0; i < this.m_rootAnimations.Length; i++)
            {
                Transform transform = this.m_rootAnimations[i].transform;
                float num2 = 0.1f;
                float num3 = 2.92f;
                float num4 = 1.67f;
                float num5 = 0.01f;
                float num6 = 0.2f;
                float num7 = 0.15f;
                if (base.CharacterView.IsMenuView)
                {
                    num2 *= 0.5f;
                    num3 *= 0.5f;
                    num4 *= 0.5f;
                }
                if (i == 0)
                {
                    num5 = 0f;
                }
                else
                {
                    num6 = 0f;
                    num7 = 0f;
                }
                float num8 = (i != 1) ? 1f : -1f;
                transform.localPosition = new Vector3(this.m_startingLocalPositions[i].x + (Mathf.Sin(Time.time * num2) * num5), this.m_startingLocalPositions[i].y + (Mathf.Sin(Time.time * num3) * num6), this.m_startingLocalPositions[i].z + ((num8 * Mathf.Sin(Time.time * num4)) * num7));
            }
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

