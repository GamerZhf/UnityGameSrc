namespace Appboy.Models.Cards
{
    using Appboy.Utilities;
    using System;
    using System.Runtime.CompilerServices;

    public class BannerCard : Card
    {
        [CompilerGenerated]
        private string <Domain>k__BackingField;
        [CompilerGenerated]
        private string <ImageUrl>k__BackingField;
        [CompilerGenerated]
        private string <Url>k__BackingField;

        public BannerCard(JSONClass json) : base(json)
        {
            if (json["image"] == null)
            {
                throw new ArgumentException("Missing required field(s).");
            }
            this.ImageUrl = (string) json["image"];
            if (json["url"] != null)
            {
                this.Url = (string) json["url"];
            }
            if (json["domain"] != null)
            {
                this.Domain = (string) json["domain"];
            }
        }

        public override string ToString()
        {
            object[] args = new object[] { base.ID, base.Type, this.ImageUrl, base.Viewed, base.Created, base.Updated, this.Url, this.Domain, base.CategoriesToString() };
            return string.Format("BannerCard[ID={0}, Type={1}, ImageUrl={2}, Viewed={3}, Created={4}, Updated={5}, Url={6}, Domain={7}, Categories={8}]", args);
        }

        public string Domain
        {
            [CompilerGenerated]
            get
            {
                return this.<Domain>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Domain>k__BackingField = value;
            }
        }

        public string ImageUrl
        {
            [CompilerGenerated]
            get
            {
                return this.<ImageUrl>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ImageUrl>k__BackingField = value;
            }
        }

        public string Url
        {
            [CompilerGenerated]
            get
            {
                return this.<Url>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Url>k__BackingField = value;
            }
        }
    }
}

