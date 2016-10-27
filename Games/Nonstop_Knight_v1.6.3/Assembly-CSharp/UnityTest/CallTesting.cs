namespace UnityTest
{
    using System;
    using UnityEngine;

    public class CallTesting : MonoBehaviour
    {
        public int afterFrames;
        public float afterSeconds;
        public Functions callOnMethod = Functions.Start;
        private int m_StartFrame;
        private float m_StartTime;
        public Method methodToCall;

        private void CallAfterFrames()
        {
            if ((this.afterFrames > 0) && ((this.m_StartFrame + this.afterFrames) <= Time.frameCount))
            {
                this.TryToCallTesting(Functions.CallAfterFrames);
            }
        }

        private void CallAfterSeconds()
        {
            if ((this.m_StartTime + this.afterSeconds) <= Time.time)
            {
                this.TryToCallTesting(Functions.CallAfterSeconds);
            }
        }

        public void FixedUpdate()
        {
            this.TryToCallTesting(Functions.FixedUpdate);
        }

        public void LateUpdate()
        {
            this.TryToCallTesting(Functions.LateUpdate);
        }

        public void OnBecameInvisible()
        {
            this.TryToCallTesting(Functions.OnBecameInvisible);
        }

        public void OnBecameVisible()
        {
            this.TryToCallTesting(Functions.OnBecameVisible);
        }

        public void OnCollisionEnter()
        {
            this.TryToCallTesting(Functions.OnCollisionEnter);
        }

        public void OnCollisionEnter2D()
        {
            this.TryToCallTesting(Functions.OnCollisionEnter2D);
        }

        public void OnCollisionExit()
        {
            this.TryToCallTesting(Functions.OnCollisionExit);
        }

        public void OnCollisionExit2D()
        {
            this.TryToCallTesting(Functions.OnCollisionExit2D);
        }

        public void OnCollisionStay()
        {
            this.TryToCallTesting(Functions.OnCollisionStay);
        }

        public void OnCollisionStay2D()
        {
            this.TryToCallTesting(Functions.OnCollisionStay2D);
        }

        public void OnControllerColliderHit()
        {
            this.TryToCallTesting(Functions.OnControllerColliderHit);
        }

        public void OnDestroy()
        {
            this.TryToCallTesting(Functions.OnDestroy);
        }

        public void OnDisable()
        {
            this.TryToCallTesting(Functions.OnDisable);
        }

        public void OnEnable()
        {
            this.TryToCallTesting(Functions.OnEnable);
        }

        public void OnJointBreak()
        {
            this.TryToCallTesting(Functions.OnJointBreak);
        }

        public void OnParticleCollision()
        {
            this.TryToCallTesting(Functions.OnParticleCollision);
        }

        public void OnTriggerEnter()
        {
            this.TryToCallTesting(Functions.OnTriggerEnter);
        }

        public void OnTriggerEnter2D()
        {
            this.TryToCallTesting(Functions.OnTriggerEnter2D);
        }

        public void OnTriggerExit()
        {
            this.TryToCallTesting(Functions.OnTriggerExit);
        }

        public void OnTriggerExit2D()
        {
            this.TryToCallTesting(Functions.OnTriggerExit2D);
        }

        public void OnTriggerStay()
        {
            this.TryToCallTesting(Functions.OnTriggerStay);
        }

        public void OnTriggerStay2D()
        {
            this.TryToCallTesting(Functions.OnTriggerStay2D);
        }

        public void Start()
        {
            this.m_StartTime = Time.time;
            this.m_StartFrame = this.afterFrames;
            this.TryToCallTesting(Functions.Start);
        }

        private void TryToCallTesting(Functions invokingMethod)
        {
            if (invokingMethod == this.callOnMethod)
            {
                if (this.methodToCall == Method.Pass)
                {
                    IntegrationTest.Pass(base.gameObject);
                }
                else
                {
                    IntegrationTest.Fail(base.gameObject);
                }
                this.afterFrames = 0;
                this.afterSeconds = 0f;
                this.m_StartTime = float.PositiveInfinity;
                this.m_StartFrame = -2147483648;
            }
        }

        public void Update()
        {
            this.TryToCallTesting(Functions.Update);
            this.CallAfterSeconds();
            this.CallAfterFrames();
        }

        public enum Functions
        {
            CallAfterSeconds,
            CallAfterFrames,
            Start,
            Update,
            FixedUpdate,
            LateUpdate,
            OnDestroy,
            OnEnable,
            OnDisable,
            OnControllerColliderHit,
            OnParticleCollision,
            OnJointBreak,
            OnBecameInvisible,
            OnBecameVisible,
            OnTriggerEnter,
            OnTriggerExit,
            OnTriggerStay,
            OnCollisionEnter,
            OnCollisionExit,
            OnCollisionStay,
            OnTriggerEnter2D,
            OnTriggerExit2D,
            OnTriggerStay2D,
            OnCollisionEnter2D,
            OnCollisionExit2D,
            OnCollisionStay2D
        }

        public enum Method
        {
            Pass,
            Fail
        }
    }
}

