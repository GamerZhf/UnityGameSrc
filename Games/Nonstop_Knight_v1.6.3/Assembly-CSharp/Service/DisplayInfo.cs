namespace Service
{
    using System;
    using System.Collections.Generic;

    public class DisplayInfo
    {
        private const string DESCRIPTION_KEY = "description";
        public Dictionary<string, string> displayInfo;
        private const string ICON_KEY = "icon";
        private const string REWARD_PRE_KEY = "reward-";

        public string GetDescription()
        {
            return this.GetValue("description");
        }

        public string GetIcon()
        {
            return this.GetValue("icon");
        }

        public string GetRewardIcon(string _rewardKey)
        {
            return this.GetValue("reward-" + _rewardKey);
        }

        private string GetValue(string key)
        {
            if ((this.displayInfo != null) && this.displayInfo.ContainsKey(key))
            {
                return this.displayInfo[key];
            }
            return null;
        }
    }
}

