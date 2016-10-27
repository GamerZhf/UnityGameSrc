namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class DeathSystem : MonoBehaviour, IDeathSystem
    {
        private List<CharacterInstance> m_characterDestructionQueue = new List<CharacterInstance>();
        private Dictionary<CharacterInstance, float> m_characterDestructionTimers = new Dictionary<CharacterInstance, float>();

        public bool allQueuedCharactersDestroyed()
        {
            return (this.m_characterDestructionQueue.Count == 0);
        }

        protected void Awake()
        {
        }

        protected void FixedUpdate()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (((activeDungeon != null) && (activeDungeon.ActiveRoom != null)) && (activeDungeon.CurrentGameplayState == GameplayState.ACTION))
            {
                for (int j = 0; j < activeDungeon.ActiveRoom.ActiveCharacters.Count; j++)
                {
                    CharacterInstance instance = activeDungeon.ActiveRoom.ActiveCharacters[j];
                    instance.AttackCooldownTimer = Mathf.Clamp(instance.AttackCooldownTimer - (Time.deltaTime * Time.timeScale), 0f, float.MaxValue);
                }
            }
            if ((activeDungeon != null) && (activeDungeon.ActiveRoom != null))
            {
                bool isDead = activeDungeon.PrimaryPlayerCharacter.IsDead;
                for (int k = 0; k < activeDungeon.ActiveRoom.ActiveCharacters.Count; k++)
                {
                    CharacterInstance target = activeDungeon.ActiveRoom.ActiveCharacters[k];
                    if (target.IsSupport && (isDead || ((target.FutureTimeOfDeath > 0f) && (Time.fixedTime > target.FutureTimeOfDeath))))
                    {
                        this.killCharacter(target, target, false, false, SkillType.NONE);
                    }
                }
            }
            for (int i = this.m_characterDestructionQueue.Count - 1; i >= 0; i--)
            {
                Dictionary<CharacterInstance, float> dictionary;
                CharacterInstance instance4;
                CharacterInstance c = this.m_characterDestructionQueue[i];
                float num4 = dictionary[instance4];
                (dictionary = this.m_characterDestructionTimers)[instance4 = c] = num4 - (Time.deltaTime * Time.timeScale);
                if (this.m_characterDestructionTimers[c] <= 0f)
                {
                    CmdDestroyCharacter.ExecuteStatic(c);
                    this.m_characterDestructionTimers.Remove(c);
                    this.m_characterDestructionQueue.Remove(c);
                }
            }
        }

        public void killAllNonPersistentCharacters(bool includeSupportCharacters, bool instantDestruction)
        {
            for (int i = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.ActiveCharacters.Count - 1; i >= 0; i--)
            {
                CharacterInstance target = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.ActiveCharacters[i];
                if (!target.IsPersistent && (includeSupportCharacters || !target.IsSupport))
                {
                    this.killCharacter(target, null, false, instantDestruction, SkillType.NONE);
                }
            }
        }

        public void killCharacter(CharacterInstance target, CharacterInstance killer, bool critted, bool instantDestruction, [Optional, DefaultParameterValue(0)] SkillType fromSkill)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (this.m_characterDestructionQueue.Contains(target))
            {
                if (instantDestruction)
                {
                    this.m_characterDestructionTimers[target] = 0f;
                }
            }
            else
            {
                CmdKillCharacter.ExecuteStatic(target, killer, critted, instantDestruction, fromSkill);
                if ((target == activeDungeon.PrimaryPlayerCharacter) && !instantDestruction)
                {
                    for (int i = 0; i < GameLogic.Binder.GameState.PersistentCharacters.Count; i++)
                    {
                        CharacterInstance instance = GameLogic.Binder.GameState.PersistentCharacters[i];
                        if (instance != target)
                        {
                            this.killCharacter(instance, null, false, false, SkillType.NONE);
                        }
                    }
                }
                if (!target.IsPersistent)
                {
                    this.m_characterDestructionQueue.Add(target);
                    if (instantDestruction)
                    {
                        this.m_characterDestructionTimers.Add(target, 0f);
                    }
                    else
                    {
                        float num2 = (ConfigGameplay.DEATH_ENTRY_DURATION + ConfigGameplay.DEATH_REMAIN_DURATION) + UnityEngine.Random.Range((float) 0f, (float) 1f);
                        if (Time.timeScale > 1f)
                        {
                            num2 *= Time.timeScale;
                        }
                        this.m_characterDestructionTimers.Add(target, num2);
                    }
                }
                if ((!target.IsPlayerCharacter && activeDungeon.hasDungeonModifier(DungeonModifierType.MonsterExploding)) && (UnityEngine.Random.Range((float) 0f, (float) 1f) <= ConfigDungeonModifiers.MonsterExploding.ProcChance))
                {
                    ExplosionSkill.ExecuteStatic(null, target.PositionAtTimeOfDeath, null, ConfigDungeonModifiers.MonsterExploding.DamagePct);
                }
            }
        }

        private void onCharacterDealtDamage(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, Vector3 worldPos, double amount, bool critted, bool damageReduced, DamageType damageType, SkillType fromSkill)
        {
            if (targetCharacter.CurrentHp <= 0.0)
            {
                this.killCharacter(targetCharacter, sourceCharacter, critted, false, fromSkill);
            }
        }

        private void onCharacterSkillExecuted(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats)
        {
            if ((skillType == SkillType.Blast) && !character.IsPlayerCharacter)
            {
                this.killCharacter(character, character, false, false, SkillType.NONE);
            }
        }

        protected void OnDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayStateChanged -= new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            eventBus.OnCharacterDealtDamage -= new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            eventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded -= new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            eventBus.OnCharacterSkillExecuted -= new GameLogic.Events.CharacterSkillExecuted(this.onCharacterSkillExecuted);
        }

        protected void OnEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayStateChanged += new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            eventBus.OnCharacterDealtDamage += new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            eventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded += new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            eventBus.OnCharacterSkillExecuted += new GameLogic.Events.CharacterSkillExecuted(this.onCharacterSkillExecuted);
        }

        private void onGameplayEnded(ActiveDungeon activeDungeon)
        {
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
        }

        private void onGameplayStateChanged(GameplayState previousState, GameplayState currentState)
        {
            if (currentState != GameplayState.START_CEREMONY_STEP1)
            {
                if (currentState == GameplayState.BOSS_START)
                {
                    this.killAllNonPersistentCharacters(false, false);
                    for (int i = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.ActiveCharacters.Count - 1; i >= 0; i--)
                    {
                        CharacterInstance target = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.ActiveCharacters[i];
                        if (target.Prefab == CharacterPrefab.KnightClone)
                        {
                            this.killCharacter(target, null, false, false, SkillType.NONE);
                        }
                    }
                }
                else if (currentState == GameplayState.RETIREMENT)
                {
                    this.killAllNonPersistentCharacters(true, false);
                }
            }
        }
    }
}

