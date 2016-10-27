namespace AiView
{
    using App;
    using GameLogic;
    using System;
    using UnityEngine;

    public class CompanionRangedAiBehaviour : AiBehaviour
    {
        private PursuitAiBehaviour m_movementBehaviour;
        private float m_nextPossibleSkillExecutionTime;
        private float m_seekTacticalPositionTime;
        private HeroTargetingAiBehaviour m_targetingAiBehaviour;

        public CompanionRangedAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
            this.m_targetingAiBehaviour = new HeroTargetingAiBehaviour(character);
            this.m_movementBehaviour = new PursuitAiBehaviour(character);
            this.m_seekTacticalPositionTime = Time.fixedTime;
            this.m_nextPossibleSkillExecutionTime = Time.fixedTime;
        }

        public override void cleanup()
        {
        }

        public override void update(float dt)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            CharacterInstance primaryPlayerCharacter = activeDungeon.PrimaryPlayerCharacter;
            if (!base.Character.isExecutingSkill())
            {
                if ((base.Character.ManualTargetPos != Vector3.zero) && (PhysicsUtil.DistBetween(base.Character, base.Character.ManualTargetPos) > 1f))
                {
                    this.m_movementBehaviour.update(dt);
                    this.m_seekTacticalPositionTime = Time.fixedTime + UnityEngine.Random.Range((float) 2f, (float) 2.5f);
                }
                else if ((!primaryPlayerCharacter.IsDead && (primaryPlayerCharacter.CurrentHpNormalized <= 0.5f)) && (Time.fixedTime >= this.m_nextPossibleSkillExecutionTime))
                {
                    if (base.Character.TargetCharacter != primaryPlayerCharacter)
                    {
                        CmdSetCharacterTarget.ExecuteStatic(base.Character, primaryPlayerCharacter, Vector3.zero);
                    }
                    if (base.Character.PhysicsBody.rotatedEnoughForMovement())
                    {
                        GameLogic.Binder.SkillSystem.activateSkill(base.Character, SkillType.Heal, -1f, null);
                        this.m_nextPossibleSkillExecutionTime = Time.fixedTime + ConfigSkills.SHARED_DATA[SkillType.Heal].Cooldown;
                    }
                }
                else if (((primaryPlayerCharacter.TargetCharacter != null) && (PhysicsUtil.DistBetween(primaryPlayerCharacter, primaryPlayerCharacter.TargetCharacter) > ConfigGameplay.PASSIVE_HP_REGEN_PROXIMITY_THRESHOLD)) || (primaryPlayerCharacter.ManualTargetPos != Vector3.zero))
                {
                    if ((base.Character.TargetCharacter != primaryPlayerCharacter.TargetCharacter) || (base.Character.ManualTargetPos != primaryPlayerCharacter.ManualTargetPos))
                    {
                        CmdSetCharacterTarget.ExecuteStatic(base.Character, primaryPlayerCharacter.TargetCharacter, primaryPlayerCharacter.ManualTargetPos);
                    }
                    this.m_movementBehaviour.update(dt);
                    this.m_seekTacticalPositionTime = Time.fixedTime;
                }
                else if ((this.m_seekTacticalPositionTime <= 0f) || (Time.fixedTime < this.m_seekTacticalPositionTime))
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
                        if (((base.Character.TargetCharacter != null) && this.m_movementBehaviour.WithinAttackDistance) && (this.m_movementBehaviour.LineOfSightToTargetCharacter && base.Character.PhysicsBody.rotatedEnoughForMovement()))
                        {
                            Vector3 position = base.Character.TargetCharacter.PhysicsBody.Transform.position;
                            base.Character.AttackRoutine = GameLogic.Binder.CommandProcessor.executeCharacterSpecific(base.Character, new CmdAttackRanged(base.Character, position), 0f);
                        }
                    }
                }
                else
                {
                    CharacterInstance toCharacter = activeDungeon.ActiveRoom.getClosestEnemyCharacter(base.Character, false);
                    if (toCharacter != null)
                    {
                        Vector3 vector;
                        int num2 = 0;
                        do
                        {
                            float num3 = base.Character.AttackRange(true) * 0.75f;
                            Vector3 vector2 = (Vector3) (new Vector3(UnityEngine.Random.insideUnitCircle.x, 0f, UnityEngine.Random.insideUnitCircle.y) * 0.5f);
                            switch (UnityEngine.Random.Range(0, 3))
                            {
                                case 0:
                                    vector = (toCharacter.PhysicsBody.Transform.position - ((Vector3) (toCharacter.PhysicsBody.Transform.right * num3))) + vector2;
                                    break;

                                case 1:
                                    vector = (toCharacter.PhysicsBody.Transform.position + ((Vector3) (toCharacter.PhysicsBody.Transform.right * num3))) + vector2;
                                    break;

                                default:
                                    vector = (toCharacter.PhysicsBody.Transform.position - ((Vector3) (toCharacter.PhysicsBody.Transform.forward * num3))) + vector2;
                                    break;
                            }
                            int? mask = null;
                            vector = activeDungeon.ActiveRoom.calculateNearestEmptySpot(vector, Vector3.zero, 1f, 1f, 6f, mask);
                            num2++;
                        }
                        while ((num2 < 20) && (!activeDungeon.ActiveRoom.lineOfSightCharacterToCharacter(base.Character, toCharacter) || (activeDungeon.ActiveRoom.getEnemyCharactersWithinRadius(vector, base.Character.AttackRange(true) * 0.5f, base.Character).Count > 0)));
                        CmdSetCharacterTarget.ExecuteStatic(base.Character, null, vector);
                    }
                    this.m_seekTacticalPositionTime = 0f;
                }
            }
        }
    }
}

