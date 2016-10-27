namespace PlayerView
{
    using System;
    using UnityEngine;

    public static class EffectUtils
    {
        public static void ResetParticleSystem(ParticleSystem ps)
        {
            ps.Stop();
            ps.Simulate(0f, true, true);
            ps.Clear();
        }
    }
}

