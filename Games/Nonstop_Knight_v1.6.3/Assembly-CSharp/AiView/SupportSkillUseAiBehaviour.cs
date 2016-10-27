namespace AiView
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class SupportSkillUseAiBehaviour : SupportAiBehaviour
    {
        [CompilerGenerated]
        private bool <RequiredLineOfSight>k__BackingField;
        [CompilerGenerated]
        private GameLogic.SkillType <SkillType>k__BackingField;
        private ManualTimer m_skillTimer;

        public SupportSkillUseAiBehaviour(CharacterInstance character, GameLogic.SkillType skillType, [Optional, DefaultParameterValue(false)] bool requireLineOfSight, [Optional, DefaultParameterValue(null)] List<PerkType> perkTypes) : base(character, perkTypes)
        {
            this.m_skillTimer = new ManualTimer();
            this.SkillType = skillType;
            this.RequiredLineOfSight = requireLineOfSight;
            this.m_skillTimer = new ManualTimer(1f / base.Character.AttacksPerSecond(false));
        }

        public override void preUpdate(float dt)
        {
            base.preUpdate(dt);
            if (!base.Character.isExecutingSkill())
            {
                this.m_skillTimer.tick(dt);
            }
        }

        public override void update(float dt)
        {
            base.update(dt);
            if (((((base.Character.TargetCharacter != null) && this.m_skillTimer.Idle) && base.m_movementBehaviour.WithinAttackDistance) && (!this.RequiredLineOfSight || GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.lineOfSightCharacterToCharacter(base.Character, base.Character.TargetCharacter))) && base.Character.PhysicsBody.rotatedEnoughForMovement())
            {
                GameLogic.Binder.SkillSystem.activateSkill(base.Character, this.SkillType, -1f, null);
                this.m_skillTimer.reset();
                base.seekTacticalPosition();
            }
        }

        public bool RequiredLineOfSight
        {
            [CompilerGenerated]
            get
            {
                return this.<RequiredLineOfSight>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RequiredLineOfSight>k__BackingField = value;
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

