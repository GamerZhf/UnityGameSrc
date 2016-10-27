namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdCleanupRoom : ICommand
    {
        private Room m_room;
        private bool m_seamlessTransition;

        public CmdCleanupRoom(Room room, bool seamlessTransition)
        {
            this.m_room = room;
            this.m_seamlessTransition = seamlessTransition;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator4F iteratorf = new <executeRoutine>c__Iterator4F();
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator4F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdCleanupRoom <>f__this;
            internal ActiveDungeon <ad>__0;
            internal int <i>__1;
            internal int <i>__2;

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
                        this.<ad>__0 = this.<>f__this.m_room.ActiveDungeon;
                        this.<>f__this.m_room.destroyAllProjectiles();
                        this.<>f__this.m_room.destroyAllDungeonBoosts();
                        this.<i>__1 = this.<>f__this.m_room.ActiveAreaEffects.Count - 1;
                        while (this.<i>__1 >= 0)
                        {
                            this.<>f__this.m_room.ActiveAreaEffects[this.<i>__1].destroy();
                            this.<i>__1--;
                        }
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_0106;

                    case 3:
                        goto Label_036F;
                        this.$PC = -1;
                        goto Label_036F;

                    default:
                        goto Label_036F;
                }
                while (Binder.EventBus.ProcessingQueuedEvents)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_0371;
                }
                Binder.DeathSystem.killAllNonPersistentCharacters(true, true);
            Label_0106:
                while (!Binder.DeathSystem.allQueuedCharactersDestroyed())
                {
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0371;
                }
                this.<>f__this.m_room.CompletionTriggered = false;
                this.<>f__this.m_room.MainBossSummoned = false;
                this.<>f__this.m_room.WildBossSummoned = null;
                this.<>f__this.m_room.BossDifficultyDuringSummon = null;
                this.<>f__this.m_room.BossSummonedWith = Room.BossSummonMethod.UNSPECIFIED;
                this.<>f__this.m_room.NumMobsSpawned = 0;
                if (!this.<>f__this.m_seamlessTransition)
                {
                    this.<i>__2 = 0;
                    while (this.<i>__2 < Binder.GameState.PersistentCharacters.Count)
                    {
                        this.<>f__this.m_room.ActiveCharacters.Remove(Binder.GameState.PersistentCharacters[this.<i>__2]);
                        this.<i>__2++;
                    }
                    if (this.<>f__this.m_room.DungeonFog != null)
                    {
                        UnityEngine.Object.Destroy(this.<>f__this.m_room.DungeonFog);
                        this.<>f__this.m_room.DungeonFog = null;
                    }
                    this.<>f__this.m_room.ActiveDungeon = null;
                    this.<>f__this.m_room.ActiveLayoutId = string.Empty;
                    this.<>f__this.m_room.RoomLayout = null;
                    this.<>f__this.m_room.LayoutRoot = null;
                    this.<>f__this.m_room.SpawnpointRoot = null;
                    this.<>f__this.m_room.CharacterSpawnpoints.Clear();
                    this.<>f__this.m_room.IslandSpawnpoints.Clear();
                    this.<>f__this.m_room.DecoSpawnpoints.Clear();
                    this.<>f__this.m_room.DungeonBlocks.Clear();
                    this.<>f__this.m_room.PlayerStartingSpawnpointIndex = 0;
                    this.<>f__this.m_room.WorldGroundPosY = 0f;
                    UnityEngine.Object.Destroy(this.<>f__this.m_room.AstarPath.gameObject);
                    this.<>f__this.m_room.AstarPath = null;
                    if (this.<ad>__0.ActiveRoom == this.<>f__this.m_room)
                    {
                        this.<ad>__0.ActiveRoom = null;
                    }
                }
                this.$current = null;
                this.$PC = 3;
                goto Label_0371;
            Label_036F:
                return false;
            Label_0371:
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
    }
}

