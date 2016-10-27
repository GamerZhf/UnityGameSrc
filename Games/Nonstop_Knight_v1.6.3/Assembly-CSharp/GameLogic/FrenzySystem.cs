namespace GameLogic
{
    using App;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class FrenzySystem : MonoBehaviour, IFrenzySystem
    {
        private List<Buff> m_activeFrenzyBuffs = new List<Buff>();
        private bool m_frenzyActive;
        private ManualTimer m_frenzyTimer = new ManualTimer();

        public void activateFrenzy()
        {
            if (!this.isFrenzyActive())
            {
                this.m_frenzyActive = true;
                Player player = GameLogic.Binder.GameState.Player;
                CharacterInstance activeCharacter = player.ActiveCharacter;
                this.m_frenzyTimer.set(this.getDuration(activeCharacter));
                Buff buff2 = new Buff();
                buff2.BaseStat1 = BaseStatProperty.AttacksPerSecond;
                buff2.Modifier = App.Binder.ConfigMeta.FRENZY_HERO_BUFF_ATK_SPEED_MODIFIER;
                buff2.DurationSeconds = 1f;
                Buff item = buff2;
                this.m_activeFrenzyBuffs.Add(item);
                GameLogic.Binder.BuffSystem.startBuff(activeCharacter, item);
                buff2 = new Buff();
                buff2.BaseStat1 = BaseStatProperty.DamagePerHit;
                buff2.Modifier = App.Binder.ConfigMeta.FRENZY_HERO_BUFF_DPH_MODIFIER;
                buff2.DurationSeconds = 1f;
                item = buff2;
                this.m_activeFrenzyBuffs.Add(item);
                GameLogic.Binder.BuffSystem.startBuff(activeCharacter, item);
                buff2 = new Buff();
                buff2.BaseStat1 = BaseStatProperty.Life;
                buff2.Modifier = App.Binder.ConfigMeta.FRENZY_HERO_BUFF_LIFE_MODIFIER;
                buff2.DurationSeconds = 1f;
                item = buff2;
                this.m_activeFrenzyBuffs.Add(item);
                GameLogic.Binder.BuffSystem.startBuff(activeCharacter, item);
                buff2 = new Buff();
                buff2.BaseStat1 = BaseStatProperty.SkillDamage;
                buff2.Modifier = App.Binder.ConfigMeta.FRENZY_HERO_BUFF_SKILLDMG_MODIFIER;
                buff2.DurationSeconds = 1f;
                item = buff2;
                this.m_activeFrenzyBuffs.Add(item);
                GameLogic.Binder.BuffSystem.startBuff(activeCharacter, item);
                PetInstance instance2 = player.Pets.getSelectedPetInstance();
                if ((instance2 != null) && (instance2.SpawnedCharacterInstance != null))
                {
                    buff2 = new Buff();
                    buff2.BaseStat1 = BaseStatProperty.AttacksPerSecond;
                    buff2.Modifier = App.Binder.ConfigMeta.FRENZY_HERO_BUFF_ATK_SPEED_MODIFIER;
                    buff2.DurationSeconds = 1f;
                    item = buff2;
                    this.m_activeFrenzyBuffs.Add(item);
                    GameLogic.Binder.BuffSystem.startBuff(instance2.SpawnedCharacterInstance, item);
                }
                activeCharacter.HeroStats.FrenzyActivations++;
                GameLogic.Binder.EventBus.FrenzyActivated();
            }
        }

        protected void Awake()
        {
        }

        public void deactivateFrenzy()
        {
            if (this.isFrenzyActive())
            {
                this.m_frenzyActive = false;
                this.m_frenzyTimer.end();
                for (int i = 0; i < this.m_activeFrenzyBuffs.Count; i++)
                {
                    GameLogic.Binder.BuffSystem.endBuff(this.m_activeFrenzyBuffs[i]);
                }
                this.m_activeFrenzyBuffs.Clear();
                GameLogic.Binder.EventBus.FrenzyDeactivated();
            }
        }

        protected void FixedUpdate()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if ((activeDungeon != null) && (activeDungeon.CurrentGameplayState == GameplayState.ACTION))
            {
                CharacterInstance primaryPlayerCharacter = GameLogic.Binder.GameState.ActiveDungeon.PrimaryPlayerCharacter;
                if (this.isFrenzyActive())
                {
                    for (int i = 0; i < this.m_activeFrenzyBuffs.Count; i++)
                    {
                        GameLogic.Binder.BuffSystem.startOrRefreshBuff(this.m_activeFrenzyBuffs[i], 1f);
                    }
                    if ((primaryPlayerCharacter.TargetCharacter != null) && primaryPlayerCharacter.canBlink(primaryPlayerCharacter.TargetCharacter.PhysicsBody.Transform.position))
                    {
                        GameLogic.Binder.BlinkSystem.blinkCharacter(primaryPlayerCharacter, primaryPlayerCharacter.TargetCharacter.PhysicsBody.Transform.position, 0f);
                    }
                    if (this.m_frenzyTimer.tick(Time.deltaTime * Time.timeScale))
                    {
                        this.deactivateFrenzy();
                    }
                }
            }
        }

        public float getDuration(ICharacterStatModifier target)
        {
            return CharacterStatModifierUtil.ApplyFrenzyDurationBonuses(target, App.Binder.ConfigMeta.FRENZY_TIMER_MAX_SECONDS);
        }

        public float getDurationBonusPerKill()
        {
            return App.Binder.ConfigMeta.FRENZY_TIMER_ADD_SECONDS_PER_MINION_KILL;
        }

        public float getDurationBonusPerMultikill()
        {
            return App.Binder.ConfigMeta.FRENZY_TIMER_ADD_SECONDS_PER_MULTIKILL;
        }

        public float getNormalizedFrenzyGauge()
        {
            return (1f - this.m_frenzyTimer.normalizedProgress());
        }

        public bool isFrenzyActive()
        {
            return this.m_frenzyActive;
        }

        private void onCharacterKilled(CharacterInstance character, CharacterInstance killer, bool critted, SkillType fromSkill)
        {
            for (int i = this.m_activeFrenzyBuffs.Count - 1; i >= 0; i--)
            {
                if (this.m_activeFrenzyBuffs[i].Character == character)
                {
                    this.m_activeFrenzyBuffs.RemoveAt(i);
                }
            }
            if (character.IsPrimaryPlayerCharacter)
            {
                this.deactivateFrenzy();
            }
        }

        private void onCharacterMeleeAttackEnded(CharacterInstance c, CharacterInstance targetCharacter, Vector3 contactWorldPt, int killCount)
        {
            if (((targetCharacter != null) && !targetCharacter.IsPlayerCharacter) && ((this.isFrenzyActive() && targetCharacter.IsDead) && !targetCharacter.IsBoss))
            {
                this.m_frenzyTimer.addAndClamp(this.getDurationBonusPerKill());
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnCharacterMeleeAttackEnded -= new GameLogic.Events.CharacterMeleeAttackEnded(this.onCharacterMeleeAttackEnded);
            GameLogic.Binder.EventBus.OnCharacterKilled -= new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            GameLogic.Binder.EventBus.OnMultikillBonusGranted -= new GameLogic.Events.MultikillBonusGranted(this.onMultikillBonusGranted);
            GameLogic.Binder.EventBus.OnPlayerRetired -= new GameLogic.Events.PlayerRetired(this.onPlayerRetired);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnCharacterMeleeAttackEnded += new GameLogic.Events.CharacterMeleeAttackEnded(this.onCharacterMeleeAttackEnded);
            GameLogic.Binder.EventBus.OnCharacterKilled += new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            GameLogic.Binder.EventBus.OnMultikillBonusGranted += new GameLogic.Events.MultikillBonusGranted(this.onMultikillBonusGranted);
            GameLogic.Binder.EventBus.OnPlayerRetired += new GameLogic.Events.PlayerRetired(this.onPlayerRetired);
        }

        private void onMultikillBonusGranted(Player player, int killCount, double coinAmount)
        {
            if (this.isFrenzyActive())
            {
                this.m_frenzyTimer.addAndClamp(this.getDurationBonusPerMultikill());
            }
        }

        private void onPlayerRetired(Player player, int retirementFloor)
        {
            this.deactivateFrenzy();
        }
    }
}

