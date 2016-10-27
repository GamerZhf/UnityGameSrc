namespace AiView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class IdleUntilWithinAggroDistanceAiBeheaviour : AiBehaviour
    {
        private AiBehaviour m_primaryAi;
        private bool m_provoked;

        public IdleUntilWithinAggroDistanceAiBeheaviour(CharacterInstance character)
        {
            base.Character = character;
            if (character.IsBoss)
            {
                switch (character.Character.BossAiBehaviour)
                {
                    case AiBehaviourType.BossSummoner:
                        this.m_primaryAi = new BossSummonerAiBehaviour(character);
                        break;

                    case AiBehaviourType.BossCaster:
                        this.m_primaryAi = new BossCasterAiBehaviour(character);
                        break;
                }
            }
            if (this.m_primaryAi == null)
            {
                switch (character.Character.CoreAiBehaviour)
                {
                    case AiBehaviourType.MonsterMelee:
                        this.m_primaryAi = new MonsterMeleeAiBehaviour(character);
                        return;

                    case AiBehaviourType.MonsterRanged:
                        this.m_primaryAi = new MonsterRangedAiBehaviour(character);
                        return;

                    case AiBehaviourType.MonsterRangedToMelee:
                        this.m_primaryAi = new MonsterRangedToMeleeAiBehaviour(character);
                        return;

                    case AiBehaviourType.MonsterFireblast:
                        this.m_primaryAi = new MonsterFireblastAiBehaviour(character);
                        return;

                    case AiBehaviourType.MonsterPuffer:
                        this.m_primaryAi = new MonsterPufferAiBehaviour(character);
                        return;
                }
                Debug.LogError("No suitable attack AI behaviour specified for " + character.Name);
            }
        }

        public override void preUpdate(float dt)
        {
            this.m_primaryAi.preUpdate(dt);
        }

        public override void update(float dt)
        {
            if (!this.m_provoked)
            {
                ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                if (activeDungeon.ActiveRoom.getEnemyCharactersWithinRadius(base.Character.PhysicsBody.Transform.position, ConfigGameplay.AGGRO_RANGE_IDLE, base.Character).Count > 0)
                {
                    this.m_provoked = true;
                }
                if (!this.m_provoked)
                {
                    List<CharacterInstance> list = activeDungeon.ActiveRoom.getEnemyCharactersWithinRadius(base.Character.PhysicsBody.Transform.position, ConfigGameplay.AGGRO_RANGE_ALARMED, base.Character);
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].TargetCharacter != null)
                        {
                            this.m_provoked = true;
                            break;
                        }
                    }
                }
            }
            if (this.m_provoked)
            {
                this.m_primaryAi.update(dt);
            }
        }
    }
}

