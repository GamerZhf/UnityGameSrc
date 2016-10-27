namespace PlayerView
{
    using System;
    using System.Collections.Generic;

    public class AudioGroup
    {
        private int m_astIndex;
        private List<AudioSourceType> m_astList;
        private static Random m_rnd = new Random();

        public AudioGroup(List<AudioSourceType> astList)
        {
            this.m_astList = astList;
            this.shuffle();
        }

        public List<AudioSourceType> GetAstList()
        {
            return this.m_astList;
        }

        public AudioSourceType GetNextAst()
        {
            this.m_astIndex++;
            if (this.m_astIndex >= this.m_astList.Count)
            {
                this.shuffle();
                this.m_astIndex = 0;
            }
            return this.m_astList[this.m_astIndex];
        }

        private void shuffle()
        {
            int count = this.m_astList.Count;
            if (count > 2)
            {
                AudioSourceType type;
                AudioSourceType type2 = this.m_astList[0];
                for (int i = 0; i < count; i++)
                {
                    int num3 = i + m_rnd.Next(count - i);
                    type = this.m_astList[num3];
                    this.m_astList[num3] = this.m_astList[i];
                    this.m_astList[i] = type;
                }
                if (((AudioSourceType) this.m_astList[0]) == type2)
                {
                    type = this.m_astList[0];
                    this.m_astList[0] = this.m_astList[count - 1];
                    this.m_astList[count - 1] = type;
                }
            }
        }
    }
}

