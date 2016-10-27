namespace GameLogic
{
    using App;
    using Service;
    using System;
    using UnityEngine;

    public class PassiveTreasureGainSystem : MonoBehaviour, IPassiveTreasureGainSystem
    {
        private long m_lastApplicationPauseTimeStamp;

        protected void Awake()
        {
        }

        private long getElapsedSecondsSince(long timestamp)
        {
            long v = Service.Binder.ServerTime.GameTime - timestamp;
            return MathUtil.Clamp(v, 0L, 0x7fffffffffffffffL);
        }

        protected void OnApplicationFocus(bool focused)
        {
            if (Application.isEditor && ConfigApp.CHEAT_EDITOR_APP_FOCUS_SIMULATES_PAUSING)
            {
                this.OnApplicationPause(!focused);
            }
        }

        protected void OnApplicationPause(bool paused)
        {
            if (App.Binder.AppContext.systemsShouldReactToApplicationPause())
            {
                if (paused)
                {
                    this.m_lastApplicationPauseTimeStamp = Service.Binder.ServerTime.GameTime;
                }
                else if ((((GameLogic.Binder.GameState != null) && (GameLogic.Binder.GameState.Player != null)) && ((this.m_lastApplicationPauseTimeStamp > 0L) && (this.getElapsedSecondsSince(this.m_lastApplicationPauseTimeStamp) > App.Binder.ConfigMeta.PASSIVE_COIN_GAIN_CEREMONY_COOLDOWN_SECONDS))) && GameLogic.Binder.GameState.Player.passiveProgressUnlocked())
                {
                    long numSeconds = MathUtil.Clamp((long) (Service.Binder.ServerTime.GameTime - this.m_lastApplicationPauseTimeStamp), (long) 0L, (long) 0x7fffffffffffffffL);
                    GameLogic.Binder.GameState.Player.addPassiveProgress(numSeconds);
                }
            }
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
        }

        private void onGameStateInitialized()
        {
            Player player = GameLogic.Binder.GameState.Player;
            if (((player.LastSerializationTimestampDuringDeserialization > 0L) && player.passiveProgressUnlocked()) && (this.getElapsedSecondsSince(player.LastSerializationTimestampDuringDeserialization) > App.Binder.ConfigMeta.PASSIVE_COIN_GAIN_CEREMONY_COOLDOWN_SECONDS))
            {
                long numSeconds = MathUtil.Clamp((long) (Service.Binder.ServerTime.GameTime - player.LastSerializationTimestampDuringDeserialization), (long) 0L, (long) 0x7fffffffffffffffL);
                player.addPassiveProgress(numSeconds);
            }
        }

        protected void Update()
        {
            if ((GameLogic.Binder.GameState.ActiveDungeon != null) && (GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom != null))
            {
            }
        }
    }
}

