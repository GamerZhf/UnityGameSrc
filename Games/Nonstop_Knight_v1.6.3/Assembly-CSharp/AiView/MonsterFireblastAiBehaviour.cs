namespace AiView
{
    using GameLogic;
    using System;
    using UnityEngine;

    public class MonsterFireblastAiBehaviour : AiBehaviour
    {
        private ManualTimer m_blastTimer = new ManualTimer(MONSTER_FIREBLAST_BUILDUP_DURATION + MONSTER_FIREBLAST_REMAINDER_DURATION);
        private MonsterTargetingAiBehaviour m_targetingBehaviour;
        public static float MONSTER_FIREBLAST_BUILDUP_DURATION = 1f;
        public static float MONSTER_FIREBLAST_REMAINDER_DURATION = 2.5f;

        public MonsterFireblastAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
            this.m_targetingBehaviour = new MonsterTargetingAiBehaviour(character);
            this.m_blastTimer.end();
        }

        public override void update(float dt)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            this.m_blastTimer.tick(dt);
            if (this.m_blastTimer.Idle)
            {
                this.m_targetingBehaviour.update(dt);
                if (base.Character.TargetCharacter == null)
                {
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(base.Character, Vector3.zero, base.Character.Facing);
                }
                else
                {
                    Vector3 vector = Vector3Extensions.ToXzVector3(base.Character.PhysicsBody.Transform.position);
                    Vector3 vector4 = Vector3Extensions.ToXzVector3(base.Character.TargetCharacter.PhysicsBody.Transform.position) - vector;
                    Vector3 normalized = vector4.normalized;
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(base.Character, Vector3.zero, normalized);
                    if (activeDungeon.ActiveRoom.characterWithinAttackDistance(base.Character, base.Character.TargetCharacter) && base.Character.PhysicsBody.rotatedEnoughForMovement())
                    {
                        GameLogic.Binder.SkillSystem.activateSkill(base.Character, SkillType.Blast, MONSTER_FIREBLAST_BUILDUP_DURATION, null);
                        this.m_blastTimer.reset();
                    }
                }
            }
        }
    }
}

