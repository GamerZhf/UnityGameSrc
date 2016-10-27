namespace AiView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class SupportAiBehaviour : AiBehaviour
    {
        protected PursuitAiBehaviour m_movementBehaviour;
        protected float m_nextPossibleHealSkillExecutionTime;
        protected float m_seekTacticalPositionTime;
        protected HeroTargetingAiBehaviour m_targetingAiBehaviour;

        public SupportAiBehaviour(CharacterInstance character, [Optional, DefaultParameterValue(null)] List<PerkType> perkTypes)
        {
            base.Character = character;
            if (perkTypes != null)
            {
                for (int i = 0; i < perkTypes.Count; i++)
                {
                    PerkType perkType = perkTypes[i];
                    PerkInstance item = new PerkInstance();
                    item.Type = perkType;
                    item.Modifier = ConfigPerks.GetBestModifier(perkType);
                    character.SupportPerks.PerkInstances.Add(item);
                }
            }
            this.attachSupportPerk(PerkType.AllyHeal);
            this.m_targetingAiBehaviour = new HeroTargetingAiBehaviour(character);
            this.m_movementBehaviour = new PursuitAiBehaviour(character);
            this.m_seekTacticalPositionTime = Time.fixedTime;
            this.m_nextPossibleHealSkillExecutionTime = Time.fixedTime;
        }

        public void attachSupportPerk(PerkType perkType)
        {
            PetInstance instance = base.Character.OwningPlayer.Pets.getPetInstance(base.Character.Character);
            if ((instance != null) && instance.Character.FixedPerks.hasUnlockedPerkOfType(instance.Level, perkType))
            {
                PerkInstance item = new PerkInstance();
                item.Type = perkType;
                item.Modifier = ConfigPerks.GetBestModifier(perkType);
                base.Character.SupportPerks.PerkInstances.Add(item);
            }
        }

        public override void cleanup()
        {
        }

        public void seekTacticalPosition()
        {
            this.m_seekTacticalPositionTime = Time.fixedTime;
        }

        public override void update(float dt)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            CharacterInstance primaryPlayerCharacter = activeDungeon.PrimaryPlayerCharacter;
            if (((base.Character.getPerkInstanceCount(PerkType.AllyHeal) > 0) && !primaryPlayerCharacter.IsDead) && ((primaryPlayerCharacter.CurrentHpNormalized <= base.Character.getGenericModifierForPerkType(PerkType.AllyHeal)) && (Time.fixedTime >= this.m_nextPossibleHealSkillExecutionTime)))
            {
                GameLogic.Binder.SkillSystem.activateSkill(base.Character, SkillType.Heal, -1f, primaryPlayerCharacter);
                this.m_nextPossibleHealSkillExecutionTime = Time.fixedTime + ConfigSkills.SHARED_DATA[SkillType.Heal].Cooldown;
            }
            if (((primaryPlayerCharacter.TargetCharacter != null) && (PhysicsUtil.DistBetween(primaryPlayerCharacter, primaryPlayerCharacter.TargetCharacter) > 6f)) || (primaryPlayerCharacter.ManualTargetPos != Vector3.zero))
            {
                if ((base.Character.TargetCharacter != primaryPlayerCharacter.TargetCharacter) || (base.Character.ManualTargetPos != primaryPlayerCharacter.ManualTargetPos))
                {
                    CmdSetCharacterTarget.ExecuteStatic(base.Character, primaryPlayerCharacter.TargetCharacter, primaryPlayerCharacter.ManualTargetPos);
                }
                this.m_movementBehaviour.update(dt);
                this.m_seekTacticalPositionTime = Time.fixedTime;
            }
            else if ((base.Character.ManualTargetPos != Vector3.zero) && (PhysicsUtil.DistBetween(base.Character, base.Character.ManualTargetPos) > 1f))
            {
                this.m_movementBehaviour.update(dt);
                this.m_seekTacticalPositionTime = Time.fixedTime + UnityEngine.Random.Range((float) 2f, (float) 2.5f);
            }
            else
            {
                Vector3 targetWorldPt = primaryPlayerCharacter.PhysicsBody.Transform.position + ((Vector3) (primaryPlayerCharacter.PhysicsBody.Transform.forward * 2f));
                if (base.Character.canBlink(targetWorldPt))
                {
                    GameLogic.Binder.BlinkSystem.blinkCharacter(base.Character, targetWorldPt, 0.6f);
                }
                else if ((this.m_seekTacticalPositionTime > 0f) && (Time.fixedTime >= this.m_seekTacticalPositionTime))
                {
                    CharacterInstance instance2 = activeDungeon.ActiveRoom.getClosestEnemyCharacter(base.Character, false);
                    if (instance2 != null)
                    {
                        bool flag = false;
                        Vector3 zero = Vector3.zero;
                        for (int i = 0; (i < 20) && !flag; i++)
                        {
                            Vector3 vector4 = new Vector3(UnityEngine.Random.insideUnitCircle.x, 0f, UnityEngine.Random.insideUnitCircle.y);
                            Vector3 normalized = vector4.normalized;
                            zero = instance2.PhysicsBody.Transform.position + ((Vector3) (normalized * base.Character.AttackRange(true)));
                            int? mask = null;
                            zero = activeDungeon.ActiveRoom.calculateNearestEmptySpot(zero, normalized, 1f, 0.25f, 2f, mask);
                            CharacterInstance instance3 = activeDungeon.ActiveRoom.getClosestEnemyCharacter(base.Character, true);
                            CharacterInstance instance4 = activeDungeon.ActiveRoom.getClosestEnemyCharacter(base.Character, false);
                            flag = (instance3 != null) && (Vector3.Distance(instance4.PhysicsBody.Transform.position, zero) >= (base.Character.AttackRange(true) * 0.5f));
                        }
                        if (flag)
                        {
                            CmdSetCharacterTarget.ExecuteStatic(base.Character, null, zero);
                        }
                    }
                    this.m_seekTacticalPositionTime = 0f;
                }
                else
                {
                    CharacterInstance b = activeDungeon.ActiveRoom.getClosestEnemyCharacter(base.Character, false);
                    if ((b != null) && (PhysicsUtil.DistBetween(base.Character, b) < (base.Character.AttackRange(true) * 0.5f)))
                    {
                        this.m_seekTacticalPositionTime = Time.fixedTime;
                    }
                    else
                    {
                        this.m_targetingAiBehaviour.update(dt);
                        this.m_movementBehaviour.update(dt);
                    }
                }
            }
        }
    }
}

