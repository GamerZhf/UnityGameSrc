namespace GameLogic
{
    using App;
    using System;
    using UnityEngine;

    public class BoostSystem : MonoBehaviour, IBoostSystem
    {
        protected void Awake()
        {
        }

        protected void FixedUpdate()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (((activeDungeon != null) && (activeDungeon.ActiveRoom != null)) && (activeDungeon.CurrentGameplayState == GameplayState.ACTION))
            {
                Player player = GameLogic.Binder.GameState.Player;
                CharacterInstance activeCharacter = player.ActiveCharacter;
                for (int i = 0; i < ConfigBoosts.ALL_BOOSTS.Count; i++)
                {
                    BoostType boost = ConfigBoosts.ALL_BOOSTS[i];
                    if (player.boostEntryExists(boost))
                    {
                        float num2 = player.getRemainingBoostSeconds(boost);
                        if (num2 > 0f)
                        {
                            bool flag = GameLogic.Binder.BuffSystem.hasBuffFromBoost(activeCharacter, boost);
                            if (!activeCharacter.IsDead && !flag)
                            {
                                ConfigBoosts.SharedData data = ConfigBoosts.SHARED_DATA[boost];
                                Buff buff = new Buff();
                                buff.FromBoost = boost;
                                buff.DurationSeconds = data.DurationSeconds;
                                buff.TimeRemaining = num2;
                                buff.HudSprite = data.Sprite.SpriteId;
                                buff.BaseStat1 = data.BaseStat1;
                                buff.BaseStat2 = data.BaseStat2;
                                buff.Modifier = data.Modifier;
                                GameLogic.Binder.BuffSystem.startBuff(activeCharacter, buff);
                            }
                        }
                        else
                        {
                            CmdStopBoost.ExecuteStatic(player, boost);
                        }
                    }
                }
            }
        }

        private void onBoostActivated(Player player, BoostType boostType, string analyticsSourceId)
        {
            CharacterInstance activeCharacter = player.ActiveCharacter;
            if (!activeCharacter.IsDead)
            {
                Buff buff = GameLogic.Binder.BuffSystem.getBuffFromBoost(activeCharacter, boostType);
                if (buff != null)
                {
                    GameLogic.Binder.BuffSystem.endBuff(buff);
                }
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnBoostActivated -= new GameLogic.Events.BoostActivated(this.onBoostActivated);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnBoostActivated += new GameLogic.Events.BoostActivated(this.onBoostActivated);
        }
    }
}

