namespace Appboy.Models.Cards
{
    using Appboy.Utilities;
    using System;
    using System.Runtime.CompilerServices;

    public class TextAnnouncementCard : Card
    {
        [CompilerGenerated]
        private string <Description>k__BackingField;
        [CompilerGenerated]
        private string <Domain>k__BackingField;
        [CompilerGenerated]
        private string <Title>k__BackingField;
        [CompilerGenerated]
        private string <Url>k__BackingField;

        public TextAnnouncementCard(JSONClass json) : base(json)
        {
            if ((json["title"] == null) || (json["description"] == null))
            {
                throw new ArgumentException("Missing required field(s).");
            }
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
            object[] args = new object[] { base.ID, base.Type, this.Title, this.Description, base.Viewed, base.Created, base.Updated, this.Url, this.Domain, base.CategoriesToString() };
            return string.Format("TextAnnouncementCard[ID={0}, Type={1}, Title={2}, Description={3}, Viewed={4}, Created={5}, Updated={6}, Url={7}, Domain={8}, Categories={9}", args);
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

