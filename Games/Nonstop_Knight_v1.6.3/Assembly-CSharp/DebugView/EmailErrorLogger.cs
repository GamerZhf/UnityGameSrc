namespace DebugView
{
    using App;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;
    using UnityEngine;

    public class EmailErrorLogger : MonoBehaviour
    {
        private int m_mailCounter;

        private bool isValidMessage(string condition)
        {
            if (condition.Contains("Skipping profile frame"))
            {
                return false;
            }
            return true;
        }

        protected void OnDisable()
        {
            Application.logMessageReceived -= new Application.LogCallback(this.onLogMessageReceived);
        }

        protected void OnEnable()
        {
            Application.logMessageReceived += new Application.LogCallback(this.onLogMessageReceived);
            this.m_mailCounter = 0;
        }

        private void onLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            if ((!Application.isEditor && !ConfigApp.CHEAT_MARKETING_MODE_ENABLED) && ((this.m_mailCounter < 2) && this.isValidMessage(condition)))
            {
                if (((type == LogType.Error) || (type == LogType.Exception)) || (type == LogType.Assert))
                {
                    string[] to = new string[] { "Kopla <bot@koplagames.com>", "Olaf Rauland <olafrauland@flaregames.com>", "Timo Boll <timo@flaregames.com>" };
                    string[] textArray2 = new string[] { ConfigApp.ProductName, "-", ConfigApp.BundleVersion, "-", App.Binder.BuildResources.getPrettyCommitId(), " - ERROR" };
                    base.StartCoroutine(this.sendMailRoutine(to, string.Concat(textArray2), condition + "\n" + stackTrace));
                    this.m_mailCounter++;
                }
                else if (type == LogType.Warning)
                {
                    string[] textArray3 = new string[] { "Kopla <bot@koplagames.com>", "Olaf Rauland <olafrauland@flaregames.com>", "Timo Boll <timo@flaregames.com>" };
                    string[] textArray4 = new string[] { ConfigApp.ProductName, "-", ConfigApp.BundleVersion, "-", App.Binder.BuildResources.getPrettyCommitId(), " - WARNING" };
                    base.StartCoroutine(this.sendMailRoutine(textArray3, string.Concat(textArray4), condition + "\n" + stackTrace));
                    this.m_mailCounter++;
                }
            }
        }

        [DebuggerHidden]
        private IEnumerator sendMailRoutine(string[] to, string subject, string text)
        {
            <sendMailRoutine>c__Iterator3B iteratorb = new <sendMailRoutine>c__Iterator3B();
            iteratorb.text = text;
            iteratorb.to = to;
            iteratorb.subject = subject;
            iteratorb.<$>text = text;
            iteratorb.<$>to = to;
            iteratorb.<$>subject = subject;
            return iteratorb;
        }

        [ContextMenu("sendTestMail()")]
        private void sendTestMail()
        {
            string[] to = new string[] { "Kopla <bot@koplagames.com>" };
            string[] textArray2 = new string[] { ConfigApp.ProductName, "-", ConfigApp.BundleVersion, "-", App.Binder.BuildResources.getPrettyCommitId(), " - TEST" };
            base.StartCoroutine(this.sendMailRoutine(to, string.Concat(textArray2), "test\ntext"));
        }

        [CompilerGenerated]
        private sealed class <sendMailRoutine>c__Iterator3B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal string <$>subject;
            internal string <$>text;
            internal string[] <$>to;
            internal WWWForm <form>__2;
            internal Dictionary<string, string> <headers>__3;
            internal int <i>__1;
            internal byte[] <rawData>__4;
            internal string <server>__0;
            internal WWW <www>__5;
            internal string subject;
            internal string text;
            internal string[] to;

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
                    {
                        this.<server>__0 = string.Empty;
                        if ((Service.Binder.SessionData != null) && !string.IsNullOrEmpty(Service.Binder.SessionData.ServerUrl))
                        {
                            this.<server>__0 = Service.Binder.SessionData.ServerUrl;
                        }
                        object[] objArray1 = new object[] { 
                            this.text, "\n--- BUILD & DEVICE INFO ---\n\n", "buildId: ", ConfigApp.BundleVersion, "-", App.Binder.BuildResources.getPrettyCommitId(), "\n", "buildType: ", App.Binder.BuildResources.getBuildTypeDescription(), "\n", "server: ", this.<server>__0, "\n", "operatingSystem: ", SystemInfo.operatingSystem, "\n", 
                            "deviceModel: ", SystemInfo.deviceModel, "\n", "deviceName: ", SystemInfo.deviceName, "\n", "deviceType: ", SystemInfo.deviceType, "\n", "deviceUniqueIdentifier: ", SystemInfo.deviceUniqueIdentifier, "\n", "graphicsDeviceID: ", SystemInfo.graphicsDeviceID, "\n", "graphicsDeviceName: ", 
                            SystemInfo.graphicsDeviceName, "\n", "graphicsDeviceType: ", SystemInfo.graphicsDeviceType, "\n", "graphicsDeviceVendor: ", SystemInfo.graphicsDeviceVendor, "\n", "graphicsDeviceVendorID: ", SystemInfo.graphicsDeviceVendorID, "\n", "graphicsDeviceVersion: ", SystemInfo.graphicsDeviceVersion, "\n", "graphicsMemorySize: ", SystemInfo.graphicsMemorySize, 
                            "\n", "graphicsMultiThreaded: ", SystemInfo.graphicsMultiThreaded, "\n", "graphicsShaderLevel: ", SystemInfo.graphicsShaderLevel, "\n", "maxTextureSize: ", SystemInfo.maxTextureSize, "\n", "processorCount: ", SystemInfo.processorCount, "\n", "processorType: ", SystemInfo.processorType, "\n", 
                            "supportedRenderTargetCount: ", SystemInfo.supportedRenderTargetCount, "\n", "supportsStencil: ", SystemInfo.supportsStencil, "\n", "systemMemorySize: ", SystemInfo.systemMemorySize
                         };
                        this.text = string.Concat(objArray1);
                        this.<i>__1 = 0;
                        while (this.<i>__1 < this.to.Length)
                        {
                            this.<form>__2 = new WWWForm();
                            this.<form>__2.AddField("from", "Mailgun Sandbox <postmaster@sandboxa7e32847fd1d4a6da00e0f46d8fd9359.mailgun.org>");
                            this.<form>__2.AddField("to", this.to[this.<i>__1]);
                            this.<form>__2.AddField("subject", this.subject);
                            this.<form>__2.AddField("text", this.text);
                            this.<headers>__3 = this.<form>__2.headers;
                            this.<rawData>__4 = this.<form>__2.data;
                            this.<headers>__3["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("api:key-394997196d628742a827e097fe48cf1e"));
                            this.<www>__5 = new WWW("https://api.mailgun.net/v3/sandboxa7e32847fd1d4a6da00e0f46d8fd9359.mailgun.org/messages", this.<rawData>__4, this.<headers>__3);
                            this.$current = this.<www>__5;
                            this.$PC = 1;
                            return true;
                        Label_042C:
                            if (!string.IsNullOrEmpty(this.<www>__5.error))
                            {
                                UnityEngine.Debug.Log("Error in sending email: " + this.<www>__5.error);
                            }
                            this.<i>__1++;
                        }
                        break;
                    }
                    case 1:
                        goto Label_042C;

                    default:
                        break;
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

