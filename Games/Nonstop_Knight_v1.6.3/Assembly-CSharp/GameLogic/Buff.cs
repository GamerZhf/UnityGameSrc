namespace GameLogic
{
    using System;
    using UnityEngine;

    public class Buff
    {
        public float AllSkillsCooldownBonus;
        public BaseStatProperty BaseStat1;
        public BaseStatProperty BaseStat2;
        public CharacterInstance Character;
        public bool Charms;
        public bool Confuses;
        public double DamagePerSecond;
        public float DurationSeconds;
        public bool Ended;
        public BoostType FromBoost;
        public PerkType FromPerk;
        public double HealingPerSecond;
        public bool HudHideTimer;
        public bool HudShowModifier;
        public bool HudShowStacked;
        public string HudSprite;
        public string Id;
        public float Modifier;
        public float ModifierBuildupTick;
        public float ModifierFactor = 1f;
        public BuffSource Source;
        public CharacterInstance SourceCharacter;
        public bool Stuns;
        public ManualTimer TickTimer;
        public float TimeRemaining;
        public float ViewScaleModifier;

        public float getNormalizedProgress(float currentTime)
        {
            if (this.HudHideTimer)
            {
                return 0f;
            }
            return (1f - Mathf.Clamp01(this.getSecondsRemaining() / this.DurationSeconds));
        }

        public float getSecondsRemaining()
        {
            return Mathf.Clamp(this.TimeRemaining, 0f, float.MaxValue);
        }

        public float TotalModifier
        {
            get
            {
                return (this.Modifier * this.ModifierFactor);
            }
        }
    }
}

