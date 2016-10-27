namespace GameLogic
{
    using App;
    using System;

    public class SkillInstance : IJsonData, IComparable<SkillInstance>
    {
        public bool InspectedByPlayer;
        public int Rank;
        public GameLogic.SkillType SkillType;

        public SkillInstance()
        {
        }

        public SkillInstance(SkillInstance another)
        {
            this.copyFrom(another);
        }

        public int CompareTo(SkillInstance other)
        {
            ConfigSkills.SharedData data = ConfigSkills.SHARED_DATA[this.SkillType];
            ConfigSkills.SharedData data2 = ConfigSkills.SHARED_DATA[other.SkillType];
            if (data.UnlockRank < data2.UnlockRank)
            {
                return -1;
            }
            if (data.UnlockRank > data2.UnlockRank)
            {
                return 1;
            }
            return 0;
        }

        public void copyFrom(SkillInstance another)
        {
            this.SkillType = another.SkillType;
            this.Rank = another.Rank;
            this.InspectedByPlayer = another.InspectedByPlayer;
        }

        public void postDeserializeInitialization()
        {
        }
    }
}

