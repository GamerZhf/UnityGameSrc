namespace Service
{
    using App;
    using PlayerView;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using UnityEngine;

    public class CustomParams : ResourceLoader
    {
        [CompilerGenerated]
        private int <ActiveHours>k__BackingField;
        [CompilerGenerated]
        private SpriteAtlasEntry <ButtonIcon>k__BackingField;
        [CompilerGenerated]
        private bool <ConsumeOnAction>k__BackingField;
        [CompilerGenerated]
        private bool <HideTimer>k__BackingField;
        [CompilerGenerated]
        private string <Placement>k__BackingField;
        [CompilerGenerated]
        private Dictionary<string, object> <PlayerConditions>k__BackingField;
        [CompilerGenerated]
        private int <Priority>k__BackingField;
        [CompilerGenerated]
        private bool <ShowInTaskPanel>k__BackingField;
        [CompilerGenerated]
        private List<string> <Triggers>k__BackingField;
        private const char ARG_SPLIT = ',';
        private const string FALLBACK_LANG = "en";
        private WWW loaderPopupImage;
        private WWW loaderShopBanner;
        private WWW loaderShopIcon;
        private const string LOCA_ARTWORK_LANG = "[LANG]";
        private const string PARAM_ACTIVE_HOURS = "promotion-active-for-hours";
        private const string PARAM_CONSUME = "promotion-consume-on-action";
        private const string PARAM_HIDE_TIMER = "promotion-hide-timer";
        private const string PARAM_PLACEMENT = "promotion-placement";
        private const string PARAM_POPUP_BUTTON = "promotion-button-icon";
        private const string PARAM_POPUP_IMAGE = "promotion-image";
        private const string PARAM_PRIORITY = "promotion-priority";
        private const string PARAM_SHOP_ICON = "promotion-shopicon";
        private const string PARAM_SHOP_IMAGE = "shopbanner-image";
        private const string PARAM_SHOW_TASKP = "promotion-show-taskpanel";
        private const string PARAM_TRIGGERS = "promotion-triggers";
        private const string PLAYER_COND_PRE = "playercondition-";
        private Texture2D popupImage;
        private readonly Dictionary<string, object> rawParams;
        private Texture2D shopBannerImage;
        private Texture2D shopIcon;

        public CustomParams(Dictionary<string, object> promoParams)
        {
            if (promoParams != null)
            {
                object obj2;
                object obj3;
                object obj4;
                object obj5;
                object obj6;
                object obj7;
                object obj8;
                this.rawParams = promoParams;
                promoParams.TryGetValue("promotion-priority", out obj2);
                if (obj2 != null)
                {
                    this.Priority = int.Parse((string) obj2);
                }
                promoParams.TryGetValue("promotion-active-for-hours", out obj3);
                if (obj3 != null)
                {
                    this.ActiveHours = int.Parse((string) obj3);
                }
                promoParams.TryGetValue("promotion-consume-on-action", out obj4);
                this.ConsumeOnAction = obj4 != null;
                promoParams.TryGetValue("promotion-hide-timer", out obj5);
                this.HideTimer = obj5 != null;
                promoParams.TryGetValue("promotion-show-taskpanel", out obj6);
                this.ShowInTaskPanel = obj6 != null;
                promoParams.TryGetValue("promotion-placement", out obj7);
                if (obj7 != null)
                {
                    this.Placement = RemotePromotion.FixPromoSlot(obj7.ToString());
                }
                promoParams.TryGetValue("promotion-button-icon", out obj8);
                if (obj8 != null)
                {
                    char[] separator = new char[] { '.' };
                    string[] strArray = obj8.ToString().Split(separator);
                    if (strArray.Length == 2)
                    {
                        this.ButtonIcon = new SpriteAtlasEntry(strArray[0], strArray[1]);
                    }
                }
                this.InitTriggers(promoParams);
                this.InitPlayerConditions(promoParams);
            }
        }

        private void DownscaleOnLowEndDevices(ref Texture2D tex)
        {
            if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Low)
            {
                TextureScale.Bilinear(tex, (int) (tex.width * 0.35f), (int) (tex.height * 0.35f));
            }
        }

        private string GetLocalizedURL(string url, string lang)
        {
            return url.Replace("[LANG]", lang);
        }

        private void InitPlayerConditions(Dictionary<string, object> promoParams)
        {
            this.PlayerConditions = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> pair in promoParams)
            {
                if (pair.Key.Contains("playercondition-"))
                {
                    string key = pair.Key.Replace("playercondition-", string.Empty);
                    this.PlayerConditions.Add(key, ValueParser.Parse(pair.Value));
                }
            }
        }

        private void InitTriggers(Dictionary<string, object> promoParams)
        {
            object obj2;
            promoParams.TryGetValue("promotion-triggers", out obj2);
            if (obj2 != null)
            {
                Regex.Replace(obj2.ToString(), @"\s+", string.Empty);
                char[] separator = new char[] { ',' };
                string[] collection = obj2.ToString().Split(separator);
                this.Triggers = new List<string>(collection);
            }
            else
            {
                this.Triggers = new List<string>();
            }
        }

        private bool IsLocalizedURL(string url)
        {
            return url.Contains("[LANG]");
        }

        [DebuggerHidden]
        private IEnumerator LoadPopupImage(string rawURL)
        {
            <LoadPopupImage>c__Iterator22B iteratorb = new <LoadPopupImage>c__Iterator22B();
            iteratorb.rawURL = rawURL;
            iteratorb.<$>rawURL = rawURL;
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        [DebuggerHidden]
        public override IEnumerator LoadResources()
        {
            <LoadResources>c__Iterator228 iterator = new <LoadResources>c__Iterator228();
            iterator.<>f__this = this;
            return iterator;
        }

        [DebuggerHidden]
        private IEnumerator LoadShopBanner(string rawURL)
        {
            <LoadShopBanner>c__Iterator22A iteratora = new <LoadShopBanner>c__Iterator22A();
            iteratora.rawURL = rawURL;
            iteratora.<$>rawURL = rawURL;
            iteratora.<>f__this = this;
            return iteratora;
        }

        [DebuggerHidden]
        private IEnumerator LoadShopIcon(string rawURL)
        {
            <LoadShopIcon>c__Iterator229 iterator = new <LoadShopIcon>c__Iterator229();
            iterator.rawURL = rawURL;
            iterator.<$>rawURL = rawURL;
            iterator.<>f__this = this;
            return iterator;
        }

        private void SetCompression(ref Texture2D tex)
        {
            tex.Compress(false);
        }

        public virtual bool Validate()
        {
            return ((((this.loaderShopIcon == null) || (this.loaderShopIcon.error == null)) && ((this.loaderPopupImage == null) || (this.loaderPopupImage.error == null))) && ((this.loaderShopBanner == null) || (this.loaderShopBanner.error == null)));
        }

        public int ActiveHours
        {
            [CompilerGenerated]
            get
            {
                return this.<ActiveHours>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ActiveHours>k__BackingField = value;
            }
        }

        public SpriteAtlasEntry ButtonIcon
        {
            [CompilerGenerated]
            get
            {
                return this.<ButtonIcon>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ButtonIcon>k__BackingField = value;
            }
        }

        public bool ConsumeOnAction
        {
            [CompilerGenerated]
            get
            {
                return this.<ConsumeOnAction>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ConsumeOnAction>k__BackingField = value;
            }
        }

        public bool HideTimer
        {
            [CompilerGenerated]
            get
            {
                return this.<HideTimer>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<HideTimer>k__BackingField = value;
            }
        }

        public string Placement
        {
            [CompilerGenerated]
            get
            {
                return this.<Placement>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Placement>k__BackingField = value;
            }
        }

        public Dictionary<string, object> PlayerConditions
        {
            [CompilerGenerated]
            get
            {
                return this.<PlayerConditions>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PlayerConditions>k__BackingField = value;
            }
        }

        public Texture2D PopupImage
        {
            get
            {
                if (this.popupImage == null)
                {
                    if ((this.loaderPopupImage == null) || (this.loaderPopupImage.error != null))
                    {
                        return null;
                    }
                    this.popupImage = this.loaderPopupImage.texture;
                    this.DownscaleOnLowEndDevices(ref this.popupImage);
                    this.SetCompression(ref this.popupImage);
                }
                return this.popupImage;
            }
        }

        public int Priority
        {
            [CompilerGenerated]
            get
            {
                return this.<Priority>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Priority>k__BackingField = value;
            }
        }

        public Texture2D ShopBannerImage
        {
            get
            {
                if (this.shopBannerImage == null)
                {
                    if ((this.loaderShopBanner == null) || (this.loaderShopBanner.error != null))
                    {
                        return null;
                    }
                    this.shopBannerImage = this.loaderShopBanner.texture;
                    this.DownscaleOnLowEndDevices(ref this.shopBannerImage);
                    this.SetCompression(ref this.shopBannerImage);
                }
                return this.shopBannerImage;
            }
        }

        public Texture2D ShopIcon
        {
            get
            {
                if (this.shopIcon == null)
                {
                    if ((this.loaderShopIcon == null) || (this.loaderShopIcon.error != null))
                    {
                        return null;
                    }
                    this.shopIcon = this.loaderShopIcon.texture;
                    this.DownscaleOnLowEndDevices(ref this.shopIcon);
                    this.SetCompression(ref this.shopIcon);
                }
                return this.shopIcon;
            }
        }

        public bool ShowInTaskPanel
        {
            [CompilerGenerated]
            get
            {
                return this.<ShowInTaskPanel>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ShowInTaskPanel>k__BackingField = value;
            }
        }

        public List<string> Triggers
        {
            [CompilerGenerated]
            get
            {
                return this.<Triggers>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Triggers>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <LoadPopupImage>c__Iterator22B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal string <$>rawURL;
            internal CustomParams <>f__this;
            internal string rawURL;

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
                        if (!this.<>f__this.IsLocalizedURL(this.rawURL))
                        {
                            this.<>f__this.loaderPopupImage = new WWW(this.rawURL);
                            this.$current = this.<>f__this.loaderPopupImage;
                            this.$PC = 3;
                        }
                        else
                        {
                            this.<>f__this.loaderPopupImage = new WWW(this.<>f__this.GetLocalizedURL(this.rawURL, App.Binder.LocaSystem.DisplayLanguage));
                            this.$current = this.<>f__this.loaderPopupImage;
                            this.$PC = 1;
                        }
                        goto Label_014E;

                    case 1:
                        if (this.<>f__this.loaderPopupImage.error == null)
                        {
                            break;
                        }
                        UnityEngine.Debug.LogWarning("could not load Image for language -" + App.Binder.LocaSystem.DisplayLanguage + "- trying to load fallback. Error: " + this.<>f__this.loaderPopupImage.error);
                        this.<>f__this.loaderPopupImage = new WWW(this.<>f__this.GetLocalizedURL(this.rawURL, "en"));
                        this.$current = this.<>f__this.loaderPopupImage;
                        this.$PC = 2;
                        goto Label_014E;

                    case 2:
                    case 3:
                        break;

                    default:
                        goto Label_014C;
                }
                this.$PC = -1;
            Label_014C:
                return false;
            Label_014E:
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

        [CompilerGenerated]
        private sealed class <LoadResources>c__Iterator228 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CustomParams <>f__this;
            internal object <banner>__1;
            internal object <icon>__0;
            internal object <popup>__2;

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
                        if (this.<>f__this.rawParams != null)
                        {
                            this.<>f__this.rawParams.TryGetValue("promotion-shopicon", out this.<icon>__0);
                            if (this.<icon>__0 != null)
                            {
                                this.$current = this.<>f__this.LoadShopIcon((string) this.<icon>__0);
                                this.$PC = 1;
                                goto Label_0134;
                            }
                            break;
                        }
                        goto Label_0132;

                    case 1:
                        break;

                    case 2:
                        goto Label_00DC;

                    case 3:
                        goto Label_012B;

                    default:
                        goto Label_0132;
                }
                this.<>f__this.rawParams.TryGetValue("shopbanner-image", out this.<banner>__1);
                if (this.<banner>__1 != null)
                {
                    this.$current = this.<>f__this.LoadShopBanner((string) this.<banner>__1);
                    this.$PC = 2;
                    goto Label_0134;
                }
            Label_00DC:
                this.<>f__this.rawParams.TryGetValue("promotion-image", out this.<popup>__2);
                if (this.<popup>__2 != null)
                {
                    this.$current = this.<>f__this.LoadPopupImage((string) this.<popup>__2);
                    this.$PC = 3;
                    goto Label_0134;
                }
            Label_012B:
                this.$PC = -1;
            Label_0132:
                return false;
            Label_0134:
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

        [CompilerGenerated]
        private sealed class <LoadShopBanner>c__Iterator22A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal string <$>rawURL;
            internal CustomParams <>f__this;
            internal string rawURL;

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
                        if (!this.<>f__this.IsLocalizedURL(this.rawURL))
                        {
                            this.<>f__this.loaderShopBanner = new WWW(this.rawURL);
                            this.$current = this.<>f__this.loaderShopBanner;
                            this.$PC = 3;
                        }
                        else
                        {
                            this.<>f__this.loaderShopBanner = new WWW(this.<>f__this.GetLocalizedURL(this.rawURL, App.Binder.LocaSystem.DisplayLanguage));
                            this.$current = this.<>f__this.loaderShopBanner;
                            this.$PC = 1;
                        }
                        goto Label_014E;

                    case 1:
                        if (this.<>f__this.loaderShopBanner.error == null)
                        {
                            break;
                        }
                        UnityEngine.Debug.LogWarning("could not load Image for language -" + App.Binder.LocaSystem.DisplayLanguage + "- trying to load fallback. Error: " + this.<>f__this.loaderShopBanner.error);
                        this.<>f__this.loaderShopBanner = new WWW(this.<>f__this.GetLocalizedURL(this.rawURL, "en"));
                        this.$current = this.<>f__this.loaderShopBanner;
                        this.$PC = 2;
                        goto Label_014E;

                    case 2:
                    case 3:
                        break;

                    default:
                        goto Label_014C;
                }
                this.$PC = -1;
            Label_014C:
                return false;
            Label_014E:
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

        [CompilerGenerated]
        private sealed class <LoadShopIcon>c__Iterator229 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal string <$>rawURL;
            internal CustomParams <>f__this;
            internal string rawURL;

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
                        if (!this.<>f__this.IsLocalizedURL(this.rawURL))
                        {
                            this.<>f__this.loaderShopIcon = new WWW(this.rawURL);
                            this.$current = this.<>f__this.loaderShopIcon;
                            this.$PC = 3;
                        }
                        else
                        {
                            this.<>f__this.loaderShopIcon = new WWW(this.<>f__this.GetLocalizedURL(this.rawURL, App.Binder.LocaSystem.DisplayLanguage));
                            this.$current = this.<>f__this.loaderShopIcon;
                            this.$PC = 1;
                        }
                        goto Label_014E;

                    case 1:
                        if (this.<>f__this.loaderShopIcon.error == null)
                        {
                            break;
                        }
                        UnityEngine.Debug.LogWarning("could not load Image for language -" + App.Binder.LocaSystem.DisplayLanguage + "- trying to load fallback. Error: " + this.<>f__this.loaderShopIcon.error);
                        this.<>f__this.loaderShopIcon = new WWW(this.<>f__this.GetLocalizedURL(this.rawURL, "en"));
                        this.$current = this.<>f__this.loaderShopIcon;
                        this.$PC = 2;
                        goto Label_014E;

                    case 2:
                    case 3:
                        break;

                    default:
                        goto Label_014C;
                }
                this.$PC = -1;
            Label_014C:
                return false;
            Label_014E:
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
    }
}

