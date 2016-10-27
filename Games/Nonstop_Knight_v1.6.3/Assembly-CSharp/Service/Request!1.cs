namespace Service
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using UnityEngine;

    public class Request<T> : IRequest
    {
        [CompilerGenerated]
        private string <ErrorMsg>k__BackingField;
        [CompilerGenerated]
        private Service.ExceptionResponse <ExceptionResponse>k__BackingField;
        public T Result;
        public IEnumerator Task;

        public static Request<string> Alive(string _path)
        {
            WWW www = new WWW(Request<T>.BuildUrl(_path));
            Request<string> request = new Request<string>();
            request.Task = request.RunAlive(www);
            return request;
        }

        private static string BuildUrl(string path)
        {
            if (path.Contains("{sessionId}"))
            {
                path = path.Replace("{sessionId}", Service.Binder.SessionData.SessionId);
            }
            return (Service.Binder.SessionData.ServerUrl + path);
        }

        public static Request<T> Get(string _path)
        {
            return Request<T>.GetUrl(Request<T>.BuildUrl(_path));
        }

        public static Request<T> GetAlive(string _path)
        {
            return Request<T>.GetUrl(Request<T>.BuildUrl(_path));
        }

        public static Request<T> GetUrl(string _url)
        {
            Service.Binder.ServiceContext.TotalKbsSent += ((float) _url.Length) / 1024f;
            WWW www = new WWW(_url);
            Request<T> request = new Request<T>();
            request.Task = request.Run(www);
            return request;
        }

        private static void Log(string msg)
        {
            Service.Binder.Logger.Log(msg);
        }

        public static Request<T> Post(string _url, object _body)
        {
            return Request<T>.PostUrl(Request<T>.BuildUrl(_url), _body, false);
        }

        public static Request<T> PostEncrypted(string _url, object _body)
        {
            return Request<T>.PostUrl(Request<T>.BuildUrl(_url), _body, App.Binder.ConfigMeta.ENABLE_POST_SECURE);
        }

        public static Request<T> PostUrl(string _url, object _body, [Optional, DefaultParameterValue(false)] bool encrypted)
        {
            string unencrypted = JsonUtils.Serialize(_body);
            if (encrypted)
            {
                Request<T>.Log("Encrypting...");
                unencrypted = AesEncryptor.Encrypt(unencrypted);
            }
            byte[] bytes = Encoding.UTF8.GetBytes(unencrypted);
            Service.Binder.ServiceContext.TotalKbsSent += ((float) _url.Length) / 1024f;
            Service.Binder.ServiceContext.TotalKbsSent += ((float) bytes.Length) / 1024f;
            WWW www = new WWW(_url, bytes);
            Request<T> request = new Request<T>();
            request.Task = request.Run(www);
            return request;
        }

        [DebuggerHidden]
        public IEnumerator Run(WWW _www)
        {
            <Run>c__Iterator22F<T> iteratorf = new <Run>c__Iterator22F<T>();
            iteratorf._www = _www;
            iteratorf.<$>_www = _www;
            iteratorf.<>f__this = (Request<T>) this;
            return iteratorf;
        }

        [DebuggerHidden]
        private IEnumerator RunAlive(WWW _www)
        {
            <RunAlive>c__Iterator22E<T> iteratore = new <RunAlive>c__Iterator22E<T>();
            iteratore._www = _www;
            iteratore.<$>_www = _www;
            iteratore.<>f__this = (Request<T>) this;
            return iteratore;
        }

        private void SyncTime(WWW _www)
        {
            if ((_www.responseHeaders != null) && _www.responseHeaders.ContainsKey("X-SERVER-TIME"))
            {
                long serverTime = long.Parse(_www.responseHeaders["X-SERVER-TIME"]);
                Service.Binder.ServerTime.SyncTime(serverTime);
                Service.Binder.EventBus.ServerTimeSynced();
            }
        }

        public string ErrorMsg
        {
            [CompilerGenerated]
            get
            {
                return this.<ErrorMsg>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ErrorMsg>k__BackingField = value;
            }
        }

        public Service.ExceptionResponse ExceptionResponse
        {
            [CompilerGenerated]
            get
            {
                return this.<ExceptionResponse>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ExceptionResponse>k__BackingField = value;
            }
        }

        public bool Success
        {
            get
            {
                return (this.ErrorMsg == null);
            }
        }

        [CompilerGenerated]
        private sealed class <Run>c__Iterator22F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal WWW _www;
            internal WWW <$>_www;
            internal Request<T> <>f__this;
            internal ServerErrorCode <code>__3;
            internal Exception <ex>__1;
            internal Exception <ex>__2;
            internal string <responseBody>__0;

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
                        Request<T>.Log("sending:" + this._www.url);
                        this.$current = this._www;
                        this.$PC = 1;
                        return true;

                    case 1:
                        this.<>f__this.ErrorMsg = this._www.error;
                        this.<responseBody>__0 = Encoding.UTF8.GetString(this._www.bytes);
                        Service.Binder.ServiceContext.TotalKbsReceived += ((float) this._www.bytes.Length) / 1024f;
                        if (!this.<>f__this.Success)
                        {
                            if ((this.<responseBody>__0 != null) && (this.<responseBody>__0.Length > 0))
                            {
                                try
                                {
                                    this.<>f__this.ExceptionResponse = JsonUtils.Deserialize<ExceptionResponse>(this.<responseBody>__0, true);
                                    if (this.<>f__this.ExceptionResponse.code == ServerErrorCode.Unspecified)
                                    {
                                        this.<>f__this.ExceptionResponse = null;
                                        Service.Binder.EventBus.NetworkStateChanged(false);
                                    }
                                }
                                catch (Exception exception2)
                                {
                                    this.<ex>__2 = exception2;
                                    Service.Binder.EventBus.NetworkStateChanged(false);
                                    Request<T>.Log(this.<ex>__2.ToString());
                                }
                                Request<T>.Log("Failed with body:" + this.<>f__this.ErrorMsg + " : " + this.<responseBody>__0);
                            }
                            else if ((this._www.responseHeaders != null) && this._www.responseHeaders.ContainsKey("WARNING"))
                            {
                                try
                                {
                                    this.<code>__3 = (ServerErrorCode) ((int) Enum.Parse(typeof(ServerErrorCode), this._www.responseHeaders["WARNING"]));
                                    ExceptionResponse response = new ExceptionResponse();
                                    response.code = this.<code>__3;
                                    response.msg = "header hack";
                                    response.service = "header hack";
                                    response.type = "header hack";
                                    this.<>f__this.ExceptionResponse = response;
                                    Request<T>.Log("Failed with code:" + this.<code>__3.ToString());
                                }
                                catch (ArgumentException)
                                {
                                    Request<T>.Log("unparsable response code " + this._www.responseHeaders["WARNING"]);
                                    Service.Binder.EventBus.NetworkStateChanged(false);
                                }
                            }
                            else
                            {
                                Request<T>.Log("error text " + this._www.text);
                                Service.Binder.EventBus.NetworkStateChanged(false);
                                if (this._www.responseHeaders == null)
                                {
                                }
                                Request<T>.Log("Failed no body:" + this.<>f__this.ErrorMsg + " " + JsonUtils.Serialize(new object()));
                            }
                            break;
                        }
                        this.<>f__this.SyncTime(this._www);
                        Request<T>.Log("recv:" + this.<responseBody>__0);
                        try
                        {
                            if (typeof(string) == typeof(T))
                            {
                                this.<>f__this.Result = (T) this.<responseBody>__0;
                            }
                            else
                            {
                                this.<>f__this.Result = JsonUtils.Deserialize<T>(this.<responseBody>__0, true);
                            }
                            Service.Binder.EventBus.NetworkStateChanged(true);
                        }
                        catch (Exception exception)
                        {
                            this.<ex>__1 = exception;
                            this.<>f__this.ErrorMsg = this.<ex>__1.ToString();
                            Service.Binder.EventBus.NetworkStateChanged(false);
                            Request<T>.Log("Cannot deserialize response body: " + this.<ex>__1);
                        }
                        break;

                    default:
                        goto Label_03A0;
                }
                this._www.Dispose();
                this.$PC = -1;
            Label_03A0:
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

        [CompilerGenerated]
        private sealed class <RunAlive>c__Iterator22E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal WWW _www;
            internal WWW <$>_www;
            internal Request<T> <>f__this;
            internal AliveResponse <aliveResp>__1;
            internal Exception <ex>__2;
            internal string <responseBody>__0;

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
                        Request<T>.Log("sending:" + this._www.url);
                        this.$current = this._www;
                        this.$PC = 1;
                        return true;

                    case 1:
                        this.<>f__this.ErrorMsg = this._www.error;
                        this.<responseBody>__0 = Encoding.UTF8.GetString(this._www.bytes);
                        Service.Binder.ServiceContext.TotalKbsReceived += ((float) this._www.bytes.Length) / 1024f;
                        if (this.<>f__this.Success)
                        {
                            try
                            {
                                this.<>f__this.Result = (T) this.<responseBody>__0;
                                this.<aliveResp>__1 = JsonUtils.Deserialize<AliveResponse>(this.<responseBody>__0, true);
                                if (this.<aliveResp>__1.alive)
                                {
                                    Service.Binder.EventBus.NetworkStateChanged(true);
                                }
                                else
                                {
                                    this.<>f__this.ErrorMsg = "Server Maintanance";
                                }
                            }
                            catch (Exception exception)
                            {
                                this.<ex>__2 = exception;
                                this.<>f__this.ErrorMsg = this.<ex>__2.Message;
                                Request<T>.Log(this.<ex>__2.Message);
                            }
                        }
                        this._www.Dispose();
                        this.$PC = -1;
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

