namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdCheatLoadDungeon : ICommand
    {
        private string m_overridenDungeonId;

        public CmdCheatLoadDungeon(string overrideDungeonId)
        {
            this.m_overridenDungeonId = overrideDungeonId;
        }

        public CmdCheatLoadDungeon(string[] serialized)
        {
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator4E iteratore = new <executeRoutine>c__Iterator4E();
            iteratore.<>f__this = this;
            return iteratore;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator4E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdCheatLoadDungeon <>f__this;
            internal ActiveDungeon <ad>__0;
            internal IEnumerator <ie>__2;
            internal Player <player>__1;

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
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdChangeGameplayState(GameplayState.END_CEREMONY, 0f), 0f);
                        this.$PC = 1;
                        goto Label_0135;

                    case 1:
                        this.<ie>__2 = TimeUtil.WaitForUnscaledSeconds(ConfigUi.FADE_TO_BLACK_DURATION);
                        break;

                    case 2:
                        break;

                    case 3:
                        this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdStartGameplay(this.<>f__this.m_overridenDungeonId, 1, false, null), 0f);
                        this.$PC = 4;
                        goto Label_0135;

                    default:
                        goto Label_0133;
                }
                if (this.<ie>__2.MoveNext())
                {
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 2;
                }
                else
                {
                    CmdEndBossTrain.ExecuteStatic(this.<player>__1);
                    this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdEndGameplay(this.<ad>__0, false), 0f);
                    this.$PC = 3;
                }
                goto Label_0135;
                this.$PC = -1;
            Label_0133:
                return false;
            Label_0135:
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

