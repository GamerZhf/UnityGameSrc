namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class BlinkSystem : MonoBehaviour, IBlinkSystem
    {
        private Queue<KeyValuePair<CharacterInstance, Vector3>> m_queue = new Queue<KeyValuePair<CharacterInstance, Vector3>>(1);

        protected void Awake()
        {
        }

        public void blinkCharacter(CharacterInstance c, Vector3 targetWorldPt, [Optional, DefaultParameterValue(0f)] float waitBefore)
        {
            this.m_queue.Clear();
            c.BlinkRoutine = Binder.CommandProcessor.executeCharacterSpecific(c, new CmdBlinkCharacter(c, targetWorldPt, waitBefore), 0f);
        }

        public void blinkCharacterQueued(CharacterInstance c, Vector3 targetWorldPt)
        {
            this.m_queue.Enqueue(new KeyValuePair<CharacterInstance, Vector3>(c, targetWorldPt));
        }

        protected void FixedUpdate()
        {
            ActiveDungeon activeDungeon = Binder.GameState.ActiveDungeon;
            if ((activeDungeon != null) && (activeDungeon.CurrentGameplayState == GameplayState.ACTION))
            {
                CharacterInstance activeCharacter = Binder.GameState.Player.ActiveCharacter;
                if (this.m_queue.Count > 0)
                {
                    Vector3 targetWorldPt = this.m_queue.Peek().Value;
                    if (activeCharacter.canBlink(targetWorldPt))
                    {
                        this.m_queue.Dequeue();
                        this.blinkCharacter(activeCharacter, targetWorldPt, 0f);
                    }
                }
            }
        }

        private void onCharacterHordeSpawned(Room.Spawnpoint spawnpoint, bool isBoss)
        {
            ActiveDungeon activeDungeon = Binder.GameState.ActiveDungeon;
            if (activeDungeon != null)
            {
                CharacterInstance primaryPlayerCharacter = activeDungeon.PrimaryPlayerCharacter;
                if ((!Binder.FrenzySystem.isFrenzyActive() && primaryPlayerCharacter.isAtBlinkDistance(spawnpoint.WorldPt)) && ((primaryPlayerCharacter.getPerkInstanceCount(PerkType.BlinkTravel) > 0) && (primaryPlayerCharacter.getGenericModifierForPerkType(PerkType.BlinkTravel) >= UnityEngine.Random.value)))
                {
                    this.blinkCharacterQueued(primaryPlayerCharacter, spawnpoint.WorldPt);
                }
            }
        }

        protected void OnDisable()
        {
            Binder.EventBus.OnCharacterHordeSpawned -= new Events.CharacterHordeSpawned(this.onCharacterHordeSpawned);
            Binder.EventBus.OnGameplayStarted -= new Events.GameplayStarted(this.onGameplayStarted);
        }

        protected void OnEnable()
        {
            Binder.EventBus.OnCharacterHordeSpawned += new Events.CharacterHordeSpawned(this.onCharacterHordeSpawned);
            Binder.EventBus.OnGameplayStarted += new Events.GameplayStarted(this.onGameplayStarted);
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            this.m_queue.Clear();
        }
    }
}

