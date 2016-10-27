namespace GameLogic
{
    using App;
    using System;
    using UnityEngine;

    public class RunestoneInstance : IJsonData
    {
        public string Id;
        public bool InspectedByPlayer;
        public int Level;
        public int Rank;
        public bool Unlocked;

        public bool canEvolve()
        {
            if (!this.Unlocked)
            {
                return false;
            }
            if (this.isAtMaxLevel())
            {
                return false;
            }
            return (this.getCompletedRankUpsForNextEvolve() >= this.getRequiredRankUpsForNextEvolve());
        }

        public int getCompletedRankUpsForNextEvolve()
        {
            int level = this.Level;
            int num2 = 0;
            while (level > 0)
            {
                num2 += App.Binder.ConfigMeta.RunestoneEvolveCurve(level--);
            }
            return Mathf.Clamp(this.Rank - num2, 0, 0x7fffffff);
        }

        public float getEvolveModifierBonus()
        {
            return 0f;
        }

        public float getNormalizedProgressToNextEvolve()
        {
            if (this.isAtMaxLevel())
            {
                return 1f;
            }
            int num = this.getCompletedRankUpsForNextEvolve();
            int num2 = this.getRequiredRankUpsForNextEvolve();
            if (num2 == 0)
            {
                return 1f;
            }
            return Mathf.Clamp01(((float) num) / ((float) num2));
        }

        public PerkType getPerkType()
        {
            return ConfigRunestones.GetRunestoneData(this.Id).PerkInstance.Type;
        }

        public int getRequiredRankUpsForNextEvolve()
        {
            int targetLevel = this.Level + 1;
            return App.Binder.ConfigMeta.RunestoneEvolveCurve(targetLevel);
        }

        public bool isAtMaxLevel()
        {
            return (this.Level >= ConfigMeta.RUNESTONE_MAX_LEVEL);
        }

        public void postDeserializeInitialization()
        {
        }
    }
}

