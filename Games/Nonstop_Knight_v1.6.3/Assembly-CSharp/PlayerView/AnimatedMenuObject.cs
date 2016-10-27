namespace PlayerView
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(Animation))]
    public class AnimatedMenuObject : MonoBehaviour
    {
        private bool? m_activeState;
        private Animation m_animation;
        private List<string> m_clipNames;

        protected void Awake()
        {
            this.m_animation = base.GetComponent<Animation>();
            this.m_clipNames = new List<string>();
            IEnumerator enumerator = this.m_animation.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AnimationState current = (AnimationState) enumerator.Current;
                    this.m_clipNames.Add(current.name);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
        }

        public void setActiveState(bool active)
        {
            if ((this.m_clipNames != null) && (!this.m_activeState.HasValue || (this.m_activeState.Value != active)))
            {
                for (int i = 0; i < this.m_clipNames.Count; i++)
                {
                    this.m_animation[this.m_clipNames[i]].enabled = false;
                }
                string str = !active ? this.m_clipNames[1] : this.m_clipNames[0];
                AnimationState state = this.m_animation[str];
                state.enabled = true;
                state.time = 0f;
                state.speed = 1f;
                state.weight = 1f;
                this.m_activeState = new bool?(active);
            }
        }
    }
}

