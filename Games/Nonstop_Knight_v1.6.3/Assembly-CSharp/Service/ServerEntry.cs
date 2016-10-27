namespace Service
{
    using System;

    public class ServerEntry
    {
        public string Description;
        public string Id;
        public string Name;
        public string Url;

        public ServerEntry(string id, string name, string description, string url)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Url = url;
        }
    }
}

