namespace Facebook.Unity
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    internal class AsyncRequestString : MonoBehaviour
    {
        private FacebookDelegate<IGraphResult> callback;
        private IDictionary<string, string> formData;
        private HttpMethod method;
        private WWWForm query;
        private Uri url;

        internal static void Get(Uri url, [Optional, DefaultParameterValue(null)] Dictionary<string, string> formData, [Optional, DefaultParameterValue(null)] FacebookDelegate<IGraphResult> callback)
        {
            Request(url, HttpMethod.GET, formData, callback);
        }

        internal static void Post(Uri url, [Optional, DefaultParameterValue(null)] Dictionary<string, string> formData, [Optional, DefaultParameterValue(null)] FacebookDelegate<IGraphResult> callback)
        {
            Request(url, HttpMethod.POST, formData, callback);
        }

        internal static void Request(Uri url, HttpMethod method, [Optional, DefaultParameterValue(null)] IDictionary<string, string> formData, [Optional, DefaultParameterValue(null)] FacebookDelegate<IGraphResult> callback)
        {
            ComponentFactory.AddComponent<AsyncRequestString>().SetUrl(url).SetMethod(method).SetFormData(formData).SetCallback(callback);
        }

        internal static void Request(Uri url, HttpMethod method, [Optional, DefaultParameterValue(null)] WWWForm query, [Optional, DefaultParameterValue(null)] FacebookDelegate<IGraphResult> callback)
        {
            ComponentFactory.AddComponent<AsyncRequestString>().SetUrl(url).SetMethod(method).SetQuery(query).SetCallback(callback);
        }

        internal AsyncRequestString SetCallback(FacebookDelegate<IGraphResult> callback)
        {
            this.callback = callback;
            return this;
        }

        internal AsyncRequestString SetFormData(IDictionary<string, string> formData)
        {
            this.formData = formData;
            return this;
        }

        internal AsyncRequestString SetMethod(HttpMethod method)
        {
            this.method = method;
            return this;
        }

        internal AsyncRequestString SetQuery(WWWForm query)
        {
            this.query = query;
            return this;
        }

        internal AsyncRequestString SetUrl(Uri url)
        {
            this.url = url;
            return this;
        }

        [DebuggerHidden]
        internal IEnumerator Start()
        {
            <Start>c__Iterator24 iterator = new <Start>c__Iterator24();
            iterator.<>f__this = this;
            return iterator;
        }

        [CompilerGenerated]
        private sealed class <Start>c__Iterator24 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal IEnumerator<KeyValuePair<string, string>> <$s_153>__1;
            internal IEnumerator<KeyValuePair<string, string>> <$s_154>__5;
            internal AsyncRequestString <>f__this;
            internal Dictionary<string, string> <headers>__3;
            internal KeyValuePair<string, string> <pair>__2;
            internal KeyValuePair<string, string> <pair>__6;
            internal string <urlParams>__0;
            internal WWW <www>__4;

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
                        if (this.<>f__this.method != HttpMethod.GET)
                        {
                            if (this.<>f__this.query == null)
                            {
                                this.<>f__this.query = new WWWForm();
                            }
                            if (this.<>f__this.method == HttpMethod.DELETE)
                            {
                                this.<>f__this.query.AddField("method", "delete");
                            }
                            if (this.<>f__this.formData != null)
                            {
                                this.<$s_154>__5 = this.<>f__this.formData.GetEnumerator();
                                try
                                {
                                    while (this.<$s_154>__5.MoveNext())
                                    {
                                        this.<pair>__6 = this.<$s_154>__5.Current;
                                        this.<>f__this.query.AddField(this.<pair>__6.Key, this.<pair>__6.Value);
                                    }
                                }
                                finally
                                {
                                    if (this.<$s_154>__5 == null)
                                    {
                                    }
                                    this.<$s_154>__5.Dispose();
                                }
                            }
                            this.<>f__this.query.headers["User-Agent"] = Constants.GraphApiUserAgent;
                            this.<www>__4 = new WWW(this.<>f__this.url.AbsoluteUri, this.<>f__this.query);
                            break;
                        }
                        this.<urlParams>__0 = !this.<>f__this.url.AbsoluteUri.Contains("?") ? "?" : "&";
                        if (this.<>f__this.formData != null)
                        {
                            this.<$s_153>__1 = this.<>f__this.formData.GetEnumerator();
                            try
                            {
                                while (this.<$s_153>__1.MoveNext())
                                {
                                    this.<pair>__2 = this.<$s_153>__1.Current;
                                    string introduced2 = Uri.EscapeDataString(this.<pair>__2.Key);
                                    this.<urlParams>__0 = this.<urlParams>__0 + string.Format("{0}={1}&", introduced2, Uri.EscapeDataString(this.<pair>__2.Value));
                                }
                            }
                            finally
                            {
                                if (this.<$s_153>__1 == null)
                                {
                                }
                                this.<$s_153>__1.Dispose();
                            }
                        }
                        this.<headers>__3 = new Dictionary<string, string>();
                        this.<headers>__3["User-Agent"] = Constants.GraphApiUserAgent;
                        this.<www>__4 = new WWW(this.<>f__this.url + this.<urlParams>__0, null, this.<headers>__3);
                        break;

                    case 1:
                        if (this.<>f__this.callback != null)
                        {
                            this.<>f__this.callback(new GraphResult(this.<www>__4));
                        }
                        this.<www>__4.Dispose();
                        UnityEngine.Object.Destroy(this.<>f__this);
                        this.$PC = -1;
                        goto Label_02CF;

                    default:
                        goto Label_02CF;
                }
                this.$current = this.<www>__4;
                this.$PC = 1;
                return true;
            Label_02CF:
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

