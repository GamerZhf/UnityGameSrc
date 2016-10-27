namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdEndGameplay : ICommand
    {
        private ActiveDungeon m_finishedDungeon;
        private bool m_seamlessTransition;

        public CmdEndGameplay(string[] serialized)
        {
            this.m_finishedDungeon = Binder.GameState.ActiveDungeon;
        }

        public CmdEndGameplay(ActiveDungeon finishedDungeon, bool seamlessTransition)
        {
            this.m_finishedDungeon = finishedDungeon;
            this.m_seamlessTransition = seamlessTransition;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator55 iterator = new <executeRoutine>c__Iterator55();
            iterator.<>f__this = this;
            return iterator;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator55 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdEndGameplay <>f__this;
            internal Room <activeRoom>__3;
            internal CharacterInstance <c>__2;
            internal int <i>__1;
            internal Player <player>__0;

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
                        this.<player>__0 = Binder.GameState.Player;
                        this.<>f__this.m_finishedDungeon.SeamlessTransition = this.<>f__this.m_seamlessTransition;
                        Binder.EventBus.GameplayEndingStarted(this.<>f__this.m_finishedDungeon);
                        this.$current = Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.ENDED, 0f), 0f);
                        this.$PC = 1;
                        goto Label_0218;

                    case 1:
                        if (!this.<>f__this.m_seamlessTransition)
                        {
                            this.<i>__1 = 0;
                            while (this.<i>__1 < Binder.GameState.PersistentCharacters.Count)
                            {
                                this.<c>__2 = Binder.GameState.PersistentCharacters[this.<i>__1];
                                this.<c>__2.resetDynamicRuntimeData();
                                CmdInterruptCharacter.ExecuteStatic(this.<c>__2, true);
                                this.<c>__2.PhysicsBody.Transform.position = Vector3.zero;
                                this.<c>__2.PhysicsBody.Transform.rotation = Quaternion.identity;
                                this.<i>__1++;
                            }
                        }
                        break;

                    case 2:
                        if (this.<>f__this.m_seamlessTransition)
                        {
                            goto Label_01F5;
                        }
                        Binder.GameState.ActiveDungeon = null;
                        this.$current = Resources.UnloadUnusedAssets();
                        this.$PC = 3;
                        goto Label_0218;

                    case 3:
                        goto Label_01F5;

                    default:
                        goto Label_0216;
                }
                if (this.<player>__0.BossTrain.Active && (this.<player>__0.BossTrain.ChargesRemaining <= 0))
                {
                    CmdEndBossTrain.ExecuteStatic(this.<player>__0);
                }
                this.<activeRoom>__3 = this.<>f__this.m_finishedDungeon.ActiveRoom;
                this.$current = Binder.CommandProcessor.execute(new CmdCleanupRoom(this.<activeRoom>__3, this.<>f__this.m_seamlessTransition), 0f);
                this.$PC = 2;
                goto Label_0218;
            Label_01F5:
                Binder.EventBus.GameplayEnded(this.<>f__this.m_finishedDungeon);
                goto Label_0216;
                this.$PC = -1;
            Label_0216:
                return false;
            Label_0218:
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

