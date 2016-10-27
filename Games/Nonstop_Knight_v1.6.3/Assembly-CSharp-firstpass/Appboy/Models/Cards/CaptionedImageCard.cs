namespace Appboy.Models.Cards
{
    using Appboy.Utilities;
    using System;
    using System.Runtime.CompilerServices;

    public class CaptionedImageCard : Card
    {
        [CompilerGenerated]
        private string <Description>k__BackingField;
        [CompilerGenerated]
        private string <Domain>k__BackingField;
        [CompilerGenerated]
        private string <ImageUrl>k__BackingField;
        [CompilerGenerated]
        private string <Title>k__BackingField;
        [CompilerGenerated]
        private string <Url>k__BackingField;

        public CaptionedImageCard(JSONClass json) : base(json)
        {
            if (((json["image"] == null) || (json["title"] == null)) || (json["description"] == null))
            {
                throw new ArgumentException("Missing required field(s).");
            }
            this.ImageUrl = (string) json["image"];
            this.Title = (string) json["title"];
            this.Description = (string) json["description"];
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
            object[] args = new object[] { base.ID, base.Type, this.Description, this.ImageUrl, base.Viewed, base.Created, base.Updated, this.Title, this.Url, this.Domain, base.CategoriesToString() };
            return string.Format("CaptionedImageCard: ID={0}, Type={1}, Description={2}, ImageUrl={3}, Viewed={4}, Created={5}, Updated={6}, Title={7}, Url={8}, Domain={9}, Categories={10}", args);
        }

        public string Description
        {
            [CompilerGenerated]
            get
            {
                return this.<Description>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Description>k__BackingField = value;
            }
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

