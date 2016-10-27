namespace AiView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class BossCasterAiBehaviour : AiBehaviour
    {
        private AiBehaviour m_attackBehaviour;
        private Parameters m_parameters;
        private ManualTimer m_skillTimer = new ManualTimer();
        private MonsterTargetingAiBehaviour m_targetingBehaviour;

        public BossCasterAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
            this.m_parameters = ParseParameters(character.Character.BossAiParameters);
            this.m_targetingBehaviour = new MonsterTargetingAiBehaviour(character);
            switch (character.Character.CoreAiBehaviour)
            {
                case AiBehaviourType.MonsterRanged:
                    this.m_attackBehaviour = new PursuitAndRangedAttackAiBehaviour(character);
                    break;

                case AiBehaviourType.MonsterRangedToMelee:
                    this.m_attackBehaviour = new PursuitAndRangedMeleeAttackAiBehaviour(character);
                    break;

                case AiBehaviourType.MonsterPuffer:
                    this.m_attackBehaviour = new StationarySkillUseAiBehaviour(character, SkillType.PoisonPuff);
                    break;

                default:
                    this.m_attackBehaviour = new PursuitAndMeleeAttackAiBehaviour(character);
                    break;
            }
            for (int i = 0; i < this.m_parameters.PerkTypes.Count; i++)
            {
                PerkType perkType = this.m_parameters.PerkTypes[i];
                PerkInstance item = new PerkInstance();
                item.Type = perkType;
                item.Modifier = ConfigPerks.GetBestModifier(perkType);
                character.BossPerks.PerkInstances.Add(item);
            }
            this.m_skillTimer.set(ConfigSkills.SHARED_DATA[this.m_parameters.SkillType].BossFirstCastDelay);
        }

        public static string GetDescription(string[] parameters)
        {
            Parameters parameters2 = ParseParameters(parameters);
            switch (parameters2.SkillType)
            {
                case SkillType.BossDefender:
                    return _.L(ConfigLoca.BOSS_AI_DEFENDER, null, false);

                case SkillType.BossSplitter:
                    return _.L(ConfigLoca.BOSS_AI_SPLITTER, null, false);

                case SkillType.NONE:
                    return "Error! Invalid caster";
            }
            int count = parameters2.PerkTypes.Count;
            for (int i = 0; i < count; i++)
            {
                ConfigPerks.SharedData data = ConfigPerks.SHARED_DATA[parameters2.PerkTypes[i]];
                if (data.LinkedToSkill != parameters2.SkillType)
                {
                    return "Error! Invalid caster";
                }
            }
            if (count > 0)
            {
                return _.L(ConfigLoca.BOSS_AI_CASTER_WITH_PERK, new <>__AnonType0<string, string>(_.L(ConfigPerks.SHARED_DATA[parameters2.PerkTypes[0]].ShortDescription, null, false), _.L(ConfigSkills.SHARED_DATA[parameters2.SkillType].Name, null, false)), false);
            }
            return _.L(ConfigLoca.BOSS_AI_CASTER, new <>__AnonType1<string>(_.L(ConfigSkills.SHARED_DATA[parameters2.SkillType].Name, null, true)), false);
        }

        private static Parameters ParseParameters(string[] parameters)
        {
            Parameters parameters2 = new Parameters();
            if (parameters != null)
            {
                parameters2.SkillType = ((parameters.Length != 0) && !string.IsNullOrEmpty(parameters[0])) ? ((SkillType) ((int) Enum.Parse(typeof(SkillType), parameters[0]))) : SkillType.NONE;
                parameters2.PerkTypes = new List<PerkType>();
                for (int i = 1; i < parameters.Length; i++)
                {
                    if (!string.IsNullOrEmpty(parameters[i]))
                    {
                        parameters2.PerkTypes.Add((PerkType) ((int) Enum.Parse(typeof(PerkType), parameters[i])));
                    }
                }
            }
            return parameters2;
        }

        public override void preUpdate(float dt)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if ((!base.Character.isExecutingSkill() && (base.Character.TargetCharacter != null)) && activeDungeon.ActiveRoom.characterWithinAttackDistance(base.Character, base.Character.TargetCharacter))
            {
                this.m_skillTimer.tick(dt);
            }
            this.m_targetingBehaviour.preUpdate(dt);
            this.m_attackBehaviour.preUpdate(dt);
        }

        public override void update(float dt)
        {
            this.m_targetingBehaviour.update(dt);
            bool idle = this.m_skillTimer.Idle;
            if (this.m_parameters.SkillType == SkillType.BossSplitter)
            {
                idle = idle && (GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.numberOfBossesAlive() < ConfigSkills.BossSplitter.MaxNumAliveClones);
            }
            if (idle)
            {
                float duration = base.Character.getSkillCooldown(this.m_parameters.SkillType);
                SkillType skillType = this.m_parameters.SkillType;
                if ((skillType == SkillType.Slam) && (this.m_parameters.PerkTypes.Contains(PerkType.SkillUpgradeSlam3) || (this.m_parameters.PerkTypes.Count == 0)))
                {
                    skillType = SkillType.BossSlam;
                }
                GameLogic.Binder.SkillSystem.activateSkill(base.Character, skillType, -1f, null);
                this.m_skillTimer.set(duration);
            }
            else
            {
                this.m_attackBehaviour.update(dt);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Parameters
        {
            public GameLogic.SkillType SkillType;
            public List<PerkType> PerkTypes;
        }
    }
}

