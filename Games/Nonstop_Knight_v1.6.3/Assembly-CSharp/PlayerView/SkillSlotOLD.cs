namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class SkillSlotOLD : DragDropTarget, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IDropHandler
    {
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        [CompilerGenerated]
        private PlayerView.SkillMenu <SkillMenu>k__BackingField;
        public UnityEngine.UI.Text SkillName;
        public UnityEngine.UI.Text Text;

        protected override bool dropAllowed(GameObject dragObject)
        {
            if (this.SkillMenu.DraggedSkillToggle == null)
            {
                return false;
            }
            return true;
        }

        public void initialize(PlayerView.SkillMenu skillMenu, int skillIndex)
        {
            this.SkillMenu = skillMenu;
            this.Text.text = (skillIndex + 1).ToString();
            base.ReceivingImage.enabled = false;
        }

        protected override void onAwake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            this.SkillName.text = string.Empty;
        }

        protected override void onDrop(GameObject dragObject)
        {
            SkillToggle draggedSkillToggle = this.SkillMenu.DraggedSkillToggle;
            this.SkillName.text = draggedSkillToggle.Name.text;
            CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
            GameLogic.Binder.CommandProcessor.execute(new CmdAssignActiveSkill(activeCharacter, draggedSkillToggle.SkillInstance.SkillType), 0f);
        }

        public void refreshContent(SkillInstance si)
        {
            if ((si == null) || (si.SkillType == SkillType.NONE))
            {
                base.ReceivingImage.enabled = false;
                this.SkillName.text = string.Empty;
            }
            else
            {
                ConfigSkills.SharedData data = ConfigSkills.SHARED_DATA[si.SkillType];
                base.ReceivingImage.enabled = true;
                base.ReceivingImage.sprite = PlayerView.Binder.SpriteResources.getSprite(data.Spritesheet, data.Sprite);
                this.SkillName.text = data.Name;
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

