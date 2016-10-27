namespace Service
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AppboyIngameMessage : ResourceLoader
    {
        [CompilerGenerated]
        private WWW <IconLoader>k__BackingField;
        [CompilerGenerated]
        private WWW <ImageLoader>k__BackingField;
        [CompilerGenerated]
        private string <JSONData>k__BackingField;
        [CompilerGenerated]
        private bool <ResourcesLoaded>k__BackingField;
        public long bg_color;
        public string campaign_id;
        public string click_action;
        public long close_btn_color;
        public int duration;
        public MessageExtras extras;
        public bool hide_chevron;
        public long icon_bg_color;
        public long icon_color;
        public string message;
        public const string MESSAGE_APPEARANCE_CUSTOM = "Custom";
        public const string MESSAGE_APPEARANCE_DEFAULT = "Default";
        public const string MESSAGE_APPEARANCE_FULL = "FullImage";
        public string message_close;
        public string slide_from;
        public long text_color;
        public string type;

        public int GetActionButtonCount()
        {
            int num = 0;
            if (this.extras != null)
            {
                if (!string.IsNullOrEmpty(this.extras.ActBtn1))
                {
                    num++;
                }
                if (!string.IsNullOrEmpty(this.extras.ActBtn2))
                {
                    num++;
                }
                if (!string.IsNullOrEmpty(this.extras.ActBtn3))
                {
                    num++;
                }
            }
            return num;
        }

        public string GetMessage()
        {
            if (this.extras == null)
            {
                return this.message;
            }
            if (string.Equals(this.extras.MsgAppearance, "Default"))
            {
                return this.message;
            }
            return this.extras.MsgBody;
        }

        [DebuggerHidden]
        public override IEnumerator LoadResources()
        {
            <LoadResources>c__Iterator201 iterator = new <LoadResources>c__Iterator201();
            iterator.<>f__this = this;
            return iterator;
        }

        public WWW IconLoader
        {
            [CompilerGenerated]
            get
            {
                return this.<IconLoader>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<IconLoader>k__BackingField = value;
            }
        }

        public WWW ImageLoader
        {
            [CompilerGenerated]
            get
            {
                return this.<ImageLoader>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ImageLoader>k__BackingField = value;
            }
        }

        public string JSONData
        {
            [CompilerGenerated]
            get
            {
                return this.<JSONData>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<JSONData>k__BackingField = value;
            }
        }

        public bool ResourcesLoaded
        {
            [CompilerGenerated]
            get
            {
                return this.<ResourcesLoaded>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ResourcesLoaded>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <LoadResources>c__Iterator201 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal AppboyIngameMessage <>f__this;

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
                        if ((this.<>f__this.extras == null) || string.IsNullOrEmpty(this.<>f__this.extras.MsgImage))
                        {
                            break;
                        }
                        this.<>f__this.ImageLoader = new WWW(this.<>f__this.extras.MsgImage);
                        this.$current = this.<>f__this.ImageLoader;
                        this.$PC = 1;
                        goto Label_011C;

                    case 1:
                        break;

                    case 2:
                        goto Label_00F3;

                    default:
                        goto Label_011A;
                }
                if ((this.<>f__this.extras != null) && !string.IsNullOrEmpty(this.<>f__this.extras.MsgIcon))
                {
                    this.<>f__this.IconLoader = new WWW(this.<>f__this.extras.MsgIcon);
                    this.$current = this.<>f__this.IconLoader;
                    this.$PC = 2;
                    goto Label_011C;
                }
            Label_00F3:
                this.<>f__this.ResourcesLoaded = true;
                if (Binder.EventBus != null)
                {
                    Binder.EventBus.AppboyIngameMessageReady();
                }
                this.$PC = -1;
            Label_011A:
                return false;
            Label_011C:
                return true;
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

        public class MessageExtras
        {
            public string ActBtn1;
            public string ActBtn2;
            public string ActBtn3;
            public string CampaignId;
            public string ForcedDisplay;
            public string MsgAppearance;
            public string MsgBody;
            public string MsgHeadline;
            public string MsgIcon;
            public string MsgImage;
            public string MsgTitle;
            public string OpenURL1;
            public string OpenURL2;
            public string OpenURL3;
        }
    }
}

