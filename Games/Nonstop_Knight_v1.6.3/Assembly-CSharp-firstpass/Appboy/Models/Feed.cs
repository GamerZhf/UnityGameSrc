namespace Appboy.Models
{
    using Appboy.Models.Cards;
    using Appboy.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Feed
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map0;
        [CompilerGenerated]
        private List<Card> <Cards>k__BackingField;
        [CompilerGenerated]
        private bool <IsFromOfflineStorage>k__BackingField;

        public Feed(string message)
        {
            JSONClass class2;
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Message cannot be null or empty.", "message");
            }
            this.Cards = new List<Card>();
            try
            {
                class2 = (JSONClass) JSON.Parse(message);
                if (class2["mFeedCards"] != null)
                {
                    JSONArray array = (JSONArray) JSON.Parse(class2["mFeedCards"].ToString());
                    Debug.Log(string.Format("parsed cards array with {0} of cards", array.Count));
                    for (int i = 0; i < array.Count; i++)
                    {
                        JSONClass asObject = array[i].AsObject;
                        try
                        {
                            Debug.Log(string.Format("Card NO. {0} json string is {1}", i, asObject));
                            Card item = CreateCardFromJson(asObject);
                            if (item != null)
                            {
                                this.Cards.Add(item);
                            }
                        }
                        catch
                        {
                            Debug.Log(string.Format("Unable to parse card from {0}", asObject));
                        }
                    }
                }
            }
            catch
            {
                throw new ArgumentException("Cannot parse feed JSON message.");
            }
            this.IsFromOfflineStorage = class2["mFromOfflineStorage"].AsBool;
        }

        private static Card CreateCardFromJson(JSONClass json)
        {
            string key = (string) json["type"];
            if (key != null)
            {
                int num;
                if (<>f__switch$map0 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(5);
                    dictionary.Add("banner_image", 0);
                    dictionary.Add("captioned_image", 1);
                    dictionary.Add("cross_promotion_small", 2);
                    dictionary.Add("short_news", 3);
                    dictionary.Add("text_announcement", 4);
                    <>f__switch$map0 = dictionary;
                }
                if (<>f__switch$map0.TryGetValue(key, out num))
                {
                    switch (num)
                    {
                        case 0:
                            return new BannerCard(json);

                        case 1:
                            return new CaptionedImageCard(json);

                        case 2:
                            return new CrossPromotionSmallCard(json);

                        case 3:
                            return new ClassicCard(json);

                        case 4:
                            return new TextAnnouncementCard(json);
                    }
                }
            }
            return null;
        }

        public List<Card> Cards
        {
            [CompilerGenerated]
            get
            {
                return this.<Cards>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Cards>k__BackingField = value;
            }
        }

        public int Count
        {
            get
            {
                return this.Cards.Count;
            }
        }

        public bool IsFromOfflineStorage
        {
            [CompilerGenerated]
            get
            {
                return this.<IsFromOfflineStorage>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<IsFromOfflineStorage>k__BackingField = value;
            }
        }
    }
}

