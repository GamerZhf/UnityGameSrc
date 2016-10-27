namespace Facebook.Unity.Example
{
    using Facebook.Unity;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class GraphRequest : MenuBase
    {
        private string apiQuery = string.Empty;
        private Texture2D profilePic;

        protected override void GetGui()
        {
            bool enabled = GUI.enabled;
            GUI.enabled = enabled && FB.IsLoggedIn;
            if (base.Button("Basic Request - Me"))
            {
                FB.API("/me", HttpMethod.GET, new FacebookDelegate<IGraphResult>(this.HandleResult), (IDictionary<string, string>) null);
            }
            if (base.Button("Retrieve Profile Photo"))
            {
                FB.API("/me/picture", HttpMethod.GET, new FacebookDelegate<IGraphResult>(this.ProfilePhotoCallback), (IDictionary<string, string>) null);
            }
            if (base.Button("Take and Upload screenshot"))
            {
                base.StartCoroutine(this.TakeScreenshot());
            }
            base.LabelAndTextField("Request", ref this.apiQuery);
            if (base.Button("Custom Request"))
            {
                FB.API(this.apiQuery, HttpMethod.GET, new FacebookDelegate<IGraphResult>(this.HandleResult), (IDictionary<string, string>) null);
            }
            if (this.profilePic != null)
            {
                GUILayout.Box(this.profilePic, new GUILayoutOption[0]);
            }
            GUI.enabled = enabled;
        }

        private void ProfilePhotoCallback(IGraphResult result)
        {
            if (string.IsNullOrEmpty(result.Error) && (result.Texture != null))
            {
                this.profilePic = result.Texture;
            }
            base.HandleResult(result);
        }

        [DebuggerHidden]
        private IEnumerator TakeScreenshot()
        {
            <TakeScreenshot>c__Iterator23 iterator = new <TakeScreenshot>c__Iterator23();
            iterator.<>f__this = this;
            return iterator;
        }

        [CompilerGenerated]
        private sealed class <TakeScreenshot>c__Iterator23 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal GraphRequest <>f__this;
            internal int <height>__1;
            internal byte[] <screenshot>__3;
            internal Texture2D <tex>__2;
            internal int <width>__0;
            internal WWWForm <wwwForm>__4;

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
                        this.$current = new WaitForEndOfFrame();
                        this.$PC = 1;
                        return true;

                    case 1:
                        this.<width>__0 = Screen.width;
                        this.<height>__1 = Screen.height;
                        this.<tex>__2 = new Texture2D(this.<width>__0, this.<height>__1, TextureFormat.RGB24, false);
                        this.<tex>__2.ReadPixels(new Rect(0f, 0f, (float) this.<width>__0, (float) this.<height>__1), 0, 0);
                        this.<tex>__2.Apply();
                        this.<screenshot>__3 = this.<tex>__2.EncodeToPNG();
                        this.<wwwForm>__4 = new WWWForm();
                        this.<wwwForm>__4.AddBinaryData("image", this.<screenshot>__3, "InteractiveConsole.png");
                        this.<wwwForm>__4.AddField("message", "herp derp.  I did a thing!  Did I do this right?");
                        FB.API("me/photos", HttpMethod.POST, new FacebookDelegate<IGraphResult>(this.<>f__this.HandleResult), this.<wwwForm>__4);
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

