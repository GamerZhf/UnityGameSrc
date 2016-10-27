namespace MATSDK
{
    using System;
    using System.Collections.Generic;

    public class MATAdMetadata
    {
        private float altitude;
        private DateTime? birthDate;
        private Dictionary<string, string> customTargets = new Dictionary<string, string>();
        private bool debugMode = false;
        private MATAdGender gender = MATAdGender.UNKNOWN;
        private HashSet<string> keywords = new HashSet<string>();
        private float latitude;
        private float longitude;

        public void addKeyword(string keyword)
        {
            if (this.keywords == null)
            {
                this.keywords = new HashSet<string>();
            }
            this.keywords.Add(keyword);
        }

        public float getAltitude()
        {
            return this.altitude;
        }

        public DateTime? getBirthDate()
        {
            return this.birthDate;
        }

        public Dictionary<string, string> getCustomTargets()
        {
            return this.customTargets;
        }

        public bool getDebugMode()
        {
            return this.debugMode;
        }

        public MATAdGender getGender()
        {
            return this.gender;
        }

        public HashSet<string> getKeywords()
        {
            return this.keywords;
        }

        public float getLatitude()
        {
            return this.latitude;
        }

        public float getLongitude()
        {
            return this.longitude;
        }

        public void setBirthDate(DateTime? birthDate)
        {
            this.birthDate = birthDate;
        }

        public void setBirthDate(int year, int month, int day)
        {
            this.birthDate = new DateTime(year, month, day);
        }

        public void setCustomTargets(Dictionary<string, string> customTargets)
        {
            this.customTargets = customTargets;
        }

        public void setDebugMode(bool debugMode)
        {
            this.debugMode = debugMode;
        }

        public void setGender(MATAdGender gender)
        {
            this.gender = gender;
        }

        public void setKeywords(HashSet<string> keywords)
        {
            this.keywords = keywords;
        }

        public void setLocation(float latitude, float longitude, float altitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.altitude = altitude;
        }
    }
}

