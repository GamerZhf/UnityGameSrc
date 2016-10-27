namespace AiView
{
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class StationarySkillUseAiBehaviour : AiBehaviour
    {
        [CompilerGenerated]
        private GameLogic.SkillType <SkillType>k__BackingField;
        private ManualTimer m_skillTimer = new ManualTimer();

        public StationarySkillUseAiBehaviour(CharacterInstance character, GameLogic.SkillType skillType)
        {
            base.Character = character;
            this.SkillType = skillType;
            this.m_skillTimer = new ManualTimer(1f / base.Character.AttacksPerSecond(false));
        }

        public override void preUpdate(float dt)
        {
            if (!base.Character.isExecutingSkill())
            {
                this.m_skillTimer.tick(dt);
            }
        }

        public override void update(float dt)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (!base.Character.isExecutingSkill())
            {
                CmdSetCharacterVelocityAndFacing.ExecuteStatic(base.Character, Vector3.zero, base.Character.Facing);
                if (this.m_skillTimer.Idle && activeDungeon.ActiveRoom.characterWithinAttackDistance(base.Character, base.Character.TargetCharacter))
                {
                    GameLogic.Binder.SkillSystem.activateSkill(base.Character, this.SkillType, 0.1f, null);
                    this.m_skillTimer.reset();
                }
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
            set
            {
                this.<SkillType>k__BackingField = value;
            }
        }
    }
}

