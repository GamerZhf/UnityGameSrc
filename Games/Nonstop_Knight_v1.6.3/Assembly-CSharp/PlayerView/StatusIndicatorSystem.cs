namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class StatusIndicatorSystem : MonoBehaviour
    {
        private List<StatusIndicator> m_statusIndicators = new List<StatusIndicator>();
        private Dictionary<SkillType, Material> sm_sharedSkillMaterials;

        protected void Awake()
        {
            Dictionary<SkillType, Material> dictionary = new Dictionary<SkillType, Material>();
            dictionary.Add(SkillType.Shield, Resources.Load<Material>("Materials/skill_shield_indicator"));
            dictionary.Add(SkillType.Midas, Resources.Load<Material>("Materials/skill_midas_indicator"));
            this.sm_sharedSkillMaterials = dictionary;
        }

        private void hideStatusIndicator(CharacterInstance c, SkillType skillType)
        {
            for (int i = this.m_statusIndicators.Count - 1; i >= 0; i--)
            {
                StatusIndicator indicator = this.m_statusIndicators[i];
                if ((indicator.FollowCharacter == c) && (indicator.SkillType == skillType))
                {
                    PlayerView.Binder.StatusIndicatorPool.returnObject(indicator);
                    this.m_statusIndicators.Remove(indicator);
                }
            }
        }

        private void hideStatusIndicators(CharacterInstance c)
        {
            for (int i = this.m_statusIndicators.Count - 1; i >= 0; i--)
            {
                StatusIndicator indicator = this.m_statusIndicators[i];
                if (indicator.FollowCharacter == c)
                {
                    PlayerView.Binder.StatusIndicatorPool.returnObject(indicator);
                    this.m_statusIndicators.Remove(indicator);
                }
            }
        }

        private void onCharacterKilled(CharacterInstance character, CharacterInstance killer, bool critted, SkillType fromSkill)
        {
            this.hideStatusIndicators(character);
        }

        private void onCharacterSkillActivated(CharacterInstance c, SkillType skillType, float buildupTime, SkillExecutionStats executionStats)
        {
            if ((skillType != SkillType.Shield) && (skillType == SkillType.Midas))
            {
            }
        }

        private void onCharacterSkillExecuted(CharacterInstance c, SkillType skillType, SkillExecutionStats executionStats)
        {
            this.hideStatusIndicator(c, skillType);
        }

        protected void OnDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayEnded -= new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            eventBus.OnCharacterKilled -= new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            eventBus.OnCharacterSkillActivated -= new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            eventBus.OnCharacterSkillExecuted -= new GameLogic.Events.CharacterSkillExecuted(this.onCharacterSkillExecuted);
        }

        protected void OnEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayEnded += new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            eventBus.OnCharacterKilled += new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            eventBus.OnCharacterSkillActivated += new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            eventBus.OnCharacterSkillExecuted += new GameLogic.Events.CharacterSkillExecuted(this.onCharacterSkillExecuted);
        }

        private void onGameplayEnded(ActiveDungeon activeDungeon)
        {
            for (int i = 0; i < GameLogic.Binder.GameState.PersistentCharacters.Count; i++)
            {
                this.hideStatusIndicators(GameLogic.Binder.GameState.PersistentCharacters[i]);
            }
        }

        private void showStatusIndicator(CharacterInstance followCharacter, SkillType skillType)
        {
            StatusIndicator item = PlayerView.Binder.StatusIndicatorPool.getObject();
            item.transform.SetParent(base.transform, false);
            item.gameObject.SetActive(true);
            item.initialize(followCharacter, skillType, this.sm_sharedSkillMaterials[skillType]);
            this.m_statusIndicators.Add(item);
        }
    }
}

