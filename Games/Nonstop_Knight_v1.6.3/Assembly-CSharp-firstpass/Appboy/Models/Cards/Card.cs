namespace Appboy.Models.Cards
{
    using Appboy;
    using Appboy.Models;
    using Appboy.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Card
    {
        [CompilerGenerated]
        private HashSet<CardCategory> <Categories>k__BackingField;
        [CompilerGenerated]
        private long <Created>k__BackingField;
        [CompilerGenerated]
        private string <ID>k__BackingField;
        [CompilerGenerated]
        private string <JsonString>k__BackingField;
        [CompilerGenerated]
        private string <Type>k__BackingField;
        [CompilerGenerated]
        private long <Updated>k__BackingField;
        [CompilerGenerated]
        private bool <Viewed>k__BackingField;

        public Card(JSONClass json)
        {
            if (json == null)
            {
                throw new ArgumentNullException("json");
            }
            this.JsonString = json.ToString();
            if (((json["id"] == null) || (json["type"] == null)) || (((json["viewed"] == null) || (json["created"] == null)) || (json["updated"] == null)))
            {
                throw new ArgumentException("Missing required field(s).");
            }
            this.ID = (string) json["id"];
            this.Type = (string) json["type"];
            this.Viewed = json["viewed"].AsBool;
            this.Created = json["created"].AsInt;
            this.Updated = json["updated"].AsInt;
            this.Categories = new HashSet<CardCategory>();
            if (json["categories"] == null)
            {
                this.Categories.Add(CardCategory.NO_CATEGORY);
            }
            else
            {
                JSONArray array = (JSONArray) JSON.Parse(json["categories"].ToString());
                if ((array == null) || (array.Count == 0))
                {
                    this.Categories.Add(CardCategory.NO_CATEGORY);
                }
                else
                {
                    for (int i = 0; i < array.Count; i++)
                    {
                        CardCategory item = (CardCategory) ((int) EnumUtils.TryParse(typeof(CardCategory), (string) array[i], true, null));
                        this.Categories.Add(item);
                    }
                }
            }
        }

        public string CategoriesToString()
        {
            List<string> list = new List<string>();
            foreach (CardCategory category in this.Categories)
            {
                list.Add(category.ToString());
            }
            return string.Join(",", list.ToArray());
        }

        public void LogClick()
        {
            if (!string.IsNullOrEmpty(this.ID))
            {
                object[] args = new object[] { this.ID };
                AppboyBinding.Appboy.Call<bool>("logFeedCardClick", args);
            }
        }

        public void LogImpression()
        {
            if (!string.IsNullOrEmpty(this.ID))
            {
                object[] args = new object[] { this.ID };
                AppboyBinding.Appboy.Call<bool>("logFeedCardImpression", args);
            }
        }

        public HashSet<CardCategory> Categories
        {
            [CompilerGenerated]
            get
            {
                return this.<Categories>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Categories>k__BackingField = value;
            }
        }

        public long Created
        {
            [CompilerGenerated]
            get
            {
                return this.<Created>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Created>k__BackingField = value;
            }
        }

        public string ID
        {
            [CompilerGenerated]
            get
            {
                return this.<ID>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ID>k__BackingField = value;
            }
        }

        public string JsonString
        {
            [CompilerGenerated]
            get
            {
                return this.<JsonString>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<JsonString>k__BackingField = value;
            }
        }

        public string Type
        {
            [CompilerGenerated]
            get
            {
                return this.<Type>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Type>k__BackingField = value;
            }
        }

        public long Updated
        {
            [CompilerGenerated]
            get
            {
                return this.<Updated>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Updated>k__BackingField = value;
            }
        }

        public bool Viewed
        {
            [CompilerGenerated]
            get
            {
                return this.<Viewed>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Viewed>k__BackingField = value;
            }
        }
    }
}

