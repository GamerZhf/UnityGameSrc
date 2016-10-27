namespace Service
{
    using App;
    using System;
    using System.Collections.Generic;

    public class MasterRemoteContent : IRemoteContent
    {
        public long CacheTimestamp;
        public App.ConfigLootTables ConfigLootTables = new App.ConfigLootTables();
        public App.ConfigMeta ConfigMeta = new App.ConfigMeta();
        public int ContentVersion;
        public Dictionary<string, int> RequiredClientVersion;
        public string Variant = "default";

        public App.ConfigMeta GetConfigMeta()
        {
            return this.ConfigMeta;
        }

        public int GetContentVersion()
        {
            return this.ContentVersion;
        }

        public int GetRequiredClientVersion(ClientType client)
        {
            return this.RequiredClientVersion[client.ToString()];
        }

        public string GetVariant()
        {
            return this.Variant;
        }
    }
}

