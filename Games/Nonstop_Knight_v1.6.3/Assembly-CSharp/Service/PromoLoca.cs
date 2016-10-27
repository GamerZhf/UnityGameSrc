namespace Service
{
    using App;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class PromoLoca
    {
        [CompilerGenerated]
        private string <PopupBody>k__BackingField;
        [CompilerGenerated]
        private string <PopupButton>k__BackingField;
        [CompilerGenerated]
        private string <PopupDescription>k__BackingField;
        [CompilerGenerated]
        private string <PopupHeadline>k__BackingField;
        [CompilerGenerated]
        private List<Mission> <PopupMissions>k__BackingField;
        [CompilerGenerated]
        private string <PopupMissionsBigPrize>k__BackingField;
        [CompilerGenerated]
        private string <PopupTitle>k__BackingField;
        [CompilerGenerated]
        private string <ShopBannerCta>k__BackingField;
        [CompilerGenerated]
        private string <ShopBannerTitle>k__BackingField;
        private const string POPUP_BODY = "popup_body";
        private const string POPUP_BUTTON = "popup_button";
        private const string POPUP_DESCRIPTION = "popup_description";
        private const string POPUP_HEADLINE = "popup_headline";
        private const string POPUP_MISSION1 = "popup_mission1";
        private const string POPUP_MISSION2 = "popup_mission2";
        private const string POPUP_MISSION3 = "popup_mission3";
        private const string POPUP_MISSIONS_BIG_PRIZE = "popup_missions_bigprize";
        private const string POPUP_TITLE = "popup_title";
        private const string SHOPBANNER_CTA = "shop_banner_cta";
        private const string SHOPBANNER_TITLE = "shop_banner_title";

        public PromoLoca(Dictionary<string, object> localized)
        {
            this.PopupMissions = new List<Mission>();
            if (localized != null)
            {
                object obj2;
                object obj3;
                object obj4;
                object obj5;
                object obj6;
                object obj7;
                object obj8;
                object obj9;
                LocaSystem locaSystem = App.Binder.LocaSystem;
                if (locaSystem.IsRightToLeft(locaSystem.selectedLanguage))
                {
                    localized = this.FixRTLText(localized);
                }
                localized.TryGetValue("popup_headline", out obj2);
                if (obj2 != null)
                {
                    this.PopupHeadline = obj2 as string;
                }
                localized.TryGetValue("popup_title", out obj3);
                if (obj3 != null)
                {
                    this.PopupTitle = obj3 as string;
                }
                localized.TryGetValue("popup_body", out obj4);
                if (obj4 != null)
                {
                    this.PopupBody = obj4 as string;
                }
                localized.TryGetValue("popup_button", out obj5);
                if (obj5 != null)
                {
                    this.PopupButton = obj5 as string;
                }
                localized.TryGetValue("popup_description", out obj6);
                if (obj6 != null)
                {
                    this.PopupDescription = obj6 as string;
                }
                localized.TryGetValue("popup_missions_bigprize", out obj7);
                if (obj7 != null)
                {
                    this.PopupMissionsBigPrize = obj7 as string;
                }
                this.ParseMission(localized, "popup_mission1");
                this.ParseMission(localized, "popup_mission2");
                this.ParseMission(localized, "popup_mission3");
                localized.TryGetValue("shop_banner_title", out obj8);
                if (obj8 != null)
                {
                    this.ShopBannerTitle = obj8 as string;
                }
                localized.TryGetValue("shop_banner_cta", out obj9);
                if (obj9 != null)
                {
                    this.ShopBannerCta = obj9 as string;
                }
            }
        }

        private Dictionary<string, object> FixRTLText(Dictionary<string, object> localized)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (string str in localized.Keys)
            {
                string str2 = localized[str] as string;
                if (string.IsNullOrEmpty(str2))
                {
                    dictionary.Add(str, localized[str]);
                }
                else
                {
                    dictionary.Add(str, _.ApplyArabicReverse(str2));
                }
            }
            return dictionary;
        }

        private void ParseMission(Dictionary<string, object> localized, string paramId)
        {
            object obj2;
            localized.TryGetValue(paramId, out obj2);
            if (obj2 != null)
            {
                char[] separator = new char[] { ';' };
                string[] strArray = obj2.ToString().Split(separator);
                if (strArray.Length == 2)
                {
                    Mission item = new Mission();
                    item.Title = strArray[0];
                    item.Description = strArray[1];
                    this.PopupMissions.Add(item);
                }
            }
        }

        public bool ValidatePopupLoca(bool allowsNoButton)
        {
            bool flag = !string.IsNullOrEmpty(this.PopupButton) || allowsNoButton;
            if (!flag)
            {
                if (ConfigApp.ProductionBuild)
                {
                    Debug.LogWarning("Loca Validation failed: one of the mandatory popup loca keys is missing");
                    return flag;
                }
                Debug.Log("Loca Validation failed: one of the mandatory popup loca keys is missing");
            }
            return flag;
        }

        public bool ValidateShopBannerLoca()
        {
            return true;
        }

        public string PopupBody
        {
            [CompilerGenerated]
            get
            {
                return this.<PopupBody>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PopupBody>k__BackingField = value;
            }
        }

        public string PopupButton
        {
            [CompilerGenerated]
            get
            {
                return this.<PopupButton>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PopupButton>k__BackingField = value;
            }
        }

        public string PopupDescription
        {
            [CompilerGenerated]
            get
            {
                return this.<PopupDescription>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PopupDescription>k__BackingField = value;
            }
        }

        public string PopupHeadline
        {
            [CompilerGenerated]
            get
            {
                return this.<PopupHeadline>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PopupHeadline>k__BackingField = value;
            }
        }

        public List<Mission> PopupMissions
        {
            [CompilerGenerated]
            get
            {
                return this.<PopupMissions>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PopupMissions>k__BackingField = value;
            }
        }

        public string PopupMissionsBigPrize
        {
            [CompilerGenerated]
            get
            {
                return this.<PopupMissionsBigPrize>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PopupMissionsBigPrize>k__BackingField = value;
            }
        }

        public string PopupTitle
        {
            [CompilerGenerated]
            get
            {
                return this.<PopupTitle>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PopupTitle>k__BackingField = value;
            }
        }

        public string ShopBannerCta
        {
            [CompilerGenerated]
            get
            {
                return this.<ShopBannerCta>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ShopBannerCta>k__BackingField = value;
            }
        }

        public string ShopBannerTitle
        {
            [CompilerGenerated]
            get
            {
                return this.<ShopBannerTitle>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ShopBannerTitle>k__BackingField = value;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Mission
        {
            public string Title;
            public string Description;
        }
    }
}

