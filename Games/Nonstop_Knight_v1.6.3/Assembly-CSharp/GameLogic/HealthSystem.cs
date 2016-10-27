namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class HealthSystem : MonoBehaviour, IHealthSystem
    {
        private List<CharacterInstance> m_characters = new List<CharacterInstance>();
        private Dictionary<CharacterInstance, float> m_hpRegenNextTick = new Dictionary<CharacterInstance, float>();

        private void addHpRegenTimer(CharacterInstance c)
        {
            this.m_characters.Add(c);
            this.m_hpRegenNextTick.Add(c, ConfigGameplay.PASSIVE_HP_REGEN_TICK_INTERVAL);
        }

        protected void Awake()
        {
        }

        protected void FixedUpdate()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (((activeDungeon != null) && (activeDungeon.ActiveRoom != null)) && (!activeDungeon.ActiveRoom.MainBossSummoned && !activeDungeon.WildBossMode))
            {
                for (int i = this.m_characters.Count - 1; i >= 0; i--)
                {
                    CharacterInstance c = this.m_characters[i];
                    if (((this.m_hpRegenNextTick[c] > 0f) && (Time.fixedTime >= this.m_hpRegenNextTick[c])) && (c.CurrentHp < c.MaxLife(true)))
                    {
                        CmdGainHp.ExecuteStatic(c, c.MaxLife(true) * ConfigGameplay.PASSIVE_HP_REGEN_PCT_PER_TICK, true);
                        this.m_hpRegenNextTick[c] = Time.fixedTime + ConfigGameplay.PASSIVE_HP_REGEN_TICK_INTERVAL;
                    }
                }
            }
        }

        private void onCharacterDealtDamage(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, Vector3 worldPos, double amount, bool critted, bool damageReduced, DamageType damageType, SkillType fromSkill)
        {
            if (targetCharacter == GameLogic.Binder.GameState.ActiveDungeon.PrimaryPlayerCharacter)
            {
                this.m_hpRegenNextTick[targetCharacter] = 0f;
            }
        }

        private void onCharacterTargetUpdated(CharacterInstance character, CharacterInstance oldTarget)
        {
            if (((character == GameLogic.Binder.GameState.ActiveDungeon.PrimaryPlayerCharacter) && (character.TargetCharacter != null)) && ((character.CurrentHp < character.MaxLife(true)) && (PhysicsUtil.DistBetween(character, character.TargetCharacter) > ConfigGameplay.PASSIVE_HP_REGEN_PROXIMITY_THRESHOLD)))
            {
                this.m_hpRegenNextTick[character] = Time.fixedTime + ConfigGameplay.PASSIVE_HP_REGEN_TICK_INTERVAL;
            }
        }

        protected void OnDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnCharacterDealtDamage -= new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            eventBus.OnCharacterTargetUpdated -= new GameLogic.Events.CharacterTargetUpdated(this.onCharacterTargetUpdated);
            eventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded -= new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
        }

        protected void OnEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnCharacterDealtDamage += new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            eventBus.OnCharacterTargetUpdated += new GameLogic.Events.CharacterTargetUpdated(this.onCharacterTargetUpdated);
            eventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded += new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
        }

        private void onGameplayEnded(ActiveDungeon activeDungeon)
        {
            this.removeHpRegenTimer(activeDungeon.PrimaryPlayerCharacter);
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            this.addHpRegenTimer(activeDungeon.PrimaryPlayerCharacter);
        }

        private void removeHpRegenTimer(CharacterInstance c)
        {
            this.m_hpRegenNextTick.Remove(c);
            this.m_characters.Remove(c);
        }
    }
}

