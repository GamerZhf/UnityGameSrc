namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class JellySpikedCharacterAnimator : AbstractCharacterAnimator
    {
        private float m_spikeAnimationTimer;
        private float m_spikeRetractionTimer;
        private Animation m_spikesAnimation;
        private bool m_spikesOut;

        protected override Dictionary<AbstractCharacterAnimator.Action, AbstractCharacterAnimator.ActionParameters> getActionClipLinks()
        {
            Dictionary<AbstractCharacterAnimator.Action, AbstractCharacterAnimator.ActionParameters> dictionary = new Dictionary<AbstractCharacterAnimator.Action, AbstractCharacterAnimator.ActionParameters>();
            string[] names = new string[] { "SpikesOut" };
            dictionary.Add(AbstractCharacterAnimator.Action.ATTACK_MELEE, new AbstractCharacterAnimator.ActionParameters(names, 0.1f, 0.8f, 0f));
            string[] textArray2 = new string[] { "SpikesIn" };
            dictionary.Add(AbstractCharacterAnimator.Action.SPIKES_IN, new AbstractCharacterAnimator.ActionParameters(textArray2, 0.1f, 0.8f, 0f));
            return dictionary;
        }

        protected override Animation getAnimationComponent()
        {
            return base.transform.FindChild("jelly_anim").GetComponent<Animation>();
        }

        protected override Dictionary<AbstractCharacterAnimator.State, AbstractCharacterAnimator.StateParameters> getStateClipLinks()
        {
            Dictionary<AbstractCharacterAnimator.State, AbstractCharacterAnimator.StateParameters> dictionary = new Dictionary<AbstractCharacterAnimator.State, AbstractCharacterAnimator.StateParameters>();
            string[] names = new string[] { "Idle" };
            dictionary.Add(AbstractCharacterAnimator.State.IDLE, new AbstractCharacterAnimator.StateParameters(names, 0.2f));
            string[] textArray2 = new string[] { "Move" };
            dictionary.Add(AbstractCharacterAnimator.State.RUN, new AbstractCharacterAnimator.StateParameters(textArray2, 0.2f));
            return dictionary;
        }

        protected override void onAwake()
        {
            this.m_spikesAnimation = base.transform.FindChild("jellyspikes_anim").GetComponent<Animation>();
        }

        private void onCharacterMeleeAttackContact(CharacterInstance character, Vector3 contactWorldPt, bool critted)
        {
            if (character == base.CharacterView.Character)
            {
                this.spikeRetraction(false);
            }
        }

        protected override void onDisable()
        {
            GameLogic.Binder.EventBus.OnCharacterMeleeAttackContact -= new GameLogic.Events.CharacterMeleeAttackContact(this.onCharacterMeleeAttackContact);
        }

        protected override void onEnable()
        {
            GameLogic.Binder.EventBus.OnCharacterMeleeAttackContact += new GameLogic.Events.CharacterMeleeAttackContact(this.onCharacterMeleeAttackContact);
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
                speed = base.CharacterView.Character.MovementSpeed(true) * 0.3f;
                state = AbstractCharacterAnimator.State.RUN;
                return true;
            }
            return base.onGetTargetAnimationState(out state, out speed);
        }

        protected override void onUpdate(float dt)
        {
            if (this.m_spikeAnimationTimer > 0f)
            {
                this.m_spikeAnimationTimer -= dt;
            }
            if (this.m_spikesOut)
            {
                this.m_spikeRetractionTimer -= dt;
                if (this.m_spikeRetractionTimer <= 0f)
                {
                    this.spikeRetraction(true);
                }
            }
        }

        private void spikeRetraction(bool retract)
        {
            this.m_spikesAnimation["SpikesIn"].enabled = false;
            this.m_spikesAnimation["SpikesOut"].enabled = false;
            if (retract)
            {
                this.m_spikesAnimation["SpikesIn"].normalizedTime = 0f;
                this.m_spikesAnimation["SpikesIn"].weight = 1f;
                this.m_spikesAnimation["SpikesIn"].speed = 1.25f;
                this.m_spikesAnimation["SpikesIn"].enabled = true;
                this.m_spikeAnimationTimer = this.m_spikesAnimation["SpikesIn"].length / this.m_spikesAnimation["SpikesIn"].speed;
                this.m_spikesOut = false;
            }
            else
            {
                this.m_spikesAnimation["SpikesOut"].normalizedTime = 0f;
                this.m_spikesAnimation["SpikesOut"].weight = 1f;
                this.m_spikesAnimation["SpikesOut"].speed = 2.5f;
                this.m_spikesAnimation["SpikesOut"].enabled = true;
                this.m_spikeAnimationTimer = this.m_spikesAnimation["SpikesOut"].length / this.m_spikesAnimation["SpikesOut"].speed;
                this.m_spikeRetractionTimer = this.m_spikeAnimationTimer + 0.5f;
                this.m_spikesOut = true;
            }
        }
    }
}

