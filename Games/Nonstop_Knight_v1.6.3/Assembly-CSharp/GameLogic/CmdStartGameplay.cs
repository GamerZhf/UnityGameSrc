namespace GameLogic
{
    using App;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class CmdStartGameplay : ICommand
    {
        private string m_dungeonId;
        private int m_floor;
        private ProgressCallback m_progressCallback;
        private bool m_seamlessTransition;

        public CmdStartGameplay(string dungeonId, int floor, bool seamlessTransition, [Optional, DefaultParameterValue(null)] ProgressCallback progressCallback)
        {
            this.m_dungeonId = dungeonId;
            this.m_floor = floor;
            this.m_seamlessTransition = seamlessTransition;
            this.m_progressCallback = progressCallback;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator6E iteratore = new <executeRoutine>c__Iterator6E();
            iteratore.<>f__this = this;
            return iteratore;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator6E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdStartGameplay <>f__this;
            internal ActiveDungeon <ad>__3;
            internal CharacterInstance <c>__11;
            internal Dungeon <dungeon>__2;
            internal Vector3 <freeWorldPos>__12;
            internal int <i>__10;
            internal ItemInstance <ii>__13;
            internal bool <isFrenzyActive>__8;
            internal DungeonMood <mood>__7;
            internal DungeonMood <overrideMood>__6;
            internal Player <player>__0;
            internal Room.Spawnpoint <primaryHeroSpawnPt>__9;
            internal TournamentInstance <selectedTournament>__4;
            internal TournamentView <tv>__5;
            internal Stopwatch <watch>__1;

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
                        this.<player>__0 = GameLogic.Binder.GameState.Player;
                        this.<watch>__1 = DebugUtil.StartStopwatch();
                        this.<dungeon>__2 = GameLogic.Binder.DungeonResources.getResource(this.<>f__this.m_dungeonId);
                        if (!this.<>f__this.m_seamlessTransition)
                        {
                            this.<ad>__3 = new ActiveDungeon();
                            break;
                        }
                        this.<ad>__3 = GameLogic.Binder.GameState.ActiveDungeon;
                        break;

                    case 1:
                        if (!this.<isFrenzyActive>__8)
                        {
                            if (this.<ad>__3.ActiveRoom.RoomLayout.RandomHeroSpawnpoint)
                            {
                                this.<primaryHeroSpawnPt>__9 = LangUtil.GetRandomValueFromList<Room.Spawnpoint>(this.<ad>__3.ActiveRoom.CharacterSpawnpoints);
                            }
                            else
                            {
                                this.<primaryHeroSpawnPt>__9 = this.<ad>__3.ActiveRoom.CharacterSpawnpoints[0];
                            }
                        }
                        else
                        {
                            this.<primaryHeroSpawnPt>__9 = this.<ad>__3.ActiveRoom.CharacterSpawnpoints[GameLogic.Binder.CharacterSpawningSystem.getPreviousMobSpawnpointIndex()];
                        }
                        this.<ad>__3.ActiveRoom.PlayerStartingSpawnpointIndex = this.<ad>__3.ActiveRoom.CharacterSpawnpoints.IndexOf(this.<primaryHeroSpawnPt>__9);
                        this.<i>__10 = 0;
                        while (this.<i>__10 < GameLogic.Binder.GameState.PersistentCharacters.Count)
                        {
                            this.<c>__11 = GameLogic.Binder.GameState.PersistentCharacters[this.<i>__10];
                            this.<c>__11.resetDynamicRuntimeData();
                            this.<c>__11.CurrentHp = this.<c>__11.MaxLife(true);
                            int? mask = null;
                            this.<freeWorldPos>__12 = this.<ad>__3.ActiveRoom.calculateNearestEmptySpot(this.<primaryHeroSpawnPt>__9.WorldPt, Vector3.zero, 1f, 1f, 6f, mask);
                            this.<c>__11.PhysicsBody.enabled = true;
                            this.<c>__11.PhysicsBody.gameObject.SetActive(true);
                            this.<c>__11.PhysicsBody.Transform.position = this.<freeWorldPos>__12;
                            this.<c>__11.PhysicsBody.Transform.rotation = this.<primaryHeroSpawnPt>__9.WorldRot;
                            this.<c>__11.CurrentHp = this.<c>__11.MaxLife(true);
                            this.<ii>__13 = this.<c>__11.getHighestLevelItemOwned();
                            if (this.<ii>__13 != null)
                            {
                                this.<c>__11.HighestLevelItemOwnedAtFloorStart.ItemType = this.<ii>__13.Item.Type;
                                this.<c>__11.HighestLevelItemOwnedAtFloorStart.Level = this.<ii>__13.Level;
                                this.<c>__11.HighestLevelItemOwnedAtFloorStart.Rank = this.<ii>__13.Rank;
                            }
                            else
                            {
                                CharacterInstance.HighestLevelItemInfo info = new CharacterInstance.HighestLevelItemInfo();
                                info.ItemType = ItemType.Weapon;
                                info.Level = 1;
                                this.<c>__11.HighestLevelItemOwnedAtFloorStart = info;
                            }
                            if (this.<i>__10 < (GameLogic.Binder.GameState.PersistentCharacters.Count - 1))
                            {
                                this.$current = new WaitForFixedUpdate();
                                this.$PC = 2;
                                goto Label_076B;
                            }
                        Label_06E9:
                            this.<i>__10++;
                        }
                        GameLogic.Binder.EventBus.GameplayStarted(this.<ad>__3);
                        if (!this.<>f__this.m_seamlessTransition)
                        {
                            this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.START_CEREMONY_STEP1, 0f), 0f);
                            this.$PC = 3;
                            goto Label_076B;
                        }
                        goto Label_0769;

                    case 2:
                        goto Label_06E9;

                    case 3:
                        goto Label_0769;
                        this.$PC = -1;
                        goto Label_0769;

                    default:
                        goto Label_0769;
                }
                this.<ad>__3.Dungeon = this.<dungeon>__2;
                this.<ad>__3.Floor = this.<>f__this.m_floor;
                this.<ad>__3.SeamlessTransition = this.<>f__this.m_seamlessTransition;
                this.<ad>__3.ActiveDungeonRuleset = null;
                GameLogic.Binder.GameState.ActiveDungeon = this.<ad>__3;
                this.<ad>__3.ActiveTournament = null;
                if (this.<player>__0.Tournaments.hasTournamentSelected())
                {
                    this.<selectedTournament>__4 = this.<player>__0.Tournaments.SelectedTournament;
                    if ((this.<selectedTournament>__4.CurrentState == TournamentInstance.State.PENDING_JOIN_CONFIRMATION) || (this.<selectedTournament>__4.CurrentState == TournamentInstance.State.ACTIVE))
                    {
                        this.<tv>__5 = Service.Binder.TournamentSystem.GetTournamentView(this.<player>__0.Tournaments.SelectedTournamentId);
                        if (this.<tv>__5 != null)
                        {
                            this.<ad>__3.ActiveTournament = this.<selectedTournament>__4;
                            this.<ad>__3.ActiveDungeonRuleset = ConfigDungeonModifiers.GetRulesetForId(this.<tv>__5.TournamentInfo.TournamentRulesetId);
                        }
                        else
                        {
                            UnityEngine.Debug.LogError("Player has tournament selected but no TournamentView cannot be obtained: " + this.<player>__0.Tournaments.SelectedTournamentId);
                        }
                    }
                    if (this.<ad>__3.ActiveTournament == null)
                    {
                        CmdSelectTournament.ExecuteStatic(this.<player>__0, null);
                    }
                }
                if ((this.<ad>__3.ActiveTournament != null) && ConfigTournaments.DUNGEON_MOOD_OVERRIDES.ContainsKey(this.<dungeon>__2.Theme))
                {
                    this.<overrideMood>__6 = ConfigTournaments.DUNGEON_MOOD_OVERRIDES[this.<dungeon>__2.Theme];
                    this.<mood>__7 = new DungeonMood(this.<dungeon>__2.Mood);
                    this.<mood>__7.HeroLightColor = this.<overrideMood>__6.HeroLightColor;
                    this.<mood>__7.HeroLightIntensity = this.<overrideMood>__6.HeroLightIntensity;
                    this.<mood>__7.HeroLightRange = this.<overrideMood>__6.HeroLightRange;
                    this.<mood>__7.FogColor = this.<overrideMood>__6.FogColor;
                    this.<mood>__7.HorizontalFogStartTerm = this.<overrideMood>__6.HorizontalFogStartTerm;
                    this.<mood>__7.HorizontalFogEndTerm = this.<overrideMood>__6.HorizontalFogEndTerm;
                    this.<mood>__7.PropColor = this.<overrideMood>__6.PropColor;
                    this.<mood>__7.Weather = this.<overrideMood>__6.Weather;
                    this.<mood>__7.postDeserializeInitialization();
                    this.<ad>__3.Mood = this.<mood>__7;
                }
                else
                {
                    this.<ad>__3.Mood = this.<dungeon>__2.Mood;
                }
                this.<isFrenzyActive>__8 = GameLogic.Binder.FrenzySystem.isFrenzyActive();
                if (this.<dungeon>__2.hasBoss())
                {
                    this.<ad>__3.BossId = this.<dungeon>__2.getBossId(this.<player>__0, this.<>f__this.m_floor);
                }
                else
                {
                    this.<ad>__3.BossId = string.Empty;
                }
                this.<ad>__3.BossesKilled = 0;
                this.<ad>__3.WildBossMode = false;
                this.<ad>__3.WildBossEscapeTriggered = false;
                this.<ad>__3.VisualizableBossRewards = null;
                this.<ad>__3.clearDungeonModifiers();
                if (this.<ad>__3.ActiveDungeonRuleset != null)
                {
                    this.<ad>__3.enableDungeonModifier(this.<ad>__3.ActiveDungeonRuleset.DungeonModifiers);
                }
                if (this.<ad>__3.DungeonEventType == DungeonEventType.None)
                {
                    this.<ad>__3.DungeonEventType = (ConfigApp.CHEAT_DUNGEON_EVENT_TYPE == DungeonEventType.None) ? App.Binder.ConfigMeta.DUNGEON_EVENT_TYPE : ConfigApp.CHEAT_DUNGEON_EVENT_TYPE;
                }
                GameLogic.Binder.EventBus.GameplayLoadingStarted(this.<ad>__3);
                this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdLoadRoom(this.<>f__this.m_seamlessTransition, this.<>f__this.m_progressCallback), 0f);
                this.$PC = 1;
                goto Label_076B;
            Label_0769:
                return false;
            Label_076B:
                return true;
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

        public delegate void ProgressCallback(float normalizedProgress);
    }
}

