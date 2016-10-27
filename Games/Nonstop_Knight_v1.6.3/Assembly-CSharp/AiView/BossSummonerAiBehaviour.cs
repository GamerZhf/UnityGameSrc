namespace AiView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.InteropServices;

    public class BossSummonerAiBehaviour : AiBehaviour
    {
        private AiBehaviour m_attackBehaviour;
        private Parameters m_parameters;
        private ConfigSkills.BossSummonerSkillSharedData m_skillData;
        private int m_skillExecutionCount;
        private MonsterTargetingAiBehaviour m_targetingBehaviour;
        private ManualTimer m_timer;

        public BossSummonerAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
            this.m_parameters = ParseParameters(character.Character.BossAiParameters);
            this.m_skillData = ConfigSkills.BOSS_SUMMONER_SKILLS[this.m_parameters.SkillType];
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
            if (this.m_skillData.AiTriggerType == TriggerType.TimeInterval)
            {
                this.m_timer = new ManualTimer(this.m_skillData.AiTriggerModifier);
            }
        }

        public static string GetDescription(string[] parameters)
        {
            if (ParseParameters(parameters).SkillType == SkillType.NONE)
            {
                return "Error! Invalid summoner";
            }
            return _.L(ConfigLoca.BOSS_AI_SUMMONER, null, false);
        }

        private static Parameters ParseParameters(string[] parameters)
        {
            Parameters parameters2 = new Parameters();
            if (parameters != null)
            {
                parameters2.SkillType = ((parameters.Length != 0) && !string.IsNullOrEmpty(parameters[0])) ? ((SkillType) ((int) Enum.Parse(typeof(SkillType), parameters[0]))) : SkillType.NONE;
            }
            return parameters2;
        }

        public override void preUpdate(float dt)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (((this.m_skillData.AiTriggerType == TriggerType.TimeInterval) && !base.Character.isExecutingSkill()) && ((base.Character.TargetCharacter != null) && activeDungeon.ActiveRoom.characterWithinAttackDistance(base.Character, base.Character.TargetCharacter)))
            {
                this.m_timer.tick(dt);
            }
            this.m_targetingBehaviour.preUpdate(dt);
            this.m_attackBehaviour.preUpdate(dt);
        }

        public override void update(float dt)
        {
            this.m_targetingBehaviour.update(dt);
            bool idle = false;
            switch (this.m_skillData.AiTriggerType)
            {
                case TriggerType.Health:
                    idle = (this.m_skillExecutionCount == 0) && (base.Character.CurrentHpNormalized <= this.m_skillData.AiTriggerModifier);
                    break;

                case TriggerType.TimeInterval:
                    idle = this.m_timer.Idle;
                    break;
            }
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (idle && (activeDungeon.ActiveRoom.numberOfFriendlyCharactersAlive(base.Character, false) < ConfigSkills.BossSummoner.SummonCountLimit))
            {
                GameLogic.Binder.SkillSystem.activateSkill(base.Character, this.m_parameters.SkillType, -1f, null);
                this.m_skillExecutionCount++;
                if (this.m_skillData.AiTriggerType == TriggerType.TimeInterval)
                {
                    this.m_timer.set(this.m_skillData.AiTriggerModifier);
                }
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
        }

        public enum TriggerType
        {
            None,
            Health,
            TimeInterval
        }
    }
}

