namespace AiView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class AiBehaviourSystem : MonoBehaviour, IAiBehaviourSystem
    {
        private List<AiBehaviour> m_aiBehaviours = new List<AiBehaviour>();
        private Dictionary<CharacterInstance, AiBehaviour> m_characterToAiBehaviourMapping = new Dictionary<CharacterInstance, AiBehaviour>();

        private void addAiBehaviour(CharacterInstance c)
        {
            AiBehaviour behaviour = null;
            switch (c.Character.CoreAiBehaviour)
            {
                case AiBehaviourType.HeroMelee:
                    behaviour = new HeroMeleeAiBehaviour(c);
                    break;

                case AiBehaviourType.HeroRanged:
                    behaviour = new HeroRangedAiBehaviour(c);
                    break;

                case AiBehaviourType.CompanionMelee:
                    behaviour = new CompanionMeleeAiBehaviour(c);
                    break;

                case AiBehaviourType.CompanionRanged:
                    behaviour = new CompanionRangedAiBehaviour(c);
                    break;

                case AiBehaviourType.SupportClone:
                    behaviour = new SupportCloneAiBehaviour(c);
                    break;

                case AiBehaviourType.SupportDecoy:
                    behaviour = new SupportDecoyAiBehaviour(c);
                    break;

                case AiBehaviourType.SupportMeleeAtk:
                    behaviour = new SupportMeleeAttackAiBehaviour(c);
                    break;

                case AiBehaviourType.SupportRangedAtk:
                    behaviour = new SupportRangedAttackAiBehaviour(c);
                    break;

                case AiBehaviourType.SupportDasher:
                    behaviour = new SupportSkillUseAiBehaviour(c, SkillType.Omnislash, false, null);
                    break;

                case AiBehaviourType.SupportWhirler:
                    behaviour = new SupportSkillUseAiBehaviour(c, SkillType.Whirlwind, false, null);
                    break;

                case AiBehaviourType.SupportLeaper:
                    behaviour = new SupportSkillUseAiBehaviour(c, SkillType.Leap, false, null);
                    break;

                case AiBehaviourType.SupportCharger:
                    behaviour = new SupportSkillUseAiBehaviour(c, SkillType.Charge, true, null);
                    break;

                case AiBehaviourType.SupportSplashSlammer:
                {
                    List<PerkType> list2 = new List<PerkType>();
                    list2.Add(PerkType.SkillUpgradeSlam4);
                    List<PerkType> perkTypes = list2;
                    behaviour = new SupportSkillUseAiBehaviour(c, SkillType.Slam, false, perkTypes);
                    break;
                }
            }
            if ((behaviour == null) && (c.Character.CoreAiBehaviour != AiBehaviourType.None))
            {
                if (c.IsPlayerCharacter)
                {
                    Debug.LogWarning("Assigning IdleUntilWithinAggroDistanceAiBeheaviour for player character.");
                }
                behaviour = new IdleUntilWithinAggroDistanceAiBeheaviour(c);
            }
            if (behaviour != null)
            {
                if (c.IsSupport)
                {
                    behaviour.UpdateTimer.set(ConfigGameplay.AI_UPDATE_INTERVAL * 0.25f);
                    behaviour.UpdateTimer.tick(behaviour.UpdateTimer.originalTimeSet() * 0.95f);
                }
                else
                {
                    behaviour.UpdateTimer.tick(UnityEngine.Random.Range((float) (ConfigGameplay.AI_UPDATE_INTERVAL * 0.75f), (float) (ConfigGameplay.AI_UPDATE_INTERVAL * 0.95f)));
                }
                this.m_characterToAiBehaviourMapping.Add(c, behaviour);
                this.m_aiBehaviours.Add(behaviour);
            }
        }

        protected void Awake()
        {
        }

        protected void FixedUpdate()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            for (int i = 0; i < this.m_aiBehaviours.Count; i++)
            {
                AiBehaviour behaviour = this.m_aiBehaviours[i];
                if (behaviour.UpdateTimer.tick(Time.deltaTime * Time.timeScale))
                {
                    CharacterInstance character = behaviour.Character;
                    float dt = behaviour.UpdateTimer.timeElapsed();
                    behaviour.UpdateTimer.reset();
                    behaviour.preUpdate(dt);
                    if (((activeDungeon == null) || (activeDungeon.ActiveRoom == null)) || ((activeDungeon.CurrentGameplayState != GameplayState.ACTION) && (activeDungeon.CurrentGameplayState != GameplayState.START_CEREMONY_STEP1)))
                    {
                        CmdSetCharacterVelocityAndFacing.ExecuteStatic(character, Vector3.zero, character.PhysicsBody.Transform.forward);
                    }
                    else if ((((activeDungeon.CurrentGameplayState != GameplayState.START_CEREMONY_STEP1) && !character.IsDead) && (!character.isAttacking() && (character.AttackCooldownTimer <= 0f))) && (!character.ExternallyControlled && !character.Stunned))
                    {
                        behaviour.update(dt);
                    }
                }
            }
        }

        private void onCharacterMeleeAttackEnded(CharacterInstance c, CharacterInstance targetCharacter, Vector3 worldPt, int killCount)
        {
            c.completeAttackTimer();
        }

        private void onCharacterPreDestroyed(CharacterInstance character)
        {
            if (this.m_characterToAiBehaviourMapping.ContainsKey(character))
            {
                this.removeAiBehaviour(character);
            }
        }

        private void onCharacterSpawned(CharacterInstance character)
        {
            this.addAiBehaviour(character);
            if (character.IsSupport)
            {
                for (int i = 0; i < GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.ActiveCharacters.Count; i++)
                {
                    CharacterInstance instance = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.ActiveCharacters[i];
                    if (!instance.IsPlayerCharacter)
                    {
                        CmdSetCharacterTarget.ExecuteStatic(instance, null, Vector3.zero);
                    }
                }
            }
        }

        protected void OnDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded -= new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            eventBus.OnCharacterSpawned -= new GameLogic.Events.CharacterSpawned(this.onCharacterSpawned);
            eventBus.OnCharacterPreDestroyed -= new GameLogic.Events.CharacterPreDestroyed(this.onCharacterPreDestroyed);
            eventBus.OnCharacterMeleeAttackEnded -= new GameLogic.Events.CharacterMeleeAttackEnded(this.onCharacterMeleeAttackEnded);
        }

        protected void OnEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded += new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            eventBus.OnCharacterSpawned += new GameLogic.Events.CharacterSpawned(this.onCharacterSpawned);
            eventBus.OnCharacterPreDestroyed += new GameLogic.Events.CharacterPreDestroyed(this.onCharacterPreDestroyed);
            eventBus.OnCharacterMeleeAttackEnded += new GameLogic.Events.CharacterMeleeAttackEnded(this.onCharacterMeleeAttackEnded);
        }

        private void onGameplayEnded(ActiveDungeon activeDungeon)
        {
            for (int i = GameLogic.Binder.GameState.PersistentCharacters.Count - 1; i >= 0; i--)
            {
                this.removeAiBehaviour(GameLogic.Binder.GameState.PersistentCharacters[i]);
            }
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            for (int i = 0; i < GameLogic.Binder.GameState.PersistentCharacters.Count; i++)
            {
                CharacterInstance key = GameLogic.Binder.GameState.PersistentCharacters[i];
                if (!this.m_characterToAiBehaviourMapping.ContainsKey(key))
                {
                    this.addAiBehaviour(key);
                }
            }
        }

        private void removeAiBehaviour(CharacterInstance c)
        {
            AiBehaviour item = this.m_characterToAiBehaviourMapping[c];
            item.cleanup();
            this.m_characterToAiBehaviourMapping.Remove(c);
            this.m_aiBehaviours.Remove(item);
        }
    }
}

