namespace GameLogic
{
    using Service;
    using System;
    using UnityEngine;

    public class PersistentTimer
    {
        private float m_durationSeconds;
        private long m_resetTimestampSeconds;

        public PersistentTimer(float durationSeconds, long resetTimestampSeconds)
        {
            this.m_durationSeconds = durationSeconds;
            this.m_resetTimestampSeconds = resetTimestampSeconds;
        }

        private float secondsElapsedSinceReset()
        {
            return (float) (Service.Binder.ServerTime.GameTime - this.m_resetTimestampSeconds);
        }

        public void set(long resetTimestampSeconds)
        {
            this.m_resetTimestampSeconds = resetTimestampSeconds;
        }

        public float timeElapsedSeconds()
        {
            return this.secondsElapsedSinceReset();
        }

        public float timeRemainingNormalized()
        {
            return Mathf.Clamp01(this.secondsElapsedSinceReset() / this.m_durationSeconds);
        }

        public float timeRemainingSeconds()
        {
            return Mathf.Clamp(this.m_durationSeconds - this.secondsElapsedSinceReset(), 0f, float.MaxValue);
        }
    }
}

