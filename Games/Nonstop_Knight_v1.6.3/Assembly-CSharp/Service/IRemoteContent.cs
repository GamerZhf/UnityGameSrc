namespace Service
{
    using App;
    using System;

    public interface IRemoteContent
    {
        ConfigMeta GetConfigMeta();
        int GetContentVersion();
        int GetRequiredClientVersion(ClientType client);
        string GetVariant();
    }
}

