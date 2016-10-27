namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class SkillSystem : MonoBehaviour, ISkillSystem
    {
        [CompilerGenerated]
        private static Comparison<SkillType> <>f__am$cache2;
        private OrderedDict<SkillType, ManualTimer> m_skillCooldownTimers = new OrderedDict<SkillType, ManualTimer>();
        private Dictionary<SkillType, int> m_skillTypeChargeCounters = new Dictionary<SkillType, int>(new SkillTypeBoxAvoidanceComparer());

        private void activateCooldownForSkill(SkillType skillType)
        {
            CharacterInstance primaryPlayerCharacter = GameLogic.Binder.GameState.ActiveDungeon.PrimaryPlayerCharacter;
            this.m_skillCooldownTimers[skillType].set(primaryPlayerCharacter.getSkillCooldown(skillType));
        }

        private void activateCooldownForSkillGroup(int skillGroup)
        {
            for (int i = 0; i < this.m_skillCooldownTimers.Keys.Count; i++)
            {
                SkillType skillType = this.m_skillCooldownTimers.keyAt(i);
                if (ConfigSkills.SHARED_DATA[skillType].Group == skillGroup)
                {
                    this.activateCooldownForSkill(skillType);
                }
            }
        }

        public void activateSkill(CharacterInstance c, SkillType skillType, [Optional, DefaultParameterValue(-1f)] float overrideBuildupTime, [Optional, DefaultParameterValue(null)] CharacterInstance overrideTargetCharacter)
        {
            float buildUpTime = !c.IsBoss ? ConfigSkills.SHARED_DATA[skillType].BuildupTime : ConfigSkills.SHARED_DATA[skillType].BossBuildupTime;
            if (overrideBuildupTime > -1f)
            {
                buildUpTime = overrideBuildupTime;
            }
            SkillExecutionStats executionStats = new SkillExecutionStats();
            executionStats.TargetCharacter = (overrideTargetCharacter == null) ? c : overrideTargetCharacter;
            Coroutine coroutine = GameLogic.Binder.CommandProcessor.executeCharacterSpecific(c, new CmdActivateSkill(c, skillType, buildUpTime, executionStats), 0f);
            if (!executionStats.PreExecuteFailed)
            {
                c.SkillRoutines.Add(skillType, coroutine);
            }
        }

        protected void Awake()
        {
            ConfigSkills.ACTIVE_HERO_SKILLS_SORTED_BY_GROUP = new List<SkillType>(ConfigSkills.ACTIVE_HERO_SKILLS);
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = delegate (SkillType s1, SkillType s2) {
                    int group = ConfigSkills.SHARED_DATA[s1].Group;
                    int num2 = ConfigSkills.SHARED_DATA[s2].Group;
                    if (group < num2)
                    {
                        return -1;
                    }
                    if ((group <= num2) && (ConfigSkills.ACTIVE_HERO_SKILLS.IndexOf(s1) < ConfigSkills.ACTIVE_HERO_SKILLS.IndexOf(s2)))
                    {
                        return -1;
                    }
                    return 1;
                };
            }
            ConfigSkills.ACTIVE_HERO_SKILLS_SORTED_BY_GROUP.Sort(<>f__am$cache2);
            for (int i = 0; i < ConfigSkills.ACTIVE_HERO_SKILLS.Count; i++)
            {
                SkillType key = ConfigSkills.ACTIVE_HERO_SKILLS[i];
                ConfigSkills.SharedData data = ConfigSkills.SHARED_DATA[key];
                ManualTimer val = new ManualTimer(data.Cooldown);
                val.end();
                this.m_skillCooldownTimers.add(key, val);
                this.m_skillTypeChargeCounters.Add(key, 0);
            }
        }

        private void endSkillCooldown(SkillType skillType)
        {
            Player player = GameLogic.Binder.GameState.Player;
            this.m_skillCooldownTimers[skillType].end();
            GameLogic.Binder.EventBus.CharacterSkillCooldownEnded(player.ActiveCharacter, skillType);
        }

        public void endSkillCooldownTimers()
        {
            for (int i = 0; i < this.m_skillCooldownTimers.Keys.Count; i++)
            {
                this.endSkillCooldown(this.m_skillCooldownTimers.keyAt(i));
            }
            for (int j = 0; j < ConfigSkills.ACTIVE_HERO_SKILLS.Count; j++)
            {
                this.m_skillTypeChargeCounters[ConfigSkills.ACTIVE_HERO_SKILLS[j]] = 0;
            }
        }

        protected void FixedUpdate()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if ((activeDungeon != null) && (activeDungeon.CurrentGameplayState == GameplayState.ACTION))
            {
                float dt = Time.deltaTime * Time.timeScale;
                float num2 = 0f;
                for (int i = 0; i < activeDungeon.ActiveRoom.ActiveCharacters.Count; i++)
                {
                    CharacterInstance instance = activeDungeon.ActiveRoom.ActiveCharacters[i];
                    if ((!instance.IsDead && instance.IsSupport) && ((instance.Prefab == CharacterPrefab.KnightClone) && (instance.getPerkInstanceCount(PerkType.SkillUpgradeClone4) > 0)))
                    {
                        num2 += instance.getGenericModifierForPerkType(PerkType.SkillUpgradeClone4);
                    }
                }
                dt += dt * num2;
                for (int j = 0; j < this.m_skillCooldownTimers.Keys.Count; j++)
                {
                    if (this.m_skillCooldownTimers.valueAt(j).tick(dt))
                    {
                        this.endSkillCooldown(this.m_skillCooldownTimers.keyAt(j));
                    }
                }
            }
        }

        public int getNumberOfUsedCharges(SkillType skillType)
        {
            return this.m_skillTypeChargeCounters[skillType];
        }

        public float getSkillCooldownNormalizedProgress(SkillType skillType)
        {
            return this.m_skillCooldownTimers[skillType].normalizedProgress();
        }

        public float getSkillCooldownTimeRemaining(SkillType skillType)
        {
            return this.m_skillCooldownTimers[skillType].timeRemaining();
        }

        private void onCharacterDealtDamage(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, Vector3 worldPos, double amount, bool critted, bool damageReduced, DamageType damageType, SkillType fromSkill)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (((targetCharacter.IsPrimaryPlayerCharacter && activeDungeon.WildBossMode) && (!activeDungeon.WildBossEscapeTriggered && (targetCharacter.CurrentHpNormalized <= ConfigTournaments.TOURNAMENT_WILD_BOSS_ESCAPE_HP_HERO_THRESHOLD))) && (activeDungeon.ActiveRoom.getCumulativeNormalizedBossHp() >= ConfigTournaments.TOURNAMENT_WILD_BOSS_ESCAPE_HP_BOSS_THRESHOLD))
            {
                for (int i = 0; i < activeDungeon.ActiveRoom.ActiveCharacters.Count; i++)
                {
                    CharacterInstance c = activeDungeon.ActiveRoom.ActiveCharacters[i];
                    if ((c.IsWildBoss && !c.IsDead) && !c.isExecutingSkill())
                    {
                        GameLogic.Binder.SkillSystem.activateSkill(c, SkillType.Escape, -1f, null);
                        activeDungeon.WildBossEscapeTriggered = true;
                    }
                }
            }
        }

        private void onCharacterSkillActivated(CharacterInstance character, SkillType skillType, float buildupTime, SkillExecutionStats executionStats)
        {
            if (character == GameLogic.Binder.GameState.ActiveDungeon.PrimaryPlayerCharacter)
            {
                Dictionary<SkillType, int> dictionary;
                SkillType type;
                int num2 = dictionary[type];
                (dictionary = this.m_skillTypeChargeCounters)[type = skillType] = num2 + 1;
                if (character.getSkillExtraCharges(skillType) < this.m_skillTypeChargeCounters[skillType])
                {
                    this.activateCooldownForSkillGroup(ConfigSkills.SHARED_DATA[skillType].Group);
                    this.m_skillTypeChargeCounters[skillType] = 0;
                }
            }
        }

        protected void OnDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnPlayerRetired -= new GameLogic.Events.PlayerRetired(this.onPlayerRetired);
            eventBus.OnCharacterSkillActivated -= new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            eventBus.OnMultikillBonusGranted -= new GameLogic.Events.MultikillBonusGranted(this.onMultikillBonusGranted);
            eventBus.OnCharacterDealtDamage -= new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
        }

        protected void OnEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnPlayerRetired += new GameLogic.Events.PlayerRetired(this.onPlayerRetired);
            eventBus.OnCharacterSkillActivated += new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            eventBus.OnMultikillBonusGranted += new GameLogic.Events.MultikillBonusGranted(this.onMultikillBonusGranted);
            eventBus.OnCharacterDealtDamage += new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            this.endSkillCooldownTimers();
        }

        private void onMultikillBonusGranted(Player player, int killCount, double coinAmount)
        {
            if (player.ActiveCharacter.getPerkInstanceCount(PerkType.MultikillCooldownReset) > 0)
            {
                this.endSkillCooldownTimers();
            }
        }

        private void onPlayerRetired(Player player, int retirementFloor)
        {
            this.endSkillCooldownTimers();
        }
    }
}

