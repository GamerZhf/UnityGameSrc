namespace Appboy.Models.InAppMessage
{
    using Appboy.Models;
    using Appboy.Utilities;
    using System;
    using System.Runtime.CompilerServices;

    public class InAppMessageSlideup : InAppMessageBase
    {
        [CompilerGenerated]
        private Color? <ChevronColor>k__BackingField;
        [CompilerGenerated]
        private bool <HideChevron>k__BackingField;
        [CompilerGenerated]
        private SlideFrom <SlideupAnchor>k__BackingField;

        public InAppMessageSlideup()
        {
        }

        public InAppMessageSlideup(JSONClass json) : base(json)
        {
            this.SlideupAnchor = (SlideFrom) ((int) EnumUtils.TryParse(typeof(SlideFrom), (string) json["slide_from"], true, SlideFrom.BOTTOM));
            this.ChevronColor = ColorUtils.HexToColor((string) json["close_btn_color"]);
            this.HideChevron = json["hide_chevron"].AsBool;
        }

        public override string ToString()
        {
            object[] args = new object[] { base.ToString(), this.SlideupAnchor, this.HideChevron, this.ChevronColor };
            return string.Format("{0}, SlideupAnchor={1}, HideChevron={2}, ChevronColor={3}", args);
        }

        public Color? ChevronColor
        {
            [CompilerGenerated]
            get
            {
                return this.<ChevronColor>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ChevronColor>k__BackingField = value;
            }
        }

        public bool HideChevron
        {
            [CompilerGenerated]
            get
            {
                return this.<HideChevron>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<HideChevron>k__BackingField = value;
            }
        }

        public SlideFrom SlideupAnchor
        {
            [CompilerGenerated]
            get
            {
                return this.<SlideupAnchor>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<SlideupAnchor>k__BackingField = value;
            }
        }
    }
}

