namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class SkillToggle : DragDropSource, IPoolable
    {
        [CompilerGenerated]
        private CharacterInstance <Owner>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        [CompilerGenerated]
        private GameLogic.SkillInstance <SkillInstance>k__BackingField;
        [CompilerGenerated]
        private PlayerView.SkillMenu <SkillMenu>k__BackingField;
        public static Color DISABLED_SKILL_TOGGLE_COLOR = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);
        public UnityEngine.UI.Image Image;
        private Vector2 m_originalSizeDelta;
        public UnityEngine.UI.Text Name;
        public UnityEngine.UI.Text Text;
        public UnityEngine.UI.Toggle Toggle;

        public void cleanUpForReuse()
        {
            this.Toggle.group = null;
            this.Toggle.onValueChanged.RemoveAllListeners();
            this.RectTm.sizeDelta = this.m_originalSizeDelta;
            base.StopAllCoroutines();
        }

        protected override bool dragAllowed()
        {
            CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
            ConfigSkills.SharedData data = ConfigSkills.SHARED_DATA[this.SkillInstance.SkillType];
            return ((this.SkillMenu.DraggedSkillToggle == null) && (activeCharacter.Rank >= data.UnlockRank));
        }

        public void initialize(PlayerView.SkillMenu skillMenu, CharacterInstance owner, GameLogic.SkillInstance skillInstance)
        {
            this.SkillMenu = skillMenu;
            this.Owner = owner;
            this.SkillInstance = skillInstance;
            ConfigSkills.SharedData data = ConfigSkills.SHARED_DATA[skillInstance.SkillType];
            this.Image.sprite = PlayerView.Binder.SpriteResources.getSprite(data.Spritesheet, data.Sprite);
            this.Name.text = _.L(data.Name, null, false);
            ColorBlock colors = this.Toggle.colors;
            if (owner.Rank >= data.UnlockRank)
            {
                object[] objArray1 = new object[] { "<color=orange>", skillInstance.Rank, "/", App.Binder.ConfigMeta.GLOBAL_LEVEL_CAP, "</color>" };
                this.Text.text = string.Concat(objArray1);
                colors.normalColor = Color.white;
            }
            else
            {
                this.Text.text = "<color=red>Lvl " + data.UnlockRank + "</color>";
                colors.normalColor = DISABLED_SKILL_TOGGLE_COLOR;
            }
            this.Toggle.colors = colors;
        }

        protected override void onAwake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            this.m_originalSizeDelta = this.RectTm.sizeDelta;
        }

        protected override void onBeginDrag(GameObject dragObject)
        {
            this.SkillMenu.DraggedSkillToggle = this;
        }

        protected override void onEndDrag(GameObject dragObject)
        {
            this.SkillMenu.DraggedSkillToggle = null;
        }

        public CharacterInstance Owner
        {
            [CompilerGenerated]
            get
            {
                return this.<Owner>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Owner>k__BackingField = value;
            }
        }

        public RectTransform RectTm
        {
            [CompilerGenerated]
            get
            {
                return this.<RectTm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RectTm>k__BackingField = value;
            }
        }

        public GameLogic.SkillInstance SkillInstance
        {
            [CompilerGenerated]
            get
            {
                return this.<SkillInstance>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<SkillInstance>k__BackingField = value;
            }
        }

        public PlayerView.SkillMenu SkillMenu
        {
            [CompilerGenerated]
            get
            {
                return this.<SkillMenu>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<SkillMenu>k__BackingField = value;
            }
        }
    }
}

