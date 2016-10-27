namespace GameLogic
{
    using App;
    using Pathfinding;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdLoadRoom : ICommand
    {
        private CmdStartGameplay.ProgressCallback m_progressCallback;
        private bool m_seamlessTransition;

        public CmdLoadRoom(bool seamlessTransition, CmdStartGameplay.ProgressCallback progressCallback)
        {
            this.m_seamlessTransition = seamlessTransition;
            this.m_progressCallback = progressCallback;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator5C iteratorc = new <executeRoutine>c__Iterator5C();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        private void progressCallback(float v)
        {
            if (this.m_progressCallback != null)
            {
                this.m_progressCallback(v);
            }
        }

        [DebuggerHidden]
        public IEnumerator refreshDungeonBlocks(List<DungeonBlock> dungeonBlocks, DungeonThemeType themeType, bool isTutorialDungeon)
        {
            <refreshDungeonBlocks>c__Iterator5D iteratord = new <refreshDungeonBlocks>c__Iterator5D();
            iteratord.dungeonBlocks = dungeonBlocks;
            iteratord.themeType = themeType;
            iteratord.isTutorialDungeon = isTutorialDungeon;
            iteratord.<$>dungeonBlocks = dungeonBlocks;
            iteratord.<$>themeType = themeType;
            iteratord.<$>isTutorialDungeon = isTutorialDungeon;
            return iteratord;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator5C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdLoadRoom <>f__this;
            internal ActiveDungeon <ad>__1;
            internal Transform <c>__10;
            internal CharacterInstance <c>__23;
            internal DungeonBlock[] <dungeonBlocks>__18;
            internal string <fogId>__7;
            internal int <i>__13;
            internal int <i>__16;
            internal int <i>__19;
            internal int <i>__22;
            internal int <i>__9;
            internal IEnumerator <ie>__21;
            internal IEnumerator <ie2>__20;
            internal string <layout>__2;
            internal MarkerSpawnPoint <mobSpawnPt>__11;
            internal RoomLayout <rl>__6;
            internal Room <room>__4;
            internal string <sceneName>__3;
            internal MarkerSpawnPointIsland <sp>__14;
            internal MarkerSpawnPointDeco <sp>__17;
            internal GameObject <spawnPointRoot>__5;
            internal Transform <spawnPointRootTm>__8;
            internal MarkerSpawnPointDeco[] <staticDecoSpawnpoints>__15;
            internal MarkerSpawnPointIsland[] <staticIslandSpawnpoints>__12;
            internal ShaderVariantCollection <sv>__24;
            internal Stopwatch <watch>__0;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                Room.Spawnpoint spawnpoint;
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<watch>__0 = DebugUtil.StartStopwatch();
                        this.<>f__this.progressCallback(0f);
                        this.<ad>__1 = GameLogic.Binder.GameState.ActiveDungeon;
                        if (this.<ad>__1.ActiveRoom == null)
                        {
                            break;
                        }
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdCleanupRoom(this.<ad>__1.ActiveRoom, this.<>f__this.m_seamlessTransition), 0f);
                        this.$PC = 1;
                        goto Label_0914;

                    case 1:
                        this.$current = null;
                        this.$PC = 2;
                        goto Label_0914;

                    case 2:
                        break;

                    case 3:
                        goto Label_0113;

                    case 4:
                        this.$current = null;
                        this.$PC = 5;
                        goto Label_0914;

                    case 5:
                        goto Label_01C1;

                    case 6:
                        goto Label_06A7;

                    case 7:
                        this.<>f__this.progressCallback(0.4f);
                        if (this.<>f__this.m_seamlessTransition)
                        {
                            goto Label_0752;
                        }
                        if (ConfigDevice.DeviceQuality() > DeviceQualityType.Low)
                        {
                            GameLogic.Binder.DecoSpawningSystem.loadAllDecos(this.<room>__4);
                            goto Label_0752;
                        }
                        this.<ie>__21 = GameLogic.Binder.DecoSpawningSystem.loadAllDecosAsync(this.<room>__4);
                        goto Label_072D;

                    case 8:
                        goto Label_072D;

                    case 9:
                        this.<>f__this.progressCallback(0.5f);
                        if (this.<>f__this.m_seamlessTransition || (ConfigDevice.DeviceQuality() < DeviceQualityType.Med))
                        {
                            goto Label_07B5;
                        }
                        StaticBatchingUtility.Combine(this.<room>__4.LayoutRoot);
                        this.$current = null;
                        this.$PC = 10;
                        goto Label_0914;

                    case 10:
                        goto Label_07B5;

                    case 11:
                        this.<room>__4.AstarPath.Scan();
                        goto Label_07F9;

                    case 12:
                        this.<>f__this.progressCallback(0.8f);
                        GC.Collect();
                        this.<>f__this.progressCallback(0.9f);
                        goto Label_08D2;

                    case 13:
                        GameLogic.Binder.EventBus.RoomLoaded(this.<room>__4);
                        goto Label_0912;

                    default:
                        goto Label_0912;
                }
                this.<>f__this.progressCallback(0.1f);
                if (!this.<>f__this.m_seamlessTransition)
                {
                    GC.Collect();
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0914;
                }
            Label_0113:
                this.<>f__this.progressCallback(0.2f);
                if (this.<ad>__1.isTutorialDungeon())
                {
                    this.<layout>__2 = LangUtil.GetRandomValueFromList<string>(this.<ad>__1.Dungeon.LayoutPool);
                }
                else
                {
                    this.<layout>__2 = LangUtil.GetRandomValueFromList<string>(ConfigDungeons.DEFAULT_DUNGEON_LAYOUT_POOL);
                }
                char[] separator = new char[] { '_' };
                this.<sceneName>__3 = this.<layout>__2.Split(separator)[0];
                if (!this.<>f__this.m_seamlessTransition)
                {
                    this.$current = Application.LoadLevelAsync(this.<sceneName>__3);
                    this.$PC = 4;
                    goto Label_0914;
                }
            Label_01C1:
                this.<>f__this.progressCallback(0.3f);
                if (this.<ad>__1.ActiveRoom == null)
                {
                    this.<ad>__1.ActiveRoom = new Room();
                }
                this.<room>__4 = this.<ad>__1.ActiveRoom;
                this.<room>__4.ActiveDungeon = this.<ad>__1;
                if (!this.<>f__this.m_seamlessTransition)
                {
                    this.<spawnPointRoot>__5 = GameObject.Find(ConfigGameplay.SPAWNPOINT_ROOT);
                    this.<room>__4.SpawnpointRoot = this.<spawnPointRoot>__5;
                    this.<rl>__6 = this.<spawnPointRoot>__5.GetComponent<RoomLayout>();
                    if (this.<rl>__6 == null)
                    {
                        this.<rl>__6 = this.<spawnPointRoot>__5.AddComponent<RoomLayout>();
                    }
                    this.<room>__4.RoomLayout = this.<rl>__6;
                    this.<room>__4.LayoutRoot = GameObject.Find(ConfigGameplay.LAYOUT_ROOT);
                }
                this.<room>__4.ActiveLayoutId = this.<layout>__2;
                this.<room>__4.RoomLayout.Id = this.<room>__4.ActiveLayoutId;
                this.<room>__4.RoomLayout.load();
                RenderSettings.fog = true;
                RenderSettings.ambientIntensity = 1f;
                this.<fogId>__7 = ConfigDungeons.GetFogIdForTheme(this.<ad>__1.Dungeon.Theme);
                if (!string.IsNullOrEmpty(this.<fogId>__7) && ((this.<room>__4.DungeonFog == null) || !this.<>f__this.m_seamlessTransition))
                {
                    this.<room>__4.DungeonFog = ResourceUtil.Instantiate<GameObject>(this.<fogId>__7);
                }
                this.<room>__4.CharacterSpawnpoints.Clear();
                this.<spawnPointRootTm>__8 = this.<room>__4.SpawnpointRoot.transform;
                this.<i>__9 = 0;
                while (this.<i>__9 < this.<spawnPointRootTm>__8.childCount)
                {
                    this.<c>__10 = this.<spawnPointRootTm>__8.GetChild(this.<i>__9);
                    this.<mobSpawnPt>__11 = this.<c>__10.GetComponent<MarkerSpawnPoint>();
                    if (this.<mobSpawnPt>__11 != null)
                    {
                        spawnpoint = new Room.Spawnpoint();
                        spawnpoint.WorldPt = this.<mobSpawnPt>__11.transform.position;
                        spawnpoint.WorldRot = this.<mobSpawnPt>__11.transform.rotation;
                        this.<room>__4.CharacterSpawnpoints.Add(spawnpoint);
                    }
                    this.<i>__9++;
                }
                this.<room>__4.WorldGroundPosY = this.<room>__4.CharacterSpawnpoints[0].WorldPt.y;
                if (this.<>f__this.m_seamlessTransition)
                {
                    goto Label_08D2;
                }
                this.<room>__4.AstarPath = GameObjectExtensions.FindComponentInChildren<AstarPath>(this.<room>__4.LayoutRoot, true);
                ((GridGraph) this.<room>__4.AstarPath.graphs[0]).collision.heightMask = Layers.AstarLayerMask;
                this.<staticIslandSpawnpoints>__12 = this.<room>__4.LayoutRoot.GetComponentsInChildren<MarkerSpawnPointIsland>();
                this.<room>__4.IslandSpawnpoints.Clear();
                this.<i>__13 = 0;
                while (this.<i>__13 < this.<staticIslandSpawnpoints>__12.Length)
                {
                    this.<sp>__14 = this.<staticIslandSpawnpoints>__12[this.<i>__13];
                    spawnpoint = new Room.Spawnpoint();
                    spawnpoint.WorldPt = this.<sp>__14.transform.position;
                    spawnpoint.WorldRot = this.<sp>__14.transform.rotation;
                    this.<room>__4.IslandSpawnpoints.Add(spawnpoint);
                    this.<i>__13++;
                }
                this.<staticDecoSpawnpoints>__15 = this.<room>__4.LayoutRoot.GetComponentsInChildren<MarkerSpawnPointDeco>();
                this.<room>__4.DecoSpawnpoints.Clear();
                this.<i>__16 = 0;
                while (this.<i>__16 < this.<staticDecoSpawnpoints>__15.Length)
                {
                    this.<sp>__17 = this.<staticDecoSpawnpoints>__15[this.<i>__16];
                    this.<room>__4.DecoSpawnpoints.Add(this.<sp>__17);
                    this.<i>__16++;
                }
                this.<room>__4.calculateIslandToDecoMapping();
                this.<room>__4.DungeonBlocks.Clear();
                this.<dungeonBlocks>__18 = this.<room>__4.LayoutRoot.GetComponentsInChildren<DungeonBlock>();
                this.<i>__19 = 0;
                while (this.<i>__19 < this.<dungeonBlocks>__18.Length)
                {
                    this.<room>__4.DungeonBlocks.Add(this.<dungeonBlocks>__18[this.<i>__19]);
                    this.<i>__19++;
                }
                this.<ie2>__20 = this.<>f__this.refreshDungeonBlocks(this.<room>__4.DungeonBlocks, this.<ad>__1.Dungeon.Theme, this.<ad>__1.isTutorialDungeon());
            Label_06A7:
                while (this.<ie2>__20.MoveNext())
                {
                    this.$current = this.<ie2>__20.Current;
                    this.$PC = 6;
                    goto Label_0914;
                }
                this.$current = null;
                this.$PC = 7;
                goto Label_0914;
            Label_072D:
                while (this.<ie>__21.MoveNext())
                {
                    this.$current = this.<ie>__21.Current;
                    this.$PC = 8;
                    goto Label_0914;
                }
            Label_0752:
                this.$current = null;
                this.$PC = 9;
                goto Label_0914;
            Label_07B5:
                this.<>f__this.progressCallback(0.6f);
                if (!this.<>f__this.m_seamlessTransition)
                {
                    this.$current = null;
                    this.$PC = 11;
                    goto Label_0914;
                }
            Label_07F9:
                this.<>f__this.progressCallback(0.7f);
                this.<i>__22 = 0;
                while (this.<i>__22 < GameLogic.Binder.GameState.PersistentCharacters.Count)
                {
                    this.<c>__23 = GameLogic.Binder.GameState.PersistentCharacters[this.<i>__22];
                    this.<room>__4.ActiveCharacters.Add(this.<c>__23);
                    this.<i>__22++;
                }
                this.<sv>__24 = Resources.Load<ShaderVariantCollection>("CustomShaderVariants");
                if (!this.<sv>__24.isWarmedUp)
                {
                    this.<sv>__24.WarmUp();
                }
                this.$current = null;
                this.$PC = 12;
                goto Label_0914;
            Label_08D2:
                this.<>f__this.progressCallback(1f);
                this.$current = null;
                this.$PC = 13;
                goto Label_0914;
                this.$PC = -1;
            Label_0912:
                return false;
            Label_0914:
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

        [CompilerGenerated]
        private sealed class <refreshDungeonBlocks>c__Iterator5D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal List<DungeonBlock> <$>dungeonBlocks;
            internal bool <$>isTutorialDungeon;
            internal DungeonThemeType <$>themeType;
            internal int <i>__2;
            internal int <instantiationCounter>__1;
            internal int <MAX_BLOCK_INSTANTIATIONS_PER_FRAME>__0;
            internal List<DungeonBlock> dungeonBlocks;
            internal bool isTutorialDungeon;
            internal DungeonThemeType themeType;

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
                        this.<MAX_BLOCK_INSTANTIATIONS_PER_FRAME>__0 = (ConfigDevice.DeviceQuality() > DeviceQualityType.Low) ? 0x7fffffff : 0x4b;
                        this.<instantiationCounter>__1 = 0;
                        this.<i>__2 = 0;
                        goto Label_00B7;

                    case 1:
                        break;

                    default:
                        goto Label_00D9;
                }
            Label_00A9:
                this.<i>__2++;
            Label_00B7:
                if (this.<i>__2 < this.dungeonBlocks.Count)
                {
                    this.dungeonBlocks[this.<i>__2].load(this.themeType, this.isTutorialDungeon);
                    if (this.<instantiationCounter>__1++ >= this.<MAX_BLOCK_INSTANTIATIONS_PER_FRAME>__0)
                    {
                        this.<instantiationCounter>__1 = 0;
                        this.$current = null;
                        this.$PC = 1;
                        return true;
                    }
                    goto Label_00A9;
                }
                goto Label_00D9;
                this.$PC = -1;
            Label_00D9:
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

