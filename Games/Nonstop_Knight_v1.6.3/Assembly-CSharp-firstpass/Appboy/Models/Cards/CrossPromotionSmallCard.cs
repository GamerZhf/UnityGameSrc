namespace Appboy.Models.Cards
{
    using Appboy.Utilities;
    using System;
    using System.Runtime.CompilerServices;

    public class CrossPromotionSmallCard : Card
    {
        [CompilerGenerated]
        private string <Caption>k__BackingField;
        [CompilerGenerated]
        private string <ImageUrl>k__BackingField;
        [CompilerGenerated]
        private string <Package>k__BackingField;
        [CompilerGenerated]
        private double <Price>k__BackingField;
        [CompilerGenerated]
        private double <Rating>k__BackingField;
        [CompilerGenerated]
        private int <ReviewCount>k__BackingField;
        [CompilerGenerated]
        private string <Subtitle>k__BackingField;
        [CompilerGenerated]
        private string <Title>k__BackingField;
        [CompilerGenerated]
        private string <Url>k__BackingField;

        public CrossPromotionSmallCard(JSONClass json) : base(json)
        {
            if ((((json["title"] == null) || (json["subtitle"] == null)) || ((json["caption"] == null) || (json["rating"] == null))) || (((json["reviews"] == null) || (json["price"] == null)) || (json["url"] == null)))
            {
                throw new ArgumentException("Missing required field(s).");
            }
            this.Title = (string) json["title"];
            this.Subtitle = (string) json["subtitle"];
            this.Caption = (string) json["caption"];
            this.ImageUrl = (string) json["image"];
            this.Rating = json["rating"].AsDouble;
            this.ReviewCount = json["reviews"].AsInt;
            this.Price = json["price"].AsDouble;
            this.Url = (string) json["url"];
            if (json["package"] == null)
            {
                throw new ArgumentException("Missing required field(s).");
            }
            this.Package = (string) json["package"];
        }

        public override string ToString()
        {
            object[] args = new object[] { base.ID, base.Type, this.Title, this.Subtitle, this.Caption, this.ImageUrl, this.Rating, this.ReviewCount, this.Price, base.Viewed, base.Created, base.Updated, base.CategoriesToString(), this.Url };
            string str = string.Format("CrossPromotionSmallCard[ID={0}, Type={1}, Title={2}, Subtitle={3}, Caption={4}, ImageUrl={5}, Rating={6}, ReviewCount={7}, Price={8}, Viewed={9}, Created={10}, Updated={11}, Categories={12}, uri={13}", args);
            string str2 = "]";
            str2 = string.Format("Package={0}]", this.Package);
            return (str + str2);
        }

        public string Caption
        {
            [CompilerGenerated]
            get
            {
                return this.<Caption>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Caption>k__BackingField = value;
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

        public string Package
        {
            [CompilerGenerated]
            get
            {
                return this.<Package>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Package>k__BackingField = value;
            }
        }

        public double Price
        {
            [CompilerGenerated]
            get
            {
                return this.<Price>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Price>k__BackingField = value;
            }
        }

        public double Rating
        {
            [CompilerGenerated]
            get
            {
                return this.<Rating>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Rating>k__BackingField = value;
            }
        }

        public int ReviewCount
        {
            [CompilerGenerated]
            get
            {
                return this.<ReviewCount>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ReviewCount>k__BackingField = value;
            }
        }

        public string Subtitle
        {
            [CompilerGenerated]
            get
            {
                return this.<Subtitle>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Subtitle>k__BackingField = value;
            }
        }

        public string Title
        {
            [CompilerGenerated]
            get
            {
                return this.<Title>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Title>k__BackingField = value;
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

