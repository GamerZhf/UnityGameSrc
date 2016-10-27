namespace Service
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class LeaderboardUpdateService : MonoBehaviour
    {
        private int m_lastHighestFloor;

        private void OnDisable()
        {
            GameLogic.Binder.EventBus.OnPlayerRetired -= new GameLogic.Events.PlayerRetired(this.OnRetired);
            GameLogic.Binder.EventBus.OnRoomCompleted -= new GameLogic.Events.RoomCompleted(this.OnRoomCompleted);
            GameLogic.Binder.EventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.OnGameplayStarted);
            App.Binder.EventBus.OnPlatformFriendsUpdated -= new App.Events.PlatformFriendsUpdated(this.OnFriendsUpdated);
            Service.Binder.EventBus.OnPlayerLoggedIn -= new Service.Events.PlayerLoggedIn(this.OnLoggedIn);
            Service.Binder.EventBus.OnPlayerRegistered -= new Service.Events.PlayerRegistered(this.OnLoggedIn);
        }

        private void OnEnable()
        {
            GameLogic.Binder.EventBus.OnPlayerRetired += new GameLogic.Events.PlayerRetired(this.OnRetired);
            GameLogic.Binder.EventBus.OnRoomCompleted += new GameLogic.Events.RoomCompleted(this.OnRoomCompleted);
            GameLogic.Binder.EventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.OnGameplayStarted);
            App.Binder.EventBus.OnPlatformFriendsUpdated += new App.Events.PlatformFriendsUpdated(this.OnFriendsUpdated);
            Service.Binder.EventBus.OnPlayerLoggedIn += new Service.Events.PlayerLoggedIn(this.OnLoggedIn);
            Service.Binder.EventBus.OnPlayerRegistered += new Service.Events.PlayerRegistered(this.OnLoggedIn);
        }

        private void OnFriendsUpdated(PlatformConnectType connectType)
        {
            if (connectType == PlatformConnectType.Facebook)
            {
                Service.Binder.TaskManager.StartTask(this.UpdateFacebookLeaderboard(), null);
            }
        }

        private void OnGameplayStarted(ActiveDungeon activeDungeon)
        {
            this.UpdateHighestFloorScore();
        }

        private void OnLoggedIn()
        {
            App.Binder.EventBus.LeaderboardLoaded(LeaderboardType.Local);
            Service.Binder.TaskManager.StartTask(Service.Binder.LeaderboardService.LoadLocalLeaderboard(), null);
        }

        private void OnRetired(Player player, int floor)
        {
            this.UpdateHighestFloorScore();
        }

        private void OnRoomCompleted(Room room)
        {
            this.UpdateHighestFloorScore();
        }

        [DebuggerHidden]
        private IEnumerator UpdateFacebookLeaderboard()
        {
            return new <UpdateFacebookLeaderboard>c__Iterator218();
        }

        private void UpdateHighestFloorScore()
        {
            int score = GameLogic.Binder.GameState.Player.getHighestFloorReached();
            if (score > this.m_lastHighestFloor)
            {
                this.m_lastHighestFloor = score;
                App.Binder.EventBus.PlayerScoreUpdated(LeaderboardType.Royal, score);
                App.Binder.EventBus.PlayerScoreUpdated(LeaderboardType.Local, score);
                App.Binder.SocialSystem.ReportLeaderboardScore((long) score);
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateFacebookLeaderboard>c__Iterator218 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Dictionary<string, FbPlatformUser> <friends>__0;
            internal LeaderboardService <lbService>__1;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                    case 1:
                        if (!Service.Binder.PlayerService.IsLoggedIn)
                        {
                            this.$current = null;
                            this.$PC = 1;
                        }
                        else
                        {
                            this.<friends>__0 = Service.Binder.FacebookAdapter.Friends;
                            if (this.<friends>__0.Count <= 0)
                            {
                                break;
                            }
                            this.<lbService>__1 = Service.Binder.LeaderboardService;
                            this.$current = this.<lbService>__1.LoadFriendsLeaderboard(PlatformConnectType.Facebook, new List<string>(this.<friends>__0.Keys));
                            this.$PC = 2;
                        }
                        return true;

                    case 2:
                        Service.Binder.FacebookAdapter.MergeFriendsInLeaderboard(this.<lbService>__1.FriendsLeaderboard);
                        break;

                    default:
                        goto Label_00C2;
                }
                this.$PC = -1;
            Label_00C2:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

