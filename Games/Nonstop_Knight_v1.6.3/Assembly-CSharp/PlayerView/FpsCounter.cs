namespace PlayerView
{
    using System;
    using System.Diagnostics;
    using UnityEngine;
    using UnityEngine.UI;

    public class FpsCounter : MonoBehaviour
    {
        public Text FpsText;
        private float m_frames;
        private Stopwatch m_stopwatch = new Stopwatch();
        private float m_timeleft;
        public const float UPDATED_INTERVAL = 0.5f;

        protected void Awake()
        {
            this.FpsText.text = "00.00";
            this.m_stopwatch.Start();
        }

        protected void Update()
        {
            this.m_timeleft -= TimeUtil.GetDeltaTime(TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
            this.m_frames++;
            if (this.m_timeleft <= 0.0)
            {
                float num = ((float) this.m_stopwatch.ElapsedMilliseconds) / 1000f;
                this.FpsText.text = "FPS: " + ((this.m_frames / num)).ToString("f2");
                this.m_timeleft = 0.5f;
                this.m_frames = 0f;
                this.m_stopwatch.Reset();
                this.m_stopwatch.Start();
            }
        }
    }
}

