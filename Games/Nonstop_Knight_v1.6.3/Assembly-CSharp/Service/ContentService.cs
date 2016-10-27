namespace Service
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ContentService
    {
        public const ClientType DEFAULT_CLIENT_TYPE = ClientType.iOS;
        private Service.MasterRemoteContent m_MasterRemoteContent;

        private ClientType GetClientType()
        {
            return ClientType.Android;
        }

        private int GetClientVersion()
        {
            return ConfigApp.InternalClientVersion;
        }

        public void InitializeContent()
        {
            this.LoadContent();
        }

        private void LoadContent()
        {
            if (this.MasterRemoteContent == null)
            {
                if (ConfigApp.ProductionBuild || ConfigApp.IsStableBuild())
                {
                    try
                    {
                        string json = IOUtil.LoadFromPersistentStorage("cached_master_remote_content.json");
                        this.MasterRemoteContent = JsonUtils.Deserialize<Service.MasterRemoteContent>(json, true);
                        this.Log("Loaded cached content");
                    }
                    catch (Exception)
                    {
                        this.Log("No cached content");
                    }
                }
                if ((this.MasterRemoteContent == null) || (this.MasterRemoteContent.ContentVersion < ConfigApp.ProductionBuildDefaultRemoteContentVersion))
                {
                    try
                    {
                        this.MasterRemoteContent = ConfigApp.LoadDefaultMasterRemoteContent();
                    }
                    catch (Exception exception)
                    {
                        UnityEngine.Debug.LogError("Error deserializing default MasterRemoteContent\n Exception: " + exception);
                        this.MasterRemoteContent = new Service.MasterRemoteContent();
                    }
                    this.Log("Using default content");
                    this.SaveContent();
                }
            }
        }

        [DebuggerHidden]
        public IEnumerator LoadRemoteContent()
        {
            <LoadRemoteContent>c__Iterator203 iterator = new <LoadRemoteContent>c__Iterator203();
            iterator.<>f__this = this;
            return iterator;
        }

        private void Log(string msg)
        {
            Service.Binder.Logger.Log(msg);
        }

        public void SaveContent()
        {
            this.MasterRemoteContent.CacheTimestamp = Service.Binder.ServerTime.GameTime;
            IOUtil.SaveToPersistentStorage(JsonUtils.Serialize(this.MasterRemoteContent), "cached_master_remote_content.json", ConfigApp.PersistentStorageEncryptionEnabled, true);
        }

        public Service.MasterRemoteContent MasterRemoteContent
        {
            get
            {
                return this.m_MasterRemoteContent;
            }
            private set
            {
                this.m_MasterRemoteContent = value;
                Service.Binder.EventBus.ContentReady();
            }
        }

        [CompilerGenerated]
        private sealed class <LoadRemoteContent>c__Iterator203 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal ContentService <>f__this;
            internal int <clientVersion>__0;
            internal Request<ContentResponse<MasterRemoteContent>> <req>__1;
            internal ContentResponseType <type>__2;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<>f__this.Log("Start loading content");
                        this.<>f__this.LoadContent();
                        this.<clientVersion>__0 = this.<>f__this.GetClientVersion();
                        this.<req>__1 = Request<ContentResponse<MasterRemoteContent>>.Get(string.Concat(new object[] { "/player/{sessionId}/content/", this.<>f__this.GetClientType(), "/", this.<clientVersion>__0, "/", this.<>f__this.MasterRemoteContent.GetContentVersion() }));
                        this.$current = this.<req>__1.Task;
                        this.$PC = 1;
                        return true;

                    case 1:
                        if (this.<req>__1.Success)
                        {
                            this.<>f__this.Log("Remote Content Response " + this.<req>__1.Result.ResponseType);
                            this.<type>__2 = this.<req>__1.Result.ResponseType;
                            if (this.<type>__2 == ContentResponseType.ClientUpdate)
                            {
                                if (string.IsNullOrEmpty(this.<req>__1.Result.ClientUrl))
                                {
                                    Service.Binder.PlayerService.PendingClientUpdateFromUrl = "http://www.nonstopknight.com/";
                                }
                                else
                                {
                                    Service.Binder.PlayerService.PendingClientUpdateFromUrl = this.<req>__1.Result.ClientUrl;
                                }
                            }
                            else if (this.<type>__2 == ContentResponseType.ContentUpdate)
                            {
                                this.<>f__this.MasterRemoteContent = this.<req>__1.Result.Content;
                                this.<>f__this.SaveContent();
                                Service.Binder.EventBus.NewContentAvailable();
                            }
                            this.$PC = -1;
                            break;
                        }
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

