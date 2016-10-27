namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class SkillTooltip : MenuContent
    {
        [CompilerGenerated]
        private GameLogic.SkillType <SkillType>k__BackingField;
        public RectTransform ArrowRectTm;
        private RectTransform m_clickedRectTm;
        public Text Title;

        protected override void onAwake()
        {
        }

        protected override void onCleanup()
        {
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.SkillType = (GameLogic.SkillType) ((int) param);
            this.m_clickedRectTm = PlayerView.Binder.InputSystem.getRectTransformUnderMouse();
            this.onRefresh();
        }

        protected override void onRefresh()
        {
            Camera menuCamera = PlayerView.Binder.MenuSystem.MenuCamera;
            if (this.m_clickedRectTm != null)
            {
                Vector3 vector2;
                Vector3 screenPoint = menuCamera.WorldToScreenPoint(this.m_clickedRectTm.position);
                RectTransformUtility.ScreenPointToWorldPointInRectangle(base.RectTm, screenPoint, menuCamera, out vector2);
                this.ArrowRectTm.position = new Vector3(vector2.x, this.ArrowRectTm.position.y, this.ArrowRectTm.position.z);
            }
            else
            {
                Debug.LogWarning("Valid RectTransform not found for click handling.");
            }
        }

        public void onSetActiveButtonClicked(int slotIdx)
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
                GameLogic.Binder.CommandProcessor.execute(new CmdAssignActiveSkill(activeCharacter, this.SkillType), 0f);
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(false);
            }
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.SkillTooltip;
            }
        }

        public GameLogic.SkillType SkillType
        {
            [CompilerGenerated]
            get
            {
                return this.<SkillType>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<SkillType>k__BackingField = value;
            }
        }
    }
}

