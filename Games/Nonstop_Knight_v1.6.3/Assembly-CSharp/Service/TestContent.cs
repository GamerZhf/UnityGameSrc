namespace Service
{
    using App;
    using System;
    using System.Collections.Generic;

    public class TestContent : IRemoteContent
    {
        public int ContentVersion;
        public Dictionary<string, int> RequiredClientVersion;
        public string variant = "default";

        public ConfigMeta GetConfigMeta()
        {
            return null;
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
            return this.variant;
        }
    }
}

