namespace Service
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ServerSelection
    {
        [CompilerGenerated]
        private ServerEntry <SelectedServer>k__BackingField;

        public ServerEntry GetDevServer(string serverid)
        {
            foreach (ServerEntry entry in ConfigService.DevServerSelectionList)
            {
                if (entry.Id == serverid)
                {
                    return entry;
                }
            }
            return null;
        }

        public void Initialize(Player player)
        {
            if (ConfigApp.ProductionBuild)
            {
                this.SelectedServer = new ServerEntry("live", "Live", string.Empty, ConfigService.LiveServerUrl);
            }
            else
            {
                string devServerId = player.Preferences.DevServerId;
                if (string.IsNullOrEmpty(devServerId))
                {
                    Debug.LogError("Server id not set in player preferences");
                }
                else if (devServerId == ConfigService.OFFLINE_SERVER_ENTRY.Id)
                {
                    this.SelectedServer = ConfigService.OFFLINE_SERVER_ENTRY;
                }
                else
                {
                    this.SelectedServer = this.GetDevServer(devServerId);
                }
                if (this.SelectedServer == null)
                {
                    Debug.LogError("Server id " + devServerId + " doesn't exist");
                }
            }
            if (this.SelectedServer != null)
            {
                Debug.Log("Selected server id: " + this.SelectedServer.Id + ", url: " + this.SelectedServer.Url);
            }
            else
            {
                Debug.LogWarning("No server selected");
            }
        }

        public ServerEntry[] DevServerList
        {
            get
            {
                return ConfigService.DevServerSelectionList;
            }
        }

        public bool IsSelected
        {
            get
            {
                return (this.SelectedServer != null);
            }
        }

        public ServerEntry SelectedServer
        {
            [CompilerGenerated]
            get
            {
                return this.<SelectedServer>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<SelectedServer>k__BackingField = value;
            }
        }
    }
}

